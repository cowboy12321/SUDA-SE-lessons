//=====================================================================
//文件名称：adc.h
//功能概要：PWM底层驱动构件头文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240304
//芯片类型：CH573F
//=====================================================================

#ifndef _ADC_H         //防止重复定义（ 开头)
#define _ADC_H


#include "mcu.h"

//通道号定义
#define ADC_CHANNEL_0 0           	//通道0  PA4
#define ADC_CHANNEL_1 1      	  	//通道1  PA5
#define ADC_CHANNEL_2 2           	//通道2  PA12
#define ADC_CHANNEL_3 3           	//通道3  PA13
#define ADC_CHANNEL_4 4           	//通道4  PA14
#define ADC_CHANNEL_5 5           	//通道5  PA25
#define ADC_CHANNEL_6 6             //预留的测试通道1
#define ADC_CHANNEL_7 7             //预留的测试通道2
#define ADC_CHANNEL_8 8           	//通道8  PB0
#define ADC_CHANNEL_9 9           	//通道9  PB6
#define ADC_CHANNEL_10 10           //预留的测试通道3
#define ADC_CHANNEL_11 11  		    //预留的测试通道4
#define ADC_CHANNEL_12 12          	//通道12 PA8
#define ADC_CHANNEL_13 13          	//通道13 PA9
#define ADC_CHANNEL_VBTA 14        	//电源监测
#define ADC_CHANNEL_VTEMP 15       	//内部温度检测
//引脚单端或差分选择    
#define AD_DIFF 1                 //差分输入
#define AD_SINGLE 0               //单端输入
//增幅定义
#define ADC_PGA_1_4 0             // -12dB, 1/4倍    建议电压 2.9V <--> 5.0V
#define ADC_PGA_1_2 1             // -6dB, 1/2倍     建议电压 1.9V <--> 3.0V
#define ADC_PGA_1 2               // 0dB, 1倍,无增益  建议电压 0.0V <--> 2.0V
#define ADC_PGA_2 3               // 6dB, 2倍        建议电压 0.6V <--> 1.5V

#define adc_ga ((R8_ADC_CFG>>4)&0b11)
#define set_ga(ga)  R8_ADC_CFG = (R8_ADC_CFG & ~(0b11<<4)) | (ga << 4)
#define ADC_Verf 1.05

//======================================================================
//函数名称：adc_init
//功能概要：初始化一个AD通道号
//参数说明：Channel：通道号。可选范围：ADC_CHANNEL_x(0=<x<=13)、
//                              ADC_CHANNEL_VBTA(14)、
//                              ADC_CHANNEL_VTEMP(15)
//         NC：本函数未使用，为增强函数可移植性
//======================================================================
void adc_init(uint16_t Channel,uint16_t NC);

//======================================================================
//函数名称：adc_read
//功能概要：将模拟量转换成数字量，并返回
//参数说明：Channel：通道号。可选范围：外部模拟通道0、1、2、3、4、5、8、9、12、13
//									 预留测试通道6、7、10、11		
//									 电源监测通道为14,内部温度检测通道为15
//内部调用:adc_ad_read,adc_voltage
//======================================================================
uint16_t adc_read(uint8_t Channel);
//======================================================================
//函 数 名:adc_mid                                                  
//功    能:获取通道channel中值滤波后的A/D转换结果                    
//参    数:channel = 通道号                                           
//返    回:该通道中值滤波后的A/D转换结果                         
//内部调用:adc_read                                               
//======================================================================
uint16_t adc_mid(uint16_t Channel);

//======================================================================
//函 数 名:adc_ave                                                    
//功    能:1路A/D转换函数(均值滤波),通道channel进行n次中值滤波,求和再作  
//          均值,得出均值滤波结果                                        
//参    数:channel = 通道号,n = 中值滤波次数                               
//返    回:该通道均值滤波后的A/D转换结果                                   
//内部调用:adc_mid                                                          
//======================================================================
uint16_t adc_ave(uint16_t Channel,uint8_t n);
//============================================================================
//函数名称：adc_mcu_temp
//功能概要：将读到的芯片内部mcu温度AD值转换为实际温度
//参数说明：mcu_temp_AD：通过adc_read函数得到的AD值
//函数返回：实际温度值
//============================================================================
float adc_mcu_temp(uint16_t mcu_temp_AD);

#endif     //防止重复定义（ 结尾）