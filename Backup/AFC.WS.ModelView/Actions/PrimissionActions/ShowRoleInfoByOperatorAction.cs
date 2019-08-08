using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.DataSources;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    /// <summary>
    /// 根据操作员的编号显示角色信息
    /// </summary>
    public class ShowRoleInfoByOperatorAction: IAction
    {
        #region IAction 成员

        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            int count = actionParamsList.Count<QueryCondition>(temp => temp.bindingData.Equals("operator_id"));
            if (count > 1)
            {
                MessageDialog.Show("您只能选择一个操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            try
            {
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id"));
                if (qc != null)
                {
                    string operatorId = qc.value.ToString();
                    AFC.WS.UI.Config.DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_OperatorroleInfo.xml");
                    DataListControl dlc = new DataListControl();
                    dlc.Initliaize(dlr);
                    IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_operatorRoleInfo");
                    if (dataSource != null)
                    {
                        List<string> list = new List<string>();
                        list.Add(string.Format("t.operator_id='{0}'", operatorId));
                        dataSource.SetQueryParams(list);
                    }
                    ShowDetailsDialog.ShowDetails("操作员 " + operatorId + "所有角色信息", dlc, 400, 400);
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
