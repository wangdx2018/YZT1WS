using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WorkStation.DB;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR.Primission;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{

    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，禁用，启用，删除角色信息
    /// </remarks>
    public class RoleManagerAction: IAction
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
                MessageDialog.Show("请选择角色", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                return false;
            }
            else
            {
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("role_status"));
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
            string roleId = actionParamsList.Single(temp => temp.bindingData.Equals("role_id")).value.ToString();
            RoleManager roleManger = new RoleManager();
            int res = roleManger.ChangeRoleStatus(roleId, Status);
            BuinessRule.GetInstace().logManager.AddLogInfo(operationCode, res.ToString());
            if (res != 0)
            {
                MessageDialog.Show("操作执行失败", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Role_Manager_Action, "1", "角色管理操作执行失败");                
                return null;
            }
            else
            {
                MessageDialog.Show("操作执行成功", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Role_Manager_Action, "0", "角色管理操作执行成功"); 
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
            if (qc.bindingData != "role_status"||string.IsNullOrEmpty(this.Status))
                return false;
            int curStatus = Convert.ToInt16(qc.value.ToString());
            switch (this.Status)
            {
                case "0": //todo 启用为从 禁用状态或者未启用状态--->启用
                    operationCode = OperationCode.Start_Using_Role;
                    if (curStatus == 1 || curStatus == 3 || curStatus == 2)
                        return true;
                    else 
                        MessageDialog.Show("该角色已处于启用状态", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                    return false;
                case "1"://todo 禁用为 启用状态--->禁用状态
                    operationCode = OperationCode.Disable_Role;
                    if (curStatus == 0)
                        return true;
                    if (curStatus == 1)
                    {
                        MessageDialog.Show("该角色已处于禁用状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return false;
                    }
                    if (curStatus == 3)
                    {
                        MessageDialog.Show("该角色尚未启用,请先启用", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return false;
                    }
                    return false;
                    break;
                case "2":
                    operationCode = OperationCode.Delete_Role;
                    System.Windows.MessageBoxResult res = MessageDialog.Show("是否要删除该角色？", "提示", MessageBoxIcon.Question,MessageBoxButtons.YesNo);
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
