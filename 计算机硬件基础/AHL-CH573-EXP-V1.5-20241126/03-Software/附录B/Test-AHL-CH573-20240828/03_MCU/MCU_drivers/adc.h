//=====================================================================
//文件名称：adc.h
//功能概要：ADC底层驱动构件头文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240813
//芯片类型：CH573F
//=====================================================================

#ifndef _ADC_H         //防止重复定义（ 开头)
#define _ADC_H


#include "mcu.h"
#include "gpio.h"

//通道号定义
#define ADC_CHANNEL_0 0           	//通道0  PA4
#define ADC_CHANNEL_1 1      	  	//通道1  PA5
#define ADC_CHANNEL_2 2           	//通道2  PA12
#define ADC_CHANNEL_3 3           	//通道3  PA13
#define ADC_CHANNEL_4 4           	//通道4  PA14
#define ADC_CHANNEL_5 5           	//通道5  PA15
#define ADC_CHANNEL_TEST1 6         //预留的测试通道1、用户不可用
#define ADC_CHANNEL_TEST2 7         //预留的测试通道2、用户不可用
#define ADC_CHANNEL_8 8           	//通道8  PB0
#define ADC_CHANNEL_9 9           	//通道9  PB6
#define ADC_CHANNEL_TEST3 10        //预留的测试通道3、用户不可用
#define ADC_CHANNEL_TEST4 11  		//预留的测试通道4、用户不可用
#define ADC_CHANNEL_12 12          	//通道12 PA8
#define ADC_CHANNEL_13 13          	//通道13 PA9
#define ADC_CHANNEL_VREFINT 14        	//电源监测
#define ADC_CHANNEL_TEMPSENSOR 15       	//内部温度检测
#define ADC_CHANNEL_DIFF_0 0x10     //差分通道1
#define ADC_CHANNEL_DIFF_1 0x11     //差分通道2

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
void adc_init(uint16_t Channel,uint16_t Nc);

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

//======================================================================
//函数名称：adc_mcu_temp
//功能概要：将读到的芯片内部mcu温度AD值转换为实际温度
//参数说明：mcu_temp_AD：通过adc_read函数得到的AD值
//函数返回：实际温度值
//======================================================================
float adc_mcu_temp(uint16_t mcu_temp_AD);

#endif     //防止重复定义（ 结尾）