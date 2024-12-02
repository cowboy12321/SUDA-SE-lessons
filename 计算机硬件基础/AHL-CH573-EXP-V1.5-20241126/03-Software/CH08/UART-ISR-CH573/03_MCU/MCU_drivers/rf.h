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

uint8_t rfsend[250];  //rf��������,rfһ֡�����Ϊ256�ֽ�
uint8_t rflength;     //rf�������ݳ���

//=========================================================================
//�������ƣ�RF_Init
//���ܸ�Ҫ��RF��ʼ��
//����˵����HW_ADR:32λ��ַ����ֹʹ��0x55555555�Լ�0xAAAAAAAA (
//         ���鲻����24��λ��ת���Ҳ�����������6��0��1 )
//�������أ���
//=========================================================================
void RF_Init(uint32_t HW_ADR);

//=========================================================================
//�������ƣ�GetRFAddr
//���ܸ�Ҫ����ȡRF�����Ϣ���洢��110��������ǰ8���ֽ�
//����˵����Hdaddr��Ӳ����ַ������RF����ͨ�ţ������յ�����ͬӲ����ַ�豸��������Ϣ
//         Staddr�������ַ������RF����ͨ�ţ���������ͬӲ����ַ�µ��豸
//�������أ���
//=========================================================================
void GetRFAddr(uint32_t* Hdaddr, uint16_t* Staddr);

//=========================================================================
//�������ƣ�RF_Tx_Data
//���ܸ�Ҫ��RF��������
//����˵����data��RF ���͵�����
//         len���������ݵĳ���
//�������أ���
//=========================================================================
void RF_Tx_Data(uint8_t* data, uint8_t len);

#endif /* SRC_FLASH_H_ */