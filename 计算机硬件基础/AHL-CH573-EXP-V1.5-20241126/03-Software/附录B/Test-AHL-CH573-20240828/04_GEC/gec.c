////=====================================================================
////�ļ����ƣ�gec.c�ļ�
////������λ��SD-Arm(sumcu.suda.edu.cn)
////���¼�¼��20181201-20200627-20240202
////��ֲ���򣺡��̶���
////=====================================================================
#include "gec.h"

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

    uint8_t user[MCU_SECTORSIZE/16];                    //User�������������
    //��2.1����ȡUser������ж���������жϴ�������׵�ַ������user����
    flash_read_logic(user,GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16);
    // ��2.2����ȡUser������ж���������жϴ�������׵�ַ������user����
    flash_erase(GEC_USER_SECTOR_START);
    //�ض��� UART1_IRQHandler
    user[112]=(uint8_t)(0x6f); user[113]=(uint8_t)(0x40); user[114]=(uint8_t)(0xaf); user[115]=(uint8_t)(0x8a);

    #if (RTThread_Start == 0)
        //�ض��� SW_Handler
        user[60]=(uint8_t)(0x6f); user[61]=(uint8_t)(0x40); user[62]=(uint8_t)(0x0f); user[63]=(uint8_t)(0xdb);
        //�ض��� SysTickHandler
        user[52]=(uint8_t)(0x6f); user[53]=(uint8_t)(0x40); user[54]=(uint8_t)(0x0f); user[55]=(uint8_t)(0x86);
    #endif
    //��2.3�����޸ĺ��user����д��User�ж�������
    flash_write(GEC_USER_SECTOR_START,0,MCU_SECTORSIZE/16,user);

    //��1����component_fun��ֵ��SYSTEM_FUNCTION������
    component_fun=(void **)(MCU_FLASH_ADDR_START+GEC_COMPONENT_LST_START*MCU_SECTORSIZE);

    //��4��printf��ʾ
    printf("  ��User��ʾ����������User��main����ִ��...\r\n\n");

    // #endif
}


// __attribute__((interrupt("WCH-Interrupt-fast")))
//  __attribute__((section(".vector_handler")))
//======================================================================
//�ж����ƣ�UART_UPDATE_Handler
//���ܸ�Ҫ��UART_UPDATE�����жϣ�������յ�������,��תBIOS��UART_UPDATE����
//��  ��: ��
//��  ��: ��
//˵  ��: ��Ҫ�����жϲ�ע��ſ�ʹ��
//======================================================================
// void UART1_IRQHandler(void)
// {
//   __asm("li  a0,0x00000070");     //��������0x00000070����a0�Ĵ���
//   __asm("jr  a0");                //ִ����ת0x00000070ΪUser����ʼ��ַ
//     return;
    
// }

// void SW_Handler(void)
// {
//     __asm("li  a0,0x0000003c");     //��������0x00000070����a0�Ĵ���
//   __asm("jr  a0");                //ִ����ת0x00000070ΪUser����ʼ��ַ
//     return;
// }

// void SysTick_Handler(void)
// {
//     __asm("li  a0,0x00000034");     //��������0x00000070����a0�Ĵ���
//   __asm("jr  a0");                //ִ����ת0x00000070ΪUser����ʼ��ַ
//     return;
// }