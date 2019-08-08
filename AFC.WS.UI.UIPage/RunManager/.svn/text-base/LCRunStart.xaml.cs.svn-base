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
    /// added by wangdx 20100511
    /// </summary>
    public partial class LCRunStart : UserControlBase
    {
        public LCRunStart()
        {
            InitializeComponent();
          
        }

        public override void InitControls()
        {
            base.InitControls();
            //todo:GetRunDate
            //todo:GetRunStatus
            this.GridRunBeginInfo.ItemsSource = BuinessRule.GetInstace().rm.GetRunTaskList(AsynMessageType.RunStart).DefaultView;
            this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
            this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
        
        }

        /// <summary>
        /// 订阅定时更新表的异步消息
        /// </summary>
        public override void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.RunStart, AsynMessageType.RunStart,
                HandleMode.Asyn, true);
        }

        public override void HandleAsynMessageForUI(AFC.BOM2.MessageDispacher.Message msg)
        {
           System.Data.DataTable dt=msg.Content as System.Data.DataTable;
           this.GridRunBeginInfo.ItemsSource = dt.DefaultView;
           index++;
           UserControlBase.DoEvents();
           if (BuinessRule.GetInstace().rm.CheckHasRunStart()) //30s超时
           {
               BuinessRule.GetInstace().rm.AbortRunMonitorThread();
               MessageDialog.Show("发送运营开始成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
               this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
               return;
           }
           if (index == 12)
           {
               BuinessRule.GetInstace().rm.AbortRunMonitorThread();
               MessageDialog.Show("运营开始失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
         
               return;
           }
        }

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        public override void UnLoadControls()
        {
            MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.RunStart, AsynMessageType.RunStart);
        }

        private int index = 0;

        private void ButtonScRunStart_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            BuinessRule.GetInstace().rm.AbortRunMonitorThread();
            IAction action = new LCRunStartAction();
            if (action.CheckValid(null))
            {
                action.DoAction(null);
            }
        }

        private void ButtonScRefesh_Click(object sender, RoutedEventArgs e)
        {
            this.GridRunBeginInfo.ItemsSource = BuinessRule.GetInstace().rm.GetRunTaskList(AsynMessageType.RunStart).DefaultView;
        }

     
    }
}
