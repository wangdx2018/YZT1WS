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
#endregion

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// ComboBox绑定类型。
    /// </summary>
    public enum DataBindType
    {
        None,
        /// <summary>
        /// sql绑定
        /// </summary>
        SqlBindData,
        /// <summary>
        /// 数组绑定
        /// </summary>
        ListBind
    }

    /// <summary>
    /// CheckBoxGroup.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxGroup : UserControl, ICommonEdit
    {
        #region [       Declarataions       ]
        private DataTable dt = null;
        /// <summary>
        /// 创建数据字典变量
        /// </summary>
        private Dictionary<string, string> _dataKeyValue = null;

        /// <summary>
        ///返回名称列表
        /// </summary>
        private List<string> _dataValue = null;

        /// <summary>
        /// stackPanel变量
        /// </summary>
        private StackPanel childStackPanel = null;

        /// <summary>
        /// 主面板变量
        /// </summary>
        private StackPanel mainStackPanel = null;

        #endregion

        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBoxGroup()
        {
            InitializeComponent();
            
        }
        #endregion

        #region [       Properties       ]
        /// <summary>
        /// 设置绑定类型
        /// </summary>
        private DataBindType _dataBindType;
        /// <summary>
        /// 设置绑定类型
        /// </summary>
        [
         Description("设置绑定类型。"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
         Category("CheckBoxGroup"),
         Filter()
         ]
        public DataBindType DataBindType
        {
            get { return _dataBindType; }
            set { _dataBindType = value; }
        }
        /// <summary>
        /// 设置SQl字符串类型，在DataBindType为SqlBindData时有效。
        /// </summary>
        private string _SQlString;
        /// <summary>
        /// 设置SQl字符串类型,在DataBindType为SqlBindData时有效。
        /// </summary>
        [
          Description("设置SQl字符串类型,在DataBindType为SqlBindData时有效。"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
          Category("CheckBoxGroup"),
          Filter()
        ]
        public string SQlString
        {
            get { return _SQlString; }
            set { _SQlString = value; }
        }
        /// <summary>
        /// 设置显示内容绑定字段，在DataBindType为SqlBindData时有效。
        /// </summary>
        private string _bindField;
        /// <summary>
        /// 设置显示内容绑定字段,在DataBindType为SqlBindData时有效。
        /// </summary>
        [
          Description("设置显示内容绑定字段,在DataBindType为SqlBindData时有效。"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
          Category("CheckBoxGroup"),
          Filter()
        ]
        public string BindField
        {
            get { return _bindField; }
            set { _bindField = value; }
        }

        /// <summary>
        /// 设置隐藏内容绑定字段，根据显示内容查找隐藏内容，在DataBindType为SqlBindData时有效。
        /// </summary>
        private string _bindHideField;
        /// <summary>
        /// 设置隐藏内容绑定字段,根据显示内容查找隐藏内容， 在DataBindType为SqlBindData时有效。
        /// </summary>
        [
          Description("设置隐藏内容绑定字段,根据显示内容查找隐藏内容，在DataBindType为SqlBindData时有效。"),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
          Category("CheckBoxGroup"),
          Filter()
        ]
        public string BindHideField
        {
            get { return _bindHideField; }
            set { _bindHideField = value; }
        }

        /// <summary>
        /// 设置数组对象名称,在DataBindType为ListBind时有效。
        /// </summary>
        private List<String> _listName=new List<String>();
        /// <summary>
        /// 设置数组对象名称,在DataBindType为ListBind时有效。
        /// </summary>
        [
        Description("设置数组对象名称,在DataBindType为ListBind时有效。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public List<string> ListName
        {
            get { return _listName; }
            set { _listName = value; }
        }

        

        /// <summary>
        /// 设置每列存放CheckBox数量
        /// </summary>
        private int _columnBindCount;
        /// <summary>
        /// 设置每列存放CheckBox数量
        /// </summary>
        [
        Description("设置每列存放CheckBox数量。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public int ColumnBindCheckBoxCount
        {
            get { return _columnBindCount; }
            set { _columnBindCount = value; }
        }

        /// <summary>
        /// 设置CheckBox行之间间隔。
        /// </summary>
        private int _rowInterval;
        /// <summary>
        /// 设置CheckBox行之间间隔。
        /// </summary>
        [
       Description("设置CheckBox行之间间隔。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("CheckBoxGroup"),
       Filter()
        ]
        public int RowIntervalHeight
        {
            get { return _rowInterval; }
            set { _rowInterval = value; }
        }
        /// <summary>
        /// 设置CheckBox列之间间隔。
        /// </summary>
        private int _columnInterval;
        /// <summary>
        /// 设置CheckBox列之间间隔。
        /// </summary>
        [
       Description("设置CheckBox列之间间隔。"),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
       Category("CheckBoxGroup"),
       Filter()
        ]
        public int ColumnIntervalWidth
        {
            get { return _columnInterval; }
            set { _columnInterval = value; }
        }
        /// <summary>
        /// 设置列的宽度。
        /// </summary>
        private int _columnWidth;
        /// <summary>
        /// 设置列的宽度。
        /// </summary>
        [
           Description("设置列的宽度。"),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
           Category("CheckBoxGroup"),
           Filter()
        ]
        public int ColumnWidth
        {
            get { return _columnWidth; }
            set { _columnWidth = value; }
        }

        /// <summary>
        /// 设定Label样式
        /// </summary>
        private string _controlStyle;

        // ---> 设定CheckBoxGroup样式
        /// <summary>
        /// 设定控件样式
        /// </summary>
        [
        Description("设定控件样式。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public string CheckBoxGroupStyle
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
        Category("CheckBoxGroup"),
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
        Category("CheckBoxGroup"),
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
        Description("设置CheckBoxGroup是否为只读"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
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
        Description("设置CheckBoxGroup是否可见"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public Visibility CanVisibility
        {
            get { return this.Visibility; }
            set { this.Visibility = value; }
        }

        /// <summary>
        /// 设置CheckBox列所在的位置
        /// </summary>
        private HorizontalAlignment _StackPanelHorizontalAlignment;

        /// <summary>
        /// 设置CheckBox列所在的位置
        /// </summary>
        [
        Description("设置CheckBox列所在的水平位置"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public HorizontalAlignment StackPanelHorizontalAlignment
        {
            get { return _StackPanelHorizontalAlignment; }
            set { _StackPanelHorizontalAlignment = value; }
        }

        /// <summary>
        /// 设置CheckBox列所在的位置
        /// </summary>
        private VerticalAlignment _CheckBoxVerticalAlignment;

        /// <summary>
        /// 设置CheckBox列所在的位置
        /// </summary>
        [
        Description("设置CheckBox列所在的垂直位置"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public VerticalAlignment CheckBoxVerticalAlignment
        {
            get { return _CheckBoxVerticalAlignment; }
            set { _CheckBoxVerticalAlignment = value; }
        }

        /// <summary>
        /// 根据绑定字段，获取另一绑定字段的值。
        /// </summary>
        public Dictionary<string, string> GetKeyValue
        {
            get { return _dataKeyValue; }
            set { _dataKeyValue = value; }
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
        /// 设置是否全部选中
        /// </summary>
        private bool _allChecked;
        /// <summary>
        /// 设置是否全部选中
        /// </summary>
        [
        Description("设置是否全部选中"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public bool AllChecked
        {
            get { return _allChecked; }
            set { _allChecked = value; }
        }

        /// <summary>
        /// 设置初始选中值（根据名称）
        /// </summary>
        private List<string> _setCheckedList;
         /// <summary>
        /// 设置初始选中值（根据名称）
        /// </summary>
        [
        Description("设置初始选中值（根据名称）"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("CheckBoxGroup"),
        Filter()
        ]
        public List<string> SetCheckedList
        {
          get { return _setCheckedList; }
          set { _setCheckedList = value; }
        }

        #endregion

        #region [       Private Methods       ]
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch { }

        }

        /// <summary>
        /// 加载数据和布局
        /// </summary>
        private void LoadData()
        {
            try
            {

                this.layout.Children.Clear();
                StackPanel mainStackPanel = CreateMainStackPanel();
                if (mainStackPanel != null)
                {
                    this.layout.Children.Add(mainStackPanel);
                }
            }
            catch(Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }
        }

        /// <summary>
        /// 加载主Panel
        /// </summary>
        /// <returns>StackPanel</returns>
        private StackPanel CreateMainStackPanel()
        {
            int checkBoxCount = 0;

            mainStackPanel = new StackPanel();

            mainStackPanel.Orientation = Orientation.Horizontal;

            mainStackPanel.HorizontalAlignment = StackPanelHorizontalAlignment;

            switch (DataBindType)
            {
                case DataBindType.None:
                    return null;
                case DataBindType.ListBind:
                    checkBoxCount = ListName.Count;
                    break;
                case DataBindType.SqlBindData:
                    {
                        bool isTrue = GetDataBySql();

                        if (isTrue)
                        {
                            if (dt == null)
                            {
                                checkBoxCount = dt.Rows.Count;
                            }
                        }
                    }
                    break;
            }
            try
            {
                CreateChildStackPanel(mainStackPanel, checkBoxCount);
            }
            catch(Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }

            return mainStackPanel;
        }

        /// <summary>
        /// 加载子列CheckBox
        /// </summary>
        /// <param name="mainStackPanel">主面板加载子内容</param>
        /// <param name="checkBoxCount">CheckBox数量</param>
        private void CreateChildStackPanel(StackPanel mainStackPanel, int checkBoxCount)
        {

            if (mainStackPanel == null || checkBoxCount == 0)
                return;

            //mainStackPanel.Children.Clear();
            _dataKeyValue = new Dictionary<string, string>();

            _dataValue = new List<string>();


            string bindDisplayField = null;

            string bindHideField = null;

            if (ColumnBindCheckBoxCount != 0 || ListName.Count != 0)
            {
                int stackPanelCount = checkBoxCount / ColumnBindCheckBoxCount;
                int leaveCount = checkBoxCount % ColumnBindCheckBoxCount;
                int m = 0;
                for (int i = 0; i < stackPanelCount; i++)
                {
                    childStackPanel = new StackPanel();
                    if (ColumnWidth != 0)
                        childStackPanel.Width = ColumnWidth;
                    else
                        childStackPanel.Width = 100;

                    childStackPanel.HorizontalAlignment = HorizontalAlignment.Left;

                    childStackPanel.VerticalAlignment = CheckBoxVerticalAlignment;

                    TextBlock textBlock = new TextBlock();
                    if (ColumnIntervalWidth != 0)
                        textBlock.Width = ColumnIntervalWidth;
                    else
                        textBlock.Width = 50;
                    for (int j = 0; j < ColumnBindCheckBoxCount; j++)
                    {
                        CheckBoxExtend cb = new CheckBoxExtend();

                        cb.Checked += new RoutedEventHandler(cb_Checked);

                        cb.Unchecked += new RoutedEventHandler(cb_Unchecked);

                        if (DataBindType == DataBindType.ListBind)
                        {
                            cb.Content = ListName[m].ToString();
                        }

                        if (DataBindType == DataBindType.SqlBindData)
                        {
                            if (dt == null)
                                return;

                            bindDisplayField = dt.Rows[m][BindField].ToString();

                            bindHideField = dt.Rows[m][BindHideField].ToString();

                            cb.Content = bindDisplayField;

                            cb.Tag = bindHideField;

                        }

                        cb.Name = "CheckBox" + m;
                        TextBlock tb = new TextBlock();
                        if (RowIntervalHeight != 0)
                            tb.Height = RowIntervalHeight;
                        else
                            tb.Height = 20;
                        if (AllChecked)
                        {
                            cb.IsChecked = true;
                        }
                        childStackPanel.Children.Add(cb);
                        childStackPanel.Children.Add(tb);
                        m++;
                    }
                    mainStackPanel.Children.Add(childStackPanel);
                    mainStackPanel.Children.Add(textBlock);
                }
                if (leaveCount != 0)
                {
                    childStackPanel = new StackPanel();
                    if (ColumnWidth != 0)
                        childStackPanel.Width = ColumnWidth;
                    else
                        childStackPanel.Width = 100;

                    childStackPanel.HorizontalAlignment = HorizontalAlignment.Left;

                    childStackPanel.VerticalAlignment = CheckBoxVerticalAlignment;


                    TextBlock textBlock = new TextBlock();

                    if (ColumnIntervalWidth != 0)
                        textBlock.Width = ColumnIntervalWidth;
                    else
                        textBlock.Width = 50;

                    for (int n = 0; n < leaveCount; n++)
                    {
                        CheckBoxExtend cb = new CheckBoxExtend();

                        cb.Checked += new RoutedEventHandler(cb_Checked);

                        cb.Unchecked += new RoutedEventHandler(cb_Unchecked);

                        if (DataBindType == DataBindType.ListBind)
                            cb.Content = ListName[m].ToString();

                        if (DataBindType == DataBindType.SqlBindData)
                        {
                            if (dt == null)
                                return;
                            bindDisplayField = dt.Rows[m][BindField].ToString();

                            bindHideField = dt.Rows[m][BindHideField].ToString();

                            cb.Content = bindDisplayField;

                            cb.Tag = bindHideField;

                        }
                        cb.Name = "CheckBox" + m;

                        TextBlock tb = new TextBlock();

                        if (RowIntervalHeight != 0)
                            tb.Height = RowIntervalHeight;
                        else
                            tb.Height = 20;
                        if (AllChecked)
                        {
                            cb.IsChecked = true;
                        }
                        childStackPanel.Children.Add(cb);
                        childStackPanel.Children.Add(tb);
                        m++;
                    }
                    mainStackPanel.Children.Add(childStackPanel); 
                }
                
            }
            try
            {
                SetChecked(SetCheckedList);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }
        }

        /// <summary>
        /// CommBo检查事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void cb_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBoxExtend checkBox = sender as CheckBoxExtend;

                if (DataBindType == DataBindType.ListBind)
                {
                    if ((checkBox.Content != null) && (!_dataValue.Contains(checkBox.Content.ToString())))
                        _dataValue.Add(checkBox.Content.ToString());
                }

                if (DataBindType == DataBindType.SqlBindData)
                {
                    if (!_dataKeyValue.ContainsKey(checkBox.Tag.ToString()) && String.IsNullOrEmpty(checkBox.Tag.ToString()))

                        _dataKeyValue.Add(checkBox.Tag.ToString(), checkBox.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBoxExtend checkBox = sender as CheckBoxExtend;

                if (DataBindType == DataBindType.ListBind)
                {
                    if (checkBox.Content != null && _dataValue.Contains(checkBox.Content.ToString()))
                        _dataValue.Remove(checkBox.Content.ToString());
                }

                if (DataBindType == DataBindType.SqlBindData)
                {
                    if (checkBox.Tag != null && _dataKeyValue.ContainsKey(checkBox.Tag.ToString()))

                        _dataKeyValue.Remove(checkBox.Tag.ToString());
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }
        }

        /// <summary>
        /// 根据sql到数据库访问数据
        /// </summary>
        private bool GetDataBySql()
        {
            try
            {
                dt = ComboBoxDataSource.GetBindData(this.SQlString);

                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("ComboBox访问数据库失败：" + ex.ToString());
                return false;
            }
        } 

        /// <summary>
        /// 设置选中值
        /// </summary>
        /// <param name="list">名称列表</param>
        private void SetChecked(List<string> list)
        {
            if (list == null)
                return;
            try
            {
                if (mainStackPanel != null)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        for (int m = 0; m < mainStackPanel.Children.Count; m++)
                        {
                            if (mainStackPanel.Children[m] is StackPanel)
                            {
                                StackPanel stackPanel = mainStackPanel.Children[m] as StackPanel;

                                for (int i = 0; i < stackPanel.Children.Count; i++)
                                {
                                    if (stackPanel.Children[i] is CheckBoxExtend)
                                    {
                                        CheckBoxExtend checkBoxExtend = stackPanel.Children[i] as CheckBoxExtend;
                                        if (checkBoxExtend.Content.ToString() == list[j].ToString())
                                        {
                                            checkBoxExtend.IsChecked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
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
            switch (DataBindType)
            {
                case DataBindType.None:
                    return null;
                case DataBindType.ListBind:
                    return this.GetValueList;

                case DataBindType.SqlBindData:
                    {
                        return this.GetKeyValue;
                    }
            }
            return null;
        }

        /// <summary>
        /// 是指控件内容
        /// </summary>
        /// <param name="value">设置的值</param>
        public void SetControlValue(object value)
        {
            try
            {
                SetChecked((List<string>)value);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Debug(ex.ToString());
            }
        }

        /// <summary>
        /// 初始化控件信息
        /// </summary>
        public void Initialize()
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Info("加载数据" + ex.ToString());
            }
        }

        #endregion
    }
}
