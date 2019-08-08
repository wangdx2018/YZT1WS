using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AFC.WS.BR;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.BOM2.UIController;
using AFC.WS.ModelView.Convertors;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Collections.ObjectModel;

namespace AFC.WS.UI.UIPage.TickMonyBoxManager
{
    /// <summary>
    /// MoneyBoxPut.xaml 的交互逻辑
    /// </summary>
    public partial class MoneyBoxPut : UserControlBase
    {
        string moneyTypeCode = string.Empty;
        string moneyBoxID = string.Empty;
        string moneyNum = string.Empty;
        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;


        public MoneyBoxPut()
        {
            InitializeComponent();
            this.cbbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cbbMoneyType_SelectionChanged);
            this.txtMoneyBoxID.KeyUp += new KeyEventHandler(txtMoneyBoxID_KeyUp);
        }

        private void cbbMoneyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            BasiMoneyTypeInfo info = cb.SelectedValue as BasiMoneyTypeInfo;
            this.currentSelected = info;
        }


        public override void UnLoadControls()
        {
            btnCancel_Click(null, null);
            this.moneyBoxClear.UnLoadControls();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
            this.moneyBoxClear.InitControls();
        }

        /// <summary>
        /// 初始化加载方法
        /// </summary>
        void InitLoad()
        {
            try
            {
                this.lblMessage.Content = "";
                List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
                string multiMoneyName = list.Find(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
                this.cbbMoneyType.ItemsSource = list;
                this.cbbMoneyType.DisplayMemberPath = "currency_name";
                this.cbbMoneyType.SetControlValue(multiMoneyName);
                this.cbbMoneyType.CanReadOnly = true;
                this.cbbMoneyType.IsEditable = false;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 清空操作。 
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtMoneyBoxID.Text = "";
            this.cbbMoneyType.SelectedIndex = 0;
            this.txtNumber1.Text = "0";
            this.txtNumber2.Text = "";
            Wrapper.Instance.InitControlValue<TextBoxExtend, GroupBox>(gbInfo);
        }

        /// <summary>
        /// 输入钱箱编码代出数据库信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtMoneyBoxID_KeyUp(object sender, KeyEventArgs e)
        {
           
            string typeCode = this.txtMoneyBoxID.Text.Trim();
            if (typeCode.Length == 8)
            {
                typeCode = typeCode.Substring(2, 2);
                if (typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币补充箱)
                {
                    initGbInfo();

                }
                else
                {
                    Wrapper.ShowDialog("请输入类型正确的钱箱编号。");
                }
            }
            
        }

        /// <summary>
        /// 钱箱装钱
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            //moneyTypeCode = Wrapper.GetComboBoxUid(cbbMoneyType);
            moneyTypeCode = this.currentSelected.currency_code;
            moneyBoxID = this.txtMoneyBoxID.Text;
            moneyNum = this.txtNumber2.Text;

            TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "moneyBoxID", value = covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(), null, null, null) });
            list.Add(new QueryCondition { bindingData = "moneyType", value = moneyTypeCode });
            list.Add(new QueryCondition { bindingData = "moneyNum", value = moneyNum });

            IAction action = new AFC.WS.ModelView.Actions.TickMonyBoxManager.MoneyBoxPutAction();
            if (action.CheckValid(list))
            {
                action.DoAction(list);
            }

        }

        public void initGbInfo()
        {
            TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
            CashBoxStatusInfo statusInfo = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(),null,null,null).ToString());
            this.txtInstallLocation.Text = TickMonyBoxHelp.Instance.GetMoneyBoxLocationState(statusInfo.box_position.ToHexNumber());
            this.txtLastOperatorTime.Text = statusInfo.update_date + statusInfo.update_time;
            this.txtMoneyTypeName.Text = TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(statusInfo.currency_code);
            this.txtTotalNumber.Text = statusInfo.currency_num.ToString();
            /////////////////////////////////
           //总金额
            //////////////////////////////////

            int moneyValue = TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(statusInfo.currency_code).currency_value.ToInt32();
  
            this.txtTotalCash.Text = (statusInfo.currency_num * moneyValue).ToString();
            this.txtNumber2.Text = statusInfo.currency_num.ToString();
        }
    }
}
