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

namespace AFC.WS.UI.CoreUI
{
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.Comm;
    /// <summary>
    /// AlarmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmWindow :Window,IDataGridSelectionChangeHandle
    {
        public List<AlarmMessage> list = new List<AlarmMessage>();

        public AlarmWindow():base()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Top = 800;
            AFC.WS.UI.Config.DataListRule dlr = AFC.WS.UI.Config.Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_alarm_message_info.xml");
            this.dl.isExistFirstAndLastPage = false;
            this.dl.isExistGoPage = false;
            this.dl.iSelectionChanged = this;
            this.dl.Initliaize(dlr, list);
            this.dl.SetSelectionIndex(0);

  

            this.Loaded += new RoutedEventHandler(AlarmWindow_Loaded);
            
            this.ResizeMode = ResizeMode.NoResize;

          
        //    BindingSelectionChoice();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ((Storyboard)this.Resources["storyBoardY"]).Begin(this);   
            
            e.Cancel = true;
           alarmWindow.Visibility=Visibility.Hidden;
        }

        private void AlarmWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.dl.SetSelectionIndex(0);
            ((Storyboard)this.Resources["myStoryBoardX"]).Begin(this);   
        }


        /// <summary>
        /// 报警消息对象
        /// </summary>
        public AlarmMessage alarmMesage = new AlarmMessage();

    
    

        public static AlarmWindow alarmWindow = null;

        public static AlarmWindow GetAlarmWindow()
        {
            if (alarmWindow == null)
                alarmWindow = new AlarmWindow();
            if (alarmWindow.Visibility == Visibility.Hidden)
            {
                alarmWindow.Visibility = Visibility.Visible;
            }
            return alarmWindow;
        }

        public void AlarmWindowShowStyle()
        {
            ((Storyboard)this.Resources["myStoryBoardX"]).Begin(this);   
        }


        #region IDataGridSelectionChangeHandle 成员

        public void HandleChange(List<QueryCondition> list)
        {
           // this.btnUpdateAlarmStyle.IsEnabled = true;

            this.alarmMesage.alarmId = list.Single(temp => temp.bindingData.Equals("alarmId")).value.ToString();
            this.labAlarmId.Content = ushort.Parse(this.alarmMesage.alarmId).ToString("X4");

            this.alarmMesage.alarmValue = list.Single(temp => temp.bindingData.Equals("alarmValue")).value.ToString();
            this.labAlarmValue.Content = this.alarmMesage.alarmValue;

            this.alarmMesage.messageSource = list.Single(temp => temp.bindingData.Equals("messageSource")).value.ToString();
            this.labAlarmSource.Content = this.alarmMesage.messageSource;
          
            this.alarmMesage.alarmContent=list.Single(temp => temp.bindingData.Equals("alarmContent")).value.ToString();
            this.txtAlarmContent.Text = this.alarmMesage.alarmContent;

            this.alarmMesage.date=list.Single(temp => temp.bindingData.Equals("date")).value.ToString();
            string date = this.alarmMesage.date;

            this.alarmMesage.time=list.Single(temp => temp.bindingData.Equals("time")).value.ToString();
            string time = this.alarmMesage.time;

            this.labAlarmTime.Content = DateTime.ParseExact(date,"yyyyMMdd",null).ToString("yyyy-MM-dd") + " "+time.ToString().Substring(0,2)+":"+time.ToString().Substring(2,2)+":"+time.ToString().Substring(4,2);

            this.alarmMesage.HandleMessagePageName = list.Single(temp => temp.bindingData.Equals("HandleMessagePageName")).value.ToString();

            this.alarmMesage.pageParams = list.Single(temp => temp.bindingData.Equals("pageParams")).value;
            
            if ( string.IsNullOrEmpty(this.alarmMesage.HandleMessagePageName))
            {
                //this.txtHyLink.Visibility = Visibility.Hidden;
                //this.hyPageIndex.IsEnabled = false;
            }
            else
            {
                //this.txtHyLink.Visibility = Visibility.Visible;
                //this.hyPageIndex.ToolTip = this.alarmMesage.HandleMessagePageName;
                //this.hyPageIndex.IsEnabled = true;
            }

           // this.BindingSelectionChoice();
        }

        #endregion


    

       



        
    }
}
