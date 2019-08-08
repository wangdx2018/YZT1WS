using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;
using AFC.WS.ModelView.Convetors;
using AFC.WS.ModelView.Convertors;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    public class UpdateShowTickValuedAction : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择自定义库存类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            try
            {
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("tick_mana_type"));
                string tickManaType = qc.value.ToString();
                TickValuedProductInfo tickValuedInfo = BuinessRule.GetInstace().GetTickValuedByCode(tickManaType);
                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\TickMonyBoxManager\ui_TickValuedProductUpdate.xml");
                if (icRule != null)
                {
                    if (tickValuedInfo != null)
                    {
                        InitTickValueDetails(icRule, tickValuedInfo);
                    }
                    ic.Initialize(icRule);
                }

                ShowDetailsDialog.ShowDetails("选中的库存编码" + tickManaType, ic, 500, 600);
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 将某个表中的具体的日志信息显示到交互界面上
        /// </summary>
        /// <param name="logInfo">Log的实体信息表</param>
        private void InitTickValueDetails(InteractiveControlRule icRule, TickValuedProductInfo tickDetails)
        {
            if (icRule == null || tickDetails == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], tickDetails);
            }
        }

           /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="operatorInfo">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, TickValuedProductInfo tickInfo)
        {
            try
            {

                TickValuedIssueConvert issueConvert = new TickValuedIssueConvert();

                TickValuedProductConvert productConvert = new TickValuedProductConvert();
                ConvertSimpleFenToYuan yuanConvert = new ConvertSimpleFenToYuan();

                if (controlInfo == null || tickInfo == null)
                    return;
                switch (controlInfo.BindingField.ToUpper())
                {
                    case "CARD_ISSUE_ID":
                        controlInfo.InitValue = issueConvert.Convert(tickInfo.card_issue_id, null, null, null).ToString();
                        break;
                    case "TICK_MANA_TYPE":
                        controlInfo.InitValue = tickInfo.tick_mana_type;
                        break;
                    case "TICK_MANA_TYPE_NAME":
                        controlInfo.InitValue = tickInfo.tick_mana_type_name;
                        break;
                    case "PRODUCT_TYPE":
                        controlInfo.InitValue = productConvert.Convert(tickInfo.product_flag,null,null,null).ToString();
                        break;
                    case "PRE_STORE_MONEY":
                        if (!string.IsNullOrEmpty(tickInfo.product_flag) && tickInfo.product_flag.Equals("00"))
                        {
                            controlInfo.InitValue = yuanConvert.Convert(tickInfo.pre_store_money, null, null, null).ToString();
                        }
                        else
                        {
                            controlInfo.InitValue = tickInfo.pre_store_money.ToString();
                        }
                        break;
                    case "TICK_DEPOSIT":
                        controlInfo.InitValue = yuanConvert.Convert(tickInfo.tick_deposit, null, null, null).ToString();
                        break;
                    case "TICK_SALE_VALUE":
                        controlInfo.InitValue = yuanConvert.Convert(tickInfo.tick_sale_value, null, null, null).ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }


        #endregion
    }
}
