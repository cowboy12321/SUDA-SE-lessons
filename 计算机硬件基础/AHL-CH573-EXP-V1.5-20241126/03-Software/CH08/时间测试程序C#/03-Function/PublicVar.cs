namespace SerialPort
{
    /// --------------------------------------------------------------------
    /// <summary>                                                           
    /// �๦��:����ȫ�ֱ���                                                 
    /// </summary>                                                          
    /// <remarks></remarks>                                                 
    /// ------------------- ------------------------------------------------
    public partial class PublicVar
    {
        //���崮�ڵ�ȫ�ֱ���,���óɾ�̬��
        public static byte[] g_ReceiveByteArray;//ȫ�ֱ�������Ž��յ�����
        public static byte[] g_SendByteArray;   //ȫ�ֱ��������Ҫ���͵�����
        public static byte[] g_SendByteLast;    //ȫ�ֱ����������������
        public static string g_SCIComNum;       //ȫ�ֱ��������ѡ��Ĵ��ں�
        public static int g_SCIBaudRate;        //ȫ�ֱ��������ѡ��Ĳ�����
    }
}                                                     