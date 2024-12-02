using GEC_LAB._02_Window;
using GEC_LAB._03_UserControl.Component.ViewModels;
using GEC_LAB._04_Class;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GEC_LAB._03_UserControl.Component
{
    /// <summary>
    /// CurveAttachment.xaml 的交互逻辑
    /// </summary>
    public partial class CurveAttachment : UserControl
    {
        public Action? DataChangeHandler;
        public Action? ColorChangeHandler;
        public Action? MaxChangeHandler;

        CurveDataModel cm;
        public CurveAttachment(CurveDataModel model,Action DataChangeHandler, Action ColorChangeHandler)
        {
            InitializeComponent();
            this.cm= model;
            this.DataChangeHandler += DataChangeHandler;
            this.ColorChangeHandler += ColorChangeHandler;
            init();
        }

        private void init() {
            Width = 240;
            Height = 245;
            txtName.Text = cm.name;
            txtX.Text = cm.XName;
            txtXFormula.Text = cm.formulaX;
            txtY.Text = cm.YName;
            txtYFormula.Text = cm.formulaY;
            autoScale.IsChecked = cm.autoScale;
            colorPicker.SelectedColor = cm.color;

            txtName.TextChanged += TextChanged;
            txtX.TextChanged += TextChanged;
            txtXFormula.TextChanged+= TextFormulaChanged;
            autoScale.Checked += CheckedChanged;
            autoScale.Unchecked += CheckedChanged;
            txtY.TextChanged += TextChanged;
            txtYFormula.TextChanged += TextFormulaChanged;
            colorPicker.SelectedColorChanged += SelectedColorChanged;

            txtYFormula.PreviewMouseLeftButtonDown += gotFocus;
            txtXFormula.PreviewMouseLeftButtonDown += gotFocus;

            xNumber.ValueChanged += MaxChanged;
            yNumber.ValueChanged += MaxChanged;
        }

        private void MaxChanged(object sender, Panuon.WPF.SelectedValueChangedRoutedEventArgs<double?> e)
        {
            MaxChangeHandler?.Invoke();
            ProjectHelper.Saved = false;
        }

        private void CheckedChanged(object sender, RoutedEventArgs e)
        {
            DataChangeHandler?.Invoke();
            ProjectHelper.Saved = false;
        }

        private void gotFocus(object sender, RoutedEventArgs e)
        {
            Control c= (Control)sender;

            TextBox? handleControl = null;
            if (c.Name == "txtXFormula") handleControl = txtXFormula;
            else if (c.Name == "txtYFormula") handleControl=txtYFormula;
            if (handleControl != null)
            {
                ExpressEditor ee = new ExpressEditor(Gobals.main, handleControl.Text, ProjectHelper.GetEnablePins());
                if (ee.ShowDialog() == true) {
                    handleControl.Text = ee.data;
                    ((Panel)handleControl.Parent).Focus();
                }
            }
        }

        private void SelectedColorChanged(object sender, Panuon.WPF.SelectedValueChangedRoutedEventArgs<Color?> e)
        {
            if(colorPicker.SelectedColor!=null) cm.color=(Color)colorPicker.SelectedColor;
            ColorChangeHandler?.Invoke();
            ProjectHelper.Saved = false; 
        }

        private void TextFormulaChanged(object sender, TextChangedEventArgs e)
        {
            cm.formulaX = txtXFormula.Text;
            cm.formulaY = txtYFormula.Text;
            ProjectHelper.Saved = false;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            cm.name=txtName.Text ;
            cm.XName=txtX.Text ;
            cm.YName=txtY.Text ;
            colorPicker.SelectedColor = cm.color;
            ProjectHelper.Saved = false;
            DataChangeHandler?.Invoke();
        }
    }
}
