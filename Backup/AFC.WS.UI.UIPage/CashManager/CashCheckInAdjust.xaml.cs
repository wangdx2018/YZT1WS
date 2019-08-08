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
using AFC.BOM2.UIController;
using AFC.WS.BR.TickMonyBoxManager;
using System.Collections.ObjectModel;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using Microsoft.Windows.Controls;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;
using AFC.WS.ModelView.Convertors;
using System.Text.RegularExpressions;

namespace AFC.WS.UI.UIPage.CashManager
{
    /// <summary>
    /// CashCheckInAdjust.xaml 的交互逻辑
    /// </summary>
    public partial class CashCheckInAdjust : UserControlBase
    {

        /// <summary>
        /// 现金操作数据集合
        /// </summary>
        private ObservableCollection<CashDetailsInfo> list = new ObservableCollection<CashDetailsInfo>();

        /// <summary>
        /// 结算流水表
        /// </summary>
        private ObservableCollection<DataOperSettlementInfo> listSettleDate = new ObservableCollection<DataOperSettlementInfo>();

        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;

        private string selSettleDate = string.Empty;

        //结算日期控件
        private Microsoft.Windows.Controls.DatePicker dp = null;
       
        public CashCheckInAdjust()
        {
            InitializeComponent();
            this.cmbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cmbMoneyType_SelectionChanged);
            //this.txtCallInMonNo.KeyUp += new KeyEventHandler(txtCallInMonNo_KeyUp);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallIn.Click += new RoutedEventHandler(btnCallIn_Click);
            this.btnQuery.Click += new RoutedEventHandler(btnQuery_Click);
            this.listSettle.ItemsSource = listSettleDate;
            this.listView1.ItemsSource = list;
        }

        /// <summary>
        /// 现金归还
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallIn_Click(object sender, RoutedEventArgs e)
        {
            IAction action = new AFC.WS.ModelView.Actions.CashManager.CashCheckInAdjustAction();
            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].CashTypeCode, value = list[i].CashNumber });
            }

            string operationCode = this.txtOperation.Text.Trim();
            //操作员
            listQueryCondition.Add(new QueryCondition { bindingData = "operationCode", value = operationCode });
            //运营日
            listQueryCondition.Add(new QueryCondition { bindingData = "settleDate", value = selSettleDate });
            if (action.CheckValid(listQueryCondition))
                action.DoAction(listQueryCondition);
        }


        /// <summary>
        /// 当结算日期改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UIElement element = this.settlementDate;
            if (element != null && element is DateTimePickerExtend)
            {
                DateTimePickerExtend cmb = element as DateTimePickerExtend;
                if (string.IsNullOrEmpty((sender as DatePicker).Text))
                    return;
                DateTime strSettDate = (DateTime)(sender as DatePicker).SelectedDate;
                if (strSettDate != null)
                {
                    selSettleDate = strSettDate.ToString("yyyyMMdd");
                }
            }

        }
        /// <summary>
        /// 删除现金调入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsChecked)
                {
                    this.list.Remove(list[i]);
                }
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
                MessageDialog.Show("请输入归还金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            string expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
            if (!Regex.IsMatch(this.txtCallInMonNo.Text, expression, RegexOptions.Compiled))
            {
                MessageDialog.Show("只能输入金额。金额范围0.00-9999999.99", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            
            return true;
        }

        //private void txtCallInMonNo_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBoxExtend tb = sender as TextBoxExtend;
        //    try
        //    {
        //        int result = int.Parse(tb.Text);
        //        CashStorageInfo csInfo = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(this.currentSelected.currency_code);
        //        if (csInfo != null)
        //        {

        //            this.txtCallInMoneyValue.Text = (result * this.currentSelected.currency_value.ConvertNumberStringToUint()).ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.txtCallInMoneyValue.Text = string.Empty;
        //        //todo:log here
        //    }
        //}

        private void cmbMoneyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            BasiMoneyTypeInfo info = cb.SelectedValue as BasiMoneyTypeInfo;
            this.currentSelected = info;
           
            //this.txtCallInMoneyValue.Text = string.Empty;
            this.txtCallInMonNo.Text = string.Empty;
        }


        public override void InitControls()
        {
            List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
            string multiMoneyName = list.Single(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
            this.cmbMoneyType.ItemsSource = list;
            this.cmbMoneyType.DisplayMemberPath = "currency_name";
            this.cmbMoneyType.SetControlValue(multiMoneyName);
            this.cmbMoneyType.CanReadOnly = true;
            this.cmbMoneyType.IsEditable = false;


            this.labTotal.Content = string.Empty;
            this.labTotalMoney.Content = string.Empty;

            UIElement elementPick = this.settlementDate;
            DateTime RunDate = DateTime.ParseExact(BuinessRule.GetInstace().rm.GetRunDate(), "yyyy年MM月dd日", null);
            if (elementPick != null && elementPick is DateTimePickerExtend)
            {
                DateTimePickerExtend pickExtend = elementPick as DateTimePickerExtend;
                Grid g = pickExtend.Content as Grid;
                if (g != null)
                {
                    foreach (var a in g.Children)
                    {
                        if (a is Microsoft.Windows.Controls.DatePicker)
                        {
                            dp = a as Microsoft.Windows.Controls.DatePicker;
                            dp.SelectedDate = RunDate;
                            dp.DisplayDate = RunDate;
                            dp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dp_SelectedDateChanged);
                            break;
                        }
                    }
                }
            }
            selSettleDate = RunDate.ToString("yyyyMMdd");
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


        /// <summary>
        /// 计算应还金额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtOperation.Text.Trim()))
            {
                MessageDialog.Show("请输入操作员!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            ConvertSimpleFenToYuan covertToYuan = new ConvertSimpleFenToYuan();
            ConvertToTime covertToTime = new ConvertToTime();
            DateTimeConvert covertToDate = new DateTimeConvert();
            string operationCode = this.txtOperation.Text.Trim();
            this.listSettleDate.Clear();
            DataOperSettlementInfo infoTemp = BuinessRule.GetInstace().GetDataOperSettlementInfo(operationCode, selSettleDate);
            if (infoTemp != null && !string.IsNullOrEmpty(infoTemp.operator_id))
            {
                infoTemp.in_oper_money = Convert.ToDecimal(covertToYuan.Convert(infoTemp.in_oper_money, null, null, null).ToString());
                infoTemp.real_rece_money =Convert.ToDecimal(covertToYuan.Convert(infoTemp.real_rece_money, null, null, null).ToString());
                infoTemp.sys_rece_money = Convert.ToDecimal(covertToYuan.Convert(infoTemp.sys_rece_money, null, null, null).ToString());
                infoTemp.run_date = covertToDate.Convert(infoTemp.run_date, null, null, null).ToString();
                infoTemp.update_date = covertToDate.Convert(infoTemp.update_date, null, null, null).ToString();
                infoTemp.update_time = covertToTime.Convert(infoTemp.update_time, null, null, null).ToString();
                listSettleDate.Add(infoTemp);
            }
            
            //差额
            this.txtDiffer.Text = TickMonyBoxHelp.Instance.getDifference(operationCode, selSettleDate).ToString();
           
        }


        public override void UnLoadControls()
        {
            this.list.Clear();
            this.listSettleDate.Clear();
            this.currentSelected = null;
            //this.txtCallInMoneyValue.Text = string.Empty;
            this.txtOperation.Text = string.Empty;
            this.txtCallInMonNo.Text = string.Empty;
            this.labTotal.Content = string.Empty;
            this.labTotalMoney.Content = string.Empty;
            this.txtDiffer.Text = string.Empty;
            this.cmbMoneyType.SelectedIndex = -1;
            dp.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(dp_SelectedDateChanged);
        }
    }
}
