﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class UpdateCashDateSettlementInfoAction : IAction
    {
        #region IAction 成员
            string line_id = string.Empty;
            string station_id = string.Empty;
            string operator_id = string.Empty;
            string oper_date = string.Empty;
            string oper_time = string.Empty;
            string balance_date = string.Empty;
            string tickets_balance = string.Empty;
            string today_cash_bank_total = string.Empty;
            string today_diff_amount = string.Empty;
            string coin_store_amount = string.Empty;
            string tvm_income = string.Empty;
            string ergency_tickets_income = string.Empty;
            string other_income = string.Empty;
            string bom_income = string.Empty;
            string group_tickets_income = string.Empty;
            string balance_income = string.Empty;
            string yesterday_bank_income = string.Empty;
            string account_income = string.Empty;
            string yesterday_bank_store_cash = string.Empty;
            string yesterday_income_amount = string.Empty;
            string today_income_amount = string.Empty;
            string today_subway_income = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            line_id = actionParamsList.Single(temp => temp.bindingData.Equals("line_id")).value.ToString();
            station_id = actionParamsList.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();
            operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            oper_date = actionParamsList.Single(temp => temp.bindingData.Equals("oper_date")).value.ToString();
            oper_time = actionParamsList.Single(temp => temp.bindingData.Equals("oper_time")).value.ToString();
            balance_date = actionParamsList.Single(temp => temp.bindingData.Equals("balance_date")).value.ToString();
            tickets_balance = actionParamsList.Single(temp => temp.bindingData.Equals("tickets_balance")).value.ToString();
            today_cash_bank_total = actionParamsList.Single(temp => temp.bindingData.Equals("today_cash_bank_total")).value.ToString();
            today_diff_amount = actionParamsList.Single(temp => temp.bindingData.Equals("today_diff_amount")).value.ToString();
            coin_store_amount = actionParamsList.Single(temp => temp.bindingData.Equals("coin_store_amount")).value.ToString();
            tvm_income = actionParamsList.Single(temp => temp.bindingData.Equals("tvm_income")).value.ToString();
            ergency_tickets_income = actionParamsList.Single(temp => temp.bindingData.Equals("ergency_tickets_income")).value.ToString();
            other_income = actionParamsList.Single(temp => temp.bindingData.Equals("other_income")).value.ToString();
            bom_income = actionParamsList.Single(temp => temp.bindingData.Equals("bom_income")).value.ToString();
            group_tickets_income = actionParamsList.Single(temp => temp.bindingData.Equals("group_tickets_income")).value.ToString();
            balance_income = actionParamsList.Single(temp => temp.bindingData.Equals("balance_income")).value.ToString();
            yesterday_bank_income = actionParamsList.Single(temp => temp.bindingData.Equals("yesterday_bank_income")).value.ToString();
            account_income = actionParamsList.Single(temp => temp.bindingData.Equals("account_income")).value.ToString();
            yesterday_bank_store_cash = actionParamsList.Single(temp => temp.bindingData.Equals("yesterday_bank_store_cash")).value.ToString();
            yesterday_income_amount = actionParamsList.Single(temp => temp.bindingData.Equals("yesterday_income_amount")).value.ToString();
            today_income_amount = actionParamsList.Single(temp => temp.bindingData.Equals("today_income_amount")).value.ToString();
            today_subway_income = actionParamsList.Single(temp => temp.bindingData.Equals("today_subway_income")).value.ToString();
            if (string.IsNullOrEmpty(tickets_balance))
            {
                MessageDialog.Show("运营日不能为空", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            CashDateSettlementInfo CDInfo = new CashDateSettlementInfo();
            int seq = 0;
            CDInfo.account_income = Convert.ToDecimal(account_income);
            CDInfo.bom_income = Convert.ToDecimal(bom_income);
            CDInfo.coin_store_amount = Convert.ToDecimal(coin_store_amount);
            CDInfo.group_tickets_income = Convert.ToDecimal(group_tickets_income);
            CDInfo.income_store = Convert.ToDecimal(balance_income);
            CDInfo.line_id = line_id;
            CDInfo.oper_date = oper_date;
            CDInfo.oper_time = oper_time;

            CDInfo.operator_id = operator_id;
            CDInfo.others_income = Convert.ToDecimal(other_income);
            CDInfo.run_date = balance_date;
            CDInfo.station_id = station_id;
            CDInfo.today_cash_bank_total = Convert.ToDecimal(today_cash_bank_total);
            CDInfo.tickets_remain = Convert.ToDecimal(tickets_balance);
            CDInfo.today_diff_amount = Convert.ToDecimal(today_diff_amount);
            CDInfo.today_income_amount = Convert.ToDecimal(today_income_amount);

            CDInfo.today_subway_income = Convert.ToDecimal(today_subway_income);
            CDInfo.tvm_income = Convert.ToDecimal(tvm_income);
            CDInfo.urgency_tikets_income = Convert.ToDecimal(ergency_tickets_income);
            CDInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            CDInfo.update_time = DateTime.Now.ToString("HHmmss");

            CDInfo.tomorrow_bank_income = Convert.ToDecimal(yesterday_bank_income);
            CDInfo.yesterday_income_amount = Convert.ToDecimal(yesterday_income_amount);

            int result = DBCommon.Instance.UpdateTable(CDInfo, "CASH_DATE_SETTLEMENT_INFO", new KeyValuePair<string, string>("line_id", CDInfo.line_id), new KeyValuePair<string, string>("station_id", CDInfo.station_id), new KeyValuePair<string, string>("run_date", CDInfo.run_date));
            if (result != 1)
            {
                MessageDialog.Show("运营日结算修改失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                MessageDialog.Show("运营日结算修改成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }

            return null;
        }

        #endregion
    }
}
