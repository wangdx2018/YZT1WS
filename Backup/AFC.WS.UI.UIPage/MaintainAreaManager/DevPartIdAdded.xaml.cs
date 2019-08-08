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
    public partial class DevPartIdAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        string DevTypeNum="";
        public DevPartIdAdded()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
        }
        private void InitLoad()
        {
            try
            {
                Wrapper.FullComboBox<BasiDevTypeInfo>(this.DevType, BuinessRule.GetInstace().GetAllDevciceType(), "device_name", "device_type", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }
        /*
        void cmbDevType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string DevTypeTemp = Wrapper.GetComboBoxUid(cmbDevType);
            this.txtMoneyNo.Text = BuinessRule.GetInstace().GetDevSettlementInfo(DevTypeTemp).currency_num.ToString();
            this.txtRealNo.Text = string.Empty;
        }*/

        private void btnAddPartID_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "DevPartId", this.DevPartId.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "DevType", Wrapper.GetComboBoxUid(DevType));
            Wrapper.Instance.AddQueryConditionToList(list, "DevPartIdName", this.DevPartIdName.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.DevPartIdAdd();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.DevPartId.Text = string.Empty;
            this.DevType.Text = string.Empty;
            this.DevPartIdName.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
