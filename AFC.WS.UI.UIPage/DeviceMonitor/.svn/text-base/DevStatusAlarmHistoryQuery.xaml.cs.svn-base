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
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// DevRunStatusQuery.xaml 的交互逻辑
    /// </summary>
    public partial class DevStatusAlarmHistoryQuery : UserControlBase
    {
        public DevStatusAlarmHistoryQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_dev_status_alarm_history.xml");
            if (icRule != null)
            {
                this.icControl.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\list_dev_status_alarm_history.xml");
            if (dlr != null)
            {
                this.dataList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.StatusLevelColorSetting());
                this.dataList.Initliaize(dlr);
            }
        }

        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_id", staionName, "btnQuery", icControl);
                Util.Instance.SetInitQuery("btn_line_id", lineName, "btnQuery", icControl);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_line_id", lineName, "btnQuery", icControl);
            }
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_dev_status_alarm_history");
        }
    }
}
