//=====================================================================
//文件名称：adc.c
//功能概要：ADC底层驱动构件头文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240812
//芯片类型：CH573F
//=====================================================================

#include "adc.h"
#include "printf.h"

#define adc_ga ((R8_ADC_CFG>>4)&0b11)
#define set_ga(ga)  R8_ADC_CFG = (R8_ADC_CFG & ~(0b11<<4)) | (ga << 4)
#define is_single(x) (x<0x10)
#define set_channel(x) R8_ADC_CHANNEL = x&0xf
#define ADC_Verf 1.05

//增幅定义
#define ADC_PGA_1_4 0             // -12dB, 1/4倍    建议电压 2.9V <--> 5.0V
#define ADC_PGA_1_2 1             // -6dB, 1/2倍     建议电压 1.9V <--> 3.0V
#define ADC_PGA_1 2               // 0dB, 1倍,无增益  建议电压 0.0V <--> 2.0V
#define ADC_PGA_2 3               // 6dB, 2倍        建议电压 0.6V <--> 1.5V

static uint32_t RoughCalib_Value = 0;	// ADC粗调偏差值

//======================================================================
//函数名称：adc_init
//功能概要：初始化一个AD通道号
//参数说明：Channel：通道号。
// 				单通道采样，可选范围：ADC_CHANNEL_x(x=0、1、2、3、4、5、8、9、12、13)、
//                              ADC_CHANNEL_VBTA、ADC_CHANNEL_VTEMP
// 				差分通道采样，可选范围：ADC_CHANNEL_DIFF_x(x=0、1)
//   				选ADC_CHANNEL_DIFF_0时，单端模式的ADC_CHANNEL_0为差分正极、单端模式的ADC_CHANNEL_2为差分负极
//   				选ADC_CHANNEL_DIFF_1时，单端模式的ADC_CHANNEL_1为差分正极、单端模式的ADC_CHANNEL_3为差分负极
//         NC：本函数未使用，为增强函数可移植性
//======================================================================
void adc_init(uint16_t Channel,uint16_t NC)
{	
	uint8_t i;
	uint32_t sum = 0;
	R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;  //关闭触摸功能

	//初始化函数不配置设置寄存器，在读取时才配置寄存器读取
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
			gpio_init(PTA_NUM|4,  GPIO_INPUT,1);  // PA4  正极
			gpio_init(PTA_NUM|12, GPIO_INPUT,1);  // PA12 负极
			break; 
		case ADC_CHANNEL_DIFF_1:
			gpio_init(PTA_NUM|5,  GPIO_INPUT,1);  // PA5  正极
			gpio_init(PTA_NUM|13, GPIO_INPUT,1);  // PA13 负极
			break;   
    default:
        break;
    }
    
	// 采样数据粗调,获取偏差值
	// R8_ADC_CHANNEL = ADC_CHANNEL_TEST1;		// TEST1-TEST4 可选
	// R8_ADC_CFG |= RB_ADC_OFS_TEST; 			// 进入测试模式
	// R8_ADC_CONVERT = RB_ADC_START;
	// while(R8_ADC_CONVERT & RB_ADC_START);
	
	// for(i = 0; i < 16; i++)
	// {
	// 	R8_ADC_CONVERT = RB_ADC_START;
	// 	while(R8_ADC_CONVERT & RB_ADC_START);
	// 	sum += (~R16_ADC_DATA) & RB_ADC_DATA;
	// }
	// sum = (sum + 8) >> 4;
	// R8_ADC_CFG &= ~RB_ADC_OFS_TEST;  //关闭测试模式

	// RoughCalib_Value = 2048-sum;	
}  

//======================================================================
//函数名称：adc_read
//功能概要：将模拟量转换成数字量，并返回
//参数说明：Channel：通道号。
// 				单通道采样，可选范围：ADC_CHANNEL_x(x=0、1、2、3、4、5、8、9、12、13)、
//                              ADC_CHANNEL_VBTA、ADC_CHANNEL_VTEMP
// 				差分通道采样，可选范围：ADC_CHANNEL_DIFF_x(x=0、1)
//   				选ADC_CHANNEL_DIFF_0时，返回ADC_CHANNEL_0-ADC_CHANNEL_2的值，范围为4096级别的-4.2-4.2v
//   				选ADC_CHANNEL_DIFF_1时，返回ADC_CHANNEL_1-ADC_CHANNEL_3的值，范围为4096级别的-4.2-4.2v
//======================================================================
uint16_t adc_read(uint8_t Channel)
{
	uint16_t mcu_AD;
	uint16_t ad;
	double voltage;
	uint8_t max=4; //最大采样4次，以保证不会死循环

	//在每次读取时配置寄存器，可以支持不同通道混合采集
	set_channel(Channel); //设置通道 
    R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;
	switch (Channel)
	{
	case ADC_CHANNEL_TEMPSENSOR:
//		内置温度传感器采样初始化
		R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;
		R8_TEM_SENSOR = RB_TEM_SEN_PWR_ON;
		R8_ADC_CHANNEL = 15;
		R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_DIFF_EN | (3 << 4);
		goto single_cap;
	case ADC_CHANNEL_VREFINT:
//    	内置电池电压采样初始化
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (ADC_PGA_1_4 << 4); // 使用-12dB模式，
		goto multi_cap;
	case ADC_CHANNEL_DIFF_0:
	case ADC_CHANNEL_DIFF_1:
	//如果是差分通道，则设置为1/4增益
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_DIFF_EN | (0x01 << 6) | (ADC_PGA_1 << 4);
		set_ga(ADC_PGA_1_4);
		goto single_cap;
	default:
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (0x01 << 6) | (ADC_PGA_1 << 4);
		goto multi_cap;
	}

multi_cap:
	//有多个档位的模式，所以需要多次采集才能确定一个准确的值
	while(max--){
		uint8_t ga=adc_ga;  
		//先采样一次，后续可能需要调整档位
		R8_ADC_CONVERT = RB_ADC_START;
		while(R8_ADC_CONVERT & RB_ADC_START);
		mcu_AD = (R16_ADC_DATA & RB_ADC_DATA) + RoughCalib_Value;
		
		if(ga == ADC_PGA_1)   //此档位采集有效范围0V - 2V
		{
			// printf("p1 %d\n",mcu_AD);
			if(mcu_AD>3900){  
				// 3900/2048 * 1.05 = 1.999
				//实际电压大于等于2V 切换到1/2档位
				set_ga(ADC_PGA_1_2);
			}else{
				//有效范围0V - 2V
				voltage = mcu_AD  / 2048.0 * ADC_Verf;
				break;
			}
		}else if(ga == ADC_PGA_1_2)  //此档位采集有效范围1.9V - 3V
		{
			// printf("p2 %d\n",mcu_AD);
			if(mcu_AD<2877){     
				//(2877/1024 - 1)*1.05 = 1.900 
				//实际电压小于1.9V 切换到1档位
				set_ga(ADC_PGA_1);
			}else if(mcu_AD>3949){ 
				//(3949/1024 - 1)*1.05 = 2.999 
				//实际电压大于等于3V 切换到1/4档位
				set_ga(ADC_PGA_1_4);
			}else{
				//有效范围1.9V - 3V
				voltage = (mcu_AD/ 1024.0 - 1) * ADC_Verf;
				break;
			}
		}
		else if(ga == ADC_PGA_1_4) //此档位采集有效范围2.9V - 5V
		{
			// printf("p4 %d\n",mcu_AD);
			if(mcu_AD<2450){ 
				//实际电压小于2.9V 切换到1/2档位
				set_ga(ADC_PGA_1_2);
			}else if(mcu_AD<2950){ 
				//实际电压小于1.9V 切换到1档位
				set_ga(ADC_PGA_1);
			}else{
				voltage= (mcu_AD / 512.0 - 3) * ADC_Verf;
				break;
			}
		}else{ //此档位不合法，切换为普通档位
			set_ga(ADC_PGA_1);
		}
	}
	ad = (int)(voltage*4095/3.3);  
	return ad>4095? 4095 : ad;  //4096级 最大值为3.3v。和其他构件行为保持一致
single_cap:
	//没有多个档位，直接返回数据就可以
	R8_ADC_CONVERT = RB_ADC_START;
	while(R8_ADC_CONVERT & RB_ADC_START);
	return (R16_ADC_DATA & RB_ADC_DATA);
}

//======================================================================
//函 数 名:adc_mid                                                  
//功    能:获取通道channel中值滤波后的A/D转换结果                    
//参    数:channel = 通道号                                           
//返    回:该通道中值滤波后的A/D转换结果                         
//内部调用:adc_ad_read                                               
//======================================================================
uint16_t adc_mid(uint16_t Channel)
{
    uint16_t a,b,c;
    uint16_t i;
    //（1）取三次A/D转换结果
    a = adc_read(Channel);
    for (i=0;i<3000;i++) ;
    b = adc_read(Channel);
    for (i=0;i<3000;i++) ;
    c = adc_read(Channel);
   // （2）从三次A/D转换结果中取中值
    return  a > b ? (b > c ? b : ( a > c ? c : a)) : ( a > c ? a: (b > c ? c : a));
}

//======================================================================
//函 数 名:adc_ave                                                    
//功    能:1路A/D转换函数(均值滤波),通道channel进行n次中值滤波,求和再作  
//          均值,得出均值滤波结果                                        
//参    数:channel = 通道号,n = 中值滤波次数                               
//返    回:该通道均值滤波后的A/D转换结果                                   
//内部调用:adc_mid                                                          
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
//函数名称：adc_mcu_temp
//功能概要：将读到的芯片内部mcu温度AD值转换为实际温度
//参数说明：mcu_temp_AD：通过adc_ad_read函数得到的AD值
//函数返回：实际温度值
//============================================================================
float adc_mcu_temp(uint16_t mcu_temp_AD)
{
    uint32_t C25_Data[2];
    float      cal;

    FLASH_EEPROM_CMD(CMD_GET_ROM_INFO, 0x7F014, C25_Data, 0);
	cal = 25+ (((mcu_temp_AD * 5100) >> 10) - ((int)(C25_Data[0]) ) * 11) / 27.0;
    return cal;
}
