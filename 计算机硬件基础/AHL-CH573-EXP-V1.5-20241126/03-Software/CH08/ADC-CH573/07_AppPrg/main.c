//======================================================================
// 文件名称：main.c（应用工程主函数）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 更新记录：202305
// 功能描述：见本工程的..\01_Doc\Readme.txt
// 移植规则：【固定】
//======================================================================
#define GLOBLE_VAR     //【固定】includes.h定义的全局变量一处声明多处使用
#include "includes.h"  //【固定】包含总头文件

//main.c使用的内部函数声明处---------------------------------------------
//主函数，一般情况下可以认为程序从此开始运行（实际上有启动过程）-----------
int main(void)
{
    printf("-----------------------------------------------------------\n");   
    printf("★金葫芦提示★                                         	    \n");
    printf("【中文名称】ADC构件的输出测试方法			        	    \n");
    printf("（1）目的：ADC单端输入与差分输入测试                    \n"); 
    printf("（2）单端：%s、内部温度传感器\n",singleName);
    printf("     差分：%s（%s、%s）\n",diffName,diffPinName1,diffPinName2);
    printf("（3）测试方法：单端：手摸芯片表面，A/D值增大，不要摸到引脚，静电可能损坏芯片\n");
    printf("                    将引脚%s分别接地、3.3V、5V，观察%s情况\n",singlePinName,singleName);
    printf("              差分：将引脚%s接地、%s接3.3V,观察%s情况\n",diffPinName1,diffPinName2,diffName);
    printf("                    将引脚%s接地、%s接3.3V,观察%s情况\n",diffPinName2,diffPinName1,diffName);
    printf("                    将引脚%s、%s短接,观察%s情况    \n" ,diffPinName1,diffPinName2,diffName);
    printf("------------------------------------------------------------\n"); 
    
    
//【1】启动部分（开头）==============================================
    
    //（1.1）【根据本函数所用的变量声明】声明main函数使用的局部变量
    vuint32_t mMainLoopCount; //主循环次数变量（主循环临时变量以m作为前缀）
    uint8_t   mFlag;          //灯的状态标志
    uint8_t   adcFlag;        //ADC检测标志
    uint32_t  mLightCount;    //灯的状态切换次数
    uint16_t num_AD1;
    uint16_t num_AD2;
    float temp;
    
    //（1.2）【不变】关总中断
    DISABLE_INTERRUPTS;
    
    //（1.3）【根据本函数所用的变量赋初值】给主函数使用的局部变量赋初值
    mMainLoopCount=0;    //主循环次数变量
    mFlag='A';           //灯的状态标志
    adcFlag=1;
    mLightCount=0;       //灯的闪烁次数
    
    //（1.4）【根据includes.h文件中声明的全局变量】给全局变量赋初值
    
    //（1.5）【根据所用到的外部硬件设备】用户外设模块初始化
    gpio_init(LIGHT_BLUE,GPIO_OUTPUT,LIGHT_ON);	//初始化蓝灯
    uart_init(UART_User, 115200);
    adc_init(DIFF_CHANNEL,0);
    adc_init(SINGLE_CHANNEL,0);
    adc_init(TEMPSENSOR_CHANNEL,0);

    //（1.6）【根据所使用的硬件模块中断】使能模块中断
    uart_enable_re_int(UART_User);     //使能用户串口接收中断
    //（1.7）【不变】开总中断
    ENABLE_INTERRUPTS;
    

//【1】启动部分（结尾）================================================

//【2】主循环部分（开头）==============================================
    for(;;)   //for(;;)（开头）
    {
        //（2.1）主循环次数变量+1
        mMainLoopCount++;
        //（2.2）未达到主循环次数设定值，继续循环
        if (mMainLoopCount<=MAINLOOP_COUNT) continue; //宏常数在user.h中定义
        //（2.3）达到主循环次数设定值，执行下列语句，进行灯的亮暗处理
        //（2.3.1）清除循环次数变量
        mMainLoopCount=0; 
        //（2.3.2）如灯状态标志mFlag为'L'，灯的闪烁次数+1并显示，改变灯状态及标志
        if (mFlag=='L')                    //判断灯的状态标志
        {
            mLightCount++;  
            printf(" 蓝灯闪烁次数mLightCount = %d\r\n",mLightCount);
            mFlag='A';                       //灯的状态标志
            gpio_set(LIGHT_BLUE,LIGHT_ON);  //灯“亮”
            printf(" LIGHT_BLUE:ON--\r\n"); //串口输出灯的状态
        }
        //（2.3.3）如灯状态标志mFlag为'A'，改变灯状态及标志
        else
        {
            mFlag='L';                       //灯的状态标志
            gpio_set(LIGHT_BLUE,LIGHT_OFF); //灯“暗”
            printf(" LIGHT_BLUE:OFF--\r\n");  //串口输出灯的状态
        }
        
        num_AD1 = adc_read(DIFF_CHANNEL);        
        printf("%s(%s、%s)的A/D值：  %d\r\n",diffName,diffPinName1,diffPinName2,num_AD1);  //串口输出差分通道1的AD值
        printf("%s电压：",diffName);
        printf("%.2f\n",diff_ad_to_voltage(num_AD1)); //串口输出差分通道1的AD值
        num_AD1 = adc_read(SINGLE_CHANNEL);        
        printf("%s(%s)的A/D值：  %d\r\n",singleName,singlePinName,num_AD1);           //串口输出差分通道1的AD值
        printf("%s(%s)电压： ",singleName,singlePinName);
        printf(" %.2f\r\n",single_ad_to_voltage(num_AD1));       //串口输出差分通道1的AD值
        num_AD2 = adc_ave(TEMPSENSOR_CHANNEL,5);                 //使用中值滤波和均值滤波
        printf("内部温度传感器的A/D值：    %d\r\n",num_AD2);       //串口输出内部传感器的AD值
        temp=adc_mcu_temp(num_AD2);
	    printf("mcu温度为%.2f度\r\n",temp);
        printf("   \r\n"); 
        
    }  //for(;;)结尾
//【2】主循环部分（结尾）=============================================
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

