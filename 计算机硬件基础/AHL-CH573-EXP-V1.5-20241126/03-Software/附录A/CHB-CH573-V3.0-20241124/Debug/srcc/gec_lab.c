#include "gec_lab.h"

//=====================================================================
// 函数名称：crc16
// 功能概要：将数据进行16位的CRC校验，返回校验后的结果值
// 参数说明：ptr:需要校验的数据缓存区
//      len:需要检验的数据长度
// 函数返回：计算得到的校验值
//=====================================================================
uint16_t crc16(uint8_t *ptr, uint16_t len)
{
    uint16_t i, j, tmp, crc16;

    crc16 = 0xffff;
    for (i = (uint16_t)0; i < len; i++)
    {
        crc16 = (uint16_t)ptr[i] ^ crc16;
        for (j = 0; j < 8; j++)
        {
            tmp = crc16 & 0x0001;
            crc16 = crc16 >> 1;
            if (tmp)
                crc16 = crc16 ^ 0xa001;
        }
    }
    return crc16;
}

//=====================================================================
//函数名称：sendFrame
//函数返回： 无
//参数说明： length：待发送数据的长度；data：待发送数据缓冲区。
//功能概要：通过emuart发送指定长度的字节数组。
//更新记录：20180602，wb；
//=====================================================================
static uint8_t emuartFrameHead = 0x80;  //帧头
static uint8_t emuartFrameTail = 0x81;  //帧尾
void sendFrame(uint8_t UART_NO,uint16_t length,uint8_t *data)
{
    uint16_t crc;
    //计算得到CRC校验码
    crc = crc16(data,length);
    //发送帧头
    uart_send1(UART_NO,emuartFrameHead);
    //发送帧长
    uart_send1(UART_NO,length);
    //发送帧数据
    uart_sendN(UART_NO,length,data);
    //发送校验码
    uart_send1(UART_NO,crc>>8);
    uart_send1(UART_NO,crc);
    //发送帧尾
    uart_send1(UART_NO,emuartFrameTail);
}

static char __buffer[BUFFER_SIZE];
static uint32_t __queue[QUEUE_SIZE];
static uint32_t __lens[QUEUE_SIZE];
static uint32_t __queue_front;
static uint32_t __queue_back;//队列的包含范围是[__queue_front,_queue_back)
static uint32_t __buffer_front;
static uint32_t __buffer_back;//队列的包含范围是[__queue_front,_queue_back)

#define buffer_front_off(x) ((__buffer_front + x) & BUFFER_MASK)
#define buffer_back_off(x)  ((__buffer_back  + x) & BUFFER_MASK)
#define queue_front_off(x)  ((__queue_front  + x) & QUEUE_MASK)
#define queue_back_off(x)   ((__queue_back   + x) & QUEUE_MASK)
#define buffer_front buffer_front_off(0) 
#define buffer_back  buffer_back_off (0)  
#define queue_front  queue_front_off (0)  
#define queue_back   queue_back_off  (0)   
//=====================================================================
//函数名称：queue_init
//函数返回： 无
//参数说明： *data, 获取到字符
//功能概要：获取队列中的字符
//返回值： 是否有值
//=====================================================================
void queue_init(){
    __queue_front = __queue_back;
    __buffer_front = __buffer_back;
}
//=====================================================================
//函数名称：queue_get
//函数返回： 无
//参数说明： *data, 获取到字符串
//功能概要：获取队列中的字符
//返回值： 长度
//=====================================================================
uint16_t queue_get(char * data){
    if(queue_front==queue_back) return 0;
    uint16_t length=__lens[__queue_front ++ & QUEUE_MASK];
    uint16_t i;
    if(data!=NULL) {
        for(i=0;i<length;++i){
            *(data+i)=__buffer[__buffer_front++ & BUFFER_MASK];
        }
    }else{
        __buffer_front+=length;
    }
    return length;
}
//=====================================================================
//函数名称：queue_put
//函数返回： 无
//参数说明： 放入队列的值
//功能概要： 把字符放入队列，该函数会删除之前的值
//返回值： 是否有值,0:无值,1:有值
//=====================================================================
void queue_put(const char * data,uint16_t length){
    if(length >= BUFFER_MASK && data==NULL) return;
    uint16_t i=0;
    __queue[queue_back]=__buffer_front;
    while( queue_back_off(1)==queue_front||
        buffer_front < buffer_back && buffer_back_off(length) >=buffer_front && buffer_back_off(length) <buffer_back||
        buffer_back <buffer_front && buffer_back_off(length) >=buffer_front||
        buffer_back <buffer_front && buffer_back_off(length) <buffer_back)
        if(!queue_get(NULL)) break;

    for(;i<length;++i){
        __buffer[ __buffer_back++ & BUFFER_MASK] = data[i];
    }
    __lens[(__queue_back++) & QUEUE_MASK]=length;

}

