//======================================================================
// 文件名称：main.c（应用工程主函数）
// 框架提供：苏大嵌入式（sumcu.suda.edu.cn）
// 更新记录：20231108-20240824
// 功能描述：见本工程的01_Doc\Readme.txt
// 移植规则：【固定】本工程07_AppPrg整个文件夹具备芯片无关性，差异体现在
//           05_UserBoard文件夹的User.h中
//======================================================================
#define GLOBLE_VAR     //【固定】includes.h定义的全局变量一处声明多处使用
#include "includes.h"  //【固定】包含总头文件

//main.c使用的内部函数声明处---------------------------------------------

//----------------------------------------------------------------------
//主函数，一般情况下可以认为程序从此开始运行（实际上有启动过程，参见书稿）
int main(void)
{
	printf("------------------------------------------------------\n");
    printf("★金葫芦提示★                                         \n");
    printf("【中文名称】GEC的NOS编程框架（Flash扇区读写数据测试)     \n");
	printf("【程序功能】                                           \n");
	printf("     ① 蓝色闪烁；                                      \n");
	printf("     ② 向FLASH中写入数据，并打印查看                     \n");
    printf("【测试过程】                                           \n");
	printf("     ① 向某扇区中写入数据，用printf打印查看              \n");
	printf("     ② 向绝对地址处开始写入数据表，用printf查看          \n");
    printf("------------------------------------------------------\n\0");  

//【1】启动部分（开头）==============================================
    //（1.1）【根据本函数所用的变量声明】声明main函数使用的局部变量
    uint32_t mMainLoopCount;  //主循环次数变量
    uint8_t  mFlag;           //灯的状态标志
    uint32_t mLightCount;     //灯的状态切换次数
	uint8_t mK1[32];	  //按照逻辑读方式从指定flash区域中读取的数据
	uint8_t mK2[32];      //按照物理读方式从指定flash区域中读取的数据
    
    uint8_t flash_test[32]={'A','B','C','D','E','F','G',' ','t',
                            'o',' ','S','o','o','c','h','o','w',' ',
                            'U','n','i','v','e','r','s','i','t','y','!'};
	uint8_t result;    //判断扇区是否为空标识

	//（1.2）【不变】关总中断
	DISABLE_INTERRUPTS;

    //（1.3）【根据本函数所用的变量赋初值】给主函数使用的局部变量赋初值
    mMainLoopCount=0;    //主循环次数变量
    mFlag='A'; 
    mLightCount=0;       //灯的闪烁次数

    //（1.4）【根据includes.h文件中声明的全局变量】给全局变量赋初值
    
    //（1.5）【根据所用到的外部硬件设备】进行用户外设模块初始化
    //（1.5.1）GPIO 引脚、 输入/输出、  初始状态
    gpio_init(LIGHT_BLUE,GPIO_OUTPUT,LIGHT_ON);
	//(1.5.2)FLASH初始化
	flash_init();
	
    //（1.6）【根据所使用的硬件模块中断】使能模块中断

    //（1.7）【不变】开总中断
    ENABLE_INTERRUPTS;
    
    //【1】启动部分（结尾）================================================
    //擦除第TEST_SECT扇区
	flash_erase(TEST_SECT);   
    //向TEST_SECT扇区第0偏移地址开始写32个字节数据
    flash_write(TEST_SECT,0,32,(uint8_t *) "Welcome to Soochow University!");
	flash_read_logic(mK1,TEST_SECT,0,32); //从该扇区读取32个字节到mK1中
	printf("（1）逻辑读方式读取第%d扇区的32字节的内容:  %s\n",TEST_SECT,mK1);
	
	//擦除第TEST_SECT扇区
	flash_erase(TEST_SECT);
	//向TEST_SECT扇区写32个字节数据，其起始地址TEST_ADDRESS（见user.h）
	flash_write_physical(TEST_ADDR,32,flash_test);
	flash_read_physical(mK2,TEST_ADDR,32);   //从该扇区读取32个字节到mK2中
	printf("（2）擦除重写后物理方式读第%d扇区的32字节的内容:  %s\n",TEST_SECT,mK2);

	result = flash_isempty(TEST_SECT,MCU_SECTORSIZE); // 判断扇区是否为空
	printf("（3）判别第%d扇区是否为空，1表示空，0表示不空，结果是：%d\n",TEST_SECT,result);
	
//【2】主循环部分（开头）==============================================
    while(1)   //while无限循环（开头）
    {
       //（2.1）主循环次数变量+1
       mMainLoopCount++;
       //（2.2）未达到主循环次数设定值，继续循环
       if (mMainLoopCount <= MAINLOOP_COUNT) continue; //宏常数在user.h中定义
       //（2.3）达到主循环次数设定值，执行下列语句，进行灯的亮暗处理
       //（2.3.1）清除循环次数变量
       mMainLoopCount=0; 
       //（2.3.2）根据灯的状态标志mFlag是否'L'进行操作
       if (mFlag=='L')  //若为'L'，置灯亮，灯闪烁次数+1，改变灯状态标志
       {
           gpio_set(LIGHT_BLUE,LIGHT_ON);      //置灯亮
           printf(" LIGHT_BLUE:ON   \n");      //输出提示
           mLightCount++;                       //灯闪烁次数+1
           printf(" 灯闪烁次数mLightCount = %d\n\n",mLightCount);
           mFlag='A';   //改变灯状态标志为'A'，下一循环使用
        }
        else            //若不为'L'（即为'A')
        {
           gpio_set(LIGHT_BLUE,LIGHT_OFF);
           printf(" LIGHT_BLUE:OFF  \n");  
           mFlag='L';            
        }
    }	//while无限循环（结尾）
//【2】主循环部分（结尾）=============================================
}   //main函数（结尾）

