//======================================================================
//文件名称：user.h（user头文件）
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//更新记录：20181201-20240802
//概要说明：（1）包含用到的头文件，为达到应用程序的可移植性，具体硬件要接
//              芯片哪个引脚，需要在这里宏定义，目的是实现对具体硬件对象
//              编程，而不是对芯片的引脚编程
//          （2）中断处理程序名字在此宏定义，以便isr.c可移植
//======================================================================
#ifndef USER_H   //防止重复定义（USER_H 开头）
#define USER_H

//【1】文件包含，根据需要增减
#include "gec.h"
#include "gpio.h"
#include "flash.h"
#include "uart.h"
#include "printf.h"
#include "SysTick.h"
//【2】为了<07_AppPrg>可复用，main.c及isr.h中用到的宏常数在这里定义
#define MAINLOOP_COUNT  (2200000)

//【3】指示灯端口及引脚定义—根据实际使用的引脚改动
//指示灯端口及引脚定义
#define  LIGHT_RED      (PTB_NUM|12)  //红灯
#define  LIGHT_GREEN    (PTB_NUM|13)  //绿灯
#define  LIGHT_BLUE     (PTB_NUM|14)  //蓝灯
//灯状态宏定义（灯亮、灯暗对应的物理电平由硬件接法决定）
#define  LIGHT_ON       0    //灯亮
#define  LIGHT_OFF      1    //灯暗

//【4】 其他模块引脚定义
#define UART_Debug   UART_1   //用于程序更新，无法被使用
#define UART_User    UART_0   //用户串口

//【5】中断服务函数宏定义，为了06、07文件夹可复用，这里注册中断服务函数
void UART0_IRQHandler(void) __attribute__((interrupt("WCH-Interrupt-fast")));
#define UART_User_Handler UART0_IRQHandler

void SysTick_Handler(void) __attribute__((interrupt("WCH-Interrupt-fast")));
#endif    //防止重复定义（USER_H 结尾）
