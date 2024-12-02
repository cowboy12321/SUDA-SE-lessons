//======================================================================
//�ļ����ƣ�mcu.h��mcuͷ�ļ���
//������λ��RISC-V(sumcu.suda.edu.cn)
//���¼�¼��20210426-20240202
//======================================================================

#ifndef __MCU_H_
#define __MCU_H_

//(1)���䶯������оƬͷ�ļ�
#include "CH573SFR.h"

//(2)���̶�������cpuͷ�ļ�
#include "cpu.h"
//��3�����䶯��MCU������Ϣ��غ곣��
//                            "1234567890123456789"
#define MCU_TYPE              "CH573 BIOS"          //MCU�ͺţ�25�ֽڣ�
#define BIOS_TYPE             "NOS V1.2 20240205"   //BIOS�汾�ţ�25�ֽڣ�
#define CORE_TYPE             "RISC-V"        //�ں�����
//                            "123456789"
#define RT_TYPE               "NOS"        //����ϵͳ����
#define MCU_SECTORSIZE        4096              //������С��B��  ��ȷ��
#define MCU_SECTOR_NUM        48             //MCU��������     ��ȷ��
#define USER_END_SECTOR       27                    //User��������
#define MCU_STACKTOP          0x20007FFF        //ջ����ַ��RAM��ߵ�ַ��
#define MCU_FLASH_ADDR_START  0x00000000        //MCU��FLASH��ʼ��ַ
#define MCU_FLASH_ADDR_LENGTH 0x0002FFFF        //MCU��FLASH���ȣ�448KB��
#define MCU_RAM_ADDR_START    0x20003800      //MCU��RAM��ʼ��ַ
#define MCU_RAM_ADDR_LENGTH   0x00004800      //MCU��RAM���ȣ�18KB��

#endif /* __MCU_H_ */
