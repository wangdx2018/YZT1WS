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
    using AFC.WS.ModelView.UIContext;
    using AFC.BOM2.MessageDispacher;

    /// <summary>
    /// 操作员登录处理Action
    /// 
    /// modified by wangdx 20130624增加了开启报警监听线程
    /// </summary>
    public class LoginAction: IAction
    {
        /// <summary>
        /// 操作员ID，操作员密码错误登录次数
        /// </summary>
        private Dictionary<string, int> dict = new Dictionary<string, int>();

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                return false;
            }
            string operatorId = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();

            string pwd = actionParamsList.Single(temp => temp.bindingData.Equals("pwd")).value.ToString();
            //return true;

            if (string.IsNullOrEmpty(pwd))
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_PassWord_Is_Empty);
                MessageDialog.Show("请输入操作员密码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(operatorId))
            {
                 BuinessRule.GetInstace().logManager.WriteErrorCode(AFC.WS.Model.Const.ErrorLogData.Priv_Operator_ID_Format_Error);
                MessageDialog.Show("请输入操作员编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            int res = BuinessRule.GetInstace().operationManager.CheckOperatorValid(operatorId, pwd);
            if (res == -1)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Operator_Not_Exist);
                MessageDialog.Show("不存在该操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (res == -2)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Operator_Not_Valid);
                MessageDialog.Show("该操作员状态非法，不能登录！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (res == -3)
            {
                MessageDialog.Show("该操作员已过有效期！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res == -4)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Station_Not_Primission);
                MessageDialog.Show("该操作员在本站无权限，不能登录！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res == -5)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_DeviceType_Not_Primission);
                MessageDialog.Show("该操作员无WS的操作权限！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res == -6)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Operator_Locked);
                MessageDialog.Show("操作员已锁定！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res == -8)
            {
                MessageDialog.Show("操作员密码已到期！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res == -9)
            {
                MessageDialog.Show("操作员未到启用时间！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res!=0&&
                dict.ContainsKey(operatorId) &&
                dict[operatorId] == 3)
            {
                BuinessRule.GetInstace().commProcess.LockOperator(operatorId.ConvertNumberStringToUint());//send lock
                MessageDialog.Show("该操作员已经尝试登录3次，不能登录！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            if (res == -7)
            {
                MessageDialog.Show("操作员密码错误！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                if (dict.ContainsKey(operatorId))
                    dict[operatorId] = dict[operatorId] + 1;
                else
                    dict.Add(operatorId, 1);
                return false;
            }

            if (res == 1)
            {
                MessageDialog.Show("该操作员即将过期！", "警告", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            }
            return true;
            //001检查用户否存在
            //005检查操作员是否锁定超过3次，提示该操作员已经重试3次，不能继续登录
            //检查登录是否超过3次，如果
            //002检查车站是否能登录
            //003能否登录WS
            //004是否联网，如未联网
            //006检查操作员是否合法
            //007密码是否正确（密码错误达到3次，发送锁定报文）
            
            //todocheck here
            //return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
           // throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string opeatorId = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();

            string pwd = actionParamsList.Single(temp => temp.bindingData.Equals("pwd")).value.ToString();

            #region send msg to sc
            int res = BuinessRule.GetInstace().commProcess.LogIn(opeatorId.ConvertNumberStringToUint(), pwd);

           // int res = BuinessRule.GetInstace().commProcess.ChangePwd(operatorId, actionParamsList[1].value.ToString(), actionParamsList[2].value.ToString());
            if (res != 0)
            {
                string message = MessageCfg.getMessageContent("1301", res.ToString());
                MessageDialog.Show(message, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Login_Action, "1", message);
                return null;
            }
            
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Login_Action, "0", "操作员登录成功");
            #endregion
             
            #region switch ui
            AFC.WS.ModelView.UIContext.UIOperation.GetInstance().SwitchUI("Welcome"); //切换UI
            #endregion

            #region OperatorBinding

            BuinessRule.GetInstace().brConext.CurrentOperatorId = opeatorId;

            #endregion

            #region AutoLogOut

            AFC.WS.ModelView.UIContext.AutoLogOutTrigger.GetInstance().StartListen();

            #endregion


            #region Start Alarm Monitor

          //  BuinessRule.GetInstace().alarmMonitor.StartAlarmMonitor();
            #endregion

            #region send msg for ui change Tag

            //send msg for ui navigator change
            MessageManager.SendMessasge(new Message
            {
                MessageType = SynMessageType.NavigationSelection,
                Content = "Welcome"
            });
            #endregion

            #region Create UI NavigatorList

            AFC.BOM2.MessageDispacher.Message msg = new AFC.BOM2.MessageDispacher.Message();
            msg.MessageType = SynMessageType.CreateNavigatorList;
            msg.Content = BuinessRule.GetInstace().operationManager.GetCurrentOperatorFunctionList(opeatorId);
            AFC.BOM2.MessageDispacher.MessageManager.SendMessasge(msg);

            #endregion


            return new ResultStatus { resultCode = 0, resultData = 0 };
            //throw new NotImplementedException();
            //001切换到Welcome界面
            //002生成参数信息，发送Navigator权限UI界面。
            //003发送字体变化通知。菜单的导航变成为 “Welcome".
            //004
        }

        #endregion
    }
}
