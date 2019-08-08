using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AFC.BOM2.MessageDispacher;
using AFC.WS.Model.Const;
using AFC.WS.UI.UIPage.TickStoreManager;

namespace AFC.WS.UI.UIPage.CashManager
{
    using AFC.BOM2.UIController;
    using System.Collections.ObjectModel;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using Microsoft.Windows.Controls;
    using AFC.WS.UI.Config;
    using AFC.WS.BR.TickMonyBoxManager;
    using AFC.WS.UI.DataSources;
    using AFC.WS.ModelView.Convertors;
    using System.Text.RegularExpressions;
    using AFC.WS.ModelView.Actions.CommonActions;
    /// <summary>
    /// CashCheckIn.xaml 的交互逻辑
    /// </summary>
    public partial class CashCheckIn : UserControlBase
    {
        /// <summary>
        /// 现金操作数据集合
        /// </summary>
        private ObservableCollection<CashDetailsInfo> list = new ObservableCollection<CashDetailsInfo>();

        /// <summary>
        /// 结算数据集合
        /// </summary>
        private ObservableCollection<DataDevSettlementInfo> listSettleDate = new ObservableCollection<DataDevSettlementInfo>();

        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;

        private string selSettleDate = string.Empty;

        //结算日期控件
        private Microsoft.Windows.Controls.DatePicker dp = null;


        //自定义票卡归还信息
        private List<TickManaProductData> tickTypeInfo = null;


        public CashCheckIn()
        {
            InitializeComponent();

            this.cmbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cmbMoneyType_SelectionChanged);
            //this.txtCallInMonNo.KeyUp += new KeyEventHandler(txtCallInMonNo_KeyUp);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallIn.Click += new RoutedEventHandler(btnCallIn_Click);
            this.btnQuery.Click += new RoutedEventHandler(btnQuery_Click);
            this.btnReturnTick.Click += new RoutedEventHandler(btnReturnTick_Click);
            this.listView1.ItemsSource = list;
            this.listSettle.ItemsSource = listSettleDate;


        }

        void btnReturnTick_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.txtOperation.Text))
            {
                 MessageDialog.Show("请输入领用操作员ID", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            PrivOperatorInfo operatorInfo = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(this.txtOperation.Text.Trim());
            if (operatorInfo == null || string.IsNullOrEmpty(operatorInfo.operator_id))
            {
                MessageDialog.Show("请输入正确的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            //调用前先计算应还金额
            btnQuery_Click(sender, e);
          
            try
            {
                this.tickTypeInfo = null;
                BaseWindow w = new BaseWindow();
                w.Width = 650;
                w.Height = 500;
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                PreTickCheckIn checkIn = new PreTickCheckIn(w);
                Message msg = new Message();
                msg.MessageType = SynMessageType.Add_Tick_Return;
                msg.Content = this.txtOperation.Text;
                MessageManager.SendMessasge(msg);
                w.Content = checkIn;
                w.ShowDialog();

            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
          
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        public override void SubscribeSynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.AddTickFinish, SynMessageType.Add_Tick_Finish, HandleMode.Syn, true);
        }
        /// <summary>
        /// 现金归还
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtOperation.Text.Trim()))
            {
                MessageDialog.Show("请输入领用操作员ID", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            if (list == null || list.Count == 0)
            {
                MessageDialog.Show("没有现金明细信息，请添加现金明细", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            //dusj add begin 增加确定提示
            MessageBoxResult result = MessageDialog.Show(string.Format("操作员{0}归还金额为{1}，是否归还？", this.txtOperation.Text, this.labTotalMoney.Content), "确认", MessageBoxIcon.Question, MessageBoxButtons.YesNo);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            //dusj add end 增加确定提示
            //如果没有点击查询，先点查询
            if (string.IsNullOrEmpty(this.txtSettle.Text) && string.IsNullOrEmpty(this.txtOperationIn.Text) && string.IsNullOrEmpty(this.txtBomIn.Text))
            {
                btnQuery_Click(sender, e);
            }
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            dpaction.subAction = new AFC.WS.ModelView.Actions.CashManager.CashCheckInAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].CashTypeCode, value = list[i].CashNumber });
            }
            string operationCode = this.txtOperation.Text.Trim();
            //操作员编码
            listQueryCondition.Add(new QueryCondition { bindingData = "operationCode", value = operationCode });
            //运营日
            listQueryCondition.Add(new QueryCondition { bindingData = "settleDate", value = selSettleDate });
            //营收金额
            listQueryCondition.Add(new QueryCondition { bindingData = "receMoney", value = Convert.ToDecimal(string.IsNullOrEmpty(this.txtBomIn.Text) ? "0" : this.txtBomIn.Text.Trim()) });
            //备用金
            listQueryCondition.Add(new QueryCondition { bindingData = "operationInMoney", value = Convert.ToDecimal(string.IsNullOrEmpty(this.txtOperationIn.Text) ? "0" : this.txtOperationIn.Text.Trim()) });
            //20120828 dusj modify begin 增加硬币库存
            listQueryCondition.Add(new QueryCondition { bindingData = "coinNo", value = this.txtCoinNo.Text.Trim() });

            //20121022 dusj modify begin 增加对自定义散票的现金和票卡归还
            listQueryCondition.Add(new QueryCondition { bindingData = "tickTypeList", value =this.tickTypeInfo });
            //20122022 dusj modify end 增加对自定义散票的现金和票卡归还

            if (dpaction.CheckValid(listQueryCondition))
                dpaction.DoAction(listQueryCondition);

            //请空画面
            ClearCashCheckIn();
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
                MessageDialog.Show("请输入归还金额!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            //string expression = @"^\d{1,7}(?:\.\d{0,2}$|$)";
            //if (!Regex.IsMatch(this.txtCallInMonNo.Text, expression, RegexOptions.Compiled))
            //{
            //    MessageDialog.Show("只能输入金额。金额范围0.00-9999999.99", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
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
            this.TickCheckIn.InitControls();

            List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
            string multiMoneyName = list.Single(temp => temp.currency_code.Equals(TickMonyBoxHelp.MultipleCurrency)).currency_name;
            this.cmbMoneyType.ItemsSource = list;
            this.cmbMoneyType.DisplayMemberPath = "currency_name";
            this.cmbMoneyType.SetControlValue(multiMoneyName);
            this.cmbMoneyType.CanReadOnly = true;
            this.cmbMoneyType.IsEditable = false;



            this.labTotal.Content = string.Empty;
            this.labTotalMoney.Content = string.Empty;
            this.txtBomIn.Text = string.Empty;
            this.txtOperationIn.Text = string.Empty;
            this.txtSettle.Text = string.Empty;
            this.txtProstore.Text = string.Empty;
            this.txtCoinNo.Text = string.Empty;


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

        private void ClearCashCheckIn()
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
            this.txtBomIn.Text = string.Empty;
            this.txtOperationIn.Text = string.Empty;
            this.txtSettle.Text = string.Empty;
            this.txtOperation.Text = string.Empty;
            this.txtProstore.Text = string.Empty;
            this.txtCoinNo.Text = string.Empty;
            this.list.Clear();
            this.tickTypeInfo = null;
            this.listSettleDate.Clear();
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

            string operationCode = this.txtOperation.Text.Trim();
            listSettleDate.Clear();
            List<DataDevSettlementInfo> listTemp = BuinessRule.GetInstace().GetDevSettlementByData(operationCode, selSettleDate);

            decimal bomIn;

            ConvertSimpleFenToYuan covertToYuan = new ConvertSimpleFenToYuan();
            ConvertToTime covertToTime = new ConvertToTime();
            DateTimeConvert covertToDate = new DateTimeConvert();
            if (listTemp != null && listTemp.Count > 0)
            {
                foreach (DataDevSettlementInfo info in listTemp)
                {
                    info.cost_amount = Convert.ToDecimal(covertToYuan.Convert(info.cost_amount, null, null, null).ToString());
                    info.deposit_amount = Convert.ToDecimal(covertToYuan.Convert(info.deposit_amount, null, null, null).ToString());
                    info.fee_amount = Convert.ToDecimal(covertToYuan.Convert(info.fee_amount, null, null, null).ToString());
                    info.tran_amount = Convert.ToDecimal(covertToYuan.Convert(info.tran_amount, null, null, null).ToString());
                    info.settlement_date = covertToDate.Convert(info.settlement_date, null, null, null).ToString();
                    listSettleDate.Add(info);
                }

                bomIn = TickMonyBoxHelp.Instance.getSettleMoney(operationCode, selSettleDate);
            }

            else
            {
                bomIn = 0;
            }


            decimal operationIn = TickMonyBoxHelp.Instance.getOperationInMoney(operationCode, selSettleDate);
            //结算金额
            this.txtBomIn.Text = covertToYuan.Convert(bomIn, null, null, null).ToString();
            //备用金
            this.txtOperationIn.Text = covertToYuan.Convert(operationIn, null, null, null).ToString();
            //应还金额
            this.txtSettle.Text = covertToYuan.Convert((bomIn + operationIn), null, null, null).ToString();

            //20121022 dusj modify begin 自定义库存列表清空
            this.tickTypeInfo = null;
            this.txtProstore.Text = "0";
            //20121022 dusj modify end 自定义库存列表清空
        }
        /// <summary>
        /// 订阅消息来时的处理
        /// </summary>
        /// <param name="msg"></param>
        public override void HandleSynMessage(Message msg)
        {
             decimal totalMoney = 0;
            switch (msg.MessageType)
            {
                case SynMessageType.Add_Tick_Finish:
                this.tickTypeInfo = msg.Content as List<TickManaProductData>;
                if (tickTypeInfo != null && tickTypeInfo.Count > 0)
                 {
                     for (int i = 0; i < tickTypeInfo.Count; i++)
                     {
                         totalMoney = totalMoney +
                                      Convert.ToDecimal(tickTypeInfo[i].CheckInMoney);
                     }
                 }
                    this.txtProstore.Text = totalMoney.ToString();
                    //营收金额
                    decimal DecbomIn = string.IsNullOrEmpty(this.txtBomIn.Text) ? 0 : Convert.ToDecimal(this.txtBomIn.Text);
                    //备用金额
                    decimal DecOperatorIn = string.IsNullOrEmpty(this.txtOperationIn.Text) ? 0 : Convert.ToDecimal(this.txtOperationIn.Text);
                    //应还金额
                    this.txtSettle.Text = (totalMoney + DecbomIn + DecOperatorIn).ToString();
                break;
            }
        }

        public override void UnLoadControls()
        {
            this.list.Clear();
            this.listSettleDate.Clear();
            this.tickTypeInfo = null;
            this.currentSelected = null;
            this.txtOperation.Text = string.Empty;
            //this.txtCallInMoneyValue.Text = string.Empty;
            this.txtCallInMonNo.Text = string.Empty;
            this.labTotal.Content = string.Empty;
            this.labTotalMoney.Content = string.Empty;
            this.txtBomIn.Text = string.Empty;
            this.txtOperationIn.Text = string.Empty;
            this.txtSettle.Text = string.Empty;
            dp.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(dp_SelectedDateChanged);
            this.TickCheckIn.UnLoadControls();
            //取消订阅
            MessageManager.CancelAllSubscribeMessage(SynMessageType.Add_Tick_Finish);
        }
    }
}
