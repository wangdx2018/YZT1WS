using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Windows;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.BR;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.TickMonyBoxManager
{
    public class EmptyMoneyBoxOutAction:  IAction
    {
       #region IAction 成员
        List<string> moneyBoxID = null;

       public bool CheckValid(List<QueryCondition> actionParamsList)
       {
           if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value != null)
           {
               moneyBoxID = (List<string>)actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value;
           }
           if (moneyBoxID != null && moneyBoxID.Count > 0)
           {
           }
           else
           {
               Wrapper.ShowDialog("请选择要调出的空钱箱。");
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
           MessageBoxResult res = MessageDialog.Show("是否要调出选中的钱箱?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
           if (res == MessageBoxResult.Yes)
           {
               try
               {
                   int delRes = 0;
                   Util.DataBase.BeginTransaction();
                   for (int i = 0; i < moneyBoxID.Count; i++)
                   {
                       delRes = TickMonyBoxHelp.Instance.delCashMoneyStatusInfo(moneyBoxID[i]) * TickMonyBoxHelp.Instance.delCashMoneyResgister(moneyBoxID[i]);
                       if (delRes != 1)
                       {
                           MessageDialog.Show("调出空钱箱失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                           Util.DataBase.Rollback();
                           ////////////////////////
                           //记日志失败
                           //////////////////////
                           BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Empty_MoneyBox_Out_Action, "1", "调出空钱箱失败");
                           return null;

                       }
                       BuinessRule.GetInstace().logManager.WriteMoneyBoxOperation(moneyBoxID[i], "09");

                   }
                   //////////////////////////////
                   //记日志成功
                   //////////////////////////////
                   BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Empty_MoneyBox_Out_Action, "0", "调出空钱箱成功");
                   AFC.WS.UI.CommonControls.MessageDialog.Show("调出空钱箱成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                   Util.DataBase.Commit();
                   Util.DataBase.SqlClose();
                   return new ResultStatus { resultCode = 0, resultData = 0 };
               }
               catch (Exception ex)
               {
                   MessageDialog.Show("调出空钱箱失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                   //////////////////////////////
                   //记日志（失败）
                   //////////////////////////////
                   BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Empty_MoneyBox_Out_Action, "1", "调出空钱箱失败");
                   return null;
               }

           }
           return null;
       }

       #endregion
    }
}
