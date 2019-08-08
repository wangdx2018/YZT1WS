using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    /// <summary>
    ///
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，根据角色信息显示操作员信息
    /// </remarks>
    public class ShowOperatorInfoByRoleIdAction: IAction
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
                MessageDialog.Show("请选择角色", "错误", MessageBoxIcon.Error,MessageBoxButtons.Ok);
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
                string roleId = actionParamsList.Single(temp => temp.bindingData.Equals("role_id")).value.ToString();
                string roleName=actionParamsList.Single(temp => temp.bindingData.Equals("role_name")).value.ToString();

                DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_opreatorInfoByRoleId.xml");
                DataListControl dlc = new DataListControl();
                dlc.Initliaize(dlr);
                IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_QueryOperatorInfoByRoleId");
                if (dataSource != null)
                {
                    List<string> list = new List<string>();
                    list.Add(string.Format("t.role_id='{0}'", roleId));
                    dataSource.SetQueryParams(list);
                }

                ShowDetailsDialog.ShowDetails("角色为" + roleId + " " + roleName+"所有操作员信息", dlc, 600, 800);
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        /// <summary>
        /// 关闭界面释放数据源
        /// </summary>
        /// <param name="sender">窗体</param>
        /// <param name="e">事件源</param>
        private void bw_Closed(object sender, EventArgs e)
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_QueryOperatorInfoByRoleId");
        }

        #endregion
    }
}
