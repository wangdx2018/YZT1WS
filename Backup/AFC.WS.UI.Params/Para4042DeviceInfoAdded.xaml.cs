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

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class Para4042DeviceInfoAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public Para4042DeviceInfoAdded()
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
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string staionId = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_id;
                Wrapper.ComboBoxSelectedItem(this.station_cn_name, staionId);
                this.station_cn_name.IsEnabled = false;
            }

            List<notify> notifyList = new List<notify>();
            notify notify1 = new notify { notifyName = "在用", notifyID = "01" };
            notify notify2 = new notify { notifyName = "停用", notifyID = "02" };
            notify notify3 = new notify { notifyName = "移除", notifyID = "03" };
            notifyList.Add(notify1);
            notifyList.Add(notify2);
            notifyList.Add(notify3);
            this.start_flag.ItemsSource = notifyList;
            this.start_flag.DisplayMemberPath = "notifyName";
        }

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "station_cn_name", this.station_cn_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_id", this.device_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "station_hall_id", this.station_hall_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_group_id", this.device_group_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_serial_no", this.device_serial_no.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_group_serial_no", this.device_group_serial_no.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "honri_index", this.honri_index.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "vertical_index", this.vertical_index.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "display_angle", this.display_angle.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_ip", this.device_ip.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "start_flag", (this.start_flag.SelectedValue as notify).notifyID);
            dpaction.subAction = new AFC.WS.ModelView.Actions.ParamActions.AddPara4042DeviceInfo();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.station_cn_name.Text = string.Empty;
            this.device_id.Text = string.Empty;
            this.station_hall_id.Text = string.Empty;
            this.device_group_id.Text = string.Empty;
            this.device_serial_no.Text = string.Empty;
            this.device_group_serial_no.Text = string.Empty;
            this.honri_index.Text = string.Empty;
            this.vertical_index.Text = string.Empty;
            this.display_angle.Text = string.Empty;
            this.device_ip.Text = string.Empty;
            this.start_flag.Text = string.Empty;
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
        /// 重写初始化控件
        /// </summary>
    }
}
