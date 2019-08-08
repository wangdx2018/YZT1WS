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
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.ModelView.Convertors;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.CommonActions;
    
    /// <summary>
    /// RfidLabelInit.xaml 的交互逻辑
    /// </summary>
    public partial class RfidLabelInit : UserControlBase
    {
        /// <summary>
        /// action处理参数信息 
        /// actionParams 参数共有两个
        /// boxType
        /// boxId
        /// </summary>
        private List<QueryCondition> actionParams = new List<QueryCondition>();

        public RfidLabelInit()
        {
            InitializeComponent();
            this.cmbRfidType.SelectionChanged += new SelectionChangedEventHandler(cmbRfidType_SelectionChanged);
            this.cmbBoxType.SelectionChanged += new SelectionChangedEventHandler(cmbBoxType_SelectionChanged);
            this.btnConnect.Click += new RoutedEventHandler(ButtonClicked);
            this.btnInit.Click += new RoutedEventHandler(ButtonClicked);
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

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnConnect")
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
            else
            {
                if (this.txtBoxId.Text.Equals("0000"))
                {
                    MessageDialog.Show("票箱编号需要从0001-9999!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return;
                }

                ComboBoxItem cbi = this.cmbBoxType.SelectedItem as ComboBoxItem;
                if (cbi != null)
                {
                    //if (cbi.Tag.ToString() == "21" ||
                    //    cbi.Tag.ToString() == "22")
                    //{
                    //    MessageDialog.Show("纸币钱箱不能在此初始化!", "提示", MessageBoxIcon.Warning, MessageBoxButtons.Ok);
                    //    return;
                    //}
                }

                

                this.AddQueryConditionData(new QueryCondition { bindingData = "boxId", value = this.txtBoxType.Text + this.txtBoxId.Text });
                IAction action = new AFC.WS.ModelView.Actions.TicketBoxManager.RFIDInitAction();
                if (action.CheckValid(actionParams))
                {
                  ResultStatus status= action.DoAction(actionParams);
                  if (status == null)
                  {
                      labTip.Content = "初始化标签失败";
                      return;
                  }
                  else
                  {
                      labTip.Content = "初始化标签成功";
                      return;
                  }

                }
            }
            
        }

        private void cmbBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            ComboBoxItem selection = cb.SelectedItem as ComboBoxItem;
            if (selection != null)
            {
                this.txtBoxType.Text = SysConfig.GetSysConfig().LocalParamsConfig.LineCode + selection.Tag.ToString();
                this.txtBoxId.Text = string.Empty;
                this.AddQueryConditionData( new QueryCondition { bindingData = "boxType", value = this.txtBoxType.Text });
            }
            else
            {
                this.txtBoxType.Text = string.Empty;
            }
        }

        public void cmbRfidType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = this.cmbBoxType;
            cb.Items.Clear();
            if (e.AddedItems == null || e.AddedItems.Count == 0)
                return;
            if (e.AddedItems[0].ToString() == "票箱RFID")
            {
              cb.Items.Add(CreateComboxItem("01","发票箱"));
              cb.Items.Add(CreateComboxItem("02", "废票箱"));
              cb.Items.Add(CreateComboxItem("03", "回收箱"));
            }
            else
            {
                cb.Items.Add(CreateComboxItem("11","硬币回收箱"));
                cb.Items.Add(CreateComboxItem("21", "纸币补充箱"));
                cb.Items.Add(CreateComboxItem("22", "纸币回收箱")); 
            }
        }

        public override void InitControls()
        {
            this.cmbRfidType.Items.Add("票箱RFID");
            this.cmbRfidType.Items.Add("钱箱RFID");
            this.txtBoxId.Initialize();
        }

        public override void UnLoadControls()
        {
            this.cmbRfidType.Items.Clear();
            this.txtBoxId.Text = string.Empty;
            this.txtBoxType.Text = string.Empty;
            this.cmbBoxType.Items.Clear();
            this.cmbBoxType.Text = string.Empty;
            this.cmbRfidType.Text = string.Empty;
            this.labTip.Content = string.Empty;
            this.actionParams.Clear();
            //base.UnLoadControls();
        }

        private ComboBoxItem CreateComboxItem(string key, string text)
        {
            ComboBoxItem cbi = new ComboBoxItem();
            cbi.Tag = key;
            cbi.Content = text;
            return cbi;
        }

    }
}
