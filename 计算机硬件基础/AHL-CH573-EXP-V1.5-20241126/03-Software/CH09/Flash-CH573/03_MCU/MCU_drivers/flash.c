//===========================================================================
//文件名称：flash.c
//功能概要：Flash底层驱动构件源文件
//版权所有：苏大嵌入式(sumcu.suda.edu.cn)
//版本更新：20230831，20240810
//芯片类型：CH573F
//===========================================================================

#include "flash.h"
#include "printf.h"

//=================内部调用函数声明=====================================
//======================================================================
//函数名称：flash_write_DoubleWord
//函数返回：0-成功 1-失败
//参数说明：addr：目标地址，要求为4的倍数且大于Flash首地址
//              （例如：0x08000004，Flash首地址为0x08000000）
//       data：写入的双字
//功能概要：Flash双字写入操作
//======================================================================
uint8_t flash_write_DoubleWord(uint32_t addr,uint32_t *data_l,uint32_t *data_h);
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

//======================================================================
//函数名称：flash_init
//函数返回：无
//参数说明：无
//功能概要：初始化flash模块
//======================================================================
void flash_init(void)
{
    FLASH_ROM_LOCK(0);     //Flash解锁
}

//======================================================================
//函数名称：flash_erase
//函数返回：函数执行执行状态：0=正常；1=异常。
//参数说明：sect：目标扇区号（范围取决于实际芯片，例如 CH573:0~119,每扇区4KB）
//功能概要：擦除flash存储器的sect扇区
//======================================================================
uint8_t flash_erase(uint16_t sect)
{
    uint32_t StartAddr;
    uint8_t flag;
    StartAddr = (uint32_t)(sect * FLASH_SECT_SIZE + FLASH_START_ADDRESS); //目标扇区的起始地址
    flag = FLASH_ROM_ERASE(StartAddr,4096);						//擦除目标扇区
    return flag;
}

//======================================================================
//函数名称：flash_write
//函数返回：函数执行状态：0=正常；1=异常。
//参数说明：sect：扇区号（范围取决于实际芯片，例如 CH573:0~119,每扇区4KB）
//        offset:写入扇区内部偏移地址（0~4092，要求为0,4,8,12，......）
//        N：写入字节数目（4~4096,要求为4,8,12,......）
//        buff：源数据缓冲区首地址
//功能概要：将buff开始的N字节写入到flash存储器的sect扇区的 offset处
//编程参考：暂无
//=======================================================================
uint8_t flash_write(uint16_t sect,uint16_t offset,uint16_t N,uint8_t *buff)
{
    //（1）定义变量
    uint16_t i;
    //（2）清除之前的编程导致的所有错误标志位
    //（3）Flash写入
    //（3.1）写入字节数后会跨扇区
    if(offset+N>FLASH_SECT_SIZE)     
    {
        //（3.1.1）先写入第一个扇区
        flash_write(sect,offset,FLASH_SECT_SIZE-offset,buff);
        //（3.1.2）再写入第二个扇区
        flash_write(sect+1,0,N-(FLASH_SECT_SIZE-offset),buff+(FLASH_SECT_SIZE-offset));
    }
    //（3.2）写入字节数不会跨扇区
    else
    {
         uint8_t data[FLASH_SECT_SIZE]; //存储当前扇区的全部值
         flash_read_logic(data,sect,0,FLASH_SECT_SIZE); //将当前扇区的全部值读入数组中
         for(i = 0;i<N;i++)             //将要写入的数据依照对应位置写入数组中
         {
             data[offset+i] = buff[i];
         }
         flash_Best(sect,0,FLASH_SECT_SIZE,data);		  //将数组写入扇
     }
    return 0;
}

//==========================================================================
//函数名称：flash_write_physical
//函数返回：函数执行状态：0=正常；非0=异常。
//参数说明： addr：目标地址，要求为4的倍数且大于Flash首地址
//              （例如：0x00000004，Flash首地址为0x00000000）
//           N：写入字节数目
//           buff：源数据缓冲区首地址
//功能概要：根据物理地址addr进行flash写入操作
//编程参考：暂无
//==========================================================================
uint8_t flash_write_physical(uint32_t addr,uint16_t N,uint8_t* buff)
{
    //（1）定义变量。sect-扇区号，offset-扇区地址
    uint16_t sect;      //扇区号
    uint16_t offset;    // 偏移地址
    uint8_t flag;
    //（2）变量赋值，将物理地址转换为逻辑地址（扇区和偏移量）
    sect = (addr-FLASH_START_ADDRESS)/FLASH_SECT_SIZE;		   //物理地址对应的扇区
    offset = addr-(sect*FLASH_SECT_SIZE)-FLASH_START_ADDRESS;  //物理地址对应扇区内偏移量
    //（3）调用写入函数写入数据
    flag = flash_write(sect,offset,N,buff);

    return flag;
}

//======================================================================
//函数名称：flash_read_logic
//函数返回：无
//参数说明：dest：读出数据存放处（传地址，目的是带出所读数据，RAM区）
//         sect：扇区号（范围取决于实际芯片，例如 CH573:0~119,每扇区4KB）
//         offset:扇区内部偏移地址（0~4092，要求为0,4,8,12，......）
//         N：读字节数目（4~4096,要求为4,8,12,......）
//功能概要：读取flash存储器的sect扇区的 offset处开始的N字节，到RAM区dest处
//编程参考：暂无
//=======================================================================
void flash_read_logic(uint8_t* dest,uint16_t sect,uint16_t offset,uint32_t N)
{
    uint32_t i, Length = ( N + 3 ) >> 2;
    // pCode：flash存储器的sect扇区的offset处地址，pBuf：读出数据存放地址
    uint32_t* pCode = ( uint32_t* ) (sect * FLASH_SECT_SIZE + offset + FLASH_START_ADDRESS);
    uint32_t* pBuf = ( uint32_t* ) dest;
    //数据拷贝
    for ( i = 0; i < Length; i++ )
    {
        *pBuf++ = *pCode++;
    }
}

//======================================================================
//函数名称：flash_read_physical
//函数返回：无
//参数说明：dest：读出数据存放处（传地址，目的是带出所读数据，RAM区）
//         addr：目标地址，要求为4的倍数（例如：0x00000004）
//         N：读字节数目
//功能概要：读取flash指定地址的内容
//======================================================================
void flash_read_physical(uint8_t *dest,uint32_t addr,uint16_t N)
{
    uint32_t i, Length = ( N + 3 ) >> 2;
    uint32_t* pCode = ( uint32_t* ) addr;
    uint32_t* pBuf = ( PUINT32 ) dest;

    for ( i = 0; i < Length; i++ )
    {
      *pBuf++ = *pCode++;
    }
}


//======================================================================
//函数名称：flash_isempty
//函数返回：1=目标区域为空；0=目标区域非空。
//参数说明：sect:所需判断的扇区号
//          N:所需判断的区域大小（该扇区的前N个字节，N不超过4096）
//功能概要：flash判空操作
//【注】：CH573的flash空值与常规芯片的0xff不同，其空值为0xf5f9bda9
//======================================================================
uint8_t flash_isempty(uint16_t sect,uint16_t N)
{
    //（1）定义变量flag-操作成功标志，dest[]-暂存数据，src目标地址
    uint16_t i,flag;
    uint8_t dest[N];
    uint8_t *src;
    //（2）变量赋值并读取数据
    flag = 1;
    src = (uint8_t *)(FLASH_START_ADDRESS + sect*FLASH_SECT_SIZE);
    memcpy(dest,src,N);
    //（3）判断区域内数据是否为空
    for(i = 0; i < N; i++)    //遍历区域内字节
    {
        switch(i%4)
        {
        	case 0:
        		if(dest[i]!=0xa9) flag = 0;
        		break;
        	case 1:
        		if(dest[i]!=0xbd) flag = 0;
        		break;
        	case 2:
        		if(dest[i]!=0xf9) flag = 0;
        		break;
        	case 3:
        		if(dest[i]!=0xf5) flag = 0;
        		break;
        	default:
        		break;
        }
        if(flag == 0)
        	break;
    }
    return flag;
}

//======================================================================
//函数名称：flash_protect
//函数返回：无
//参数说明：M=0时，解开对Flash的保护；
//         M=1时，保护系统非易失配置信息存储区InfoFlash
//         M=2时，保护系统引导程序存储区 BootLoader
//         M=3时，保护用户应用程序存储区CodeFlash和用户非易失数据存储区DataFlash
//功能概要：flash保护操作
//说    明：flash保护之后无法对对应区域进行擦除或是写入操作
//         InfoFlash： 0x0007E000-0x0007FFFF
//         BootLoader：0x00078000-0x0007DFFF
//         CodeFlash： 0x00000000-0x0006FFFF
//         DataFlash： 0x00070000-0x00077FFF
//======================================================================
void flash_protect(uint8_t M)
{
	FLASH_ROM_LOCK(M);
}

//----------------------以下为内部函数存放处----------------------------
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
    addr = (uint32_t)(FLASH_START_ADDRESS+sect*FLASH_SECT_SIZE+offset);
    uint8_t flag=FLASH_ROM_WRITE(addr,buf,N);
    return flag;
}

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
