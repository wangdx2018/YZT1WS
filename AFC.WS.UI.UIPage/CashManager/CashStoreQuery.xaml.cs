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

namespace AFC.WS.UI.UIPage.CashManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    /// <summary>
    /// ParamsVersionInfoQuery.xaml 的交互逻辑
    /// </summary>
    public partial class CashStoreQuery : UserControlBase
    {
        public CashStoreQuery()
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

            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_cash_storage_info.xml");
            if (icRule != null)
            {
                this.CashRoomIc.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_cash_storage_info.xml");
            if (dlr != null)
            {
                this.CashRoomList.Initliaize(dlr);
            }

            #region param syn Failed UI
            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_cash_in_operator_info.xml");
            if (icRule != null)
            {
                this.OperHandic.Initialize(icRule);
            }
            dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_cash_in_operator_info.xml");
            if (dlr != null)
            {
                //this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamSynFailedColorSetting());
                this.OperHandiclist.Initliaize(dlr);
            }
            #endregion


            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_cash_box_status_info.xml");
            if (icRule != null)
            {
                this.CashBoxic.Initialize(icRule);
            }
            dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_cash_box_status_info.xml");
            if (dlr != null)
            {
                //this.devParamList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamsDevFullEditColorSetting());
                this.CashBoxlist.Initliaize(dlr);
            }
            
            CashStorePieQuery tspq = new CashStorePieQuery();
            this.tbPieQuery.Content = tspq;
            tspq.InitControls(); 
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_cash_storage_info.xml");
            DataSourceManager.DisponseDataSource("ds_cash_in_operator_info.xml");
            DataSourceManager.DisponseDataSource("ds_cash_box_status_info");
        }


        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string line_Name = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", CashRoomIc);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", CashRoomIc);



                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", OperHandic);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", OperHandic);


                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", CashBoxic);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", CashBoxic);


            }

           

            else
            {
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", CashRoomIc);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", OperHandic);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", CashBoxic);

            }
        }

    }
}
