/*
 * gpio.c
 *
 *  Created on: 2021年4月12日
 *      Author: liuxiao
 */

#include "gpio.h"


//内部函数声明
void gpio_get_port_pin(uint16_t port_pin,uint8_t* port,uint8_t* pin);

//=====================================================================
//函数名称：gpio_init
//函数返回：无
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//         dir：引脚方向（0=输入，1=输出,可用引脚方向宏定义）
//         state：端口引脚初始状态（0=低电平，1=高电平）
//功能概要：初始化指定端口引脚作为GPIO引脚功能，并定义为输入或输出，若是输出，
//         还指定初始状态是低电平或高电平
//=====================================================================
void gpio_init(uint16_t port_pin, uint8_t dir, uint8_t state)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);

    switch(port)
    {
    case 0: //端口A
        if(dir == 1)  //定义为输出引脚
        {
            R32_PA_DIR |= (1 << pin );
            gpio_set(port_pin,state);
        }
        else
        {
            R32_PA_DIR &=~(1 << pin );
        }
        break;
    case 1: //端口B
        if(dir == 1)  //定义为输出引脚
        {
            R32_PB_DIR |= (1 << pin );
            gpio_set(port_pin,state);
        }
        else
        {
            R32_PB_DIR &=~(1 << pin );
        }
        break;

    }
}

//=====================================================================
//函数名称：gpio_set
//函数返回：无
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//          state：希望设置的端口引脚状态（0=低电平，1=高电平）
//功能概要：当指定端口引脚被定义为GPIO功能且为输出时，本函数设定引脚状态
//=====================================================================
void gpio_set(uint16_t port_pin, uint8_t state)
{
   uint8_t port,pin;    //声明端口port、引脚pin变量
   //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
   gpio_get_port_pin(port_pin,&port,&pin);

   switch(port)
   {
   case 0: //端口A
       //根据state，设置对应引脚状态
       if(1 == state)    //高电平(该引脚对应置位寄存器置1)
           R32_PA_OUT |= (1<<pin);
       else              //低电平(该引脚对应重置寄存器置1)
           R32_PA_CLR |= (1<<pin);
       break;
   case 1: //端口B
       //根据state，设置对应引脚状态
      if(1 == state)    //高电平(该引脚对应置位寄存器置1)
          R32_PB_OUT |= (1<<pin);
      else              //低电平(该引脚对应重置寄存器置1)
          R32_PB_CLR |= (1<<pin);
      break;

   }
}

//=====================================================================
//函数名称：gpio_get
//函数返回：指定端口引脚的状态（1或0）
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//功能概要：当指定端口引脚被定义为GPIO功能且为输入时，本函数获取指定引脚状态
//=====================================================================
uint8_t gpio_get(uint16_t port_pin)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    uint32_t tempA;       //临时存放寄存器里的值
    uint32_t tempB;       //临时存放寄存器里的值
    uint8_t value;
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    tempA=R32_PA_DIR;
    tempB=R32_PB_DIR;
    switch(port)
    {
    case 0: //端口A
        if(tempA&(1<<pin))//GPIO输出
        {
            tempA=R32_PA_OUT;
            if((tempA&(1<<pin)))
                value=1;
            else
               value=0;
        }
        else //输入
        {
            tempA=R32_PA_PIN;
            if((tempA&(1<<pin)))
                value=1;
            else
                value=0;
        }
        break;
    case 1: //端口B
        tempB=R32_PB_PIN;
        if((tempB&(1<<pin)))
            value=1;
        else
            value=0;
        break;
    }
    return value;
}

//=====================================================================
//函数名称：gpio_reverse
//函数返回：无
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//功能概要：当指定端口引脚被定义为GPIO功能且为输出时，本函数反转引脚状态
//=====================================================================
void gpio_reverse(uint16_t port_pin)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    uint32_t tempA;       //临时存放寄存器里的值
    uint32_t tempB;       //临时存放寄存器里的值
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    tempA=R32_PA_OUT;
    tempB=R32_PB_OUT;
    switch(port)
    {
    case 0: //端口A
        if(tempA&(1<<pin))
            R32_PA_CLR |= (1<<pin);
        else
            R32_PA_OUT |= (1<<pin);
        break;
    case 1: //端口B
        if(tempB&(1<<pin))
            R32_PB_CLR |= (1<<pin);
        else
            R32_PB_OUT |= (1<<pin);
        break;

    }
}

//=====================================================================
//函数名称：gpio_pull
//函数返回：无
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//       pullselect：下拉/上拉（PULL_DOWN=下拉，PULL_UP=上拉）
//功能概要：当指定端口引脚被定义为GPIO功能且为输入时，本函数设置引脚下拉/上拉
//=====================================================================
void gpio_pull(uint16_t port_pin, uint8_t pullselect)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    switch(port)
    {
    case 0: //端口A
        if(pullselect == 1)  //上拉
        {
            R32_PA_PD_DRV &= ~(1<<pin);
            R32_PA_PU |= (1<<pin);
        }
        else { //下拉
            R32_PA_PD_DRV |= (1<<pin);
            R32_PA_PU &= ~(1<<pin);
        }
        break;
    case 1: //端口B
        if(pullselect == 1)  //上拉
        {
            R32_PB_PD_DRV &= ~(1<<pin);
            R32_PB_PU |= (1<<pin);
        }
        else { //下拉
            R32_PB_PD_DRV |= (1<<pin);
            R32_PB_PU &= ~(1<<pin);
        }
        break;
    }
}
//=====================================================================
//函数名称：gpio_enable_int
//函数返回：无
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//          irqtype：引脚中断类型，由宏定义给出，再次列举如下：
//                  RISING_EDGE  9      //上升沿触发
//                  FALLING_EDGE 10     //下降沿触发
//                  LowLevel   11       //低电平触发
//                  HighLevel  12       //高电平触发
//功能概要：当指定端口引脚被定义为GPIO功能且为输入时，本函数开启引脚中断，并
//          设置中断触发条件。
//=====================================================================
void gpio_enable_int(uint16_t port_pin,uint8_t irqtype)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    if(port==1&&pin==23) pin=9;
    if(port==1&&pin==22) pin=8;
    if(port==0)
    {
        switch( irqtype )
        {
            case LowLevel:      // 低电平触发
                R16_PA_INT_MODE &= ~(1<<pin);
                R32_PA_CLR |= (1<<pin);
                break;

            case HighLevel:     // 高电平触发
                R16_PA_INT_MODE &= ~(1<<pin);
                R32_PA_OUT |= (1<<pin);
                break;

            case FALLING_EDGE:      // 下降沿触发
                R16_PA_INT_MODE |= (1<<pin);
                R32_PA_CLR |= (1<<pin);
                break;

            case RISING_EDGE:      // 上升沿触发
                R16_PA_INT_MODE |= (1<<pin);
                R32_PA_OUT |= (1<<pin);
                break;

            default :
                break;
        }
        R16_PA_INT_IF |= (1<<pin);   //清中断标志位
        R16_PA_INT_EN |= (1<<pin);  //使能相应的中断
        PFIC_EnableIRQ(GPIO_A_IRQn);
    }
    else if(port==1)
    {
        switch( irqtype )
        {
            case LowLevel:      // 低电平触发
                R16_PB_INT_MODE &= ~(1<<pin);
                R32_PB_CLR |= (1<<pin);
                break;

            case HighLevel:     // 高电平触发
                R16_PB_INT_MODE &= ~(1<<pin);
                R32_PB_OUT |= (1<<pin);
                break;

            case FALLING_EDGE:      // 下降沿触发
                R16_PB_INT_MODE |= (1<<pin);
                R32_PB_CLR |= (1<<pin);
                break;

            case RISING_EDGE:      // 上升沿触发
                R16_PB_INT_MODE |= (1<<pin);
                R32_PB_OUT |= (1<<pin);
                break;
            default :
                break;
        }
        R16_PB_INT_IF |= (1<<pin);
        R16_PB_INT_EN |= (1<<pin);
        PFIC_EnableIRQ(GPIO_B_IRQn);
    }

}

//=====================================================================
//函数名称：gpio_get_int
//函数返回：引脚GPIO中断标志（1或0）1表示引脚有GPIO中断，0表示引脚没有GPIO中断。
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//功能概要：当指定端口引脚被定义为GPIO功能且为输入时,获取中断标志。
//=====================================================================
uint8_t gpio_get_int(uint16_t port_pin)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    uint8_t value;
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    if(port==1&&pin==23) pin=9;
    if(port==1&&pin==22) pin=8;
    switch(port)
    {
    case 0:
        if ((R16_PA_INT_IF&(1<<pin))==(1<<pin)) {
            value=1;
        }
        else value=0;
        break;
    case 1:
        if ((R16_PB_INT_IF&(1<<pin))==(1<<pin)) {
            value=1;
        }
        else value=0;
        break;
    }
    return value;
}

//=====================================================================
//函数名称：gpio_clear_int
//函数返回：无
//参数说明：port_pin：(端口号)|(引脚号)（如：(PTB_NUM)|(9) 表示为B口9号脚）
//功能概要：当指定端口引脚被定义为GPIO功能且为输入时,清除中断标志。
//=====================================================================
void gpio_clear_int(uint16_t port_pin)
{
    uint8_t port,pin;    //声明端口port、引脚pin变量
    //根据带入参数port_pin，解析出端口与引脚分别赋给port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    if(port==1&&pin==23) pin=9;
    if(port==1&&pin==22) pin=8;
    switch(port)
    {
    case 0:
        R16_PA_INT_IF |=(1<<pin);
        //PFIC_DisableIRQ(GPIO_A_IRQn);
        break;
    case 1:
        R16_PB_INT_IF |=(1<<pin);
        //PFIC_DisableIRQ(GPIO_B_IRQn);
        break;
    }
}

//----------------------以下为内部函数存放处-----------------
//=====================================================================
//函数名称：gpio_get_port_pin
//函数返回：无
//参数说明：port_pin：端口号|引脚号（如：(PTB_NUM)|(9) 表示为B口9号脚）
//       port：端口号（传指带出参数）
//       pin:引脚号（0~15，实际取值由芯片的物理引脚决定）（传指带出参数）
//功能概要：将传进参数port_pin进行解析，得出具体端口号与引脚号，分别赋值给port与pin,返回。
//       （例：(PTB_NUM)|(9)解析为PORTB与9，并将其分别赋值给port与pin）。
//=====================================================================
void gpio_get_port_pin(uint16_t port_pin,uint8_t* port,uint8_t* pin)
{
    *port = (port_pin>>8);
    *pin = port_pin;
}





