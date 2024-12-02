//=====================================================================
//文件名称：pwm.c
//功能概要：PWM底层驱动构件源程序文件
//制作单位：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20240516-20240829
//芯片类型：CH573
//=====================================================================
#include "pwm.h"

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
uint16_t gc_period_minus_one=255;
void pwm_init(uint16_t pwmNo, uint32_t clockFre, uint32_t period, 
               double duty, uint8_t align, uint8_t pol)
{
    if(pwmNo&0xff){
        R8_PWM_CLOCK_DIV=60000000/clockFre;
        if(pwmNo&CH_PWM4) gpio_init(PTA_NUM|12, GPIO_OUTPUT,1); // PA12 - PWM4
        if(pwmNo&CH_PWM5) gpio_init(PTA_NUM|13, GPIO_OUTPUT,1); // PA13 - PWM5
        if(pwmNo&CH_PWM6) gpio_init(PTB_NUM|0,  GPIO_OUTPUT,1); // PB0 - PWM6
        if(pwmNo&CH_PWM7) gpio_init(PTB_NUM|4,  GPIO_OUTPUT,1); // PB4 - PWM7
        if(pwmNo&CH_PWM8) gpio_init(PTB_NUM|6,  GPIO_OUTPUT,1); // PB6 - PWM8
        if(pwmNo&CH_PWM9) gpio_init(PTB_NUM|7,  GPIO_OUTPUT,1); // PB7 - PWM9
        if(pwmNo&CH_PWM10) gpio_init(PTB_NUM|14, GPIO_OUTPUT,1); // PB14 - PWM10
        if(pwmNo&CH_PWM11) gpio_init(PTB_NUM|23, GPIO_OUTPUT,1); // PB23 - PWM11
        
        //设置周期
        switch(period)
        {
            case 256:
                R8_PWM_CONFIG = R8_PWM_CONFIG & 0xf2;
                gc_period_minus_one=255;
                break;

            case 255:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | 0x01;
                gc_period_minus_one=254;
                break;

            case 128:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | (1 << 2);
                gc_period_minus_one=127;
                break;

            case 127:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | (1 << 2) | 0x01;
                gc_period_minus_one=126;
                break;

            case 64:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | (2 << 2);
                gc_period_minus_one=63;
                break;

            case 63:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | (2 << 2) | 0x01;
                gc_period_minus_one=62;
                break;

            case 32:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | (3 << 2);
                gc_period_minus_one=31;
                break;

            case 31:
                R8_PWM_CONFIG = (R8_PWM_CONFIG & 0xf2) | (3 << 2) | 0x01;
                gc_period_minus_one=30;
                break;

            default:
                break;
        }
        
        (pol==PWM_PLUS) ? (R8_PWM_POLAR &= ~(pwmNo)):(R8_PWM_POLAR |= (pwmNo)) ;
        for(uint8_t i = 0; i < 8; i++)
        {
            if((pwmNo >> i) & 1)
            {
                *((volatile uint8_t *)((&R8_PWM4_DATA) + i)) =(uint8_t)( gc_period_minus_one*duty/100);
            }
        }
        R8_PWM_OUT_EN |= pwmNo;
    }else if(pwmNo & 0xf00){
        if(pwmNo&PWM_TIM0){
            R32_TMR0_CNT_END = period * 60000000ll / clockFre;
            R8_TMR0_CTRL_MOD = RB_TMR_ALL_CLEAR;
            R32_TMR0_FIFO = R32_TMR0_CNT_END * duty / 100;
            R8_TMR0_CTRL_MOD = RB_TMR_COUNT_EN | RB_TMR_OUT_EN | (1 << 4) | (0B00 << 6);
            R32_PA_PD_DRV &= ~9;
            R32_PA_DIR |= 9;
        }
        if(pwmNo&PWM_TIM1){
            R32_PB_PD_DRV &= ~(1<<10);
            R32_PB_DIR |= (1<<10);
            R16_PIN_ALTERNATE |= RB_PIN_TMR1;

            R8_TMR1_CTRL_MOD = RB_TMR_ALL_CLEAR;
            R32_TMR1_CNT_END = period * 60000000ll / clockFre;
            R8_TMR1_CTRL_MOD = RB_TMR_COUNT_EN | RB_TMR_OUT_EN | (pol << 4) | (0B11 << 6);
            R32_TMR1_FIFO =    R32_TMR1_CNT_END * duty / 100;
        }
        if(pwmNo&PWM_TIM2){
            R32_PB_PD_DRV &= ~(1<<11);
            R32_PB_DIR |= (1<<11);
            R16_PIN_ALTERNATE |= RB_PIN_TMR2;

            R8_TMR2_CTRL_MOD = RB_TMR_ALL_CLEAR;
            R32_TMR2_CNT_END = period * 60000000ll / clockFre;
            R8_TMR2_CTRL_MOD = RB_TMR_COUNT_EN | RB_TMR_OUT_EN | (1 << 4) | (0B11 << 6);
            R32_TMR2_FIFO =    R32_TMR2_CNT_END * duty / 100;
            
        }
        if(pwmNo&PWM_TIM3){            
            R8_TMR2_CTRL_MOD = RB_TMR_ALL_CLEAR;
            R32_TMR2_CNT_END = period * 60000000ll / clockFre;
            R8_TMR2_CTRL_MOD = RB_TMR_COUNT_EN | RB_TMR_OUT_EN | (1 << 4) | (0B11 << 6);
            R32_TMR2_FIFO =    R32_TMR3_CNT_END * duty / 100;
            
            R32_PB_PD_DRV &= ~22;
            R32_PB_DIR |= 22;
        }
    }
}

//=====================================================================
// 函数名称：pwm_update
// 功能概要：PWM更新
// 参数说明：pwmNo：通道号，使用.h文件中的宏常数
//          duty：占空比：0.0-100.0对应0%-100%
// 函数返回：无
//=====================================================================
void pwm_update(uint16_t pwmNo, double duty)
{
    if(pwmNo&0xff){
        for(uint8_t i = 0; i < 8; i++)
        {
            if((pwmNo >> i) & 1)
            {
                *((volatile uint8_t *)((&R8_PWM4_DATA) + i)) =(uint8_t)( gc_period_minus_one*duty/100);
            }
        }
    }else if(pwmNo & 0xf00){
        if(pwmNo&PWM_TIM0) R32_TMR0_FIFO = R32_TMR0_CNT_END * duty / 100;
        if(pwmNo&PWM_TIM1) R32_TMR1_FIFO = R32_TMR1_CNT_END * duty / 100;
        if(pwmNo&PWM_TIM2) R32_TMR2_FIFO = R32_TMR2_CNT_END * duty / 100;
        if(pwmNo&PWM_TIM3) R32_TMR2_FIFO = R32_TMR3_CNT_END * duty / 100;
    }
}

//=====================================================================
// 函数名称：pwm_stop
// 功能概要：PWM停止
// 参数说明：pwmNo：通道号，使用.h文件中的宏常数
// 函数返回：无
//=====================================================================
void pwm_stop(uint16_t pwmNo){
    
    if(pwmNo&0xff){
        R8_PWM_OUT_EN&= ~pwmNo;
    }else if(pwmNo & 0xf00){
        if(pwmNo&PWM_TIM0) R8_TMR0_CTRL_MOD &= ~RB_TMR_COUNT_EN;
        if(pwmNo&PWM_TIM1) R8_TMR1_CTRL_MOD &= ~RB_TMR_COUNT_EN;
        if(pwmNo&PWM_TIM2) R8_TMR2_CTRL_MOD &= ~RB_TMR_COUNT_EN;
        if(pwmNo&PWM_TIM3) R8_TMR3_CTRL_MOD &= ~RB_TMR_COUNT_EN;
    }
}
//------以下为内部函数------

