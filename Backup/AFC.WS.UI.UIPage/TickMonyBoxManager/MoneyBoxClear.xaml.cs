using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AFC.BOM2.UIController;
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Config;
using AFC.WS.UI.RfidRW;
using AFC.WS.UI.Common;
using AFC.WS.ModelView.Convertors;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.UIPage.TicketBoxManager;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using Microsoft.Windows.Controls;

namespace AFC.WS.UI.UIPage.TickMonyBoxManager
{
    /// <summary>
    /// MoneyBoxOut.xaml 的交互逻辑
    /// 
    /// 钱箱领用。
    /// </summary>
    public partial class MoneyBoxClear : UserControlBase
    {
        MoneyBoxRFID rfid = null;
        string deviceID = string.Empty;
        /// <summary>
        /// 钱箱信信列表。
        /// </summary>
        public List<DisplayMoneyBox> MoneyBoxList = new List<DisplayMoneyBox>();

        public MoneyBoxClear()
        {
            InitializeComponent();
            this.txtFact1Coin.KeyUp += new KeyEventHandler(txtFact1Coin_KeyUp);
            this.txtFact1.KeyUp += new KeyEventHandler(txtFact1_KeyUp);
            this.txtFact10.KeyUp += new KeyEventHandler(txtFact10_KeyUp);
            this.txtFact5.KeyUp += new KeyEventHandler(txtFact5_KeyUp);
            this.txtMoneyBoxID.KeyUp += new KeyEventHandler(txtMoneyBoxID_KeyUp);
            this.txtFact20.KeyUp += new KeyEventHandler(txtFact20_KeyUp);
            this.txtFact50.KeyUp += new KeyEventHandler(txtFact50_KeyUp);
            this.txtFact100.KeyUp += new KeyEventHandler(txtFact100_KeyUp);
            this.txtFact00.KeyUp += new KeyEventHandler(txtFact00_KeyUp);
            this.txtDeviceID.SelectionChanged += new SelectionChangedEventHandler(txtDeviceID_SelectionChanged);
        }


        public override void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this, "ReadMoneyRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);
            //base.SubscribeAsynMessage();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
            //MessageManager.SubscribeMessage(this, "ReadMoneyRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            MoneyBoxList.Clear();
            btnCancel_Click(null, null);
            RfidRW.RfidReadAsynHandle.AbortAsynHandle();
            MessageManager.CancelAllSubscribeMessage(RfidReadAsynHandle.Finish_Read_Rfid);
        }

        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            this.lblMessage.Content = "";

            MoneyBoxList.Clear();
           


            this.txtFact1Coin.IsEnabled = false;
            this.txtFact5.IsEnabled = false;
            this.txtFact10.IsEnabled = false;
            this.txtFact1.IsEnabled = false;
            this.txtFact100.IsEnabled = false;
            this.txtFact20.IsEnabled = false;
            this.txtFact50.IsEnabled = false;
            this.txtFact00.IsEnabled = false;

            string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;

            List<BasiDevInfo> tvmList = BuinessRule.GetInstace().GetBasiDevInfo(stationID, "01");
            foreach (var tvm in tvmList)
            {
                tvm.device_name = "TVM" + tvm.device_id.Substring(6, 2).ConvertHexStringToUint().ToString();
            }
            this.txtDeviceID.ItemsSource = tvmList;
            this.txtDeviceID.DisplayMemberPath = "device_name";

            this.txtOperatorID.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;

        }

        void txtDeviceID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            BasiDevInfo info = cb.SelectedValue as BasiDevInfo;
            if (info != null)
            {
                this.deviceID = info.device_id;
            }
        }




        #region --> key up

        void txtMoneyBoxID_KeyUp(object sender, KeyEventArgs e)
        {
            //if (rfid == null)
            //{
            string typeCode = this.txtMoneyBoxID.Text.Trim();
            if (typeCode.Length == 8)
            {
                typeCode = typeCode.Substring(2, 2);
                if (typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币回收箱 || typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币补充箱)
                {
                    this.txtFact1.IsEnabled = false;
                    this.txtFact10.IsEnabled = false;
                    this.txtFact5.IsEnabled = false;
                    this.txtFact100.IsEnabled = false;
                    this.txtFact20.IsEnabled = false;
                    this.txtFact50.IsEnabled = false;
                    this.txtFact1Coin.IsEnabled = false;
                    this.txtFact00.IsEnabled = true;
                    //多币种
                    this.txtFact00.Text = string.Empty;

                }
                else if (typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.硬币回收箱)
                {
                    if (this.rfid == null)
                    {
                        this.btnReadRFIDInfo.Focus();
                        if (Wrapper.ShowDialog("硬币钱箱未检测到RFID,请读RFID,否则会不写RFID。是否还要继续归还？", false) == MessageBoxResult.No)
                        {
                            return;
                            
                        }
                    }
                    this.txtFact1.IsEnabled = false;
                    this.txtFact10.IsEnabled = false;
                    this.txtFact5.IsEnabled = false;
                    this.txtFact100.IsEnabled = false;
                    this.txtFact20.IsEnabled = false;
                    this.txtFact50.IsEnabled = false;
                    this.txtFact00.IsEnabled = false;
                    this.txtFact1Coin.IsEnabled = true;
                    //多币种
                    this.txtFact1Coin.Text = string.Empty;
                 
                }
                //清空画面
                this.txtFact1Coin.Text = string.Empty;
                this.txtFact00.Text = string.Empty;
                this.txtTotalNumberFact.Text = string.Empty;
                this.txtTotalMoneyFact.Text = string.Empty;
                initGbInfo();
            }

        }

        void txtFact5_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }

        void txtFact10_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }

        void txtFact1_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }

        void txtFact1Coin_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }

        void txtFact100_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }

        void txtFact20_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }

        void txtFact50_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }
        void txtFact00_KeyUp(object sender, KeyEventArgs e)
        {
            CounterTotalCash();
            CounterTotalNumber();
            SetControlIsEnabled();
        }


        #endregion --> key up

        /// <summary>
        /// 取消输入信息。 
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Wrapper.Instance.InitControlValue<TextBoxExtend, GroupBox>(gbMoneyBoxRFIDInfo);
            Wrapper.Instance.InitControlValue<TextBoxExtend, GroupBox>(gbMoneyBoxInInfo);
            Wrapper.Instance.InitControlValue<TextBoxExtend, GroupBox>(gbBaseInfo);

            this.txtFact1.IsEnabled = false;
            this.txtFact10.IsEnabled = false;
            this.txtFact100.IsEnabled = false;
            this.txtFact5.IsEnabled = false;
            this.txtFact50.IsEnabled = false;
            this.txtFact20.IsEnabled = false;
            this.txtFact1Coin.IsEnabled = false;
            this.txtFact00.IsEnabled = false;

            this.txtDeviceID.SetControlValue(null);

            this.rfid = null;
            this.deviceID = string.Empty;
        }

        /// <summary>
        /// 读取RFID信息。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnReadRFIDInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RfidRW.RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidRW.MoneyBoxRFID));
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }


        private void moneyClearPro()
        {
            try
            {
                MoneyBoxInOrOutBody body = new MoneyBoxInOrOutBody();
                body.Body = MoneyTypeCodeBody();

                TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
                DoublePrimissionAction dpaction = new DoublePrimissionAction();
                List<QueryCondition> list = new List<QueryCondition>();
                list.Add(new QueryCondition { bindingData = "moneyBoxID", value = covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(), null, null, null) });
                list.Add(new QueryCondition { bindingData = "moneyBoxType", value = rfid });
                list.Add(new QueryCondition { bindingData = "operatorID", value = this.txtOperatorID.Text });
                list.Add(new QueryCondition { bindingData = "moneyBoxInOrOutBody", value = body });
                list.Add(new QueryCondition { bindingData = "rfid", value = rfid });
                //2011.8.8 add by dusj report print  begin
                list.Add(new QueryCondition { bindingData = "beforeMoney", value = this.txtTotalCash.Text }); //应还金额
                list.Add(new QueryCondition { bindingData = "totalMoney", value = this.txtTotalMoneyFact.Text }); //实还金额
                //2011.8.8 add by dusj end
                //2012.12.25 add by dusj begin
                list.Add(new QueryCondition { bindingData = "deviceID", value = this.deviceID}); //应还金额
                //2012.12.25 add by dusj end

                dpaction.subAction = new AFC.WS.ModelView.Actions.TickMonyBoxManager.MoneyBoxClearAction();
                dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                if (dpaction.CheckValid(list))
                {
                    ResultStatus result = dpaction.DoAction(list);
                    if (result != null && result.resultCode == 0)
                    {
                        btnCancel_Click(null, null);
                    }
                }


            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }


        /// <summary>
        /// 币种种类代码
        /// </summary>
        /// <returns>返回币种种类代码集合</returns>
        List<MoneyTypeCodeInfo> MoneyTypeCodeBody()
        {
            List<MoneyTypeCodeInfo> bodyList = new List<MoneyTypeCodeInfo>();

            MoneyTypeCodeInfo body = new MoneyTypeCodeInfo();

            //if (rfid == null)
            //{
            body.MoneyTypeCode = this.txtFact1.Uid.ToString();
            body.Cash = this.txtFact1.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact10.Uid.ToString();
            body.Cash = this.txtFact10.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact100.Uid.ToString();
            body.Cash = this.txtFact100.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact1Coin.Uid.ToString();
            body.Cash = this.txtFact1Coin.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);
            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact20.Uid.ToString();
            body.Cash = this.txtFact20.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact5.Uid.ToString();
            body.Cash = this.txtFact5.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact50.Uid.ToString();
            body.Cash = this.txtFact50.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }

            body = new MoneyTypeCodeInfo();
            body.MoneyTypeCode = this.txtFact00.Uid.ToString();
            body.Cash = this.txtFact00.Text.ToInt32();
            if (body.Cash >= 0)
            {
                bodyList.Add(body);

            }
            //}
            //else
            //{
            //body = new MoneyTypeCodeInfo();
            //body.MoneyTypeCode = this.txtFact1Coin.Uid.ToString();
            //body.Cash = this.txtFact1Coin.Text.ToInt32();
            //bodyList.Add(body);
            //}

            return bodyList;
        }

        /// <summary>
        /// 设置硬币控件是否可以输入
        /// </summary>
        private void SetControlIsEnabledCoin()
        {
            if (this.txtFact1Coin.Text.Trim().ToInt32() > 0)
            {
                this.txtFact1.IsEnabled = false;
                this.txtFact10.IsEnabled = false;
                this.txtFact100.IsEnabled = false;
                this.txtFact5.IsEnabled = false;
                this.txtFact50.IsEnabled = false;
                this.txtFact20.IsEnabled = false;
            }
            else
            {
                this.txtFact1.IsEnabled = true;
                this.txtFact10.IsEnabled = true;
                this.txtFact100.IsEnabled = true;
                this.txtFact5.IsEnabled = true;
                this.txtFact50.IsEnabled = true;
                this.txtFact20.IsEnabled = true;
            }
        }


        private void SetControlIsEnabled()
        {
            if (
                this.txtFact1.Text.Trim().ToInt32() > 0 ||
                this.txtFact10.Text.Trim().ToInt32() > 0 ||
                this.txtFact100.Text.Trim().ToInt32() > 0 ||
                this.txtFact5.Text.Trim().ToInt32() > 0 ||
                this.txtFact50.Text.Trim().ToInt32() > 0 ||
                this.txtFact20.Text.Trim().ToInt32() > 0
                )
            {
                this.txtFact1Coin.IsEnabled = false;
                this.txtFact00.IsEnabled = false;
            }
            if (this.txtFact00.Text.Trim().ToInt32() > 0)
            {
                this.txtFact1.IsEnabled = false;
                this.txtFact10.IsEnabled = false;
                this.txtFact100.IsEnabled = false;
                this.txtFact5.IsEnabled = false;
                this.txtFact50.IsEnabled = false;
                this.txtFact20.IsEnabled = false;
                this.txtFact1Coin.IsEnabled = false;
            }

        }

        /// <summary>
        /// 计算现金金额总数
        /// </summary>
        private void CounterTotalCash()
        {
            int result = TickMonyBoxHelp.Instance.GetMoneyValue(this.txtFact50) +
                TickMonyBoxHelp.Instance.GetMoneyValue(this.txtFact5) +
                TickMonyBoxHelp.Instance.GetMoneyValue(this.txtFact20) +
                this.txtFact1Coin.Text.ToInt32() +
                TickMonyBoxHelp.Instance.GetMoneyValue(this.txtFact100) +
                TickMonyBoxHelp.Instance.GetMoneyValue(this.txtFact10) +
                this.txtFact1.Text.ToInt32() + this.txtFact00.Text.ToInt32();
            this.txtTotalMoneyFact.Text = result.ToString();
        }

        /// <summary>
        /// 计算现金数量
        /// </summary>
        private void CounterTotalNumber()
        {//钱币总数量	2	HEX	纸币的张数或硬币的总张数或枚数。注3
            int result = 0;

            result = this.txtFact50.Text.ToInt32() +
            this.txtFact5.Text.ToInt32() +
            this.txtFact20.Text.ToInt32() +
            this.txtFact1Coin.Text.ToInt32() +
            this.txtFact100.Text.ToInt32() +
            this.txtFact10.Text.ToInt32() +
            this.txtFact00.Text.ToInt32() +
            this.txtFact1.Text.ToInt32();
            this.txtTotalNumberFact.Text = result.ToString();
        }

        private int CounterMoneyTypeTotal()
        {
            int result =
                this.txtFact5.Text.ToInt32() +
                this.txtFact1Coin.Text.ToInt32() +
                this.txtFact10.Text.ToInt32() +
                this.txtFact1.Text.ToInt32();
            return result.ToString().ToInt32();
        }

        private void btnRfidConnectTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowWindowAction action = new ShowWindowAction();
                action.Title = "RFID读写器连接测试";
                action.Width = 380;
                action.Height = 250;
                action.IsCheckNULL = false;
                action.ucb = new RFIDConnectTest();
                List<QueryCondition> list = new List<QueryCondition>();
                action.DoAction(list);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            try
            {
                if (msg.MessageType == RfidRW.RfidReadAsynHandle.Finish_Read_Rfid)
                {
                    TicketOrMoneyBoxIdConvetor covertToDecimal = new TicketOrMoneyBoxIdConvetor();
                    this.rfid = msg.Content as RfidRW.MoneyBoxRFID;
                    ConvertSimpleFenToYuan convertFenToYuan = new ConvertSimpleFenToYuan();
                    if (this.rfid == null)
                        return;
                    RfidRW.RfidReadAsynHandle.AbortAsynHandle();

                    /////////////////////////////////////////////
                    string moneyID = covertToDecimal.Convert(rfid.moneyBoxId.ToString().PadLeft(8, '0'), null, null, null).ToString();
                    //20120910 修改原因纸币钱箱可读RFID
                    //不是硬币钱箱
                    /*if (!moneyID.Substring(2, 2).Equals("11"))
                    {
                        this.rfid = null;

                        return;
                    }*/

                    this.txtMoneyBoxID.Text = covertToDecimal.Convert(rfid.moneyBoxId.ToString().PadLeft(8, '0'), null, null, null).ToString();


                    if (moneyID.Substring(2, 2).Equals("11"))
                    {
                        //2012.12.25 dusj modify begin
                        //this.txtDeviceID.Text = covertToDecimal.Convert(rfid.deviceId, null, null, null).ToString();
                        string rfiDevice = "TVM" + covertToDecimal.Convert(rfid.deviceId, null, null, null).ToString().Substring(6, 2).ConvertHexStringToUint().ToString();
                        this.txtDeviceID.SetControlValue(rfiDevice);
                        this.txtLocationState.Text =
                            TickMonyBoxHelp.Instance.GetMoneyBoxLocationState(rfid.moneyBoxLocationId);

                        Wrapper.Instance.ConsoleWriteLine("领用时读取RFID信息时，钱箱的状态值是：" + rfid.moneyBoxLocationId,
                                                          LogFlag.DebugFormat);

                        this.txtOperatorState.Text =
                            TickMonyBoxHelp.Instance.GetMoneyBoxOperateState(rfid.moneyBoxOperatorStatus);
                        this.txtInstallLocation.Text =
                            TickMonyBoxHelp.Instance.GetTicketMoneyBoxInstallPosition(rfid.setupLocation);
                        this.txtLastOperatorTime.Text = rfid.LastOperatorTime;
                        this.txtMoneyTypeName.Text =
                            TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(rfid.moneyCode.ToString("x2"));
                        this.txtTotalNumber.Text = rfid.moneyTotalNumber.ToString();
                        this.txtTotalCash.Text =
                            convertFenToYuan.Convert(rfid.moneyTotalCount.ToString(), null, null, null).ToString();

                    }
                    else
                    {
                        initGbInfo();
                    }
                    this.lblMessage.Content = Wrapper.Instance.GetRfidSuccessMessageInfo();

                    //2011.7.12 add begin
                    //如果是硬币钱箱一元硬币打开
                    //if (moneyID.Substring(2, 2).Equals("11"))
                    //{
                    //    this.txtFact00.IsEnabled = false;
                    //    this.txtFact1Coin.IsEnabled = true;
                    //}
                    //else
                    //{
                    this.txtFact00.IsEnabled = true;
                    this.txtFact1Coin.IsEnabled = false;
                    //}

                    //清空画面值
                    this.txtFact00.Text = string.Empty;
                    this.txtFact1Coin.Text = string.Empty;
                    this.txtTotalNumberFact.Text = string.Empty;
                    this.txtTotalMoneyFact.Text = string.Empty;

                }
                else
                {
                    this.lblMessage.Content = Wrapper.Instance.GetRfidSuccessMessageInfo();
                    btnCancel_Click(null, null);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }


        public void ClearRfidInfo()
        {
            txtMoneyBoxID.Text = string.Empty;
            //txtDeviceID.Text = string.Empty;
            txtLocationState.Text = string.Empty;
            txtOperatorState.Text = string.Empty;
            txtInstallLocation.Text = string.Empty;
            txtLastOperatorTime.Text = string.Empty;
            txtMoneyTypeName.Text = string.Empty;
            txtTotalNumber.Text = string.Empty;
            txtTotalCash.Text = string.Empty;
        }
        /// <summary>
        /// 填充钱箱明细列表。
        /// </summary>
        /// <param name="dg">DataGrid控件</param>
        /// <param name="mb">钱箱信息类</param>
        public void SetDataGridMoneyBoxList(DataGrid dg, DisplayMoneyBox mb)
        {

            if (dg == null || mb == null)
            {
                return;
            }
            MoneyBoxList.Add(mb);
            List<DisplayMoneyBox> temp = new List<DisplayMoneyBox>();
            foreach (var v in MoneyBoxList)
            {
                DisplayMoneyBox box = new DisplayMoneyBox();
                box.DeviceID = v.DeviceID;
                box.MoneyBoxID = v.MoneyBoxID;
                box.OperatorID = v.OperatorID;
                box.TotalCash = v.TotalCash;
                box.TotalNumber = v.TotalNumber;

                temp.Add(box);
            }
            dg.ItemsSource = temp.ToArray();
        }

        public override void CancleSubscribeSynMessage()
        {
            MessageManager.CancelAllSubscribeMessage(RfidRW.RfidReadAsynHandle.Finish_Read_Rfid);
            //base.CancleSubscribeSynMessage();
        }


        /// <summary>
        /// 钱箱清点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            moneyClearPro();
        }

        public void initGbInfo()
        {
          TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
            ConvertSimpleFenToYuan   converToYuan = new ConvertSimpleFenToYuan();
            CashBoxStatusInfo statusInfo = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(), null, null, null).ToString());
            this.txtInstallLocation.Text = TickMonyBoxHelp.Instance.GetMoneyBoxLocationState(statusInfo.box_position.ToHexNumber());
            this.txtLastOperatorTime.Text = statusInfo.update_date + statusInfo.update_time;
            this.txtMoneyTypeName.Text = TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(statusInfo.currency_code);
            //dusj 20121105 modifty begin 
            //this.txtTotalNumber.Text = statusInfo.currency_num.ToString();
            /////////////////////////////////
            //总金额
            //////////////////////////////////

            int moneyValue = TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(statusInfo.currency_code).currency_value.ToInt32();
           //this.txtTotalCash.Text = (statusInfo.currency_num * moneyValue).ToString();
            this.txtTotalCash.Text = converToYuan.Convert(statusInfo.total_money_value, null, null, null).ToString();
            if (moneyValue == 0)
            {
                this.txtTotalNumber.Text = "0";
            }
            else
            {
                this.txtTotalNumber.Text =
                    (Convert.ToDecimal(converToYuan.Convert(statusInfo.total_money_value, null, null, null))/moneyValue)
                        .ToString();
            }
            //dusj 20121105 modify end 
        }

    }
}
