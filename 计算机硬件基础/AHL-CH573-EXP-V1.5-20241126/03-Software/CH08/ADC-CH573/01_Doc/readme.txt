【中文名称】认识浮点数的存储方式
【程序功能】 
	    ① 蓝色闪烁；
	    ② 输出浮点数的存储情况。

浮点数计算方法：
先将一个浮点数转化为二进制数，并把该二进制数中的小数点向左移位，
直到小数点的左边只有一位且为1，将移的位数记做n，而将小数点的右边的位数记做m。
IEEE 754约定单精度指数偏移量位127，所以指数偏移量E=127+n。
IEEE 754约定单精度尾数长度为23，在上述小数点后的数后面加上23-m个0
（若m>23，就不用添加），即得到二进制尾数M。
若该数为正数，则S=0，反之就S=1。
这样就能得到符号位S，二进制尾数M，指数偏移量E，
将这三者按表1格式组合在一起，就得到了该浮点数在计算机中的存储值。   

例1：
32.125，先将32.125转换为二进制数，经过计算得二进制：10000.001，
而IEEE754约定小数点左边隐含有一位，所以100000.001=1.0000001*2^5
（小数点向左偏移4位），得指数偏移量位E为5+127=132，即10000100，
IEEE754约定单精度尾数长度为23，所以尾数M为00000001000000000000000，
由于32.125>0，即为整数，所以符号位S为0，最终存储值为0x42008000。

例4：-0.0078125，将数字部分值0.0078125转换为二进制数，得0.0000001=1.0*2^(-7)，
指数偏移量E为-7+127=120，即01111000，单精度尾数长度为23，
所以0.0078125的尾数M为00000000000000000000000,
由于-0.0078125<0，符号位S为1，最终存储值为0xBC000000。
 

