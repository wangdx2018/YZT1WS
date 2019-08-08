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

namespace AFC.WS.UI.Primission
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.DataSources;

    /// <summary>
    /// 采用WS2.0基础组件
    /// </summary>
    public partial class OperatorUnLock : UserControlBase
    {
        public OperatorUnLock()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {

            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_operatorLockStatus.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }

            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_opeator_locking.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
        }

        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string line_Name = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {   
                Util.Instance.SetInitQuery("btn_company_name", staionName, "btnQuery",ic);
            }
            //base.InitlizeCompleteDone();
        }
        
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_opeatorUnlocking");
        }
    }
}
