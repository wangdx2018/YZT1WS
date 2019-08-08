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
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.RunManager;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    /// <summary>
    /// LCRunEnd.xaml 的交互逻辑
    /// </summary>
    public partial class LCRunEnd : UserControlBase
    {

        private int index = 0;

        public LCRunEnd()
        {
            InitializeComponent();
            this.btnRunEnd.Click += new RoutedEventHandler(btnRunEnd_Click);
            this.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            this.GridRunBeginInfo.ItemsSource = BuinessRule.GetInstace().rm.GetRunTaskList(AsynMessageType.RunEnd).DefaultView;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.GridRunBeginInfo.ItemsSource = BuinessRule.GetInstace().rm.GetRunTaskList(AsynMessageType.RunEnd).DefaultView;
        }

        
        private void btnRunEnd_Click(object sender, RoutedEventArgs e)
        {
           

            IAction action = new LCRunEndAction();
            if (action.CheckValid(null))
            {
                index = 0;
                BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                 action.DoAction(null);
            }
        }

        public override void InitControls()
        {
            base.InitControls();
            this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
            this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
        }

        public override void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.RunEnd, AsynMessageType.RunEnd, HandleMode.Asyn, true);
            //base.SubscribeAsynMessage();
        }

        public override void UnLoadControls()
        {
            MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.RunEnd,AsynMessageType.RunEnd);
            //base.UnLoadControls();
        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            System.Data.DataTable dt = msg.Content as System.Data.DataTable;
            this.GridRunBeginInfo.ItemsSource = dt.DefaultView;
            System.Windows.Forms.Application.DoEvents();
            index++;
            if (BuinessRule.GetInstace().rm.CheckHasRunEnd()) //30s超时
            {
                BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                MessageDialog.Show("运营结束已成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
                this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
                return;
            }
            if (index == 6) //todo:time out handle
            {
                BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                MessageDialog.Show("运营结束失败，请查看执行失败的任务!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
               
                return;
            }
        }
    }
}
