/*=====================================================================
//�ļ����ƣ�main.s
//���ܸ�Ҫ��������Ա�̣����ù���GPIO��ʵ��������˸
//��Ȩ���У��մ�Ƕ��ʽ(sumcu.suda.edu.cn)
//�汾���£�20220102-20240208
//=====================================================================*/

.include "includes.inc"    /* ���������ͷ�ļ� */

/* �������ݶΣ���data�Σ�RAM���򣬱���ʹ�õ�����=================== */
.section .data       /* �������ݶ� */
hello_information:   /* �����ַ�������ż�Ϊ�ַ����׵�ַ��\0Ϊ������־  */
    .ascii "\n"
    .ascii "-----------------------------------------------------------------\n"
    .ascii "����«��ʾ��                                                   \n"
    .ascii "���������ơ�RISC-V��๤�̿�ܣ�GPIO��������������                \n"
    .ascii "�������ܡ�����GPIO��๹������������˸                          \n"
    .ascii "��Ӳ�����ӡ���������05_UserBoard�ļ�����user.inc�ļ�              \n"
    .ascii "������˵�����۲�Ӳ�����ϵ�����                                    \n"
    .ascii "----------------------------------------------------------------\n\0"
printf_format1:           /* ����һ��printfʹ�õ����ݸ�ʽ���Ʒ� */
    .ascii "��˸����mLightCount =\0"
printf_format2:           /* ����һ��printfʹ�õ����ݸ�ʽ���Ʒ� */
    .ascii "%d\n\0"
printf_format3:    
     .ascii "LIGHT_BLUE:ON--\n\0"
printf_format4:    
     .ascii "LIGHT_BLUE:OFF--\n\n\0"    
.align 4                  /* ���ֽڶ��� */
mMainLoopCount:           /* ��ѭ��������������ѭ����ʱ������m��Ϊǰ׺��*/
    .word 0               /* ��ֵΪ0 */
mLigtCount:               /* �Ƶ�״̬�л�����*/
    .word 0               /* ��ֵΪ0 */  
mFlag:                    /* �Ƶ�״̬��־ */
    .byte 'A'             /* ��ֵΪ'A'������ */

/* �������Σ���Falsh���򣬴�Ŵ��롢����=========================== */
.section  .text        /* �������� */
.type main function    /* ���������main���Ϊ��������  */
.global main           /* ���������main���Ϊȫ�ֺ��������ڳ�ʼ������ */
.align 2               /* ָ������ݲ���2�ֽڶ��룬����16λָ� */

/*��������һ������¿�����Ϊ����Ӵ˿�ʼ���У�ʵ�������������̣�------ */
main:
    /*��1���������֣���ͷ��========================================== */
    /*��1.1������ջ�ռ� */
    addi sp,sp,-48           /* ����ջ�ռ� */
    /*��1.2�������䡿�����ж� */
	LI t0, 0x8
	CSRC mstatus, t0
	
    la a0,hello_information  /* ͨ�����Դ��������ʾ */
    call printf
    /*��1.3���û�����ģ���ʼ�� */
    /* ��ʼ��GPIO�����ƣ����������� */
    li a0,LIGHT_BLUE             /* �Ƶ����� */ 
    li a1,GPIO_OUTPUT            /* ��� */
    li a2,LIGHT_OFF              /* ��ʼ״̬ */
    call gpio_init               /* ���ú��� */
    /*��1.4�������ж� */
	LI t0, 0x8
	CSRS mstatus, t0
    /* ��1���������֣���β��======================================== */
	
/*��2����ѭ�����֣���ͷ��=========================================== */
main_loop: 
    /*��2.1����ѭ����������mMainLoopCount+1���ж��Ƿ��趨ֵ */
       /* mMainLoopCount+1 */
       la a5,mMainLoopCount    /* a5������mMainLoopCount�ĵ�ַ */
       lw t1,0(a5)             /* t1������mMainLoopCount��ֵ */
       addi t1,t1,1            /* t1��t1+1 */
       sw t1,0(a5)             /* �Ż�RAM�� */
       /* �ж��Ƿ��趨ֵ */
       li t2,MAINLOOP_COUNT    /* ������user.inc�к궨�� */
       bltu t1,t2,main_loop    /* С��ת,��t1<t2ת */
    /*��2.2���ﵽ��ѭ�������趨ֵ��ִ��������䣬���еƵ��������� */
       /* �����ѭ���������� */
       la a5, mMainLoopCount    
       li t1,0
       sw t1,0(a5)
       /* �ж���״̬��־mFlag�Ƿ�Ϊ'L' */
       la a5, mFlag
       lb t1,0(a5)
       li t2,'L'
       bne t1,t2,main_Light_OFF       /* t1��t2ת */
       /* ��״̬��־mFlagΪ'L' */
       la a5,mLigtCount               /* ����˸����+1 */     
       lw t1,0(a5)                
       addi t1,t1,1                
       sw t1,0(a5)   
       la a0,printf_format1           /* �����ʾ ��˸����mLightCount= */
       call printf 
       /* ͨ�����Դ��������ʾ ��˸��������ֵ��a0,a1Ϊ��ڲ��� */ 
       la a0,printf_format2           /* a0��data_format�ĵ�ַ */
       la a5,mLigtCount               /* a5������mLigtCount�ĵ�ַ */
       lw a1,0(a5)                    /* a1������mLigtCount��ֵ */
       call printf                    /* ���ú��� */
       /* ������ */
       li a0,LIGHT_BLUE               /* ��1����a0 �� LIGHT_BLUE */
       li a1,LIGHT_ON                 /* ��2����a1 �� LIGHT_ON */
       call gpio_set                  /* ���ú��� */ 
       la a0,printf_format3           /* ����Ƶ�״̬ */
       call printf
       la a1,mFlag                    /* �Ƶ�״̬��־��Ϊ'A'������ */
       li t1,'A'
       sw t1,0(a1)
       j main_exit
main_Light_OFF:   /* ��״̬��־mFlag��Ϊ'L'����Ϊ'A' */
       /* ���ư�*/
       li a0,LIGHT_BLUE               /* ��1����a0 �� LIGHT_BLUE */
       li a1,LIGHT_OFF                /* ��2����a1 �� LIGHT_OFF */
       call gpio_set                  /* ���ú��� */ 
       la a0,printf_format4           /* ����Ƶ�״̬ */
       call printf 
       la a1,mFlag                    /* �Ƶ�״̬��־��Ϊ'L'������ */
       li t1,'L'
       sw t1,0(a1)
main_exit:
    j main_loop                       /* ����ѭ�� */
/*��2����ѭ�����֣���β��=========================================== */
    /* ��ջ�ָ��ֳ�����ʹ���в������ÿһ��������ջ�ָ��� */
    addi sp, sp, 48             /* �ͷ�ջ֡ */
    ret                         /* ���� */
    