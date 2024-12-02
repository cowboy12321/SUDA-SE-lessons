
    .ascii "\n"
    .ascii "-----------------------------------------------------------------\n"
    .ascii "★金葫芦提示★                                                   \n"
    .ascii "【中文名称】RISC-V汇编工程框架（GPIO构件测试样例）                \n"
    .ascii "【程序功能】基于GPIO汇编构件控制蓝灯闪烁                          \n"
    .ascii "【硬件连接】见本工程05_UserBoard文件夹下user.inc文件              \n"
    .ascii "【操作说明】观察硬件板上的蓝灯                                    \n"
    .ascii "----------------------------------------------------------------\n\0"

1．Flash
512K 字节非易失存储 FlashROM： （Flash扇区大小4KB）
（1）448KB 用户应用程序存储区 CodeFlash（0x00000000-0x0006FFFF）
（2）32KB 用户非易失数据存储区 DataFlash（0x00070000-0x00077FFF）
（3）24KB 系统引导程序存储区 BootLoader（0x00078000-0x0007DFFF）
（4）8KB 系统非易失配置信息存储区 InfoFlash（0x0007E000-0x0007FFFF）
2．RAM
18K 字节易失数据存储 SRAM：（0x20003800-0x20007FFF）
（1）16KB 双电源供电的睡眠保持存储区 RAM16K
（2）2KB 双电源供电的睡眠保持存储区 RAM2K
3．引脚信息
芯片提供了2组GPIO端口PA和PB，共22个通用输入输出引脚，每个引脚都具有中断和唤醒功能，部分引脚具有复用及映射功能。
PA 端口中，仅PA[4]、PA[5]、PA[8]～PA[15]位有效，对应芯片上10个GPIO引脚，PB 端口中，仅PB[0]、PB[4]、PB[6]、
PB[7]、PB[10]～PB[15]、PB[22]、PB[23]位有效，对应芯片上12个GPIO引脚。

4．WCHISPTool下载
http://www.wch.cn/downloads/WCHISPTool_Setup_exe.html 


【20240202】
    1.CH573没有总中断需要cpu.h内对应设置空值
    2.由于BIOS程序的Flash扇区的大小和编号修正过所以对应修改链接文件、flash构件、mcu.h、gec.h内的扇区信息
    3.因为要求做到可07文件夹可移植，所以原先的isr.c内中断函数存在硬件压栈和设定代码段的设计是不合理的，可以通过对应修改user.h内的串口中断宏定义

【20240203】修改为汇编样例程序
    1.由于新增了汇编的.s源程序文件和.inc头文件，需要对makefile作出修改，保证编译时能寻址到头文件
    新增对.s文件的处理
        $(D_OBJ)/%.o:$(SRC_S)/%.s
        @echo 'Building file: $<'
        @echo 'Invoking: GNU RISC-V Cross Assembler'
        riscv-none-embed-gcc -march=rv32imac -mabi=ilp32 -msmall-data-limit=8 -mno-save-restore -$(opt)\
    -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -Wunused -Wuninitialized -g\
    -x assembler-with-cpp $(include) -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -c -o "$@" "$<"
        @echo 'Finished building: $<'
        @

    2.为了实现在MounRiver Studio端的交叉编译，同样需要设置头文件路径
    Properties->C/C++ Build->Settings->GNU RISC-V Cross Assembler->Includes，在其中添加头文件目录
    
    3.将GPIO构件使用汇编语言编写后，由于其源程序为.S文件，所以在makefile中同样需要加上头文件的路径，
      否则会出现无法找到头文件的问题

【20240311】 修正uart_re1()函数，再接受完一个字节后通过读取LSR寄存器清空中断标志位
      
【20240314】启动工作在机器模式，新增全局中断

【20240327】BIOS升至V1.5 RTOS，修改USER扇区及RAM空间；新增中断继承Vectors相关处理

//功能说明：BLE收发程序一体，默认为接收状态，可通过uart1串口中断发送待发送数据，
//此时变为接收状态，发送数据格式为：:  +  有效数据    + ;（帧头+有效数据+帧尾），
//去掉4字节软件地址，一次性最大可发送248字节。发送成功后，又切换成接收模式

【20240717】修改gpio.s
根据涂定凡反馈的信息来看，之前CH573的GPIO构件存在问题：
当gpio初始化为OUTPUT模式时，再将其初始化为INPUT模式会无法成功
从代码中可以看到原来的操作模式为    GPIO_OUTPUT = 0x1 , GPIO_INPUT = 0x0
    case 0: //端口A
        if(dir == 1)  //定义为输出引脚
        {
            R32_PA_DIR |= (GPIO_OUTPUT << pin );
            gpio_set(port_pin,state);
        }
        else
        {
            R32_PA_DIR &=~(GPIO_INPUT << pin );
        }
        break;
当初始化为OUTPUT模式时 
    R32_PA_DIR 对应pin位为1
再将其初始化为INPUT模式时，对应pin位的变化位
    0x1 &= ~(0x0) = 0x1 & 0x1 = 0x1
结果还是输出模式不变，应该修改为
    0x1 &= ~(0x1) = 0x1 & 0x0 = 0x0
即修改输入模式配置处的GPIO_INPUT为GPIO_OUTPUT
本汇编代码的gpio.s对应修改