////=====================================================================
////�ļ����ƣ�gec.c�ļ�
////������λ��SD-Arm(sumcu.suda.edu.cn)
////���¼�¼��20181201-20200627-20240202
////��ֲ���򣺡��̶���
////=====================================================================
#include "gec.h"

//======================================================================
//�������ƣ�IRQ_OPC_CHANGE
//�������أ��ض����Ӧ�Ĳ�����
//����˵����IRQ_NUM�����ж�������,user[]����user����ж�������
//���ܸ�Ҫ����BIOS�ڵ��жϷ������̽��м̳�
//         ����CH573���ж�������������ԣ�������Ҫ���ж��������ڵ�Ŀ��λ
//         �޸�Ϊ���ӵ�ǰ�ж�������λ����ת��BIOS�ڶ�Ӧ�жϷ������̡���Ӧ��
//         ������
//======================================================================
uint32_t IRQ_OPC_CHANGE(int IRQ_NUM, uint8_t* user)
{    
    static uint32_t opcode = 0;
    uint32_t bios_addr=0, user_addr=0;  //������ڵ�ַ
    uint32_t user_opc=0;
    uint32_t imm1=0,imm2=0;
    uint32_t user_pc=0;
    if(IRQ_NUM == SysTick_IRQn)     //��SysTick_Handler�жϽ����ض���
    {
        user_addr = (uint32_t)SysTick_Handler;
        bios_addr = (uint32_t)component_fun[28];
        user_opc = ((uint32_t *)user)[SysTick_IRQn+1];
    }
    else if(IRQ_NUM == SWI_IRQn)    //��SysTick_Handler�жϽ����ض���
    {
        user_addr = (uint32_t)SW_Handler;
        bios_addr = (uint32_t)component_fun[29];
        user_opc = ((uint32_t *)user)[SWI_IRQn+1];
    }
    else if(IRQ_NUM == UART1_IRQn)  //��SysTick_Handler�жϽ����ض���
    {
        user_addr = (uint32_t)UART1_IRQHandler;
        bios_addr = (uint32_t)component_fun[30];
        user_opc = ((uint32_t *)user)[UART1_IRQn+1];
    }
    else                            //��֧��SysTick_Handler��SW_Handler��UART1_IRQHandler���ض���
       return 0;
    // printf("addr1:%lx\r\n", user_addr);
    // printf("addr2:%lx\r\n", bios_addr);
    // printf("opc:%lx\r\n", user_opc);

    //�жϸ�ָ���Ƿ�Ϊ��ǰ��ת���������ǰ��ת˵���Ѿ��޸Ĺ����ж������������ٴ��޸�
    if((user_opc&0x80000000)) return ((uint32_t *)user)[IRQ_NUM+1];         

    imm1 = ( ((user_opc&0x80000000)>>11) | ((user_opc&0x7FE00000)>>20) | ((user_opc&0x00100000)>>9) | ((user_opc&0x000FF000)) );
    if(imm1>>20==1)         //��ǰ��ת��������λ
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
//�������ƣ�Vectors_Init
//�������أ���
//����˵������
//���ܸ�Ҫ��User��BIOS�ж�������Ĳ��ּ̳�,�����⺯��ָ���ʼ��
//�޸���Ϣ��WYH��20200805���淶
//======================================================================
void  Vectors_Init()
{
    //���û����򣬱��뱾�δ���
    // #if (GEC_USER_SECTOR_START!=0)
    //��1����Flash����1������ǰ24�ֽ�Ϊ�գ���д���豸���кż�����汾�ų�ֵ
    // if(flash_isempty(MCU_SECTOR_NUM-1,24))
    // {
    //     flash_write_physical((MCU_SECTOR_NUM-1)*MCU_SECTORSIZE+
    //     MCU_FLASH_ADDR_START,24,(uint8_t *)"0123456789ABCDEF20200716");
    // }
    //��1����component_fun��ֵ��SYSTEM_FUNCTION������
    component_fun=(void **)(MCU_FLASH_ADDR_START+GEC_COMPONENT_LST_START*MCU_SECTORSIZE);
    
    uint8_t user[MCU_SECTORSIZE/16];                    //User�������������
    uint32_t opcode;
    //��2.1����ȡUser������ж���������жϴ�������׵�ַ������user����
    flash_read_logic(user,GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16);
    // ��2.2����ȡUser������ж���������жϴ�������׵�ַ������user����
    flash_erase(GEC_USER_SECTOR_START);
    //�ض��� UART1_IRQHandler
    // user[112]=(uint8_t)(0x6f); user[113]=(uint8_t)(0x40); user[114]=(uint8_t)(0xaf); user[115]=(uint8_t)(0x8a);
    opcode = IRQ_OPC_CHANGE(UART1_IRQn, user);
    ((uint32_t *)user)[UART1_IRQn+1] = opcode;
    #if (RTThread_Start == 0)
        //�ض��� SW_Handler
        // user[60]=(uint8_t)(0x6f); user[61]=(uint8_t)(0x40); user[62]=(uint8_t)(0x0f); user[63]=(uint8_t)(0xdb);
        opcode = IRQ_OPC_CHANGE(SWI_IRQn, user);
        ((uint32_t *)user)[SWI_IRQn+1] = opcode;
        //�ض��� SysTickHandler
        // user[52]=(uint8_t)(0x6f); user[53]=(uint8_t)(0x40); user[54]=(uint8_t)(0x0f); user[55]=(uint8_t)(0x86);
        opcode = IRQ_OPC_CHANGE(SysTick_IRQn, user);
        ((uint32_t *)user)[SysTick_IRQn+1] = opcode;
    #endif
    //��2.3�����޸ĺ��user����д��User�ж�������
    flash_write(GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16,user);

    //��4��printf��ʾ
    printf("  ��User��ʾ����������User��main����ִ��...\r\n\n");

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