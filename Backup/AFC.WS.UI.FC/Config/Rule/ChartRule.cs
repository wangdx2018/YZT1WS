using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visifire.Charts;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 图表规则文件类；
    /// 
    /// 用来配置图表控件的信息，如可以配置如下属性：
    /// 
    /// 是否动态刷新按钮、是否要图表是否需要滚动条、X轴上绑定字段、
    /// 图表的标题、Y轴标题、皮肤主题、刻度值、时间间隔、是否3D显示、
    /// 是否配置刷新按钮、是否配置保存图片按钮、坐标元素间隔、
    /// 坐标类型、Button样式、ComboBox样式、数据源名称、图表的位置、
    /// 图表种类、DataSeries集合、定时器刷新时间设置(单位/秒)、 
    /// 设置Button的位置(左、中、右)等属性；
    /// </summary>
    public class ChartRule
    {

        #region --> Property
        /// <summary>
        /// 是否动态刷新
        /// </summary>
        private bool _CanDynamicRefurbish = false;
        /// <summary>
        /// 是否动态刷新
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否动态刷新"),
        Description("是否动态刷新"),
        Category("属性设置")]
        public bool CanDynamicRefurbish
        {
            get { return _CanDynamicRefurbish; }
            set { _CanDynamicRefurbish = value; }
        }
        /// <summary>
        /// 是否要滚动条
        /// </summary>
        private bool _CanScroll = false;
        /// <summary>
        /// 是否要滚动条
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否要滚动条"),
        Description("是否要滚动条"),
        Category("属性设置")]
        public bool CanScroll
        {
            get { return _CanScroll; }
            set { _CanScroll = value; }
        }
        /// <summary>
        ///  X轴上绑定字段
        /// </summary>
        private string _AxisXLabel;
        /// <summary>
        /// X轴上绑定字段
        /// </summary>
        [Description("X轴上绑定字段"),
        XmlAttribute(),
        DisplayName("X轴上绑定字段"),
        Category("属性设置")]
        public string AxisXLabel
        {
            get { return _AxisXLabel; }
            set { _AxisXLabel = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        [XmlAttribute(),
        DisplayName("标题"),
        Description("标题"),
        Category("属性设置")]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        /// <summary>
        /// X轴标题
        /// </summary>
        private string _AxisXTitle;
        /// <summary>
        /// X轴标题
        /// </summary>
        [Description("X轴标题"),
        XmlAttribute(),
        DisplayName("X轴标题"),
        Category("属性设置")]
        public string AxisXTitle
        {
            get { return _AxisXTitle; }
            set { _AxisXTitle = value; }
        }
        /// <summary>
        /// Y轴标题
        /// </summary>
        private string _AxisYTitle;
        /// <summary>
        /// Y轴标题
        /// </summary>
        [XmlAttribute(),
        DisplayName("Y轴标题"),
        Description("Y轴标题"),
        Category("属性设置")]
        public string AxisYTitle
        {
            get { return _AxisYTitle; }
            set { _AxisYTitle = value; }
        }
        /// <summary>
        /// 皮肤主题
        /// </summary>
        private string _Style = string.Empty;
        /// <summary>
        /// 皮肤主题
        /// </summary>
        [XmlAttribute(),
        DisplayName("皮肤主题"),
        Category("属性设置")]
        public string Style
        {
            get { return _Style; }
            set { _Style = value; }
        }
        /// <summary>
        /// 刻度值
        /// </summary>
        private int _ScaleValue = 5;
        /// <summary>
        /// 刻度值
        /// </summary>
        [XmlAttribute(),
        DisplayName("刻度值"),
        Description("刻度值"),
        Category("属性设置")]
        public int ScaleValue
        {
            get { return _ScaleValue; }
            set { _ScaleValue = value; }
        }
        /// <summary>
        /// 时间间隔
        /// </summary>
        private IntervalTypes _IntervalType = IntervalTypes.Auto;
        /// <summary>
        /// 时间间隔
        /// </summary>
        [XmlAttribute(),
        DisplayName("时间间隔"),
        Description("时间间隔"),
        Category("属性设置")]
        public IntervalTypes IntervalType
        {
            get { return _IntervalType; }
            set { _IntervalType = value; }
        }
        /// <summary>
        /// 是否3D显示
        /// </summary>
        private bool _CanView3D = true;
        /// <summary>
        /// 是否3D显示
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否3D显示"),
        Category("属性设置")]
        public bool CanView3D
        {
            get { return _CanView3D; }
            set { _CanView3D = value; }
        }
        /// <summary>
        /// 是否配置刷新按钮
        /// </summary>
        private bool _CanRefurbish = true;
        /// <summary>
        /// 是否配置刷新按钮
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否配置刷新按钮"),
        Description("是否配置刷新按钮"),
        Category("属性设置")]
        public bool CanRefurbish
        {
            get { return _CanRefurbish; }
            set { _CanRefurbish = value; }
        }

        /// <summary>
        /// 是否配置保存图片按钮
        /// </summary>
        private bool _CanSaveImage = true;
        /// <summary>
        /// 是否配置保存图片按钮
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否配置保存图片按钮"),
        Description("是否配置保存图片按钮"),
        Category("属性设置")]
        public bool CanSaveImage
        {
            get { return _CanSaveImage; }
            set { _CanSaveImage = value; }
        }
        /// <summary>
        /// 坐标元素间隔
        /// </summary>        
        private double _Interval = 10.0;
        /// <summary>
        /// 坐标元素间隔
        /// </summary>
        [XmlAttribute(),
        Description("坐标元素间隔"),
        DisplayName("坐标元素间隔"),
        Category("属性设置")]
        public double Interval
        {
            get { return _Interval; }
            set { _Interval = value; }
        }

        /// <summary>
        /// 坐标类型
        /// </summary>
        private ChartValueTypes _ChartValueType = ChartValueTypes.Auto;
        /// <summary>
        /// 坐标类型
        /// </summary>
        [XmlAttribute(),
        DisplayName("坐标类型"),
        Description("坐标类型"),
        Category("属性设置")]
        public ChartValueTypes ChartValueType
        {
            get { return _ChartValueType; }
            set { _ChartValueType = value; }
        }

        /// <summary>
        /// Button样式
        /// </summary>
        private string _ButtonStyle;
        /// <summary>
        /// Button样式
        /// </summary>
        [XmlAttribute(),
        DisplayName("Button样式"),
        Description("Button样式"),
        Category("属性设置")]
        public string ButtonStyle
        {
            get { return _ButtonStyle; }
            set { _ButtonStyle = value; }
        }

        /// <summary>
        /// ComboBox样式
        /// </summary>
        private string _ComboBoxStyle;
        /// <summary>
        /// ComboBox样式
        /// </summary>
        [XmlAttribute(),
        DisplayName("ComboBox样式"),
        Description("ComboBox样式"),
        Category("属性设置")]
        public string ComboBoxStyle
        {
            get { return _ComboBoxStyle; }
            set { _ComboBoxStyle = value; }
        }

        /// <summary>
        /// 数据源名称
        /// </summary>
        private string _DataSourceName;
        /// <summary>
        /// 数据源名称
        /// </summary>
        [XmlAttribute(),
        DisplayName("数据源名称"),
        Description("数据源名称"),
        Category("数据源设置")]
        public string DataSourceName
        {
            get { return _DataSourceName; }
            set { _DataSourceName = value; }
        }

        /// <summary>
        /// 图表的位置。
        /// </summary>
        private ChartLocations _ChartLocation = ChartLocations.Bottom;
        /// <summary>
        /// 图表的位置。
        /// </summary>
        [XmlAttribute(),
        DisplayName("图表的位置"),
        Description("图表的位置"),
        Category("属性设置")]
        public ChartLocations ChartLocation
        {
            get { return _ChartLocation; }
            set { _ChartLocation = value; }
        }

        /// <summary>
        /// 图表种类
        /// </summary>
        private List<ChartProperty> _ChartList;
        /// <summary>
        /// 图表种类
        /// </summary>
        [DisplayName("图表种类"),
        Description("图表种类"),
        Category("集合属性设置")]
        public List<ChartProperty> ChartList
        {
            get
            {
                if (null == _ChartList)
                {
                    _ChartList = new List<ChartProperty>();
                }
                return _ChartList;
            }
            set { _ChartList = value; }
        }
        /// <summary>
        /// DataSeries集合
        /// </summary>        
        private List<DataSeriesProperty> _DataSeriesList;
        /// <summary>
        /// DataSeries集合
        /// </summary>
        [DisplayName("DataSeries集合"),
        Description("DataSeries集合"),
        Category("集合属性设置")]
        public List<DataSeriesProperty> DataSeriesList
        {
            get
            {
                if (null == _DataSeriesList)
                {
                    _DataSeriesList = new List<DataSeriesProperty>();
                }
                return _DataSeriesList;
            }
            set { _DataSeriesList = value; }
        }

        /// <summary>
        /// 定时器刷新时间设置(单位/秒)。
        /// </summary>
        private int _AutoRefreshTime = 10;
        /// <summary>
        /// 定时器刷新时间设置(单位/秒)。
        /// </summary>
        [XmlAttribute(),
        DisplayName("刷新时间设置(单位/秒)"),
        Description("定时器刷新时间设置(单位/秒)；最大时间为3600秒，最小为1秒。"),
        Category("属性设置")]
        public int AutoRefreshTime
        {
            get { return _AutoRefreshTime; }
            set
            {
                if (JudgeSecondCondition(value))
                {
                    _AutoRefreshTime = value;
                }
                else
                {
                    MessageBox.Show("定时器刷新时间设置的最大时间为3600秒，最小为1秒。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// 设置Button的位置(左、中、右)
        /// </summary>
        private System.Windows.HorizontalAlignment _ButtonLocation = System.Windows.HorizontalAlignment.Center;
        /// <summary>
        /// 设置Button的位置(左、中、右)
        /// </summary>
        [XmlAttribute(),
        DisplayName("设置Button的位置"),
        Description("设置Button的位置(左、中、右)"),
        Category("属性设置")]
        public System.Windows.HorizontalAlignment ButtonLocation
        {
            get { return _ButtonLocation; }
            set { _ButtonLocation = value; }
        }

        #endregion --> Property

        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return AxisXTitle == null ? this.GetType().Name : AxisXTitle;
        }
        /// <summary>
        /// 判断自动刷新时间是否合在规定范围。
        /// </summary>
        /// <param name="second">时间（秒）</param>
        /// <returns>true:时间设置正确；false:时间设置失败。</returns>
        private bool JudgeSecondCondition(int second)
        {
            bool result = false;

            if (second > 0 && second <= 3600)
            {
                result = true;
            }
            return result;
        }

        #endregion --> Methods

    }
}

