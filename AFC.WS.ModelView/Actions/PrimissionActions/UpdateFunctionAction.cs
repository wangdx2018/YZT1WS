using System.Collections.Generic;
using System.Linq;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.BR.Primission;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Convertors;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using System;
    using AFC.WS.ModelView.Convertors;
    using AFC.WS.Model.Const;
    /// <summary>
    /// 更新角色信息Action 
    /// 负责人：王冬欣  最后修改日期：20091221
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，更新角色信息
    /// </remarks>
    public class UpdateShowFunctionAction:  IAction
    {
        #region IAction 成员
        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择功能", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                return false;
            }
            return true;

        }
        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            return true;
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_updateFunction.xml");
            if (icRule == null)
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Function_Action, "1", "功能信息修改失败");
                return null;
            }
            InteractiveControl ic = new InteractiveControl();
            FunctionManager fm = new FunctionManager();
            PrivFunctionInfo ri = fm.GetFunctionInfoById(actionParamsList.Single(temp => temp.bindingData.Equals("function_id")).value.ToString());
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingData(icRule.ControlList[i], ri);
            }
            ic.Initialize(icRule);
            ShowDetailsDialog.ShowDetails("功能信息修改", ic, 400, 400);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Function_Action, "0", "功能信息修改成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        private void ReBindingData(ControlProperty cp, PrivFunctionInfo fi)
        {
            switch (cp.BindingField)
            {
                case "function_id":
                    cp.InitValue =fi.function_id ;
                    break;
                case "function_name":
                    cp.InitValue = fi.function_name;
                    break;
                case "function_status":
                    cp.InitValue = HandleRoleStatus(Convert.ToInt32(fi.function_status));
                    break;
                case "update_date":
                    AFC.WS.ModelView.Convertors.DateTimeConvert dt = new AFC.WS.ModelView.Convertors.DateTimeConvert();
                    cp.InitValue = dt.Convert(fi.update_date, null, null, null).ToString();
                    break;
                case "update_time":
                    ConvertToTime time = new ConvertToTime();
                    cp.InitValue = time.Convert(fi.update_time,null,null,null).ToString();
                    break;
                case "operator_id":
                    cp.InitValue = fi.operator_id;
                    break;
                case "device_type":
                    cp.InitValue = fi.device_type;
                    break;
            }
        }

        private string HandleRoleStatus(int roleStatus)
        {
            switch (roleStatus)
            {
                case 0:
                    return "已启用";
                case 1:
                    return "已禁用";
                case 2:
                    return "已删除";
                case 3:
                    return "未启用";
                default:
                    return "未知";
            }

        }


        #endregion
    }



    public class UpdateFunctionAction:  IAction
    {

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
                return false;
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return false;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string functionId = actionParamsList.Single(temp => temp.bindingData.Equals("function_id")).value.ToString();
            string functionName = actionParamsList.Single(temp => temp.bindingData.Equals("function_name")).value.ToString();

            if (string.IsNullOrEmpty(functionId) || string.IsNullOrEmpty(functionName))
                return null;
            FunctionManager fm = new FunctionManager();
            int res = fm.UpdateFunctionInfo(functionId, functionName);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Function, res.ToString());
            if (res == 0)
            {
                MessageDialog.Show("修改功能信息成功", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                return new ResultStatus { resultData = 0, resultCode = 0 };
            }
            else
            {
                MessageDialog.Show("修改功能信息失败", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                return null;
            }
        }

        #endregion
    }
}
