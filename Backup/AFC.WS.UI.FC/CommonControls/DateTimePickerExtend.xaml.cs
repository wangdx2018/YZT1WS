#region [       Copyright (C), 2009,  中软AFC, Token Shen.     ]
/************************************************************
  FileName: DateTimePickerExtend.xaml
  
  Author: 沈克涛    
 
  Version :  1.0   
 
  Date:20090729
 
  Description:获得日期，并设置日期   
 
  Function List:  
 
    1. SetStyle  // ---> 设置样式
 
  History: 
 
      <author>   <time>      <version >     <desc>
 
      沈克涛    2009/07/29     1.0         增加代码说明
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
using System.Windows.Shapes;
using System.ComponentModel;
using AFC.WS.UI.Common;
using Microsoft.Windows.Controls;
#endregion

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// 格式化为符合需要的数据，
    /// 
    /// 保持和数据库一致，方便查询和修改。
    /// </summary>
    public enum FormatType
    {
        /// <summary>
        /// 20080808
        /// </summary>
        yyyyMMdd = 0,
        /// <summary>
        /// 080808
        /// </summary>
        HHmmss = 1,
        yyyyMMddHHmmss = 2
    }

    /// <summary>
    /// DateTimePickerExtend.xaml 的交互逻辑，
    /// 
    /// 在原有DateTimePicker的基础上添加了日期的赋值，时间的显示和修改，显示日期时间。
    /// 
    /// wangdx 20110414 增加了时间格式的赋值，通过XML属性配置。注释了Initliaize的中的DataTimeFormat的注释。
    /// 
    /// 如果在Get中为空，设置默认值。
    /// </summary>
    public partial class DateTimePickerExtend : UserControl, ICommonEdit
    {

        #region [       Constructor       ]
        /// <summary>
        /// 
        /// </summary>
        public DateTimePickerExtend()
        {
            InitializeComponent();
           

        }

        #endregion

        #region [       Properties       ]
        // ---> 设定DatePicker格式
        /// <summary>
        /// 设定DatePicker格式
        /// </summary>
        [
        Description("设定DatePicker格式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public DatePickerFormat DatePickerFormat
        {
            get { return this.datePicker.SelectedDateFormat; }
            set { this.datePicker.SelectedDateFormat = value; }
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
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public string DatePickerStyle
        {
            get { return _datePickerStyle; }
            set { _datePickerStyle = value; }
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
        Category("DateTimePickerExtend"),
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
        Category("DateTimePickerExtend"),
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
        private string _ContentDatePicker;
        /// <summary>
        /// 设置DateTimePicker值
        /// </summary>
        [
        Description("设定DatePicker内容。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public string ContentDatePicker
        {
            get { return _ContentDatePicker; }
            set { _ContentDatePicker = value; }
        }

        /// <summary>
        /// 设置DateTimePicker是否可见
        /// </summary>
        [
        Description("设置DateTimePicker是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
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
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public bool CanEnabled
        {
            get { return this.datePicker.IsEnabled; }
            set { this.datePicker.IsEnabled = value; }
        }
        /// <summary>
        /// 格式化为用户需要的数据，内部使用
        /// </summary>
        [
        Description("设置FormatDateTime是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public string FormatDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 设置是否显示当前日期
        /// </summary>
        [
        Description("设置是否显示当前日期"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public bool SetIsNull
        {
            get { return this.datePicker.IsNull; }
            set { this.datePicker.IsNull = value; }
        }

        [
       Description("获取日期控件"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("DateTimePickerExtend"),
       Filter()
       ]
        public DatePicker DatePickerControl
        {
            get { return this.datePicker; }
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
                    this.Width = ControlWidth;
                }
                else
                {
                    //this.Width = 150;
                }
                if (ControlHeight != 0)
                {
                    this.Height = ControlHeight;
                }
                else
                {
                    //this.Height = 23;
                }

                if (DatePickerStyle != null)
                {
                    Style style = this.FindResource(DatePickerStyle) as Style;

                    this.datePicker.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置DatePicker出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       ICommonEdit 成员       ]

        /// <summary>
        /// 返回DateTimePicker的内容
        /// </summary>
        /// <returns>object</returns>
        public object GetControlValue()
        {
            string datePickerText = null;

            if (!string.IsNullOrEmpty(FormatDateTime))
            {
                if (!string.IsNullOrEmpty(this.datePicker.Text))
                {
                    datePickerText = Convert.ToDateTime(this.datePicker.Text).ToString(FormatDateTime);
                }
                return datePickerText;
            }
            else
            {
                 FormatDateTime = "yyyy-MM-dd";
            }
           
            try
            {
                if (!string.IsNullOrEmpty(this.datePicker.Text))
                {
                    if (DatePickerFormat == DatePickerFormat.Time)
                    {
                        datePickerText = Convert.ToDateTime(this.datePicker.Text).ToString("HHmmss");
                    }
                    else if (DatePickerFormat == DatePickerFormat.FullDate)
                    {
                        datePickerText = Convert.ToDateTime(this.datePicker.Text).ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        datePickerText = Convert.ToDateTime(this.datePicker.Text).ToString("yyyy-MM-dd");
                    }
                }
                return datePickerText;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置DateTimePicker值
        /// </summary>
        /// <param name="value">值</param>
        public void SetControlValue(object value)
        {
            if (value != null)
            {
                if (datePicker != null)
                {
                    
                    this.datePicker.Text = value.ToString();
                }
            }
            else
            {
                this.datePicker.SetTextNull = "";
                //if (DatePickerFormat == DatePickerFormat.Time)
                //{
                //    this.datePicker.Text = DateTime.Now.ToLongTimeString();
                //}
                //else if (DatePickerFormat == DatePickerFormat.FullDate)
                //{
                //    this.datePicker.Text = DateTime.Now.ToString();
                //}
                //else
                //{
                //    this.datePicker.Text = DateTime.Now.ToLongDateString();
                //}

            }
        }

        /// <summary>
        /// 初始化控件信息
        /// </summary>
        public void Initialize()
        {
            try
            {
                SetStyle();
                if (!string.IsNullOrEmpty(this.datePicker.Text))
                    this.ContentDatePicker = this.datePicker.Text;
               // FormatDateTime = "yyyy-MM-dd";
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("DateTimePickerExtend控件Initialize()方法异常:" + ex.ToString());
            }
        }

        #endregion

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ContentDatePicker = this.datePicker.Text;
        }

    }
}
