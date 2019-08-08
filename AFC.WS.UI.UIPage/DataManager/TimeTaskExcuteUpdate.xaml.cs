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
            this.task_execute_way.SelectionChanged += new SelectionChangedEventHandler(task_execute_way_SelectionChanged);
            this.task_execute_method.SelectionChanged += new SelectionChangedEventHandler(task_execute_method_SelectionChanged);
            this.task_source.SelectionChanged += new SelectionChangedEventHandler(task_source_SelectionChanged);
            this.task_is_effect.SelectionChanged += new SelectionChangedEventHandler(task_is_effect_SelectionChanged);
            this.task_is_related_running_time.SelectionChanged += new SelectionChangedEventHandler(task_is_related_running_time_SelectionChanged);
        }

        public override void InitControls()
        {
   
            List<QueryCondition> list1 = this.Tag as List<QueryCondition>;
            this.task_id.Text = list1.Single(temp => temp.bindingData.Equals("task_id")).value.ToString();
            this.task_name.Text = list1.Single(temp => temp.bindingData.Equals("task_name")).value.ToString();

            string task_execute_way_name = list1.Single(temp => temp.bindingData.Equals("task_execute_way")).value.ToString();
            if (task_execute_way_name == "任务以函数调用") { this.task_execute_way.Tag = "1"; this.task_execute_way.SelectedIndex = 0; }
            else if (task_execute_way_name == "任务以fork子进程执行") { this.task_execute_way.Tag = "2"; this.task_execute_way.SelectedIndex = 1; }

            this.task_execute_funpro.Text = list1.Single(temp => temp.bindingData.Equals("task_execute_funpro")).value.ToString();
            
            string task_execute_method_name = list1.Single(temp => temp.bindingData.Equals("task_execute_method")).value.ToString();
            if (task_execute_method_name == "任务轮询执行") { this.task_execute_method.Tag = "1"; this.task_execute_method.SelectedIndex = 0; }
            else if (task_execute_method_name == "任务明天定点执行") { this.task_execute_method.Tag = "2"; this.task_execute_method.SelectedIndex = 1; }

            this.task_start_executing_time.Text = list1.Single(temp => temp.bindingData.Equals("task_start_executing_time")).value.ToString();
            this.task_end_executing_time.Text = list1.Single(temp => temp.bindingData.Equals("task_end_executing_time")).value.ToString();
            this.task_interval.Text = list1.Single(temp => temp.bindingData.Equals("task_interval")).value.ToString();
            
            string task_source_name = list1.Single(temp => temp.bindingData.Equals("task_source")).value.ToString();
            if (task_source_name == "任务来源为系统内部") { this.task_source.Tag = "1"; this.task_source.SelectedIndex = 0; }
            else if (task_source_name == "任务来源为ACC") { this.task_source.Tag = "2"; this.task_source.SelectedIndex = 1; }
            
            string task_is_effect_name = list1.Single(temp => temp.bindingData.Equals("task_is_effect")).value.ToString();
            if (task_is_effect_name == "生效") { this.task_is_effect.Tag = "1"; this.task_is_effect.SelectedIndex = 0; }
            else if (task_is_effect_name == "未生效") { this.task_is_effect.Tag = "2"; this.task_is_effect.SelectedIndex = 1; }

            string task_is_related_running_time_name = list1.Single(temp => temp.bindingData.Equals("task_is_related_running_time")).value.ToString();
            if (task_is_related_running_time_name == "任务与运行时间表有关") { this.task_is_related_running_time.Tag = "1"; this.task_is_related_running_time.SelectedIndex = 0; }
            else if (task_is_related_running_time_name == "任务与运行时间表无关") { this.task_is_related_running_time.Tag = "2"; this.task_is_related_running_time.SelectedIndex = 1; }
            
            this.task_remak.Text = list1.Single(temp => temp.bindingData.Equals("task_remak")).value.ToString();
            //Wrapper.ComboBoxSelectedItem(this.start_flag, this.start_flag.Tag.ToString());
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "task_id", this.task_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_name", this.task_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_execute_way", this.task_execute_way.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "task_execute_funpro", this.task_execute_funpro.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_execute_method", this.task_execute_method.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "task_start_executing_time", this.task_start_executing_time.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_end_executing_time", this.task_end_executing_time.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_interval", this.task_interval.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_source", this.task_source.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "task_is_effect", this.task_is_effect.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "task_is_related_running_time", this.task_is_related_running_time.Tag.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "task_remak", this.task_remak.Text);
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
            this.task_execute_way.Text = string.Empty;
            this.task_execute_funpro.Text = string.Empty;
            this.task_execute_method.Text = string.Empty;
            this.task_start_executing_time.Text = string.Empty;
            this.task_end_executing_time.Text = string.Empty;
            this.task_interval.Text = string.Empty;
            this.task_source.Text = string.Empty;
            this.task_is_effect.Text = string.Empty;
            this.task_is_related_running_time.Text = string.Empty;
            this.task_remak.Text = string.Empty;
        }

        private void task_execute_way_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.task_execute_way.Tag = value;
            }
            catch (Exception ex)
            {

            }
        }

        private void task_execute_method_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.task_execute_method.Tag = value;
            }
            catch (Exception ex)
            {

            }
        }

        private void task_source_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.task_source.Tag = value;
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
                this.task_is_effect.Tag = value;
            }
            catch (Exception ex)
            {

            }
        }

        private void task_is_related_running_time_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();
                this.task_is_related_running_time.Tag = value;
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
