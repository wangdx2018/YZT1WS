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
using AFC.WS.UI.DataSources;
using AFC.WS.UI.Config;
using AFC.WS.UI.Components;
using AFC.WS.UI.Common;
using AFC.WS.BR;

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    /// <summary>
    /// TicketTrackingQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TicketTrackingQuery : UserControlBase
    {
        public TicketTrackingQuery()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {

            this.InitliaizeQueryUI(@".\RuleFiles\TickMonyBoxManager\ui_data_ykt_his.xml",
                                                  @".\RuleFiles\TickMonyBoxManager\list_data_ykt_his.xml",
                                                  this.uiYkt,
                                                  this.lisYkt);



            this.InitliaizeQueryUI(@".\RuleFiles\TickMonyBoxManager\ui_data_ypt_his.xml",
                                                 @".\RuleFiles\TickMonyBoxManager\list_data_ypt_his.xml",
                                                this.uiYpt,
                                                 this.listYpt);
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_data_ykt_his");
            DataSourceManager.DisponseDataSource("ds_data_ypt_his");
        }

        /// <summary>
        /// 初始化UI查询界面
        /// </summary>
        /// <param name="uiFileName">查询规则文件名</param>
        /// <param name="listFileName">列表规则文件名</param>
        /// <param name="ic">交互界面控件</param>
        /// <param name="dlc">列表控件</param>
        private void InitliaizeQueryUI(string uiFileName, string listFileName, InteractiveControl ic, DataListControl dlc)
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(uiFileName);
            if (icRule != null)
            {
                ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(listFileName);
            if (dlr != null)
            {
                dlc.Initliaize(dlr);
            }
        }

        /// <summary>
        /// 初始化连动事件
        /// </summary>
        public override void InitlizeCompleteDone()
        {
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
                Util.Instance.SetInitQuery("btn_station_id", staionName, "btnQuery", this.uiYpt);
                Util.Instance.SetInitQuery("btn_station_id", staionName, "btnQuery", this.uiYkt);
            }
            //base.InitlizeCompleteDone();
        }

    }
}
