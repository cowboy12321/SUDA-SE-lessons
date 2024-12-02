/*======================================================================
/*文件名称：gpio.inc
/*功能概要：D1 GPIO底层驱动构件（汇编）程序头文件
/*版权所有：苏州大学嵌入式与物联网研究中心(sumcu.suda.edu.cn)
/*更新记录：2019-09-27 V2.0
/*======================================================================*/

/*端口号地址偏移量宏定义*/
.equ PTA_NUM  ,  (0<<8)
.equ PTB_NUM  ,  (1<<8)

/* GPIO引脚方向宏定义 */
.equ GPIO_INPUT    ,  (0)
.equ GPIO_OUTPUT   ,  (1)

/* GPIO引脚中断类型宏定义 */
.equ RISING_EDGE  ,  (1)     /* 上升沿触发 */
.equ FALLING_EDGE ,  (2)     /* 下降沿触发 */
.equ LowLevel     ,  (3)     /* 低电平触发 */
.equ HighLevel    ,  (4)     /* 高电平触发 */

/* GPIO引脚拉高低状态宏定义 */
.equ PULL_UP      ,  (1)   /* 拉高 */
.equ PULL_DOWN    ,  (0)   /* 拉低 */

/*GPIO寄存器基地址宏定义*/
.equ R16_PIN_ALTERNATE      ,  0x40001018
.equ R16_PIN_ANALOG_IE      ,  0x4000101A
.equ R16_PA_INT_EN          ,  0x40001090
.equ R16_PB_INT_EN          ,  0x40001092
.equ R16_PA_INT_MODE        ,  0x40001094
.equ R16_PB_INT_MODE        ,  0x40001096
.equ R16_PA_INT_IF          ,  0x4000109C
.equ R16_PB_INT_IF          ,  0x4000109E
.equ R32_PA_DIR             ,  0x400010A0
.equ R32_PA_PIN             ,  0x400010A4
.equ R32_PA_OUT             ,  0x400010A8
.equ R32_PA_CLR             ,  0x400010AC
.equ R32_PA_PU              ,  0x400010B0
.equ R32_PA_PD_DRV          ,  0x400010B4
.equ R32_PB_DIR             ,  0x400010C0
.equ R32_PB_PIN             ,  0x400010C4
.equ R32_PB_OUT             ,  0x400010C8
.equ R32_PB_CLR             ,  0x400010CC
.equ R32_PB_PU              ,  0x400010D0
.equ R32_PB_PD_DRV          ,  0x400010D4
