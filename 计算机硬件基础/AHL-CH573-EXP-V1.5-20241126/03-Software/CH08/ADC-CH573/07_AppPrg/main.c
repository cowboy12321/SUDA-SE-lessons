//======================================================================
// �ļ����ƣ�main.c��Ӧ�ù�����������
// ����ṩ���մ�Ƕ��ʽ��sumcu.suda.edu.cn��
// ���¼�¼��202305
// �����������������̵�..\01_Doc\Readme.txt
// ��ֲ���򣺡��̶���
//======================================================================
#define GLOBLE_VAR     //���̶���includes.h�����ȫ�ֱ���һ�������ദʹ��
#include "includes.h"  //���̶���������ͷ�ļ�

//main.cʹ�õ��ڲ�����������---------------------------------------------
//��������һ������¿�����Ϊ����Ӵ˿�ʼ���У�ʵ�������������̣�-----------
int main(void)
{
    printf("-----------------------------------------------------------\n");   
    printf("����«��ʾ��                                         	    \n");
    printf("���������ơ�ADC������������Է���			        	    \n");
    printf("��1��Ŀ�ģ�ADC�������������������                    \n"); 
    printf("��2�����ˣ�%s���ڲ��¶ȴ�����\n",singleName);
    printf("     ��֣�%s��%s��%s��\n",diffName,diffPinName1,diffPinName2);
    printf("��3�����Է��������ˣ�����оƬ���棬A/Dֵ���󣬲�Ҫ�������ţ����������оƬ\n");
    printf("                    ������%s�ֱ�ӵء�3.3V��5V���۲�%s���\n",singlePinName,singleName);
    printf("              ��֣�������%s�ӵء�%s��3.3V,�۲�%s���\n",diffPinName1,diffPinName2,diffName);
    printf("                    ������%s�ӵء�%s��3.3V,�۲�%s���\n",diffPinName2,diffPinName1,diffName);
    printf("                    ������%s��%s�̽�,�۲�%s���    \n" ,diffPinName1,diffPinName2,diffName);
    printf("------------------------------------------------------------\n"); 
    
    
//��1���������֣���ͷ��==============================================
    
    //��1.1�������ݱ��������õı�������������main����ʹ�õľֲ�����
    vuint32_t mMainLoopCount; //��ѭ��������������ѭ����ʱ������m��Ϊǰ׺��
    uint8_t   mFlag;          //�Ƶ�״̬��־
    uint8_t   adcFlag;        //ADC����־
    uint32_t  mLightCount;    //�Ƶ�״̬�л�����
    uint16_t num_AD1;
    uint16_t num_AD2;
    float temp;
    
    //��1.2�������䡿�����ж�
    DISABLE_INTERRUPTS;
    
    //��1.3�������ݱ��������õı�������ֵ����������ʹ�õľֲ���������ֵ
    mMainLoopCount=0;    //��ѭ����������
    mFlag='A';           //�Ƶ�״̬��־
    adcFlag=1;
    mLightCount=0;       //�Ƶ���˸����
    
    //��1.4��������includes.h�ļ���������ȫ�ֱ�������ȫ�ֱ�������ֵ
    
    //��1.5�����������õ����ⲿӲ���豸���û�����ģ���ʼ��
    gpio_init(LIGHT_BLUE,GPIO_OUTPUT,LIGHT_ON);	//��ʼ������
    uart_init(UART_User, 115200);
    adc_init(DIFF_CHANNEL,0);
    adc_init(SINGLE_CHANNEL,0);
    adc_init(TEMPSENSOR_CHANNEL,0);

    //��1.6����������ʹ�õ�Ӳ��ģ���жϡ�ʹ��ģ���ж�
    uart_enable_re_int(UART_User);     //ʹ���û����ڽ����ж�
    //��1.7�������䡿�����ж�
    ENABLE_INTERRUPTS;
    

//��1���������֣���β��================================================

//��2����ѭ�����֣���ͷ��==============================================
    for(;;)   //for(;;)����ͷ��
    {
        //��2.1����ѭ����������+1
        mMainLoopCount++;
        //��2.2��δ�ﵽ��ѭ�������趨ֵ������ѭ��
        if (mMainLoopCount<=MAINLOOP_COUNT) continue; //�곣����user.h�ж���
        //��2.3���ﵽ��ѭ�������趨ֵ��ִ��������䣬���еƵ���������
        //��2.3.1�����ѭ����������
        mMainLoopCount=0; 
        //��2.3.2�����״̬��־mFlagΪ'L'���Ƶ���˸����+1����ʾ���ı��״̬����־
        if (mFlag=='L')                    //�жϵƵ�״̬��־
        {
            mLightCount++;  
            printf(" ������˸����mLightCount = %d\r\n",mLightCount);
            mFlag='A';                       //�Ƶ�״̬��־
            gpio_set(LIGHT_BLUE,LIGHT_ON);  //�ơ�����
            printf(" LIGHT_BLUE:ON--\r\n"); //��������Ƶ�״̬
        }
        //��2.3.3�����״̬��־mFlagΪ'A'���ı��״̬����־
        else
        {
            mFlag='L';                       //�Ƶ�״̬��־
            gpio_set(LIGHT_BLUE,LIGHT_OFF); //�ơ�����
            printf(" LIGHT_BLUE:OFF--\r\n");  //��������Ƶ�״̬
        }
        
        num_AD1 = adc_read(DIFF_CHANNEL);        
        printf("%s(%s��%s)��A/Dֵ��  %d\r\n",diffName,diffPinName1,diffPinName2,num_AD1);  //����������ͨ��1��ADֵ
        printf("%s��ѹ��",diffName);
        printf("%.2f\n",diff_ad_to_voltage(num_AD1)); //����������ͨ��1��ADֵ
        num_AD1 = adc_read(SINGLE_CHANNEL);        
        printf("%s(%s)��A/Dֵ��  %d\r\n",singleName,singlePinName,num_AD1);           //����������ͨ��1��ADֵ
        printf("%s(%s)��ѹ�� ",singleName,singlePinName);
        printf(" %.2f\r\n",single_ad_to_voltage(num_AD1));       //����������ͨ��1��ADֵ
        num_AD2 = adc_ave(TEMPSENSOR_CHANNEL,5);                 //ʹ����ֵ�˲��;�ֵ�˲�
        printf("�ڲ��¶ȴ�������A/Dֵ��    %d\r\n",num_AD2);       //��������ڲ���������ADֵ
        temp=adc_mcu_temp(num_AD2);
	    printf("mcu�¶�Ϊ%.2f��\r\n",temp);
        printf("   \r\n"); 
        
    }  //for(;;)��β
//��2����ѭ�����֣���β��=============================================
}   //main��������β��


//����Ϊ���������õ��Ӻ���================================================



//========================================================================
/*
֪ʶҪ�أ�
��1��main.c��һ��ģ�壬���ļ����д�������漰�����Ӳ���ͻ�����ͨ�����ù���
ʵ�ֶ�Ӳ���ĸ�Ԥ��
��2�����ļ��жԺ�GLOBLE_VAR�����˶��壬�����ڰ���"includes.h"ͷ�ļ�ʱ���ᶨ
��ȫ�ֱ������������ļ��а���"includes.h"ͷ�ļ�ʱ��
����ʱ���Զ�����extern
*/

