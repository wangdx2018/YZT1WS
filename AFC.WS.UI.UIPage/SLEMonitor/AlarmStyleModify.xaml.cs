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
using AFC.BOM2.UIController;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;
using AFC.WS.Model.Const;

namespace AFC.WS.UI.UIPage.SLEMonitor
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmStyleModify : UserControlBase
    {
        public AlarmStyleModify()
        {
            InitializeComponent();
            this.btnOk.Click += new RoutedEventHandler(btnOk_Click);
            
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            //:todo 01:get statusId,statusValue
            //todo: 02:call GetCurerntSelect function
             // todo: call action function handle db operation
            try
            {
                var listParams = this.Tag as List<QueryCondition>;
            

                
                string statusId = listParams.Single(temp => temp.bindingData.Equals("CSS_STATUS_ID")).value.ToString();
                string statusValue = listParams.Single(temp => temp.bindingData.Equals("CSS_STATUS_VALUE")).value.ToString();
                int res=BuinessRule.GetInstace().sleMonitor.UpdateDevAlarmStyle(statusId, statusValue, GetCurrentSelect());
                if (res == 0)
                {
                    //todo:successful
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Alarm_Style, "0", "报警方式设置成功");
                    MessageDialog.Show("设备状态报警样式设置成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                else
                {
                    //todo: show tip for operator
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Alarm_Style, "1", "报警方式设置失败");
                    MessageDialog.Show("设备状态报警样式设置失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);

                }
                //ShowDetailsDialog sdd = new ShowDetailsDialog();
                //sdd.Content = this;
                //sdd.Title = "报警设置";
                //sdd.ClosingEvent += new ShowDetailsDialog.HandleWindowClose(() => { });

                BaseWindow bw = listParams.Single(temp => temp.bindingData.Equals("window")).value as BaseWindow;
                if (bw != null)
                {
                    bw.Close();
                }

            }
            catch (Exception ex)
            {
                ;
            }
            
            //throw new NotImplementedException();
        }

        private List<QueryCondition> list=null;

        public override void InitControls()
        {
            //todo:get value from action ParamList
            var listParams = this.Tag as List<QueryCondition>;

            if (listParams != null &&
                listParams.Count > 0)
            {
                if (listParams.Count ==1)
                    MessageDialog.Show("请选择需要设置的状态类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                list = listParams;
                string alarmStyle = listParams.Single(temp => temp.bindingData.Equals("ALARM_STYLE")).value.ToString();

                //todo : call binding
                BindingDataToUI(alarmStyle);
            }
            //else
            //{
            //    MessageDialog.Show("请选择报警类别", UIHelper.GetResouceValueByID("DialogMessage_Tip"), MessageBoxIcon.Information, MessageBoxButtons.Ok);
                             
            //}
            //Binding Value
            //base.InitControls();
        }

        private string GetCurrentSelect()
        {
            string result = "";
            if (this.radShakeImage.IsChecked.Value)
            {
                result = result + "1";
            }
            else
            {
                result = result + "0";
            }
            if (this.radShowDlg.IsChecked.Value)
            {
                result = result + "1";
            }
            else
            {
                result = result + "0";
            }
            if (this.radSound.IsChecked.Value)
            {
                result = result + "1";
            }
            else
            {
                result = result + "0";
            }
            return result.ToUShort().ConvertNumberToHexString();
        }

        public override void UnLoadControls()
        {
            base.UnLoadControls();
        }


        private void BindingDataToUI(string currentSelectValue)
        {
            if (string.IsNullOrEmpty(currentSelectValue))
            {
                //todo: log here

            }
            switch (currentSelectValue)
            {
                case "不报警":
                    break;
                case "全报警":
                    this.radShakeImage.IsChecked = true;
                    this.radShowDlg.IsChecked = true;
                    this.radSound.IsChecked = true;
                    break;
                case "提示框":
                    this.radShowDlg.IsChecked = true;
                    break;
                case "铃声":
                    this.radSound.IsChecked = true;
                    break;
                case "闪烁":
                    this.radShakeImage.IsChecked = true;
                    break;
                case "铃声+闪烁":
                    this.radShakeImage.IsChecked = true;
                    this.radSound.IsChecked = true;
                    break;
                case "铃声+提示框":
                    this.radShowDlg.IsChecked = true;
                    this.radSound.IsChecked = true;
                    break;
                case "闪烁+提示框":
                    this.radShakeImage.IsChecked = true;
                    this.radShowDlg.IsChecked = true;
                    break;
            }
        }
    }
}
