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

namespace AFC.WS.UI.UIPage.DataManager
{
    /// <summary>
    /// OperContentQuery.xaml 的交互逻辑
    /// </summary>
    public partial class OperContentQuery :  UserControlBase
    {
        public OperContentQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\DataManager\ui_operContentQuery.xml");
            if (icRule != null)
            {
                this.icControl.Initialize(icRule);
                // InitliaizeData();
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\DataManager\list_operContentQuery.xml");
            if (dlr != null)
            {
                this.dataList.Initliaize(dlr);
            }
        }


        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_station_name", staionName, "btnQuery", icControl);
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", icControl);
            }
            else 
            {
                Util.Instance.SetInitQuery("btn_line_name", lineName, "btnQuery", icControl);
            }
        }


        /// <summary>
        /// 卸载控件
        /// </summary>
        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_operContentQuery");
        }

    }
}