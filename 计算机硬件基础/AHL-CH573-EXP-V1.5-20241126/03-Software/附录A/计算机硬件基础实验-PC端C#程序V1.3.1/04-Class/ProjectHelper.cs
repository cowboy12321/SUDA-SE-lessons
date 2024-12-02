using GEC_LAB._04_Class.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Panuon.WPF.UI;
using Panuon.WPF.UI.Configurations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;

namespace GEC_LAB._04_Class
{
    public class ProjectHelper
    {

        private static Project project = new ();
        private static string? projectPath;
        public readonly static string PathFileName = "GECProject";
        
        public delegate void DataChanged(int no);

        public static event Action? OnReload;
        public static event Action? BeforeSave;
        public static event DataChanged? McuReload;
        public static event Action? SaveStatusChanged;
        public static event DataChanged? PinStatusChangedEvent;
        public static event Action? BasicInfoChanged;
        public static event Action? ImgAddEvent;

        private readonly static string LOG = "ProjectHelper";
        private static MCU? nowMCU;
        private static bool __saved = true;
        public static bool Saved
        {
            get => __saved;
            set
            {
                if (__saved != value)
                {
                    __saved = value;
                    SaveStatusChanged?.Invoke();
                }
                __saved = value;
            }
        }
        #region 初始化
        static ProjectHelper()
        {
            Trace.WriteLine("ProjectHelper initing");
            McuReload += mcuId =>
            {
                nowMCU = McuHelper.Instance.getMcuById(mcuId);
            };
        }
        #endregion

        #region 对project的操作
        public static string Name { get => project.name; set { if (project.name != value) { project.name = value; Saved = false; BasicInfoChanged?.Invoke(); } } }
        public static string Target { get => project.target; set { if (project.target != value) { project.target = value; Saved = false; BasicInfoChanged?.Invoke(); } } }
        public static ReadOnlyCollection<byte[]> ImgList { get => new (project.imgList); }
        public static int SelectedMcuId { get => project.selectedMcu; }
        public static MCU? SelectedMcu { get => nowMCU; }
        public static ReadOnlyCollection<SavedComponent> Components { get => new (project.components); }
        public static ReadOnlyDictionary<int, ExpPin> Pins { get => new (project.pins); }

        public static void EraseImg(int index)
        {
            if (project.imgList.Count > index)
            {
                project.imgList.RemoveAt(index);
                Saved = false;
            }
        }
        public static void PushImg(byte[] img)
        {
            project.imgList.Add(img);
            Saved = false;
            ImgAddEvent?.Invoke();
        }

        public static void ConfigPin(short pinNo,string name) {

            if (project.pins.ContainsKey(pinNo))
            {
                if (project.pins[pinNo].Name == name) return;
                project.pins[pinNo].Name = name;
            }
            else
            {
                string label = "undefined:project";
                if (nowMCU != null) { label = nowMCU.labelMap[pinNo]; }
                project.pins[pinNo] = new ExpPin() { no = pinNo, Name = label, label = label, pinMode = ExpPin.PinMode.None, enable = false };
            }
            PinStatusChangedEvent?.Invoke(pinNo);
        }
        public static void ConfigPin(short pinNo,ExpPin.PinMode pinMode, bool enable)
        {
            if (project.pins.ContainsKey(pinNo))
            {
                ExpPin pin = project.pins[pinNo];
                if (pin.pinMode == pinMode && pin.enable == enable) return;
                pin.pinMode = pinMode;
                pin.enable = enable;
                Saved = false;
            }
            else
            {
                string label = "undefined:project";
                if (nowMCU != null) { label = nowMCU.labelMap[pinNo];}
                project.pins[pinNo] = new ExpPin() {no= pinNo,Name = label, label= label, pinMode = pinMode, enable = enable };
                Saved = false;
            }
            PinStatusChangedEvent?.Invoke(pinNo);
        }
        public static void ConfigPin(short pinNo,string name, ExpPin.PinMode pinMode, bool enable)
        {
            if (project.pins.ContainsKey(pinNo))
            {
                ExpPin pin = project.pins[pinNo];
                if (pin.pinMode == pinMode && pin.enable == enable && project.pins[pinNo].Name == name) return;
                pin.pinMode = pinMode;
                pin.enable = enable;
                pin.Name = name;
            }
            else
            {
                string label = "undefined:project";
                if (nowMCU != null) { label = nowMCU.labelMap[pinNo]; }
                project.pins[pinNo] = new ExpPin() { no = pinNo, Name = name, label = label, pinMode = pinMode, enable = enable };
            }

            PinStatusChangedEvent?.Invoke(pinNo);
        }
        
        public static List<object> GetEnablePins()
        {
            List<object> list = new ();
            list.AddRange(project.pins.Values.Where(item => item.enable).Select(item => (object)item).ToList());
            return list;
        }
        public static ExpPin? GetPin(int pinNo) {
            return project.pins[pinNo];
        }
        public static List<ExpPin> GetPins() {
            return project.pins.Values.ToList();
        }
        public static void ClearSavedComponent()
        {
            project.components.Clear();
            Saved = false;
        }
        public static void AddSavedComponent(SavedComponent sc)
        {
            project.components.Add(sc);
            Saved = false;
        }
        #endregion

        #region 初始化
        public static void Init()
        {
            string[] strings = Environment.GetCommandLineArgs();
            if (strings.Length > 1)
            {
                Project? pj = JsonConvert.DeserializeObject<Project>(File.ReadAllText(strings[1]));
                if (pj != null)
                {
                    projectPath = strings[1];
                    project = pj;
                    CheckProject();
                    ReloadProject();
                    return;
                }
                else
                {
                    MessageBoxX.Show("文件读取失败，打开默认实验!", MessageBoxIcon.Warning);
                    Gobals.logger?.info(LOG, "con not load project from " + strings[1]);
                    projectPath = null;
                }
            }

            if (projectPath == null)
            {
                string? p = null;
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using IsolatedStorageFileStream isfs = storage.OpenFile(PathFileName, FileMode.OpenOrCreate, FileAccess.Read);
                    using StreamReader reader = new (isfs);
                    p = reader.ReadToEnd();
                }

                try
                {
                    Project? pj = JsonConvert.DeserializeObject<Project>(File.ReadAllText(p));
                    if (pj != null)
                    {
                        projectPath = p;
                        project = pj;
                        Gobals.logger?.info(LOG, "read project from " + p);
                        return;
                    }
                }
                catch (Exception)
                {
                    Gobals.main?.gobalStatus("项目加载失败");
                    Gobals.logger?.info(LOG, "con not load project from " + p);
                }
                finally
                {
                    CheckProject();
                    ReloadProject();
                }
            }
        }

        #endregion

        public static void NewProject()
        {
            if (CheckAndSave())
            {
                int _ = project.selectedMcu;
                project = new()
                {
                    selectedMcu = _
                };
                CheckProject();
                projectPath = null;
                ReloadProject();
                Gobals.main?.gobalToast("新实验创建成功");
            }
        }

        public static bool CheckAndSave()
        {
            if (Saved == false)
            {
                var setting = Application.Current.FindResource("CustomSetting") as MessageBoxXSetting;
                MessageBoxResult res = MessageBoxX.Show(Gobals.main, "实验未保存，需要保存吗？", "提示", MessageBoxButton.YesNoCancel, MessageBoxIcon.Info, DefaultButton.YesOK, setting);
                if (res == MessageBoxResult.Yes)
                {
                    SaveProject();
                }
                else if (res == MessageBoxResult.Cancel || res == MessageBoxResult.None)
                {
                    return false;
                }
            }
            return true;
        }

        public static void OptimizationProject() { 
            
        }
        public static string FixProject(string text)
        {
            text = text.Replace("._04_UserControl", "._03_UserControl");
            text = text.Replace("UserControl.component.", "UserControl.Component.");
            return text;

        }
        public static void UpdateProject(Project? project) {
            if (project == null) return;
            project.version = Gobals.defaultProjectVersion;
        }

        public static void SavePath()
        {
            if (projectPath != null)
            {
                using IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                if (storage.FileExists(PathFileName)) { storage.DeleteFile(PathFileName); }
                using (IsolatedStorageFileStream isfs = storage.OpenFile(PathFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    isfs.Write(System.Text.Encoding.UTF8.GetBytes(projectPath));
                }
                string[] strings = storage.GetFileNames();
            }
        }

        public static void SaveProject()
        {
            try
            {
                Project pj = project;
                if (projectPath == null)
                {
                    SaveFileDialog sfd = new ()
                    {
                        InitialDirectory = "C:\\Users\\desktop",
                        Filter = "GEC实验文件|*.glproject",
                        FileName = project.name,
                        Title = "保存"
                    };
                    if (sfd.ShowDialog() == true)
                    {
                        projectPath = sfd.FileName;
                    }
                }
                if (projectPath != null&&projectPath.Length>0)
                {
                    BeforeSave?.Invoke();
                    string json = JsonConvert.SerializeObject(pj, Formatting.Indented) ;
                    File.WriteAllText(projectPath, json);
                    GetAndShowTitle();
                    SavePath();
                    Saved = true;
                }
            }
            catch (Exception ex)
            {
                Gobals.main?.gobalMessage(ex.Message);
                Saved = false;
            }
        }

        public static void SaveAsProject()
        {
            SaveFileDialog sfd = new()
            {
                InitialDirectory = "C:\\Users\\desktop",
                Filter = "实验文件|*.glproject",
                Title = "另存为"
            };
            if (sfd.ShowDialog() == true)
            {
                projectPath = sfd.FileName;
                SaveProject();
            }
        }

        public static void OpenProject()
        {
            if (CheckAndSave())
            {
                OpenFileDialog ofd = new()
                {
                    InitialDirectory = "C:\\Users\\desktop",
                    Filter = "GEC实验文件|*.glproject",
                    Title = "打开实验"
                };
                if (ofd.ShowDialog() == true)
                {
                    SimpleProject? version;
                    Project? pj=null;
                    string text = File.ReadAllText(ofd.FileName);
                    bool open = false;
                    try
                    {
                        version = JsonConvert.DeserializeObject<SimpleProject>(text);
                        if (version == null) MessageBox.Show("打开失败！");
                        else if (version.version < Gobals.defaultProjectVersion)
                        {
                            MessageBoxResult res = MessageBoxX.Show(Gobals.main, "过时的项目，是否强制打开？这可能会破坏该项目。", 
                                "提示", MessageBoxButton.OKCancel, MessageBoxIcon.Info, DefaultButton.CancelNo, Gobals.confirmSettings);
                            if (res == MessageBoxResult.OK)
                            {
                                open = true;
                                text = FixProject(text);
                                pj = JsonConvert.DeserializeObject<Project>(text);
                                UpdateProject(pj);
                            }
                        }
                        else if(version.version>Gobals.defaultProjectVersion)
                        {
                            MessageBox.Show("过时的软件，请联系开发者获取最新软件！");
                        }
                        else
                        {
                            pj = JsonConvert.DeserializeObject<Project>(text);
                            open = true;
                        }

                        if(open){
                            if (pj == null) MessageBox.Show("打开失败！");
                            else
                            {
                                projectPath = ofd.FileName;
                                project = pj;
                                CheckProject();
                                ReloadProject();
                            }

                        }
                    }catch( Exception)
                    {
                        MessageBox.Show("文件异常！");
                    }
                }
            }
        }


        public static void ReloadProject()
        {
            GetAndShowTitle();
            OnReload?.Invoke();
            McuReload?.Invoke(project.selectedMcu);
            Saved = true;
        }

        public static void CheckProject()
        {
            if (project == null)
            {
                project = new Project();
                Gobals.logger?.info(LOG, "use new project");
            }
            if (project.imgData != null)
            {
                project.imgList.Add(project.imgData);
                project.imgData = null;
            }
            OptimizationProject();
        }

        public static void ChangeMcu(int mcuId)
        {
            MessageBoxResult res = MessageBoxX.Show(Gobals.main, "更换MCU后可能会导致数据丢失，确定？", "提示", MessageBoxButton.YesNo, MessageBoxIcon.Info, DefaultButton.YesOK, Gobals.confirmSettings);
            if (res == MessageBoxResult.Yes)
            {
                if (McuHelper.Instance.contains(mcuId))
                {
                    if (mcuId != project.selectedMcu)
                    {
                        project.selectedMcu = mcuId;
                        Saved = false;

                    }
                    CheckProject();
                    McuReload?.Invoke(mcuId);
                }
                else
                {
                    Gobals.main?.gobalToast("未找到该芯片");
                }
            }
        }

        private static void GetAndShowTitle()
        {
            string? filename = Path.GetFileNameWithoutExtension(projectPath);
            if (filename == null) Gobals.main?.gobalTitle(Name);
            else Gobals.main?.gobalTitle(filename);
        }
    }
}
