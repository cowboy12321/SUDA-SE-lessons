//=====================================================================
// 文件名称：isr.c（中断处理程序源文件）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 版本更新：20230915
// 功能描述：提供中断处理程序编程框架
// 移植规则：【固定】
//=====================================================================
#include "includes.h"

//======================================================================
//程序名称：UART_User_Handler
//触发条件：UART_User串口收到一个字节触发
//备    注：进入本程序后，可使用uart_get_re_int函数可再进行中断标志判断
//          （1-有UART接收中断，0-没有UART接收中断）
//======================================================================
void UART_User_Handler(void)
{
    //（1）内部变量声明
    uint8_t flag,ch;
    DISABLE_INTERRUPTS;      //关总中断
    //（2）未触发串口接收中断，退出
    if(!uart_get_re_int(UART_User)) goto UART_User_Handler_EXIT;
    //（3）收到一个字节，读出该字节数据
    ch = uart_re1(UART_User,&flag);         //调用接收一个字节的函数
    if(!flag) goto UART_User_Handler_EXIT;   //实际未收到数据，退出
    //（4）已经收到一个字节，在局部变量ch中
    uart_send1(UART_User,ch);               //回发一个字节
    //（5）【公共退出区】
UART_User_Handler_EXIT:
    ENABLE_INTERRUPTS;//开总中断
}


/*
 知识要素：
 1.本文件中的中断处理函数调用的均是相关设备封装好的具体构件，在更换芯片
 时，只需保证设备的构件接口一致，即可保证本文件的相关中断处理函数不做任何
 更改，从而达到芯片无关性的要求。
 */