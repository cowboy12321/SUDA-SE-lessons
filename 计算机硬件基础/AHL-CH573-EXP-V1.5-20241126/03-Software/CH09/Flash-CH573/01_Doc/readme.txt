//功能说明：BLE收发程序一体，默认为接收状态，可通过uart1串口中断发送待发送数据，
//此时变为接收状态，发送数据格式为：:  +  有效数据    + ;（帧头+有效数据+帧尾），
//去掉4字节软件地址，一次性最大可发送248字节。发送成功后，又切换成接收模式


1．Flash
256K 字节非易失存储 FlashROM： 
（1）192KB 用户应用程序存储区 CodeFlash（0x00000000-0x0002FFFF）
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

【20240311】
修正uart_re1()函数，再接受完一个字节后通过读取LSR寄存器清空中断标志位
 
【20240314】
	【更正】CH573存在总中断，原先启动在用户模式无法操作mstatus寄存器实现开关总中断操作，将启动文件中mstatus的起始值修改为0x1880启动在机器模式

【20240323】
    1.BIOS升级至V1.5 RT-Thread驻留，对应扇区号增大，修改相关参数
    2.为了适配RT-Thread驻留的BIOS版本，User段的RAM起始地址需要预留出至少6K的空间，从0x20006400开始
    3.新增SW_Handler和Systick的中断继承

【20240324】
1.新增Vectors_Init中断继承，原先中断继承方式为先触发User程序里面的UART_Update中断，再在中断内跳转至BIOS中断
    UART1_IRQHandler()
    {
        __asm("跳转至BIOS中断")
    }
    先修改为重定向USER中断向量表0xc070(UART1_IRQHandler中断位置)，为“j 0x94<BIOS 的UART1中断>”
    由于CH573中断向量表内填的不是中断函数入口地址而是跳转指令，所以无法直接调用BIOS那边0x70的指令，需要用绝对跳转指令跳转
2.对齐.text段代码，解决flash_erase()地址未对齐导致的连续复位问题
3.由于USER RAM空间有限，在使用flash_write函数的时候，由于其内部会拷贝一份需要写入的输入，所以写入数据data[]的大小不能过大
  过大会导致创建的临时数组不在USER RAM的地址范围内，从而导致FLASH_ROM_WRITE无法定位位置

【20240325】
1.linkfile文件对齐text段，调用flash_erase会因为地址没对齐而复位
2.控制flash_write的长度，函数内部会申请RAM空间，太大的话会爆，
3.新增Vectors_Init()处理中断继承

【20240905】V2.0在睿师傅的基础上更新，优化中断继承功能
由于软件测试功能对于UART1_IRQHandler做出了修改，导致SW_Handler的入口地址发生变动
原先采用的固定地址的中断继承方式不再使用，经由睿师傅提案，采取如下更新
    0.原先三个函数卡死的位置不用卡了
    1.利用commopent_list继承sw_handler,systick_handler,uart1_irqhandler的中断函数入口地址
    2.在User里面设计一个指令转换函数
        首先读取中断向量表里面sw_handler,systick_handler,uart1_irqhandler中断向量表里面对应的操作码
        解析操作码，算出对应的PC值，User里面的入口地址-操作码里面的偏移量=当前指令的PC值
        然后根据PC值-BIOS里面的函数入口地址得到偏移量
        最后根据偏移量生成新的操作码并复写

    imm[20]     imm[10:1]     imm[11]     imm[19:12]     [rd]     [opcode]
    x           xxxxxxxxxx    x           xxxxxxxx       00000    1101111

    1.user.h里面extern一下SysTick_Handler和SW_Handler
    2.在gec.c里面的ComponentFun表的28，29，30号位置记录SysTick_Handler，SW_Handler，UART1_IRQHandler在BIOS里面的入口地址

    USER_IRQ - USER_IMM == USER_PC
    USER_PC  - BIOS_IRQ == TARGET_IMM
    TARGET_IMM  -->  TARGET_CMD

【20240912】优化中断继承
上面那个中断继承有点问题，如果直接使用冷热复位的话，最后写入的opc会有问题
因为flash内存储的值不会变动，所以会再跑一遍指令转换的函数
现在加一条判断，判断当前user中断向量表里面的指令非否为向前跳转，如果为向前跳转说明已经继承了BIOS的中断，此时无需再次指令转换
    //判断该指令是否为向前跳转，如果是向前跳转说明已经修改过了中断向量表，无需再次修改
    if((user_opc&0x80000000)) return ((uint32_t *)user)[IRQ_NUM+1];   

【20241104】RF无线传输
新增RF无线传输功能
    1.所使用的各类函数都重定向至BIOS内的对应函数，这么处理是为了节约RAM资源
    LIBCHxBLE.a静态库内的部分函数被定义在了RAM空间内，总大小4K+，考虑到CH573 User部分一共10K左右的内存
    就不在User段重新再分配4K+空间链接无线蓝牙静态库了
    2.由于RF相关信息都定义在了第110号扇区，不建议在User代码内直接修改，如果要修改可以使用对应的IDE功能
    所以设计通过GetRFAddr来获取相关信息。不手动修改RF硬件地址的另一个原因是硬件地址也会和 “无线User更新” 功能相关，随意修改可能会使该功能失效
    3.无线更新两个重要的响应函数为
         RF_ProcessEvent        事件处理函数，调用tmos_set_event后触发
         RF_2G4StatusCallBack   回调函数，处理完tmos_set_event的事件或是收到别的设备发来的数据后触发
                                收到的数据对应在rf_Receive()函数里面处理

改模板可以用于：运行在User段程序时，如果接收到了更新数据帧，则会自动跳转BIOS进入从机无线更新等待状态

【注】FLASH的相关操作也继承了BIOS里面的，省点空间