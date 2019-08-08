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
using AFC.WS.UI.Config;
#endregion

namespace AFC.WS.UI.CommonControls
{
   
    /// <summary>
    /// DoublePermissions.xaml 的交互逻辑
    /// </summary>
    public partial class DoublePermissions : Window
    {
        #region [       Declarations       ]
        /// <summary>
        /// 关闭时委托
        /// </summary>
        /// <param name="result"></param>
        public delegate void OnDoublePermissionsClosedDelegate(string operId,string operPwd,string otherOperId,string otherOperPwd );

        /// <summary>
        /// 关闭时调用
        /// </summary>
        public event OnDoublePermissionsClosedDelegate OnDoublePermissionsClosed;
        #endregion

      
        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        private DoublePermissions()
        {
          
        }

        public DoublePermissions(string logonOperatorID)
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(DoublePermissions_KeyDown);
            this.txtCurrentUserID.Text = logonOperatorID;
            this.txtCurrentUserID.IsEnabled = false;
            this.ButtonOk.Click += new RoutedEventHandler(ButtonOk_Click);
            this.ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
        }

        #endregion [       Constructor       ]


        #region [Event]

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.pwdCurrentUserPwd.TextPassword))
            {
                MessageDialog.Show("请输入第一个操作员的密码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (string.IsNullOrEmpty(this.txtOtherUserID.textBox.Text))
            {
                MessageDialog.Show("请输入第二个操作员ID", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (string.IsNullOrEmpty(this.pwdOtherUserPwd.TextPassword))
            {
                MessageDialog.Show("请输入第二个操作员的密码" ,"提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.txtCurrentUserID.Text.Equals(this.txtOtherUserID.Text))
            {
                MessageDialog.Show("不能输入相同的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.OnHandleDoublePrimission != null)
            {
                OnHandleDoublePrimission(this, new MyEventArgs
                {
                    firstId = this.txtCurrentUserID.Text,
                    firstPwd = this.pwdCurrentUserPwd.TextPassword,
                    secordId = this.txtOtherUserID.Text,
                    secordPwd = this.pwdOtherUserPwd.TextPassword
                });
            }
        }

        /// <summary>
        /// 点击回车时执行
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void DoublePermissions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               ButtonOk_Click(sender, new RoutedEventArgs());
            }
        }

        #endregion


        public delegate void HandleDoublePrimission(object sender, MyEventArgs e);


        public event HandleDoublePrimission OnHandleDoublePrimission;
     
    }

    public class MyEventArgs : EventArgs
    {
        public string firstId;

        public string firstPwd;

        public string secordId;

        public string secordPwd;


    }
}
