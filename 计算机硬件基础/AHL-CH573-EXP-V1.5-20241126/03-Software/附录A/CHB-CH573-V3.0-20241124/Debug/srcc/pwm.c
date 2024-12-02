//=====================================================================
//文件名称：pwm.c
//功能概要：PWM底层驱动构件源程序文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20230516-20231215
//芯片类型：MSPM0L1306
//=====================================================================
#include "pwm.h"
#include "printf.h"
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
uint16_t gc_period_minus_one=255;
void pwm_init(uint16_t pwmNo, uint32_t clockFre, uint16_t period, 
               double duty, uint8_t align, uint8_t pol)
{
    R8_PWM_CLOCK_DIV=16000000/clockFre;
    switch (pwmNo)
    {
    case PWM_PIN0: gpio_init(PTA_NUM|12, GPIO_OUTPUT,1);break; // PA12 - PWM4
    case PWM_PIN1: gpio_init(PTA_NUM|13, GPIO_OUTPUT,1);break; // PA13 - PWM5
    case PWM_PIN2: gpio_init(PTB_NUM|0,  GPIO_OUTPUT,1);break;  // PB0 - PWM6
    case PWM_PIN3: gpio_init(PTB_NUM|4,  GPIO_OUTPUT,1);break;  // PB4 - PWM7
    case PWM_PIN4: gpio_init(PTB_NUM|6,  GPIO_OUTPUT,1);break;  // PB6 - PWM8
    case PWM_PIN5: gpio_init(PTB_NUM|7,  GPIO_OUTPUT,1);break;  // PB7 - PWM9
    case PWM_PIN6: gpio_init(PTB_NUM|14, GPIO_OUTPUT,1);break; // PB14 - PWM10
    case PWM_PIN7: gpio_init(PTB_NUM|23, GPIO_OUTPUT,1);break; // PB23 - PWM11
    default:
        break;
    }
    //设置周期
    switch(period)
    {
        case PWMX_Cycle_256:
            R8_PWM_CONFIG = R8_PWM_CONFIG & 0xf0;
            gc_period_minus_one=255;
            break;

        case PWMX_Cycle_255:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | 0x01;
            gc_period_minus_one=254;
            break;

        case PWMX_Cycle_128:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | (1 << 2);
            gc_period_minus_one=127;
            break;

        case PWMX_Cycle_127:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | (1 << 2) | 0x01;
            gc_period_minus_one=126;
            break;

        case PWMX_Cycle_64:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | (2 << 2);
            gc_period_minus_one=63;
            break;

        case PWMX_Cycle_63:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | (2 << 2) | 0x01;
            gc_period_minus_one=62;
            break;

        case PWMX_Cycle_32:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | (3 << 2);
            gc_period_minus_one=31;
            break;

        case PWMX_Cycle_31:
            R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf0) | (3 << 2) | 0x01;
            gc_period_minus_one=30;
            break;

        default:
            break;
    }
    

    (pol==PWM_PLUS) ? (R8_PWM_POLAR |= (pwmNo)) : (R8_PWM_POLAR &= ~(pwmNo));
    for(uint8_t i = 0; i < 8; i++)
    {
        if((pwmNo >> i) & 1)
        {
            *((volatile uint8_t *)((&R8_PWM4_DATA) + i)) =(uint8_t)( gc_period_minus_one*duty/100);
        }
    }
    R8_PWM_OUT_EN |= pwmNo;
}
//=====================================================================
// 函数名称：pwm_update
// 功能概要：PWM更新
// 参数说明：pwmNo：pwm模块号，在.h的宏定义给出 
//          duty：占空比：0.0~100.0对应0%~100%
// 函数返回：无
//=====================================================================
void pwm_update(uint16_t pwmNo, double duty)
{
    for(uint8_t i = 0; i < 8; i++)
    {
        if((pwmNo >> i) & 1)
        {
            *((volatile uint8_t *)((&R8_PWM4_DATA) + i)) =(uint8_t)( gc_period_minus_one*duty/100);
        }
    }
}
//=====================================================================
// 函数名称：pwm_stop
// 功能概要：PWM停止
// 参数说明：pwmNo：pwm模块号，在.h的宏定义给出 
// 函数返回：无
//=====================================================================
void pwm_stop(uint16_t pwmNo){
    R8_PWM_OUT_EN&= ~pwmNo;
}
//------以下为内部函数------

