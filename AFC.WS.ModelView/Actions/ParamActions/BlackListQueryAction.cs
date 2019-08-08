using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.DataSources;

    /// <summary>
    /// added by wangdx 20100510 黑名单卡查询Action
    /// 
    /// </summary>
    public class BlackListQueryAction:IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
                return false;
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return false;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            try
            {
                string para_version = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();
                List<string> list = new List<string>();
                list.Add(string.Format("para_version='{0}'", para_version));
                this.NotifyDataSourceQueryConditionChange("ds_para_4011_ykt_blacklist",list);
                this.NotifyDataSourceQueryConditionChange("ds_para_4012_ypt_full_black_list",list);
                this.NotifyDataSourceQueryConditionChange("ds_para_4013_ypt_incre_black_list",list);
                this.NotifyDataSourceQueryConditionChange("ds_para_4014_ypt_range_blacklist",list);
                this.NotifyDataSourceQueryConditionChange("ds_para_4015_staff_blacklist",list);
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void NotifyDataSourceQueryConditionChange(string dataSourceName,List<string> list)
        {
            IDataSource ds = DataSourceManager.LookupDataSourceByName(dataSourceName);
            if (ds != null)
                ds.SetQueryParams(list);
        }

        #endregion
    }
}
