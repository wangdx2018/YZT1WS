using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace AFC.WS.UI.UIPage.DataManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using System.Collections.ObjectModel;
    using AFC.WS.ModelView.Actions.CommonActions;
    using System.Data;
    using AFC.WS.BR.TickMonyBoxManager;
    using System.Windows.Forms;
    /// <summary>
    /// CashCallIn.xaml 的交互逻辑
    /// </summary>
    public partial class DateReUploadRecordsCom : UserControlBase
    {

        private List<QueryCondition> list = new List<QueryCondition>();
        string deviceId = "";
        string dataType = "";
        string localFileNum = "";
        string sendedFileNum = "";

        public DateReUploadRecordsCom()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            try
            {
                deviceId = list.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
                dataType = list.Single(temp => temp.bindingData.Equals("data_type")).value.ToString();
                localFileNum = list.Single(temp => temp.bindingData.Equals("local_file_num")).value.ToString();
                sendedFileNum = list.Single(temp => temp.bindingData.Equals("sended_file_num")).value.ToString();
                if (localFileNum.Equals(sendedFileNum))
                {
                    MessageDialog.Show("该设备全部数据已经上传", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                   BaseWindow bw=list.Single(temp => temp.bindingData.Equals("window")).value as BaseWindow;
                   bw.Close();
                }
            }
            catch
            {
                MessageDialog.Show("请选择要补传数据的设备", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Wrapper.Instance.AddQueryConditionToList(list, "deviceId", deviceId);
            Wrapper.Instance.AddQueryConditionToList(list, "dataType", dataType);
            Wrapper.Instance.AddQueryConditionToList(list, "tranDateBegin", Wrapper.GetDateTimePickerValue(tranDateBegin).ToString("yyyy-MM-dd"));
            Wrapper.Instance.AddQueryConditionToList(list, "tranDateEnd", Wrapper.GetDateTimePickerValue(tranDateEnd).ToString("yyyy-MM-dd"));
            IAction action = new AFC.WS.ModelView.Actions.DataManager.ReUploadRecordsAction();
            if (action.CheckValid(list))
            {
                action.DoAction(list);
            }
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Wrapper.SetDateTimePickerExtend(tranDateBegin, DateTimeType.Day, 0);
            Wrapper.SetDateTimePickerExtend(tranDateEnd, DateTimeType.Day, 0);
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
