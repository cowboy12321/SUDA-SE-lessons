//2020.11.22 ���� 1.�������¶�ͼ�λ��Ͳ����Ĺ��ܣ�
//                Ҫ�йؼ��֡�оƬ�ڲ��¶�'�͡����������¶ȡ������ı�ʶ
//                �¶ȸ�ʽҪ��:(Ӣ��ð��)����'��'�����뷨����wendu������ҵ����ţ��м���¶�ֵ����ʾ��
//                ����"оƬ�ڲ��¶�Ϊ:37��"��"���������¶�Ϊ:38��"��
//                2. �����˲ʵƵ�ͼ�λ���Printf��ʽΪ������ɫ�������硾��ɫ����
����ִ�м������̣�

��1����Program.cs�ļ�����������FormSCI.cs
     Ӧ�ó��������ڵ�ΪMain()��
         Application.Run(new FrmSCI());

��2����FormSCI.cs�����й��캯�������д����ʼ��
         InitializeComponent();
     LoadΪ�û����ش���ʱ�������¼��������Load�¼��������Ѿ�����ΪFrmSCI_Load
         this.Load += new System.EventHandler(this.FrmSCI_Load);

��3����ʼ���д����Load�¼�������FrmSCI_Load
     3.1 ʹ[����ѡ���]���ڿ���״̬��
            this.CbSCIComNum.Enabled = true;
        �Զ���������,��������뵽[����ѡ���]�У�
            SCIPorts = System.IO.Ports.SerialPort.GetPortNames();
            this.CbSCIComNum.Items.Add(SCIPorts[i]);
        ��ʼ���ں�Ϊ�������ĵ�һ������
            this.CbSCIComNum.SelectedIndex = 0;
     3.2 ����������ʼ��ʾֵ�����ر�ǩ��ʼ��Ϊ"�򿪴���"������Ϊ����״̬                      
                  ��ʼ������ѡ���һ�115200��115200��38400��19200��9600����������ѡ������״̬
                  ��ʼѡ���ͷ�ʽѡ���һ��ַ������ַ�����ʮ���ơ�ʮ�����ƣ���ѡ���ͷ�ʽ�����״̬

��4�� �������"�򿪴���"��"�رմ���"
         BtnSCISwitch_Click()
     4.1 �򿪴���
            sci.SCIInit(SCIPort, PublicVar.g_SCIComNum, PublicVar.g_SCIBaudRate);
            ���ô��ڹ���ʵ���࣬��SCI.cs��SCIInit�������д��ڳ�ʼ���ʹ�
     4.2 �رմ���
            sci.SCIClose(this.SCIPort);
            ���ô��ڹ���ʵ���࣬��SCI.cs��SCIClose�����رմ���

��5���򿪴��ں�������� 
     ���ڡ�DataReceived���¼����жϣ��Ĵ�����������Ϊ���뻺�����е��ֽ���Ϊ���ͣ������͹��������ݳ��Ȳ�Ϊ0
         SCIReceInt()
     ����ÿ�ν��յ����ݽ��ᴥ��DataReceived�¼����жϣ���DataReceived�¼��������Ѿ�����ΪSCIPort_DataReceived
         SCIPort_DataReceived()
     5.1 ���ô��ڽ��պ���
           sci.SCIReceiveData(SCIPort, ref PublicVar.g_ReceiveByteArray);
           ���ô��ڹ���ʵ���࣬��SCI.cs��SCIReceiveData����ʵ�ִ��ڽ��ջ��������ݣ���������g_ReceiveByteArray
     5.2 �ַ�����ʽ��ʾ
          �����ַ�����ʽ,���ǵ������к���,ֱ�ӵ���ϵͳ����ĺ���,���������ַ���
              str = Encoding.Default.GetString(PublicVar.g_ReceiveByteArray); 
          ��ʾ�ַ����������� 
              SCIUpdateRevtxtbox(TbShowString, str); 
              �������ݽ����¼���UI�������ڲ�ͬ�߳���,�����ڽ����յ���������ʾ��form�е�textbox��ʱ��Ҫʹ��invoke����
              ί�е�����ʵ�ֿ��̵߳�ͨ��,��һ����SCIUpdateRevtxtbox()����ʵ��
     5.3 ʮ/ʮ��������ʽ��ʾ�����ֽڽ��д���
              SCIUpdateRevtxtbox(TbShowDec, PublicVar.g_ReceiveByteArray[i].ToString("D3") + "  ");
              SCIUpdateRevtxtbox(TbShowHex, PublicVar.g_ReceiveByteArray[i].ToString("X2") + "  ");


��6�����"��������"��ť"�ô��ڷ�������
         BtnSCISend_Click()
     6.1 ���Ʒ��Ϳ����������ݵĸ�ʽ��TbSCISend���KeyPress�¼��������Ѿ�����ΪTbSCISend_KeyPress()
            TbSCISend_KeyPress()
            �ַ�������
            ʮ���Ʒ���,�Զ��ţ�Ӣ�����뷨�£�����,���ݷ�Χ0-255,�������˸��
            ʮ�����Ʒ��ͣ��Զ��ţ�Ӣ�����뷨�£�����,���ݷ�Χ00-FF,�������˸��
           6.1.1 �жϷ��ͷ�ʽ
            SendType = CbSCISendType.SelectedIndex;
           6.1.2 ����һ���Զ��任��С������洢����
            System.Collections.ArrayList SendData = new System.Collections.ArrayList();
     6.2 ѡ���ַ�����ʽ����
          ��Ҫ���͵����ݽ��б���,����ȡ���������ݳ���
              len =System.Text.Encoding.Default.GetBytes(this.TbSCISend.Text).Length;
          ��̬����len�ֽڵ�Ԫ����������ŷ�������
              PublicVar.g_SendByteArray = new byte[len];
          ��ȡTbSCISend�ı�����ֵ
              PublicVar.g_SendByteArray =System.Text.Encoding.Default.GetBytes(this.TbSCISend.Text); 
     6.3 ѡ��ʮ���ƻ�ʮ�����Ʒ�ʽ����
          ���ı����е���ת��Ϊʮ/ʮ�����ƴ���ArrayList���ʵ������SendData
              SendData.Add(Convert.ToByte(str, 10));
              SendData.Add(Convert.ToByte(str, 16));
          ��̬����ռ��ŷ�������
              PublicVar.g_SendByteArray = new byte[count];
          ���Ѿ�ת��������ݷ��뵽ȫ�ֱ���g_SendByteArray��
              PublicVar.g_SendByteArray[i] = (byte)SendData[i];
     6.4  ����ȫ�ֱ���g_SendByteArray�е�����
            sci.SCISendData(this.SCIPort, ref PublicVar.g_SendByteArray);
            ���ô��ڹ���ʵ���࣬��SCI.cs��SCISendData����ʵ�ִ��ڷ�������


���̿�ܣ�
    �ù�����01-Doc��02-Form��03-Function��04-Control��05-Image��06-DataBase���Լ�Properties��app.config��
���ļ��С�������Ҫ˵�������ļ������ŵ����ݣ������û�����ݵ��˽�ù��̡�
  ��1��01-Doc����Ÿù��̵�˵���ĵ��ȣ�����Ĳ���˵���ĵ���
  ��2��02-Form�ļ��У����빤���õ���һϵ����ƽ����
  ��3��03-Function�ļ��У����빦�ܺ�������Ҫ�����ĵ���Program.cs������������ڣ���PublicVar.cs�����ȫ�ֱ�������
       SCI.cs������ͨ��ģ���õ��ĺ�������
  ��4��04-Control�ļ��У����Զ����û�����Ŀؼ���
  ��5��05-Image�ļ��У����ڴ洢�������õ���ͼƬ���һ�06-Image����������ѡ�������ļ������
       ��Ҫ��ͼƬ���������һ���Ŀ����ѡ�����ԡ���Դ����������ļ�������ͼƬ����ʱ�ῴ�����������ͼƬ��
  ��6��06-DataBase�ļ��У�������c#+ACCESS�������ݿ⣬���ڴ�����ݿ����ش��롣
  ��7��Properties�ļ��У���������򼯵����� ���� AssemblyInfo.cs��Resources.resx��
       Settings.settings�ļ������ڱ�����򼯵���Ϣ�������ƣ��汾�ȣ���Щ��Ϣһ������Ŀ
       ��������е����ݶ�Ӧ������Ҫ�ֶ���д��
  ��8��app.config�ļ���app.config���û��Զ��������ļ����ܹ��Ƚ�����޸�һЩ������Ϣ�������ݿ����ӣ�
       ʹ�������û������ľ�OK�ˡ�


��̼���Ҫ�أ�

(1)�˹����õ���ί�е�,ί��Delegate��VB.net��C#����һ��ǳ���Ҫ������,���൱��
   C��C++�����е�ָ�벢�������Ͱ�ȫ��.����ҪӦ���ڻص��������¼�����,Ҳ������
   ��������,����һ�ֲο�����,�ɻ���System.Delegate������
(2)�ڽ����Ϸ�����һ��״̬��,��ʾִ�й���,�����г��ִ���ʱ,���ڵ��ԡ�
(3)ÿ�д��벻����78��,���ڴ�ӡ���롣
(4)Ϊ�����ֱ���,���е�ȫ�ֱ�������"g_"ǰ׺��
(5)���ڴ��ڵ�ReceivedBytesThresholdֵֻ����Ϊ����,��SCIģ�����Ƕ���
   ��SCIReceInt����,�Ա����û�ѡ�����ý��ն��ٸ��ֽ�ʱ���������жϡ�
(6)����SCIPort��DataReceived�¼����ڲ��������жϵ�ʱ����á�
(7)�ڷ��Ϳ�����������,�����붺��ʱҪ��Ӣ�����뷨�����롣
(8)C#����,����SCI�е�SCIReceiveData������ʱ��,���ݵ�������ǰ���ref����Ϊ����
   ,��Ȼ�޷��ı�ԭ�����ֵ��
(9)���ڴ������ַ����еĺ�������,�ڽ����ַ���ʱ,ֱ���ñ���ķ�ʽ��ȡ��������
   ����,Ȼ��������ί�����ı�������ʾ,��������VB.NET��ʾһ��һ���ֽڴ���
   �����ַ���ʹ��System.Text.Encoding.Default.GetString(byte[] bytes),������
   �ǽ��ֽ�����byte[]��ָ������ת��Ϊ�ַ���,����Default��ʾѡ��ϵͳ��ǰANSI��
   ��ҳ�ı���������,����ϵͳ������������������ѡ��������ļ���,��Ĭ�ϱ���Ϊ
   GB2312.���ֶλ�����ΪASCII��Unicode��,������Ϣ�ɲ鿴MSDN��Encoding�������
   ˵������GetString(byte[] bytes)�������Ӧ����GetBytes(string strings)����,
   ��������ǰ���෴,ʹ�÷������ơ�
(10)SerialPort�ؼ��Դ������¼�:DataReceived,ErrorReceived��PinChanged,���ڿ�
   �������Դ����е��¼���(�����Ͱ�ť)�в鿴.���¼��Ҳ������ֵ���м����¼���
   ���������ƺ�C#����Form.Designer.cs���Զ����ɶ�Ӧ�¼���ί��ע����롣
(11)���ڱ����̴��������ݽ��յ����̴�������:����,��ѡ��ô���ͨ�Ŷ˿ڼ�����
   ��,���´򿪴��ڰ�ť��.�ڷ����ı������������ݣ��������������ݡ����ȰѴ�
   �ڡ�DataReceived���¼��Ĵ�����������Ϊ�������ݵĳ��ȡ������ܻ������ﵽ����
   ������ʱ�򣬴���SCIPort_DataReceived�¼�;����,������SCIPort_DataReceived��
   ���е���SCIReceiveData����,���������뻺�����е�����ȡ�������д���;���,��
   �����ݽ����¼���UI�������ڲ�ͬ�߳���,�����ڽ����յ���������ʾ��form�е�
   textbox��ʱ��Ҫʹ��invoke����ί�е�����ʵ�ֿ��̵߳�ͨ��,��һ����SCIUpdateRevtxtbox
   ����ʵ�֡�

