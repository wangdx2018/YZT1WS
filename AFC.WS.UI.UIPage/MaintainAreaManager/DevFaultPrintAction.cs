using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.UIPage.MaintainAreaManager
{
    using AFC.WS.UI.Common;
    using System.Data;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.UIPage.CashManager;

    public class DevFaultPrintAction : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择故障单数据", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
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
            }

            //CrystalRptData crd = new CrystalRptData();
            //crd.ShowRptDialog(new AFC.WS.UI.UIPage.MaintainAreaManager.CrystalMaintainFaultRptStatusReport(), dict, new DataTable());
            return null;
        }

        #endregion
    }
}
