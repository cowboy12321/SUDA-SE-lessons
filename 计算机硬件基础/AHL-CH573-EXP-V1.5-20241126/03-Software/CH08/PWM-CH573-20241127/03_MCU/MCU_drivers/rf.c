#ifndef RF_C_
#define RF_C_

#include "rf.h"
#include "printf.h"

extern uint16_t RF_ProcessEvent( uint8_t task_id, uint16_t events );
extern void RF_2G4StatusCallBack( uint8_t sta , uint8_t crc, uint8_t *rxBuf );

//=========================================================================
//�������ƣ�RF_Init
//���ܸ�Ҫ��RF��ʼ��,Ĭ�ϳ�ʼ��Ϊ����״̬
//����˵����HW_ADR:32λ��ַ����ֹʹ��0x55555555�Լ�0xAAAAAAAA (
//         ���鲻����24��λ��ת���Ҳ�����������6��0��1 )
//�������أ���
//=========================================================================
void RF_Init(uint32_t HW_ADR)
{
    uint8_t state;
    rfConfig_t rfConfig;
    CH57X_BLEInit( ); //BLE��ʼ��
    HAL_Init(  );     //Ӳ����ʼ��
    RF_RoleInit( );
    rftaskID = TMOS_ProcessEventRegister( RF_ProcessEvent );  //�����ʼ��
    rfConfig.accessAddress = HW_ADR;
    rfConfig.CRCInit = 0x555555;
    rfConfig.Channel = 8;
    rfConfig.Frequency = 2480000;
    rfConfig.LLEMode = LLE_MODE_BASIC|LLE_MODE_EX_CHANNEL; // ʹ�� LLE_MODE_EX_CHANNEL ��ʾ ѡ�� rfConfig.Frequency ��Ϊͨ��Ƶ��
    rfConfig.rfStatusCB = RF_2G4StatusCallBack;
    state = RF_Config( &rfConfig );
    // printf("RFInitState: %x\n",state);   //0-success
    // WWDG_ResetCfg(DISABLE);
    //  { // RX mode
       state = RF_Rx(data, 10, 0xFF, 0xFF);
    //    PRINT("RX mode.state = %x\n", state);
    // printf("RX mode.state = %x\n", state);
    //  }

    // { // TX mode
    // 	tmos_set_event( rftaskID , SBP_RF_PERIODIC_EVT );
    // }
}

//=========================================================================
//�������ƣ�GetRFAddr
//���ܸ�Ҫ����ȡRF�����Ϣ���洢��110��������ǰ8���ֽ�
//����˵����Hdaddr��Ӳ����ַ������RF����ͨ�ţ������յ�����ͬӲ����ַ�豸��������Ϣ
//         Staddr�������ַ������RF����ͨ�ţ���������ͬӲ����ַ�µ��豸
//�������أ���
//=========================================================================
void GetRFAddr(uint32_t* Hdaddr, uint16_t* Staddr)
{
    uint8_t RFINFO[8];
    flash_read_logic(RFINFO,RFInfoSect,RFInfoSectOffest,8);                      //��101�����ж���Ӳ����ַ�������ַ
    *Hdaddr  = RFINFO[0];
    *Hdaddr += RFINFO[1] << 8;
    *Hdaddr += RFINFO[2] << 16;
    *Hdaddr += RFINFO[3] << 24;

    *Staddr  = RFINFO[4];
    *Staddr += RFINFO[5] << 8;

    printf("���豸Ӳ����ַΪ��0x%x,�����ַΪ��0x%d \r\n", *Hdaddr, *Staddr);

    if(RFINFO[6] == 0x1 && RFINFO[7] == 0x0)
      printf("�豸����Ϊ������\r\n");
    else if(RFINFO[6] == 0x0 && RFINFO[7] == 0x1)
      printf("�豸����Ϊ���ӻ�\r\n");
}

//=========================================================================
//�������ƣ�RF_Tx_Data
//���ܸ�Ҫ��RF��������
//����˵����data��RF ���͵�����
//         len���������ݵĳ���
//�������أ���
//=========================================================================
void RF_Tx_Data(uint8_t* data, uint8_t len)
{
  for(uint8_t i = 0; i < len; i++)
  {
    rfsend[i] = data[i];
  }

  rflength = len;

  tmos_set_event(rftaskID, RF_TX_TEST_EVENT);
}

#endif /* RF_C_ */