using GEC_LAB._04_Class;
using GEC_LAB.Properties;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace GEC_LAB._03_UserControl
{
    internal class PageStep01ViewModel:BindableBase
    {
        #region 变量绑定

        public string Name
        {
            get { return ProjectHelper.Name; }
            set { ProjectHelper.Name = value; RaisePropertyChanged(); }
        }


        public string Target
        {
            get { return ProjectHelper.Target; }
            set { ProjectHelper.Target = value; RaisePropertyChanged();  }
        }

        private BitmapSource? uploadImg;
        public BitmapSource? UploadImg
        {
            get => uploadImg;
            set { uploadImg = value; RaisePropertyChanged(); }
        }


        public DelegateCommand ImgUploadCommand {  get; private set; }
        #endregion
        #region 函数
        public PageStep01ViewModel()
        {
            ImgUploadCommand = new DelegateCommand(ImgUpload);
            ProjectHelper.OnReload += () => { loadData(); };
            UploadImg = ImgHelper.BitmapToBitmapImage(Resources.openImg);
            loadData();
        }
        public void ImgUpload() {
            OpenFileDialog fileSelector = new OpenFileDialog();
            fileSelector.InitialDirectory = null;
            fileSelector.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            fileSelector.RestoreDirectory = true;
            fileSelector.Title = "选择图片";
            if (true == fileSelector.ShowDialog())
            {
                ProjectHelper.PushImg(CommonUtils.readFile(fileSelector.FileName));
            }
        }
        public void loadData()
        {
            Name = ProjectHelper.Name;
            Target = ProjectHelper.Target;
        }
        #endregion
    }
}
