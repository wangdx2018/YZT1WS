using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.DataSources;
    /// <summary>
    /// </summary> 
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，显示角色的全部合法的系统功能信息
    /// </remarks>
    public class ShowRoleInfoByFunctionIdAction  :IAction
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
                MessageDialog.Show("请选择功能", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            try
            {
                string functionId = actionParamsList.Single(temp => temp.bindingData.Equals("function_id")).value.ToString();
                string functionName = actionParamsList.Single(temp => temp.bindingData.Equals("function_name")).value.ToString();

                DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_roleInfoByFunctionId.xml");
                DataListControl dlc = new DataListControl();
                dlc.Initliaize(dlr);
                IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_RoleInfoByFunctionId");
                if (dataSource != null)
                {
                    List<string> list = new List<string>();
                    list.Add(string.Format("rf.function_id='{0}'", functionId));
                    dataSource.SetQueryParams(list);
                }
                ShowDetailsDialog.ShowDetails("功能" + functionId + " " + functionName+"  所有的角色信息", dlc, 600,600);
                return new ResultStatus { resultCode = 0, resultData = 0 };

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}
