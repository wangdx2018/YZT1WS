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
using AFC.WS.BR.ReportManager;
using AFC.BOM2.UIController;

using AFC.WS.UI.Config;
using System.IO;
using AFC.WS.BR;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;


namespace AFC.WS.UI.UIPage.ReportManager
{
    /// <summary>
    /// LookHistoryReport.xaml 的交互逻辑
    /// </summary>
    public partial class LookHistoryReport : UserControlBase
    {
        public LookHistoryReport()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(LookHistoryReport_Loaded);
        }

        private void LookHistoryReport_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportManagerHelper.Instance.DeleteFileFromHistoryReportPath();

                Wrapper.FullComboBox(this.cbbStationId, 
                    BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode), 
                    "station_cn_name", 
                    "station_id",
                    true, false);

                //Wrapper.FullComboBox(this.cbbClassTimeId, BRContext.Instance.BasiWorkGroupInfoItem, "work_group_name", "work_group_id", true, true);
                Wrapper.FullComboBox(this.cbbBissType,
                  BuinessRule.GetInstace().GetAllBasiTranTypeInfos(),
                  "afc_type_name",
                  "afc_type",
                  true,true);

                Wrapper.FullComboBox(this.cbbOperatorId, 
                    BuinessRule.GetInstace().GetAllOperatorInfo(), "operator_id", "operator_id", true, true);

                InitTreeView();

                InitDateTimePickerExtend();

                ReportManagerHelper.Instance.InitControlIsEnabled(this.gParamCondition, false);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 初始化TreeView控件
        /// </summary>
        private void InitTreeView()
        {
            List<BasiReportTypeInfo> item = ReportManagerHelper.Instance.ReportTypeInfoItem;
            ReportManagerHelper.Instance.SetTreeViewReportTypeInfo(this.tvRoot, item);
        }

        /// <summary>
        /// 初始化日期控件
        /// </summary>
        private void InitDateTimePickerExtend()
        {
            Wrapper.SetDateTimePickerExtend(this.dtpRunDate, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpRunDateBegin, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpRunDateEnd, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpTranDate, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpTranDateBegin, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpTranDateEnd, DateTimeType.Minutes, 0);

        }

        private void dgHistoryInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AFC.WS.Model.DB.rptLookHistoryReportModel model = this.dgHistoryInfo.SelectedItem as rptLookHistoryReportModel;
            if (model != null)
            {
                ReportManagerHelper.Instance.SetParamValueControl(this.gParamCondition, model.report_condition_param.Split('|'),
                    model.report_param_value.Split('_'));

            }
        }

        private void tvRoot_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                ReportManagerHelper.Instance.InitDateTimePickerValue(this.gParamCondition);

                TreeViewItem item = this.tvRoot.SelectedItem as TreeViewItem;

                if (item == null)
                {
                    return;
                }
                if (this.lblReportName != null)
                {
                    this.lblReportName.Content = item.Header;
                }

                BasiReportInfo ri = item.Tag as BasiReportInfo;

                if (ri != null)
                {
                    this.dgHistoryInfo.ItemsSource = null;
                    List<rptLookHistoryReportModel> rhItem = ReportManagerHelper.Instance.HistoryReportItemByReportName(ri.report_name);
                    if (rhItem != null)
                    {
                        this.dgHistoryInfo.ItemsSource = rhItem.ToArray();
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        private void btnOpenHistoryReport_Click(object sender, RoutedEventArgs e)
        {
            rptLookHistoryReportModel model = this.dgHistoryInfo.SelectedItem as rptLookHistoryReportModel;
            if (model != null)
            {
                //model.report_path;
                String directoryPath = Environment.CurrentDirectory +
                   SysConfig.GetSysConfig().ReportCfg.HistoryReportPath;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string path = directoryPath + model.report_save_name;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                FTPCommon ftpCom = new FTPCommon(BuinessRule.GetInstace().brConext.FtpUser,
                    BuinessRule.GetInstace().brConext.FtpPwd,
                    BuinessRule.GetInstace().brConext.FtpAddress);
                int result = ftpCom.FTPDownLoad(model.report_path, path);
                if (result == 0)
                {
                    ReportManagerHelper.Instance.ReportOpen("", path);
                }
                else
                {
                    Wrapper.ShowDialog("没有从服务器上找到要打开的报表。");
                }
            }
            else
            {
                Wrapper.ShowDialog("选择要打开的报表。");
            }
        }

    }
}
