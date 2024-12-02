//=====================================================================
//文件名称：adc.c
//功能概要：PWM底层驱动构件头文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240304
//芯片类型：CH573F
//=====================================================================

#include "adc.h"
#include "printf.h"

static uint32_t RoughCalib_Value = 0;	// ADC粗调偏差值

//======================================================================
//函数名称：adc_init
//功能概要：初始化一个AD通道号
//参数说明：Channel：通道号。可选范围：ADC_CHANNEL_x(0=<x<=13)、
//                              ADC_CHANNEL_VBTA(14)、
//                              ADC_CHANNEL_VTEMP(15)
//         NC：本函数未使用，为增强函数可移植性
//======================================================================
void adc_init(uint16_t Channel,uint16_t Nc)
{	
	uint16_t i;
	uint32_t sum = 0;
	R8_TKEY_CFG &= ~RB_TKEY_PWR_ON;  //关闭触摸功能
    
    if(Channel == ADC_CHANNEL_VTEMP)
    {
//		内置温度传感器采样初始化
	    R8_TEM_SENSOR = RB_TEM_SEN_PWR_ON;
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_DIFF_EN | (3 << 4);
	}else if(Channel == ADC_CHANNEL_VBTA)
    {
//    	内置电池电压采样初始化
	    R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (0 << 4); // 使用-12dB模式，
	}else{
//		外部信号单通道采样初始化
		R8_ADC_CFG = RB_ADC_POWER_ON | RB_ADC_BUF_EN | (0x01 << 6) | (ADC_PGA_1 << 4);
	}

	// 采样数据粗调,获取偏差值
	R8_ADC_CHANNEL = ADC_CHANNEL_6;		    // 6/7/10/11 可选
	R8_ADC_CFG |= RB_ADC_OFS_TEST; 			// 进入测试模式
	R8_ADC_CONVERT = RB_ADC_START;
	while(R8_ADC_CONVERT & RB_ADC_START);
	for(i = 0; i < 16; i++)
	{
		R8_ADC_CONVERT = RB_ADC_START;
		while(R8_ADC_CONVERT & RB_ADC_START);
		sum += (~R16_ADC_DATA) & RB_ADC_DATA;
	}
	sum = (sum + 8) >> 4;
	R8_ADC_CFG &= ~RB_ADC_OFS_TEST; // 关闭测试模式

	R8_ADC_CHANNEL = Channel;        //设置通道
	RoughCalib_Value = 2048-sum;	
}  

//======================================================================
//函数名称：adc_read
//功能概要：将模拟量转换成数字量，并返回
//参数说明：Channel：通道号。可选范围：外部模拟通道0、1、2、3、4、5、8、9、12、13
//									 预留测试通道6、7、10、11		
//									 电源监测通道为14,内部温度检测通道为15
//内部调用:adc_ad_read,adc_voltage
//======================================================================
uint16_t adc_read(uint8_t Channel)
{
	R8_ADC_CHANNEL = Channel;        //设置通道
	uint16_t mcu_AD;
	uint16_t ad;
	double voltage;
	uint8_t max=3; //最大采样3次，以保证不会死循环
	while(max--){
		uint8_t ga=adc_ga;  
		//先采样一次，后续可能需要调整档位
		R8_ADC_CONVERT = RB_ADC_START;
		while(R8_ADC_CONVERT & RB_ADC_START);
		mcu_AD = (R16_ADC_DATA & RB_ADC_DATA) + RoughCalib_Value;

		if(ga == ADC_PGA_1)   
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
		}else if(ga == ADC_PGA_1_2)  //有效范围1.9V - 3V
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
		else if(ga == ADC_PGA_1_4) //有效范围2.9V - 5V
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
		}else{
			// printf("p? %d\n",mcu_AD);
			set_ga(ADC_PGA_1);
		}
	}
	ad = (int)(voltage*4095/3.3);  
	return ad>4095? 4095 : ad;  //4096级 最大值为3.3v。和其他构件行为保持一致
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
    for (i=0;i<200;i++) ;
    b = adc_read(Channel);
    for (i=0;i<200;i++) ;
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
	//（1）定义变量
	uint32_t C25 = 0;
    int temp = 0.0;
    C25 = (*((PUINT32)0x7F014));
	//（2）转换成实际温度
    /* 当前温度 = 标准温度 + （ADC偏差 * ADC线性系数） */
    temp = (((C25 >> 16) & 0xFFFF) ? ((C25 >> 16) & 0xFFFF) : 25) +
           (mcu_temp_AD - ((int)(C25 & 0xFFFF))) * 10 / 27;
	printf("mcu温度为%d度\r\n",temp);
	
    return (float)temp; 
}
