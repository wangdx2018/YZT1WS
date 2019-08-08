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
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    //--->操作员信息查询（在该界面中定义解锁，密码重置等功能，通过多个Action来实现）。
    /// <summary
    /// 操作员信息查询（在该界面中定义解锁，密码重置等功能，通过多个Action来实现）。
    /// 采用WS2.0基础组件
    /// </summary>
    public partial class OperatorInfoQuery : UserControlBase
    {
        public OperatorInfoQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_operatorScEnable.xml");
            if (icRule != null)
            {
                this.icControl.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_operatorScEnable.xml");
            if (dlr != null)
            {
                this.dataList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.OperatorStatusColorSetting());
                this.dataList.Initliaize(dlr);
            }
            //string station = AFC.WS.UI.Common.SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            //if (!string.IsNullOrEmpty(station))
            //{
            //    IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_OperatorInfo");
            //    if (dataSource != null)
            //    {
            //        List<string> paraList = new List<string>();
            //        paraList.Add(string.Format("company_name ='{0}'", station));
            //        dataSource.SetQueryParams(paraList);
            //    }
            //}
           
            
        }

        public override void InitlizeCompleteDone()
        {
            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string line_Name = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                Util.Instance.SetInitQuery("btn_company_name", staionName, "btnQuery", icControl);
            }
            //base.InitlizeCompleteDone();
        }
        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
            //base.UnLoadControls();
            DataSourceManager.DisponseDataSource("ds_OperatorInfo");
        }
    }
}
