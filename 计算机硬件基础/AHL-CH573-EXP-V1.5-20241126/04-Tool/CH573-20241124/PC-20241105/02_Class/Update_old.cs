using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AHL_GEC
{
    class Update_old
    {
        //插入帧,总字节486
        // LayoutKind.Sequential用于强制将成员按其出现的顺序进行顺序布局,字符串转换成ANSI字符串，Pack表示1字节对齐
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct insertData
        {
            public ushort frameNum;      //总帧数
            public ushort curFrame;      //当前帧数
            public byte operate;         //操作，0：复制，1：插入，2：替换（只用于main地址跳转）
            public byte insertNum;       //一帧中插入代码块个数，代码块内地址是连续的，代码块地址是不连续的
            // SizeConst用来定义数组大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public ushort[] insertInfo;  //插入代码块的起始地址和长度,每2个数据一组，最多20组
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
            public byte[] data;          //插入数据
        }

        private List<hexStruct_old> hexList;      //传入的Hex文件链表数据
        private List<insertData> insertList;  //待发送至终端的flash插入结构体链表

        private byte[] check;       //检查帧状态，每1位表示一帧发送状态，1：已发送；0：未发送

        //======================================================================
        //函数名称：Update构造函数
        //函数返回：无
        //参数说明：hexList：待更新的Hex文件链表数据
        //功能概要：Update构造函数，根据传入的Hex数据构造返回终端的更新数据帧和命令帧
        //======================================================================
        public  Update_old(List<hexStruct_old> hexList)
        {
            //（1）变量声明
            int i;
            ushort sum, cur;
            insertData insdata;

            //（2）获得传入的Hex文件链表数据
            this.hexList = hexList;
            //（3）初始化发送至终端的链表
            this.insertList = new List<insertData>();
            //（4）构造开始帧，数据任意,在发送至终端时命令需设为0
            insdata = new insertData();
            insdata.insertInfo = new ushort[40];
            insdata.data = new byte[400];
            this.insertList.Add(insdata);//将开始帧插入链表
            //（5）构造数据帧，发送时命令为1
            getBackList();
            //（6）构造检查帧和更新帧，数据任意，其对应的命令分别为2，3
            this.insertList.Add(insdata);  //添加检查帧
            this.insertList.Add(insdata);  //添加更新帧
            //（7）修改总帧数和帧号
            sum = (ushort)this.insertList.Count;
            cur = 0;
            for (i = 0; i < this.insertList.Count; i++)
            {
                insdata = this.insertList[i];
                insdata.frameNum = sum;
                insdata.curFrame = cur++;
                this.insertList.RemoveAt(i);
                this.insertList.Insert(i, insdata);
            }
            //（8）初始化帧发送标识
            check = new byte[sum];
            for (i = 0; i < check.Length; i++) check[i] = 0;
        }

        //======================================================================
        //函数名称：getNextFrame
        //函数返回：byte[]:下一个未发送至终端的数据帧
        //参数说明：无
        //功能概要：根据check数组来获取下一帧，进行组帧操作并返回
        //======================================================================
        public byte[] getNextFrame()
        {
            //（1）变量声明
            int i, j;
            byte order;
            byte[] data;
            byte[] frame;

            data = null;
            frame = null;
            order = 1;
            //（2）找到当前待更新的帧
            for (i = 0; i < check.Length; i++)
            {
                if (check[i] == 0)
                {
                    check[i] = 1;
                    break;
                }
            }
            if (i >= check.Length) return null;
            //（3）判断发送命令
            if (i == 0)                      //开始帧
                order = 0;
            else if (i == check.Length - 2)  //检查帧
                order = 2;
            else if (i == check.Length - 1)  //更新帧
                order = 3;
            else                             //数据帧
                order = 1;
            //（4）从链表取出数据,插入帧中查找帧数据
            for (j = 0; j < this.insertList.Count; j++)
            {
                if (this.insertList[j].curFrame == i)
                {
                    //结构体数据转为byte数组
                    data = structToBytes(this.insertList[j], Marshal.SizeOf(this.insertList[j]));
                    goto getNextFrame_ENCODE;
                }
            }
        //（5）组帧操作
        getNextFrame_ENCODE:
            if (order == 3) check[i] = 1;  //对于更新帧，不需要获取返回值
            frame = new byte[data.Length + 1];
            frame[0] = order;
            Array.Copy(data, 0, frame, 1, data.Length);
            return frame;
        }


        //======================================================================
        //函数名称：getFrameNum
        //函数返回：返回当前更新帧个数
        //参数说明：无
        //功能概要：获取当前更新帧个数
        //======================================================================
        public int getFrameNum()
        {
            int count;

            count = this.insertList.Count;

            return count;
        }

        //======================================================================
        //函数名称：getCurCount
        //函数返回：返回当前准备更新的帧号
        //参数说明：无
        //功能概要：获取当前准备更新的帧号
        //======================================================================
        public int getCurCount()
        {
            int i;

            for (i = 0; i < check.Length; i++)
            {
                if (check[i] == 0) return i;
            }

            return -1;
        }

        //======================================================================
        //函数名称：getSendCount
        //函数返回：返回当前已更新的帧个数
        //参数说明：无
        //功能概要：获取当前已更新的帧个数
        //======================================================================
        public int getSendCount()
        {
            int i;
            int count;

            count = 0;
            for (i = 0; i < check.Length; i++)
            {
                if (check[i] == 1) count++;
            }

            return count;
        }

        //======================================================================
        //函数名称：checkClear
        //函数返回：index:清检查帧某位，可实现该帧重新发送
        //参数说明：无
        //功能概要：清空某检查帧指定位，实现该帧重新发送
        //======================================================================
        public void checkClear(int index)
        {
            if (index >= this.insertList.Count)
                index = this.insertList.Count - 1;
            else if (index < 0)
                index = 0;

            check[index] = 0;
        }

        //======================================================================
        //函数名称：isEmpty
        //函数返回：true:更新帧更新完毕;false:更新帧未更新完
        //参数说明：无
        //功能概要：判断当前更新帧是否更新完毕
        //======================================================================
        public bool isEmpty()
        {
            //（1）变量声明
            int i;
            //（2）若check数组有0值，表示当前有更新帧未发送至终端
            for (i = 0; i < check.Length; i++)
            {
                if (check[i] == 0)
                    return false;
            }

            return true;
        }

        public byte[] frameEncode(byte[] SendByteArray)
        {
            //（1）组发送的数据帧
            byte[] sendData = new byte[SendByteArray.Length + 2];
            int index = 0;
            //用于判断是否是服务器端远程更新软件发来的数据
            sendData[index++] = 0xa5;
            sendData[index++] = 0x06;
            //数据
            if (SendByteArray.Length > 0)
                Array.Copy(SendByteArray, 0, sendData, index, SendByteArray.Length);
            return sendData;
        }


        //======内部函数======
        //======================================================================
        //函数名称：getBackList
        //函数返回：无
        //参数说明：无
        //功能概要：根据传入的Hex文件数据按照插入帧的格式构造数据帧（非命令帧）
        //======================================================================
        private void getBackList()
        {
            //（1）变量声明
            int i, j;
            ushort addr;
            ushort len;
            ushort lastAddr;
            ushort lastLen;
            ushort sumLen;
            byte[] data;

            insertData idata;

            //（2）数据初始化
            idata = new insertData();
            idata.insertInfo = new ushort[40];
            idata.data = new byte[400];

            //（3）遍历Hex数据链表来构造返回插入链表
            for (i = 0; i < this.hexList.Count; i++)
            {
                //（3.1）获取本行数据基本信息
                //获取本行代码待插入的地址
                addr = this.hexList[i].address;
                //获取本行代码长度
                len = this.hexList[i].len;
                //获取本行代码数据
                data = this.hexList[i].data;

                //（3.2）进行插入帧赋值操作
                if (idata.insertNum == 0)  //判断当前行数据是否是第一个数据
                {
                    idata.operate = 1;
                    idata.insertNum = 1;
                    idata.insertInfo[0] = addr;
                    idata.insertInfo[1] = len;
                    Array.Copy(data, 0, idata.data, 0, len);
                }
                else  //其他情况
                {
                    //获取当前已插入的数据总长度
                    sumLen = 0;
                    for (j = 0; j < idata.insertNum; j++)
                        sumLen += idata.insertInfo[j * 2 + 1];
                    //判断已插入的数据是否超出上限，包括插入个数和总的插入数据长度
                    if ((idata.insertNum >= 20) || (sumLen + len > 400))  //超出界限，新建一帧数据
                    {
                        //将当前插入帧加入链表
                        this.insertList.Add(idata);
                        //新建一个插入帧并赋值
                        idata = new insertData();
                        idata.insertInfo = new ushort[40];
                        idata.data = new byte[400];
                        idata.operate = 1;
                        idata.insertNum = 1;
                        idata.insertInfo[0] = addr;
                        idata.insertInfo[1] = len;
                        Array.Copy(data, 0, idata.data, 0, len);
                    }
                    else  //未超出上限，再本插入帧中继续添加数据
                    {
                        //判断本次插入数据是否与上次的相连
                        //先取出先前一行数据信息，包括地址和长度
                        lastAddr = idata.insertInfo[(idata.insertNum - 1) * 2];
                        lastLen = idata.insertInfo[(idata.insertNum - 1) * 2 + 1];
                        if (lastAddr + lastLen == addr)  //相连，添加数据并修改上次数据长度
                        {
                            Array.Copy(data, 0, idata.data, sumLen, len);
                            idata.insertInfo[(idata.insertNum - 1) * 2 + 1] += len;
                        }
                        else  //不相连，添加新的数据
                        {
                            idata.insertInfo[idata.insertNum * 2] = addr;
                            idata.insertInfo[idata.insertNum * 2 + 1] = len;
                            idata.insertNum++;
                            Array.Copy(data, 0, idata.data, sumLen, len);
                        }
                    }
                }//end if
            }//end for
            //（4）结束遍历，把最后的插入帧加入链表
            this.insertList.Add(idata);
        }

        //======================================================================
        //函数名称：structToBytes
        //函数返回：结构体转换为对应的byte数组
        //参数说明：structObj:结构体数据;size:结构体数据字节大小
        //功能概要：将结构体数据转换为对应的byte数据
        //======================================================================
        private byte[] structToBytes(object structObj, int size)
        {
            //（1）变量声明
            byte[] bytes;
            IntPtr structPtr;

            //（2）变量空间申请
            bytes = new byte[size];
            structPtr = Marshal.AllocHGlobal(size);
            //（3）将结构体数据拷入分配号的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //（4）从内存空间拷贝到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //（5）释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return bytes;
        }
        //----------------------------------------------------------------------------
    }
}

