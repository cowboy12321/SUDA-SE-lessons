using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace GEC_LAB._04_Class.Models
{
    public class SideMenuItem : BindableBase {
		private string? text;

		public string? Text
		{
			get { return text; }
			set { text = value; }
		}
		private string? page;

		public string? Page
		{
			get { return page; }
			set { page = value; }
		}
		private string? bodyPage;

		public string ?BodyPage
		{
			get { return bodyPage; }
			set { bodyPage = value; }
		}
	}
    public class McuPinMode :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string? name;

		public string? Name
		{
			get { return name; }
			set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); }
		}

		private bool enable;

		public bool Enable
		{
			get { return enable; }
			set { enable = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Enable")); }
        }

		private int no;

		public int No
		{
			get { return no; }
			set { no = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("No")); }
		}

        private string label;

        public string Label
        {
            get { return label; }
            set { label = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Label")); }
        }
        private int pinMode;

        // 1 analog in
        // 2 analog  out
        // 3 digital in
        // 4 digital out
        public int PinMode
		{
			get { return pinMode; }
			set { pinMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PinMode")); }
		}
		public McuPinMode() { 
			label= "";
		}
		public McuPinMode(ExpPin pin)
		{
			label = "";
			Name = pin.Name;
			No = pin.no;
			Enable = pin.enable;
			PinMode = (int)pin.pinMode;
			Label= pin.label; 
        }
    }
	public class ComponentItemModel :BindableBase{
		private string? text;

		public string? Text
		{
			get { return text; }
			set { text = value; }
		}
		private ImageSource? source;

		public ImageSource? Source
        {
			get { return source; }
			set { source = value; }
		}

		public Type? type;
    }
    public class LabSeries:BindableBase
    {
		private string uuid = Guid.NewGuid().ToString();
		public string UUID { get => uuid; set => uuid = value; }
        private string name = "曲线1";
        public string Name { get => name; set => name = value; }
        private string formulaX = "";
        public string FormulaX { get => formulaX; set => formulaX = value; }
        private string formulaY = "";
        public string FormulaY { get => formulaY; set => formulaY = value;}
        private Color lineColor = Color.FromRgb(0, 0, 0);
        public Color LineColor { get => lineColor; set => lineColor = value;}
        public List<GPoint> points = new List<GPoint>();
		public LabSeries() { 
		}
		public LabSeries(LabSeries series)
		{
			this.name= series.name + "的复制";
			this.formulaY = series.formulaY;
			this.formulaX	= series.formulaX;
			this.lineColor = series.lineColor;
			this.points = new (series.points);
		}
    }

    public class OscilChannel : BindableBase
    {
        private string uuid = Guid.NewGuid().ToString();
        public string UUID { get => uuid; set => uuid = value; }
        private string name = "曲线1";
        public string Name { get => name; set => name = value; }
        private string formula = "";
        public string Formula { get => formula; set => formula = value; }
		private double offset;
		public double Offset{get { return offset; }set { offset = value; }}

		private Color lineColor = Color.FromRgb(0xff, 0x26, 0x68);
        public Color LineColor { get => lineColor; set => lineColor = value; }
        public List<GPoint> points = new List<GPoint>();
        public OscilChannel()
        {
        }
        public OscilChannel(OscilChannel ch)
        {
            this.name = ch.name + "的复制";
            this.formula = ch.formula;
            this.lineColor = ch.lineColor;
            this.points = new(ch.points);
        }
    }
}
