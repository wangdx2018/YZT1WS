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
using AFC.WS.BR;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;

namespace AFC.WS.UI.UIPage.Maintenance
{
    /// <summary>
    /// NoLabelPartsReg.xaml 的交互逻辑
    /// </summary>
    public partial class NoLabelPartsReg : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public NoLabelPartsReg()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            try
            {
                this.txtPartsID.Text = string.Empty;
                this.txtOperator.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                this.txtNum.Text = string.Empty;
                //供应商
                Wrapper.FullComboBox(this.cbbSuppliers, BuinessRule.GetInstace().GetBasiProviderInfo(), "mc_dep_name", "provider_id", false);
                //部件ID
                Wrapper.FullComboBox(this.cbbParts, BuinessRule.GetInstace().GetBasiDevTypeInfo(), "dev_part_cn_name", "dev_part_id", false);

            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }
        /// <summary>
        /// 调入按钮按下时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Wrapper.Instance.AddQueryConditionToList(list, "provider", Wrapper.GetComboBoxUid(this.cbbSuppliers));
                Wrapper.Instance.AddQueryConditionToList(list, "partType", Wrapper.GetComboBoxUid(this.cbbParts));
                Wrapper.Instance.AddQueryConditionToList(list, "partID", this.txtPartsID.Text.Trim());
                Wrapper.Instance.AddQueryConditionToList(list, "operatorID", this.txtOperator.Text.Trim());
                Wrapper.Instance.AddQueryConditionToList(list, "partsNum", this.txtNum.Text.Trim());



                IAction action = new AFC.WS.ModelView.Actions.Maintenance.NoLabelPartsRegAction();
                if (action.CheckValid(list))
                {
                    action.DoAction(list);
                }

                btnCancel_Click(sender, e);

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }

        }

        /// <summary>
        /// 取消按钮压下时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtPartsID.Text = string.Empty;
            this.txtOperator.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            this.txtNum.Text = string.Empty;
            this.cbbSuppliers.SelectedIndex = -1;
            this.cbbParts.SelectedIndex = -1;
        }
    }
}
