//=====================================================================
// 文件名称：isr.c（中断处理程序源文件）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 更新记录：20240614
// 功能描述：提供中断处理程序编程框架
// 移植规则：【固定】本工程07_AppPrg整个文件夹具备芯片无关性，差异体现在
//           05_UserBoard文件夹的User.h中
//=====================================================================
#include "includes.h"

//函数声明
void UART_User_Handler(void);

//======================================================================
// 程序名称：UART_User_Handler
// 触发条件：UART_User串口收到一个字节触发
// 备   注：进入本程序后，可使用uart_get_re_int函数可再进行中断标志判断
//          （1-有UART接收中断，0-没有UART接收中断）
//======================================================================
void UART_User_Handler(void)
{
    //【1】关中断
    DISABLE_INTERRUPTS;

    //【2】声明临时变量
    uint8_t flag,ch;

    //【3】判断是否为本中断触发
    if (!uart_get_re_int(UART_User)) goto UART_User_Handler_exit;

    //【4】确证是本中断触发，读取接到的字节赋给变量ch，flag是收到数据标志
    ch=uart_re1(UART_User,&flag);   //调用接收一个字节的函数，清接收中断位

    //【5】根据flag判断是否真正收到一个字节的数据
    if (flag)                       //有数据
    {
        uart_send1(UART_User,ch);  //回发接收到的字节
    }

    // 【6】开中断
UART_User_Handler_exit:
    ENABLE_INTERRUPTS;
}

