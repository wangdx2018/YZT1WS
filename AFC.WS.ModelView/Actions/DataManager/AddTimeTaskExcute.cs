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

        
        string task_name = string.Empty;
        string task_enable = string.Empty;
        string exec_method = string.Empty;
        string start_exec_date = string.Empty;
        string start_exec_week = string.Empty;
        string start_exec_time = string.Empty;
        string exec_interval = string.Empty;
        

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_name")).value != null)
            {
                task_name = actionParamsList.Single(temp => temp.bindingData.Equals("task_name")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("task_enable")).value != null)
            {
                task_enable = actionParamsList.Single(temp => temp.bindingData.Equals("task_enable")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("exec_method")).value != null)
            {
                exec_method = actionParamsList.Single(temp => temp.bindingData.Equals("exec_method")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("start_exec_date")).value != null)
            {
                start_exec_date = actionParamsList.Single(temp => temp.bindingData.Equals("start_exec_date")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("start_exec_week")).value != null)
            {
                start_exec_week = actionParamsList.Single(temp => temp.bindingData.Equals("start_exec_week")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("start_exec_time")).value != null)
            {
                start_exec_time = actionParamsList.Single(temp => temp.bindingData.Equals("start_exec_time")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("exec_interval")).value != null)
            {
                exec_interval = actionParamsList.Single(temp => temp.bindingData.Equals("exec_interval")).value.ToString();
            }
            

            
            if (string.IsNullOrEmpty(task_name))
            {
                Wrapper.ShowDialog("请填写任务名称。");
                return false;
            }
            if (string.IsNullOrEmpty(task_enable))
            {
                Wrapper.ShowDialog("请填写任务启用状态。");
                return false;
            }
            if (string.IsNullOrEmpty(exec_method))
            {
                Wrapper.ShowDialog("任务执行方式");
                return false;
            }
            if (string.IsNullOrEmpty(start_exec_date))
            {
                Wrapper.ShowDialog("轮询任务开始执行日期。");
                return false;
            }
            else if (string.IsNullOrEmpty(start_exec_week))
            {
                Wrapper.ShowDialog("轮询任务开始执行日期周。");
                return false;
            }
            if (string.IsNullOrEmpty(start_exec_time))
            {
                Wrapper.ShowDialog("轮询任务开始执行时间。");
                return false;
            }
            else if (!Utility.Instance.IsValidTIME(exec_interval))
            {
                Wrapper.ShowDialog("请填写任务执行间隔。");
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
            TaskManage info = new TaskManage();
            info.task_name = task_name;
            info.cost_time = "00";
            info.end_exec_time = "000000";
            info.exec_interval = exec_interval;
            info.exec_method = exec_method;
            info.exec_status = "0";
            info.last_exec_date = "00000000";
            info.last_exec_time = "000000";
            info.start_exec_date = start_exec_date;
            info.start_exec_time = start_exec_time;
            info.start_exec_week = start_exec_week;
            info.task_enable = task_enable;
            info.total_exec_timer = "00";
            info.total_fail_timer = "00";

            //插入新维修工区
            res = DBCommon.Instance.InsertTable(info, "task_manage");
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