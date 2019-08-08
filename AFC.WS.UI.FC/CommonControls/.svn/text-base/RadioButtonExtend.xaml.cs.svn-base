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
    /// RiadioButtonExtend.xaml 的交互逻辑 
    /// </summary>
    public partial class RiadioButtonExtend : RadioButton,ICommonEdit
    {
        #region [       Constructor       ]

        /// <summary>
        /// 创建接口变量
        /// </summary>
        private IValueConverter createValueClassInstance = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RiadioButtonExtend()
        {
            InitializeComponent();
           // this.Loaded += new RoutedEventHandler(RiadioButtonExtend_Loaded);
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RiadioButtonExtend_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetStyle();
            }
            catch
            { }
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
        Category("RiadioButtonExtend"),
        Filter()
        ]
        public string RadioButtonStyle
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
        Category("RiadioButtonExtend"),
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
        Category("RiadioButtonExtend"),
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
        Description("设置RiadioButton是否为只读"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("RiadioButtonExtend"),
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
        Description("设置RiadioButton是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("RiadioButtonExtend"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }

        /// <summary>
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        private string _userControlName;
        /// <summary>
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        [
        Description("获得实现接口用户控件命名空间和类名称，只需在引起其他控件的控件中填写。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string UserControlClassName
        {
            get { return _userControlName; }
            set { _userControlName = value; }
        }
        #endregion

        #region [       Set Converter Data      ]
        /// <summary>
        /// 创建用户控件对象
        /// </summary>
        /// <param name="className">用户控件类名称</param>
        /// <returns>True:成功，False：失败</returns>
        private bool CreateConverterInstance(string className)
        {
            try
            {
                Type type = Type.GetType(className);

                if (type != null)
                {
                    createValueClassInstance = Activator.CreateInstance(type) as IValueConverter;

                    if (createValueClassInstance != null)
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
        /// 通过调用接口转换数据
        /// </summary>
        /// <returns></returns>
        private object ConverterData()
        {
            try
            {
                if (!String.IsNullOrEmpty(UserControlClassName))
                {
                    bool isTrue = CreateConverterInstance(UserControlClassName);

                    if (isTrue)
                    {
                        return createValueClassInstance.Convert(this.Content, null, null, null);
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
                return null;
            }

        }

        #endregion

        #region [       Set Style      ]

        /// <summary>
        /// 设置控件样式
        /// </summary>
        private void SetStyle()
        {
            try
            {
             //   this.VerticalContentAlignment = VerticalAlignment.Center;
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
                    this.Height = 23;
                }
                if (RadioButtonStyle != null)
                {
                    Style style = this.FindResource(RadioButtonStyle) as Style;

                    this.Style = style;
                }
                else
                {

                    //Style style = this.FindResource("RadioButtonStyle") as Style;

                    //this.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置RadioButtonStyle出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       ICommonEdit 成员       ]

        /// <summary>
        /// 获得控件值
        /// </summary>
        /// <returns></returns>
        public object GetControlValue()
        {
            object res = ConverterData();

            if (createValueClassInstance == null)
                return this.Content;
            else
            {
                if (res != null)
                    return res;
                else
                    return this.Content;
            }
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="value">要设置的值</param>
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
