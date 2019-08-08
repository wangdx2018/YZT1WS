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
    /// SCRunStart.xaml 的交互逻辑
    /// </summary>
    
    public partial class SCRunStart : UserControlBase
    {
        private int index = 0;

        public SCRunStart()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            base.InitControls();
            //todo:GetRunDate
            //todo:GetRunStatus
            //this.GridRunBeginInfo.ItemsSource = BR.BuinessRule.GetInstace().rm.GetRunTaskList().DefaultView;
            //this.GridDevStatusInfo.ItemsSource = BR.BuinessRule.GetInstace().rm.GetAllDevRunStatus(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).DefaultView;
            this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
            this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
            this.scRunEnd.InitControls();
        }

        /// <summary>
        /// 订阅定时更新表的异步消息
        /// </summary>
        public override void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.RunStart, AsynMessageType.RunStart,
                HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.DeviceRunStart, AsynMessageType.DeviceRunStart,
                HandleMode.Asyn, true);
            this.scRunEnd.SubscribeAsynMessage();
        }

        public override void HandleAsynMessageForUI(AFC.BOM2.MessageDispacher.Message msg)
        {
            System.Data.DataTable dt = msg.Content as System.Data.DataTable;
            //服务器运营开始
            if (msg.MessageType ==AsynMessageType.RunStart)
            {
                System.Windows.Forms.Application.DoEvents();
                this.GridRunBeginInfo.ItemsSource = dt.DefaultView;
                index++;
                if (BuinessRule.GetInstace().rm.CheckHasRunStart()) //30s超时
                {
                    BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                    if (this.cbDevWakeUp.IsChecked.Value)
                    {
                        List<TJComm.DeviceRange> list = new List<TJComm.DeviceRange>();
                        list.Add(new TJComm.DeviceRange { stationId = SysConfig.GetSysConfig().LocalParamsConfig.StationCode.ConvertHexStringToUshort(), special_flag = 1, deviceRange = new List<uint>() });
                        BuinessRule.GetInstace().commProcess.ControlCmd(0x01, ControlCode.REMOTE_WEAK_UP, list);
                    }
                    MessageDialog.Show("运营开始成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    this.txtRunDate.Text = BuinessRule.GetInstace().rm.GetRunDate();
                    this.txtRunStauts.Text = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
                    index = 0;
                    return;
                }
                if (index == 8)
                {
                    BuinessRule.GetInstace().rm.AbortRunMonitorThread();
                    if (this.cbDevWakeUp.IsChecked.Value)
                    {
                        List<TJComm.DeviceRange> list = new List<TJComm.DeviceRange>();
                        list.Add(new TJComm.DeviceRange { stationId = SysConfig.GetSysConfig().LocalParamsConfig.StationCode.ConvertHexStringToUshort(), special_flag = 1, deviceRange = new List<uint>() });
                        BuinessRule.GetInstace().commProcess.ControlCmd(0x01, ControlCode.REMOTE_WEAK_UP, list);
                    }
                    MessageDialog.Show("运营开始失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                  
                    index = 0;
                    return;
                }

        

            }
            //设备运营开始
            if(msg.MessageType ==AsynMessageType.DeviceRunStart)
            {
                this.GridDevStatusInfo.ItemsSource = dt.DefaultView;
                index++;
                if (BuinessRule.GetInstace().rm.CheckHasDevStart()) //30s超时
                {
                    BuinessRule.GetInstace().rm.AbortDevRunMonitor();
                    MessageDialog.Show("设备运营开始成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);

              
                    index = 0;
                    return;
                }
                if (index == 6)
                {
                    BuinessRule.GetInstace().rm.AbortDevRunMonitor();
                    int totalNum = 0;
                    int deviceNum = 0;
                    if (BuinessRule.GetInstace().rm.CheckPartDevStart(out deviceNum,out totalNum))
                    {
                        MessageDialog.Show(string.Format("车站设备共{0}台,其中运营开始成功{1}台!",totalNum,deviceNum), "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    }
                    else
                    {
                        MessageDialog.Show("设备运营开始失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
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
            MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.RunStart, AsynMessageType.RunStart);
            MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.RunStart, AsynMessageType.DeviceRunStart);

            this.scRunEnd.UnLoadControls();
        }

      

        private void ButtonScRunStart_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
         
           BuinessRule.GetInstace().rm.AbortRunMonitorThread();
            IAction action = new SCRunStartAction();
            if (action.CheckValid(null))
            {
                action.DoAction(null);
            }
        }

        private void ButtonScRefesh_Click(object sender, RoutedEventArgs e)
        {
            this.GridRunBeginInfo.ItemsSource = BuinessRule.GetInstace().rm.GetRunTaskList(AsynMessageType.RunStart).DefaultView;
        }

        private void ButtonDevRunStart_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            BuinessRule.GetInstace().rm.AbortDevRunMonitor();
            IAction action = new SCDevStartAction();
            if (action.CheckValid(null))
            {
                action.DoAction(null);
            }

        }

        private void ButtonDevRefesh_Click(object sender, RoutedEventArgs e)
        {
            this.GridDevStatusInfo.ItemsSource = BuinessRule.GetInstace().rm.GetAllDevRunStatus(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).DefaultView;
        }

    }
}
