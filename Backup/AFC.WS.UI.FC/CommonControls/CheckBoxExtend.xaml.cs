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
#endregion

namespace AFC.WS.UI.CommonControls 
{
    /// <summary>
    /// 自定义CheckBox，在原来的基础上，赋予新的功能。
    /// </summary>
    public partial class CheckBoxExtend : CheckBox, ICommonEdit
    {
        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBoxExtend()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(CheckBoxExtend_Loaded);
        }
        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void CheckBoxExtend_Loaded(object sender, RoutedEventArgs e)
        {
            SetStyle();
        }

        #endregion

        #region [       Properties       ]
        /// <summary>
        /// 设定ComboBox样式
        /// </summary>
        private string _controlStyle;

        // ---> 设定ComboBox样式
        /// <summary>
        /// 设定ComboBox样式
        /// </summary>
        [
        Description("设定控件样式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxExtend"),
        Filter()
        ]
        public string CheckBoxStyle
        {
            get { return _controlStyle; }
            set { _controlStyle = value; }
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
        Category("Common Properties"),
        Filter()
        ]
        public int ControlWidth
        {
            get { return _controlWidth; }
            set { _controlWidth = value; }
        }
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        private int _controlHeight;
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        [
        Description("设定控件高度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxExtend"),
        Filter()
        ]
        public int ControlHeight
        {
            get { return _controlHeight; }
            set { _controlHeight = value; }
        }
        /// <summary>
        /// 设置CheckBox是否为只读
        /// </summary>
        [
          Description("设置CheckBox是否为只读"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
          Category("CheckBoxExtend"),
          Filter()
        ]
        public bool CanEnabled
        {
            get { return this.IsEnabled; }
            set { this.IsEnabled = value; }
        }
        /// <summary>
        /// 设置CheckBox是否可见
        /// </summary>
        [
        Description("设置CheckBox是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxExtend"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }
        #endregion

        #region [       Set Style      ]

        /// <summary>
        /// 设置CheckBox样式，包括高度和宽度。
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
                    //this.Width = 150;
                }
                if (ControlHeight != 0)
                {
                    this.Height = ControlHeight;
                }
                else
                {
                   // this.Height = 23;
                }
                if (CheckBoxStyle != null)
                {
                    Style style = this.FindResource(CheckBoxStyle) as Style;

                    this.Style = style;
                }
                else
                {
                   // Style style = this.FindResource("CheckBoxStyle") as Style;

                   // this.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置CheckBoxStyle出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       ICommonEdit 成员     ]

        /// <summary>
        /// 获得控件值
        /// </summary>
        /// <returns></returns>
        public object GetControlValue()
        {
            return this.Content;
        }
        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="value">value</param>
        public void SetControlValue(object value)
        {
            this.Content = value;
        }

         /// <summary>
        /// 初始化控件信息
        /// </summary>
        public void Initialize()
        {
            try
            {
                SetStyle();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("设置样式失败" + ex.ToString());
            }
        }
        #endregion
    }
}
