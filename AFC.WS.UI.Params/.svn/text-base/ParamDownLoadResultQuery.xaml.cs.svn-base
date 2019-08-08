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

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.BR;

    /// <summary>
    /// ParamDownLoadResultQuery.xaml 的交互逻辑
    /// </summary>
    public partial class ParamDownLoadResultQuery : UserControlBase
    {
        public ParamDownLoadResultQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_para_dload_busi_data.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_para_dload_busi_data.xml");
            if (dlr != null)
            {
                this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamDownLoadResultColorSetting());
                this.list.Initliaize(dlr);
            }

            paraSwitchQuery.InitControls();
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_para_dload_busi_data");

            paraSwitchQuery.UnLoadControls();
        }

        public override void InitlizeCompleteDone()
        {
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
            }

            paraSwitchQuery.InitlizeCompleteDone();
        }
    }
}
