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
                        (labTip as ICommonEdit).SetControlValue(value+"角色编号在1到1000之间");
                        break;
                    case "SCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value+"角色编号在1001到2000之间");
                        break;
                    case "MCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value+"角色编号在2001到3000之间");
                        break;
                    case "TCWS相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在3001到4000之间");
                        break;
                    case "AGM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在4001到5000之间");
                        break;
                    case "BOM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在5001到6000之间");
                        break;
                    case "TVM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在6001到7000之间");
                        break;
                    case "EQM相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在7001到8000之间");
                        break;
                    case "PCA相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在8001到9000之间");
                        break;
                    case "ES相关":
                        (labTip as ICommonEdit).SetControlValue(value + "角色编号在9001到10000之间");
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
