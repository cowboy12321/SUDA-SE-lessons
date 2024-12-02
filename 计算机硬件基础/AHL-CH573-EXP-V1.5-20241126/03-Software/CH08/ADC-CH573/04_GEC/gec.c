////=====================================================================
////文件名称：gec.c文件
////制作单位：SD-Arm(sumcu.suda.edu.cn)
////更新记录：20181201-20200627-20240202
////移植规则：【固定】
////=====================================================================
#include "gec.h"

//======================================================================
//函数名称：IRQ_OPC_CHANGE
//函数返回：重定向对应的操作码
//参数说明：IRQ_NUM——中断向量号,user[]——user里的中断向量表
//功能概要：对BIOS内的中断服务例程进行继承
//         由于CH573的中断向量表的特殊性，所以需要将中断向量表内的目标位
//         修改为“从当前中断向量号位置跳转到BIOS内对应中断服务例程”对应的
//         操作码
//======================================================================
uint32_t IRQ_OPC_CHANGE(int IRQ_NUM, uint8_t* user)
{    
    static uint32_t opcode = 0;
    uint32_t bios_addr=0, user_addr=0;  //函数入口地址
    uint32_t user_opc=0;
    uint32_t imm1=0,imm2=0;
    uint32_t user_pc=0;
    if(IRQ_NUM == SysTick_IRQn)     //对SysTick_Handler中断进行重定向
    {
        user_addr = (uint32_t)SysTick_Handler;
        bios_addr = (uint32_t)component_fun[28];
        user_opc = ((uint32_t *)user)[SysTick_IRQn+1];
    }
    else if(IRQ_NUM == SWI_IRQn)    //对SysTick_Handler中断进行重定向
    {
        user_addr = (uint32_t)SW_Handler;
        bios_addr = (uint32_t)component_fun[29];
        user_opc = ((uint32_t *)user)[SWI_IRQn+1];
    }
    else if(IRQ_NUM == UART1_IRQn)  //对SysTick_Handler中断进行重定向
    {
        user_addr = (uint32_t)UART1_IRQHandler;
        bios_addr = (uint32_t)component_fun[30];
        user_opc = ((uint32_t *)user)[UART1_IRQn+1];
    }
    else                            //仅支持SysTick_Handler、SW_Handler、UART1_IRQHandler的重定向
       return 0;
    // printf("addr1:%lx\r\n", user_addr);
    // printf("addr2:%lx\r\n", bios_addr);
    // printf("opc:%lx\r\n", user_opc);

    //判断该指令是否为向前跳转，如果是向前跳转说明已经修改过了中断向量表，无需再次修改
    if((user_opc&0x80000000)) return ((uint32_t *)user)[IRQ_NUM+1];         

    imm1 = ( ((user_opc&0x80000000)>>11) | ((user_opc&0x7FE00000)>>20) | ((user_opc&0x00100000)>>9) | ((user_opc&0x000FF000)) );
    if(imm1>>20==1)         //向前跳转，补符号位
        imm1 = 0xFFE00000 | imm1;
    // printf("imm1:%lx\r\n", imm1);
    user_pc = user_addr - imm1;
    // printf("pc:%lx\r\n", user_pc);
    imm2 = bios_addr - user_pc;
    // printf("imm2:%lx\r\n", imm2);
    opcode = (((imm2&0x100000)<<11) | ((imm2&0x7FE)<<20) | ((imm2&0x800)<<9) | (imm2&0xFF000)) | (0x0) | (0x6f);
    // printf("opcode:%lx\r\n", opcode);
    return opcode;
}
//======================================================================
//函数名称：Vectors_Init
//函数返回：无
//参数说明：无
//功能概要：User对BIOS中断向量表的部分继承,构件库函数指针初始化
//修改信息：WYH，20200805，规范
//======================================================================
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
    //（1）给component_fun赋值，SYSTEM_FUNCTION函数用
    component_fun=(void **)(MCU_FLASH_ADDR_START+GEC_COMPONENT_LST_START*MCU_SECTORSIZE);
    
    uint8_t user[MCU_SECTORSIZE/16];                    //User向量表变量数组
    uint32_t opcode;
    //（2.1）读取User程序的中断向量表各中断处理程序首地址赋并给user数组
    flash_read_logic(user,GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16);
    // （2.2）读取User程序的中断向量表各中断处理程序首地址赋并给user数组
    flash_erase(GEC_USER_SECTOR_START);
    //重定向 UART1_IRQHandler
    // user[112]=(uint8_t)(0x6f); user[113]=(uint8_t)(0x40); user[114]=(uint8_t)(0xaf); user[115]=(uint8_t)(0x8a);
    opcode = IRQ_OPC_CHANGE(UART1_IRQn, user);
    ((uint32_t *)user)[UART1_IRQn+1] = opcode;
    #if (RTThread_Start == 0)
        //重定向 SW_Handler
        // user[60]=(uint8_t)(0x6f); user[61]=(uint8_t)(0x40); user[62]=(uint8_t)(0x0f); user[63]=(uint8_t)(0xdb);
        opcode = IRQ_OPC_CHANGE(SWI_IRQn, user);
        ((uint32_t *)user)[SWI_IRQn+1] = opcode;
        //重定向 SysTickHandler
        // user[52]=(uint8_t)(0x6f); user[53]=(uint8_t)(0x40); user[54]=(uint8_t)(0x0f); user[55]=(uint8_t)(0x86);
        opcode = IRQ_OPC_CHANGE(SysTick_IRQn, user);
        ((uint32_t *)user)[SysTick_IRQn+1] = opcode;
    #endif
    //（2.3）将修改后的user数组写回User中断向量表
    flash_write(GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16,user);

    //（4）printf提示
    printf("  【User提示】：将进入User的main函数执行...\r\n\n");

    // #endif
}

void SYS_ResetExecute( void )
{
  FLASH_ROM_SW_RESET();
  R8_SAFE_ACCESS_SIG = SAFE_ACCESS_SIG1;
  R8_SAFE_ACCESS_SIG = SAFE_ACCESS_SIG2;
  R8_RST_WDOG_CTRL |= RB_SOFTWARE_RESET;
  R8_SAFE_ACCESS_SIG = 0;
}