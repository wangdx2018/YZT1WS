using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using AFC.BOM2.UIController;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Common;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.UI.CommonControls;
using System.Text.RegularExpressions;

namespace AFC.WS.UI.UIPage.CashManager
{
    /// <summary>
    /// CashStoreAdjust.xaml 的交互逻辑
    /// </summary>
    public partial class CashStoreAdjust : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        //调整方法：0:总数调整、1:正向调整、2:负向调整
        string adjustMethod = string.Empty;
        //调用的button 0:库存调整、 4：待解行、5：解行
        string buttonMethod = string.Empty;
        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;


        public CashStoreAdjust()
        {
            InitializeComponent();
            this.cmbMoneyStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbMoneyStoreType_SelectionChanged);
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
                this.UpAdjust.IsChecked = true;
                List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetCoinMultMoneyTypeCodeInfo();
                string multiMoneyName = list.Single(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
                this.cmbMoneyStoreType.ItemsSource = list;
                this.cmbMoneyStoreType.DisplayMemberPath = "currency_name";
                this.cmbMoneyStoreType.SetControlValue(multiMoneyName);
                //this.cmbMoneyStoreType.CanReadOnly = true;
                //this.cmbMoneyStoreType.IsEditable = false;


            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        void cmbMoneyStoreType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string moneyType = Wrapper.GetComboBoxUid(cmbMoneyStoreType);
            //this.txtMoneyNo.Text = BuinessRule.GetInstace().GetCashStorageById(moneyType).currency_num.ToString();
            //this.txtRealNo.Text = string.Empty;


            ComboBox cb = sender as ComboBox;
            BasiMoneyTypeInfo info = cb.SelectedValue as BasiMoneyTypeInfo;
            this.currentSelected = info;
            if (info != null)
            {

                CashStorageInfo csInfo = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(info.currency_code);
                if (csInfo != null)
                {
                    this.txtMoneyNo.Text = (csInfo.currency_num * info.currency_value.ConvertNumberStringToUint()).ToString();
                }
                //多币种
                if (info.currency_code == "00")
                {

                    CashWaitingToBankInfo waitInfo = BuinessRule.GetInstace().GetCashWaitingById(info.currency_code);
                    if (waitInfo != null)
                    {
                        this.txtMoneyWait.Text = (waitInfo.total_value / 100).ToString();
                    }
                    this.btnWait.IsEnabled = true;
                    this.btnSolution.IsEnabled = true;
                }
                else
                {
                    this.txtMoneyWait.Text = "0";
                    this.btnWait.IsEnabled = false;
                    this.btnSolution.IsEnabled = false;
                }

            }
            this.txtRealNo.Text = string.Empty;

        }

        private void btnAdjust_Click(object sender, RoutedEventArgs e)
        {
            buttonMethod = "0";
            //输入项正确性检查
            if (CheckValid())
            {
                if (adjustMethod.Equals("2"))
                {
                    decimal storageNum = Convert.ToDecimal(this.txtMoneyNo.Text);
                    decimal adjustNum = Convert.ToDecimal(this.txtRealNo.Text);
                    //如果负向调整大于库存数
                    if (adjustNum > storageNum)
                    {
                        MessageDialog.Show("负向调整金额不能大于在库金额。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return;
                    }
                }
                businessAction();
            }


        }

        private void businessAction()
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "adjustMethod", adjustMethod);
            Wrapper.Instance.AddQueryConditionToList(list, "buttonMethod", buttonMethod);
            Wrapper.Instance.AddQueryConditionToList(list, "moneyCode", (cmbMoneyStoreType.SelectedValue as BasiMoneyTypeInfo).currency_code);
            Wrapper.Instance.AddQueryConditionToList(list, "moneyNo", this.txtMoneyNo.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "moneyWait", this.txtMoneyWait.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "moneyReal", this.txtRealNo.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "remark", convertString(this.txtRemark.Text));
            dpaction.subAction = new AFC.WS.ModelView.Actions.CashManager.CashStoreAdjustAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            if (dpaction.CheckValid(list))
            {
                dpaction.DoAction(list);
            }
            InitLoad();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.txtRealNo.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            this.UpAdjust.IsChecked = true;

        }

        public override void UnLoadControls()
        {
            this.txtRealNo.Text = string.Empty;
            this.txtMoneyNo.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            this.txtMoneyWait.Text = string.Empty;

        }

        void txtPutNo_TextChanged(object sender, EventArgs e)
        {

            label1.Content = "剩下可输入字数:" + (50 - this.txtRemark.Text.Length).ToString();
        }

        private void AdjustMothod_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (sender is RiadioButtonExtend)
                {
                    RiadioButtonExtend adjustRadio = sender as RiadioButtonExtend;

                    adjustMethod = adjustRadio.Uid;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("AdjustMothod_Checked函数异常:" + ex.ToString());
            }

        }

        //待解行
        private void btnWait_Click(object sender, RoutedEventArgs e)
        {
            buttonMethod = "4";
            //输入项正确性检查
            if (CheckValid())
            {

                decimal storageNum = Convert.ToDecimal(string.IsNullOrEmpty(this.txtMoneyNo.Text) ? "0" : this.txtMoneyNo.Text);
                decimal adjustNum = Convert.ToDecimal(string.IsNullOrEmpty(this.txtRealNo.Text) ? "0" : this.txtRealNo.Text);
                decimal waitNum = Convert.ToDecimal(string.IsNullOrEmpty(this.txtMoneyWait.Text) ? "0" : this.txtMoneyWait.Text);
                //总量调整
                if (adjustMethod == "0")
                {
                    //如果待解行大于库存数
                    if (adjustNum > storageNum)
                    {
                        MessageDialog.Show("待解行金额不能大于在库金额。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return;
                    }
                }
                //正向调整
                if (adjustMethod == "1")
                {
                    if (adjustNum > storageNum)
                    {
                        MessageDialog.Show("待解行金额不能大于在库金额。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return;
                    }
                }

                //负向调整
                if (adjustMethod == "2")
                {
                    if (adjustNum > waitNum)
                    {
                        MessageDialog.Show("负调整金额不能大于待解行金额。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return;
                    }
                }

                businessAction();

            }

        }
        //解行
        private void btnSolution_Click(object sender, RoutedEventArgs e)
        {
            buttonMethod = "5";
            //输入项正确性检查
            if (CheckValid())
            {
                //if (adjustMethod != "0")
                //{
                //    MessageDialog.Show("解行操作只能按总数调整。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                //    this.TotalAdjust.IsChecked = true;
                //    return;
                //}
                decimal storageNum = Convert.ToDecimal(this.txtMoneyWait.Text);
                decimal adjustNum = Convert.ToDecimal(this.txtRealNo.Text);
                //如果解行不等于待解行数
                if (adjustNum != storageNum)
                {
                    MessageDialog.Show("解行金额不能不等于待解行金额。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return;
                }

                businessAction();
            }


        }

        private bool CheckValid()
        {
            if (string.IsNullOrEmpty(this.txtRealNo.Text))
            {
                MessageDialog.Show("请输入操作金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            string expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
            if (!Regex.IsMatch(this.txtRealNo.Text, expression, RegexOptions.Compiled))
            {
                MessageDialog.Show("只能输入金额。金额范围0.00-9999999.99", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public string convertString(string value)
        {
            string after = value.Replace("\"", string.Empty).Replace("'", string.Empty);
            return after;
        }
    }
}
