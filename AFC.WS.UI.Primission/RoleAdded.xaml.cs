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
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// 负责人：王冬欣 最后修改日期：20100105
    /// 增加角色信息 采用WS2.0基础组件
    /// </summary>
    public partial class RoleAdded : UserControlBase
    {
        public RoleAdded()
        {
            InitializeComponent();
        }


        public LabelExtend labTip = null;

        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_addRole.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            UIElement element = this.ic.GetCommonControlByName("comDeviceType");
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
        /// 设备类型变化选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBoxExtend)
            {

                string value = (sender as ComboBoxExtend).SelectedValue.ToString();
                switch (value)
                {
              
                    case "LCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value+"角色编号在00000001到00001000之间");
                        break;
                    case "SCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value+"角色编号在00001001到00002000之间");
                        break;
                    case "MCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value+"角色编号在00002001到00003000之间");
                        break;
                    case "TCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00003001到00004000之间");
                        break;
                    case "AGM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00004001到00005000之间");
                        break;
                    case "BOM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00005001到00006000之间");
                        break;
                    case "TVM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00006001到00007000之间");
                        break;
                    case "EQM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00007001到00008000之间");
                        break;
                    case "PCA相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00008001到00009000之间");
                        break;
                    case "ES相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00009001到00010000之间");
                        break;
                    case "SDG相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在00010001到00011000之间");
                        break;

                }
            }
        }

        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
          //  ic = null;
            //base.UnLoadControls();
        }


    }
}
