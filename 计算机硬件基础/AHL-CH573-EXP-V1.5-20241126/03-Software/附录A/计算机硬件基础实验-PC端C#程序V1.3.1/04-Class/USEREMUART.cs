using System;
using System.Collections.Concurrent;
using System.Threading;

namespace cn.edu.suda.sumcu.iot
{
    /// <summary>
    /// 类功能：搭配终端的“emuart”构件可以实现串口数据的可靠收发，并能够自动进行搜索设备
    /// 类的使用方法
    /// 【1】本类对外的接口主要包括成员变量、方法和事件。
    /// 方法包括：（1）findDevice寻找设备（2）send发送串口数据
    /// 事件包括：（1）DataReceivedEvent串口接收事件
    /// 【2】类的使用步骤
    /// （1）创建本类的对象，如：EMUART device = new EMUART();
    /// （2）使用方法findDevice查找到设备，具体findDevice的使用方法见方法头
    /// （3）使用DataReceivedEvent事件注册串口数据接收的处理函数。
    /// 如：device.DataReceivedEvent += new EMUART.DataReceived(emuart_recv);
    /// （4）在需要的时候，调用send方法进行数据发送。使用方法见方法头。
    /// </summary>
    public class USEREMUART
    {
        //串口操作工具类
        //实例化系统串口类

        public delegate void DataReceived(byte[] data);
        public event DataReceived? DataReceivedEvent;             //数据接收事件   
        public delegate void RawDataReceived(byte[] data);
        public event RawDataReceived? RawDataReceivedEvent;             //数据接收事件   
        public ISerialPort? _Uartport;       //系统提供的串口对象
        public bool haveUE = false;
        public bool OnUpdate = false;
        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能: 构造函数                           
        /// </summary>                                                                     
        /// ------------------------------------------------------------------------------
        public USEREMUART()
        {
            this._Uartport = null;
            this.haveUE = false;
            receiveInit();
        }



        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:设置超时时间。                                  
        /// </summary>                                                              
        /// -----------------------------------------------------------------------------
        public class ISerialPort : System.IO.Ports.SerialPort
        {
            private bool IOpen = true;
            public new bool IsOpen
            {
                get
                {
                    return base.IsOpen && IOpen;
                }
            }
            public readonly int DefaultTimeOut = 200;
            public ISerialPort() : base()
            {
                ReadTimeout = WriteTimeout = DefaultTimeOut;//设置读写超时时间，单位：ms

            }
            public new void Open()
            {
                Open(DefaultTimeOut);
            }
            public void Open(int timeout)
            {
                Action action = () =>
                {
                    try
                    {
                        IOpen = true;
                        base.Open();
                    }
                    catch { }
                };
                Thread thread = new Thread(() =>
                {
                    action.Invoke();
                });
                thread.Start();
                thread.Join(timeout);
                if (thread.ThreadState == ThreadState.Running)
                {
                    IOpen = false;
                }
            }
        }
        
        public delegate bool Checker();
        public int findDeviceByChecker(Checker checker, out string com, int baudRate = 9600)
        {
            com = "";
            //（1）定义并初始化局部变量
            int i;
            string s1 = string.Empty;    //初始化临时字符串（置空）;


            //若串口不为空，且被打开了，则首先关闭。防止多次调用本函数出错
            if (_Uartport != null && _Uartport.IsOpen) _Uartport.Close();
            //（2）查找是否有串口
            //查询所有串口，串口数在uartNoArray[0]中
            string[] uartCom = System.IO.Ports.SerialPort.GetPortNames();
            //若没有找到串口，直接返回1
            if (uartCom.Length == 0)
            {
                return 1;
            }
            //（3）若存在串口，则遍历串口，并判断是否为目标设备。
            bufferClear();//清除接收数组缓冲区


            for (i = 0; i < uartCom.Length; i++)
            {
                com = uartCom[i];
                if (!open(uartCom[i], baudRate)) continue;    //若打开失败，则继续遍历
                this.bufferClear();//清除接收数组缓冲区

                if (checker())
                {
                    return 0;
                }

                if (_Uartport != null) _Uartport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数

                this.haveUE = true;
            }
            return 2;

        }


        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:结束终端设备的组帧过程，使之可以响应握手过程。                                  
        /// </summary>                                                                                                             
        /// <param name="baudRate">波特率，默认值为9600</param>                                                           
        /// -----------------------------------------------------------------------------
        public int terminate(int baudRate = 9600)
        {
            //（1）定义并初始化局部变量
            int i;
            byte[] terminate = { (byte)'S', (byte)'t', (byte)'O', (byte)'p', (byte)'U', (byte)'e', (byte)'M', (byte)'y', (byte)'S', (byte)'e', (byte)'L', (byte)'f' };
            //（2）查找是否有串口
            //查询所有串口，串口数在uartNoArray[0]中
            string[] uartCom = System.IO.Ports.SerialPort.GetPortNames();
            //若没有找到串口，直接返回1
            if (uartCom.Length == 0) return 1;
            //若串口不为空，且被打开了，则首先关闭。防止多次调用本函数出错
            if (_Uartport != null && _Uartport.IsOpen) _Uartport.Close();

            //（3）若存在串口，则遍历串口，并逐一发送结束字符串。
            for (i = 0; i < uartCom.Length; i++)
            {
                //（3.1）打开串口
                if (!open(uartCom[i], baudRate)) continue;    //若打开失败，则继续遍历
                //（3.2）发送结束组帧字符串，并等待返回
                _Uartport?.Write(terminate, 0, terminate.Length);  //发送数据

            }
            return 0;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:发送数据到终端，并等待终端返回,并可以设置最长等待时间                                  
        /// </summary>                                                                     
        /// <param name="SendByteArray">待发送的数据</param>                                          
        /// <param name="RecvData">输出参数，存储串口返回的数据</param>                                          
        /// <param name="time">最长等待时间，单位为毫秒，默认为500毫秒</param>
        /// <param name="cnt">最多重复尝试次数</param>                                          
        /// <returns>true：发送成功；false：发送失败</returns>                  
        /// -----------------------------------------------------------------------------
        public byte[]? send(byte[] SendByteArray, int time = 500, int cnt = 1)
        {
            //（1）定义并初始化局部变量
            byte[]? RecvData = null;
            try
            {
                //（2）若串口为空，或尚未打开，或发送数据为空，则直接返回发送失败
                if (_Uartport == null || !_Uartport.IsOpen || SendByteArray == null)
                    return null;
                //仅发送，不接收 20190516 （2/2）
                if (time == 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        //不关闭接收事件 20190520 （1/1）
                        _Uartport.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                        Thread.Sleep(20);
                        _Uartport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                        this.send(SendByteArray);           //发送数据
                    }

                    return null;

                }

                //（3）将发送数据进行组帧，并发送
                for (int j = 0; j < cnt; j++)
                {
                    //设置串口接收事件的触发条件为ReadBufferSize，目的是暂时关闭串口接收事件
                    _Uartport.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                    System.Threading.Thread.Sleep(10);  //防止串口频繁发送致死   //todo
                    this.send(SendByteArray);           //发送数据

                    //（3）等待有正确数据返回
                    for (int i = 0; i < time; i++)
                    {
                        System.Threading.Thread.Sleep(1);
                        RecvData = read();
                        if (RecvData == null) continue;
                        if (RecvData.Length != 0)
                        {
                            _Uartport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                            return RecvData;
                        }

                    }
                }
                _Uartport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                return RecvData;
            }
            catch
            {
                if(_Uartport!=null) _Uartport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                return RecvData;
            }
        }



        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:发送数据到终端，不等待终端返回                                  
        /// </summary>                                                                     
        /// <param name="SendByteArray">待发送的数据</param>                                          
        /// <returns>true：发送成功；false：发送失败</returns>                  
        /// -----------------------------------------------------------------------------
        public bool send(byte[] SendByteArray)
        {
            //（1）判断入口参数
            if (_Uartport == null || !_Uartport.IsOpen || SendByteArray.Length <= 0)
                return false;
            //（2）组发送的数据帧
            byte[] sendData = new byte[SendByteArray.Length + 5];
            int index = 0;
            //帧头
            sendData[index++] = emuartFrameHead;
            //数据长度
            sendData[index++] = (byte)(SendByteArray.Length);
            //数据
            if (SendByteArray.Length > 0) Array.Copy(SendByteArray, 0, sendData, index, SendByteArray.Length);
            index += SendByteArray.Length;
            //CRC校验
            UInt16 temp16 = emuart_crc16(SendByteArray, SendByteArray.Length);
            sendData[index++] = (byte)(temp16 >> 8);
            sendData[index++] = (byte)(temp16);
            //发送帧尾
            sendData[index++] = emuartFrameTail;
            //（3）发送数据
            _Uartport.Write(sendData, 0, sendData.Length);  //发送数据
            return true;
        }


        private ConcurrentQueue<byte> recvBuffer = new ConcurrentQueue<byte>();
        private Thread? DataProcessThread;
        private void receiveInit()
        {
            if (DataProcessThread != null) return;
            DataProcessThread = new Thread(() => {
                while (true)
                {
                    while (recvBuffer.TryDequeue(out byte b))
                    {
                        CreateFrame(b);
                    }
                }
            });
            DataProcessThread.Start();
        }


        int frameIndex = 0;
        int frameLength = 0;
        private void CreateFrame(byte d)
        {
            byte[] CRC = new byte[2];
            UInt16 crc;
            if ((frameIndex == 0 && d != emuartFrameHead))//未接收到数据或者未遇到帧头
            {
                frameIndex = frameLength = 0;
                return ;
            }
            if (frameIndex == ReadBufferSize - 1)
            {
                frameIndex = 0;
                return ;
            }
            recvData[frameIndex++] = d;  //存入数据
            if (frameIndex == 1)//读取有效数据长度
            {
                frameLength = recvData[1];
            }
            if (frameLength >= 0 && frameIndex > frameLength + 4)//接收到的数据达到一帧长度。23为帧头帧尾等长度
            {
                int prei = frameIndex, prel = frameLength;
                byte[] tmpData = new byte[frameLength];

                Array.Copy(recvData, 2, tmpData, 0, frameLength);

                //CRC校验
                crc = emuart_crc16(tmpData, frameLength);

                CRC[0] = (byte)((crc >> 8) & 0xff);
                CRC[1] = (byte)(crc & 0xff);

                if (recvData[frameIndex-1] != emuartFrameTail  //未遇到帧尾
                    || CRC[0] != recvData[frameIndex - 3] || CRC[1] != recvData[frameIndex - 2])//CRC检验错误
                {
                    frameIndex = frameLength = 0;
                    return ;
                }

                frameIndex = frameLength = 0;
                DataReceivedEvent?.Invoke(tmpData);
            }
        }

        //串口接收事件
        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:串口接收处理函数。                                  
        /// </summary>                                                                     
        /// <param name="sender">事件触发者</param>                                          
        /// <param name="e">串口事件参数</param>                                          
        /// <returns>无</returns>                  
        /// -----------------------------------------------------------------------------
        private void recv(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int len = 0;
            try
            {
                if (_Uartport != null && _Uartport.IsOpen && _Uartport.BytesToRead > 0)
                {
                    len = _Uartport.BytesToRead;
                    byte[] data = new byte[len];
                    _Uartport.Read(data, 0, len);
                    if (RawDataReceivedEvent != null) RawDataReceivedEvent(data);//触发数据接收事件
                    foreach (byte b in data)
                    {
                        recvBuffer.Enqueue(b);
                    }
                }
            }
            catch
            {

            }

        }


        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:打开串口                            
        /// </summary>                                                                     
        /// <param name="portName">串口名</param>                                          
        /// <param name="baudRate">波特率</param>                                          
        /// <returns>true：打开成功；false：打开失败</returns>                  
        /// -----------------------------------------------------------------------------
        private bool open(string portName, int baudRate = 9600)
        {
            if (_Uartport != null && _Uartport.IsOpen)
            {
                _Uartport.Close();
                _Uartport.Dispose();
                Thread.Sleep(20);
            }

            _Uartport = new ISerialPort();
            _Uartport.PortName = portName;   //设置串口号
            _Uartport.BaudRate = baudRate;   //设置波特率
            _Uartport.Parity = System.IO.Ports.Parity.None;//设置无奇偶校验
            _Uartport.DataBits = 8;          //设置8比特数据位
            _Uartport.StopBits = System.IO.Ports.StopBits.One;//设置1位停止位
            _Uartport.ReadBufferSize = ReadBufferSize; //接收缓冲区大小(字节) 
            _Uartport.WriteBufferSize = WriteBufferSize;//发送缓冲区大小(字节)
            _Uartport.ReceivedBytesThreshold = 1;      //设置串口触发条件
            try
            {

                _Uartport.Open();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        //


        public bool reOpen(string portName, int baudRate = 9600)
        {
            if (open(portName, baudRate) == true)
            {
                if (_Uartport != null) _Uartport.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(recv);//设置接收处理函数
                return true;
            }
            else
            {
                return false;
            }
        }






        /// ------------------------------------------------------------------------------
        /// <summary>  
        /// 功    能:读串口接收数据并返回                            
        /// </summary>                                                                     
        /// <returns>读取到的数据</returns>                  
        /// -----------------------------------------------------------------------------
        private byte[]? read()
        {
            byte[]? RecvData = null;
            byte[] buff = new byte[ReadBufferSize];
            if (_Uartport == null) return null;
            int count = _Uartport.BytesToRead;
            if (count > 0)
                _Uartport.Read(buff, 0, count);
            //while (_Uartport.BytesToRead > 0 && i < ReadBufferSize)
            //{
            //    try
            //    {
            //        buff[i++] = (byte)_Uartport.ReadByte();
            //        _Uartport.Read(,)
            //    }
            //    catch { }
            //}
            RecvData = new byte[count];
            Array.Copy(buff, RecvData, count);   //将数据返回
            return RecvData;
        }

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

        public void bufferClear()
        {
            Array.Clear(this.recvData, 0, this.recvData.Length);
            Array.Clear(this.intRecvData, 0, this.intRecvData.Length);
        }



        //============================START==定义私有变量====================================
        private const int ReadBufferSize = 4096;            //接收缓冲区大小设置
        private const int WriteBufferSize = 2048;           //发送缓冲区大小设置
        private byte emuartFrameHead = 0x80;
        private byte emuartFrameTail =  0x81;
        private byte[] recvData = new byte[ReadBufferSize]; //数据接收缓冲区
        private byte[] intRecvData = new byte[ReadBufferSize]; //数据接收缓冲区
        //============================END==定义私有变量====================================
    }
}
