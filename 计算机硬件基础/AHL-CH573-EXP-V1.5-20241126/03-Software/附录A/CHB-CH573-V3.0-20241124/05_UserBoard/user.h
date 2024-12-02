//======================================================================
//文件名称：user.h（user头文件）
//制作单位:RISC-V(sumcu.suda.edu.cn)
//更新记录：20210426-20240202
//概要说明：（1）包含用到的头文件，为达到应用程序的可移植性，具体硬件要接
//              芯片哪个引脚，需要在这里宏定义，目的是实现对具体硬件对象
//              编程，而不是对芯片的引脚编程
//          （2）中断处理程序名字在此宏定义，以便isr.c可移植
//======================================================================

#ifndef __USER_H_
#define __USER_H_

//（1）【变动】文件包含，根据需要增减
#include "gpio.h"
#include "flash.h"
#include "uart.h"
#include "printf.h"
#include "gec.h"
#include "adc.h"
#include "pwm.h"
//（2）【变动】为了<07_AppPrg>可复用，main.c及isr.h中用到的宏常数在这里定义
#define MAINLOOP_COUNT  (4000000)

//（3）【变动】指示灯端口及引脚定义—根据实际使用的引脚改动
//指示灯端口及引脚定义

#define  LIGHT_RED      (PTB_NUM|12)  //红灯
#define  LIGHT_GREEN    (PTB_NUM|13)  //绿灯
#define  LIGHT_BLUE     (PTB_NUM|14)  //蓝灯
//灯状态宏定义（灯亮、灯暗对应的物理电平由硬件接法决定）
#define  LIGHT_ON       0    //灯亮
#define  LIGHT_OFF      1    //灯暗

//（4）【变动】UART可用模块定义
#define UART_Debug   UART_1   //用于程序更新，无法被使用
#define UART_User    UART_0   //用户串口

//（5）【变动】为了06、07文件夹可复用，这里注册中断服务函数
void UART0_IRQHandler(void) __attribute__((interrupt("WCH-Interrupt-fast")));
#define UART_User_Handler UART0_IRQHandler


//（6）gec_lab
#define PTB PTB_NUM
#define PTA PTA_NUM
#define INPUT_ON       1    //高电平
#define INPUT_OFF      0    //低电平
#define G_DA1 PWM_PIN1 //(PTA13)
#define G_DA2 PWM_PIN6 //(PTB14)

#define G_AD1 ADC_CHANNEL_0 //PTA4
#define G_AD2 ADC_CHANNEL_1	//PTA5
#define G_AD3 ADC_CHANNEL_2	//PTA12
#define G_AD4 ADC_CHANNEL_4	//PTA14

#define LAB_DIGITAL_GET(pinData) gpio_get(pinData)
#define LAB_DIGITAL_INIT(pinData,model) gpio_init(pinData,model,INPUT_OFF) 
#define LAB_DIGITAL_CTRL(pinData,value) gpio_init(pinData,GPIO_OUTPUT,value)
#define LAB_ANALOG_CTRL(pinData,value) pwm_init(pinData, 16000000, PWMX_Cycle_256, value * 100.0 / 4095, PWM_CENTER, PWM_MINUS)
#define LAB_ANALOG_INIT(pinData) adc_init(pinData,AD_SINGLE);
#define LAB_ANALOG_AVE(pinData) adc_ave(pinData,10)
#define LAB_ANALOG_AVE_FAST(pinData) adc_ave(pinData,5)
#define PIN_MAX 40
extern uint16_t digitalPin[PIN_MAX];
extern uint16_t analogInPin[PIN_MAX];
extern uint16_t analogOutPin[PIN_MAX];

#endif /* __USER_H_ */
