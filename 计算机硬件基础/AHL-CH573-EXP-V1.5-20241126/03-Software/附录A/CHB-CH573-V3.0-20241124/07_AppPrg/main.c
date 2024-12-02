//======================================================================
//文件名称：main.c（应用工程主函数）
//框架提供：苏大嵌入式（sumcu.suda.edu.cn）
//版本更新：201911-202306
//功能描述：见本工程的..\01_Doc\Readme.txt
//移植规则：【固定】
//======================================================================
#define GLOBLE_VAR     //【固定】includes.h定义的全局变量一处声明多处使用
#include "includes.h"  //【固定】包含总头文件

//main.c使用的内部函数声明处---------------------------------------------
enum{ mcu_desc_len=6};
uint16_t analogInList[PIN_MAX];       // 模拟输入引脚列表
uint8_t analogInString[PIN_MAX][4];   // 模拟输入字符串
uint8_t analogInCount;           // 模拟输入字节数
uint8_t analogOutString[PIN_MAX][4];  // 模拟输出字符串
uint8_t analogOutCount;          // 模拟输出字节数
uint16_t digitalInList[PIN_MAX];      // 数字输入引脚列表
uint8_t digitalInString[PIN_MAX][2];  // 数字输入字符串
uint8_t digitalInCount;          // 数字输入字节数
uint8_t digitalOutString[PIN_MAX][2]; // 数字输出字符串
uint8_t digitalOutCount;         // 数字输出字节数

// #define __debug
//前22位大版本+18位小版本+6位MCU-id号
uint8_t mcu_desc[mcu_desc_len]={0xa0,0x00,0x03,0x00,0x00,0x03};
// #define __debug
//======================================================================
//函数名称：handle
//函数返回：无
//参数说明：data：数据指针，size：数据大小
//功能概要：处理temp中的数据
//内部调用：无
//======================================================================
void handle(uint8_t * data,uint8_t size){
    vuint8_t i,flag,type,pin;
    vuint16_t val;
    #ifdef __debug
    printf("handle,data=");
    for(int i=0;i<size && data[i]!=FrameTail;++i)  printf("0x%x ",data[i]);
    printf("\n");
    #endif
    switch (data[0])
    {
    case 0x90:
        //（1.1）若接受到数据为0b1001_0000，发送用户信息并清空缓存
        uart_sendN(UART_User,mcu_desc_len,mcu_desc);
        analogInCount = analogOutCount = digitalInCount = digitalOutCount = 0;
        // printf("shake\n");
        return;
    }
    flag = i = 0; // 当前正在处理的数据指针
    while (i < size)
    {
        switch (data[i])
        {
            case 0xa0:
            // （2.1.1）若接受到数据为0b1010_0000，清空输入缓存
                analogInCount = digitalInCount = 0;
                i++;
                #ifdef __debug
                printf("clear in\n");
                #endif
                continue;
            case 0xa1:
            // （2.1.2）若接受到数据为0b1010_0001，清空输出缓存
                analogOutCount = digitalOutCount = 0;
                i++;
                printf("clear out\n");
                continue;
        }

        if(data[i]==FrameTail) break;
        // （2.3）接收到的数据开头不是0b1000_0000，为指令信息
        //        type:模式号，取出高字节的第3-5位:
        //             000: 模拟输入(adc)
        //             001: 模拟输出(dac)
        //             010: 数字输入(gpio_in)
        //             011: 数字输出(gpio_out)
        //        pin: 引脚号，取出高字节的第0-1位和低字节的第0-5位，取值范围为0-255
        //        val: 数值，默认为0
        type = (data[i] >> 5) & 0x7;
        pin = data[i+1] &0x7f;
        #ifdef __debug
        printf("type=%d,pin=%d\n",type,pin);
        #endif
        //（2.4） 根据code的值进行不同的处理
        switch (type)
        {
        case 0b000:
            // （2.4.1）0b000为模拟输入，2字节数据
            // （a）将模拟输入引脚号存入analogInList
            analogInList[analogInCount] = analogInPin[pin];
            LAB_ANALOG_INIT(analogInPin[pin]);
            // （b）将模拟输入数值存入analogInString
            analogInString[analogInCount][1] = data[i + 1] ;
            // （c）模拟输入字节数 + 1
            analogInCount++;
            // （d）模拟输入占用2字节
            #ifdef __debug
            printf("ai,count=%d\n",analogInCount);
            #endif
            i += 2;
            break;
        case 0b001:
            // （2.4.2）0b001为模拟输出，3字节数据
            // (a)将模拟输出数值存入analogOutString
            analogOutString[analogOutCount][0] = 0xc0 | data[i] & 0x1f;
            analogOutString[analogOutCount][1] = data[i + 1] & 0x7f;
            analogOutString[analogOutCount][2] = data[i + 2] & 0x7f;
            // (b)模拟输出字节数 + 1
            analogOutCount++;
            // (c)将模拟输出数值存入val,并更新模拟输出
            val = (data[i + 2] & 0x7f) << 5 | data[i] & 0x1f;
            LAB_ANALOG_CTRL(analogOutPin[pin],val);
            // (d)模拟输出占用4字节
            i += 3;
            flag=1;
            #ifdef __debug
            printf("ao,v=%d,count=%d\n",val,analogOutCount);
            #endif
            break;
        case 0b010:
            // （2.4.3）0b010为数字输入，2字节数据
            // （a）将数字输入引脚号存入digitalInList
            digitalInList[digitalInCount] = digitalPin[pin];
            // （b）将数字输入数值存入digitalInString
            digitalInString[digitalInCount][1] = data[i + 1] ;
            // （c）数字输入字节数 + 1
            digitalInCount++;
            // （d）更新数字输入
            LAB_DIGITAL_INIT(digitalPin[pin], GPIO_INPUT);
            // （e）数字输入占用2字节
            i += 2;
            #ifdef __debug
            printf("di,count=%d\n",digitalInCount);
            #endif
            break;
        case 0b011:
            // （2.4.4）0b011为数字输出，2字节数据
            // （a）将数字输出数值存入digitalOutString
            digitalOutString[digitalOutCount][0] = 0b11100000 | data[i];
            digitalOutString[digitalOutCount][1] = data[i + 1];
            // （b）数字输出字节数 + 1
            digitalOutCount++;
            // （c）更新数字输出
            val = data[i] & 0x1f;
            LAB_DIGITAL_INIT(digitalPin[pin], GPIO_OUTPUT);
            LAB_DIGITAL_CTRL(digitalPin[pin], val? INPUT_ON : INPUT_OFF);
            // （d）数字输出占用2字节
            i += 2;
            flag=1;
            #ifdef __debug
            printf("do,v=%d,count=%d\n",val,digitalOutCount);
            #endif
            break;
        // （2.4.5）默认情况，跳过这一字节
        default:
            i++;
        }
    }
}

//======================================================================
//函数名称：send
//函数返回：无
//参数说明：无
//功能概要：收集信息并发送给上位机
//内部调用：无
//======================================================================
void send(){
    uint8_t i,index,buff[50];
    uint16_t val;
    index=0;
    // （1）发送模拟监控数据
    for (i = 0; i < analogInCount; ++i)
    {
        //(3.1)读取引脚电压
        val = LAB_ANALOG_AVE(analogInList[i]);
        //(3.2)将电压存入数据的第1 3个字节的低5 7位
        buff[index++]=0xc0|(val & 0x1f); // 0x30 start of data
        buff[index++]=analogInString[i][1];
        buff[index++]=val >>5 & 0x7f;
    }
    // （2）发送模拟输出数据
    for (i = 0; i < analogOutCount; ++i)
    {
        memcpy(buff+index,analogOutString[i],3);
        index+=3;
    }
    // （3）发送数字监控数据
    for (i = 0; i < digitalInCount; ++i)
    {
        //(3.1)如果是数字监控为1，将digitalInString的第一字节的第0位置为1
        if (LAB_DIGITAL_GET(digitalInList[i]) == INPUT_ON)
        {
            buff[index++]=0b11100001;
        }
        // (3.2)如果是数字监控为0，将digitalInString的第一字节的第3位置为0
        else
        {
            buff[index++]=0b11100000;
        }
        buff[index++]=digitalInString[i][1];
    }
    // （4）发送数字输出数据
    for (i = 0; i < digitalOutCount; ++i)
    {
        memcpy(buff+index,digitalOutString[i],2);
        index+=2;
    }
    if(index!=0)sendFrame(UART_User,index,buff);
}
//====


//主函数，一般情况下可以认为程序从此开始运行（实际上有启动过程）-----------
int main(void)
{
    printf("系统启动...\r\n");

    uint8_t i,flag,type,pin;
    char ch;
	uint16_t d1;
    uint8_t temp[MSG_SIZE];      //存放一个消息（每个消息为16个字节）
    uint8_t light_data=1;
    uint16_t light_count=0,data_length;
    digitalInCount = digitalOutCount = analogInCount = analogOutCount = 0;
    LAB_DIGITAL_CTRL(LIGHT_BLUE,light_data^=1);

    printf("初始化...\r\n");
	DISABLE_INTERRUPTS;
    uart_init(UART_User,115200);
    uart_enable_re_int(UART_User);
    queue_init();
	ENABLE_INTERRUPTS;
  
    printf("------------------------------------------------------\n");
    printf("    金葫芦提示： GEC_LAB下位机软件v3.0 20241124         \n");
    printf("    金葫芦提示： for pc 1.3.x                          \n");
    printf("    金葫芦提示： 无操作系统版本                          \n");
    printf("------------------------------------------------------\n");
    #ifdef __debug
    printf("！！！debug模式启动！！！\n");
    #endif
    while(1){
        //（1）等待消息，参数：消息名，消息内容，消息的字节数，永久等待
#ifdef __debug
        for(data_length =0 ;data_length<0xffff;++data_length) __asm("nop");
#endif
        for(data_length =0 ;data_length<0xff;++data_length) __asm("nop");
        if(data_length=queue_get(temp)){
        //（2）正常返回，有消息要处理，调用handle函数
            handle(temp,data_length);
        }
        if(light_count>800 ){
        //（3）当没有数据传入时才改变灯状态
            if( (analogInCount | analogOutCount | digitalInCount | digitalOutCount )==0)
                LAB_DIGITAL_CTRL(LIGHT_BLUE,light_data^=1);
            light_count=0;
        }
        light_count++;
        //（4）调用发送函数，每个循环都发送一次消息给上位机
        send();
    }
}   //main函数（结尾）


//以下为主函数调用的子函数================================================



//========================================================================
/*
知识要素：
（1）main.c是一个模板，该文件所有代码均不涉及具体的硬件和环境，通过调用构件
实现对硬件的干预。
（2）本文件中对宏GLOBLE_VAR进行了定义，所以在包含"includes.h"头文件时，会定
义全局变量，在其他文件中包含"includes.h"头文件时，
编译时会自动增加extern
*/


