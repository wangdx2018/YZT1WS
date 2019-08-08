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
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Actions.ParamActions;
using AFC.WS.BR;
using System.Data;
using AFC.WS.ModelView.Actions.CommonActions;

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// Para4044AlarmLampUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class Para4044AlarmLampUpdate : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        UpdatePara4044AlarmLamp para4044 = new UpdatePara4044AlarmLamp();

        public Para4044AlarmLampUpdate()
        {
            InitializeComponent();
            
        }

        public override void InitControls()
        {
            this.ctrLightEdit.SetHeader("灯处理");
            this.ctrVoiceEdit.SetHeader("声音处理");

            //todo:GetData From DB

            List<QueryCondition> data=this.Tag as List<QueryCondition>;
            this.list = data;
            cmbIssuerID.Text = data[0].value.ToString();
            cmbTickProType.Items.Add(data[1].value);
            cmbTickProType.Text = data[1].value.ToString();
           
            this.ctrLightEdit.SetControlValue(data[2].value);
            this.ctrVoiceEdit.SetControlValue(data[3].value);
                     
            //DataTable dt = this.Tag as DataTable;

            //this.ctrLightEdit.SetControlValue(dt.Rows[0][2]);

            //base.InitControls();
        }

        private void cmbIssuerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string queryCmd = string.Empty;

            //if (cmbIssuerID.Text == "ACC")
            //{
            //    queryCmd = string.Format("select * from basi_product_type_info t where t.card_issue_id={0}", 1);

            //}
            //if (cmbIssuerID.Text == "一卡通")
            //{
            //    queryCmd = string.Format("select * from basi_product_type_info t where t.card_issue_id={0}", 99);
            //}

            //this.cmbTickProType.ItemsSource = DBCommon.Instance.GetTModelValue<BasiProductTypeInfo>(queryCmd);

            //this.cmbTickProType.DisplayMemberPath = "product_type_name_cn";

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //DoublePrimissionAction para4044 = new DoublePrimissionAction();

            try
            {
                string lightInfo = this.ctrLightEdit.GetControlValue().ToString();
                string soundInfo = this.ctrVoiceEdit.GetControlValue().ToString();


                list.Single(temp => temp.bindingData.Equals("lamp_control")).value = (object)lightInfo;

                list.Single(temp => temp.bindingData.Equals("voice_control")).value = (object)soundInfo;
                //Wrapper.Instance.AddQueryConditionToList(list, "lamp_control", lightInfo);
                //Wrapper.Instance.AddQueryConditionToList(list, "voice_control", soundInfo);

                //para4044.subAction = new AFC.WS.ModelView.Actions.ParamActions.UpdatePara4044AlarmLamp();
                //para4044.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;

                if (!para4044.CheckValid(list))
                {
                    return;
                }

                para4044.DoAction(list);

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
           


        }

        

        private void cmbIssuerID_DropDownClosed(object sender, EventArgs e)
        {
            string queryCmd = string.Empty;

            if (cmbIssuerID.Text == "ACC")
            {
                //queryCmd = string.Format("select * from basi_product_type_info t where t.card_issue_id='{0}'", 1);
                queryCmd = string.Format("select * from basi_tick_mana_type_info t where t.card_issue_id='{0}'", 1);

            }
            if (cmbIssuerID.Text == "一卡通")
            {
                //queryCmd = string.Format("select * from basi_product_type_info t where t.card_issue_id='{0}'", 99);
                queryCmd = string.Format("select * from basi_tick_mana_type_info t where t.card_issue_id='{0}'", 99);
            }

            this.cmbTickProType.ItemsSource = DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(queryCmd);

            this.cmbTickProType.DisplayMemberPath = "tick_mana_type_name";

        }
    }
}
