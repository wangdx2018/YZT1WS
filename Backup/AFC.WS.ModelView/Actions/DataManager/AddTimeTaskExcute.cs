using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.RfidRW;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Windows;
using AFC.WS.BR.LogManager;
using AFC.WS.BR.TickBoxManager;
using AFC.WS.Model.Const;
using AFC.WS.UI.BR.Data;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class AddTimeTaskExcute : IAction
    {
        #region IAction 成员

        string task_id = string.Empty;
        string task_name = string.Empty;
        string task_execute_way = string.Empty;
        string task_execute_funpro = string.Empty;
        string task_execute_method = string.Empty;
        string task_start_executing_time = string.Empty;
        string task_end_executing_time = string.Empty;
        string task_interval = string.Empty;
        string task_source = string.Empty;
        string task_is_effect = string.Empty;
        string task_is_related_running_time = string.Empty;
        string task_remak = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_id")).value != null)
            {
                task_id = actionParamsList.Single(temp => temp.bindingData.Equals("task_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_name")).value != null)
            {
                task_name = actionParamsList.Single(temp => temp.bindingData.Equals("task_name")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_execute_way")).value != null)
            {
                task_execute_way = actionParamsList.Single(temp => temp.bindingData.Equals("task_execute_way")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_execute_funpro")).value != null)
            {
                task_execute_funpro = actionParamsList.Single(temp => temp.bindingData.Equals("task_execute_funpro")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_execute_method")).value != null)
            {
                task_execute_method = actionParamsList.Single(temp => temp.bindingData.Equals("task_execute_method")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_start_executing_time")).value != null)
            {
                task_start_executing_time = actionParamsList.Single(temp => temp.bindingData.Equals("task_start_executing_time")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_end_executing_time")).value != null)
            {
                task_end_executing_time = actionParamsList.Single(temp => temp.bindingData.Equals("task_end_executing_time")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_interval")).value != null)
            {
                task_interval = actionParamsList.Single(temp => temp.bindingData.Equals("task_interval")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_source")).value != null)
            {
                task_source = actionParamsList.Single(temp => temp.bindingData.Equals("task_source")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_is_effect")).value != null)
            {
                task_is_effect = actionParamsList.Single(temp => temp.bindingData.Equals("task_is_effect")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_is_related_running_time")).value != null)
            {
                task_is_related_running_time = actionParamsList.Single(temp => temp.bindingData.Equals("task_is_related_running_time")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_remak")).value != null)
            {
                task_remak = actionParamsList.Single(temp => temp.bindingData.Equals("task_remak")).value.ToString();
            }

            if (string.IsNullOrEmpty(task_id))
            {
                Wrapper.ShowDialog("请填写任务编号。");
                return false;
            }
            if (string.IsNullOrEmpty(task_name))
            {
                Wrapper.ShowDialog("请填写任务名称。");
                return false;
            }
            if (string.IsNullOrEmpty(task_execute_way))
            {
                Wrapper.ShowDialog("请填写任务执行方式。");
                return false;
            }
            if (string.IsNullOrEmpty(task_execute_funpro))
            {
                Wrapper.ShowDialog("请填写程序绝对路径。");
                return false;
            }
            if (string.IsNullOrEmpty(task_execute_method))
            {
                Wrapper.ShowDialog("请填写任务执行方法。");
                return false;
            }
            if (string.IsNullOrEmpty(task_start_executing_time))
            {
                Wrapper.ShowDialog("轮询任务开始执行时间。");
                return false;
            }
            else if (!Utility.Instance.IsValidTIME(task_start_executing_time))
            {
                Wrapper.ShowDialog("请填写正确格式的轮询任务开始执行时间(例如：000001)。");
                return false;
            }
            if (string.IsNullOrEmpty(task_end_executing_time))
            {
                Wrapper.ShowDialog("轮询任务执行结束时间。");
                return false;
            }
            else if (!Utility.Instance.IsValidTIME(task_end_executing_time))
            {
                Wrapper.ShowDialog("请填写正确格式的轮询任务结束执行时间(例如：235959)。");
                return false;
            }
            if (string.IsNullOrEmpty(task_interval))
            {
                Wrapper.ShowDialog("请填写任务执行时间。");
                return false;
            }
            if (string.IsNullOrEmpty(task_source))
            {
                Wrapper.ShowDialog("请填写任务来源。");
                return false;
            }
            if (string.IsNullOrEmpty(task_is_effect))
            {
                Wrapper.ShowDialog("请填写任务是否生效。");
                return false;
            }
            if (string.IsNullOrEmpty(task_is_related_running_time))
            {
                Wrapper.ShowDialog("任务与运行时间相关性。");
                return false;
            }
            if (string.IsNullOrEmpty(task_remak))
            {
                Wrapper.ShowDialog("请填写任务备注。");
                return false;
            }

            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //开启事务
            Util.DataBase.BeginTransaction();
            int res = 0;
            TaskExecuteSchedule info = new TaskExecuteSchedule();
            info.task_id = task_id.ToInt32();
            info.task_name = task_name;
            info.task_execute_way = task_execute_way;
            info.task_execute_funpro = task_execute_funpro;
            info.task_execute_method = task_execute_method;
            info.task_start_executing_time = task_start_executing_time;
            info.task_end_executing_time = task_end_executing_time;
            info.task_interval = task_interval.ToInt32();
            info.task_source = task_source;
            info.task_is_effect = task_is_effect;
            info.task_is_related_running_time = task_is_related_running_time;
            info.task_remak = task_remak;

            //插入新维修工区
            res = DBCommon.Instance.InsertTable(info, "task_execute_schedule");
            if (res == 1)
            {
                Wrapper.ShowDialog("定时任务添加成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Task_Execute_Schedule_Add, "0", "定时任务添加成功");
                Util.DataBase.Commit();
                return null;
            }
            else
            {
                Wrapper.ShowDialog("定时任务添加失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Task_Execute_Schedule_Add, "1", "定时任务添加失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}