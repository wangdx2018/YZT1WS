#region [       Copyright (C), 2009,  中软AFC, Token Shen.     ]
/************************************************************
  FileName: TextBoxExtend.xaml
  
  Author: 沈克涛    
 
  Version :  1.0   
 
  Date:20090715
 
  Description: 文本框的扩展实现   
 
  Function List:  
 
    1. SetTextBoxDataType(Control ctrl, TextBoxDataType value)  // ---> 设置验证的类型
 
    2. FocusEvent(object sender, TextBoxDataType dataType, string message) // ---> 失去焦点时调用的事件
 
  History: 
 
      <author>   <time>      <version >     <desc>
 
      沈克涛    2009/07/15     1.0         增加代码说明
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
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;
using AFC.WS.UI.Common;
using System.Runtime.InteropServices;
#endregion

namespace AFC.WS.UI.CommonControls
{
    #region [       TextBoxValidateType      ]
    /// <summary>
    /// 文本框内容类型
    /// </summary>
    public enum TextBoxDataType
    {
        /// <summary>
        /// 无限制（即混合型）
        /// </summary>
        None,
        /// <summary>
        /// 全数字
        /// </summary>
        AllNumbers,
        /// <summary>
        /// 十六进制
        /// </summary>
        Hex,
        /// <summary>
        /// 金额验证
        /// </summary>
        Amount,

        /// <summary>
        /// 正金额验证
        /// </summary>
        PlusAmount,
        /// <summary>
        /// 所有有理数
        /// </summary>
        RationalNumericValue,
        /// <summary>
        /// 字母或数字
        /// </summary>
        LetterOrDigit,
        /// <summary>
        /// 汉字或字母或数字
        /// </summary>
        ChineseOrLetterOrDigit,
        /// <summary>
        /// 全字母
        /// </summary>
        AllLetters,
        /// <summary>
        /// 全大写字母
        /// </summary>
        AllCapitalLetters,
        /// <summary>
        /// 全小写字母
        /// </summary>
        AllLowercaseLetters,
        /// <summary>
        /// 邮箱
        /// </summary>
        Email,
        /// <summary>
        /// IP地址
        /// </summary>
        IPAddress,
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 时间
        /// </summary>
        Time,
        /// <summary>
        /// 身份证
        /// </summary>
        IdentityCard,   //@"\d{17}[\d|X]|\d{15}";
        /// <summary>
        /// 邮政编码
        /// </summary>
        PastalCode,     //expression = @"\d{6}";
        /// <summary>
        /// MAC地址 //Mac地址正则：[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]
        /// </summary>
        MacIP,
        /// <summary>
        /// 自定义正则表达式,需要在RegularExpression属性添加正则表达式
        /// </summary>
        UserDefinedExpression
    }
    #endregion

    /// <summary>
    /// TextBoxExtend.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxExtend : UserControl, ICommonEdit
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

        #region [       Constructor       ]
        /// <summary>
        /// 够着函数
        /// </summary>
        public TextBoxExtend()
        {
            InitializeComponent();

            ValidizorImage.Visibility = Visibility.Collapsed;

            this.textBox.Loaded += new RoutedEventHandler(textBox_Loaded);
            this.textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            InputMethod.SetIsInputMethodEnabled(this.textBox, true);
  
            this.textBox.AddHandler(TextBoxExtend.KeyDownEvent, new RoutedEventHandler(HandleHandledKeyDown), true); 
        }
        

        /// <summary>
        /// 文本框改变事件。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int count = 0;
                if (TextBoxValidate == TextBoxDataType.ChineseOrLetterOrDigit)
                {
                    if (sender is TextBox)
                    {
                        TextBox tbe = sender as TextBox;

                        int length = GetByteLength(tbe.Text);
                        if (length > this.RegMaxLength)
                        {
                            if (count < 1)
                            {
                                //this.textBox.Text = stringFormat(tbe.Text, this.RegMaxLength);
                                SetErrorInfo("输入字符或中文过长！长度最大为：" + this.RegMaxLength);
                                count++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 格式化字符串长度，超出部分显示省略号,区分汉字跟字母。汉字2个字节，字母数字一个字节
        /// </summary>
        /// <param name="str">要格式化处理的字符串</param>
        /// <param name="n">返回新的字符串长度</param>
        /// <returns>新的字符串</returns>
        public string stringFormat(string str, int n)
        {
            string temp = string.Empty;
            if (System.Text.Encoding.Default.GetByteCount(str) <= n)  //如果长度比需要的长度n小,返回原字符串
            {
                return str;
            }
            else
            {
                int t = 0;
                char[] q = str.ToCharArray();
                for (int i = 0; i < q.Length && t < n; i++)
                {
                    if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9FA5)  //是否汉字
                    {
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                return (temp);
            }
        }

        /// <summary>
        /// 获取指定长度的字符串
        /// </summary>
        /// <param name="text">非指定长度的字符串,包括汉字、数字、字母等</param>
        /// <returns>指定长度的字符串</returns>
        private string GetLengthString(string text)
        {
            string lenString = null;
            try
            {
                char[] charArray = text.ToCharArray();
                int count = 0;

                for (int i = 0; i < charArray.Length; i++)
                {
                    int len = GetByteLength(charArray[i].ToString());
                    count = count + len;
                    lenString = lenString + charArray[i].ToString();
                    if (count == this.RegMaxLength)
                    {
                        return lenString;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            return lenString;
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
                if (TextBoxValidate != TextBoxDataType.ChineseOrLetterOrDigit)
                {
                    ke.Handled = false;
                    this.textBox.Text = string.Empty;
                    SetErrorInfo("不允许输入空格!");
                }
            }
            //if (ke.Key == Key.ImeModeChange)
            //{
            //    MessageBox.Show("sdfk");
            //}
        } 
        #endregion

        #region [       Set Style       ]
        /// <summary>
        /// 窗体加载事件。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void textBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (TextBoxValidate == TextBoxDataType.ChineseOrLetterOrDigit)
            {
                InputMethod.SetIsInputMethodEnabled(this.textBox, true);
                InputMethod.SetPreferredImeConversionMode(this.textBox, ImeConversionModeValues.Native);
            }
            try
            {
                SetStyle();
            }
            catch { }
        }
        /// <summary>
        /// 设置宽度和高度，设置TextBox样式。
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
                    //this.Height = 23;
                }
                if (TextBoxStyle != null)
                {
                    Style style = this.FindResource(TextBoxStyle) as Style;

                    this.textBox.Style = style;
                }
                else
                {
                    Style style = this.FindResource("TextStyle") as Style;

                    this.textBox.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("TextBox设置样式出错:" + ex.ToString());
            }
            try
            {
                if (BorderStyle != null)
                {
                    Style style = this.FindResource(BorderStyle) as Style;

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

        #region [       Properties      ]

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
        Category("TextBoxExtend"),
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
        /// 设置控件宽度
        /// </summary>
        private int _controlWidth;
        /// <summary>
        /// 设置控件宽度
        /// </summary>
        [
        Description("设定控件宽度。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
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
        Category("TextBoxExtend"),
        Filter()
        ]
        public int ControlHeight
        {
            get { return _controlHeight; }
            set { _controlHeight = value; }
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
        Category("TextBoxExtend"),
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
        Category("TextBoxExtend"),
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
        private Int64 _maxValue = 9999999999;

        // ---> 设置输入数据的最大值,输入数字时使用。
        /// <summary>
        /// 设置输入数据的最大值
        /// </summary>
        [
        Description("设置最大值"),
        Category("TextBoxExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public Int64 MaxValue
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
        Category("TextBoxExtend"),
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
        Category("TextBoxExtend"),
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
        /// <summary>
        /// 设置文本框的样式
        /// </summary>
        private string _textStyle;

        // ---> 设置文本框的样式。
        /// <summary>
        /// 设置文本框的样式
        /// </summary>
        [
        Description("设置文本框的样式。"),
        Category("TextBoxExtend"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()
        ]
        public string TextBoxStyle
        {
            get
            {
                return _textStyle;
            }
            set
            {
                _textStyle = value;
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
        public string Text
        {
            get { return this.textBox.Text.Trim(); }
            set { this.textBox.Text = value; }
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
        /// 是否为只读
        /// </summary>
        [
        Description("设置TextBox是否为只读"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public bool CanReadOnly
        {
            get { return this.textBox.IsReadOnly; }
            set { this.textBox.IsReadOnly = value; }
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        [
        Description("设置TextBox是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }
        /// <summary>
        /// 
        /// 此属性设置实现接口的用户控件名称。
        /// 
        /// </summary>
        private string _userControlName;
        /// <summary>
        /// 
        /// 此属性设置实现接口的用户控件名称。
        /// 
        /// </summary>
        [
        Description("获得实现接口用户控件命名空间和类名称，只需在引起其他控件的控件中填写。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public string UserControlClassName
        {
            get { return _userControlName; }
            set { _userControlName = value; }
        }

        /// <summary>
        /// 
        /// 将 TextWrapping 属性设置为 Wrap 会导致输入的文本在到达 TextBox 控件的边缘时换至新行，
        /// 
        /// 必要时会自动扩展 TextBox 控件以便为新行留出空间。
        /// 
        /// </summary>
        [
        Description("将 TextWrapping 属性设置为 Wrap 会导致输入的文本在到达 TextBox 控件的边缘时换至新行，。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public TextWrapping TextWrap
        {
            get { return this.textBox.TextWrapping; }
            set { this.textBox.TextWrapping = value; }
        }
        /// <summary>
        /// 
        /// 将 AcceptsReturn 属性设置为 true 会导致在按 Return 键时插入新行，
        /// 
        /// 必要时会再次自动扩展 TextBox 以便为新行留出空间。
        /// 
        /// </summary>
        [
        Description("将 AcceptsReturn 属性设置为 true 会导致在按 Return 键时插入新行，必要时会再次自动扩展 TextBox 以便为新行留出空间。。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public bool AcceptReturn
        {
            get { return this.textBox.AcceptsReturn; }
            set { this.textBox.AcceptsReturn = value; }
        }
        /// <summary>
        /// 
        /// VerticalScrollBarVisibility 属性向 TextBox 添加一个滚动条，以便在 TextBox 超出包含它的框架或窗口的大小时，
        /// 
        /// 可以滚动 TextBox 的内容。
        /// 
        /// </summary>
        [
        Description("TextBox 添加一个滚动条，以便在 TextBox 超出包含它的框架或窗口的大小时,可以滚动 TextBox 的内容。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return this.textBox.VerticalScrollBarVisibility; }
            set { this.textBox.VerticalScrollBarVisibility = value; }
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


        private string _UserDefinedInfo;
        /// <summary>
        /// 自定义验证提示信息
        /// </summary>
        [
        Description("自定义验证提示信息。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public string UserDefinedInfo
        {
            get { return _UserDefinedInfo; }
            set { _UserDefinedInfo = value; }
        }

        /// <summary>
        ///设置是否显示灰色
        /// </summary>
        [
        Description("设置是否显示灰色。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
        ]
        public  bool CanEnabled
        {
            get { return textBox.IsEnabled; }
            set { textBox.IsEnabled = value; }
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
                        return createValueClassInstance.Convert(this.Text, null, null, null);
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
                    case TextBoxDataType.Hex:
                        ctrl.KeyDown += new KeyEventHandler(Hex_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        //ctrl.LostFocus += new RoutedEventHandler(AllNumbers_LostFocus);
                        break;
                    case TextBoxDataType.Amount:
                        ctrl.KeyDown += new KeyEventHandler(Amount_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(Amout_LostFocus);
                        break;
                    case TextBoxDataType.PlusAmount:
                        ctrl.KeyDown += new KeyEventHandler(PlusAmount_KeyPress);
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.LostFocus += new RoutedEventHandler(Amout_LostFocus);
                        break;
                    case TextBoxDataType.RationalNumericValue:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(RationalNumeric_KeyPress);
                        break;
                    case TextBoxDataType.LetterOrDigit:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(LetterOrDigit_KeyPress);
                        break;
                    case TextBoxDataType.ChineseOrLetterOrDigit:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(ChineseOrLetterOrDigit_KeyPress);
                        break;
                    case TextBoxDataType.AllLetters:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(AllLetters_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(AllLetters_LostFocus);
                        break;
                    case TextBoxDataType.AllCapitalLetters:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(AllCapitalLetters_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(AllCapitalLetters_LostFocus);
                        break;
                    case TextBoxDataType.AllLowercaseLetters:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(AllLowercaseLetters_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(AllLowercaseLetters_LostFocus);
                        break;
                    case TextBoxDataType.Email:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(Email_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(Email_LostFocus);
                        break;
                    case TextBoxDataType.IPAddress:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(IPAddress_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(IPAddress_LostFocus);
                        break;
                    case TextBoxDataType.Date:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(Date_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(Date_LostFocus);
                        break;
                    case TextBoxDataType.Time:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(Time_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(Time_LostFocus);
                        break;
                    case TextBoxDataType.IdentityCard:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(IdentityCard_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(IdentityCard_LostFocus);
                        break;
                    case TextBoxDataType.PastalCode:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(PastalCode_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(PastalCode_LostFocus);
                        break;
                    case TextBoxDataType.MacIP:
                        ctrl.KeyDown += new KeyEventHandler(ctrl_KeyDown);
                        ctrl.KeyDown += new KeyEventHandler(MacIP_KeyPress);
                        ctrl.LostFocus += new RoutedEventHandler(MacIP_LostFocus);
                        break;
                    case TextBoxDataType.UserDefinedExpression:
                        ctrl.LostFocus += new RoutedEventHandler(UserDefinedExpression_LostFocus);
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
            if (this.textBox.Text.Length < RegMaxLength)
            {
                HideErrorInfo();
                e.Handled = false;
            }

            if (this.textBox.Text.Length >= RegMaxLength)
            {
                if (this.textBox.SelectedText.Length > 0)
                    e.Handled = false;
                else
                {
                    if ((e.Key != Key.Back) && (e.Key != Key.Tab) && (e.Key != Key.System) && (e.Key != Key.Enter))
                    {
                        SetErrorInfo("输入内容过长，超过" + RegMaxLength + "位");
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
                if (this.textBox.Text == "" || this.textBox.Text == null)
                {
                    SetErrorInfo("不允许为空");
                    return;
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(this.textBox.Text))
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
                                string date = Convert.ToDateTime(this.textBox.Text.Trim()).ToShortDateString();
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
                                string time = Convert.ToDateTime(textBox.Text.Trim()).ToShortTimeString();
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
                            if (textBox.Text.Length > 0)
                            {
                                try
                                {
                                    if (Convert.ToInt64(textBox.Text) > Convert.ToInt64(MaxValue))
                                    {
                                        message = "输入值不能大于最大值" + MaxValue;
                                        SetErrorInfo("输入值不能大于最大值" + MaxValue);
                                        if (this.textBox.Focusable == true)
                                            return;
                                    }//End if;
                                    if (Convert.ToInt64(textBox.Text) < Convert.ToInt64(MinValue))
                                    {
                                        message = "输入值不能小于最小值 " + MinValue;
                                        SetErrorInfo("输入值不能小于最小值 " + MinValue);
                                        this.Focus();
                                        return;
                                    }//End if;
                                }
                                catch (Exception ee)
                                {
                                    SetErrorInfo("只能输入数字。" + ee.Message);
                                    return;
                                }//End try;
                            }//End if;判断长度
                            break;

                        //dusj add 2012.08.03 begin
                        case TextBoxDataType.Amount:
                            expression = @"^\d{1,7}(?:\.\d{0,2}$|$)"; 
                            break;
                        case TextBoxDataType.PlusAmount:
                            expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
                            break;
                        //dusj add 2012.08.03 end
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

                    if (Regex.IsMatch(textBox.Text, expression, RegexOptions.Compiled))
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



        void Amout_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.Amount, "只能输入金额。金额范围0.00-9999999.99");
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

        /// <summary>
        /// 自定义验证信息
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void UserDefinedExpression_LostFocus(object sender, EventArgs e)
        {
            FocusEvent(sender, TextBoxDataType.UserDefinedExpression, UserDefinedInfo);
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
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Tab) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Oem1) || (e.Key == Key.Back) || (e.Key == Key.Enter))
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
            if ((e.Key >= Key.A && e.Key <= Key.F) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Tab) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.OemMinus) || (e.Key == Key.Enter))
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

            if ((e.Key == Key.X) || (e.Key == Key.P) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Back) || (e.Key == Key.OemPeriod) || (e.Key == Key.Tab) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Enter))
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
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Back) || (e.Key == Key.OemPeriod) || (e.Key == Key.Tab) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Enter))
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

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Back) || (e.Key == Key.OemPeriod) || (e.Key == Key.Tab) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Enter))
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
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Back) || (e.Key == Key.Tab) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Enter))
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
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key == Key.Tab) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Back) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Enter))
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
            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key == Key.Tab) || (e.Key == Key.CapsLock) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.LeftShift) || (e.Key == Key.RightShift) || (e.Key == Key.Back) || (e.Key == Key.Enter))
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

            if ((e.Key >= Key.D0 && e.Key <= Key.D9)||(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Back) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.System) || e.Key == Key.Enter)
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
        /// 只能输入0~9之间的数字和a~f或A~F之间的字母。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Hex_KeyPress(object sender, KeyEventArgs e)
        {

            if (e == null)
            {
                return;
            }

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key >= Key.A && e.Key <= Key.F) || (e.Key == Key.Back) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Enter))
            {
                e.Handled = false;
                HideErrorInfo();
            }
            else
            {
                SetErrorInfo("请输入0~9之间的数字和a~f或A~F之间的字母！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 验证金额
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void Amount_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            string expression = @"\.";
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)||(e.Key==Key.OemMinus)||(e.Key == Key.NumLock) || (this.textBox.Text.Length > 0 && !Regex.IsMatch(this.textBox.Text, expression, RegexOptions.Compiled) && e.Key == Key.OemPeriod) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Back) || (e.Key == Key.Enter))
            {
                if (this.textBox.Text.IndexOf(".") > -1)
                {
                    string[] strSplot = this.textBox.Text.Split('.');
                    //已经有小数点2位了则取消输入
                    if (strSplot[1].Length > 1)
                    {
                        //取消输入
                        e.Handled = true;
                    }
                }

                if (this.textBox.Text.IndexOf("-") > -1&&this.textBox.Text.Length>5)
                {
                        //取消输入
                        e.Handled = true;
                }

                if (e.Key == Key.OemMinus && this.textBox.Text.Length != 0)
                {
                    //取消输入
                    e.Handled = true;
                }


                if (this.textBox.Text.IndexOf("-") < 0 && this.textBox.Text.IndexOf(".") < 0 && this.textBox.Text.Length > 6)
                {
                    //取消输入
                    e.Handled = true;
                }

                if (e.Key == Key.OemPeriod)
                {
                    if ((this.textBox.Text.Length!=(RegMaxLength-1)))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        SetErrorInfo("输入金额有误，此金额最大长度末尾位不能为“.”！");
                        e.Handled = true;
                    }
                }
            }
            else
            {
                SetErrorInfo("请输入正确的金额格式,如：2.88！");
                e.Handled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlusAmount_KeyPress(object sender, KeyEventArgs e)
        {
            {
                if (e == null)
                {
                    return;
                }
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (this.textBox.Text.Length > 0 && e.Key == Key.OemPeriod) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Back) || (e.Key == Key.Enter))
                {
                    if (e.Key == Key.OemPeriod)
                    {
                        if ((this.textBox.Text.Length != (RegMaxLength - 1)))
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            SetErrorInfo("输入金额有误，此金额最大长度末尾位不能为“.”！");
                            e.Handled = true;
                        }
                    }
                }
                else
                {
                    SetErrorInfo("请输入正确的金额格式,如：2.88！");
                    e.Handled = true;
                }
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
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (this.textBox.Text.Length == 0 && e.Key == Key.OemMinus) || (this.textBox.Text.Length >= 0 && e.Key == Key.OemPeriod) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Back) || (e.Key == Key.Enter))
            {
                if (e.Key == Key.OemPeriod)
                {
                    if (!(this.textBox.Text.IndexOf(".") >= 0) || this.textBox.Text.IndexOf(".") <= -1)
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
            if (e.Key == Key.Space || e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                e.Handled = true;

            if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Back) || (e.Key == Key.Enter))
                e.Handled = false;
            else
            {
                SetErrorInfo("请输入字母或数字！");
                e.Handled = true;
            }
        }

        /// <summary>
        /// 汉字或字母或数字
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void ChineseOrLetterOrDigit_KeyPress(object sender, KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            try
            {

                //if (TextBoxValidate == TextBoxDataType.ChineseOrLetterOrDigit)
                //{
                //    int length = GetByteLength(this.textBox.Text);
                //    if (length > this.RegMaxLength)
                //    {
                //       // this.textBox.Text = stringFormat(this.textBox.Text, this.RegMaxLength);
                //        SetErrorInfo("输入字符或中文过长！");
                //        e.Handled = true;
                //    }
                //    else
                //    {
                if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.NumLock) || (e.Key == Key.Tab) || (e.Key == Key.Back) || (e.Key == Key.Space) || (e.Key == Key.System) || (e.Key == Key.RightCtrl) || (e.Key == Key.LeftCtrl) || (e.Key == Key.RightShift) || (e.Key == Key.LeftShift) || (e.Key == Key.ImeProcessed) || (e.Key == Key.LeftAlt) || (e.Key == Key.RightAlt) || (e.Key == Key.Tab) || (e.Key == Key.Enter))
                            e.Handled = false;
                        else
                        {
                            e.Handled = true;
                            SetErrorInfo("请输入汉字、字母或数字！");
                        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
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

        #region [       Stop Text Paste      ]

        /// <summary>
        /// 粘贴时发生此事件，当粘贴长度超过最大长度时，提示。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
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
            int tlen = GetByteLength(Text);  //文本框原有文本的长度
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
        /// 获取控件内容
        /// </summary>
        /// <returns>object</returns>
        public object GetControlValue()
        {
            object res = ConverterData();

            if (createValueClassInstance == null)

                return this.textBox.Text;

            else
            {
                if (res != null)
                    return res;
                else
                    return this.textBox.Text;
            }

        }

        /// <summary>
        /// 设置控件内容
        /// </summary>
        /// <param name="value">值</param>
        public void SetControlValue(object value)
        {
            if (value != null)
            {
                this.textBox.Text = value.ToString();
            }
            else
            {
                this.textBox.Text = "";
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

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置样式异常" + ex.ToString());
            }
        }
        #endregion


    }
}
