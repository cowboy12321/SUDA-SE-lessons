using ImTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace GEC_LAB._04_Class
{
    internal class DataRefreshCenter
    {
        /// <summary>
        /// 存放的key 都为数字pin引脚号
        /// digitalOriData 的value是一个元组，元组的第一个数字为0的出现次数，第二个数字为1的出现次数
        /// </summary>
        private DictionaryHelper<int, double> analogData = new DictionaryHelper<int, double>();
        private DictionaryHelper<int, bool> digitalData = new DictionaryHelper<int, bool>();
        private readonly static string LOG = "DataRefreshCenter";
        private readonly int componentTimerInterval = 50;
        private readonly int DataWatchDogInterval = 100;//300 毫秒(ms)
        private DateTime lastRefreshTime = DateTime.Now;
        private Thread? WatchDogThread;
        public bool enableDog = false;
        private bool dogStatus = false;
        public Action? DataReceiveEvent;
        public bool DogStatus { get => dogStatus;}
        public bool EnableDog {
            get => enableDog;
            set { 
                lastRefreshTime = DateTime.Now;
                if (enableDog=value)
                {
                    if(WatchDogThread==null || !WatchDogThread.IsAlive) {
                        WatchDogThread = new Thread(WatchDog_Tick);
                        WatchDogThread.Start();
                        lastRefreshTime = DateTime.Now;
                        dogStatus = true;
                    }
                    else
                    {
                        lastRefreshTime = DateTime.Now;
                        dogStatus = true;
                    }
                }
                else
                {
                    dogStatus = false;
                }
            } 
        }
        private bool enableComponentTick = false;
        public bool EnableComponentTick
        {
            get { return enableComponentTick; }
            set { if (value == true && enableComponentTick==false) { 
                    new Thread(component_tick).Start(); 
                }
                enableComponentTick = value; }
        }
        public delegate bool Checker();
        public Checker? WatchDogHungry;
        public Action? ComponentTickEvent;
        public bool dataChangedRefresh=false;

        #region 对外开放
        public static double getAnalog(int pin, double defaultValue = 0)
        {
            return GetInstance().analogData.GetValueOrDefault(pin, defaultValue);
        }
        public static bool getDigital(int pin, bool defaultValue = false)
        {
            return GetInstance().digitalData.GetValueOrDefault(pin, defaultValue);
        }
        public static bool tryGetAnalog(int pin, out double value)
        {
            return GetInstance().analogData.TryGetValue(pin, out value);
        }
        public static bool tryGetDigital(int pin, out bool value)
        {
            return GetInstance().digitalData.TryGetValue(pin, out value);
        }
        #endregion

        #region 初始化
        private void init()
        {
            EmuartHandler.GetInstance().UserByteReceiveEvent += DataProcessNew;
            Gobals.logger?.info(LOG, "init done!");
        }

        #endregion

        #region 源数据处理
        private void DataProcessNew(byte[] data) {
            lastRefreshTime = DateTime.Now;
            //string temp = "";
            for(int i = 0; i < data.Length; )
            {
                byte ch1, ch2, ch3;
                ch1 = data[i++];
                ch2 = data[i++];
                int pin = ch2 & 0x3f;
                if ((ch1 & 0x20) != 0)
                {
                    //digital
                    digitalData[pin] = (ch1 & 0x01) == 0x01;
                    //temp += "digital " + pin + "=" + ((ch1 & 0x04) == 0x04)+"   ";
                    continue;
                }

                ch3 = data[i++];

                int val = (ch3 & 0x7f) << 5 | (ch1 & 0x1f);
                analogData[pin]= val* 3.3 / 4095;
                //temp += "analog " + pin + "=" + val + "("+(val * 3.3 / 4095) +")   ";
            }
            DataReceiveEvent?.Invoke();
            //Gobals.logger?.info("DataRefreshCenter", temp);
        }

        #endregion

        #region 定时组件管理
        private void component_tick()
        {
            while (enableComponentTick)
            {
                ComponentTickEvent?.Invoke();
                Thread.Sleep(componentTimerInterval);
            }
        }
        #endregion

        #region 数据看门狗

        private void WatchDog_Tick()
        {
            while (enableDog)
            {
                DateTime now = DateTime.Now;
                if ((now - lastRefreshTime).Ticks/*100ns*/ > DataWatchDogInterval * 10000)
                {
                    if (dogStatus)
                    {
                        bool? v = WatchDogHungry?.Invoke();
                        dogStatus = v==true;
                    }
                }
                Thread.Sleep(DataWatchDogInterval);
            }
        }
        #endregion

        #region 表达式计算
        public bool tryCalculateLogic(string express, out bool res)
        {
            express.Replace('（', '(');
            express.Replace('）', ')');
            int l = express.IndexOf('['), r = express.IndexOf("]");
            int ll, rr;
            while (l != -1 && r != -1)
            {
                string subs = express.Substring(l, r - l + 1);
                ll = subs.IndexOf("(");
                rr = subs.IndexOf(")");
                if (ll != -1 && r != -1 && rr - ll > 1)
                {
                    int pin = int.Parse(subs.Substring(ll + 1, rr - ll - 1));
                    express = express.Substring(0, l) + digitalData.GetValueOrDefault(pin, false) + express.Substring(r + 1);
                }
                l = express.IndexOf('[');
                r = express.IndexOf("]");
            }
            try
            {
                res = CommonUtils.clacLogicExpress(express);
                return true;
            }
            catch (Exception)
            {
                res = false;
                return false;
            }
        }
        public bool tryCalculate(string express, out double res)
        {
            try
            {
                express.Replace('（', '(');
                express.Replace('）', ')');
                int l = express.IndexOf('['), r = express.IndexOf("]");
                int ll = -1, rr = -1;
                while (l != -1 && r != -1)
                {
                    string subs = express.Substring(l, r - l + 1);
                    ll = subs.IndexOf("(");
                    rr = subs.IndexOf(")");
                    if (ll != -1 && r != -1 && rr - ll > 1)
                    {
                        int pin = int.Parse(subs.Substring(ll + 1, rr - ll - 1));
                        if(analogData.ContainsKey(pin))
                        {
                            express = express.Substring(0, l) + analogData.GetValueOrDefault(pin, 0) + express.Substring(r + 1);
                        }
                        else
                        {
                            express = express.Substring(0, l) + (digitalData.GetValueOrDefault(pin, false)?3.3:0) + express.Substring(r + 1);

                        }
                        ll = rr = -1;
                    }else
                    {
                        throw new FormatException("illegal express");
                    }
                    l = express.IndexOf('[');
                    r = express.IndexOf("]");
                }
                express = express.Replace('÷', '/');
                express = express.Replace('＋', '+');
                express = express.Replace('－', '-');
                express = express.Replace('×', '*');

                if (l != -1 || r != -1 || ll != -1 || r != -1)
                {
                    res = -1;
                    return false;
                }
                else
                {
                    string? s = new DataTable().Compute(express, "false").ToString();
                    if (s != null)
                    {
                        if (s == "") res = 0;
                        else res = Math.Round(double.Parse(s), 2);
                    }
                    else res = -1;
                    return s != null;
                }
            }
            catch (Exception)
            {
                res = -1;
               return false;
            }
        }
        #endregion

        #region 单例
        // 定义一个静态变量来保存类的实例
        public static DataRefreshCenter Instance { get => GetInstance(); }
        private static DataRefreshCenter? instance;

        // 定义私有构造函数，使外界不能创建该类实例
        private DataRefreshCenter()
        {
            init();
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static DataRefreshCenter GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (instance == null)
            {
                lock ("lock")
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (instance == null)
                    {
                        instance = new DataRefreshCenter();
                    }
                }
            }
            return instance;
        }
        #endregion
    }
}