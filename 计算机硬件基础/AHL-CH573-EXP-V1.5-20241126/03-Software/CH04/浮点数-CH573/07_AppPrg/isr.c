//=====================================================================
// 文件名称：isr.c（中断处理程序源文件）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 更新记录：20230914
// 功能描述：提供中断处理程序编程框架
// 移植规则：【固定】
//=====================================================================
#include <includes.h>

void rf_Receive(uint8_t *rxBuf );

//======================================================================
// 程序名称：UART_User_Handler
// 触发条件：UART_User串口收到一个字节触发
// 备   注：进入本程序后，可使用uart_get_re_int函数可再进行中断标志判断
//          （1-有UART接收中断，0-没有UART接收中断）
//======================================================================
void UART_User_Handler(void)
{
    //【1】关中断
    DISABLE_INTERRUPTS;

    //【2】声明临时变量
    uint8_t flag,ch;

    //【3】判断是否为本中断触发
    if (!uart_get_re_int(UART_User)) goto UART_User_Handler_exit;

    //【4】确证是本中断触发，读取接到的字节赋给变量ch，flag是收到数据标志
    ch=uart_re1(UART_User,&flag);   //调用接收一个字节的函数，清接收中断位

    //【5】根据flag判断是否真正收到一个字节的数据
    if (flag)                       //有数据
    {
        uart_send1(UART_User,ch);   //回发接收到的字节
    }
	
	if(ch == 'A')
	{
		__asm("li t0, 0x8");
		__asm("csrc mstatus, t0");
	}
	
    // 【6】开中断
UART_User_Handler_exit:
    ENABLE_INTERRUPTS;
}

//=========================================================================
//函数名称：RF_2G4StatusCallBack
//功能概要：RF 状态回调，注意：不可在此函数中直接调用RF接收或者发送API，需要使用事件的方式调用
//          该函数在处理完相应事件（例如发送完成）或是接收到数据时触发
//参数说明：sta：状态类型，对应触发该回调函数的原因
//          crc：数据接收，接收到的数据是否通过了crc校验，0-crc校验通过，1,2-crc校验不通过
//         rxBuf：接收到的数据，其中rxBuf[0]为本次传输数据强度，rxBuf[1]为传输的数据长度
//函数返回：无
//=========================================================================
void RF_2G4StatusCallBack(uint8_t sta, uint8_t crc, uint8_t *rxBuf)
{
    switch(sta)
    {
        case TX_MODE_TX_FINISH:     //发送结束
        {
            uint8_t state;
            // //关闭rf状态
            RF_Shut();
            // //设置rf为接收模式
            state = RF_Rx( data,10, 0xFF, 0xFF );
            break;
        }
        case TX_MODE_TX_FAIL:
        {
            break;
        }
        case TX_MODE_RX_DATA:
        {
            if (crc == 0) {
                uint8_t i;

                PRINT("tx recv,rssi:%d\n", (int8_t)rxBuf[0]);
                PRINT("len:%d-", rxBuf[1]);

                for (i = 0; i < rxBuf[1]; i++) {
                    PRINT("%x ", rxBuf[i + 2]);
                }
                PRINT("\n");
            } else {
                if (crc & (1<<0)) {
                    PRINT("crc error\n");
                }

                if (crc & (1<<1)) {
                    PRINT("match type error\n");
                }
            }
            break;
        }
        case TX_MODE_RX_TIMEOUT: // Timeout is about 200us
        {
            break;
        }
        case RX_MODE_RX_DATA:
        {
        //(2.1)判断crc检验是否成功
        if (( crc == 1 ) || ( crc == 2 ))  //crc校验出错
        {
            __asm ("nop");
            printf("crc error\r\n");
        }
        //(2.2)CRC校验成功
        else
        {
            printf("recv_data\r\n");
            rf_Receive(rxBuf);              //接收数据处理函数
        }
        tmos_set_event(rftaskID, SBP_RF_RF_RX_EVT);
        break;
        }
        case RX_MODE_TX_FINISH:
        {
            tmos_set_event(rftaskID, SBP_RF_RF_RX_EVT);
            break;
        }
        case RX_MODE_TX_FAIL:
        {
            break;
        }
    }
    // PRINT("STA: %x\n", sta);
}

//=========================================================================
//函数名称：RF_ProcessEvent
//功能概要：RF 事件处理，响应tmos_set_event，通过该函数设置的任务将在本函数内执行对应操作
//参数说明：task_id：任务ID
//          events：事件标志
//函数返回：未完成事件
//=========================================================================
uint16_t RF_ProcessEvent(uint8_t task_id, uint16_t events)
{
    if(events & SYS_EVENT_MSG)
    {
        uint8_t *pMsg;

        if((pMsg = tmos_msg_receive(task_id)) != NULL)
        {
            // Release the TMOS message
            tmos_msg_deallocate(pMsg);
        }
        // return unprocessed events
        return (events ^ SYS_EVENT_MSG);
    }
    if(events & SBP_RF_START_DEVICE_EVT)
    {
        tmos_start_task(rftaskID, SBP_RF_PERIODIC_EVT, 1000);
        return events ^ SBP_RF_START_DEVICE_EVT;
    }
    if(events & SBP_RF_PERIODIC_EVT)
    {
        tmos_start_task( rftaskID , SBP_RF_PERIODIC_EVT ,1000 );
        return events^SBP_RF_START_DEVICE_EVT;
    }
    if(events & SBP_RF_RF_RX_EVT)
    {
        uint8 state;
        RF_Shut();
        state = RF_Rx( data,10, 0xFF, 0xFF );
        return events^SBP_RF_RF_RX_EVT;
    }
    if( events & RF_TX_TEST_EVENT )      //发送一帧数据
    {
        uint8_t len;
        RF_Shut();
        len = rflength;
        if(len)
        {
            RF_Tx( (uint8_t *)&rfsend, len, 0xff, 0xff );
        }
        return (events^RF_TX_TEST_EVENT);
    }
    return 0;
}

//=========================================================================
//函数名称：rf_Receive
//功能概要：接收数据处理函数
//参数说明：rxBuf:接收到的数据
//          rxBuf[0]:信号强度
//          rxBuf[1]:数据长度
//函数返回：接收状态标志位（=0,接收正常，=其他值，接收异常）
//=========================================================================
void rf_Receive(uint8_t *rxBuf )
{
    uint16_t len = rxBuf[1];

    if(rxBuf[2] == 'S' && rxBuf[3] == 'C' && rxBuf[len+1] == 'H')   //收到的数据为更新请求
    {
        uint8_t *addr_WaitForUpdate;
        addr_WaitForUpdate = (uint8_t *)(component_fun[48]);
        *addr_WaitForUpdate = 1;        //将从机等待更新标志位置1
        NVIC_SystemReset();             //系统重启
    }
    else
    {
        for(uint8_t i = 0; i < rxBuf[1]; i++)
            uart_send1(UART_Debug, rxBuf[i+2]);
        printf("\r\n");
    }
}