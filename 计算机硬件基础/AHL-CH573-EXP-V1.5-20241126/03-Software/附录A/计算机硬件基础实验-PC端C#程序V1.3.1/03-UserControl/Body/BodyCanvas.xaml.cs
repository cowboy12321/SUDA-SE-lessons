using GEC_LAB._04_Class;
using GEC_LAB._04_Class.Interface;
using GEC_LAB._04_Class.Models;
using GEC_LAB._04_Class.MoveControl;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GEC_LAB._03_UserControl.body
{
    /// <summary>
    /// BodyCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class BodyCanvas : UserControl
    {
        public BodyCanvas()
        {
            InitializeComponent();
            init();
        }
        public void init()
        {
            bodyCanvas.AllowDrop = true;
            bodyCanvas.Drop += BodyCanvas_Drop;
            bodyCanvas.MouseDown += (s, e) => MouseController.CancelAttachShow();
            ProjectHelper.OnReload += () => restoreComponent(ProjectHelper.Components);
            ProjectHelper.BeforeSave +=storeComponent;
            restoreComponent(ProjectHelper.Components);
        }
        private void BodyCanvas_Drop(object sender, DragEventArgs e)
        {
            ComponentItemModel m = (ComponentItemModel)e.Data.GetData(typeof(ComponentItemModel));
            if (m != null && m.type != null)
            {
                Point point = e.GetPosition(bodyCanvas);
                object? o = Activator.CreateInstance(m.type);
                if (o is Control)
                {
                    Control c = (Control)o;
                    Canvas.SetTop(c, point.Y);
                    Canvas.SetLeft(c, point.X);
                    Canvas.SetZIndex(c, Gobals.canvasZIndex++);
                    bodyCanvas.Children.Add(c);
                    ProjectHelper.Saved = false;
                }
            }
        }
        public void storeComponent()
        {
            ProjectHelper.ClearSavedComponent();
            foreach (var item in bodyCanvas.Children)
            {
                if (item is ISavedComponent)
                {
                    ISavedComponent s = (ISavedComponent)item;
                    SavedComponent sc = s.save();
                    if (item is Control)
                    {
                        Control c = (Control)item;
                        sc.ZIndex = Canvas.GetZIndex(c);
                        sc.left = Canvas.GetLeft(c);
                        sc.top = Canvas.GetTop(c);
                    }
                    ProjectHelper.AddSavedComponent(sc);
                }
            }
        }
        public void restoreComponent(ReadOnlyCollection<SavedComponent> components)
        {

            DataRefreshCenter.Instance.ComponentTickEvent -= DataRefreshCenter.Instance.ComponentTickEvent;
            bodyCanvas.Children.Clear();
            foreach (var com in components)
            {
                object? v = null;
                if (com.type != null) v = Activator.CreateInstance(com.type);
                if (v != null && v is ISavedComponent)
                {
                    ISavedComponent s = (ISavedComponent)v;
                    s.restore(com.data);
                }
                if (v != null && v is Control)
                {
                    Control c = (Control)v;
                    Canvas.SetLeft(c, com.left);
                    Canvas.SetTop(c, com.top);
                    Canvas.SetZIndex(c, com.ZIndex);
                    if (Gobals.canvasZIndex <= com.ZIndex) Gobals.canvasZIndex = com.ZIndex + 1;
                    bodyCanvas.Children.Add(c);
                }
            }
        }
    }
}
