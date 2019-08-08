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

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    /// <summary>
    /// TickDispatchInfoQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TickDispatchInfoQuery : UserControlBase
    {
        public TickDispatchInfoQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\TickMonyBoxManager\ui_tick_dis_log_in.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
                // InitliaizeData();
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\TickMonyBoxManager\list_dispatch_log_in.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }

            InteractiveControlRule icRuleOut = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\TickMonyBoxManager\ui_tick_dispatch_log_out.xml");
            if (icRuleOut != null)
            {
                this.icOut.Initialize(icRuleOut);
                // InitliaizeData();
            }
            DataListRule dlrOut = Utility.Instance.GetDataListObject(@".\RuleFiles\TickMonyBoxManager\list_tick_dispatch_log_out.xml");
            if (dlrOut != null)
            {
                this.listOut.Initliaize(dlrOut);
            }


        }

        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
              //  Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", ic);
            //    Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", icOut);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", icOut);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", icOut);
            }
            //base.InitlizeCompleteDone();
        }

        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_tick_dispatch_log_in");
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_tick_dispatch_log_out");
            
        }
    }
}
