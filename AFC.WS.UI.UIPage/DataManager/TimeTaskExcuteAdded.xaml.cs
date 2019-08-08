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
    public partial class TimeTaskExcuteAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public TimeTaskExcuteAdded()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
        }
        private void InitLoad()
        {
            List<notify> notifyList = new List<notify>();
            notify notify1 = new notify { notifyName = "任务以函数调用执行", notifyID = "1" };//任务执行方式。1：任务以函数调用执行；2：任务以fork子进程执行
            notify notify2 = new notify { notifyName = "任务以fork子进程执行", notifyID = "2" };
            notifyList.Add(notify1);
            notifyList.Add(notify2);
            this.task_execute_way.ItemsSource = notifyList;
            this.task_execute_way.DisplayMemberPath = "notifyName";

            List<notify> notifyList1 = new List<notify>();
            notify notify3 = new notify { notifyName = "任务轮询执行", notifyID = "1" };//任务执行方法。1：任务轮询执行；2：任务明天定点执行。
            notify notify4 = new notify { notifyName = "任务明天定点执行", notifyID = "2" };
            notifyList1.Add(notify3);
            notifyList1.Add(notify4);
            this.task_execute_method.ItemsSource = notifyList1;
            this.task_execute_method.DisplayMemberPath = "notifyName";

            List<notify> notifyList2 = new List<notify>();
            notify notify5 = new notify { notifyName = "任务来源为系统内部", notifyID = "1" };//1：任务来源为系统内部；2：任务来源为ACC
            notify notify6 = new notify { notifyName = "任务来源为ACC", notifyID = "2" };
            notifyList2.Add(notify5);
            notifyList2.Add(notify6);
            this.task_source.ItemsSource = notifyList2;
            this.task_source.DisplayMemberPath = "notifyName";

            List<notify> notifyList3 = new List<notify>();
            notify notify7 = new notify { notifyName = "任务已经生效", notifyID = "1" };//1：任务已经生效；2：任务还未生效
            notify notify8 = new notify { notifyName = "任务还未生效", notifyID = "2" };
            notifyList3.Add(notify7);
            notifyList3.Add(notify8);
            this.task_is_effect.ItemsSource = notifyList3;
            this.task_is_effect.DisplayMemberPath = "notifyName";

            List<notify> notifyList4 = new List<notify>();
            notify notify9 = new notify { notifyName = "任务与运行时间表有关", notifyID = "1" };//1：任务与运行时间表有关；2：任务与运行时间表无关
            notify notify0 = new notify { notifyName = "任务与运行时间表无关", notifyID = "2" };
            notifyList4.Add(notify9);
            notifyList4.Add(notify0);
            this.task_is_related_running_time.ItemsSource = notifyList4;
            this.task_is_related_running_time.DisplayMemberPath = "notifyName";
        }

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "task_id", this.task_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_name", this.task_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_execute_way", (this.task_execute_way.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "task_execute_funpro", this.task_execute_funpro.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_execute_method", (this.task_execute_method.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "task_start_executing_time", this.task_start_executing_time.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_end_executing_time", this.task_end_executing_time.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_interval", this.task_interval.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_source", (this.task_source.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "task_is_effect", (this.task_is_effect.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "task_is_related_running_time", (this.task_is_related_running_time.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "task_remak", this.task_remak.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.AddTimeTaskExcute();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.task_id.Text = string.Empty;
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

        private class notify
        {
            private string _notifyName;
            private string _notifyID;

            public string notifyName
            {
                get
                {
                    return this._notifyName;
                }
                set
                {
                    this._notifyName = value;
                }
            }

            public string notifyID
            {
                get
                {
                    return this._notifyID;
                }
                set
                {
                    this._notifyID = value;
                }
            }
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
