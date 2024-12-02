#create:2018-10-12
#auther:zsl,wb

#方便阅读本文件的知识
#(1)字符'\'表示换行
#(2)所有目标的第一条指令前必须有一个tab，不可用4个空格代替
#(3)$@  表示目标文件
#   $^  表示所有的依赖文件
#   $<  表示第一个依赖文件
#   $?  表示比目标还要新的依赖文件列表

#定义本文件中使用到的变量【根据实际需要修改】
#控制台输出程序路径
printf    := echo

#.C文件文件夹
SRC_C := ./srcc
#.S文件文件夹
SRC_S := ./srcs
#.o文件存放目录
D_OBJ := ./obj
#.d文件存放目录
D_MK  := ./dmk
#.C文件列表
SRC_CList := $(wildcard ./srcc/*.c)
#.CPP文件列表
#【20200305 1/8】 姜家乐&王浩波 新增.cpp的文件列表，仿照.c文件列表添加
SRC_CPPList := $(wildcard ./srcc/*.cpp)
#.s文件列表
#【20200305 2/8】 姜家乐&王浩波 新增.S文件列表，并将原来.s和.S进行区分
SRC_sList := $(wildcard ./srcs/*.s)
#.S文件列表
SRC_SList := $(wildcard ./srcs/*.S)

#链接文件(.ld)的存储路径
linkFile := "c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\03_MCU\Linker_file\CH573F.ld"
#定义生成的文件名
finalGoal := Flash-CH573
#定义头文件所在的路径
include := -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\01_Doc" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\02_CPU" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\03_MCU" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\04_GEC" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\05_UserBoard" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\06_SoftComPonent" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\07_AppPrg" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\Debug\obj" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\Debug\srcc" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\Debug\srcs" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\03_MCU\Linker_file" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\03_MCU\MCU_drivers" -I"c:\Users\liangfan\OneDrive\enbed\CH573\AHL-CH573-EXP-V1.3-20241124\03-Software\CH09\Flash-CH573\03_MCU\startup"
lib_path:= -L"../03_MCU\MCU_drivers" -L"../03_MCU\MCU_drivers\LIB" -L"../03_MCU\startup" -L"../"\

#定义优化级别。共有O0、O1、O2、Os和Og，默认使用Og
opt	:= O1
#.o文件列表
#【20200305 3/8】 姜家乐&王浩波 将原来的OBJ_C改为OBJ-CS，新增.S的.o生成文件

#把.c文件替换为.o文件
OBJ_CS := $(addprefix $(D_OBJ)/,$(patsubst %.c,%.o,$(notdir $(SRC_CList))))
#把.cpp文件替换为.o文件
OBJ_CS += $(addprefix $(D_OBJ)/,$(patsubst %.cpp,%.o,$(notdir $(SRC_CPPList))))
#把.s文件替换为.o文件  
OBJ_CS += $(addprefix $(D_OBJ)/,$(patsubst %.s,%.o,$(notdir $(SRC_sList))))
#把.S文件替换为.o文件  
OBJ_CS += $(addprefix $(D_OBJ)/,$(patsubst %.S,%.o,$(notdir $(SRC_SList))))

#.d文件列表
#【20200305 4/8】 姜家乐&王浩波 新增.S的.d生成文件

#把.c文件替换为.d文件
SRC_MK := $(addprefix $(D_OBJ)/, $(patsubst %.c,%.d,$(notdir $(SRC_CList))))
#把.cpp文件替换为.d文件
SRC_MK += $(addprefix $(D_OBJ)/, $(patsubst %.cpp,%.d,$(notdir $(SRC_CPPList))))
#把.s文件替换为.d文件
SRC_MK += $(addprefix $(D_OBJ)/, $(patsubst %.s,%.d,$(notdir $(SRC_sList))))
#把.S文件替换为.d文件  
SRC_MK += $(addprefix $(D_OBJ)/, $(patsubst %.S,%.d,$(notdir $(SRC_SList))))


$(D_OBJ)/%.o:$(SRC_C)/%.c
	@echo 'Building file: $<'
	@echo 'Invoking: GNU RISC-V Cross C Compiler'
	riscv-none-embed-gcc 2>>error.log  -march=rv32imac -mabi=ilp32 -mcmodel=medany -msmall-data-limit=8 -mno-save-restore -$(opt)\
 -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -g -DDEBUG=1 -DLOG $(include)\
 -std=gnu99 -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -c -o "$@" "$<"
	@echo 'Finished building: $<'
	@
	

$(D_OBJ)/%.o:$(SRC_S)/%.S
	@echo 'Building file: $<'
	@echo 'Invoking: GNU RISC-V Cross Assembler'
	riscv-none-embed-gcc 2>>error.log  -march=rv32imac -mabi=ilp32 -mcmodel=medany -msmall-data-limit=8 -mno-save-restore -$(opt)\
 -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -g\
 -x assembler -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -c -o "$@" "$<"
	@echo 'Finished building: $<'
	@


# 罗列文件中的目标。目标为希望生成的文件。其中“all”和“clean”为伪目标
all:$(finalGoal).siz $(finalGoal).lst $(finalGoal).hex


$(finalGoal).elf: $(OBJ_CS)
	@echo 'Building target: $@'
	@echo 'Invoking: GNU RISC-V Cross C Linker'
	riscv-none-embed-gcc 2>>error.log  -march=rv32imac -mabi=ilp32 -mcmodel=medany -msmall-data-limit=8 -mno-save-restore -$(opt)\
 -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections\
 -g -T $(linkFile) -nostartfiles -Xlinker --gc-sections $(lib_path) -Xlinker --print-memory-usage\
 -Wl,-Map,"$(finalGoal).map"\
 --specs=nano.specs --specs=nosys.specs -o "$(finalGoal).elf" $(OBJ_CS) -Wl,--no-relax
	@echo 'Finished building target: $@'
	@

$(finalGoal).hex: $(finalGoal).elf
	@echo 'Invoking: GNU RISC-V Cross Create Flash Image'
	riscv-none-embed-objcopy -O ihex "$(finalGoal).elf"  "$(finalGoal).hex"
	@echo 'Finished building: $@'
	@

$(finalGoal).lst: $(finalGoal).elf
	@echo 'Invoking: GNU RISC-V Cross Create Listing'
	riscv-none-embed-objdump --source --all-headers --demangle --line-numbers --wide "$(finalGoal).elf" > "$(finalGoal).lst"
	@echo 'Finished building: $@'
	@

$(finalGoal).siz: $(finalGoal).elf
	@echo 'Invoking: GNU RISC-V Cross Print Size'
	riscv-none-embed-size --format=berkeley "$(finalGoal).elf"
	@echo 'Finished building: $@'
	@

#设置清理工作空间的伪目标
#仅删除目标文件，其它文件若未发生变动则不编译，有利于提高编译速度
fastClean:
	rm -rf "$(finalGoal).elf" "$(finalGoal).hex" "$(finalGoal).map"
	-@$(printf) 'make: fast clean has be done'
	-@$(printf) ' '
	
#删除所有中间文件和目标文件，全部重新编译	
clean:
	-$(RM)  "$(finalGoal).elf" "$(finalGoal).hex" "$(finalGoal).map" "$(finalGoal).lst" "$(finalGoal).hex" $(OBJ_C) $(SRC_MK)
	@echo 'make: clean has be done'
	-@

#指定哪些目标为伪目标
.PHONY: all clean fastClean test


