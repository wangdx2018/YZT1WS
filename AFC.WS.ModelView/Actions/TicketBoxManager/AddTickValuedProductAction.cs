using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    public class AddTickValuedProductAction : IAction
    {
        #region IAction ��Ա
        public string tickType = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null && actionParamsList.Count == 0)
            {
                return false;
            }
             try
             {
                 if (actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type_name")).value == null)
                 {
                     MessageDialog.Show("�������Զ�������������", "��ʾ", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                     return false;
                 }
                 if (actionParamsList.Single(temp => temp.bindingData.Equals("card_issue_id")).value == null)
                 {
                     MessageDialog.Show("�������Զ����濨������", "��ʾ", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                     return false;
                 }
                 if (actionParamsList.Single(temp => temp.bindingData.Equals("product_type")).value == null)
                 {
                     MessageDialog.Show("�������Զ������Ʒ����", "��ʾ", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                     return false;
                 }
                 return true;
             }
             catch (Exception ex)
             {
                 return false;
             }
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string currManaType = BuinessRule.GetInstace().GetMaxTickValuedCode();

            ConvertYuanToFen convertFen = new ConvertYuanToFen();
            TickValuedProductInfo tickValuedInfo = new TickValuedProductInfo();
            tickValuedInfo.tick_mana_type = currManaType;
            tickValuedInfo.card_issue_id = actionParamsList.Single(temp => temp.bindingData.Equals("card_issue_id")).value.ToString();
            tickValuedInfo.product_flag = actionParamsList.Single(temp => temp.bindingData.Equals("product_type")).value.ToString();
            if (!string.IsNullOrEmpty(tickValuedInfo.product_flag) && tickValuedInfo.product_flag.Equals("00"))
            {
                tickValuedInfo.pre_store_money = Convert.ToDecimal(convertFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("pre_store_money")).value, null, null, null).ToString());
            }
            else
            {
                tickValuedInfo.pre_store_money = Convert.ToDecimal(actionParamsList.Single(temp => temp.bindingData.Equals("pre_store_money")).value.ToString());
            }

            tickValuedInfo.tick_deposit = Convert.ToDecimal(convertFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("tick_deposit")).value, null, null, null).ToString());
            tickValuedInfo.tick_mana_type_name = actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type_name")).value.ToString();
            tickValuedInfo.tick_sale_value = Convert.ToDecimal(convertFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("tick_sale_value")).value,null,null,null).ToString());
            tickValuedInfo.ticket_phy_type = string.Empty;
            tickValuedInfo.update_date = System.DateTime.Now.ToString("yyyyMMdd");
            tickValuedInfo.update_time = System.DateTime.Now.ToString("HHmmss");

            int res = DBCommon.Instance.InsertTable(tickValuedInfo, "tick_valued_product_info");

            //�����LCWS����LC�����Ľ���ͬ��
            if ("LCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
            {
                int sendRes = BuinessRule.GetInstace().commProcess.TickUpdateNotify(tickValuedInfo);
            }
            if (res != 1)
            {
                Wrapper.ShowDialog("�������Զ���������ʧ�ܡ�");
            }
            else
            {
                Wrapper.ShowDialog("�������Զ��������ͳɹ�,�������Ϊ" + currManaType);
            }

            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
