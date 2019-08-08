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
    public partial class BasiMaintainAreaInfoAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public BasiMaintainAreaInfoAdded()
        {
            InitializeComponent();
        }      

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "MaintainAreaID", this.MaintainAreaID.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "MaintainAreaName", this.MaintainAreaName.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "MaintainAreaAddress", this.MaintainAreaAddress.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "MaintainAreaContector", this.MaintainAreaContector.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "MaintainAreaPhone", this.MaintainAreaPhone.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.BasiMaintainAreaInfoAdd();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.MaintainAreaID.Text = string.Empty;
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
