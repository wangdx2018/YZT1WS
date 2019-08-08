using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using AFC.WS.BR.Maintenance;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;
using Microsoft.Windows.Controls;
using AFC.BOM2.UIController;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Convetors;
using AFC.WS.ModelView.Convertors;

namespace AFC.WS.UI.UIPage.Maintenance
{
    /// <summary>
    /// NoLabelPartsInout.xaml 的交互逻辑
    /// </summary>
    public partial class NoLabelPartsInout : UserControlBase
    {
    
          //部件状态信息
        public List<DisplayNoLabelParts> PartsList = new List<DisplayNoLabelParts>();

        string partsStatus = string.Empty;

        public NoLabelPartsInout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {

            this.txtPartsID.Text = string.Empty;
            this.PartsList.Clear();
            SetDataGridPartsList(this.dgPartsOutInfo, null);
        }

  
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtPartsID.Text = string.Empty;
            this.txtNum.Text = string.Empty;
            this.txtProvider.Text = string.Empty;
            this.txtLastOperatorTime.Text = string.Empty;
            this.txtLastOperatorDate.Text = string.Empty;
            this.txtInstoreNum.Text = string.Empty;
            this.txtParts.Text = string.Empty;
            this.txtOperatorID.Text = string.Empty;

        }
        private void btnTuneOut_Click(object sender, RoutedEventArgs e)
        {
            //调出
            partsStatus = "02";
            partsInOutPro();
        }
        /// <summary>
        /// 填充钱箱明细列表。
        /// </summary>
        /// <param name="dg">DataGrid控件</param>
        /// <param name="mb">钱箱信息类</param>
        public void SetDataGridPartsList(DataGrid dg, DisplayNoLabelParts mb)
        {

            if (dg == null || mb == null)
            {
                return;
            }
            PartsList.Add(mb);
            List<DisplayNoLabelParts> temp = new List<DisplayNoLabelParts>();
            foreach (var v in PartsList)
            {
                DisplayNoLabelParts part = new DisplayNoLabelParts();
                part.update_operator = v.update_operator;
                part.dev_part_cn_name = v.dev_part_cn_name;
                part.instore_num = v.instore_num;
                part.mc_dep_name = v.mc_dep_name;
                part.part_id = v.part_id;
                temp.Add(part);
            }
            dg.ItemsSource = temp.ToArray();
        }


        private void partsInOutPro()
        {
            try
            {
                ResultStatus status = null;

                List<QueryCondition> list = new List<QueryCondition>();
                list.Add(new QueryCondition { bindingData = "partsID", value = this.txtPartsID.Text.Trim() });
                list.Add(new QueryCondition { bindingData = "operatorID", value = this.txtOperatorID.Text });
                list.Add(new QueryCondition { bindingData = "partsStatus", value = partsStatus });
                list.Add(new QueryCondition { bindingData = "partsNum", value =this.txtNum.Text });
                list.Add(new QueryCondition { bindingData = "instoreNum", value = this.txtInstoreNum.Text});

                IAction action = new AFC.WS.ModelView.Actions.Maintenance.NoLabelPartsOutAction();
                if (action.CheckValid(list))
                {

                    status = action.DoAction(list);
                }

                if (status != null && status.resultCode == 0)
                {

                    DisplayNoLabelParts mb = new DisplayNoLabelParts();

                    mb.mc_dep_name = this.txtProvider.Text.Trim();
                    mb.dev_part_cn_name = this.txtParts.Text.Trim();
                    mb.instore_num = (this.txtInstoreNum.Text.ToInt32() - this.txtNum.Text.ToInt32()).ToString();
                    mb.part_id = this.txtPartsID.Text.Trim();
                    mb.update_operator = this.txtOperatorID.Text.Trim();
                    SetDataGridPartsList(this.dgPartsOutInfo, mb);
                    btnCancel_Click(null, null);
                }
                
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MaintainNoLablePartStore store = MaintenanceManager.Instance.GetNoLabelPartStoreById(this.txtPartsID.Text);
            PartStatusConvert convert = new PartStatusConvert();
            DateTimeConvert  dateConvert = new DateTimeConvert();
            ConvertToTime    timeConvert = new ConvertToTime();
            if (store != null && !string.IsNullOrEmpty(store.part_id))
            {
                this.txtOperatorID.Text = store.update_operator;
                this.txtLastOperator.Text = store.update_operator;
                this.txtParts.Text = store.part_type_id;
                this.txtProvider.Text = store.provider_id;
                this.txtLastOperatorTime.Text = timeConvert.Convert(store.update_time, null, null, null).ToString();
                this.txtInstoreNum.Text = store.instore_num.ToString();
                this.txtLastOperatorDate.Text = dateConvert.Convert(store.update_date, null, null, null).ToString();
            }
        }
    }
}
