//======================================================================
//�ļ����ƣ�uart.c
//���ܸ�Ҫ��uart�ײ���������Դ�ļ�
//��Ȩ���У����ݴ�ѧǶ��ʽϵͳ���������о���(sumcu.suda.edu.cn)
//���¼�¼��2020-0918 V1.0 GXY
//======================================================================

#include "uart.h"


//====���崮��IRQ�Ŷ�Ӧ��====
__attribute__((section (".other"))) IRQn_Type table_irq_uart[4]= {UART0_IRQn, UART1_IRQn, UART2_IRQn, UART3_IRQn};



//�ڲ���������
__attribute__((section (".update"))) uint8_t uart_is_uartNo(uint8_t uartNo);

//======================================================================
//�������ƣ�uart_init
//���ܸ�Ҫ����ʼ��uartģ��
//����˵����uartNo:���ںţ�UART_1��UART_2��UART_3
//          baud:�����ʣ�300��600��1200��2400��4800��9600��19200��115200...
//�������أ���
//======================================================================
void uart_init(uint8_t uartNo, uint32_t baud_rate)
{
    uint32_t x;

//    SetSysClock( CLK_SOURCE_PLL_60MHz );
    switch (uartNo)
    {
    case UART_0:    // ����0
#ifdef UART0_GROUP
            //
            switch (UART0_GROUP)
            {
                case 0:

                   R32_PB_OUT |= ((uint32_t)(bTXD0));
                   //PB4->RX
                   R32_PB_PD_DRV &= ~((uint32_t)(bRXD0));
                   R32_PB_PU     |= ((uint32_t)(bRXD0));
                   R32_PB_DIR    &= ~((uint32_t)(bRXD0));
                   //PB7->TX
                   R32_PB_PD_DRV &= ~((uint32_t)(bTXD0));
                   R32_PB_DIR    |= ((uint32_t)(bTXD0));

                   R8_UART0_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;     // FIFO�򿪣�������4�ֽ�
                   R8_UART0_LCR = RB_LCR_WORD_SZ;
                   R8_UART0_IER = RB_IER_TXD_EN;
                   R8_UART0_DIV = 1;

                   //���ò�����115200
                   x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                   x = (x + 5) / 10;
                   R16_UART0_DL = (uint16_t)x;
                   break;
                case 1:
                    //uart0����ӳ��
                  R16_PIN_ALTERNATE |= RB_PIN_UART0;

                  R32_PA_OUT |= ((uint32_t)(bTXD0_));
                  //PA15->RX
                  R32_PA_PD_DRV &= ~((uint32_t)(bRXD0_));
                  R32_PA_PU     |= ((uint32_t)(bRXD0_));
                  R32_PA_DIR    &= ~((uint32_t)(bRXD0_));
                  //PA14->TX
                  R32_PA_PD_DRV &= ~((uint32_t)(bTXD0_));
                  R32_PA_DIR    |= ((uint32_t)(bTXD0_));

                  R8_UART0_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;     // FIFO�򿪣�������4�ֽ�
                  R8_UART0_LCR = RB_LCR_WORD_SZ;
                  R8_UART0_IER = RB_IER_TXD_EN;
                  R8_UART0_DIV = 1;

                  //���ò�����115200
                  x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                  x = (x + 5) / 10;
                  R16_UART0_DL = (uint16_t)x;
                  break;
                default:
                    break;
            }
#endif
            break;
        case UART_1:
#ifdef UART1_GROUP
            //
            switch (UART1_GROUP) {
                case 0:

                    R32_PA_OUT |= ((uint32_t)(bTXD1));
                    //PA8->RX
                    R32_PA_PD_DRV &= ~((uint32_t)(bRXD1));
                    R32_PA_PU     |= ((uint32_t)(bRXD1));
                    R32_PA_DIR    &= ~((uint32_t)(bRXD1));

                    //PA9->TX
                    R32_PA_PD_DRV &= ~((uint32_t)(bTXD1));
                    R32_PA_DIR    |= ((uint32_t)(bTXD1));

                    R8_UART1_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;       // FIFO�򿪣�������4�ֽ�
                    R8_UART1_LCR = RB_LCR_WORD_SZ;
                    R8_UART1_IER = RB_IER_TXD_EN;
                    R8_UART1_DIV = 1;

                    //���ò�����115200
                    x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                    x = (x + 5) / 10;
                    R16_UART1_DL = (uint16_t)x;
                    break;
                case 1:
                    //uart1����ӳ��
                    R16_PIN_ALTERNATE |= RB_PIN_UART1;

                    R32_PB_OUT |= ((uint32_t)(bTXD1_));
                    //PB12->RX
                    R32_PB_PD_DRV &= ~((uint32_t)(bRXD1_));
                    R32_PB_PU     |= ((uint32_t)(bRXD1_));
                    R32_PB_DIR    &= ~((uint32_t)(bRXD1_));
                    //PB13->TX
                    R32_PB_PD_DRV &= ~((uint32_t)(bTXD1_));
                    R32_PB_DIR    |= ((uint32_t)(bTXD1_));

                    R8_UART1_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;       // FIFO�򿪣�������4�ֽ�
                    R8_UART1_LCR = RB_LCR_WORD_SZ;
                    R8_UART1_IER = RB_IER_TXD_EN;
                    R8_UART1_DIV = 1;

                    //���ò�����115200
                    x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                    x = (x + 5) / 10;
                    R16_UART1_DL = (uint16_t)x;
                    break;
                default:
                    break;
            }
#endif
            break;
        case UART_2:

              R32_PB_OUT |= ((uint32_t)(bTXD2));
              //PB22->RX
              R32_PB_PD_DRV &= ~((uint32_t)(bRXD2));
              R32_PB_PU     |= ((uint32_t)(bRXD2));
              R32_PB_DIR    &= ~((uint32_t)(bRXD2));
              //PB23->TX
              R32_PB_PD_DRV &= ~((uint32_t)(bTXD2));
              R32_PB_DIR    |= ((uint32_t)(bTXD2));

              R8_UART2_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;      // FIFO�򿪣�������4�ֽ�
              R8_UART2_LCR = RB_LCR_WORD_SZ;
              R8_UART2_IER = RB_IER_TXD_EN;
              R8_UART2_DIV = 1;

              //���ò�����115200
              x = 10 * (480000000/(0x08)) / 8 / baud_rate;
              x = (x + 5) / 10;
              R16_UART2_DL = (uint16_t)x;
              break;
        case UART_3:

                R32_PA_OUT |= ((uint32_t)(bTXD3));
                //PA4->RX
                R32_PA_PD_DRV &= ~((uint32_t)(bRXD3));
                R32_PA_PU     |= ((uint32_t)(bRXD3));
                R32_PA_DIR    &= ~((uint32_t)(bRXD3));
                //PA5->TX
                R32_PA_PD_DRV &= ~((uint32_t)(bTXD3));
                R32_PA_DIR    |= ((uint32_t)(bTXD3));

                R8_UART3_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;       // FIFO�򿪣�������4�ֽ�
                R8_UART3_LCR = RB_LCR_WORD_SZ;
                R8_UART3_IER = RB_IER_TXD_EN;
                R8_UART3_DIV = 1;

               //���ò�����115200
                x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                x = (x + 5) / 10;
                R16_UART3_DL = (uint16_t)x;
                break;
        default:
            break;
    }

}

//======================================================================
//�������ƣ�uart_send1
//����˵����uartNo: ���ں�:UART_1��UART_2��UART_3
//          ch:Ҫ���͵��ֽ�
//�������أ�����ִ��״̬��1=���ͳɹ���0=����ʧ�ܡ�
//���ܸ�Ҫ�����з���1���ֽ�
//======================================================================
__attribute__((section (".update"))) uint8_t uart_send1(uint8_t uartNo,uint8_t ch)
{
  uint32_t t;

    //�жϴ��봮�ںŲ����Ƿ���������ֱ���˳�
    if(uart_is_uartNo(uartNo)==0)
    {
        return 0;
    }
    //�ж�UARTx��
    switch (uartNo)
    {
        case 0:
          for (t = 0; t < 0xFBBB; t++)//��ѯָ������
          {
            if(R8_UART0_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART0_THR = ch;
              break;
            }
          }
            break;
        case 1:
          for (t = 0; t < 0xFBBB; t++)//��ѯָ������
          {
            if(R8_UART1_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART1_THR = ch;
              break;
            }
          }
            break;
        case 2:
          for (t = 0; t < 0xFBBB; t++)//��ѯָ������
          {
            if(R8_UART2_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART2_THR = ch;
              break;
            }
          }
            break;
        case 3:
          for (t = 0; t < 0xFBBB; t++)//��ѯָ������
          {
            if(R8_UART3_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART3_THR = ch;
              break;
            }
          }
            break;
        default:
            break;
    }

    if (t >= 0xFBBB)
        return 0; //���ͳ�ʱ������ʧ��
    else
        return 1; //�ɹ�����
}

//======================================================================
//�������ƣ�uart_sendN
//����˵����uartNo: ���ں�:UART_1��UART_2��UART_3
//         buff: ���ͻ�����
//         len:���ͳ���
//�������أ� ����ִ��״̬��1=���ͳɹ���0=����ʧ��
//���ܸ�Ҫ������ ����n���ֽ�
//======================================================================
uint8_t uart_sendN(uint8_t uartNo ,uint16_t len ,uint8_t* buff)
{
    uint16_t i;
    //�жϴ��ںŲ����Ƿ���������ֱ���˳�
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }

    switch (uartNo)
        {
        case 0:
            while(len)
            {
                if(R8_UART0_TFC != UART_FIFO_SIZE)
                {
                    for(int j = 0; j < 200; j++) __asm("nop");
                    R8_UART0_THR = *buff++;
                    len--;
                }
            }
           return 1;

            break;
        case 1:
            while(len)
            {
               if(R8_UART1_TFC != UART_FIFO_SIZE)
              {
                  for(int j = 0; j < 200; j++) __asm("nop");
                  R8_UART1_THR = *buff++;
                  len--;
              }
            }
            return 1;

         break;
        case 2:
            while(len)
            {
                if(R8_UART2_TFC != UART_FIFO_SIZE)
                {
                    for(int j = 0; j < 200; j++) __asm("nop");
                    R8_UART2_THR = *buff++;
                    len--;
                }
            }
            return 1;

            break;
        case 3:
            while(len)
            {
                if(R8_UART3_TFC != UART_FIFO_SIZE)
                {
                    for(int j = 0; j < 200; j++) __asm("nop");
                    R8_UART3_THR = *buff++;
                    len--;
                }
            }
            return 1;

            break;
        default:
            break;

    }
    if(i<len)
    {
        return 0;   //���ͳ���
    }
    else
        return 1;  //���ͳɹ�


}

//======================================================================
//�������ƣ�uart_send_string
//����˵����uartNo:UARTģ���:UART_1��UART_2��UART_3
//          buff:Ҫ���͵��ַ������׵�ַ
//�������أ� ����ִ��״̬��1=���ͳɹ���0=����ʧ��
//���ܸ�Ҫ����ָ��UART�˿ڷ���һ����'\0'�������ַ���
//======================================================================
__attribute__((section (".update"))) uint8_t uart_send_string(uint8_t uartNo, uint8_t* buff)
{
    uint16_t len;

    //�жϴ��봮�ںŲ����Ƿ���������ֱ���˳�
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }
    //����ָ��ָ����ַ�����ĳ��ȣ����ﲻ����sizeof����Ϊsizeof(ָ��)���õ���ָ�볤��Ϊ4
    len=strlen(buff);

    switch (uartNo)
    {
        case 0:
            while(len)
            {
                if(R8_UART0_TFC != UART_FIFO_SIZE)
                {
                    R8_UART0_THR = *buff++;
                    len--;
                }
            }
           return 1;

            break;
        case 1:
            while(len)
            {
               if(R8_UART1_TFC != UART_FIFO_SIZE)
              {

                  R8_UART1_THR = *buff++;
                  len--;
              }
            }
            return 1;

         break;
        case 2:
            while(len)
            {
                if(R8_UART2_TFC != UART_FIFO_SIZE)
                {
                    R8_UART2_THR = *buff++;
                    len--;
                }
            }
            return 1;

            break;
        case 3:
            while(len)
            {
                if(R8_UART3_TFC != UART_FIFO_SIZE)
                {
                    R8_UART3_THR = *buff++;
                    len--;
                }
            }
            return 1;

            break;
        default:
            break;

    }
    return 1;
            //���ͳɹ�
}

//======================================================================
//�������ƣ�uart_re1
//����˵����uartNo: ���ں�:UART_1��UART_2��UART_3
//        *fp:���ճɹ���־��ָ��:*fp=1:���ճɹ���*fp=0:����ʧ��
//�������أ����շ����ֽ�
//���ܸ�Ҫ�����н���1���ֽ�
//======================================================================
uint8_t uart_re1(uint8_t uartNo,uint8_t *fp)
{
    uint8_t dat;

    switch (uartNo)
    {
        case 0:
            *fp =1;
            return R8_UART0_RBR;
            break;
        case 1:
            *fp =1;
            dat = R8_UART1_RBR;
            break;
        case 2:
            *fp =1;
            dat = R8_UART2_RBR;
            break;
        case 3:
            *fp =1;
            dat = R8_UART3_RBR;
            break;
        default:
            break;
    }
    return (dat);
}

//======================================================================
//�������ƣ�uart_reN
//����˵����uartNo: ���ں�:UART_1��UART_2��UART_3
//          buff: ���ջ�����
//          len:���ճ���
//�������أ�����ִ��״̬ 1=���ճɹ�;0=����ʧ��
//���ܸ�Ҫ������ ����n���ֽ�,����buff��
//======================================================================
uint8_t uart_reN(uint8_t uartNo ,uint16_t len ,uint8_t* buff)
{
    uint16_t i;
    uint8_t flag = 0;

    //�жϴ��봮�ںŲ����Ƿ���������ֱ���˳�
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }

    //�ж��Ƿ��ܽ�������
    for (i = 0; i < len; i++)
    {
        buff[i] = uart_re1(uartNo, &flag); //��������
//        PRINT("%c",buff[i]);
    }
    if (i < len)
        return 0; //����ʧ��
    else
        return 1; //���ճɹ�

}

//======================================================================
//�������ƣ�uart_enable_re_int
//����˵����uartNo: ���ں�:UART_1��UART_2��UART_3
//�������أ���
//���ܸ�Ҫ�������ڽ����ж�
//======================================================================
void uart_enable_re_int(uint8_t uartNo)
{
    //�жϴ��봮�ںŲ����Ƿ���������ֱ���˳�
    if(!uart_is_uartNo(uartNo))
    {
        return;
    }

    switch (uartNo)
    {
        case 0:
            R8_UART0_IER |= (RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            R8_UART0_MCR |= RB_MCR_INT_OE;
            PFIC_EnableIRQ( UART0_IRQn);
            break;
        case 1:
            R8_UART1_FCR = (R8_UART1_FCR&~RB_FCR_FIFO_TRIG)|(3<<6);

            R8_UART1_IER |= (RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            R8_UART1_MCR |= RB_MCR_INT_OE;
            PFIC_EnableIRQ( UART1_IRQn );
            break;
        case 2:
            R8_UART2_IER |= (RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            R8_UART2_MCR |= RB_MCR_INT_OE;
            PFIC_EnableIRQ( UART2_IRQn );
            break;
        case 3:
            R8_UART3_IER |= (RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            R8_UART3_MCR |= RB_MCR_INT_OE;
            PFIC_EnableIRQ( UART3_IRQn );
            break;

        default:
            break;
    }

}

//======================================================================
//�������ƣ�uart_disable_re_int
//����˵����uartNo: ���ں� :UART_1��UART_2��UART_3
//�������أ���
//���ܸ�Ҫ���ش��ڽ����ж�
//======================================================================
void uart_disable_re_int(uint8_t uartNo)
{
    //�жϴ��봮�ںŲ����Ƿ���������ֱ���˳�
    if(!uart_is_uartNo(uartNo))
    {
        return;
    }

    switch (uartNo)
    {
        case 0:
            R8_UART0_IER &= ~(RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            PFIC_DisableIRQ( UART0_IRQn);
            break;
        case 1:
            R8_UART1_IER &= ~(RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            PFIC_DisableIRQ( UART1_IRQn );
            break;
        case 2:
            R8_UART2_IER &= ~(RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            PFIC_DisableIRQ( UART2_IRQn );
            break;
        case 3:
            R8_UART3_IER &= ~(RB_IER_RECV_RDY|RB_IER_LINE_STAT);
            PFIC_DisableIRQ( UART3_IRQn );
            break;
        default:
            break;
    }

}


//======================================================================
//�������ƣ�uart_get_re_int
//����˵����uartNo: ���ں� :UART_1��UART_2��UART_3
//�������أ�1:û���жϲ�����6��������·״̬�жϣ�4���������ݿ����жϣ�
//          12���������ݳ�ʱ�жϣ�2��THR�Ĵ���Ϊ���жϣ�0��MODEM����仯�ж�
//���ܸ�Ҫ����ȡ���ڽ����жϱ�־,ͬʱ���÷����ж�
//======================================================================
uint8_t uart_get_re_int(uint8_t uartNo)
{
    uint8_t event;
    uint8_t flag;
    //�жϴ��봮�ںŲ����Ƿ���������ֱ���˳�
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }
    //��ȡ��ǰ�����жϱ�־��
    switch (uartNo)
    {
        case 0:
            event = R8_UART0_IIR&RB_IIR_INT_MASK;
           switch (event)
           {
            case UART_II_LINE_STAT:
                flag = 6;
                break;
            case UART_II_RECV_RDY:
                flag = 4;
                break;
            case UART_II_RECV_TOUT:
                flag = 12;
                break;
            case UART_II_THR_EMPTY:
                flag = 2;
                break;
            case UART_II_MODEM_CHG:
                flag = 0;
                break;
            case UART_II_NO_INTER:
                flag = 1;
                break;
            default:
                break;
          }
            break;
        case 1:
            event = R8_UART1_IIR&RB_IIR_INT_MASK;
           switch (event)
           {
            case UART_II_LINE_STAT:
                flag = 6;
                break;
            case UART_II_RECV_RDY:
                flag = 4;
                break;
            case UART_II_RECV_TOUT:
                flag = 12;
                break;
            case UART_II_THR_EMPTY:
                flag = 2;
                break;
            case UART_II_MODEM_CHG:
                flag = 0;
                break;
            case UART_II_NO_INTER:
                flag = 1;
                break;
            default:
                break;
           }

            break;
        case 2:
           event = R8_UART2_IIR&RB_IIR_INT_MASK;
           switch (event)
           {
            case UART_II_LINE_STAT:
                flag = 6;
                break;
            case UART_II_RECV_RDY:
                flag = 4;
                break;
            case UART_II_RECV_TOUT:
                flag = 12;
                break;
            case UART_II_THR_EMPTY:
                flag = 2;
                break;
            case UART_II_MODEM_CHG:
                flag = 0;
                break;
            case UART_II_NO_INTER:
                flag = 1;
                break;
           default:
               break;
          }
            break;
       case 3:
          event = R8_UART3_IIR&RB_IIR_INT_MASK;
          switch (event)
          {
           case UART_II_LINE_STAT:
               flag = 6;
               break;
           case UART_II_RECV_RDY:
               flag = 4;
               break;
           case UART_II_RECV_TOUT:
               flag = 12;
               break;
           case UART_II_THR_EMPTY:
               flag = 2;
               break;
           case UART_II_MODEM_CHG:
               flag = 0;
               break;
           case UART_II_NO_INTER:
               flag = 1;
               break;
          default:
              break;
         }
           break;
    default:
        break;
    }
    return flag;
}

//======================================================================
//�������ƣ�uart_deinit
//����˵����uartNo: ���ں� :UART_1��UART_2��UART_3
//�������أ���
//���ܸ�Ҫ��uart����ʼ�����رմ���ʱ��
//======================================================================
void uart_deinit(uint8_t uartNo)
{
    switch (uartNo)
    {
      case UART_0:    // ����0
#ifdef UART0_GROUP
          //
          switch (UART0_GROUP)
          {
              case 0:
                 R32_PB_OUT &= ~((uint32_t)(bTXD0));
                 //PB4->RX
                 R32_PB_PD_DRV &= ~((uint32_t)(bRXD0));
                 R32_PB_PU     &= ~((uint32_t)(bRXD0));
                 R32_PB_DIR    &= ~((uint32_t)(bRXD0));
                 //PB7->TX
                 R32_PB_PD_DRV &= ~((uint32_t)(bTXD0));
                 R32_PB_DIR    &= ~((uint32_t)(bTXD0));

                 break;
              case 1:
                  //uart0����ӳ��
                R16_PIN_ALTERNATE &= ~RB_PIN_UART0;

                R32_PA_OUT &= ~((uint32_t)(bTXD0_));
                //PA15->RX
                R32_PA_PD_DRV &= ~((uint32_t)(bRXD0_));
                R32_PA_PU     &= ~((uint32_t)(bRXD0_));
                R32_PA_DIR    &= ~((uint32_t)(bRXD0_));
                //PA14->TX
                R32_PA_PD_DRV &= ~((uint32_t)(bTXD0_));
                R32_PA_DIR    &= ~((uint32_t)(bTXD0_));

                break;
              default:
                  break;
          }
#endif
          break;
      case UART_1:
#ifdef UART1_GROUP
          //
          switch (UART1_GROUP)
          {
              case 0:

                  R32_PA_OUT &= ~((uint32_t)(bTXD1));
                  //PA8->RX
                  R32_PA_PD_DRV &= ~((uint32_t)(bRXD1));
                  R32_PA_PU     &= ~((uint32_t)(bRXD1));
                  R32_PA_DIR    &= ~((uint32_t)(bRXD1));

                  //PA9->TX
                  R32_PA_PD_DRV &= ~((uint32_t)(bTXD1));
                  R32_PA_DIR    &= ~((uint32_t)(bTXD1));

                  break;
              case 1:
                  //uart1����ӳ��
                  R16_PIN_ALTERNATE &= ~RB_PIN_UART1;

                  R32_PB_OUT &= ~((uint32_t)(bTXD1_));
                  //PB12->RX
                  R32_PB_PD_DRV &= ~((uint32_t)(bRXD1_));
                  R32_PB_PU     &= ~((uint32_t)(bRXD1_));
                  R32_PB_DIR    &= ~((uint32_t)(bRXD1_));
                  //PB13->TX
                  R32_PB_PD_DRV &= ~((uint32_t)(bTXD1_));
                  R32_PB_DIR    &= ~((uint32_t)(bTXD1_));

                  break;
              default:
                  break;
          }
#endif
          break;
      case UART_2:

            R32_PB_OUT &= ~((uint32_t)(bTXD2));
            //PB22->RX
            R32_PB_PD_DRV &= ~((uint32_t)(bRXD2));
            R32_PB_PU     &= ~((uint32_t)(bRXD2));
            R32_PB_DIR    &= ~((uint32_t)(bRXD2));
            //PB23->TX
            R32_PB_PD_DRV &= ~((uint32_t)(bTXD2));
            R32_PB_DIR    &= ~((uint32_t)(bTXD2));

            break;
      case UART_3:

              R32_PA_OUT &= ~((uint32_t)(bTXD3));
              //PA4->RX
              R32_PA_PD_DRV &= ~((uint32_t)(bRXD3));
              R32_PA_PU     &= ~((uint32_t)(bRXD3));
              R32_PA_DIR    &= ~((uint32_t)(bRXD3));
              //PA5->TX
              R32_PA_PD_DRV &= ~((uint32_t)(bTXD3));
              R32_PA_DIR    &= ~((uint32_t)(bTXD3));

              break;
      default:
          break;
  }

}





//----------------------����Ϊ�ڲ�������Ŵ�--------------------------------------
//=====================================================================
//�������ƣ�uart_is_uartNo
//�������أ�1:���ں��ں���Χ�ڣ�0�����ںŲ�����
//����˵�������ں�uartNo  :UART_1��UART_2��UART_3
//���ܸ�Ҫ��Ϊ����׳�Զ��ж�uartNo�Ƿ��ڴ������ַ�Χ��
//=====================================================================
__attribute__((section (".update"))) uint8_t uart_is_uartNo(uint8_t uartNo)
{
    if(uartNo < UART_0 || uartNo > UART_3)
        return 0;
    else
        return 1;
}
