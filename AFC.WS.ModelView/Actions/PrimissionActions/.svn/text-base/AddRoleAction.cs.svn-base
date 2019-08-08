using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;

    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，能够添加某个角色
    /// </remarks>
    public class AddRoleAction:IAction
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
              
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("role_class"));
                if (qc != null)
                {
                    string data = qc.value.ToString() ;
                    QueryCondition qcData = actionParamsList.Single(temp => temp.bindingData.Equals("role_id"));
                    if (qcData != null)
                    {
                        string roleId = qcData.value.ToString();
                        if (string.IsNullOrEmpty(roleId))
                        {
                            MessageDialog.Show("请输入角色编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }
                        RoleManager roleManger = new RoleManager();
                        if (roleManger.IsExistRole(roleId))
                        {
                            MessageDialog.Show("已经存在该角色", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            BuinessRule.GetInstace().logManager.WriteErrorCode(AFC.WS.Model.Const.ErrorLogData.Priv_Role_ID_Exist);
                            return false;
                        }
                        if (!CheckRoleClassPairs(data, roleId))
                        {
                            return false;
                        }
                    }
                }

                string roleName = actionParamsList.Single(temp => temp.bindingData.Equals("role_name")).value.ToString();
                if (string.IsNullOrEmpty(roleName))
                {
                    MessageDialog.Show("请输入角色名称", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.WriteErrorCode(AFC.WS.Model.Const.ErrorLogData.Priv_Role_Name_Is_Empty);
                    return false;
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
            string roleId = actionParamsList.Single(temp => temp.bindingData.Equals("role_id")).value.ToString();
            string roleName = actionParamsList.Single(temp => temp.bindingData.Equals("role_name")).value.ToString();
            RoleManager roleManger = new RoleManager();
            if (roleManger.AddRole(roleId, roleName) == 0)
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Role, "0");
                MessageDialog.Show("增加角色成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Role_Action, "0", "增加角色成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Create_New_Operator, "1");
                MessageDialog.Show("增加角色失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Role_Action, "1", "增加角色失败");
                return null;
            }

        }


        /// <summary>
        /// LCWS相关,SCWS相关,MCWS相关,TCWS相关,AG相关,BOM相关,TVM相关,EQM相关,PCA相关,ES相关
        /// </summary>
        /// <param name="roleClass"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private bool CheckRoleClassPairs(string roleClass, string roleId)
        {
            int res = 0;
            bool result = int.TryParse(roleId, out res);
            switch (roleClass)
            {
                case "LCWS相关":
                   return CheckValid(result, res,1001,roleClass);
                case "SCWS相关":
                    return CheckValid(result, res,2001,roleClass);
                case "MCWS相关":
                    return CheckValid(result,res, 3001,roleClass);
                case "TCWS相关":
                   return CheckValid(result,res, 4001,roleClass);
                case "AGM相关":
                    return CheckValid(result,res, 5001,roleClass);
                case "BOM相关":
                    return CheckValid(result,res, 6001,roleClass);
                case "TVM相关":
                    return CheckValid(result, res, 7001, roleClass);
                case "EQM相关":
                    return CheckValid(result, res, 8001, roleClass);
                case "PCA相关":
                    return CheckValid(result, res, 9001, roleClass);
                case "ES相关":
                    return CheckValid(result, res, 10001, roleClass);
                default:
                    return false;
            }
        }

        private bool CheckValid(bool result, int res,int compareValue,string content)
        {
            bool flag = result ? res >= (compareValue - 1000) && res < compareValue : result;
            if (!flag)
            {
                MessageDialog.Show(content + "角色编号应该大于" + (compareValue - 1000).ToString() + "并且小于" + compareValue.ToString(), "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            return flag;
        }

        #endregion
    }
}
