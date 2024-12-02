//======================================================================
// �ļ����ƣ�main.c��Ӧ�ù�����������
// ����ṩ���մ�Ƕ��ʽ��sumcu.suda.edu.cn��
// �汾���£�20240828
// �����������������̵�..\01_Doc\Readme.txt
// ��ֲ���򣺡��̶���
//======================================================================
#define GLOBLE_VAR
#include "includes.h" //������ͷ�ļ�

//----------------------------------------------------------------------
// ����ʹ�õ����ڲ�����
// main.cʹ�õ��ڲ�����������
void Delay_ms(uint16_t u16ms);

//----------------------------------------------------------------------
// ��������һ������¿�����Ϊ����Ӵ˿�ʼ���У�ʵ�������������̣��μ���壩
int main(void)
{
    // ��1��======�������֣���ͷ��==========================================
    // ��1.1������main����ʹ�õľֲ�����
    uint32_t mCount;      // ��ʱ�Ĵ���
    uint32_t i;           // ѭ������
    uint16_t mcu_temp_AD; // оƬ�¶�ADֵ
    float mcu_temp;       // оƬ�¶Ȼع�ֵ
    float preTemp;        // ��һ���¶ȴ������¶�
    // ��1.2�������䡿�����ж�
    DISABLE_INTERRUPTS;

    // ��1.3����������ʹ�õľֲ���������ֵ
    mCount = 0; // �Ǵ���
    preTemp = 0;

    // ��1.4����ȫ�ֱ�������ֵ

    // ��1.5���û�����ģ���ʼ��
    gpio_init(LIGHT_BLUE, GPIO_OUTPUT, LIGHT_OFF);  // ��ʼ������
    gpio_init(LIGHT_GREEN, GPIO_OUTPUT, LIGHT_OFF); // ��ʼ���̵�
    gpio_init(LIGHT_RED, GPIO_OUTPUT, LIGHT_OFF);   // ��ʼ�����
    adc_init(TEMPSENSOR_CHANNEL, 0);                // ��ʼ��ADоƬ�¶�ģ��
    uart_init(UART_User, 115200);                   // ��ʼ������ģ��
    Delay_ms(100);                                  // ��ʱ100ms��ֹADC��δ���������ɵ�һ�ζ�������
    // ��1.6��ʹ��ģ���ж�
    uart_enable_re_int(UART_User);

    // ��1.7�������䡿�����ж�
    ENABLE_INTERRUPTS;

    // ��1��======�������֣���β��==========================================

    // ��2��======��ѭ�����֣���ͷ��========================================
    for (;;) // for(;;)����ͷ��
    {
        // ��2.1����ʱ1��
        Delay_ms(1000);

        // ��2.2����ȡоƬ �¶�
        printf("------------------------\r\n\n");
        mcu_temp_AD = adc_ave(TEMPSENSOR_CHANNEL,10);
        mcu_temp = adc_mcu_temp(mcu_temp_AD); // ��оƬ�¶�ADֵת����ʵ���¶�
        printf("mcu_temp_AD = %d\n", mcu_temp_AD);
        printf("оƬ�ڲ��¶�Ϊ:%6.2lf��\r\n", mcu_temp);

        if (preTemp > 0 && (mcu_temp > preTemp) && (mcu_temp - preTemp >= 1.0))
        {
            preTemp = mcu_temp; // ��¼���ϴε��¶�
            printf("����оƬ����������\n");
            // ֻ�е���ԭ������������²Ž�������
            // ��֤�������
            gpio_set(LIGHT_BLUE,LIGHT_OFF);
            gpio_set(LIGHT_GREEN,LIGHT_OFF);
            gpio_set(LIGHT_RED,LIGHT_OFF);
            // ��˸�Ƶ�
            printf("ָʾ����ɫΪ����ɫ��");
            for (i = 0; i < 3; i++)
            {
                gpio_reverse(LIGHT_RED);
                gpio_reverse(LIGHT_GREEN);
                Delay_ms(1000);
                gpio_reverse(LIGHT_RED);
                gpio_reverse(LIGHT_GREEN);
                Delay_ms(1000);
            }
            // ���ƻָ�֮ǰ��״̬
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
            preTemp = mcu_temp; // ��¼���ϴε��¶�
        }

        mCount++;
        // ������40��ʱ�����¿�ʼ����
        // ����һֱ�ۼ�
        if (mCount >= 40)
        {
            mCount = 0;
            printf("ָʾ����ɫΪ����ɫ��");
        }
        // ��2.3�����״̬��־mFlagΪ'L'���Ƶ���˸����+1����ʾ���ı��״̬����־
        if (mCount % 5 == 0)
        {
            gpio_reverse(LIGHT_RED);
            printf(" LIGHT_RED:reverse--\n"); // ��������Ƶ�״̬
            if (mCount / 5 == 1)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
            else if (mCount / 5 == 3)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
            else if (mCount / 5 == 5)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
            else if (mCount / 5 == 7)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
        }
        if (mCount % 10 == 0) // �жϵƵ�״̬��־
        {
            gpio_reverse(LIGHT_GREEN);
            printf(" LIGHT_GREEN:reverse--\n"); // ��������Ƶ�״̬
            if (mCount / 10 == 1)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
            else if (mCount / 10 == 3)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
        }
        if (mCount % 20 == 0) // �жϵƵ�״̬��־
        {
            gpio_reverse(LIGHT_BLUE);
            printf(" LIGHT_BLUE:reverse--\n"); // ��������Ƶ�״̬
            if (mCount / 20 == 1)
            {
                printf("ָʾ����ɫΪ����ɫ��");
            }
        }
    } // for(;;)��β
    // ��2��======��ѭ�����֣���β��========================================
} // main��������β��

//======����Ϊ���������õ��Ӻ���===========================================
//======================================================================
// �������ƣ�Delay_ms
// �������أ���
// ����˵�������ƺ���
// ���ܸ�Ҫ����ʱ - ���뼶
//======================================================================
void Delay_ms(uint16_t u16ms)
{
    for (volatile uint32_t i = 0; i < SYS_FREQ / 72000 * u16ms; i++)
        __asm("NOP");
}

//========================================================================
/*
֪ʶҪ�أ�
��1��main.c��һ��ģ�壬���ļ����д�������漰�����Ӳ���ͻ�����ͨ�����ù���
ʵ�ֶ�Ӳ���ĸ�Ԥ��
��2�����ļ��жԺ�GLOBLE_VAR�����˶��壬�����ڰ���"includes.h"ͷ�ļ�ʱ���ᶨ
��ȫ�ֱ������������ļ��а���"includes.h"ͷ�ļ�ʱ��
����ʱ���Զ�����extern
*/
