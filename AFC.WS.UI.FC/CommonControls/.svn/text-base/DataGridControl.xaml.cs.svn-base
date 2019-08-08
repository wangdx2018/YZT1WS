#region [       Copyright (C), 2009,  中软AFC     ]
/************************************************************
  FileName: DataGridControl.xaml
  
  Author: 沈克涛    
 
  Version :  1.0   
 
  Date:20090715
 
  Description: 数据列表分级显示数据   
 
  Function List:  
 
    1. LoadListContent  // ---> 加载一级数据
 
    2. LoadDetailContent // ---> 加载详细数据
 
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
using System.Windows.Shapes;
using System.Data;
using Microsoft.Windows.Controls;
using System.Xml;
using AFC.WS.UI.Common;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Threading;

#endregion

namespace AFC.WS.UI.CommonControls 
{
    // --> 初始化DataGrid控件，加载设备信息数据，点击设备信息数据，显示设备下各部件的详细信息
    /// <summary>
    /// 初始化DataGrid控件，加载设备信息数据，
    /// 
    /// 点击设备信息数据，显示设备下各部件的详细信息。
    /// </summary>
    /// <remarks>
    /// 初始化DataGrid控件，在不改变原来结构的基础上，实现二级数据联动。
    /// </remarks>
    public partial class DataGridControl : System.Windows.Controls.UserControl
    {
        #region [       Declarations       ]
        /// <summary>
        /// 创建用户控件对象使用
        /// </summary>
        IDataProviderInterface createClassInstance = null;

        /// <summary>
        /// 查询等待委托
        /// </summary>
        public delegate void SerarchDelegate();

        /// <summary>
        /// Create a new instance of timer object 
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region [       Constructor       ]
        /// <summary>
        /// 构造函数，初始化控件。
        /// </summary>
        public DataGridControl()
        {
            InitializeComponent();
        }
        #endregion

        #region [       Properties       ]
        /// <summary>
        /// 获得使用DataGridControl用户控件名称。
        /// </summary>
        private string userControlName;

        /// <summary>
        /// 获得使用DataGridControl用户控件名称。
        /// </summary>
        [
        Description("设定验证的数据类型"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TextBoxExtend"),
        Filter()
         ]
        public string UserControlName
        {
            get { return userControlName; }
            set { userControlName = value; }
        }

        private bool _isAutoRefresh;

        /// <summary>
        /// 设置是否使用定时循环刷新数据
        /// </summary>
        [
        Description("设置是否使用定时循环刷新数据"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("IsAutoRefresh"),
        Filter()
         ]
        public bool IsAutoRefresh
        {
            get { return _isAutoRefresh; }
            set { _isAutoRefresh = value; }
        }
        private int _refreshTime;

        /// <summary>
        /// 设置刷新时间
        /// </summary>
        [
        Description("设置刷新时间"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("RefreshTime"),
        Filter()
         ]
        public int RefreshTime
        {
            get { return _refreshTime; }
            set { _refreshTime = value; }
        }

        #endregion

        #region [       Private Methods       ]
        /// <summary>
        /// 选中数据表格项事件，当选中时出发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                this.dataGridDeviceInfo.RowDetailsTemplate = null;
                DataRowView drv = this.dataGridDeviceInfo.SelectedItems[0] as DataRowView;
                bool isTrue = LoadDetailContent(drv);

                if (isTrue)
                {
                    WriteLog.Log_Info("加载详细数据成功.");
                }
                else
                {
                    WriteLog.Log_Info("加载详细数据失败.");
                }
                //if (e.AddedCells.Count > 0 && e.RemovedCells.Count > 0)
                //{
                //    bool isTrue = LoadDetailContent();

                //    if (isTrue)
                //    {
                //        WriteLog.Log_Info("加载详细数据成功.");
                //    }
                //    else
                //    {
                //        WriteLog.Log_Info("加载详细数据失败.");
                //    }
                //}
                //else
                //{
                //    if (this.dataGridDeviceInfo.SelectedIndex == 0 && e.AddedCells.Count > 0)
                //    {
                //        bool isTrue = LoadDetailContent();

                //        if (isTrue)
                //        {
                //            WriteLog.Log_Info("加载详细数据成功.");
                //        }
                //        else
                //        {
                //            WriteLog.Log_Info("加载详细数据失败.");
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SearchingControlHost.Children.Add(new Searching());
            try
            {
                if (IsAutoRefresh)
                {
                    try
                    {
                       // timerProgress.Start();
                        //timerProgress.Interval=new TimeSpan(0,0,0

                        timer.Tick += new EventHandler(timer_Tick);

                        int refreshTime = 0;

                        if (RefreshTime != 0)
                        {
                            refreshTime = RefreshTime * 1000;
                        }
                        else
                        {
                            refreshTime = 600 * 1000;
                        }

                        timer.Interval = new TimeSpan(0, 0, 0, 0, refreshTime);

                        timer.Start();

                    }
                    catch (Exception ex)
                    {
                        WriteLog.Log_Error("动态加载设备状态信息异常:" + ex.ToString());
                    }
                }

                try
                {
                    //Thread th = new Thread(new ThreadStart(SetStart));
                    //th.Start();

                    //DoEvents();

                    ((Storyboard)this.Resources["Show"]).Begin(this);
                }
                catch
                { }
                this.dataGridDeviceInfo.AlternationCount = 0;
                
                LoadListContent();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 定时刷新设备状态数据。 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.dataGridDeviceInfo.AlternationCount = 0;

            bool isTrue = LoadListContent();
        }

        /// <summary>
        /// 创建用户控件对象，调用加载一级数据，赋值到DataGrid控件。
        /// </summary>
        /// <returns>True:成功，False：失败</returns>
        private bool LoadListContent()
        {
            try
            {

                this.dataGridDeviceInfo.ItemsSource = null;

                this.dataGridDeviceInfo.IsReadOnly = true;

                this.dataGridDeviceInfo.CanUserAddRows = false;

                bool isTrue = CreateInstance(UserControlName);
            
                if (isTrue)
                {
                    this.dataGridDeviceInfo.ItemsSource = createClassInstance.LoadListData();  
                    
                    return true;
                }
                else
                {
                    WriteLog.Log_Info("创建用户控件对象失败");
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("加载数据时出错：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 重新赋值功能
        /// </summary>
        /// <param name="dv">显示信息列表</param>
        public void ResetListContent(DataView dv)
        {
            this.dataGridDeviceInfo.ItemsSource = null;

            this.dataGridDeviceInfo.ItemsSource = dv;
        }

        /// <summary>
        /// 加载详细数据，创建绑定列，赋值到绑定列。
        /// </summary>
        /// <returns>True:成功，False：失败</returns>
        private bool LoadDetailContent(DataRowView drv)
        {
            try
            {
                DataView dv = null;

                this.dataGridDeviceInfo.RowDetailsTemplate = null;

                if (drv != null)
                {
                    dv = createClassInstance.LoadDetailData(drv);

                    if (dv.Count != 0)
                    {
                        DataTemplate dataTemplate = new DataTemplate();

                        FrameworkElementFactory innerFactory = new FrameworkElementFactory(typeof(DataGrid));

                        FrameworkElementFactory spOuterFactory = new FrameworkElementFactory(typeof(Border));

                        FrameworkElementFactory spOuterFactoryStackPanel = new FrameworkElementFactory(typeof(StackPanel));

                        spOuterFactory.SetValue(Border.StyleProperty, SetBorderStyle());
                        //spOuterFactoryStackPanel.SetValue(StackPanel.WidthProperty,900.00);
                        //spOuterFactoryStackPanel.SetValue(StackPanel.HeightProperty, 1900.00);
                        spOuterFactoryStackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                        innerFactory.SetValue(DataGrid.FocusableProperty, true);

                        Binding bind = new Binding();

                        bind.Source = dv;

                        //绑定数据
                        innerFactory.SetBinding(DataGrid.ItemsSourceProperty, bind);
                        //设置自动生成列
                        innerFactory.SetValue(DataGrid.AutoGenerateColumnsProperty, true);
                        //设置水平滚动条
                        innerFactory.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                        innerFactory.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                        innerFactory.SetValue(ScrollViewer.CanContentScrollProperty, true);
                        innerFactory.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, true);
                        innerFactory.SetValue(DataGrid.CanUserAddRowsProperty, false);
                        innerFactory.SetValue(DataGrid.IsReadOnlyProperty, true);
                        innerFactory.SetValue(DataGrid.AllowDropProperty, true);
                        innerFactory.SetValue(DataGrid.AreRowDetailsFrozenProperty, true);
                        innerFactory.SetValue(DataGrid.CanUserResizeRowsProperty, false);
                        innerFactory.SetValue(DataGrid.CanUserResizeColumnsProperty, false);
                        //设置行的样式
                        //Style rowStyle = SetRowStyle();
                        //if (rowStyle != null)
                        //{
                        //    innerFactory.SetValue(DataGrid.RowStyleProperty, rowStyle);
                        //}
                        //设置行的样式
                        Style rowDetailHeaderStyle = SetRowHeaderStyle();

                        if (rowDetailHeaderStyle != null)
                        {
                            innerFactory.SetValue(DataGrid.RowHeaderStyleProperty, rowDetailHeaderStyle);
                        }

                        //Style style = SetGridStyle();
                        //if (style != null)
                        //{
                        //    innerFactory.SetValue(Grid.StyleProperty, style);
                        //}

                        spOuterFactoryStackPanel.AppendChild(innerFactory);

                        spOuterFactory.AppendChild(spOuterFactoryStackPanel);

                        dataTemplate.VisualTree = spOuterFactory;

                        dataGridDeviceInfo.RowDetailsTemplate = dataTemplate;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("加载详细数据出错：" + ex.ToString());

                return false;
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
                        createClassInstance = Activator.CreateInstance(type) as IDataProviderInterface;

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
        /// 显示完成时触发。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ObjectAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            this.SearchingControlHost.Children.Clear();
        }

        /// <summary>
        /// 设置启动时等待画面
        /// </summary>
        private void SetStart()
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
              new SerarchDelegate(() =>
              {
                  SearchingControlHost.Children.Add(new Searching());
              }));
        }

        /// <summary>
        /// 关闭线程
        /// </summary>
        public void StopTimer()
        {
            this.timer.Stop();
        }
        #endregion

        #region [       Set Style      ]

        /// <summary>
        /// 设置Border样式。
        /// </summary>
        /// <returns>style</returns>
        private Style SetBorderStyle()
        {
            try
            {
                Style style = this.FindResource("BorderStyle") as Style;

                return style;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置BorderStyle出错:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///设置详细信息Grid样式
        /// </summary>
        /// <returns>style</returns>
        private Style SetGridStyle()
        {
            try
            {
                Style style = this.FindResource("GridStyle") as Style;

                return style;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置GridStyle出错:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///设置详细信息Grid样式
        /// </summary>
        /// <returns>style</returns>
        private Style SetRowStyle()
        {
            try
            {
                object styleObj = this.FindResource("RowStyle");

                if (styleObj != null)
                {
                    Style style = styleObj as Style;

                    return style;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置RowStyle出错:" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///设置详细信息Grid样式
        /// </summary>
        /// <returns>style</returns>
        private Style SetRowHeaderStyle()
        {
            try
            {
                object styleObj = this.FindResource("rowDetailHeaderStyle");
                if (styleObj != null)
                {
                    Style style = styleObj as Style;

                    return style;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("设置RowHeaderStyle出错:" + ex.ToString());
                return null;
            }
        }

        #endregion

    }
}
