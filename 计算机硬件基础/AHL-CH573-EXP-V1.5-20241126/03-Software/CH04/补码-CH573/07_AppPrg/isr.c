//=====================================================================
// �ļ����ƣ�isr.c���жϴ������Դ�ļ���
// ����ṩ���մ�Ƕ��ʽ��sumcu.suda.edu.cn��
// ���¼�¼��20230914
// �����������ṩ�жϴ�������̿��
// ��ֲ���򣺡��̶���
//=====================================================================
#include <includes.h>

void rf_Receive(uint8_t *rxBuf );

//======================================================================
// �������ƣ�UART_User_Handler
// ����������UART_User�����յ�һ���ֽڴ���
// ��   ע�����뱾����󣬿�ʹ��uart_get_re_int�������ٽ����жϱ�־�ж�
//          ��1-��UART�����жϣ�0-û��UART�����жϣ�
//======================================================================
void UART_User_Handler(void)
{
    //��1�����ж�
    DISABLE_INTERRUPTS;

    //��2��������ʱ����
    uint8_t flag,ch;

    //��3���ж��Ƿ�Ϊ���жϴ���
    if (!uart_get_re_int(UART_User)) goto UART_User_Handler_exit;

    //��4��ȷ֤�Ǳ��жϴ�������ȡ�ӵ����ֽڸ�������ch��flag���յ����ݱ�־
    ch=uart_re1(UART_User,&flag);   //���ý���һ���ֽڵĺ�����������ж�λ

    //��5������flag�ж��Ƿ������յ�һ���ֽڵ�����
    if (flag)                       //������
    {
        uart_send1(UART_User,ch);   //�ط����յ����ֽ�
    }
	
	if(ch == 'A')
	{
		__asm("li t0, 0x8");
		__asm("csrc mstatus, t0");
	}
	
    // ��6�����ж�
UART_User_Handler_exit:
    ENABLE_INTERRUPTS;
}

//=========================================================================
//�������ƣ�RF_2G4StatusCallBack
//���ܸ�Ҫ��RF ״̬�ص���ע�⣺�����ڴ˺�����ֱ�ӵ���RF���ջ��߷���API����Ҫʹ���¼��ķ�ʽ����
//          �ú����ڴ�������Ӧ�¼������緢����ɣ����ǽ��յ�����ʱ����
//����˵����sta��״̬���ͣ���Ӧ�����ûص�������ԭ��
//          crc�����ݽ��գ����յ��������Ƿ�ͨ����crcУ�飬0-crcУ��ͨ����1,2-crcУ�鲻ͨ��
//         rxBuf�����յ������ݣ�����rxBuf[0]Ϊ���δ�������ǿ�ȣ�rxBuf[1]Ϊ��������ݳ���
//�������أ���
//=========================================================================
void RF_2G4StatusCallBack(uint8_t sta, uint8_t crc, uint8_t *rxBuf)
{
    switch(sta)
    {
        case TX_MODE_TX_FINISH:     //���ͽ���
        {
            uint8_t state;
            // //�ر�rf״̬
            RF_Shut();
            // //����rfΪ����ģʽ
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
        //(2.1)�ж�crc�����Ƿ�ɹ�
        if (( crc == 1 ) || ( crc == 2 ))  //crcУ�����
        {
            __asm ("nop");
            printf("crc error\r\n");
        }
        //(2.2)CRCУ��ɹ�
        else
        {
            printf("recv_data\r\n");
            rf_Receive(rxBuf);              //�������ݴ�����
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
//�������ƣ�RF_ProcessEvent
//���ܸ�Ҫ��RF �¼�������Ӧtmos_set_event��ͨ���ú������õ������ڱ�������ִ�ж�Ӧ����
//����˵����task_id������ID
//          events���¼���־
//�������أ�δ����¼�
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
    if( events & RF_TX_TEST_EVENT )      //����һ֡����
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
//�������ƣ�rf_Receive
//���ܸ�Ҫ���������ݴ�����
//����˵����rxBuf:���յ�������
//          rxBuf[0]:�ź�ǿ��
//          rxBuf[1]:���ݳ���
//�������أ�����״̬��־λ��=0,����������=����ֵ�������쳣��
//=========================================================================
void rf_Receive(uint8_t *rxBuf )
{
    uint16_t len = rxBuf[1];

    if(rxBuf[2] == 'S' && rxBuf[3] == 'C' && rxBuf[len+1] == 'H')   //�յ�������Ϊ��������
    {
        uint8_t *addr_WaitForUpdate;
        addr_WaitForUpdate = (uint8_t *)(component_fun[48]);
        *addr_WaitForUpdate = 1;        //���ӻ��ȴ����±�־λ��1
        NVIC_SystemReset();             //ϵͳ����
    }
    else
    {
        for(uint8_t i = 0; i < rxBuf[1]; i++)
            uart_send1(UART_Debug, rxBuf[i+2]);
        printf("\r\n");
    }
}