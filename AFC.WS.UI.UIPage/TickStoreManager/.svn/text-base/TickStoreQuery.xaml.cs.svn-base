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
    /// ParamsVersionInfoQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TickStoreQuery : UserControlBase
    {
        public TickStoreQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
           /* #region local params query UI
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_basi_tick_mana_type_info.xml");
            if (icRule != null)
            {
                this.localParamIc.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_basi_tick_mana_type_info.xml");
            if (dlr != null)
            {
                this.localParamList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamsEditColorSetting());
                this.localParamList.Initliaize(dlr);

            }
            #endregion  */

            TickStorePieQuery tspq = new TickStorePieQuery();
            this.tbPieQuery.Content = tspq;
            tspq.InitControls();

            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_tick_storage_info.xml");
            if (icRule != null)
            {
                this.TickRoomIc.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_tick_storage_info.xml");
            if (dlr != null)
            {
                this.TickRoomList.Initliaize(dlr);
            }

            #region param syn Failed UI
            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_tick_in_operator_info.xml");
            if (icRule != null)
            {
                this.OperHandic.Initialize(icRule);
            }
            dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_tick_in_operator_info.xml");
            if (dlr != null)
            {
                //this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamSynFailedColorSetting());
                this.OperHandiclist.Initliaize(dlr);
            }
            #endregion


            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_tick_box_status.xml");
            if (icRule != null)
            {
                this.TicketBoxic.Initialize(icRule);
            }
            dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\list_tick_box_status_info.xml");
            if (dlr != null)
            {
                //this.devParamList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamsDevFullEditColorSetting());
                this.TicketBoxlist.Initliaize(dlr);
            }
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_tick_storage_info");
            DataSourceManager.DisponseDataSource("ds_tick_in_operator_info.xml");
            DataSourceManager.DisponseDataSource("ds_tick_box_status_info");
        }



        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                //Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", TicketBoxic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", OperHandic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", TickRoomIc);
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", TicketBoxic);
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", OperHandic);
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", TickRoomIc);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", TicketBoxic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", OperHandic);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", TickRoomIc);
            }
        }
    }
}
