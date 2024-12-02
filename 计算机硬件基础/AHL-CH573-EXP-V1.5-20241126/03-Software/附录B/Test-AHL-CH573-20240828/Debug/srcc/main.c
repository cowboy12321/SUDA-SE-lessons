//======================================================================
// 文件名称：main.c（应用工程主函数）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 版本更新：20240828
// 功能描述：见本工程的..\01_Doc\Readme.txt
// 移植规则：【固定】
//======================================================================
#define GLOBLE_VAR
#include "includes.h" //包含总头文件

//----------------------------------------------------------------------
// 声明使用到的内部函数
// main.c使用的内部函数声明处
void Delay_ms(uint16_t u16ms);

//----------------------------------------------------------------------
// 主函数，一般情况下可以认为程序从此开始运行（实际上有启动过程，参见书稿）
int main(void)
{
    // 【1】======启动部分（开头）==========================================
    // （1.1）声明main函数使用的局部变量
    uint32_t mCount;      // 延时的次数
    uint32_t i;           // 循环变量
    uint16_t mcu_temp_AD; // 芯片温度AD值
    float mcu_temp;       // 芯片温度回归值
    float preTemp;        // 上一次温度传感器温度
    // （1.2）【不变】关总中断
    DISABLE_INTERRUPTS;

    // （1.3）给主函数使用的局部变量赋初值
    mCount = 0; // 记次数
    preTemp = 0;

    // （1.4）给全局变量赋初值

    // （1.5）用户外设模块初始化
    gpio_init(LIGHT_BLUE, GPIO_OUTPUT, LIGHT_OFF);  // 初始化蓝灯
    gpio_init(LIGHT_GREEN, GPIO_OUTPUT, LIGHT_OFF); // 初始化绿灯
    gpio_init(LIGHT_RED, GPIO_OUTPUT, LIGHT_OFF);   // 初始化红灯
    adc_init(TEMPSENSOR_CHANNEL, 0);                // 初始化AD芯片温度模块
    uart_init(UART_User, 115200);                   // 初始化串口模块
    Delay_ms(100);                                  // 延时100ms防止ADC还未采样完成造成第一次读数错误
    // （1.6）使能模块中断
    uart_enable_re_int(UART_User);

    // （1.7）【不变】开总中断
    ENABLE_INTERRUPTS;

    // 【1】======启动部分（结尾）==========================================

    // 【2】======主循环部分（开头）========================================
    for (;;) // for(;;)（开头）
    {
        // （2.1）延时1秒
        Delay_ms(1000);

        // （2.2）读取芯片 温度
        printf("------------------------\r\n\n");
        mcu_temp_AD = adc_ave(TEMPSENSOR_CHANNEL,10);
        mcu_temp = adc_mcu_temp(mcu_temp_AD); // 将芯片温度AD值转换成实际温度
        printf("mcu_temp_AD = %d\n", mcu_temp_AD);
        printf("芯片内部温度为:%6.2lf℃\r\n", mcu_temp);

        if (preTemp > 0 && (mcu_temp > preTemp) && (mcu_temp - preTemp >= 1.0))
        {
            preTemp = mcu_temp; // 记录下上次的温度
            printf("触摸芯片！！！！！\n");
            // 只有当灯原来是亮的情况下才将灯置灭
            // 保证灯是灭的
            gpio_set(LIGHT_BLUE,LIGHT_OFF);
            gpio_set(LIGHT_GREEN,LIGHT_OFF);
            gpio_set(LIGHT_RED,LIGHT_OFF);
            // 闪烁黄灯
            printf("指示灯颜色为【黄色】");
            for (i = 0; i < 3; i++)
            {
                gpio_reverse(LIGHT_RED);
                gpio_reverse(LIGHT_GREEN);
                Delay_ms(1000);
                gpio_reverse(LIGHT_RED);
                gpio_reverse(LIGHT_GREEN);
                Delay_ms(1000);
            }
            // 将灯恢复之前的状态
            if ((mCount / 5) & 1)
                gpio_reverse(LIGHT_RED);
            if ((mCount / 10) & 1)
                gpio_reverse(LIGHT_GREEN);
            if ((mCount / 20) & 1)
                gpio_reverse(LIGHT_BLUE);
            continue;
        }
        else
        {
            preTemp = mcu_temp; // 记录下上次的温度
        }

        mCount++;
        // 当秒数40秒时，重新开始计数
        // 避免一直累加
        if (mCount >= 40)
        {
            mCount = 0;
            printf("指示灯颜色为【暗色】");
        }
        // （2.3）如灯状态标志mFlag为'L'，灯的闪烁次数+1并显示，改变灯状态及标志
        if (mCount % 5 == 0)
        {
            gpio_reverse(LIGHT_RED);
            printf(" LIGHT_RED:reverse--\n"); // 串口输出灯的状态
            if (mCount / 5 == 1)
            {
                printf("指示灯颜色为【红色】");
            }
            else if (mCount / 5 == 3)
            {
                printf("指示灯颜色为【黄色】");
            }
            else if (mCount / 5 == 5)
            {
                printf("指示灯颜色为【紫色】");
            }
            else if (mCount / 5 == 7)
            {
                printf("指示灯颜色为【白色】");
            }
        }
        if (mCount % 10 == 0) // 判断灯的状态标志
        {
            gpio_reverse(LIGHT_GREEN);
            printf(" LIGHT_GREEN:reverse--\n"); // 串口输出灯的状态
            if (mCount / 10 == 1)
            {
                printf("指示灯颜色为【绿色】");
            }
            else if (mCount / 10 == 3)
            {
                printf("指示灯颜色为【青色】");
            }
        }
        if (mCount % 20 == 0) // 判断灯的状态标志
        {
            gpio_reverse(LIGHT_BLUE);
            printf(" LIGHT_BLUE:reverse--\n"); // 串口输出灯的状态
            if (mCount / 20 == 1)
            {
                printf("指示灯颜色为【蓝色】");
            }
        }
    } // for(;;)结尾
    // 【2】======主循环部分（结尾）========================================
} // main函数（结尾）

//======以下为主函数调用的子函数===========================================
//======================================================================
// 函数名称：Delay_ms
// 函数返回：无
// 参数说明：近似毫秒
// 功能概要：延时 - 毫秒级
//======================================================================
void Delay_ms(uint16_t u16ms)
{
    for (volatile uint32_t i = 0; i < SYS_FREQ / 72000 * u16ms; i++)
        __asm("NOP");
}

//========================================================================
/*
知识要素：
（1）main.c是一个模板，该文件所有代码均不涉及具体的硬件和环境，通过调用构件
实现对硬件的干预。
（2）本文件中对宏GLOBLE_VAR进行了定义，所以在包含"includes.h"头文件时，会定
义全局变量，在其他文件中包含"includes.h"头文件时，
编译时会自动增加extern
*/
