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
using System.Windows.Forms;
using System.IO;
using System.Threading;
using AFC.WS.Model.DB;


/*
 * 修改日期20101130 修改了显示皮肤自动切换报警提示信息文字。注释掉了GetWindow函数 edit by wangdx
 * 
 * 
 * */
namespace AFC.WS.UI.CoreUI
{
    using AFC.BOM2.UIController;
    using System.Windows.Media.Animation;
    using System.Runtime.InteropServices;
    using System.Data;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    using AFC.WS.Model.Comm;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.WS.UI.DataSources;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.UIPage.DeviceMonitor;

    /// <summary>
    /// StatusBar.xaml 的交互逻辑
    /// </summary>
    public partial class StatusBar : UserControlBase
    {
        ///// <summary>
        ///// timer线程
        ///// </summary>
        //System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        ///// <summary>
        ///// 报警提示
        ///// </summary>
        //private int alarmTipItem=0;

        ///// <summary>
        ///// 默认发送时间间隔
        ///// </summary>
        //private int sendTime = 60000; 

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusBar()
        {
            InitializeComponent();
            this.rootLayout.DataContext = AFC.WS.BR.BuinessRule.GetInstace().brConext;
            //this.alarmMessageGif.Visibility = Visibility.Collapsed;
            //this.alarmMessageImage.Visibility = Visibility.Visible;
            timer.Interval = 10 * 1000;
            timer.Start();
            timer.Tick += delegate(object sender, EventArgs e)
            {
                this.labRunDate.Content = BuinessRule.GetInstace().rm.GetRunDate();//set run date
                bool res=AFC.WS.BR.BuinessRule.GetInstace().ConnectDB(AFC.WS.UI.Common.SysConfig.GetSysConfig().LocalParamsConfig.DBConnectionString);
                AFC.WS.BR.BuinessRule.GetInstace().brConext.DbOnLineStatus = res;
            };

            this.alarmMessageImage.MouseLeftButtonDown += new MouseButtonEventHandler(alarmMessageImage_MouseLeftButtonDown);
            this.alarmMessageCurrentImage.MouseLeftButtonDown += new MouseButtonEventHandler(alarmMessageCurrentImage_MouseLeftButtonDown);
            this.alarmMessageGif.MouseLeftButtonUp += new MouseButtonEventHandler(alarmMessageGif_MouseLeftButtonUp);
        }

        private void alarmMessageGif_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.alarmMessageGif.Visibility = Visibility.Collapsed;
            this.alarmMessageImage.Visibility = Visibility.Visible;
            this.alarmMessageCurrentImage.Visibility = Visibility.Visible;
            DataSourceManager.NotfiyDataSourceChange("ds_alarmMessageInfo");

            ShowAlarmMessageDialog();

        }

        private void alarmMessageImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataSourceManager.NotfiyDataSourceChange("ds_alarmMessageInfo");

            ShowAlarmMessageDialog();
        }

        private void alarmMessageCurrentImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowAlarmCurrentMessageDialog();
        }

        /// <summary>
        /// 显示报警消息对话框
        /// </summary>
        private void ShowAlarmMessageDialog()
        {

            ShowWindowAction action = new ShowWindowAction();
            action.Title = "当前未确认的设备报警信息查询";
            action.ucb = new DevCurrentStatusAlarmQuery();
            action.Width = 900;
            action.Height = 600;

            List<QueryCondition> list = new List<QueryCondition>();
            //list.Add(new QueryCondition { bindingData = "device_id", operation = OperationSymbols.Equal, value = info.device_id });
            action.DoAction(list);

            //AlarmWindow qa = AlarmWindow.GetAlarmWindow();
            //this.alarmMessageImage.Visibility = Visibility.Visible;
            //this.alarmMessageCurrentImage.Visibility = Visibility.Visible;
            //this.alarmMessageGif.Visibility = Visibility.Collapsed;

            //qa.Topmost = true;
            //Screen[] screens = Screen.AllScreens;
            //Screen screen = screens[0];//获取屏幕变量
            ////   qa.Top = 600;
            //qa.Left = screen.WorkingArea.Width - qa.Width;
            //qa.Show();
            //qa.AlarmWindowShowStyle();

        }

        /// <summary>
        /// 2013年5月30日根据马晓春要求增加显示当前设备报警及故障的显示对话框，形式同之前的设备历史报警的表示方式
        /// </summary>
        private void ShowAlarmCurrentMessageDialog()
        {
            ShowWindowAction action = new ShowWindowAction();
            action.Title = "当前设备的所有运行状态";
            action.ucb = new DevCurrentErrorStatusQuery();
            action.Width = 900;
            action.Height = 600;

            List<QueryCondition> list = new List<QueryCondition>();
            //list.Add(new QueryCondition { bindingData = "device_id", operation = OperationSymbols.Equal, value = info.device_id });
            action.DoAction(list);

        }

        public override void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this, "SatusBar", AsynMessageType.RunEnd, HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, "StatusBar", AsynMessageType.RunStart, HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, "SatusBar", AsynMessageType.AlarmStatusClose, HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, "StatusBar", AsynMessageType.AlarmStatusOpen, HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, "StatusBar", AsynMessageType.CommAsynMsg, HandleMode.Asyn, true);
            MessageManager.SubscribeMessage(this, "StatusBar", AsynMessageType.AlarmMonitor, HandleMode.Asyn, true);
        }

        public override void SubscribeSynMessage()
        {
            //MessageManager.SubscribeMessage(this, "SatusBar", AsynMessageType.RunEnd, HandleMode.Asyn, true);
            //MessageManager.SubscribeMessage(this, "StatusBar", AsynMessageType.RunStart, HandleMode.Asyn, true);
            //base.SubscribeSynMessage();
        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType ==AsynMessageType.RunStart ||
                msg.MessageType == AsynMessageType.RunEnd)
            {
                this.runStatus.Content = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
            }

            if (msg.MessageType == AsynMessageType.AlarmStatusOpen ||
             msg.MessageType == AsynMessageType.AlarmStatusClose)
            {
                this.alarmStatus.Content = BuinessRule.GetInstace().brConext.GetAlarmStatus();
            }

            if (msg.MessageType == AsynMessageType.AlarmMonitor)
            {
               // ShowAlarmMessageDialog();
                BeepSound();
                DataSourceManager.NotfiyDataSourceChange("ds_alarmMessageInfo");
                ShowAlarmMessageDialog();
            }

            #region 异步消息处理
            if (msg.MessageType == AsynMessageType.CommAsynMsg)
            {
                if (msg.MessageParam is DevStatus_1325)
                {
                    //MessageDialog.Show((msg.MessageParam as DevStatus_1325).ToString(),
                    //    "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    DevStatus_1325 status = msg.MessageParam as DevStatus_1325;
                    string statusLevel = string.Empty;
                    switch (status.devStatus)
                    {
                        case 0:
                            statusLevel = "正常";
                            break;
                        case 1:
                            statusLevel = "警告";
                            break;
                        case 2:
                            statusLevel = "报警";
                            break;
                        case 3:
                            statusLevel = "故障";
                            break;
                    }
                    for (int i = 0; i < status.devStatusInfo.Count; i++)
                    {
                        BasiStatusIdInfo bsi =
                            BuinessRule.GetInstace().GetBasiStatusIdInfo(
                                status.devStatusInfo[i].statusId.ToString("X4"),
                                status.devStatusInfo[i].statusValue.ToString("x2"));
                        AlarmMessage almsg = new AlarmMessage
                        {
                            alarmId = status.devStatusInfo[i].statusId.ToString(),
                            alarmValue = status.devStatusInfo[i].statusValue.ToString("x2"),
                            //alarmContent = status.headerData.deviceId.ToString("X8")+" "+bsi.css_status_id_name+":"+bsi.css_status_value_name+ "   报警级别:  " + statusLevel,
                            date = DateTime.Now.ToString("yyyyMMdd"),
                            time = DateTime.Now.ToString("HHmmss"),
                            //messageSource = status.headerData.deviceId.ToString("X8")
                        };

                        ErrorAlarm ea = new ErrorAlarm();
                        //todo:check alarm message 
                        string isDev = ea.GetStatusIsDev(bsi.css_status_id, bsi.css_status_value, almsg.messageSource);
                        if (isDev != "01")
                        {
                            if (AlarmWindow.GetAlarmWindow().list.Count < 100)
                            {
                                AlarmWindow.GetAlarmWindow().list.Insert(0, almsg);
                            }

                            else
                            {
                                var temp = AlarmWindow.GetAlarmWindow().list;

                                temp.Remove(temp[temp.Count - 1]);

                                AlarmWindow.GetAlarmWindow().list.Insert(0, almsg);

                            }
                        }

                        //Thread.Sleep(5000);
                      
                        //List<string> list = ea.GetAlarmStyle(status.devStatus.ToString("x2"));
                        string list = ea.GetStatusAlarmStyle(bsi.css_status_id, bsi.css_status_value);
                        if (list == null || isDev == "01")
                            return;

                        if (list.Contains("00"))//不报警
                        {
                            return;
                        }
                        //2013年6月14日，根据天津业主及马晓春要求，报警状态分为报警和不报警两个状态，在此基础上王冬欣提出报警中应该包含多种可选择形式，例如可以单独声音报警，也可以报警和弹框同时显示。
                        else if (list.Contains("6f"))
                        {
                            this.alarmMessageGif.Visibility = Visibility.Visible;
                            this.alarmMessageImage.Visibility = Visibility.Hidden;
                            this.alarmMessageCurrentImage.Visibility = Visibility.Hidden;
                         ShowAlarmMessageDialog();
                            BeepSound();
                        }
                        else if (list.Contains("01"))
                        {
                            BeepSound();
                        }
                        else if (list.Contains("64"))
                        {
                            this.alarmMessageGif.Visibility = Visibility.Visible;
                            this.alarmMessageImage.Visibility = Visibility.Hidden;
                            this.alarmMessageCurrentImage.Visibility = Visibility.Hidden;
                        }
                        else if (list.Contains("0a"))
                        {
                            ShowAlarmMessageDialog();
                        }
                        else if (list.Contains("65"))
                        {
                            this.alarmMessageGif.Visibility = Visibility.Visible;
                            this.alarmMessageImage.Visibility = Visibility.Hidden;
                            this.alarmMessageCurrentImage.Visibility = Visibility.Hidden;
                            BeepSound();
                        }
                        else if (list.Contains("0b"))
                        {
                            ShowAlarmMessageDialog();
                            BeepSound();
                        }
                        else if (list.Contains("6e"))
                        {
                            this.alarmMessageGif.Visibility = Visibility.Visible;
                            this.alarmMessageImage.Visibility = Visibility.Hidden;
                            this.alarmMessageCurrentImage.Visibility = Visibility.Hidden;
                            ShowAlarmMessageDialog();
                        }
                    }
                    return;
                }
            #endregion

             //AlarmWindow.GetAlarmWindow().list.Add(new AFC.WS.Model.Comm.AlarmMessage{ alarmContent}
            }
            
        }

        public override void InitControls()
        {
            this.labDeviceId.Content = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode; //set deviceId
            this.labRunDate.Content = BuinessRule.GetInstace().rm.GetRunDate();//set run date
            this.runStatus.Content = BuinessRule.GetInstace().brConext.GetCurrentStationRunStatus();
            this.alarmStatus.Content = BuinessRule.GetInstace().brConext.GetAlarmStatus();
            //todo: Get run status from db
            //todo: Get version info from db
            //base.InitControls();

            this.labVerNum.Content = BuinessRule.GetInstace().GetCurrentVersionPara().para_version;

            string futureNo=string.Empty;
            string futureDate=string.Empty;

            if (!BuinessRule.GetInstace().paraManager.IsExistNewVersion(out futureNo, out futureDate))
            {
                this.labVerWarn.Content = "当前无新版本";
            }
            else
            {
                this.labVerWarn.Foreground = Brushes.Red;
                this.labVerWarn.Content = string.Format("存在将来版本,版本号{0}", futureNo);
                this.labVerWarn.ToolTip = this.labVerWarn.Content + ",生效日期 " + futureDate;
            }
        }

        public override void UnLoadControls()
        {
            MessageManager.CancelAllSubscribeMessage(AsynMessageType.RunEnd);
        }




        /// <summary>
        ///　引用蜂鸣声音函数。
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")] //导入beep函数 
        public static extern bool Beep(int frequency, int duration);

        /// <summary>
        /// 发出蜂鸣声。
        /// </summary>
        public void BeepSound()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                bool BeepSign = true;
                while (BeepSign)
                {
                    //2013年6月14日从2200变更为1000
                    Beep(2200, 50);
                    System.Threading.Thread.Sleep(100);
                    end = DateTime.Now;
                    if (end.Subtract(start).TotalSeconds > 5)
                        BeepSign = false;
                }
            }));
            thread.Start();
        }

   
    }
}
