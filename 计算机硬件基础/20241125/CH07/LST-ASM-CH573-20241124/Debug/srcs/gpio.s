.include "gpio.inc"

.equ NOERROR,       0x0
.equ ERROR,         0x1
.equ LEDON,         0x0
.equ LEDOFF,        0x1

#�������洢text�ο�ʼ��ʵ�ʴ���洢��Flash��
.section   .text
.align 2                          /*ָ������ݲ���2�ֽڶ���*/


gpio_port_pin_resolution:
	/* ������ջ */
	addi sp, sp, -64			/* �����ջ���*/
	sw ra, 60(sp)				/* ���Ĵ�����ַд����ջ��*/
	/* �����GPIO�˿ںź����ź� */
	srli t4,a0,0x8				/* t4=a0=�˿ں� srli�����ƶ�ָ��*/
	andi t5,a0,255				/* t5=a0=���ź� andi�ǰ�λ��*/
	mv a0,t4
	mv a1,t5
	/* �ͷ�ջ */
	lw ra, 60(sp)               /* Restore the return address �ָ����ص�ַ LW ָ�һ�� 32 λ��ֵ�Ӵ洢������Ĵ��� rd���� */
    addi sp, sp, 64             /* Deallocating the stack frame �ͷ�ջ֡ addi���ָ��*/
    mv  a0,	t4					/* load return value 0 ��ȡ����ֵ */
    mv  a1, t5
    ret							/* ���� */

/*======================================================================
// �������ƣ�gpio_init
// �������أ���
// ����˵����r0:(�˿ں�|(���ź�)),��:(PTB_NUM|(5u))��ʾB��5��,ͷ�ļ����к궨��
//           r1:���ŷ���0=����,1=���,�������ŷ���궨�壩
//           r2:�˿����ų�ʼ״̬��0=�͵�ƽ��1=�ߵ�ƽ��
// ���ܸ�Ҫ����ʼ��ָ���˿�������ΪGPIO���Ź��ܣ�������Ϊ�������������������
//           ��ָ����ʼ״̬�ǵ͵�ƽ��ߵ�ƽ
// ��    ע���˿�x��ÿ�����ſ��ƼĴ���PORTx_PCRn�ĵ�ַ=PORT_PCR_BASE+x*0x1000+n*4
//           ����:x=0~4����ӦA~E;n=0~31
//======================================================================  */
.type GPIO_init function          /*����gpio_initΪ��������*/
.global gpio_init                 /*��gpio_init�����ȫ�ֺ���������оƬ��ʼ��֮�����*/
gpio_init:
	addi sp, sp, -64			/* �����ջ���*/
	sw ra, 60(sp)				/* ���Ĵ�����ַд����ջ�� SWָ��Ĵ��� rs2���е� 32 λֵ���浽�洢����*/
	mv a3,a2					/* ��������3������a2��ֵ��a3 */
	mv a2,a1					/* ��������2������a1��ֵ��a2*/
    /* �����GPIO�˿ںź����ź� */
	srli t4,a0,0x8				/* t4=a0=�˿ں� */
	andi t5,a0,255				/* t5=a0=���ź� */
	mv a0,t4					/* a0=�˿ں� */
	mv a1,t5					/* a1=���ź� */

    /* �ж϶˿ں� */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA           /* ����a0=�˿ںţ�=��a4=0x00����ʱ����תGPIOA */
    j GPIOB                     /* ����a0=�˿ںţ�!=��a4=0x00����ʱ����תGPIOA */

/* �˿�A GPIOA */
GPIOA:
    /* �ж�GPIO�������ģʽ */
    beqz a2, GPIOA_input_H       /* ������2������a2=0������תgpio_input_H */
    li t4,1						/* t4=1 */
    beq a2,t4, GPIOA_output_H	/* ������2������a2=1������תgpio_output_H */
    li a2, ERROR
    j exit

/* �˿�B GPIOB */
GPIOB:
    /* �ж�GPIO�������ģʽ */
    beqz a2, GPIOB_input_H       /* ������2������a2=0������תgpio_input_H */
    li t4,1						/* t4=1 */
    beq a2,t4, GPIOB_output_H	/* ������2������a2=1������תgpio_output_H */
    li a2, ERROR
    j exit

/* ����GPIOA����Ϊ���ģʽ */
GPIOA_output_H:
    li t0, R32_PA_DIR           /* ��PA����Ĵ�����ֵ��t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_DIR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(GPIO_OUTPUT << pin ))��(t2=(R32_PA_DIR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�ķ��� */

    beqz a3, OUT_PUT_LOW_A      /* ��ʼ״̬����͵�ƽ */
    j OUT_PUT_HIGH_A            /* ��ʼ״̬����ߵ�ƽ */

/* ����͵�ƽ */
OUT_PUT_LOW_A:
    li t0, R32_PA_CLR           /* ��R32_PA_CLR�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_CLR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PA_CLR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�����ֵ */

    j exit

/* ����ߵ�ƽ */
OUT_PUT_HIGH_A:
    li t0, R32_PA_OUT           /* ��R32_PA_OUT�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_OUT�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PA_OUT))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�����ֵ */

    j exit

/* ����GPIOA����Ϊ����ģʽ */
GPIOA_input_H:
    li t0, R32_PA_DIR           /* ��R32_PA_DIR�Ĵ�����ֵ��t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    not t1, t1                  /* t1ȡ������t1����ӦλΪ0 */
    lw t2, 0(t0)                /* ��ȡR32_PA_DIR�Ĵ�����ֵ */
    and t2, t2, t1              /* (t1=~(GPIO_OUTPUT << pin ))��(t2=(R32_PA_DIR))ֵ��λ�룬����t2 */
    sw t2, 0(t0)                /* ����GPIOAx�ķ��� */

    j exit

/* ����GPIOB����Ϊ���ģʽ */
GPIOB_output_H:
    li t0, R32_PB_DIR           /* ��PB����Ĵ�����ֵ��t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_DIR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(GPIO_OUTPUT << pin ))��(t2=(R32_PB_DIR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�ķ��� */

    beqz a3, OUT_PUT_LOW_B      /* ��ʼ״̬����͵�ƽ */
    j OUT_PUT_HIGH_B            /* ��ʼ״̬����ߵ�ƽ */

/* ����͵�ƽ */
OUT_PUT_LOW_B:
    li t0, R32_PB_CLR           /* ��R32_PB_CLR�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_CLR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PB_CLR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�����ֵ */

    j exit

/* ����ߵ�ƽ */
OUT_PUT_HIGH_B:
    li t0, R32_PB_OUT           /* ��R32_PB_OUT�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_OUT�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PB_OUT))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�����ֵ */

    j exit

/* ����GPIOB����Ϊ����ģʽ */
GPIOB_input_H:
    li t0, R32_PB_DIR           /* ��R32_PB_DIR�Ĵ�����ֵ��t0 */
    li t1, GPIO_OUTPUT          /* t1=GPIO_OUTPUT=0x01 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    not t1, t1                  /* t1ȡ������t1����ӦλΪ0 */
    lw t2, 0(t0)                /* ��ȡR32_PB_DIR�Ĵ�����ֵ */
    and t2, t2, t1              /* (t1=~(GPIO_OUTPUT << pin ))��(t2=(R32_PB_DIR))ֵ��λ�룬����t2 */
    sw t2, 0(t0)                /* ����GPIOBx�ķ��� */
    j exit

/* �˳�ջ */
exit:
	lw ra, 60(sp)               /* Restore the return address �ָ����ص�ַ */
    addi sp, sp, 64             /* Deallocating the stack frame �ͷ�ջ֡ */
    li  a0,0					/* load return value 0 ��ȡ����ֵ */
	ret

/*=====================================================================
//�������ƣ�gpio_set
//�������أ���
// ����˵����r0:(�˿ں�|(���ź�)),��:(PTB_NUM|(5u))��ʾB��5��,ͷ�ļ����к궨��
//           r1:ϣ�����õĶ˿�����״̬��0=�͵�ƽ��1=�ߵ�ƽ��
//���ܸ�Ҫ����ָ�����ű�����ΪGPIO������Ϊ���ʱ���������趨����״̬
// ��    ע���˿�x��ÿ�����ſ��ƼĴ���PORTx_PCRn�ĵ�ַ=PORT_PCR_BASE+x*0x1000+n*4
//           ����:x=0~4����ӦA~E;n=0~31
//=====================================================================*/
.type gpio_set function         /*����gpio_setΪ�������� */
.global gpio_set                /*��gpio_set�����ȫ�ֺ���������оƬ��ʼ��֮�����*/
gpio_set:
/* ͨ������ջָ��������ջ�ռ����ڴ�žֲ������ʹ�ŵ��ú������ص�ַ��
      ��������ջ�ռ�ֳ�16�ֽڣ�   raΪ���ص�ַ�Ĵ�����ռ��4���ֽڣ���ra
      �еķ��ص�ַ����spָ���ַƫ��16���ֽڵ�λ��*/
	addi sp, sp, -64			/* ����ջ�ռ�*/
	sw ra, 60(sp)				/* �洢���ص�ַ */

	mv a2,a1					/* �������ڶ���������ֵ��a2 */
	/* �����GPIO�˿ںź����ź� */
	srli t4,a0,0x8				/* t4=a0=�˿ں� */
	andi t5,a0,255				/* a2=a0=���ź� */
	mv a0,t4					/* a0=�˿ں� */
	mv a1,t5					/* a1=���ź� */

    /* �ж϶˿ں� */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA_SET       /* ����a0=�˿ںţ�=��a4=0x00����ʱ����תGPIOA_SET */
    j GPIOB_SET                 /* ����a0=�˿ںţ�!=��a4=0x00����ʱ����תGPIOB_SET */

GPIOA_SET:
    beqz a2, GPIOA_SET_LOW      /* ��������͵�ƽ */
    j GPIOA_SET_HIGH            /* ��������ߵ�ƽ */

/* ����͵�ƽ */
GPIOA_SET_LOW:
    li t0, R32_PA_CLR           /* ��R32_PA_CLR�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_CLR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PA_CLR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�����ֵ */

    j SET_exit

/* ����ߵ�ƽ */
GPIOA_SET_HIGH:
    li t0, R32_PA_OUT           /* ��R32_PA_OUT�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_OUT�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PA_OUT))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�����ֵ */

    j SET_exit

GPIOB_SET:
    beqz a2, GPIOB_SET_LOW      /* ��������͵�ƽ */
    j GPIOB_SET_HIGH            /* ��������ߵ�ƽ */

/* ����͵�ƽ */
GPIOB_SET_LOW:
    li t0, R32_PB_CLR           /* ��R32_PB_CLR�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_CLR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PB_CLR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�����ֵ */

    j SET_exit

/* ����ߵ�ƽ */
GPIOB_SET_HIGH:
    li t0, R32_PB_OUT           /* ��R32_PB_OUT�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_OUT�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PB_OUT))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�����ֵ */

    j SET_exit

/* �˳����ͷ�ջ�ռ� */
SET_exit:
	lw ra, 60(sp)               /* �ָ����ص�ַ */
    addi sp, sp, 64             /* �ͷ�ջ�ռ� */
    ret							/* ���� */

/*======================================================================
// �������ƣ�gpio_reverse
// �������أ���
// ����˵����r0:(�˿ں�)|(���ź�),��:(PTB_NUM|(5u))��ʾB��5��,ͷ�ļ����к궨��
// ���ܸ�Ҫ����תָ������״̬
//======================================================================*/
.type gpio_reverse function     /*����gpio_reverseΪ��������  */
.global gpio_reverse            /*��gpio_reverse�����ȫ�ֺ���������оƬ��ʼ��֮����� */
gpio_reverse:
/* ͨ������ջָ��������ջ�ռ����ڴ�žֲ������ʹ�ŵ��ú������ص�ַ��
      ��������ջ�ռ�ֳ�16�ֽڣ�   raΪ���ص�ַ�Ĵ�����ռ��4���ֽڣ���ra
      �еķ��ص�ַ����spָ���ַƫ��16���ֽڵ�λ��*/
	addi sp, sp, -64			/* �����ջ���*/
	sw ra, 60(sp)				/* ���Ĵ�����ַд����ջ��*/

	/* �����GPIO�˿ںź����ź� */
	srli t4,a0,0x8				/* t4=a0=�˿ں� */
	andi t5,a0,255				/* a2=a0=���ź� */
	mv a0,t4					/* a0=�˿ں� */
	mv a1,t5					/* a1=���ź� */

    /* �ж϶˿ں� */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA_REVERSE   /* ����a0=�˿ںţ�=��a4=0x00����ʱ����תGPIOA_REVERSE */
    j GPIOB_REVERSE             /* ����a0=�˿ںţ�!=��a4=0x00����ʱ����תGPIOB_REVERSE */

GPIOA_REVERSE:
    li t0, R32_PA_OUT           /* t0=R32_PA_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */

    beqz t1, GPIOA_REV_H        /* ��ǰΪ�͵�ƽ����ת�ɸߵ�ƽ */
    j GPIOA_REV_L               /* ��ǰΪ�ߵ�ƽ����ת�ɵص�ƽ */

GPIOA_REV_L:
    li t0, R32_PA_CLR           /* ��R32_PA_CLR�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_CLR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PA_CLR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�����ֵ */

    j gpio_reverse_exit

GPIOA_REV_H:
    li t0, R32_PA_OUT           /* ��R32_PA_OUT�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PA_OUT�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PA_OUT))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOAx�����ֵ */

    j SET_exit

GPIOB_REVERSE:
    li t0, R32_PB_OUT           /* t0=R32_PB_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */

    beqz t1, GPIOB_REV_H        /* ��ǰΪ�͵�ƽ����ת�ɸߵ�ƽ */
    j GPIOB_REV_L               /* ��ǰΪ�ߵ�ƽ����ת�ɵص�ƽ */

GPIOB_REV_L:
    li t0, R32_PB_CLR           /* ��R32_PB_CLR�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_CLR�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PB_CLR))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�����ֵ */

    j gpio_reverse_exit

GPIOB_REV_H:
    li t0, R32_PB_OUT           /* ��R32_PB_OUT�Ĵ�����ֵ��t0 */
    li t1, 1                    /* t1=1 */
    sll t1, t1, a1              /* ��t1���ƣ�a1=���źţ� */
    lw t2, 0(t0)                /* ��ȡR32_PB_OUT�Ĵ�����ֵ */
    or t2, t2, t1               /* (t1=(1 << pin ))��(t2=(R32_PB_OUT))ֵ��λ�򣬴���t2 */
    sw t2, 0(t0)                /* ����GPIOBx�����ֵ */

    j gpio_reverse_exit

gpio_reverse_exit:
	lw ra, 60(sp)               /* �ָ����ص�ַ */
    addi sp, sp, 64             /* �ͷ�ջ�ռ� */
    ret		

/*======================================================================
// �������ƣ�gpio_get
// �������أ�r2:ָ���˿����ŵ�״̬��1��0��
// ����˵����r0:(�˿ں�)|(���ź�),��:(PTB_NUM|(5u))��ʾB��5��,ͷ�ļ����к궨��
// ���ܸ�Ҫ����ָ���˿����ű�����ΪGPIO������Ϊ����ʱ����������ȡָ������״̬
//======================================================================*/
.type gpio_get function         /*����gpio_getΪ��������*/
.global gpio_get                /*��gpio_get�����ȫ�ֺ���������оƬ��ʼ��֮�����*/
gpio_get:
/* ͨ������ջָ��������ջ�ռ����ڴ�žֲ������ʹ�ŵ��ú������ص�ַ��
      ��������ջ�ռ�ֳ�16�ֽڣ�   raΪ���ص�ַ�Ĵ�����ռ��4���ֽڣ���ra
      �еķ��ص�ַ����spָ���ַƫ��16���ֽڵ�λ��*/
	addi sp, sp, -64			/* �����ջ���*/
	sw ra, 60(sp)				/* ���Ĵ�����ַд����ջ��*/

	/* �����GPIO�˿ںź����ź� */
	srli t4,a0,0x8				/* t4=a0=�˿ں� */
	andi t5,a0,255				/* t5=a0=���ź� */
	mv a2,t4					/* a2=�˿ں� */
	mv a3,t5					/* a3=���ź� */

    /* �ж϶˿ں� */
    li a4, 0x00                 /* a4=0x00 */
    beq a0, a4, GPIOA_GET       /* ����a0=�˿ںţ�=��a4=0x00����ʱ����תGPIOA_GET */
    j GPIOB_GET                 /* ����a0=�˿ںţ�!=��a4=0x00����ʱ����תGPIOB_GET */

GPIOA_GET:
    li t0, R32_PA_DIR           /* t0=R32_PA_DIR */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */

    beqz t1, GPIOA_GET_IN       /* GPIOAx���� */
    j GPIOA_GET_OUT             /* GPIOAx��� */

GPIOA_GET_IN:
    li t0, R32_PA_PIN           /* t0=R32_PA_PIN */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */

    beqz t1, RETURN_0           /* GPIOAx���룬״̬Ϊ�͵�ƽ */
    j RETURN_1                  /* GPIOAx���룬״̬Ϊ�ߵ�ƽ */

GPIOA_GET_OUT:
    li t0, R32_PA_OUT           /* t0=R32_PA_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */

    beqz t1, RETURN_0           /* GPIOAx���룬״̬Ϊ�͵�ƽ */
    j RETURN_1                  /* GPIOAx���룬״̬Ϊ�ߵ�ƽ */

GPIOB_GET:
    li t0, R32_PB_DIR           /* t0=R32_PB_DIR */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */

    beqz t1, GPIOB_GET_IN       /* GPIOBx���� */
    j GPIOB_GET_OUT             /* GPIOBx��� */

GPIOB_GET_IN:
    li t0, R32_PB_PIN           /* t0=R32_PB_PIN */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */

    beqz t1, RETURN_0           /* GPIOAx���룬״̬Ϊ�͵�ƽ */
    j RETURN_1                  /* GPIOAx���룬״̬Ϊ�ߵ�ƽ */

GPIOB_GET_OUT:
    li t0, R32_PB_OUT           /* t0=R32_PB_OUT */
    lw t2, 0(t0)
    li t1, 1                    /* t1=1 */
    sll t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */
    and t1, t1, t2              /* t1=t1&t2 */
    srl t1, t1, a3              /* ��t1���ƣ�a3=���źţ� */

    beqz t1, RETURN_0           /* GPIOAx���룬״̬Ϊ�͵�ƽ */
    j RETURN_1                  /* GPIOAx���룬״̬Ϊ�ߵ�ƽ */


RETURN_0:
    lw ra, 64(sp)               /* �ָ����ص�ַ */
    addi sp, sp, 64             /* �ͷ�ջ�ռ�*/
    li  a0,0					/* ����ֵ0 */
    ret							/* ���� */

RETURN_1:
	lw ra, 60(sp)               /* �ָ����ص�ַ */
    addi sp, sp, 64             /* �ͷ�ջ�ռ� */
    li  a0,1					/* ����ֵ1 */
    ret	
