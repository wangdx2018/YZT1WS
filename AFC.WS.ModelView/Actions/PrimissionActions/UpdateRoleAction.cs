using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Actions;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Convertors;
    using AFC.WS.Model.DB;
    using AFC.WS.BR.Primission;
    using AFC.WS.ModelView.Convertors;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    /// <summary>
    /// 更新角色信息Action 
    /// 负责人：王冬欣  最后修改日期：20091221
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，更新角色信息
    /// </remarks>
    public class UpdateShowRoleAction: IAction
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
                MessageDialog.Show("请选择角色", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_updateRole.xml");
            if (icRule == null)
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Role_Action, "1", "修改角色信息失败");
                return null;
            }
           InteractiveControl ic = new InteractiveControl();
           RoleManager roleManger = new RoleManager();
           PrivRoleInfo ri = roleManger.GetRoleInfoById(actionParamsList.Single(temp => temp.bindingData.Equals("role_id")).value.ToString());
           for (int i = 0; i < icRule.ControlList.Count; i++)
           {
               ReBindingData(icRule.ControlList[i], ri);
           }
             ic.Initialize(icRule);
             ShowDetailsDialog.ShowDetails("角色信息修改", ic, 400, 500);
             BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Role_Action, "0", "修改角色信息成功");
             return new ResultStatus { resultCode = 0, resultData = 0 };
        }
        /// <summary>
        /// 重新绑定数据对象
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="ri"></param>
        private void ReBindingData(ControlProperty cp, PrivRoleInfo ri)
        {
            switch (cp.BindingField)
            {
                case "role_id":
                    cp.InitValue = ri.role_id;
                    break;
                case "role_name":
                    cp.InitValue = ri.role_name;
                    break;
                case "role_status":
                    cp.InitValue = HandleRoleStatus(Convert.ToInt32(ri.role_status));
                    break;
                case "update_date":
                    AFC.WS.ModelView.Convertors.DateTimeConvert dt = new AFC.WS.ModelView.Convertors.DateTimeConvert();
                    cp.InitValue = dt.Convert(ri.update_date,null,null,null).ToString();
                    break;
                case "update_time":
                    ConvertToTime time = new ConvertToTime();
                    cp.InitValue =time.Convert(ri.update_time,null,null,null).ToString();
                    break;
                case "updating_operator_id":
                    cp.InitValue = ri.updating_operator_id;
                    break;
            }
        }
        /// <summary>
        /// 通过状态的数值得到中文名
        /// </summary>
        /// <param name="roleStatus">角色状态ID</param>
        /// <returns>返回角色状态的中文名</returns>
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

    public class UpdateRoleAction : IAction
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
            string roleId = actionParamsList.Single(temp => temp.bindingData.Equals("role_id")).value.ToString();
            string roleName = actionParamsList.Single(temp => temp.bindingData.Equals("role_name")).value.ToString();

            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(roleName))
                return null;
            RoleManager roleManger = new RoleManager();
            int res = roleManger.UpdateRoleInfo(roleId, roleName);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Role, res.ToString());
            if (res == 0)
            {
                MessageDialog.Show("修改角色信息成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Role_Action, "0", "修改角色信息成功");
                return new ResultStatus { resultData = 0, resultCode = 0 };
            }
            else
            {
                MessageDialog.Show("修改角色信息失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Role_Action, "1", "修改角色信息失败");
                return null;
            }
        }

        #endregion
    }
}
