using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WS.UI.DataSources;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.UI.Actions
{
    /// <summary>
    /// 默认Action，继承IAction接口，对数据进行合法性检查。
    /// 
    /// </summary>
    public class DbAction:IAction
    {     
        /// <summary>
        /// 获得数据源名称
        /// </summary>
        [Filter()]
        public string DataSourceName
        {
            set;
            get;
        }

        /// <summary>
        /// 开始时间控件名称
        /// </summary>
        [Filter()]
        public string StartDateControlName
        {
            set;
            get;
        }

        /// <summary>
        /// 结束日期控件名称
        /// </summary>
        [Filter()]
        public string EndDateControlName
        {
            set;
            get;
        }

        /// <summary>
        /// 日期时间格式串
        /// </summary>
        [Filter()]
        public string DateTimeFormat
        {
            set;
            get;
        }



        #region IAction 成员
        /// <summary>
        /// 检查输入是否合法性
        /// </summary>
        ///<param name="list">actionParams list的数据集合</param>
        /// <returns>检查合法 true，不合法 false</returns>
        public virtual bool CheckValid(List<QueryCondition> list )
        {
            try
            {
                if (string.IsNullOrEmpty(StartDateControlName) && string.IsNullOrEmpty(EndDateControlName))
                    //没有配置有效起始时间的控件名有效结束日期的控件名  此时返回成功。（不检查日期合法性）
                    return true;
                if ((string.IsNullOrEmpty(StartDateControlName) && !string.IsNullOrEmpty(EndDateControlName)
                    || (!string.IsNullOrEmpty(StartDateControlName) && string.IsNullOrEmpty(EndDateControlName))))
                {
                    WriteLog.Log_Error("需要全部配置 StartDateControlName，EndDateControlName");
                    return false;
                }
                string start = string.Empty ;
                string end=string.Empty;
                QueryCondition startDate = list.First(temp => temp.controlName.Equals(StartDateControlName));
                if (startDate != null)
                {
                    if (startDate.value != null)
                        start = startDate.value.ToString();
                }
                QueryCondition endDate = list.First(temp => temp.controlName.Equals(EndDateControlName));
                if (endDate != null)
                {
                    if (endDate.value != null)
                        end = endDate.value.ToString();
                }
                if (string.IsNullOrEmpty(start) && string.IsNullOrEmpty(end))
                    return true;

                if (string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                {
                  MessageDialog.Show("请输入"+startDate.controlLabelName,"错误",MessageBoxIcon.Error,MessageBoxButtons.Ok);
                    return false;
                }
                if (!string.IsNullOrEmpty(start) && string.IsNullOrEmpty(end))
                {
                    MessageDialog.Show("请输入" + endDate.controlLabelName, "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }
                if (string.IsNullOrEmpty(DateTimeFormat))
                {
                    DateTimeFormat = "yyyyMMdd";
                }
                DateTime dtStart = DateTime.ParseExact(start, DateTimeFormat, null);
                DateTime dtEnd = DateTime.ParseExact(end, DateTimeFormat, null);
                if (dtEnd.Subtract(dtStart).Days < 0)
                {
                    MessageDialog.Show(startDate.controlLabelName+"大于"+endDate.controlLabelName, "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
           
              
        }

        /// <summary>
        /// 预留的权限接口
        /// </summary>
        /// <param name="authInfo">权限信息</param>
        /// <returns>符合该权限返回ture，否则false</returns>
        public  virtual bool CheckPremission(object authInfo)
        {
            return true;
        }
        /// <summary>
        /// 执行Action
        /// </summary>
        /// <param name="actionParamsList">actionParams list的数据集合</param>
        /// <returns>执行结果</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            List<string> queryConditions = Util.CreateQueryConditions(actionParamsList);
            for (int i = 0; i < queryConditions.Count; i++)
            {
                WriteLog.Log_Debug("action params is key=[" + queryConditions[i] + "]");
            }
            if (!string.IsNullOrEmpty(DataSourceName))
            {
                IDataSource ds=DataSourceManager.LookupDataSourceByName(this.DataSourceName);
                ds.SetQueryParams(queryConditions);
            }
            return null; 
        }

        #endregion
    }

    
}
