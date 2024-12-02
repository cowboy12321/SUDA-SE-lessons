//=====================================================================
//文件名称：pwm.h
//功能概要：PWM底层驱动构件头文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240516-20240829
//芯片类型：CH573
//=====================================================================

#ifndef PWM_H
#define PWM_H

//包含芯片头文件
#include "CH573SFR.h"
#include "gpio.h"

//宏定义

//PWM对齐方式宏定义:边沿对齐、中心对齐
#define PWM_EDGE    0
#define PWM_CENTER  1
//PWM极性选择宏定义：正极性、负极性
#define PWM_PLUS    0
#define PWM_MINUS   1

//PWM通道号
//定时器PWM引脚
#define PWM_TIM0  0x100       //PTA9  
#define PWM_TIM1  0x200       //PTB10
#define PWM_TIM2  0x400       //PTB11
#define PWM_TIM3  0x800       //PTB22
//专用PWM引脚
#define PWM_PIN0  CH_PWM4     //PTA12
#define PWM_PIN1  CH_PWM5     //PTA13
#define PWM_PIN2  CH_PWM7     //PB4
#define PWM_PIN3  CH_PWM9     //PB7
#define PWM_PIN4  CH_PWM10    //PB14
#define PWM_PIN5  CH_PWM11    //PB23

//=====================================================================
// 函数名称：pwm_init
// 功能概要：pwm初始化函数
// 参数说明：pwmNo：通道号，使用.h文件中的宏常数
//          clockFre：时钟频率，单位：hz
//                    定时器PWM选2-60M；专用PWM选：235294-60M
//          period：周期，单位为个数，即计数器跳动次数，
//                  定时器PWM：可选不大于时钟频率的任何正整数；
//                  专用PWM：只能取31、32、63、64、127、128、255、256
//          duty：占空比：0.0-100.0对应0%-100%
//          align：对齐方式 ，本构件无，取0
//          pol：极性，在头文件宏定义给出，如PWM_PLUS为正极性
//函数返回：无
//使用说明：使用时注意，专用PWM的时钟频率和周期是所有通道共用的，
//         一个通道不能独立设置，但占空比和极性可以独立设置
//=====================================================================
void pwm_init(uint16_t pwmNo,uint32_t clockFre,uint32_t period,
               double  duty,uint8_t align, uint8_t pol);

//=====================================================================
// 函数名称：pwm_update
// 功能概要：PWM更新
// 参数说明：pwmNo：通道号，使用.h文件中的宏常数
//          duty：占空比：0.0-100.0对应0%-100%
// 函数返回：无
//=====================================================================
void pwm_update(uint16_t pwmNo,double duty);

//=====================================================================
// 函数名称：pwm_stop
// 功能概要：PWM停止
// 参数说明：pwmNo：通道号，使用.h文件中的宏常数
// 函数返回：无
//=====================================================================
void pwm_stop(uint16_t pwmNo);

#define CH_PWM4   0x01  // PWM4 通道
#define CH_PWM5   0x02  // PWM5 通道
#define CH_PWM6   0x04  // PWM6 通道
#define CH_PWM7   0x08  // PWM7 通道
#define CH_PWM8   0x10  // PWM8 通道
#define CH_PWM9   0x20  // PWM9 通道
#define CH_PWM10  0x40  // PWM10 通道
#define CH_PWM11  0x80  // PWM11 通道

#endif