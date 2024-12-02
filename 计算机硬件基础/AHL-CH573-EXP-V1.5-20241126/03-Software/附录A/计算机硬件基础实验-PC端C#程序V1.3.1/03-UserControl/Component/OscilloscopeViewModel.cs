using GEC_LAB._03_UserControl.BasicComponent;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class;
using Microsoft.Xaml.Behaviors;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GEC_LAB._03_UserControl.Component
{
    public class OscilloscopeViewModel : BindableBase
    {
        #region 绑定
        private ObservableCollection<OscilChannel> seriesList = new();

        public ObservableCollection<OscilChannel> SeriesList
        {
            get { return seriesList; }
            set
            {
                seriesList = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }
        private string name = "未命名示波器";

        public string Name
        {
            get { return name; }
            set
            {
                name = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private string yName = "Y";

        public string YName
        {
            get { return yName; }
            set
            {
                yName = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private bool rolling = true;

        public bool Rolling
        {
            get { return rolling; }
            set
            {
                rolling = value; RaisePropertyChanged();
                ProjectHelper.Saved = false;
            }
        }

        private double rollingTime;

        public double RollingTime
        {
            get { return rollingTime; }
            set { rollingTime = value; }
        }

        public DelegateCommand newSeriesCommand { get; private set; }
        #endregion
        #region 初始化
        public OscilloscopeViewModel()
        {
            newSeriesCommand = new DelegateCommand(seriesAddNew);

        }

        #endregion
        #region 内部函数
        private void seriesAddNew()
        {
            seriesList.Add(new());
            seriesListChangedEvent?.Invoke(this, new RoutedEventArgs());
        }
        #endregion
        #region 开放函数

        public event RoutedEventHandler? clearSeriesEvent;

        public void RaiseClearSeriesEvent(object sender, RoutedEventArgs? e) { clearSeriesEvent?.Invoke(sender, e); }

        public event RoutedEventHandler? nameChangedEvent;
        public void RaiseNameChangedEvent(object sender, RoutedEventArgs? e) { nameChangedEvent?.Invoke(sender, e); }

        public event RoutedEventHandler? colorChangedEvent;
        public void RaiseColorChangedEvent(object sender, RoutedEventArgs? e) { colorChangedEvent?.Invoke(sender, e); }

        public event RoutedEventHandler? seriesListChangedEvent;
        public void RaiseSeriesListChangedEvent(object sender, RoutedEventArgs? e) { seriesListChangedEvent?.Invoke(sender, e); }


        public event RoutedEventHandler? offsetChangedEvent;
        public void RaiseOffsetChangedEvent(object sender, RoutedEventArgs? e) { offsetChangedEvent?.Invoke(sender, e); }

        public void restore(OscilloscopeDataModel dataModel)
        {
            Rolling = dataModel.Rolling;
            RollingTime = dataModel.RollingTime;
            YName = dataModel.YName;
            seriesList = new(dataModel.series);

        }
        public OscilloscopeDataModel save()
        {
            return new OscilloscopeDataModel()
            {
                Rolling = Rolling,
                RollingTime = RollingTime,
                YName = YName,
                series = seriesList.Select(i => i).ToList(),
            };
        }
        #endregion
    }


    public class OscilloscopeSeriesBehavior : Behavior<OscilChannelController>
    {
        private readonly string LOG = "OscilloscopeSeriesBehavior";
        public OscilloscopeAttachment attachment
        {
            get { return (OscilloscopeAttachment)GetValue(attachmentProperty); }
            set { SetValue(attachmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for attachment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty attachmentProperty =
            DependencyProperty.Register("attachment", typeof(OscilloscopeAttachment), typeof(OscilloscopeSeriesBehavior), new PropertyMetadata(null));



        protected override void OnAttached()
        {
            AssociatedObject.copyFromThisEvent += copy;
            AssociatedObject.deleteThisEvent += delete;
            AssociatedObject.clearThisEvent += clear;
            AssociatedObject.colorChangedEvent += colorChanged;
            AssociatedObject.nameChangedEvent += nameChanged;
            AssociatedObject.formulaChangedEvent += formulaChangedEvent;
            AssociatedObject.offsetChangedEvent += offsetChangedEvent;
        }

        private void offsetChangedEvent(object sender, RoutedEventArgs e)
        {
            getIndexAndHandle(sender, (index, vm) =>
            {
                vm.SeriesList[index].Offset = ((OscilChannelController)sender).Offset;
                vm.RaiseOffsetChangedEvent(vm.SeriesList[index], e);
            });
        }

        private void formulaChangedEvent(object sender, RoutedEventArgs e)
        {
            getIndexAndHandle(sender, (index, vm) =>
            {
                vm.SeriesList[index].Formula = ((OscilChannelController)sender).Formula;
            });
        }

        private void nameChanged(object sender, RoutedEventArgs e)
        {
            getIndexAndHandle(sender, (index, vm) =>
            {
                vm.SeriesList[index].Name = ((OscilChannelController)sender).ChannelName;
                vm.RaiseNameChangedEvent(vm.SeriesList[index], e);
            });
        }

        private void colorChanged(object sender, RoutedEventArgs e)
        {
            getIndexAndHandle(sender, (index,vm) =>
            {
                vm.SeriesList[index].LineColor = ((OscilChannelController)sender).LineColor;
                vm.RaiseColorChangedEvent(vm.SeriesList[index], e);
            });
        }
        delegate void Func(int index, OscilloscopeViewModel vm);
        private void getIndexAndHandle(object sender, Func f)
        {
            int index = getIndex(sender);
            if (index != -1 && attachment.DataContext is OscilloscopeViewModel vm)
            {
                f(index,vm);
            }
            else
            {
                Gobals.logger?.error(LOG, "下标寻找失败或数据绑定异常");
            }

        }

        private int getIndex(object sender)
        {
            if (sender is OscilChannelController sc)
            {
                if (attachment.DataContext is OscilloscopeViewModel vm)
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
            if (index != -1 && attachment.DataContext is OscilloscopeViewModel vm)
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
            getIndexAndHandle(sender, (index, vm) =>
            {
                vm.SeriesList.RemoveAt(index);
                vm.RaiseSeriesListChangedEvent(vm.SeriesList, null);
            });
        }

        private void copy(object sender, RoutedEventArgs e)
        {
            getIndexAndHandle(sender, (index, vm) =>
            {
                vm.SeriesList.Add(new OscilChannel(vm.SeriesList[index]));
                vm.RaiseSeriesListChangedEvent(vm.SeriesList, null);
            });
        }

        protected override void OnDetaching()
        {

            AssociatedObject.copyFromThisEvent -= copy;
            AssociatedObject.deleteThisEvent -= delete;
        }
    }

}
