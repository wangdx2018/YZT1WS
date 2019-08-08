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
using AFC.WS.UI.BR;
using AFC.WS.UI.Components;
using AFC.WS.UI.BR.Data;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;

using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using System.IO;
using AFC.WS.Model.DB;
using AFC.WS.BR.ReportManager;
using AFC.WS.BR;

namespace AFC.WS.UI.UIPage.ReportManager
{
    /// <summary>
    /// AutoPrintReportFrom.xaml 的交互逻辑
    /// </summary>
    public partial class AutoPrintReportFrom : UserControlBase
    {
        public AutoPrintReportFrom()
        {
            InitializeComponent();
            this.btnSave.Click += new RoutedEventHandler(btnSave_Click);
            this.Loaded += new RoutedEventHandler(AutoPrintReportFrom_Loaded);
            this.Unloaded += new RoutedEventHandler(AutoPrintReportFrom_Unloaded);

        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem cbi = this.cbbReportName.SelectedItem as ComboBoxItem;
            if (cbi != null)
            {
                BasiReportInfo ri = cbi.Tag as BasiReportInfo;
                if (ri != null)
                {

                    RptAutoPrintInfo rapi = new RptAutoPrintInfo();
                    rapi.report_condition_param = ri.report_condition_param;
                    rapi.report_name = ri.report_name;
                    rapi.report_sub_type_id = ri.report_sub_type_id;
                    rapi.report_type_id = ri.report_type_id;

                    bool result = ReportManagerHelper.Instance.JudgeAutoPrintReportIsExist(rapi);
                    if (result)
                    {
                        Wrapper.ShowDialog(ri.report_name + " 已经存在。", MessageBoxIcon.Warning);
                        return;
                    }
                    result = ReportManagerHelper.Instance.AddAutoPrintReport(rapi);
                    if (!result)
                    {
                        Wrapper.ShowDialog("添加[" + ri.report_name + "]失败。", MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        Wrapper.ShowDialog("添加[" + ri.report_name + "]成功。", MessageBoxIcon.Information);
                    }

                    DataSourceManager.NotfiyDataSourceChange("ds_report_auto_print_info");
                }
            }
        }

        void AutoPrintReportFrom_Unloaded(object sender, RoutedEventArgs e)
        {
            DataSourceManager.DisponseDataSource("ds_report_auto_print_info");
        }

        void AutoPrintReportFrom_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Wrapper.FullComboBox(this.cbbReportType, ReportManagerHelper.Instance.ReportTypeInfoItem, "report_type_name", "report_type_id", false, true);

                string currentDir =Environment.CurrentDirectory;
                DataListControl dl = new DataListControl();
                int result = dl.Initliaize(currentDir + @"\BusinessCfg\ReportManager\DataList\dl_report_auto_print_info.xml");
                if (result == 0)
                {
                    this.gbAutpPrintReport.Content = dl;
                }
                if (!string.IsNullOrEmpty(SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath))
                {
                    this.txtPathDisplay.Text = SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath;

                    if (!SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath.Contains(':'))
                    {
                        this.txtPathDisplay.Text = Environment.CurrentDirectory + SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath;
                    }
                }
                else
                {
                    this.txtPathDisplay.Text = Environment.CurrentDirectory + @"/Report/AutoSaverPath/";
                }
                if (!Directory.Exists(this.txtPathDisplay.Text))
                {
                    WriteLog.Log_Info("您设置的路径不存在，显示的路径为默认路径，有任何疑问请联系维护人员!" + this.txtPathDisplay.Text);
                    this.txtPathDisplay.Text = Environment.CurrentDirectory + @"/Report/AutoSaverPath/";
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        public override void UnLoadControls()
        {
            base.UnLoadControls();
            DataSourceManager.DisponseDataSource("ds_report_auto_print_info");
        }

        private void cbbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = this.cbbReportType.SelectedItem as ComboBoxItem;
            if (cbi != null)
            {
                BasiReportTypeInfo rsti = cbi.Tag as BasiReportTypeInfo;
                Wrapper.FullComboBox(this.cbbReportSubType,
                    ReportManagerHelper.Instance.ReportSubTypeInfoItemByReportTypeId1(rsti.report_type_id), "report_sub_type_name", "report_sub_type_id", false, true);
            }
        }

        private void cbbReportSubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = this.cbbReportSubType.SelectedItem as ComboBoxItem;
            if (cbi != null)
            {
                BasiReportSubTypeInfo rsti = cbi.Tag as BasiReportSubTypeInfo;
                if (rsti != null)
                {
                    Wrapper.FullComboBox(this.cbbReportName, ReportManagerHelper.Instance.ReportInfoItemByReportTypeIDAndSubTypeID1(rsti.report_type_id, rsti.report_sub_type_id), "report_name", "report_sub_type_id", false, true);
                }
            }

        }

        private void btnSaveAutoPrintTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String autoPrintTime = this.tpAutoPrintTime.Text;
                string time = Convert.ToDateTime(this.tpAutoPrintTime.Text).ToString("HH:mm:ss");
                SysConfig.GetSysConfig().ReportCfg.AutoPrintTime = time;

                int result = SysConfig.GetSysConfig().WrtieSysConfigFile();
                if (result == 0)
                {
                    Wrapper.ShowDialog("设置自动打印时间成功！", MessageBoxIcon.Information);
                }
                else
                {
                    Wrapper.ShowDialog("设置自动打印时间失败！", MessageBoxIcon.Information);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 设置保存路径
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSetSavePath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtPathDisplay.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 保存设置路径
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSavePath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string autoPrintPath = this.txtPathDisplay.Text;

                SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath = autoPrintPath;

                int result = SysConfig.GetSysConfig().WrtieSysConfigFile();
                if (result == 0)
                {
                    Wrapper.ShowDialog("设置自动保存路径成功！", MessageBoxIcon.Information);
                }
                else
                {
                    Wrapper.ShowDialog("设置自动保存路径失败！", MessageBoxIcon.Information);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }
    }
}
