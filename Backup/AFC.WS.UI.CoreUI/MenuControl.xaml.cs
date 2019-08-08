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
using System.Windows.Media.Animation;
using System.Collections.Specialized;
using System.IO;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.UI.CoreUI
{
    using AFC.BOM2.UIController;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    using AFC.WS.ModelView;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using Microsoft.VisualBasic.Devices;
    
    
    /// <summary>
    /// 主菜单的定义，added by wangdx
    /// </summary>
    public partial class MenuControl : UserControlBase
    {
        private Storyboard perChar = new Storyboard();

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        //--->构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MenuControl()
        {
            InitializeComponent();

          
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        private void StartStoryBoard()
        {

            _text.TextEffects = new TextEffectCollection();
            for (int i = 0; i < _text.Text.Length; i++)
            {
                TextEffect effect = new TextEffect();
                effect.Transform = new TranslateTransform();
                effect.PositionStart = i;
                effect.PositionCount = 1;
                _text.TextEffects.Add(effect);
                DoubleAnimation anim = new DoubleAnimation();
                anim.To = 5;
                anim.AccelerationRatio = .5;
                anim.DecelerationRatio = .5;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                anim.Duration = TimeSpan.FromSeconds(1);
                anim.BeginTime = TimeSpan.FromMilliseconds(250 * i);
                Storyboard.SetTargetProperty(anim,
                new PropertyPath("TextEffects[" + i + "].Transform.Y"));
                Storyboard.SetTargetName(anim, _text.Name);
                perChar.Children.Add(anim);
            }
            perChar.Begin(this);
        }

     
       

      

       
        //--->创建打开菜单
        /// <summary>
        /// 创建打开的主菜单
        /// </summary>
        private void BuildOpenMenu()
        {
            List<Function> funcList = AFC.WS.ModelView.FunctionDataCollection.GetFunctionDataCollection().GetMainFunctionList();

      
            if (funcList != null)
            {
           

                // Add the recent files to the menu as menu items
                if (funcList.Count > 0)
                {
                    foreach (Function funcItem in funcList)
                    {
                       
                        if (funcItem.Childern.Count(temp => 
                            temp.sysFunctionId.Equals(FunctionDataCollection.totalAllPrimissionFunction)) > 0)
                        {
                            MenuItem item = new MenuItem();
                            //item.Style = SetStyle("MenuItemStyle");
                            item.Header = funcItem.text;
                            item.CommandParameter = funcItem;
                            AddChildFunction(item);
                            this.mainMenu.Items.Add(item);
                            this.mainMenu.Items.Add(new Separator());
                        }
                    }
                }
            }
        }

        //--->设置皮肤的key
        /// <summary>
        /// 设置皮肤样式
        /// </summary>
        /// <param name="key">皮肤Style的key</param>
        /// <returns></returns>
        private Style SetStyle(string key)
        {
            try
            {
                object obj = this.FindResource(key);
                return obj != null ? obj as Style : null;
            }
            catch
            {
                return null;
            }
        }

        //--->增加功能子菜单
        /// <summary>
        /// 增加子功能。
        /// </summary>
        /// <param name="expander"></param>
        /// <returns></returns>
        private void AddChildFunction(MenuItem itemFunction)
        {
            Function item = (Function)itemFunction.CommandParameter;
            // itemFunction.Items.Add(new Separator()); 
            foreach (Function functionItem in item.Childern)
            {

                MenuItem childItem = new MenuItem();
                
                childItem.Header = functionItem.text;

               // childItem.Style = SetStyle("MenuItemStyle");

                childItem.Click += new RoutedEventHandler(childItem_Click);

                childItem.CommandParameter = functionItem;

                childItem.FontSize = 13;

                itemFunction.Items.Add(childItem);
            }
        }

        //--->增加功能子菜单
        /// <summary>
        /// 增加子功能。
        /// </summary>
        /// <param name="expander"></param>
        /// <returns></returns>
        private void AddChildFunctions(MenuItem itemFunction)
        {
            List<Function> item = (List<Function>)(itemFunction.CommandParameter);
            //  itemFunction.Items.Add(new Separator()); 
            foreach (Function functionItem in item)
            {

                MenuItem childItem = new MenuItem();

                childItem.Header = functionItem.text;

               // childItem.Style = SetStyle("MenuItemStyle");

                childItem.Click += new RoutedEventHandler(childItem_Click);

                childItem.CommandParameter = functionItem;

                childItem.FontSize = 13;

                itemFunction.Items.Add(childItem);
            }
        }

        //--->子菜单功能的单击事件
        /// <summary>
        /// 子菜单单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void childItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;

            if (mi.CommandParameter != null)
            {
                Function item = mi.CommandParameter as Function;
                Message msg = new Message();
                msg.Content = item.buinessControlId;
                msg.MessageType = SynMessageType.SwichPage;
                msg.MessageParam = item;
                MessageManager.SendMessasge(msg);
                HandleMenuClicked(item);
            }
        }

        //--->创建更改皮肤菜单
        /// <summary>
        /// Builds the Skins Menu
        /// </summary>
        private void BuildSkinsMenu()
        {
            List<StyleInfo> list = GetAllStyleInfo();
            SkinsMenu.Items.Clear();
            if (list != null && list.Count > 0)
            {
                foreach (var style in list)
                {
                    MenuItem skin = new MenuItem();
                    skin.Style = SetStyle("MenuItemStyle");
                    skin.Header = style.fileName.Split('.')[0];
                    skin.Click += new RoutedEventHandler(skin_Click);
                    SkinsMenu.Items.Add(skin);
                    skin.Tag = style.path;
                }
            }
        }

        //-->皮肤文件菜单单击事件
        /// <summary>
        /// 皮肤更换文件内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem item = sender as MenuItem;
                if (item != null)
                {
                    UIController.GetControllerInstance().ApplySkinFile(item.Tag.ToString(), Application.Current, true);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        //--->界面加载
        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.hiddenNavigator.Visibility = Visibility.Collapsed;
            this.ShowNavigator.Visibility = Visibility.Collapsed;
            this.miExitToLogonUI.Visibility = Visibility.Collapsed;
            //try
            //{
                //BuildOpenMenu();
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Equals("LCWS"))
            {
                this.labMode.Visibility = Visibility.Collapsed;
                this.labModeTip.Visibility = Visibility.Collapsed;
                this.labSplit.Visibility = Visibility.Collapsed;
            }
                BuildSkinsMenu();
        }

        //--->覆盖父类的方法来订阅同步消息
        /// <summary>
        /// 订阅同步消息
        /// </summary>
        public override void SubscribeSynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.MainMenu, SynMessageType.Mode_Change, HandleMode.Syn, true);
            //MessageManager.SubscribeMessage(this, MessageSubscribeId.MainMenu, SynMessageType.MainMenuChanged, HandleMode.Syn, true);
            //MessageManager.SubscribeMessage(this, MessageSubscribeId.MainMenu, SynMessageType.Change_Menu_Operator, HandleMode.Syn, true);
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.MainMenu, SynMessageType.CreateNavigatorList, HandleMode.Syn, true);

            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.MainMenu, SynMessageType.NavigationSelection, HandleMode.Syn, true);
        }

        //--->取消订阅导航菜单的单击事件
        /// <summary>
        /// 覆盖父类的方法来取消订阅同步消息
        /// </summary>
        public override void CancleSubscribeSynMessage()
        {
           MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.MainMenu, SynMessageType.Mode_Change);
            //MessageManager.CancelSubscribeMessage(MessageSubscribeId.MainMenu, SynMessageType.MainMenuChanged);
            //MessageManager.CancelSubscribeMessage(MessageSubscribeId.MainMenu, SynMessageType.Change_Menu_Operator);
           MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.MainMenu, SynMessageType.CreateNavigatorList);
           MessageManager.CancelSubscribeMessage(SynMessageSubscribeId.MainMenu, SynMessageType.NavigationSelection);
        }

        //--->处理同步消息
        /// <summary>
        /// 处理同步消息
        /// </summary>
        /// <param name="msg">消息体来处理同步消息</param>
        public override void HandleSynMessage(Message msg)
        {
            if (msg.MessageType == SynMessageType.Mode_Change)
            {
                this.labMode.Content = BuinessRule.GetInstace().GetCurrentMode();
                if (this.labMode.Content.ToString().Contains("正常"))
                {
                    this.labMode.Foreground = Brushes.Green;
                }
                else
                {
                    this.labMode.Foreground = Brushes.Red;
                }
            }


            #region 导航切换处理
            if (msg.MessageType == SynMessageType.NavigationSelection)
            {
                if (msg.Content.ToString() != "Welcome")
                {
                    object id = msg.Content;
                    uint functionId = 0;
                    bool res = uint.TryParse(id.ToString(), out functionId);
                    if (res)
                    {
                         HandleMenuClicked(FunctionDataCollection.GetFunctionDataCollection().GetFunctionById(functionId));
                    }
                }
                else
                {
                    if (msg.MessageParam == null)
                    {
                        this.labMap.Content = "欢迎";
                        this.hiddenNavigator.Visibility = Visibility.Visible;//设置菜单的样式为Visible。
                        this.miExitToLogonUI.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.labMap.Content = "登录";
                        
                        MenuItem item = this.NewMenu;
                        this.hiddenNavigator.Visibility = Visibility.Collapsed;
                        this.ShowNavigator.Visibility = Visibility.Collapsed;
                        this.miExitToLogonUI.Visibility = Visibility.Collapsed;
                        this.mainMenu.Items.Clear();
                        this.mainMenu.Items.Add(item);
                    }
                }
            }
            #endregion





            #region 根据权限创建菜单
            if (msg.MessageType == SynMessageType.CreateNavigatorList)
            {
                Dictionary<Function, List<Function>> dict =FunctionDataCollection.GetFunctionDataCollection().GetParentFunctionCollection(msg.Content as List<string>);
                List<Function> funcList = dict.Select(temp => temp.Key).ToList();

                MenuItem firstItem = this.mainMenu.Items[0] as MenuItem;

                this.mainMenu.Items.Clear();

                this.mainMenu.Items.Add(firstItem);

                if (funcList != null)
                {
                    // Clear existing menu items
                    // OpenMenu.Items.Clear();

                    // Add the recent files to the menu as menu items
                    if (funcList.Count > 0)
                    {
                        foreach (var funcItem in funcList)
                        {

                            MenuItem item = new MenuItem();
                            item.Header = funcItem.text;
                            item.CommandParameter = dict[funcItem];
                            AddChildFunctions(item);
                            this.mainMenu.Items.Add(item);
                            this.mainMenu.Items.Add(new Separator());

                        }
                    }
                }
            }
            #endregion
        }

        //--->处理菜单的单击事件
        /// <summary>
        /// 处理菜单单击事件
        /// </summary>
        /// <param name="functionId">功能的编号</param>
        private void HandleMenuClicked(Function fun)
        {
           
            string subText = fun.text;
            uint parentId = fun.parentId;
            Function parent = AFC.WS.ModelView.FunctionDataCollection.GetFunctionDataCollection().GetFunctionById(parentId);
            if (parent != null)
            {
                string mainText = parent.text;
                labMap.Content = mainText + "——>" + subText;
            }
            //}
        }

        //--->得到对应的皮肤文件
        /// <summary>
        /// 得到皮肤的路径代码
        /// </summary>
        /// <returns></returns>
        private List<StyleInfo> GetAllStyleInfo()
        {
            List<StyleInfo> list = new List<StyleInfo>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(@".\Style");
                if (di != null)
                {
                    foreach (var temp in di.GetFiles())
                    {
                        StyleInfo styleInfo = new StyleInfo();
                        styleInfo.fileName = temp.Name;
                        styleInfo.path = di.Name + "\\" + temp.Name;
                        list.Add(styleInfo);
                    }
                    return list;
                }
                return null;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("Can't find the directory path[@.\\style]");
                return null;
            }

        }

        private DateTime lastCheckTime=DateTime.Now;

        //--->初始化菜单
        /// <summary>
        /// 初始化菜单
        /// </summary>
        public override void InitControls()
        {
            timer.Tick += new EventHandler(delegate(object sender, EventArgs e)
            {
                this.labTime.Content = DateTime.Now.ToString("yyyy 年 MM 月 dd 日 HH:mm:ss");

                if (DateTime.Now.Subtract(lastCheckTime).TotalSeconds >= 10)
                {
                    Computer localMachine = new Computer();
                    BuinessRule.GetInstace().brConext.NetworkStatus = localMachine.Network.IsAvailable;

                    BuinessRule.GetInstace().brConext.DbOnLineStatus =
                        BuinessRule.GetInstace().ConnectDB(SysConfig.GetSysConfig().LocalParamsConfig.DBConnectionString);
                    this.labMode.Content = BuinessRule.GetInstace().GetCurrentMode();
                    if (this.labMode.Content.ToString().Contains("正常"))
                    {
                        this.labMode.Foreground = Brushes.Green;
                        
                    }
                    else
                    {
                        this.labMode.Foreground = Brushes.Red;
                    }
                    lastCheckTime = DateTime.Now;
                }

            });
            this.timer.Interval = 1000;
            timer.Start();

            //begin storyboard
            StartStoryBoard();

            //set station Id
            SysConfig config = SysConfig.GetSysConfig();
            string stationId = config.LocalParamsConfig.StationCode;
            AFC.WS.Model.DB.BasiStationInfo info = BuinessRule.GetInstace().GetStationInfoById(stationId);
            this.labStation.Content = info.station_cn_name;
            //set system name
            this.labSystemTypeContent.Content = config.LocalParamsConfig.SystemName;

            this.labOperator.DataContext = BuinessRule.GetInstace().brConext;

            this.labMode.Content = BuinessRule.GetInstace().GetCurrentMode();
        }

        /// <summary>
        /// 隐藏或显示导航
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void hiddenNavigator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem item = sender as MenuItem;
                if (item.Name == "hiddenNavigator")
                {
                    UIController.GetControllerInstance().SettingCommonControlVisable("CurrentFunction", false);
                    this.hiddenNavigator.Visibility = Visibility.Collapsed;
                    this.ShowNavigator.Visibility = Visibility.Visible;
                }
                else
                {
                    UIController.GetControllerInstance().SettingCommonControlVisable("CurrentFunction", true);
                    this.hiddenNavigator.Visibility=Visibility.Visible;
                    this.ShowNavigator.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 打印当前窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {
           AFC.WS.UI.Common.ScreenPrint.Instance.BeginScreenshots();
        }

        /// <summary>
        /// 弹出关于窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenu_Click(object sender, RoutedEventArgs e)
        {
            BaseWindow bw = new BaseWindow();

            AboutForm af = new AboutForm(bw);
            bw.Content =af;
            bw.Width = 300;
            bw.Height = 280;
            bw.MinHeight = bw.Height;
            bw.MinWidth = bw.Width;
            bw.MaxHeight = bw.Height;
            bw.MaxWidth = bw.Width;

            bw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bw.ShowDialog();
        }

        /// <summary>
        /// 退出到登入界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miExitToLogonUI_Click(object sender, RoutedEventArgs e)
        {
            AFC.WS.ModelView.Actions.PrimissionActions.LogoutAction logout = new AFC.WS.ModelView.Actions.PrimissionActions.LogoutAction();
            logout.DoAction(null);
        }

        /// <summary>
        /// 关闭应用程序。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    string clientName = BRContext.Instance.GetClientName();
            //    if (Wrapper.ShowDialog("您确定要关闭" + clientName + "吗？", false) == MessageBoxResult.Yes)
            //    {
            //        int result = BRContext.CreateCommProcess.LogOff(BRContext.OperatorID);
            //        if (result != 0)
            //        {
            //            Wrapper.Instance.ConsoleWriteLine("系统关闭失败，请重试！", LogFlag.ErrorFormat);
            //            WriteLog.Log_Info("系统登出请求失败，失败原因是" + BRContext.Instance.MessageContent(result));
            //        }
            //        Environment.Exit(0);
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            //}
        }

        private void SysConfigRewrite_Click(object sender, RoutedEventArgs e)
        {
            BaseWindow bw = new BaseWindow();

            SysConfigRewrite sys = new SysConfigRewrite(bw);
            bw.Content = sys;
            bw.Width = 430;
            bw.Height = 400;
            bw.MinHeight = bw.Height;
            bw.MinWidth = bw.Width;
            bw.MaxHeight = bw.Height;
            bw.MaxWidth = bw.Width;

            bw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bw.ShowDialog();
        }

    }

    //-->样式资源文件实体类
    /// <summary>
    /// 样式资源文件实体类，内部类
    /// </summary>
    internal class StyleInfo
    {
        //--->文件名
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName;

        //-->全路径名
        /// <summary>
        /// 全路径名
        /// </summary>
        public string path;
    }
}