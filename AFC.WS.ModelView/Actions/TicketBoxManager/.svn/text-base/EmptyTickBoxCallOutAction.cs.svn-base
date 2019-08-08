using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Windows;
using AFC.WS.BR;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    /// <summary>
    /// edit by wangdx  20110712
    /// 
    /// 增加了票箱调出的Log
    /// </summary>
    public class EmptyTickBoxCallOutAction : IAction
    {

        #region IAction 成员

        List<string> tickBoxID = null;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tickBoxID")).value != null)
            {
                tickBoxID = (List<string>)actionParamsList.Single(temp => temp.bindingData.Equals("tickBoxID")).value;
            }
            if (tickBoxID != null && tickBoxID.Count > 0)
            {
            }
            else
            {
                Wrapper.ShowDialog("请选择要调出的空票箱。");
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
            MessageBoxResult res = AFC.WS.UI.CommonControls.MessageDialog.Show("是否要调出选中的票箱?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    int delRes = 0;
                    AFC.WS.BR.TickBoxManager.TickBoxManager tickStore = BuinessRule.GetInstace().tickMan;

                    Util.DataBase.BeginTransaction();
                    for (int i = 0; i < tickBoxID.Count; i++)
                    {
                        delRes = tickStore.delTickBoxStatusInfo(tickBoxID[i]) * tickStore.delTickBoxRegistorInfo(tickBoxID[i]);
                        if (delRes != 1)
                        {
                            AFC.WS.UI.CommonControls.MessageDialog.Show("调出空票箱失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            Util.DataBase.Rollback();
                            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Empty_TickBox_CallOut_Action, "1", "调出空票箱失败");
                            ////////////////////////
                            //记日志失败
                            //////////////////////
                            return null;

                        }
                        BuinessRule.GetInstace().logManager.WriteTickBoxOperation(tickBoxID[i], "09");

                    }
                    //////////////////////////////
                    //记日志成功
                    //////////////////////////////
                    Util.DataBase.Commit();
                    
                    //BuinessRule.GetInstace().logManager.WriteTickBoxOperation(tickBoxID[i], BoxOperationType.BOX_CALL_OUT);
                    AFC.WS.UI.CommonControls.MessageDialog.Show("调出空票箱成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Empty_TickBox_CallOut_Action, "0", "调出空票箱成功");
                    return new ResultStatus { resultData = 0, resultCode = 0 };
                }
                catch (Exception ex)
                {
                    AFC.WS.UI.CommonControls.MessageDialog.Show("调出空钱箱失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Empty_TickBox_CallOut_Action, "1", "调出空票箱失败");
                    //////////////////////////////
                    //记日志（失败）
                    //////////////////////////////
                    return null;
                }

            }
            return null;
        }

        #endregion
    }
}
