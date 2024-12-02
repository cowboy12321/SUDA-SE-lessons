//======================================================================
//�ļ����ƣ�cpu.h��cpuͷ�ļ���
//������λ��RISC-V(sumcu.suda.edu.cn)
//���¼�¼��20210426
//======================================================================

#ifndef __CPU_H_
#define __CPU_H_

#include "stdint.h"
#include "core_riscv.h"
#include "CH573SFR.h"


//��3�����䶯����λ���
// ������λ
#define IS_PIN_RESET_OCCURED    ((R8_RESET_STATUS & RB_RESET_FLAG)== RST_FLAG_MR)
// �ϵ縴λ
#define IS_POWERON_RESET          ((R8_RESET_STATUS & RB_RESET_FLAG)== RST_FLAG_RPOR)
//д1�����Ÿ�λ��־λ
#define CLEAR_PIN_RESET_FLAG      (R8_GLOB_CFG_INFO |= RB_CFG_RESET_EN)  //�����⣬�����

//��3�����̶����жϺ궨��,����RISC-V�ܹ����򲻱䶯
//��3�������ں˶����ġ��жϺ궨��
#define ENABLE_INTERRUPTS       ({ __asm("li t0, 0x8"); __asm("csrs mstatus, t0"); })// �����ж�()
#define DISABLE_INTERRUPTS      ({ __asm("li t0, 0x8"); __asm("csrc mstatus, t0"); })// �����ж�()

//��4�����̶������Ż����ͱ����궨��
typedef volatile uint8_t      vuint8_t;   // ���Ż��޷���8λ�����ֽ�
typedef volatile uint16_t     vuint16_t;  // ���Ż��޷���16λ������
typedef volatile uint32_t     vuint32_t;  // ���Ż��޷���32λ��������
typedef volatile int8_t       vint_8;     // ���Ż��з���8λ��
typedef volatile int16_t      vint_16;    // ���Ż��з���16λ��
typedef volatile int16_t      vint_32;    // ���Ż��з���32λ��
//��5�����̶���λ�����꺯������λ����λ����üĴ���һλ��״̬��
#define BSET(bit,Register)  ((Register)|= (1<<(bit)))    //�üĴ�����һλ
#define BCLR(bit,Register)  ((Register) &= ~(1<<(bit)))  //��Ĵ�����һλ
#define BGET(bit,Register)  (((Register) >> (bit)) & 1)  //��üĴ���һλ��״̬

#endif /* 02_CPU_CPU_H_ */
