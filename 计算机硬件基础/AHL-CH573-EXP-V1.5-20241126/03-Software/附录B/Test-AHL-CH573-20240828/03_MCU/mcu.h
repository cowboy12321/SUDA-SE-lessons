//======================================================================
//文件名称：mcu.h（mcu头文件）
//制作单位：RISC-V(sumcu.suda.edu.cn)
//更新记录：20210426-20240202
//======================================================================

#ifndef __MCU_H_
#define __MCU_H_

//(1)【变动】包含芯片头文件
#include "CH573SFR.h"

//(2)【固定】包含cpu头文件
#include "cpu.h"
//（3）【变动】MCU基本信息相关宏常数
//                            "1234567890123456789"
#define MCU_TYPE              "CH573 BIOS"          //MCU型号（25字节）
#define BIOS_TYPE             "NOS V1.2 20240205"   //BIOS版本号（25字节）
#define CORE_TYPE             "RISC-V"        //内核类型
//                            "123456789"
#define RT_TYPE               "NOS"        //操作系统类型
#define MCU_SECTORSIZE        4096              //扇区大小（B）  待确定
#define MCU_SECTOR_NUM        48             //MCU扇区总数     待确认
#define USER_END_SECTOR       27                    //User结束扇区
#define MCU_STACKTOP          0x20007FFF        //栈顶地址（RAM最高地址）
#define MCU_FLASH_ADDR_START  0x00000000        //MCU的FLASH起始地址
#define MCU_FLASH_ADDR_LENGTH 0x0002FFFF        //MCU的FLASH长度（448KB）
#define MCU_RAM_ADDR_START    0x20003800      //MCU的RAM起始地址
#define MCU_RAM_ADDR_LENGTH   0x00004800      //MCU的RAM长度（18KB）

#endif /* __MCU_H_ */
