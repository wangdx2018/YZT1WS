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

namespace AFC.WS.UI.UIPage.DataManager
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
    public partial class TimeTaskExcuteUpdate : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        string station_cn_id = "";

        public TimeTaskExcuteUpdate()
        {
            InitializeComponent();
            this.task_enable.SelectionChanged += new SelectionChangedEventHandler(task_is_effect_SelectionChanged);
            this.exec_method.SelectionChanged += new SelectionChangedEventHandler(task_execute_method_SelectionChanged);
        }

        public override void InitControls()
        {
   
            List<QueryCondition> list1 = this.Tag as List<QueryCondition>;
            this.task_name.Text = list1.Single(temp => temp.bindingData.Equals("t.task_name")).value.ToString();

            string task_enable_name = list1.Single(temp => temp.bindingData.Equals("t.task_enable")).value.ToString();
            if (task_enable_name == "不启用") { this.task_enable.Tag = "0"; this.task_enable.SelectedIndex = 0; }
            else if (task_enable_name == "启用") { this.task_enable.Tag = "1"; this.task_enable.SelectedIndex = 1; }

            this.start_exec_date.Text = list1.Single(temp => temp.bindingData.Equals("t.start_exec_date")).value.ToString();

            string exec_method_name = list1.Single(temp => temp.bindingData.Equals("t.exec_method")).value.ToString();
            if (exec_method_name == "任务轮询执行") { this.exec_method.Tag = "1"; this.exec_method.SelectedIndex = 0; }
            else if (exec_method_name == "每天执行一次") { this.exec_method.Tag = "2"; this.exec_method.SelectedIndex = 1; }
            else if (exec_method_name == "每周执行一次") { this.exec_method.Tag = "3"; this.exec_method.SelectedIndex = 2; }
            else if (exec_method_name == "每月执行一次") { this.exec_method.Tag = "4"; this.exec_method.SelectedIndex = 3; }
            else if (exec_method_name == "每年执行一次") { this.exec_method.Tag = "5"; this.exec_method.SelectedIndex = 4; }


            this.start_exec_week.Text = list1.Single(temp => temp.bindingData.Equals("t.start_exec_week")).value.ToString();
            this.start_exec_time.Text = list1.Single(temp => temp.bindingData.Equals("t.start_exec_time")).value.ToString();
            this.exec_interval.Text = list1.Single(temp => temp.bindingData.Equals("t.exec_interval")).value.ToString();
            
           
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "task_name", this.task_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_enable", this.task_enable.Tag.ToString());            
            Wrapper.Instance.AddQueryConditionToList(list, "start_exec_date", this.start_exec_date.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "start_exec_week", this.start_exec_week.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "start_exec_time", this.start_exec_time.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "exec_interval", this.exec_interval.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "exec_method", this.exec_method.Tag.ToString());
            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.UpdateTimeTaskExcute();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //this.task_id.Text = string.Empty;
            this.task_name.Text = string.Empty;
            this.task_enable.Text = string.Empty;
            this.exec_method.Text = string.Empty;
            this.start_exec_date.Text = string.Empty;
            this.start_exec_week.Text = string.Empty;
            this.start_exec_time.Text = string.Empty;
            this.exec_interval.Text = string.Empty;
        }

        private void task_execute_method_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.exec_method.Tag = value;
            }
            catch (Exception ex)
            {

            }
        }

      

        private void task_is_effect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.task_enable.Tag = value;
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
