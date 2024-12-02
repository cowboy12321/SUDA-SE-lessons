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
#include "gec.h"
#include "gpio.h"
#include "flash.h"
#include "uart.h"
#include "printf.h"
#include "SysTick.h"
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

void SysTick_Handler(void) __attribute__((interrupt("WCH-Interrupt-fast")));
#endif    //��ֹ�ظ����壨USER_H ��β��
