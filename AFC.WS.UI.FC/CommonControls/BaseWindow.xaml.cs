using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.ComponentModel;


namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// BaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BaseWindow : Window
    {
        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseWindow()
        {
            try
            {
                InitializeStyle();
                IsHideMinimize = true;
                IsHideMaximum = true;
                IsHideClose = false;
                //ImagePath = "/Image/afc.ico";
                this.Loaded += delegate
                {
                    InitializeEvent();
                };

                this.KeyUp += new KeyEventHandler(BaseWindow_KeyUp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 當用戶的按鍵是 "Esc"是，則關閉當前窗體。
        /// </summary>
        /// <param name="sender">當前窗體</param>
        /// <param name="e">按鍵事件類</param>
        void BaseWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        #endregion

        #region [       Properties       ]
        /// <summary>
        /// 设置窗体是否显示最小化按钮
        /// </summary>
        private bool _isHideMinimize;
        /// <summary>
        /// 设置窗体是否显示最小化按钮
        /// </summary>
        [
        Description("设置窗体是否显示最小化按钮。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("BaseWindow"),
        ]
        public bool IsHideMinimize
        {
            get { return _isHideMinimize; }
            set { _isHideMinimize = value; }
        }

        /// <summary>
        /// 设置窗体是否显示最大化按钮
        /// </summary>
        private bool _isHideMaximum;
        /// <summary>
        /// 设置窗体是否显示最大化按钮
        /// </summary>
        [
        Description("设置窗体是否显示最大化按钮。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("BaseWindow"),
        ]
        public bool IsHideMaximum
        {
            get { return _isHideMaximum; }
            set { _isHideMaximum = value; }
        }
        /// <summary>
        /// 设置窗体是否显示关闭按钮
        /// </summary>
        private bool _isHideClose;
        /// <summary>
        /// 设置窗体是否显示关闭按钮
        /// </summary>
        [
        Description("设置窗体是否显示关闭按钮。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("BaseWindow"),
        ]
        public bool IsHideClose
        {
            get { return _isHideClose; }
            set { _isHideClose = value; }
        }

        /// <summary>
        /// 设置窗体左上角图标路径
        /// </summary>
        private string _ImagePath;
        /// <summary>
        /// 设置窗体左上角图标路径
        /// </summary>
        [
        Description("设置窗体左上角图标路径。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("BaseWindow"),
        ]
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        #endregion

        #region [       Private methods       ]
        /// <summary>
        /// 初始化操作按钮事件，当选择最小化、最大化、关闭按钮时触发。
        /// </summary>
        private void InitializeEvent()
        {
            try
            {
                ControlTemplate baseWindowTemplate = (ControlTemplate)Application.Current.Resources["BaseWindowControlTemplate"];

                Button minBtn = (Button)baseWindowTemplate.FindName("Minimize", this);
                minBtn.Click += delegate
                {
                    this.WindowState = WindowState.Minimized;
                };
                if (IsHideMinimize)
                    minBtn.Visibility = Visibility.Collapsed;

                Button maxBtn = (Button)baseWindowTemplate.FindName("Maximize", this);

                maxBtn.Click += delegate
                {
                    this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
                };

                if (IsHideMaximum)
                    maxBtn.Visibility = Visibility.Collapsed;


                Button closeBtn = (Button)baseWindowTemplate.FindName("Exit", this);
                closeBtn.Click += delegate
                {
                    this.Close();
                };
                if (IsHideClose)
                    closeBtn.Visibility = Visibility.Collapsed;

                Border borderTitle = (Border)baseWindowTemplate.FindName("Chrome", this);
                borderTitle.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        this.DragMove();
                    }
                };
                if (this.ResizeMode != ResizeMode.NoResize)
                {
                    borderTitle.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
                    {
                        if (e.ClickCount >= 2)
                        {
                            maxBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        }
                    };
                }
                if (!string.IsNullOrEmpty(ImagePath))
                {
                    Image image = (Image)baseWindowTemplate.FindName("TitleImage", this);

                    image.Source = new BitmapImage(new Uri(ImagePath, UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 初始化样式
        /// </summary>
        private void InitializeStyle()
        {
            this.Style = (Style)Application.Current.Resources["BaseWindowStyle"];
        }

        #endregion
    }
}
