using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AFC.BOM2.UIController;
using AFC.WS.UI.RfidRW;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR;
using AFC.WS.ModelView.Convertors;
using AFC.WS.Model.DB;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Config;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Common;
using AFC.WS.UI.UIPage.TicketBoxManager;

namespace AFC.WS.UI.UIPage.TickMonyBoxManager
{
    /// <summary>
    /// MoneyBoxAdd.xaml 的交互逻辑
    /// </summary>
    public partial class MoneyBoxAdd : UserControlBase
    {
        MoneyBoxRFID rfid = null;
        string moneyTypeCode = string.Empty;
        string moneyBoxID = string.Empty;
        string moneyNum = string.Empty;
        string deviceID = string.Empty;
        /// <summary>
        /// 选中的现金币种代码
        /// </summary>
        private BasiMoneyTypeInfo currentSelected = null;
        /// <summary>
        /// 钱箱信信列表。
        /// </summary>
        public MoneyBoxAdd()
        {
            InitializeComponent();
            this.cbbMoneyType.SelectionChanged += new SelectionChangedEventHandler(cbbMoneyType_SelectionChanged);
            this.txtDeviceID.SelectionChanged += new SelectionChangedEventHandler(txtDeviceID_SelectionChanged);
            this.btnReadRFIDInfo.Click += new RoutedEventHandler(btnReadRFID_Click);
            this.txtMoneyBoxID.KeyUp += new KeyEventHandler(txtMoneyBoxID_KeyUp);

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

        private void cbbMoneyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            BasiMoneyTypeInfo info = cb.SelectedValue as BasiMoneyTypeInfo;
            this.currentSelected = info;
        }


        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
            MessageManager.SubscribeMessage(this, "ReadMoneyRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);

            this.moneyBoxClear.InitControls();
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            btnCancel_Click(null, null);
            RfidRW.RfidReadAsynHandle.AbortAsynHandle();
            MessageManager.CancelAllSubscribeMessage(RfidReadAsynHandle.Finish_Read_Rfid);
            this.moneyBoxClear.UnLoadControls();
        }

        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            this.lblMessage.Content = "";
            List<BasiMoneyTypeInfo> list = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
            this.cbbMoneyType.ItemsSource = list;
            this.cbbMoneyType.DisplayMemberPath = "currency_name";

            this.cbbMoneyType.SelectedIndex = 0;
            this.cbbMoneyType.IsEnabled = false;
            this.cbbMoneyType.IsReadOnly = true;
            this.txtNumber2.Text = "0";
            this.txtNumber2.IsEnabled = false;

            this.txtOperatorID.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            tab.SelectedIndex = 0;
            //2012.12.25 dusj add begin
            string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            
            List<BasiDevInfo> tvmList  = BuinessRule.GetInstace().GetBasiDevInfo(stationID, "01");
            foreach (var tvm in tvmList)
            {
                tvm.device_name = "TVM" + tvm.device_id.Substring(6, 2).ConvertHexStringToUint().ToString();
            }
            this.txtDeviceID.ItemsSource = tvmList;
            this.txtDeviceID.DisplayMemberPath = "device_name";
            //2012.12.25 dusj add end
        }

        private void btnReadRFID_Click(object sender, RoutedEventArgs e)
        {
            //Message msg = new Message();
            //msg.MessageType = RfidReadAsynHandle.Finish_Read_Rfid;
            //msg.Content = new RfidRW.MoneyBoxRFID { MoneyBoxId = "03214444" };
            //MessageManager.SendMessasge(msg);
            RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidRW.MoneyBoxRFID));
        }





        #region --> key up

        void txtMoneyBoxID_KeyUp(object sender, KeyEventArgs e)
        {
            //if (rfid == null)
            //{
            string typeCode = this.txtMoneyBoxID.Text.Trim();
            if (typeCode.Length == 8)
            {
                string moneyType = typeCode.Substring(2, 2);
                if (moneyType.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币补充箱)
                {
                    this.txtNumber2.IsEnabled = true;
                }
                if (moneyType.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.硬币回收箱)
                {
                    if (this.rfid == null)
                    {
                        this.btnReadRFIDInfo.Focus();
                        if (Wrapper.ShowDialog("硬币钱箱未检测到RFID,请读RFID,否则会不写RFID。是否还要继续补充？", false) == MessageBoxResult.No)
                        {
                            return;

                        }
                    }
                }
                initGbInfo();
            }
            //}
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
            Wrapper.Instance.InitControlValue<TextBoxExtend, GroupBox>(gbBaseInfo);

            this.txtNumber2.Text = "0";
            this.txtNumber2.IsEnabled = false;
            this.txtOperatorID.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            this.txtDeviceID.SetControlValue(null);
            this.deviceID = string.Empty;
            this.rfid = null;
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
                    //20120910 修改原因纸币RFID可读
                    //不是硬币钱箱
                    /*if (!moneyID.Substring(2, 2).Equals("11"))
                    {
                        this.rfid = null;

                        return;
                    }*/
                    this.txtMoneyBoxID.Text =
                          covertToDecimal.Convert(rfid.moneyBoxId.ToString().PadLeft(8, '0'), null, null, null).
                              ToString();
                    if (moneyID.Substring(2, 2).Equals("11"))
                    {
                        //2012.12.25 dusj modify begin
                        //this.txtDeviceID.Text = covertToDecimal.Convert(rfid.deviceId, null, null, null).ToString();
                        string rfiDevice = "TVM"+covertToDecimal.Convert(rfid.deviceId, null, null, null).ToString().Substring(6, 2).ConvertHexStringToUint().ToString();
                        this.txtDeviceID.SetControlValue(rfiDevice);
                        //2012.12.25 dusj modify end
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


                    string moneyType = moneyID.Substring(2, 2);
                    if (moneyType.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币补充箱)
                    {
                        this.txtNumber2.IsEnabled = true;
                        this.txtNumber2.Text = "0";
                    }

                }
                else
                {
                    this.lblMessage.Content = Wrapper.Instance.GetRfidSuccessMessageInfo();
                    btnCancel_Click(null, null);
                }

                RfidReadAsynHandle.AbortAsynHandle();
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        public override void CancleSubscribeSynMessage()
        {
            MessageManager.CancelAllSubscribeMessage(RfidRW.RfidReadAsynHandle.Finish_Read_Rfid);

        }

        public void ClearRfidInfo()
        {
            txtMoneyBoxID.Text = string.Empty;
            //txtDeviceID.Text = string.Empty;
            txtDeviceID.SetControlValue(null);
            txtLocationState.Text = string.Empty;
            txtOperatorState.Text = string.Empty;
            txtInstallLocation.Text = string.Empty;
            txtLastOperatorTime.Text = string.Empty;
            txtMoneyTypeName.Text = string.Empty;
            txtTotalNumber.Text = string.Empty;
            txtTotalCash.Text = string.Empty;

        }


        public void initGbInfo()
        {
            TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
            CashBoxStatusInfo statusInfo = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(), null, null, null).ToString());
            this.txtInstallLocation.Text = TickMonyBoxHelp.Instance.GetMoneyBoxLocationState(statusInfo.box_position.ToHexNumber());
            this.txtLastOperatorTime.Text = statusInfo.update_date + statusInfo.update_time;
            this.txtMoneyTypeName.Text = TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(statusInfo.currency_code);
            //dusj 20121105 modify begin 
            //this.txtTotalNumber.Text = statusInfo.currency_num.ToString();
            /////////////////////////////////
            //总金额
            //////////////////////////////////

            int moneyValue = TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(statusInfo.currency_code).currency_value.ToInt32();

            //this.txtTotalCash.Text = (statusInfo.currency_num * moneyValue).ToString();
            ConvertSimpleFenToYuan  convertToYuan =new ConvertSimpleFenToYuan();

            this.txtTotalCash.Text = convertToYuan.Convert(statusInfo.total_money_value, null, null, null).ToString();
            if (moneyValue == 0)
            {
                this.txtTotalNumber.Text = "0";
            }
            else
            {
                this.txtTotalNumber.Text =
                    (Convert.ToDecimal(convertToYuan.Convert(statusInfo.total_money_value, null, null, null))/
                     moneyValue).ToString();
            }

            this.txtNumber2.Text = "0";
        }

        private void btnPut_Click(object sender, RoutedEventArgs e)
        {
            moneyTypeCode = this.currentSelected.currency_code;
            moneyBoxID = this.txtMoneyBoxID.Text;
            moneyNum = this.txtNumber2.Text;

            TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "moneyBoxID", value = covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(), null, null, null) });
            list.Add(new QueryCondition { bindingData = "moneyType", value = moneyTypeCode });
            list.Add(new QueryCondition { bindingData = "moneyNum", value = moneyNum });

            list.Add(new QueryCondition { bindingData = "rfid", value = rfid });
            list.Add(new QueryCondition { bindingData = "operatorID", value = this.txtOperatorID.Text });
            //2012.12.25 dusj modify begin
            list.Add(new QueryCondition { bindingData = "deviceID", value = this.deviceID });
            //2012.12.25 dusj modify end 
            IAction action = new AFC.WS.ModelView.Actions.TickMonyBoxManager.MoneyBoxPutAction();
            if (action.CheckValid(list))
            {
                ResultStatus result = action.DoAction(list);
                if (result != null && result.resultCode == 0)
                {
                    btnCancel_Click(sender, e);
                }
            }



        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            if (tab != null)
            {
                if ((e.AddedItems.Count != 0) &&
                    ((e.AddedItems[0] as TabItem) != null))
                {
                    if (tab.SelectedIndex == 0)
                    {
                        this.ClearRfidInfo();
                        this.moneyBoxClear.UnLoadControls();
                        this.InitControls();
                    }
                    else
                    {
                        this.moneyBoxClear.ClearRfidInfo();
                        this.UnLoadControls();
                        this.moneyBoxClear.InitControls();
                        this.moneyBoxClear.SubscribeAsynMessage();
                    }
                }
            }
        }

    }
}
