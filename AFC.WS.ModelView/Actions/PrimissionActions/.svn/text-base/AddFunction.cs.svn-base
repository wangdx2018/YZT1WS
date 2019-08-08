using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;

    /// <summary>
    /// 增加系统功能Action
    /// WS2.0基础组件中调用的此Action处理类，能够添加某个
    /// 系统功能
    /// </remarks>
    public class AddFunction:IAction
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
                MessageDialog.Show("请选择功能", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                string deviceType = actionParamsList.Single(temp => temp.bindingData.Equals("device_type")).value.ToString();
                string function_id = actionParamsList.Single(temp => temp.bindingData.Equals("function_id")).value.ToString();
                string functionName = actionParamsList.Single(temp => temp.bindingData.Equals("function_name")).value.ToString();
                if (function_id.Length != 8)
                {
                    if (function_id.Trim().Length == 0)
                    {
                        MessageDialog.Show("请输入功能编号", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return false;
                    }
                    MessageDialog.Show("格式不合法，输入长度为8!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.WriteErrorCode(AFC.WS.Model.Const.ErrorLogData.Priv_Function_ID_Not_Valid);
                    return false;
                }
                if (!function_id.Substring(0, 2).Equals(deviceType))
                {

                    MessageDialog.Show("前两位必须为" + deviceType, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.WriteErrorCode(AFC.WS.Model.Const.ErrorLogData.Priv_Function_ID_Not_Valid);
                    return false;
                }
             
                if (functionName.Trim().Length == 0)
                {
                    MessageDialog.Show("请输入功能名称", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            return true;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            string functionId = actionParamsList.Single(temp => temp.bindingData.Equals("function_id")).value.ToString();
            string functionName = actionParamsList.Single(temp => temp.bindingData.Equals("function_name")).value.ToString();
            FunctionManager fm = new FunctionManager();
            if (fm.IsExistFunction(functionId))
            {
                MessageDialog.Show("存在该功能编号，无法重复添加", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.WriteErrorCode(AFC.WS.Model.Const.ErrorLogData.Priv_Function_ID_Exist);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Function, "1", "存在该功能编号，无法重复添加");
                return null;
            }
            int res = fm.AddFunction(functionId, functionName);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_New_Function, res.ToString());
            if (res == 0)
            {
                MessageDialog.Show("增加功能成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Function, "0", "增加功能成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                MessageDialog.Show("增加功能失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Function, "1", "增加功能失败");
                return null;
            }
    
            //throw new NotImplementedException();
        }

        #endregion
    }
}
