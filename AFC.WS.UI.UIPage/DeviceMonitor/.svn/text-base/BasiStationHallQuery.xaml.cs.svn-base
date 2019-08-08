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
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;
using AFC.WS.UI.Common;
using AFC.WS.BR;

namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// BasiStationHallQuery.xaml 的交互逻辑
    /// </summary>
    public partial class BasiStationHallQuery : UserControlBase
    {
        public BasiStationHallQuery()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\CashBoxManager\ui_stationHall.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\CashBoxManager\list_stationHall.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }

            this.basiDevInfo.InitControls();
            this.basiHallGroupQuery.InitControls();
            this.devStationRunStatusQuery.InitControls();
        }

        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_id", staionName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_line_id", lineName, "btnQuery", ic);
            }
            else 
            {
                Util.Instance.SetInitQuery("btn_line_id", lineName, "btnQuery", ic);            
            }

            this.basiDevInfo.InitlizeCompleteDone();
            this.basiHallGroupQuery.InitlizeCompleteDone();
            this.devStationRunStatusQuery.InitlizeCompleteDone();
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_stationHall");

            this.basiDevInfo.UnLoadControls();
            this.basiHallGroupQuery.UnLoadControls();
            this.devStationRunStatusQuery.UnLoadControls();
        }
    }
}
