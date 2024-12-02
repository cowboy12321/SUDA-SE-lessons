using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AHL_GEC
{
    class Update
    {
        //更新开始帧  帧标识为0  共10个字节
        // LayoutKind.Sequential用于强制将成员按其出现的顺序进行顺序布局,字符串转换成ANSI字符串，Pack表示1字节对齐
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct startData
        {
            public byte type;          //帧标识 0
            public byte style;         //更新方式，0：整体替换更新；1：整体保留更新；2：增量保留更新
            public ushort frameNum;    //总帧数
            public ushort curFrame;    //当前帧号
            public uint codeSize;      //更新代码总大小（用于定位flash擦除时定位扇区位置和大小）
        }

        //更新复制帧  帧标识为1  共474个字节
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct copyData
        {
            public byte type;           //帧标识 1
            public ushort curFrame;     //当前帧号
            public byte segNum;         //复制数据段个数
            // SizeConst用来定义数组大小
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 94)]  //2*47，最多47组，字节376
            public uint[] addInfo;      //复制数据段地址信息（2个一组，起始地址、复制地址）
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 47)]  //1*47，最多47组，字节94
            public ushort[] lenInfo;    //复制数据段长度信息
        }

        //更新插入帧  帧标识为2  共476个字节
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct insertData
        {
            public byte type;           //帧标识 2
            public ushort curFrame;     //当前帧号
            public byte segNum;         //插入代码段个数
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]  //1*12，最多12组，字节48
            public uint[] addInfo;      //插入代码段地址信息
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]  //1*12，最多12组，字节24
            public ushort[] lenInfo;    //插入代码段长度信息
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
            public byte[] data;         //插入数据
        }

        //更新检查帧  帧标识为3  共85个字节
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct checkData
        {
            public byte type;           //帧标识 3
            public ushort curFrame;     //当前帧号
            public byte loseNum;      //丢失帧数
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public ushort[] loseFrame;  //丢失帧号，最多40个
        }

        //更新命令帧  帧标识为4  共3个字节
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct updateData
        {
            public byte type;           //帧标识 4
            public ushort curFrame;     //当前帧号
        }

        //更新返回帧  帧标识为5  共4个字节
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct backData
        {
            public byte type;           //帧标识 5
            public ushort curFrame;     //返回当前帧号
            public byte flag;           //返回状态 0：成功；其他：失败
        }

        private struct lcs  //用于存储新旧Hex文件的最长公共子序列
        {
            public int lineNum;  //行号 旧Hex行号
            public int rowNum;   //列号 新Hex行号
        }

        private List<hexStruct> oldHexList;  //原Hex文件链表数据
        private List<hexStruct> newHexList;  //新Hex文件链表数据
        private List<lcs> lcsList;           //LCS算法优化后的最大公共子序列链表

        private List<copyData> copyList;
        private List<insertData> insertList;

        private List<byte[]> reList;         //返回的数据链表
        byte[] check;                        //检查帧状态，每1位表示一帧数据发送状态，1：已发送；0：未发送

        private int updateFlag;  //更新标志 0：默认初始值；
                                 //         1：未进行过文本比较，整体更新；
                                 //         2：新旧Hex文件一致，无需更新；
                                 //         3：进行过文本比较，增量更新；
        private byte updateStyle; //更新方式 0：整体替换更新；
                                 //         1：整体保留更新；
                                 //         2：增量保留更新；

        //======================================================================
        //函数名称：Update
        //函数返回：无
        //参数说明：style:更新方式，0：整体替换更新；1：整体保留更新；2：增量保留更新
        //          newHexList：待更新的Hex文件数据；oldHexList：原Hex文件数据；
        //功能概要：Update构造函数，初始化Update类全局变量，并进行文本比较构造
        //          返回帧数据
        //======================================================================
        public Update(byte style = 0, List<hexStruct> newHexList = null, List<hexStruct> oldHexList = null)
        {
            //（1）Update类全局变量初始化
            this.updateStyle = style;
            this.oldHexList = oldHexList;
            this.newHexList = newHexList;
            this.lcsList = new List<lcs>();
            this.copyList = new List<copyData>();
            this.insertList = new List<insertData>();
            this.reList = new List<byte[]>();
            this.check = new byte[0];
            //（2）替换更新不需要原Hex文件
            if (style == 0 || style == 1) this.oldHexList = null;
            //（3）构造更新数据
            if (newHexList != null)
            {
                //找出新旧两个Hex文件的最长公共子序列
                Needleman_Wunsch();  //至此，lcsList已赋值
                //构造更新复制链表和更新插入链表数据
                lcsCalculate();
                //构造更新返回数据
                getReturnData();
            }
        }

        //======================================================================
        //函数名称：getNextFrame
        //函数返回：byte[]:下一帧待发送至终端的更新数据帧
        //参数说明：无
        //功能概要：根据check数组来获取下一帧数据
        //======================================================================
        public byte[] getNextFrame()
        {
            //（1）变量声明
            int i;

            //（2）确定下一帧更新的帧号
            for (i = 0; i < this.check.Length; i++)
            {
                if (this.check[i] == 0) break;    //找到未更新的帧
            }

            if (i == this.check.Length - 1) this.check[i] = 1;  //更新命令帧无需返回

            if (i >= this.check.Length)  //所有的数据已发送完毕
                return null;

            //（3）从返回链表中取出对应的数据并返回
            return this.reList[i];
            //return this.reList[1];
        }

        public byte[] get11frame()
        {
            return this.reList[11];
        }

        //======================================================================
        //函数名称：updateRecv
        //函数返回：0：终端返回数据成功
        //          1：接收到的非更新返回帧数据；2：更新返回帧帧标识错误；
        //          3：终端执行异常
        //参数说明：frame：终端执行更新操作后的返回数据
        //功能概要：更新数据帧发送至终端后，终端执行返回数据至frame，
        //          解析frame数据来判断终端操作状态
        //======================================================================
        public int updateRecv(byte[] frame)
        {
            //（1）变量声明
            int i;
            int rv;
            backData backdata;
            checkData checkdata;

            rv = 0;  //返回值默认成功，为0
            //（2）判断接收返回帧类型
            if (frame.Length == Marshal.SizeOf(typeof(backData)))  //接收到更新返回帧
            {
                backdata = (backData)bytesToStruct(frame, typeof(backData));
                //判断返回帧标识是否为5
                if (backdata.type == 5)
                {
                    //判断终端执行成功与否
                    if (backdata.flag == 0 || backdata.flag == 1 || backdata.flag == 2)
                    {
                        this.check[backdata.curFrame] = 1;
                    }
                    //其他终端执行状态，待添加
                    else if (backdata.flag == 3) //终端之前更新过程中断，需重新开始
                    {
                        for (i = 0; i < this.check.Length; i++)
                        {
                            this.check[i] = 0;
                        }
                    }
                    else
                    {
                        //终端执行异常
                        rv = 3;
                    }
                }
                else
                {
                    //更新返回帧帧标识错误
                    rv = 2;
                }
            }
            else if(frame.Length == Marshal.SizeOf(typeof(checkData)))  //接收到更新检查帧
            {
                checkdata = (checkData)bytesToStruct(frame, typeof(checkData));
                if (checkdata.type == 3)
                {
                    for (i = 0; i < checkdata.loseNum; i++)
                    {
                        this.check[checkdata.loseFrame[i]] = 0;
                    }
                }
                else
                {
                    //更新返回帧帧标识错误
                    rv = 2;
                }
            }
            else
            {
                //接收到的非更新返回帧数据
                rv = 1;
            }

            return rv;
        }

        //======================================================================
        //函数名称：getFrameNum
        //函数返回：返回当前更新帧总个数
        //参数说明：无
        //功能概要：获取当前更新帧总个数
        //======================================================================
        public int getFrameNum()
        {
            return this.check.Length;
        }

        //======================================================================
        //函数名称：getUpdatedCount
        //函数返回：返回当前已更新的帧个数
        //参数说明：无
        //功能概要：获取当前已更新的帧个数
        //======================================================================
        public int getUpdatedCount()
        {
            //（1）变量声明
            int i;
            int count;

            //（2）获取当前已更新的帧个数
            count = 0;
            for (i = 0; i < this.check.Length; i++)
                if (this.check[i] == 1) count++;

            return count;
        }

        //======================================================================
        //函数名称：getNextIndex
        //函数返回：返回下一个待更新的帧号
        //参数说明：无
        //功能概要：获取下一个待更新的帧号
        //======================================================================
        public int getNextIndex()
        {
            //（1）变量声明
            int i;

            //（2）确定下一帧更新的帧号
            for (i = 0; i < this.check.Length-1; i++)
            {
                if (this.check[i] == 0) break;
            }

            return i;
        }


        //-------------------------------内部函数-------------------------------------

        //======================================================================
        //函数名称：getReturnData
        //函数返回：无
        //参数说明：无
        //功能概要：构造更新返回数据，共5种帧（更新开始帧、更新复制帧、更新插入帧
        //         、更新检查帧、更新命令帧），将其转换为byte数组后插入返回链表中
        //======================================================================
        private void getReturnData()
        {
            //（1）变量声明
            ushort sum;     //待发送的总帧数
            ushort curNum;  //当前帧号
            uint codesize;  //代码大小
            int nListCount; //新Hex文件链表长度
            byte[] data;    //返回数据
            startData startdata = new startData();   //更新开始帧变量
            copyData copydata;                       //更新复制帧变量
            insertData insertdata;                   //更新插入帧变量
            checkData checkdata = new checkData();   //更新检查帧变量
            updateData updatedata = new updateData();   //更新命令帧变量

            //（2）计算本次待更新的代码大小
            nListCount = this.newHexList.Count;
            codesize = this.newHexList[nListCount - 1].address + this.newHexList[nListCount - 1].len - this.newHexList[0].address;

            //（3）判断是否进行过文本比较
            if (this.lcsList.Count == 0)  //未进行文本比较操作，需进行整体更新
            {
                this.updateFlag = 1;
                sum = (ushort)(this.insertList.Count + 3);  //无更新复制帧数据
                curNum = 0;
                this.check = new byte[sum];  //初始化检查帧状态
                //更新开始帧
                startdata.type = 0;
                startdata.style = this.updateStyle;
                startdata.frameNum = sum;
                startdata.curFrame = curNum++;
                startdata.codeSize = codesize;
                data = structToBytes(startdata, Marshal.SizeOf(startdata));//结构体转数组
                this.reList.Add(data);  //返回链表添加数据
                //更新插入帧
                foreach(var idata in this.insertList)
                {
                    insertdata = idata;
                    insertdata.curFrame = curNum++;
                    data = structToBytes(insertdata, Marshal.SizeOf(insertdata));//结构体转数组
                    this.reList.Add(data);  //返回链表添加数据
                }
                //更新检查帧
                checkdata.type = 3;
                checkdata.curFrame = curNum++;
                data = structToBytes(checkdata, Marshal.SizeOf(checkdata));//结构体转数组
                this.reList.Add(data);  //返回链表添加数据
                //更新命令帧
                updatedata.type = 4;
                updatedata.curFrame = curNum;
                data = structToBytes(updatedata, Marshal.SizeOf(updatedata));//结构体转数组
                this.reList.Add(data);  //返回链表添加数据
            }
            else if (this.lcsList.Count == this.newHexList.Count &&  //新旧Hex文件一致，无需更新
                this.lcsList.Count == this.oldHexList.Count)
            {
                this.updateFlag = 2;
            }
            else   //进行过文本比较
            {
                this.updateFlag = 3;
                sum = (ushort)(this.copyList.Count + this.insertList.Count + 3);  //无更新复制帧数据
                curNum = 0;
                this.check = new byte[sum];  //初始化检查帧状态
                //更新开始帧
                startdata.type = 0;
                startdata.style = this.updateStyle;
                startdata.frameNum = sum;
                startdata.curFrame = curNum++;
                startdata.codeSize = codesize;
                data = structToBytes(startdata, Marshal.SizeOf(startdata));//结构体转数组
                this.reList.Add(data);  //返回链表添加数据
                //更新复制帧
                foreach (var cdata in this.copyList)
                {
                    copydata = cdata;
                    copydata.curFrame = curNum++;
                    data = structToBytes(copydata, Marshal.SizeOf(copydata));//结构体转数组
                    this.reList.Add(data);  //返回链表添加数据
                }
                //更新插入帧
                foreach (var idata in this.insertList)
                {
                    insertdata = idata;
                    insertdata.curFrame = curNum++;
                    data = structToBytes(insertdata, Marshal.SizeOf(insertdata));//结构体转数组
                    this.reList.Add(data);  //返回链表添加数据
                }
                //更新检查帧
                checkdata.type = 3;
                checkdata.curFrame = curNum++;
                data = structToBytes(checkdata, Marshal.SizeOf(checkdata));//结构体转数组
                this.reList.Add(data);  //返回链表添加数据
                //更新命令帧
                updatedata.type = 4;
                updatedata.curFrame = curNum;
                data = structToBytes(updatedata, Marshal.SizeOf(updatedata));//结构体转数组
                this.reList.Add(data);  //返回链表添加数据
            }
        }

        //======================================================================
        //函数名称：lcsCalculate
        //函数返回：无
        //参数说明：无
        //功能概要：构造更新复制链表和更新插入链表数据
        //======================================================================
        private void lcsCalculate()
        {
            //（1）变量声明
            int i, j;
            int lcsCount;      //lcs链表遍历计数（游标）
            int oldIndex;      //子序列在旧Hex文件中的位置
            uint oldAddress;   //子序列在旧Hex文件中的Flash位置信息
            ushort len;        //子序列在旧/新Hex文件中的有效数据长度

            int newIndex;      //子序列在新Hex文件中的位置
            uint newAddress;   //子序列在新Hex文件中的Flash位置信息（或插入行数据在新Hex文件中的Flash位置信息）

            byte[] data;       //新Hex文件中的行有效数据信息

            int preSegIndex;   //更新复制链表已有的数据段个数（或更新插入链表已有的代码段个数）
            uint preOldAddress;//更新复制链表中前一个数据段的起始地址
            uint preNewAddress;//更新复制链表中前一个数据段的目标地址
            uint preAddress;   //更新插入链表中前一个代码段的目标地址
            ushort preLen;     //更新复制链表中前一个数据段的数据长度（或更新插入链表中前一个代码段的数据长度）

            int sumLen;        //更新插入帧中有效数据总长

            copyData cdata = new copyData();
            cdata.addInfo = new uint[94];
            cdata.lenInfo = new ushort[47];

            insertData idata = new insertData();
            idata.addInfo = new uint[12];
            idata.lenInfo = new ushort[12];
            idata.data = new byte[400];

            //（2）遍历
            lcsCount = 0;
            for (i = 0; i < this.newHexList.Count; i++)
            {
                //若lcs链表不为空，且当前新Hex行数据在最大公共子序列中，进行复制操作
                if (this.lcsList.Count != 0 && lcsCount < this.lcsList.Count
                    && this.lcsList[lcsCount].rowNum == i)
                {
                    //获得当前子序列在新旧Hex文件中的位置
                    oldIndex = this.lcsList[lcsCount].lineNum;  //旧Hex文件中的位置
                    newIndex = this.lcsList[lcsCount].rowNum;    //新Hex文件中的位置
                    //获取当前子序列在新旧Hex文件中的其他信息
                    oldAddress = this.oldHexList[oldIndex].address;
                    newAddress = this.newHexList[newIndex].address;
                    len = this.newHexList[newIndex].len;
                    //判断是否为第一帧数据
                    if (cdata.segNum == 0)
                    {
                        cdata.type = 1;    //设置更新复制帧标识为1
                        cdata.segNum = 1;  //设置复制数据段个数为1
                        cdata.addInfo[0] = oldAddress;  //设置复制的起始地址
                        cdata.addInfo[1] = newAddress;  //设置复制的目标地址
                        cdata.lenInfo[0] = len;         //设置复制的数据长度
                    }
                    //判断当前更新复制帧是否已满
                    else if (cdata.segNum >= 47)  //当前更新复制帧已满，将该帧数据插入链表并新建一个更新复制帧
                    {
                        //将当前更新复制帧插入复制帧链表
                        this.copyList.Add(cdata);
                        //新建更新复制帧，并赋值
                        cdata = new copyData();
                        cdata.addInfo = new uint[94];
                        cdata.lenInfo = new ushort[47];
                        cdata.type = 1;    //设置更新复制帧标识为1
                        cdata.segNum = 1;  //设置复制代码段个数为1
                        cdata.addInfo[0] = oldAddress;  //设置复制的起始地址
                        cdata.addInfo[1] = newAddress;  //设置复制的目标地址
                        cdata.lenInfo[0] = len;         //设置复制的数据长度
                    }
                    else  //当前更新复制帧未满
                    {
                        //判断当前子序列是否可与前一个子数据段进行拼接
                        //先获取前一个数据段的相关数据
                        preSegIndex = cdata.segNum;
                        preOldAddress = cdata.addInfo[(preSegIndex - 1) * 2];
                        preNewAddress = cdata.addInfo[(preSegIndex - 1) * 2 + 1];
                        preLen = cdata.lenInfo[preSegIndex - 1];
                        //再进行拼接判断
                        if (preOldAddress + preLen == oldAddress && preNewAddress + preLen == newAddress)
                        {
                            //可以拼接，仅需修改前一个数据段的复制数据长度即可
                            cdata.lenInfo[preSegIndex - 1] += len;
                        }
                        else 
                        {
                            //不可以拼接，增加一个新的数据段
                            cdata.addInfo[preSegIndex * 2] = oldAddress;
                            cdata.addInfo[preSegIndex * 2 + 1] = newAddress;
                            cdata.lenInfo[preSegIndex] = len;
                            cdata.segNum++;
                        }
                    }
                    //进行下一个子序列的判断
                    lcsCount++;
                }
                //否则，进行插入操作(整体更新)
                else {
                    //获取当前待插入数据在新Hex文件中的信息
                    newAddress = this.newHexList[i].address;  //获取更新插入帧目标地址信息
                    len = this.newHexList[i].len;             //获取更新插入帧数据长度
                    data = this.newHexList[i].data;           //获取更新插入帧有效数据
                    //判断是否为第一帧数据
                    if (idata.segNum == 0)
                    {
                        idata.type = 2;    //设置更新插入帧标识为2
                        idata.segNum = 1;  //设置插入代码段个数为1
                        idata.addInfo[0] = newAddress;  //设置插入的目标地址
                        idata.lenInfo[0] = len;         //设置插入的数据长度
                        Array.Copy(data, 0, idata.data, 0, len);//设置插入的有效数据
                    }
                    else 
                    {
                        //获取更新插入帧中有效数据的总数
                        sumLen = 0;
                        for (j = 0; j < idata.segNum; j++)
                        {
                            sumLen += idata.lenInfo[j];
                        }
                        //判断当前更新插入帧是否已满，插入代码段个数需在12个以内，有效数据长度少于400
                        if ((idata.segNum >= 12) || (sumLen + len > 400))  //更新插入帧已满
                        {
                            //将当前更新插入帧加入插入帧链表
                            this.insertList.Add(idata);
                            //新建更新插入帧，并赋值
                            idata = new insertData();
                            idata.addInfo = new uint[12];
                            idata.lenInfo = new ushort[12];
                            idata.data = new byte[400];
                            idata.type = 2;    //设置更新插入帧标识为2
                            idata.segNum = 1;  //设置插入代码段个数为1
                            idata.addInfo[0] = newAddress;  //设置插入的目标地址
                            idata.lenInfo[0] = len;         //设置插入的数据长度
                            Array.Copy(data, 0, idata.data, 0, len);//设置插入的有效数据
                        }
                        else  //更新插入帧未满
                        {
                            //判断当前插入数据是否可与前一个代码段进行拼接
                            //先获取前一个代码段的相关数据
                            preSegIndex = idata.segNum;
                            preAddress = idata.addInfo[preSegIndex - 1];
                            preLen = idata.lenInfo[preSegIndex - 1];
                            //再进行拼接判断
                            if (preAddress + preLen == newAddress)
                            {
                                //可以拼接，需修改前一个代码段的数据长度并添加相应数据
                                Array.Copy(data, 0, idata.data, sumLen, len);
                                idata.lenInfo[preSegIndex - 1] += len;
                            }
                            else
                            {
                                //不可以拼接，增加一个新的插入段
                                idata.addInfo[preSegIndex] = newAddress;
                                idata.lenInfo[preSegIndex] = len;
                                idata.segNum++;
                                Array.Copy(data, 0, idata.data, sumLen, len);
                            }
                        }
                    }
                }//end if
            }//end for

            //（3）遍历结束，将最后的复制帧和插入帧加入链表
            this.copyList.Add(cdata);
            this.insertList.Add(idata);
        }


        //======================================================================
        //函数名称：Needleman_Wunsch
        //函数返回：无
        //参数说明：无
        //功能概要：使用Needleman_Wunsch算法找出新旧两个Hex文件的最长公共子序列
        //======================================================================
        private void Needleman_Wunsch()
        {
            //（1）变量声明
            int i, j;
            int m;       //二维矩阵行数
            int n;       //二维矩阵列数
            int[,] c;    //c[i,j]记录序列Xi和Yj的最长公共子序列的长度
            int[,] b;    //b[i,j]记录c[i,j]的值是由哪一个子问题的解达到的，值为1代表↖、2代表↑、3代表←

            //（2）判断能否进行文本比较
            if (this.oldHexList == null || this.oldHexList.Count == 0)
                return;

            //（3）获取二维矩阵长和宽
            m = this.oldHexList.Count + 1;
            n = this.newHexList.Count + 1;

            //（4）构造矩阵
            c = new int[m, n];  //c[i,j]记录序列Xi和Yj的最长公共子序列的长度
            b = new int[m, n];  //b[i,j]记录c[i,j]的值是由哪一个子问题的解达到的，值为↖、↑、←

            //（5）初始化矩阵
            for (i = 0; i < m; i++) c[i, 0] = 0;
            for (i = 0; i < n; i++) c[0, i] = 0;

            //（6）动态规划，计算最优值
            for (i = 1; i < m; i++)
            {
                for (j = 1; j < n; j++)
                {
                    if (isEqual(this.oldHexList[i - 1].data, this.newHexList[j - 1].data))  //xi == yj
                    {
                        c[i, j] = c[i - 1, j - 1] + 1;
                        b[i, j] = 1;
                    }
                    else if (c[i - 1, j] >= c[i, j - 1])
                    {
                        c[i, j] = c[i - 1, j];
                        b[i, j] = 2;
                    }
                    else
                    {
                        c[i, j] = c[i, j - 1];
                        b[i, j] = 3;
                    }
                }
            }

            //（7）获取最长公共子序列
            lcsRegress(b, this.oldHexList.Count, this.newHexList.Count);

        }

        //======================================================================
        //函数名称：lcsRegress
        //函数返回：无
        //参数说明：b：记录c[i,j]的值是由哪一个子问题的解达到的，
        //             值为1代表↖、2代表↑、3代表←
        //          i，j：当前递归遍历的数组行和列
        //功能概要：反向递归子问题记录数组，构造最长公共子序列
        //======================================================================
        private void lcsRegress(int[,] b,int i, int j)
        {
            //（1）判断递归结束条件
            if (i == 0 || j == 0)
                return;
            //（2）找到相同子序列
            if (b[i, j] == 1)  
            {
                lcsRegress(b, i - 1, j - 1);
                lcs lcs = new lcs();
                lcs.lineNum = i - 1;
                lcs.rowNum = j - 1;
                this.lcsList.Add(lcs);
            }
            //（3）未找到相同子序列，继续递归遍历
            else if (b[i, j] == 2)
                lcsRegress(b, i - 1, j);
            else
                lcsRegress(b, i, j - 1);
        }

        //======================================================================
        //函数名称：isEqual
        //函数返回：true：两个byte数组相同；false：两个byte数组不同
        //参数说明：a：byte数组1；b：byte数组2
        //功能概要：比较byte数组a和b，相同返回true，否则返回false
        //======================================================================
        bool isEqual(byte[] a, byte[] b)
        {
            //（1）变量声明
            bool rv;
            int i;

            rv = true;//返回值默认为真
            //（2）若两个byte数组长度不一样，则不相同
            if (a.Length != b.Length)
            {
                rv = false;
                goto isEqual_EXIT;
            }
            //（3）判断byte数组各位字节数据是否一致
            for (i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    rv = false;
                    goto isEqual_EXIT;
                }
            }
        //（4）至此，两个byte数组相同
        isEqual_EXIT:
            return rv;
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

        //======================================================================
        //函数名称：bytesToStruct
        //函数返回：byte数组转换为对应的结构体
        //参数说明：bytes:字节数组;type:结构体类型
        //功能概要：将byte字节数组数据转换为对应的结构体数据
        //======================================================================
        private object bytesToStruct(byte[] bytes, Type type)
        {
            //（1）变量声明
            int size;
            object obj;
            IntPtr structPtr;

            size = Marshal.SizeOf(type);
            //（2）判断字节长度
            if (size > bytes.Length) return null;
            //（3）分配结构体内存空间
            structPtr = Marshal.AllocHGlobal(size);
            //（4）将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //（5）将内存空间转换为目标结构体
            obj = Marshal.PtrToStructure(structPtr, type);
            //（6）释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }
        //----------------------------------------------------------------------------
   }
}

