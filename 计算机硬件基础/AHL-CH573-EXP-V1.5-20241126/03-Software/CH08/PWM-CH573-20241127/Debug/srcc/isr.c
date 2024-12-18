//=====================================================================
// 文件名称：isr.c（中断处理程序源文件）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 更新记录：20241015
// 功能描述：提供中断处理程序编程框架
// 移植规则：【固定】本工程07_AppPrg整个文件夹具备芯片无关性，差异体现在
//           05_UserBoard文件夹的User.h中
//=====================================================================
#include "includes.h"





/*
 知识要素：
 1.本文件中的中断处理函数调用的均是相关设备封装好的具体构件，在更换芯片
 时，只需保证设备的构件接口一致，即可保证本文件的相关中断处理函数不做任何
 更改，从而达到芯片无关性的要求。
 */