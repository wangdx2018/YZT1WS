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
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using System.Collections.ObjectModel;
using AFC.WS.UI.Components;

namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// UpsToDevice.xaml 的交互逻辑
    /// </summary>
    public partial class UpsToDevice : UserControlBase
    {
        public UpsToDevice()
        {
            InitializeComponent();
        }
        private string upsId;
        public override void InitControls()
        {

            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            System.Windows.Window window =
                list.Single(temp => temp.bindingData.Equals("window")).value as System.Windows.Window;
            upsId=list.Single(temp => temp.bindingData.Equals("t.ups_id")).value.ToString();
            window.Title = "为UPS" + upsId + "分配设备";
            if (list != null)
            {
                this.upsId = upsId;
            }
            if (!string.IsNullOrEmpty(upsId))
            {
                // this.roleToFun.SetGroupHeader("Ups设备关系");
                this.UpsToDev.SetGroupHeader("Ups设备关系");

                // this.roleToFun.SetCurrentLabel("当前设备列表");    
                this.UpsToDev.SetCurrentLabel("当前设备列表");

                //this.roleToFun.SetLeftLabel("可选的设备列表");
                this.UpsToDev.SetLeftLabel("可选的设备列表");

                this.UpsToDev.BindingCurrent(InitCurrenDeviceInfo(upsId));
                this.UpsToDev.BindingLeft(InitAllDeviceInfo(upsId));
                this.UpsToDev.OnOKButtonClicked += new RelactionContol.FunctionCliecked(OnOKButtonClicked);
            }
        }
        private void OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            MessageBoxResult res = AFC.WS.UI.CommonControls.MessageDialog.Show("是否调整UPS"+ upsId + "?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    AFC.WS.BR.DeviceMonitor.AgUpsManager upsManger = new AFC.WS.BR.DeviceMonitor.AgUpsManager();
                    upsManger.DeleteUpsToDeviceRelaction(upsId);
                    for (int i = 0; i < e.current.Count; i++)
                    {
                        upsManger.AddUpsToDeviceRelaction(upsId, e.current[i].ID);
                    }
                    upsManger.UpdateDateTimeInfo(upsId, list);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_allot, "0", "设备分配成功");
                    AFC.WS.UI.CommonControls.MessageDialog.Show("设备分配成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                catch (Exception ex)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_allot, "1", "设备分配失败");
                    AFC.WS.UI.CommonControls.MessageDialog.Show("设备分配失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
            }

        }

        public ObservableCollection<Data> InitAllDeviceInfo(string upsId)
        {
            AFC.WS.BR.DeviceMonitor.AgUpsManager upsManger = new AFC.WS.BR.DeviceMonitor.AgUpsManager();
            ObservableCollection<Data> current = InitCurrenDeviceInfo(upsId);
            List<BasiDevInfo> list = upsManger.GetAllNormalDeviceInfos();
            if (list != null)
            {
                for (int i = 0; i < current.Count; i++)
                {
                    if (BuinessRule.GetInstace().GetBasiDeviceIdInfos(current[i].ID).device_id == null)
                    {
                        continue;
                    }
                    BasiDevInfo info = list.Single(temp => temp.device_id.Equals(current[i].ID));
                    if (list.Contains(info))
                    {
                        list.Remove(info);
                    }
                }

                ObservableCollection<Data> leftData = new ObservableCollection<Data>();
                for (int i = 0; i < list.Count; i++)
                {
                    leftData.Add(new Data { ID = list[i].device_id, Text = list[i].device_id, IsChecked = false });
                }
                return leftData;
            }
            return new ObservableCollection<Data>();
        }

        public ObservableCollection<Data> InitCurrenDeviceInfo(string upsId)
        {


            AFC.WS.BR.DeviceMonitor.AgUpsManager upsManger = new AFC.WS.BR.DeviceMonitor.AgUpsManager();
            List<BasiDevInfo> DevList = upsManger.getDeviceByUpsID(upsId);
            ObservableCollection<Data> list = new ObservableCollection<Data>();
            if (DevList != null)
            {
                for (int i = 0; i < DevList.Count; i++)
                {
                    BasiDevInfo info = (BasiDevInfo)DevList[i];
                    list.Add(new Data { ID = info.device_id.ToString(), Text = info.device_id.ToString() });
                }
            }
            return list;

        }
    }
}
