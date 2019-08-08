using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using AFC.BOM2.UIController;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Convertors;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.BOM2.MessageDispacher;

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    /// <summary>
    /// PreTickCheckIn.xaml 的交互逻辑
    /// </summary>
    public partial class PreTickCheckIn : UserControlBase
    {

        private BaseWindow BW;
        /// <summary>
        /// 票卡操作数据集合
        /// </summary>
        private ObservableCollection<TickManaProductData> list = new ObservableCollection<TickManaProductData>();

        private string operatorID;

        public PreTickCheckIn()
        {
            InitializeComponent();
            this.cmbTickStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbTickStoreType_SelectionChanged);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.AddTickReturn, SynMessageType.Add_Tick_Return, HandleMode.Syn, true);
            this.Loaded += new RoutedEventHandler(PreTickCheckIn_Loaded);
        }

        void PreTickCheckIn_Loaded(object sender, RoutedEventArgs e)
        {
            InitControls();
        }

          public PreTickCheckIn(BaseWindow bw)
              : this()
          {
              this.BW = bw;
          }
        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            Message msg = new Message();
            msg.MessageType = SynMessageType.Add_Tick_Finish;
            msg.Content = this.list.ToList();
            MessageManager.SendMessasge(msg);
           if(BW!=null)
           {
               BW.Close();
           }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsChecked)
                {
                    list.Remove(list[i]);
                }
            }
            UpdateTotalCount();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //领用张数
            int inStoreNum;
            //归还张数
            int checkOutNum;

            bool res = int.TryParse(this.txtTickNo.Text, out inStoreNum);
            if (!res)
            {
                MessageDialog.Show("票务室库存张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            res = int.TryParse(this.txtRealNo.Text, out checkOutNum);
            if (!res)
            {
                MessageDialog.Show("输入张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.CheckUIValid(inStoreNum, checkOutNum))
            {
                TickValuedProductInfo btmt = this.cmbTickStoreType.SelectedItem as TickValuedProductInfo;
                ConvertSimpleFenToYuan convertYuan = new ConvertSimpleFenToYuan();
                if (btmt != null)
                {
                    TickManaProductData tod = new TickManaProductData
                                                  {
                                                      IsChecked = false,
                                                      TickNum = checkOutNum,
                                                      TickStoreTypeName = btmt.tick_mana_type_name,
                                                      TickStoreType = btmt.tick_mana_type,
                                                      OperatorId = operatorID,
                                                      UpdateTime = DateTime.Now.ToString("HH:mm:ss"),
                                                      UpdateDate = DateTime.Now.ToString("yyyy-MM-dd"),
                                                      CheckInMoney =
                                                          Convert.ToDecimal(convertYuan.Convert(btmt.tick_sale_value, null,
                                                                                              null, null))*(inStoreNum-checkOutNum)
                    };
                    this.AddTickManaTypeInfoToList(tod);
                    UpdateTotalCount();
                    this.txtRealNo.Text = string.Empty;
                }
            }
        }

        private void UpdateTotalCount()
        {
            int total = 0; ;
            decimal moneyTotal = 0;
            if (list.Count == 0)
            {
                this.labTotal.Content = string.Empty;
                this.labMoney.Content = string.Empty;
            }

            for (int i = 0; i < list.Count; i++)
            {
                total = list[i].TickNum + total;
                moneyTotal = list[i].CheckInMoney + moneyTotal;
                
            }
            this.labTotal.Content = total.ToString();
            this.labMoney.Content = moneyTotal.ToString();
        }

        private void cmbTickStoreType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.txtTickNo.Text = string.Empty;
            this.txtRealNo.Text = string.Empty;
            this.txtRealNo.IsEnabled = true;
            this.btnAdd.IsEnabled = true;

            ComboBox cb = sender as ComboBox;
            TickValuedProductInfo info = cb.SelectedItem as TickValuedProductInfo;
            if (info != null)
            {
                string type = info.tick_mana_type;
                if (!string.IsNullOrEmpty(type))
                {
                    TickInOperatorInfo tickInOperator = BuinessRule.GetInstace().tickMan.GetOperatorInTickNum(type, operatorID);
                    this.txtTickNo.Text = tickInOperator.ticket_in_hand.ToString();
                }
                else
                {
                    this.txtTickNo.Text = "0";
                }

            }
            //throw new NotImplementedException();
        }

        public override void InitControls()
        {
            List<TickValuedProductInfo> list = BuinessRule.GetInstace().GetTickValuedProductInfo();
            this.cmbTickStoreType.ItemsSource = list;
            this.cmbTickStoreType.DisplayMemberPath = "tick_mana_type_name";
            this.listView1.ItemsSource = this.list;
            UpdateTotalCount();
        }

        /// <summary>
        /// 检查UI输入是否合法
        /// </summary>
        /// <param name="instoreNum">当前库存张数</param>
        /// <param name="checkNum">调出数量</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckUIValid(int instoreNum, int checkNum)
        {
            //if (instoreNum < checkNum)
            //{
            //    MessageDialog.Show("归还张数必须小于领用张数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }

        public override void UnLoadControls()
        {
            this.list.Clear();
            //取消订阅
            MessageManager.CancelAllSubscribeMessage(SynMessageType.Add_Tick_Return);
            //base.UnLoadControls();
        }
        /// <summary>
        /// 增加库存关
        /// </summary>
        /// <param name="data"></param>
        private void AddTickManaTypeInfoToList(TickManaProductData data)
        {
            if (data == null)
            {
                WriteLog.Log_Error("param is null");
                return;
            }
            if (string.IsNullOrEmpty(data.TickStoreType))
            {
                WriteLog.Log_Error("tick store type is null or empty");
                return;
            }
            try
            {
                int  res = list.Count(temp => temp.TickStoreType.Equals(data.TickStoreType));
                if (res == 1)
                {
                    TickManaProductData last = list.Single(temp => temp.TickStoreType.Equals(data.TickStoreType));

                    last.TickNum = data.TickNum + last.TickNum;
                    last.CheckInMoney = data.CheckInMoney + last.CheckInMoney;

                }
                else
                    this.list.Add(data);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 订阅消息来时的处理
        /// </summary>
        /// <param name="msg"></param>
        public override void HandleAsynMessageForUI(Message msg)
        {
            switch (msg.MessageType)
            {
                case SynMessageType.Add_Tick_Return:
                    operatorID = msg.Content.ToString();
                    this.txtOperatorId.Text = operatorID;
                    PrivOperatorInfo operatorInfo = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(operatorID);
                    if (operatorInfo == null || string.IsNullOrEmpty(operatorInfo.operator_id))
                    {
                        MessageDialog.Show("请输入正确的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    }
                    else
                    {
                        this.txtOperatorName.Text = operatorInfo.operator_name;
                    }
               break;
            }
        }
    }
}
