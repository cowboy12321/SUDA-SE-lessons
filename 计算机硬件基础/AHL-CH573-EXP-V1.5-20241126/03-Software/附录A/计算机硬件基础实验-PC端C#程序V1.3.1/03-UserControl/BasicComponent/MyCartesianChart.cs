using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEC_LAB._03_UserControl.BasicComponent
{
    class MyCartesianChart: CartesianChart
    {
        public MyCartesianChart() {
            base.MouseWheel += (s, e) => e.Handled = true;
        }
    }
}
