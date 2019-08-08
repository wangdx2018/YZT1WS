using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AFC.BOM2.UIController;
using AFC.WS.UI.BR.Data;
using AFC.WS.UI.BR;
using AFC.WS.UI.CommonControls;

using AFC.WS.UI.DataSources;
using AFC.WS.UI.Config;
using System.Threading;
using AFC.WS.UI.BR.Data.PassengerFlow;
using AFC.WS.UI.Common;
using System.Data;
using AFC.WS.Model.DB;
using AFC.WS.BR.ReportManager;
using AFC.WS.BR;

namespace AFC.WS.UI.UIPage.ReportManager
{
    /// <summary>
    /// ReportManagerMain.xaml 的交互逻辑
    /// </summary>
    public partial class ReportManagerMain : UserControlBase
    {
        #region --> 属性、变量、构造方法。

      

        /// <summary>
        /// 日期枚举变量
        /// </summary>
        DateEnum de = DateEnum.None;

        /// <summary>
        /// 报表打印线程。
        /// </summary>
        Thread reportThread = null;

        /// <summary>
        /// 报表信息类
        /// </summary>
        BasiReportInfo ri = null;

        /// <summary>
        /// 参数条件内容类
        /// </summary>
        ParamCondition pc = null;

        /// <summary>
        /// 历史报表信息
        /// </summary>
        RptHistoryInfo rhi = null;

        bool IsBatchPrint = false;

        public ReportManagerMain()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ReportManagerMain_Loaded);
            this.Unloaded += new RoutedEventHandler(ReportManagerMain_Unloaded);
            ReportManagerHelper.Instance.ReportPrintEvent += new ReportPrintEventHandle(Instance_ReportPrintEvent);
            this.dtpRunDateBegin.DatePickerControl.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(RunDateBegin_SelectedDateChanged);
            this.dtpRunDateEnd.DatePickerControl.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(RunDateEnd_SelectedDateChanged);
            this.dtpTranDateBegin.DatePickerControl.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(TranDateBegin_SelectedDateChanged);
            this.dtpTranDateEnd.DatePickerControl.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(TranDateEnd_SelectedDateChanged);
            this.cbbDeviceType.SelectionChanged += new SelectionChangedEventHandler(cbbDeviceType_SelectionChanged);
            this.cbbStationId.SelectionChanged += new SelectionChangedEventHandler(cbbStation_SelectionChanged);
            Wrapper.Instance.CloseProcessByProcessName("excel");
            ReportManagerHelper.Instance.ReportPrintEvent+=new ReportPrintEventHandle(Instance_ReportPrintEvent);
        }

        void Instance_ReportPrintEvent(object sender, ReportPrintEventArgs e)
        {
            this.Dispatcher.Invoke(new ReportPrintEventHandle((object o, ReportPrintEventArgs rp) =>
            {
                if (e.Status == ReportStatus.CreateReportError)
                {
                    try
                    {
                        Wrapper.Instance.CloseProcessByProcessName("excel");
                        Wrapper.ShowDialog(e.MessageContent, MessageBoxIcon.Error);
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                }
                else
                {
                    this.lblMessage.Content = e.MessageContent;
                }
                if (IsBatchPrint)
                {
                    this.btnPrintReport.IsEnabled = e.IsBatchPrintComplete;
                    this.btnAutoPrintReport.IsEnabled = e.IsBatchPrintComplete;
                }
                else
                {
                    this.btnPrintReport.IsEnabled = e.IsComplete;
                }

            }), new object[] { sender, e });

        }

       private void cbbDeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            ComboBoxItem item = cbb.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                if (item.Content.ToString() != "全部")
                {
                    List<BasiDevInfo> list = BuinessRule.GetInstace().GetBasiDevInfo(this.cbbStationId.Text.Equals("全部") ? "%" :  BuinessRule.GetInstace().GetStationInfoByName(this.cbbStationId.Text).station_id, item.Uid);// BuinessRule.GetInstace().GetBasiDevTypeInfoByName(item.Uid).device_type);
                    Wrapper.FullComboBox(this.cbbDeviceID, list, "device_id", "device_id", true, true);
                }
                else
                {
                    List<BasiDevInfo> list = BuinessRule.GetInstace().GetBasiDevInfo(this.cbbStationId.Text.Equals("全部") ? "%" : BuinessRule.GetInstace().GetStationInfoByName(this.cbbStationId.Text).station_id, "%");
                    Wrapper.FullComboBox(this.cbbDeviceID, list, "device_id", "device_id", true, true);
                }
            }
        }

       private void cbbStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
           ComboBox cbb = sender as ComboBox;
           ComboBoxItem item = cbb.SelectedItem as ComboBoxItem;
           if (item != null)
           {
               if (item.Content.ToString() != "全部")
               {
                   List<BasiDevInfo> list = BuinessRule.GetInstace().GetBasiDevInfo(item.Uid, this.cbbDeviceType.Text.Equals("全部") ? "%" : BuinessRule.GetInstace().GetBasiDevTypeInfoByName(this.cbbDeviceType.Text).device_type);
                   Wrapper.FullComboBox(this.cbbDeviceID, list, "device_id", "device_id", true, true);
               }
               else
               {
                   List<BasiDevInfo> list = BuinessRule.GetInstace().GetBasiDevInfo("%", this.cbbDeviceType.Text.Equals("全部") ? "%" : BuinessRule.GetInstace().GetBasiDevTypeInfoByName(this.cbbDeviceType.Text).device_type);
                   Wrapper.FullComboBox(this.cbbDeviceID, list, "device_id", "device_id", true, true);
               }
           }
       }

        #endregion --> 属性、变量、构造方法。

        #region --> 事件处理。

        void ReportManagerMain_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportManagerHelper.Instance.Clear();
                ri = null;
                pc = null;
                rhi = null;
                ReportManagerHelper.Instance.ReportPrintEvent -= new ReportPrintEventHandle(Instance_ReportPrintEvent);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        void ReportManagerMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.InitStationAreaInfo();
                InitDateTimePickerExtend();
                InitTreeView();
                ReportManagerHelper.Instance.InitControlIsEnabled(this.gParamCondition, false);
                btnAutoPrintReportSet.IsEnabled = true;
                btnAutoPrintReport.IsEnabled = true;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }


     

        //private void Instance_ReportPrintEvent(object sender, ReportPrintEventArgs e)
        //{
        //    this.Dispatcher.Invoke(new ReportPrintEventHandle((object o, ReportPrintEventArgs rp) =>
        //    {
        //        if (e.Status == ReportStatus.CreateReportError)
        //        {
        //            try
        //            {
        //             //   Wrapper.Instance.CloseProcessByProcessName("excel");
        //                Wrapper.ShowDialog(e.MessageContent, MessageBoxIcon.Error);
        //            }
        //            catch (Exception ee)
        //            {
        //                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
        //            }
        //        }
        //        else
        //        {
        //            this.lblMessage.Content = e.MessageContent;
        //        }
        //        if (IsBatchPrint)
        //        {
        //            this.btnPrintReport.IsEnabled = e.IsBatchPrintComplete;
        //            this.btnAutoPrintReport.IsEnabled = e.IsBatchPrintComplete;
        //        }
        //        else
        //        {
        //            this.btnPrintReport.IsEnabled = e.IsComplete;
        //        }

        //    }), new object[] { sender, e });

        //}

        #region --> 日期选择事件。

        void TranDateBegin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateBegin(Wrapper.GetDateTimePickerValue(this.dtpTranDateBegin), dtpTranDateEnd);
        }

        void TranDateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateEnd(Wrapper.GetDateTimePickerValue(this.dtpTranDateEnd), this.dtpTranDateBegin);
            de = DateEnum.None;
        }

        void RunDateBegin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateBegin(Wrapper.GetDateTimePickerValue(this.dtpRunDateBegin), dtpRunDateEnd);
            de = DateEnum.None;
        }

        void RunDateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDateEnd(Wrapper.GetDateTimePickerValue(this.dtpRunDateEnd), this.dtpRunDateBegin);
        }

        #endregion --> 日期选择事件。

        #endregion --> 事件处理。

        #region --> 初始化操作。

        /// <summary>
        /// 初始化TreeView控件
        /// </summary>
        void InitTreeView()
        {
            List<BasiReportTypeInfo> item = ReportManagerHelper.Instance.ReportTypeInfoItem;
            ReportManagerHelper.Instance.SetTreeViewReportTypeInfo(this.tvRoot, item);
        }


        /// <summary>
        /// 初始化车站信息
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        private bool InitStationAreaInfo()
        {
            try
            {
                #region init station combox

                List<BasiStationInfo> stationList = BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
                Wrapper.FullComboBox(this.cbbStationId,
                  stationList,
                "station_cn_name",
                "station_id",
                true, true);


                this.cbbDeviceType.SelectedIndex = 0;
                this.cbbDeviceType.IsEnabled = false;

                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SCWS"))
                {
                    this.cbbStationId.IsEnabled = false;
                    Wrapper.ComboBoxSelectedItem(this.cbbStationId, SysConfig.GetSysConfig().LocalParamsConfig.StationCode);

                }

                #endregion

                #region init buiness type
                Wrapper.FullComboBox(this.cbbBissType,
                  BuinessRule.GetInstace().GetAllBasiTranTypeInfos(),
                  "afc_name",
                  "afc_type",
                  true, true);
                #endregion

                #region init operator
               
                Wrapper.FullComboBox(this.cbbOperatorId,
                    BuinessRule.GetInstace().GetAllOperatorInfo(), "operator_id", "operator_id", true, true);
                #endregion

                #region init device id
                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SCWS"))
                {
                    Wrapper.FullComboBox(this.cbbDeviceID,
                        BuinessRule.GetInstace().GetBasiDevInfoOnlySLEDevice(
                        SysConfig.GetSysConfig().LocalParamsConfig.StationCode), "device_id", "device_id", true, true);
                }
                else 
                {
                    Wrapper.FullComboBox(this.cbbDeviceID,
                      BuinessRule.GetInstace().GetBasiDevInfoOnlySLEDevice(
                      "%"), "device_id", "device_id", true, true);              
                }
                #endregion

                #region init tick type info

                Wrapper.FullComboBox(this.cbbProductTypeId,
                    BuinessRule.GetInstace().GetBasiTickManaTypeInfo(false), "tick_mana_type_name", "tick_mana_type", true, true);

                #endregion

                return true;
            }

            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return false;
            }
        }

        /// <summary>
        /// 初始化日期控件
        /// </summary>
        void InitDateTimePickerExtend()
        {
            Wrapper.SetDateTimePickerExtend(this.dtpRunDate, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpRunDateBegin, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpRunDateEnd, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpTranDate, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpTranDateBegin, DateTimeType.Minutes, 0);
            Wrapper.SetDateTimePickerExtend(this.dtpTranDateEnd, DateTimeType.Minutes, 0);

            this.dtpRunDate.DatePickerControl.SelectedDate = DateTime.Parse(BuinessRule.GetInstace().rm.GetRunDate());

         
        }


        #endregion --> 初始化操作。

        #region --> 选择报表事件。

        private void tvRoot_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.cbbStationId.CanReadOnly = true;
            Wrapper.ComboBoxSelectedItem(this.cbbStationId, SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            this.InitStationAreaInfo();
            this.lblMessage.Content = null;
            TreeViewItem item = this.tvRoot.SelectedItem as TreeViewItem;
            ReportManagerHelper.Instance.SetParamConditionControl(this.gParamCondition, item, this.lblReportName);
            try
            {
               
                SetDateEnd(DateTime.Now, this.dtpTranDateBegin);
                SetDateEnd(DateTime.Now, this.dtpRunDateBegin);
                SetDateBegin(Wrapper.GetDateTimePickerValue(this.dtpTranDateBegin), dtpTranDateEnd);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            btnAutoPrintReportSet.IsEnabled = true;
            btnAutoPrintReport.IsEnabled = true;        //最好是判断已经运营结束了，此控件可以使用。
        }

        #endregion --> 选择报表事件。

        #region --> Button 控件单击事件。

        private void btnPrintReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsBatchPrint = false;
                TreeViewItem item = this.tvRoot.SelectedItem as TreeViewItem;
                if (item == null)
                {
                    return;
                }
                ri = item.Tag as BasiReportInfo;
                if (ri == null)
                {
                    return;
                }
                if (this.Checked(ri) == false)
                {
                    return;
                }
                pc = null;
                pc = this.GetParamCondition(ri);

                //if (this.tpTimeBegin.IsEnabled == true && this.tpTimeEnd.IsEnabled == true)
                //{

                //    if (pc.TimeScope.TimeBegin.Equals(pc.TimeScope.TimeEnd))
                //    {
                //        Wrapper.ShowDialog("起始时间与结束时间相同。");
                //        return;
                //    }
                //    if (pc.TimeScope.TimeEnd.ToInt32() < pc.TimeScope.TimeBegin.ToInt32())
                //    {
                //        Wrapper.ShowDialog("起始时间大于结束时间。");
                //        return;
                //    }
                //}

                rhi = new RptHistoryInfo();
                rhi.report_add_date = DateTime.Now.ToString("yyyyMMdd");
                rhi.report_add_time = DateTime.Now.ToString("HHmmss");
                rhi.report_name = this.lblReportName.Content.ToString();
                rhi.report_path = SysConfig.GetSysConfig().ReportCfg.HistoryReportPath + "/" + rhi.report_save_name;
                rhi.report_sub_type_id = ri.report_sub_type_id;
                rhi.report_type_id = ri.report_type_id;
                rhi.station_id = Wrapper.GetComboBoxUid(this.cbbStationId);
                rhi.report_condition_param = ri.report_condition_param;
                string paramValue = ReportManagerHelper.Instance.GetControlvalue(pc, ri.report_condition_param);
                rhi.report_param_value = paramValue.Substring(0, paramValue.LastIndexOf("_"));

                pc.ReportInfo = ri;
                pc.HistoryInfo = rhi;

                reportThread = new Thread(new ThreadStart(ReportPrint));
                reportThread.Start();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                MessageBox.Show(string.Format("exception source={0},exception trace={1}", ex.Source, ex.StackTrace));
                return;
            }

        }

        private void btnAutoPrintReport_Click(object sender, RoutedEventArgs e)
        {
            IsBatchPrint = true;
            reportThread = new Thread(new ThreadStart(ReportManagerHelper.Instance.ReportAutoPrint));
            reportThread.Start();
        }

        private void btnAutoPrintReportSet_Click(object sender, RoutedEventArgs e)
        {
            AutoPrintReportFrom aprf = new AutoPrintReportFrom();
            BaseWindow bw = new BaseWindow();
            bw.Content = aprf;
            bw.Width = 820;
            bw.Height = 550;
            bw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bw.ShowDialog();
            bw.Closed += new EventHandler(bw_Closed);
        }

        void bw_Closed(object sender, EventArgs e)
        {
            DataSourceManager.DisponseDataSource("ds_report_auto_print_info");
        }

        private void btnSetAutoPrint_Click(object sender, RoutedEventArgs e)
        {
            if (this.lblReportName.Content == null)
            {
                return;
            }
        }

        #endregion --> Button 控件单击事件。

        /// <summary>
        /// 合法性验证
        /// </summary>
        /// <returns>true-合法；false-非法</returns>
        private bool Checked(BasiReportInfo ri)
        {
            switch (ri.report_frequency_type)
            {
                case "01":      //时实
                    //-->判断是否可用。
                    //-->判断trandate控件是否可用。
                    if (this.dtpTranDateBegin.IsEnabled == true && this.dtpTranDateEnd.IsEnabled == true)
                    {
                        //-->判断两个日期是否相同。
                        String dtTranBegin = Wrapper.GetDateTimePickerValue(dtpTranDateBegin).ToyyyyMMdd();
                        String dtTranEnd = Wrapper.GetDateTimePickerValue(dtpTranDateEnd).ToyyyyMMdd();
                        if (dtTranBegin.Equals(dtTranEnd))
                        {
                            //-->说明是同一天，再判断两个时间。
                            if (!CheckedTime())
                            {
                                return false;
                            }
                        }
                    }

                    //-->判断runDate控件是否可用。
                    if (this.dtpRunDateBegin.IsEnabled == true && this.dtpRunDateEnd.IsEnabled == true)
                    {
                        //-->判断两个日期是否相同。
                        String dtRunDateBegin = Wrapper.GetDateTimePickerValue(this.dtpRunDateBegin).ToyyyyMMdd();
                        String dtRunDateEnd = Wrapper.GetDateTimePickerValue(this.dtpRunDateEnd).ToyyyyMMdd();
                        if (dtRunDateBegin.Equals(dtRunDateEnd))
                        {
                            //-->说明是同一天，再判断两个时间。
                            if (!CheckedTime())
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case "02":      //日报
                    CheckedTime();

                    break;
                case "03":

                    break;
                default:

                    break;
            }
            return true;
        }

        /// <summary>
        /// 判断时间选择是否正确。
        /// </summary>
        /// <returns></returns>
        private bool CheckedTime()
        {
            if (tpTimeBegin.IsEnabled == true && tpTimeEnd.IsEnabled == true)
            {
                string timeBegin = this.tpTimeBegin.SelectedTime.ToString().Replace(":", "");
                string timeEnd = this.tpTimeEnd.SelectedTime.ToString().Replace(":", "");

                string TimeBegin = timeBegin.LastIndexOf('.') > 0 ? timeBegin.Substring(0, timeBegin.LastIndexOf('.')) : timeBegin;
                string TimeEnd = timeEnd.LastIndexOf('.') > 0 ? timeEnd.Substring(0, timeEnd.LastIndexOf('.')) : timeEnd;

                if (TimeBegin.Equals(TimeEnd))
                {
                    Wrapper.ShowDialog("起始时间与结束时间相同。");
                    return false;
                }
                if (TimeBegin.ToInt32() > TimeEnd.ToInt32())
                {
                    Wrapper.ShowDialog("起始时间大于结束时间。");
                    return false;
                }
            }
            return true;
        }

        #region --> 方法。

        string currentMonth;
        string currentYear;

        /// <summary>
        /// 报表打印
        /// </summary>
        void ReportPrint()
        {
            try
            {
                //-->1、创建报表。
                ReportManagerHelper.Instance.ReportCreate(ri.report_name, pc);
                WriteLog.Log_Info("ReportPrint创建报表:" + ri.report_name);
                ReportManagerHelper.Instance.ReportOpen(ri.report_name, ReportManagerHelper.Instance.LocationReportFilePath);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }
        }

        /// <summary>
        /// 设置开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtp"></param>
        void SetDateBegin(DateTime dt, DateTimePickerExtend dtp)
        {
            TreeViewItem item = this.tvRoot.SelectedItem as TreeViewItem;
            if (item == null)
            {
                return;
            }
            BasiReportInfo temp = item.Tag as BasiReportInfo;
            if (temp != null)
            {
                switch (temp.report_frequency_type)
                {
                    case "01":  //01-时实
                        if (tpTimeBegin.IsEnabled)
                        {
                            this.tpTimeBegin.Text = "02:00:00";
                        }
                        break;
                    case "02":  //02-日报
                        if (de == DateEnum.RunDateDay)
                        {
                            break;
                        }
                        de = DateEnum.RunDateDay;
                        dtp.DatePickerControl.SelectedDate = dt;//dt.AddDays(1);
                        break;
                    case "03":  //03-周报
                        if (de == DateEnum.RunDateWeek)
                        {
                            break;
                        }
                        de = DateEnum.RunDateWeek;
                        int week = ReportManagerHelper.Instance.GetWeekOfYear(dt);
                        int day = (week - 1) * 7 - DateTime.Now.DayOfYear + 2;
                        DateTime dtWeek = DateTime.Now.AddDays(day);
                        dtp.DatePickerControl.SelectedDate = dtWeek;
                        break;
                    case "04":  //04-月报；
                        int days = DateTime.DaysInMonth(dt.Year, dt.Month);
                        DateTime dtDays = DateTime.ParseExact(dt.Year.ToString() + dt.Month.ToString("00") + days.ToString("00"), "yyyyMMdd", null);
                        dtp.DatePickerControl.SelectedDate = dtDays;

                        this.currentMonth = dtDays.ToString("yyyy年MM月");
                        this.currentYear = dt.Year.ToString() + "年";
                        break;
                    case "05":  //05-年报
                        DateTime dtYear = DateTime.ParseExact(dt.Year.ToString() + "1231", "yyyyMMdd", null);
                        dtp.DatePickerControl.SelectedDate = dtYear;
                        this.currentYear = dtYear.Year.ToString() + "年";
                        break;
                    default:
                        break;
                }
                de = DateEnum.None;
            }
        }

        /// <summary>
        /// 设置结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtp"></param>
        void SetDateEnd(DateTime dt, DateTimePickerExtend dtp)
        {
            TreeViewItem item = this.tvRoot.SelectedItem as TreeViewItem;
            if (item == null)
            {
                return;
            }
            BasiReportInfo temp = item.Tag as BasiReportInfo;
            if (temp != null)
            {
                switch (temp.report_frequency_type)
                {
                    case "01":  //01-时实
                        if (this.tpTimeEnd.IsEnabled == true)
                        {
                            this.tpTimeEnd.Text = "02:00:00";
                        }
                        break;
                    case "02":  //02-日报
                        if (de == DateEnum.RunDateDay)
                        {
                            break;
                        }
                        de = DateEnum.RunDate;
                        dtp.DatePickerControl.SelectedDate = dt;//dt.AddDays(-0);
                        break;
                    case "03":  //03-周报
                        if (de == DateEnum.RunDateWeek)
                        {
                            break;
                        }
                        de = DateEnum.RunDateWeek;
                        int week = ReportManagerHelper.Instance.GetWeekOfYear(dt);
                        int days = (week - 2) * 7 - DateTime.Now.DayOfYear + 3;
                        DateTime dtWeek = DateTime.Now.AddDays(days);
                        dtp.DatePickerControl.SelectedDate = dtWeek;
                        break;
                    case "04":  //04-月报
                        DateTime tempdt = DateTime.ParseExact(dt.Year.ToString() + "-" + dt.Month.ToString("00") + "-01", "yyyy-MM-dd", null);
                        dtp.DatePickerControl.SelectedDate = tempdt;
                        this.currentMonth = tempdt.ToString("yyyy年MM月");
                        this.currentYear = dt.Year.ToString() + "年";
                        break;
                    case "05":  //05-年报
                        DateTime year = DateTime.ParseExact(dt.Year.ToString() + "-01-01", "yyyy-MM-dd", null);
                        dtp.DatePickerControl.SelectedDate = year;
                        this.currentYear = dt.Year.ToString() + "年";
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获取当前的参数条件
        /// </summary>
        /// <returns></returns>
        ParamCondition GetParamCondition(BasiReportInfo ri)
        {
          
            ParamCondition pc = new ParamCondition();
            pc.LineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
         //   pc.DeviceID=WaitCallback
            pc.StationID = Wrapper.GetComboBoxUid(this.cbbStationId);
            pc.LineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
            {
                BasiStationInfo si = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
                pc.StationName = si != null ? si.station_cn_name : "全部";// Wrapper.GetComboBoxText(this.cbbStationId);
                pc.StationID = si != null ? si.station_id : "%";// Wrapper.GetComboBoxUid(this.cbbStationId);
            }
            else
            {
                pc.StationID = Wrapper.GetComboBoxUid(this.cbbStationId);
                BasiStationInfo si = BuinessRule.GetInstace().GetStationInfoById(pc.StationID);
                pc.StationID = si.station_id != null ? si.station_id : "%";
                pc.StationName = si.station_cn_name != null ? si.station_cn_name : "全部";// Wrapper.GetComboBoxText(this.cbbStationId);
            }
            pc.SystemDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            pc.ReportName = this.lblReportName.Content.ToString();
            pc.TimeInterval = Wrapper.GetComboBoxUid(this.cbbTimeInterval).ToInt32();

            pc.CardIssuerId = this.cbbCardIssuerId.Uid.ToString();

            //-->获取交易类型及交易子类型。
            BasiTranTypeInfo data = Wrapper.Instance.GetComboBoxTValue<BasiTranTypeInfo>(this.cbbBissType);
            if (data != null)
            {
                pc.BissType = data.afc_type;
                pc.BissSubType = "";
                pc.BissTypeName = data.afc_name;
                pc.BissSubTypeName = "";
            }
            else
            {
                pc.BissSubType = "%";
                pc.BissType = "%";
                pc.BissSubTypeName = "全部";
                pc.BissTypeName = "全部";
            }

            BasiTickManaTypeInfo bp = Wrapper.Instance.GetComboBoxTValue<BasiTickManaTypeInfo>(this.cbbProductTypeId);
            if (bp != null)
            {
                pc.ProductTypeName = bp.tick_mana_type_name;
                pc.ProductTypeId = bp.tick_mana_type.ToString();
                pc.CardIssuerId = bp.card_issue_id;
            }
            else
            {
                pc.ProductTypeId = "%";
                pc.ProductTypeName = "全部";
                pc.CardIssuerId = "%";
            }
            try
            {
                BasiDevInfo bd = Wrapper.Instance.GetComboBoxTValue<BasiDevInfo>(this.cbbDeviceID);
                if (bd != null)
                {
                    pc.DeviceID = bd.device_id;
                    pc.DeviceCode = bd.device_id;
                    pc.DeviceType = BuinessRule.GetInstace().GetDevTypeInfoById(bd.device_type).device_type;
                    pc.DeviceTypeName = BuinessRule.GetInstace().GetDevTypeInfoById(bd.device_type).device_name;
                }
                else
                {
                    pc.DeviceID = "%";
                    pc.DeviceCode = "全部";
                    pc.DeviceType = "%";
                    pc.DeviceTypeName = "全部";
                }

                if (!string.IsNullOrEmpty(Wrapper.GetComboBoxText(this.cbbDeviceID)) && !"%".Equals(Wrapper.GetComboBoxUid(this.cbbDeviceID)))
                {
                    pc.DeviceID = Wrapper.GetComboBoxUid(this.cbbDeviceID);
                    pc.DeviceCode = Wrapper.GetComboBoxUid(this.cbbDeviceID);
                }
                else
                {
                    pc.DeviceID = "%";
                    pc.DeviceCode = "全部";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("ex msg={0},ex traceSource={1}", ex.Message, ex.StackTrace));
            }

            //-->获取操作员ID号。
            pc.OperatorId = Wrapper.GetComboBoxUid(this.cbbOperatorId);
            pc.OperatorIdName = Wrapper.GetComboBoxText(this.cbbOperatorId);

            //-->获取时间段。
            pc.TimeScope = this.GetDateTimeScope();// dts;

            if (cbbDeviceType.IsEnabled == true)
            {
                pc.DeviceType = Wrapper.GetComboBoxUid(this.cbbDeviceType);
                pc.DeviceTypeName = Wrapper.GetComboBoxText(this.cbbDeviceType);
            }

            switch (ri.report_frequency_type)//.Equals("01")) //时实
            {
                case "01":
                    pc.StatisticsBeginDatetime = GetDateTime(dtpTranDateBegin).ToString("yyyy-MM-dd") + " " +
                        pc.TimeScope.TimeBegin.FormatToTime();
                    pc.StatisticsEndDatetime = GetDateTime(dtpTranDateEnd).ToString("yyyy-MM-dd") + " " +
                        pc.TimeScope.TimeEnd.FormatToTime();

                    pc.StatisticsDateTimeScope = pc.StatisticsBeginDatetime + "~" + pc.StatisticsEndDatetime;
                    break;

                case "02":      //02-日报

                    break;
            }
            GetStatisticsDatetimeScope(ri, pc);//获取统计时间

            //-->获取文件路径。 第一获取报表类型、第二获取报表子类型、第三就是报表名称。
            BasiReportTypeInfo rti = ReportManagerHelper.Instance.GetReportTypeInfoByPK(ri.report_type_id);
            string reportTypeName = rti.report_type_name;
            BasiReportSubTypeInfo rsti = ReportManagerHelper.Instance.GetReportSubTypeInfoByPK(ri.report_type_id, ri.report_sub_type_id);
            string reportSubTypeName = rsti.report_sub_type_name;
            
            pc.LocationTemplateFilePath = Environment.CurrentDirectory +
                SysConfig.GetSysConfig().ReportCfg.LocalReportPath +
                reportTypeName + "\\" +
                reportSubTypeName + "\\" +
                ri.report_name + ".xls";

            /*
            pc.ReportSaveFilePath = Environment.CurrentDirectory +
                SysConfig.GetSysConfig().ReportCfg.ReportTempPath; */

            //存到文件夹名称为当前日期的一个文件夹下，文件夹路径为当前桌面
            pc.ReportSaveFilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" +
               DateTime.Now.ToString("yyyyMMdd") + "\\"; 
            return pc;
        }

        /// <summary>
        /// 获取统计时间
        /// </summary>
        /// <param name="ri"></param>
        /// <param name="pc"></param>
        private void GetStatisticsDatetimeScope(BasiReportInfo ri, ParamCondition pc)
        {
            string[] condition = ri.report_condition_param.Split('|');
            if (ri.report_frequency_type.Equals("01")) //时实
            {
                if (dtpTranDateBegin.IsEnabled)
                {
                    pc.StatisticsBeginDatetime = GetDateTime(dtpTranDateBegin).ToString("yyyy-MM-dd") + " " +
                        pc.TimeScope.TimeBegin.FormatToTime();
                    pc.StatisticsEndDatetime = GetDateTime(dtpTranDateEnd).ToString("yyyy-MM-dd") + " " +
                        pc.TimeScope.TimeEnd.FormatToTime();
                }
                else
                {
                pc.StatisticsBeginDatetime = GetDateTime(dtpRunDateBegin).ToString("yyyy-MM-dd") + " " +
                    pc.TimeScope.TimeBegin.FormatToTime();
                pc.StatisticsEndDatetime = GetDateTime(dtpRunDateEnd).ToString("yyyy-MM-dd") + " " +
                    pc.TimeScope.TimeEnd.FormatToTime();
                }
                

            }
            else if (ri.report_frequency_type.Equals("02") ||
                        ri.report_frequency_type.Equals("04"))
            {
                //02-日报

                if (condition != null && condition.Length > 0)
                {
                    foreach (string s in condition)
                    {
                        switch (ri.report_frequency_type)
                        {
                            case "02":
                                switch (s.ToLower())
                                {
                                    case "rundate":
                                        pc.StatisticsBeginDatetime = GetDateTime(dtpRunDate).ToString("yyyy-MM-dd");
                                        pc.StatisticsEndDatetime = pc.StatisticsBeginDatetime;
                                        break;
                                    case "trandate":
                                        pc.StatisticsBeginDatetime = GetDateTime(dtpTranDate).ToString("yyyy-MM-dd");
                                        pc.StatisticsEndDatetime = pc.StatisticsBeginDatetime;
                                        break;
                                }

                                break;
                            case "04":
                                switch (s.ToLower())
                                {
                                    case "trandatebegin":
                                        pc.StatisticsBeginDatetime = GetDateTime(dtpTranDateBegin).ToString("yyyy-MM-dd");
                                        break;
                                    case "trandateend":
                                        pc.StatisticsEndDatetime = GetDateTime(dtpTranDateEnd).ToString("yyyy-MM-dd");
                                        break;


                                    case "rundatebegin":
                                        pc.StatisticsBeginDatetime = GetDateTime(dtpRunDateBegin).ToString("yyyy-MM-dd");
                                        break;

                                    case "rundateend":
                                        pc.StatisticsEndDatetime = GetDateTime(dtpRunDateEnd).ToString("yyyy-MM-dd");
                                        break;
                                }
                                break;
                        }//end switch;
                    }//end foreacth
                }//end if
            }
            if (ri.report_name == "车站进出站量实时统计报表")
            {

                pc.StatisticsDateTimeScope = BuinessRule.GetInstace().rptManager.DateTimeFormat(pc.TimeScope.TranDate).ToyyyyMMdd() + " " + pc.TimeScope.TimeBegin.FormatToTime() + "~" + 
                    BuinessRule.GetInstace().rptManager.DateTimeFormat(pc.TimeScope.TranDate).ToyyyyMMdd() + " " + pc.TimeScope.TimeEnd.FormatToTime();
            }
            else
                pc.StatisticsDateTimeScope = pc.StatisticsBeginDatetime + "~" + pc.StatisticsEndDatetime;
        }

        /// <summary>
        /// 获取时间段。
        /// </summary>
        /// <returns></returns>
        DateTimeScope GetDateTimeScope()
        {
            DateTimeScope dts = new DateTimeScope();
            dts.TranDateBegin = GetDateTime(dtpTranDateBegin).ToString("yyyyMMdd");
            dts.TranDateEnd = GetDateTime(dtpTranDateEnd).ToString("yyyyMMdd");
            dts.TranDate = GetDateTime(dtpTranDate).ToString("yyyyMMdd");

            string timeBegin = this.tpTimeBegin.SelectedTime.ToString().Replace(":", "");
            dts.TimeBegin = timeBegin.LastIndexOf('.') > 0 ? timeBegin.Substring(0, timeBegin.LastIndexOf('.')) : timeBegin;
            string timeEnd = this.tpTimeEnd.SelectedTime.ToString().Replace(":", "");
            dts.TimeEnd = timeEnd.LastIndexOf('.') > 0 ? timeEnd.Substring(0, timeEnd.LastIndexOf('.')) : timeEnd;

            dts.RunDateBegin = GetDateTime(dtpRunDateBegin).ToString("yyyyMMdd");
            dts.RunDateEnd = GetDateTime(dtpRunDateEnd).ToString("yyyyMMdd");

            dts.Year = this.currentYear;
            dts.Month = this.currentMonth;

            try
            {
                dts.RunDate = GetDateTime(dtpRunDate).ToString("yyyyMMdd");
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            return dts;
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="dtp"></param>
        /// <returns></returns>
        DateTime GetDateTime(DateTimePickerExtend dtp)
        {
            return dtp.ContentDatePicker.JudgeIsNullOrEmpty() == true ?
                DateTime.Parse("1900-01-01") : Wrapper.GetDateTimePickerValue(dtp);
        }

        #endregion --> 方法
    }



    /// <summary>
    /// 日期枚举
    /// </summary>
    enum DateEnum
    {
        None = 0,

        RunDate = 2,
        RunDateDay = 21,
        RunDateWeek = 22,
        RunDateMonth = 23,
        RunDateYear = 24,

        TranDate = 3,
        TranDateDay = 31,
        TranDateWeek = 32,
        TranDateMonth = 33,
        TranDateYear = 34,

    }
}
