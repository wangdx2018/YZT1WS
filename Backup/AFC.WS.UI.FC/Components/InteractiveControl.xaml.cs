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
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;
using System.Reflection;


namespace AFC.WS.UI.Components
{
    /// <summary>
    /// 用户交互控件，该类的主要功能是可以让用户通过配置文件来动态
    /// 的生成交互控件。
    /// 增加了交互提示
    /// edited by wangdx 20111205 
    /// 增加了多语言的Binding Key 验证。
    /// </summary>
    public partial class InteractiveControl : UserControl
    {
        #region 字段
        /// <summary>
        /// 交互控件规则配置实体类
        /// </summary>
        private InteractiveControlRule config = null;


        public InteractiveControlRule Config
        {
            get { return this.config; }
        }


        /// <summary>
        /// 该list是将控件包装之后放到列表中的，由于有些控件需要配置Label属性
        /// 需要动态的配置label来显示内容。
        /// </summary>
        private Dictionary<string, FinalControl> controlList = new Dictionary<string, FinalControl>();

        /// <summary>
        /// Action集合 ,key:action中对应Button控件的名字，Value:Action操作类对象
        /// </summary>
        private Dictionary<string, KeyValuePair<IAction, Button>> actionDict = new Dictionary<string, KeyValuePair<IAction, Button>>();

        /// <summary>
        /// action的Button容器
        /// </summary>
        private StackPanel actionCollection = null;

        /// <summary>
        /// 重置按钮
        /// </summary>
        private Button resetButton = null;

        /// <summary>
        /// 流式布局的不占据一行的控件容器
        /// </summary>
        private WrapPanel flowPanel = new WrapPanel();

        /// <summary>
        /// 常量 Action的行的名字
        /// </summary>
        public const string ActionRowName = "actionRow";

        /// <summary>
        /// 常量 通用控件（不占一行）的行的名字
        /// </summary>
        public const string CommonControlRowName = "commonControlRow";

        /// <summary>
        /// 常量，Action的列的名字
        /// </summary>
        public const string ActionColumnName = "actionColumnName";

        /// <summary>
        /// Reset 按钮的名称
        /// </summary>
        public const string ResetButtonName = "btnReset";

        /// <summary>
        /// 
        /// </summary>
        public const string CommonControlColumnName = "commonControlColumn";

        /// <summary>
        /// 在栈式布局中的行也行之间的距离
        /// </summary>
        public uint RowSpace
        {
            set;
            get;
        }

        /// <summary>
        /// 在流布局中顶端的所占的高度
        /// </summary>
        public int TopSpace
        {
            set;
            get;
        }

        /// <summary>
        /// Lab的样式
        /// </summary>
        public string LabelStyle
        {
            set;
            get;
        }

        /// <summary>
        /// 行高
        /// </summary>
        public double RowHeight
        {
            set;
            get;
        }

        /// <summary>
        /// Label列所占据的相对宽度(label.Width/(label.Width+control.Width) 
        /// </summary>
        public double LabWidthPercent
        {
            set;
            get;
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public InteractiveControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 返回基本控件对象
        /// </summary>
        /// <param name="controlName">控件名字</param>
        /// <returns>基本控件对象</returns>
        public UIElement GetCommonControlByName(string controlName)
        {
            var temp = from data in this.actionDict
                       where data.Value.Value.Name.Equals(controlName)
                       select data.Value.Value;

            if (!this.controlList.ContainsKey(controlName)&&temp.Count()<1)
                return null;
             if (this.controlList.ContainsKey(controlName))
                return this.controlList[controlName].element;
            if (temp.Count() > 0)
                return temp.ToList()[0] as UIElement;
            return null;
        }

        #region 布局相关的操作函数

        /// <summary>
        /// 创建流式布局容器
        /// </summary>
        private int CreateFlowLayout()
        {
            RowDefinition rdTop = new RowDefinition();
            if (TopSpace == 0)
                TopSpace = 10;
            rdTop.Height = new GridLength(TopSpace, GridUnitType.Pixel);
            rootLayout.RowDefinitions.Add(rdTop);
            RowDefinition temp = new RowDefinition();
            rootLayout.RowDefinitions.Add(temp);
            if (config == null)
                return -1;
            this.flowPanel = new WrapPanel();
            rootLayout.Children.Add(flowPanel);
            Grid.SetRow(this.flowPanel, 1);
            return 0;
        }

        /// <summary>
        /// 将控件添加到流式布局
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int AddControlsToFlowLayout()
        {
            try
            {
                foreach (var temp in controlList)
                {
                    if (temp.Value.label != null)
                    {
                        flowPanel.Children.Add(UIHelper.GetControlSpace());
                        flowPanel.Children.Add(temp.Value.label);
                        temp.Value.label.VerticalAlignment = VerticalAlignment.Center;
                    }
                    flowPanel.Children.Add(UIHelper.GetControlSpace());
                    flowPanel.Children.Add(temp.Value.element);

                    if (temp.Value.element is FrameworkElement)
                    {
                        
                        FrameworkElement fwe = temp.Value.element as FrameworkElement;
                        fwe.VerticalAlignment = VerticalAlignment.Center;
                        fwe.Height = double.NaN;
                       // fwe.Width = double.NaN;
                    }
                    flowPanel.Children.Add(UIHelper.GetControlSpace());
                }
                if (config.CanRest && this.resetButton != null)
                {
                    flowPanel.Children.Add(resetButton);
                    flowPanel.Children.Add(UIHelper.GetControlSpace());
                }
                foreach (var action in actionDict)
                {
                    flowPanel.Children.Add(action.Value.Value);
                    flowPanel.Children.Add(UIHelper.GetControlSpace());
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
        /// 创建栈式布局
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int CreateStackLayout()
        {
            if (config == null)
                return -1;
            switch (config.ActionLocation)
            {
                case ActionLocations.Bottom:
                case ActionLocations.Top:
                    
                        RowDefinition actionRow = new RowDefinition();
                        actionRow.Name = ActionRowName;
                        if (config.ActionHeight != 0)
                        {
                            config.ActionHeight = 20;
                        }
                        actionRow.Height = new GridLength(config.ActionHeight, GridUnitType.Star);
                    
                    RowDefinition gridRow = new RowDefinition();
                    gridRow.Name = CommonControlRowName;
                    gridRow.Height = new GridLength(100-config.ActionHeight, GridUnitType.Star);
                    if (config.ActionLocation == ActionLocations.Top)
                    {
                        if (IsNeedCreateActionLayout())
                        {
                            rootLayout.RowDefinitions.Add(actionRow);
                        }
                        else
                        {
                            gridRow.Height = new GridLength(100,GridUnitType.Star);
                        }
                        rootLayout.RowDefinitions.Add(gridRow);
                    }
                    else
                    {
                        rootLayout.RowDefinitions.Add(gridRow);
                        if (IsNeedCreateActionLayout())
                        {
                            rootLayout.RowDefinitions.Add(actionRow);
                        }
                        else
                        {
                            gridRow.Height = new GridLength(100, GridUnitType.Star);
                        }
                        
                    }
                    break;
                case ActionLocations.Right:
                case ActionLocations.Left:
                    ColumnDefinition actionColumn = new ColumnDefinition();
                    actionColumn.Name = ActionColumnName;
                    if (config.ActionHeight == 0)
                    {
                        config.ActionHeight = 20;
                    }
                    actionColumn.Width = new GridLength(config.ActionHeight, GridUnitType.Star);
                    ColumnDefinition gridColumn = new ColumnDefinition();
                    gridColumn.Name = CommonControlColumnName;
                    gridColumn.Width = new GridLength(100-config.ActionHeight, GridUnitType.Star);
                    if (config.ActionLocation == ActionLocations.Left)
                    {
                        if (IsNeedCreateActionLayout())
                        {
                            rootLayout.ColumnDefinitions.Add(actionColumn);
                        }
                        else
                        {
                            gridColumn.Width = new GridLength(100, GridUnitType.Star);
                        }
                        rootLayout.ColumnDefinitions.Add(gridColumn);
                    }
                    else
                    {
                        rootLayout.ColumnDefinitions.Add(gridColumn);
                        if (IsNeedCreateActionLayout())
                        {
                            rootLayout.ColumnDefinitions.Add(actionColumn);
                        }
                        else
                        {
                            gridColumn.Width = gridColumn.Width = new GridLength(100, GridUnitType.Star);
                        }
                    }
                    break;
            }
            return 0;
        }


        private bool IsNeedCreateActionLayout()
        {
            return config.ActionList.Count > 0 || config.CanRest;
        }

        /// <summary>
        /// 得到需要创建的总的行数（不占1行的行数+占有1行的行数）
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int GetTotalRowCount()
        {
            return GetPublicRowCount() + GetSingleRowCount();
        }

        /// <summary>
        /// 得到公共控件的行数
        /// </summary>
        /// <returns>返回公共控件的行数</returns>
        private int GetPublicRowCount()
        {
            int publicRowCount = 0;
            int count = GetPublicRowControls().Count;
            if (config.ColumnCount == 0)
                config.ColumnCount = 1;
            publicRowCount = count / config.ColumnCount + ((count % config.ColumnCount == 0) ? 0 : 1);
            return publicRowCount;
        }

        /// <summary>
        /// 得到占有1行的公共控件的行数
        /// </summary>
        /// <returns>得到占有1行的公共控件的行数</returns>
        private int GetSingleRowCount()
        {
            int publicRowCount = 0;
            List<FinalControl> listFinal = GetSingleRowControls();
            for (int i = 0; i < listFinal.Count; i++)
            {
                if (listFinal[i].occupyRowCount <= 2)
                    listFinal[i].occupyRowCount = 2;
                publicRowCount += listFinal[i].occupyRowCount; //actionRow
                publicRowCount += 1;//Label row
            }

            return publicRowCount;
        }

        /// <summary>
        ///创建通用控件的布局（其中不包括控件）
        /// </summary>
        /// <returns>返回Grid对象</returns>
        private Grid GetCommonControlLayout()
        {
            #region add Rows
            Grid grid = new Grid();
            int publicRowCount = GetPublicRowCount();
            for (int i = 0; i < publicRowCount; i++)
            {
                if (RowSpace == 0)
                    RowSpace = 10;
                RowDefinition space = new RowDefinition { Height = new GridLength(RowSpace, GridUnitType.Pixel) };
                grid.RowDefinitions.Add(space);
                RowDefinition rd = new RowDefinition();
                if (this.RowHeight != 0)
                    rd.Height = new GridLength(this.RowHeight, GridUnitType.Pixel);
                grid.RowDefinitions.Add(rd);
            }
                //add single row 
            List<FinalControl> list = GetSingleRowControls();
            for (int i = 0; i < list.Count; i++)
            {
                // add space
                if (RowSpace == 0)
                    RowSpace = 10;
                RowDefinition space = new RowDefinition { Height = new GridLength(RowSpace, GridUnitType.Pixel) };
                grid.RowDefinitions.Add(space);

                // add control row
                if (list[i].occupyRowCount == 0)
                    list[i].occupyRowCount = 2;
                for (int index = 0; index < list[i].occupyRowCount + 1; index++)
                {
                    RowDefinition rd = new RowDefinition();
                    if (this.RowHeight != 0)
                        rd.Height = new GridLength(this.RowHeight, GridUnitType.Pixel);
                    grid.RowDefinitions.Add(rd);
                }
            }
            //add bottom space
            double height;
            if (this.RowHeight == 0)
                height = 10;
            else
                height = this.RowHeight;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(height, GridUnitType.Pixel) });

            #endregion

            #region add columns
           // grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Pixel) });
            if (config.ColumnCount == 0)
                config.ColumnCount = 1;
            int totalColumnCount = config.ColumnCount * 2;//label and control
            if (this.LabWidthPercent == 0 || this.LabWidthPercent >= 100)
            {
                this.LabWidthPercent = config.LabWidthPercent;//no setting label default 35%,control is 65%
            }
            for (int i = 0; i < totalColumnCount; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                grid.ColumnDefinitions.Add(cd);
                if (i % 2 == 0)
                    cd.Width = new GridLength(this.LabWidthPercent, GridUnitType.Star);
                else
                    cd.Width = new GridLength(100 - this.LabWidthPercent, GridUnitType.Star);
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20,GridUnitType.Pixel) });

            #endregion

            return grid;
        }

        /// <summary>
        /// 得到添加完成控件之后的Grid，该Grid中有控件和布局
        /// </summary>
        /// <returns>返回Grid对象</returns>
        private Grid GetAddedControlsGrid()
        {
            
            List<FinalControl> publicRowControls = GetPublicRowControls();
            Grid grid = GetCommonControlLayout();
            
            int rowIndex = 1;
            int columnIndex = 0;

            for (int i = 0; i < publicRowControls.Count; i++)
            {
                    Label labTip = publicRowControls[i].label;
                    if (labTip != null)
                    {
                        labTip.Height = double.NaN;
                         labTip.Width = double.NaN;
                         if (!grid.Children.Contains(labTip))
                         {
                             grid.Children.Add(labTip);
                         }
                        Grid.SetRow(labTip, rowIndex);
                        labTip.HorizontalAlignment = HorizontalAlignment.Right;
                       labTip.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetColumn(labTip, columnIndex);
                    }
                    columnIndex++;
                    FrameworkElement frameworkElement = publicRowControls[i].element as FrameworkElement;
                    frameworkElement.Width = double.NaN;
                    frameworkElement.Height = double.NaN;
                    grid.Children.Add(publicRowControls[i].element);
                    frameworkElement.HorizontalAlignment = HorizontalAlignment.Stretch;
                    frameworkElement.VerticalAlignment = VerticalAlignment.Center;
                 
                    Grid.SetColumn(publicRowControls[i].element, columnIndex);
                    Grid.SetRow(publicRowControls[i].element, rowIndex);
                    columnIndex++;
                    if (columnIndex == config.ColumnCount * 2 )
                    {
                        columnIndex = 0;
                        rowIndex+=2;
                    }                
            }

            int count = GetPublicRowControls().Count;
            int publicRowCount = count / config.ColumnCount + ((count % config.ColumnCount == 0) ? 0 : 1);
            if((publicRowCount*2)-1==rowIndex)
             rowIndex+=2;
            List<FinalControl> singleRowCount = GetSingleRowControls();
            for (int i = 0; i < singleRowCount.Count; i++)
            {
                if (singleRowCount[i].label != null)
                {
                    grid.Children.Add(singleRowCount[i].label);
                    singleRowCount[i].label.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(singleRowCount[i].label, rowIndex);
                    singleRowCount[i].label.VerticalContentAlignment = VerticalAlignment.Bottom;
                    Grid.SetColumn(singleRowCount[i].label, 0);
                    Grid.SetColumnSpan(singleRowCount[i].label, config.ColumnCount * 2-1);
                }
                rowIndex+=1;
                grid.Children.Add(singleRowCount[i].element);
                Grid.SetRow(singleRowCount[i].element, rowIndex);
                Grid.SetRowSpan(singleRowCount[i].element, singleRowCount[i].occupyRowCount);
                Grid.SetColumn(singleRowCount[i].element, 1);
                Grid.SetColumnSpan(singleRowCount[i].element, config.ColumnCount * 2-1);
                (singleRowCount[i].element as FrameworkElement).Width = double.NaN;
                (singleRowCount[i].element as FrameworkElement).Height = double.NaN;
                rowIndex += singleRowCount[i].occupyRowCount;
                rowIndex += 1;
            }
            return grid;
        }

        /// <summary>
        /// 创建Action的布局
        /// </summary>
        /// <returns>返回StackPanel</returns>
        private StackPanel CreateActionLayout()
        {
            StackPanel sp=ActionManager.CreateActionLayout(config.ActionLocation, config.ActionAlign);
            return sp;
        }

        /// <summary>
        /// 将控件添加到布局器中
        /// </summary>
        /// <param name="gridLayoutName">在Grid格的名称</param>
        /// <param name="control">控件实例</param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int AddControlToStackLayout(string gridLayoutName, UIElement control)
        {
            try
            {
                ColumnDefinition cd;
                RowDefinition rd;
                int index;
                switch (gridLayoutName)
                {
                    case ActionColumnName:
                    case CommonControlColumnName:
                        rootLayout.Children.Add(control);
                        cd = rootLayout.ColumnDefinitions.Single(temp => temp.Name == gridLayoutName);
                        index = rootLayout.ColumnDefinitions.IndexOf(cd);
                        Grid.SetColumn(control, index);
                        break;
                    case ActionRowName:
                    case CommonControlRowName:
                        rootLayout.Children.Add(control);
                        rd = rootLayout.RowDefinitions.Single(temp => temp.Name == gridLayoutName);
                        index = rootLayout.RowDefinitions.IndexOf(rd);
                        Grid.SetRow(control, index);
                        break;
                    default:
                        break;
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
        ///初始化
        /// </summary>
        /// <returns>成功返回0,否则返回相应的错误代码</returns>
        public int Initialize(string configFileName)
        {
            config = AFC.WS.UI.Config.Utility.Instance.GetInteractiveControlObject(configFileName);
            if (config == null)
                return -1;
            return Initialize(config);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">规则文件对象</param>
        /// <returns>成功返回0,否则返回相应的错误代码</returns>
        public int Initialize(AFC.WS.UI.Config.InteractiveControlRule config)
        {
            if (config == null)
                return -1;
            rootLayout.Children.Clear();
            rootLayout.RowDefinitions.Clear();
            rootLayout.ColumnDefinitions.Clear();
            this.controlList.Clear();
            //this.flowPanel.Children.Clear();
           // this.actionCollection.Children.Clear();
            this.actionDict.Clear();
            this.config = config;
            this.RowHeight = this.config.RowHeight;
            this.TopSpace = config.TopSpace;
          
            CreateCommonControls();//创建基础控件
            int res = CreateActions();//创建Action
            if (res != 0)
                return res;
            CreateReset();
            if (config.LayoutMode == LayoutModes.Flow)
            {
                res = CreateFlowLayout();
                if (res != 0)
                    return res;
                AddControlsToFlowLayout();
                return 0;
            }
            else
            {
                res = CreateStackLayout();
                if (res != 0)
                    return res;
                Grid grid = GetAddedControlsGrid();
                actionCollection= CreateActionLayout();
                AddActionButtonToStackPanel();
                if (config.ActionLocation == ActionLocations.Bottom || config.ActionLocation == ActionLocations.Top)
                {
                    if (IsNeedCreateActionLayout())
                    {
                        res = AddControlToStackLayout(ActionRowName, actionCollection);
                        if (res != 0)
                            return res;
                    }
                    res = AddControlToStackLayout(CommonControlRowName, grid);
                    if (res != 0)
                        return res;
                }
                else
                {
                    if (IsNeedCreateActionLayout())
                    {
                        res = AddControlToStackLayout(ActionColumnName, actionCollection);
                        if (res != 0)
                            return res;
                    }
                    res = AddControlToStackLayout(CommonControlColumnName, grid);
                    if (res != 0)
                        return res;
                }
                //rootLayout.Children.Add(grid);
            }
            return 0;
        }


        #endregion

        #region 基础控件相关的操作函数
        /// <summary>
        /// 得到独占一行的控件
        /// </summary>
        /// <returns>返回独占一行的列表</returns>
        private List<FinalControl> GetSingleRowControls()
        {
            var list = from temp in this.controlList
                       where temp.Value.isSingleRow
                       select temp.Value;
            return list.ToList();
        }

        /// <summary>
        /// 得到公共行的控件集合
        /// </summary>
        /// <returns>公共行控件集合</returns>
        private List<FinalControl> GetPublicRowControls()
        {
            var list = from temp in this.controlList
                       where !temp.Value.isSingleRow
                       select temp.Value;
            return list.ToList();
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        ///<returns>成功返回0，异常返回相应的错误代码</returns>
        private int CreateCommonControl(ControlProperty property,int tabIndex)
        {
            try
            {
                WriteLog.Log_Info("will create control Instance +[" + property.ControlTypeName + "]");
                if (string.IsNullOrEmpty(property.ControlTypeName))
                    return -1;
                Control control = Activator.CreateInstance(Type.GetType(property.ControlTypeName)) as Control;
                if (control != null)
                {

                    control.Name = property.ControlName;//set controlName
                    control.TabIndex = tabIndex;
                    if (!string.IsNullOrEmpty(property.ToolTip))
                    {
                        control.ToolTip = property.ToolTip;
                    }

                    if (property.Style != null)
                    {
                        UIHelper.SetControlStyle(control, property.Style);// set control style
                    }
                    for (int i = 0; i < property.PropertyValues.Count; i++) //Initliaize control value
                    {
                        PropertyInfo pi = control.GetType().GetProperty(property.PropertyValues[i].Key);
                        if (pi != null)
                        {
                            object res = UIHelper.ParsePropertyValue(pi, property.PropertyValues[i].Value);
                            if (res != null)
                                pi.SetValue(control, res, null);
                        }
                    }
                    if (control is ICommonEdit)
                    {
                        (control as ICommonEdit).Initialize();
                        (control as ICommonEdit).SetControlValue(property.InitValue);
                    }

                    FinalControl ci = new FinalControl();
                    ci.element = control;
                    ci.isSingleRow = property.IsOccupancyRow;
                    ci.name = property.ControlName;
                    ci.occupyRowCount = property.RowsCount;
                    ci.property = property;
                    if (!string.IsNullOrEmpty(property.Lable))
                    {
                        ci.label = new Label();

                        #region edit by wangdx 20111205 增加了多语言的设置

                        ci.label.Content = UIHelper.GetBindingFormatValue(property.Lable);
                        #endregion
                    }

                    if (!this.controlList.ContainsKey(ci.name))
                    {
                        this.controlList.Add(ci.name, ci);
                        return 0;
                    }
                    else
                    {
                        WriteLog.Log_Error("control name [ " + property.ControlName + "] has Exist");
                        return -1;
                    }
                }
                
                return -1;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("message: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return -1;
            }
        }

        /// <summary>
        /// 创建所有的控件
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private void CreateCommonControls()
        {
     
            if (config == null)
                return;
            int res = 0;
            for (int i = 0; i < config.ControlList.Count; i++)
            {
                res = CreateCommonControl(config.ControlList[i], i);
                if (res != 0)
                {
                    WriteLog.Log_Error("Create control error!");
                    continue;
                }
            }
            return;
        }

        #endregion

        #region Action 相关的操作函数

        /// <summary>
        /// 创建Action列表
        /// </summary>
        private int CreateActions()
        {
            try
            {
                if (config == null)
                    return -1;
                for (int i = 0; i < config.ActionList.Count; i++)
                {
                    KeyValuePair<string, KeyValuePair<IAction, Button>> temp = ActionManager.CreateAction(config.ActionList[i], this.ActionClicked, config.ButtonStyle, i+200);
                    if (temp.Key == string.Empty && temp.Value.Key == null && temp.Value.Value == null)
                        continue;
                    if (!this.actionDict.ContainsKey(temp.Key))
                        this.actionDict.Add(temp.Key, temp.Value);
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 单击Action按钮时创建进行的事件响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                //todo:check other data
                if (btn.Name == ResetButtonName)//重置按钮 处理
                {
                    Reset();
                    return;
                }
                List<QueryCondition> mappingData = CreateMappingData();
                IAction action = this.actionDict[btn.Name].Key;
                if (action != null)
                {
                    try
                    {
                        if (action.CheckValid(mappingData))//检查Action的合法性
                        {
                            action.DoAction(mappingData);//执行Action
                            return;
                        }
                        else
                        {
                            //MessageBox.Show("Action 检查非法!");
                            //Reset();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 将Action中的Button添加到容器中
        /// </summary>
        private void AddActionButtonToStackPanel()
        {
            if (config.CanRest)
            {
                CreateReset();
                actionCollection.Children.Add(UIHelper.GetControlSpace());
                actionCollection.Children.Add(this.resetButton);
            }

            if (actionDict.Count == 0)
                return;
            foreach (KeyValuePair<string, KeyValuePair<IAction, Button>> temp in actionDict)
            {
                actionCollection.Children.Add(UIHelper.GetControlSpace());
                actionCollection.Children.Add(temp.Value.Value);
            }
           
              actionCollection.Children.Add(UIHelper.GetControlSpace());
        }

        /// <summary>
        /// 创建邦定字段和邦定数值的键值对
        /// </summary>
        /// <returns>绑定字段和键值对的集合</returns>
        private List<QueryCondition> CreateMappingData()
        {
            List<QueryCondition> list = new List<QueryCondition>();
            foreach (var temp in this.controlList)
            {
                if (temp.Value.property.BindingField != null)
                {
                    if (temp.Value.element is ICommonEdit)
                    {
                        ICommonEdit edit = temp.Value.element as ICommonEdit;
                        try
                        {
                            QueryCondition conditon = new QueryCondition();
                            conditon.value = edit.GetControlValue();
                            ControlProperty cp=this.config.ControlList.Single(control => control.ControlName.Equals(temp.Value.name));
                            if (!string.IsNullOrEmpty(cp.ConvertoTypeName))
                            {
                                try
                                {
                                    IConvertor convert = Activator.CreateInstance(Type.GetType(cp.ConvertoTypeName)) as IConvertor;
                                    if (convert != null)
                                        conditon.value = convert.Convert(conditon.value, null, null, null);
                                }
                                catch (Exception ex)
                                {
                                    WriteLog.Log_Error("Create " + cp.ConvertoTypeName + " error!");
                                  
                                }
                            }

                            conditon.bindingData = temp.Value.property.BindingField;
                            conditon.controlLabelName = temp.Value.label.Content.ToString().Split('：')[0];
                            conditon.controlLabelName = conditon.controlLabelName.Split(':')[0];
                            conditon.operation = temp.Value.property.Symbols;
                            conditon.controlName = temp.Value.name;
                            list.Add(conditon);
                            //WriteLog.Log_Info("bindingField:" + temp.Value.property.BindingField + " value: " + conditon.value.ToString() + "operatorSymbols is :" + conditon.operation.ToString());
                        }
                        catch (Exception ex)
                        {
                            WriteLog.Log_Error(ex.ToString());
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 单击Reset 功能按钮函数
        /// </summary>
        private void Reset()
        {
            foreach (var temp in this.controlList)
            {
                if (temp.Value.element is ICommonEdit)
                {
                    ICommonEdit eidt = temp.Value.element as ICommonEdit;
                    try
                    {
                        eidt.SetControlValue(temp.Value.property.InitValue);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error(ex);
                    }
                }
            }
        }

        /// <summary>
        ///创建重置按钮，将重置按钮添加到布局中
        /// </summary>
        private void CreateReset()
        {
            if (config == null)
                return;
            if (this.config.CanRest)
            {
                this.resetButton = UIHelper.CreateButton("重置", ActionClicked, ResetButtonName, config.ButtonStyle);
            }
        }
        #endregion


        public void HandleAction(string btnName)
        {
            ActionClicked(new Button { Name = btnName }, new RoutedEventArgs());
        }
    }
}
