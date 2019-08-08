using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WorkStation.DB;
using System.Data;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;


namespace AFC.WS.UI.DataSources
{
    /// <summary>
    /// 图表使用数据源，继承IDataSource接口。
    /// </summary>
    public partial class ChartDataSource : IDataSource
    {

        #region IDisposable 成员

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDataSource 成员

        /// <summary>
        /// 数据查询条件
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>返回查询结果数据集</returns>
        public void SetQueryParams(List<string> queryCondition)
        {
            
        }
        /// <summary>
        /// 为当前的数据集排序
        /// </summary>
        /// <param name="sortedName">排序字段</param>
        /// <param name="type">排序方式，正序或者逆序</param>
        /// <returns>返回数据结果集</returns>
        public void SetSortParams(string sortedName, SortedType type)
        {
            
        }
        /// <summary>
        /// 分页函数
        /// </summary>
        /// <param name="pageSize">每页中所包含的项</param>
        /// <param name="pageIndex"></param>
        /// <returns>数据源集合</returns>
        public DataTable FetchPagingData(int pageIndex, int pageSize)
        {
            return null;
        }
        /// <summary>
        /// 加载数据源
        /// </summary>
        /// <returns>数据源集合</returns>
        public DataTable GetDataTable()
        {
            DataTable dt = new System.Data.DataTable("my table");
            DataColumnCollection c = dt.Columns;
            c.Add("选择", typeof(bool));
            c.Add("设备编码", typeof(string));
            c.Add("线路", typeof(string));
            c.Add("车站", typeof(string));
            c.Add("设备名称", typeof(string));
            c.Add("timePoint", typeof(string));
            Random rd = new Random();
            DataRowCollection r = dt.Rows;
            for (int i = 0; i <100; i++)
            {
                r.Add(new object[] {false,rd.Next(0,500), 
                    i+10, 
                     -i ,
                    i+100 ,
                    format(i)
            
                });
            }
            return dt;
        }
        /// <summary>
        /// 加载数据源
        /// </summary>
        /// <returns>数据源集合</returns>
        public DataTable GetDataTable1()
        {
            DataTable dt = new System.Data.DataTable("my table");
            DataColumnCollection c = dt.Columns;
            c.Add("选择", typeof(bool));
            c.Add("设备编码", typeof(string));
            c.Add("线路", typeof(string));
            c.Add("车站", typeof(string));
            c.Add("设备名称", typeof(string));
            c.Add("timePoint", typeof(string));
            Random rd = new Random();
            DataRowCollection r = dt.Rows;
            for (int i = 0; i < 100; i++)
            {
                r.Add(new object[] {false,rd.Next(0,400), 
                    i+10, 
                     -i ,
                    i+100 ,
                    format(i)
            
                });
            }
            return dt;
        }
        /// <summary>
        /// 格式化使用
        /// </summary>
        /// <returns>数据源集合</returns>
        private string format(int i)
        {
            if (i < 10)
            {
                return "000" + i.ToString();
            }
            if (i >= 10 && i < 100)
            {
                return "00" + i.ToString();
            }
            if (i >= 100)
            {
                return "0" + i.ToString();
            }
            return "0000";
        }
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
        /// 得到当前数据数据列表中的行数
        /// </summary>
        /// <returns>行数</returns>
        public int Count()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
