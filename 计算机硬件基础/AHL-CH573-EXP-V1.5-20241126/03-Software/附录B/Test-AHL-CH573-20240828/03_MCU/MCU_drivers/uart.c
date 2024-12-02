//======================================================================
//文件名称：uart.c
//功能概要：uart底层驱动构件源文件
//版权所有：苏州大学嵌入式系统与物联网研究所(sumcu.suda.edu.cn)
//更新记录：2020-0918 V1.0 GXY
//======================================================================

#include "uart.h"


//====定义串口IRQ号对应表====
__attribute__((section (".other"))) IRQn_Type table_irq_uart[4]= {UART0_IRQn, UART1_IRQn, UART2_IRQn, UART3_IRQn};



//内部函数声明
__attribute__((section (".update"))) uint8_t uart_is_uartNo(uint8_t uartNo);

//======================================================================
//函数名称：uart_init
//功能概要：初始化uart模块
//参数说明：uartNo:串口号：UART_1、UART_2、UART_3
//          baud:波特率：300、600、1200、2400、4800、9600、19200、115200...
//函数返回：无
//======================================================================
void uart_init(uint8_t uartNo, uint32_t baud_rate)
{
    uint32_t x;

//    SetSysClock( CLK_SOURCE_PLL_60MHz );
    switch (uartNo)
    {
    case UART_0:    // 串口0
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

                   R8_UART0_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;     // FIFO打开，触发点4字节
                   R8_UART0_LCR = RB_LCR_WORD_SZ;
                   R8_UART0_IER = RB_IER_TXD_EN;
                   R8_UART0_DIV = 1;

                   //配置波特率115200
                   x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                   x = (x + 5) / 10;
                   R16_UART0_DL = (uint16_t)x;
                   break;
                case 1:
                    //uart0引脚映射
                  R16_PIN_ALTERNATE |= RB_PIN_UART0;

                  R32_PA_OUT |= ((uint32_t)(bTXD0_));
                  //PA15->RX
                  R32_PA_PD_DRV &= ~((uint32_t)(bRXD0_));
                  R32_PA_PU     |= ((uint32_t)(bRXD0_));
                  R32_PA_DIR    &= ~((uint32_t)(bRXD0_));
                  //PA14->TX
                  R32_PA_PD_DRV &= ~((uint32_t)(bTXD0_));
                  R32_PA_DIR    |= ((uint32_t)(bTXD0_));

                  R8_UART0_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;     // FIFO打开，触发点4字节
                  R8_UART0_LCR = RB_LCR_WORD_SZ;
                  R8_UART0_IER = RB_IER_TXD_EN;
                  R8_UART0_DIV = 1;

                  //配置波特率115200
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

                    R8_UART1_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;       // FIFO打开，触发点4字节
                    R8_UART1_LCR = RB_LCR_WORD_SZ;
                    R8_UART1_IER = RB_IER_TXD_EN;
                    R8_UART1_DIV = 1;

                    //配置波特率115200
                    x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                    x = (x + 5) / 10;
                    R16_UART1_DL = (uint16_t)x;
                    break;
                case 1:
                    //uart1引脚映射
                    R16_PIN_ALTERNATE |= RB_PIN_UART1;

                    R32_PB_OUT |= ((uint32_t)(bTXD1_));
                    //PB12->RX
                    R32_PB_PD_DRV &= ~((uint32_t)(bRXD1_));
                    R32_PB_PU     |= ((uint32_t)(bRXD1_));
                    R32_PB_DIR    &= ~((uint32_t)(bRXD1_));
                    //PB13->TX
                    R32_PB_PD_DRV &= ~((uint32_t)(bTXD1_));
                    R32_PB_DIR    |= ((uint32_t)(bTXD1_));

                    R8_UART1_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;       // FIFO打开，触发点4字节
                    R8_UART1_LCR = RB_LCR_WORD_SZ;
                    R8_UART1_IER = RB_IER_TXD_EN;
                    R8_UART1_DIV = 1;

                    //配置波特率115200
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

              R8_UART2_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;      // FIFO打开，触发点4字节
              R8_UART2_LCR = RB_LCR_WORD_SZ;
              R8_UART2_IER = RB_IER_TXD_EN;
              R8_UART2_DIV = 1;

              //配置波特率115200
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

                R8_UART3_FCR = (2<<6) | RB_FCR_TX_FIFO_CLR | RB_FCR_RX_FIFO_CLR | RB_FCR_FIFO_EN;       // FIFO打开，触发点4字节
                R8_UART3_LCR = RB_LCR_WORD_SZ;
                R8_UART3_IER = RB_IER_TXD_EN;
                R8_UART3_DIV = 1;

               //配置波特率115200
                x = 10 * (480000000/(0x08)) / 8 / baud_rate;
                x = (x + 5) / 10;
                R16_UART3_DL = (uint16_t)x;
                break;
        default:
            break;
    }

}

//======================================================================
//函数名称：uart_send1
//参数说明：uartNo: 串口号:UART_1、UART_2、UART_3
//          ch:要发送的字节
//函数返回：函数执行状态：1=发送成功；0=发送失败。
//功能概要：串行发送1个字节
//======================================================================
__attribute__((section (".update"))) uint8_t uart_send1(uint8_t uartNo,uint8_t ch)
{
  uint32_t t;

    //判断传入串口号参数是否有误，有误直接退出
    if(uart_is_uartNo(uartNo)==0)
    {
        return 0;
    }
    //判断UARTx号
    switch (uartNo)
    {
        case 0:
          for (t = 0; t < 0xFBBB; t++)//查询指定次数
          {
            if(R8_UART0_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART0_THR = ch;
              break;
            }
          }
            break;
        case 1:
          for (t = 0; t < 0xFBBB; t++)//查询指定次数
          {
            if(R8_UART1_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART1_THR = ch;
              break;
            }
          }
            break;
        case 2:
          for (t = 0; t < 0xFBBB; t++)//查询指定次数
          {
            if(R8_UART2_LSR &RB_LSR_TX_FIFO_EMP){
              R8_UART2_THR = ch;
              break;
            }
          }
            break;
        case 3:
          for (t = 0; t < 0xFBBB; t++)//查询指定次数
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
        return 0; //发送超时，发送失败
    else
        return 1; //成功发送
}

//======================================================================
//函数名称：uart_sendN
//参数说明：uartNo: 串口号:UART_1、UART_2、UART_3
//         buff: 发送缓冲区
//         len:发送长度
//函数返回： 函数执行状态：1=发送成功；0=发送失败
//功能概要：串行 接收n个字节
//======================================================================
uint8_t uart_sendN(uint8_t uartNo ,uint16_t len ,uint8_t* buff)
{
    uint16_t i;
    //判断串口号参数是否有误，有误直接退出
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
        return 0;   //发送出错
    }
    else
        return 1;  //发送成功


}

//======================================================================
//函数名称：uart_send_string
//参数说明：uartNo:UART模块号:UART_1、UART_2、UART_3
//          buff:要发送的字符串的首地址
//函数返回： 函数执行状态：1=发送成功；0=发送失败
//功能概要：从指定UART端口发送一个以'\0'结束的字符串
//======================================================================
__attribute__((section (".update"))) uint8_t uart_send_string(uint8_t uartNo, uint8_t* buff)
{
    uint16_t len;

    //判断传入串口号参数是否有误，有误直接退出
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }
    //计算指针指向的字符数组的长度，这里不能用sizeof，因为sizeof(指针)，得到的指针长度为4
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
            //发送成功
}

//======================================================================
//函数名称：uart_re1
//参数说明：uartNo: 串口号:UART_1、UART_2、UART_3
//        *fp:接收成功标志的指针:*fp=1:接收成功；*fp=0:接收失败
//函数返回：接收返回字节
//功能概要：串行接收1个字节
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
//函数名称：uart_reN
//参数说明：uartNo: 串口号:UART_1、UART_2、UART_3
//          buff: 接收缓冲区
//          len:接收长度
//函数返回：函数执行状态 1=接收成功;0=接收失败
//功能概要：串行 接收n个字节,放入buff中
//======================================================================
uint8_t uart_reN(uint8_t uartNo ,uint16_t len ,uint8_t* buff)
{
    uint16_t i;
    uint8_t flag = 0;

    //判断传入串口号参数是否有误，有误直接退出
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }

    //判断是否能接受数据
    for (i = 0; i < len; i++)
    {
        buff[i] = uart_re1(uartNo, &flag); //接受数据
//        PRINT("%c",buff[i]);
    }
    if (i < len)
        return 0; //接收失败
    else
        return 1; //接收成功

}

//======================================================================
//函数名称：uart_enable_re_int
//参数说明：uartNo: 串口号:UART_1、UART_2、UART_3
//函数返回：无
//功能概要：开串口接收中断
//======================================================================
void uart_enable_re_int(uint8_t uartNo)
{
    //判断传入串口号参数是否有误，有误直接退出
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
//函数名称：uart_disable_re_int
//参数说明：uartNo: 串口号 :UART_1、UART_2、UART_3
//函数返回：无
//功能概要：关串口接收中断
//======================================================================
void uart_disable_re_int(uint8_t uartNo)
{
    //判断传入串口号参数是否有误，有误直接退出
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
//函数名称：uart_get_re_int
//参数说明：uartNo: 串口号 :UART_1、UART_2、UART_3
//函数返回：1:没有中断产生，6：接收线路状态中断，4：接收数据可用中断，
//          12：接收数据超时中断，2：THR寄存器为空中断，0：MODEM输入变化中断
//功能概要：获取串口接收中断标志,同时禁用发送中断
//======================================================================
uint8_t uart_get_re_int(uint8_t uartNo)
{
    uint8_t event;
    uint8_t flag;
    //判断传入串口号参数是否有误，有误直接退出
    if(!uart_is_uartNo(uartNo))
    {
        return 0;
    }
    //获取当前接收中断标志，
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
//函数名称：uart_deinit
//参数说明：uartNo: 串口号 :UART_1、UART_2、UART_3
//函数返回：无
//功能概要：uart反初始化，关闭串口时钟
//======================================================================
void uart_deinit(uint8_t uartNo)
{
    switch (uartNo)
    {
      case UART_0:    // 串口0
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
                  //uart0引脚映射
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
                  //uart1引脚映射
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





//----------------------以下为内部函数存放处--------------------------------------
//=====================================================================
//函数名称：uart_is_uartNo
//函数返回：1:串口号在合理范围内，0：串口号不合理
//参数说明：串口号uartNo  :UART_1、UART_2、UART_3
//功能概要：为程序健壮性而判断uartNo是否在串口数字范围内
//=====================================================================
__attribute__((section (".update"))) uint8_t uart_is_uartNo(uint8_t uartNo)
{
    if(uartNo < UART_0 || uartNo > UART_3)
        return 0;
    else
        return 1;
}
