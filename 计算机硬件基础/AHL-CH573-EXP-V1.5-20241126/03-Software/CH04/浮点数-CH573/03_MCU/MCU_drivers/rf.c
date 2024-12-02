#ifndef RF_C_
#define RF_C_

#include "rf.h"
#include "printf.h"

extern uint16_t RF_ProcessEvent( uint8_t task_id, uint16_t events );
extern void RF_2G4StatusCallBack( uint8_t sta , uint8_t crc, uint8_t *rxBuf );

//=========================================================================
//函数名称：RF_Init
//功能概要：RF初始化,默认初始化为接收状态
//参数说明：HW_ADR:32位地址，禁止使用0x55555555以及0xAAAAAAAA (
//         建议不超过24次位反转，且不超过连续的6个0或1 )
//函数返回：无
//=========================================================================
void RF_Init(uint32_t HW_ADR)
{
    uint8_t state;
    rfConfig_t rfConfig;
    CH57X_BLEInit( ); //BLE初始化
    HAL_Init(  );     //硬件初始化
    RF_RoleInit( );
    rftaskID = TMOS_ProcessEventRegister( RF_ProcessEvent );  //任务初始化
    rfConfig.accessAddress = HW_ADR;
    rfConfig.CRCInit = 0x555555;
    rfConfig.Channel = 8;
    rfConfig.Frequency = 2480000;
    rfConfig.LLEMode = LLE_MODE_BASIC|LLE_MODE_EX_CHANNEL; // 使能 LLE_MODE_EX_CHANNEL 表示 选择 rfConfig.Frequency 作为通信频点
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
//函数名称：GetRFAddr
//功能概要：获取RF相关信息，存储在110号扇区的前8个字节
//参数说明：Hdaddr：硬件地址，用于RF无线通信，可以收到来自同硬件地址设备发出的信息
//         Staddr：软件地址，用于RF无线通信，用于区分同硬件地址下的设备
//函数返回：无
//=========================================================================
void GetRFAddr(uint32_t* Hdaddr, uint16_t* Staddr)
{
    uint8_t RFINFO[8];
    flash_read_logic(RFINFO,RFInfoSect,RFInfoSectOffest,8);                      //从101扇区中读出硬件地址和软件地址
    *Hdaddr  = RFINFO[0];
    *Hdaddr += RFINFO[1] << 8;
    *Hdaddr += RFINFO[2] << 16;
    *Hdaddr += RFINFO[3] << 24;

    *Staddr  = RFINFO[4];
    *Staddr += RFINFO[5] << 8;

    printf("该设备硬件地址为：0x%x,软件地址为：0x%d \r\n", *Hdaddr, *Staddr);

    if(RFINFO[6] == 0x1 && RFINFO[7] == 0x0)
      printf("设备类型为：主机\r\n");
    else if(RFINFO[6] == 0x0 && RFINFO[7] == 0x1)
      printf("设备类型为：从机\r\n");
}

//=========================================================================
//函数名称：RF_Tx_Data
//功能概要：RF发送数据
//参数说明：data：RF 发送的数据
//         len：发送数据的长度
//函数返回：无
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