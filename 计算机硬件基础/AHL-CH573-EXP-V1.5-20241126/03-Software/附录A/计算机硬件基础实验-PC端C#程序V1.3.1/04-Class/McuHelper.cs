using System.IO;
using System;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using GEC_LAB._04_Class.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GEC_LAB._04_Class
{
    public class McuHelper
    {
        #region 1.全局变量
        private static string DebugMcuPath = "05-Resources\\MCU";
        private static string McuPath = "MCU";
        private static DictionaryHelper<int, MCU> mcuMap = new DictionaryHelper<int, MCU>();
        #endregion
        #region 3.单例与初始化
        private static McuHelper? instance;
        public static McuHelper Instance { get => GetInstance(); }

        private McuHelper()
        {
            if (Debugger.IsAttached)
            {
                DirectoryInfo? d = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
                if (d != null)
                {
#pragma warning disable 8602
                    string? mcuPath = Path.Combine(d.Parent.Parent.Parent.ToString(), DebugMcuPath);
#pragma warning restore 8602
                    loadDirectory(mcuPath);
                }
            }
            else
            {
                loadDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, McuPath));
            }
        }
        private void loadDirectory(string mcuPath){
            string record = "";
            foreach (string s in Directory.GetFiles(mcuPath))
            {
                try
                {
                    if (s.EndsWith(".mcu.json"))
                    {
                        MCU? m = JsonConvert.DeserializeObject<MCU>(File.ReadAllText(s));
                        if (m != null && m.mcuIdentifier > 0)
                        {
                            if (!checkMcu(m, out string errorString)) { MessageBox.Show("读取MCU文件错误！错误文件：" + s + "\n错误类型：" + errorString); }
                            else mcuMap.put(m.mcuIdentifier, m);
                        }
                    }
                }
                catch (Exception)
                {
                    record += "\n" + s;
                }
            }
            if (record.Length>0)
            {
                MessageBox.Show("MCU文件格式错误，不符合要求的文件格式！错误文件："+ record);
            }
        }
        private bool checkMcu(MCU mcu,out string error)
        {
            string spin = "";
            Collection<int> checks = new Collection<int>();
            mcu.digitalPin.ForEach(pin => {
                //checkExpPin(pin, ExpPin.PinMode.DigitalIn);
                if (checks.Contains(pin)) spin+=" digital:"+pin;
                else checks.Add(pin);
            });
            checks.Clear();
            mcu.analogInPin.ForEach(pin => {
                //checkExpPin(pin, ExpPin.PinMode.AnalogIn);
                if (checks.Contains(pin)) spin += " AnalogIn:" + pin;
                else checks.Add(pin);
            });
            checks.Clear();
            mcu.analogOutPin.ForEach(pin => {
                //checkExpPin(pin, ExpPin.PinMode.AnalogOut);
                if (checks.Contains(pin)) spin += " AnalogOut:" + pin;
                else checks.Add(pin);
            });

            error = "";
            if (mcuMap.ContainsKey(mcu.mcuIdentifier)) error += "mcu Identifier 重复，重复码：" + mcu.mcuIdentifier;
            if(spin != "") error += "引脚编号重复，重复引脚："+spin; 
            
            return error=="";
        }

        public static McuHelper GetInstance()
        {
            if (instance == null)
            {
                lock ("lock")
                {
                    if (instance == null)
                    {
                        instance = new McuHelper();
                    }
                }
            }
            return instance;
        }
        #endregion
        #region 4.开放函数
        
        
        public int GetMcuCount()
        {
            return mcuMap.Count;
        }

        public List<MCU> GetMcuList()
        {
            return mcuMap.Values.ToList<MCU>();
        }
        public bool contains(int id)
        {
            return mcuMap.ContainsKey(id);
        }
        public MCU? getMcuById(int id)
        {
            return mcuMap.GetValueOrDefault(id);
        }

        public static List<ExpPin> getAnalogIn(MCU mcu)
        {
            return mcu.analogInPin.Select(pin => new ExpPin()
            {
                no = pin,
                Name = mcu.labelMap.GetValueOrDefault(pin, "undefined:mcu"),
                label = mcu.labelMap.GetValueOrDefault(pin, "undefined:mcu"),
                pinMode = ExpPin.PinMode.AnalogIn
            }).ToList();
        }
        public static List<ExpPin> getAnalogOut(MCU mcu)
        {
            return mcu.analogOutPin.Select(pin => new ExpPin()
            {
                no = pin,
                Name = mcu.labelMap.GetValueOrDefault(pin, "undefined:mcu"),
                label = mcu.labelMap.GetValueOrDefault(pin, "undefined:mcu"),
                pinMode = ExpPin.PinMode.AnalogOut
            }).ToList();
        }
        public static List<ExpPin> getDigital(MCU mcu)
        {
            return mcu.digitalPin.Select(pin => new ExpPin()
            {
                no = pin,
                Name = mcu.labelMap.GetValueOrDefault(pin, "undefined:mcu"),
                label = mcu.labelMap.GetValueOrDefault(pin, "undefined:mcu"),
                pinMode = ExpPin.PinMode.DigitalIn
            }).ToList();
        }
        #endregion
    }
}
