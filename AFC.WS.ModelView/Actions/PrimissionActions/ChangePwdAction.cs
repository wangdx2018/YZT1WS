using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.Const;
    using AFC.WS.BR.Primission;
    using AFC.WS.BR.CommBuiness;
    using AFC.WS.BR;
    using AFC.WS.ModelView.UIContext;
 
    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，能够修改操作员密码
    /// </remarks>
    public class ChangePwdAction:IAction
    {

        #region IAction 成员

        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
           // throw new NotImplementedException();
            if (actionParamsList == null || actionParamsList.Count == 0)
                return false;

            if (actionParamsList[1].value.ToString() == string.Empty)
            {
                MessageDialog.Show("请输入旧密码！", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                return false;
            }

            if (actionParamsList[2].value.ToString().Equals(string.Empty) && actionParamsList[3].value.ToString().Equals(string.Empty))
            {
                MessageDialog.Show("请输入新密码！", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                return false;
            }

            if (!actionParamsList[2].value.ToString().Equals(actionParamsList[3].value.ToString()))
            {
                MessageDialog.Show("输入的两次密码不同,请您重新输入！！", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                return false;
            }

            if (actionParamsList[1].value.ToString() == actionParamsList[2].value.ToString())
            {
                MessageDialog.Show("旧密码和新密码相同!!", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                return false;
            }

            OperatorManager pm = new OperatorManager();

            if (!pm.CheckPassWord(actionParamsList[0].value.ToString(), actionParamsList[1].value.ToString()))
            {
                MessageDialog.Show("密码错误", "错误", MessageBoxIcon.Error);
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
            //throw new NotImplementedException();
            return true;
        }

        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
          
            ResultStatus status = new ResultStatus();
            uint operatorId = actionParamsList[0].value.ToString().ConvertNumberStringToUint();
            int res = BuinessRule.GetInstace().commProcess.ChangePwd(operatorId, actionParamsList[1].value.ToString(), actionParamsList[2].value.ToString());
            string message = MessageCfg.getMessageContent("1302", res.ToString());
            if (res == 0)
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_Pwd_Change, res.ToString(), "更新密码成功");
            }
            else
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_Pwd_Change, res.ToString(), "更新密码失败");
            }


           /* switch (res)
            {
                case PwdModifyResult.Modify_Failed
                    MessageDialog.Show("密码修改失败", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                    break;
                case PwdModifyResult.Operator_Disable_Or_StopUsing
                    MessageDialog.Show("操作员已停用或终止", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                    break;
                case PwdModifyResult.No_Primission
                    MessageDialog.Show("操作员没有权限", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    break;
                case PwdModifyResult.Operator_Not_Exist
                    MessageDialog.Show("操作员不存在", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                    break;
                case PwdModifyResult.The_Same_Password
                    MessageDialog.Show("两次输入的密码相同", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                    break;
                case PwdModifyResult.Operator_Nooper
                    MessageDialog.Show("初始处理状态什么操作也不做", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    break;
                case PwdModifyResult.Successful
                    MessageDialog.Show("密码修改成功,下一个运行日生效!\r\n今天登录，请您继续使用旧密码!", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                    break;
                default
                    MessageDialog.Show("通讯错误 错误代码" + res.ToString(), "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                    break;
            }*/
            MessageDialog.Show(message, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            status.resultData =res;
            //MessageDialog.Show("密码修改成功", "提示", MessageBoxIcon.Information);
            return status;
        }

        #endregion
    }
}
