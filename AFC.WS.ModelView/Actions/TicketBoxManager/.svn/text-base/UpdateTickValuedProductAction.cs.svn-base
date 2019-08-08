using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.ModelView.Convertors;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    public class UpdateTickValuedProductAction : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null && actionParamsList.Count == 0)
                return false;
            string tickTypeName = actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type_name")).value.ToString();
            if (string.IsNullOrEmpty(tickTypeName))
            {
                MessageDialog.Show("请输入库存类型名称", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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

            string tickManaType = actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type")).value.ToString();
            TickValuedProductInfo tickValuedInfo = BuinessRule.GetInstace().GetTickValuedByCode(tickManaType);
            ConvertYuanToFen convertFen = new ConvertYuanToFen();


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

            tickValuedInfo.tick_deposit = Convert.ToDecimal(convertFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("tick_deposit")).value,null,null,null).ToString());
            tickValuedInfo.tick_mana_type = actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type")).value.ToString();
            tickValuedInfo.tick_mana_type_name = actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type_name")).value.ToString();
            tickValuedInfo.tick_sale_value = Convert.ToDecimal(convertFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("tick_sale_value")).value,null,null,null).ToString());
            tickValuedInfo.ticket_phy_type = string.Empty;
            tickValuedInfo.update_date = System.DateTime.Now.ToString("yyyyMMdd");
            tickValuedInfo.update_time = System.DateTime.Now.ToString("HHmmss");

            int res = 0;
            res = DBCommon.Instance.UpdateTable(tickValuedInfo, "tick_valued_product_info", new KeyValuePair<string, string>("tick_mana_type", tickValuedInfo.tick_mana_type));

            //如果是LCWS，向LC发报文进行同步
            if ("LCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
            {
                int sendRes = BuinessRule.GetInstace().commProcess.TickUpdateNotify(tickValuedInfo);
            }
            if (res != 1)
            {
                Wrapper.ShowDialog("更新自定义票种失败。");
            }
            else
            {
                Wrapper.ShowDialog("更新自定义票种成功。");
            }

            return new ResultStatus { resultCode = 0, resultData = 0 };

        }

        #endregion
    }
}
