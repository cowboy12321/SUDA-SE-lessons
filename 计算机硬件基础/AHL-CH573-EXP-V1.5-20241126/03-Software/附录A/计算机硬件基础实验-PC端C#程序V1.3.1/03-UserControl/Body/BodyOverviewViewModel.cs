using GEC_LAB._04_Class;
using GEC_LAB.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GEC_LAB._03_UserControl.Body
{
    class BodyOverviewViewModel:BindableBase
    {
        
		public string ExpName
		{
			get { return ProjectHelper.Name; }
			set { RaisePropertyChanged(); }
		}

		public string ExpTarget
		{
			get { return ProjectHelper.Target; }
			set { RaisePropertyChanged(); }
		}

        private ObservableCollection<BitmapSource> imgList = new ObservableCollection<BitmapSource>();
        public ObservableCollection<BitmapSource> ImgList
        {
            get { return imgList; }
            private set { imgList = value; RaisePropertyChanged(); }
        }
        private Visibility emptyImgVisible = Visibility.Visible;

        public Visibility EmptyImgVisible
        {
            get { return emptyImgVisible; }
            set { emptyImgVisible = value;RaisePropertyChanged(); }
        }

        private int selectedIndex=0;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }


        public DelegateCommand ImgDeleteCommand { get; private set; }
        public readonly string LOG = "BodyOverviewViewModel";
        private void init()
        {
            EmptyImgVisible = Visibility.Visible;
            imgList.Clear();
            BitmapSource? fail = ImgHelper.BitmapToBitmapImage(Resources.imgOpenFail);
            if (fail != null)
            {
                foreach (var img in ProjectHelper.ImgList)
                {
                    EmptyImgVisible = Visibility.Hidden;
                    BitmapSource? bitmapSource = ImgHelper.byteToBitmapSource(img);
                    if (bitmapSource != null) imgList.Add(bitmapSource);
                    else imgList.Add(fail);
                }
            }
        }
        public BodyOverviewViewModel()
        {
            Gobals.logger?.info(LOG,"init");
            init();
            ProjectHelper.BasicInfoChanged += () =>
			{
				ExpName = ExpTarget="";
			};
			ProjectHelper.OnReload += () =>
			{
				ExpName = ExpTarget = "";
                init();

            };

            ProjectHelper.ImgAddEvent += () =>
            {
                BitmapSource? fail = ImgHelper.BitmapToBitmapImage(Resources.imgOpenFail);
                if (fail != null)
                {
                    EmptyImgVisible = Visibility.Hidden;
                    BitmapSource? bitmapSource = ImgHelper.byteToBitmapSource(ProjectHelper.ImgList[ProjectHelper.ImgList.Count - 1]);
                    if (bitmapSource != null) imgList.Add(bitmapSource);
                    else imgList.Add(fail);
                }

            };


            ImgDeleteCommand = new DelegateCommand(() => { 
                ProjectHelper.EraseImg(SelectedIndex);
                imgList.RemoveAt(SelectedIndex);
                if(imgList.Count == 0)
                {
                    EmptyImgVisible = Visibility.Visible;
                }
            });
            Gobals.logger?.info(LOG, "init done");
        }

    }
}
