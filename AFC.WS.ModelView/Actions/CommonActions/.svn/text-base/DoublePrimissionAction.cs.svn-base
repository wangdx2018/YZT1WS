using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.CommonActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    using AFC.WS.UI.Common;

    /// 作者：王冬欣 
    /// 日期：20110221
    /// 代码功能：通用的显示界面的Action。
    /// 通过配置UserBaseControl的反射类名来创建界面,action的参数在界面的Tag属性中取值。
    /// 修订记录：edit by wangdx 20120801 在通用的Action处理中
    ///           增加了第二个操作员密码输入三次错误后，锁定该操作员。
    ///          
    public class DoublePrimissionAction:  IAction
    {

        /// <summary>
        /// 操作员和密码输入错误次数
        /// </summary>
        private Dictionary<string, int> dictPwdFailed = new Dictionary<string, int>();

        /// <summary>
        /// action的子类型（如不配置，需要设置subAction）
        /// </summary>
        public string SubActionType
        {
            set;
            get;
        }

        /// <summary>
        /// 第一个操作员ID，必须配置
        /// </summary>
        public string CurrentOperationId
        {
            set;
            get;
        }

        /// <summary>
        /// 第二个操作员ID
        /// </summary>
        private string secordOperatorId = null;

        /// <summary>
        /// 双权限检查完成功后的控件,SubAction 需要实现IDoublePrimission接口
        /// </summary>
        public IAction subAction = null;



        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (string.IsNullOrEmpty(this.CurrentOperationId))
                return false;
            if (!string.IsNullOrEmpty(this.CurrentOperationId) &&
                this.subAction != null)
                return this.CheckDoublePrimission(CurrentOperationId, out this.secordOperatorId);
            if (!string.IsNullOrEmpty(this.CurrentOperationId) &&
                !string.IsNullOrEmpty(this.SubActionType))
                return this.CheckDoublePrimission(CurrentOperationId, out this.secordOperatorId);

            return false;
            // throw new NotImplementedException();
            //subAction = Activator.CreateInstance(Type.GetType(SubActionType)) as IAction;
            //return subAction.CheckValid(actionParamsList);
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            if (subAction == null)
            {
                subAction = Activator.CreateInstance(Type.GetType(this.SubActionType)) as IAction;
            }
            if (subAction != null &&
                subAction.CheckValid(actionParamsList))
            {

                if (subAction.DoAction(actionParamsList) != null)
                {
                    if (subAction is IDoublePrimissionHandler)
                    {
                        IDoublePrimissionHandler handler = subAction as IDoublePrimissionHandler;
                        try
                        {
                            handler.HandleDoublePrimission(this.secordOperatorId);
                        }
                        catch (Exception ex)
                        {
                            WriteLog.Log_Error(ex);
                            WriteLog.Log_Error(handler.GetType().ToString() + " handle error!");
                        }
                    }
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
            }
            return null;
        }

        /// <summary>
        /// 双权限认证
        /// </summary>
        /// <param name="currentOperatorId">当前操作员</param>
        /// <param name="secondOperatorid">第二个操作员</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckDoublePrimission(string currentOperatorId, out string secondId)
        {
            secondId = string.Empty;
            DoublePermissions dp = new DoublePermissions(this.CurrentOperationId);
            dp.OnHandleDoublePrimission += new DoublePermissions.HandleDoublePrimission(dp_OnHandleDoublePrimission);
            dp.ShowDialog();
            if (string.IsNullOrEmpty(secondId))
                return false;
            return true;
        }


        private void CheckPwdErrorTime(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId))
                return;
            if (dictPwdFailed.ContainsKey(operatorId))
            {
                if (dictPwdFailed[operatorId] <3)
                    dictPwdFailed[operatorId] = dictPwdFailed[operatorId] + 1;
                else
                    BuinessRule.GetInstace().commProcess.LockOperator(operatorId.ConvertNumberStringToUint());
            }
            else
            {
                dictPwdFailed.Add(operatorId, 1);
            }

        }


        /// <summary>
        /// doublePrimission的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dp_OnHandleDoublePrimission(object sender, MyEventArgs e)
        {
            int res = BuinessRule.GetInstace().operationManager.CheckOperatorValid(e.firstId, e.firstPwd);

            if (res != 0)
            {
                HandleErrorCode(res,"第一个");
                if (res == -7)
                {
                    CheckPwdErrorTime(e.firstId);
                }
                return;
            }
            res = BuinessRule.GetInstace().operationManager.CheckOperatorValid(e.secordId, e.secordPwd);

            if (res != 0)
            {
                HandleErrorCode(res, "第二个");
                if (res == -7)
                {
                    CheckPwdErrorTime(e.secordId);
                }
                return;
            }
                this.secordOperatorId = e.secordId;
                (sender as DoublePermissions).Close();
        }


        private void HandleErrorCode(int res,string header)
        {
            if (res == -1)
            {
                MessageDialog.Show(header+"操作员不存在！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (res == -7)
            {
                MessageDialog.Show(header + "操作员密码错误！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            if (res == -2)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Operator_Not_Valid);
                MessageDialog.Show(header + "操作员状态非法，不能登录！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            if (res == -3)
            {
                MessageDialog.Show(header + "操作员已过有效期！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (res == -4)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Station_Not_Primission);
                MessageDialog.Show(header + "操作员在本站无权限，不能登录！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (res == -5)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_DeviceType_Not_Primission);
                MessageDialog.Show(header + "操作员无WS的操作权限！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (res == -6)
            {
                BuinessRule.GetInstace().logManager.WriteErrorCode(ErrorLogData.Priv_Operator_Locked);
                MessageDialog.Show(header + "操作员已锁定！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (res == -8)
            {
                MessageDialog.Show(header + "操作员密码已到期！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (res == -9)
            {
                MessageDialog.Show(header + "操作员未到启用时间！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
        }

    }
}

