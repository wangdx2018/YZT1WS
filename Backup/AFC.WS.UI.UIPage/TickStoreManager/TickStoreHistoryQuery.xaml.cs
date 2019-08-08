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
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;

    /// <summary>
    /// TickStoreHistoryQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TickStoreHistoryQuery : UserControlBase
    {
        public TickStoreHistoryQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            
          //  TickStorePieQuery tspq = new TickStorePieQuery();
          //  this.tbPieQuery.Content = tspq;
           // tspq.InitControls();

            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_tick_storage_history_info.xml");
            if (icRule != null)
            {
                this.TickRoomIc.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_tick_storage_history_info.xml");
            if (dlr != null)
            {
                this.TickRoomList.Initliaize(dlr);
            }

         
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_tick_storage_history_info");
        }



        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string line_Name = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", TickRoomIc);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", TickRoomIc);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", TickRoomIc);
            }
            
        }
    }
}
