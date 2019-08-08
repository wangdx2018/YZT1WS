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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
#endregion

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// PasswordExtend.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordExtend : UserControl, ICommonEdit
    {
        #region [       Declarations      ]
        /// <summary>
        /// 获取键盘某键的状态
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        /// <summary>
        /// 创建接口变量
        /// </summary>
        private IValueConverter createValueClassInstance = null;

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
        Category("PasswordExtend"),
        Filter()
        ]
        public string PasswordStyle
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
        Category("PasswordExtend"),
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
        Category("PasswordExtend"),
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
        Description("设置PasswordExtend是否为只读"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("PasswordExtend"),
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
        Description("设置PasswordExtend是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("PasswordExtend"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }

        /// <summary>
        /// 设置密码显示字符
        /// </summary>
        [
        Description("设置密码显示字符"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("PasswordExtend"),
        Filter()
        ]
        public char PassWordChar
        {
            get { return this.passwordBox.PasswordChar; }
            set { this.passwordBox.PasswordChar = value; }
        }
        /// <summary>
        /// 设置验证数据的类型
        /// </summary>
        private TextBoxDataType _textBoxContent;

        // ---> 设置验证数据的类型
        /// <summary>
        /// 设置验证数据的类型
        /// </summary>
        [
        Description("设定验证的数据类型"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("PasswordExtend"),
        Filter()
        ]
        public TextBoxDataType TextBoxValidate
        {
            get
            {
                return _textBoxContent;
            }
            set
            {
                _textBoxContent = value;
                SetTextBoxDataType(this, _textBoxContent);
            }
        }
        /// <summary>
        /// 设置输入数据的最大长度
        /// </summary>
        private int _regMaxLength = 8;

        // ---> 设置输入数据的最大长度
        /// <summary>
        /// 设置输入数据的最大长度
        /// </summary>
        [
        Description("设置最大长度"),
        Category("PasswordExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public int RegMaxLength
        {
            get
            {
                return _regMaxLength;
            }
            set
            {
                _regMaxLength = value;
            }
        }
        /// <summary>
        /// 设置输入数据的最小长度
        /// </summary>
        private int _regMinLength = 1;

        // ---> 设置输入数据的最小长度
        /// <summary>
        /// 设置输入数据的最小长度
        /// </summary>
        [
        Description("设置最小长度"),
        Category("PasswordExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public int RegMinLength
        {
            get
            {
                return _regMinLength;
            }
            set
            {
                _regMinLength = value;
            }
        }
        /// <summary>
        /// 设置输入数据的最大值
        /// </summary>
        private int _maxValue = 99999999;

        // ---> 设置输入数据的最大值,输入数字时使用。
        /// <summary>
        /// 设置输入数据的最大值
        /// </summary>
        [
        Description("设置最大值"),
        Category("PasswordExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }
        /// <summary>
        /// 设置输入数据的最小值
        /// </summary>
        private int _minValue = 0;

        // ---> 设置输入数据的最小值,输入数字时使用。
        /// <summary>
        /// 设置输入数据的最小值
        /// </summary>
        [
        Description("设置最小值"),
        Category("PasswordExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public int MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }
        /// <summary>
        /// 设置输入验证正则表达式
        /// </summary>
        private string _regularExpression;

        // ---> 设置输入验证正则表达式。
        /// <summary>
        /// 设置输入验证正则表达式
        /// </summary>
        [
        Description("设置自定义正则表达式"),
        Category("PasswordExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public string RegularExpression
        {
            get
            {
                return _regularExpression;
            }
            set
            {
                _regularExpression = value;
            }
        }
        // ---> 设置Text的内容。
        /// <summary>
        /// 设置Text的内容
        /// </summary>
        [
        Description("设置Text的内容。"),
        Category("TextBoxExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public string TextPassword
        {
            get { return this.passwordBox.Password; }
            set { this.passwordBox.Password = value; }
        }
        /// <summary>
        /// 获取错误信息
        /// </summary>
        private string _errorText;

        // ---> 获得错误信息，并显示。
        /// <summary>
        /// 获取错误信息
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;

                this.ValidizorImage.ToolTip = value;
            }
        }
        /// <summary>
        /// 设置Border的样式
        /// </summary>
        private string _borderStyle;

        // ---> 设置Border的样式。
        /// <summary>
        /// 设置Border的样式
        /// </summary>
        [
        Description("设置Border的样式。"),
        Category("TextBoxExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public string BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                _borderStyle = value;
            }
        }

        private bool _isNUll = false;
        /// <summary>
        /// 设定TextBox是否允许为空
        /// </summary>
        [
        Description("设定TextBox是否允许为空。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public bool IsNull
        {
            get { return _isNUll; }
            set { _isNUll = value; }
        }
        #endregion

        #region [       Set Validate Type      ]

        /// <summary>
        /// 设置文本框的类型。
        /// </summary>
        /// <remarks>
        /// 设置文本框的类型
        /// </remarks>
        /// <param name="ctrl">控件</param>
        /// <param name="value">设置值</param>
        public void SetTextBoxDataType(Control ctrl, TextBoxDataType value)
        {
            try
            {
                switch (value)
                {
                    case TextBoxDataType.None:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        break;
                    case TextBoxDataType.AllNumbers:
                        ctrl.KeyDown += new KeyEventHandler(AllNumbers_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(AllNumbers_LostFocus);
                        break;
                    case TextBoxDataType.RationalNumericValue:
                        ctrl.KeyDown += new KeyEventHandler(RationalNumeric_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        break;
                    case TextBoxDataType.LetterOrDigit:
                        ctrl.KeyDown += new KeyEventHandler(LetterOrDigit_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        break;
                    case TextBoxDataType.AllLetters:
                        ctrl.KeyDown += new KeyEventHandler(AllLetters_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(AllLetters_LostFocus);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        break;
                    case TextBoxDataType.AllCapitalLetters:
                        ctrl.KeyDown += new KeyEventHandler(AllCapitalLetters_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(AllCapitalLetters_LostFocus);
                        break;
                    case TextBoxDataType.AllLowercaseLetters:
                        ctrl.KeyDown += new KeyEventHandler(AllLowercaseLetters_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(AllLowercaseLetters_LostFocus);
                        break;
                    case TextBoxDataType.Email:
                        ctrl.KeyDown += new KeyEventHandler(Email_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(Email_LostFocus);
                        break;
                    case TextBoxDataType.IPAddress:
                        ctrl.KeyDown += new KeyEventHandler(IPAddress_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(IPAddress_LostFocus);
                        break;
                    case TextBoxDataType.Date:
                        ctrl.KeyDown += new KeyEventHandler(Date_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(Date_LostFocus);
                        break;
                    case TextBoxDataType.Time:
                        ctrl.KeyDown += new KeyEventHandler(Time_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(Time_LostFocus);
                        break;
                    case TextBoxDataType.IdentityCard:
                        ctrl.KeyDown += new KeyEventHandler(IdentityCard_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(IdentityCard_LostFocus);
                        break;
                    case TextBoxDataType.PastalCode:
                        ctrl.KeyDown += new KeyEventHandler(PastalCode_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(PastalCode_LostFocus);
                        break;
                    case TextBoxDataType.MacIP:
                        ctrl.KeyDown += new KeyEventHandler(MacIP_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(MacIP_LostFocus);
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 控件按下时发生
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ctrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.passwordBox.Password.Length < RegMaxLength)
            {
                HideErrorInfo();
                e.Handled = false;
            }
            else
            {
                if (this.passwordBox.Password.Length == RegMaxLength)
                {
                    if ((e.Key != Key.Back) && (e.Key != Key.Tab) && (e.Key != Key.System) && (e.Key != Key.Enter))
                    {
                        SetErrorInfo("密码过长，超过" + RegMaxLength + "位");
                        e.Handled = true;
                    }
                }
            }
            if (e.Key == Key.Enter)
                e.Handled = false;
        }
        #endregion

        #region [       LostFocus Event     ]

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="message">数据信息</param>
        private void FocusEvent(object sender, TextBoxDataType dataType, string message)
        {
            String expression = String.Empty;
            HideErrorInfo();
            if (IsNull)
            {
                if (this.passwordBox.Password == "" || this.passwordBox.Password == null)
                {
                    SetErrorInfo("不允许为空");
                    return;
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(this.passwordBox.Password))
                {
                    switch (dataType)
                    {
                        case TextBoxDataType.UserDefinedExpression:
                            if (RegularExpression != "")
                            {
                                expression = RegularExpression;
                            }
                            break;
                        case TextBoxDataType.Date:
                            try
                            {
                                string date = Convert.ToDateTime(this.passwordBox.Password.Trim()).ToShortDateString();
                                expression = @"^([1-9]\d{3})-(0?[1-9]|10|11|12)-([0-2]?\d|30|31)$";
                                HideErrorInfo();
                            }
                            catch
                            {
                                SetErrorInfo("日期输入不正确，请输入正确的日期!如(2008-08-08)。");
                                return;
                            }//End try;
                            break;

                        case TextBoxDataType.Time:
                            try
                            {
                                string time = Convert.ToDateTime(this.passwordBox.Password.Trim()).ToShortTimeString();
                                expression = @"^(20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
                                HideErrorInfo();
                            }
                            catch
                            {
                                SetErrorInfo("时间输入不正确，请输入正确的时间!如(13:20:03)");
                                return;
                            }//End try;
                            break;

                        case TextBoxDataType.Email:
                            expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                            break;

                        case TextBoxDataType.IPAddress:
                            expression = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
                            break;

                        case TextBoxDataType.AllNumbers:
                            expression = @"^\d{" + RegMinLength + "," + RegMaxLength + "}$";
                            HideErrorInfo();
                            if (this.passwordBox.Password.Length > 0)
                            {
                                try
                                {
                                    if (Convert.ToInt64(this.passwordBox.Password) > Convert.ToInt64(MaxValue))
                                    {
                                        message = "输入值不能大于最大值" + MaxValue;
                                        SetErrorInfo("输入值不能大于最大值" + MaxValue);
                                        if (this.passwordBox.Focusable == true)
                                            return;
                                    }//End if;
                                    if (Convert.ToInt64(this.passwordBox.Password) < Convert.ToInt64(MinValue))
                                    {
                                        message = "输入值不能小于最小值 " + MinValue;
                                        SetErrorInfo("输入值不能小于最小值 " + MinValue);
                                        this.Focus();
                                        return;
                                    }//End if;
                                    if (this.passwordBox.Password.Length <= RegMaxLength)
                                    {
                                        HideErrorInfo();
                                    }
                                    else
                                    {
                                        SetErrorInfo("密码过长，超过" + RegMaxLength + "位");
                                        return;
                                    }
                                }
                                catch (Exception ee)
                                {
                                    SetErrorInfo("只能输入数字。" + ee.Message);
                                    return;
                                }//End try;
                            }//End if;判断长度
                            break;

                        case TextBoxDataType.AllLetters:
                            expression = @"^[a-zA-Z]{" + RegMinLength + "," + RegMaxLength + "}$";
                            break;

                        case TextBoxDataType.AllCapitalLetters:
                            expression = @"^[A-Z]{" + RegMinLength + "," + RegMaxLength + "}$";
                            break;

                        case TextBoxDataType.AllLowercaseLetters:
                            expression = @"^[a-z]{" + RegMinLength + "," + RegMaxLength + "}$";
                            break;

                        case TextBoxDataType.IdentityCard:
                            expression = @"^((\d{15})|([0-9]{17}[Xx0-9]{1}))$";//^\d{17}[\d|X]|\d{15}$";        //@"^\d{15}|\d{18}|\d{17}[X]$";// @"^\d{17}X|\d{18}|\d{15}$";
                            break;

                        case TextBoxDataType.PastalCode:
                            expression = @"^\d{6}$";
                            break;

                        case TextBoxDataType.MacIP:
                            expression = @"(([0-9A-Fa-f][0-9A-Fa-f]-[0-9A-Fa-f][0-9A-Fa-f]-[0-9A-Fa-f][0-9A-Fa-f]-[0-9A-Fa-f][0-9A-Fa-f]-[0-9A-Fa-f][0-9A-Fa-f]-[0-9A-Fa-f][0-9A-Fa-f])|([0-9A-Fa-f][0-9A-Fa-f]:[0-9A-Fa-f][0-9A-Fa-f]:[0-9A-Fa-f][0-9A-Fa-f]:[0-9A-Fa-f][0-9A-Fa-f]:[0-9A-Fa-f][0-9A-Fa-f]:[0-9A-Fa-f][0-9A-Fa-f]))$";
                            break;
                    }//End switch;


                    if (Regex.IsMatch(this.passwordBox.Password, expression, RegexOptions.Compiled))
                    {
                        HideErrorInfo();
                    }
                    else
                    {
                        SetErrorInfo(message);
                    }//End if;验证判断是否成功。
                }
            }
            catch (Exception ee)
            {
                SetErrorInfo(ee.Message);
                return;
            }
        
        }

        /// <summary>
        /// 邮政编码验证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void PastalCode_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.PastalCode, "邮政编码是六位数字。");
        }

        /// <summary>
        /// 身份证号验证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void IdentityCard_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.IdentityCard, "身份证号15位或18位数字或17位数字及一位字母X。");
        }

        /// <summary>
        /// 时间验证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Time_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.Time, "时间输入格式不正确！如(13:20:03)。");
        }

        /// <summary>
        /// 日期验证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Date_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.Date, "日期输入格式不正确！如(2008-08-08)。");
        }

        /// <summary>
        /// IP地址验证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void IPAddress_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.IPAddress, "IP地址输入不正确！如(192.168.1.1)。");
        }

        /// <summary>
        /// Email地址验证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Email_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.Email, "Email地址输入不正确。如(abc@cssweb.com.cn)");
        }

        /// <summary>
        /// 全部都是小写字母
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllLowercaseLetters_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.AllLowercaseLetters, "字母全部小写。");
        }

        /// <summary>
        /// 全部都是大小字母
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllCapitalLetters_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.AllCapitalLetters, "字母全部大写。");
        }

        /// <summary>
        /// 全部都是字母
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllLetters_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.AllLetters, "只能输入字母。");
        }

        /// <summary>
        /// 全部都是数字
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllNumbers_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.AllNumbers, "只能输入数字。");
        }

        /// <summary>
        /// Mac地址失去焦点事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void MacIP_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.MacIP, "Mac地址输入不正确。如（00-18-8B-80-1D-A5或00:18:8B:80:1D:A5)");
        }

        #endregion -->失去焦点事件

        #region [       KeyPress Event      ]
        /// <summary>
        /// email地址。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Date_KeyPress(object sender, KeyEventArgs e)
        {
            HideErrorInfo();
        }

        /// <summary>
        /// email地址。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Time_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key == Key.Tab) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Oem1) || (e.Key == Key.Back))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入数字，不能输入字符！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// email地址。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Email_KeyPress(object sender, KeyEventArgs e)
        {
            HideErrorInfo();
        }

        /// <summary>
        /// Mac地址按键点事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void MacIP_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.A && e.Key <= Key.F) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key == Key.Tab) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.OemMinus))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 身份证
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void IdentityCard_KeyPress(object sender, KeyEventArgs e)
        {

            if (e == null)
            {
                return;
            }

            if ((e.Key == Key.X) || (e.Key == Key.P) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key == Key.Back) || (e.Key == Key.OemPeriod) || (e.Key == Key.Tab))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入正确的身份证号码！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void PastalCode_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key == Key.Back) || (e.Key == Key.OemPeriod) || (e.Key == Key.Tab))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入数字。");
                e.Handled = true;
            }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void IPAddress_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Back) || (e.Key == Key.OemPeriod) || (e.Key == Key.Tab))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入数字。");
                e.Handled = true;
            }

        }

        /// <summary>
        /// 只能输入小写字母
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllLowercaseLetters_KeyPress(object sender, KeyEventArgs e)
        {

            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Back) || (e.Key == Key.Tab))
            {
                //判断CapsLock的状态
                int iKeyState = GetKeyState(0x14);

                if (iKeyState == -128 || iKeyState == 0 || e.Key == Key.CapsLock)
                {
                    e.Handled = false;
                    HideErrorInfo();
                }
                else
                {
                    SetErrorInfo("请输入字母，字母全部小写。");
                    e.Handled = true;
                }
            }
            else
            {
                SetErrorInfo("请输入字母，字母全部小写。");
                e.Handled = true;
            }


        }

        /// <summary>
        /// 全部大字母
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllCapitalLetters_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key == Key.Tab) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Back))
            {
                //判断CapsLock的状态
                int iKeyState = GetKeyState(0x14);

                if (iKeyState == -127 || iKeyState == 1 || e.Key == Key.CapsLock)
                {
                    e.Handled = false;
                    HideErrorInfo();
                }
                else
                {
                    SetErrorInfo("请输入字母，字母全部大写。");
                    e.Handled = true;
                }
            }
            else
            {
                SetErrorInfo("请输入字母，字母全部大写。");
                e.Handled = true;
            }

        }

        /// <summary>
        /// 只能输入A~Z及a~z之间的字母
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AllLetters_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key == Key.Tab) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift)||(e.Key==Key.Enter))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入A~Z及a~z之间的字母！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 只能输入0~9之间的数字。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void AllNumbers_KeyPress(object sender, KeyEventArgs e)
        {

            if (e == null)
            {
                return;
            }

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Tab) || e.Key == Key.Enter)
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入数字，不能输入字符！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 所有的有理数
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void RationalNumeric_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (this.passwordBox.Password.Length == 0 && e.Key == Key.OemMinus) || (this.passwordBox.Password.Length >= 0 && e.Key == Key.OemPeriod) || (e.Key == Key.Tab) || (e.Key == Key.Back))
            {
                if (e.Key == Key.OemPeriod)
                {
                    if (!(this.passwordBox.Password.IndexOf(".") >= 0) || this.passwordBox.Password.IndexOf(".") <= -1)
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                SetErrorInfo("请输入有理数！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 字母或数字
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void LetterOrDigit_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key == Key.Tab) || (e.Key == Key.Back))
                e.Handled = false;
            else
            {
                SetErrorInfo("请输入字母或数字！");
                e.Handled = true;
            }
        }

        #endregion

        #region [       Validate Methods      ]

        /// <summary>
        /// 设置并显示错误信息
        /// </summary>
        /// <param name="errorInfo">异常信息</param>
        private void SetErrorInfo(string errorInfo)
        {
            this.ErrorText = errorInfo;
            ValidizorImage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 无错误信息时隐藏错误提示
        /// </summary>
        private void HideErrorInfo()
        {
            ValidizorImage.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region [       Constructor      ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public PasswordExtend()
        {
            InitializeComponent();
            ValidizorImage.Visibility = Visibility.Collapsed;
            InputMethod.SetIsInputMethodEnabled(this.passwordBox, false);
            this.passwordBox.AddHandler(PasswordExtend.KeyDownEvent, new RoutedEventHandler(HandleHandledKeyDown), true); 

        }

        /// <summary>
        /// 判断是否为空格键
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void HandleHandledKeyDown(object sender, RoutedEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Key.Space)
            {
                ke.Handled = false;
                this.passwordBox.Password = string.Empty;
                SetErrorInfo("不允许输入空格!");
            }
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
                this.PassWordChar = '*';

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
                if (PasswordStyle != null)
                {
                    Style style = this.FindResource(PasswordStyle) as Style;

                    this.passwordBox.Style = style;
                }
                else
                {
                    Style style = this.FindResource("PasswordStyle") as Style;

                    this.passwordBox.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置PassWord Style出错:" + ex.ToString());
            }

            try
            {
                if (BorderStyle != null)
                {
                    Style style = this.FindResource("BorderStyle") as Style;

                    this.border.Style = style;
                }
                else
                {
                    Style style = this.FindResource("BorderStyle") as Style;

                    this.border.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("Border设置样式出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       Stop Text Paste      ]

        /// <summary>
        /// 粘贴时发生此事件，当粘贴长度超过最大长度时，提示。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var commandName = ((System.Windows.Input.RoutedUICommand)(e.Command)).Text;
            if (commandName == "粘贴")
            {

                IDataObject iData = Clipboard.GetDataObject();//取剪贴板对象
                if (iData.GetDataPresent(DataFormats.UnicodeText, true)) //判断是否是Text
                {
                    string text = (string)iData.GetData(DataFormats.UnicodeText);//取数据
                    if (CheckByteLengthFlow(text))
                    {
                        if (_textBoxContent == TextBoxDataType.AllNumbers)
                        {
                            int num = -1;
                            try
                            {
                                num = Convert.ToInt32(text);
                            }
                            catch
                            { }
                            if (num != -1)
                            {
                                HideErrorInfo();
                                e.Handled = false;
                                return;
                            }
                            else
                            {
                                SetErrorInfo("不允许粘贴不是数字的字符");
                                e.Handled = true;
                                return;
                            }
                        }
                        HideErrorInfo();
                        e.Handled = false;
                    }
                    else
                    {
                        SetErrorInfo("粘贴的所有字符长度大于设置的最大长度");
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// 判断即将输入的文本长度是否溢出
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>是否溢出</returns>
        private bool CheckByteLengthFlow(string text)
        {
            int len = GetByteLength(text);    //输入的字符的长度
            int tlen = GetByteLength(TextPassword);  //文本框原有文本的长度
            //int slen = GetByteLength(this.textBox.SelectedText);    //文本框选中文本的长度
            return ((tlen + len) <= RegMaxLength) ? true : false;
        }

        /// <summary>
        /// 计算文本字节长度，区分多字节字符
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>文本字节长度</returns>
        private int GetByteLength(string text)
        {
            return System.Text.Encoding.Default.GetBytes(text).Length;
        }
        #endregion

        #region [       ICommonEdit 成员      ]

        /// <summary>
        /// 初始化控件
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
        /// <summary>
        /// 返回密码
        /// </summary>
        /// <returns>object</returns>
        public object GetControlValue()
        {
            return this.passwordBox.Password;
        }

        /// <summary>
        /// 赋初始值
        /// </summary>
        /// <param name="value">初值</param>
        public void SetControlValue(object value)
        {
            if(value!=null)
            this.passwordBox.Password = value.ToString();
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetStyle();
            }
            catch
            {
            }
        }
    }
}
