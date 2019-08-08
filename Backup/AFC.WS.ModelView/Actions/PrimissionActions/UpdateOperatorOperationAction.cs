using System;
using System.Collections.Generic;
using System.Linq;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，更新操作员信息
    /// </remarks>
    public class UpdateOperatorOperationAction: IAction
    {
        #region IAction 成员
        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null && actionParamsList.Count == 0)
                return false;
            try
            {
                string operatorId = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.operator_id")).value.ToString();
                if (string.IsNullOrEmpty(operatorId))
                {
                    MessageDialog.Show("请输入八位操作员编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (operatorId.Length < 8)
                {
                    MessageDialog.Show("请输入八位操作员编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                string operatorName = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.operator_name")).value.ToString();
                if (string.IsNullOrEmpty(operatorName))
                {
                    MessageDialog.Show("请输入操作员名字", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                /*string password = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.current_password")).value.ToString();
                if (string.IsNullOrEmpty(password))
                {
                    MessageDialog.Show("请输入密码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }*/
                Object validityStartDate = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.validity_start_date")).value;
                if (validityStartDate == null)
                {
                    MessageDialog.Show("请输入有效开始日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                Object validityEndDate = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.validity_end_date")).value;
                if (validityEndDate == null)
                {
                    MessageDialog.Show("请输入有效结束日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.password_invalidity_date")).value == null)
                {
                    MessageDialog.Show("请输入密码失效日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.validity_first_login_date")).value == null)
                {
                    MessageDialog.Show("请输入首次登录日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }

                if (actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.is_multyly_login")).value == null)
                {
                    MessageDialog.Show("请选择是否允许多重登录", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }

                if (validityStartDate != null && validityEndDate != null)
                {

                    string DateTimeFormat = "yyyy-MM-dd";

                    DateTime dtStart = DateTime.ParseExact(validityStartDate.ToString(), DateTimeFormat, null);
                    DateTime dtEnd = DateTime.ParseExact(validityEndDate.ToString(), DateTimeFormat, null);
                    if (dtEnd.Subtract(dtStart).Days < 0)
                    {
                        MessageDialog.Show("有效开始日期大于有效结束日期", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
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
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string operatorId = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.operator_id")).value.ToString();
            OperatorManager operatorManger = new OperatorManager();
            PrivOperatorInfo operatorInfo = operatorManger.GetOperatorInfoByOperatorId(operatorId);
            operatorInfo.company_name = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.company_name")).value.ToString();
            operatorInfo.validity_start_date = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.validity_start_date")).value.ToString().Replace("-", "");
            operatorInfo.validity_end_date = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.validity_end_date")).value.ToString().Replace("-", "");
            operatorInfo.validity_first_login_date = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.validity_first_login_date")).value.ToString().Replace("-", "");
            operatorInfo.password_invalidity_date = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.password_invalidity_date")).value.ToString().Replace("-", "");
            operatorInfo.is_multyly_login = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.is_multyly_login")).value.ToString();
            operatorInfo.operator_name = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.operator_name")).value.ToString();
            operatorInfo.contact_info_one = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.contact_info_one")).value.ToString();
            operatorInfo.contact_address = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.contact_address")).value.ToString();
            operatorInfo.email_address = actionParamsList.Single(temp => temp.bindingData.Equals("priv_operator_info.email_address")).value.ToString();

            OperatorManager opermanger = new OperatorManager();
            int res = 0;
            res = opermanger.UpdateOperatorInfo(operatorInfo);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator, res.ToString());
            if (res == 0)
            {
                MessageDialog.Show("更新操作员信息成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator_Operation_Action, "0", "更新操作员信息成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                MessageDialog.Show("更新操作员信息失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator_Operation_Action, "1", "更新操作员信息失败");
                return null;
            }
        }

        #endregion
    }
}
