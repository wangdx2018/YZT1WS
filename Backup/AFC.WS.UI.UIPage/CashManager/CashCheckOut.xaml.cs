using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.WS.UI.Common;
using AFC.BOM2.UIController;
using System.Collections.ObjectModel;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using System.Text.RegularExpressions;
using AFC.WS.ModelView.Actions.CommonActions;

namespace AFC.WS.UI.UIPage.CashManager
{
    /// <summary>
    /// CashCheckOut.xaml 的交互逻辑
    /// 
    /// edited by wangdx 20121010 增加了领用确认和清空Label数值
    /// </summary>
    public partial class CashCheckOut : UserControlBase
    {
        /// <summary>
        /// 现金操作数据集合
        /// </summary>
        private ObservableCollection<CashDetailsInfo> list = new ObservableCollection<CashDetailsInfo>();

        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;

        public CashCheckOut()
        {
            InitializeComponent();
            this.cmbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cmbMoneyType_SelectionChanged);
            //this.txtCallInMonNo.KeyUp += new KeyEventHandler(txtCallInMonNo_KeyUp);
            this.txtOperatorId.KeyUp += new KeyEventHandler(txtOperatorId_KeyUp);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);
            this.listView1.ItemsSource = list;
        }

        public void txtOperatorId_KeyUp(object sender, KeyEventArgs e)
        {
            string operatorCode = this.txtOperatorId.Text.Trim();
            if (operatorCode.Length == 8)
            {
                PrivOperatorInfo operatorInfo = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(operatorCode);
                if (operatorInfo == null || string.IsNullOrEmpty(operatorInfo.operator_id))
                {
                    MessageDialog.Show("请输入正确的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                else
                {
                    this.txtOperatorName.Text = operatorInfo.operator_name;
                }
            }
        }
        /// <summary>
        /// 现金调出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtOperatorId.Text))
            {
                MessageDialog.Show("请输入领用操作员ID", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            if (list == null || list.Count == 0)
            {
                MessageDialog.Show("没有现金明细信息，请添加现金明细", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            MessageBoxResult result = MessageDialog.Show(string.Format("为操作员{1}配发金额为{0}，是否配发？", this.labTotalMoney.Content,this.txtOperatorName.Text), "确认", MessageBoxIcon.Question, MessageBoxButtons.YesNo);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }
          
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            dpaction.subAction = new AFC.WS.ModelView.Actions.CashManager.CashCheckOutAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;

            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].CashTypeCode, value = list[i].CashNumber });
            }
            listQueryCondition.Add(new QueryCondition { bindingData = "operationCode", value = this.txtOperatorId.Text.Trim() });
            if (dpaction.CheckValid(listQueryCondition))
                dpaction.DoAction(listQueryCondition);

            CashCheckOutInit();
        }

        /// <summary>
        /// 删除现金调出列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var temp = new ObservableCollection<CashDetailsInfo>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsChecked)
                {
                    temp.Add(list[i]);
                }
            }
            for (int i = 0; i < temp.Count; i++)
            {
                list.Remove(temp[i]);
            }
            this.UpdateTotalMoneyCountAndValue();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 增加现金调入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (this.CheckValid())
            {
                CashDetailsInfo info = new CashDetailsInfo();
                info.CashNumber = Convert.ToDouble(this.txtCallInMonNo.Text);
                info.CashTypeCode = this.currentSelected.currency_code;
                info.OperatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                info.TotalMoneyValue = Convert.ToDouble(this.txtCallInMonNo.Text);
                info.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd");
                info.UpdateTime = DateTime.Now.ToString("HH:mm:ss");
                info.CashTypeName = this.cmbMoneyType.Text;
                info.IsChecked = false;
                this.labTotalMoney.Content = string.Empty;
                this.labTotal.Content = string.Empty;
                AddCashDetails(info);
                this.UpdateTotalMoneyCountAndValue();

                this.txtCallInMonNo.Text = string.Empty;
            }

        }

        private bool CheckValid()
        {
            if (string.IsNullOrEmpty(this.cmbMoneyType.Text))
            {
                MessageDialog.Show("请选择币种代码!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtCallInMonNo.Text))
            {
                MessageDialog.Show("请输入领出金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            //int res = 0;
            //if (!int.TryParse(this.txtCallInMonNo.Text, out res))
            //{
            //    MessageDialog.Show("领出张数不合法!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            string expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
            if (!Regex.IsMatch(this.txtCallInMonNo.Text, expression, RegexOptions.Compiled))
            {
                MessageDialog.Show("只能输入金额。金额范围0.00-9999999.99", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            double storageNum = Convert.ToDouble(this.txtMoneyValue.Text.Trim());
            double intCallInMonNo = Convert.ToDouble(this.txtCallInMonNo.Text.Trim());
            if (intCallInMonNo > storageNum)
            {
                MessageDialog.Show("领出金额不能大于在库金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            return true;
        }

        private void txtCallInMonNo_KeyUp(object sender, KeyEventArgs e)
        {
            //TextBoxExtend tb = sender as TextBoxExtend;
            //try
            //{
            //    int result = int.Parse(tb.Text);
            //    CashStorageInfo csInfo = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(this.currentSelected.currency_code);
            //    if (csInfo != null)
            //    {

            //        this.txtCallInMoneyValue.Text = (result * this.currentSelected.currency_value.ConvertNumberStringToUint()).ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.txtCallInMoneyValue.Text = string.Empty;
            //    //todo:log here
            //}
        }

        private void cmbMoneyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            BasiMoneyTypeInfo info = cb.SelectedValue as BasiMoneyTypeInfo;
            this.currentSelected = info;
            if (info != null)
            {
                CashStorageInfo csInfo = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(info.currency_code);
                if (csInfo != null)
                {
                    this.txtMonNo.Text = csInfo.currency_num.ToString();
                    this.txtMoneyValue.Text = (csInfo.currency_num * info.currency_value.ConvertNumberStringToUint()).ToString();
                }
            }
            //this.txtCallInMoneyValue.Text = string.Empty;
            this.txtCallInMonNo.Text = string.Empty;
        }


        public override void InitControls()
        {
            CashCheckOutInit();

            this.TickCheckOut.InitControls();
        }

        private void CashCheckOutInit()
        {
            List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
            string multiMoneyName = list.Single(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
            this.cmbMoneyType.ItemsSource = list;
            this.cmbMoneyType.DisplayMemberPath = "currency_name";
            this.cmbMoneyType.SetControlValue(multiMoneyName);
            this.cmbMoneyType.CanReadOnly = true;
            this.cmbMoneyType.IsEditable = false;

            this.txtOperatorId.Text = string.Empty;
            this.txtOperatorName.Text = string.Empty;

            this.labTotalMoney.Content = string.Empty;
            this.labTotal.Content = string.Empty;
   
            this.list.Clear();
        }

        private void UpdateTotalMoneyCountAndValue()
        {
            double totalCount = 0;
            double totalValue = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalCount += list[i].CashNumber;
                totalValue += list[i].TotalMoneyValue;
            }
            this.labTotal.Content = totalCount.ToString();
            this.labTotalMoney.Content = totalValue.ToString();
        }

        /// <summary>
        /// 增加库存关
        /// </summary>
        /// <param name="data"></param>
        private void AddCashDetails(CashDetailsInfo data)
        {
            if (data == null)
            {
                WriteLog.Log_Error("param is null");
                return;
            }
            if (string.IsNullOrEmpty(data.CashTypeCode))
            {
                WriteLog.Log_Error("cash type is null or empty");
                return;
            }
            try
            {
                int res = list.Count(temp => temp.CashTypeCode.Equals(data.CashTypeCode));
                if (res == 1)
                {
                    CashDetailsInfo last = list.Single(temp => temp.CashTypeCode.Equals(data.CashTypeCode));
                    last.CashNumber = data.CashNumber + last.CashNumber;
                    last.TotalMoneyValue = last.TotalMoneyValue + data.TotalMoneyValue;
                }
                else
                    this.list.Add(data);
            }
            catch (Exception ex)
            {

            }

        }


        public override void UnLoadControls()
        {
            this.list.Clear();
            this.currentSelected = null;
            //this.txtCallInMoneyValue.Text = string.Empty;
            this.txtCallInMonNo.Text = string.Empty;
            this.labTotal.Content = string.Empty;
            this.labTotalMoney.Content = string.Empty;
            this.txtMonNo.Text = string.Empty;
            this.txtMoneyValue.Text = string.Empty;
            this.txtOperatorId.Text = string.Empty;
            this.txtOperatorName.Text = string.Empty;
            this.TickCheckOut.UnLoadControls();
            //base.UnLoadControls();
        }
    }
}
