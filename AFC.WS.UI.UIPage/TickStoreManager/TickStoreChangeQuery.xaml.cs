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
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;
    using AFC.WS.BR;
    /// <summary>
    /// TickStoreChangeQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TickStoreChangeQuery : UserControlBase
    {
        public TickStoreChangeQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\TickMonyBoxManager\ui_tickStoreChangeLog.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
                // InitliaizeData();
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\TickMonyBoxManager\list_tickStoreChangeLog.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }

            if (dlr != null)
            {
                //this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.OpeClassColorSetting());
                this.list.Initliaize(dlr);
            }

            this.tickCheckInOutQuery.InitControls();
            this.tickDispatchInfoQuery.InitControls();
        }


        /// <summary>
        /// 初始化连动事件
        /// </summary>
        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string line_Name = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", ic);
            }
            else 
            {
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", ic);          
            }
            this.tickDispatchInfoQuery.InitlizeCompleteDone();
            this.tickCheckInOutQuery.InitlizeCompleteDone();
            //base.InitlizeCompleteDone();
        }

        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_tickStoreChangeLog");
            //base.UnLoadControls();
            this.tickCheckInOutQuery.UnLoadControls();
            this.tickDispatchInfoQuery.UnLoadControls();
        }

    }
}
