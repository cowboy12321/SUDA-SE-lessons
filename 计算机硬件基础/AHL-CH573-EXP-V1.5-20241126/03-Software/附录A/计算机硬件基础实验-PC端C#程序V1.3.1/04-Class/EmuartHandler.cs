using cn.edu.suda.sumcu.iot;
using Newtonsoft.Json;
using ScottPlot.Colormaps;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Documents;

namespace GEC_LAB._04_Class
{
    internal class EmuartHandler
    {

        #region 1.自定义结构
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct ShakeData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] uecomType;                            //通信模组类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] mcuType;                              //MCU类型
            public uint uStartAds;                              //User程序起始地址
            public uint uCodeSize;                              //User程序总代码大小
            public uint replaceNum;                             //替换更新最大字节
            public uint reserveNum;                             //保留更新最大字节（不等于0意味着有User程序）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] BIOSVersion;                          //BIOS版本
            public uint RAMStart;                               //RAM起始地址
            public uint RAMLength;                              //RAM大小
            public uint FlashStart;                             //Flash起始地址
            public uint FlashLength;                            //Flash大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] coreType;                             //内核类型
            public uint mcuSectSize;                            //扇区大小
            public uint RTOSSize;                               //操作系统空间大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] RTType;                               //操作系统版本
            public uint RAMUserStart;                           //RAM空间User的起始地址
            public uint SystemClock;                            //主频大小
        };

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct ShakeData_Tiny
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public byte[] uecomType;  //通信模组类型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] mcuType;                              //MCU类型
            public uint uStartAds;     //User程序起始地址
            public uint uCodeSize;     //User程序总代码大小
            public uint replaceNum;    //替换更新最大字节
            public uint reserveNum;    //保留更新最大字节（不等于0意味着有User程序）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] BIOSVersion;                          //BIOS版本
        };
        #endregion

        #region 2.全局变量
        private ShakeData shakeData;
        private ShakeData_Tiny shakeData_Tiny;
        private EMUART DebugEmuart = new EMUART();      //串口通信类对象
        private USEREMUART UserEmuart = new USEREMUART();
        private string uecomType = "", mcuType = "", mcuVersion = "";
        private bool closing = false;      //标识是否正在关闭串口
        private bool listening = false;    //标识是否执行完invoke相关操作
        private string LOG = "EmuartHandler";
        public bool UserState => (UserEmuart == null || UserEmuart._Uartport == null) ? false : UserEmuart._Uartport.IsOpen;
        public bool DebugState => (DebugEmuart == null || DebugEmuart._Uartport == null) ? false : DebugEmuart._Uartport.IsOpen;
        public string DebugInfo = "";
        public int UserVersion = -1;
        public int LittleUserVersion = -1;
        public int McuId = -1;
        public delegate void Receive(string data);
        public delegate void ByteReceive(byte[] data);
        public Receive? DebugRawDataReceiveEvent;
        public Receive? UserRawDataReceiveEvent;
        public ByteReceive? UserByteReceiveEvent;
        #endregion

        #region 3.单例与初始化
        // 定义一个静态变量来保存类的实例
        public static EmuartHandler Instance=new EmuartHandler();

        // 定义私有构造函数，使外界不能创建该类实例
        private EmuartHandler()
        {
            Init();
        }

        public bool Init() //!! 可能会重复初始化
        {
            DebugEmuart = new EMUART();
            UserEmuart = new USEREMUART();
            UserVersion = -1;
            LittleUserVersion = -1;
            return true;
        }
        public static EmuartHandler GetInstance()
        {
            return Instance;
        }
        #endregion

        #region 连接
        public delegate void MSG(string s);
        public void linkDebugDevice(MSG msg)
        {
            #region 2.1 变量声明
            int ret;            //返回值
            string com = "";    //串口信息
            byte[]? recv = null;//保存串口接收信息
            byte[] shake = { 10, (byte)'s', (byte)'h', (byte)'a', (byte)'k', (byte)'e' }; //与终端握手帧数据
            #endregion

            #region 2.2 清除一些可能余留信息
            DebugInfo = "";
            msg("连接中：正在连接GEC...");     //底部提示
            #endregion

            #region 2.3 重新遍历串口，寻找终端
            if (DebugEmuart._Uartport != null)
            {
                DebugEmuart._Uartport.Close();
                DebugEmuart._Uartport.Dispose();
                Thread.Sleep(10);
            }
            DebugEmuart.RawDataReceivedEvent -= new EMUART.RawDataReceived(DebugRawDataRecv);
            msg("连接中：正在连接GEC...正在查找匹配设备...");     //底部提示
            ret = DebugEmuart.findDevice(out com, Gobals.BIOSBaudRate);  //寻找emuart设备
            msg("连接中：正在连接GEC...尝试与匹配设备建立通信...");    //底部提示
            DebugEmuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DebugRawDataRecv);
            #endregion

            #region 2.4根据寻找串口的返回值确定执行下列分支
            if (ret == 1)
            {
                msg("当前不存在可用串口");       //右上角显示区
                closing = true;     //正在关闭串口
                while (listening)
                {
                }
                if (DebugEmuart._Uartport != null)
                {
                    DebugEmuart._Uartport.Close();
                }
                closing = false;    //关闭完成
                return;
            }
            else if (ret == 2)
            {
                try
                {
                    DebugEmuart.terminate(115200);
                }
                catch (Exception) { }

                msg("已连接串口" + com + "但未找到设备");       //右上角显示区
                closing = true;     //正在关闭串口
                while (listening) ;
                DebugEmuart?._Uartport?.Close();
                DebugEmuart?._Uartport?.Dispose();
                Thread.Sleep(10);
                closing = false;    //关闭完成
                return;
            }

            //【4.3】如果找到串口与UE,向设备发送握手帧
            msg("连接中：正在连接GEC...向该设备发送握手信息，准备接收设备返回消息...");     //底部提示
            DebugEmuart.bufferClear();   //清除接收数组缓冲区
            DebugEmuart.send(shake, out recv, 200, 3); //获得设备的信息在recv中
            msg("连接中：正在连接GEC...正在处理设备返回信息...");     //底部提示
            if (recv == null)
            {
                try
                {
                    DebugEmuart.terminate(115200); //发送数据给终端设备，让终端设备清空其数据缓冲区
                }
                catch (Exception) { }
                msg("找到GEC在" + com + "但握手失败，请再次单击[重新连接]按钮");  //右上角显示
                closing = true;     //正在关闭串口
                while (listening) ;
                if (DebugEmuart._Uartport != null)
                {
                    try
                    {
                        DebugEmuart._Uartport.Close();
                        DebugEmuart._Uartport.Dispose();
                        Thread.Sleep(10);
                    }
                    catch (Exception) { }
                }
                closing = false;    //关闭完成
                return;
            }


            //【4.4】发送握手帧后，若收到设备返回数据，处理之
#pragma warning disable 8605
            if (recv.Length == Marshal.SizeOf(typeof(ShakeData)))
            {
                msg("成功连接到GEC设备");
                try
                {
                    shakeData = (ShakeData)CommonUtils.bytesToStruct(recv, typeof(ShakeData));
                    //获取握手帧数据
                    uecomType = Encoding.Default.GetString(shakeData.uecomType).Replace("\0", "");
                    mcuType = Encoding.Default.GetString(shakeData.mcuType).Replace("\0", "");
                    mcuVersion = Encoding.Default.GetString(shakeData.BIOSVersion).Replace("\0", "");
                }
                catch (Exception)
                {
                    throw;
                }
                DebugInfo = com + "：" + uecomType + " " + mcuType + " " + mcuVersion;      //右上角显示区

                if (DebugEmuart.haveUE)
                {
                    DebugEmuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DebugRawDataRecv);
                }
            }
            else if (recv.Length == Marshal.SizeOf(typeof(ShakeData_Tiny)))
            {
                msg("成功连接到GEC设备");
                try
                {
                    shakeData_Tiny = (ShakeData_Tiny)CommonUtils.bytesToStruct(recv, typeof(ShakeData_Tiny));
                    //获取握手帧数据
                    uecomType = Encoding.Default.GetString(shakeData_Tiny.uecomType).Replace("\0", "");
                    mcuType = Encoding.Default.GetString(shakeData_Tiny.mcuType).Replace("\0", "");
                    mcuVersion = Encoding.Default.GetString(shakeData_Tiny.BIOSVersion).Replace("\0", "");
                }
                catch (Exception)
                {
                    throw;
                }
                DebugInfo = com + "：" + uecomType + " " + mcuType + " " + mcuVersion;      //右上角显示区

                if (DebugEmuart.haveUE)
                {
                    DebugEmuart.RawDataReceivedEvent += new EMUART.RawDataReceived(DebugRawDataRecv);
                }

            }
            else
            {
                try
                {
                    DebugEmuart.terminate(115200); //发送数据给终端设备，让终端设备清空其数据缓冲区
                }
                catch (Exception) { }
                msg("找到GEC在" + com + "但握手失败，请再次单击[重新连接]按钮");  //右上角显示
                closing = true;     //正在关闭串口
                while (listening) ;
                if (DebugEmuart._Uartport != null)
                {
                    try
                    {
                        DebugEmuart._Uartport.Close();
                        DebugEmuart._Uartport.Dispose();
                        Thread.Sleep(10);
                    }
                    catch (Exception) { }
                }
                closing = false;    //关闭完成
                return;
            }

#pragma warning restore 8605
            #endregion

        }

        public void linkUserDevice(MSG msg)
        {

            msg("连接中：连接用户设备中...");
            UserEmuart.RawDataReceivedEvent -= new USEREMUART.RawDataReceived(UserRawDataRecv);
            UserEmuart.DataReceivedEvent -= new USEREMUART.DataReceived(UserDataRecv);
            int ret = UserEmuart.findDeviceByChecker(() => {
                int cnt = 4;
                while ((cnt--) != 0)
                {
                    if (check()) return true;
                }
                return false;
                
            }, out string com, Gobals.UserBaudRate);

            switch (ret)
            {
                case 0:
                    UserEmuart.DataReceivedEvent += new USEREMUART.DataReceived(UserDataRecv);
                    UserEmuart.RawDataReceivedEvent += new USEREMUART.RawDataReceived(UserRawDataRecv);
                    Gobals.logger?.info(LOG,"user link success");
                    return;
                case 1:
                    msg("user连接失败：没有用户串口！");
                    Gobals.logger?.info(LOG, "user连接失败：没有用户串口！");
                    break;
                case 2:
                    msg("user连接失败：找不到下位机！");
                    Gobals.logger?.info(LOG, "user连接失败：找不到下位机！");
                    break;
            }
            stop();
        }

        private bool check() {
            UserEmuart.bufferClear();//清除接收数组缓冲区
            UserVersion = LittleUserVersion = McuId = -1;
            byte[]? tmpRecv = UserEmuart.send(new byte[] { 144 }, 20, 2);
            if (tmpRecv == null || tmpRecv.Length == 0)
            {
                Thread.Sleep(20);
                tmpRecv = UserEmuart.send(new byte[] { 144 }, 20, 2);
            }
            if (tmpRecv == null) return false;
            int index = 0;
            for(int i = 0; i < tmpRecv.Length; ++i)
            {
                if (index == 0)
                {
                    if ((tmpRecv[i] & 0xe0) != 0xa0) continue; //第一字节必须为0b101xxxxx
                }
                else
                {
                    if ((tmpRecv[i] & 0x80) == 0x80 ) {//后续字节必须为0b0xxxxxxx
                        i--;
                        index = 0;
                        continue;
                    }
                }
                index++;
                if (index == 6)
                {
                    UserVersion = (tmpRecv[i-5] & 0x0f) << 14;
                    UserVersion |= (tmpRecv[i - 4] & 0x7f) << 7;
                    UserVersion |= (tmpRecv[i - 3] & 0x7f);
                    LittleUserVersion = (tmpRecv[i - 2] & 0x7f) << 7;
                    LittleUserVersion |= (tmpRecv[i - 1] & 0x7f) ;
                    McuId = (tmpRecv[i] & 0x3f);
                    return true;
                }
            }
            return false;;

        }

        #endregion

        #region 发送
        public bool sendMessage(byte[] bytes)
        {
            if (UserEmuart != null && UserEmuart.haveUE)
            {
                try { UserEmuart.send(bytes); } catch (Exception) { return false; }
            }
            return true;
        }
        #endregion

        #region 接受

        //private ConcurrentQueue<byte> recvBuffer = new ConcurrentQueue<byte>();
        //private Thread? DataProcessThread;
        //private void receiveInit() {
        //    if (DataProcessThread != null) return;
        //    DataProcessThread = new Thread(() => {
        //        while (true)
        //        {
        //            while (recvBuffer.TryDequeue(out byte b))
        //            {
        //                CreateFrame(b);
        //            }
        //        }
        //    });
        //    DataProcessThread.Start();
        //}
        private void UserDataRecv(byte[] data)
        {
            UserByteReceiveEvent?.Invoke(data);
        }
        private void UserRawDataRecv(byte[] data)
        {
            if (closing)
            {
                return;
            }
            try
            {
                listening = true;    //开始处理数据，可能会用到多线程
                //将字符串转为汉字
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                string str = Encoding.GetEncoding("GBK").GetString(data);
                UserRawDataReceiveEvent?.Invoke(str);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                listening = false;
            }
        }
        private void DebugRawDataRecv(byte[] data)
        {
            if (closing)
            {
                return;
            }
            try
            {
                listening = true;    //开始处理数据，可能会用到多线程

                //将字符串转为汉字
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                string str = Encoding.GetEncoding("GBK").GetString(data);
                DebugRawDataReceiveEvent?.Invoke(str);
                DebugEmuart.bufferClear();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                listening = false;
            }
        }

        private const int ReadBufferSize = 4096;
        private byte[] recvData=new byte[ReadBufferSize];
        //private void CreateFrame(byte d)
        //{
        //    byte[] CRC = new byte[2];
        //    UInt16 crc;
        //    if ((frameIndex == 0 && d != emuartFrameHead))//未接收到数据或者未遇到帧头
        //    {
        //        frameIndex = frameLength = 0;
        //        return;
        //    }
        //    if (frameIndex == ReadBufferSize - 1) {
        //        frameIndex = 0;
        //        return;
        //    }
        //    recvData[frameIndex++] = d;  //存入数据
        //    if (frameIndex == 1)//读取有效数据长度
        //    {
        //        frameLength = recvData[1];
        //    }
        //    if (frameLength >= 0 && frameIndex >= frameLength + 4)//接收到的数据达到一帧长度。23为帧头帧尾等长度
        //    {
        //        int prei = frameIndex, prel = frameLength;
        //        byte[] tmpData = new byte[frameLength];

        //        Array.Copy(recvData, 4, tmpData, 0, frameLength);

        //        //CRC校验
        //        crc = emuart_crc16(tmpData, frameLength);

        //        CRC[0] = (byte)((crc >> 8) & 0xff);
        //        CRC[1] = (byte)(crc & 0xff);

        //        if (recvData[frameIndex] != emuartFrameTail  //未遇到帧尾
        //            || CRC[0] != recvData[frameIndex - 2] || CRC[1] != recvData[frameIndex - 1])//CRC检验错误
        //        {
        //            frameIndex = frameLength = 0;
        //            return;
        //        }

        //        frameIndex = frameLength = 0;
        //        UserByteReceiveEvent?.Invoke(tmpData);
        //    }
        //}

        //=====================================================================
        //函数名称：emuart_crc16
        //功能概要：将数据进行16位的CRC校验，返回校验后的结果值
        //参数说明：ptr:需要校验的数据缓存区
        //                len:需要检验的数据长度
        //函数返回：计算得到的校验值
        //=====================================================================
        UInt16 emuart_crc16(byte[] ptr, int len)
        {
            UInt16 i, j, tmp, crc16;

            crc16 = 0xffff;
            for (i = 0; i < len; i++)
            {
                crc16 = (UInt16)(ptr[i] ^ crc16);
                for (j = 0; j < 8; j++)
                {
                    tmp = (UInt16)(crc16 & 0x0001);
                    crc16 = (UInt16)(crc16 >> 1);
                    if (tmp != 0)
                        crc16 = (UInt16)(crc16 ^ 0xa001);
                }
            }
            return crc16;
        }
        #endregion

        public void stop()
        {
            EMUART e1 = DebugEmuart;
            USEREMUART e2 = UserEmuart;
            new Thread(() =>
            {
                e1?._Uartport?.Close();
                e1?._Uartport?.Dispose();
                e2?._Uartport?.Close();
                e2?._Uartport?.Dispose();
            }).Start();
            Init();
        }
        ~EmuartHandler()
        {
            if (DebugEmuart._Uartport != null)
            {
                DebugEmuart._Uartport.Close();
                DebugEmuart._Uartport.Dispose();
            }
            if (UserEmuart._Uartport != null)
            {
                UserEmuart._Uartport.Close();
                UserEmuart._Uartport.Dispose();
            }
        }
    }
}
