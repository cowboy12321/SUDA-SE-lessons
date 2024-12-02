//======================================================================
//文件名称：cpu.h（cpu头文件）
//制作单位：RISC-V(sumcu.suda.edu.cn)
//更新记录：20210426
//======================================================================

#ifndef __CPU_H_
#define __CPU_H_

#include "stdint.h"
#include "core_riscv.h"
#include "CH573SFR.h"


//（3）【变动】复位相关
// 按键复位
#define IS_PIN_RESET_OCCURED    ((R8_RESET_STATUS & RB_RESET_FLAG)== RST_FLAG_MR)
// 上电复位
#define IS_POWERON_RESET          ((R8_RESET_STATUS & RB_RESET_FLAG)== RST_FLAG_RPOR)
//写1清引脚复位标志位
#define CLEAR_PIN_RESET_FLAG      (R8_GLOB_CFG_INFO |= RB_CFG_RESET_EN)  //有问题，待解决

//（3）【固定】中断宏定义,若是RISC-V架构，则不变动
//（3）【因内核而更改】中断宏定义
#define ENABLE_INTERRUPTS       ({ __asm("li t0, 0x8"); __asm("csrs mstatus, t0"); })// 开总中断()
#define DISABLE_INTERRUPTS      ({ __asm("li t0, 0x8"); __asm("csrc mstatus, t0"); })// 关总中断()

//（4）【固定】不优化类型别名宏定义
typedef volatile uint8_t      vuint8_t;   // 不优化无符号8位数，字节
typedef volatile uint16_t     vuint16_t;  // 不优化无符号16位数，字
typedef volatile uint32_t     vuint32_t;  // 不优化无符号32位数，长字
typedef volatile int8_t       vint_8;     // 不优化有符号8位数
typedef volatile int16_t      vint_16;    // 不优化有符号16位数
typedef volatile int16_t      vint_32;    // 不优化有符号32位数
//（5）【固定】位操作宏函数（置位、清位、获得寄存器一位的状态）
#define BSET(bit,Register)  ((Register)|= (1<<(bit)))    //置寄存器的一位
#define BCLR(bit,Register)  ((Register) &= ~(1<<(bit)))  //清寄存器的一位
#define BGET(bit,Register)  (((Register) >> (bit)) & 1)  //获得寄存器一位的状态

#endif /* 02_CPU_CPU_H_ */
