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

namespace AFC.WS.UI.UIPage.TicketBoxManager
{
    using AFC.BOM2.UIController;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.TicketBoxManager;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    
    /// <summary>
    /// 票箱领用归还界面
    /// </summary>
    public partial class TicketBoxIn : UserControlBase
    {

        private List<QueryCondition> actionParams = new List<QueryCondition>();


        private List<TickBoxOperatorInfo> list = new List<TickBoxOperatorInfo>();

        private RfidTicketboxInfo info = null;

        public TicketBoxIn()
        {
            InitializeComponent();
            this.btnCheckIn.Click += new RoutedEventHandler(btnCheckIn_Click);
            this.btnCheckOut.Click += new RoutedEventHandler(btnCheckOut_Click);
            this.btnReadRFID.Click += new RoutedEventHandler(btnReadRFID_Click);
            this.btnReset.Click += new RoutedEventHandler(btnReset_Click);
            this.btnRfidConnect.Click += new RoutedEventHandler(btnRfidConnect_Click);
        }

        public override void InitControls()
        {
            this.rfidInfo.ClearRfidInfo();
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.TickBoxCheckInOrOut, RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Asyn, true);
        }

        /// <summary>
        /// 增加Action的参数
        /// </summary>
        /// <param name="qc">action的参数</param>
        private void AddQueryConditionData(QueryCondition qc)
        {
            if (qc == null)
            {
                WriteLog.Log_Error("qc is null ");
                return;
            }
            try
            {
                QueryCondition temp = this.actionParams.Single(a => a.bindingData.Equals(qc.bindingData));
                if (temp != null)
                {
                    temp.value = qc.value;
                }
                else
                {
                    this.actionParams.Add(qc);
                }
            }
            catch (Exception ex)
            {
                this.actionParams.Add(qc);
            }
        }

        private void btnRfidConnect_Click(object sender, RoutedEventArgs e)
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.rfidInfo.ClearRfidInfo();
        }

        private void btnReadRFID_Click(object sender, RoutedEventArgs e)
        {
            RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidTicketboxInfo));
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
       
            AddQueryConditionData(new QueryCondition { bindingData = "rfidInfo", value = info });
            IAction action = new TickBoxCheckOutAction();
            if (action.CheckValid(actionParams))
            {
                ResultStatus res=action.DoAction(actionParams);
                if (res != null &&
                 res.resultCode == 0 &&
                 Convert.ToInt32(res.resultData.ToString())== 0)
                {
                    BindingToList("领用");
                }
            }
        }

        /// <summary>
        /// 将票箱领用信息绑定到列表中
        /// </summary>
        private void BindingToList(string type)
        {
            // this.dgTicketBoxInInfo.AutoGenerateColumns = false;
            this.list.Add(new TickBoxOperatorInfo
            {
                ticketBoxId = this.info.TicketboxId,
                ticketBoxStaus = type,
                currentNumber = this.info.ticketNumber.ToString(),
                operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId,
                updateDate = DateTime.Now.ToString("yyyy年MM月dd日"),
                updateTime = DateTime.Now.ToString("HH:mm:ss")
            });
            AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor convetor = new AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor();
            var tempList = from aa in list
                           select new
                           {
                               票箱编码 = convetor.Convert(aa.ticketBoxId, null, null, null).ToString(),

                               操作员编码 = aa.operatorId,

                               票箱状态 = aa.ticketBoxStaus,

                               票箱张数 = aa.currentNumber,

                               更新时间 = aa.updateTime,

                               更新日期 = aa.updateDate
                           };

            this.dgTicketBoxInInfo.ItemsSource = tempList.ToList();
        }

        private void btnCheckIn_Click(object sender, RoutedEventArgs e)
        {
          
            AddQueryConditionData(new QueryCondition { bindingData = "rfidInfo", value = info });
            IAction action = new TickBoxCheckInAction();
            if (action.CheckValid(actionParams))
            {
              ResultStatus res=  action.DoAction(actionParams);
              if (res != null &&
                  res.resultCode == 0 &&
                  Convert.ToInt32(res.resultData.ToString()) == 0)
              {
                  BindingToList("归还");
              }
            }
        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == RfidReadAsynHandle.Finish_Read_Rfid)
            {
                info = msg.Content as RfidTicketboxInfo;
                this.rfidInfo.SetTicketBoxRfidInfo(info);
                RfidReadAsynHandle.AbortAsynHandle();
            }
        }

        public override void UnLoadControls()
        {
            RfidReadAsynHandle.AbortAsynHandle();
            MessageManager.CancelAllSubscribeMessage(RfidReadAsynHandle.Finish_Read_Rfid);
            info = null;
            this.actionParams.Clear();
            this.dgTicketBoxInInfo.ItemsSource = null;
            this.list.Clear();
        }


        
    }
}
