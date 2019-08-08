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

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// Param4044AlarmLamp.xaml 的交互逻辑
    /// </summary>
    public partial class Param4044AlarmLamp : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        AddPara4044AlarmLampAction para4044 = new AddPara4044AlarmLampAction();

        public Param4044AlarmLamp()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string lightInfo = this.ctrLightEdit.GetControlValue().ToString();
            string soundInfo = this.ctrVoiceEdit.GetControlValue().ToString();

            
            Wrapper.Instance.AddQueryConditionToList(list, "para_version", this.txtParaVersion.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "card_issuer_id", this.cmbIssuerID.Text=="ACC"?"01":"99");
            Wrapper.Instance.AddQueryConditionToList(list, "tick_product_type", (this.cmbTickProType.SelectedItem as BasiTickManaTypeInfo).tick_mana_type);
            Wrapper.Instance.AddQueryConditionToList(list, "lamp_control",lightInfo);
            Wrapper.Instance.AddQueryConditionToList(list, "voice_control",soundInfo);

            if (!para4044.CheckValid(list))
            {
                return;
            }

            para4044.DoAction(list);


        }

        public override void InitControls()
        {
            this.ctrLightEdit.SetHeader("灯处理");
            this.ctrVoiceEdit.SetHeader("声音处理");
            this.ctrLightEdit.SetControlValue(0);
            this.ctrVoiceEdit.SetControlValue(string.Empty);
            //base.InitControls();
        }

        private void cmbIssuerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(cmbIssuerID.Text))
            //{
            //    MessageDialog.Show("请输入发行商ID！","提示",MessageBoxIcon.Information,MessageBoxButtons.Ok);
            //    return;
            //}
            //string queryCmd=string.Empty;

            //if (cmbIssuerID.Text=="ACC")
            //{
            //   queryCmd= string.Format("select * from basi_product_type_info t where t.card_issue_id={0}",1);
 
            //}
            //if (cmbIssuerID.Text=="一卡通")
            //{
            //    queryCmd = string.Format("select * from basi_product_type_info t where t.card_issue_id={0}", 99);
            //}

            //this.cmbTickProType.ItemsSource = DBCommon.Instance.GetTModelValue<BasiProductTypeInfo>(queryCmd);

            //this.cmbTickProType.DisplayMemberPath = "product_type_name_cn";

        }

        private void cmbIssuerID_DropDownClosed(object sender, EventArgs e)
        {
            string queryCmd = string.Empty;

            if (cmbIssuerID.Text == "ACC")
            {
                queryCmd = string.Format("select * from basi_tick_mana_type_info t where t.card_issue_id='{0}'", 1);

            }
            if (cmbIssuerID.Text == "一卡通")
            {
                queryCmd = string.Format("select * from basi_tick_mana_type_info t where t.card_issue_id='{0}'", 99);
            }

            this.cmbTickProType.ItemsSource = DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(queryCmd);

            this.cmbTickProType.DisplayMemberPath = "tick_mana_type_name";
        }
    }
}
