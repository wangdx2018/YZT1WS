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

namespace AFC.WS.UI.Primission

{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Common;
    /// <summary>
    ///负责人 ：王冬欣  最后修改日期：20091204
    ///增加功能界面，采用WS基础组件
    /// </summary>
    public partial class FunctionAdded : UserControlBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FunctionAdded()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Lab提示信息
        /// </summary>
        private LabelExtend labTip = null;

        /// <summary>
        /// 
        /// </summary>
        public override void InitControls()
        {
            List<AFC.WS.UI.Common.QueryCondition> list = this.Tag as List<AFC.WS.UI.Common.QueryCondition>;

            System.Windows.Window window = list.Single(temp => temp.bindingData.Equals("window")).value as System.Windows.Window;

            window.Title = "新增功能";
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_addFunction.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            UIElement element = this.ic.GetCommonControlByName("btn_priv_function_info_device_type");
            LabelExtend lab = this.ic.GetCommonControlByName("labTip") as LabelExtend;
            if (lab != null)
            {
                labTip = lab;
            }
            if (element != null && element is ComboBoxExtend)
            {
                ComboBoxExtend cmbDeviceType = element as ComboBoxExtend;
                cmbDeviceType.SelectionChanged += new SelectionChangedEventHandler(cmbDeviceType_SelectionChanged);
            }

        }

        /// <summary>
        /// 设备类型选择变化事件
        /// </summary>
        /// <param name="sender">事件发起源</param>
        /// <param name="e">事件源</param>
        private void cmbDeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBoxExtend)
            {
                ComboBoxExtend element = sender as ComboBoxExtend;
                string value = element.SelectedValue.ToString();
                switch (value)
                {
                    case "LCWS":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为12");
                        break;
                    case "SCWS":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为12");
                        break;
                    case "MCWS":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为12");
                        break;
                    case "TCWS":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为12");
                        break;
                    case "AG":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为06");
                        break;
                    case "BOM":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为02");
                        break;
                    case "TVM":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为01");
                        break;
                    case "EQM":
                        (labTip as ICommonEdit).SetControlValue(value + "功能编号:前两位为04");
                        break;

                }
            }
        }
    }
}
