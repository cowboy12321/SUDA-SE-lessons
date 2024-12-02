#ifndef __CRC_H__
#define __CRC_H__
#include "user.h"

//QUEUE_SIZE 必须为2的幂次
#define QUEUE_SIZE 0x100
#define QUEUE_MASK 0x0ff
#define BUFFER_SIZE 0x1000
#define BUFFER_MASK 0x0fff
static uint8_t FrameHead = 0x80; // 帧头
static uint8_t FrameTail = 0x81; // 帧尾

#define MSG_SIZE 16

uint16_t crc16(uint8_t *ptr, uint16_t len);
void sendFrame(uint8_t UART_NO,uint16_t length,uint8_t *data);
//=====================================================================
//函数名称：queue_init
//函数返回： 无
//参数说明： *data, 获取到字符
//功能概要：获取队列中的字符
//返回值： 是否有值
//=====================================================================
void queue_init();
//=====================================================================
//函数名称：queue_get
//函数返回： 无
//参数说明： *data, 获取到字符串
//功能概要：获取队列中的字符
//返回值： 长度
//=====================================================================
uint16_t queue_get(char * data);
//=====================================================================
//函数名称：queue_put
//函数返回： 无
//参数说明： 放入队列的值
//功能概要： 把字符放入队列，该函数会删除之前的值
//返回值： 是否有值,0:无值,1:有值
//=====================================================================
void queue_put(const char * data,uint16_t length);


#endif