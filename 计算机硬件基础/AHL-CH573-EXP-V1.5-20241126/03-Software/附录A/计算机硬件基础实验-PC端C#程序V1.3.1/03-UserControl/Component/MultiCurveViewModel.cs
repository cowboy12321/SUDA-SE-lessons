using GEC_LAB._03_UserControl.BasicComponent;
using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Models;
using Microsoft.Xaml.Behaviors;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace GEC_LAB._03_UserControl.Component
{
    public class MultiCurveViewModel :BindableBase
    {
        #region 绑定
        private ObservableCollection<LabSeries> seriesList=new ();

        public ObservableCollection<LabSeries> SeriesList
        {
            get { return seriesList; }
            set { seriesList = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }
        private string name = "未命名多曲线";

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private string xName = "X";

        public string XName
        {
            get { return xName; }
            set { xName = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private string yName = "Y";

        public string YName
        {
            get { return yName; }
            set { yName = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private bool autoScale = true;

        public bool AutoScale
        {
            get { return autoScale; }
            set { autoScale = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private int? selectSeriesIndex;

        public int? SelectSeriesIndex
        {
            get { return selectSeriesIndex; }
            set { selectSeriesIndex = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }


        public DelegateCommand newSeriesCommand { get; private set; }
        #endregion
        #region 初始化
        public MultiCurveViewModel()
        {
            newSeriesCommand = new DelegateCommand(seriesAddNew);
            
        }

        #endregion
        #region 内部函数
        private void seriesAddNew() {
            seriesList.Add(new() );
            seriesListChangedEvent?.Invoke(this, new RoutedEventArgs());
        }
        #endregion
        #region 开放函数

        public event RoutedEventHandler? clearSeriesEvent;

        public void RaiseClearSeriesEvent(object sender,RoutedEventArgs? e) { clearSeriesEvent?.Invoke(sender,e); }

        public event RoutedEventHandler? nameChangedEvent;
        public void RaiseNameChangedEvent(object sender, RoutedEventArgs? e) { nameChangedEvent?.Invoke(sender, e); }

        public event RoutedEventHandler? colorChangedEvent;
        public void RaiseColorChangedEvent(object sender, RoutedEventArgs? e) {  colorChangedEvent?.Invoke(sender, e); }

        public event RoutedEventHandler? seriesListChangedEvent;
        public void RaiseSeriesListChangedEvent(object sender, RoutedEventArgs? e) { seriesListChangedEvent?.Invoke(sender, e); }
        public void restore(MultiCurveDataModel dataModel)
        {
            AutoScale = dataModel.autoScale;
            XName = dataModel.XName;
            YName = dataModel.YName;
            SelectSeriesIndex = dataModel.selectIndex;
            seriesList = new(dataModel.series);

        }
        public MultiCurveDataModel save() {
            return new MultiCurveDataModel()
            {
                autoScale = AutoScale,
                XName = XName,
                YName = YName,
                selectIndex = SelectSeriesIndex,
                series = seriesList.Select(i=>i).ToList(),
            };
        }
        #endregion
    }
    public class MultiCurveSeriesBehavior : Behavior<SeriesController>
    {
        private readonly string LOG = "SeriesControllerBehavior";
        public MultiCurveAttachment attachment
        {
            get { return (MultiCurveAttachment)GetValue(attachmentProperty); }
            set { SetValue(attachmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for attachment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty attachmentProperty =
            DependencyProperty.Register("attachment", typeof(MultiCurveAttachment), typeof(MultiCurveSeriesBehavior), new PropertyMetadata(null));



        protected override void OnAttached()
        {
            AssociatedObject.copyFromThisEvent += copy;
            AssociatedObject.deleteThisEvent += delete;
            AssociatedObject.clearThisEvent += clear;
            AssociatedObject.colorChangedEvent += colorChanged;
            AssociatedObject.nameChangedEvent += nameChanged;
            AssociatedObject.formulaChangedEvent += formulaChangedEvent;
        }

        private void formulaChangedEvent(object sender, RoutedEventArgs e)
        {
            int index = getIndex(sender);
            if (index != -1 && attachment.DataContext is MultiCurveViewModel vm)
            {
                vm.SeriesList[index].FormulaX = ((SeriesController)sender).FormulaX;
                vm.SeriesList[index].FormulaY = ((SeriesController)sender).FormulaY;
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常 in clear");
            }
        }

        private void nameChanged(object sender, RoutedEventArgs e)
        {
            int index = getIndex(sender);
            if (index != -1 && attachment.DataContext is MultiCurveViewModel vm)
            {
                vm.SeriesList[index].Name = ((SeriesController)sender).CurveName;
                vm.RaiseNameChangedEvent(vm.SeriesList[index], e); 
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常 in clear");
            }
        }

        private void colorChanged(object sender, RoutedEventArgs e)
        {
            int index = getIndex(sender);
            if (index != -1 && attachment.DataContext is MultiCurveViewModel vm)
            {
                vm.SeriesList[index].LineColor = ((SeriesController)sender).LineColor;
                vm.RaiseColorChangedEvent(vm.SeriesList[index], e);
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常 in clear");
            }
        }

        private int getIndex(object sender)
        {
            if (sender is SeriesController sc)
            {
                if (attachment.DataContext is MultiCurveViewModel vm)
                {
                    for (int i = 0; i < vm.SeriesList.Count; i++)
                    {
                        if (vm.SeriesList[i].UUID == sc.UUID)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;

        }
        private void clear(object sender, RoutedEventArgs e)
        {
            int index = getIndex(sender);
            if(index != -1 && attachment.DataContext is MultiCurveViewModel vm)
            {
                vm.RaiseClearSeriesEvent(vm.SeriesList[index], null);
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常 in clear");
            }
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            int index = getIndex(sender);
            if (index != -1 && attachment.DataContext is MultiCurveViewModel vm)
            {
                vm.SeriesList.RemoveAt(index);
                vm.RaiseSeriesListChangedEvent(vm.SeriesList, null);
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常 in delete");
            }
        }

        private void copy(object sender, RoutedEventArgs e)
        {
            int index = getIndex(sender);
            if (index != -1 && attachment.DataContext is MultiCurveViewModel vm)
            {
                vm.SeriesList.Add(new LabSeries(vm.SeriesList[index]));
                vm.RaiseSeriesListChangedEvent(vm.SeriesList, null);
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常 in copy");
            }
        }

        protected override void OnDetaching()
        {

            AssociatedObject.copyFromThisEvent -= copy;
            AssociatedObject.deleteThisEvent -= delete;
        }
    }

}