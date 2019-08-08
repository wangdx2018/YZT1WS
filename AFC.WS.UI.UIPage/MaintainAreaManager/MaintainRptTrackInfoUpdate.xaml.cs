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
    using AFC.WS.UI.Convertors;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainRptTrackInfoUpdate : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        string fault_doc_id = string.Empty;

        public MaintainRptTrackInfoUpdate()
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
            List<QueryCondition> list1 = this.Tag as List<QueryCondition>;

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string lineId = list1.Single(temp => temp.bindingData.Equals("line_id")).value.ToString();
                Wrapper.ComboBoxSelectedItem(this.line_name, lineId);
                this.line_name.IsEnabled = false;
                string stationId = list1.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();
                Wrapper.ComboBoxSelectedItem(this.station_cn_name, stationId);
                this.station_cn_name.IsEnabled = false;
            }
            else if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("LC"))
            {
                string lineName = list1.Single(temp => temp.bindingData.Equals("line_id")).value.ToString();
                Wrapper.ComboBoxSelectedItem(this.line_name, lineName);
                this.line_name.IsEnabled = false;
            }

            fault_doc_id=list1.Single(temp => temp.bindingData.Equals("fault_doc_id")).value.ToString();
            string maintenance_level_name = list1.Single(temp => temp.bindingData.Equals("maintenance_level")).value.ToString();
            if (maintenance_level_name == "一般") { this.maintenance_level.Tag = "01"; this.maintenance_level.SelectedIndex = 0; }
            else if (maintenance_level_name == "紧急") { this.maintenance_level.Tag = "02"; this.maintenance_level.SelectedIndex = 1; }
            else if (maintenance_level_name == "加急") { this.maintenance_level.Tag = "03"; this.maintenance_level.SelectedIndex = 2; }
            this.device_id.Text = list1.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();          
            //this.fault_time.Content = list1.Single(temp => temp.bindingData.Equals("fault_time")).value.ToString();
            
            this.input_operator_id.Text = list1.Single(temp => temp.bindingData.Equals("input_operator_id")).value.ToString();
            this.assign_operator_id.Text = list1.Single(temp => temp.bindingData.Equals("assign_operator_id")).value.ToString();
            string faultStatus = list1.Single(temp => temp.bindingData.Equals("fault_status")).value.ToString();
            if (faultStatus == "已上报") { this.fault_status.Tag = "01"; this.fault_status.SelectedIndex = 0; }
            else if (faultStatus == "解决中") { this.fault_status.Tag = "02"; this.fault_status.SelectedIndex = 1; }
            else if (faultStatus == "已解决") { this.fault_status.Tag = "03"; this.fault_status.SelectedIndex = 2; }
            this.dev_part_cn_name.Text = list1.Single(temp => temp.bindingData.Equals("dev_part_cn_name")).value.ToString();
            this.maintenance_area_id.Text = list1.Single(temp => temp.bindingData.Equals("maintenance_area_id")).value.ToString();
            this.remark.Text = list1.Single(temp => temp.bindingData.Equals("remark")).value.ToString();

            DateTimeConvert convert = new DateTimeConvert();
            convert.DateTimeFormat = "yyyy年MM月dd日";
            this.fault_date.SetControlValue(convert.Convert(list1.Single(temp => temp.bindingData.Equals("fault_date")).value.ToString(), null, null, null).ToString());
             // Convert.ToDateTime(this.tpAutoPrintTime.Text).ToString("HH:mm:ss")
            //DateTime.ParseExact(date,"yyyyMMdd",null).ToString("yyyy-MM-dd")
            this.fault_time.Text = DateTime.ParseExact(list1.Single(temp => temp.bindingData.Equals("fault_time")).value.ToString(), "HHmmss", null).ToString("HH:mm:ss");
            //this.fault_time.SetControlValue(convertTime.Convert(list1.Single(temp => temp.bindingData.Equals("fault_time")).value.ToString(), null, null, null).ToString());
            //this.fault_time.SetControlValue(list1.Single(temp => temp.bindingData.Equals("fault_time")).value.ToString());

        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "fault_doc_id", fault_doc_id);
            Wrapper.Instance.AddQueryConditionToList(list, "line_name", this.line_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "station_cn_name", this.station_cn_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "maintenance_level", this.maintenance_level.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "device_id", this.device_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "fault_date", GetDateTime(this.fault_date).ToString("yyyyMMdd"));
            Wrapper.Instance.AddQueryConditionToList(list, "fault_time", this.fault_time.SelectedTime.ToString().Replace(":", ""));
            Wrapper.Instance.AddQueryConditionToList(list, "input_operator_id", this.input_operator_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "assign_operator_id", this.assign_operator_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "fault_status", this.fault_status.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "dev_part_cn_name", this.dev_part_cn_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "maintenance_area_id", this.maintenance_area_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "remark", this.remark.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.UpdateMaintainFaultRptStatus();
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
