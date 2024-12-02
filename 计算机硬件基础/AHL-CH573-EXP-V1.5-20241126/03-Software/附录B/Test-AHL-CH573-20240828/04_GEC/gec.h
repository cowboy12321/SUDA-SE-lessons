//======================================================================
//�ļ����ƣ�gec.h��GECоƬ����ͷ�ļ���
//������λ���մ�arm��������(sumcu.suda.edu.cn)
//���¼�¼��2018.12-20200627-20240202
//======================================================================
#ifndef GEC_H    //��ֹ�ظ����壨GEC_H ��ʼ��
#define GEC_H
#include "mcu.h"
#include "printf.h"
#include "flash.h"
#include "Os_Self_API.h"

//���䶯��GEC������Ϣ==================================================
#define GEC_USER_SECTOR_START    (12)   /*��0������2/2����0-0������31-��BIOS����*/
#define GEC_COMPONENT_LST_START  (10)   //�����⺯���б�ʼ������
// #define BIOS_SYSTICK_IRQn        (12+1)
// #define BIOS_SW_IRQn             (14+1)
// #define BIOS_UART_UPDATE_IRQn    (27+1)   //BIOS����д�봮�ڵ��жϺ�

//���䶯����̬������ʼ��������������
#define GEC_DYNAMIC_START        (8)
#define GEC_DYNAMIC_END	         (9)


//���̶����洢���³�����ʼ��������������
#define GEC_SAVE_USER_SECTOR_START    (32)
#define GEC_SAVE_USER_SECTOR_END      (47)

#define GEC_USERBASE         (MCU_FLASH_ADDR_START + GEC_USER_SECTOR_START*MCU_SECTORSIZE)

//���̶��������⺯��ָ���ϵͳ���ܺ�������===========================================
void **  component_fun;

void  Vectors_Init();
// extern void UART1_IRQHandler(void);
// extern void SW_Handler(void);
// extern void SysTick_Handler(void);
#endif  //��ֹ�ظ����壨GEC_H ��β��
