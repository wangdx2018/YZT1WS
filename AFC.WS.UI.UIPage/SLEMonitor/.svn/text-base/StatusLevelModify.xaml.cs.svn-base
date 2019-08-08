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
    public partial class StatusLevelModify : UserControlBase
    {
        public StatusLevelModify()
        {
            InitializeComponent();
            this.btnOk.Click += new RoutedEventHandler(btnOk_Click);
            //ShowDetailsDialog.ShowDetails(UIHelper.GetResouceValueByID("menu_Status_Level_Set"), rootLayout, 350, 200);
            
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
                int res = BuinessRule.GetInstace().sleMonitor.UpdateStatusLevel(statusId, statusValue, GetCurrentSelect());

                if (res == 0)
                {
                    //todo:successful
                    MessageDialog.Show("设备状态级别设置成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Status_Level, "0", "报警级别设置成功");
                }
                else
                {
                    //todo: show tip for operator
                    MessageDialog.Show("设备状态级别设置失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Status_Level, "1", "报警级别设置失败");

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
                string StatusLevel = listParams.Single(temp => temp.bindingData.Equals("STATUS_LEVEL")).value.ToString();

                //todo : call binding
                BindingDataToUI(StatusLevel);
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
            
            if (this.normal.IsChecked.Value)
                return this.normal.Tag.ToString();
            if (this.error.IsChecked.Value)
                return this.error.Tag.ToString();
            if (this.warning.IsChecked.Value)
                return this.warning.Tag.ToString();

            return "FF";
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
                case "正常":
                    this.normal.IsChecked = true;
                    break;
                case "警告":
                    this.warning.IsChecked = true;
                    break;
                case "故障":
                    this.error.IsChecked = true;
                    break;
            }
        }
    }
}
