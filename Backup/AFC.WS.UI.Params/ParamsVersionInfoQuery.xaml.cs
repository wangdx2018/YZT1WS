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
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    /// <summary>
    /// ParamsVersionInfoQuery.xaml 的交互逻辑
    /// </summary>
    public partial class ParamsVersionInfoQuery : UserControlBase
    {
        public ParamsVersionInfoQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            #region local params query UI
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_para_local_full_ver_info.xml");
            if (icRule != null)
            {
                this.localParamIc.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_para_local_full_ver_info.xml");
            if (dlr != null)
            {
                this.localParamList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamsEditColorSetting());
                this.localParamList.Initliaize(dlr);

            }
            #endregion

            #region param syn Failed UI
            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_param_syn_failed.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_param_syn_failed.xml");
            if (dlr != null)
            {
                this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamSynFailedColorSetting());
                this.list.Initliaize(dlr);
            }
            #endregion


            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_para_dev_full_ver_info.xml");
            if (icRule != null)
            {
                this.devIc.Initialize(icRule);
            }
             dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_para_dev_full_ver_info.xml");
            if (dlr != null)
            {
                this.devParamList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamsDevFullEditColorSetting());
                this.devParamList.Initliaize(dlr);
            }
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_para_dev_full_ver_info");
            DataSourceManager.DisponseDataSource("ds_param_syn_failed");
            DataSourceManager.DisponseDataSource("ds_para_local_full_ver_info");
        }

   

        public override void InitlizeCompleteDone()
        {
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", ic);
             //   Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", localParamIc);
                Util.Instance.SetInitQuery("btn_station_cn_name", staionName, "btnQuery", devIc);
            }
        }
    }
}
