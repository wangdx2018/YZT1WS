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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AFC.WS.UI.UIPage.SLEMonitor
{
    /// <summary>
    /// TickBoxInfo.xaml 的交互逻辑
    /// </summary>
    public partial class HopperInfo : UserControl
    {
        public HopperInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置票箱的文本
        /// </summary>
        /// <param name="header">文本内容</param>
        public string Header
        {
            set { this.header.Header = value; }
            get { return this.header.Header.ToString(); }
        }


        public HopperInfo(string txtTicketNumStatus, string txtTicketCurrentNum)
            : this()
        {

            this.txtTicketNumStatus.Text = txtTicketNumStatus;
            this.txtTicketCurrentNum.Text = txtTicketCurrentNum;
        }
    }
}
