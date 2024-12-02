//=====================================================================
//文件名称：pwm.h
//功能概要：PWM底层驱动构件头文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240105
//芯片类型：MSPM0L1306
//=====================================================================

#ifndef PWM_H
#define PWM_H

//包含芯片头文件
#include "CH573SFR.h"
#include "gpio.h"

//宏定义

//PWM对齐方式宏定义:边沿对齐、中心对齐
#define PWM_EDGE   0
#define PWM_CENTER 1
//PWM极性选择宏定义：正极性、负极性
#define PWM_PLUS   0
#define PWM_MINUS  1
//PWM通道号
#define CH_PWM4     0x01  // PWM4 通道
#define CH_PWM5     0x02  // PWM5 通道
#define CH_PWM6     0x04  // PWM6 通道
#define CH_PWM7     0x08  // PWM7 通道
#define CH_PWM8     0x10  // PWM8 通道
#define CH_PWM9     0x20  // PWM9 通道
#define CH_PWM10    0x40  // PWM10 通道
#define CH_PWM11    0x80  // PWM11 通道
#define  PWM_PIN0  CH_PWM4     //PTA12
#define  PWM_PIN1  CH_PWM5     //PTA13
#define  PWM_PIN2  CH_PWM6     //PB0
#define  PWM_PIN3  CH_PWM7     //PB4
#define  PWM_PIN4  CH_PWM8     //PB6
#define  PWM_PIN5  CH_PWM9     //PB7
#define  PWM_PIN6  CH_PWM10    //PB14
#define  PWM_PIN7  CH_PWM11    //PB23

#define  PWMX_Cycle_256 256 // 256 个PWMX周期
#define  PWMX_Cycle_255 255 // 255 个PWMX周期
#define  PWMX_Cycle_128 128 // 128 个PWMX周期
#define  PWMX_Cycle_127 127 // 127 个PWMX周期 
#define  PWMX_Cycle_64  64  // 64 个PWMX周期
#define  PWMX_Cycle_63  63  // 63 个PWMX周期
#define  PWMX_Cycle_32  32  // 32 个PWMX周期
#define  PWMX_Cycle_31  31  // 31 个PWMX周期

//=====================================================================
// 函数名称：pwm_init
// 功能概要：pwm初始化函数
// 参数说明：pwmNo：pwm模块号，在.h的宏定义给出
//          clockFre：时钟频率，单位：hz 可选：62745-16M
//          period：周期，单位为个数，即计数器跳动次数，定义在.h文件中
//          duty：占空比：0.0~100.0对应0%~100%
//          align：对齐方式 ，本构件无
//          pol：极性，在头文件宏定义给出，如PWM_PLUS为正极性
// 函数返回：  无
//使用说明：（1）时钟频率和周期是所有pwm共用的，只有占空比和极性是每个通道单独
//              设置的
//=====================================================================
void pwm_init(uint16_t pwmNo,uint32_t clockFre,uint16_t period,
               double  duty,uint8_t align, uint8_t pol);

//=====================================================================
// 函数名称：pwm_update
// 功能概要：PWM更新
// 参数说明：pwmNo：pwm模块号，在.h的宏定义给出 
//          duty：占空比：0.0~100.0对应0%~100%
// 函数返回：无
//=====================================================================
void pwm_update(uint16_t pwmNo,double duty);

//=====================================================================
// 函数名称：pwm_stop
// 功能概要：PWM停止
// 参数说明：pwmNo：pwm模块号，在.h的宏定义给出 
// 函数返回：无
//=====================================================================
void pwm_stop(uint16_t pwmNo);
#endif