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

namespace AFC.WS.UI.UIPage.SLEMonitor
{
    using AFC.BOM2.UIController;
    using AFC.WS.ModelView.Actions.DeviceMonitor;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.WS.UI.Common;

    /// <summary>
    /// AlarmSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmSetting : UserControlBase
    {
        /// <summary>
        /// 报警类型
        /// </summary>
        private string alarmType;

        /// <summary>
        /// 报警方式
        /// </summary>
        private List<string> alarmStyle = new List<string>();

        public AlarmSetting()
        {
            InitializeComponent();
            this.cbDialogAlarm.Checked += new RoutedEventHandler(CBCheck);
            this.cbImageTwinkle.Checked += new RoutedEventHandler(CBCheck);
            this.cbNoAlarm.Checked += new RoutedEventHandler(CBCheck);
            this.cbSoundAlarm.Checked += new RoutedEventHandler(CBCheck);

            this.cbDialogAlarm.Unchecked += new RoutedEventHandler(CBUnCheck);
            this.cbImageTwinkle.Unchecked += new RoutedEventHandler(CBUnCheck);
            this.cbNoAlarm.Unchecked += new RoutedEventHandler(CBUnCheck);
            this.cbSoundAlarm.Unchecked += new RoutedEventHandler(CBUnCheck);
            this.cmbErrorLevel.SelectionChanged += new SelectionChangedEventHandler(cmbErrorLevel_SelectionChanged);
            this.btnOk.Click += new RoutedEventHandler(btnOk_Click);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.alarmStyle = GetSelectItem();
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "alarmType", value = alarmType });
            list.Add(new QueryCondition { bindingData = "alarmStyle", value = this.alarmStyle });
            IAction action = new AlarmSettingAction();
            if (action.CheckValid(list))
            {
                action.DoAction(list);
            }
            
        }

        private void cmbErrorLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            ComboBoxItem cbItem=cb.SelectedItem as ComboBoxItem;
            alarmType = cbItem.Tag.ToString();
            AFC.WS.BR.SLEMonitorManager.ErrorAlarm ea = new ErrorAlarm();
            List<string> list = ea.GetAlarmStyle(alarmType);
            this.UpdateSelectedStatus();
            BindingData(list);
        }

        private List<string> GetSelectItem()
        {
            List<string> list = new List<string>();
            if (this.cbDialogAlarm.IsChecked.Value)
            {
                list.Add(cbDialogAlarm.Tag.ToString());
            }
            if (this.cbImageTwinkle.IsChecked.Value)
            {
                list.Add(cbImageTwinkle.Tag.ToString());
            }
            if (this.cbSoundAlarm.IsChecked.Value)
            {
                list.Add(cbSoundAlarm.Tag.ToString());
            }
            if (this.cbNoAlarm.IsChecked.Value)
            {
                list.Add(cbNoAlarm.Tag.ToString());
            }
            return list;
        }

        private void BindingData(List<string> list)
        {
            if(list==null||list.Count==0)
                return;
            foreach (var temp in list)
            {
                if (temp.Equals("00"))
                {
                    this.cbNoAlarm.IsChecked = true;
                    this.cbImageTwinkle.IsChecked=false;
                    this.cbSoundAlarm.IsChecked=false;
                    this.cbDialogAlarm.IsChecked=false;
                    break;
                }
                if (temp.Equals("01"))
                {
                    this.cbImageTwinkle.IsChecked = true;
                    this.cbNoAlarm.IsChecked = false;
                }
                if (temp.Equals("02"))
                {
                    this.cbDialogAlarm.IsChecked = true;
                    this.cbNoAlarm.IsChecked = false;
                }
                if (temp.Equals("03"))
                {
                    this.cbSoundAlarm.IsChecked = true;
                    this.cbNoAlarm.IsChecked = false;
                }
            }
        }

        private void UpdateSelectedStatus()
        {
            this.cbSoundAlarm.IsChecked = false;
            this.cbNoAlarm.IsChecked = false;
            this.cbImageTwinkle.IsChecked = false;
            this.cbDialogAlarm.IsChecked = false;
        }

        private  void CBUnCheck(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Name.Equals("cbNoAlarm"))
            {
                return;
            }
            if (!this.cbDialogAlarm.IsChecked.Value ||
                !this.cbImageTwinkle.IsChecked.Value ||
                !this.cbSoundAlarm.IsChecked.Value)
            {
                this.cbNoAlarm.IsChecked= false;
            }   
        }

        public override void InitControls()
        {
            //base.InitControls();
        }

        private void CBCheck(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Name == "cbNoAlarm")
            {
                this.cbDialogAlarm.IsChecked = false;
                this.cbImageTwinkle.IsChecked = false;
                this.cbSoundAlarm.IsChecked = false;
                this.cbNoAlarm.IsChecked = true;
            }
            else
            {
                this.cbNoAlarm.IsChecked = false;
            }
        }

        public override void UnLoadControls()
        {
            UpdateSelectedStatus();
        }
    }
}
