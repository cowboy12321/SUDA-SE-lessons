/*=====================================================================
//文件名称：user.inc
//功能概要：汇编工程用户头文件
//版本更新：20180810-20240216
//=====================================================================*/

/* 【1】调试串口函数宏定义及文件包含 */
.equ printf  , myprintf

/* 【2】 全局宏常数，为了可移植 */
.equ MAINLOOP_COUNT,600000     /*主循环次数设定值（常量）*/

/* 【3】指示灯端口及引脚定义—根据实际使用的引脚改动 */
    /*小灯使用的端口/引脚*/
    .equ PTA_NUM  ,  (0<<8)
    .equ PTB_NUM  ,  (1<<8)
    .equ LIGHT_BLUE  , (PTB_NUM|14)       
    .equ LIGHT_RED   , (PTB_NUM|12)
    .equ LIGHT_GREEN , (PTB_NUM|13)
    /*灯状态宏定义（灯亮、灯暗对应的物理电平由硬件接法决定）*/
    .equ LIGHT_ON  , 0                /*灯亮*/
    .equ LIGHT_OFF , 1                /*灯暗*/
    /*GPIO引脚方向宏定义*/
    .equ GPIO_INPUT   ,  (0)
    .equ GPIO_OUTPUT  ,  (1)
/*【4】其他外设模块硬件引脚定义 */
    .equ UART_User,(0)

/*【5】为了06、07文件夹可复用，这里注册中断服务例程 */
/*用户中断服务例程声明及其与缺省中断服务例程的关联*/
/*定义原中断处理程序名*/
.type  UART0_IRQHandler, function
.global UART0_IRQHandler
/* 定义新中断处理程序名 */
.type UART_User_Handler, function
.global UART_User_Handler
/*建立新旧中断处理程序名的映射*/
.equ UART0_IRQHandler,UART_User_Handler




