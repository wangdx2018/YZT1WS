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
using AFC.WS.UI.Common;
using AFC.WS.UI.Components;
using AFC.WS.BR.PassengerFlow;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.UI.BR.Data.PassengerFlow;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Data;
using AFC.WS.ModelView.Convertors;

namespace AFC.WS.UI.UIPage.PassengerFlow
{
    /// <summary>
    /// PassengerFlowHistoryInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PassengerFlowHistoryInfo : UserControlBase, IChartDataSource
    {

        #region --> 属性。

        /// <summary>
        /// 按票种进站统计
        /// </summary>
        ChartControl ccEntryPie = null;
        /// <summary>
        /// 按票种出站统计
        /// </summary>
        ChartControl ccExitPit = null;
        /// <summary>
        /// 按客流种类统计
        /// </summary>
        ChartControl ccLine = null;
        /// <summary>
        /// 历史客流查询结果.
        /// </summary>
        List<HistoryPassengerFlowData> PassengerFlowQueryResultList;
        /// <summary>
        /// 客流统计类型。
        /// </summary>
        PassengerFlowCounterEnum CounterType = PassengerFlowCounterEnum.None;

        #endregion --> 属性。

        #region IChartDataSource 成员

        public void GetDataSource(out List<object> XValue, out List<List<string>> YValue, int Interval)
        {
            string beginTime = "开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            PassengerFlowHelper.HistorySetPassengerFlowLine(out XValue, out YValue, Interval);
            beginTime += "-->结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            Wrapper.Instance.ConsoleWriteLine(beginTime, LogFlag.InfoFormat);
        }
        #endregion

        /// <summary>
        /// 构造函数。
        /// </summary>
        public PassengerFlowHistoryInfo()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PassengerFlowHistoryInfo_Loaded);
        }

        /// <summary>
        /// 控件的Load事件。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void PassengerFlowHistoryInfo_Loaded(object sender, RoutedEventArgs e)
        {
            PassengerFlowHelper.HistoryPassengerFlowQuery += new HistoryPassengerFlowQueryResultEventHandler(PFH_HistoryPassengerFlowQuery);
            PassengerFlowHelper.HistoryPassengerFlowPie += new HistoryPassengerFlowPieEventHandler(PFH_HistoryPassengerFlowPie);
            PassengerFlowHelper.PassengerFlowNumber += new PassengerFlowNumberEventHandler(PFH_PassengerFlowNumber);
            Wrapper.SetDateTimePickerExtend(this.dtpBegin, DateTimeType.Day, -1);
            Wrapper.SetDateTimePickerExtend(this.dtpEnd, DateTimeType.Day, 0);
            this.timeBegin.SetControlValue(DateTime.Now.ToString("HH:mm:ss"));
            this.timeEnd.SetControlValue(DateTime.Now.ToString("HH:mm:ss"));

            Initload();
        }

        /// <summary>
        /// 客流数量
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PFH_PassengerFlowNumber(object sender, PassengerFlowNumberEventArgs e)
        {
            this.dgMonitorTotal.ItemsSource = null;
            this.dgMonitorTotal.AutoGenerateColumns = false;
            this.dgMonitorTotal.IsReadOnly = true;
            this.dgMonitorTotal.ItemsSource = e.PassengerFlowNumberItem;
        }

        /// <summary>
        /// 历史客流查询结果饼图数据事件委托。
        /// </summary>
        /// <param name="sender">存放当前类内容 this</param>
        /// <param name="e">历史客流查询结果饼图事件类</param>
        private void PFH_HistoryPassengerFlowPie(object sender, HistoryPassengerFlowPieEventArgs e)
        {
            List<DataSeriesProperty> dspList = new List<DataSeriesProperty>();
            foreach (HistoryPassengerFlowPieData d in e.HpList)
            {
                DataSeriesProperty dsp = new DataSeriesProperty();
                dsp.LegnedName = PassengerFlowHelper.getFormatText(d.Card_issue_name + d.Product_type_name_cn);
                dspList.Add(dsp);
            }
            //-->修改DataSeries的个数。
            if (e.PieType == PieEnum.Entry)
            {
                ccEntryPie.config.DataSeriesList = dspList;
            }
            else if (e.PieType == PieEnum.Exit)
            {
                ccExitPit.config.DataSeriesList = dspList;
            }
        }

        /// <summary>
        /// 历史客流查询结果数据事件委托。
        /// </summary>
        /// <param name="sender">存放当前类内容 this</param>
        /// <param name="e">历史客流查询结果事件类</param>
        private void PFH_HistoryPassengerFlowQuery(object sender, HistoryPassengerFlowQueryResultEventArgs e)
        {
            PassengerFlowQueryResultList = e.PassengerFlowQueryResultList;
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            PassengerFlowHelper.HistoryPassengerFlowQuery -= new HistoryPassengerFlowQueryResultEventHandler(PFH_HistoryPassengerFlowQuery);
            PassengerFlowHelper.PassengerFlowNumber -= new PassengerFlowNumberEventHandler(PFH_PassengerFlowNumber);
            base.UnLoadControls();
            this.ccLine.ShutThread();
            this.ccExitPit.ShutThread();
            this.ccEntryPie.ShutThread();
           
        }

        #region --> 翻页。

        void SetCurrentPageIndexInfo()
        {
            ModifyQueryDateTime();
            lblCurrentPageIndex.Content = "当前第" + PassengerFlowHelper.HistoryPageCurrentIndex + "页";
            if (ccLine != null)
            {
                ccLine.Refresh();
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (PassengerFlowHelper.HistoryPageCurrentIndex > 1 &&
                   PassengerFlowHelper.HistoryPageCurrentIndex <= PassengerFlowHelper.HistoryPageCount)
            {
                PassengerFlowHelper.HistoryPageCurrentIndex--;
                SetCurrentPageIndexInfo();
            }
        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            if (PassengerFlowHelper.HistoryPageCurrentIndex > 0 &&
                  PassengerFlowHelper.HistoryPageCurrentIndex < PassengerFlowHelper.HistoryPageCount)
            {
                PassengerFlowHelper.HistoryPageCurrentIndex++;
                SetCurrentPageIndexInfo();
            }
        }

        #endregion --> 翻页。

        private void cbbStationCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string stationid = Wrapper.GetComboBoxUid(this.cbbStationCode);
            this.cbbStationHall.IsEnabled = true;
            if (stationid == "%")
            {
                stationid = "";
                this.cbbStationHall.Items.Clear();
                this.cbbStationHall.IsEnabled = false;
            }
            else
            {
                if (BuinessRule.GetInstace().GetBasiStationHall(stationid) != null)
                {
                    Wrapper.FullComboBox(this.cbbStationHall,
                        BuinessRule.GetInstace().GetBasiStationHall(stationid), "station_hall_name", "station_hall_id");
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //-->判断开始时间是否大于结束时间。

      
            PassengerFlowHelper.initStartDateTime = null;
            PassengerFlowHelper.initEndDateTime = null;
            string dateBegin = Convert.ToDateTime(this.dtpBegin.ContentDatePicker).ToString("yyyy-MM-dd");
            string timeBegin = Convert.ToDateTime(this.timeBegin.Text).ToString("HH:mm:ss");
            DateTime _b = DateTime.Parse(dateBegin + " " + timeBegin);

            PassengerFlowHelper.initStartDateTime = _b.ToString("yyyMMddHHmm");
            string dateEnd = Convert.ToDateTime(this.dtpEnd.ContentDatePicker).ToString("yyyy-MM-dd");
            string timeEnd = Convert.ToDateTime(this.timeEnd.Text).ToString("HH:mm:ss");
            DateTime _e = DateTime.Parse(dateEnd + " " + timeEnd);

            PassengerFlowHelper.initEndDateTime = _e.ToString("yyyMMddHHmm");


            TimeSpan ts = _e - _b;
            if (ts.Ticks < 0)
            {
                Wrapper.ShowDialog("开始时间不能大于结束时间。");
                return;
            }

            SetHistoryQueryCondition();
            InitNextPage();
            Refresh();
        }

        private void btnSetParameter_Click(object sender, RoutedEventArgs e)
        {
            PassengerFlowParamConfig cfg = new PassengerFlowParamConfig(1);
            BaseWindow w = new BaseWindow();
            w.Content = cfg;
            w.Width = 800;
            w.Height = 600;
            w.Title = "客流参数设置";
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cfg.SetWindowClose(w);
            cfg.gpTimeSet.IsEnabled = false;
            object obj = w.ShowDialog();
            if (cfg.IsSaveSuccess == true)
            {
                List<DataSeriesProperty> dspList = new List<DataSeriesProperty>();
                List<PassFlowConfig> pcList = PassengerFlowHelper.HistoryPassengerMonitoryCfg();
                foreach (PassFlowConfig pc in pcList)
                {
                    DataSeriesProperty dsp = new DataSeriesProperty();
                    dsp.LegnedName = pc.Value;
                    dspList.Add(dsp);
                }
                ccLine.config.DataSeriesList = dspList;
                ccLine.Refresh();
            }
        }

        private void btnExportData_Click(object sender, RoutedEventArgs e)
        {
            ExportDate();
        }

        #region --> 初始化操作。
        /// <summary>
        /// 初始化加载方法
        /// </summary>
        void Initload()
        {
            Wrapper.FullComboBox(this.cbbDeviceType, BuinessRule.GetInstace().GetSleDevTypeInfoItem(), "device_name", "device_type");
            Wrapper.FullComboBox(this.cbbPassengerFlowType, PassengerFlowHelper.PassengerFlowTypeItem, "Value", "Key");
            Wrapper.FullComboBox(this.cbbStationCode, BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode), "station_cn_name", "station_id");
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
            {
                cbbStationCode.CanReadOnly = true;
                cbbStationCode.IsEditable = false;
                Wrapper.ComboBoxSelectedItem(this.cbbStationCode, SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            }

            SetHistoryQueryCondition();  
            InitNextPage();
            InitLineChartControl();          
            InitExitChartControl();          
            InitEntryChartControl();
        }

        /// <summary>
        /// 初始化折线图
        /// </summary>
        void InitLineChartControl()
        {
            this.dpPassengerFlowType.Children.Clear();
            ccLine = new ChartControl();

            ccLine.UserControlClassName = this.GetType().Namespace + "." + this.GetType().Name + "," + "AFC.WS.UI.UIPage";
            ChartRule cr = Utility.Instance.GetChartRuleObject(@".\RuleFiles\PassengerFlow\PassengerFlowParamConfig.xml");
            try
            {
                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
                {
                    cr.Title = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
                }
                else
                {
                    cr.Title = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            ccLine.TitleTime = "";
            ccLine.ButtonLocation = HorizontalAlignment.Center;
            ccLine.DataSourceWay = SqlCount.Multi;
            ccLine.AxisXTicksStyle = "AxisXTicks";
            ccLine.AxisYTicksStyle = "AxisYTicks";
            ccLine.ChartGridXStyle = "ChartGridX";
            ccLine.ChartGridYStyle = "ChartGridY";
            ccLine.DataSeriesStyle = "DataSeries";

            List<DataSeriesProperty> dspList = new List<DataSeriesProperty>();
            List<PassFlowConfig> pcList = PassengerFlowHelper.HistoryPassengerMonitoryCfg();
            foreach (PassFlowConfig pc in pcList)
            {
                DataSeriesProperty dsp = new DataSeriesProperty();
                dsp.LegnedName = pc.Value;
                dspList.Add(dsp);
            }
            cr.DataSeriesList = dspList;
            ccLine.Initialize(cr);
            this.dpPassengerFlowType.Children.Add(ccLine);

            this.lblCurrentPageIndex.Content = "当前第" + PassengerFlowHelper.HistoryPageCurrentIndex + "页";
        }

        /// <summary>
        /// 初始化进站图形
        /// </summary>
        void InitEntryChartControl()
        {
            this.dpEntryCounter.Children.Clear();
            ccEntryPie = new ChartControl();
            PassengerFlowEntryPie pie = new PassengerFlowEntryPie(ccEntryPie);
            this.dpEntryCounter.Children.Add(ccEntryPie);
        }

        /// <summary>
        /// 初始化出站图形
        /// </summary>
        void InitExitChartControl()
        {
            this.dpExitCounter.Children.Clear();
            ccExitPit = new ChartControl();
            PassengerFlowExitPie pie = new PassengerFlowExitPie(ccExitPit);
            this.dpExitCounter.Children.Add(ccExitPit);
        }

        #endregion --> 初始化操作。

        /// <summary>
        /// 设置查询条件。
        /// </summary>
        private void SetHistoryQueryCondition()
        {
            PassengerFlowHelper.initStartDateTime = null;
            PassengerFlowHelper.initEndDateTime = null;
            string dateBegin = Convert.ToDateTime(this.dtpBegin.ContentDatePicker).ToString("yyyy-MM-dd");
            string timeBegin = Convert.ToDateTime(this.timeBegin.Text).ToString("HH:mm:ss");
            DateTime _b = DateTime.Parse(dateBegin + " " + timeBegin);

            PassengerFlowHelper.initStartDateTime = _b.ToString("yyyMMddHHmm");
            string dateEnd = Convert.ToDateTime(this.dtpEnd.ContentDatePicker).ToString("yyyy-MM-dd");
            string timeEnd = Convert.ToDateTime(this.timeEnd.Text).ToString("HH:mm:ss");
            DateTime _e = DateTime.Parse(dateEnd + " " + timeEnd);

            PassengerFlowHelper.initEndDateTime = _e.ToString("yyyMMddHHmm");

            PassengerFlowQueryCondition qc = new PassengerFlowQueryCondition();
          
            qc.ControlBeginTime = Convert.ToDateTime(this.dtpBegin.ContentDatePicker).ToString("yyyyMMdd") + Convert.ToDateTime(this.timeBegin.Text).ToString("HHmmss");
            qc.DtControlBeginTime = DateTime.Parse(Convert.ToDateTime(this.dtpBegin.ContentDatePicker).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(this.timeBegin.Text).ToString("HH:mm:ss"));
            qc.ControlEndTime = Convert.ToDateTime(this.dtpEnd.ContentDatePicker).ToString("yyyyMMdd") + Convert.ToDateTime(this.timeEnd.Text).ToString("HHmmss");
            qc.DtControlEndTime = DateTime.Parse(Convert.ToDateTime(this.dtpEnd.ContentDatePicker).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(this.timeEnd.Text).ToString("HH:mm:ss"));
            qc.DeviceType = Wrapper.GetComboBoxUid(this.cbbDeviceType);
            if (qc.DeviceType == "%")
            {
                qc.DeviceType = "";
            }
            qc.PassengerFlowType = Wrapper.GetComboBoxUid(this.cbbPassengerFlowType);
            if (qc.PassengerFlowType == "%")
            {
                qc.PassengerFlowType = "";
                qc.CardIssueID = "";
            }
            qc.StationHallID = Wrapper.GetComboBoxUid(this.cbbStationHall);
            if (qc.StationHallID == "%")
            {
                qc.StationHallID = "";
            }
            qc.StationID = Wrapper.GetComboBoxUid(this.cbbStationCode);
            if (qc.StationID == "%")
            {
                qc.StationID = "";
            }
            qc.TimeInterval = Convert.ToInt32(Wrapper.GetComboBoxText(this.cbbTimeInterval));

            PassengerFlowHelper.HistoryTimeInterval = qc.TimeInterval;
            PassengerFlowHelper.HistoryQueryCondition = qc;

            ModifyQueryDateTime();
        }

        /// <summary>
        /// 修改查询条件。
        /// </summary>
        private void ModifyQueryDateTime()
        {
            PassengerFlowQueryCondition qc = PassengerFlowHelper.HistoryQueryCondition;

            //-->判断当前是每几页。
            int pageIndex = PassengerFlowHelper.HistoryPageCurrentIndex;

            //-->每页的点数。
            int point = PassengerFlowHelper.HistoryPagePoint;

            //-->时间间隔。
            int interval = qc.TimeInterval;

            //-->查询开始时间。
            int totalMinutes = (pageIndex - 1) * interval * (point - 1);

            if (1 == pageIndex)
            {
                qc.BeginTime = qc.DtControlBeginTime;
            }
            else //获取分钟数。
            {
                qc.BeginTime = qc.DtControlBeginTime.AddMinutes(totalMinutes);
            }


            //-->查询结束时间。
            int totalMinutesEnd = pageIndex * interval * (point - 1);
            DateTime dt = qc.DtControlBeginTime.AddMinutes(totalMinutesEnd);

            TimeSpan ts = qc.DtControlEndTime - dt;
            if (ts.Ticks < 0)
            {
                qc.EndTime = qc.DtControlEndTime;
            }
            else
            {
                qc.EndTime = dt;
            }

            Wrapper.Instance.ConsoleWriteLine("开始时间：[" + qc.BeginTime.ToString("yyyy-MM-dd HH:mm:ss") + "]", LogFlag.InfoFormat);
            Wrapper.Instance.ConsoleWriteLine("结束时间：[" + qc.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "]", LogFlag.InfoFormat);
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitNextPage()
        {
            //时间差。
            TimeSpan span = PassengerFlowHelper.HistoryQueryCondition.DtControlEndTime - PassengerFlowHelper.HistoryQueryCondition.DtControlBeginTime;
            //-->获取参数。
            PassengerFlowParamCfg cfg = PassengerFlowHelper.HistoryCfg;
            //-->获取分钟。
            int minute = span.Days * 24 * 60 + span.Hours * 60 + span.Minutes;
            //-->获取页面点得个数。
            PassengerFlowHelper.HistoryPagePoint = cfg.PagePoint;
            cfg.TimeInterval = PassengerFlowHelper.HistoryQueryCondition.TimeInterval;
            PassengerFlowHelper.HistoryTimeInterval = cfg.TimeInterval;
            //-->获取有多少页。
            int count = minute % (cfg.PagePoint * cfg.TimeInterval);
            if (count > 0)
            {
                count = Convert.ToInt32(minute / (cfg.PagePoint * cfg.TimeInterval)) + 1;
            }
            else
            {
                count = Convert.ToInt32(minute / (cfg.PagePoint * cfg.TimeInterval));
            }
            PassengerFlowHelper.HistoryPageCount = count;
            this.lblTotalPageNumber.Content = "共" + count + "页";
            PassengerFlowHelper.HistoryPageCurrentIndex = 1;

            SetCurrentPageIndexInfo();
        }

        /// <summary>
        /// 刷新。
        /// </summary>
        private void Refresh()
        {
            if (ccLine != null)
            {
                ccLine.Refresh();
            }
            if (ccEntryPie != null)
            {
                ccEntryPie.Refresh();
            }
            if (ccExitPit != null)
            {
                ccExitPit.Refresh();
            }
        }

        /// <summary>
        /// 导出数量
        /// </summary>
        private void ExportDate()
        {
            List<HistoryPassengerFlowData> pmList = new List<HistoryPassengerFlowData>();
            DateTimeConvert formatCovert = new DateTimeConvert();
            //-->1、获取共有多少个时段。
            //时间差。
            TimeSpan span = PassengerFlowHelper.HistoryQueryCondition.DtControlEndTime - PassengerFlowHelper.HistoryQueryCondition.DtControlBeginTime;
            int minute = span.Days * 24 * 60 + span.Hours * 60 + span.Minutes;
            int timeInterval = PassengerFlowHelper.HistoryQueryCondition.TimeInterval;
            //-->2、获取时间段个数。
            int IntervalTimeCount = 0;
            if ((minute % timeInterval) == 0)
            {
                IntervalTimeCount = minute / timeInterval;
            }
            else
            {
                IntervalTimeCount = minute / timeInterval;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("车站"));
            dt.Columns.Add(new DataColumn("站厅名称"));
            dt.Columns.Add(new DataColumn("卡发行商"));
            dt.Columns.Add(new DataColumn("客流类型"));
            dt.Columns.Add(new DataColumn("时间"));
            dt.Columns.Add(new DataColumn("数量"));

            //--3、开始循环时间段。
            DateTime BeginTime = PassengerFlowHelper.HistoryQueryCondition.DtControlBeginTime;
            List<PassengerData> pdList = GetPassengerFlowData();
            string beginA = "ExportDate()方法：开始" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');

            for (int i = 1; i < IntervalTimeCount; i++)
            {
                DateTime beginTime = BeginTime.AddMinutes((i - 1) * timeInterval);
                DateTime endTime = BeginTime.AddMinutes(i * timeInterval);
                string conditionBeginTime = beginTime.ToString("HHssmm");

                var tempQueryResult = PassengerFlowQueryResultList.Where(
                    p => Convert.ToDateTime(formatCovert.Convert(p.Tran_date,null,null,null).ToString()).AddMinutes(Convert.ToInt32(p.Tran_time_min.Substring(0, 2)) * 60 +
                Convert.ToInt32(p.Tran_time_min.Substring(2, 2))) >= beginTime);

               
                foreach (PassengerData pd in pdList)
                {
                    PassengerData pdTemp = new PassengerData();
                    pdTemp.DatetimeSegment = BeginTime.AddMinutes((i - 1) * timeInterval).ToString("yyyy-MM-dd HH:mm");
                    foreach (var v in tempQueryResult)
                    {
                        int currentMinute = Convert.ToInt32(v.Tran_time_min.Substring(0, 2)) * 60 +
                            Convert.ToInt32(v.Tran_time_min.Substring(2, 2));
                        DateTime CurrentTime = Convert.ToDateTime(formatCovert.Convert(v.Tran_date, null, null, null).ToString()).AddMinutes(currentMinute);
                        if (CurrentTime >= beginTime &&
                            CurrentTime < endTime &&
                            v.Issuer_id == pd.Card_issuer_id &&
                            v.Afc_type == pd.pass_type_id &&
                            v.station_hall_id == pd.Station_hall_id &&
                            v.station_cn_name == pd.Station_cn_name

                            )
                        {
                            pdTemp.Total += v.Total;
                        }
                    }
                    dt.Rows.Add(pd.Station_cn_name, pd.Station_hall_name,
                        pd.CardIssueName,
                        pd.pass_type_name,
                        pdTemp.DatetimeSegment + "~" + BeginTime.AddMinutes(i * timeInterval).ToString("yyyy-MM-dd HH:mm"),
                        pdTemp.Total);
                }
                
            }
            beginA += "结束：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            Wrapper.Instance.ConsoleWriteLine(beginA, LogFlag.InfoFormat);
            //2012.01.09 modify begin
            System.Windows.Forms.SaveFileDialog fileDlg = new System.Windows.Forms.SaveFileDialog();
            fileDlg.Filter = "Excel Worksheets|*.xls";
            fileDlg.Title = "请您选择Excel文件存放的路径";
            fileDlg.RestoreDirectory = true;
            System.Windows.Forms.DialogResult dr = fileDlg.ShowDialog();
            Util.exportToExcel(dt,fileDlg.FileName);
            //2012.01.09 modify end
            //Util.exportToExcel(dt, "c:\\" + Guid.NewGuid().ToString() + ".xls");
        }

        /// <summary>
        /// 获取客流数据类集合
        /// </summary>
        /// <returns>返回客流数据类集合</returns>
        private List<PassengerData> GetPassengerFlowData()
        {
            List<PassengerData> pdList = new List<PassengerData>();
            foreach (HistoryPassengerFlowData v in PassengerFlowQueryResultList)
            {
                bool result = JudgeIsExist(v, pdList);
                if (result == false)
                {
                    PassengerData p = new PassengerData();
                    p.Station_cn_name = v.station_cn_name;
                    p.Station_id = v.Station_id;
                    p.Station_hall_id = v.station_hall_id;
                    p.Card_issuer_id = v.Issuer_id;
                    p.pass_type_id = v.Afc_type;
                    p.pass_type_name = v.Afc_type_name;
                    try
                    {
                        p.Station_hall_name = BuinessRule.GetInstace().GetBasiStationHall(v.Station_id).Where(b =>
                        b.station_hall_id == v.station_hall_id).GetTContext<BasiStationHallIdInfo>().station_hall_name;
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    
                    if (v.Issuer_id == 1)
                    {
                        p.CardIssueName = "一票通";
                    }
                    else
                    {
                        p.CardIssueName = "一卡通";
                    }
                    pdList.Add(p);
                }
            }
            return pdList;
        }

        /// <summary>
        /// 判断历史客流数据类是否存在。
        /// </summary>
        /// <param name="v">历史客流数据类</param>
        /// <param name="pdList">客流数据类集合</param>
        /// <returns>true:存在；false:不存在</returns>
        private bool JudgeIsExist(HistoryPassengerFlowData v, List<PassengerData> pdList)
        {
            PassengerData pd = pdList.Where(p =>
                p.Station_cn_name == v.station_cn_name &&
                p.Station_hall_id == v.station_hall_id &&
                p.Card_issuer_id == v.Issuer_id &&
                p.pass_type_id == v.Afc_type &&
                p.pass_type_name == v.Afc_type_name).ReturnT<PassengerData>();

            if (pd == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
