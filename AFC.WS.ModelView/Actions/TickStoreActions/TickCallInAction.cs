using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.TickStoreActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    using AFC.WS.ModelView.Actions.CommonActions;

    /// <summary>
    /// edited by wangdx 20120206
    /// 增加了废票的处理
    /// </summary>
    public class TickCallInAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
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
                    MessageDialog.Show("没有调入散票信息，请添加散票", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("没有选择调出车站，请选择调出车站", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            for (int i = 0; i < actionParamsList.Count; i++)
            {
                if (!actionParamsList[i].bindingData.Equals("callOutStation")
                  )
                {
                    int res = 0;
                    string tickManType = actionParamsList[i].bindingData;
                    decimal instoreNum = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManType,actionParamsList[i].controlName).in_store_num;
                    int reduceNumber = int.Parse(actionParamsList[i].value.ToString());
                    int total = (int)instoreNum + reduceNumber;
                    Util.DataBase.BeginTransaction();

                 //   string tickStatus = actionParamsList.Single(temp => temp.bindingData.Equals("tickStatus")).value.ToString();

                    res = BuinessRule.GetInstace().tickMan.UpdateTickSotreInfo(tickManType, total, actionParamsList[i].controlName);
                    if (res != 1)
                    {
                        WriteLog.Log_Error("update tick_store_info error tickManaType=[" + tickManType + "],number=[" + total.ToString() + "]");
                        Util.DataBase.Rollback();
                        MessageDialog.Show("票卡调人失败", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return null;
                    }

                    string outStationId = actionParamsList.Single(temp => temp.bindingData.Equals("callOutStation")).value.ToString();

                    res = BuinessRule.GetInstace().tickMan.AddTickCallInLog(
                        outStationId,
                        string.Empty,
                        tickManType,
                        (int)reduceNumber,
                        (int)reduceNumber,
                       actionParamsList[i].controlName);

                    if (res != 0)
                    {
                        WriteLog.Log_Error("insert tick_dispatch_info error");
                        Util.DataBase.Rollback();
                        MessageDialog.Show("票卡调人失败", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return null;
                    }

                    res = BuinessRule.GetInstace().tickMan.AddTickStoreChangeLog("00", (int)instoreNum, (int)total, tickManType, "",actionParamsList[i].controlName);

                    if (res != 0)
                    {
                        WriteLog.Log_Error("insert tick_store_change_info error");
                        Util.DataBase.Rollback();
                        MessageDialog.Show("票卡调人失败", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return null;
                    }
                    //BuinessRule.GetInstace().logManager.WriteTickBoxOperation("00000000", "12", tickManType, 0, reduceNumber);
                    Util.DataBase.Commit();
                    //001get tick store info number
                    //002reduce value
                    //todoWriteLog
                }
            }
            MessageDialog.Show("票卡调人成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_CallIn_Action, "0", "票卡调人成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res= BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Tick_CallIn_Action, operatorId, "票卡调人");
            return res == 0;
        }

        #endregion
    }
}
