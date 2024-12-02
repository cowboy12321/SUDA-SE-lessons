//=====================================================================
//�ļ����ƣ�pwm.c
//���ܸ�Ҫ��PWM�ײ���������Դ�����ļ�
//������λ���մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20240516-20240829
//оƬ���ͣ�CH573
//=====================================================================
#include "pwm.h"

//=====================================================================
// �������ƣ�pwm_init
// ���ܸ�Ҫ��pwm��ʼ������
// ����˵����pwmNo��ͨ���ţ�ʹ��.h�ļ��еĺ곣��
//          clockFre��ʱ��Ƶ�ʣ���λ��hz
//                    ��ʱ��PWMѡ2-60M��ר��PWMѡ��235294-60M
//          period�����ڣ���λΪ������������������������
//                  ��ʱ��PWM����ѡ������ʱ��Ƶ�ʵ��κ���������
//                  ר��PWM��ֻ��ȡ31��32��63��64��127��128��255��256
//          duty��ռ�ձȣ�0.0-100.0��Ӧ0%-100%
//          align�����뷽ʽ ���������ޣ�ȡ0
//          pol�����ԣ���ͷ�ļ��궨���������PWM_PLUSΪ������
//�������أ���
//ʹ��˵����ʹ��ʱע�⣬ר��PWM��ʱ��Ƶ�ʺ�����������ͨ�����õģ�
//         һ��ͨ�����ܶ������ã���ռ�ձȺͼ��Կ��Զ�������
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
        
        //��������
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
// �������ƣ�pwm_update
// ���ܸ�Ҫ��PWM����
// ����˵����pwmNo��ͨ���ţ�ʹ��.h�ļ��еĺ곣��
//          duty��ռ�ձȣ�0.0-100.0��Ӧ0%-100%
// �������أ���
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
// �������ƣ�pwm_stop
// ���ܸ�Ҫ��PWMֹͣ
// ����˵����pwmNo��ͨ���ţ�ʹ��.h�ļ��еĺ곣��
// �������أ���
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
//------����Ϊ�ڲ�����------

