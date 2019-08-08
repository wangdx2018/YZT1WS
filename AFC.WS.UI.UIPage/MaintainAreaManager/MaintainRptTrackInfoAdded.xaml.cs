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

namespace AFC.WS.UI.UIPage.MaintainAreaManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainRptTrackInfoAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public MaintainRptTrackInfoAdded()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
        }
        private void InitLoad()
        {
            try
            {
                Wrapper.FullComboBox<BasiStationInfo>(this.station_cn_name, BuinessRule.GetInstace().GetAllStationInfo(), "station_cn_name", "station_id", false, false);
                Wrapper.FullComboBox<BasiLineIdInfo>(this.line_name, BuinessRule.GetInstace().GetAllLineInfos(), "line_name", "line_id", false, false);
                Wrapper.FullComboBox<BasiDevPartIdInfo>(this.dev_part_cn_name, BuinessRule.GetInstace().GetAllDevPartId(), "dev_part_cn_name", "dev_part_id", false, false);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string lineId = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_id;
                Wrapper.ComboBoxSelectedItem(this.line_name, lineId);
                this.line_name.IsEnabled = false;
                string stationId = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_id;
                Wrapper.ComboBoxSelectedItem(this.station_cn_name, stationId);
                this.station_cn_name.IsEnabled = false;
            }
            else if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("LC"))
            {
                string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
                Wrapper.ComboBoxSelectedItem(this.line_name, lineName);
                this.line_name.IsEnabled = false;
            }

            List<notify> notifyList = new List<notify>();
            notify notify1 = new notify { notifyName = "一般", notifyID = "01" };
            notify notify2 = new notify { notifyName = "紧急", notifyID = "02" };
            notify notify3 = new notify { notifyName = "加急", notifyID = "03" };
            notifyList.Add(notify1);
            notifyList.Add(notify2);
            notifyList.Add(notify3);
            this.maintenance_level.ItemsSource = notifyList;
            this.maintenance_level.DisplayMemberPath = "notifyName";

            List<notify> notifyList1 = new List<notify>();
            notify notify4 = new notify { notifyName = "已上报", notifyID = "01" };
            notify notify5 = new notify { notifyName = "解决中", notifyID = "02" };
            notify notify6 = new notify { notifyName = "已解决", notifyID = "03" };
            notifyList1.Add(notify4);
            notifyList1.Add(notify5);
            notifyList1.Add(notify6);
            this.fault_status.ItemsSource = notifyList1;
            this.fault_status.DisplayMemberPath = "notifyName";
            this.fault_status.SelectedIndex = 1;
            this.fault_status.IsEnabled = false;
            this.assign_operator_id.IsEnabled = false;
            Wrapper.SetDateTimePickerExtend(this.fault_date, DateTimeType.Day, 0);
            this.fault_time.SetControlValue(DateTime.Now.ToString("HH:mm:ss"));
        }

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "line_name", this.line_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "station_cn_name", this.station_cn_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "maintenance_level", (this.maintenance_level.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "device_id", this.device_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "fault_date",  GetDateTime(this.fault_date).ToString("yyyyMMdd"));
            Wrapper.Instance.AddQueryConditionToList(list, "fault_time", this.fault_time.SelectedTime.ToString().Replace(":", ""));
            Wrapper.Instance.AddQueryConditionToList(list, "input_operator_id", this.input_operator_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "assign_operator_id", this.assign_operator_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "fault_status", (this.fault_status.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "dev_part_cn_name", this.dev_part_cn_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "maintenance_area_id", this.maintenance_area_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "remark", this.remark.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.AddMaintainFaultRptStatus();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //this.line_name.Text = string.Empty;
            //this.station_cn_name.Text = string.Empty;
            this.maintenance_level.Text = string.Empty;
            this.device_id.Text = string.Empty;
            Wrapper.SetDateTimePickerExtend(this.fault_date, DateTimeType.Day, 0);
            this.fault_time.SetControlValue(DateTime.Now.ToString("HH:mm:ss"));
            this.input_operator_id.Text = string.Empty;
            this.assign_operator_id.Text = string.Empty;
            this.fault_status.Text = string.Empty;
            this.dev_part_cn_name.Text = string.Empty;
            this.maintenance_area_id.Text = string.Empty;
            this.remark.Text = string.Empty;
        }

        private class notify
        {
            private string _notifyName;
            private string _notifyID;

            public string notifyName
            {
                get
                {
                    return this._notifyName;
                }
                set
                {
                    this._notifyName = value;
                }
            }

            public string notifyID
            {
                get
                {
                    return this._notifyID;
                }
                set
                {
                    this._notifyID = value;
                }
            }

        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="dtp"></param>
        /// <returns></returns>
        DateTime GetDateTime(DateTimePickerExtend dtp)
        {
            return dtp.ContentDatePicker.JudgeIsNullOrEmpty() == true ?
                DateTime.Parse("1900-01-01") : Wrapper.GetDateTimePickerValue(dtp);
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
