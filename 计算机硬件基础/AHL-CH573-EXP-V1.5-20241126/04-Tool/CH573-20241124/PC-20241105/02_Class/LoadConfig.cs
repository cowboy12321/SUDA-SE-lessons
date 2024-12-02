using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHL_GEC
{
    public class LoadConfig
    {
        public class ModulePath 
        {       
            public string name;          //MCU名称
            public string path;    //模板文件路径   
        }
        public class Config
        {
            public List<ModulePath> modulePath;
        }
        public  Config config;
        public Dictionary<string, string> module_path = new Dictionary<string, string>();
        public bool GetConfig()
        {
            try
            {
                 string configContent = File.ReadAllText(Environment.CurrentDirectory + @"\Config.json");
                 config = JsonConvert.DeserializeObject<Config>(configContent);
                 
                foreach (ModulePath m in config.modulePath)
                {
                    module_path.Add(m.name, m.path);
                }
            }
            catch (Exception)
            {
                throw;
                //return false;
            }
            return true;
        }
    }
}
