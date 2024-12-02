//======================================================================
//�ļ����ƣ�main.c��Ӧ�ù�����������
//����ṩ���մ�Ƕ��ʽ��sumcu.suda.edu.cn��
//�汾���£�201911-202306
//�����������������̵�..\01_Doc\Readme.txt
//��ֲ���򣺡��̶���
//======================================================================
#define GLOBLE_VAR     //���̶���includes.h�����ȫ�ֱ���һ�������ദʹ��
#include "includes.h"  //���̶���������ͷ�ļ�

//main.cʹ�õ��ڲ�����������---------------------------------------------
enum{ mcu_desc_len=6};
uint16_t analogInList[PIN_MAX];       // ģ�����������б�
uint8_t analogInString[PIN_MAX][4];   // ģ�������ַ���
uint8_t analogInCount;           // ģ�������ֽ���
uint8_t analogOutString[PIN_MAX][4];  // ģ������ַ���
uint8_t analogOutCount;          // ģ������ֽ���
uint16_t digitalInList[PIN_MAX];      // �������������б�
uint8_t digitalInString[PIN_MAX][2];  // ���������ַ���
uint8_t digitalInCount;          // ���������ֽ���
uint8_t digitalOutString[PIN_MAX][2]; // ��������ַ���
uint8_t digitalOutCount;         // ��������ֽ���

// #define __debug
//ǰ22λ��汾+18λС�汾+6λMCU-id��
uint8_t mcu_desc[mcu_desc_len]={0xa0,0x00,0x03,0x00,0x00,0x03};
// #define __debug
//======================================================================
//�������ƣ�handle
//�������أ���
//����˵����data������ָ�룬size�����ݴ�С
//���ܸ�Ҫ������temp�е�����
//�ڲ����ã���
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
        //��1.1�������ܵ�����Ϊ0b1001_0000�������û���Ϣ����ջ���
        uart_sendN(UART_User,mcu_desc_len,mcu_desc);
        analogInCount = analogOutCount = digitalInCount = digitalOutCount = 0;
        // printf("shake\n");
        return;
    }
    flag = i = 0; // ��ǰ���ڴ��������ָ��
    while (i < size)
    {
        switch (data[i])
        {
            case 0xa0:
            // ��2.1.1�������ܵ�����Ϊ0b1010_0000��������뻺��
                analogInCount = digitalInCount = 0;
                i++;
                #ifdef __debug
                printf("clear in\n");
                #endif
                continue;
            case 0xa1:
            // ��2.1.2�������ܵ�����Ϊ0b1010_0001������������
                analogOutCount = digitalOutCount = 0;
                i++;
                printf("clear out\n");
                continue;
        }

        if(data[i]==FrameTail) break;
        // ��2.3�����յ������ݿ�ͷ����0b1000_0000��Ϊָ����Ϣ
        //        type:ģʽ�ţ�ȡ�����ֽڵĵ�3-5λ:
        //             000: ģ������(adc)
        //             001: ģ�����(dac)
        //             010: ��������(gpio_in)
        //             011: �������(gpio_out)
        //        pin: ���źţ�ȡ�����ֽڵĵ�0-1λ�͵��ֽڵĵ�0-5λ��ȡֵ��ΧΪ0-255
        //        val: ��ֵ��Ĭ��Ϊ0
        type = (data[i] >> 5) & 0x7;
        pin = data[i+1] &0x7f;
        #ifdef __debug
        printf("type=%d,pin=%d\n",type,pin);
        #endif
        //��2.4�� ����code��ֵ���в�ͬ�Ĵ���
        switch (type)
        {
        case 0b000:
            // ��2.4.1��0b000Ϊģ�����룬2�ֽ�����
            // ��a����ģ���������źŴ���analogInList
            analogInList[analogInCount] = analogInPin[pin];
            LAB_ANALOG_INIT(analogInPin[pin]);
            // ��b����ģ��������ֵ����analogInString
            analogInString[analogInCount][1] = data[i + 1] ;
            // ��c��ģ�������ֽ��� + 1
            analogInCount++;
            // ��d��ģ������ռ��2�ֽ�
            #ifdef __debug
            printf("ai,count=%d\n",analogInCount);
            #endif
            i += 2;
            break;
        case 0b001:
            // ��2.4.2��0b001Ϊģ�������3�ֽ�����
            // (a)��ģ�������ֵ����analogOutString
            analogOutString[analogOutCount][0] = 0xc0 | data[i] & 0x1f;
            analogOutString[analogOutCount][1] = data[i + 1] & 0x7f;
            analogOutString[analogOutCount][2] = data[i + 2] & 0x7f;
            // (b)ģ������ֽ��� + 1
            analogOutCount++;
            // (c)��ģ�������ֵ����val,������ģ�����
            val = (data[i + 2] & 0x7f) << 5 | data[i] & 0x1f;
            LAB_ANALOG_CTRL(analogOutPin[pin],val);
            // (d)ģ�����ռ��4�ֽ�
            i += 3;
            flag=1;
            #ifdef __debug
            printf("ao,v=%d,count=%d\n",val,analogOutCount);
            #endif
            break;
        case 0b010:
            // ��2.4.3��0b010Ϊ�������룬2�ֽ�����
            // ��a���������������źŴ���digitalInList
            digitalInList[digitalInCount] = digitalPin[pin];
            // ��b��������������ֵ����digitalInString
            digitalInString[digitalInCount][1] = data[i + 1] ;
            // ��c�����������ֽ��� + 1
            digitalInCount++;
            // ��d��������������
            LAB_DIGITAL_INIT(digitalPin[pin], GPIO_INPUT);
            // ��e����������ռ��2�ֽ�
            i += 2;
            #ifdef __debug
            printf("di,count=%d\n",digitalInCount);
            #endif
            break;
        case 0b011:
            // ��2.4.4��0b011Ϊ���������2�ֽ�����
            // ��a�������������ֵ����digitalOutString
            digitalOutString[digitalOutCount][0] = 0b11100000 | data[i];
            digitalOutString[digitalOutCount][1] = data[i + 1];
            // ��b����������ֽ��� + 1
            digitalOutCount++;
            // ��c�������������
            val = data[i] & 0x1f;
            LAB_DIGITAL_INIT(digitalPin[pin], GPIO_OUTPUT);
            LAB_DIGITAL_CTRL(digitalPin[pin], val? INPUT_ON : INPUT_OFF);
            // ��d���������ռ��2�ֽ�
            i += 2;
            flag=1;
            #ifdef __debug
            printf("do,v=%d,count=%d\n",val,digitalOutCount);
            #endif
            break;
        // ��2.4.5��Ĭ�������������һ�ֽ�
        default:
            i++;
        }
    }
}

//======================================================================
//�������ƣ�send
//�������أ���
//����˵������
//���ܸ�Ҫ���ռ���Ϣ�����͸���λ��
//�ڲ����ã���
//======================================================================
void send(){
    uint8_t i,index,buff[50];
    uint16_t val;
    index=0;
    // ��1������ģ��������
    for (i = 0; i < analogInCount; ++i)
    {
        //(3.1)��ȡ���ŵ�ѹ
        val = LAB_ANALOG_AVE(analogInList[i]);
        //(3.2)����ѹ�������ݵĵ�1 3���ֽڵĵ�5 7λ
        buff[index++]=0xc0|(val & 0x1f); // 0x30 start of data
        buff[index++]=analogInString[i][1];
        buff[index++]=val >>5 & 0x7f;
    }
    // ��2������ģ���������
    for (i = 0; i < analogOutCount; ++i)
    {
        memcpy(buff+index,analogOutString[i],3);
        index+=3;
    }
    // ��3���������ּ������
    for (i = 0; i < digitalInCount; ++i)
    {
        //(3.1)��������ּ��Ϊ1����digitalInString�ĵ�һ�ֽڵĵ�0λ��Ϊ1
        if (LAB_DIGITAL_GET(digitalInList[i]) == INPUT_ON)
        {
            buff[index++]=0b11100001;
        }
        // (3.2)��������ּ��Ϊ0����digitalInString�ĵ�һ�ֽڵĵ�3λ��Ϊ0
        else
        {
            buff[index++]=0b11100000;
        }
        buff[index++]=digitalInString[i][1];
    }
    // ��4�����������������
    for (i = 0; i < digitalOutCount; ++i)
    {
        memcpy(buff+index,digitalOutString[i],2);
        index+=2;
    }
    if(index!=0)sendFrame(UART_User,index,buff);
}
//====


//��������һ������¿�����Ϊ����Ӵ˿�ʼ���У�ʵ�������������̣�-----------
int main(void)
{
    printf("ϵͳ����...\r\n");

    uint8_t i,flag,type,pin;
    char ch;
	uint16_t d1;
    uint8_t temp[MSG_SIZE];      //���һ����Ϣ��ÿ����ϢΪ16���ֽڣ�
    uint8_t light_data=1;
    uint16_t light_count=0,data_length;
    digitalInCount = digitalOutCount = analogInCount = analogOutCount = 0;
    LAB_DIGITAL_CTRL(LIGHT_BLUE,light_data^=1);

    printf("��ʼ��...\r\n");
	DISABLE_INTERRUPTS;
    uart_init(UART_User,115200);
    uart_enable_re_int(UART_User);
    queue_init();
	ENABLE_INTERRUPTS;
  
    printf("------------------------------------------------------\n");
    printf("    ���«��ʾ�� GEC_LAB��λ�����v3.0 20241124         \n");
    printf("    ���«��ʾ�� for pc 1.3.x                          \n");
    printf("    ���«��ʾ�� �޲���ϵͳ�汾                          \n");
    printf("------------------------------------------------------\n");
    #ifdef __debug
    printf("������debugģʽ����������\n");
    #endif
    while(1){
        //��1���ȴ���Ϣ����������Ϣ������Ϣ���ݣ���Ϣ���ֽ��������õȴ�
#ifdef __debug
        for(data_length =0 ;data_length<0xffff;++data_length) __asm("nop");
#endif
        for(data_length =0 ;data_length<0xff;++data_length) __asm("nop");
        if(data_length=queue_get(temp)){
        //��2���������أ�����ϢҪ��������handle����
            handle(temp,data_length);
        }
        if(light_count>800 ){
        //��3����û�����ݴ���ʱ�Ÿı��״̬
            if( (analogInCount | analogOutCount | digitalInCount | digitalOutCount )==0)
                LAB_DIGITAL_CTRL(LIGHT_BLUE,light_data^=1);
            light_count=0;
        }
        light_count++;
        //��4�����÷��ͺ�����ÿ��ѭ��������һ����Ϣ����λ��
        send();
    }
}   //main��������β��


//����Ϊ���������õ��Ӻ���================================================



//========================================================================
/*
֪ʶҪ�أ�
��1��main.c��һ��ģ�壬���ļ����д�������漰�����Ӳ���ͻ�����ͨ�����ù���
ʵ�ֶ�Ӳ���ĸ�Ԥ��
��2�����ļ��жԺ�GLOBLE_VAR�����˶��壬�����ڰ���"includes.h"ͷ�ļ�ʱ���ᶨ
��ȫ�ֱ������������ļ��а���"includes.h"ͷ�ļ�ʱ��
����ʱ���Զ�����extern
*/


