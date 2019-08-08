using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Actions.TickStoreActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;

    public class TickCallOutAction: IAction, IDoublePrimissionHandler
    {
        #region IAction 成员

        bool IAction.CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("没有选择调出车站，请选择调出车站", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                int count = actionParamsList.Count(temp => temp.bindingData.Equals("callOutStation"));
                if (count == actionParamsList.Count)
                {
                    MessageDialog.Show("没有调出散票信息，请添加散票", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("没有选择调人车站，请选择调人车站", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
        }

        bool IAction.CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        ResultStatus IAction.DoAction(List<QueryCondition> actionParamsList)
        {
            int res = 0;
            for (int i = 0; i < actionParamsList.Count; i++)
            {
                if (!actionParamsList[i].bindingData.Equals("callOutStation"))
                {
                    string tickManType = actionParamsList[i].bindingData;
                    decimal instoreNum = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManType,actionParamsList[i].controlName).in_store_num;
                    int reduceNumber = int.Parse(actionParamsList[i].value.ToString());
                    int leftNo = (int)instoreNum - reduceNumber;
                    BuinessRule.GetInstace().tickMan.UpdateTickSotreInfo(tickManType, leftNo,actionParamsList[i].controlName);
                    string outStationId = actionParamsList.Single(temp => temp.bindingData.Equals("callOutStation")).value.ToString();
                    res = BuinessRule.GetInstace().tickMan.AddTickCallOutLog(outStationId,
                        tickManType, reduceNumber, reduceNumber,actionParamsList[i].controlName);
                    if (res != 0)
                    {
                        WriteLog.Log_Error("insert tick_dispatch_info error");
                        Util.DataBase.Rollback();
                        return null;
                    }
                    res = BuinessRule.GetInstace().tickMan.AddTickStoreChangeLog("01", (int)instoreNum, (int)leftNo, tickManType, "", actionParamsList[i].controlName);

                    if (res != 0)
                    {
                        WriteLog.Log_Error("insert tick_store_change_info error");
                        Util.DataBase.Rollback();
                        MessageDialog.Show("票卡调出失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return null;
                    }
                    //BuinessRule.GetInstace().logManager.WriteTickBoxOperation("00000000", "12", tickManType, 0, reduceNumber);
                    Util.DataBase.Commit();
                }
                //001get tick store info number
                //002reduce value
                //todoWriteLog
            }
            MessageDialog.Show("票卡调出成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_CallOut_Action, "0", "票卡调出成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Tick_CallOut_Action, operatorId, "票卡调出");
            return res == 0;
        }

        #endregion
    }
}
