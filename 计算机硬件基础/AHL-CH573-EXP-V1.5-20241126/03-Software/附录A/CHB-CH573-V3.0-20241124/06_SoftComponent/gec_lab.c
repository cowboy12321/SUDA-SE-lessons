#include "gec_lab.h"

//=====================================================================
// �������ƣ�crc16
// ���ܸ�Ҫ�������ݽ���16λ��CRCУ�飬����У���Ľ��ֵ
// ����˵����ptr:��ҪУ������ݻ�����
//      len:��Ҫ��������ݳ���
// �������أ�����õ���У��ֵ
//=====================================================================
uint16_t crc16(uint8_t *ptr, uint16_t len)
{
    uint16_t i, j, tmp, crc16;

    crc16 = 0xffff;
    for (i = (uint16_t)0; i < len; i++)
    {
        crc16 = (uint16_t)ptr[i] ^ crc16;
        for (j = 0; j < 8; j++)
        {
            tmp = crc16 & 0x0001;
            crc16 = crc16 >> 1;
            if (tmp)
                crc16 = crc16 ^ 0xa001;
        }
    }
    return crc16;
}

//=====================================================================
//�������ƣ�sendFrame
//�������أ� ��
//����˵���� length�����������ݵĳ��ȣ�data�����������ݻ�������
//���ܸ�Ҫ��ͨ��emuart����ָ�����ȵ��ֽ����顣
//���¼�¼��20180602��wb��
//=====================================================================
static uint8_t emuartFrameHead = 0x80;  //֡ͷ
static uint8_t emuartFrameTail = 0x81;  //֡β
void sendFrame(uint8_t UART_NO,uint16_t length,uint8_t *data)
{
    uint16_t crc;
    //����õ�CRCУ����
    crc = crc16(data,length);
    //����֡ͷ
    uart_send1(UART_NO,emuartFrameHead);
    //����֡��
    uart_send1(UART_NO,length);
    //����֡����
    uart_sendN(UART_NO,length,data);
    //����У����
    uart_send1(UART_NO,crc>>8);
    uart_send1(UART_NO,crc);
    //����֡β
    uart_send1(UART_NO,emuartFrameTail);
}

static char __buffer[BUFFER_SIZE];
static uint32_t __queue[QUEUE_SIZE];
static uint32_t __lens[QUEUE_SIZE];
static uint32_t __queue_front;
static uint32_t __queue_back;//���еİ�����Χ��[__queue_front,_queue_back)
static uint32_t __buffer_front;
static uint32_t __buffer_back;//���еİ�����Χ��[__queue_front,_queue_back)

#define buffer_front_off(x) ((__buffer_front + x) & BUFFER_MASK)
#define buffer_back_off(x)  ((__buffer_back  + x) & BUFFER_MASK)
#define queue_front_off(x)  ((__queue_front  + x) & QUEUE_MASK)
#define queue_back_off(x)   ((__queue_back   + x) & QUEUE_MASK)
#define buffer_front buffer_front_off(0) 
#define buffer_back  buffer_back_off (0)  
#define queue_front  queue_front_off (0)  
#define queue_back   queue_back_off  (0)   
//=====================================================================
//�������ƣ�queue_init
//�������أ� ��
//����˵���� *data, ��ȡ���ַ�
//���ܸ�Ҫ����ȡ�����е��ַ�
//����ֵ�� �Ƿ���ֵ
//=====================================================================
void queue_init(){
    __queue_front = __queue_back;
    __buffer_front = __buffer_back;
}
//=====================================================================
//�������ƣ�queue_get
//�������أ� ��
//����˵���� *data, ��ȡ���ַ���
//���ܸ�Ҫ����ȡ�����е��ַ�
//����ֵ�� ����
//=====================================================================
uint16_t queue_get(char * data){
    if(queue_front==queue_back) return 0;
    uint16_t length=__lens[__queue_front ++ & QUEUE_MASK];
    uint16_t i;
    if(data!=NULL) {
        for(i=0;i<length;++i){
            *(data+i)=__buffer[__buffer_front++ & BUFFER_MASK];
        }
    }else{
        __buffer_front+=length;
    }
    return length;
}
//=====================================================================
//�������ƣ�queue_put
//�������أ� ��
//����˵���� ������е�ֵ
//���ܸ�Ҫ�� ���ַ�������У��ú�����ɾ��֮ǰ��ֵ
//����ֵ�� �Ƿ���ֵ,0:��ֵ,1:��ֵ
//=====================================================================
void queue_put(const char * data,uint16_t length){
    if(length >= BUFFER_MASK && data==NULL) return;
    uint16_t i=0;
    __queue[queue_back]=__buffer_front;
    while( queue_back_off(1)==queue_front||
        buffer_front < buffer_back && buffer_back_off(length) >=buffer_front && buffer_back_off(length) <buffer_back||
        buffer_back <buffer_front && buffer_back_off(length) >=buffer_front||
        buffer_back <buffer_front && buffer_back_off(length) <buffer_back)
        if(!queue_get(NULL)) break;

    for(;i<length;++i){
        __buffer[ __buffer_back++ & BUFFER_MASK] = data[i];
    }
    __lens[(__queue_back++) & QUEUE_MASK]=length;

}

