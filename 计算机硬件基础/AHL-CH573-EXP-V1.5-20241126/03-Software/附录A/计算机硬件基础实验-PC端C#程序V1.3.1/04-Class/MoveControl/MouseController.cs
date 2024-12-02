using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GEC_LAB._04_Class.MoveControl
{
    class MouseController
    {
        private readonly Control c;
        private Point delta;
        private bool _isStartMove = false;
        private bool _isRightPessed=false;
        private bool _isMoved = false;
        private Control? attach;
        public static Control? currentShowAttach;
        public MouseController(Control c)
        {
            this.c = c;
            init();
        }
        public MouseController(Control c, Control attach)
        {
            this.c = c;
            this.attach = attach;
            initAttach();
            init();
        }

        public void init()
        {
            c.ContextMenu = new ContextMenu();
            MenuItem menu = new MenuItem() { Header = "删除" };
            menu.Click += Delete;
            c.ContextMenu.Items.Add(menu);
            if(attach!=null) attach.MouseDown += (s, e) => { e.Handled=true; };
        }

        public void setMovable()
        {
            c.MouseDown += MouseDown;
            c.MouseMove += MouseMove;
            c.MouseUp += MouseUp;
        }
        public void setAttach(Control attach) {
            this.attach = attach;
            initAttach();
        }
        private void initAttach()
        {
            if(attach!=null)
            attach.MouseDown += (s, e) => e.Handled = true;
        }
        public void removeSelf()
        {
            Panel p = (Panel)c.Parent;
            CancelAttachShow();
            p.Children.Remove(c);
            ProjectHelper.Saved = false;
        }
        public void topping()
        {
            if (Canvas.GetZIndex(c) != Gobals.canvasZIndex - 1)
            {
                Canvas.SetZIndex(c, Gobals.canvasZIndex++);
                ProjectHelper.Saved = false;
            }
        }
        #region 内部函数
        private void Delete(object sender, RoutedEventArgs e)
        {
            removeSelf();
        }


        public static void CancelAttachShow()
        {
            if (currentShowAttach != null&& currentShowAttach.Parent!=null) {
                ((Panel)currentShowAttach.Parent).Children.Remove(currentShowAttach);
            }
            currentShowAttach = null;
        }
        public void showAttach()
        {
            CancelAttachShow();
            if (attach != null)
            {
                Canvas.SetLeft(attach, Canvas.GetLeft(c) + c.Width);
                Canvas.SetTop(attach, Canvas.GetTop(c));
                Canvas.SetZIndex(attach, Canvas.GetZIndex(c));
                currentShowAttach = attach;
                if (!((Panel)c.Parent).Children.Contains(attach)) ((Panel)c.Parent).Children.Add(attach);
            }
        }
        
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            topping();
            if (attach != null) Canvas.SetZIndex(attach, Canvas.GetZIndex(c));

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                c.CaptureMouse();
                Point point = e.GetPosition(null);
                Point controlPosition = new Point(Canvas.GetLeft(c), Canvas.GetTop(c));
                delta = (Point)(controlPosition - point);
                if (delta.X < 0)
                {
                    _isStartMove = true;
                    _isMoved = false;
                }
                e.Handled = true;
            }
            if (e.RightButton == MouseButtonState.Pressed) _isRightPessed = true;
        }
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (_isStartMove && e.LeftButton == MouseButtonState.Pressed)
            {
                CancelAttachShow();
                Point p = e.GetPosition(null);
                Canvas.SetLeft(c, Math.Max(0, delta.X + p.X));
                Canvas.SetTop(c, Math.Max(0, delta.Y + p.Y));
                _isMoved = true;
                ProjectHelper.Saved = false;
            }
            e.Handled = true;
        }
        private void MouseUp(object sender, MouseEventArgs e)
        {
            c.ReleaseMouseCapture();
            _isStartMove= false;
            if (_isMoved || (currentShowAttach != null && attach != currentShowAttach)) CancelAttachShow();
            else if (!_isMoved  && !_isRightPessed)
            {
                showAttach();
            }
            _isRightPessed = false;
            delta.X = delta.Y = 0;
        }
        #endregion
    }
}
