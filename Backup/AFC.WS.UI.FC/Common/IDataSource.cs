using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 数据源接口，创建新的数据源时继承此接口。
    /// </summary>
    public interface IDataSource:IDisposable
    {
        /// <summary>
        /// 数据查询条件
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>返回查询结果数据集</returns>
        void SetQueryParams(List<string> queryCondition);

        /// <summary>
        /// 为当前的数据集排序
        /// </summary>
        /// <param name="sortedName">排序字段</param>
        /// <param name="type">排序方式，正序或者逆序</param>
        /// <returns>返回数据结果集</returns>
        void SetSortParams(string sortedName, SortedType type);

        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="pageSize">每页中所包含的项</param>
        /// <param name="pageIndex"></param>
        /// <returns>数据源集合</returns>
        DataTable FetchPagingData(int pageIndex,int pageSize);

        /// <summary>
        /// 加载数据源
        /// </summary>
        /// <returns>数据源集合</returns>
        DataTable GetDataTable();

        /// <summary>
        /// 数据源名称
        /// </summary>
        string DataSourceName
        {
            set;
            get;
        }

        /// <summary>
        /// 得到当前数据数据列表中的行数
        /// </summary>
        /// <returns>行数</returns>
        int Count();

   
        
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum SortedType : byte
    {
        /// <summary>
        /// 正序
        /// </summary>
        SortAscending=0,

        /// <summary>
        /// 逆序
        /// </summary>
        SortDescding=1
    }
}
