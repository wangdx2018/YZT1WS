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

namespace AFC.WS.UI.UIPage.ModeManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Comm;
    using AFC.WS.Model.Const;
    using AFC.WS.UI.CommonControls;
   
    /// <summary>
    /// ModeSetting.xaml 的交互逻辑
    /// 
    /// edit by wangdx 20110622 在模式控制中将车站ID改成了车站服务器设备ID
    /// 
    /// 20110622绑定UI服务模式
    /// </summary>
    public partial class ModeSetting : UserControlBase
    {
        public ModeSetting()
        {
            InitializeComponent();
            this.rb24run.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbModeDate.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbModeEntry.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbModeExit.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbModeFare.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbModeTime.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbModeTrouble.Checked += new RoutedEventHandler(SubModeCheck);
            this.rbClose.Checked += new RoutedEventHandler(SubModeCheck);

            this.rbDownEmergencyMode.Checked += new RoutedEventHandler(MainModeCheck);
            this.rbEmergencyMode.Checked += new RoutedEventHandler(MainModeCheck);
            this.rbNormalMode.Checked += new RoutedEventHandler(MainModeCheck);

            this.cbbStation.SelectionChanged += new SelectionChangedEventHandler(cbbStation_SelectionChanged);
            this.cbbLine.SelectionChanged += new SelectionChangedEventHandler(cbbLine_SelectionChanged);

        }

        private void cbbStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb=sender as ComboBox;
            BasiStationInfo bsif = cb.SelectedItem as BasiStationInfo;
            if (bsif != null)
            {
                this.modeStationId = bsif.device_id.ToString().ConvertHexStringToUint();
            }
         
        }

        private uint modeCode=uint.MaxValue;

        private uint modeStationId;


        public override void InitControls()
        {
            #region set current Mode
            this.lblCurrentMode.Content = BuinessRule.GetInstace().GetCurrentMode();

            if (this.lblCurrentMode.Content.ToString().Contains("正常"))
            {
                this.lblCurrentMode.Foreground = Brushes.Green;
            }
            else
            {
                this.lblCurrentMode.Foreground = Brushes.Red;
            }
            #endregion

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
              
                BasiStationInfo bsi = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);

                this.grpStationSelect.Visibility = Visibility.Collapsed;
                this.modeStationId=bsi.device_id.ConvertHexStringToUint();

            }
            else
            {
                BasiLineIdInfo bli = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
                List<BasiLineIdInfo> list = new List<BasiLineIdInfo>();
                list.Add(bli);
                this.cbbLine.ItemsSource = list;
                this.cbbLine.DisplayMemberPath = "line_name";
                this.cbbLine.SelectedIndex = 0;
                this.cbbLine.CanReadOnly = true ;

                this.cbbStation.ItemsSource = BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
                this.cbbStation.DisplayMemberPath = "station_cn_name";
                this.cbbStation.SelectedIndex = 0;


                this.gridMode.Visibility = Visibility.Collapsed;
            }
        }

        private void SubModeCheck(object sender, RoutedEventArgs e)
        {
           this.modeCode= (sender as RadioButton).Tag.ToString().ConvertNumberStringToUint();
        }

       private void SetRadBtnValue(bool value)
       {
           foreach (var temp in (gbDown.Content as Grid).Children)
           {
               if (temp is RadioButton)
               {
                   (temp as RadioButton).IsChecked = value;
               }
           }
       }

       private void MainModeCheck(object sender, RoutedEventArgs e)
       {
           RadioButton rb = sender as RadioButton;
           if (rb == null)
               return;
           switch (rb.Name)
           {
               case "rbEmergencyMode":
               case "rbNormalMode":
                   SetRadBtnValue(false);
                   this.gbDown.IsEnabled = false;
                     this.modeCode= (sender as RadioButton).Tag.ToString().ConvertNumberStringToUint();
    
                   break;
               case "rbDownEmergencyMode":
                   //SetRadBtnValue(true);
                   this.gbDown.IsEnabled = true;
                 //    this.modeCode= (sender as RadioButton).Tag.ToString().ConvertNumberStringToUint();
    
                   break;
           }
       }

        private void btuReFresh_Click(object sender, RoutedEventArgs e)
        {
            SetRadBtnValue(false);
            this.gbDown.IsEnabled = false;
            this.rbDownEmergencyMode.IsChecked = false;
            this.rbEmergencyMode.IsChecked = false;
            this.rbNormalMode.IsChecked = false;
            this.modeCode =uint.MaxValue;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.modeCode == uint.MaxValue)
            {
                MessageDialog.Show("请选择设置的模式", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "stationId", value = this.modeStationId });
            list.Add(new QueryCondition { bindingData = "modeCode", value = this.modeCode });
            dpaction.subAction = new AFC.WS.ModelView.Actions.ModeActions.SetModeAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            if (dpaction.CheckValid(list))
            {
               ResultStatus res= dpaction.DoAction(list);
               if (res.resultCode == 0 &&
                   res.resultData.ToString() == "0")
               {
                   MessageManager.SendMessasge(new Message { MessageType = SynMessageType.Mode_Change });
                   this.lblCurrentMode.Content = BuinessRule.GetInstace().GetCurrentMode();
                   if (this.lblCurrentMode.Content.ToString().Contains("正常"))
                   {
                       this.lblCurrentMode.Foreground = Brushes.Green;
                   }
                   else
                   {
                       this.lblCurrentMode.Foreground = Brushes.Red;
                   }
               }
            }
        }

        private void cbbLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            BasiLineIdInfo blid = cb.SelectedItem as BasiLineIdInfo;
            if (blid == null)
                return;
            string lineId = blid.line_id;
            this.cbbStation.ItemsSource = BuinessRule.GetInstace().GetAllStationInfo(lineId);
        }
    }
}
