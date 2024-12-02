//=====================================================================
//�ļ����ƣ�adc.c
//���ܸ�Ҫ��PWM�ײ���������ͷ�ļ�
//������λ���մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20240304
//оƬ���ͣ�CH573F
//=====================================================================

#include "adc.h"
#include "printf.h"

static uint32_t RoughCalib_Value = 0;	// ADC�ֵ�ƫ��ֵ

//======================================================================
//�������ƣ�adc_init
//���ܸ�Ҫ����ʼ��һ��ADͨ����
//����˵����Channel��ͨ���š���ѡ��Χ��ADC_CHANNEL_x(0=<x<=13)��
//                              ADC_CHANNEL_VBTA(14)��
//                              ADC_CHANNEL_VTEMP(15)
//         NC��������δʹ�ã�Ϊ��ǿ��������ֲ��
//======================================================================
void adc_init(uint16_t Channel,uint16_t Nc)
{	
	uint16_t i;
	uint32_t sum = 0;
	R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;  //�رմ�������
    
    if(Channel == ADC_CHANNEL_VTEMP)
    {
//		�����¶ȴ�����������ʼ��
	    R8_TEM_SENSOR = RB_TEM_SEN_PWR_ON;
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_DIFF_EN | (3 << 4);
	}else if(Channel == ADC_CHANNEL_VBTA)
    {
//    	���õ�ص�ѹ������ʼ��
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (0 << 4); // ʹ��-12dBģʽ��
	}else{
//		�ⲿ�źŵ�ͨ��������ʼ��
		R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (0x01 << 6) | (ADC_PGA_1 << 4);
	}

	// �������ݴֵ�,��ȡƫ��ֵ
	R8_ADC_CHANNEL = ADC_CHANNEL_6;		    // 6/7/10/11 ��ѡ
	R8_ADC_CFG |= RB_ADC_OFS_TEST; 			// �������ģʽ
	R8_ADC_CONVERT = RB_ADC_START;
	while(R8_ADC_CONVERT & RB_ADC_START);
	for(i = 0; i < 16; i++)
	{
		R8_ADC_CONVERT = RB_ADC_START;
		while(R8_ADC_CONVERT & RB_ADC_START);
		sum += (~R16_ADC_DATA) & RB_ADC_DATA;
	}
	sum = (sum + 8) >> 4;
	R8_ADC_CFG &= ~RB_ADC_OFS_TEST; // �رղ���ģʽ

	R8_ADC_CHANNEL = Channel;        //����ͨ��
	RoughCalib_Value = 2048-sum;	
}  

//======================================================================
//�������ƣ�adc_read
//���ܸ�Ҫ����ģ����ת������������������
//����˵����Channel��ͨ���š���ѡ��Χ���ⲿģ��ͨ��0��1��2��3��4��5��8��9��12��13
//									 Ԥ������ͨ��6��7��10��11		
//									 ��Դ���ͨ��Ϊ14,�ڲ��¶ȼ��ͨ��Ϊ15
//�ڲ�����:adc_ad_read,adc_voltage
//======================================================================
uint16_t adc_read(uint8_t Channel)
{
	R8_ADC_CHANNEL = Channel;        //����ͨ��
	uint16_t mcu_AD;
	uint16_t ad;
	double voltage;
	uint8_t max=3; //������3�Σ��Ա�֤������ѭ��
	while(max--){
		uint8_t ga=adc_ga;  
		//�Ȳ���һ�Σ�����������Ҫ������λ
		R8_ADC_CONVERT = RB_ADC_START;
		while(R8_ADC_CONVERT & RB_ADC_START);
		mcu_AD = (R16_ADC_DATA & RB_ADC_DATA) + RoughCalib_Value;

		if(ga == ADC_PGA_1)   
		{
			// printf("p1 %d\n",mcu_AD);
			if(mcu_AD>3900){  
				// 3900/2048 * 1.05 = 1.999
				//ʵ�ʵ�ѹ���ڵ���2V �л���1/2��λ
				set_ga(ADC_PGA_1_2);
			}else{
				//��Ч��Χ0V - 2V
				voltage = mcu_AD  / 2048.0 * ADC_Verf;
				break;
			}
		}else if(ga == ADC_PGA_1_2)  //��Ч��Χ1.9V - 3V
		{
			// printf("p2 %d\n",mcu_AD);
			if(mcu_AD<2877){     
				//(2877/1024 - 1)*1.05 = 1.900 
				//ʵ�ʵ�ѹС��1.9V �л���1��λ
				set_ga(ADC_PGA_1);
			}else if(mcu_AD>3949){ 
				//(3949/1024 - 1)*1.05 = 2.999 
				//ʵ�ʵ�ѹ���ڵ���3V �л���1/4��λ
				set_ga(ADC_PGA_1_4);
			}else{
				//��Ч��Χ1.9V - 3V
				voltage = (mcu_AD/ 1024.0 - 1) * ADC_Verf;
				break;
			}
		}
		else if(ga == ADC_PGA_1_4) //��Ч��Χ2.9V - 5V
		{
			// printf("p4 %d\n",mcu_AD);
			if(mcu_AD<2450){ 
				//ʵ�ʵ�ѹС��2.9V �л���1/2��λ
				set_ga(ADC_PGA_1_2);
			}else if(mcu_AD<2950){ 
				//ʵ�ʵ�ѹС��1.9V �л���1��λ
				set_ga(ADC_PGA_1);
			}else{
				voltage= (mcu_AD / 512.0 - 3) * ADC_Verf;
				break;
			}
		}else{
			// printf("p? %d\n",mcu_AD);
			set_ga(ADC_PGA_1);
		}
	}
	ad = (int)(voltage*4095/3.3);  
	return ad>4095? 4095 : ad;  //4096�� ���ֵΪ3.3v��������������Ϊ����һ��
}

//======================================================================
//�� �� ��:adc_mid                                                  
//��    ��:��ȡͨ��channel��ֵ�˲����A/Dת�����                    
//��    ��:channel = ͨ����                                           
//��    ��:��ͨ����ֵ�˲����A/Dת�����                         
//�ڲ�����:adc_ad_read                                               
//======================================================================
uint16_t adc_mid(uint16_t Channel)
{
    uint16_t a,b,c;
    uint16_t i;
    //��1��ȡ����A/Dת�����
    a = adc_read(Channel);
    for (i=0;i<200;i++) ;
    b = adc_read(Channel);
    for (i=0;i<200;i++) ;
    c = adc_read(Channel);
   // ��2��������A/Dת�������ȡ��ֵ
    return  a > b ? (b > c ? b : ( a > c ? c : a)) : ( a > c ? a: (b > c ? c : a));
}

//======================================================================
//�� �� ��:adc_ave                                                    
//��    ��:1·A/Dת������(��ֵ�˲�),ͨ��channel����n����ֵ�˲�,�������  
//          ��ֵ,�ó���ֵ�˲����                                        
//��    ��:channel = ͨ����,n = ��ֵ�˲�����                               
//��    ��:��ͨ����ֵ�˲����A/Dת�����                                   
//�ڲ�����:adc_mid                                                          
//======================================================================
uint16_t adc_ave(uint16_t Channel,uint8_t n) 
{
    uint16_t i;
    uint32_t j;
    j = 0;
    for (i = 0; i < n; i++) j += adc_mid(Channel);
    j = j/n;
    return (uint16_t)j;
}
//============================================================================
//�������ƣ�adc_mcu_temp
//���ܸ�Ҫ����������оƬ�ڲ�mcu�¶�ADֵת��Ϊʵ���¶�
//����˵����mcu_temp_AD��ͨ��adc_ad_read�����õ���ADֵ
//�������أ�ʵ���¶�ֵ
//============================================================================
float adc_mcu_temp(uint16_t mcu_temp_AD)
{
	//��1���������
	uint32_t C25 = 0;
    int temp = 0.0;
    C25 = (*((PUINT32)0x7F014));
	//��2��ת����ʵ���¶�
    /* ��ǰ�¶� = ��׼�¶� + ��ADCƫ�� * ADC����ϵ���� */
    temp = (((C25 >> 16) & 0xFFFF) ? ((C25 >> 16) & 0xFFFF) : 25) +
           (mcu_temp_AD - ((int)(C25 & 0xFFFF))) * 10 / 27;
	printf("mcu�¶�Ϊ%d��\r\n",temp);
	
    return (float)temp; 
}
