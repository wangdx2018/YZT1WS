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

namespace AFC.WS.UI.UIPage.ModeManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// StationModeQuery.xaml 的交互逻辑
    /// </summary>
    public partial class StationModeQuery : UserControlBase
    {

        private ComboBoxExtend cbLine;

        private ComboBoxExtend cbStation;

        public StationModeQuery()
        {
            InitializeComponent();

          
        }

        private void cbe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbStation= this.ic.GetCommonControlByName("btn_station_cn_name") as ComboBoxExtend;
            if (cbStation != null)
            {
                try
                {
                    string lineId = string.Empty;
                    foreach(var temp in this.cbLine.DataKeyValue)
                    {
                        if (temp.Key.Equals((this.cbLine.SelectedItem as ComboBoxItem).Content.ToString()))
                            lineId = temp.Value;
                    }
                    cbStation.SqlContent = string.Format("select * from basi_station_info t where t.line_id='{0}'", lineId);
                    cbStation.BindType = BindType.SqlBindData;
                    cbStation.BindHideField = "station_id";
                    cbStation.BindDisplayField = "station_cn_name";
                    cbStation.Initialize();
                }
                catch (Exception ex)
                {

                }
            }
        }


        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_run_mode_status.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\list_run_mode_status.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
            cbLine = this.ic.GetCommonControlByName("btn_line_name") as ComboBoxExtend;
            if (cbLine != null)
            {
                cbLine.SelectionChanged += new SelectionChangedEventHandler(cbe_SelectionChanged);
            }
        }


        public override void UnLoadControls()
        {
            if (cbLine != null)
            {
                cbLine.SelectionChanged -= new SelectionChangedEventHandler(cbe_SelectionChanged);

            }
            DataSourceManager.DisponseDataSource("ds_run_mode_status");
        }

        public override void InitlizeCompleteDone()
        {
            //string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            //string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            //if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            //{
            //    Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
            //    Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", ic);
            //}
            //else
            //{
            //    Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", ic);
            //}
            ////base.InitlizeCompleteDone();
        }
    }
}
