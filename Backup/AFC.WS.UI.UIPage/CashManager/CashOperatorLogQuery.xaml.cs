﻿using System;
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
using AFC.WS.BR;
using AFC.WS.UI.Common;
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIPage.CashManager
{
    /// <summary>
    /// CashOperatorLogQuery.xaml 的交互逻辑
    /// </summary>
    public partial class CashOperatorLogQuery : UserControlBase
    {
        public CashOperatorLogQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\CashBoxManager\ui_cashOperationLog.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\CashBoxManager\list_cashOperatorLog.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
        }

        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_cash_operator_return_log_station_id", staionName, "btnQuery", ic);
                Util.Instance.SetInitQuery("btn_cash_operator_return_log_line_id", lineName, "btnQuery", ic);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_cash_operator_return_log_line_id", lineName, "btnQuery", ic);
            }

        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_cashOperatorLog");
        }
    }
}
