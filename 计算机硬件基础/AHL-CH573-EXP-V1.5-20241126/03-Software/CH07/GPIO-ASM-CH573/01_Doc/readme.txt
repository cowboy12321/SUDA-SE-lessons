
    .ascii "\n"
    .ascii "-----------------------------------------------------------------\n"
    .ascii "����«��ʾ��                                                   \n"
    .ascii "���������ơ�RISC-V��๤�̿�ܣ�GPIO��������������                \n"
    .ascii "�������ܡ�����GPIO��๹������������˸                          \n"
    .ascii "��Ӳ�����ӡ���������05_UserBoard�ļ�����user.inc�ļ�              \n"
    .ascii "������˵�����۲�Ӳ�����ϵ�����                                    \n"
    .ascii "----------------------------------------------------------------\n\0"

1��Flash
512K �ֽڷ���ʧ�洢 FlashROM�� ��Flash������С4KB��
��1��448KB �û�Ӧ�ó���洢�� CodeFlash��0x00000000-0x0006FFFF��
��2��32KB �û�����ʧ���ݴ洢�� DataFlash��0x00070000-0x00077FFF��
��3��24KB ϵͳ��������洢�� BootLoader��0x00078000-0x0007DFFF��
��4��8KB ϵͳ����ʧ������Ϣ�洢�� InfoFlash��0x0007E000-0x0007FFFF��
2��RAM
18K �ֽ���ʧ���ݴ洢 SRAM����0x20003800-0x20007FFF��
��1��16KB ˫��Դ�����˯�߱��ִ洢�� RAM16K
��2��2KB ˫��Դ�����˯�߱��ִ洢�� RAM2K
3��������Ϣ
оƬ�ṩ��2��GPIO�˿�PA��PB����22��ͨ������������ţ�ÿ�����Ŷ������жϺͻ��ѹ��ܣ��������ž��и��ü�ӳ�书�ܡ�
PA �˿��У���PA[4]��PA[5]��PA[8]��PA[15]λ��Ч����ӦоƬ��10��GPIO���ţ�PB �˿��У���PB[0]��PB[4]��PB[6]��
PB[7]��PB[10]��PB[15]��PB[22]��PB[23]λ��Ч����ӦоƬ��12��GPIO���š�

4��WCHISPTool����
http://www.wch.cn/downloads/WCHISPTool_Setup_exe.html 


��20240202��
    1.CH573û�����ж���Ҫcpu.h�ڶ�Ӧ���ÿ�ֵ
    2.����BIOS�����Flash�����Ĵ�С�ͱ�����������Զ�Ӧ�޸������ļ���flash������mcu.h��gec.h�ڵ�������Ϣ
    3.��ΪҪ��������07�ļ��п���ֲ������ԭ�ȵ�isr.c���жϺ�������Ӳ��ѹջ���趨����ε�����ǲ�����ģ�����ͨ����Ӧ�޸�user.h�ڵĴ����жϺ궨��

��20240203���޸�Ϊ�����������
    1.���������˻���.sԴ�����ļ���.incͷ�ļ�����Ҫ��makefile�����޸ģ���֤����ʱ��Ѱַ��ͷ�ļ�
    ������.s�ļ��Ĵ���
        $(D_OBJ)/%.o:$(SRC_S)/%.s
        @echo 'Building file: $<'
        @echo 'Invoking: GNU RISC-V Cross Assembler'
        riscv-none-embed-gcc -march=rv32imac -mabi=ilp32 -msmall-data-limit=8 -mno-save-restore -$(opt)\
    -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -Wunused -Wuninitialized -g\
    -x assembler-with-cpp $(include) -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -c -o "$@" "$<"
        @echo 'Finished building: $<'
        @

    2.Ϊ��ʵ����MounRiver Studio�˵Ľ�����룬ͬ����Ҫ����ͷ�ļ�·��
    Properties->C/C++ Build->Settings->GNU RISC-V Cross Assembler->Includes�����������ͷ�ļ�Ŀ¼
    
    3.��GPIO����ʹ�û�����Ա�д��������Դ����Ϊ.S�ļ���������makefile��ͬ����Ҫ����ͷ�ļ���·����
      ���������޷��ҵ�ͷ�ļ�������

��20240311�� ����uart_re1()�������ٽ�����һ���ֽں�ͨ����ȡLSR�Ĵ�������жϱ�־λ
      
��20240314�����������ڻ���ģʽ������ȫ���ж�

��20240327��BIOS����V1.5 RTOS���޸�USER������RAM�ռ䣻�����жϼ̳�Vectors��ش���

//����˵����BLE�շ�����һ�壬Ĭ��Ϊ����״̬����ͨ��uart1�����жϷ��ʹ��������ݣ�
//��ʱ��Ϊ����״̬���������ݸ�ʽΪ��:  +  ��Ч����    + ;��֡ͷ+��Ч����+֡β����
//ȥ��4�ֽ������ַ��һ�������ɷ���248�ֽڡ����ͳɹ������л��ɽ���ģʽ

��20240717���޸�gpio.s
����Ϳ������������Ϣ������֮ǰCH573��GPIO�����������⣺
��gpio��ʼ��ΪOUTPUTģʽʱ���ٽ����ʼ��ΪINPUTģʽ���޷��ɹ�
�Ӵ����п��Կ���ԭ���Ĳ���ģʽΪ    GPIO_OUTPUT = 0x1 , GPIO_INPUT = 0x0
    case 0: //�˿�A
        if(dir == 1)  //����Ϊ�������
        {
            R32_PA_DIR |= (GPIO_OUTPUT << pin );
            gpio_set(port_pin,state);
        }
        else
        {
            R32_PA_DIR &=~(GPIO_INPUT << pin );
        }
        break;
����ʼ��ΪOUTPUTģʽʱ 
    R32_PA_DIR ��ӦpinλΪ1
�ٽ����ʼ��ΪINPUTģʽʱ����Ӧpinλ�ı仯λ
    0x1 &= ~(0x0) = 0x1 & 0x1 = 0x1
����������ģʽ���䣬Ӧ���޸�Ϊ
    0x1 &= ~(0x1) = 0x1 & 0x0 = 0x0
���޸�����ģʽ���ô���GPIO_INPUTΪGPIO_OUTPUT
���������gpio.s��Ӧ�޸�