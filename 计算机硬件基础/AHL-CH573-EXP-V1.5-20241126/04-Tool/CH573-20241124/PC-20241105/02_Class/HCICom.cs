using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;


/*
【不动】
 在更改项目时，本文件中的代码不需要做修改
 */

//串口操作类主要针对iot应用开发，故放在cn.edu.suda.sumcu.iot空间下的util子空间中
namespace cn.edu.suda.sumcu.iot.util
{
    public class HCICom
    {
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：HCICom
        ///  功能概要：构造函数
        ///  内部调用：无
        ///  </summary> 
        /// -------------------------------------------------------------------
        public HCICom()
        {
            this.HCIComTarget = "";
            this.data = new Queue<DATA>();
            this.recvCount = 0;
            this.listenLocal = false;//默认不监听本地
        }

        /// -------------------------------------------------------------------
        /// （1）对外属性
        /// -------------------------------------------------------------------
        /// （1.1）连接方式和连接的目标地址
        /// 例如：监听本地的8035端口时         "local:8035"
        ///监听122.112.137.285的8035号端口时   "connect:122.112.137.285:8035"
        public string HCIComTarget;
        ///（1.2）存储接收到的数据帧数,设置为只读属性
        public long recvCount { get; private set; }

        /// -------------------------------------------------------------------
        /// （2）对外方法
        /// -------------------------------------------------------------------
        /// （2.1）init--------------------------------------------------------
        ///  <summary> 
        ///  方法名称：init
        ///  功能概要：初始化HCICom，载入属性值，并根据传入的IMSI建立通信连接
        ///  内部调用：BeginAccept BeginConnect
        ///  </summary> 
        ///  <param name="IMSI">存储IMSI号的string数组</param>
        ///  <returns>0：初始化成功；1：不支持此种通信方式 ；2:目标地址错误
        ///           3：监听目标地址失败；4：连接服务器失败；
        ///  </returns>
        /// -------------------------------------------------------------------
        public int Init(string[] IMSI)
        {
            //（1） 定义本函数使用的变量
            string address = "";    //存储目标地址
            string ip = "";         //存储IP地址
            string port = "";       //存储端口号
            IPEndPoint endPoint = null; //临时变量
            Socket socket;              //临时变量
            StateObject state =    //存储需要传递的socket信息和缓冲区
                  new StateObject();
            int retVal;

            //（2）从属性HCIComTarget中提取出目标地址和通信方式
            try
            {
                if (this.HCIComTarget.Contains("local:"))//若目标地址含"local:"
                {
                    address =                           //获得目标地址
                        this.HCIComTarget.Remove(0, 6);
                    this.listenLocal = true;//标记listenLocal为监听本地端口
                    ip = "0.0.0.0";
                    port = address;
                }
                else if (Regex.Matches(this.HCIComTarget, "[a-zA-Z]").Count == 0
                      && this.HCIComTarget.Split('.').Length - 1 == 3
                      && this.HCIComTarget.Split(':').Length - 1 == 1
                )      //若目标地址不含有字母，且有一个':'和3个'.'
                {
                    address = this.HCIComTarget;  //获得目标地址
                    this.listenLocal = false;//标记listenLocal为不监听本地端口
                    string[] tmp = address.Split(':');
                    ip = tmp[0];
                    port = tmp[1];  //解析出address中的IP和端口号    
                }
                else
                {
                    retVal = 1;           //不支持此种通信方式
                    goto Init_exit;
                }
            }
            catch
            {
                retVal = 2;           //目标地址错误
                goto Init_exit;
            }
            //（3）按照指定的方式建立通讯，完成初始化
            try
            {
                //（3.1.1）新建套接字
                socket = new Socket(AddressFamily.InterNetwork,
                     SocketType.Stream, ProtocolType.Tcp);
                //（3.1.2）设置监听的端口号
                endPoint = new IPEndPoint
                    (IPAddress.Parse(ip), Convert.ToInt32(port));
                //（3.1.3）根据通信方式建立连接
                if (listenLocal == true)
                {
                    socket.Bind(endPoint); //将套接字绑定到IP和端口上
                    socket.Listen(1024);   //设置监听队列的长度（等待连接的最大数目，不包括已经连接的数目）
                    //开始异步监听(一旦监听到，则触发AcceptCallBack方法)
                    socket.BeginAccept(new AsyncCallback(AcceptCallBack), socket);
                }
                else
                {
                    this.connect_socket = socket;
                    this.connect_socket.Connect(endPoint);         //建立连接
                    state.workSocket =     //将客户端的socket赋值给state的成员
                         this.connect_socket;
                    //调用开始接收的回调函数BeginReceive，并将缓冲区state.buffer、
                    //缓冲区大小StateObject.BufferSize和接收到数据后应调用的函数
                    //ReceiveCallBack传入回调函数BeginReceive
                    this.connect_socket.BeginReceive
                        (state.buffer, 0, StateObject.BufferSize,
                        SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                    foreach (string one_imsi in IMSI)
                    {
                        //发初始化包
                        this.Send(one_imsi, Encoding.Default.GetBytes("$init$"));
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            {
                retVal = 3;           //建立通信失败
                goto Init_exit;
            }
            retVal = 0;
        Init_exit:
            return retVal;
        }

        /// （2.2）Read--------------------------------------------------------
        ///  <summary> 
        ///  方法名称：Read
        ///  功能概要：读出缓冲区中的一帧数据，并通过传入参数传出。
        ///            建议在接收事件中调用
        ///  内部调用：无
        ///  </summary> 
        ///  <param name="imsi">发送本条数据的终端IMSI号</param>
        ///  <param name="buffer">本条数据的内容</param>
        ///  <returns>true：读取成功；false：接收缓冲区为空</returns>
        /// -------------------------------------------------------------------
        public bool Read(ref string imsi, ref byte[] buffer)
        {
            //（1） 定义本函数使用的变量
            DATA retData;                 //存储从队列中取出的数据
            //（2）读取数据
            if (this.data.Count == 0)     //若接收数据缓冲队列为空
            {
                recvCount = this.data.Count;
                return false;
            }
            retData = this.data.Dequeue();//从缓存队列取出数据并存入retData
            recvCount = this.data.Count;  //更新接收数据的长度
            imsi = retData.imsi;          //返回接收到的imsi号
            buffer = retData.data;        //返回接收到的数据
            return true;
        }

        ///（2.3）Send---------------------------------------------------------
        ///  <summary> 
        ///  方法名称：Send
        ///  功能概要：发送操作
        ///  内部调用：内部函数 SendCallBack
        ///  </summary> 
        ///  <param name="imsi">本条数据将要发送至的终端IMSI号</param>
        ///  <param name="data">待发送的数据内容</param>
        ///  <returns>0：发送成功；1：发送失败 ；
        ///  </returns>
        /// -------------------------------------------------------------------
        public int Send(string imsi, byte[] data)
        {
            //（1） 定义本函数使用的变量
            byte[] frame;
            List<Socket> socket = null;
            int i;
            int retVal;
            //RC4编码
            //            rc4_state rc4_state = new rc4_state();
            //rc4_init(rc4_state, _key, _key.Length);
            //rc4_crypt(rc4_state, data, buf, buf.Length);
            try
            {
                //（2） 调用函数frameEncode组帧
                frame = frameEncode(imsi, data, data.Length);
                //（3）将数据发送出去
                if (listenLocal == false)     //若为连接远程服务器
                {
                    //（3.1）直接使用存储的connect_socket发送数据
                    connect_socket.Send(frame, SocketFlags.None);
                }
                else                         //若为监听本地端口
                {
                    //（3.2）从imsiSocket表中找到待发送IMSI对应的socket
                    for (i = 0; i < imsiSocket.Count; i++)
                    {
                        if (imsiSocket[i].imsi == imsi)
                        {
                            socket = imsiSocket[i].socket;
                            break;
                        }
                    }
                    //通过寻找到的socket发送数据
                    for (i = 0; i < socket.Count; i++)
                    {
                        try
                        {
                            socket[i].Send(frame, SocketFlags.None);
                            string str = ByteToString(frame);
                        }
                        catch     //发送失败
                        {
                            socket[i].Close();
                            socket.Remove(socket[i]); //移除此socket
                        }
                    }
                    if (socket.Count == 0)   //全部发送失败
                    {
                        retVal = 1;
                        goto Send_exit;
                    }
                }
            }
            catch
            {
                retVal = 1;
                goto Send_exit;
            }
            retVal = 0;
        Send_exit:
            Thread.Sleep(2);    //防止连续发送出错，延时2毫秒
            return retVal;
        }

        
        public static string ByteToString(byte[] InBytes)
        {
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2} ",InByte);
            }
            return StringOut;
        } 
        /// -------------------------------------------------------------------
        /// （3）对外事件
        /// -------------------------------------------------------------------

        ///（3.1）outputMessage------------------------------------------------
        ///  <summary> 
        ///  方法名称：OutputError
        ///  功能概要：错误输出委托函数
        ///  内部调用：无
        ///  </summary> 
        ///  <param name="type">输出信息类型，可取以下值
        ///            1：监听本机失败        2：连接服务器失败
        ///            3：与客户端断开连接    4：与服务器断开连接
        ///            5：数据接收错误        6：与服务器建立连接成功。
        ///            （message为IMSI号）
        ///            </param>
        ///  <param name="message">输出的具体信息</param>
        /// -------------------------------------------------------------------
        public delegate void OutputError(int type, string message);
        public event OutputError OutputErrorEvent;         //信息输出事件    

        ///（3.2）recv---------------------------------------------------------
        ///  <summary> 
        ///  方法名称：DataReceived
        ///  功能概要：数据接收委托函数
        ///  内部调用：无
        ///  </summary> 
        /// -------------------------------------------------------------------
        public delegate void DataReceived();
        public event DataReceived DataReceivedEvent;             //数据接收事件   



        /// -------------------------------------------------------------------
        /// 以下为私有的属性和方法
        /// -------------------------------------------------------------------
        //（1） 属性
        private bool listenLocal;           //标志是否为监听本地
        private Socket connect_socket;      //存储连接时的socket
        //存储imsi和socket对应表
        private List<imsi_socket> imsiSocket = new List<imsi_socket>();
        private byte[] _key = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
        private Queue<DATA> data;       //接收数据缓冲队列，存储接收到的数据


        //（2） 方法
        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：AcceptCallBack
        ///  功能概要：监听回调操作
        ///  内部调用：内部函数 ReceiveCallBack、AcceptCallBack
        ///            事件     OutputErrorEvent
        ///  <param name="ar">异步操作状态</param>
        ///  </summary> 
        /// -------------------------------------------------------------------
        private void AcceptCallBack(IAsyncResult ar)
        {
            //（1）定义本函数使用到的变量
            //存储开启异步接收需要传递的socket信息和缓冲区
            StateObject state = new StateObject();
            //获得用来监听的socket
            Socket server = ar.AsyncState as Socket;
            //结束本次监听，并获得连接到本机的客户端的socket
            Socket client = server.EndAccept(ar);
            //将客户端的socket赋值给state的成员
            state.workSocket = client;
            try
            {
                //（2）调用开始接收的回调函数BeginReceive，并将缓冲区state.buffer、
                //缓冲区大小StateObject.BufferSize和接收到数据后应调用的函数
                //ReceiveCallBack传入回调函数BeginReceive
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize,
                    SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                //（3）调用开始监听的回调函数BeginAccept，继续监听
                server.BeginAccept(new AsyncCallback(AcceptCallBack), server);
            }
            catch (Exception e)
            {
                //输出错误信息
                if (OutputErrorEvent != null) OutputErrorEvent(1,
                    "【错误】监听发生错误： " + e.Message + "\r\n");
            }
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：ReceiveCallBack
        ///  功能概要：接收回调操作
        ///  内部调用：内部函数 ReceiveCallBack
        ///            事件     DataReceivedEvent、OutputErrorEvent
        ///  <param name="ar">异步操作状态</param>
        ///  </summary> 
        /// -------------------------------------------------------------------
        private void ReceiveCallBack(IAsyncResult ar)
        {
            //（1）定义本函数使用的变量
            int length = 0;       //保存接收到的数据字节数         
            //获取回调本函数的socket信息
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            imsi_socket iSocket;
            //            rc4_state rc4_state = new rc4_state();  
            byte[] dataCut = null;
            byte[] data = null;
            string imsi = "";
            DATA tData;
            int i, j;

            //（2）结束本次接收，并获取接收到的数据长度
            try
            {
                length = client.EndReceive(ar);    //获取接收到的数据长度
            }
            catch
            {
                if (client == null) return;  //socket为空，结束接收并返回
                client.Close();
                //若为监听本地，且已经注册错误输出事件
                if (listenLocal == true && OutputErrorEvent != null)
                {
                    for (i = 0; i < imsiSocket.Count; i++)
                    {
                        for (j = 0; j < imsiSocket[i].socket.Count; j++)
                        {
                            if (imsiSocket[i].socket[j] == client)
                            {
                                if (OutputErrorEvent != null)
                                    OutputErrorEvent(3, "【监听】与客户端" +
                                        imsiSocket[i].imsi + "断开连接\r\n");
                                return;
                            }
                        }
                    }
                }
                //若为连接服务器，且已经注册错误输出事件
                else if (listenLocal == false && OutputErrorEvent != null)
                {
                    if (OutputErrorEvent != null) OutputErrorEvent(4,
                        "【连接】与服务器" + client.RemoteEndPoint.ToString()
                        + "断开连接\r\n");
                }
            }
            try
            {
                //（3）保存接收到的数据到dataCut中
                if (length > 0)                //若接收到了数据
                {
                    dataCut = new byte[length];//拷贝缓冲区中的数据到datacut
                    Array.Copy(state.buffer, 0, dataCut, 0, length);
                    //RC4解码
                    //rc4_init(rc4_state, _key, _key.Length);
                    //rc4_crypt(rc4_state, state.buffer, dataCut, length);

                    //（4）将dataCut中的数据解帧并放入data中
                    if (!frameDecode(dataCut, ref data, ref imsi))//若解帧失败
                        goto ReceiveCallBack_exit;//退出本函数
                    //（5）更新或添加已经保存的socket
                    if (listenLocal == true)   //若监听本机端口
                    {
                        for (i = 0; i < imsiSocket.Count; i++)
                        {
                            if (imsiSocket[i].imsi == imsi && !imsiSocket[i].socket.Contains(client))
                            {
                                //添加本imsi的新socket
                                imsiSocket[i].socket.Add(client);
                                break;
                            }
                        }
                        if (i == imsiSocket.Count)//若imsi和socket表中无此imsi
                        {
                            //实例化socket列表
                            iSocket.socket = new List<Socket>();
                            //添加本imsi和最新的socket
                            iSocket.socket.Add(client);
                            iSocket.imsi = imsi;
                            imsiSocket.Add(iSocket);//存入imsi和socket对应表中
                        }
                    }
                    else               //若连接服务器的端口，直接保存本socket
                    {
                        this.connect_socket = client;
                    }
                    if (Encoding.Default.GetString(data) == "$init$")//若为初始化
                    {
                        if (OutputErrorEvent != null) OutputErrorEvent(6,
                            imsi);
                        goto ReceiveCallBack_exit; //直接跳出
                    }
                    //（6）将imsi号和对应的数据存入接收数据缓冲区
                    tData.imsi = imsi;
                    //                    tData.data = new byte[data.Length];
                    //                    Array.Copy(data,tData.data,data.Length);
                    tData.data = data;
                    this.data.Enqueue(tData);
                    recvCount = this.data.Count;
                    //（7）触发数据接收事件
                    if (DataReceivedEvent != null) DataReceivedEvent();
                ReceiveCallBack_exit:
                    //（8）真正退出之前重新开始异步接收
                    state.buffer = new byte[StateObject.BufferSize];
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize,
                    SocketFlags.None, new AsyncCallback(ReceiveCallBack), state);
                }
            }
            catch (Exception e)
            {
                //输出错误信息
                if (OutputErrorEvent != null) OutputErrorEvent(5,
                     "【错误】接收数据发生错误 " + e.Message + "\r\n");
            }
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：Swap
        ///  功能概要：字节的数据交换
        ///  <param name="a">需要交换的数据</param>
        ///  <param name="b">需要交换的数据</param>
        ///  </summary> 
        /// -------------------------------------------------------------------
        private void Swap(ref byte a, ref byte b)
        {
            byte tmp;
            tmp = a;
            a = b;
            b = a;
        }

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：rc4_init
        ///  功能概要：RC4加密初始化
        ///  内部调用：内部函数 Swap
        ///  <param name="statea">rc4数据</param>
        ///  <param name="key">密钥</param>
        ///  <param name="keylen">密钥长度</param>
        ///  </summary> 
        /// -------------------------------------------------------------------
        //private void rc4_init(rc4_state state, byte[] key, int keylen)
        //{
        //    //（1）定义本函数使用的变量
        //    int i;
        //    byte j;
        //    //（2）加密的参数赋初值
        //    for (i = 0; i < 256; i++)
        //    {
        //        state.perm[i] = (byte)i;
        //    }
        //    state.index1 = 0;
        //    state.index2 = 0;
        //    //（3）根据密钥和密钥长度对加密的参数进行修改
        //    for (i = 0, j = 0; i < 256; i++)
        //    {
        //        j += (byte)(state.perm[i] + key[i % keylen]);
        //        Swap(ref state.perm[i], ref state.perm[j]);
        //    }
        //}

        /// -------------------------------------------------------------------
        ///  <summary> 
        ///  方法名称：rc4_crypt
        ///  功能概要：RC4加密、解密
        ///  内部调用：内部函数 Swap
        ///  <param name="statea">rc4数据</param>
        ///  <param name="input">输入数据</param>
        ///  <param name="output">输出数据</param>
        ///  <param name="buflen">数据长度</param>
        ///  </summary> 
        /// -------------------------------------------------------------------
        //private void rc4_crypt(rc4_state state, byte[] input, byte[] output, int buflen)
        //{
        //    //（1）定义本函数使用的变量
        //    int i;
        //    byte j;
        //    //（2）对数据进行加密或解密
        //    for (i = 0; i < buflen; i++)
        //    {
        //        state.index1++;
        //        state.index2 += state.perm[state.index1];

        //        Swap(ref state.perm[state.index1], ref state.perm[state.index2]);

        //        j = (byte)(state.perm[state.index1] + state.perm[state.index2]);
        //        output[i] = (byte)(input[i] ^ state.perm[j]);
        //    }
        //}

        /// -------------------------------------------------------------------
        /// <summary>【不动】
        /// 函数名称：frameDecode
        /// 功能概要：从接收到的帧frame中，解出数据和imsi，分别存到data和imsi中
        /// 说    明：
        /// </summary>
        /// <param name="frame">接收到的数据帧</param>
        /// <param name="data">解析出的帧中数据部分</param>
        /// <param name="imsi">帧中包含的设备IMSI号</param>
        /// <returns>=true 解帧成功，=false 解帧失败 </returns>
        /// -------------------------------------------------------------------
        private bool frameDecode(byte[] frame, ref byte[] data, ref string imsi)
        {
            //（1）定义本函数使用的变量
            bool ans = false;
            byte[] dataLength;        //数据长度.
            byte temp;                //临时变量.
            byte[] crcData;           //CRC校验数据.
            byte[] crcResult;         //CRC生成结果.
            byte[] imsiByte;          //IMSI数据.
            data = null;              //数据.
            imsi = "";
            int length = frame.Length;
            //（2）判断帧头帧尾是否正确
            //判断帧头是否正确.
            if (frame[0] != System.Text.Encoding.UTF8.GetBytes("V")[0] ||
                frame[1] != System.Text.Encoding.UTF8.GetBytes("!")[0]) goto frameDecode_End;
            //判断帧尾是否正确.
            if (frame[length - 2] != System.Text.Encoding.UTF8.GetBytes("S")[0] ||
                frame[length - 1] != System.Text.Encoding.UTF8.GetBytes("$")[0]) goto frameDecode_End;

            //（3）对接收帧中除帧头帧尾以及校验以外的数据进行CRC校验.
            crcData = new byte[length - 6];
            Array.Copy(frame, 2, crcData, 0, length - 6);
            crcResult = CRC16(crcData);
            if (crcResult[0] != frame[length - 4] || crcResult[1] != frame[length - 3])
                goto frameDecode_End;
            //（4）解析出发送设备的IMSI.
            imsiByte = new byte[15];
            Array.Copy(frame, 2, imsiByte, 0, 15);
            imsi = System.Text.Encoding.Default.GetString(imsiByte);
            //（5）解析出数据长度.
            dataLength = new byte[2];
            Array.Copy(frame, 17, dataLength, 0, 2);
            temp = dataLength[0];
            dataLength[0] = dataLength[1];
            dataLength[1] = temp;
            if (length - 23 != BitConverter.ToUInt16(dataLength, 0)) goto frameDecode_End;
            //（6）解析出数据.
            data = new byte[length - 23];
            Array.Copy(frame, 19, data, 0, length - 23);
            ans = true;
        frameDecode_End:
            return ans;
        }
        /// -------------------------------------------------------------------
        /// <summary>【不动】
        /// 函数名称：frameEncode
        /// 功能概要：把输入的数据data和imsi，组成帧
        /// 说    明：
        /// </summary>
        /// <param name="data">帧中数据部分</param>
        ///  <param name="imsi">帧中包含的设备IMSI号</param>
        /// <returns>完整的一帧</returns>
        /// -------------------------------------------------------------------
        private byte[] frameEncode(string imsi, byte[] data, int length)
        {
            //（1）定义本函数使用的变量
            byte[] imsiByte;            //IMSI.
            byte[] dataLength;          //数据长度.
            byte[] crcData;             //CRC校验数据.
            byte[] crcResult;           //CRC生成结果.
            byte[] frame = new byte[length + 23];
            //（2）加入帧头和帧尾
            //帧头.
            frame[0] = System.Text.Encoding.UTF8.GetBytes("V")[0];
            frame[1] = System.Text.Encoding.UTF8.GetBytes("!")[0];
            //帧尾.
            frame[length + 21] = System.Text.Encoding.UTF8.GetBytes("S")[0];
            frame[length + 22] = System.Text.Encoding.UTF8.GetBytes("$")[0];
            //（3）加入IMSI.
            imsiByte = System.Text.Encoding.Default.GetBytes(imsi);
            Array.Copy(imsiByte, 0, frame, 2, 15);
            //（4）加入帧长.
            dataLength = new byte[2];
            dataLength = BitConverter.GetBytes((ushort)length);
            frame[17] = dataLength[1];
            frame[18] = dataLength[0];
            //（5）加入数据.
            Array.Copy(data, 0, frame, 19, length);
            //（6）加入CRC校验结果.
            crcData = new byte[length + 17];
            Array.Copy(frame, 2, crcData, 0, length + 17);
            crcResult = CRC16(crcData);
            frame[length + 19] = crcResult[0];
            frame[length + 20] = crcResult[1];
            //（7）返回组帧完成后的数据
            return frame;
        }
        /// -------------------------------------------------------------------
        /// <summary>【不动】
        /// 函数名称：CRC16
        /// 功能概要：计算输入数据的CRC校验码
        /// 说    明：
        /// </summary>
        /// <param name="data">待校验数据</param>
        /// <returns>16位的校验码</returns>
        /// -------------------------------------------------------------------
        private byte[] CRC16(byte[] data)
        {
            //（1）定义本函数使用的变量
            int len = data.Length;
            ushort crc;
            int i, j;
            byte hi;           //高位置.
            byte lo;           //低位置.
            if (len > 0)
            {
                //产生data的CRC校验码
                crc = 0xFFFF;
                for (i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                hi = (byte)((crc & 0xFF00) >> 8);  //高位置.
                lo = (byte)(crc & 0x00FF);         //低位置.
                return new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }

        //（3） 内部使用的类和结构体
        class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
        }

        //class rc4_state
        //{
        //    public byte[] perm = new byte[256];
        //    public byte index1;
        //    public byte index2;
        //}
        private struct imsi_socket
        {
            public string imsi;
            public List<Socket> socket;
        }
        private struct DATA
        {
            public string imsi;
            public byte[] data;
        }
    }

}
