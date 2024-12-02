//======================================================================
//�ļ����ƣ�user.h��userͷ�ļ���
//������λ���մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//���¼�¼��20181201-20240802
//��Ҫ˵������1�������õ���ͷ�ļ���Ϊ�ﵽӦ�ó���Ŀ���ֲ�ԣ�����Ӳ��Ҫ��
//              оƬ�ĸ����ţ���Ҫ������궨�壬Ŀ����ʵ�ֶԾ���Ӳ������
//              ��̣������Ƕ�оƬ�����ű��
//          ��2���жϴ�����������ڴ˺궨�壬�Ա�isr.c����ֲ
//======================================================================
#ifndef USER_H   //��ֹ�ظ����壨USER_H ��ͷ��
#define USER_H

//��1���ļ�������������Ҫ����
#include "Os_United_API.h"
#include "Os_Self_API.h"
#include "gec.h"
#include "gpio.h"
#include "flash.h"
#include "uart.h"
#include "printf.h"
#include "adc.h"

//��2��Ϊ��<07_AppPrg>�ɸ��ã�main.c��isr.h���õ��ĺ곣�������ﶨ��
#define MAINLOOP_COUNT  (2200000)

//��3��ָʾ�ƶ˿ڼ����Ŷ��塪����ʵ��ʹ�õ����ŸĶ�
//ָʾ�ƶ˿ڼ����Ŷ���
#define  LIGHT_RED      (PTB_NUM|12)  //���
#define  LIGHT_GREEN    (PTB_NUM|13)  //�̵�
#define  LIGHT_BLUE     (PTB_NUM|14)  //����
//��״̬�궨�壨�������ư���Ӧ�������ƽ��Ӳ���ӷ�������
#define  LIGHT_ON       0    //����
#define  LIGHT_OFF      1    //�ư�

//��4�� ����ģ�����Ŷ���
#define UART_Debug   UART_1   //���ڳ�����£��޷���ʹ��
#define UART_User    UART_0   //�û�����

//��5���жϷ������궨�壬Ϊ��06��07�ļ��пɸ��ã�����ע���жϷ�����
void UART0_IRQHandler(void) __attribute__((interrupt("WCH-Interrupt-fast")));
#define UART_User_Handler UART0_IRQHandler

// ���ͨ��
#define SYS_FREQ 60000000
#define DIFF_CHANNEL ADC_CHANNEL_DIFF_0
static char diffName[] = "���ͨ��0";
static char diffPinName1[] = "PA12";
static char diffPinName2[] = "PA4";
#define diff_ad_to_voltage(x) (x*8.4/4096-4.2)
// ��ͨ������
#define SINGLE_CHANNEL ADC_CHANNEL_4
static char singleName[] = "ͨ��4";
static char singlePinName[] = "PA14";
#define single_ad_to_voltage(x) (x*3.3/4096)
// �¶Ȳ���
#define TEMPSENSOR_CHANNEL ADC_CHANNEL_TEMPSENSOR
#endif    //��ֹ�ظ����壨USER_H ��β��
