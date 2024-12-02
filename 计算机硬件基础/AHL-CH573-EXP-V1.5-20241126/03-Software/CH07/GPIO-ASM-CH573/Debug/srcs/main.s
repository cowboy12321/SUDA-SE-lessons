/*=====================================================================
//文件名称：main.s
//功能概要：汇编语言编程，调用构件GPIO，实现蓝灯闪烁
//版权所有：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20220102-20240208
//=====================================================================*/

.include "includes.inc"    /* 包含汇编总头文件 */

/* 定义数据段，即data段（RAM区域，变量使用的区域）=================== */
.section .data       /* 定义数据段 */
hello_information:   /* 定义字符串，标号即为字符串首地址，\0为结束标志  */
    .ascii "\n"
    .ascii "-----------------------------------------------------------------\n"
    .ascii "★金葫芦提示★                                                   \n"
    .ascii "【中文名称】RISC-V汇编工程框架（GPIO构件测试样例）                \n"
    .ascii "【程序功能】基于GPIO汇编构件控制蓝灯闪烁                          \n"
    .ascii "【硬件连接】见本工程05_UserBoard文件夹下user.inc文件              \n"
    .ascii "【操作说明】观察硬件板上的蓝灯                                    \n"
    .ascii "----------------------------------------------------------------\n\0"
printf_format1:           /* 定义一个printf使用的数据格式控制符 */
    .ascii "闪烁次数mLightCount =\0"
printf_format2:           /* 定义一个printf使用的数据格式控制符 */
    .ascii "%d\n\0"
printf_format3:    
     .ascii "LIGHT_BLUE:ON--\n\0"
printf_format4:    
     .ascii "LIGHT_BLUE:OFF--\n\n\0"    
.align 4                  /* 四字节对齐 */
mMainLoopCount:           /* 主循环次数变量（主循环临时变量以m作为前缀）*/
    .word 0               /* 初值为0 */
mLigtCount:               /* 灯的状态切换次数*/
    .word 0               /* 初值为0 */  
mFlag:                    /* 灯的状态标志 */
    .byte 'A'             /* 初值为'A'（暗） */

/* 定义代码段，即Falsh区域，存放代码、常数=========================== */
.section  .text        /* 定义代码段 */
.type main function    /* 声明下面的main标号为函数类型  */
.global main           /* 声明下面的main标号为全局函数，便于初始化调用 */
.align 2               /* 指令和数据采用2字节对齐，兼容16位指令集 */

/*主函数，一般情况下可以认为程序从此开始运行（实际上有启动过程）------ */
main:
    /*【1】启动部分（开头）========================================== */
    /*（1.1）申请栈空间 */
    addi sp,sp,-48           /* 申请栈空间 */
    /*（1.2）【不变】关总中断 */
	LI t0, 0x8
	CSRC mstatus, t0
	
    la a0,hello_information  /* 通过调试串口输出提示 */
    call printf
    /*（1.3）用户外设模块初始化 */
    /* 初始化GPIO（蓝灯），三个参数 */
    li a0,LIGHT_BLUE             /* 灯的引脚 */ 
    li a1,GPIO_OUTPUT            /* 输出 */
    li a2,LIGHT_OFF              /* 初始状态 */
    call gpio_init               /* 调用函数 */
    /*（1.4）开总中断 */
	LI t0, 0x8
	CSRS mstatus, t0
    /* 【1】启动部分（结尾）======================================== */
	
/*【2】主循环部分（开头）=========================================== */
main_loop: 
    /*（2.1）主循环次数变量mMainLoopCount+1，判断是否到设定值 */
       /* mMainLoopCount+1 */
       la a5,mMainLoopCount    /* a5←变量mMainLoopCount的地址 */
       lw t1,0(a5)             /* t1←变量mMainLoopCount的值 */
       addi t1,t1,1            /* t1←t1+1 */
       sw t1,0(a5)             /* 放回RAM中 */
       /* 判断是否到设定值 */
       li t2,MAINLOOP_COUNT    /* 常数在user.inc中宏定义 */
       bltu t1,t2,main_loop    /* 小于转,即t1<t2转 */
    /*（2.2）达到主循环次数设定值，执行下列语句，进行灯的亮暗处理 */
       /* 清除主循环次数变量 */
       la a5, mMainLoopCount    
       li t1,0
       sw t1,0(a5)
       /* 判定灯状态标志mFlag是否为'L' */
       la a5, mFlag
       lb t1,0(a5)
       li t2,'L'
       bne t1,t2,main_Light_OFF       /* t1≠t2转 */
       /* 灯状态标志mFlag为'L' */
       la a5,mLigtCount               /* 灯闪烁次数+1 */     
       lw t1,0(a5)                
       addi t1,t1,1                
       sw t1,0(a5)   
       la a0,printf_format1           /* 输出提示 闪烁次数mLightCount= */
       call printf 
       /* 通过调试串口输出显示 闪烁次数次数值，a0,a1为入口参数 */ 
       la a0,printf_format2           /* a0←data_format的地址 */
       la a5,mLigtCount               /* a5←变量mLigtCount的地址 */
       lw a1,0(a5)                    /* a1←变量mLigtCount的值 */
       call printf                    /* 调用函数 */
       /* 蓝灯亮 */
       li a0,LIGHT_BLUE               /* 第1参数a0 ← LIGHT_BLUE */
       li a1,LIGHT_ON                 /* 第2参数a1 ← LIGHT_ON */
       call gpio_set                  /* 调用函数 */ 
       la a0,printf_format3           /* 输出灯的状态 */
       call printf
       la a1,mFlag                    /* 灯的状态标志改为'A'（暗） */
       li t1,'A'
       sw t1,0(a1)
       j main_exit
main_Light_OFF:   /* 灯状态标志mFlag不为'L'，即为'A' */
       /* 蓝灯暗*/
       li a0,LIGHT_BLUE               /* 第1参数a0 ← LIGHT_BLUE */
       li a1,LIGHT_OFF                /* 第2参数a1 ← LIGHT_OFF */
       call gpio_set                  /* 调用函数 */ 
       la a0,printf_format4           /* 输出灯的状态 */
       call printf 
       la a1,mFlag                    /* 灯的状态标志改为'L'（亮） */
       li t1,'L'
       sw t1,0(a1)
main_exit:
    j main_loop                       /* 继续循环 */
/*【2】主循环部分（结尾）=========================================== */
    /* 从栈恢复现场（即使运行不到这里，每一个函数将栈恢复） */
    addi sp, sp, 48             /* 释放栈帧 */
    ret                         /* 返回 */
    