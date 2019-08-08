using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.UIPage.CashManager
{
    using AFC.WS.UI.Common;
    using System.Data;
    using AFC.WS.UI.CommonControls;

    public class TVMSettlementPrintAction:IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择TVM结帐单数据", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
          
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return false;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();

            for (int i = 0; i < actionParamsList.Count; i++)
            {

                dict.Add(actionParamsList[i].bindingData, actionParamsList[i].value.ToString());
                if(actionParamsList[i].bindingData.Contains("sum"))
                {
                    double res=0;
                    bool result=false;
                    result = double.TryParse(actionParamsList[i].value.ToString(), out res);
                    if(result)
                    {
                        dict[actionParamsList[i].bindingData] = (res / 100).ToString();
                    }
                }
            }

            CrystalRptData crd = new CrystalRptData();
            crd.ShowRptDialog(new AFC.WS.UI.UIPage.CashManager.CrystalTvmSettlementReport(), dict, new DataTable());
            return null;
        }

        #endregion
    }
}
