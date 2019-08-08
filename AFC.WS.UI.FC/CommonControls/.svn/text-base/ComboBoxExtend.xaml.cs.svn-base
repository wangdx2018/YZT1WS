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
using System.Data;
using AFC.WS.UI.DataSources;
using System.Collections;
#endregion

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// ComboBox绑定类型。
    /// </summary>
    public enum BindType
    {
        /// <summary>
        ///默认绑定形式
        /// </summary>
        None,
        /// <summary>
        /// sql绑定
        /// </summary>
        SqlBindData,
        /// <summary>
        /// 数据字符串
        /// </summary>
        DataString
    }

    /// <summary>
    /// ComboBoxExtend.xaml 的交互逻辑，
    /// 
    /// 在继承原有ComboBox基础上添加绑定数据源功能。
    /// </summary>
    public partial class ComboBoxExtend : ComboBox, ICommonEdit
    {
        #region [       Declarataions      ]

        /// <summary>
        ///  创建DataTable 变量
        /// </summary>
        private DataTable dt = null;

        /// <summary>
        /// 创建接口变量
        /// </summary>
        private IComboBoxDataSource createClassInstance = null;

        /// <summary>
        /// 创建接口变量
        /// </summary>
        private IValueConverter createValueClassInstance = null;


        /// <summary>
        /// 创建数据字典变量
        /// </summary>
        private Dictionary<string, string> _dataKeyValue = null;

        /// <summary>
        /// 创建数据字典变量
        /// </summary>
        private Dictionary<string, string> _currentKeyValue = null;
        #endregion

        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public ComboBoxExtend()
        {
            InitializeComponent();

            this.SelectionChanged += new SelectionChangedEventHandler(ComboBoxExtend_SelectionChanged);
        }

        #endregion
        
        #region [       Properties      ]
        /// <summary>
        /// 设置绑定到ComboBox的数据类型
        /// </summary>
        private BindType _bindType;
        // ---> 设置绑定到ComboBox的数据类型。
        /// <summary>
        /// 设置绑定到ComboBox的数据类型
        /// </summary>
        [
        Description("设置绑定到ComboBox的数据类型。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public BindType BindType
        {
            get { return _bindType; }
            set { _bindType = value; }
        }
        /// <summary>
        /// 设置或获取SQl语句
        /// </summary>
        private string _sqlContent;
        // ---> 设置SQL语句，查询数据，绑定到ComboBox数据项。
        /// <summary>
        /// 设置或获取SQl语句
        /// </summary>
        [
        Description("设置SQL语句，查询数据,绑定到ComboBox。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string SqlContent
        {
            get { return _sqlContent; }
            set { _sqlContent = value; }
        }
        /// <summary>
        /// 设置绑定到ComboBox的数据字段，在选中Sql绑定类型中使用，其他绑定时可以为空。
        /// </summary>
        private string _bingDisplayField;
        // ---> 设置绑定到ComboBox的数据字段。
        /// <summary>
        /// 设置绑定到ComboBox的数据字段，在选中Sql绑定类型中使用，其他绑定时可以为空。
        /// </summary>
        [
        Description("设置绑定到ComboBox的显示数据字段。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string BindDisplayField
        {
            get { return _bingDisplayField; }
            set { _bingDisplayField = value; }
        }
        /// <summary>
        /// 设置绑定到ComboBox的数据字段，在选中Sql绑定类型中使用，其他绑定时可以为空。
        /// </summary>
        private string _bingHideField;
        // ---> 设置绑定到ComboBox的数据字段。
        /// <summary>
        /// 设置绑定到ComboBox的数据字段，在选中Sql绑定类型中使用，其他绑定时可以为空。
        /// </summary>
        [
        Description("设置绑定到ComboBox的隐藏数据字段。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string BindHideField
        {
            get { return _bingHideField; }
            set { _bingHideField = value; }
        }
        /// <summary>
        /// 设置绑定数据值，在选中DataString绑定类型中使用，SQl绑定时可以为空。
        /// </summary>
        private string _dataString;
        // ---> 设置绑定数据值,约定格式，中间用逗号分隔。
        /// <summary>
        /// 设置绑定数据值，在选中DataString绑定类型中使用，SQl绑定时可以为空。
        /// </summary>
        [
        Description("设置绑定数据值"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public String DataString
        {
            get { return _dataString; }
            set { _dataString = value; }
        }
        /// <summary>
        /// 设置隐藏字段绑定，在选中DataString绑定类型中使用，SQl绑定时可以为空。
        /// </summary>
        private string _hideDataString;
        // ---> 设置隐藏字段绑定,约定格式，中间用逗号分隔。
        /// <summary>
        /// 设置隐藏字段绑定，在选中DataString绑定类型中使用，SQl绑定时可以为空。
        /// </summary>
        [
        Description("设置隐藏字段绑定"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string HideDataString
        {
            get { return _hideDataString; }
            set { _hideDataString = value; }
        }
        /// <summary>
        /// 设定ComboBox起始值
        /// </summary>
        private string _startValue;
        // ---> 设定ComboBox起始值
        /// <summary>
        /// 设定ComboBox起始值
        /// </summary>
        [
        Description("设定ComboBox起始值。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }
        /// <summary>
        /// 设定ComboBox样式
        /// </summary>
        private string _comStyle;
        // ---> 设定ComboBox样式
        /// <summary>
        /// 设定ComboBox样式
        /// </summary>
        [
        Description("设定ComboBox样式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string ComboBoxStyle
        {
            get { return _comStyle; }
            set { _comStyle = value; }
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
        Category("ComboBoxExtend"),
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
        Category("ComboBoxExtend"),
        Filter()
        ]
        public int ControlHeight
        {
            get { return _controlHeight; }
            set { _controlHeight = value; }
        }
        /// <summary>
        /// 设置联动控件名称，若是多个ComboBox控件，用逗号分开。
        /// </summary>
        private string _bindControlName;
        /// <summary>
        /// 设置联动控件名称，若是多个ComboBox控件，用逗号分开。
        /// </summary>
        [
        Description("设置联动控件名称，若是多个ComboBox控件，用逗号分开。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public string BindControlName
        {
            get { return _bindControlName; }
            set { _bindControlName = value; }
        }

        /// <summary>
        ///ComboBox联动处理时，要继承IComboBoxDataSource接口，
        ///
        /// Combox中要调用实现接口中的方法，
        ///
        /// 若得到实现接口中的方法，需要知道实现接口的用户控件类名称，
        /// 
        /// 此属性要设置实现接口的用户控件名称。
        /// 
        /// </summary>
        private string _userControlName;
        /// <summary>
        ///ComboBox联动处理时，要继承IComboBoxDataSource接口，
        ///
        /// Combox中要调用实现接口中的方法，
        ///
        /// 若得到实现接口中的方法，需要知道实现接口的用户控件类名称，
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
        /// <summary>
        /// 是否为只读
        /// </summary>
        [
          Description("设置ComboBox是否为只读"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
          Category("ComboBoxExtend"),
          Filter()
        ]
        public bool CanReadOnly
        {
            get { return this.IsReadOnly; }
            set { this.IsReadOnly = value; }
        }
        /// <summary>
        /// 设置ComboBox是否可见
        /// </summary>
        [
        Description("设置ComboBox是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }
        /// <summary>
        /// 数据集合对象
        /// </summary>
        private object _dataObject;

        /// <summary>
        /// 数据集合对象
        /// </summary>
        public object DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value; }
        }
        /// <summary>
        /// 根据绑定字段，获取另一绑定字段的值。
        /// </summary>
        public Dictionary<string, string> DataKeyValue
        {
            get { return _dataKeyValue; }
            set { _dataKeyValue = value; }
        }

        /// <summary>
        ///选择项中是否有全部 True:显示,False:隐藏
        /// </summary>
        private bool _IsAll = true;
        /// <summary>
        ///选择项中是否有全部 True:显示,False:隐藏
        /// </summary>
        [
        Description("选择项中是否有全部"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("ComboBoxExtend"),
        Filter()
        ]
        public bool IsAll
        {
            get { return _IsAll; }
            set { _IsAll = value; }
        }

        #endregion

        #region [       Set Style      ]

        /// <summary>
        /// 设置宽度和高度，样式。
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
                    this.Height = 23;
                }

                this.IsEnabled =! this.CanReadOnly;

                this.Visibility = this.CanVisibility;
                

                
                if (ComboBoxStyle != null)
                {
                    Style style = this.FindResource(ComboBoxStyle) as Style;

                    this.Style = style;
                }
                else
                {
                    Style style = this.FindResource("ComboBoxStyle") as Style;

                    this.Style = style;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置ComboBoxStyle出错:" + ex.ToString());
            }
        }

        #endregion

        #region [       Binding Data Methods      ]

        /// <summary>
        /// ComboBox选中改变事件。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ComboBoxExtend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(BindControlName))
                {
                    ComboBoxExtend comboBoxExtend = sender as ComboBoxExtend;

                    ComboBoxItem item = comboBoxExtend.SelectedItem as ComboBoxItem;

                    string value = item.Content.ToString();

                    string[] bindControlName = BindControlName.Split(',');

                    _currentKeyValue = new Dictionary<string, string>();

                    _currentKeyValue.Add(value, DataKeyValue[value]);
                    //Util.Instance.BindComboBoxMethod(this, 
                    //    new AFC.WS.UI.FC.Common.BindComboBoxEventArgs(_currentKeyValue, bindControlName,this.BindHideField));
                    if (!String.IsNullOrEmpty(UserControlClassName))
                    {
                        bool isTrue = CreateInstance(UserControlClassName);

                        if (isTrue)
                        {
                            //  ComboBoxExtend combobox = ((FrameworkElement)Parent).FindName(bindControlName[0]) as ComboBoxExtend;

                            createClassInstance.BindComboBox(_currentKeyValue, bindControlName, (FrameworkElement)Parent);
                        }
                    }
                    else
                    {
                        WriteLog.Log_Info("UserControlClassName用户控件类名称为空！");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("ComboBox选中事件出现异常: " + ex.ToString());
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
                        createClassInstance = Activator.CreateInstance(type) as IComboBoxDataSource;

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
        private bool CreateConverterInstance(string className)
        {
            try
            {
                Type type = Type.GetType(className);

                if (type != null)
                {
                    if (createValueClassInstance == null)
                        createValueClassInstance = Activator.CreateInstance(type) as IValueConverter;

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
        /// ComboBox加载事件,调用绑定数据方法
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetStyle();
            }
            catch { }
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

        /// <summary>
        /// 加载绑定类型，根据绑定类型，绑定数据。
        /// </summary>
        private void LoadBindType(ComboBoxExtend comboBox)
        {
            try
            {
                _dataKeyValue = new Dictionary<string, string>();

                switch (BindType)
                {
                    case BindType.SqlBindData:
                        SqlBindData(comboBox);
                        break;
                    case BindType.DataString:
                        ArrayBindData(comboBox);
                        break;
                    case BindType.None:
                        if (DataObject != null)
                            DataObjectBind(comboBox);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("ComboBox控件内部错误：" + ex.ToString());
            }
        }

        /// <summary>
        /// 根据SQL绑定数据处理
        /// </summary>
        private void SqlBindData(ComboBoxExtend comboBox)
        {
            string bindDisplayField = null;

            string bindHideField = null;

            bool isTrue = comboBox.GetDataBySql(comboBox);

            if (isTrue)
            {
                if (dt == null)
                {
                    return;
                }
                try
                {
                    comboBox.Items.Clear();

                    comboBox.DataKeyValue.Clear();

                    comboBox.Text = "";

                    if (IsAll)
                    {
                        ComboBoxItem item1 = new ComboBoxItem();

                        item1.Content = "全部";

                        comboBox.Items.Add(item1); 
                        
                        comboBox.DataKeyValue.Add(item1.Content.ToString(), "%%");
                    }

                  

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        if (comboBox.dt.Rows[i][BindDisplayField] != null)
                            item.Content = comboBox.dt.Rows[i][BindDisplayField].ToString();

                        comboBox.Items.Add(item);
                    }

                    //comboBox.ItemsSource = comboBox.dt.DefaultView;

                    //Binding bind= new Binding();

                    //bind.Path=new PropertyPath(BindDisplayField);

                    //comboBox.SetBinding(ComboBox.ItemsSourceProperty, bind);

                    //comboBox.DisplayMemberPath = comboBox.dt.Columns[BindDisplayField].ToString();

                    comboBox.Text = StartValue;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            if (comboBox.dt.Rows[i][BindDisplayField] != null)
                                bindDisplayField = comboBox.dt.Rows[i][BindDisplayField].ToString();
                        }
                        catch
                        {
                            WriteLog.Log_Info("DataTable中不存在" + BindDisplayField + "绑定字段对应的数据！");
                            return;
                        }
                        try
                        {
                            //bindHideField = comboBox.dt.Rows[i][BindHideField].ToString();
                            /*************************************************************
                             * Modify Date:2009-08-24PM
                             * 
                             * Modify Note:当BindHideField为Null的时候，会出异常。
                             * 
                             *************************************************************/
                            bindHideField = BindHideField != null ? comboBox.dt.Rows[i][BindHideField].ToString() : null;
                        }
                        catch
                        { }
                        if (!comboBox.DataKeyValue.ContainsKey(bindDisplayField) && !string.IsNullOrEmpty(bindDisplayField))
                            comboBox.DataKeyValue.Add(bindDisplayField, bindHideField);
                        else
                        {
                            //WriteLog.Log_Info("bindDisplayField不允许为空或重复Key");
                        }
                    }


                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error("绑定到ComboBox数据失败" + ex.ToString());
                }
            }
            else
            {
                WriteLog.Log_Error("获取数据库数据失败");
            }
        }

        /// <summary>
        /// 字符串数组绑定
        /// </summary>
        private void ArrayBindData(ComboBoxExtend comboBox)
        {
            try
            {
                string bindDisplayField = null;

                string bindHideField = null;

                comboBox.ItemsSource = null;

                comboBox.DataKeyValue.Clear();

                if (!string.IsNullOrEmpty(DataString))
                {
                    string[] dataString = comboBox.DataString.Split(',');

                    comboBox.ItemsSource = dataString;

                    comboBox.Text = comboBox.StartValue;

                    string[] hideDataString = comboBox.HideDataString.Split(',');

                    for (int i = 0; i < dataString.Count(); i++)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(dataString[i]))
                                bindDisplayField = dataString[i];
                        }
                        catch
                        {
                            WriteLog.Log_Info("DataTable中不存在" + BindDisplayField + "绑定字段对应的数据！");
                            return;
                        }
                        try
                        {
                            if (!string.IsNullOrEmpty(hideDataString[i]))
                            bindHideField = hideDataString[i].ToString();
                        }
                        catch
                        { }

                        if (!comboBox.DataKeyValue.ContainsKey(bindDisplayField) && bindDisplayField != null && bindHideField != "")
                            comboBox.DataKeyValue.Add(bindDisplayField, bindHideField);
                        else
                        {
                           // WriteLog.Log_Info("bindDisplayField不允许为空或重复Key！");
                        }

                    }
                }
                else
                {
                    WriteLog.Log_Info("DataString字段内容为空！");
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("字符串数组绑定数据失败:" + ex.ToString());
            }
        }

        /// <summary>
        /// 数据集合绑定
        /// </summary>
        private void DataObjectBind(ComboBoxExtend comboBox)
        {
            try
            {
                comboBox.ItemsSource = null;

                if (DataObject != null)
                {
                    comboBox.ItemsSource = comboBox.DataObject as System.Collections.IEnumerable;
                }
                else
                {
                    WriteLog.Log_Info("数据集合绑定值为空！");
                }

                comboBox.Text = comboBox.StartValue;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("数据对象绑定失败:" + ex.ToString());
            }
        }

        /// <summary>
        /// 根据sql到数据库访问数据
        /// </summary>
        private bool GetDataBySql(ComboBoxExtend comboBox)
        {
            try
            {
                //if (dt == null)
                //{
                dt = ComboBoxDataSource.GetBindData(comboBox.SqlContent);
                //}
                if (dt == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("ComboBox访问数据库失败：" + ex.ToString());
                return false;
            }
        }

        #endregion

        #region [       ICommonEdit 成员      ]

        /// <summary>
        /// 获得ComboBox内容
        /// </summary>
        /// <returns>object</returns>
        public object GetControlValue()
        {
            if (this.Text == "全部" || String.IsNullOrEmpty(this.Text))
                return null;
            object res = ConverterData();

            if (createValueClassInstance == null)
            {

                if (String.IsNullOrEmpty(BindHideField))
                {
                    return this.Text;
                }
                else
                {
                    string temp = DataKeyValue[this.Text];
                    return temp;
                }
                //return this.Text;
            }
            else
            {
                if (res != null)
                    return res;
                else
                    return this.Text;
            }

        }

        /// <summary>
        /// 设置ComboBox值
        /// </summary>
        /// <param name="value">控件内容</param>
        public void SetControlValue(object value)
        {
            if (value != null)
            {
                this.StartValue = value.ToString();
                this.Text = value.ToString();
               
            }
            else
            {
                this.StartValue = "";
                this.Text = "全部";
            }
        }

        /// <summary>
        /// 初始化控件信息
        /// </summary>
        public void Initialize()
        {
            SetStyle();
            LoadBindType(this);
        }

        /// <summary>
        /// 联动时最后调用重新绑定数据。
        /// </summary>
        public void LoadBindType()
        {
            LoadBindType(this);
        }
        #endregion

    }
}