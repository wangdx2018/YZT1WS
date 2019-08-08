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
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class BasiHallGroupIdInfoAdded : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
        string lineId = "";
        string stationId = "";
        string StationHallIdText = "";
        string StationHallNameText = "";

        public BasiHallGroupIdInfoAdded()
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
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            try
            {
                Wrapper.FullComboBox<BasiStationInfo>(this.StationName, BuinessRule.GetInstace().GetAllStationInfo(), "station_cn_name", "station_id", false, false);
                Wrapper.FullComboBox<BasiLineIdInfo>(this.LineName, BuinessRule.GetInstace().GetAllLineInfos(), "line_name", "line_id", false, false);
                //Wrapper.FullComboBox<BasiStationHallIdInfo>(this.StationHallId, BuinessRule.GetInstace().GetBasiStationHall(SysConfig.GetSysConfig().LocalParamsConfig.StationCode), "station_hall_id", "station_hall_id", false, false);
                Wrapper.FullComboBox<BasiStationHallIdInfo>(this.StationHallName, BuinessRule.GetInstace().GetBasiStationHall(SysConfig.GetSysConfig().LocalParamsConfig.StationCode), "station_hall_name", "station_hall_name", false, false);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

                lineId = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_id;
                Wrapper.ComboBoxSelectedItem(this.LineName, lineId);
                this.LineName.IsEnabled = false;
                stationId = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_id;
                Wrapper.ComboBoxSelectedItem(this.StationName, stationId);
                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
                {
                    this.StationName.IsEnabled = false;
                }
        }

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "LineName", this.LineName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationName", this.StationName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallId", BuinessRule.GetInstace().GetBasiStationHallByName(stationId, this.StationHallName.Text).station_hall_id.ToString());
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallName", this.StationHallName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "HallGroupId", this.HallGroupId.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "HallGroupName", this.HallGroupName.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.RunManager.BasiHallGroupIdInfoAdd();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list1);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.HallGroupName.Text = string.Empty;
            this.HallGroupId.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
