using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

///---------------------------------------------------------------------
/// <summary>                                                           
/// ��          :SCI:���ڹ���ʵ����                                     
/// ��   ��   ��:�򿪴��ڡ��رմ��ڼ����ڵĽ��պͷ��͹���               
/// ���к�������:                                                       
///             (1)SCIInit:���ڳ�ʼ��                                   
///             (2)SCISendData:��������                                 
///             (3)SCIReceiveData:��������                              
///             (4)SCIClose:�رմ���                                    
///             (5)SCIReceInt: ���ô��ڽ����ж�����                   
/// ˵        ��:��ģ�������Ĵ�������޹�                             
/// </summary>                                                          
/// <remarks></remarks>                                                 
/// --------------------------------------------------------------------


namespace SerialPort
{

    public class SCI : System.IO.Ports.SerialPort
    {

        /// ----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:���쵱ǰ��Ķ��󣬳�ʼ�����ڳ�Ա����
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// <param name="ComNum">���ں�,�ַ�������</param>                  
        /// <param name="Baud">������,����</param>                          
            
        /// ----------------------------------------------------------------
        //public SCI(string ComNum, Int32 Baud)
        //{
        //    try{
        //        this.Close();               //��֤��ʼ��֮ǰ�ǹرյ�
        //        this.PortName = ComNum;     //���ô��ں�
        //        this.BaudRate = Baud;       //���ò�����
        //        this.Parity = System.IO.Ports.Parity.None;//��������żУ��
        //        this.DataBits = 8;          //����8��������λ
        //        this.StopBits = System.IO.Ports.StopBits.One;//����1λֹͣλ
        //        this.ReadBufferSize = 4096; //���ջ�������С(�ֽ�) 
        //        this.WriteBufferSize = 2048;//���ͻ�������С(�ֽ�)
        //    }catch{
        //        Console.WriteLine("SCI���󴴽�ʧ��");
        //    }
        //}

        /// ----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:�򿪴���
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// ----------------------------------------------------------------
        public bool SCIOpen()
        {
            try
            {
                this.Open();                //�򿪴���
            }
            catch
            {
                return false;
            }
            return true;
        }


        ///-----------------------------------------------------------------
        /// <summary>                                                      
        /// ��    ��:�رմ���                                               
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// <returns>����һ������ֵ,�����ڳɹ��رպ�,����true              
        ///          ���򷵻�false</returns>                                
        ///-----------------------------------------------------------------
        public bool SCIClose()
        {
            try
            {
                this.DiscardInBuffer(); //�������ջ�����������
                this.DiscardOutBuffer();//�������ͻ�����������
                this.Dispose();         //�ͷŴ���ͨ�ŵ�������Դ
                this.Close();           //�رմ���
            }
            catch
            {
                //��������,����false
                return false;
            }
            return true;
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:���ڷ�������                                           
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// <param name="SendArray">���Ҫ���͵�����,�ֽ�����</param>      
        /// <returns>����һ������ֵ,�����ͳɹ���,����True;                 
        ///          ���򷵻�False</returns>                                
        ///-----------------------------------------------------------------
        public bool SCISendData(ref byte[] SendArray)
        {
            if (!this.IsOpen){
                return false;
            }
            try{
                this.Write(SendArray, 0, SendArray.Length);//ͨ�����ڷ��ͳ�ȥ
            }catch{
                return false;//��������,����false
            }
            return true;
        }





        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:��ȡ���ڽ��ջ��������ݣ��������顣����Ϊ����ַ��ʽ����
        ///          �����롣             
        /// ��������:��                                                     
        /// </summary>                                                      
        /// <param name="ReceiveArray">��Ž�����������,�ֽ�����</param>    
        /// <returns>����һ������ֵ,�����ճɹ���,����true                   
        ///          ����,����false</returns>                               
        ///-----------------------------------------------------------------
        public bool SCIReceiveData(ref byte[] ReceiveArray)
        {
            int lenPre,lenNow;

            lenPre = 0;
            lenNow = 1;

            if (!this.IsOpen)
            {
                return false;
            }

            //һ֡�����ͳһת�봦����ֹ���ĳ�������
            while (lenPre < lenNow)
            {

                System.Threading.Thread.Sleep(Convert.ToInt32(Math.Ceiling(2.0*9*1000/(this.BaudRate)) ));
                lenPre = lenNow;
                lenNow = this.BytesToRead;//��ȡ���ջ������е��ֽ���
            
            }

            try{
                ReceiveArray = new byte[lenNow];
                this.Read(ReceiveArray, 0, lenNow);//�ӽ��ջ������ж�ȡ���ݣ��������ReceiveArray��,�����������       
            }catch{
                return false;//��������,����false
            }
            return true;//��ȷ������true
        }

        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:���ô��ڡ�DataReceived���¼����жϣ��Ĵ�������                                          
        /// ��������:��                                                     
        /// </summary>                                                     
        /// <param name="a">���á�DataReceived���¼��Ĵ�������,����</param>                 
        ///-----------------------------------------------------------------
        public void SCIReceInt(int a)
        {
            //���ô��ڽ����ж�����
            this.ReceivedBytesThreshold = a;
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        /// ��1����ѯ�������ںź���:��ѯ��������
        /// ��ʽ��������
        /// </summary>
        /// <returns>���ں����� </returns>
        /// ------------------------------------------------------------------------------
        public static string[] SCIGetPorts()
        {
            
            return GetPortNames();
        }


        /// ----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:��ʼ������,���򿪴���                                  
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// <param name="Port">���ڿؼ�,��������</param>                    
        /// <param name="ComNum">���ں�,�ַ�������</param>                  
        /// <param name="Baud">������,����</param>                          
        /// <returns>����һ������ֵ,�����ڳɹ��򿪺�,����true               
        ///          �����ڴ��쳣ʱ,����false </returns>                  
        /// ----------------------------------------------------------------
        public bool SCIInit(System.IO.Ports.SerialPort Port,
                            string ComNum, Int32 Baud)
        {
            try
            {
                
                Port.PortName = ComNum;     //���ô��ں�
                Port.Close();               //��֤��ʼ��֮ǰ�ǹرյ�
                Port.BaudRate = Baud;       //���ò�����
                Port.Parity = System.IO.Ports.Parity.None;//��������żУ��
                Port.DataBits = 8;          //����8��������λ
                Port.StopBits = System.IO.Ports.StopBits.One;//����1λֹͣλ
                Port.ReadBufferSize = 4096; //���ջ�������С(�ֽ�) 
                Port.WriteBufferSize = 2048;//���ͻ�������С(�ֽ�)
                Port.Open();                //�򿪴���
            }
            catch
            {

                return false; //��������,����false
            }
            return true;
        }





        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:���ڷ�������                                           
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// <param name="Port">���ڿؼ�,��������</param>                   
        /// <param name="SendArray">���Ҫ���͵�����,�ֽ�����</param>      
        /// <returns>����һ������ֵ,�����ͳɹ���,����True;                 
        ///          ���򷵻�False</returns>                                
        ///-----------------------------------------------------------------
        public bool SCISendData(System.IO.Ports.SerialPort Port,
                                       ref byte[] SendArray)
        {
            try
            {
                Port.Write(SendArray, 0, SendArray.Length);//ͨ�����ڷ��ͳ�ȥ
            }
            catch
            {
                //��������,����false
                return false;
            }
            return true;
        }




        ///-----------------------------------------------------------------
        /// <summary>                                                       
        /// ��    ��:��ȡ���ڽ��ջ��������ݣ��������顣����Ϊ����ַ��ʽ����
        ///          �����롣             
        /// ��������:��                                                     
        /// </summary>                                                      
        /// <param name="Port">���ڿؼ�,��������</param>                    
        /// <param name="ReceiveArray">��Ž�����������,�ֽ�����</param>    
        /// <returns>����һ������ֵ,�����ճɹ���,����true                   
        ///          ����,����false</returns>                               
        ///-----------------------------------------------------------------
        public bool SCIReceiveData(System.IO.Ports.SerialPort Port,
                                    ref byte[] ReceiveArray)
        {
            int lenPre, lenNow;
            lenPre = 0;
            lenNow = 1;

            //һ֡�����ͳһת�봦����ֹ���ĳ�������
            while (lenPre < lenNow)
            {
                //�����ʴ���ÿ�봫�͵��ֽ�������1000/Port.BaudRate�����봫��һλ
                //һ֡������8����λ��1ֹͣλ��9λ�����ݷ��뻺�����ٴӻ�����ȡ����������ʱ��Ҫ����2��֤���㹻ʱ�䴦��
                //����ǰ�̹߳��𣬹���ʱ��Ϊ(2.0 * 9 * 1000 / (Port.BaudRate))����
                System.Threading.Thread.Sleep(Convert.ToInt32(Math.Ceiling(2.0 * 9 * 1000 / (Port.BaudRate))));
                lenPre = lenNow;
                lenNow = Port.BytesToRead;//��ȡ���ջ������е��ֽ���
            }
            try
            {
                ReceiveArray = new byte[lenNow];
                Port.Read(ReceiveArray, 0, lenNow);//�ӽ��ջ������ж�ȡ���ݣ��������ReceiveArray��,�����������       
            }
            catch
            {
                return false;//��������,����false
            }
            return true;//��ȷ������true
        }




        ///-----------------------------------------------------------------
        /// <summary>                                                      
        /// ��    ��:�رմ���                                               
        /// �ڲ�����:��                                                     
        /// </summary>                                                      
        /// <param name="Port">���ڿؼ�,��������</param>                    
        /// <returns>����һ������ֵ,�����ڳɹ��رպ�,����true              
        ///          ���򷵻�false</returns>                                
        ///-----------------------------------------------------------------
        public bool SCIClose(System.IO.Ports.SerialPort Port)
        {
            try
            {
                Port.DiscardInBuffer(); //�������ջ�����������
                Port.DiscardOutBuffer();//�������ͻ�����������
                Port.Dispose();         //�ͷŴ���ͨ�ŵ�������Դ
                Port.Close();           //�رմ���
            }
            catch
            {
                //��������,����false
                return false;
            }
            return true;
        }


    }
}