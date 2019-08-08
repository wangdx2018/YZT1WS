#region [       Copyright (C), 2009,  中软AFC, Token Shen.     ]
/************************************************************
  FileName: ChartControl.xaml
  
  Author: 沈克涛    
 
  Version :  1.0   
 
  Date:20090729
 
  Description: 文本框的扩展实现   
 
  Function List:  
 
    1. LoadStackPanel()  // ---> 加载功能按钮
 
    2. LoadChart()  // ---> 加载图表控件
 
  History: 
 
      <author>   <time>      <version >     <desc>
 
      沈克涛    2009/07/29     1.0         增加代码说明
      沈克涛    2010/01/14     1.0         增加客流监视相应代码
 * ***********************************************************/
#endregion

#region [       Using namespaces       ]
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
using System.Windows.Threading;
using System.Windows.Shapes;
using AFC.WS.UI.Common;
using Visifire.Charts;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;
using System.Data;
using System.IO;
using System.Threading;
using AFC.WS.UI.CommonControls;
using System.ComponentModel;
#endregion

namespace AFC.WS.UI.Components
{
    /// <summary>
    /// 使用数据源个数。
    /// </summary>
    public enum SqlCount
    {
        /// <summary>
        /// 单个SQl语句使用
        /// </summary>
        Single = 0,
        /// <summary>
        /// 多个SQL语句使用
        /// </summary>
        Multi = 1
    }

    /// <summary>
    /// 使用图表基础控件，重写其中的方法，设置样式，加载数据。
    /// 
    /// 图表控件继承IDataSourceClient接口，实现随数据源变化刷新图表控件内容。
    /// 
    /// </summary>
    /// <remarks>
    /// 使用图表控件，通过继承接口，实现数据的绑定。
    /// </remarks>
    public partial class ChartControl : UserControl, IDataSourceClient
    {
        #region [       Declarations       ]

        /// <summary>
        /// Chart规则文件对象
        /// </summary>
        public ChartRule config = null;

        /// <summary>
        /// 数据源对象
        /// </summary>
        private IDataSource dataSource = null;

        /// <summary>
        /// 图形种类
        /// </summary>
        private string chartType = null;

        /// <summary>
        /// 控件之间间隔宽度
        /// </summary>
        private double textBlockWidth = 20;

        /// <summary>
        /// 控件之间间隔高度
        /// </summary>
        private double textBlockHeight = 50;

        /// <summary>
        /// 图形控件对象
        /// </summary>
        private Chart chart = null;

        /// <summary>
        /// DataTable对象
        /// </summary>
        private DataTable dt = null;

        /// <summary>
        /// Create a new instance of timer object 
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// 数据点对象
        /// </summary>
        private DataPoint dataPoint = null;

        /// <summary>
        /// Create a new instance of DataSeries
        /// </summary>
        private DataSeries dataSeries = null;

        /// <summary>
        ///Create a new instance of SpecialDataSeries 
        /// </summary>
        private DataSeries specialSeries = null;

        /// <summary>
        /// Create a new instance of ComboBox
        /// </summary>
        private ComboBoxExtend comboBox = null;

        /// <summary>
        /// Create a new instance of listDataSeries
        /// </summary>
        private List<DataSeries> listDataSeries = new List<DataSeries>();

        /// <summary>
        /// 创建接口对象
        /// </summary>
        private IChartDataSource createClassInstance = null;

        /// <summary>
        /// 创建格式化类对象
        /// </summary>
        private IValueConverter createConverterClassInstance = null;

        /// <summary>
        /// 多个数据源的情况，创建存放X轴数据对象。
        /// </summary>
        private List<object> Xlist = null;

        /// <summary>
        /// 多个数据源的情况，创建存放Y轴数据对象。
        /// </summary>
        private List<List<string>> Ylist = null;

        /// <summary>
        /// 创建X轴对象
        /// </summary>
        private Axis axisX = null;

        #endregion

        #region [       Properties       ]
        /// <summary>
        /// 设置ComboBox样式。
        /// </summary>
        private string comStyle;
        /// <summary>
        /// 设置ComboBox样式。
        /// </summary>
        [
         Description("设定ComboBox样式。"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         Category("ChartControl"),
         Filter()
         ]
        public string ComboBoxStyle
        {
            get { return comStyle; }
            set { comStyle = value; }
        }
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        private int _controlWidth;
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        [
        Description("设定控件宽度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public int ComboBoxWidth
        {
            get { return _controlWidth; }
            set { _controlWidth = value; }
        }
        /// <summary>
        /// 设置控件高度
        /// </summary>
        private int _controlHeight;
        /// <summary>
        /// 设置控件高度
        /// </summary>
        [
        Description("设定控件高度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public int ComboBoxHeight
        {
            get { return _controlHeight; }
            set { _controlHeight = value; }
        }
        /// <summary>
        /// 设置控件高度
        /// </summary>
        private int _refreshTime;
        /// <summary>
        /// 设置控件高度
        /// </summary>
        [
        Description("设定控件高度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public int RefreshTime
        {
            get { return _refreshTime; }
            set { _refreshTime = value; }
        }

        /// <summary>
        /// 选择数据源的方式，在控件中调用数据源或通过接口调用数据源。
        /// </summary>
        private SqlCount _dataSourceWay;

        /// <summary>
        /// 选择数据源的方式，在控件中调用数据源或通过接口调用数据源。
        /// </summary>
        [
        Description("选择数据源的方式，在控件中调用数据源或通过接口调用数据源。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public SqlCount DataSourceWay
        {
            get { return _dataSourceWay; }
            set { _dataSourceWay = value; }
        }

        /// <summary>
        /// Chart多个数据源绑定，要继承IChartDataSource接口，
        ///
        /// Chart中要调用实现接口中的方法，方法中返回Table对象。
        ///
        /// 若得到实现接口中的方法，需要知道实现接口的用户控件类名称，
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        private string _userControlName;
        /// <summary>
        /// Chart多个数据源绑定，要继承IChartDataSource接口，
        ///
        /// Chart中要调用实现接口中的方法，方法中返回Table对象。
        ///
        /// 若得到实现接口中的方法，需要知道实现接口的用户控件类名称，
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        [
        Description("获得实现接口用户控件命名空间和类名称，多个数据源绑定时使用。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string UserControlClassName
        {
            get { return _userControlName; }
            set { _userControlName = value; }
        }
        ///<summary>
        /// 格式化AxisXLabel
        /// 
        /// 若得到实现接口中的方法，需要知道实现接口的用户控件类名称，
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        private string _conveterClassName;

        ///<summary>
        /// 格式化AxisXLabel
        /// 
        /// 若得到实现接口中的方法，需要知道实现接口的用户控件类名称，
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        [
        Description("格式化AxisXLabel。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string ConveterClassName
        {
            get { return _conveterClassName; }
            set { _conveterClassName = value; }
        }
        /// <summary>
        /// 设置ChartStyle
        /// </summary>
        [
        Description("设置ChartStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string ChartStyle
        {
            get;
            set;
        }

        /// <summary>
        /// 设置DataSeriesStyle
        /// </summary>
        [
        Description("设置DataSeriesStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string DataSeriesStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置MainTitleStyle
        /// </summary>
        [
        Description("设置MainTitleStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string MainTitleStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置SubTitleStyle
        /// </summary>
        [
        Description("设置SubTitleStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string SubTitleStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置AxisXStyle
        /// </summary>
        [
        Description("设置AxisXStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string AxisXStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置AxisYStyle
        /// </summary>
        [
        Description("设置AxisYStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string AxisYStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置PlotAreaStyle
        /// </summary>
        [
        Description("设置PlotAreaStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string PlotAreaStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置AxisXLabelsStyle
        /// </summary>
        [
        Description("设置AxisXLabelsStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string AxisXLabelsStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置AxisYLabelsStyle
        /// </summary>
        [
        Description("设置AxisYLabelsStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string AxisYLabelsStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置AxisXTicksStyle
        /// </summary>
        [
        Description("设置AxisXTicksStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string AxisXTicksStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置AxisYTicksStyle
        /// </summary>
        [
       Description("设置AxisYTicksStyle。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("ChartControl"),
       Filter()
       ]
        public string AxisYTicksStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置LegendStyle
        /// </summary>
        [
       Description("设置LegendStyle。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("ChartControl"),
       Filter()
       ]
        public string LegendStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置ToolTipStyle
        /// </summary>
        [
        Description("设置ToolTipStyle。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public string ToolTipStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [
         Description("设置SelectionEnabled。"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         Category("ChartControl"),
         Filter()
         ]
        public bool SelectionEnabled
        {
            get;
            set;
        }
        /// <summary>
        /// 设置ChartGridXStyle
        /// </summary>
        [
       Description("设置ChartGridXStyle。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("ChartControl"),
       Filter()
       ]
        public string ChartGridXStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置ChartGridYStyle
        /// </summary>
        [
       Description("设置ChartGridYStyle。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("ChartControl"),
       Filter()
       ]
        public string ChartGridYStyle
        {
            get;
            set;
        }
        /// <summary>
        /// 设置ButtonLocation
        /// </summary>
        private HorizontalAlignment _buttonLocation;
        /// <summary>
        /// 设置ButtonLocation
        /// </summary>
        [
        Description("设置ButtonLocation。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public HorizontalAlignment ButtonLocation
        {
            get { return _buttonLocation; }
            set { _buttonLocation = value; }
        }
        /// <summary>
        ///设置间隔时间
        /// </summary>
        private int _intervalTime;

        /// <summary>
        ///设置间隔时间
        /// </summary>
         [
        Description("设置间隔时间。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public int IntervalTime
        {
            get { return _intervalTime; }
            set { _intervalTime = value; }
        }

        /// <summary>
        /// 设置默认图形类型
        /// </summary>
        private RenderAs _renderType;
        /// <summary>
        /// 设置默认图形类型
        /// </summary>
        [
        Description("设置默认图形类型。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ChartControl"),
        Filter()
        ]
        public RenderAs RenderType
        {
            get { return _renderType; }
            set
            {
                _renderType = value;
                chartType = value.ToString();
            }
        }

        private string _titleTime;
        /// <summary>
        ///标题时间
        /// </summary>
        [
       Description("设置标题时间。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("ChartControl"),
       Filter()
       ]
        public string TitleTime
        {
            get { return _titleTime; }
            set { _titleTime = value; }
        }
        #endregion

        #region [       Constructor       ]
        /// <summary>
        /// 构造函数，初始化窗体。
        /// </summary>
        public ChartControl()
        {
            SelectionEnabled = true;

            ButtonLocation = HorizontalAlignment.Center;

            DataSourceWay = SqlCount.Single;

            RenderType = RenderAs.Line;

            chartType = RenderType.ToString();

            InitializeComponent();
        }
        #endregion

        #region [       Set Style      ]
        /// <summary>
        /// 获取图表样式。
        /// </summary>
        private Style GetStyle(string style)
        {
            try
            {
                Style style1 = null;
                object obj = this.FindResource(style);
                if (obj != null)
                {
                   style1 = obj as Style;
                }

                if (style1 != null)
                    return style1;
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置ComboBoxStyle出错:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 设置ComboBox样式。
        /// </summary>
        private bool SetStyle()
        {
            try
            {
                if (ComboBoxWidth != 0)
                {
                    this.comboBox.ControlWidth = ComboBoxWidth;
                }
                if (ComboBoxHeight != 0)
                {
                    this.comboBox.ControlWidth = ComboBoxHeight;
                }
                if (!string.IsNullOrEmpty(config.ComboBoxStyle))
                {
                    this.comboBox.ComboBoxStyle = config.ComboBoxStyle;
                }
                else
                {
                    if (ComboBoxStyle != null)
                    {
                        this.comboBox.ComboBoxStyle = ComboBoxStyle;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置ComboBoxStyle出现异常:" + ex.ToString());
                return false;
            }
        }

        #endregion

        #region [       Public Methods       ]

        /// <summary>
        /// 初始化布局，加载配置文件，加载数据源。
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>0：成功，-1：失败</returns>
        public int Initialize(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return -1;
            try
            {
                LoadConfig(fileName);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("窗体加载函数中出现异常:" + ex.ToString());
            }
            return Initialize(config);
        }

        /// <summary>
        /// 创建布局，加载图形，加载数据。
        /// </summary>
        /// <param name="config">规则文件名称</param>
        /// <returns>0：成功， -1失败</returns>
        public int Initialize(ChartRule config)
        {
            try
            {

                this.config = config;
                if (this.config == null)
                    return -1;
                bool isStyle = SetStyle();
                if (!isStyle)
                    WriteLog.Log_Info("设置ComboBox样式出错！");



                if (DataSourceWay == SqlCount.Single)
                {
                    bool getSource = GetDataSource();

                    if (!getSource)
                        WriteLog.Log_Info("没有获得数据，出现异常！");
                }

                bool loadLayout = LoadLayout();

                if (!loadLayout)
                    WriteLog.Log_Info("加载布局出现异常！");

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 关闭线程
        /// </summary>
        public void ShutThread()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        #endregion

        #region [       Private Methods       ]

        /// <summary>
        /// 加载布局
        /// </summary>
        /// <returns>True：成功，False：失败</returns>
        private bool LoadLayout()
        {
            try
            {
                if (config.CanDynamicRefurbish)
                {
                    try
                    {
                        LoadStackPanel();

                        LoadChart();

                        gridLayout.Background = chart.Background;

                        timer.Tick += new EventHandler(timer_Tick);

                        int refreshTime = 0;

                        if (config.AutoRefreshTime != 0)
                        {
                            refreshTime = config.AutoRefreshTime * 1000;
                        }
                        else
                        {
                            if (RefreshTime != 0)
                            {
                                refreshTime = RefreshTime * 1000;
                            }
                            else
                            {
                                refreshTime = 3000;
                            }
                        }

                        timer.Interval = new TimeSpan(0, 0, 0, 0, refreshTime);

                        timer.Start();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error("动态加载图形出现异常:" + ex.ToString());
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        LoadStackPanel();

                        LoadChart();
                        gridLayout.Background = chart.Background;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error("静态加载图形出现异常:" + ex.ToString());
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("加载布局出现异常：" + ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// 启动线程监视。
        /// </summary>
        public void ThreadStart()
        {
            if (config.CanDynamicRefurbish)
            {
                if (timer != null && !timer.IsEnabled)
                {
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// 获取查询数据源，在有交互界面时使用
        /// </summary>
        private bool GetQueryDataSource()
        {
            try
            {
                bool isData = CreateDataSource();
                if (!isData)
                    return false;
                bool istrue = CreateQueryDataTable();
                if (!istrue)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("获取数据源异常:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 获取数据源，默认数据源。
        /// </summary>
        private bool GetDataSource()
        {

            try
            {
                bool isData = CreateDataSource();
                if (!isData)
                    return false;
                bool istrue = CreateDataTable();
                if (!istrue)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("获取数据源异常:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configName">配置文件名称</param>
        /// <returns>True：成功，False：失败</returns>
        private bool LoadConfig(string configName)
        {
            if (string.IsNullOrEmpty(configName)) return false;
            try
            {
                if (!System.IO.File.Exists(configName))
                {
                    WriteLog.Log_Info("fileName:" + configName + " not found");

                    return false;
                }

                config = AFC.WS.UI.Config.Utility.Instance.GetChartRuleObject(configName);

                if (config == null) return false;

                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("加载配置文件异常:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 加载StackPanel控件。
        /// </summary>
        /// <returns>True:成功，False:失败</returns>
        private bool LoadStackPanel()
        {
            try
            {
                RowDefinition rowDef1 = new RowDefinition();

                this.gridLayout.RowDefinitions.Add(rowDef1);

                RowDefinition rowDef2 = new RowDefinition();

                this.gridLayout.RowDefinitions.Add(rowDef2);

                if (config.ChartLocation == ChartLocations.Bottom)
                {

                    StackPanel stack = CreatePanel();

                    stack.Orientation = Orientation.Horizontal;

                    if (config.ButtonLocation != null)
                        stack.HorizontalAlignment = config.ButtonLocation;
                    else
                    {
                        if (ButtonLocation != null)
                            stack.HorizontalAlignment = ButtonLocation;
                    }

                    Grid.SetRow(stack, 0);

                    if (config.CanRefurbish == true || config.CanSaveImage == true || config.ChartList.Count > 1)
                    {
                        this.gridLayout.RowDefinitions[0].Height = new GridLength(textBlockHeight, GridUnitType.Pixel);
                    }
                    else
                    {
                        this.gridLayout.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Pixel);

                        WriteLog.Log_Info("没有功能按钮或选择图形控件");
                    }

                    this.gridLayout.Children.Add(stack);
                }
                if (config.ChartLocation == ChartLocations.Top)
                {
                    StackPanel stack = CreatePanel();

                    stack.Orientation = Orientation.Horizontal;

                    stack.HorizontalAlignment = HorizontalAlignment.Center;

                    Grid.SetRow(stack, 1);

                    if (config.CanRefurbish == true || config.CanSaveImage == true || config.ChartList.Count > 1)
                    {
                        this.gridLayout.RowDefinitions[1].Height = new GridLength(textBlockHeight, GridUnitType.Pixel);
                    }
                    else
                    {
                        this.gridLayout.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);

                        WriteLog.Log_Info("没有功能按钮或选择图形控件");
                    }

                    this.gridLayout.Children.Add(stack);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("加载StackPanel异常：" + ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 加载图表控件,设置图表控件的位置。
        /// </summary>
        /// <returns>True:成功，False=失败</returns>
        private bool LoadChart()
        {
            try
            {

                if (config.ChartLocation == ChartLocations.Bottom)
                {
                    Chart chartInstance = CreateChart();
                    if (chartInstance == null) return false;

                    Grid.SetRow(chartInstance, 1);

                    this.gridLayout.Children.Add(chartInstance);
                }
                if (config.ChartLocation == ChartLocations.Top)
                {
                    Chart chartInstance = CreateChart();

                    if (chartInstance == null) return false;

                    Grid.SetRow(chartInstance, 0);

                    this.gridLayout.Children.Add(chartInstance);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("加载图形异常:" + ex.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建StackPanel上的控件。
        /// </summary>
        /// <returns>WrapPanel的对象</returns>
        private StackPanel CreatePanel()
        {
            StackPanel flowPanel = new StackPanel();
            try
            {

                /*********************************************************************
                 *  Add Date：2009-07-29 PM
                 *  
                 *      Note：加一个判断，判断config是否为空。
                 * 
                 * *******************************************************************/
                if (config != null)
                {

                    if (config.CanRefurbish)
                    {
                        Button btnRefresh = CreateButton("刷新图像", "btnRefreshImage");

                        flowPanel.Children.Add(btnRefresh);

                        flowPanel.Orientation = Orientation.Horizontal;

                        flowPanel.Children.Add(new TextBlock { Width = textBlockWidth, Height = textBlockHeight });
                    }
                    if (config.CanSaveImage)
                    {
                        Button btnImage = CreateButton("保存图片", "btnSaveImage");

                        flowPanel.Children.Add(btnImage);

                        flowPanel.Children.Add(new TextBlock() { Width = textBlockWidth, Height = textBlockHeight });
                    }
                    if (config.ChartList.Count > 0)
                    {
                        ComboBox comboBox = CreateComboBox();

                        flowPanel.Children.Add(comboBox);

                        flowPanel.Children.Add(new TextBlock() { Width = textBlockWidth, Height = textBlockHeight });
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("创建StackPanel异常:" + ex.ToString());
            }
            return flowPanel;
        }

        /// <summary>
        /// 创建有内容的ComboBox.
        /// </summary>
        /// <returns>ComboBox的对象</returns>
        private ComboBox CreateComboBox()
        {
            try
            {
                StringBuilder stringBuilderName = new StringBuilder();

                StringBuilder stringBuilderType = new StringBuilder();
                Dictionary<RenderAs, string> dic = new Dictionary<RenderAs, string>();

                comboBox = new ComboBoxExtend();

                comboBox.BindType = BindType.DataString;

                /*********************************************************************
                 *  Add Date：2009-07-29 PM
                 *  
                 *      Note：加一个判断，判断config是否为空。
                 * 
                 * *******************************************************************/
                if (config != null)
                {

                    comboBox.ComboBoxStyle = config.ComboBoxStyle;

                    for (int i = 0; i < config.ChartList.Count; i++)
                    {
                        stringBuilderName.Append(config.ChartList[i].Name + ",");

                        stringBuilderType.Append(config.ChartList[i].ChartType + ",");
                        dic.Add(config.ChartList[i].ChartType, config.ChartList[i].Name);
                    }
                }
                if (dic.Count != 0 && dic.ContainsKey(RenderType))
                {
                    comboBox.StartValue = dic[RenderType].ToString();
                }
                comboBox.DataString = stringBuilderName.ToString();

                comboBox.ControlWidth = 150;
                comboBox.ControlHeight = 23;
                comboBox.HideDataString = stringBuilderType.ToString();
            
                comboBox.Initialize();

                comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("创建ComboBox产生异常:" + ex.ToString());
            }
            return comboBox;
        }

        /// <summary>
        /// ComboBox值改变事件。
        /// </summary>
        /// <param name="sender">触发者</param>
        /// <param name="e">e</param>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (sender is ComboBox)
                {

                    ComboBoxExtend comboBoxExtend = sender as ComboBoxExtend;

                    string chartName = comboBoxExtend.SelectedItem.ToString();

                    if (!string.IsNullOrEmpty(chartName))
                        chartType = comboBoxExtend.DataKeyValue[chartName];

                    if (!string.IsNullOrEmpty(chartType))
                    {
                        if (DataSourceWay == SqlCount.Single)
                            GetQueryDataSource();
                        RenderAs renderType = (RenderAs)Enum.Parse(typeof(RenderAs), chartType);
                        this.RenderType = renderType;
                        SetSeriesType(renderType);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("ComboBox改变事件发生异常:" + ex.ToString());
            }
        }

        /// <summary>
        /// 设置图形的种类。
        /// </summary>
        /// <param RenderAs="renderType">图形类型</param>
        private void SetSeriesType(RenderAs renderType)
        {
            if (IntervalTime != 0)
            {
                config.Interval = IntervalTime;
            }
            try
            {
                if (config.DataSeriesList.Count == 1)
                {
                    chart.Series.Clear();
                    if (DataSourceWay == SqlCount.Single)
                        CreateXYValueOne();
                    else
                        ConvertPieType();

                    chart.Series[0].RenderAs = renderType;
                }
                if (config.DataSeriesList.Count > 1)
                {
                    if (chartType == RenderAs.Pie.ToString() || chartType == RenderAs.Doughnut.ToString())
                    {
                        chart.Series.Clear();
                        ConvertPieType();
                        chart.Series[0].RenderAs = renderType;
                    }
                    else
                    {
                        chart.Series.Clear();
                        ConvertPieType();
                        for (int i = 0; i < config.DataSeriesList.Count; i++)
                        {
                            chart.Series[i].RenderAs = renderType;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("切换图形异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 创建Chart的对象
        /// </summary>
        /// <returns>返回Chart对象</returns>
        private Chart CreateChart()
        {
            try
            {
                chart = new Chart();

                Legend legend = new Legend();

                Title title = new Title();

                Visifire.Charts.ToolTip toolTip = new Visifire.Charts.ToolTip();

                /*********************************************************************
                 *  Add Date：2009-07-30 AM
                 *  
                 *      Note：加一个判断，判断config是否为空。
                 * 
                 * *******************************************************************/
                if (config != null)
                {

                    chart.ScrollingEnabled = config.CanScroll;

                    chart.View3D = config.CanView3D;

                    chart.Theme = config.Style;

                    if (!string.IsNullOrEmpty(ChartStyle))
                    {
                        Style style = GetStyle(ChartStyle);
                        if (style != null)
                            chart.Style = style;
                    }

                    if (!string.IsNullOrEmpty(MainTitleStyle))
                    {
                        Style style = GetStyle(MainTitleStyle);
                        if (style != null)
                            title.Style = style;
                    }
                    if (!string.IsNullOrEmpty(PlotAreaStyle))
                    {
                        Style style = GetStyle(PlotAreaStyle);
                        if (style != null)
                            chart.PlotArea.Style = style;
                    }
                    if (!string.IsNullOrEmpty(LegendStyle))
                    {
                        Style style = GetStyle(LegendStyle);
                        if (style != null)
                            legend.Style = style;
                    }
                    if (!string.IsNullOrEmpty(ToolTipStyle))
                    {
                        Style style = GetStyle(ToolTipStyle);
                        if (style != null)
                            toolTip.Style = style;
                    }
                    title.Text = config.Title + "/" + TitleTime + "客流监视图";
                }

                chart.Titles.Add(title);

                chart.ToolTips.Add(toolTip);

                chart.Legends.Add(legend);
                //一个DataSeries
                if (config.DataSeriesList.Count == 1)
                {
                    if (DataSourceWay == SqlCount.Single)
                        chart = OneDataSeries();
                    else
                    {
                        chart = MultiDataSourceChart();
                    }
                }
                //多个DataSeries
                if (config.DataSeriesList.Count > 1)
                {
                    if (DataSourceWay == SqlCount.Single)
                        chart = MultiDataSeries();
                    else
                    {
                        chart = MultiDataSourceChart();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("创建图形异常:" + ex.ToString());
            }
            return chart;
        }

        /// <summary>
        /// 一个DataSeries，创建图形
        /// </summary>
        /// <returns></returns>
        private Chart OneDataSeries()
        {
            ChartGrid gridX = new ChartGrid();

            gridX.LineThickness = 0.5;

            ChartGrid gridY = new ChartGrid();

            gridY.LineThickness = 0.5;

            axisX = new Axis();

            Axis axisY = new Axis();

            Ticks tickX = new Ticks();

            Ticks tickY = new Ticks();

            /*********************************************************************
             *  Add Date：2009-07-30 AM
             *  
             *      Note：加一个判断，判断config是否为空。
             * 
             * *******************************************************************/
            if (config != null)
            {

                axisX.IntervalType = config.IntervalType;



                axisX.Title = config.AxisXTitle;

                axisY.IntervalType = config.IntervalType;

                axisY.Title = config.AxisYTitle;

                if (!string.IsNullOrEmpty(AxisXStyle))
                {
                    Style style = GetStyle(AxisXStyle);
                    if (style != null)
                        axisX.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYStyle))
                {
                    Style style = GetStyle(AxisYStyle);
                    if (style != null)
                        axisY.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisXLabelsStyle))
                {
                    Style style = GetStyle(AxisXLabelsStyle);
                    if (style != null)
                        axisX.AxisLabels.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYLabelsStyle))
                {
                    Style style = GetStyle(AxisYLabelsStyle);
                    if (style != null)
                        axisY.AxisLabels.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisXTicksStyle))
                {
                    Style style = GetStyle(AxisXTicksStyle);
                    if (style != null)
                        tickX.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYTicksStyle))
                {
                    Style style = GetStyle(AxisYTicksStyle);
                    if (style != null)
                        tickY.Style = style;
                }
                if (!string.IsNullOrEmpty(ChartGridXStyle))
                {
                    Style style = GetStyle(ChartGridXStyle);
                    if (style != null)
                        gridX.Style = style;
                }
                if (!string.IsNullOrEmpty(ChartGridYStyle))
                {
                    Style style = GetStyle(ChartGridYStyle);
                    if (style != null)
                        gridY.Style = style;
                }

                axisX.Ticks.Add(tickX);

                axisY.Ticks.Add(tickY);

                axisY.Grids.Add(gridY);

                axisX.Grids.Add(gridX);



                chart.AxesY.Add(axisY);

                CreateXYValueOne();

            }

            return chart;
        }

        /// <summary>
        /// 一个DataSeries，创建X轴Y轴坐标值。
        /// </summary>
        private void CreateXYValueOne()
        {
            axisX.Interval = config.Interval;

            chart.AxesX.Add(axisX);

            object xvalue = null;

            if (!String.IsNullOrEmpty(ConveterClassName))
            {
                bool isTrue = CreateConveterInstance(ConveterClassName);
                if (isTrue)
                {
                    WriteLog.Log_Info("创建ConveterInstance成功！");
                }
                else
                {
                    WriteLog.Log_Info("创建ConveterInstance失败！");
                }
            }

            /*********************************************************************
             *  Add Date：2009-07-30 AM
             *  
             *      Note：加一个判断，判断config是否为空。
             * 
             * *******************************************************************/
            if (config == null)
            {
                return;
            }

            dataSeries = new DataSeries();

            dataSeries.XValueType = config.ChartValueType;

            dataSeries.SelectionEnabled = SelectionEnabled;

            if (!string.IsNullOrEmpty(DataSeriesStyle))
            {
                Style style = GetStyle(DataSeriesStyle);

                if (style != null)
                    dataSeries.Style = style;
            }
            dataSeries.Name = config.DataSeriesList[0].LegnedName;

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                dataPoint = new DataPoint();

                if ((dataSeries.XValueType == ChartValueTypes.Numeric) || (dataSeries.XValueType == ChartValueTypes.Auto))
                {
                    if (createConverterClassInstance != null)
                        xvalue = createConverterClassInstance.Convert(dt.Rows[j][config.AxisXLabel], null, null, null);

                    if (xvalue != null)
                    {

                        dataPoint.AxisXLabel = xvalue.ToString();
                    }
                    else
                    {
                        dataPoint.AxisXLabel = dt.Rows[j][config.AxisXLabel].ToString();
                    }
                }
                else
                {
                    dataPoint.XValue = dt.Rows[j][config.AxisXLabel];
                }

                dataPoint.YValue = Convert.ToDouble(dt.Rows[j][config.DataSeriesList[0].AxisYValue]);

                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);
        }

        /// <summary>
        /// 多个DataSeries，创建图形。
        /// </summary>
        /// <returns>Chart</returns>
        private Chart MultiDataSeries()
        {
            ChartGrid gridX = new ChartGrid();

            ChartGrid gridY = new ChartGrid();

            axisX = new Axis();

            Axis axisY = new Axis();

            Ticks tickX = new Ticks();

            Ticks tickY = new Ticks();

            /*********************************************************************
             *  Add Date：2009-07-29 PM
             *  
             *      Note：加一个判断，判断config是否为空。
             * 
             * *******************************************************************/
            if (config != null)
            {

                axisX.IntervalType = config.IntervalType;

                axisX.Title = config.AxisXTitle;

                axisY.Title = config.AxisYTitle;

                if (!string.IsNullOrEmpty(AxisXStyle))
                {
                    Style style = GetStyle(AxisXStyle);
                    if (style != null)
                        axisX.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYStyle))
                {
                    Style style = GetStyle(AxisYStyle);
                    if (style != null)
                        axisY.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisXLabelsStyle))
                {
                    Style style = GetStyle(AxisXLabelsStyle);
                    if (style != null)
                        axisX.AxisLabels.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYLabelsStyle))
                {
                    Style style = GetStyle(AxisYLabelsStyle);
                    if (style != null)
                        axisY.AxisLabels.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisXTicksStyle))
                {
                    Style style = GetStyle(AxisXTicksStyle);
                    if (style != null)
                        tickX.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYTicksStyle))
                {
                    Style style = GetStyle(AxisYTicksStyle);
                    if (style != null)
                        tickY.Style = style;
                }
                if (!string.IsNullOrEmpty(ChartGridXStyle))
                {
                    Style style = GetStyle(ChartGridXStyle);
                    if (style != null)
                        gridX.Style = style;
                }
                if (!string.IsNullOrEmpty(ChartGridYStyle))
                {
                    Style style = GetStyle(ChartGridYStyle);
                    if (style != null)
                        gridY.Style = style;
                }

                axisX.Ticks.Add(tickX);

                axisY.Ticks.Add(tickY);

                axisY.Grids.Add(gridY);

                axisX.Grids.Add(gridX);

                chart.AxesY.Add(axisY);

                CreateXYValue();

                ConvertPieType();

            }

            return chart;
        }

        /// <summary>
        /// 图形之间的切换时调用。
        /// </summary>
        private void ConvertPieType()
        {
            if (chartType == RenderAs.Pie.ToString() || chartType == RenderAs.Doughnut.ToString())
            {
                try
                {
                    listDataSeries.Clear();

                    chart.Series.Clear();
                    if (DataSourceWay == SqlCount.Single)
                        CreateXYValue();
                    else
                    { CreateXYValueMultiData(); }

                    specialSeries = new DataSeries();
                    RenderAs renderType = (RenderAs)Enum.Parse(typeof(RenderAs), chartType);

                    specialSeries.RenderAs = renderType;
                    specialSeries.SelectionEnabled = SelectionEnabled;

                    if (!string.IsNullOrEmpty(DataSeriesStyle))
                    {
                        Style style = GetStyle(DataSeriesStyle);

                        if (style != null)
                            specialSeries.Style = style;
                    }

                    for (int i = 0; i < listDataSeries.Count; i++)
                    {
                        dataPoint = new DataPoint();

                        dataPoint.AxisXLabel = listDataSeries[i].Name;

                        dataPoint.YValue = listDataSeries[i].DataPoints.Sum(rf => rf.YValue);

                        specialSeries.DataPoints.Add(dataPoint);
                    }
                    chart.Series.Add(specialSeries);
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error("创建Pie和环形图形异常：" + ex.ToString());
                }
            }
            else
            {
                try
                {
                    listDataSeries.Clear();

                    chart.Series.Clear();
                    if (DataSourceWay == SqlCount.Single)
                        CreateXYValue();
                    else
                    { CreateXYValueMultiData(); }

                    if (listDataSeries != null)
                    {
                        foreach (DataSeries dataSerie in listDataSeries)
                            chart.Series.Add(dataSerie);
                    }
                    else
                    {
                        WriteLog.Log_Info("listDataSeries为空！");
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error("创建Y轴数据异常：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 创建X轴Y轴数值
        /// </summary>
        private void CreateXYValue()
        {
            axisX.Interval = config.Interval;

            chart.AxesX.Add(axisX);

            object xvalue = null;

            if (listDataSeries.Count() == 0)
            {

                /*********************************************************************
                 *  Add Date：2009-07-30 AM
                 *  
                 *      Note：加一个判断，判断config是否为空。
                 * 
                 * *******************************************************************/
                if (config == null)
                {
                    return;
                }

                for (int i = 0; i < config.DataSeriesList.Count; i++)
                {
                    dataSeries = new DataSeries();

                    dataSeries.XValueType = config.ChartValueType;

                    dataSeries.SelectionEnabled = SelectionEnabled;

                    if (!string.IsNullOrEmpty(DataSeriesStyle))
                    {
                        Style style = GetStyle(DataSeriesStyle);

                        if (style != null)
                            dataSeries.Style = style;
                    }

                    dataSeries.Name = config.DataSeriesList[i].LegnedName;

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dataPoint = new DataPoint();

                        try
                        {
                            if ((dataSeries.XValueType == ChartValueTypes.Numeric) || (dataSeries.XValueType == ChartValueTypes.Auto))
                            {
                                if (createConverterClassInstance != null)
                                    xvalue = createConverterClassInstance.Convert(dt.Rows[j][config.AxisXLabel], null, null, null);

                                if (xvalue != null)
                                {
                                    dataPoint.AxisXLabel = xvalue.ToString();
                                }
                                else
                                {
                                    dataPoint.AxisXLabel = dt.Rows[j][config.AxisXLabel].ToString();
                                }
                            }
                            else
                            {
                                dataPoint.XValue = dt.Rows[j][config.AxisXLabel];
                            }

                            dataPoint.YValue = Convert.ToDouble(dt.Rows[j][config.DataSeriesList[i].AxisYValue]);
                        }
                        catch (Exception ex)
                        {
                            WriteLog.Log_Error("要求Y轴的坐标必须为Double类型异常：" + ex.ToString());
                        }

                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    listDataSeries.Add(dataSeries);
                }
            }
        }

        /// <summary>
        /// 多个DataSeries，多个SQL情况，创建图形。
        /// </summary>
        /// <returns></returns>
        private Chart MultiDataSourceChart()
        {
            if (chart.AxesY.Count != 0)
                chart.AxesY.Clear();
            if (chart.AxesX.Count != 0)
                chart.AxesX.Clear();
            ChartGrid gridX = new ChartGrid();

            ChartGrid gridY = new ChartGrid();

            axisX = new Axis();

            Axis axisY = new Axis();

            Ticks tickX = new Ticks();

            Ticks tickY = new Ticks();

            if (config != null)
            {
                axisX.IntervalType = config.IntervalType;

                axisX.Interval = config.Interval;

                axisX.Title = config.AxisXTitle;

                axisY.Title = config.AxisYTitle;

                if (!string.IsNullOrEmpty(AxisXStyle))
                {
                    Style style = GetStyle(AxisXStyle);
                    if (style != null)
                        axisX.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYStyle))
                {
                    Style style = GetStyle(AxisYStyle);
                    if (style != null)
                        axisY.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisXLabelsStyle))
                {
                    Style style = GetStyle(AxisXLabelsStyle);
                    if (style != null)
                        axisX.AxisLabels.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYLabelsStyle))
                {
                    Style style = GetStyle(AxisYLabelsStyle);
                    if (style != null)
                        axisY.AxisLabels.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisXTicksStyle))
                {
                    Style style = GetStyle(AxisXTicksStyle);
                    if (style != null)
                        tickX.Style = style;
                }
                if (!string.IsNullOrEmpty(AxisYTicksStyle))
                {
                    Style style = GetStyle(AxisYTicksStyle);
                    if (style != null)
                        tickY.Style = style;
                }
                if (!string.IsNullOrEmpty(ChartGridXStyle))
                {
                    Style style = GetStyle(ChartGridXStyle);
                    if (style != null)
                        gridX.Style = style;
                }
                if (!string.IsNullOrEmpty(ChartGridYStyle))
                {
                    Style style = GetStyle(ChartGridYStyle);
                    if (style != null)
                        gridY.Style = style;
                }

                axisX.Ticks.Add(tickX);

                axisY.Ticks.Add(tickY);

                axisY.Grids.Add(gridY);

                axisX.Grids.Add(gridX);

                //chart.AxesX.Add(axisX);

                chart.AxesY.Add(axisY);

                //CreateXYValueMultiData();

                ConvertPieType();
            }

            return chart;
        }

        /// <summary>
        /// 创建X轴Y轴数值
        /// </summary>
        private void CreateXYValueMultiData()
        {
            if (axisX != null)
            {
                axisX.Interval = config.Interval;

                chart.AxesX.Add(axisX);

                object xvalue = null;

                Xlist = new List<object>();

                Ylist = new List<List<string>>();

                if (!String.IsNullOrEmpty(ConveterClassName))
                {
                    bool isTrue = CreateConveterInstance(ConveterClassName);
                    if (isTrue)
                    {
                        WriteLog.Log_Info("创建ConveterInstance成功！");
                    }
                    else
                    {
                        WriteLog.Log_Info("创建ConveterInstance失败！");
                    }
                }

                try
                {
                    if (!String.IsNullOrEmpty(UserControlClassName))
                    {
                        bool isTrue = CreateInstance(UserControlClassName);

                        if (isTrue)
                        {
                            createClassInstance.GetDataSource(out Xlist, out Ylist, (int)config.ScaleValue);
                        }
                        else
                        {
                            WriteLog.Log_Info("创建用户控件对象失败！");
                        }

                    }
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error("Chart控件中创建用户对象出现异常:" + ex.ToString());
                }

                string beginTime = "开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');


                if (Xlist.Count == 0)// || Ylist.Count == 0)
                {
                    return;
                }
                double yValue;

                if (listDataSeries.Count() == 0)
                {
                    if (config == null)
                    {
                        return;
                    }
                    for (int i = 0; i < config.DataSeriesList.Count; i++)
                    {
                        dataSeries = new DataSeries();

                        RenderAs renderType = (RenderAs)Enum.Parse(typeof(RenderAs), chartType);

                        dataSeries.RenderAs = renderType;

                        dataSeries.XValueType = config.ChartValueType;

                        dataSeries.SelectionEnabled = SelectionEnabled;

                        if (!string.IsNullOrEmpty(DataSeriesStyle))
                        {
                            Style style = GetStyle(DataSeriesStyle);

                            if (style != null)
                                dataSeries.Style = style;
                        }

                        dataSeries.Name = config.DataSeriesList[i].LegnedName;

                        for (int j = 0; j < Xlist.Count; j++)
                        {
                            dataPoint = new DataPoint();

                            try
                            {
                                if ((dataSeries.XValueType == ChartValueTypes.Numeric) || (dataSeries.XValueType == ChartValueTypes.Auto))
                                {
                                    if (createConverterClassInstance != null)
                                        xvalue = createConverterClassInstance.Convert(Xlist[j], null, null, null);

                                    if (xvalue != null)
                                    {
                                        dataPoint.AxisXLabel = xvalue.ToString();
                                    }
                                    else
                                    {
                                        dataPoint.AxisXLabel = Xlist[j].ToString();
                                    }
                                    try
                                    {
                                        if (Ylist.Count != 0 && i < Ylist.Count && j < Ylist[i].Count)
                                        {
                                            yValue = Double.Parse(Ylist[i][j]);
                                        }
                                        else
                                        {
                                            yValue = 0;
                                        }
                                    }
                                    catch
                                    {
                                        yValue = 0;
                                    }

                                    dataPoint.YValue = yValue;

                                }
                                else
                                {
                                    dataPoint.XValue = Xlist[j];
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log_Error("要求Y轴的坐标必须为Double类型异常：" + ex.ToString());
                            }

                            dataSeries.DataPoints.Add(dataPoint);


                        }
                        listDataSeries.Add(dataSeries);
                    }
                }

                beginTime += "-->结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                System.Console.WriteLine(beginTime);
            }
        }

        /// <summary>
        /// Event handler for Tick event of Timer
        /// </summary>
        /// <param name="sender"> System.Windows.Threading.DispatcherTimer</param>
        /// <param name="e">EventArgs</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        ///导出图片，导出后可以打印，另存为其他图片。
        /// </summary>
        /// <param name="path">导出路径</param>
        /// <param name="surface">chart对象</param>
        private bool ExportToPng(string path, Chart surface)
        {
            try
            {
                if (path == null || surface == null) return false;

                // Save current canvas transform
                Transform transform = surface.LayoutTransform;
                // reset current transform (in case it is scaled or rotated)
                surface.LayoutTransform = null;

                // Create a render bitmap and push the surface to it
                RenderTargetBitmap renderBitmap =
                  new RenderTargetBitmap(
                    (int)surface.ActualWidth,
                    (int)surface.ActualHeight + (int)textBlockHeight,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderBitmap.Render(surface);

                // Create a file stream for saving image
                using (FileStream outStream = new FileStream(path, FileMode.Create))
                {
                    // Use png encoder for our data
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    // save the data to the stream
                    encoder.Save(outStream);
                }

                // Restore previously saved layout
                surface.LayoutTransform = transform;
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("导出图片产生异常：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 创建控件中的按钮。
        /// </summary>
        /// <param name="content">控件上显示的内容</param>
        /// <param name="controlName">控件的名字</param>
        /// <returns>返回Button的实例</returns>
        private Button CreateButton(string content, string controlName)
        {
            if (string.IsNullOrEmpty(controlName)) return null;
            try
            {
                Button btn = new Button();
                btn.Width = ActionManager.Btn_Default_Width;
                btn.Height = ActionManager.Btn_Default_Height;
                btn.Content = content;
                if (config.ButtonStyle != null)
                    UIHelper.SetControlStyle(btn, config.ButtonStyle);
                btn.Click += new RoutedEventHandler(btn_Click);
                btn.Name = controlName;
                return btn;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("创建Button产生异常:" + ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// 点击保存按钮时触发
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;

                if (btn.Name == "btnSaveImage")
                {
                    SaveImage();
                }
                if (btn.Name == "btnRefreshImage")
                {
                    try
                    {
                        QueryRefresh();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 保存图片,图片类型为：Png、Jpg。
        /// </summary>
        public void SaveImage()
        {
            try
            {
                System.Windows.Forms.SaveFileDialog saveImage = new System.Windows.Forms.SaveFileDialog();

                saveImage.InitialDirectory = "C:\\";
                saveImage.RestoreDirectory = true;
                saveImage.Filter = "Png Image(*.png)|*.png|Jpg Imgae(*jpg)|*.jpg|All Files|*.*";

                saveImage.Title = "请您选择保存图片文件存放的路径";
                try
                {
                    if (saveImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ExportToPng(saveImage.FileName, chart);
                        MessageBox.Show("保存成功");
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("导出图片中出现异常:" + ex.ToString());
            }
        }

        /// <summary>
        /// 自动刷新图像
        /// </summary>
        public void Refresh()
        {              
            string beginTime = "开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');

            try
            {


                if (chartType == null)
                {
                    chartType = RenderType.ToString();
                }
                if (!string.IsNullOrEmpty(chartType))
                {
                    if (DataSourceWay == SqlCount.Single)
                    {
                        //if (dataSource == null)
                        //{
                        bool isTrue = GetDataSource();
                        if (isTrue)
                        {
                            WriteLog.Log_Info("读取数据源成功！");
                        }
                        else
                        {
                            WriteLog.Log_Info("读取数据源失败！");
                        }
                        //}
                        RenderAs renderType = (RenderAs)Enum.Parse(typeof(RenderAs), chartType);

                        SetSeriesType(renderType);
                    }
                    else
                    {
                        RenderAs renderType = (RenderAs)Enum.Parse(typeof(RenderAs), chartType);

                        SetSeriesType(renderType);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("自动刷新中是出现异常：" + ex.ToString());
            }
            beginTime += "-->结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            //System.Console.WriteLine("Refresh() -->" + beginTime);

        }

        /// <summary>
        /// 查询刷新图像
        /// </summary>
        private void QueryRefresh()
        {
            try
            {
                if (chartType == null)
                {
                    chartType = RenderType.ToString();
                }
                if (!string.IsNullOrEmpty(chartType))
                {
                    if (DataSourceWay == SqlCount.Single)
                    {
                        bool isTrue = GetQueryDataSource();
                        if (isTrue)
                        {
                            WriteLog.Log_Info("读取数据源成功！");
                        }
                        else
                        {
                            WriteLog.Log_Info("读取数据源失败！");
                        }
                    }
                    RenderAs renderType = (RenderAs)Enum.Parse(typeof(RenderAs), chartType);

                    SetSeriesType(renderType);
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("自动刷新中是出现异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 创建用户控件对象
        /// </summary>
        /// <param name="className">用户控件类名称</param>
        /// <returns>True:成功，False：失败</returns>
        private bool CreateInstance(string className)
        {

            try
            {
                Type type = Type.GetType(className);

                if (type != null)
                {
                    if (createClassInstance == null)
                        createClassInstance = Activator.CreateInstance(type) as IChartDataSource;

                    if (createClassInstance != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    WriteLog.Log_Info("ComboBox中创建用户控件type：为空！");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("创建类对象时出错：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 创建用户控件对象
        /// </summary>
        /// <param name="className">用户控件类名称</param>
        /// <returns>True:成功，False：失败</returns>
        private bool CreateConveterInstance(string className)
        {
            try
            {
                Type type = Type.GetType(className);

                if (type != null)
                {
                    if (createConverterClassInstance == null)
                        createConverterClassInstance = Activator.CreateInstance(type) as IValueConverter;

                    if (createConverterClassInstance != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    WriteLog.Log_Info("ComboBox中创建用户控件type：为空！");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("创建类对象时出错：" + ex.ToString());
                return false;
            }
        }

        #endregion

        #region [       Create Data Source       ]

        /// <summary>
        /// 创建数据源
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private bool CreateDataSource()
        {
            try
            {

                /*********************************************************************
                 *  Add Date：2009-07-29 PM
                 *  
                 *      Note：加一个判断，判断config是否为空。
                 * 
                 * *******************************************************************/
                if (config == null)
                {
                    return false;
                }

                dataSource = DataSourceManager.LookupDataSourceByName(config.DataSourceName);

                if (dataSource != null)
                {
                    DataSourceManager.RegesiterDataSourceClient(dataSource, this);
                    return true;
                }
                WriteLog.Log_Error("Create dataSource name=[" + config.DataSourceName + "] error !");

                return false;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("Create dataSource error:" + ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// 创建查询数据源
        /// </summary>
        /// <returns>True：成功，False：失败</returns>
        private bool CreateQueryDataTable()
        {
            try
            {
                dt = dataSource.GetDataTable();

                if (dt == null || dt.Rows.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);

                return false;
            }
        }

        /// <summary>
        /// 创建自动刷新数据源
        /// </summary>
        /// <returns>数据表</returns>
        private bool CreateDataTable()
        {
            try
            {

                /*********************************************************************
                 *  Add Date：2009-07-30 AM
                 *  
                 *      Note：加一个判断，判断dataSource是否为空。
                 * 
                 * *******************************************************************/
                if (dataSource == null)
                {
                    return false;
                }

                dt = dataSource.GetDataTable();

                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);

                return false;
            }
        }
        #endregion

        #region [       IDataSourceClient 成员      ]

        /// <summary>
        /// 刷新时调用
        /// </summary>
        public void HandleDataSourceChange()
        {
            Refresh();
        }

        /// <summary>
        /// 释放用户控件对象
        /// </summary>
        public void HandleDataSourceDispose()
        {
            dataSource = null;
        }

        #endregion
    }
}
