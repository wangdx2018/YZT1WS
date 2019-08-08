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
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;
using AFC.WS.UI.RfidRW;
using AFC.WS.UI.Common;
using AFC.WS.UI.DataSources;
using AFC.WS.ModelView.Convertors;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.UIPage.TicketBoxManager;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;

namespace AFC.WS.UI.UIPage.TickMonyBoxManager
{
    /// <summary>
    /// MoneyBoxRegister.xaml 的交互逻辑
    /// </summary>
    public partial class MoneyBoxRegister : UserControlBase
    {
        private  MoneyBoxRFID rfid;
        private List<QueryCondition> list = new List<QueryCondition>();

        public MoneyBoxRegister()
        {
            InitializeComponent();
            this.txtMoneyBoxID.KeyUp += new KeyEventHandler(txtMoneyBoxID_KeyUp);
        }
        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
            MessageManager.SubscribeMessage(this, "ReadMoneyRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);
            this.mbCallOut.InitControls();
        }

        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            try
            {
               // this.txtRFID.IsEnabled = false;
                this.lblMessage.Content = "";

                Wrapper.Instance.InitControlValue<TextBoxExtend, GroupBox>(gbMoneyBoxRegister);
                Wrapper.FullComboBox(this.cbbMoenyBoxType, TickMonyBoxHelp.Instance.GetMoneyCashTypeList(), "name", "code",false);
                InitLoadRegistInfo();
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 钱箱ID按下。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void txtMoneyBoxID_KeyUp(object sender, KeyEventArgs e)
        {
            string moneyBoxId = this.txtMoneyBoxID.Text.Trim();
            if (moneyBoxId.Length >= 8)
            {
                string moneyBoxType = moneyBoxId.Substring(2, 2);
                Wrapper.ComboBoxSelectedItem(this.cbbMoenyBoxType, moneyBoxType);
                if (!moneyBoxType.ToString().Equals("11"))
                {
                    this.txtRFID.Text = "FFFFFFFF";
                }

            }
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_cash_money_box_reg_info");
            this.Focus();
            btnReset_Click(null, null);
            base.UnLoadControls();
            RfidRW.RfidReadAsynHandle.AbortAsynHandle();

            this.mbCallOut.UnLoadControls();
        }

        /// <summary>
        /// 加载钱箱登记信息。
        /// </summary>
        private void InitLoadRegistInfo()
        {
            try
            {
                string currentDir = Util.GetCurrentApplicationDirectory;
                DataListControl dlcMD = new DataListControl();
                int result = dlcMD.Initliaize(currentDir + @".\RuleFiles\TickMonyBoxManager\list_cash_money_box_reg_info.xml");
                if (result == 0)
                {
                    this.gbMoneyBoxInfo.Content = dlcMD;
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

               /// <summary>
        /// 重置内容。 
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.txtRFID.Text = "";
            this.txtMoneyBoxID.Text = "";
            this.cbbMoenyBoxType.SelectedIndex = 0;
            this.rfid = null;

        }

        /// <summary>
        /// 获取钱箱的RFID信息。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnReadRFIDInfo_Click(object sender, RoutedEventArgs e)
        {
            RfidRW.RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidRW.MoneyBoxRFID));
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
            if (msg.MessageType == RfidRW.RfidReadAsynHandle.Finish_Read_Rfid)
            {
                RfidRW.RfidReadAsynHandle.AbortAsynHandle();
                this.rfid = msg.Content as RfidRW.MoneyBoxRFID;
                TicketOrMoneyBoxIdConvetor covertToDecimal = new TicketOrMoneyBoxIdConvetor();
                if (rfid != null && rfid.moneyBoxId.JudgeIsNullOrEmpty() == false)
                {
                    string moneyID = covertToDecimal.Convert(rfid.moneyBoxId.ToString().PadLeft(8, '0'), null, null, null).ToString();
                    //20120910 修改原因纸币钱箱RFID可读
                    //不是硬币钱箱
                    /*if (!moneyID.Substring(2, 2).Equals("11"))
                    {
                        this.rfid = null;

                        return;
                    }*/
                    this.txtMoneyBoxID.Text = covertToDecimal.Convert(rfid.moneyBoxId.ToString().PadLeft(8, '0'),null,null,null).ToString();
                    this.txtRFID.Text = BuinessRule.GetInstace().rfidRw.GetRFIDPhysicalId(1);
                    this.lblMessage.Content = Wrapper.Instance.GetRfidSuccessMessageInfo();
                    string moneyBoxTypeCode = this.txtMoneyBoxID.Text.Substring(2, 2);
                    Wrapper.ComboBoxSelectedItem(this.cbbMoenyBoxType, moneyBoxTypeCode);

                }
                else
                {
                    lblMessage.Content = Wrapper.Instance.GetRfidSuccessMessageInfo();
                    btnReset_Click(null, null);
                }
            }
        }


        public override void CancleSubscribeSynMessage()
        {
            MessageManager.CancelAllSubscribeMessage(RfidRW.RfidReadAsynHandle.Finish_Read_Rfid);
        }

        private void btnMoneyBoxRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TicketOrMoneyBoxIdConvetor covertHex = new TicketOrMoneyBoxIdConvetor();
                Wrapper.Instance.AddQueryConditionToList(list, "moneyBoxID", covertHex.ConvertBack(this.txtMoneyBoxID.Text,null,null,null).ToString());
                Wrapper.Instance.AddQueryConditionToList(list,"moneyBoxRFID", this.txtRFID.Text);
                Wrapper.Instance.AddQueryConditionToList(list,"moenyBoxType", Wrapper.GetComboBoxUid(cbbMoenyBoxType));
                Wrapper.Instance.AddQueryConditionToList(list,"rfid", rfid);


                IAction action = new AFC.WS.ModelView.Actions.TickMonyBoxManager.MoneyBoxRegisterAction();
                if (action.CheckValid(list))
                {
                    action.DoAction(list);
                }

                btnReset_Click(sender,e);
                InitLoadRegistInfo();

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

    }

}
