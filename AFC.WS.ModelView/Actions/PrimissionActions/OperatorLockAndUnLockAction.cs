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
    using AFC.WS.BR.CommBuiness;
    using AFC.WS.BR.LogManager;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    using AFC.WS.ModelView.UIContext;
    
    //-->操作员解锁和锁定Action
    /// <summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，锁定Action
    /// </remarks>
    public class OperatorLockAndUnLockAction: IAction
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
                MessageDialog.Show("请选择操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            var collection = actionParamsList.Where(temp => temp.bindingData.Equals("operator_id"));
            if (collection.Count() > 1)
            {
                 MessageDialog.Show("您只能选择一个操作员进行解锁!", "提示",MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            if (string.IsNullOrEmpty(this.LockedOperation))
                return null;
            try
            {
                int res;
                string operatorId = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
                res=BuinessRule.GetInstace().commProcess.UnlockOperator(operatorId.ConvertNumberStringToUint());
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_UnLocked, res.ToString());
                string message = MessageCfg.getMessageContent("1305", res.ToString());
                if (res == 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_Lock_And_UnLock_Action, "0", "操作员解锁成功");
                    MessageDialog.Show(message, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
                else 
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_Lock_And_UnLock_Action, "1", "操作员解锁失败");
                    MessageDialog.Show(message, "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return null;
                }
                
         
            }
            catch (Exception ex)
            {

            }
            return null;
              
        }

        /// <summary>
        /// 锁定操作
        /// </summary>
        [Filter()]
        public string LockedOperation
        {
            set;
            get;
        }

        #endregion
    }

}
