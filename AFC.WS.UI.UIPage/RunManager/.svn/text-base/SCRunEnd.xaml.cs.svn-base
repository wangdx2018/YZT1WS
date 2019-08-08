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
using AFC.BOM2.UIController;
using AFC.WS.Model.Const;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.ModelView.Actions.RunManager;
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR;

namespace AFC.WS.UI.UIPage.RunManager
{
    /// <summary>
    /// SCRunEnd.xaml 的交互逻辑
    /// </summary>
    public partial class SCRunEnd : UserControlBase
    {
        public SCRunEnd()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            base.InitControls();
            //todo:GetRunDate
            //todo:GetRunStatus
            //this.GridRunEndInfo.ItemsSource = BR.BuinessRule.GetInstace().rm.GetRunTaskList().DefaultView;
            //this.GridDevStatusInfo.ItemsSource = BR.BuinessRule.GetInstace().rm.GetAllDevRunStatus(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).DefaultView;
            this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
            this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();//.GetRunStatus();

        }

        /// <summary>
        /// 订阅定时更新表的异步消息
        /// </summary>
        public override void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.RunEnd, AsynMessageType.RunEnd,
                HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.DeviceRunEnd, AsynMessageType.DeviceRunEnd,
                HandleMode.Asyn, true);

        }

        public override void HandleAsynMessageForUI(AFC.BOM2.MessageDispacher.Message msg)
        {
            System.Data.DataTable dt = msg.Content as System.Data.DataTable;
            //服务器运营结束
            if (msg.MessageType == AsynMessageType.RunEnd)
            {
                this.GridRunEndInfo.ItemsSource = dt.DefaultView;
                AFC.BOM2.UIController.UserControlBase.DoEvents();
                index++;
                if (BuinessRule.GetInstace().rm.CheckHasRunEnd()) //30s超时
                {
                    BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                    MessageDialog.Show("运营结束成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
                    this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
                    index = 0;
                    return;
                }
                if (index == 6)
                {
                    BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                    MessageDialog.Show("运营结束失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                   
                    index = 0;
                    return;
                }
            }
            //设备运营结束
            if (msg.MessageType == AsynMessageType.DeviceRunEnd)
            {
                this.GridDevStatusInfo.ItemsSource = dt.DefaultView;
                index++;
                if (BuinessRule.GetInstace().rm.CheckHasDevEnd()) //30s超时
                {
                    BuinessRule.GetInstace().rm.AbortDevRunMonitor();
                    MessageDialog.Show("设备运营结束成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                   
                    index = 0;
                    return;
                }
                if (index == 6)
                {
                    BuinessRule.GetInstace().rm.AbortDevRunMonitor();
                    //部份运营结束成功
                    int totalNum = 0;
                    int deviceNum = 0;
                    if (BuinessRule.GetInstace().rm.CheckPartDevEnd(out deviceNum,out totalNum))
                    {
                        MessageDialog.Show(string.Format("车站设备共{0}台,其中运营结束成功{1}台!", totalNum, deviceNum), "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    }
                    else
                    {
                        MessageDialog.Show("设备运营结束失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    }
                   
                    index = 0;
                    return;
                }
            }
        }

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        public override void UnLoadControls()
        {
            MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.RunEnd,AsynMessageType.RunEnd);
            MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.DeviceRunEnd, AsynMessageType.DeviceRunEnd);
        }

        private int index = 0;

        private void ButtonScRefesh_Click(object sender, RoutedEventArgs e)
        {
            this.GridRunEndInfo.ItemsSource = BuinessRule.GetInstace().rm.GetRunTaskList(AsynMessageType.RunEnd).DefaultView;
        }

        private void ButtonDevRefesh_Click(object sender, RoutedEventArgs e)
        {
            this.GridDevStatusInfo.ItemsSource = BuinessRule.GetInstace().rm.GetAllDevRunStatus(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).DefaultView;
        }

        private void ButtonRunEnd_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            BuinessRule.GetInstace().rm.AbortDevRunMonitor();
            IAction action = new SCDevEndAction();
            if (action.CheckValid(null))
            {
                action.DoAction(null);
            }
        }

        private void ButtonScRunEnd_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            BuinessRule.GetInstace().rm.AbortRunMonitorThread();
            IAction action = new SCRunEndAction();
            if (action.CheckValid(null))
            {
                action.DoAction(null);
            }
        }
    }
}
