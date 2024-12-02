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
#include "rf.h"

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

#endif /* __USER_H_ */
