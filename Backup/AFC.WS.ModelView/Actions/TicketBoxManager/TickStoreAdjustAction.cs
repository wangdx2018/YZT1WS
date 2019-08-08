using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.RfidRW;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Windows;
using AFC.WS.BR.LogManager;
using AFC.WS.BR.TickBoxManager;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    public class TickStoreAdjustAction:  IAction
    {
        #region IAction 成员

        string tickManaType = string.Empty;
        string  tickNo = string.Empty;
        string  tickReal = string.Empty;
        string tickStatus = string.Empty;
        string remark = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tickManaType")).value != null)
            {
                tickManaType = actionParamsList.Single(temp => temp.bindingData.Equals("tickManaType")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tickNo")).value != null)
            {
                tickNo = actionParamsList.Single(temp => temp.bindingData.Equals("tickNo")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tickReal")).value != null)
            {
                tickReal = actionParamsList.Single(temp => temp.bindingData.Equals("tickReal")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tickStatus")).value != null)
            {
                tickStatus = actionParamsList.Single(temp => temp.bindingData.Equals("tickStatus")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("remark")).value != null)
            {
                remark = actionParamsList.Single(temp => temp.bindingData.Equals("remark")).value.ToString();
            }

            if (string.IsNullOrEmpty(tickManaType))
            {
                Wrapper.ShowDialog("请选择库存类型。");
                return false;
            }
            if (string.IsNullOrEmpty(tickReal))
            {
                Wrapper.ShowDialog("请填写库存调整数量。");
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            Util.DataBase.BeginTransaction();
            int res = BuinessRule.GetInstace().tickMan.UpdateTickSotreInfo(tickManaType, tickReal.ToInt32(),tickStatus);
            if (res != 1)
            {
                WriteLog.Log_Error("tick store adjust error,tickManType=[" + tickManaType + "] ,update tick_store_info error");
                Util.DataBase.Rollback();
                return null;
            }
            res = BuinessRule.GetInstace().tickMan.AddTickStoreChangeLog("02", Convert.ToInt32(this.tickNo),
                Convert.ToInt32(this.tickReal), this.tickManaType, remark, tickStatus);

            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_store_change_info error");
                Util.DataBase.Rollback();
                return null;
            }
            if (!string.IsNullOrEmpty(remark))
            {
                LogManager log = new LogManager();
               res= log.AddRemarkInfo(BuinessRule.GetInstace().OperatorId, "票卡库存调整", remark);
               if (res != 0)
               {
                   WriteLog.Log_Error("insert remark_log info error!");
                   Util.DataBase.Rollback();
                   return null;
               }
            }
            Util.DataBase.Commit();

            if (res == 0)
            {
                Wrapper.ShowDialog("库存调整成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickStore_Adjust_Action, "0", "车票库存调整成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickStore_Adjust_Action, "1", "车票库存调整失败");
            }
            return null;
        }

        #endregion
    }

}