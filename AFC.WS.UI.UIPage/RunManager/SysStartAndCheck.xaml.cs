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

namespace AFC.WS.UI.UIPage.RunManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.BR.SysStart;
    using System.Threading;
    using System.Data;


    /// <summary>
    /// SysStartAndCheck1.xaml 的交互逻辑
    /// 
    /// edited by wangdx 20120316
    /// 
    ///在自检的时候启动水晶报表
    /// </summary>
    public partial class SysStartAndCheck : UserControlBase
    {

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public SysStartAndCheck()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            this.Loaded += new RoutedEventHandler(SysStartAndCheck_Loaded);
            Thread thread = new Thread(new ThreadStart(() =>
            {
                //AFC.WS.UI.UIPage.CashManager.CrystalRptData cryRptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();
                //cryRptData.ShowRptDialog(new AFC.WS.UI.UIPage.TicketBoxManager.CrystalTicketBoxInOutReport(),
                //    new Dictionary<string, string>(),
                //    new DataTable());
                //cryRptData.myForm.Close();
            }));
            
          thread.Start();
            
        }

        void SysStartAndCheck_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
            //throw new NotImplementedException();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            SysCheck();
        }


        public void SysCheck()
        {
            timer.Stop();

            int res = 0;

            SetLabelContent(this.labSCConnectionContent, "正在连接服务器......");

            System.Windows.Forms.Application.DoEvents();
            res = BuinessRule.GetInstace().commProcess.Init() ? 0 : 1;

            if (res == 0)
            {
                BuinessRule.GetInstace().brConext.NetworkStatus = (res == 0);//set net work connect
                AFC.WS.UI.Common.WriteLog.Log_Info("Socket conenct done");
                res = BuinessRule.GetInstace().commProcess.CheckIn();
                BuinessRule.GetInstace().brConext.OnlineStatus = (res == 0);//set net work connect
            }

            //todo:CheckIn

            System.Threading.Thread.Sleep(500);
            this.SetStatusLabel(res, this.labSCConnectionResult);


            SetLabelContent(this.labDBConnectionContent, "正在连接数据库......");
            System.Windows.Forms.Application.DoEvents();
            res = BuinessRule.GetInstace().ConnectDB(AFC.WS.UI.Common.SysConfig.GetSysConfig().LocalParamsConfig.DBConnectionString) ? 0 : 1;
            //todo :connection to d
            BuinessRule.GetInstace().brConext.DbOnLineStatus = (res == 0);
            this.SetStatusLabel(res, this.labDBConnectionResult);

            SetLabelContent(this.labTimeSynContent, "正在时钟同步......");

            System.Windows.Forms.Application.DoEvents();
            res = 0;
          //   res = BuinessRule.GetInstace().tsm.TimeSyn();
           // todo: syn time
            this.SetStatusLabel(res, this.labTimeSynResult);

            SetLabelContent(this.labFTPConnectionContent, "正在连接FTP服务器......");

            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(500);
          //  todo:connect ftp server

            this.SetStatusLabel(0, this.labFTPConnectionResult);

            SetLabelContent(this.labSoftwareUpdate, "软件更新中......");

            System.Windows.Forms.Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            //todo:update software
            SetLabelContent(this.labSoftwareUpdate,"正在准备更新软件,请等候........");
            if (BuinessRule.GetInstace().brConext.NetworkStatus)
            {
                SoftAndParaUpdate softParaUpdate = new SoftAndParaUpdate();
                res = softParaUpdate.SoftWareUpdate();
            }
            else
                res = -1;
            this.SetStatusLabel(res, this.labSoftwareUpdateResult);

            AFC.WS.ModelView.UIContext.UIOperation operation = new AFC.WS.ModelView.UIContext.UIOperation();
            operation.SwitchUI("Login");
        }


        //--->设置文本的文字
        /// <summary>
        /// 设置文本的文字
        /// </summary>
        /// <param name="result"></param>
        /// <param name="label"></param>
        private void SetStatusLabel(int result, System.Windows.Controls.Label label)
        {
            if (result == 0)
                label.Content = "成功";
            else
            {
                label.Content = "失败";
                label.Foreground = Brushes.Red as Brush;
            }
            // System.Windows.Forms.Application.DoEvents();
        }

        private void SetLabelContent(Label label, string value)
        {
            label.Content = value;
        }
    }
}
