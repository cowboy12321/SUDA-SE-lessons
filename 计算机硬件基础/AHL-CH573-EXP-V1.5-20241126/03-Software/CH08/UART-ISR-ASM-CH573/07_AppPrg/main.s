/*=====================================================================
//�ļ����ƣ�main.s
//���ܸ�Ҫ��������Ա�̣����ù���GPIO��ʵ��������˸
//��Ȩ���У��մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20220102-20240301
//=====================================================================*/

.include "includes.inc"    /* ���������ͷ�ļ� */

/* �������ݶΣ���data�Σ�RAM���򣬱���ʹ�õ�����=================== */
.section .data       /* �������ݶ� */
hello_information:   /* �����ַ�������ż�Ϊ�ַ����׵�ַ��\0Ϊ������־  */
    .ascii "\n"
    .ascii "-----------------------------------------------------------------\n"
    .ascii "����«��ʾ��                                                   \n"
    .ascii "���������ơ�RISC-V��๤�̿�ܣ�GPIO��UART��������������          \n"
    .ascii "�������ܡ������������GPIO�����������������Ӵ����ж�             \n"
    .ascii "��Ӳ�����ӡ���������05_UserBoard�ļ�����user.inc�ļ�              \n"
    .ascii "������˵����ͨ����IDE�˵��������ߡ��������ڹ��ߡ���                \n"
    .ascii "                   ���û����� ������MCU�������ֽ�               \n"
    .ascii "----------------------------------------------------------------\n\0"
printf_format1:           /* ����һ��printfʹ�õ����ݸ�ʽ���Ʒ� */
    .ascii "��˸����mLightCount =\0"
printf_format2:           /* ����һ��printfʹ�õ����ݸ�ʽ���Ʒ� */
    .ascii "%d\n\0"       /* һ��������ʮ���� */
printf_format3:    
     .ascii "LIGHT_BLUE:ON--\n\0"
printf_format4:    
     .ascii "LIGHT_BLUE:OFF--\n\n\0"  
     
.align 4                  /* ���� */
mMainLoopCount:           /* ��ѭ��������������ѭ����ʱ������m��Ϊǰ׺��*/
    .word 0               /* ��ֵΪ0 */
.align 2    
mLigtCount:               /* �Ƶ�״̬�л�����*/
    .half 0  
.align 1      
mFlag:                    /* �Ƶ�״̬��־ */
    .byte 'A'             /* ��ֵΪ'A'������ */

/* �������Σ���Falsh���򣬴�Ŵ��롢����=========================== */
.section  .text           /* �������� */
.type main function       /* ���������main���Ϊ��������  */
.global main              /* ���������main���Ϊȫ�ֺ��������ڳ�ʼ������ */
.align 2                  /* ָ������ݲ���2�ֽڶ��룬����16λָ� */

/*��������һ������¿�����Ϊ����Ӵ˿�ʼ���У�ʵ�������������̣�------ */
main:
    /*��1���������֣���ͷ��========================================== */
    /*��1.1������ջ�ռ估���������ʾ */
    addi sp,sp,-48              /* ����ջ�ռ� */
                                /* ͨ�����Դ��������ʾ */
    la a0,hello_information     /* ����a0�����ں� */
    call printf                 /* ���ú��� */ 
    /*��1.2���û�����ģ���ʼ�� */
                                /* ��ʼ��GPIO�����ƣ����������� */
    li a0,LIGHT_BLUE            /* ��1����a0���Ƶ����� */ 
    li a1,GPIO_OUTPUT           /* ��2����a1������Ϊ��� */
    li a2,LIGHT_OFF             /* ��3����a2����ʼ״̬ */
    call gpio_init              /* ���ú��� */
                                /* ��ʼ������UART_User */
    li a0,UART_User             /* ��1����a0�����ں� */
    li a1,115200                /* ��2����a1�������� */
    call uart_init              /* ���ú��� */
    /*��1.3��ʹ��ģ���ж� */
                                /* ʹ��UART_User���ڽ����ж� */
    li a0,UART_User             /* ֻ��һ������a0�����ں� */
    call uart_enable_re_int
    /* ��1���������֣���β��======================================== */
	
/*��2����ѭ�����֣���ͷ��=========================================== */
main_loop: 
    /*��2.1����ѭ����������mMainLoopCount+1���ж��Ƿ��趨ֵ */
                                /* mMainLoopCount+1 */
       la a5,mMainLoopCount     /* a5������mMainLoopCount�ĵ�ַ */
       lw t1,0(a5)              /* t1������mMainLoopCount��ֵ */
       addi t1,t1,1             /* t1��t1+1 */
       sw t1,0(a5)              /* �Ż�RAM�� */
                                /* �ж��Ƿ��趨ֵ */
       li t2,MAINLOOP_COUNT     /* ������user.inc�к궨�� */
       bltu t1,t2,main_loop     /* С��ת,��t1<t2ת */
    /*��2.2���ﵽ��ѭ�������趨ֵ��ִ��������䣬���еƵ��������� */
                                /* �����ѭ����������mMainLoopCount */
       la a5, mMainLoopCount     
       li t1,0
       sw t1,0(a5)
                                /* �ж���״̬��־mFlag�Ƿ�Ϊ'L'������ */
       la a5, mFlag
       lb t1,0(a5)              /* ע��mFlagΪ�ֽ��ͱ��� */
       li t2,'L'
       bne t1,t2,main_Light_OFF /* t1��t2ת */
                                /* ��״̬��־mFlag='L'��� */
                                /* ����˸����+1 */
       la a5,mLigtCount         /* a5������mLigtCount�ĵ�ַ */           
       lh t1,0(a5)              /* t1������mLigtCountֵ�����֣�*/
       addi t1,t1,1                
       sh t1,0(a5)              /* ��أ����֣� */
                                /* ͨ�����Դ��������ʾ ��˸����mLightCount= */
       la a0,printf_format1          
       call printf 
                                /* ͨ�����Դ��������ʾ ��˸��������ֵ */ 
       la a0,printf_format2     /* ��1����a0����ʽ�ĵ�ַ */
       la a5,mLigtCount               
       lh a1,0(a5)              /* ��2����a1������mLigtCount��ֵ */
       call printf              /* ���ú��� */
                                /* ������ */
       li a0,LIGHT_BLUE         /* ��1����a0���Ƶ����� */
       li a1,LIGHT_ON           /* ��2����a1���Ƶ�״̬ */
       call gpio_set            
                                /* ͨ�����Դ��������ʾ �Ƶ�״̬ */
       la a0,printf_format3           
       call printf
                                /* �Ƶ�״̬��־��Ϊ'A'������ */
       la a1,mFlag             
       li t1,'A'
       sb t1,0(a1)
       j main_exit
main_Light_OFF:   /* ��״̬��־mFlag��'L'�������ʾ�����ư����ı��־ */
       la a0,printf_format4           /* ����Ƶ�״̬ */
       call printf 
                                      /* ���ư�*/
       li a0,LIGHT_BLUE               /* ��1����a0 �� LIGHT_BLUE */
       li a1,LIGHT_OFF                /* ��2����a1 �� LIGHT_OFF */
       call gpio_set                  
                                      /* �Ƶ�״̬��־��Ϊ'L'������ */
       la a1,mFlag                    
       li t1,'L'
       sb t1,0(a1)
main_exit:
    j main_loop                       /* ����ѭ�� */
/*��2����ѭ�����֣���β��=========================================== */
    /* ��ջ�ָ��ֳ�����ʹ���в������ÿһ��������ջ�ָ��� */
    addi sp, sp, 48                   /* �ͷ�ջ֡ */
    ret                               /* ���� */
