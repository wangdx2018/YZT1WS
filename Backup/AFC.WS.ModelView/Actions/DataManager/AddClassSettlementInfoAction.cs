using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class AddClassSettlementInfoAction : IAction
    {
        #region IAction 成员
        string tickets_remain = string.Empty;
        string cash_balance = string.Empty;
        string run_date = string.Empty;
        string bom_shift_id = string.Empty;
        string operator_id = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            tickets_remain = actionParamsList.Single(temp => temp.bindingData.Equals("tickets_remain")).value.ToString();
            cash_balance = actionParamsList.Single(temp => temp.bindingData.Equals("cash_balance")).value.ToString();
            run_date = actionParamsList.Single(temp => temp.bindingData.Equals("run_date")).value.ToString();
            operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            if (string.IsNullOrEmpty(tickets_remain))
            {
                tickets_remain = "0";
            }
            if (string.IsNullOrEmpty(cash_balance))
            {
                cash_balance = "0";
            }
            if (string.IsNullOrEmpty(run_date))
            {
                MessageDialog.Show("运营日不能为空!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(operator_id))
            {
                MessageDialog.Show("操作员不能为空!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            ResultStatus rs = new ResultStatus();
            CashBomShiftSettlementInfo csInfo = new CashBomShiftSettlementInfo();
            csInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            csInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            int seq = 0;
            csInfo.bom_shift_id = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            csInfo.cash_balance = cash_balance;
            csInfo.operator_id = operator_id;
            csInfo.run_date = run_date;
            csInfo.oper_date = DateTime.Now.ToString("yyyyMMdd");
            csInfo.oper_time = DateTime.Now.ToString("HHmmss");
            csInfo.tickets_remain = tickets_remain;
            csInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            csInfo.update_time = DateTime.Now.ToString("HHmmss");
            int result = DBCommon.Instance.InsertTable(csInfo, "cash_bom_shift_settlement_info");
            if (result != 1)
            {
                MessageDialog.Show("班次结算增加失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                rs.resultCode = 1;
            }
            else
            {
                MessageDialog.Show("班次结算增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                rs.resultCode = 0;
            }
            return rs;
        }

        #endregion
    }
}
