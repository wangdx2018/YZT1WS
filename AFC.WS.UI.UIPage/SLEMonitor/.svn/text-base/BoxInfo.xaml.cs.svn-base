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
    public partial class BoxInfo : UserControl
    {
        public BoxInfo()
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


        public BoxInfo(string totalStatus, string setupStatus, string numStatus, string num):this()
        {
        
            this.txtTicketAllState.Text = totalStatus;
            this.txtTicketInstallState.Text = setupStatus;
            this.txtTicketNumState.Text = numStatus;
            this.txtTicketNum.Text = num;
        }

        public BoxInfo(string totalStatus, string setupStatus, string numStatus, string num,string cashCode,string totalValue,Visibility visiable)
            : this(totalStatus,setupStatus,numStatus,num)
        {

            this.labCashCode.Visibility = Visibility.Hidden;
            this.labTotalValue.Visibility = visiable;
            this.txtcashCode.Visibility = Visibility.Hidden;
            this.txtTotalValue.Visibility = visiable;
            this.txtcashCode.Text = cashCode;
            this.txtTotalValue.Text = totalValue;
        }

        
        


    }
}
