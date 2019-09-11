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
            notify notify1 = new notify { notifyName = "不启用", notifyID = "0" };//任务执行方式。1：任务以函数调用执行；2：任务以fork子进程执行
            notify notify2 = new notify { notifyName = "启用", notifyID = "1" };
            notifyList.Add(notify1);
            notifyList.Add(notify2);
            this.task_enable.ItemsSource = notifyList;
            this.task_enable.DisplayMemberPath = "notifyName";

            List<notify> notifyList1 = new List<notify>();
            notify notify3 = new notify { notifyName = "任务轮询执行", notifyID = "1" };//任务执行方法。1：任务轮询执行；2：任务明天定点执行。
            notify notify4 = new notify { notifyName = "每天执行一次", notifyID = "2" };
            notify notify5 = new notify { notifyName = "每周执行一次", notifyID = "3" };
            notify notify6 = new notify { notifyName = "每月执行一次", notifyID = "4" };
            notify notify7 = new notify { notifyName = "每年执行一次", notifyID = "5" };
            notifyList1.Add(notify3);
            notifyList1.Add(notify4);
            notifyList1.Add(notify5);
            notifyList1.Add(notify6);
            notifyList1.Add(notify7);
            this.exec_method.ItemsSource = notifyList1;
            this.exec_method.DisplayMemberPath = "notifyName";

            
            
        }

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();            
            Wrapper.Instance.AddQueryConditionToList(list, "task_name", this.task_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "task_enable", (this.task_enable.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "exec_method", (this.exec_method.SelectedValue as notify).notifyID);
            Wrapper.Instance.AddQueryConditionToList(list, "start_exec_date", this.start_exec_date.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "start_exec_week", this.start_exec_week.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "start_exec_time", this.start_exec_time.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "exec_interval", this.exec_interval.Text);    



            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.AddTimeTaskExcute();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.task_name.Text = string.Empty;
            this.task_enable.Text = string.Empty;
            this.exec_method.Text = string.Empty;
            this.start_exec_date.Text = string.Empty;
            this.start_exec_week.Text = string.Empty;
            this.start_exec_time.Text = string.Empty;
            this.exec_interval.Text = string.Empty;
            
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
