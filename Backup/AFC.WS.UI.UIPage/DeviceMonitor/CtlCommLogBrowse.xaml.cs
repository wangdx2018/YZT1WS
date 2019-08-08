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
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.BR;
namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// CtlCommLogBrowse.xaml 的交互逻辑
    /// </summary>
    public partial class CtlCommLogBrowse : UserControlBase
    {
             /// <summary>
        /// 初始化控件
        /// </summary>
        public CtlCommLogBrowse()
        {
            InitializeComponent();
        }
         /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\LogQuery\ui_ctrlLogInfo.xml");
            if (icRule != null)
            {
                this.icControl.Initialize(icRule);
                // InitliaizeData();
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\LogQuery\list_ctrlLogInfo.xml");
            if (dlr != null)
            {
                this.dataList.Initliaize(dlr);
            }

            if (dlr != null)
            {
                this.dataList.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.OpeClassColorSetting());
                this.dataList.Initliaize(dlr);
            }
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
                Util.Instance.SetInitQuery("btn_station_id", staionName, "btnQuery", icControl);
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", icControl);
            }
            else
            {
                Util.Instance.SetInitQuery("btn_line_name", line_Name, "btnQuery", icControl);         
            }
            //base.InitlizeCompleteDone();
        }
        /// <summary>
        /// 设备类型变化控件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxExtend extend = sender as ComboBoxExtend;
            string key = extend.SelectedValue.ToString().Split(':')[1].Trim();
            string deviceType = extend.DataKeyValue[key];

            ComboBoxExtend cmbOperatorCode = this.icControl.GetCommonControlByName("btn_oper_code") as ComboBoxExtend;
            cmbOperatorCode.SqlContent = string.Format("select * from basi_oper_code_info t  where t.device_type='{0}'", deviceType);
            cmbOperatorCode.BindDisplayField = "oper_content";
            cmbOperatorCode.BindHideField = "oper_code";

            cmbOperatorCode.Initialize();
            cmbOperatorCode.Text = "全部";

            TextBoxExtend txt = this.icControl.GetCommonControlByName("btn_device_id") as TextBoxExtend;
            txt.SetControlValue(deviceType);


        }

        /// <summary>
        /// 卸载控件
        /// </summary>
        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_ctrlLogInfo");
        }
    }
}
