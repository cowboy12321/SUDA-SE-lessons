////=====================================================================
////文件名称：gec.c文件
////制作单位：SD-Arm(sumcu.suda.edu.cn)
////更新记录：20181201-20200627-20240202
////移植规则：【固定】
////=====================================================================
#include "gec.h"

void  Vectors_Init()
{
    //有用户程序，编译本段代码
    // #if (GEC_USER_SECTOR_START!=0)
    //（1）若Flash倒数1扇区的前24字节为空，则写入设备序列号及软件版本号初值
    // if(flash_isempty(MCU_SECTOR_NUM-1,24))
    // {
    //     flash_write_physical((MCU_SECTOR_NUM-1)*MCU_SECTORSIZE+
    //     MCU_FLASH_ADDR_START,24,(uint8_t *)"0123456789ABCDEF20200716");
    // }

    uint8_t user[MCU_SECTORSIZE/16];                    //User向量表变量数组
    //（2.1）读取User程序的中断向量表各中断处理程序首地址赋并给user数组
    flash_read_logic(user,GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16);
    // （2.2）读取User程序的中断向量表各中断处理程序首地址赋并给user数组
    flash_erase(GEC_USER_SECTOR_START);
    //重定向 UART1_IRQHandler
    user[112]=(uint8_t)(0x6f); user[113]=(uint8_t)(0x40); user[114]=(uint8_t)(0xaf); user[115]=(uint8_t)(0x8a);

    #if (RTThread_Start == 0)
        //重定向 SW_Handler
        user[60]=(uint8_t)(0x6f); user[61]=(uint8_t)(0x40); user[62]=(uint8_t)(0x0f); user[63]=(uint8_t)(0xdb);
        //重定向 SysTickHandler
        user[52]=(uint8_t)(0x6f); user[53]=(uint8_t)(0x40); user[54]=(uint8_t)(0x0f); user[55]=(uint8_t)(0x86);
    #endif
    //（2.3）将修改后的user数组写回User中断向量表
    flash_write(GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16,user);

    //（1）给component_fun赋值，SYSTEM_FUNCTION函数用
    component_fun=(void **)(MCU_FLASH_ADDR_START+GEC_COMPONENT_LST_START*MCU_SECTORSIZE);

    //（4）printf提示
    printf("  【User提示】：将进入User的main函数执行...\r\n\n");

    // #endif
}


// __attribute__((interrupt("WCH-Interrupt-fast")))
//  __attribute__((section(".vector_handler")))
//======================================================================
//中断名称：UART_UPDATE_Handler
//功能概要：UART_UPDATE接收中断，处理接收到的数据,跳转BIOS的UART_UPDATE处理
//参  数: 无
//返  回: 无
//说  明: 需要启动中断并注册才可使用
//======================================================================
// void UART1_IRQHandler(void)
// {
//   __asm("li  a0,0x00000070");     //将立即数0x00000070赋给a0寄存器
//   __asm("jr  a0");                //执行跳转0x00000070为User的起始地址
//     return;
    
// }

// void SW_Handler(void)
// {
//     __asm("li  a0,0x0000003c");     //将立即数0x00000070赋给a0寄存器
//   __asm("jr  a0");                //执行跳转0x00000070为User的起始地址
//     return;
// }

// void SysTick_Handler(void)
// {
//     __asm("li  a0,0x00000034");     //将立即数0x00000070赋给a0寄存器
//   __asm("jr  a0");                //执行跳转0x00000070为User的起始地址
//     return;
// }