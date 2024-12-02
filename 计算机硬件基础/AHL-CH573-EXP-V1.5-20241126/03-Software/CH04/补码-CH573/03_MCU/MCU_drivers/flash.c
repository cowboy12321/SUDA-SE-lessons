//===========================================================================
//文件名称：flash.c
//功能概要：Flash底层驱动构件源文件
//版权所有：SD-Arm(sumcu.suda.edu.cn)
//版本更新：20200831-20200904
//芯片类型：CH573F
//===========================================================================


#include "flash.h"

//=================内部调用函数声明=====================================
//======================================================================
//函数名称：flash_write_DoubleWord
//函数返回：0-成功 1-失败
//参数说明：addr：目标地址，要求为4的倍数且大于Flash首地址
//              （例如：0x08000004，Flash首地址为0x08000000）
//       data：写入的双字
//功能概要：Flash双字写入操作
//======================================================================
uint8_t flash_write_DoubleWord(uint32_t addr,uint32_t* data_l,uint32_t* data_h);
//======================================================================
//函数名称：flash_Best
//函数返回：0-成功 1-失败
//参数说明：sect:待写入扇区号
//            offset:待写入数据位置的偏移地址
//            N：待写入数据字节数
//            buf:待写入数据的首地址
//功能概要：首位地址都对齐的情况下的数据写入
//======================================================================
uint8_t flash_Best(uint16_t sect,uint16_t offset,uint16_t N,uint8_t *buf);

//======================================================================
//函数名称：flash_write
//函数返回：函数执行状态：0=正常；1=异常。
//参数说明：sect：扇区号（范围取决于实际芯片，例如 STM32L433:0~127,每扇区2KB）
//        offset:写入扇区内部偏移地址（0~2044，要求为0,4,8,12，......）
//        N：写入字节数目（4~2048,要求为4,8,12,......）
//        buf：源数据缓冲区首地址
//功能概要：将buf开始的N字节写入到flash存储器的sect扇区的 offset处
//编程参考：暂无
//=======================================================================
uint8_t flash_write_sect(uint16_t sect,uint16_t offset,uint32_t N,uint8_t *buf);

//======================================================================
//函数名称：flash_init
//函数返回：无
//参数说明：无
//功能概要：初始化flash模块
//======================================================================
void flash_init(void)
{

    FLASH_ROM_LOCK(0);

}


//======================================================================
//函数名称：flash_erase
//函数返回：函数执行执行状态：0=正常；1=异常。
//参数说明：sect：目标扇区号（范围取决于实际芯片
//功能概要：擦除flash存储器的sect扇区
//======================================================================
uint8_t flash_erase(uint16_t sect)
{
    uint32_t StartAddr;
    uint8_t flag;
    StartAddr = (uint32_t)(sect * Flash_Sect_size + Flash_Address);
    flag = FLASH_ROM_ERASE(StartAddr,4096);
    return flag;

}
//======================================================================
//函数名称：flash_write
//函数返回：函数执行状态：0=正常；1=异常。
//参数说明：sect：扇区号（范围取决于实际芯片，例如 STM32L433:0~127,每扇区2KB）
//        offset:写入扇区内部偏移地址（0~2044，要求为0,4,8,12，......）
//        N：写入字节数目（4~2048,要求为4,8,12,......）
//        buf：源数据缓冲区首地址
//功能概要：将buf开始的N字节写入到flash存储器的sect扇区的 offset处
//编程参考：暂无
//=======================================================================
uint8_t flash_write_sect(uint16_t sect,uint16_t offset,uint32_t N,uint8_t *buf)
{
    uint32_t StartAddr;
    uint8_t flag;

    StartAddr = (uint32_t)(sect * Flash_Sect_size + offset + Flash_Address);
    flag = FLASH_ROM_WRITE(StartAddr,buf,N);

    return flag;
}


//======================================================================
//函数名称：flash_write
//函数返回：函数执行状态：0=正常；1=异常。
//参数说明：sect：扇区号（范围取决于实际芯片，例如 STM32L433:0~127,每扇区2KB）
//        offset:写入扇区内部偏移地址（0~2044，要求为0,4,8,12，......）
//        N：写入字节数目（4~2048,要求为4,8,12,......）
//        buf：源数据缓冲区首地址
//功能概要：将buf开始的N字节写入到flash存储器的sect扇区的 offset处
//编程参考：暂无
//=======================================================================
uint8_t flash_write(uint16_t sect,uint16_t offset,uint16_t N,uint8_t *buf)
{
    //（1）定义变量
    uint16_t i;
    //（2）清除之前的编程导致的所有错误标志位
    //（3.1）写入字节数后会跨扇区
    if(offset+N>Flash_Sect_size)
    {

        //（3.1.1）先写入第一个扇区
        flash_write(sect,offset,Flash_Sect_size-offset,buf);
        //（3.1.2）再写入第二个扇区
        flash_write(sect+1,0,N-(Flash_Sect_size-offset),buf+(Flash_Sect_size-offset));
    }
    //（3.2）写入字节数不会跨扇区
    else
     {
         uint8_t data[Flash_Sect_size]; //存储当前扇区的全部值
         flash_read_logic(data,sect,0,Flash_Sect_size); //将当前扇区的全部值读入数组中
         //将要写入的数据依照对应位置写入数组中
         for(i = 0;i<N;i++)
         {
             data[offset+i] = buf[i];
         }

         flash_Best(sect,0,Flash_Sect_size,data);
     }
    return 0;
}



//==========================================================================
//函数名称：flash_write_physical
//函数返回：函数执行状态：0=正常；非0=异常。
//参数说明： addr：目标地址，要求为4的倍数且大于Flash首地址
//              （例如：0x08000004，Flash首地址为0x08000000）
//       cnt：写入字节数目（8~512）
//       buf：源数据缓冲区首地址
//功能概要：flash写入操作
//编程参考：暂无
//==========================================================================
uint8_t flash_write_physical(uint32_t addr,uint16_t N,uint8_t* buf)
{
    //（1）定义变量。sect-扇区号，offset-扇区地址
    uint16_t sect;   //扇区号
    uint16_t offset;    // 偏移地址
    uint8_t flag;
    //（2）变量赋值，将物理地址转换为逻辑地址（扇区和偏移量）
    sect = (addr-Flash_Address)/Flash_Sect_size;
    offset = addr-(sect*Flash_Sect_size)-Flash_Address;
    //（3）调用写入函数写入数据
    flag = flash_write(sect,offset,N,buf);

    return flag;
}



//======================================================================
//函数名称：flash_Best
//函数返回：0-成功 1-失败
//参数说明：sect:待写入扇区号
//            offset:待写入数据位置的偏移地址
//            N：待写入数据字节数
//            buf:待写入数据的首地址
//功能概要：首位地址都对齐的情况下的数据写入
//编程参考：暂无
//======================================================================
uint8_t flash_Best(uint16_t sect,uint16_t offset,uint16_t N,uint8_t *buf)
{
    uint32_t addr;
    addr = (uint32_t)(Flash_Address+sect*Flash_Sect_size+offset);
    uint8_t flag=FLASH_ROM_WRITE(addr,buf,N);
    return flag;
}



//======================================================================
//函数名称：flash_read_logic
//函数返回：无
//参数说明：Buffer：读出数据存放处（传地址，目的是带出所读数据，RAM区）
//       sect：扇区号（范围取决于实际芯片，例如 STM32L433:0~127,每扇区2KB）
//       offset:扇区内部偏移地址（0~2024，要求为0,4,8,12，......）
//       N：读字节数目（4~2048,要求为4,8,12,......）//
//功能概要：读取flash存储器的sect扇区的 offset处开始的N字节，到RAM区dest处
//编程参考：暂无
//=======================================================================
void flash_read_logic(uint8_t* Buffer,uint16_t sect,uint16_t offset,uint32_t N)
{

    uint32_t i, Length = ( N + 3 ) >> 2;
    uint32_t* pCode = ( uint32_t* ) (sect * Flash_Sect_size + offset + Flash_Address);
    uint32_t* pBuf = ( uint32_t* ) Buffer;

    for ( i = 0; i < Length; i++ )
    {
      *pBuf++ = *pCode++;
    }

}

//======================================================================
//函数名称：flash_read_physical
//函数返回：无
//参数说明：Buffer：读出数据存放处（传地址，目的是带出所读数据，RAM区）
//       addr：目标地址，要求为4的倍数（例如：0x00000004）
//       N：读字节数目（0~1020,要求为4，8,12,......）
//功能概要：读取flash指定地址的内容
//======================================================================
void flash_read_physical(uint8_t *Buffer,uint32_t addr,uint16_t N)
{
    uint32_t i, Length = ( N + 3 ) >> 2;
    uint32_t* pCode = ( uint32_t* ) addr;
    uint32_t* pBuf = ( PUINT32 ) Buffer;

    for ( i = 0; i < Length; i++ )
    {
      *pBuf++ = *pCode++;
    }
}

//======================================================================
//函数名称：flash_isempty-----待定------20210425
//函数返回：1=目标区域为空；0=目标区域非空。
//参数说明：所要探测的flash区域初始地址
//功能概要：flash判空操作
//编程来源：暂无
//======================================================================
uint8_t flash_isempty(uint16_t sect,uint16_t N,uint8_t* buffer)
{
    uint32_t StartAddr;
    uint8_t flag;

    StartAddr = ((uint32_t)(sect * 4096)) + 0x00000000;
    flag = FLASH_ROM_VERIFY(StartAddr,buffer,N);

    return flag;

}


//----------------------以下为内部函数存放处----------------------------
//======================================================================
//函数名称：flash_write_DoubleWord
//函数返回：0-成功 1-失败
//参数说明：addr：目标地址，要求为4的倍数且大于Flash首地址
//              （例如：0x08000004，Flash首地址为0x08000000）
//       data：写入的双字
//功能概要：Flash双字写入操作（STM32L433每次只能实现双字写入，先写低位字，再写高位字）
//编程参考：暂无
//======================================================================
uint8_t flash_write_DoubleWord(uint32_t addr,uint32_t *data_l,uint32_t *data_h)
{
    uint8_t flag1;
    uint8_t flag2;

    flag1 = FLASH_ROM_WRITE(addr,data_l,4);
    flag2 = FLASH_ROM_WRITE((uint32_t)(addr+4),data_h,4);

    return 0;

}
