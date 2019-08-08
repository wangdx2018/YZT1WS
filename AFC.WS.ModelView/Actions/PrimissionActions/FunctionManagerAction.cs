using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;

    /// <summary>
    /// 设置功能信息的状态
    /// 负责人：王冬欣  最后修改日期：20091223
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，设置功能信息的状态
    /// </remarks>
    public class FunctionManagerAction: IAction
    {
        #region IAction 成员

        public string operationCode = "";
        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择功能", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            else
            {
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("function_status"));
                if (qc != null)
                {
                    return CheckStatus(qc);
                }
                return false;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            return false;
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string functionId = actionParamsList.Single(temp => temp.bindingData.Equals("function_id")).value.ToString();
            FunctionManager fm = new FunctionManager();
            int res = fm.ChangeFunctionStatus(functionId, this.Status);
            BuinessRule.GetInstace().logManager.AddLogInfo(operationCode, res.ToString());
            if (res != 0)
            {
                MessageDialog.Show("操作执行失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Function_Manager_Action, "1", "操作执行失败");
                return null;
            }
            else
            {
                MessageDialog.Show("操作执行成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Function_Manager_Action, "0", "操作执行成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
        }

        /// <summary>
        /// 已启用,已禁用,已删除,未启用
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        private bool CheckStatus(QueryCondition qc)
        {
            if (qc.bindingData != "function_status" || string.IsNullOrEmpty(this.Status))
                return false;
            int curStatus = Convert.ToInt16(qc.value.ToString());
            switch (this.Status)
            {
                case "0": //todo 启用为从 禁用状态或者未启用状态--->启用
                    operationCode = OperationCode.Start_Using_Function;
                    if (curStatus == 1 || curStatus == 3)
                        return true;
                    else
                        MessageDialog.Show("该功能已处于启用状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                case "1"://todo 禁用为 启用状态--->禁用状态
                    operationCode = OperationCode.Disable_Function;
                    if (curStatus == 0)
                        return true;
                    if (curStatus == 1)
                    {
                        MessageDialog.Show("该功能已处于禁用状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return false;
                    }
                    if (curStatus == 3)
                    {
                        MessageDialog.Show("该功能尚未启用,请先启用", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return false;
                    }
                    return false;
                    
                case "2":
                    operationCode = OperationCode.Delete_Function;
                    System.Windows.MessageBoxResult res = MessageDialog.Show("是否要删除该功能？", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
                    if (res == System.Windows.MessageBoxResult.Yes)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 定义状态信息
        /// </summary>
        [Filter()]
        public string Status
        {
            set;
            get;
        }

        #endregion
    }
}
