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
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class BasiDevInfoUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
        string StationHallIdText = "";
        string StationHallNameText = "";
        string HallGroupIdText = "";
        string HallGroupNameText = "";
        string lineId = "";
        string stationId = "";
        string stationNames = "";

        public BasiDevInfoUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            try
            {
                StationHallIdText = list.Single(temp => temp.bindingData.Equals("station_hall_id")).value.ToString();
                stationNames = list.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
                stationId = BuinessRule.GetInstace().GetStationInfoByName(stationNames).station_id;
            }
            catch
            {
                MessageDialog.Show("请选择要更新的设备站厅组别序号信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            StationHallNameText = list.Single(temp => temp.bindingData.Equals("station_hall_name")).value.ToString();
            HallGroupIdText = list.Single(temp => temp.bindingData.Equals("hall_group_id")).value.ToString();
            HallGroupNameText = list.Single(temp => temp.bindingData.Equals("hall_group_name")).value.ToString();
            try
            {
                Wrapper.FullComboBox<BasiStationInfo>(this.StationName, BuinessRule.GetInstace().GetAllStationInfo(), "station_cn_name", "station_id", false, false);
                Wrapper.FullComboBox<BasiLineIdInfo>(this.LineName, BuinessRule.GetInstace().GetAllLineInfos(), "line_name", "line_id", false, false);
                Wrapper.FullComboBox<BasiStationHallIdInfo>(this.StationHallId, BuinessRule.GetInstace().GetBasiStationHall(stationId), "station_hall_id", "station_hall_id", false, false);
                Wrapper.FullComboBox<BasiStationHallIdInfo>(this.StationHallName, BuinessRule.GetInstace().GetBasiStationHall(stationId), "station_hall_name", "station_hall_name", false, false);
                Wrapper.FullComboBox<BasiHallGroupIdInfo>(this.HallGroupName, BuinessRule.GetInstace().GetBasiHallGroupIdInfo(stationId, StationHallIdText), "hall_group_name", "hall_group_name", false, false);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            lineId = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_id;
            Wrapper.ComboBoxSelectedItem(this.LineName, lineId);
            this.LineName.IsEnabled = false;
            stationId = BuinessRule.GetInstace().GetStationInfoById(stationId).station_id;
            Wrapper.ComboBoxSelectedItem(this.StationName, stationId);
            this.StationName.IsEnabled = false;
            Wrapper.ComboBoxSelectedItem(this.StationHallId, StationHallIdText);
            //this.StationHallId.IsEnabled = false;
            Wrapper.ComboBoxSelectedItem(this.StationHallName, StationHallNameText);
            //this.StationHallName.IsEnabled = false;
            this.DeviceId.Text = list.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            this.DeviceId.IsEnabled = false;
            Wrapper.ComboBoxSelectedItem(this.HallGroupName, HallGroupNameText);
            this.HallGroupName.IsEnabled = true;
            this.HallGroupSerialNo.Text = list.Single(temp => temp.bindingData.Equals("group_serial_no")).value.ToString();
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            //DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "LineName", this.LineName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationName", this.StationName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "DeviceId", this.DeviceId.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallId", this.StationHallId.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallName", this.StationHallName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "HallGroupName", this.HallGroupName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "HallGroupSerialNo", this.HallGroupSerialNo.Text);
            IAction dpaction = new AFC.WS.ModelView.Actions.RunManager.BasiDevInfoUpdate();

            //dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            if (dpaction.CheckValid(list1))
            {
                dpaction.DoAction(list1);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.StationHallName.Text = string.Empty;
            this.HallGroupName.Text = string.Empty;
        }

        private void stationHallId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationHallIdText=(this.StationHallId.SelectedItem as ComboBoxItem).Content.ToString();
            StationHallNameText = BuinessRule.GetInstace().GetBasiStationHallById(stationId, StationHallIdText).station_hall_name;
            Wrapper.ComboBoxSelectedItem(this.StationHallName, StationHallNameText);
            Wrapper.FullComboBox<BasiHallGroupIdInfo>(this.HallGroupName, BuinessRule.GetInstace().GetBasiHallGroupIdInfo(stationId, StationHallIdText), "hall_group_name", "hall_group_name", false, false);
            this.HallGroupName.SelectedIndex = 0;
        }

        private void stationHallName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationHallNameText = (this.StationHallName.SelectedItem as ComboBoxItem).Content.ToString();
            StationHallIdText = BuinessRule.GetInstace().GetBasiStationHallByName(stationId, StationHallNameText).station_hall_id;
            Wrapper.ComboBoxSelectedItem(this.StationHallId, StationHallIdText);
            Wrapper.FullComboBox<BasiHallGroupIdInfo>(this.HallGroupName, BuinessRule.GetInstace().GetBasiHallGroupIdInfo(stationId, StationHallIdText), "hall_group_name", "hall_group_name", false, false);
            this.HallGroupName.SelectedIndex = 0;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
