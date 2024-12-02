//=====================================================================
//�ļ����ƣ�pwm.h
//���ܸ�Ҫ��PWM�ײ���������ͷ�ļ�
//������λ���մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20240105
//оƬ���ͣ�MSPM0L1306
//=====================================================================

#ifndef PWM_H
#define PWM_H

//����оƬͷ�ļ�
#include "CH573SFR.h"
#include "gpio.h"

//�궨��

//PWM���뷽ʽ�궨��:���ض��롢���Ķ���
#define PWM_EDGE   0
#define PWM_CENTER 1
//PWM����ѡ��궨�壺�����ԡ�������
#define PWM_PLUS   0
#define PWM_MINUS  1
//PWMͨ����
#define CH_PWM4     0x01  // PWM4 ͨ��
#define CH_PWM5     0x02  // PWM5 ͨ��
#define CH_PWM6     0x04  // PWM6 ͨ��
#define CH_PWM7     0x08  // PWM7 ͨ��
#define CH_PWM8     0x10  // PWM8 ͨ��
#define CH_PWM9     0x20  // PWM9 ͨ��
#define CH_PWM10    0x40  // PWM10 ͨ��
#define CH_PWM11    0x80  // PWM11 ͨ��
#define  PWM_PIN0  CH_PWM4     //PTA12
#define  PWM_PIN1  CH_PWM5     //PTA13
#define  PWM_PIN2  CH_PWM6     //PB0
#define  PWM_PIN3  CH_PWM7     //PB4
#define  PWM_PIN4  CH_PWM8     //PB6
#define  PWM_PIN5  CH_PWM9     //PB7
#define  PWM_PIN6  CH_PWM10    //PB14
#define  PWM_PIN7  CH_PWM11    //PB23

#define  PWMX_Cycle_256 256 // 256 ��PWMX����
#define  PWMX_Cycle_255 255 // 255 ��PWMX����
#define  PWMX_Cycle_128 128 // 128 ��PWMX����
#define  PWMX_Cycle_127 127 // 127 ��PWMX���� 
#define  PWMX_Cycle_64  64  // 64 ��PWMX����
#define  PWMX_Cycle_63  63  // 63 ��PWMX����
#define  PWMX_Cycle_32  32  // 32 ��PWMX����
#define  PWMX_Cycle_31  31  // 31 ��PWMX����

//=====================================================================
// �������ƣ�pwm_init
// ���ܸ�Ҫ��pwm��ʼ������
// ����˵����pwmNo��pwmģ��ţ���.h�ĺ궨�����
//          clockFre��ʱ��Ƶ�ʣ���λ��hz ��ѡ��62745-16M
//          period�����ڣ���λΪ������������������������������.h�ļ���
//          duty��ռ�ձȣ�0.0~100.0��Ӧ0%~100%
//          align�����뷽ʽ ����������
//          pol�����ԣ���ͷ�ļ��궨���������PWM_PLUSΪ������
// �������أ�  ��
//ʹ��˵������1��ʱ��Ƶ�ʺ�����������pwm���õģ�ֻ��ռ�ձȺͼ�����ÿ��ͨ������
//              ���õ�
//=====================================================================
void pwm_init(uint16_t pwmNo,uint32_t clockFre,uint16_t period,
               double  duty,uint8_t align, uint8_t pol);

//=====================================================================
// �������ƣ�pwm_update
// ���ܸ�Ҫ��PWM����
// ����˵����pwmNo��pwmģ��ţ���.h�ĺ궨����� 
//          duty��ռ�ձȣ�0.0~100.0��Ӧ0%~100%
// �������أ���
//=====================================================================
void pwm_update(uint16_t pwmNo,double duty);

//=====================================================================
// �������ƣ�pwm_stop
// ���ܸ�Ҫ��PWMֹͣ
// ����˵����pwmNo��pwmģ��ţ���.h�ĺ궨����� 
// �������أ���
//=====================================================================
void pwm_stop(uint16_t pwmNo);
#endif