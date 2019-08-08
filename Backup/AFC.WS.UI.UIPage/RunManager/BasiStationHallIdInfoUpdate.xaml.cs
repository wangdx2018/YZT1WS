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
    public partial class BasiStationHallIdInfoUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
        string StationHallIdOld = "";
        string StationNames = "";

         public BasiStationHallIdInfoUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InitLoad();
        }

        private void InitLoad()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            try
            {
                StationNames = list.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
                StationHallIdOld = list.Single(temp => temp.bindingData.Equals("station_hall_id")).value.ToString();
            }
            catch (Exception ee)
            {
                MessageDialog.Show("请选择要更新的站厅信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            this.StationHallId.Text = StationHallIdOld;
            this.StationHallName.Text = list.Single(temp => temp.bindingData.Equals("station_hall_name")).value.ToString();
            try
            {
                Wrapper.FullComboBox<BasiStationInfo>(this.StationName, BuinessRule.GetInstace().GetAllStationInfo(), "station_cn_name", "station_id", false, false);
                Wrapper.FullComboBox<BasiLineIdInfo>(this.LineName, BuinessRule.GetInstace().GetAllLineInfos(), "line_name", "line_id", false, false);
            }
            catch (Exception ee)
            {
                MessageDialog.Show("请选择要更新的站厅信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

                string lineId = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_id;
                Wrapper.ComboBoxSelectedItem(this.LineName, lineId);
                this.LineName.IsEnabled = false;
                string stationId = BuinessRule.GetInstace().GetStationInfoByName(StationNames).station_id;
                Wrapper.ComboBoxSelectedItem(this.StationName, stationId);
                this.StationName.IsEnabled = false;
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            //DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "LineName", this.LineName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationName", this.StationName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallIdOld", StationHallIdOld);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallId", this.StationHallId.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "StationHallName", this.StationHallName.Text);
            IAction dpaction = new AFC.WS.ModelView.Actions.RunManager.BasiStationHallIdInfoUpdate();
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
            this.StationHallId.Text = string.Empty;
            this.StationHallName.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
