//======================================================================
//文件名称：gec.h（GEC芯片引脚头文件）
//制作单位：苏大arm技术中心(sumcu.suda.edu.cn)
//更新记录：2018.12-20200627-20240202
//======================================================================
#ifndef GEC_H    //防止重复定义（GEC_H 开始）
#define GEC_H
#include "mcu.h"
#include "printf.h"
#include "flash.h"
#include "Os_Self_API.h"

//【变动】GEC基本信息==================================================
#define GEC_USER_SECTOR_START    (12)   /*【0启动】2/2处，0-0启动，31-从BIOS启动*/
#define GEC_COMPONENT_LST_START  (10)   //构件库函数列表开始扇区号
// #define BIOS_SYSTICK_IRQn        (12+1)
// #define BIOS_SW_IRQn             (14+1)
// #define BIOS_UART_UPDATE_IRQn    (27+1)   //BIOS程序写入串口的中断号

//【变动】动态命令起始扇区、结束扇区
#define GEC_DYNAMIC_START        (8)
#define GEC_DYNAMIC_END	         (9)


//【固定】存储更新程序起始扇区、结束扇区
#define GEC_SAVE_USER_SECTOR_START    (32)
#define GEC_SAVE_USER_SECTOR_END      (47)

#define GEC_USERBASE         (MCU_FLASH_ADDR_START + GEC_USER_SECTOR_START*MCU_SECTORSIZE)

//【固定】构件库函数指针和系统功能函数声明===========================================
void **  component_fun;

void  Vectors_Init();
// extern void UART1_IRQHandler(void);
// extern void SW_Handler(void);
// extern void SysTick_Handler(void);
#endif  //防止重复定义（GEC_H 结尾）
