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
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;
using Microsoft.Windows.Controls.Primitives;
using Microsoft.Windows.Controls;
using System.Data;
using AFC.WS.UI.Convertors;



namespace AFC.WS.UI.Components
{
    /// <summary>
    /// DataListControl.xaml 的交互逻辑
    /// 
    /// 修改日期:20110110  修改全选数据不全的问题 修改人:王冬欣
    /// 修改日期:20110111 修改了全选后的处理逻辑  修改人:王冬欣
    /// </summary>
    public partial class DataListControl : UserControl, IDataSourceClient
    {
        /// <summary>
        /// 数据源对象
        /// </summary>
        private IDataSource dataSource = null;

        /// <summary>
        /// DataGrid
        /// </summary>
        private DataGrid dataGrid = new DataGrid();

        /// <summary>
        /// ActionList
        /// </summary>
        private Dictionary<string, KeyValuePair<IAction, Button>> actionDict = new Dictionary<string, KeyValuePair<IAction, Button>>();

        /// <summary>
        /// 数据列表的规则文件
        /// </summary>
        private DataListRule config = null;

        /// <summary>
        /// 当前的数据表对象
        /// </summary>
        private System.Data.DataTable dataTable = null;

        /// <summary>
        /// action的的列表集合
        /// </summary>
        private StackPanel actionCollection = null;

        /// <summary>
        /// 该集合中包括了控件（Label和Button）
        /// </summary>
        private Dictionary<string, UIElement> controlDitc = new Dictionary<string, UIElement>();

        /// <summary>
        /// 该控件容器存放列表的基本的按钮(如导出到Excel,分页等)
        /// </summary>
        private StackPanel gridContextControlCollection = new StackPanel();

        /// <summary>
        /// 当前页的页码
        /// </summary>
        private int currentPageIndex = 1;

        /// <summary>
        /// 总条数
        /// </summary>
        private int totalCount = 0;

        /// <summary>
        /// 查找QueryList (多选)
        /// </summary>
        private Dictionary<int, List<QueryCondition>> Selected = new Dictionary<int, List<QueryCondition>>();


        /// <summary>
        /// 邦定字段和排序方式
        /// </summary>
        private Dictionary<string, System.ComponentModel.ListSortDirection> sortedData = new Dictionary<string, System.ComponentModel.ListSortDirection>();

        /// <summary>
        /// 当前选中的列的行
        /// </summary>
        private int selectIndex = -1;


        /// <summary>
        /// 选中列处理
        /// </summary>
        public IDataGridSelectionChangeHandle iSelectionChanged = null;

       
        /// <summary>
        /// 选中的CheckBox在单选中使用
        /// </summary>
        private CheckBox currentCheckBox = new CheckBox();

        /// <summary>
        /// 设置需要改变颜色的接口
        /// </summary>
        private IDataGridRowColorSetting iDataGridRowColorSet = null;

        /// <summary>
        /// 创建行的索引
        /// </summary>
        private int createRowIndex = 0;

        #region [ Const]

        /// <summary>
        /// 导出Excel功能按钮名字
        /// </summary>
        public const string Btn_Exprort_Excel_Name = "btnExprotExcel";

        /// <summary>
        /// 刷新功能按钮名称
        /// </summary>
        public const string Btn_Refresh_Name = "btnRefresh";

        /// <summary>
        /// 下一页功能按钮名称
        /// </summary>
        public const string Btn_NextPage_Name = "btnNextPage";

        /// <summary>
        /// 上一页功能按钮名称
        /// </summary>
        public const string Btn_LastPage_Name = "btnPreviewPage";

        /// <summary>
        /// 首页
        /// </summary>
        public const string Btn_First_Page = "btnFirstPage";

        /// <summary>
        /// 末页
        /// </summary>
        public const string Btn_Last_Page = "btnLastPage";

        /// <summary>
        /// Txt跳转
        /// </summary>
        public const string Txt_Go_To_Page = "txtGoToPage";

        /// <summary>
        /// Button跳转到某页
        /// </summary>
        public const string Btn_Go_To_Page = "btnGoToPage";

        /// <summary>
        /// 总页数（Lab的名字）
        /// </summary>
        public const string Lab_TotalPageCount = "labTotalPageCount";

        /// <summary>
        /// 当前页（lab的名字)
        /// </summary>
        public const string Lab_CurrentPageIndex = "labCurrentPageCount";

        /// <summary>
        /// 所有的总的数量
        /// </summary>
        public const string Lab_TotalItemCount = "labTotalItemCount";

        /// <summary>
        /// Action左右布局中的Grid控件的名字
        /// </summary>
        public const string Grid_Control_Name = "gridControlName";

        /// <summary>
        /// action 所在行的名字
        /// </summary>
        public const string Action_Row_Name = "actionRowName";

        /// <summary>
        /// action所在列的名字
        /// </summary>
        public const string Action_Column_Name = "actionColumnName";

        /// <summary>
        /// 和列表控件相关的控件集合
        /// </summary>
        public const string Controls_Collection_Row_Name = "cotrolsCollectionName";

        /// <summary>
        /// 数据列表控件放到的行的名字
        /// </summary>
        public const string Grid_Row_Name = "dataGridRowName";

        /// <summary>
        /// 表格的容器名称（Action的左右布局时候的使用)
        /// </summary>
        public const string Grid_Collection_Column_Name = "gridCollectionName";

        /// <summary>
        /// 当前页中所包括的数量
        /// </summary>
        public const string Lab_CurrentPageCount = "labCurrentPageNumber";
        #endregion

        public bool isExistFirstAndLastPage = true;

        public bool isExistGoPage = true;

        public DataListControl()
        {
            InitializeComponent();
            this.dataGrid.Loaded+=new RoutedEventHandler(dataGrid_Loaded);
            this.dataGrid.Sorting += new DataGridSortingEventHandler(dataGrid_Sorting);
            this.dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            this.dataGrid.SelectedCellsChanged += new SelectedCellsChangedEventHandler(dataGrid_SelectedCellsChanged);
          
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (this.iDataGridRowColorSet != null)
            {
                createRowIndex = e.Row.GetIndex();
                iDataGridRowColorSet.SetCurrentDataGridRow(e.Row, ((e.Row.Item) as DataRowView).Row);
            }
        }

        /// <summary>
        /// 对外提供的设置具体某行颜色的接口
        /// </summary>
        /// <param name="setGridRowColor">设置某行的具体颜色</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool SetGridRowColor(IDataGridRowColorSetting setGridRowColor)
        {
            try
            {
                this.iDataGridRowColorSet = setGridRowColor;
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 设置选中行
        /// </summary>
        /// <param name="index">选中行的索引</param>
        public void SetSelectionIndex(int index)
        {
            this.dataGrid.SelectedIndex = 0;
        }
        
        /// <summary>
        /// 初始化数据列表控件
        /// </summary>
        /// <param name="fileName">数据列表的配置文件的路径</param>
        /// <param name="dataSourceDirectionaryPath">数据源的目录名字（默认为Config目录）</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int Initliaize(string fileName,params object[] data)
        {
            config = AFC.WS.UI.Config.Utility.Instance.GetDataListObject(fileName);
            if (config != null)
            {
                if (data.Length == 0)
                    return Initliaize(config);
                else
                    return Initliaize(config, data);
            }
            return -1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dlr">规则文件的数据</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int Initliaize(DataListRule dlr,params object[] data)
        {
            try
            {
                /*********************************************************************
                 *  Add Date：2009-07-29 PM
                 *  
                 *      Note：加一个判断，判断参数DatalistRule是否为空。
                 * 
                 * *******************************************************************/
                if (dlr == null)
                {
                    return -1;
                }
                config = dlr;
                rootLayout.Children.Clear();
                this.dataGrid.Columns.Clear();
                this.Selected.Clear();
                rootLayout.RowDefinitions.Clear();
                this.sortedData.Clear();
                
                if (this.actionCollection != null)
                {
                    this.actionCollection.Children.Clear();
                }
                this.controlDitc.Clear();
                this.actionDict.Clear();

                rootLayout.ColumnDefinitions.Clear();
                int res = 0;
                if (data.Length == 0)
                {
                    res = CreateDataSource();//001 创建数据源
                }
                else
                {
                    res = CreateDataSource(data);//001 创建数据源
                }
                if (res != 0)
                {
                    //MessageBox.Show("数据源创建失败");
                    return res;
                }
                this.dataTable = CreateDataTable();
                if (dataTable == null)
                {
                    //MessageBox.Show("数据获取失败");
                    for (int i = 0; i < config.ColumnList.Count; i++)
                    {
                        DataGridTextColumn dgt = new DataGridTextColumn();
                        if (!string.IsNullOrEmpty(config.ColumnList[i].HeaderName))
                        {
                            dgt.Header =UIHelper.GetBindingFormatValue(config.ColumnList[i].HeaderName);
                        }
                        else if (!string.IsNullOrEmpty(config.ColumnList[i].BindingField))
                        {
                            dgt.Header = config.ColumnList[i].BindingField;
                        }
                        dgt.Width = new DataGridLength(config.ColumnList[i].Width, DataGridLengthUnitType.Pixel);
                        dataGrid.Columns.Add(dgt);
                    }
                }
                else
                {
                    DataTable dt = CreateDataTable(this.dataTable);//创建带有邦定字段的数据源
                    this.dataTable = CreateDataTableWithSelected(dt, out res);//带有选择字段的数据源
                    Binding(dataTable, true);
                }

                CreateTableContextControl(); //004 初始化DataGrid表格中的特定属
                CreateActions();//005 创建所有的Action
                res = CreateActionCollection();//006创建Action容器
                if (res != 0)
                    return res;
                res = AddActionButtonToLayout();//007将Action Button添加到容器上
                if (res != 0)
                    return res;
                InitliaizeTableContextControls();//008 创建并添加和容器列表控件相关的属性

                CreateLayout();//创建布局

                if (config.ActionLocation == ActionLocations.Left || config.ActionLocation == ActionLocations.Right)
                {
                    AddControlToLayout(this.actionCollection, Action_Column_Name);
                }
                else
                {
                    AddControlToLayout(this.actionCollection, Action_Row_Name);
                }
                AddControlToLayout(this.dataGrid, Grid_Row_Name);

                AddControlToLayout(gridContextControlCollection, DataListControl.Controls_Collection_Row_Name);

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                WriteLog.Log_Error("Initlaize grid error!!");
                return -1;
            }
        }


        #region [ DataSource Handler]

        /// <summary>
        /// 创建数据源
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int CreateDataSource(params object[] data)
        {

            /*********************************************************************
             *  Add Date：2009-07-29 PM
             *  
             *      Note：加一个判断，判断config是否为空。
             * 
             * *******************************************************************/
            if (config == null)
            {
                return -1;
            }

            dataSource = DataSourceManager.LookupDataSourceByName(config.DataSourceName);
            if (dataSource != null)
            {
                DataSourceManager.RegesiterDataSourceClient(dataSource, this);
                if (dataSource is ObjectDataSource&&data.Length>0)
                {
                    try
                    {
                        (dataSource as ObjectDataSource).SetObject(data[0]);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error(ex.ToString());
                    }
                }
                return 0;
            }
            WriteLog.Log_Error("Create dataSource name=[" + config.DataSourceName + "] error !");
            return -1;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns>返回创建完成之后的数据表</returns>
        private DataTable CreateDataTable()
        {
            DataTable dt = null;
            try
            {
                this.totalCount = dataSource.Count();
                if (config.PageRecordCount != 0 && config.Paging)
                    dt = dataSource.FetchPagingData(1, config.PageRecordCount);
                else
                {
                    dt = dataSource.GetDataTable();
                }
                return dt;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return null;
            }

        }

        /// <summary>
        /// 将数据绑定到列表中
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int Binding(DataTable dt, bool isNeedCreateColumns)
        {
            // DataGrid.DataGridRowList.Clear();
           
            this.dataGrid.AutoGenerateColumns = false;
            /*********************************************************************
             *  Add Date：2009-07-29 PM
             *  
             *      Note：加一个判断，判断dt是否为空。
             * 
             * *******************************************************************/
            if (config == null || dt == null)
            {
               // dt.Clear();
                this.dataGrid.ItemsSource = dt.DefaultView;
                return -1;
            }

            this.dataGrid.ItemsSource = dt.DefaultView;

            if (config.SelectionMode != SelectionModes.None)
            {
                DataGridTemplateColumn selectColumn = null;
                if (isNeedCreateColumns)
                {
                    selectColumn = new DataGridTemplateColumn();
                    selectColumn.CanUserReorder = false;
                    selectColumn.CanUserResize = false;
                    selectColumn.CanUserSort = false;

                    if (config.SelectionMode == SelectionModes.MultiChoice)
                    {
                        selectColumn.HeaderTemplate = GetCheckBoxHeader();
                    }

                    selectColumn.CellTemplate = new DataTemplate();
                    FrameworkElementFactory stackFactory = new FrameworkElementFactory(typeof(StackPanel));
                    stackFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                    stackFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                    FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox));
                    factory.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(checkBox_Checked));
                    factory.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(checkBox_Unchecked));
                    Binding bind = new Binding();
                    stackFactory.AppendChild(factory);
                    selectColumn.CellTemplate.VisualTree = stackFactory;
                    bind.Path = new PropertyPath(dt.Columns[0].ColumnName);
                    selectColumn.Header = dt.Columns[0].ColumnName;
                    factory.SetBinding(CheckBox.IsCheckedProperty, bind);
                    this.dataGrid.Columns.Add(selectColumn);
                }
                else
                {
                    selectColumn = this.dataGrid.Columns[0] as DataGridTemplateColumn;
                }
            }

            int index = config.SelectionMode != SelectionModes.None ? 1 : 0;

            for (int i = index; i < config.ColumnList.Count + index; i++)
            {

                DataGridTextColumn temp = null;
                if (isNeedCreateColumns)
                {
                    temp = new DataGridTextColumn();
                }
                else
                {
                    temp = this.dataGrid.Columns[i] as DataGridTextColumn;
                }
                DataColumn dc = null;
                try
                {
                    dc = dt.Columns[this.GetFormartString(config.ColumnList[i - index].BindingField.Trim(),'.')];
                    if (dc != null)
                    {
                        Binding binding = new Binding();
                        if (!string.IsNullOrEmpty(config.ColumnList[i - index].ConvertoTypeName))
                        {
                            try
                            {
                                binding.Converter = Activator.CreateInstance(Type.GetType(config.ColumnList[i - index].ConvertoTypeName)) as IValueConverter;

                                foreach (var propertyValue in config.ColumnList[i - index].PropertyValues)
                                {
                                    System.Reflection.PropertyInfo pi = binding.Converter.GetType().GetProperty(propertyValue.Key);
                                    if (pi != null)
                                    {
                                        object res = UIHelper.ParsePropertyValue(pi, propertyValue.Value);
                                        if (res != null)
                                            pi.SetValue(binding.Converter, res, null);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log_Error(ex);
                                WriteLog.Log_Error("Binding data error: Convertor type [" + config.ColumnList[i - index].ConvertoTypeName + "] error!");
                            }
                        }
                        binding.Path = new PropertyPath(dc.ColumnName);
                        if (!string.IsNullOrEmpty(config.ColumnList[i - index].HeaderName.Trim()))
                            temp.Header = config.ColumnList[i - index].HeaderName.Trim();
                        else
                            temp.Header = config.ColumnList[i - index].BindingField.Trim();
                        temp.IsReadOnly = true;
                        temp.Width = new DataGridLength(config.ColumnList[i - index].Width, DataGridLengthUnitType.Pixel);
                        temp.Binding = binding;
                        temp.CanUserResize = false;
                        temp.CanUserReorder = false;
                        temp.Visibility = config.ColumnList[i - index].IsVisbility ? Visibility.Visible : Visibility.Hidden;//增加了隐藏列
                        if (isNeedCreateColumns)
                        {
                            this.dataGrid.Columns.Add(temp);
                        }
                    }

                    else
                    {
                       
                        WriteLog.Log_Error(this.GetFormartString(config.ColumnList[i - index].BindingField.Trim(), '.') + " not found in DataTable");
                       // i--;
                        //continue;
                    }
                    
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                    WriteLog.Log_Error("binding column name=[" + config.ColumnList[i - index].BindingField + "] error!");
                    //this.dataGrid.AutoGenerateColumns
                }
            }
            

            return 0;

        }


        private DataTemplate GetCheckBoxHeader()
        {
            DataTemplate template = new DataTemplate();
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel));
            FrameworkElementFactory factoryTb = new FrameworkElementFactory(typeof(TextBlock));
            factoryTb.SetValue(TextBlock.TextProperty, "全选");

            FrameworkElementFactory factoryCb = new FrameworkElementFactory(typeof(CheckBox));
            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            factory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factoryCb.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(AllCheck_Check));
            factoryCb.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(AllCheck_UnCheck));

            factory.AppendChild(factoryCb);
            factory.AppendChild(factoryTb);
            template.VisualTree = factory;
            return template;
        }


        //private bool IsAllCheck = false;

        private void AllCheck_Check(object sender, RoutedEventArgs e)
        {
            isAllCheck = true;
            for (int i = 0; i < this.dataTable.Rows.Count; i++)
            {

                this.dataGrid.SelectedIndex = i;
                this.dataTable.Rows[i][0] = true;
            }
            isAllCheck = false;
            //var tempCollection = this.Selected.OrderBy(temp => temp.Key);
            //this.Selected.Clear();
            //foreach (var aa in tempCollection)
            //{
            //    this.Selected.Add(aa.Key, aa.Value);
            //}
            /* this.Selected.Clear();*/



            /*for (int i = 0; i < this.dataTable.Rows.Count; i++)
            {
                List<QueryCondition> list = new List<QueryCondition>();
                for (int j = 1; j < this.dataTable.Columns.Count; j++)
                {
                    QueryCondition qc = new QueryCondition();
                    qc.bindingData = config.ColumnList[j- 1].BindingField;
                    qc.operation = OperationSymbols.Equal;
                    qc.value = this.dataTable.Rows[i][qc.bindingData];
                    list.Add(qc);
                }
                
                this.Selected.Add(i, list);
            }
            
            Binding(this.dataTable, false);*/
        }
        private void AllCheck_UnCheck(object sender, RoutedEventArgs e)
        {
            this.Selected.Clear();
            for (int i = 0; i < this.dataTable.Rows.Count; i++)
            {
                this.dataGrid.SelectedIndex = i;
                this.dataTable.Rows[i][0] = false;
            }
            isAllCheck = false;
            // Binding(this.dataTable, false);
        }

        /// <summary>
        /// 创建带有表头的DataTable，能够绑定数据
        /// </summary>
        /// <param name="dt">在数据源获得的DataTable</param>
        /// <returns>带有表头的数据表</returns>
        private DataTable CreateDataTable(DataTable dt)
        {

            if (dt == null)
                return null;
            DataTable temp = dt.Copy();
            List<string> columnsName = new List<string>();
            for (int index = 0; index < temp.Columns.Count; index++) //找到DataTable中没有绑定的列
            {
                bool isExist = false;
                for (int j = 0; j < config.ColumnList.Count; j++)
                {
                    if (config.ColumnList[j].BindingField != null)
                    {
                        if (this.GetFormartString(config.ColumnList[j].BindingField.ToLower(), '.') == temp.Columns[index].ColumnName.ToLower())
                        {
                            isExist = true;
                            // temp.Columns[index].ColumnName = config.ColumnList[j].BindingField.ToLower();//为了避免联表查询带来在存在相同字段 
                            break;
                        }
                    }
                }
                if (!isExist)
                {
                    columnsName.Add(temp.Columns[index].ColumnName);
                }
            }
            //删除不要的列，没有邦定的列
            for (int i = 0; i < columnsName.Count; i++)
            {
                temp.Columns.Remove(columnsName[i]);
            }

            return temp;
        }

        /// <summary>
        /// 转换成带有格式的DataTable
        /// </summary>
        /// <returns>返回转化之后的DataTable</returns>
        private DataTable CovertFormatDataTable(DataTable dt)
        {
            if (dt == null)
                return null;
            Dictionary<string, IConvertor> convertorCollection = new Dictionary<string, IConvertor>();

            if (config == null)
            {
                return null;
            }

            for (int i = 0; i < config.ColumnList.Count; i++)
            {
                if (!string.IsNullOrEmpty(config.ColumnList[i].ConvertoTypeName))
                {
                    try
                    {
                        IConvertor convert = Activator.CreateInstance(Type.GetType(config.ColumnList[i].ConvertoTypeName)) as IConvertor;
                        if (convert != null)
                        {
                            for(int j=0;j<config.ColumnList[i].PropertyValues.Count;j++)
                            {
                                Type.GetType(config.ColumnList[i].ConvertoTypeName).GetProperty(config.ColumnList[i].PropertyValues[j].Key).SetValue(convert, config.ColumnList[i].PropertyValues[j].Value, null);
                            }
                            convertorCollection.Add(config.ColumnList[i].BindingField, convert);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error(ex);
                    }
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (var temp in convertorCollection)
                {
                    try
                    {
                        dt.Rows[i][temp.Key] = temp.Value.Convert(dt.Rows[i][temp.Key], null, null, null);
                        dt.Rows[i].AcceptChanges();//提交修改
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error(ex);
                    }
                }

                //object aa=dt.Rows[i].ItemArray;
            }
            return dt;
        }

        private DataTable CreateDataTableWithSelected(DataTable dt, out int res)
        {
            res = 0;
            try
            {
                if (config == null||dt==null)
                    return null;
                if (config.SelectionMode != SelectionModes.None)
                {
                    if (!dt.Columns.Contains("选择"))
                    {
                        dt.Columns.Add("选择", typeof(bool));
                        dt.Columns["选择"].DefaultValue = false;
                        dt.Columns["选择"].SetOrdinal(0);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][0] = false;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                WriteLog.Log_Error("Create select dataTable error!");
                res = -1;
                return null;
            }
        }


        #endregion

        #region [Action Handle]
        /// <summary>
        /// 创建Action的布局
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int CreateActionCollection()
        {
            if (config == null)
            {
                return -1;
            }
            this.actionCollection = ActionManager.CreateActionLayout(config.ActionLocation, config.ActionAlign);
            return this.actionCollection == null ? -1 : 0;

        }

        /// <summary>
        /// 创建所有的Action
        /// </summary>
        private void CreateActions()
        {
            if (config == null)
            {
                return;
            }

            for (int i = 0; i < config.ActionList.Count; i++)
            {
                KeyValuePair<string, KeyValuePair<IAction, Button>> temp = ActionManager.CreateAction(config.ActionList[i], ButtonClicked, config.ButtonStyle,i+500);
                if (temp.Key == string.Empty && temp.Value.Value == null && temp.Value.Value == null)
                    continue;
                this.actionDict.Add(temp.Key, temp.Value);
            }
        }
        /// <summary>
        /// 将action的功能按钮添加到Action容器中
        /// </summary>
        private int AddActionButtonToLayout()
        {
            try
            {
                foreach (var temp in this.actionDict)
                {
                    this.actionCollection.Children.Add(temp.Value.Value);
                    this.actionCollection.Children.Add(UIHelper.GetControlSpace());
                }
                if (this.controlDitc.ContainsKey(Btn_Exprort_Excel_Name))
                {
                    this.actionCollection.Children.Add(controlDitc[Btn_Exprort_Excel_Name]);
                    this.actionCollection.Children.Add(UIHelper.GetControlSpace());
                }
                if(this.controlDitc.ContainsKey(Btn_Refresh_Name))
                {
                    this.actionCollection.Children.Add(controlDitc[Btn_Refresh_Name]);
                    this.actionCollection.Children.Add(UIHelper.GetControlSpace());
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return -1;
            }
        }

        /// <summary>
        /// Button 按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                if (btn.Name == Btn_Exprort_Excel_Name)
                {
                    HandleExprotExcel();
                    return;
                }
                if (btn.Name == Btn_Refresh_Name)
                {
                    Refresh();
                    return;
                }
                if (btn.Name == Btn_LastPage_Name || btn.Name == Btn_NextPage_Name)
                {
                    if (btn.Name == Btn_LastPage_Name)
                    {
                        if (currentPageIndex > 1)
                            this.currentPageIndex--;
                    }
                    else
                    {
                        if (currentPageIndex < totalCount)
                            this.currentPageIndex++;
                    }
                    PageOperation(this.currentPageIndex);
                    return;
                }
                if (btn.Name == Btn_First_Page)
                {
                    this.currentPageIndex = 1;
                    PageOperation(1);
                    return;
                }
                if (btn.Name == Btn_Last_Page)
                {
                    int res = this.totalCount / config.PageRecordCount + ((this.totalCount % config.PageRecordCount) == 0 ? 0 : 1);
                    this.currentPageIndex = res;
                    PageOperation(res);
                    return;
                }
                if (btn.Name == Btn_Go_To_Page)
                {
                    TextBox tb = controlDitc[Txt_Go_To_Page] as TextBox;
                    if (tb != null)
                    {
                        int res = 0;
                        int pageCount = this.totalCount / config.PageRecordCount + ((this.totalCount % config.PageRecordCount) == 0 ? 0 : 1);
                        bool result = int.TryParse(tb.Text.Trim(), out res);
                        if (result && pageCount >= res)
                            this.currentPageIndex = res;
                        PageOperation(this.currentPageIndex);
                        return;
                    }
                }
                try
                {
                    IAction action = actionDict[btn.Name].Key;
                    if (action != null)
                    {
                        List<QueryCondition> list = CreateQueryListCondition();
                        try
                        {
                            if (list == null)
                            {
                                WriteLog.Log_Error("Create queryParams data error!");
                                return;
                            }
                            if (action.CheckValid(list))
                            {
                                int res = 0;
                                ResultStatus status = action.DoAction(list);
                                if (status != null && status.resultCode == 0)
                                {
                                    Refresh();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //todo:
                        }

                    }
                }
                catch (Exception ex)
                {
                    //todo: log here
                }
            }
        }

        /// <summary>
        /// 创建选择列
        /// </summary>
        private void CreateSelectedColumn()
        {
            if (config == null)
                return;
            //this.dataGrid.Loaded += new RoutedEventHandler(dataGrid_Loaded);
            if (config.SelectionMode == SelectionModes.None)
                return;
            else
            {
                
            }

        }

        void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
           
        }


        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitilaizeDataGrid()
        {
            try
            {
                this.dataGrid.AutoGenerateColumns = false;
                this.dataGrid.CanUserAddRows = false;
                this.dataGrid.CanUserResizeColumns = true;
                this.dataGrid.CanUserReorderColumns = false;
                //this.dataGrid.AlternationCount = 5;
                this.dataGrid.CanUserDeleteRows = false;
                //this.dataGrid.AlternatingRowBackground = Brushes.CornflowerBlue;
                //this.dataGrid.AlternationCount = 2;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }

        }

        private bool isAllCheck = false;

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (sender is DataGrid)
            {

                DataGrid grid = sender as DataGrid;
                this.selectIndex = grid.SelectedIndex;
                if (isAllCheck)
                {
                    checkBox_Checked(new CheckBox(), new RoutedEventArgs());
                }

                if (this.iSelectionChanged != null&&this.selectIndex>=0)
                {
                    DataRow dr = this.dataTable.Rows[this.selectIndex];
                    List<QueryCondition> list = new List<QueryCondition>();
                    for (int j = 0; j < this.config.ColumnList.Count; j++)
                    {
                        QueryCondition condition = new QueryCondition();
                        condition.bindingData = config.ColumnList[j].BindingField;
                        condition.operation = AFC.WS.UI.Common.OperationSymbols.Equal;
                        condition.value = dr[GetFormartString(condition.bindingData, '.')];
                        list.Add(condition);
                    }
                    this.iSelectionChanged.HandleChange(list);
                }

            }
            
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.isAllCheck = false;
            if (config.SelectionMode == SelectionModes.MultiChoice)
            {
                if (this.Selected.ContainsKey(this.dataGrid.SelectedIndex))
                    this.Selected.Remove(this.selectIndex);
            }
            else
            {
                CheckBox ch = sender as CheckBox;
                if (ch.Equals(currentCheckBox))
                {
                    this.Selected.Clear();
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.isAllCheck = false;
                for (int i = 0; i < this.dataGrid.SelectedItems.Count; i++)
                {
                    List<QueryCondition> list = new List<QueryCondition>();
                    for (int j = 0; j < this.config.ColumnList.Count; j++)
                    {
                        QueryCondition condition = new QueryCondition();
                        condition.bindingData = config.ColumnList[j].BindingField;
                        condition.operation = AFC.WS.UI.Common.OperationSymbols.Equal;
                        condition.value = (this.dataGrid.SelectedItem as DataRowView).Row[GetFormartString(condition.bindingData,'.')];
                        list.Add(condition);
                    }
                    if (config.SelectionMode == SelectionModes.Single)
                    {
                        this.currentCheckBox = sender as CheckBox;
                        this.Selected.Clear();
                    }


                    /***************************************************
                     * 
                     * 
                     * 当已经选择之后，再选择“全选”时出错。
                     * 
                     * 判断是否已经添加了。
                     * 
                     * 
                     * *************************************************/
                    if (!Selected.ContainsKey(this.selectIndex))
                    {

                        Selected.Add(this.selectIndex, list);
                    }
                }

                if (config.SelectionMode == SelectionModes.Single)
                {
                    UpdateSelectRow();
                }
            }
            catch (Exception ex)
            {
                this.currentCheckBox = sender as CheckBox;
                this.Selected.Clear();
                this.currentCheckBox.IsChecked = false;
                WriteLog.Log_Error(ex.Message);

            }
        }

        private void UpdateSelectRow()
        {
            if (config.SelectionMode == SelectionModes.Single)
            {
                for (int i = 0; i < this.dataTable.Rows.Count; i++)
                {
                    if (i == this.selectIndex)
                    {
                        this.dataTable.Rows[i][0] = true;
                    }
                    else
                    {
                        this.dataTable.Rows[i][0] = false;
                    }

                }
            }

        }

        /// <summary>
        /// 创建选中的查询条件，传递到Action中
        /// </summary>
        /// <returns>查询集合</returns>
        private List<QueryCondition> CreateQueryListCondition()
        {
            if (config == null)
                return null;
            //   config.SelectionMode = SelectionModes.MultiChoice;
            List<QueryCondition> list = new List<QueryCondition>();

            foreach (var temp in this.Selected)
            {
                list.AddRange(temp.Value);
            }
            return list;

        }

        /// <summary>
        /// 清除选择的信息
        /// </summary>
        private void ClearSelectedItems()
        {
            this.selectIndex = -1;
          
            this.Selected.Clear();
        }

        /// <summary>
        /// 创建分页显示按钮
        /// </summary>
        private void CreatePagingButton()
        {
            if (config == null)
                return;
            if (config.Paging && config.PageRecordCount != 0)//不配置所有数据显示在一页中
            {
                Button btn = UIHelper.CreateButton("上一页", ButtonClicked, Btn_LastPage_Name, config.ButtonStyle);
                if (btn != null)
                    controlDitc.Add(Btn_LastPage_Name, btn);
                Button btn2 = UIHelper.CreateButton("下一页", ButtonClicked, Btn_NextPage_Name, config.ButtonStyle);
                if (btn2 != null)
                    controlDitc.Add(Btn_NextPage_Name, btn2);
                if (this.isExistFirstAndLastPage)
                {
                    Button btn3 = UIHelper.CreateButton("首页", ButtonClicked, Btn_First_Page, config.ButtonStyle);
                    if (btn3 != null)
                        controlDitc.Add(Btn_First_Page, btn3);

                    Button btn4 = UIHelper.CreateButton("末页", ButtonClicked, Btn_Last_Page, config.ButtonStyle);
                    if (btn4 != null)
                        controlDitc.Add(Btn_Last_Page, btn4);
                }
                if (this.isExistGoPage)
                {
                    TextBox tb = new TextBox();
                    tb.Width = 30;
                    tb.Height = ActionManager.Btn_Default_Height;
                    tb.Name = Txt_Go_To_Page;
                    controlDitc.Add(Txt_Go_To_Page, tb);

                    Button btn5 = UIHelper.CreateButton("转到", ButtonClicked, Btn_Go_To_Page, config.ButtonStyle);
                    if (btn5 != null)
                        controlDitc.Add(Btn_Go_To_Page, btn5);
                }


                //总的页数
                System.Windows.Controls.Label lab = new System.Windows.Controls.Label();
                lab.Name = Lab_TotalPageCount;
                controlDitc.Add(Lab_TotalPageCount, lab);
                //当前页
                System.Windows.Controls.Label labCurrentPageIndex = new System.Windows.Controls.Label();
                labCurrentPageIndex.Name = Lab_CurrentPageIndex;
                controlDitc.Add(Lab_CurrentPageIndex, labCurrentPageIndex);

                //当前页中的个数
                System.Windows.Controls.Label labCurrentPageCount = new System.Windows.Controls.Label();
                labCurrentPageCount.Name = Lab_CurrentPageCount;
                controlDitc.Add(Lab_CurrentPageCount, labCurrentPageCount);

                //总条数
                System.Windows.Controls.Label labTotalItemsCount = new Label();
                labTotalItemsCount.Name = Lab_TotalItemCount;
                controlDitc.Add(Lab_TotalItemCount, labTotalItemsCount);
                RefreshPagingInfo(1);
            }
        }

        /// <summary>
        /// 创建刷新按钮
        /// </summary>
        private void CreateRefreshButton()
        {
            if (config == null)
                return;
            if (config.CanRefurbish)
            {
                Button btn = UIHelper.CreateButton("刷新", ButtonClicked, Btn_Refresh_Name, config.ButtonStyle);
                if (btn != null)
                    this.controlDitc.Add(Btn_Refresh_Name, btn);
            }
        }

        /// <summary>
        /// 创建导出Excel Button
        /// </summary>
        private void CreateImportToExcelButton()
        {
            if (config == null)
                return;
            if (config.CanExportExcel)
            {
                Button btn = UIHelper.CreateButton("导出Excel", ButtonClicked, Btn_Exprort_Excel_Name, config.ButtonStyle);
                if (btn != null)
                    this.controlDitc.Add(Btn_Exprort_Excel_Name, btn);
            }
        }

        /// <summary>
        /// 处理导出Excel
        /// </summary>
        private void HandleExprotExcel()
        {

            System.Windows.Forms.SaveFileDialog fileDlg = new System.Windows.Forms.SaveFileDialog();
            fileDlg.Filter = "Excel Worksheets|*.xls";
            fileDlg.Title = "请您选择Excel文件存放的路径";
            fileDlg.RestoreDirectory = true;
            System.Windows.Forms.DialogResult dr = fileDlg.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                if (config.SelectionMode != SelectionModes.None)
                {
                    System.Windows.MessageBoxResult result = MessageBox.Show("Yes 导出选中数据，NO导出所有数据?", "提示", MessageBoxButton.YesNo);
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {
                        if (this.Selected.Count < 1)
                        {
                            MessageBox.Show("列表中没有选中项，请您先选中列表");
                            return;
                        }
                        DataTable dt = null;
                        if (config.Paging && config.PageRecordCount > 0)
                            dt = dataSource.FetchPagingData(currentPageIndex, config.PageRecordCount);
                        else
                        {
                            dt = dataSource.GetDataTable();
                        }
                        DataTable temp = dt.Copy();
                        temp.Clear();
                        foreach (var data in this.Selected)
                        {
                            temp.Rows.Add(dt.Rows[data.Key].ItemArray);
                        }
                        temp = CreateDataTable(temp);
                        temp = CovertFormatDataTable(temp);
                        ImportToExcel(temp, fileDlg.FileName);
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        DataTable temp = dataSource.FetchPagingData(1, 5000);
                        if (temp != null)
                        {
                            dt = temp.Copy();
                            dt = CreateDataTable(dt);
                            dt = CovertFormatDataTable(dt);
                            ImportToExcel(dt, fileDlg.FileName);
                        }
                        else //无数据时候处理
                        {
                            MessageBox.Show("该列表中无数据，无法导出");
                        }
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    DataTable temp = dataSource.FetchPagingData(1, 5000);
                    if (temp != null)
                    {
                        dt = temp.Copy();
                        dt = CreateDataTable(dt);
                        dt = CovertFormatDataTable(temp);
                        ImportToExcel(dt, fileDlg.FileName);
                    }
                    else //无数据时候处理
                    {
                        MessageBox.Show("该列表中无数据，无法导出");
                    }
                }

            }


        }

        private void ImportToExcel(DataTable dt, string fileName)
        {
            try
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    for (int j = 0; j < config.ColumnList.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(config.ColumnList[j].HeaderName.Trim()) && config.ColumnList[j].BindingField.ToLower() == dt.Columns[i].ColumnName.ToLower())
                        {
                            dt.Columns[i].ColumnName = config.ColumnList[j].HeaderName;
                        }
                    }
                }

                if (dt.Rows.Count > 5000)
                {
                    dt.Clear();
                    for (int i = 0; i < 5000; i++)
                    {
                        dt.Rows.Add(this.dataTable.Rows[i].ItemArray);
                    }
                }
                AFC.WS.UI.Common.Util.exportToExcel(dt, fileName);
               // MessageBox.Show("导出Excel 成功");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
        }

        /// <summary>
        /// 刷新功能按钮
        /// </summary>
        private void Refresh()
        {
            
            //todo:
            this.currentPageIndex = 1;//重新刷新数据
            try
            {

                this.totalCount = dataSource.Count();
                int res = 0;
                if (config.PageRecordCount != 0 && config.Paging)
                {
                    
                    DataTable dt=this.CreateDataTableWithSelected(CreateDataTable(dataSource.FetchPagingData(currentPageIndex, config.PageRecordCount)), out res);
                    if (dt == null)
                    {
                        this.dataTable.Clear();
                    }
                    else
                    {
                        this.dataTable = dt;
                    }
                    Binding(dataTable, false);
                    RefreshPagingInfo(currentPageIndex);
                    ClearSelectedItems();
                }
                else
                {
                    DataTable dt = dataSource.GetDataTable();
                    if (dt == null&&this.dataTable!=null)
                    {
                        this.dataTable.Clear();
                    }
                    else
                    {
                        
                        this.dataTable=CreateDataTableWithSelected(dt, out res);
                    }
                    Binding(dataTable, false);
                    ClearSelectedItems();
                }
                if (this.iSelectionChanged != null)
                {
                    this.SetSelectionIndex(-1);
                    this.SetSelectionIndex(0);
                }
                   
                //todo: clear selectrow

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
        }

        /// <summary>
        /// 翻页操作
        /// </summary>
        /// <param name="pageIndex">页的索引</param>
        private void PageOperation(int pageIndex)
        {
            if (this.dataSource != null)
            {
                try
                {
                    int res = 0;
                    DataTable dt = this.CreateDataTable(dataSource.FetchPagingData(pageIndex, config.PageRecordCount));
                    this.dataTable = this.CreateDataTableWithSelected(dt, out res);
                    if (res != 0)
                        return;
                    ClearSelectedItems();
                    Binding(dataTable, false);

                    RefreshPagingInfo(pageIndex);



                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                }
            }
        }

        /// <summary>
        /// 刷新翻页的信息（包括按钮的可用和不可用处理）
        /// </summary>
        /// <param name="pageIndex">需要显示的数据页</param>
        private void RefreshPagingInfo(int pageIndex)
        {
            if (config == null)
                return;
            long pagetotalCount = this.totalCount / config.PageRecordCount + ((this.totalCount % config.PageRecordCount) == 0 ? 0 : 1);
            Label labCurrentIndex = controlDitc[Lab_CurrentPageIndex] as Label;

            labCurrentIndex.Content = "共计 " + pagetotalCount.ToString() + " 页 " + "  当前页 " + pageIndex.ToString();

            Label labCurrentPageCount = controlDitc[Lab_CurrentPageCount] as Label;

            if (this.dataTable != null)
            {
                labCurrentPageCount.Content = "该页共 " + this.dataTable.Rows.Count.ToString() + " 条";
            }

            Button btnLast = controlDitc[Btn_LastPage_Name] as Button;

            Button btnNext = controlDitc[Btn_NextPage_Name] as Button;

            Label labTotalItemCount = controlDitc[Lab_TotalItemCount] as Label;

            labTotalItemCount.Content = "总条数  " +  (this.totalCount <= 0 ? "0" : this.totalCount.ToString());

            
            if (pagetotalCount == 1)//总共为一页
            {
                btnLast.IsEnabled = false;
                btnNext.IsEnabled = false;
                return;
            }
            if (pageIndex == pagetotalCount)
            {
                btnNext.IsEnabled = false;
                btnLast.IsEnabled = true;
                return;
            }
            if (pageIndex == 1)
            {
                btnNext.IsEnabled = true;
                btnLast.IsEnabled = false;
                return;
            }
            btnLast.IsEnabled = true;
            btnNext.IsEnabled = true;

        }

        /// <summary>
        /// 初始化列表特定的属性（数据选择列，
        /// </summary>
        private void CreateTableContextControl()
        {

            if (config == null)
            {
                return;
            }
            InitilaizeDataGrid();
            CreateSelectedColumn();
            CreatePagingButton();
            CreateImportToExcelButton();
            CreateRefreshButton();
            
            InitliaizeSortedColumnsData();
            if (config.Style != null)
            {
                UIHelper.SetControlStyle(this.dataGrid, config.Style);
            }
        }

        /// <summary>
        /// 初始化排序
        /// </summary>
        private void InitliaizeSortedColumnsData()
        {
            if (config == null)
                return;
            for (int i = 0; i < config.ColumnList.Count; i++)
            {
                sortedData.Add(config.ColumnList[i].BindingField, System.ComponentModel.ListSortDirection.Ascending);
            }
        }

        /// <summary>
        /// 清除排序数据
        /// </summary>
        private void ClearSortedColumnData(string key)
        {
            List<string> list = this.sortedData.Keys.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != key)
                    sortedData[list[i]] = System.ComponentModel.ListSortDirection.Ascending;
            }
        }

        /// <summary>
        /// datagrid排序事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            try
            {

                string bindingData = config.ColumnList.Single(temp => temp.HeaderName.Trim() == e.Column.Header.ToString().Trim() ||
                    temp.BindingField.Trim() == e.Column.Header.ToString().Trim()).BindingField;

                SortedType type;
                ClearSortedColumnData(bindingData);
                if (sortedData[bindingData] == System.ComponentModel.ListSortDirection.Ascending)
                {
                    type = SortedType.SortAscending;
                    sortedData[bindingData] = System.ComponentModel.ListSortDirection.Descending;
                    e.Column.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                }
                else
                {
                    type = SortedType.SortDescding;
                    sortedData[bindingData] = System.ComponentModel.ListSortDirection.Ascending;
                    e.Column.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                }
                ClearSelectedItems();
                dataSource.SetSortParams(bindingData, type);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
        }

        #endregion

        #region Layout operation

        /// <summary>
        /// 总共分为四种布局，Action在左边，Action在右边，上边，下边
        /// </summary>
        private int CreateLayout()
        {
            if (config == null)
            {
                return -1;
            }

            switch (config.ActionLocation)
            {
                case ActionLocations.Left:
                case ActionLocations.Right:
                    return CreateVerticalLayout();
                case ActionLocations.Bottom:
                case ActionLocations.Top:
                    return CreateHorizontalLayout();
            }
            return 0;
        }

        /// <summary>
        /// 创建水平布局
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int CreateHorizontalLayout()
        {
            try
            {

                if (config == null)
                {
                    return -1;
                }
                //create action row
                RowDefinition rdAction = new RowDefinition();
                rdAction.Name = Action_Row_Name;
                if (config.ActionList.Count > 0||config.CanExportExcel||config.CanRefurbish)//无Action时候不创建
                {
                    rdAction.Height = new GridLength(8, GridUnitType.Star);
                }
                else
                {
                    rdAction.Height = new GridLength(0, GridUnitType.Star);
                }

                //create table controls row
                RowDefinition rdControl = new RowDefinition();
                rdControl.Name = DataListControl.Controls_Collection_Row_Name;
                rdControl.Height = new GridLength(8, GridUnitType.Star);

                //create table row
                RowDefinition rdGrid = new RowDefinition();
                rdGrid.Name = DataListControl.Grid_Row_Name;
                rdGrid.Height = new GridLength(84, GridUnitType.Star);

                if (config.ActionLocation == ActionLocations.Top)//actions in top location handle
                {
                    rootLayout.RowDefinitions.Add(rdAction);
                    rootLayout.RowDefinitions.Add(rdGrid);
                    rootLayout.RowDefinitions.Add(rdControl);
                }
                else// bottom handle
                {
                   
                    rootLayout.RowDefinitions.Add(rdGrid);
                    rootLayout.RowDefinitions.Add(rdControl);
                    rootLayout.RowDefinitions.Add(rdAction);
                 
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return -1;
            }

        }

        /// <summary>
        /// 创建竖直方向上的布局
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int CreateVerticalLayout()
        {
            try
            {
                //create action column
                ColumnDefinition cdAction = new ColumnDefinition();
                cdAction.Name = Action_Column_Name;
                if (config.ActionList.Count > 0||config.CanRefurbish||config.CanExportExcel)
                {
                    cdAction.Width = new GridLength(15, GridUnitType.Star);
                }
                else
                {
                    cdAction.Width = new GridLength(0, GridUnitType.Star);
                }


                //create table and controls column
                ColumnDefinition cdGrid = new ColumnDefinition();
                cdGrid.Name = Grid_Collection_Column_Name;
                cdGrid.Width = new GridLength(85, GridUnitType.Star);


                //initliaize grid that containing two rows
                Grid grid = new Grid();
                // create table row
                RowDefinition rd = new RowDefinition();
                rd.Name = DataListControl.Grid_Row_Name;
                rd.Height = new GridLength(80, GridUnitType.Star);

                RowDefinition rdControls = new RowDefinition();
                rdControls.Name = DataListControl.Controls_Collection_Row_Name;
                rdControls.Height = new GridLength(20, GridUnitType.Star);

                grid.RowDefinitions.Add(rd);
                grid.RowDefinitions.Add(rdControls);

                grid.Name = Grid_Control_Name;
                rootLayout.Children.Add(grid);

                if (config.ActionLocation == ActionLocations.Left)
                {
                    rootLayout.ColumnDefinitions.Add(cdAction);
                    rootLayout.ColumnDefinitions.Add(cdGrid);
                    Grid.SetColumn(grid, 1);
                }
                else
                {
                    rootLayout.ColumnDefinitions.Add(cdGrid);
                    rootLayout.ColumnDefinitions.Add(cdAction);
                    Grid.SetColumn(grid, 0);
                }

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("Create Vertical Layout error!");
                WriteLog.Log_Error(ex);
                return -1;
            }

        }

        /// <summary>
        /// 创建Table的自身的按钮
        /// </summary>
        private void InitliaizeTableContextControls()
        {
            gridContextControlCollection = new StackPanel();
            gridContextControlCollection.Orientation = Orientation.Horizontal;
            gridContextControlCollection.HorizontalAlignment = HorizontalAlignment.Center;

            foreach (var temp in this.controlDitc)
            {
                if (temp.Key != Btn_Refresh_Name && temp.Key != Btn_Exprort_Excel_Name)
                {
                    gridContextControlCollection.Children.Add(temp.Value);
                    ((FrameworkElement)temp.Value).VerticalAlignment = VerticalAlignment.Center;//按照顶端对齐
                    gridContextControlCollection.Children.Add(UIHelper.GetControlSpace());
                }
            }
        }


        private void AddControlToLayout(UIElement control, string name)
        {
            try
            {
                switch (config.ActionLocation)
                {
                    case ActionLocations.Left:
                    case ActionLocations.Right:
                        if (name == Action_Column_Name)
                        {
                            ColumnDefinition cd = rootLayout.ColumnDefinitions.Single(temp => temp.Name == name);
                            if (cd == null)
                                throw new Exception("can't find the column columnName=[" + name + " ]");
                            rootLayout.Children.Add(control);
                            Grid.SetColumn(control, rootLayout.ColumnDefinitions.IndexOf(cd));
                        }
                        else
                        {
                            Grid grid = LogicalTreeHelper.FindLogicalNode(rootLayout, DataListControl.Grid_Control_Name) as Grid;

                            if (grid != null)
                            {
                                RowDefinition rd = grid.RowDefinitions.Single(r => r.Name == name);
                                if (rd == null)
                                    throw new Exception("can't find the row name=[ " + name + "]");
                                grid.Children.Add(control);
                                Grid.SetRow(control, grid.RowDefinitions.IndexOf(rd));
                            }
                        }
                        break;
                    case ActionLocations.Bottom:
                    case ActionLocations.Top:
                        RowDefinition row = rootLayout.RowDefinitions.Single(r => r.Name == name);
                        if (row == null)
                            throw new Exception("can't find the row Name=[ " + name + "]");
                        rootLayout.Children.Add(control);
                        Grid.SetRow(control, rootLayout.RowDefinitions.IndexOf(row));
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
        }


        #endregion

        #region IDataSourceClient 成员

        private delegate void RefreshDelegate();

        public void HandleDataSourceChange()
        {
            this.Dispatcher.Invoke(new RefreshDelegate(() => Refresh()));
        }

        public void HandleDataSourceDispose()
        {
            dataSource = null;
        }

        #endregion


        private string GetFormartString(string value,char split)
        {
            return !value.Contains(split) ? value : value.Split(split)[1].ToString();
        }


      
    }
}
