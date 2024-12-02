//2020.11.22 刘鑫 1.增加了温度图形化和播报的功能，
//                要有关键字‘芯片内部温度'和‘热敏电阻温度’字样的标识
//                温度格式要是:(英文冒号)，和'℃'（输入法输入wendu后可以找到符号）中间加温度值来表示。
//                例："芯片内部温度为:37℃"，"热敏电阻温度为:38℃"。
//                2. 增加了彩灯的图形化，Printf格式为：【颜色】，例如【红色】。
程序执行简明流程：

【1】在Program.cs文件中启动运行FormSCI.cs
     应用程序的主入口点为Main()：
         Application.Run(new FrmSCI());

【2】在FormSCI.cs中运行构造函数，进行窗体初始化
         InitializeComponent();
     Load为用户加载窗体时发生的事件，窗体的Load事件处理函数已经设置为FrmSCI_Load
         this.Load += new System.EventHandler(this.FrmSCI_Load);

【3】开始运行窗体的Load事件处理函数FrmSCI_Load
     3.1 使[串口选择框]处于可用状态：
            this.CbSCIComNum.Enabled = true;
        自动搜索串口,并将其加入到[串口选择框]中：
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.CbSCIComNum.Items.Add(SCIPorts[i]);
        初始串口号为搜索到的第一个串口
            this.CbSCIComNum.SelectedIndex = 0;
     3.2 设置其他初始显示值：开关标签初始化为"打开串口"，开关为可用状态                      
                  初始波特率选择第一项：115200（115200、38400、19200、9600），波特率选择框可用状态
                  初始选择发送方式选择第一项：字符串（字符串、十进制、十六进制），选择发送方式框可用状态

【4】 点击开关"打开串口"或"关闭串口"
         BtnSCISwitch_Click()
     4.1 打开串口
            sci.SCIInit(SCIPort, PublicVar.g_SCIComNum, PublicVar.g_SCIBaudRate);
            调用串口功能实现类，在SCI.cs中SCIInit函数进行串口初始化和打开
     4.2 关闭串口
            sci.SCIClose(this.SCIPort);
            调用串口功能实现类，在SCI.cs中SCIClose函数关闭串口

【5】打开串口后接收数据 
     串口“DataReceived”事件（中断）的触发条件设置为输入缓冲区中的字节数为整型，即发送过来的数据长度不为0
         SCIReceInt()
     串口每次接收到数据将会触发DataReceived事件（中断），DataReceived事件处理函数已经设置为SCIPort_DataReceived
         SCIPort_DataReceived()
     5.1 调用串口接收函数
           sci.SCIReceiveData(SCIPort, ref PublicVar.g_ReceiveByteArray);
           调用串口功能实现类，在SCI.cs中SCIReceiveData函数实现串口接收缓冲区数据，放入数组g_ReceiveByteArray
     5.2 字符串形式显示
          对于字符串形式,考虑到可能有汉字,直接调用系统定义的函数,处理整个字符串
              str = Encoding.Default.GetString(PublicVar.g_ReceiveByteArray); 
          显示字符串接收内容 
              SCIUpdateRevtxtbox(TbShowString, str); 
              由于数据接收事件与UI主程序在不同线程中,所以在将接收到的数据显示在form中的textbox中时需要使用invoke方法
              委托调用来实现跨线程的通信,这一步由SCIUpdateRevtxtbox()函数实现
     5.3 十/十六进制形式显示（按字节进行处理）
              SCIUpdateRevtxtbox(TbShowDec, PublicVar.g_ReceiveByteArray[i].ToString("D3") + "  ");
              SCIUpdateRevtxtbox(TbShowHex, PublicVar.g_ReceiveByteArray[i].ToString("X2") + "  ");


【6】点击"发送数据"按钮"让串口发送数据
         BtnSCISend_Click()
     6.1 限制发送框内输入数据的格式，TbSCISend框的KeyPress事件处理函数已经设置为TbSCISend_KeyPress()
            TbSCISend_KeyPress()
            字符串发送
            十进制发送,以逗号（英文输入法下）隔开,数据范围0-255,允许用退格键
            十六进制发送，以逗号（英文输入法下）隔开,数据范围00-FF,允许用退格键
           6.1.1 判断发送方式
            SendType = CbSCISendType.SelectedIndex;
           6.1.2 定义一个自动变换大小的数组存储数据
            System.Collections.ArrayList SendData = new System.Collections.ArrayList();
     6.2 选择字符串方式发送
          将要发送的数据进行编码,并获取编码后的数据长度
              len =System.Text.Encoding.Default.GetBytes(this.TbSCISend.Text).Length;
          动态分配len字节单元内容用来存放发送数据
              PublicVar.g_SendByteArray = new byte[len];
          获取TbSCISend文本的码值
              PublicVar.g_SendByteArray =System.Text.Encoding.Default.GetBytes(this.TbSCISend.Text); 
     6.3 选择十进制或十六进制方式发送
          将文本框中的数转化为十/十六进制存入ArrayList类的实例对象SendData
              SendData.Add(Convert.ToByte(str, 10));
              SendData.Add(Convert.ToByte(str, 16));
          动态分配空间存放发送数据
              PublicVar.g_SendByteArray = new byte[count];
          将已经转化后的数据放入到全局变量g_SendByteArray中
              PublicVar.g_SendByteArray[i] = (byte)SendData[i];
     6.4  发送全局变量g_SendByteArray中的数据
            sci.SCISendData(this.SCIPort, ref PublicVar.g_SendByteArray);
            调用串口功能实现类，在SCI.cs中SCISendData函数实现串口发送数据


工程框架：
    该工程有01-Doc、02-Form、03-Function、04-Control、05-Image、06-DataBase、以及Properties、app.config几
个文件夹。这里主要说明各个文件夹所放的内容，方便用户更快捷地了解该工程。
  （1）01-Doc，存放该工程的说明文档等，具体的参阅说明文档。
  （2）02-Form文件夹，放入工程用到的一系列设计界面框。
  （3）03-Function文件夹，放入功能函数。主要代码文档有Program.cs（表明程序入口）、PublicVar.cs（存放全局变量）、
       SCI.cs（串口通信模块用到的函数）。
  （4）04-Control文件夹，可以定义用户定义的控件。
  （5）05-Image文件夹，用于存储工程所用到的图片。右击06-Image，添加现有项。选择所有文件，添加
       需要的图片。接下来右击项目名，选择属性、资源，添加现有文件，导入图片，这时会看到导入进来的图片。
  （6）06-DataBase文件夹，适用于c#+ACCESS操作数据库，用于存放数据库的相关代码。
  （7）Properties文件夹，定义你程序集的属性 ，有 AssemblyInfo.cs、Resources.resx、
       Settings.settings文件，用于保存程序集的信息，如名称，版本等，这些信息一般与项目
       属性面板中的数据对应，不需要手动编写。
  （8）app.config文件，app.config是用户自定义配置文件，能够比较灵活修改一些配置信息，如数据库连接，
       使得在配置环境更改就OK了。


编程技术要素：

(1)此工程用到了委托的,委托Delegate是VB.net与C#语言一项非常重要的特性,它相当于
   C或C++语言中的指针并且是类型安全的.它主要应用于回调函数和事件处理,也用于异
   步处理中,它是一种参考类型,由基类System.Delegate派生。
(2)在界面上放置了一个状态条,显示执行过程,当运行出现错误时,便于调试。
(3)每行代码不超过78列,便于打印代码。
(4)为了区分变量,所有的全局变量都加"g_"前缀。
(5)由于串口的ReceivedBytesThreshold值只能设为正数,在SCI模块我们定义
   了SCIReceInt过程,以便于用户选择设置接收多少个字节时触发接收中断。
(6)对象SCIPort的DataReceived事件是在产生接受中断的时候调用。
(7)在发送框中输入数据,当输入逗号时要在英文输入法下输入。
(8)C#语言,在类SCI中的SCIReceiveData函数的时候,传递的数组名前需加ref设置为引用
   ,不然无法改变原数组的值。
(9)对于处理发送字符串中的汉字问题,在接收字符串时,直接用编码的方式获取整个的字
   符串,然后再利用委托在文本框中显示,而不是如VB.NET所示一个一个字节处理。
   编码字符串使用System.Text.Encoding.Default.GetString(byte[] bytes),其作用
   是将字节序列byte[]按指定编码转换为字符串,其中Default表示选择系统当前ANSI代
   码页的编码来编码,若在系统的区域与语言设置中选择的是中文简体,则默认编码为
   GB2312.此字段还可设为ASCII或Unicode等,具体信息可查看MSDN中Encoding类的属性
   说明。与GetString(byte[] bytes)方法相对应还有GetBytes(string strings)方法,
   其作用与前者相反,使用方法类似。
(10)SerialPort控件自带三个事件:DataReceived,ErrorReceived和PinChanged,可在控
   件的属性窗口中的事件栏(闪电型按钮)中查看.在事件右侧的属性值栏中键入事件处
   理函数的名称后C#会在Form.Designer.cs中自动生成对应事件的委托注册代码。
(11)关于本工程处理串口数据接收的流程大致如下:首先,在选择好串口通信端口及波特
   率,按下打开串口按钮后.在发送文本框中输入数据，单击“发送数据”后。先把串
   口“DataReceived”事件的触发条件设置为发送数据的长度。当接受缓冲区达到触发
   条件的时候，触发SCIPort_DataReceived事件;接着,程序在SCIPort_DataReceived事
   件中调用SCIReceiveData函数,将串口输入缓冲区中的数据取出并进行处理;最后,由
   于数据接收事件与UI主程序在不同线程中,所以在将接收到的数据显示在form中的
   textbox中时需要使用invoke方法委托调用来实现跨线程的通信,这一步由SCIUpdateRevtxtbox
   函数实现。

