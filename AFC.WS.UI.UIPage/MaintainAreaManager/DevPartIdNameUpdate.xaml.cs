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

namespace AFC.WS.UI.UIPage.MaintainAreaManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class DevPartIdNameUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
        string DevTypeUid="";

        public DevPartIdNameUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            //Window window = list.Single(temp => temp.bindingData.Equals("window")).value as Window;
            this.DevPartId.Text = list.Single(temp => temp.bindingData.Equals("dev_part_id")).value.ToString();
            DevTypeUid = list.Single(temp => temp.bindingData.Equals("device_type")).value.ToString();
            this.DevPartIdName.Text = list.Single(temp => temp.bindingData.Equals("dev_part_cn_name")).value.ToString();
            try
            {
                Wrapper.FullComboBox<BasiDevTypeInfo>(this.DevType, BuinessRule.GetInstace().GetAllDevciceType(), "device_name", "device_type", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            Wrapper.ComboBoxSelectedItem(this.DevType, DevTypeUid);
            
        }

        private void btnUpdatePartID_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "DevPartId", this.DevPartId.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "DevType", Wrapper.GetComboBoxUid(DevType));
            Wrapper.Instance.AddQueryConditionToList(list1, "DevPartIdName", this.DevPartIdName.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.DevPartIdNameUpdate();

            //dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list1);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //this.DevPartId.Text = string.Empty;
            this.DevType.Text = string.Empty;
            this.DevPartIdName.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
