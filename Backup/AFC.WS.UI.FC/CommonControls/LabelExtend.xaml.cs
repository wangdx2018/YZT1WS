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
    /// LabelExtend.xaml 的交互逻辑
    /// </summary>
    public partial class LabelExtend :Label, ICommonEdit
    {
        #region [       Constructor       ]
        /// <summary>
        /// 
        /// </summary>
        public LabelExtend()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(LabelExtend_Loaded);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelExtend_Loaded(object sender, RoutedEventArgs e)
        {
           SetStyle();
        }

        #endregion

        #region [       Properties       ]
        /// <summary>
        /// 
        /// </summary>
        private string _controlStyle;

        // ---> 设定Label样式
        /// <summary>
        /// 设定Label样式
        /// </summary>
        [
        Description("设定控件样式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("LabelExtend"),
        Filter()
        ]
        public string LabelStyle
        {
            get { return _controlStyle; }
            set { _controlStyle = value; }
        }

        private int _controlWidth;
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        [
        Description("设定控件宽度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("LabelExtend"),
        Filter()
        ]
        public int ControlWidth
        {
            get { return _controlWidth; }
            set { _controlWidth = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private int _controlHeight;
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        [
        Description("设定控件高度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("LabelExtend"),
        Filter()
        ]
        public int ControlHeight
        {
            get { return _controlHeight; }
            set { _controlHeight = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [
        Description("设置LabelExtend是否为只读"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("LabelExtend"),
        Filter()
        ]
        public bool CanEnabled
        {
            get { return this.IsEnabled; }
            set { this.IsEnabled = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [
        Description("设置LabelExtend是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("LabelExtend"),
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
        /// 设置样式
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
                   // this.Width = 150;
                }
                if (ControlHeight != 0)
                {
                    this.Height = ControlHeight;
                }
                else
                {
                  //  this.Height = 23;
                }
                if (LabelStyle != null)
                {
                    Style style = this.FindResource(LabelStyle) as Style;

                    this.Style = style;
                }
                else
                {
                   // Style style = this.FindResource("LabelStyle") as Style;

                   // this.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置LabelStyle出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       ICommonEdit 成员       ]

        /// <summary>
        /// 获取控件内容
        /// </summary>
        /// <returns>object</returns>
        public object GetControlValue()
        {
            return Content;
        }

        /// <summary>
        /// 是指控件内容
        /// </summary>
        /// <param name="value">设置的值</param>
        public void SetControlValue(object value)
        {
            this.Content = value;
        }


        /// <summary>
        /// 初始化控件信息
        /// </summary>
        public void Initialize()
        {
            //try
            //{
            //    SetStyle();
            //}
            //catch (Exception ex)
            //{
            //    WriteLog.Log_Info("设置样式失败" + ex.ToString());
            //}
        }
        #endregion
    }
}
