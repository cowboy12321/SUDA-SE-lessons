//=====================================================================
//�ļ����ƣ�isr.c���жϴ������Դ�ļ���
//����ṩ�����ݴ�ѧǶ��ʽϵͳ���������о�����sumcu.suda.edu.cn��
//�汾���£�20170801-20220524
//�����������ṩ�жϴ�������̿��
//��ֲ���򣺡��̶���
//=====================================================================
#include "includes.h"

//���ļ��ڲ�����������--------------------------------------------------
uint16_t emuart_frame(uint8_t uartNo, uint8_t ch, uint8_t *data);

//======================================================================
//�жϷ����������ƣ�UART_User_Handler
//����������UART_User�����յ�һ���ֽڴ���
//�������ܣ������յ�һ���ֽں󣬽��뱾�������У��������ڲ�������֡����
//          CreateFrame������֡��ɣ�������Ϣ����
//˵    ����ʹ��ȫ�ֱ���
//======================================================================

void UART_User_Handler(void)
{
    //�ֲ�����
	uint8_t ch;
	uint8_t flag;
	static uint8_t recv_dateframe[11];  //���ڽ����ַ�����
    static uint8_t recv_data[64];
    static vuint16_t gcRecvLen=0;

	DISABLE_INTERRUPTS;      //�����ж�
	//-------------------------------
    //����һ���ֽ�
	ch = uart_re1(UART_User,&flag);
	if(flag)
	{
        // ��1����ȡ��֡
        //  0��δ��֡�ɹ�����0����֡�ɹ����ҷ���ֵΪ���յ������ݳ���
        if (gcRecvLen == 0){
            gcRecvLen = emuart_frame(UART_User, ch, (uint8_t *)recv_data);
        }
        if(gcRecvLen){
            queue_put(recv_data+2,gcRecvLen);
            gcRecvLen=0;
        }
	}
	//-------------------------------
	ENABLE_INTERRUPTS;       //�����ж�
}

//�ڲ����ú���



//===========================================================================
// �������ƣ�emuart_frame
// ���ܸ�Ҫ����֡��������֡�����ݼ���֡�С�������Ӧ���������жϴ�������С�
// ����˵����ch�����ڽ��յ�һ���ֽ����ݣ�����data����Ž��յ������ݡ�
// �������أ�0��δ��֡�ɹ�����0����֡�ɹ����ҷ���ֵΪ���յ������ݳ���
// ��ע��ʮ����������֡��ʽ
//      ֡ͷ       + ���ݳ���         + ��Ч����   +CRCУ���� +  ֡β
//      FrameHead(1�ֽ�)
//      len(1�ֽ�)
//      ��Ч����(N�ֽ� N=data[1])
//      CRCУ��(2�ֽ�)
//      FrameTail(1�ֽ�)
//�������ݷ���data+2��ͷ��������
//===========================================================================
uint16_t emuart_frame(uint8_t uartNo, uint8_t ch, uint8_t *data)
{
    static uint16_t index = 0;
    static uint16_t length = 0;
    uint16_t ret_val;
    uint16_t i;
    uint16_t crc;
    // ��1����δ���յ����ݻ���δ����֡ͷ�����˳�������0  ֡ͷ����Ϊ1���ֽ�
    if (index == 0 && ch != FrameHead)
    {
        goto uecom_recv_err;
    }
    if (ch == FrameHead) index = 0;
    // ��2����ȷ���յ���֡ͷ������ִ�У������ݴ���data����
    data[(index++)] = ch;
    // ��3������ȡ�����ĸ�����ʱ�������Ч���ݳ���

    if (index == 2) length = data[1];
    // printf("index:%d\n",index);
    // ��4��������յ������ݴﵽһ֡���ȣ�֡����=��Ч���ݳ���+8��������CRCУ�飬���У��ͨ�����������ݵĴ���
    if (length != 0 && index > length + 4)
    {
        // ��4.1������Ч���ݽ���CRCУ��
        crc = crc16((data + 2), length);
        // ��4.2����δ��ȡ��֡β��CRCУ������������0
        if (data[index-1] != FrameTail || (crc >> 8) != data[index - 3] || (crc & 0xff) != data[index - 2]) 
        {
#ifdef __debug 
            printf("����CRCУ��� length:%d, crc: %#04x, data:%#02x%02x\n",length,crc,data[index-3],data[index-2]);
#endif
            goto uecom_recv_err;
        }

        ret_val = length; // ������Ч���ݳ���
        index=length = 0;             // ����Ч���ݳ���Ϊ0
        goto uecom_recv_exit;
    }
    ret_val = 0;
    goto uecom_recv_exit;
uecom_recv_exit:

    return ret_val;

uecom_recv_err:
    index = 0;
    length = 0;
    return 0;
}



/*
 ֪ʶҪ�أ�
 1.���ļ��е��жϴ��������õľ�������豸��װ�õľ��幹�����ڸ���оƬ
 ʱ��ֻ�豣֤�豸�Ĺ����ӿ�һ�£����ɱ�֤���ļ�������жϴ����������κ�
 ���ģ��Ӷ��ﵽоƬ�޹��Ե�Ҫ��
 */