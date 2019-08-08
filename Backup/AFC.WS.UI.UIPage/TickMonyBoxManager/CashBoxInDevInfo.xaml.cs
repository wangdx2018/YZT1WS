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

namespace AFC.WS.UI.UIPage.TickMonyBoxManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    /// <summary>
    /// StationModeQuery.xaml 的交互逻辑
    /// </summary>
    public partial class CashBoxInDevInfo : UserControlBase
    {
        public CashBoxInDevInfo()
        {
            InitializeComponent();
        }


        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_cash_box_in_dev_info.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_cash_box_in_dev_info.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
        }

        /// <summary>
        /// 初始化连动事件
        /// </summary>
        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", ic);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", ic);
            }
            //base.InitlizeCompleteDone();
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_cash_box_in_dev_info");
        }

    }
}
