/*
 * gpio.c
 *
 *  Created on: 2021��4��12��
 *      Author: liuxiao
 */

#include "gpio.h"


//�ڲ���������
void gpio_get_port_pin(uint16_t port_pin,uint8_t* port,uint8_t* pin);

//=====================================================================
//�������ƣ�gpio_init
//�������أ���
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//         dir�����ŷ���0=���룬1=���,�������ŷ���궨�壩
//         state���˿����ų�ʼ״̬��0=�͵�ƽ��1=�ߵ�ƽ��
//���ܸ�Ҫ����ʼ��ָ���˿�������ΪGPIO���Ź��ܣ�������Ϊ�������������������
//         ��ָ����ʼ״̬�ǵ͵�ƽ��ߵ�ƽ
//=====================================================================
void gpio_init(uint16_t port_pin, uint8_t dir, uint8_t state)
{
    uint8_t port,pin;    //�����˿�port������pin����
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
    gpio_get_port_pin(port_pin,&port,&pin);

    switch(port)
    {
    case 0: //�˿�A
        if(dir == 1)  //����Ϊ�������
        {
            R32_PA_DIR |= (1 << pin );
            gpio_set(port_pin,state);
        }
        else
        {
            R32_PA_DIR &=~(1 << pin );
        }
        break;
    case 1: //�˿�B
        if(dir == 1)  //����Ϊ�������
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
//�������ƣ�gpio_set
//�������أ���
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//          state��ϣ�����õĶ˿�����״̬��0=�͵�ƽ��1=�ߵ�ƽ��
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ���ʱ���������趨����״̬
//=====================================================================
void gpio_set(uint16_t port_pin, uint8_t state)
{
   uint8_t port,pin;    //�����˿�port������pin����
   //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
   gpio_get_port_pin(port_pin,&port,&pin);

   switch(port)
   {
   case 0: //�˿�A
       //����state�����ö�Ӧ����״̬
       if(1 == state)    //�ߵ�ƽ(�����Ŷ�Ӧ��λ�Ĵ�����1)
           R32_PA_OUT |= (1<<pin);
       else              //�͵�ƽ(�����Ŷ�Ӧ���üĴ�����1)
           R32_PA_CLR |= (1<<pin);
       break;
   case 1: //�˿�B
       //����state�����ö�Ӧ����״̬
      if(1 == state)    //�ߵ�ƽ(�����Ŷ�Ӧ��λ�Ĵ�����1)
          R32_PB_OUT |= (1<<pin);
      else              //�͵�ƽ(�����Ŷ�Ӧ���üĴ�����1)
          R32_PB_CLR |= (1<<pin);
      break;

   }
}

//=====================================================================
//�������ƣ�gpio_get
//�������أ�ָ���˿����ŵ�״̬��1��0��
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ����ʱ����������ȡָ������״̬
//=====================================================================
uint8_t gpio_get(uint16_t port_pin)
{
    uint8_t port,pin;    //�����˿�port������pin����
    uint32_t tempA;       //��ʱ��żĴ������ֵ
    uint32_t tempB;       //��ʱ��żĴ������ֵ
    uint8_t value;
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    tempA=R32_PA_DIR;
    tempB=R32_PB_DIR;
    switch(port)
    {
    case 0: //�˿�A
        if(tempA&(1<<pin))//GPIO���
        {
            tempA=R32_PA_OUT;
            if((tempA&(1<<pin)))
                value=1;
            else
               value=0;
        }
        else //����
        {
            tempA=R32_PA_PIN;
            if((tempA&(1<<pin)))
                value=1;
            else
                value=0;
        }
        break;
    case 1: //�˿�B
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
//�������ƣ�gpio_reverse
//�������أ���
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ���ʱ����������ת����״̬
//=====================================================================
void gpio_reverse(uint16_t port_pin)
{
    uint8_t port,pin;    //�����˿�port������pin����
    uint32_t tempA;       //��ʱ��żĴ������ֵ
    uint32_t tempB;       //��ʱ��żĴ������ֵ
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    tempA=R32_PA_OUT;
    tempB=R32_PB_OUT;
    switch(port)
    {
    case 0: //�˿�A
        if(tempA&(1<<pin))
            R32_PA_CLR |= (1<<pin);
        else
            R32_PA_OUT |= (1<<pin);
        break;
    case 1: //�˿�B
        if(tempB&(1<<pin))
            R32_PB_CLR |= (1<<pin);
        else
            R32_PB_OUT |= (1<<pin);
        break;

    }
}

//=====================================================================
//�������ƣ�gpio_pull
//�������أ���
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//       pullselect������/������PULL_DOWN=������PULL_UP=������
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ����ʱ��������������������/����
//=====================================================================
void gpio_pull(uint16_t port_pin, uint8_t pullselect)
{
    uint8_t port,pin;    //�����˿�port������pin����
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    switch(port)
    {
    case 0: //�˿�A
        if(pullselect == 1)  //����
        {
            R32_PA_PD_DRV &= ~(1<<pin);
            R32_PA_PU |= (1<<pin);
        }
        else { //����
            R32_PA_PD_DRV |= (1<<pin);
            R32_PA_PU &= ~(1<<pin);
        }
        break;
    case 1: //�˿�B
        if(pullselect == 1)  //����
        {
            R32_PB_PD_DRV &= ~(1<<pin);
            R32_PB_PU |= (1<<pin);
        }
        else { //����
            R32_PB_PD_DRV |= (1<<pin);
            R32_PB_PU &= ~(1<<pin);
        }
        break;
    }
}
//=====================================================================
//�������ƣ�gpio_enable_int
//�������أ���
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//          irqtype�������ж����ͣ��ɺ궨��������ٴ��о����£�
//                  RISING_EDGE  9      //�����ش���
//                  FALLING_EDGE 10     //�½��ش���
//                  LowLevel   11       //�͵�ƽ����
//                  HighLevel  12       //�ߵ�ƽ����
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ����ʱ�����������������жϣ���
//          �����жϴ���������
//=====================================================================
void gpio_enable_int(uint16_t port_pin,uint8_t irqtype)
{
    uint8_t port,pin;    //�����˿�port������pin����
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
    gpio_get_port_pin(port_pin,&port,&pin);
    if(port==1&&pin==23) pin=9;
    if(port==1&&pin==22) pin=8;
    if(port==0)
    {
        switch( irqtype )
        {
            case LowLevel:      // �͵�ƽ����
                R16_PA_INT_MODE &= ~(1<<pin);
                R32_PA_CLR |= (1<<pin);
                break;

            case HighLevel:     // �ߵ�ƽ����
                R16_PA_INT_MODE &= ~(1<<pin);
                R32_PA_OUT |= (1<<pin);
                break;

            case FALLING_EDGE:      // �½��ش���
                R16_PA_INT_MODE |= (1<<pin);
                R32_PA_CLR |= (1<<pin);
                break;

            case RISING_EDGE:      // �����ش���
                R16_PA_INT_MODE |= (1<<pin);
                R32_PA_OUT |= (1<<pin);
                break;

            default :
                break;
        }
        R16_PA_INT_IF |= (1<<pin);   //���жϱ�־λ
        R16_PA_INT_EN |= (1<<pin);  //ʹ����Ӧ���ж�
        PFIC_EnableIRQ(GPIO_A_IRQn);
    }
    else if(port==1)
    {
        switch( irqtype )
        {
            case LowLevel:      // �͵�ƽ����
                R16_PB_INT_MODE &= ~(1<<pin);
                R32_PB_CLR |= (1<<pin);
                break;

            case HighLevel:     // �ߵ�ƽ����
                R16_PB_INT_MODE &= ~(1<<pin);
                R32_PB_OUT |= (1<<pin);
                break;

            case FALLING_EDGE:      // �½��ش���
                R16_PB_INT_MODE |= (1<<pin);
                R32_PB_CLR |= (1<<pin);
                break;

            case RISING_EDGE:      // �����ش���
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
//�������ƣ�gpio_get_int
//�������أ�����GPIO�жϱ�־��1��0��1��ʾ������GPIO�жϣ�0��ʾ����û��GPIO�жϡ�
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ����ʱ,��ȡ�жϱ�־��
//=====================================================================
uint8_t gpio_get_int(uint16_t port_pin)
{
    uint8_t port,pin;    //�����˿�port������pin����
    uint8_t value;
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
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
//�������ƣ�gpio_clear_int
//�������أ���
//����˵����port_pin��(�˿ں�)|(���ź�)���磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ����ʱ,����жϱ�־��
//=====================================================================
void gpio_clear_int(uint16_t port_pin)
{
    uint8_t port,pin;    //�����˿�port������pin����
    //���ݴ������port_pin���������˿������ŷֱ𸳸�port,pin
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

//----------------------����Ϊ�ڲ�������Ŵ�-----------------
//=====================================================================
//�������ƣ�gpio_get_port_pin
//�������أ���
//����˵����port_pin���˿ں�|���źţ��磺(PTB_NUM)|(9) ��ʾΪB��9�Žţ�
//       port���˿ںţ���ָ����������
//       pin:���źţ�0~15��ʵ��ȡֵ��оƬ���������ž���������ָ����������
//���ܸ�Ҫ������������port_pin���н������ó�����˿ں������źţ��ֱ�ֵ��port��pin,���ء�
//       ������(PTB_NUM)|(9)����ΪPORTB��9��������ֱ�ֵ��port��pin����
//=====================================================================
void gpio_get_port_pin(uint16_t port_pin,uint8_t* port,uint8_t* pin)
{
    *port = (port_pin>>8);
    *pin = port_pin;
}





