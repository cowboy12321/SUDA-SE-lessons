//=====================================================================
//�ļ����ƣ�pwm.c
//���ܸ�Ҫ��PWM�ײ���������Դ�����ļ�
//������λ���մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20230516-20231215
//оƬ���ͣ�MSPM0L1306
//=====================================================================
#include "pwm.h"
#include "printf.h"
//=====================================================================
// �������ƣ�pwm_init
// ���ܸ�Ҫ��pwm��ʼ������
// ����˵����pwmNo��pwmģ��ţ���.h�ĺ궨�����
//          clockFre��ʱ��Ƶ�ʣ���λ��hz ��ѡ��62745-16M
//          period�����ڣ���λΪ������������������������������.h�ļ���
//          duty��ռ�ձȣ�0.0~100.0��Ӧ0%~100%
//          align�����뷽ʽ ����������
//          pol�����ԣ���ͷ�ļ��궨���������PWM_PLUSΪ������
// �������أ�  ��
//ʹ��˵������1��ʱ��Ƶ�ʺ�����������pwm���õģ�ֻ��ռ�ձȺͼ�����ÿ��ͨ������
//              ���õ�
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
    //��������
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
// �������ƣ�pwm_update
// ���ܸ�Ҫ��PWM����
// ����˵����pwmNo��pwmģ��ţ���.h�ĺ궨����� 
//          duty��ռ�ձȣ�0.0~100.0��Ӧ0%~100%
// �������أ���
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
// �������ƣ�pwm_stop
// ���ܸ�Ҫ��PWMֹͣ
// ����˵����pwmNo��pwmģ��ţ���.h�ĺ궨����� 
// �������أ���
//=====================================================================
void pwm_stop(uint16_t pwmNo){
    R8_PWM_OUT_EN&= ~pwmNo;
}
//------����Ϊ�ڲ�����------

