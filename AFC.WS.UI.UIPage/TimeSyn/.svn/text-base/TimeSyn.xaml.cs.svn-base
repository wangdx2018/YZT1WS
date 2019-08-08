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

namespace AFC.WS.UI.UIPage.TimeSyn
{
    using AFC.BOM2.UIController;
    using AFC.WS.ModelView.Actions.TimeSyn;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.CommonActions;
    /// <summary>
    /// TimeSyn.xaml 的交互逻辑
 
    /// </summary>
    public partial class TimeSyn : UserControlBase
    {
        public TimeSyn()
        {
            InitializeComponent();
            this.radioTimeSys.Checked += new RoutedEventHandler(RadioBtnChecked);
            this.radionHandTimeSys.Checked += new RoutedEventHandler(RadioBtnChecked);
         
        }

        public override void InitControls()
        {
            string date=DateTime.Now.ToString("yyyy年MM月dd日");
            this.dateTime.SetControlValue(date);
            this.timePicker.SelectedTime = DateTime.Now.TimeOfDay;
            this.radioTimeSys.IsChecked = true;
            this.dateTime.FormatDateTime = "yyyyMMdd";
        }

       private void RadioBtnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null)
                return;
            if (rb.Name.Equals("radioTimeSys"))
            {
                this.timePicker.IsEnabled = !rb.IsChecked.Value;
                this.dateTime.IsEnabled = !rb.IsChecked.Value;
            }
            else
            {
                this.timePicker.IsEnabled = rb.IsChecked.Value;
                this.dateTime.IsEnabled = rb.IsChecked.Value;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
           
                object date=this.dateTime.GetControlValue();
                WriteLog.Log_Info("current date="+date.ToString());
                object time = this.timePicker.GetControlValue();
                WriteLog.Log_Info("current time="+date.ToString());
                DateTime dt=new DateTime();
            
                bool res= DateTime.TryParseExact(date.ToString()+time.ToString(),"yyyyMMddHH:mm:ss",null,System.Globalization.DateTimeStyles.None,out dt);
                uint currentTime = 0;
                if (res)
                {
                    currentTime = (uint)dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds-8*60*60;
                    IAction action = new TimeSynAction();
                    List<QueryCondition> list = new List<QueryCondition>();
                    list.Add(new QueryCondition { bindingData = "isForce", value = radioTimeSys.IsChecked.Value });
                    list.Add(new QueryCondition { bindingData = "currentTime", value = currentTime });

                    DoublePrimissionAction dpaction = new DoublePrimissionAction();
                    dpaction.subAction = action;
                    dpaction.CurrentOperationId = AFC.WS.BR.BuinessRule.GetInstace().brConext.CurrentOperatorId;
                    if (dpaction.CheckValid(list))
                    {
                        dpaction.DoAction(list);
                    }
                }
                else
                {
                    WriteLog.Log_Error("格式转换错误!");

                }
                
           
            
        }
    }
}
