namespace AHL_GEC
{
    /// --------------------------------------------------------------------
    /// <summary>                                                           
    /// 类功能:定义全局变量                                                 
    /// </summary>                                                          
    /// <remarks></remarks>                                                 
    /// ------------------- ------------------------------------------------
    public partial class PublicVar
    {
        //定义串口的全局变量,设置成静态的
        public static byte[] g_ReceiveByteArray;//全局变量，存放接收的数据
        public static byte[] g_ReceiveByteArray2;//全局变量，存放接收的数据

        public static byte[] g_SendByteArray;   //全局变量，存放要发送的数据
        public static byte[] g_SendByteLast;    //全局变量，存放最后的数据
        public static string g_SCIComNum;       //全局变量，存放选择的串口号
        public static string g_SCIComNum2;       //全局变量，存放选择的串口号

        public static int g_SCIBaudRate;        //全局变量，存放选择的波特率

        public static System.IO.Ports.SerialPort uartPort;
        public static bool reconnected=false;//软件更新界面是否成功重新连接（没进行重新连接也是false）
        //public static FrmSCI mfrm_SCI;//使软件更新界面能调用串口调试界面的函数
    }
}