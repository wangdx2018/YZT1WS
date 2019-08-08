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
    /// MoneyBoxInOut.xaml 的交互逻辑
    /// </summary>
    public partial class MoneyBoxInOut : UserControlBase
    {
        MoneyBoxRFID rfid = null;
        /// <summary>
        /// 钱箱信信列表。
        /// </summary>
        public List<DisplayMoneyBox> MoneyBoxList = new List<DisplayMoneyBox>();
        string boxPosition = string.Empty;

        public MoneyBoxInOut()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
            MessageManager.SubscribeMessage(this, "ReadMoneyRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            MoneyBoxList.Clear();
            this.dgMoneyBoxOutInfo.ItemsSource = null;
            this.txtMoneyBoxID.KeyUp -= new KeyEventHandler(txtMoneyBoxID_KeyUp);
            btnCancel_Click(null, null);
            RfidRW.RfidReadAsynHandle.AbortAsynHandle();
        }

        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            this.lblMessage.Content = "";

            MoneyBoxList.Clear();
            this.txtMoneyBoxID.KeyUp += new KeyEventHandler(txtMoneyBoxID_KeyUp);

            this.txtFact1Coin.IsEnabled = false;
            this.txtFact5.IsEnabled = false;
            this.txtFact10.IsEnabled = false;
            this.txtFact1.IsEnabled = false;
            this.txtFact100.IsEnabled = false;
            this.txtFact20.IsEnabled = false;

            this.txtOperatorID.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;

             SetDataGridMoneyBoxList(this.dgMoneyBoxOutInfo, null);

        }

        #region --> key up

        void txtMoneyBoxID_KeyUp(object sender, KeyEventArgs e)
        {
            if (rfid == null)
            {
                string typeCode = this.txtMoneyBoxID.Text.Trim();
                if (typeCode.Length == 8)
                {
                    typeCode = typeCode.Substring(2, 2);
                    if (typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币回收箱 || typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币补充箱)
                    {
                        iniTxtFact();
                        
                    }
                }
            }
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

      
        private void  moneyInOutPro()
        {
            try
            {
                ResultStatus status =null;

                TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
                DoublePrimissionAction dpaction = new DoublePrimissionAction();
                List<QueryCondition> list = new List<QueryCondition>();
                list.Add(new QueryCondition { bindingData = "moneyBoxID", value = covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(), null, null, null) });
                list.Add(new QueryCondition { bindingData = "moneyBoxType", value = rfid });
                list.Add(new QueryCondition { bindingData = "operatorID", value = this.txtOperatorID.Text });
                list.Add(new QueryCondition { bindingData = "rfid", value = rfid });
                list.Add(new QueryCondition { bindingData = "boxPosition", value = boxPosition });
                IAction action  = new AFC.WS.ModelView.Actions.TickMonyBoxManager.MoneyBoxInAction();
                if (action.CheckValid(list))
                {

                    status = action.DoAction(list);
                }

                if (status!=null &&  status.resultCode == 0)
                {

                    DisplayMoneyBox mb = new DisplayMoneyBox();

                    mb.MoneyBoxID = this.txtMoneyBoxID.Text.Trim();
                    mb.OperatorID = this.txtOperatorID.Text.Trim();
                    if (rfid == null)
                    {
                        mb.TotalCash = this.txtTotalMoneyFact.Text.Trim();
                        mb.TotalNumber = this.txtTotalNumberFact.Text.Trim();

                    }
                    else
                    {
                        mb.TotalCash = this.txtTotalCash.Text.Trim();
                        mb.TotalNumber = this.txtTotalNumber.Text.Trim();

                    }

                    SetDataGridMoneyBoxList(this.dgMoneyBoxOutInfo, mb);
                }

              
                rfid = null;
                btnCancel_Click(null, null);
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
                    if (!moneyID.Substring(2, 2).Equals("11"))
                    {
                        this.rfid = null;
                        return;
                    }

                    txtMoneyBoxID.Text = covertToDecimal.Convert(rfid.moneyBoxId.ToString().PadLeft(8, '0'), null, null, null).ToString();
                    //txtDeviceID.Text = covertToDecimal.Convert(rfid.deviceId,null,null,null).ToString();
                    txtLocationState.Text = TickMonyBoxHelp.Instance.GetMoneyBoxLocationState(rfid.moneyBoxLocationId);

                    Wrapper.Instance.ConsoleWriteLine("领用时读取RFID信息时，钱箱的状态值是：" + rfid.moneyBoxLocationId, LogFlag.DebugFormat);

                    txtOperatorState.Text = TickMonyBoxHelp.Instance.GetMoneyBoxOperateState(rfid.moneyBoxOperatorStatus);
                    txtInstallLocation.Text = TickMonyBoxHelp.Instance.GetTicketMoneyBoxInstallPosition(rfid.setupLocation);
                    txtLastOperatorTime.Text = rfid.LastOperatorTime;
                    txtMoneyTypeName.Text = TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(rfid.moneyCode.ToString("x2"));
                    txtTotalNumber.Text = rfid.moneyTotalNumber.ToString();
                    txtTotalCash.Text = convertFenToYuan.Convert(rfid.moneyTotalCount.ToString(), null, null, null).ToString();
      

                    this.lblMessage.Content = Wrapper.Instance.GetRfidSuccessMessageInfo();
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
        /// 钱箱领用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            boxPosition = ((byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中).ToString("x2");
            moneyInOutPro();
        }

        /// <summary>
        /// 钱箱归还
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            boxPosition = ((byte)TickMonyBoxHelp.MoneyBoxPositionState.在库).ToString("x2");
            moneyInOutPro();
        }

        /// <summary>
        /// 初始化领用归还信息
        /// </summary>
        private void iniTxtFact()
        {
            DateTimeConvert date = new DateTimeConvert();
            ConvertToTime time = new ConvertToTime();
            TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
            ConvertSimpleFenToYuan convertFenToYuan = new ConvertSimpleFenToYuan();
            CashBoxStatusInfo statusInfo = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(covertHex.ConvertBack(this.txtMoneyBoxID.Text.Trim(),null,null,null).ToString());
            if (statusInfo != null)
            {
                txtLocationState.Text = TickMonyBoxHelp.Instance.GetMoneyBoxLocationState(statusInfo.box_position.ToByte());
                txtOperatorState.Text = "";
                txtInstallLocation.Text ="";
                txtLastOperatorTime.Text = statusInfo.update_date + statusInfo.update_time;
                txtMoneyTypeName.Text = TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(statusInfo.currency_code);
                txtTotalNumber.Text = statusInfo.currency_num.ToString();
                //////////////////////////////////////
                //总金额
                txtTotalCash.Text = convertFenToYuan.Convert(statusInfo.total_money_value, null, null, null).ToString();
            }
            //多币种只显示总数量和总金额
            switch (statusInfo.currency_code.ToUInt32())
            {
                //1元硬币
                case (11):
                    txtFact1Coin.Text = statusInfo.currency_num.ToString();
                    break;
                //1元纸币
                case (31):
                    txtFact1.Text = statusInfo.currency_num.ToString();
                    break;
                //2元纸币
                case (32):
                    txtFact1.Text = statusInfo.currency_num.ToString();
                    break;
                //5元纸币
                case (33):
                     this.txtFact5.Text = statusInfo.currency_num.ToString();
                    break;
                //10元纸币
                case (34):
                    this.txtFact10.Text = statusInfo.currency_num.ToString();
                    break;
                //20元纸币
                case (35):
                    this.txtFact20.Text=statusInfo.currency_num.ToString();
                    break;
                //50元纸币
                case (36):
                    this.txtFact50.Text = statusInfo.currency_num.ToString();
                    break;
                //100元纸币
                case (37):
                    this.txtFact100.Text = statusInfo.currency_num.ToString();
                    break;
                default:
                    break;
            }

            this.txtTotalNumberFact.Text = statusInfo.currency_num.ToString();
            this.txtTotalMoneyFact.Text = convertFenToYuan.Convert(statusInfo.total_money_value, null, null, null).ToString();
        }

    }
}
