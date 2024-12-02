#ifndef __RF_H_
#define __RF_H_

#include "CH57xBLE_LIB.H"
#include "flash.h"
#include "printf.h"

#define SBP_RF_START_DEVICE_EVT          1
#define SBP_RF_PERIODIC_EVT              2
#define SBP_RF_RF_RX_EVT                 4
#define RF_TX_TEST_EVENT                 8
#define RF_TX_SYNC_EVENT                 8

#define RFInfoSect          110
#define RFInfoSectOffest    0

extern void **  component_fun;

#define CH57X_BLEInit                   ((void(*)( void ))(component_fun[33]))
#define HAL_Init                        ((void(*)( void ))(component_fun[34]))
#define RF_RoleInit                     ((bStatus_t(*)( void ))(component_fun[35]))
#define RF_Config                       ((bStatus_t(*)( rfConfig_t *pConfig ))(component_fun[36]))
#define TMOS_ProcessEventRegister       ((tmosTaskID(*)( pTaskEventHandlerFn eventCb ))(component_fun[37]))
#define TMOS_SystemProcess              ((void(*)( void ))(component_fun[38]))
#define RF_Rx                           ((bStatus_t(*)( uint8_t *txBuf, uint8_t txLen, uint8_t pktRxType, uint8_t pktTxType ))(component_fun[39]))
#define RF_Tx                           ((bStatus_t(*)( uint8_t *txBuf, uint8_t txLen, uint8_t pktTxType, uint8_t pktRxType ))(component_fun[40]))
#define RF_Shut                         ((bStatus_t(*)( void ))(component_fun[41]))
// #define WWDG_ResetCfg                   ((void(*)( FunctionalState s ))(component_fun[42]))
#define mDelaymS                        ((void(*)( UINT16 t ))(component_fun[43]))
#define tmos_set_event                  ((bStatus_t(*)( tmosTaskID taskID, tmosEvents event ))(component_fun[44]))
#define tmos_msg_receive                ((uint8_t*(*)( tmosTaskID taskID ))(component_fun[45]))
#define tmos_msg_deallocate             ((bStatus_t(*)( uint8_t *msg_ptr ))(component_fun[46]))
#define tmos_start_task                 ((BOOL(*)( tmosTaskID taskID, tmosEvents event, tmosTimer time  ))(component_fun[47]))

uint8_t rftaskID;
uint8_t data[10];

uint8_t rfsend[250];  //rf发送数据,rf一帧数据最长为256字节
uint8_t rflength;     //rf发送数据长度

//=========================================================================
//函数名称：RF_Init
//功能概要：RF初始化
//参数说明：HW_ADR:32位地址，禁止使用0x55555555以及0xAAAAAAAA (
//         建议不超过24次位反转，且不超过连续的6个0或1 )
//函数返回：无
//=========================================================================
void RF_Init(uint32_t HW_ADR);

//=========================================================================
//函数名称：GetRFAddr
//功能概要：获取RF相关信息，存储在110号扇区的前8个字节
//参数说明：Hdaddr：硬件地址，用于RF无线通信，可以收到来自同硬件地址设备发出的信息
//         Staddr：软件地址，用于RF无线通信，用于区分同硬件地址下的设备
//函数返回：无
//=========================================================================
void GetRFAddr(uint32_t* Hdaddr, uint16_t* Staddr);

//=========================================================================
//函数名称：RF_Tx_Data
//功能概要：RF发送数据
//参数说明：data：RF 发送的数据
//         len：发送数据的长度
//函数返回：无
//=========================================================================
void RF_Tx_Data(uint8_t* data, uint8_t len);

#endif /* SRC_FLASH_H_ */