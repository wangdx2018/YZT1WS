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
using AFC.WS.UI.Common;
using AFC.BOM2.UIController;
using AFC.WS.UI.Components;
using System.Windows.Threading;
using AFC.WS.BR.PassengerFlow;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.WS.BR.ParamsManager;
using AFC.WS.Model.DB;

namespace AFC.WS.UI.UIPage.PassengerFlow
{
    /// <summary>
    /// PassengerFlowInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PassengerFlowInfo : UserControlBase, IChartDataSource
    {
        /// <summary>
        /// 图表控件。
        /// </summary>
        private ChartControl chartControl = null;

        /// <summary>
        /// 定义一个窗体
        /// </summary>
        private BaseWindow w = null;

        /// <summary>
        /// 是否是第一次加载图表控件。
        /// </summary>
        private bool isInitChartControl = false;

        /// <summary>
        /// 客流监视参数设置类
        /// </summary>
        PassengerFlowParamCfg pfpc;

        /// <summary>
        /// 定时器
        /// </summary>
        DispatcherTimer timer;

        /// <summary>
        /// 图表图例。
        /// </summary>
        private Visifire.Charts.RenderAs RenderType = Visifire.Charts.RenderAs.Line;

        #region IChartDataSource 成员

        public void GetDataSource(out List<object> XValue, out List<List<string>> YValue, int Interval)
        {
            string beginTime = "开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            PassengerFlowHelper.SetPassengerMonitor(out XValue, out YValue, Interval);
            beginTime += "-->结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            Wrapper.Instance.ConsoleWriteLine(beginTime, LogFlag.InfoFormat);

        }

        #endregion

        /// <summary>
        /// 構造函數
        /// </summary>
        public PassengerFlowInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            try
            {
                InitLoad();
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 30000);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 刷新页数使用
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            InitNextPage();
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            if (this.chartControl != null)
            {
                this.chartControl.ShutThread();
            }
            timer.Tick -= new EventHandler(timer_Tick);
            PassengerFlowHelper.PassengerFlowNumber -= new PassengerFlowNumberEventHandler(PFH_PassengerFlowNumber);
            base.UnLoadControls();
            this.chartControl.ShutThread();
        }

        #region --> 初始分操作。
        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            PassengerFlowHelper.PassengerFlowNumber += new PassengerFlowNumberEventHandler(PFH_PassengerFlowNumber);
            PassengerFlowHelper.DeviceName = "";
            isInitChartControl = false;
            InitNextPage();
            InitTreeView();
            InitChartControl();
            this.lblCurrentPageIndex.Content = "当前第" + PassengerFlowHelper.PageCurrentIndex + "页";
        }

        /// <summary>
        /// 客流统计事件委托。
        /// </summary>
        /// <param name="sender">存放当前类内容 this</param>
        /// <param name="e">客流统计事件类</param>
        private void PFH_PassengerFlowNumber(object sender, PassengerFlowNumberEventArgs e)
        {
            this.dgMonitorTotal.ItemsSource = null;
            this.dgMonitorTotal.AutoGenerateColumns = false;
            this.dgMonitorTotal.IsReadOnly = true;
            this.dgMonitorTotal.ItemsSource = e.PassengerFlowNumberItem;
        }

        /// <summary>
        /// 初始化界面有幾頁。
        /// </summary>
        private void InitNextPage()
        {
            //-->获取参数。
            PassengerFlowParamCfg cfg = SysConfig.GetSysConfig().PassengerFlowParamCfg;
            //-->获取分钟。
            int minute = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            //-->获取页面点得个数。
            PassengerFlowHelper.PagePoint = cfg.PagePoint;
            PassengerFlowHelper.TimeInterval = cfg.TimeInterval;
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
            PassengerFlowHelper.PageCount = count;
            this.lblTotalPage.Content = "共" + count + "页";

            //-->判断当前是第几页。
            int pageMinute = cfg.PagePoint * cfg.TimeInterval;
            if (Convert.ToInt32(minute / pageMinute) > 0)
            {
                PassengerFlowHelper.PageCurrentIndex = Convert.ToInt32(minute / pageMinute) + 1;
            }
            else
            {
                PassengerFlowHelper.PageCurrentIndex = 1;
            }
            this.lblCurrentPageIndex.Content = "当前第" + PassengerFlowHelper.PageCurrentIndex + "页";
        }

        /// <summary>
        /// 获取客流标题
        /// </summary>
        /// <returns></returns>
        private string GetPassengerTitleInfo()
        {
            PassengerFlowParamCfg cfg = SysConfig.GetSysConfig().PassengerFlowParamCfg;
            int total = (cfg.PagePoint - 1) * cfg.TimeInterval;
            string content = "";
            if ((total % 60) == 0)
            {
                content = Convert.ToInt32(total / 60) + "小时";
            }
            else
            {
                content = Convert.ToInt32(total / 60) + "小时" + total % 60 + "分钟";
            }
            return content;
        }

        /// <summary>
        /// 初始化圖列
        /// </summary>
        private void InitChartControl()
        {
            if (chartControl != null)
            {
                chartControl.ShutThread();
            }
            isInitChartControl = true;
            this.dockPanel.Children.Clear();
            chartControl = new ChartControl();
            chartControl.UserControlClassName = this.GetType().Namespace + "." + this.GetType().Name + "," + "AFC.WS.UI.UIPage";
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
            chartControl.ButtonLocation = HorizontalAlignment.Center;
            chartControl.DataSourceWay = SqlCount.Multi;
            chartControl.AxisXTicksStyle = "AxisXTicks";
            chartControl.AxisYTicksStyle = "AxisYTicks";
            chartControl.ChartGridXStyle = "ChartGridX";
            chartControl.ChartGridYStyle = "ChartGridY";
            chartControl.DataSeriesStyle = "DataSeries";

            chartControl.RenderType = RenderType;
            List<DataSeriesProperty> dspList = new List<DataSeriesProperty>();
            List<PassFlowConfig> pcList = PassengerFlowHelper.GetPassengerMonitorConfig();
            foreach (PassFlowConfig pc in pcList)
            {
                DataSeriesProperty dsp = new DataSeriesProperty();
                dsp.LegnedName = pc.Value;
                dspList.Add(dsp);
            }
            cr.DataSeriesList = dspList;
            this.chartControl.Initialize(cr);
            this.dockPanel.Children.Add(chartControl);
        }

        /// <summary>
        /// 初始化加載線路、車站、站廳信息。
        /// </summary>
        private void InitTreeView()
        {
            tvStationList.Items.Clear();
            TreeViewItem item = new TreeViewItem();
            item.IsExpanded = true;
            item.IsSelected = true;
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
            {
                BasiStationInfo obj = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
                if (obj != null)
                {
                    item.Header = obj.station_cn_name ;
                    item.Uid = obj.station_id;
                    item.Tag = obj;
                }
                PassengerFlowHelper.SetHallInfo(item, SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            }
            else
            {
                BasiLineIdInfo obj = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
                item.Header = obj.line_name;
                item.Uid = obj.line_id;
                item.Tag = obj;
                PassengerFlowHelper.SetStationInfo(item, obj.line_id);
            }
            this.tvStationList.Items.Add(item);
        }

        #endregion --> 初始化操作。

        /// <summary>
        /// 监视车站查询。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void tvStationList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = this.tvStationList.SelectedItem as TreeViewItem;
            if (item == null)
            {
                return;
            }
            if (item.IsExpanded == true)
            {
                if (!(item.Tag is BasiLineIdInfo))
                {
                    PassengerFlowHelper.SetTreeViewItemIsExpanded(item);
                }
            }
            else
            {
                item.IsExpanded = true;
            }
            if (isInitChartControl == true)
            {
                PassengerFlowHelper.ClearMonitorCondition();
                PassengerFlowHelper.GetMonitorCondition(item);
                lblMonitorInfo.Content = PassengerFlowHelper.GetCurrentMonitorInfo(item);
                //InitChartControl();
                this.chartControl.Refresh();
            }
        }

        #region --> 翻页

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (PassengerFlowHelper.PageCurrentIndex > 1 &&
                   PassengerFlowHelper.PageCurrentIndex <= PassengerFlowHelper.PageCount)
            {
                PassengerFlowHelper.PageCurrentIndex--;
                lblCurrentPageIndex.Content = "当前第" + PassengerFlowHelper.PageCurrentIndex + "页";
                this.chartControl.Refresh();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (PassengerFlowHelper.PageCurrentIndex > 0 &&
                  PassengerFlowHelper.PageCurrentIndex < PassengerFlowHelper.PageCount)
            {
                PassengerFlowHelper.PageCurrentIndex++;
                lblCurrentPageIndex.Content = "当前第" + PassengerFlowHelper.PageCurrentIndex + "页";
                this.chartControl.Refresh();
            }
        }

        #endregion --> 翻页

        #region -->设备客流监视设置。

        /// <summary>
        /// 所有设备监视
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void rbtnAll_Checked(object sender, RoutedEventArgs e)
        {
            if (chartControl != null)
            {
                RenderType = chartControl.RenderType;
                DevicePassengerFlowMonitor("全部", "");
            }
        }

        /// <summary>
        /// 查看TVM客流监视
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void rbtnTVM_Checked(object sender, RoutedEventArgs e)
        {
            RenderType = chartControl.RenderType;
            DevicePassengerFlowMonitor(this.rbtnTVM.Content.ToString(), this.rbtnTVM.Uid);
        }

        /// <summary>
        /// BOM客流监视.
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void rbtnBOM_Checked(object sender, RoutedEventArgs e)
        {
            RenderType = chartControl.RenderType;
            DevicePassengerFlowMonitor(this.rbtnBOM.Content.ToString(), this.rbtnBOM.Uid);
        }

        /// <summary>
        /// AG客流监视
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void rbtnAG_Checked(object sender, RoutedEventArgs e)
        {
            RenderType = chartControl.RenderType;
            DevicePassengerFlowMonitor(this.rbtnAG.Content.ToString(), this.rbtnAG.Uid);
        }

        /// <summary>
        /// EQM客流监视
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void rbtnEQM_Checked(object sender, RoutedEventArgs e)
        {
            RenderType = chartControl.RenderType;
            DevicePassengerFlowMonitor(this.rbtnEQM.Content.ToString(), this.rbtnEQM.Uid);
        }

        /// <summary>
        /// 设置设备客流监视参数。
        /// </summary>
        /// <param name="deviceName">设备名称：全部、TVM、BOM、AG、EQM</param>
        /// <param name="deviceType">设备类型</param>
        void DevicePassengerFlowMonitor(string deviceName, string deviceType)
        {
            PassengerFlowHelper.DeviceName = deviceName;
            PassengerFlowHelper.DeviceType = deviceType;
            if (isInitChartControl == true)
            {
                this.chartControl.Refresh();
            }
        }

        #endregion -->设备客流监视设置。

        /// <summary>
        /// 参数配置按键事件。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void btnParameterConfig_Click(object sender, RoutedEventArgs e)
        {
            PassengerFlowParamConfig cfg = new PassengerFlowParamConfig(0);
            w = new BaseWindow();
            w.Content = cfg;
            w.Width = 800;
            w.Height = 600;
            w.Title = "客流参数设置";
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cfg.SetWindowClose(w);
            object obj = w.ShowDialog();
            if (cfg.IsSaveSuccess == true)
            {
                List<DataSeriesProperty> dspList = new List<DataSeriesProperty>();
                List<PassFlowConfig> pcList = PassengerFlowHelper.GetPassengerMonitorConfig();
                foreach (PassFlowConfig pc in pcList)
                {
                    DataSeriesProperty dsp = new DataSeriesProperty();
                    dsp.LegnedName = pc.Value;
                    dspList.Add(dsp);
                }
                chartControl.config.DataSeriesList = dspList;
                InitNextPage();
                this.chartControl.Refresh();
            }
        }

        /// <summary>
        /// 客流监视的启动与停止。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void btnPassengerFlowMonitor_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnPassengerFlowMonitor.Content.ToString() == "启动监视")
            {
                this.chartControl.ThreadStart();
                this.btnPassengerFlowMonitor.Content = "停止监视";
            }
            else
            {
                this.chartControl.ShutThread();
                this.btnPassengerFlowMonitor.Content = "启动监视";
            }
        }
    }
}