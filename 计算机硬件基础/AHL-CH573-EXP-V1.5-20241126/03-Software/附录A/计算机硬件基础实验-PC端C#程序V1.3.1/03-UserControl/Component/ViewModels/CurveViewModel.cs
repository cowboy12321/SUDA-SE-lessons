using Prism.Mvvm;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GEC_LAB._03_UserControl.Component.ViewModels
{
    public class CurveViewModel:BindableBase
    {
		private double xMax;

		public double XMax
		{
			get { return xMax; }
			set { xMax = value; RaisePropertyChanged(); }
		}
		private double yMax;

		public double YMax
		{
			get { return yMax; }
			set { yMax = value; RaisePropertyChanged(); }
		}
		private bool autoScale;

		public bool AutoScale
		{
			get { return autoScale; }
			set { autoScale = value; RaisePropertyChanged(); }
		}

	}
    public class BoolReverser : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			if(value is bool) {
				return !(bool)value;
			}
			return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }
    }
}
