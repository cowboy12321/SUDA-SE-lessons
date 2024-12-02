.include "gpio.inc"

.equ NOERROR,       0x0
.equ ERROR,         0x1
.equ LEDON,         0x0
.equ LEDOFF,        0x1

#定义代码存储text段开始，实际代码存储在Flash中
.section   .text
.align 2                          /*指令和数据采用2字节对齐*/


gpio_port_pin_resolution:
	/* 进行入栈 */
	addi sp, sp, -64			/* 分配堆栈框架*/
	sw ra, 60(sp)				/* 将寄存器地址写到堆栈上*/
	/* 计算出GPIO端口号和引脚号 */
	srli t4,a0,0x8				/* t4=a0=端口号 srli是右移动指令*/
	andi t5,a0,255				/* t5=a0=引脚号 andi是按位与*/
	mv a0,t4
	mv a1,t5
	/* 释放栈 */
	lw ra, 60(sp)               /* Restore the return address 恢复返回地址 LW 指令将一个 32 位数值从存储器读入寄存器 rd’中 */
    addi sp, sp, 64             /* Deallocating the stack frame 释放栈帧 addi相加指令*/
    mv  a0,	t4					/* load return value 0 读取返回值 */
    mv  a1, t5
    ret							/* 返回 */

/*======================================================================
// 函数名称：gpio_init
// 函数返回：无
// 参数说明：r0:(端口号|(引脚号)),例:(PTB_NUM|(5u))表示B口5脚,头文件中有宏定义
//           r1:引脚方向（0=输入,1=输出,可用引脚方向宏定义）
//           r2:端口引脚初始状态（0=低电平，1=高电平）
// 功能概要：初始化指定端口引脚作为GPIO引脚功能，并定义为输入或输出。若是输出，
//           还指定初始状态是低电平或高电平
// 备    注：端口x的每个引脚控制寄存器PORTx_PCRn的地址=PORT_PCR_BASE+x*0x1000+n*4
//           其中:x=0~4，对应A~E;n=0~31
//======================================================================  */
.type GPIO_init function          /*声明gpio_init为函数类型*/
.global gpio_init                 /*将gpio_init定义成全局函数，便于芯片初始化之后调用*/
gpio_init:
	addi sp, sp, -64			/* 分配堆栈框架*/
	sw ra, 60(sp)				/* 将寄存器地址写到堆栈上 SW指令将寄存器 rs2’中的 32 位值保存到存储器中*/
	mv a3,a2					/* 将函数第3个参数a2赋值给a3 */
	mv a2,a1					/* 将函数第2个参数a1赋值给a2*/
    /* 计算出GPIO端口号和引脚号 */
	srli t4,a0,0x8				/* t4=a0=端口号 */
	andi t5,a0,255				/* t5=a0=引脚号 */
	mv a0,t4					/* a0=端口号 */
	mv a1,t5					/* a1=引脚号 */

    /* 判断端口号 */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA           /* 当（a0=端口号）=（a4=0x00）的时候跳转GPIOA */
    j GPIOB                     /* 当（a0=端口号）!=（a4=0x00）的时候跳转GPIOA */

/* 端口A GPIOA */
GPIOA:
    /* 判断GPIO输入输出模式 */
    beqz a2, GPIOA_input_H       /* 函数第2个参数a2=0，则跳转gpio_input_H */
    li t4,1						/* t4=1 */
    beq a2,t4, GPIOA_output_H	/* 函数第2个参数a2=1，则跳转gpio_output_H */
    li a2, ERROR
    j exit

/* 端口B GPIOB */
GPIOB:
    /* 判断GPIO输入输出模式 */
    beqz a2, GPIOB_input_H       /* 函数第2个参数a2=0，则跳转gpio_input_H */
    li t4,1						/* t4=1 */
    beq a2,t4, GPIOB_output_H	/* 函数第2个参数a2=1，则跳转gpio_output_H */
    li a2, ERROR
    j exit

/* 配置GPIOA引脚为输出模式 */
GPIOA_output_H:
    li t0, R32_PA_DIR           /* 将PA方向寄存器赋值给t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_DIR寄存器的值 */
    or t2, t2, t1               /* (t1=(GPIO_OUTPUT << pin ))与(t2=(R32_PA_DIR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的方向 */

    beqz a3, OUT_PUT_LOW_A      /* 初始状态输出低电平 */
    j OUT_PUT_HIGH_A            /* 初始状态输出高电平 */

/* 输出低电平 */
OUT_PUT_LOW_A:
    li t0, R32_PA_CLR           /* 将R32_PA_CLR寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_CLR寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PA_CLR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的输出值 */

    j exit

/* 输出高电平 */
OUT_PUT_HIGH_A:
    li t0, R32_PA_OUT           /* 将R32_PA_OUT寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_OUT寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PA_OUT))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的输出值 */

    j exit

/* 配置GPIOA引脚为输入模式 */
GPIOA_input_H:
    li t0, R32_PA_DIR           /* 将R32_PA_DIR寄存器赋值给t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    not t1, t1                  /* t1取反存在t1，对应位为0 */
    lw t2, 0(t0)                /* 读取R32_PA_DIR寄存器的值 */
    and t2, t2, t1              /* (t1=~(GPIO_OUTPUT << pin ))与(t2=(R32_PA_DIR))值按位与，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的方向 */

    j exit

/* 配置GPIOB引脚为输出模式 */
GPIOB_output_H:
    li t0, R32_PB_DIR           /* 将PB方向寄存器赋值给t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_DIR寄存器的值 */
    or t2, t2, t1               /* (t1=(GPIO_OUTPUT << pin ))与(t2=(R32_PB_DIR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的方向 */

    beqz a3, OUT_PUT_LOW_B      /* 初始状态输出低电平 */
    j OUT_PUT_HIGH_B            /* 初始状态输出高电平 */

/* 输出低电平 */
OUT_PUT_LOW_B:
    li t0, R32_PB_CLR           /* 将R32_PB_CLR寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_CLR寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PB_CLR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的输出值 */

    j exit

/* 输出高电平 */
OUT_PUT_HIGH_B:
    li t0, R32_PB_OUT           /* 将R32_PB_OUT寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_OUT寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PB_OUT))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的输出值 */

    j exit

/* 配置GPIOB引脚为输入模式 */
GPIOB_input_H:
    li t0, R32_PB_DIR           /* 将R32_PB_DIR寄存器赋值给t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    not t1, t1                  /* t1取反存在t1，对应位为0 */
    lw t2, 0(t0)                /* 读取R32_PB_DIR寄存器的值 */
    and t2, t2, t1              /* (t1=~(GPIO_OUTPUT << pin ))与(t2=(R32_PB_DIR))值按位与，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的方向 */
    j exit

/* 退出栈 */
exit:
	lw ra, 60(sp)               /* Restore the return address 恢复返回地址 */
    addi sp, sp, 64             /* Deallocating the stack frame 释放栈帧 */
    li  a0,0					/* load return value 0 读取返回值 */
	ret

/*=====================================================================
//函数名称：gpio_set
//函数返回：无
// 参数说明：r0:(端口号|(引脚号)),例:(PTB_NUM|(5u))表示B口5脚,头文件中有宏定义
//           r1:希望设置的端口引脚状态（0=低电平，1=高电平）
//功能概要：当指定引脚被定义为GPIO功能且为输出时，本函数设定引脚状态
// 备    注：端口x的每个引脚控制寄存器PORTx_PCRn的地址=PORT_PCR_BASE+x*0x1000+n*4
//           其中:x=0~4，对应A~E;n=0~31
//=====================================================================*/
.type gpio_set function         /*声明gpio_set为函数类型 */
.global gpio_set                /*将gpio_set定义成全局函数，便于芯片初始化之后调用*/
gpio_set:
/* 通过调整栈指针分配出出栈空间用于存放局部变量和存放调用函数返回地址，
      主函数中栈空间分出16字节，   ra为返回地址寄存器，占用4个字节，将ra
      中的返回地址放入sp指针地址偏移16个字节的位置*/
	addi sp, sp, -64			/* 分配栈空间*/
	sw ra, 60(sp)				/* 存储返回地址 */

	mv a2,a1					/* 将函数第二个参数赋值给a2 */
	/* 计算出GPIO端口号和引脚号 */
	srli t4,a0,0x8				/* t4=a0=端口号 */
	andi t5,a0,255				/* a2=a0=引脚号 */
	mv a0,t4					/* a0=端口号 */
	mv a1,t5					/* a1=引脚号 */

    /* 判断端口号 */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA_SET       /* 当（a0=端口号）=（a4=0x00）的时候跳转GPIOA_SET */
    j GPIOB_SET                 /* 当（a0=端口号）!=（a4=0x00）的时候跳转GPIOB_SET */

GPIOA_SET:
    beqz a2, GPIOA_SET_LOW      /* 设置输出低电平 */
    j GPIOA_SET_HIGH            /* 设置输出高电平 */

/* 输出低电平 */
GPIOA_SET_LOW:
    li t0, R32_PA_CLR           /* 将R32_PA_CLR寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_CLR寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PA_CLR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的输出值 */

    j SET_exit

/* 输出高电平 */
GPIOA_SET_HIGH:
    li t0, R32_PA_OUT           /* 将R32_PA_OUT寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_OUT寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PA_OUT))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的输出值 */

    j SET_exit

GPIOB_SET:
    beqz a2, GPIOB_SET_LOW      /* 设置输出低电平 */
    j GPIOB_SET_HIGH            /* 设置输出高电平 */

/* 输出低电平 */
GPIOB_SET_LOW:
    li t0, R32_PB_CLR           /* 将R32_PB_CLR寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_CLR寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PB_CLR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的输出值 */

    j SET_exit

/* 输出高电平 */
GPIOB_SET_HIGH:
    li t0, R32_PB_OUT           /* 将R32_PB_OUT寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_OUT寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PB_OUT))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的输出值 */

    j SET_exit

/* 退出并释放栈空间 */
SET_exit:
	lw ra, 60(sp)               /* 恢复返回地址 */
    addi sp, sp, 64             /* 释放栈空间 */
    ret							/* 返回 */

/*======================================================================
// 函数名称：gpio_reverse
// 函数返回：无
// 参数说明：r0:(端口号)|(引脚号),例:(PTB_NUM|(5u))表示B口5脚,头文件中有宏定义
// 功能概要：反转指定引脚状态
//======================================================================*/
.type gpio_reverse function     /*声明gpio_reverse为函数类型  */
.global gpio_reverse            /*将gpio_reverse定义成全局函数，便于芯片初始化之后调用 */
gpio_reverse:
/* 通过调整栈指针分配出出栈空间用于存放局部变量和存放调用函数返回地址，
      主函数中栈空间分出16字节，   ra为返回地址寄存器，占用4个字节，将ra
      中的返回地址放入sp指针地址偏移16个字节的位置*/
	addi sp, sp, -64			/* 分配堆栈框架*/
	sw ra, 60(sp)				/* 将寄存器地址写到堆栈上*/

	/* 计算出GPIO端口号和引脚号 */
	srli t4,a0,0x8				/* t4=a0=端口号 */
	andi t5,a0,255				/* a2=a0=引脚号 */
	mv a0,t4					/* a0=端口号 */
	mv a1,t5					/* a1=引脚号 */

    /* 判断端口号 */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA_REVERSE   /* 当（a0=端口号）=（a4=0x00）的时候跳转GPIOA_REVERSE */
    j GPIOB_REVERSE             /* 当（a0=端口号）!=（a4=0x00）的时候跳转GPIOB_REVERSE */

GPIOA_REVERSE:
    li t0, R32_PA_OUT           /* t0=R32_PA_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a1              /* 将t1右移（a1=引脚号） */

    beqz t1, GPIOA_REV_H        /* 当前为低电平，反转成高电平 */
    j GPIOA_REV_L               /* 当前为高电平，反转成地电平 */

GPIOA_REV_L:
    li t0, R32_PA_CLR           /* 将R32_PA_CLR寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_CLR寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PA_CLR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的输出值 */

    j gpio_reverse_exit

GPIOA_REV_H:
    li t0, R32_PA_OUT           /* 将R32_PA_OUT寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PA_OUT寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PA_OUT))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOAx的输出值 */

    j SET_exit

GPIOB_REVERSE:
    li t0, R32_PB_OUT           /* t0=R32_PB_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a1              /* 将t1右移（a1=引脚号） */

    beqz t1, GPIOB_REV_H        /* 当前为低电平，反转成高电平 */
    j GPIOB_REV_L               /* 当前为高电平，反转成地电平 */

GPIOB_REV_L:
    li t0, R32_PB_CLR           /* 将R32_PB_CLR寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_CLR寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PB_CLR))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的输出值 */

    j gpio_reverse_exit

GPIOB_REV_H:
    li t0, R32_PB_OUT           /* 将R32_PB_OUT寄存器赋值给t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* 将t1左移（a1=引脚号） */
    lw t2, 0(t0)                /* 读取R32_PB_OUT寄存器的值 */
    or t2, t2, t1               /* (t1=(1 << pin ))与(t2=(R32_PB_OUT))值按位或，存入t2 */
    sw t2, 0(t0)                /* 设置GPIOBx的输出值 */

    j gpio_reverse_exit

gpio_reverse_exit:
	lw ra, 60(sp)               /* 恢复返回地址 */
    addi sp, sp, 64             /* 释放栈空间 */
    ret		

/*======================================================================
// 函数名称：gpio_get
// 函数返回：r2:指定端口引脚的状态（1或0）
// 参数说明：r0:(端口号)|(引脚号),例:(PTB_NUM|(5u))表示B口5脚,头文件中有宏定义
// 功能概要：当指定端口引脚被定义为GPIO功能且为输入时，本函数获取指定引脚状态
//======================================================================*/
.type gpio_get function         /*声明gpio_get为函数类型*/
.global gpio_get                /*将gpio_get定义成全局函数，便于芯片初始化之后调用*/
gpio_get:
/* 通过调整栈指针分配出出栈空间用于存放局部变量和存放调用函数返回地址，
      主函数中栈空间分出16字节，   ra为返回地址寄存器，占用4个字节，将ra
      中的返回地址放入sp指针地址偏移16个字节的位置*/
	addi sp, sp, -64			/* 分配堆栈框架*/
	sw ra, 60(sp)				/* 将寄存器地址写到堆栈上*/

	/* 计算出GPIO端口号和引脚号 */
	srli t4,a0,0x8				/* t4=a0=端口号 */
	andi t5,a0,255				/* t5=a0=引脚号 */
	mv a2,t4					/* a2=端口号 */
	mv a3,t5					/* a3=引脚号 */

    /* 判断端口号 */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA_GET       /* 当（a0=端口号）=（a4=0x00）的时候跳转GPIOA_GET */
    j GPIOB_GET                 /* 当（a0=端口号）!=（a4=0x00）的时候跳转GPIOB_GET */

GPIOA_GET:
    li t0, R32_PA_DIR           /* t0=R32_PA_DIR */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* 将t1左移（a3=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* 将t1右移（a3=引脚号） */

    beqz t1, GPIOA_GET_IN       /* GPIOAx输入 */
    j GPIOA_GET_OUT             /* GPIOAx输出 */

GPIOA_GET_IN:
    li t0, R32_PA_PIN           /* t0=R32_PA_PIN */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* 将t1左移（a3=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* 将t1右移（a3=引脚号） */

    beqz t1, RETURN_0           /* GPIOAx输入，状态为低电平 */
    j RETURN_1                  /* GPIOAx输入，状态为高电平 */

GPIOA_GET_OUT:
    li t0, R32_PA_OUT           /* t0=R32_PA_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* 将t1左移（a3=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* 将t1右移（a3=引脚号） */

    beqz t1, RETURN_0           /* GPIOAx输入，状态为低电平 */
    j RETURN_1                  /* GPIOAx输入，状态为高电平 */

GPIOB_GET:
    li t0, R32_PB_DIR           /* t0=R32_PB_DIR */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* 将t1左移（a3=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* 将t1右移（a3=引脚号） */

    beqz t1, GPIOB_GET_IN       /* GPIOBx输入 */
    j GPIOB_GET_OUT             /* GPIOBx输出 */

GPIOB_GET_IN:
    li t0, R32_PB_PIN           /* t0=R32_PB_PIN */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* 将t1左移（a3=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* 将t1右移（a3=引脚号） */

    beqz t1, RETURN_0           /* GPIOAx输入，状态为低电平 */
    j RETURN_1                  /* GPIOAx输入，状态为高电平 */

GPIOB_GET_OUT:
    li t0, R32_PB_OUT           /* t0=R32_PB_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* 将t1左移（a3=引脚号） */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* 将t1右移（a3=引脚号） */

    beqz t1, RETURN_0           /* GPIOAx输入，状态为低电平 */
    j RETURN_1                  /* GPIOAx输入，状态为高电平 */


RETURN_0:
    lw ra, 64(sp)               /* 恢复返回地址 */
    addi sp, sp, 64             /* 释放栈空间*/
    li  a0,0					/* 返回值0 */
    ret							/* 返回 */

RETURN_1:
	lw ra, 60(sp)               /* 恢复返回地址 */
    addi sp, sp, 64             /* 释放栈空间 */
    li  a0,1					/* 返回值1 */
    ret	
