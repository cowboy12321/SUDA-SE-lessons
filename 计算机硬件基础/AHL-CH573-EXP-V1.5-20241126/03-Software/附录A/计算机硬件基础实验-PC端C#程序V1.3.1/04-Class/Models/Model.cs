using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GEC_LAB._04_Class.Models
{
    public class SimpleProject
    {
        public int version;
    }
    public class Project
    {
        public string name = "未命名实验";
        public string target = "";
        public int version = Gobals.defaultProjectVersion;  
        public readonly DictionaryHelper<int,ExpPin>  pins = new(); //project 2
        public List<SavedComponent> components = new(); //project 2
        public int selectedMcu = Gobals.DEFAULT_MCU; //project 2
        public byte[] ?imgData;  //project 2->2
        public List<byte[]> imgList=new(); //project 3
        public Project() { }
        public new bool Equals(object? obj)
        {
            if(obj==null||!(obj is Project)) return false;
            Project rhs= (Project)obj;
            return name.Equals(rhs.name)&& 
                target.Equals(rhs.target)&&
                version.Equals(rhs.version)&&
                pins.Equals(rhs.pins)&&
                components.Equals(rhs.components)&&
                selectedMcu.Equals(rhs.selectedMcu)&&
                imgData==rhs.imgData;
        }
    }
    public class MCU
    {
        public int mcuIdentifier = -1;
        public string mcuName = "未命名MCU";
        public int targetMcuVersion=0;
        public readonly List<int> leftPanel=new List<int>();
        public readonly List<int> rightPanel = new List<int>();
        public readonly List<int> leftNoPanel = new();
        public readonly List<int> rightNoPanel = new();
        public readonly Dictionary<int, string> labelMap = new Dictionary<int, string>();
        public readonly List<short> analogInPin = new();
        public readonly List<short> analogOutPin = new();
        public readonly List<short> digitalPin = new();
    }
    public class SavedComponent { 
        public Type ?type;
        public object? data;
        public double top;
        public double left;
        public int ZIndex;
        public SavedComponent() { 
        }
        public SavedComponent(Type t) {
            this.type = t;
        }
        public SavedComponent(Type t,object data)
        {
            this.type = t;
            this.data= data;
        }
    }
    public class ExpPin{
        public short no;
        private string name="";
        public string Name { 
            get =>name;
            set
            {
                if (name == value) return;
                name = value;
                NameChanged?.Invoke();
            }
        }
        public string label = "";
        public bool enable = false;
        public PinMode pinMode = PinMode.AnalogIn;
        public override string ToString()
        {
            return name + "(" + no + ")";
        }
        [JsonIgnore]
        public Action? NameChanged;
        public enum PinMode{None=0, AnalogIn=1,AnalogOut=2,DigitalIn=3,DigitalOut=4 }
    }
    public class GPoint
    {
        public double x, y;
        public GPoint(double x, double y) { this.x = x; this.y = y; }
    }
}
