using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 对数据表进行操作的类。
    /// </summary>
    public class DbHelper
    {
        #region --> Property

        /// <summary>
        /// 实例化类
        /// </summary>
        private static DbHelper _Instance;

        /// <summary>
        /// 实例化类
        /// </summary>
        public static DbHelper Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance = new DbHelper();
                }
                return _Instance;
            }
            set { _Instance = value; }
        }

        /// <summary>
        /// 將獲取存放配置文件當中數據庫的所有者。
        /// </summary>
        private string _DatabaseOwner;

        /// <summary>
        /// 获取当前的所有者。
        /// </summary>
        public string DatabaseOwner
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_DatabaseOwner))
                    {

                        string connectionString = ConfigurationManager.AppSettings["DBConnectionString"].ToString();
                        string[] scArray = connectionString.Split(';');

                        //string user = "";
                        foreach (string str in scArray)
                        {
                            if (str.Contains("user"))
                            {
                                //user = str.ToUpper();

                                string[] temp = str.Split('=');
                                _DatabaseOwner = temp[1].ToUpper();
                                break;
                            }
                        }
                    }
                    return _DatabaseOwner;
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                    return "";
                }
            }
        }

        #endregion --> Property

        #region --> Methods

        //-->获取数据库中所有表。
        /// <summary>
        /// 获取数据库中所有表。
        /// </summary>
        /// <returns>返回一个List集体</returns>
        public List<string> GetAllTables()
        {
            List<string> strList = new List<string>();

            string sqlQuery = string.Format("select table_name from information_schema.tables where table_schema='{0}' and table_type='base table';", ConfigurationManager.AppSettings["DBName"]);

            Utility.Instance.ConsoleWriteLine(sqlQuery, LogFlag.InfoFormat);

            int retCode = 0;

           
            DataSet ds = Util.DataBase.SqlQuery(out retCode, sqlQuery);

            if (retCode != 0)
            {
                return null;
            }

            int MaxCount = ds.Tables[0].Rows.Count;

            for (int i = 0; i < MaxCount; i++)
            {
                string tableName = ds.Tables[0].Rows[i]["table_name"].ToString();

                strList.Add(tableName);
            }

            return strList;
        }

        //-->获取表的字段
        /// <summary>
        /// 根据表的名称获取，表字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns>返回 TableFieldProperty集合</returns>
        public List<TableFieldProperty> FieldListByTableName(string tableName)
        {
            string sqlQuery = string.Format(@"SELECT
	table_name as 'TABLE_NAME',
	column_name as 'COLUMN_NAME',
	column_comment as 'comments',
	data_type as 'DATA_TYPE'
FROM
	information_schema. COLUMNS
WHERE
	table_name='{0}'", tableName);

            Utility.Instance.ConsoleWriteLine(sqlQuery, LogFlag.InfoFormat);

            int retCode = 0;

            DataSet ds = Util.DataBase.SqlQuery(out retCode, sqlQuery);

            if (retCode != 0)
            {
                return null;
            }

            List<TableFieldProperty> fieldList = new List<TableFieldProperty>();

            int MaxCount = ds.Tables[0].Rows.Count;

            for (int i = 0; i < MaxCount; i++)
            {

                TableFieldProperty tf = new TableFieldProperty();

                tf.ColumnName = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString().ToLower();

                tf.DateType = ds.Tables[0].Rows[i]["DATA_TYPE"].ToString().ToLower();

                tf.Comments = ds.Tables[0].Rows[i]["comments"].ToString();

                tf.BindingField = tableName.ToLower() + "." + tf.ColumnName;

                fieldList.Add(tf);
            }

            return fieldList;
        }

        //-->生成SQL语句。
        /// <summary>
        /// 根据字段集合和表的名称，拼出SQL语句。
        /// </summary>
        /// <param name="fieldList">字段集合</param>
        /// <param name="tableName">表的名称</param>
        /// <returns>返回string SQL语句</returns>
        public string GetSqlSentence(List<TableFieldProperty> fieldList, string tableName)
        {
            if (fieldList != null && fieldList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (TableFieldProperty tf in fieldList)
                {
                    sb.Append(tableName.ToLower() + "." + tf.ColumnName).Append(",");
                }

                return "select " + sb.Remove(sb.Length - 1, 1).ToString() + " from " + tableName.ToLower();
            }
            else
            {
                return "";
            }


        }
        /// <summary>
        /// 存放数据表中所有表字段备注。
        /// </summary>
        private List<TableColumnComments> _GetAllColumnComments;

        /// <summary>
        /// 获取数据中所有表字段备注
        /// </summary>
        /// <returns>返回 TableColumnComments 集合</returns>
        public List<TableColumnComments> GetAllColumnComments()
        {
            if (_GetAllColumnComments == null ||
                _GetAllColumnComments.Count <= 0)
            {
                _GetAllColumnComments = new List<TableColumnComments>();

                string sqlQuery = string.Format("select atc.owner,atc.table_name,atc.column_name,atc.comments from all_col_comments atc ");
                sqlQuery += string.Format(" where atc.comments is not null and  atc.owner = '{0}' order by atc.table_name ", DatabaseOwner);

                Utility.Instance.ConsoleWriteLine(sqlQuery, LogFlag.Info);

                int retCode = 0;

                DataSet ds = Util.DataBase.SqlQuery(out retCode, sqlQuery);

                if (retCode != 0)
                {
                    return null;
                }

                int MaxCount = ds.Tables[0].Rows.Count;

                TableColumnComments tcc = null;

                for (int i = 0; i < MaxCount; i++)
                {

                    tcc = new TableColumnComments();

                    tcc.ColumnName = ds.Tables[0].Rows[i]["column_name"].ToString().ToLower();

                    tcc.Comments = ds.Tables[0].Rows[i]["comments"] != null ? ds.Tables[0].Rows[i]["comments"].ToString().ToLower() : "";

                    tcc.Owner = ds.Tables[0].Rows[i]["owner"].ToString().ToLower();

                    tcc.TableName = ds.Tables[0].Rows[i]["table_name"].ToString().ToLower();

                    _GetAllColumnComments.Add(tcc);
                }
            }

            return _GetAllColumnComments;
        }

        /// <summary>
        /// 获取表列的值
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">表列字段名称</param>
        /// <returns>返回表字段的备注内容</returns>
        public string GetTableFieldComment(string tableName, string columnName)
        {
            string result = "";
            foreach (TableColumnComments tcc in GetAllColumnComments())
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    if (tcc.ColumnName == columnName.ToLower())
                    {
                        if (tcc.Comments != "")
                        {
                            result = tcc.Comments;
                            break;
                        }
                    }
                }
                else
                {
                    if (tcc.ColumnName == columnName && tcc.TableName == tableName)
                    {
                        result = tcc.Comments;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据SQL语句获取字段。
        /// </summary>
        /// <param name="retCode">返回代码</param>
        /// <param name="sqlQuery">SQL语句</param>
        /// <returns>返回SQL语句中字段属性集合</returns>
        public List<TableFieldProperty> GetFieldListBySqlSentence(out int retCode, string sqlQuery)
        {
            if (string.IsNullOrEmpty(sqlQuery))
            {
                retCode = -1;
                return null;
            }
            //sqlQuery = string.Format("select * from dev_info di ");
            //sqlQuery += string.Format("left join glb_station_info gsi on di.line_code = gsi.line_code and di.station_code = gsi.station_code ");
            //sqlQuery = sqlQuery.Replace(';', ' ');
            //sqlQuery += string.Format(" where 1<>1 ");

            sqlQuery = string.Format("{0}  where rownum < 1 ", sqlQuery.Replace(';', ' '));

            Utility.Instance.ConsoleWriteLine(sqlQuery, LogFlag.Info);

            DataSet ds = Util.DataBase.SqlQuery(out retCode, sqlQuery);

            if (retCode != 0)
            {
                return new List<TableFieldProperty>();
            }

            List<TableFieldProperty> tfList = new List<TableFieldProperty>();

            TableFieldProperty tf = null;

            int MaxCount = ds.Tables[0].Columns.Count;

            for (int i = 0; i < MaxCount; i++)
            {
                tf = new TableFieldProperty();

                tf.DateType = ds.Tables[0].Columns[i].DataType.Name.ToString();

                tf.ColumnName = ds.Tables[0].Columns[i].ColumnName.ToLower();

                tf.BindingField = ds.Tables[0].Columns[i].ColumnName.ToLower();

                tf.Comments = GetTableFieldComment(null, tf.ColumnName);

                tfList.Add(tf);
            }

            return tfList;
        }

        #endregion --> Methods
    }
}
