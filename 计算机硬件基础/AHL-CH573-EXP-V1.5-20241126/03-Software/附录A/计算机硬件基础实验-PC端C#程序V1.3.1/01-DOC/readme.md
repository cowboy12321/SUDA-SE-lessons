代码注意事项：
	1、在设计component和attach时，必须要指定他俩的width和height，否则可能会造成鼠标识别异常
## 代码结构
##### 01-DOC:参考文档
##### 02-Window：窗口设计
##### 03-Component：界面组件
##### 04-Class：类文件
##### 05-Resources：资源文件


---
## 版本更新记录

1.2 
1. 添加了逻辑分析仪

1.2.1
1. 修复逻辑分析仪颜色

1.2.2
1. 对于文件格式不正确的mcu文件，换了一个更好的提示

1.2.3
1. 修复了 在连接设备界面切换实验不会更新引脚信息 的bug
1. 在mcu界面添加了引脚编号
1. 优化了序列生成器的发送逻辑

1.2.4
1. 修改CH573芯片引脚信息

1.3
1. 序列生成器 添加循环模式
1. 添加组件 示波器
1. 优化多曲线图
1. 系统优化

---
## 页面导航
### 总览
采用的是Prism框架进行导航。导航依赖于Prism自动注入的一个IRegionManager。主要有三个步骤：界面占位、注册、调用。
### 步骤
1. 在页面设计文件中使用``` <ContentControl prism:RegionManager.RegionName="[name]" /> ```标签来表示一个导航的占位。[name]填导航区域的名字，
在本工程中所有导航的名字均被存放在PrismNavigation类中。参考GecLab.xaml的xaml文件。
2. 在App.xaml中注册控件。对工程文件中的App.xaml右键查看其代码，有一个RegisterTypes函数注册所有可导航的控件。例如
```containerRegistry.RegisterForNavigation<PageStep01, PageStep01ViewModel>(PrismNaviName.pageStep01);```
PageStep01是导航的控件设计，PageStep01ViewModel为该控件的模型类，圆括号内为该控件的名字。所有控件的名字也保存在PrismNavigation类中。参考app.xaml
3. 在需要的地方调用。本工程的页面导航在GecLabViewModels（该为主界面的模型类）中。GecLabViewModels中有一条动态指令(DelegateCommand,用于下放给xaml文件调用，
命令调用参考geclab.xaml)名为NavigateCommand，通过```regionManager.Regions[PrismRegions.sideBarRegion].RequestNavigate(s.Page);```
实现将某个区域载入指定控件
---
## 鼠标控件(MouseController)
### 控件附件（attachment）
#### 使用方法
1. 在控件中定义一个MouseController变量。例如: ```MouseController mouseController; ```
2. 使用带attach的MouseController构造函数初始化MouseController变量。（或者使用普通构造函数并在之后使用setAttach绑定attach）
```mouseController = new MouseController(this, new CurveAttachment(curveModel, attachDataChanged, attachColorChanged)); ```
3. 使用MouseController中的setMovable激活鼠标控件。
通过上面三个步骤已经完成了attach和component的绑定，后续操作皆由MoucseController完成。请注意attach和component之间的数据绑定需要自行完成。
#### 实现原理以及开放函数
```
		public void setAttach(Control attach)	//绑定新的attach
		public void showAttach()		//如果该component已经绑定attach，则把其显示
		public static void CancelAttachShow()	//取消当前的attach显示
```
### 鼠标拖动
鼠标拖动和attach实现集成在MouseController中，任何组件想实现移动，得在其构造函数中以自身传参构造一个MouseController并调用其setMoveable()方法。
注：相关的鼠标单击事件会在此类中管理。此类会中断冒泡事件，故setMoveable()之后，其父组件将收不到鼠标的冒泡事件。
使用方法：
传入一个参数 c： 控制该组件的鼠标事件，在鼠标单击时不做出反应
传入一个参数c,和一个附加组件attach：控制该组件的鼠标事件，并且在鼠标单击该组件后，附加组件会出现在该组件的右上角。
附加组件可以随时更改删除。
实现原理：
	鼠标拖动：
		允许组件在一个画布中任意拖动（该画布需要在父组件的xaml文件中提前指定）
		鼠标拖动过程可以分为三个阶段，鼠标按下、鼠标移动、鼠标松开。根据这三个阶段做不同的行为。
		[1]鼠标按下：如果是鼠标左键，我们需要记录此时元件在画布中的位置，以便后续计算。要做一些正确性检验，一些非法的变量要排除，然后根据状态标记是否为可移动状态。鼠标按下后若鼠标移动过快可能会移出组件，导致接收不到移动事件，所以要用该组件的captureMouse方法绑定到全局的鼠标监听
		[2]鼠标移动：如果再第一阶段已经被标记为可移动状态，则根据鼠标当前位置计算组件的位置，实时更新
		[3]鼠标松开：结束可移动状态，并释放鼠标
#### 自定义右键菜单
```
    this.ContextMenu.Items.Clear();
    MenuItem item = new MenuItem() { Header= "修改序列" };
    item.Click += edit;
    this.ContextMenu.Items.Add(item);
    item = new MenuItem() { Header = "删除" };
    item.Click +=(s,e)=>mouseController.removeSelf();
    this.ContextMenu.Items.Add(item);
```
---
## 下位机通讯
### 总览
EMUART 是直接和下位机打交道的一个类，每一个串口都会拥有一个EMUART。
EmuartHandler 是EMUART的包装类，封装EMUART使得其功能更加适合项目(切换到不同项目，可能会需要修改内部部分变量)。
在EmuartHandler封装了MCU BIOS的信息、debugEmuart和UserEmuart的状态、连接设备、发送消息。在外层使用时需要检查连接是否成功(EmuartHandler.UserState)，然后只需要调用Emuart.sendMessage()或者Emuart.analogControl(OrderCommand[])。
注：连接设备需要调用linkDebugDevice，传入一个委托，这个委托会输出相关事件信息。

### 下位机通讯协议(20240529)

#### 上位机->下位机

##### 一条指令构成
第一字节

|	位  	| 内容   |
|---	|---   | 
|7:6	| 01    |
|5:3    | 000:analogIn &emsp; 001:analogOut <br> 010:digitalIn &emsp;  011:digitalOut <br> 1xx:undefined |
|2      | digitalVal |
|1:0    | pin(7:6)  |

第二字节

|	位  	| 内容		|
|---	|---		| 
|7:6	| 01		|
|5:0    | pin(5:0)	|

第三、四字节

|	位  	| 内容		|
|---	|---		| 
|7:6	| 01		|
|5:0    | data 	    |

#### 下位机 -> 上位机

##### 一条指令构成
第一字节

|	位  	| 内容   |
|---	|---   | 
|7:4	| 0000:version &emsp; 0001:reserved <br> 0010: reserved &emsp; 0011:normaldata  |
|3      | isDigital |
|2      | digitalVal |
|1:0    | pin(7:6)  |

第二字节

|	位  	| 内容		|
|---	|---		| 
|7:6	| 01		|
|5:0    | pin(5:0)	|

第三、四字节

|	位  	| 内容		|
|---	|---		| 
|7:6	| 01		|
|5:0    | data 	    |

---
## 添加组件
1. 写一个主控件和一个附加控件。附加控件可以不写，附加控件是为了辅助控制主控件而存在
1. 主控件继承ISavedComponent，实现其方法就可以在退出时保存其状态信息
1. 主控件内部加入一个MouseController实例，并在主控件初始化时将这个MouseController初始化，如果有附加控件，在对MouseController初始化时需要同时传入（当然也可以初始化后通过方法写入）。
1. 在pageStep03.xaml的初始化过程中加入对该控件的支持即可

---
## 代码打包发布
使用Advanced installer打包软件，步骤如下
1. 在Visual Studio中发布代码。工程中已经设置好发布文件夹为bin/publish
2. 打开05-Resources中的GEC_LAB.aip，在产品详情中更改软件名称版本
3. 构建项目。在源文件可以看到setupFile，复制里面的文件并删除该文件夹。 