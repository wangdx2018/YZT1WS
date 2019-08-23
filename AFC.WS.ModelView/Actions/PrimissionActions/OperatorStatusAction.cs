using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;


namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;

    //-->启用,禁用，密码终止操作员 
    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，启用,禁用，密码终止操作员
    /// </remarks>
    public class OperatorStatusAction:  IAction
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
                MessageDialog.Show("请您选择操作员", "提示",MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            return CheckOperatorAction(actionParamsList);
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
            if (actionParamsList == null || actionParamsList.Count == 0)
                return null;
            try
            {
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id"));
                if (qc != null)
                {
                    if (this.OperatorStatus == AFC.WS.Model.Const.OperatorStatus.ForceChangePwd)
                    {
                        System.Windows.MessageBoxResult result=MessageDialog.Show("是否将该操作员密码变成111111?", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
                        if (result != System.Windows.MessageBoxResult.Yes)
                        {
                            return null;
                        }
                    }
                    if (this.OperatorStatus == AFC.WS.Model.Const.OperatorStatus.delOperaor)
                    {
                        System.Windows.MessageBoxResult result = MessageDialog.Show("是否确定将该操作员删除?", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
                        if (result != System.Windows.MessageBoxResult.Yes)
                        {
                            return null;
                        }
                    }
                    OperatorManager operatorManager = new OperatorManager();
                    int res = operatorManager.SetOperatorStatus(this.OperatorStatus, qc.value.ToString());
                    BuinessRule.GetInstace().logManager.AddLogInfo(operationCode, res.ToString());
                    if (res==0)
                    {
                        MessageDialog.Show("修改成功，下一个运营日生效", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_Status_Action, "0", "操作员状态修改成功");
                        return new ResultStatus { resultCode = 0, resultData = res };
                    }
                    MessageDialog.Show(string.Format("状态修改失败 错误代码{0}", res.ToString()), "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_Status_Action, "1", "操作员状态修改失败");
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("error in [" + this.GetType().ToString() + "]");
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        [Description("操作员的状态设置选择。Normal = 00 ,Disable = 01, PassWordEnd = 02,ForceChangePwd = 03 ,Locked = 04, NoYetUsing=05， delOperaor=06"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()]
        public string OperatorStatus
        {
            get;
            set;
        }

        #endregion

        //-->检查禁用操作员
        /// <summary>
        /// 检查禁用操作员
        /// </summary>
        /// <param name="list">Action参数列表</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckOperatorAction(List<QueryCondition> list)
        {
            try
            {
                QueryCondition qc = list.Single(temp => temp.bindingData.Equals("operator_id"));
                if (qc != null)
                {
                    OperatorManager operatorManager = new OperatorManager();
                    PrivOperatorInfo operatorInfo = operatorManager.GetOperatorInfoByOperatorId(qc.value.ToString());
                    if (operatorInfo != null)
                    {
                        switch (this.OperatorStatus)
                        {
                            case AFC.WS.Model.Const.OperatorStatus.Disable:
                                operationCode = OperationCode.Operator_Disable;
                                return this.CheckDisableOperatorAction(operatorInfo);
                            case AFC.WS.Model.Const.OperatorStatus.NoYetUsing:
                                return this.CheckEnableOperationAction(operatorInfo);
                            case AFC.WS.Model.Const.OperatorStatus.PassWordEnd:
                                operationCode = OperationCode.End_Operator_Pwd;
                                return this.CheckPasswordEndAction(operatorInfo);
                            case AFC.WS.Model.Const.OperatorStatus.ForceChangePwd:
                                operationCode = OperationCode.Operator_Pwd_Reset;
                                return this.CheckPasswordResetAction(operatorInfo);
                            case AFC.WS.Model.Const.OperatorStatus.Normal:
                                operationCode = OperationCode.Start_Using_Operator;
                                return true;
                            case AFC.WS.Model.Const.OperatorStatus.delOperaor:
                                operationCode = OperationCode.Delete_Operator;
                                return true;
                            default:
                                return true;
                        }
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
           
            
                
        }

        //-->检查禁用操作员
        /// <summary>
        /// 检查启用操作员
        /// </summary>
        /// <param name="operatorInfo">操作员信息</param>
        /// <returns>成功返回ture，否则返回false</returns>
        private bool CheckDisableOperatorAction(PrivOperatorInfo operatorInfo)
        {
            if (operatorInfo.operator_status == AFC.WS.Model.Const.OperatorStatus.Disable)
            {
                MessageDialog.Show("该操作员已处于禁用状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (operatorInfo.operator_status == AFC.WS.Model.Const.OperatorStatus.NoYetUsing)
            {
                MessageDialog.Show("该操作员未启用，无法禁用，请先启用!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (BuinessRule.GetInstace().brConext.CurrentOperatorId == operatorInfo.operator_id)
            {
                MessageDialog.Show("不能对当前操作员进行禁用", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        //-->检查启用和正常为同一状态操作员
        /// <summary>
        /// 检查启用操作员，操作员状态必须为已禁用
        /// </summary>
        /// <param name="operatorInfo">操作员信息类</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckEnableOperationAction(PrivOperatorInfo operatorInfo)
        {
            if (operatorInfo.operator_status != AFC.WS.Model.Const.OperatorStatus.Normal)
                return true;
            else
            {
                MessageDialog.Show("该操作员处于正常状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
        }

        //--->检查操作员密码终止
        /// <summary>
        /// 检查操作员密码终止
        /// </summary>
        /// <param name="operatorInfo">操作员信息</param>
        /// <returns>成功返回0，否则返回false</returns>
        private bool CheckPasswordEndAction(PrivOperatorInfo operatorInfo)
        {
            if (operatorInfo.operator_status == AFC.WS.Model.Const.OperatorStatus.NoYetUsing)
            {
                MessageDialog.Show("该操作员未启用,请先启用", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (operatorInfo.operator_status == AFC.WS.Model.Const.OperatorStatus.PassWordEnd)
            {
                MessageDialog.Show("该操作员已是密码终止状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (BuinessRule.GetInstace().brConext.CurrentOperatorId == operatorInfo.operator_id)
            {
                MessageDialog.Show("不能对当前操作员进行密码终止", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }


        //--->检查操作员密码重置
        /// <summary>
        /// 检查操作员密码重置
        /// </summary>
        /// <param name="operatorInfo">操作员信息</param>
        /// <returns>成功返回0，否则返回false</returns>
        private bool CheckPasswordResetAction(PrivOperatorInfo operatorInfo)
        {
            if (operatorInfo.operator_status == AFC.WS.Model.Const.OperatorStatus.NoYetUsing)
            {
                MessageDialog.Show("该操作员未启用,无法重置密码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (BuinessRule.GetInstace().brConext.CurrentOperatorId == operatorInfo.operator_id)
            {
                MessageDialog.Show("不能对当前操作员进行密码重置", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
           
            return true;
        }
    }

  
}
