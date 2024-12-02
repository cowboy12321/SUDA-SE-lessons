//======================================================================
//�ļ����ƣ�user.h��userͷ�ļ���
//������λ:RISC-V(sumcu.suda.edu.cn)
//���¼�¼��20210426-20240202
//��Ҫ˵������1�������õ���ͷ�ļ���Ϊ�ﵽӦ�ó���Ŀ���ֲ�ԣ�����Ӳ��Ҫ��
//              оƬ�ĸ����ţ���Ҫ������궨�壬Ŀ����ʵ�ֶԾ���Ӳ������
//              ��̣������Ƕ�оƬ�����ű��
//          ��2���жϴ�����������ڴ˺궨�壬�Ա�isr.c����ֲ
//======================================================================

#ifndef __USER_H_
#define __USER_H_

//��1�����䶯���ļ�������������Ҫ����
#include "gpio.h"
#include "flash.h"
#include "uart.h"
#include "printf.h"
#include "gec.h"
#include "adc.h"
#include "pwm.h"
//��2�����䶯��Ϊ��<07_AppPrg>�ɸ��ã�main.c��isr.h���õ��ĺ곣�������ﶨ��
#define MAINLOOP_COUNT  (4000000)

//��3�����䶯��ָʾ�ƶ˿ڼ����Ŷ��塪����ʵ��ʹ�õ����ŸĶ�
//ָʾ�ƶ˿ڼ����Ŷ���

#define  LIGHT_RED      (PTB_NUM|12)  //���
#define  LIGHT_GREEN    (PTB_NUM|13)  //�̵�
#define  LIGHT_BLUE     (PTB_NUM|14)  //����
//��״̬�궨�壨�������ư���Ӧ�������ƽ��Ӳ���ӷ�������
#define  LIGHT_ON       0    //����
#define  LIGHT_OFF      1    //�ư�

//��4�����䶯��UART����ģ�鶨��
#define UART_Debug   UART_1   //���ڳ�����£��޷���ʹ��
#define UART_User    UART_0   //�û�����

//��5�����䶯��Ϊ��06��07�ļ��пɸ��ã�����ע���жϷ�����
void UART0_IRQHandler(void) __attribute__((interrupt("WCH-Interrupt-fast")));
#define UART_User_Handler UART0_IRQHandler


//��6��gec_lab
#define PTB PTB_NUM
#define PTA PTA_NUM
#define INPUT_ON       1    //�ߵ�ƽ
#define INPUT_OFF      0    //�͵�ƽ
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
