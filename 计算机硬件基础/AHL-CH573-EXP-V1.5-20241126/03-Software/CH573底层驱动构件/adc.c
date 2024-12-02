//=====================================================================
//�ļ����ƣ�adc.c
//���ܸ�Ҫ��ADC�ײ���������ͷ�ļ�
//������λ���մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20240812
//оƬ���ͣ�CH573F
//=====================================================================

#include "adc.h"
#include "printf.h"

#define adc_ga ((R8_ADC_CFG>>4)&0b11)
#define set_ga(ga)  R8_ADC_CFG = (R8_ADC_CFG & ~(0b11<<4)) | (ga << 4)
#define is_single(x) (x<0x10)
#define set_channel(x) R8_ADC_CHANNEL = x&0xf
#define ADC_Verf 1.05

//��������
#define ADC_PGA_1_4 0             // -12dB, 1/4��    �����ѹ 2.9V <--> 5.0V
#define ADC_PGA_1_2 1             // -6dB, 1/2��     �����ѹ 1.9V <--> 3.0V
#define ADC_PGA_1 2               // 0dB, 1��,������  �����ѹ 0.0V <--> 2.0V
#define ADC_PGA_2 3               // 6dB, 2��        �����ѹ 0.6V <--> 1.5V

static uint32_t RoughCalib_Value = 0;	// ADC�ֵ�ƫ��ֵ

//======================================================================
//�������ƣ�adc_init
//���ܸ�Ҫ����ʼ��һ��ADͨ����
//����˵����Channel��ͨ���š�
// 				��ͨ����������ѡ��Χ��ADC_CHANNEL_x(x=0��1��2��3��4��5��8��9��12��13)��
//                              ADC_CHANNEL_VBTA��ADC_CHANNEL_VTEMP
// 				���ͨ����������ѡ��Χ��ADC_CHANNEL_DIFF_x(x=0��1)
//   				ѡADC_CHANNEL_DIFF_0ʱ������ģʽ��ADC_CHANNEL_0Ϊ�������������ģʽ��ADC_CHANNEL_2Ϊ��ָ���
//   				ѡADC_CHANNEL_DIFF_1ʱ������ģʽ��ADC_CHANNEL_1Ϊ�������������ģʽ��ADC_CHANNEL_3Ϊ��ָ���
//         NC��������δʹ�ã�Ϊ��ǿ��������ֲ��
//======================================================================
void adc_init(uint16_t Channel,uint16_t NC)
{	
	uint8_t i;
	uint32_t sum = 0;
	R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;  //�رմ�������

	//��ʼ���������������üĴ������ڶ�ȡʱ�����üĴ�����ȡ
    switch (Channel)
    {
		case ADC_CHANNEL_0:  gpio_init(PTA_NUM|4,  GPIO_INPUT,1);break;  // PA4
		case ADC_CHANNEL_1:  gpio_init(PTA_NUM|5,  GPIO_INPUT,1);break;  // PA5
		case ADC_CHANNEL_2:  gpio_init(PTA_NUM|12, GPIO_INPUT,1);break;  // PA12
		case ADC_CHANNEL_3:  gpio_init(PTA_NUM|13, GPIO_INPUT,1);break;  // PA13
		case ADC_CHANNEL_4:  gpio_init(PTA_NUM|14, GPIO_INPUT,1);break;  // PA14 
		case ADC_CHANNEL_5:  gpio_init(PTA_NUM|15, GPIO_INPUT,1);break;  // PA15
		case ADC_CHANNEL_8:  gpio_init(PTB_NUM|0,  GPIO_INPUT,1);break;  // PB0
		case ADC_CHANNEL_9:  gpio_init(PTB_NUM|6,  GPIO_INPUT,1);break;  // PB6
		case ADC_CHANNEL_12: gpio_init(PTA_NUM|8,  GPIO_INPUT,1);break;  // PA8
		case ADC_CHANNEL_13: gpio_init(PTA_NUM|9,  GPIO_INPUT,1);break;  // PA9
		case ADC_CHANNEL_DIFF_0: 
			gpio_init(PTA_NUM|4,  GPIO_INPUT,1);  // PA4  ����
			gpio_init(PTA_NUM|12, GPIO_INPUT,1);  // PA12 ����
			break; 
		case ADC_CHANNEL_DIFF_1:
			gpio_init(PTA_NUM|5,  GPIO_INPUT,1);  // PA5  ����
			gpio_init(PTA_NUM|13, GPIO_INPUT,1);  // PA13 ����
			break;   
    default:
        break;
    }
    
	// �������ݴֵ�,��ȡƫ��ֵ
	// R8_ADC_CHANNEL = ADC_CHANNEL_TEST1;		// TEST1-TEST4 ��ѡ
	// R8_ADC_CFG |= RB_ADC_OFS_TEST; 			// �������ģʽ
	// R8_ADC_CONVERT = RB_ADC_START;
	// while(R8_ADC_CONVERT & RB_ADC_START);
	
	// for(i = 0; i < 16; i++)
	// {
	// 	R8_ADC_CONVERT = RB_ADC_START;
	// 	while(R8_ADC_CONVERT & RB_ADC_START);
	// 	sum += (~R16_ADC_DATA) & RB_ADC_DATA;
	// }
	// sum = (sum + 8) >> 4;
	// R8_ADC_CFG &= ~RB_ADC_OFS_TEST;  //�رղ���ģʽ

	// RoughCalib_Value = 2048-sum;	
}  

//======================================================================
//�������ƣ�adc_read
//���ܸ�Ҫ����ģ����ת������������������
//����˵����Channel��ͨ���š�
// 				��ͨ����������ѡ��Χ��ADC_CHANNEL_x(x=0��1��2��3��4��5��8��9��12��13)��
//                              ADC_CHANNEL_VBTA��ADC_CHANNEL_VTEMP
// 				���ͨ����������ѡ��Χ��ADC_CHANNEL_DIFF_x(x=0��1)
//   				ѡADC_CHANNEL_DIFF_0ʱ������ADC_CHANNEL_0-ADC_CHANNEL_2��ֵ����ΧΪ4096�����-4.2-4.2v
//   				ѡADC_CHANNEL_DIFF_1ʱ������ADC_CHANNEL_1-ADC_CHANNEL_3��ֵ����ΧΪ4096�����-4.2-4.2v
//======================================================================
uint16_t adc_read(uint8_t Channel)
{
	uint16_t mcu_AD;
	uint16_t ad;
	double voltage;
	uint8_t max=4; //������4�Σ��Ա�֤������ѭ��

	//��ÿ�ζ�ȡʱ���üĴ���������֧�ֲ�ͬͨ����ϲɼ�
	set_channel(Channel); //����ͨ�� 
    R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;
	switch (Channel)
	{
	case ADC_CHANNEL_TEMPSENSOR:
//		�����¶ȴ�����������ʼ��
		R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;
		R8_TEM_SENSOR = RB_TEM_SEN_PWR_ON;
		R8_ADC_CHANNEL = 15;
		R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_DIFF_EN | (3 << 4);
		goto single_cap;
	case ADC_CHANNEL_VREFINT:
//    	���õ�ص�ѹ������ʼ��
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (ADC_PGA_1_4 << 4); // ʹ��-12dBģʽ��
		goto multi_cap;
	case ADC_CHANNEL_DIFF_0:
	case ADC_CHANNEL_DIFF_1:
	//����ǲ��ͨ����������Ϊ1/4����
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_DIFF_EN | (0x01 << 6) | (ADC_PGA_1 << 4);
		set_ga(ADC_PGA_1_4);
		goto single_cap;
	default:
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (0x01 << 6) | (ADC_PGA_1 << 4);
		goto multi_cap;
	}

multi_cap:
	//�ж����λ��ģʽ��������Ҫ��βɼ�����ȷ��һ��׼ȷ��ֵ
	while(max--){
		uint8_t ga=adc_ga;  
		//�Ȳ���һ�Σ�����������Ҫ������λ
		R8_ADC_CONVERT = RB_ADC_START;
		while(R8_ADC_CONVERT & RB_ADC_START);
		mcu_AD = (R16_ADC_DATA & RB_ADC_DATA) + RoughCalib_Value;
		
		if(ga == ADC_PGA_1)   //�˵�λ�ɼ���Ч��Χ0V - 2V
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
		}else if(ga == ADC_PGA_1_2)  //�˵�λ�ɼ���Ч��Χ1.9V - 3V
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
		else if(ga == ADC_PGA_1_4) //�˵�λ�ɼ���Ч��Χ2.9V - 5V
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
		}else{ //�˵�λ���Ϸ����л�Ϊ��ͨ��λ
			set_ga(ADC_PGA_1);
		}
	}
	ad = (int)(voltage*4095/3.3);  
	return ad>4095? 4095 : ad;  //4096�� ���ֵΪ3.3v��������������Ϊ����һ��
single_cap:
	//û�ж����λ��ֱ�ӷ������ݾͿ���
	R8_ADC_CONVERT = RB_ADC_START;
	while(R8_ADC_CONVERT & RB_ADC_START);
	return (R16_ADC_DATA & RB_ADC_DATA);
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
    for (i=0;i<3000;i++) ;
    b = adc_read(Channel);
    for (i=0;i<3000;i++) ;
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
    uint32_t C25_Data[2];
    float      cal;

    FLASH_EEPROM_CMD(CMD_GET_ROM_INFO, 0x7F014, C25_Data, 0);
	cal = 25+ (((mcu_temp_AD * 5100) >> 10) - ((int)(C25_Data[0]) ) * 11) / 27.0;
    return cal;
}
