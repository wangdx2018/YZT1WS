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
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.CommonControls;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.ModelView.Convertors;
    using AFC.WS.BR;
    //-->票箱登记
    /// <summary>
    /// 负责人：王冬欣  最后修改日期：20100201
    /// 票箱登记 采用WS2.0基础组件
    /// </summary>
    public partial class TicketBoxRegister : UserControlBase
    {

        private Button btn = new Button();

        public TicketOrMoneyBoxIdConvetor convetor = new TicketOrMoneyBoxIdConvetor();

        public TicketBoxRegister()
        {
            InitializeComponent();
           
        }

        //-->初始化控件
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            tickBoxCallOut.InitControls();

            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\TickMonyBoxManager\UI_TicketboxRegister.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);

            }

            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\TickMonyBoxManager\list_tick_box_reg_info.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }


            UIElement element = ic.GetCommonControlByName("btnReadRfid");
            if (element != null)
            {
                btn = element as Button;
                btn.Click += new RoutedEventHandler(btn_Click);
             
            }

            MessageManager.SubscribeMessage(this, "ReadTicketRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            RfidRW.RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidRW.RfidTicketboxInfo));
        }

        public override void  HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == RfidRW.RfidReadAsynHandle.Finish_Read_Rfid)
            {
                int res = int.Parse(msg.MessageParam.ToString());
                if (res != 0)
                {
                   
                   // BRContext.Instance.tbm.HandleReadRFIDResult(res);
                    return;
                }
                string rifdLabel =BuinessRule.GetInstace().rfidRw.GetRFIDPhysicalId(1);
                TextBoxExtend txtRFid = ic.GetCommonControlByName("txtTicketBoxId") as TextBoxExtend;
                TextBoxExtend txtRFidLabel = ic.GetCommonControlByName("txtTicketboxRfid") as TextBoxExtend;
                txtRFid.Text = this.convetor.Convert((msg.Content as AFC.WS.UI.RfidRW.RfidTicketboxInfo).ticketboxId, null, null, null).ToString(); ;
                txtRFidLabel.Text = rifdLabel;
                RfidRW.RfidReadAsynHandle.AbortAsynHandle();
            }
            //RfidRW.RfidReadAsynHandle.AbortAsynHandle();
        }

      

        //--->卸载控件
        /// <summary>
        /// 卸载控件
        /// </summary>
        public override void UnLoadControls()
        {
            tickBoxCallOut.UnLoadControls();
            DataSourceManager.DisponseDataSource("ds_tick_box_reg_info");
            RfidRW.RfidReadAsynHandle.AbortAsynHandle();
            btn.Click -= new RoutedEventHandler(btn_Click);
            
        }


        public override void CancleSubscribeSynMessage()
        {
            MessageManager.CancelAllSubscribeMessage(RfidRW.RfidReadAsynHandle.Finish_Read_Rfid);
           
        }
    }
}
