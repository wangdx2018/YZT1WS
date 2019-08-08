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

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class Para4042DeviceInfoUpdate : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        string station_cn_id = "";

        public Para4042DeviceInfoUpdate()
        {
            InitializeComponent();
            this.start_flag.SelectionChanged += new SelectionChangedEventHandler(start_flag_SelectionChanged);
        }

        public override void InitControls()
        {
   
            List<QueryCondition> list1 = this.Tag as List<QueryCondition>;
            this.station_cn_name.Text = list1.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
            station_cn_id = list1.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();            
            this.device_id.Text = list1.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            this.station_hall_id.Text = list1.Single(temp => temp.bindingData.Equals("station_hall_id")).value.ToString();
            this.device_group_id.Text = list1.Single(temp => temp.bindingData.Equals("device_group_id")).value.ToString();
            this.device_serial_no.Text = list1.Single(temp => temp.bindingData.Equals("device_serial_no")).value.ToString();
            this.device_group_serial_no.Text = list1.Single(temp => temp.bindingData.Equals("device_group_serial_no")).value.ToString();
            this.honri_index.Text = list1.Single(temp => temp.bindingData.Equals("honri_index")).value.ToString();
            this.vertical_index.Text = list1.Single(temp => temp.bindingData.Equals("vertical_index")).value.ToString();
            this.display_angle.Text = list1.Single(temp => temp.bindingData.Equals("display_angle")).value.ToString();
            this.device_ip.Text = list1.Single(temp => temp.bindingData.Equals("device_ip")).value.ToString();
            string start_flag_name = list1.Single(temp => temp.bindingData.Equals("start_flag")).value.ToString();
            if (start_flag_name == "在用") { this.start_flag.Tag = "01"; this.start_flag.SelectedIndex = 0; }
            else if (start_flag_name == "停用") { this.start_flag.Tag = "02"; this.start_flag.SelectedIndex = 1; }
            else if (start_flag_name == "移除") { this.start_flag.Tag = "03"; this.start_flag.SelectedIndex = 2; }
            //Wrapper.ComboBoxSelectedItem(this.start_flag, this.start_flag.Tag.ToString());
            try
            {
                Wrapper.FullComboBox<BasiStationInfo>(this.station_cn_name, BuinessRule.GetInstace().GetAllStationInfo(), "station_cn_name", "station_id", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            Wrapper.ComboBoxSelectedItem(this.station_cn_name, station_cn_id);
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "station_cn_name", this.station_cn_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_id", this.device_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "station_hall_id", this.station_hall_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_group_id", this.device_group_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_serial_no", this.device_serial_no.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_group_serial_no", this.device_group_serial_no.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "honri_index", this.honri_index.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "vertical_index", this.vertical_index.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "display_angle", this.display_angle.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "device_ip", this.device_ip.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "start_flag", this.start_flag.Tag.ToString());
            dpaction.subAction = new AFC.WS.ModelView.Actions.ParamActions.UpdatePara4042DeviceInfo();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //this.station_cn_name.Text = string.Empty;
            //this.device_id.Text = string.Empty;
            this.station_hall_id.Text = string.Empty;
            this.device_group_id.Text = string.Empty;
            this.device_serial_no.Text = string.Empty;
            this.device_group_serial_no.Text = string.Empty;
            this.honri_index.Text = string.Empty;
            this.vertical_index.Text = string.Empty;
            this.display_angle.Text = string.Empty;
            this.device_ip.Text = string.Empty;
            this.start_flag.Text = string.Empty;
        }

        private void start_flag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.start_flag.Tag = value;
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
