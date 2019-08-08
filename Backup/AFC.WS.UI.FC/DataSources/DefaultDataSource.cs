using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WorkStation.DB;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using AFC.WS.UI.Config;

namespace AFC.WS.UI.DataSources
{
    /// <summary>
    /// 默认数据源，继承IDataSource接口，实现其内部方法。
    /// </summary>
    public class DefaultDataSource : IDataSource
    {

        /// <summary>
        /// 获取数据源名称
        /// </summary>
        [Browsable(false)]
        public string DataSourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 拼写好的数据库脚本
        /// </summary>
        private string sqlCmd;

        /// <summary>
        /// 设置查询条件
        /// </summary>
        private List<string> queryConditionData;

        /// <summary>
        /// 设置排序参数
        /// </summary>
        private string orderByParams;

        #region 自定义属性

        /// <summary>
        /// 数据库连接串
        /// </summary>
        [Filter()]
        public string DBConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Sql 不包括where的数据字段，如果包括where SQL语句将错误，
        /// 联合查询需要通过join来将两个表来建立联系。
        /// </summary>
        [Filter()]
        public string Sql
        {
            set;
            get;
        }

        /// <summary>
        /// 需要分组的字段
        /// </summary>
        [Filter()]
        public string GroupParams
        {
            set;

            get;
        }

        /// <summary>
        /// Having的条件
        /// </summary>
        [Filter()]
        public string HavingParams
        {
            set;
            get;
        }

        /// <summary>
        /// Where 的条件
        /// </summary>
        [Filter()]
        public string WhereParams
        {
            set;

            get;
        }


        /// <summary>
        /// OrderByParams
        /// </summary>
        [Filter()]
        public string OrderByParams
        {
            set;
            get;
        }

        #endregion

        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void InitliaizeDBConnectionString()
        {
            try
            {
                if (string.IsNullOrEmpty(DataSourceManager.ConnectStr))
                {
                    if (string.IsNullOrEmpty(this.DBConnectionString))
                    {
                        try
                        {
                            this.DBConnectionString = ConfigurationSettings.AppSettings["DBConnectionString"];
                        }
                        catch (Exception ex)
                        {
                            WriteLog.Log_Error("Not found [DBConnectionString] setting in appConfig.xml");
                        }
                    }
                    return;
                }
                this.DBConnectionString = DataSourceManager.ConnectStr;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 创建查询字符串
        /// </summary>
        /// <returns>返回查询条件字符串</returns>
        private string CreateQueryString()
        {
            if (string.IsNullOrEmpty(this.Sql)) //sql cmd handle
            {
                WriteLog.Log_Error("datasource must set sql property!");
                return null;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Sql);// sql 查询

            if (!string.IsNullOrEmpty(WhereParams)) //where handle
            {
                sb.Append(" ");
                sb.Append("where");
                sb.Append(" ");
                sb.Append(WhereParams);
                sb.Append(" ");
            }

            if (queryConditionData!=null&&queryConditionData.Count > 0)
            {
                if (string.IsNullOrEmpty(WhereParams))
                {
                    sb.Append(" ");
                    sb.Append("where");
                }
                else
                {
                    sb.Append("and");
                }
                for (int i = 0; i < this.queryConditionData.Count; i++)
                {
                    if (!string.IsNullOrEmpty(queryConditionData[i]))
                    {
                        sb.Append(" ");
                        sb.Append(queryConditionData[i]);
                        sb.Append(" ");
                        if (i < this.queryConditionData.Count - 1)
                            sb.Append("and");
                    }
                }
            }

            if (!string.IsNullOrEmpty(GroupParams)) //group handle
            {
                sb.Append(" ");
                sb.Append("group by");
                sb.Append(" ");
                sb.Append(GroupParams);
                sb.Append(" ");
            }

            if (!string.IsNullOrEmpty(HavingParams))//having handle
            {
                sb.Append(" ");
                sb.Append("having");
                sb.Append(" ");
                sb.Append(HavingParams);
                sb.Append(" ");
            }

            if (!string.IsNullOrEmpty(OrderByParams)||!string.IsNullOrEmpty(orderByParams)) //order by handle
            {
                sb.Append(" ");
                sb.Append("order by ");
                sb.Append(" ");
                if (!string.IsNullOrEmpty(OrderByParams))
                {
                    sb.Append(OrderByParams);
                    sb.Append(" ");
                }
                if (!string.IsNullOrEmpty(orderByParams))
                {
                    if (!string.IsNullOrEmpty(OrderByParams))
                    {
                        sb.Append(",");
                    }
                    sb.Append(orderByParams);
                }
            }

            string finalCmd = sb.ToString();
            WriteLog.Log_Info("query cmd is: " + finalCmd);

            this.sqlCmd = finalCmd;

            return finalCmd;
        }

        #region IDataSource 成员

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <returns></returns>
        public void SetQueryParams(List<string> queryCondition)
        {
            this.queryConditionData = queryCondition;
            DataSourceManager.NotfiyDataSourceChange(DataSourceName);
        }

        /// <summary>
        /// 得到数据表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            //throw new NotImplementedException();
            string cmd = CreateQueryString();
            if (string.IsNullOrEmpty(cmd))
                return null;
            InitliaizeDBConnectionString();
            DBO dbo = new DBO(this.DBConnectionString);
            try
            {
                int res = dbo.SqlConnect();
                if (res != 0)
                    return null;
                DataSet ds = dbo.SqlQuery(out res, cmd);
                if (res != 0)
                    return null;
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Config.Utility.Instance.ConsoleWriteLine(ex, LogFlag.DebugFormat);
                return null;
            }
            finally
            {
                dbo.SqlClose();
            }

        }


        /// <summary>
        /// 得到数据表中的Count
        /// </summary>
        /// <returns>数据表中的数据的个数</returns>
        public int Count()
        {
            //if (string.IsNullOrEmpty(sqlCmd))
            //{
                sqlCmd = CreateQueryString();
                string cmd = string.Format("select count(*) from ({0})", sqlCmd);
                InitliaizeDBConnectionString();
                DBO dbo = new DBO(this.DBConnectionString);
                DataTable dt = null;
                try
                {
                    int res = dbo.SqlConnect();
                    if (res != 0)
                        return -1;
                    DataSet ds = dbo.SqlQuery(out res, cmd);
                    if (res != 0)
                        return -1;
                    dt = ds.Tables[0];
                    res = Convert.ToInt32(dt.Rows[0][0]);
                    return res;
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                    return -1;
                }
                finally
                {
                    dbo.SqlClose();
                }
        }


        /// <summary>
        /// 排序功能函数
        /// </summary>
        /// <param name="sortedName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetSortParams(string sortedName, SortedType type)
        {
            if (type == SortedType.SortAscending)
            {
                this.orderByParams= sortedName + " asc ";
            }
            else
            {
                this.orderByParams= sortedName + "  desc";
            }
            DataSourceManager.NotfiyDataSourceChange(this.DataSourceName);//通知数据源变化
        }

        /// <summary>
        /// 取分页中的数据，从第一页开始
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页的数量</param>
        /// <returns>包含当前页的数据</returns>
        public DataTable FetchPagingData(int pageIndex, int pageSize)
        {
            string cmd= CreateQueryString();

            string finalCmd = string.Format("select * from (select a.*, rownum rd from ({0}) a where rownum<={2} ) where rd>={1}", cmd, pageSize * (pageIndex - 1) + 1, pageIndex * pageSize);

            WriteLog.Log_Info("sql cmd is :" + finalCmd);

            DBO dbo = new DBO(this.DBConnectionString);
            try
            {
                int res = dbo.SqlConnect();
                if (res != 0)
                    return null;
                DataSet ds = dbo.SqlQuery(out res, finalCmd);
                if (res != 0)
                    return null;
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return null;
            }
            finally
            {
                dbo.SqlClose();
            }
        }

    
        #endregion

        #region IDisposable 成员

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.sqlCmd = null;
            this.queryConditionData = null;
        }

        #endregion

    }
}
