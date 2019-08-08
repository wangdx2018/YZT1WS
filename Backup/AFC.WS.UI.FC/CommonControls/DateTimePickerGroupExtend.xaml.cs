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
using System.Windows.Shapes;
using System.ComponentModel;
using AFC.WS.UI.Common;
using Microsoft.Windows.Controls;
using AFC.WS.UI.CommonControls;
#endregion


namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// DateTimePickerGroupExtend.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePickerGroupExtend : UserControl, ICommonEdit
    {

        #region [       Constructor       ]
        /// <summary>
        /// 
        /// </summary>
        public DateTimePickerGroupExtend()
        {
            InitializeComponent();

        }

        #endregion

        #region [       Declarataions       ]

        /// <summary>
        ///返回名称列表
        /// </summary>
        private List<string> _dataValue = null;

        #endregion

        #region [       Properties       ]
        // ---> 设定DatePicker格式
        /// <summary>
        /// 设定DatePicker格式
        /// </summary>
        [
        Description("设定DatePicker格式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public DatePickerFormat DatePickerFormat
        {
            get { return this.datePickerStart.SelectedDateFormat; }
            set { this.datePickerStart.SelectedDateFormat = value; this.datePickerEnd.SelectedDateFormat = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _datePickerStyle;

        // ---> 设定ComboBox样式
        /// <summary>
        /// 设定ComboBox样式
        /// </summary>
        [
        Description("设定DatePicker样式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public string DatePickerStyle
        {
            get { return _datePickerStyle; }
            set { _datePickerStyle = value; }
        }


        private string _labelText;

        [
       Description("设定Label显示。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("DateTimePickerGroupExtend"),
       Filter()
       ]

        public string LabelText
        {
            get { return _labelText; }
            set { _labelText = value; }
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
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public int ControlWidth
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
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public int ControlHeight
        {
            get { return _controlHeight; }
            set { _controlHeight = value; }
        }
        /// <summary>
        /// 设置DateTimePicker值
        /// </summary>
        private string _ContentDatePickerStart;


        private string _ContentDatePickerEnd;
        /// <summary>
        /// 设置DateTimePicker值
        /// </summary>
        [
        Description("设定DatePicker内容。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public string ContentDatePickerStart
        {
            get { return _ContentDatePickerStart; }
            set { _ContentDatePickerStart = value; }
        }

        public string ContentDatePickerEnd
        {
            get { return _ContentDatePickerEnd; }
            set { _ContentDatePickerEnd = value; }
        }


        /// <summary>
        /// 设置DateTimePicker是否可见
        /// </summary>
        [
        Description("设置DateTimePicker是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }
        /// <summary>
        /// 设置DateTimePicker是否可见
        /// </summary>
        [
        Description("设置DateTimePicker是否可用"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public bool CanEnabled
        {
            get { return this.datePickerStart.IsEnabled; }
            set { this.datePickerStart.IsEnabled = value; }
        }
        /// <summary>
        /// 格式化为用户需要的数据，内部使用
        /// </summary>
        [
        Description("设置FormatDateTime是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public string FormatDateTime
        {
            get;
            set;
        }


        /// <summary>
        /// 返回
        /// </summary>
        public List<string> GetValueList
        {
            get { return _dataValue; }
            set { _dataValue = value; }
        }



        /// <summary>
        /// 设置初始值（根据名称）
        /// </summary>
        private List<string> _setDateTimeList;
        /// <summary>
        /// 设置初始选中值（根据名称）
        /// </summary>
        [
        Description("设置初始选中值（根据名称）"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerGroupExtend"),
        Filter()
        ]
        public List<string> SetDateTimeList
        {
            get { return _setDateTimeList; }
            set { _setDateTimeList = value; }
        }

        #endregion

        #region [       Set Style      ]

        /// <summary>
        /// 设置DateTimePicker样式。
        /// </summary>
        private void SetStyle()
        {
            try
            {
                //this.datePicker.Text = DateTime.Now.ToString();
                if (ControlWidth != 0)
                {
                    this.labelStart.Width = ControlWidth*0.3;
                    this.labelEnd.Width = ControlWidth * 0.1;
                    this.datePickerStart.Width = ControlWidth * 0.3;
                    this.datePickerEnd.Width = ControlWidth * 0.3;
                }
                else if (DatePickerFormat == DatePickerFormat.Time || DatePickerFormat == DatePickerFormat.FullDate)
                {
                    this.labelStart.Width = 180;
                    this.datePickerStart.Width = 180;
                    this.labelEnd.Width = 20;
                    this.datePickerEnd.Width = 180;
                }
                else
                {
                    this.labelStart.Width = 120;
                    this.datePickerStart.Width = 120;
                    this.labelEnd.Width = 20;
                    this.datePickerEnd.Width = 120;
                }
                if (ControlHeight != 0)
                {
                    this.Height = ControlHeight;
                }
                else
                {
                    this.Height = 23;
                }

                if (DatePickerStyle != null)
                {
                    Style style = this.FindResource(DatePickerStyle) as Style;

                    this.datePickerStart.Style = style;
                    this.datePickerEnd.Style = style;


                    Style labelstyle = this.FindResource("LabelStyle") as Style;

                    this.labelStart.Style = labelstyle;
                    this.labelEnd.Style = labelstyle;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置DatePicker出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       Private Methods       ]

        /// <summary>
        /// 设置初始值
        /// </summary>
        /// <param name="list">初期值列表</param>
        private void SetDateTime(List<string> list)
        {
            if (list != null && list.Count == 2)
            {
                this.datePickerStart.DatePickerContent = list[0].ToString();
                this.datePickerEnd.DatePickerContent = list[1].ToString();
            }
            else
            {
                this.datePickerStart.SetTextNull = "";
                this.datePickerEnd.SetTextNull = ""; 
            }
        }

         #endregion

        #region ICommonEdit 成员

        public void Initialize()
        {
            try
            {
                SetStyle();
                if (!string.IsNullOrEmpty(ContentDatePickerStart))
                {
                    this.datePickerStart.DatePickerContent = ContentDatePickerStart;
                }
                else
                {
                    ContentDatePickerStart = this.datePickerStart.Text;
                }
                if (!string.IsNullOrEmpty(ContentDatePickerEnd))
                {
                    this.datePickerEnd.DatePickerContent = ContentDatePickerEnd;
                }
                else
                {
                    ContentDatePickerEnd = this.datePickerEnd.Text;
                }
                FormatDateTime = "yyyy-MM-dd";

                if (string.IsNullOrEmpty(LabelText))
                {
                    LabelText = "日期范围";
                }
                this.labelStart.Content = LabelText;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("设置样式失败:" + ex.ToString());
            }
        }
        public object GetControlValue()
        {

            string[] arrDatePicker = new string[2];
            string datePickerStart = "";
            string datePickerEnd="";
            try
            {
                if (!string.IsNullOrEmpty(this.datePickerStart.Text))
                    {
                        if (DatePickerFormat == DatePickerFormat.Time || DatePickerFormat == DatePickerFormat.FullDate)
                        {
                            datePickerStart = Convert.ToDateTime(this.datePickerStart.Text).ToString("HHmmss");
                        }
                        else
                        {
                            datePickerStart = Convert.ToDateTime(this.datePickerStart.Text).ToString("yyyy-MM-dd");
                        }
                       
                    }

                    if (!string.IsNullOrEmpty(this.datePickerEnd.Text))
                        {
                            if (DatePickerFormat == DatePickerFormat.Time || DatePickerFormat == DatePickerFormat.FullDate)
                            {
                                datePickerEnd = Convert.ToDateTime(this.datePickerEnd.Text).ToString("HHmmss");
                            }
                            else
                            {
                                datePickerEnd = Convert.ToDateTime(this.datePickerEnd.Text).ToString("yyyy-MM-dd");
                            }
                           
                        }

                    arrDatePicker[0] = datePickerStart;
                    arrDatePicker[1] = datePickerEnd;
                    return arrDatePicker as object;
             }
            
            catch
            {
                return null;
            }
 

        }

        public void SetControlValue(object value)
        {
            try
            {
                SetDateTime((List<string>)value);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }
        }

        #endregion
    }
}
       