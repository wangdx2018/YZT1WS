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
    /// WS2.0基础组件中调用的此Action处理类，强制登出
    /// </remarks>
    public class ForceLogOutAction : IAction
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
                MessageDialog.Show("请选择列表中强制登出的操作员", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                return false;
            }
            //if ("01" == actionParamsList.First(temp => temp.bindingData.Equals("login_status")).value.ToString())
            //{
            //    MessageDialog.Show("操作员已经登出", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            //    return false;
            //}
            //if (BuinessRule.GetInstace().brConext.CurrentOperatorId == actionParamsList.First(temp => temp.bindingData.Equals("operator_id")).value.ToString())
            //{
            //    MessageDialog.Show("不能强制登出当前操作员", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }
        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            QueryCondition qcOperatorId = actionParamsList.First(temp => temp.bindingData.Equals("operator_id"));
            uint operatorId =qcOperatorId.value.ToString().ConvertNumberStringToUint();

            QueryCondition qcDevId = actionParamsList.First(temp => temp.bindingData.Equals("device_id"));
            uint deviceId = qcDevId.value.ToString().ConvertHexStringToUint();

            QueryCondition qcStationId = actionParamsList.First(temp => temp.bindingData.Equals("t.station_id"));
            ushort stationId = qcStationId.value.ToString().ToHexNumberUShort();
            
            int res = BuinessRule.GetInstace().commProcess.OperatorForceLogOut(operatorId,deviceId,stationId);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Operator_LogOut, res.ToString());
           if (res == 0)
           {
               MessageDialog.Show("强制登出指令下发成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Force_LogOut_Action, "0", "强制登出指令下发成功");
               return new ResultStatus { resultCode = 0, resultData = 0 };
           }
           MessageDialog.Show("强制登出指令下发失败,错误代码" + res.ToString(), "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
           BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Force_LogOut_Action, "1", "强制登出指令下发失败,错误代码");
           return null;

        }

        #endregion
    }
}
