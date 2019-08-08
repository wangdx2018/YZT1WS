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
    public partial class BasiMaintainAreaInfoUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
        String MaintainAreaStatus="00";

        public BasiMaintainAreaInfoUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            //Window window = list.Single(temp => temp.bindingData.Equals("window")).value as Window;
            this.MaintainAreaID.Text = list.Single(temp => temp.bindingData.Equals("maintain_area_id")).value.ToString();
            this.MaintainAreaName.Text = list.Single(temp => temp.bindingData.Equals("maintain_area_name")).value.ToString();
            this.MaintainAreaAddress.Text = list.Single(temp => temp.bindingData.Equals("maintain_area_address")).value.ToString();
            this.MaintainAreaContector.Text = list.Single(temp => temp.bindingData.Equals("dute_person")).value.ToString();
            this.MaintainAreaPhone.Text = list.Single(temp => temp.bindingData.Equals("phone")).value.ToString();
            MaintainAreaStatus = list.Single(temp => temp.bindingData.Equals("status")).value.ToString();
            if (MaintainAreaStatus == "已删除")
            {
                this.MaintainAreaID.IsEnabled = false;
                this.MaintainAreaName.IsEnabled = false;
                this.MaintainAreaAddress.IsEnabled = false;
                this.MaintainAreaContector.IsEnabled = false;
                this.MaintainAreaPhone.IsEnabled = false;
                Wrapper.ShowDialog("已删除的工区不允许进行操作。");
                button1.IsEnabled = false;
                button2.IsEnabled = false;
            }
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "MaintainAreaID", this.MaintainAreaID.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "MaintainAreaName", this.MaintainAreaName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "MaintainAreaAddress", this.MaintainAreaAddress.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "MaintainAreaContector", this.MaintainAreaContector.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "MaintainAreaPhone", this.MaintainAreaPhone.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.BasiMaintainAreaInfoUpdate();

            //dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list1);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.MaintainAreaName.Text = string.Empty;
            this.MaintainAreaAddress.Text = string.Empty;
            this.MaintainAreaContector.Text = string.Empty;
            this.MaintainAreaPhone.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
