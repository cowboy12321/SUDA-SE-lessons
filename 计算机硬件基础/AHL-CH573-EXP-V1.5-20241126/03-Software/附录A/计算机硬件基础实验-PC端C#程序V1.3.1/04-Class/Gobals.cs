using GEC_LAB._02_Window;
using GEC_LAB._03_UserControl.Component;
using GEC_LAB._04_Class.Models;
using Panuon.WPF.UI.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace GEC_LAB._04_Class
{
    public class Gobals
    {
        #region 1.静态变量区域
        #region 全局变量
        public static MainWindow? main;
        public static Logger? logger;
        #endregion 
        #region 实验相关
        public static readonly int DEFAULT_MCU=3;

        #endregion
        #region 桌面相关
        public static int canvasZIndex = 0;
        public static MessageBoxXSetting? confirmSettings;

        public static readonly List<ComponentItemModel> componentItemModels = new List<ComponentItemModel>() {
                    new ComponentItemModel() { Text = "仪表盘",Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.仪表盘),type = typeof(VImeter)},
                    new ComponentItemModel() { Text = "曲线图",Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.曲线图),type = typeof(Curve) } ,
                    new ComponentItemModel() { Text = "数字量监控",Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.digital0),type = typeof(Led) },
                    new ComponentItemModel() { Text = "多曲线图",Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.多曲线图),type = typeof(MultiCurve) } ,
                    new ComponentItemModel() {Text="示波器",Source=ImgHelper.BitmapToBitmapImage(Properties.Resources.示波器),type=typeof(Oscilloscope) } ,
                    new ComponentItemModel() { Text = "逻辑分析仪",Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.逻辑分析仪),type = typeof(LogicAnalyzer) } ,
                    new ComponentItemModel() { Text = "序列生成器",Source = ImgHelper.BitmapToBitmapImage(Properties.Resources.序列生成器),type = typeof(SeqGenerator) } ,
                };
    #endregion
    #region 软件信息版本
    private static readonly string DateFileName = "DATE";
        public static readonly string softVersion = "1.3.1";
        public static readonly string createDate;
        public static readonly string softName = "计算机硬件基础实验平台";
        public static readonly string softNameEng = "gec lab";
        public static readonly int targetMcuVersion = 3;
        public static readonly int defaultProjectVersion = 4; //soft 1.3
        /**
         * mcuVersion:
         * 1:修改mcu描述符，前24位大版本+后24位小版本-> 前24位大版本+12位小版本+12位MCU-id号
         * 2:修改MCU描述符,前24位大版本+12位小版本+12位MCU-id号->前22位大版本+12位小版本+12位MCU-id号
         */
        #endregion
        #region 系统配置
        public readonly static TimeSpan ToastInterval = new TimeSpan(10000000);
        public readonly static int BIOSBaudRate = 115200;
        public readonly static int UserBaudRate = 115200;
        #endregion
        #endregion
        #region 2.静态初始化 全局设置
        static Gobals()
        {

            Trace.WriteLine("Gobals init");
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateFileName)))
            {
                string p = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateFileName));
                if (p != null) createDate = p;
                else createDate = "未知时间";
            }
            else createDate = "未知时间";

            McuHelper.GetInstance();
            confirmSettings = Application.Current.FindResource("CustomSetting") as MessageBoxXSetting;
        }

        #endregion
    }
}
