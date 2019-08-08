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
using System.Windows.Shapes;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using System.ComponentModel;

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// DateSelect.xaml 的交互逻辑
    /// </summary>
    public partial class DateSelect : UserControl, ICommonEdit
    {
        #region [       Properties       ]

        // ---> 设定ComboBox样式
        /// <summary>
        /// 设定ComboBox样式
        /// </summary>
        [
        Description("设定DateSelect样式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public string DateSelectText
        {
            get { return DateRangSelect.value; }
            set { DateRangSelect.value = value; }
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
        private string _ContentDateSelect;
        /// <summary>
        /// 设置DateTimePicker值
        /// </summary>
        [
        Description("设定DateSelect内容。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("DateTimePickerExtend"),
        Filter()
        ]
        public string ContentDateSelect
        {
            get { return _ContentDateSelect; }
            set { _ContentDateSelect = value; }
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

        #endregion

        #region [       Constructor       ]
        public DateSelect()
        {
            InitializeComponent();
            DateSelectText = "";
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
                if (ControlWidth != 0)
                {
                    this.Width = ControlWidth;
                }
                else
                {
                    this.Width = 150;
                }
                if (ControlHeight != 0)
                {
                    this.Height = ControlHeight;
                }
                else
                {
                    this.Height = 30;
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置DatePicker出错:" + ex.ToString());
            }
        }

        #endregion

        #region ICommonEdit 成员

        public void Initialize()
        {
            try
            {
                SetStyle();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("设置样式失败:" + ex.ToString());
            }
           
        }


        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //(sender as Window).DragMove();
        }

        public object GetControlValue()
        {
            if (!string.IsNullOrEmpty(this.DateText.Text))
            {
                if (this.DateText.Text.Equals("全部"))
                {
                    return null;
                }

                if (this.DateText.Text.Equals("近一天"))
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
                if(this.DateText.Text.Equals("近一周"))
                {
                    return DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");   

                }
                if (this.DateText.Text.Equals("近一月"))
                {
                    return DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); 
                }
                if (this.DateText.Text.Equals("近一年"))
                {
                    return DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                }

            }
            else
            {
               return null;
            }

            return null;
        }

        public void SetControlValue(object value)
        {
            if (value != null)
            {
                if (DateText != null)
                {
                    this.DateText.Text = value.ToString();
                }
            }
            else
            {
                this.DateText.Text = "";
            }
        }

        #endregion

        private void selectDate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DateRangWindow dateRangWindow = new DateRangWindow();
            dateRangWindow.Top = e.GetPosition(null).Y;
            dateRangWindow.Left = e.GetPosition(null).X;
            dateRangWindow.pointX = e.GetPosition(null).X;
            dateRangWindow.pointY = e.GetPosition(null).Y;
            dateRangWindow.ResizeMode = ResizeMode.NoResize;
            dateRangWindow.Width = 300;
            dateRangWindow.Height = 80;
            dateRangWindow.MaxWidth = 300;
            dateRangWindow.MaxHeight = 80;
            dateRangWindow.Title = "日期范围选择";
            dateRangWindow.WindowStyle = WindowStyle.None;
            dateRangWindow.ShowDialog();
            this.DateText.Text = DateSelectText;

        }

        private void selectDate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectDate_MouseDown(sender, e);
        }
    }
}
