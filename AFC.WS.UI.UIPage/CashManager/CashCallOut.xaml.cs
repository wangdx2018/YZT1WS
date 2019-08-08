using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace AFC.WS.UI.UIPage.CashManager
{
    using AFC.BOM2.UIController;
    using System.Collections.ObjectModel;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.Const;
    using AFC.WS.ModelView.Actions.CommonActions;
    using System.Data;
    using AFC.WS.BR.TickMonyBoxManager;
    using System.Text.RegularExpressions;
    /// <summary>
    /// CashCallOut.xaml 的交互逻辑
    /// </summary>
    public partial class CashCallOut : UserControlBase
    {
        /// <summary>
        /// 调出现金列表
        /// </summary>
        private ObservableCollection<CashDetailsInfo> list = new ObservableCollection<CashDetailsInfo>();

        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;

        /// <summary>
        /// 操作代码
        /// </summary>
        string operatorCode = string.Empty;

        public CashCallOut()
        {
            InitializeComponent();
            this.cmbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cmbMoneyType_SelectionChanged);
            //this.txtCallInMonNo.KeyUp += new KeyEventHandler(txtCallInMonNo_KeyUp);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);
            //this.btnSolution.Click += new RoutedEventHandler(btnSolution_Click);
            //this.btnPrint.Click += new RoutedEventHandler(btnPrint_Click);
            this.listView1.ItemsSource = list;
        }

        void printCrystal()
        {
            if (string.IsNullOrEmpty(Wrapper.GetComboBoxText(this.cmbStation)))
            {
                MessageDialog.Show("请选择调入车站!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            //AFC.WS.UI.UIPage.CashManager.CrystalRptData rptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();

            Dictionary<string, string> dict = new Dictionary<string, string>();

            string messageInform = string.Empty;
            if (operatorCode == OperationCode.Cash_Call_Out)
            {
                messageInform = "现金调出";
            }
            else
            {
                messageInform = "现金解行";
            }

            dict.Add("ReportTitle1", messageInform);
            dict.Add("RequestBatchNo", TickMonyBoxHelp.Instance.GetSequenceNextVal().ToString());
            dict.Add("OperatorID", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            dict.Add("RequestLocationID", BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name);
            dict.Add("DispatchLocationID", Wrapper.GetComboBoxText(this.cmbStation));
            dict.Add("DispatchType", "调出");

            string dispathWay = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name + "----->" + Wrapper.GetComboBoxText(this.cmbStation);

            DataTable dt = new DataTable();

            dt.Columns.Add("CashName", typeof(string));
            dt.Columns.Add("CashNum", typeof(string));
            dt.Columns.Add("CashTotalValue", typeof(string));
            dt.Columns.Add("DispatchWay", typeof(string));


            for (int i = 0; i < list.Count; i++)
            {
                dt.Rows.Add(new string[] { list[i].CashTypeName, list[i].CashNumber.ToString(), list[i].TotalMoneyValue.ToString(), dispathWay });
            }



            //rptData.ShowRptDialog(new AFC.WS.UI.UIPage.CashManager.CrystalCashInOutReport(), dict, dt);
        }

       /* void btnSolution_Click(object sender, RoutedEventArgs e)
        {
            operatorCode = OperationCode.Cash_Solution;
            cashOutSoulution();
        }*/

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

        /// <summary>
        /// 现金调出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            operatorCode = OperationCode.Cash_Call_Out;
            cashOutSoulution();
        }

        private void cashOutSoulution()
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            dpaction.subAction = new AFC.WS.ModelView.Actions.CashManager.CashCallOutAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].CashTypeCode, value = list[i].CashNumber });
            }
            listQueryCondition.Add(new QueryCondition { bindingData = "operatorCode", value = operatorCode});
            if (dpaction.CheckValid(listQueryCondition))
            {
                dpaction.DoAction(listQueryCondition);
                if (this.isPrint.IsChecked == true)
                {
                    printCrystal();
                }

                this.list.Clear();
                InitControls();
            }
        }

        /// <summary>
        /// 删除现金调入列表
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
                MessageDialog.Show("请输入调出金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            string expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
            if (!Regex.IsMatch(this.txtCallInMonNo.Text, expression, RegexOptions.Compiled))
            {
                MessageDialog.Show("只能输入金额。金额范围0.00-9999999.99", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            double intCallInMonNo = double.Parse(this.txtCallInMonNo.Text);
            double stroageNo = double.Parse(this.txtMonNo.Text);
            if (intCallInMonNo > stroageNo)
            {
                MessageDialog.Show("调出金额不能大于在金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            return true;
        }

       /* private void txtCallInMonNo_KeyUp(object sender, KeyEventArgs e)
        {
            TextBoxExtend tb = sender as TextBoxExtend;
            try
            {
                int result = int.Parse(tb.Text);
                CashStorageInfo csInfo = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(this.currentSelected.currency_code);
                if (csInfo != null)
                {

                    this.txtCallInMoneyValue.Text = (result * this.currentSelected.currency_value.ConvertNumberStringToUint()).ToString();
                }
            }
            catch (Exception ex)
            {
                this.txtCallInMoneyValue.Text = string.Empty;
                //todo:log here
            }
        }*/

        public override void InitControls()
        {
            this.isPrint.IsChecked = false;
            List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
            string multiMoneyName = list.Single(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
            this.cmbMoneyType.ItemsSource = list;
            this.cmbMoneyType.DisplayMemberPath = "currency_name";
            this.cmbMoneyType.SetControlValue(multiMoneyName);
            this.cmbMoneyType.CanReadOnly = true;
            this.cmbMoneyType.IsEditable = false;


            this.labTotal.Content = string.Empty;
            this.labTotalMoney.Content = string.Empty;
            string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            string localStationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            Wrapper.FullComboBox(this.cmbStation, BuinessRule.GetInstace().GetAllStationInfo(lineID, localStationID), "station_cn_name", "station_id", true);

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

            //base.UnLoadControls();
        }

    }
}
