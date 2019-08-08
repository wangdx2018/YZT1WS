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
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// DevRunStatusQuery.xaml 的交互逻辑
    /// </summary>
    public partial class DevRunStatusQuery : UserControlBase
    {
        public DevRunStatusQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\LogQuery\ui_devStatusQuery.xml");
            if (icRule != null)
            {
                this.icControl.Initialize(icRule);
                // InitliaizeData();
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\LogQuery\list_devStatusQuery.xml");
            if (dlr != null)
            {
                this.dataList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.StatusLevelColorSetting());
                this.dataList.Initliaize(dlr);
            }

            IDataSource ds = DataSourceManager.LookupDataSourceByName("ds_devStatusQuery");
            List<string> list = new List<string>();
            List<QueryCondition> queryData = this.Tag as List<QueryCondition>;
            if (queryData != null)
            {
                list.Add(string.Format("device_id='{0}'", queryData[0].value));
                ds.SetQueryParams(list);
            }

            (icControl.GetCommonControlByName("btn_device_id") as AFC.WS.UI.CommonControls.TextBoxExtend).Text = queryData[0].value.ToString();

            string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
            string line_Name = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;           

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {

                Util.Instance.SetInitQuery("btn_station_id", staionName, "btnQuery", icControl);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", icControl);
            }
            else 
            {
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", icControl);
            }

        }

        /// <summary>
        /// 初始化连动事件
        /// </summary>
        public override void InitlizeCompleteDone()
        {
         
            //base.InitlizeCompleteDone();
        }

        /// <summary>
        /// 卸载控件
        /// </summary>
        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_devStatusQuery");
        }
    }
}
