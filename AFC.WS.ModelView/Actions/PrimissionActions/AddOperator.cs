using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;

    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，能够添加某个操作员
    /// </remarks>
    public class AddOperatorAction:IAction
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
                if (actionParamsList.Single(temp => temp.bindingData.Equals("company_id")).value == null)
                {
                    MessageDialog.Show("请输入工作单位", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                string operatorId = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
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
                string operatorName = actionParamsList.Single(temp => temp.bindingData.Equals("operator_name")).value.ToString();
                if (string.IsNullOrEmpty(operatorName))
                {
                    MessageDialog.Show("请输入操作员名字", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                OperatorManager operatorManger = new OperatorManager();
                if (operatorManger.IsExistCurrentOperator(operatorId))
                {
                    MessageDialog.Show("已经存在该操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                string password = actionParamsList.Single(temp => temp.bindingData.Equals("password")).value.ToString();
                if (string.IsNullOrEmpty(password))
                {
                    MessageDialog.Show("请输入密码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                Object validityStartDate = actionParamsList.Single(temp => temp.bindingData.Equals("validity_date_start")).value;
                if (validityStartDate==null)
                {
                    MessageDialog.Show("请输入有效开始日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                Object validityEndDate = actionParamsList.Single(temp => temp.bindingData.Equals("validity_date_end")).value;
                if (validityEndDate == null)
                {
                    MessageDialog.Show("请输入有效结束日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("pwd_invalidity_date")).value == null)
                {
                    MessageDialog.Show("请输入密码失效日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
             
                if (validityStartDate != null && validityEndDate!=null)
                {
                    
                     string  DateTimeFormat = "yyyy-MM-dd";

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
            PrivOperatorInfo operatorInfo = new PrivOperatorInfo();
            operatorInfo.company_id = actionParamsList.Single(temp => temp.bindingData.Equals("company_id")).value.ToString();
            operatorInfo.operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            operatorInfo.operator_name = actionParamsList.Single(temp => temp.bindingData.Equals("operator_name")).value.ToString();
            operatorInfo.operator_display_id = operatorInfo.operator_id;
            operatorInfo.is_multi_login = actionParamsList.Single(temp => temp.bindingData.Equals("is_multi_login")).value.ToString();
            //operatorInfo.login_status = "01";
            operatorInfo.email_address = actionParamsList.Single(temp => temp.bindingData.Equals("email_address")).value.ToString();
            //operatorInfo.lock_status = "00";
            if (actionParamsList.Single(temp => temp.bindingData.Equals("pwd_invalidity_date")).value == null)
            {
                operatorInfo.pwd_invalidity_date = "";
            }
            else
            {
                operatorInfo.pwd_invalidity_date = actionParamsList.Single(temp => temp.bindingData.Equals("pwd_invalidity_date")).value.ToString().Replace("-", "");
            }
            operatorInfo.operator_status = "05";
            if (actionParamsList.Single(temp => temp.bindingData.Equals("validity_date_start")).value == null)
            {
                operatorInfo.validity_date_start = "";
            }
            else
            {

                operatorInfo.validity_date_start = actionParamsList.Single(temp => temp.bindingData.Equals("validity_date_start")).value.ToString().Replace("-", "");
            }
            operatorInfo.password = actionParamsList.Single(temp => temp.bindingData.Equals("password")).value.ToString();
            operatorInfo.password_his1 = operatorInfo.password;
            operatorInfo.password_his2 = operatorInfo.password;
            if (actionParamsList.Single(temp => temp.bindingData.Equals("validity_date_end")).value == null)
            {
                operatorInfo.validity_date_end = "";
            }
            else
            {
                operatorInfo.validity_date_end = actionParamsList.Single(temp => temp.bindingData.Equals("validity_date_end")).value.ToString().Replace("-", "");
            }
            operatorInfo.contact_info1 = actionParamsList.Single(temp => temp.bindingData.Equals("contact_info1")).value.ToString();
            operatorInfo.contact_info2 = actionParamsList.Single(temp => temp.bindingData.Equals("contact_info2")).value.ToString();
            operatorInfo.contact_address = actionParamsList.Single(temp => temp.bindingData.Equals("contact_address")).value.ToString();
            operatorInfo.validity_date_first_login = DateTime.Now.ToString("yyyyMMdd");
            operatorInfo.pwd_set_mode = OperationCode.passwordSetMode;
            operatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            operatorInfo.update_time = DateTime.Now.ToString("HHmmss");
            operatorInfo.upd_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            OperatorManager operatorManger = new OperatorManager();
            int res = operatorManger.AddNewOperator(operatorInfo);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Create_New_Operator, res.ToString());
            if (res == 0)
            {
                MessageDialog.Show("增加操作员成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Operator, "0", "增加操作员成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                MessageDialog.Show("增加操作员失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Operator, "1", "增加操作员失败");
                return null;
            }
           
        }

        #endregion
    }
}
