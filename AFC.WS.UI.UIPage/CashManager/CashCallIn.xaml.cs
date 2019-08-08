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
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using System.Collections.ObjectModel;
    using AFC.WS.ModelView.Actions.CommonActions;
    using System.Data;
    using AFC.WS.BR.TickMonyBoxManager;
    using System.Text.RegularExpressions;
    /// <summary>
    /// CashCallIn.xaml 的交互逻辑
    /// </summary>
    public partial class CashCallIn : UserControlBase
    {

        /// <summary>
        /// 调入现金列表
        /// </summary>
        private ObservableCollection<CashDetailsInfo> list = new ObservableCollection<CashDetailsInfo>();

        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;

        public CashCallIn()
        {
            InitializeComponent();
            this.cmbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cmbMoneyType_SelectionChanged);
            //this.txtCallInMonNo.KeyUp += new KeyEventHandler(txtCallInMonNo_KeyUp);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallIn.Click += new RoutedEventHandler(btnCallIn_Click);
            //this.btnPrint.Click += new RoutedEventHandler(btnPrint_Click);
            this.listView1.ItemsSource = list;
        }

        void printCrystal()
        {
            if (string.IsNullOrEmpty(Wrapper.GetComboBoxText(this.cmbStation)))
            {
                MessageDialog.Show("请选择调出车站!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            //AFC.WS.UI.UIPage.CashManager.CrystalRptData rptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();

            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("ReportTitle1", "现金调入");
            dict.Add("RequestBatchNo", TickMonyBoxHelp.Instance.GetSequenceNextVal().ToString());
            dict.Add("OperatorID", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            dict.Add("RequestLocationID", BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name);
            dict.Add("DispatchLocationID", Wrapper.GetComboBoxText(this.cmbStation));
            dict.Add("DispatchType", "调入");

            string dispathWay = Wrapper.GetComboBoxText(this.cmbStation) + "----->" + BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;


            DataTable dt = new DataTable();

            dt.Columns.Add("CashName", typeof(string));
            dt.Columns.Add("CashNum", typeof(string));
            dt.Columns.Add("CashTotalValue", typeof(string));
            dt.Columns.Add("DispatchWay", typeof(string));


            for (int i = 0; i < list.Count; i++)
            {
                dt.Rows.Add(new string[] { list[i].CashTypeName, list[i].CashNumber.ToString(), list[i].TotalMoneyValue.ToString(), dispathWay});
            }



            //rptData.ShowRptDialog(new AFC.WS.UI.UIPage.CashManager.CrystalCashInOutReport(),
            //    dict, dt);
        }

        /// <summary>
        /// 现金调入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallIn_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            dpaction.subAction = new AFC.WS.ModelView.Actions.CashManager.CashCallInAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].CashTypeCode, value = list[i].CashNumber });
            }

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
                //info.TotalMoneyValue = Convert.ToInt32(this.txtCallInMoneyValue.Text);
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
                MessageDialog.Show("请输入调入金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            //string expression = @"^[0-9]+(.[0-9]{2})?$";
            string expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
            if (!Regex.IsMatch(this.txtCallInMonNo.Text, expression, RegexOptions.Compiled))
            {
                MessageDialog.Show("只能输入金额。金额范围0.00-9999999.99", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            
            return true;
        }

        /*private void txtCallInMonNo_KeyUp(object sender, KeyEventArgs e)
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
            List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
            string multiMoneyName = list.Single(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
            this.isPrint.IsChecked = false;
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

            this.cashAdjust.InitControls();
            this.cashOut.InitControls();
           
        }

        private void UpdateTotalMoneyCountAndValue()
        {
            double totalCount=0;
            double totalValue = 0 ;
            for (int i = 0; i < list.Count; i++)
            {
                totalCount +=   list[i].CashNumber;
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

            this.cashOut.UnLoadControls();
            this.cashAdjust.UnLoadControls();
            //base.UnLoadControls();
        }
    }
}
