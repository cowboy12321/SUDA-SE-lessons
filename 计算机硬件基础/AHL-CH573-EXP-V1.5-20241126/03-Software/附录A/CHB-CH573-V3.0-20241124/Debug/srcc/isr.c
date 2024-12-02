//=====================================================================
//文件名称：isr.c（中断处理程序源文件）
//框架提供：苏州大学嵌入式系统与物联网研究所（sumcu.suda.edu.cn）
//版本更新：20170801-20220524
//功能描述：提供中断处理程序编程框架
//移植规则：【固定】
//=====================================================================
#include "includes.h"

//本文件内部函数声明处--------------------------------------------------
uint16_t emuart_frame(uint8_t uartNo, uint8_t ch, uint8_t *data);

//======================================================================
//中断服务例程名称：UART_User_Handler
//触发条件：UART_User串口收到一个字节触发
//基本功能：串口收到一个字节后，进入本程序运行；本程序内部调用组帧函数
//          CreateFrame，当组帧完成，放入消息队列
//说    明：使用全局变量
//======================================================================

void UART_User_Handler(void)
{
    //局部变量
	uint8_t ch;
	uint8_t flag;
	static uint8_t recv_dateframe[11];  //串口接收字符数组
    static uint8_t recv_data[64];
    static vuint16_t gcRecvLen=0;

	DISABLE_INTERRUPTS;      //关总中断
	//-------------------------------
    //接收一个字节
	ch = uart_re1(UART_User,&flag);
	if(flag)
	{
        // 【1】获取组帧
        //  0：未组帧成功；非0：组帧成功，且返回值为接收到的数据长度
        if (gcRecvLen == 0){
            gcRecvLen = emuart_frame(UART_User, ch, (uint8_t *)recv_data);
        }
        if(gcRecvLen){
            queue_put(recv_data+2,gcRecvLen);
            gcRecvLen=0;
        }
	}
	//-------------------------------
	ENABLE_INTERRUPTS;       //开总中断
}

//内部调用函数



//===========================================================================
// 函数名称：emuart_frame
// 功能概要：组帧，将待组帧的数据加入帧中。本函数应被放置于中断处理程序中。
// 参数说明：ch：串口接收到一个字节数据；数组data：存放接收到的数据。
// 函数返回：0：未组帧成功；非0：组帧成功，且返回值为接收到的数据长度
// 备注：十六进制数据帧格式
//      帧头       + 数据长度         + 有效数据   +CRC校验码 +  帧尾
//      FrameHead(1字节)
//      len(1字节)
//      有效数据(N字节 N=data[1])
//      CRC校验(2字节)
//      FrameTail(1字节)
//返回数据放在data+2开头的数组中
//===========================================================================
uint16_t emuart_frame(uint8_t uartNo, uint8_t ch, uint8_t *data)
{
    static uint16_t index = 0;
    static uint16_t length = 0;
    uint16_t ret_val;
    uint16_t i;
    uint16_t crc;
    // （1）若未接收到数据或者未遇到帧头，则退出并返回0  帧头长度为1个字节
    if (index == 0 && ch != FrameHead)
    {
        goto uecom_recv_err;
    }
    if (ch == FrameHead) index = 0;
    // （2）正确接收到了帧头，继续执行，将数据存入data数组
    data[(index++)] = ch;
    // （3）当获取到第四个数据时，求出有效数据长度

    if (index == 2) length = data[1];
    // printf("index:%d\n",index);
    // （4）如果接收到的数据达到一帧长度（帧长度=有效数据长度+8），进行CRC校验，如果校验通过将进行数据的处理。
    if (length != 0 && index > length + 4)
    {
        // （4.1）对有效数据进行CRC校验
        crc = crc16((data + 2), length);
        // （4.2）当未获取到帧尾或CRC校验出错，则出错返回0
        if (data[index-1] != FrameTail || (crc >> 8) != data[index - 3] || (crc & 0xff) != data[index - 2]) 
        {
#ifdef __debug 
            printf("错误：CRC校验错 length:%d, crc: %#04x, data:%#02x%02x\n",length,crc,data[index-3],data[index-2]);
#endif
            goto uecom_recv_err;
        }

        ret_val = length; // 返回有效数据长度
        index=length = 0;             // 令有效数据长度为0
        goto uecom_recv_exit;
    }
    ret_val = 0;
    goto uecom_recv_exit;
uecom_recv_exit:

    return ret_val;

uecom_recv_err:
    index = 0;
    length = 0;
    return 0;
}



/*
 知识要素：
 1.本文件中的中断处理函数调用的均是相关设备封装好的具体构件，在更换芯片
 时，只需保证设备的构件接口一致，即可保证本文件的相关中断处理函数不做任何
 更改，从而达到芯片无关性的要求。
 */