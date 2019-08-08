#region [       Copyright (C), 2009,  中软AFC, Token Shen.     ]
/************************************************************
  FileName: MessageDialog.xaml
  
  Author: 沈克涛    
 
  Version :  0.1   
 
  Date:20090915
 
  Description: 弹出对话框   
 
  Function List:  
 
  History: 
 
      <author>   <time>      <version >     <desc>
 
      沈克涛    2009/09/15     0.1         增加代码说明
 * ***********************************************************/
#endregion

#region [       Using namespaces       ]
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
using AFC.WS.UI.Components;
#endregion

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// MessageDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MessageDialog : Window
    {
        #region [       Declarations       ]
        /// <summary>
        /// 关闭时委托
        /// </summary>
        /// <param name="result"></param>
        public delegate void MessageBoxClosedDelegate(MessageBoxResult result);

        /// <summary>
        /// 关闭时调用
        /// </summary>
        public static event MessageBoxClosedDelegate OnMessageBoxClosed;

        /// <summary>
        /// 点击按钮得到的结果
        /// </summary>
        public static MessageBoxResult Result { get; set; }

        /// <summary>
        /// MessageDialog变量
        /// </summary>
        public static MessageDialog dialog = null;

        #endregion

        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageDialog()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(MessageDialog_KeyDown);
            this.btnYes.Focus();
        }

        #endregion

        #region [       Private methods       ]

        /// <summary>
        /// 添加按下回车键，则自动点击确定按钮
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MessageDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnYes_Click(sender, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// 点击取消按钮时发生
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            this.Close();
        }

        /// <summary>
        /// 点击确定按钮时发生
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (btnYes.Content.ToString().ToLower().Equals("是") == true)
            {
                //yes button
                Result = MessageBoxResult.Yes;
            }
            else
            {
                //ok button
                Result = MessageBoxResult.OK;
            }

            this.Close();
        }

        /// <summary>
        /// 点击取消按钮时发生
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 设置按钮数量
        /// </summary>
        /// <param name="buttons">buttons</param>
        /// <returns>MessageBoxButtons</returns>
        private static MessageBoxButtons DisplayButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.Ok:
                    dialog.btnCancel.Visibility = Visibility.Collapsed;
                    dialog.btnNo.Visibility = Visibility.Collapsed;
                    dialog.btnYes.Visibility = Visibility.Visible;
                    dialog.btnYes.Content = "确定";
                    return MessageBoxButtons.Ok;

                case MessageBoxButtons.OkCancel:
                    dialog.btnCancel.Visibility = Visibility.Visible;
                    dialog.btnNo.Visibility = Visibility.Collapsed;
                    dialog.btnYes.Visibility = Visibility.Visible;
                    dialog.btnYes.Content = "确定";
                    dialog.btnNo.Content = "取消";
                    return MessageBoxButtons.OkCancel;


                case MessageBoxButtons.YesNo:
                    dialog.btnCancel.Visibility = Visibility.Collapsed;
                    dialog.btnNo.Visibility = Visibility.Visible;
                    dialog.btnYes.Visibility = Visibility.Visible;
                    dialog.btnYes.Content = "是";
                    dialog.btnNo.Content = "否";
                    return MessageBoxButtons.YesNo;

                case MessageBoxButtons.YesNoCancel:
                    dialog.btnCancel.Visibility = Visibility.Visible;
                    dialog.btnNo.Visibility = Visibility.Visible;
                    dialog.btnYes.Visibility = Visibility.Visible;
                    dialog.btnYes.Content = "是";
                    dialog.btnNo.Content = "否";
                    dialog.btnCancel.Content = "取消";
                    return MessageBoxButtons.YesNoCancel;

                default:
                    return MessageBoxButtons.Ok;
            }
        }

        /// <summary>
        /// 设置消息对话框图片样式
        /// </summary>
        /// <param name="icon">图片样式</param>
        private static void DisplayIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    dialog.imgIcon.Source = new BitmapImage(new Uri(@"image\MessageDialog\error.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Information:
                    dialog.imgIcon.Source = new BitmapImage(new Uri(@"image\MessageDialog\info.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Question:
                    dialog.imgIcon.Source = new BitmapImage(new Uri(@"image\MessageDialog\question.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.Warning:
                    dialog.imgIcon.Source = new BitmapImage(new Uri(@"image\MessageDialog\warning.png", UriKind.Relative));
                    break;

                case MessageBoxIcon.None:
                    dialog.imgIcon.Source = new BitmapImage(new Uri(@"image\MessageDialog\info.png", UriKind.Relative));
                    break;
                default:
                    dialog.imgIcon.Source = new BitmapImage(new Uri(@"image\MessageDialog\info.png", UriKind.Relative));
                    break;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private static void MessageBoxChildWindow_Closed(object sender, EventArgs e)
        {
            if (OnMessageBoxClosed != null)
                OnMessageBoxClosed(Result);
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Window).DragMove();
        }

        #endregion

        #region [       Public methods       ]
        /// <summary>
        /// 显示消息对话框,自行全部信息指定全部信息。
        /// </summary>
        /// <param name="icon">显示的图标</param>
        /// <param name="text">信息内容</param>
        /// <param name="caption">对话框标题</param>
        /// <param name="buttonTypes">按钮类别数组</param>
        /// <param name="buttonLabels">按钮文字</param>
        /// <param name="defaultButton">缺省按钮</param>
        /// <returns>用户点击的按钮类型</returns>
        public static MessageBoxResult Show(string text, string caption, string[] buttonLabels, MessageBoxIcon icon, MessageBoxButtons buttonTypes)
        {
            dialog = new MessageDialog();

            dialog.Closed += new EventHandler(MessageBoxChildWindow_Closed);
            dialog.Title = caption;
            dialog.txtMsg.Text = text;
            dialog.Topmost = true;
            DisplayButtons(buttonTypes);
            DisplayIcon(icon);
            if (buttonLabels.Length != 0)
            {
                for (int i = 0; i < buttonLabels.Length; i++)
                {
                    if (buttonTypes == MessageBoxButtons.OkCancel)
                    {
                        if (i == 0)
                            dialog.btnYes.Content = buttonLabels[i];
                        else if (i == 1)
                            dialog.btnCancel.Content = buttonLabels[i];
                    }
                    if (buttonTypes == MessageBoxButtons.YesNo)
                    {
                        if (i == 0)
                            dialog.btnYes.Content = buttonLabels[i];
                        else if (i == 1)
                            dialog.btnNo.Content = buttonLabels[i];
                    }
                    if (buttonTypes == MessageBoxButtons.Ok)
                    {
                        if (i == 0)
                            dialog.btnYes.Content = buttonLabels[i];
                    }
                    if (buttonTypes == MessageBoxButtons.YesNoCancel)
                    {
                        if (i == 0)
                            dialog.btnYes.Content = buttonLabels[i];
                        else if (i == 1)
                            dialog.btnNo.Content = buttonLabels[i];
                        else if (i == 2)
                            dialog.btnCancel.Content = buttonLabels[i];
                    }
                }
            }
            dialog.ShowDialog();
            return Result;

        }

        /// <summary>
        /// 显示信息提示对话框。
        /// </summary>
        /// <param name="text">信息内容</param>
        /// <returns>用户点击的按钮类型</returns>
        public static MessageBoxResult Show(string text)
        {
            dialog = new MessageDialog();

            dialog.Title = "确认信息";
            dialog.txtMsg.Text = text;
            dialog.Topmost = true;
            DisplayButtons(MessageBoxButtons.OkCancel);
            DisplayIcon(MessageBoxIcon.Information);
            dialog.ShowDialog();
            return Result;

        }

        /// <summary>
        /// 显示信息提示对话框。
        /// </summary>
        /// <param name="owner">父窗口</param>
        /// <param name="text">信息内容</param>
        /// <param name="caption">对话框标题</param>
        /// <returns>用户点击的按钮类型</returns>
        public static MessageBoxResult Show(string text, string caption)
        {
            dialog = new MessageDialog();

            dialog.Title = caption;
            dialog.txtMsg.Text = text;
            dialog.Topmost = true;
            DisplayButtons(MessageBoxButtons.OkCancel);
            DisplayIcon(MessageBoxIcon.Information);
            dialog.ShowDialog();
            return Result;

        }

        /// <summary>
        /// 根据图标类型显示对话框，按钮按照图标的类型设定。
        /// </summary>
        /// <param name="icon">图标类型</param>
        /// <param name="text">信息内容</param>
        /// <param name="caption">对话框标题</param>
        /// <returns>用户点击的按钮类型</returns>
        public static MessageBoxResult Show(string text, string caption, MessageBoxIcon icon)
        {
            dialog = new MessageDialog();
            dialog.Title = caption;
            dialog.txtMsg.Text = text;
            dialog.Topmost = true;
            DisplayButtons(MessageBoxButtons.OkCancel);
            DisplayIcon(icon);
            dialog.ShowDialog();
            return Result;

        }

        /// <summary>
        /// 根据图标类型和按钮类型显示对话框。
        /// </summary>
        /// <param name="icon">图标类型</param>
        /// <param name="text">信息内容</param>
        /// <param name="caption">对话框标题</param>
        /// <param name="buttons">按钮类型，使用Windows预定义类型。</param>
        /// <returns></returns>
        public static MessageBoxResult Show(string text, string caption, MessageBoxIcon icon,
                                        MessageBoxButtons buttons)
        {
            dialog = new MessageDialog();

            dialog.Title = caption;
            dialog.txtMsg.Text = text;
            dialog.Topmost = true;
            DisplayButtons(buttons);
            DisplayIcon(icon);
            dialog.ShowDialog();
            return Result;

        }

        /// <summary>
        /// 根据图标类型和按钮类型显示对话框。
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="msg">提示信息</param>
        /// <param name="icon">图标类型</param>
        /// <param name="btn">按钮类型，使用Windows预定义类型</param>
        /// <param name="keys">传过来的参数</param>
        /// <returns></returns>
        public static MessageBoxResult ShowDialog(string title, string msg, MessageBoxIcon icon,
                                        MessageBoxButtons btn,params object [] keys)
        {
            for (int i=0 ; keys != null && i < keys.Length ; i ++)    
            {
                if (keys[i] != null && keys[i] is String )
                {
                    keys[i] = UIHelper.GetResouceValueByID(keys[i]);
                }
            }
            string message = UIHelper.GetResouceValueByID(msg);
            message = String.Format(message, keys);
            string caption = UIHelper.GetResouceValueByID(title);

            dialog = new MessageDialog();

            dialog.Title = caption;
            dialog.txtMsg.Text = message;
            dialog.Topmost = true;
            DisplayButtons(btn);
            DisplayIcon(icon);
            dialog.ShowDialog();
            return Result;
 
        }


      

      
        #endregion
    }
    /// <summary>
    /// 提示按钮
    /// </summary>
    public enum MessageBoxButtons
    {
        /// <summary>
        /// 确定
        /// </summary>
        Ok,
        /// <summary>
        /// 确定取消
        /// </summary>
        YesNo,
        /// <summary>
        /// 确定取消
        /// </summary>
        YesNoCancel,
        /// <summary>
        /// 确定取消
        /// </summary>
        OkCancel
    }

    /// <summary>
    /// 提示图片
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// 提示信息。
        /// </summary>
        Information = 0,
        /// <summary>
        /// 需要用户确认。
        /// </summary>
        Question = 1,
        /// <summary>
        /// 警告信息。
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 错误信息。
        /// </summary>
        Error = 3,
        /// <summary>
        /// 无
        /// </summary>
        None = 4
    }
}
