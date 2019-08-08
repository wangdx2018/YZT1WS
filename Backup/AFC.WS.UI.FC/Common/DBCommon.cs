using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace AFC.WS.UI.Common
{
    using AFC.WorkStation.DB;
    using System.Globalization;
    using AFC.WS.UI.Config;
    using System.Windows;
    using System.Windows.Media;


    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110221
    /// 代码功能：数据库的通用操作类，通过实体类进行增删改查的通用处理。
    /// 修订记录：20110304 wangdx 增加了 CreateModelData函数
    /// 
    /// 修订记录：20110317 增加了16进制显示
    /// 
    /// 修订记录：20120706 修改了将日期转换成DateTime函数。增加了时差。
    /// </summary>
    public class DBCommon
    {

        #region [private Method]

        private static DBCommon _Instance;

        /// <summary>
        /// 单件创建Util类对象
        /// </summary>
        public static DBCommon Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance = new DBCommon();
                }
                return DBCommon._Instance;
            }
        }

        /// <summary>
        /// 给泛型对象赋值。
        /// </summary>
        /// <typeparam name="T">泛型对象类型</typeparam>
        /// <param name="item">泛型对象</param>
        /// <param name="columns">数据列的集合</param>
        /// <param name="dr">数据行内容</param>
        /// <returns>返回T类型实例</returns>
        private T GetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new()
        {
            if (item == null)
            {
                return default(T);
            }
            try
            {
                Type t = item.GetType();
                MemberInfo[] miList = t.GetMembers();
                foreach (var mi in miList)
                {
                    bool isExist = false;
                    foreach (DataColumn dc in columns)
                    {
                        if (dc.ColumnName.ToLower() == mi.Name.ToLower())
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (mi is PropertyInfo)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        if (isExist)
                        {
                            try
                            {
                                //pi.SetValue(item, JudgeValue(pi.PropertyType.Name.ToString(), dr, mi.Name), null);
                                pi.SetValue(item, Util.ParsePropertyValue(pi, dr[mi.Name].ToString()), null);
                            }
                            catch (Exception ee)
                            {

                                WriteLog.Log_Error("public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new() 方法,给T赋值时出错," +
                                     t.Name + ";字段名称：" + mi.Name + "。");
                            }
                        }
                    }
                    if (mi is FieldInfo)
                    {
                        FieldInfo fi = mi as FieldInfo;
                        if (isExist)
                        {
                            try
                            {
                                fi.SetValue(item, dr[mi.Name]);
                            }
                            catch (Exception ee)
                            {
                                WriteLog.Log_Error("public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new() 方法,给T赋值时出错。");
                            }
                        }
                    }
                }
                return item;
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error("public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new() 方法出错。");
                return default(T);
            }
        }

        /// <summary>
        /// 返回DataTable中的数据值
        /// </summary>
        /// <param name="propertyType">属性类型</param>
        /// <param name="dr">数据行</param>
        /// <param name="columnName">列名</param>
        /// <returns>返回DataTable转换之后的数据</returns>
        private object JudgeValue(string propertyType, DataRow dr, string columnName)
        {
            object item = null;
            bool valueIsNull = dr.IsNull(columnName);
            if (valueIsNull == true)
            {
                switch (propertyType.ToLower())
                {
                    case "string":
                        item = "";
                        break;
                    case "decimal":
                        item = Convert.ToDecimal(0);
                        break;
                    case "int16":
                        item = Convert.ToInt16(0);
                        break;
                    case "int32":
                        item = Convert.ToInt32(0);
                        break;
                    case "datetime":
                        item = DateTime.Now;
                        break;
                    case "dbnull":
                        item = "";
                        break;
                    default:
                        item = "";
                        break;
                }
            }
            else
            {
                item = dr[columnName];
            }
            return item;
        }

        /// <summary>
        /// 获取DataTable里的内容。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="dt">datatable</param>
        /// <returns>返回一个T集合对象</returns>
        private List<T> GetTModelValue<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            try
            {
                List<T> item = new List<T>();
                foreach (DataRow dr in dt.Rows)
                {
                    T a = new T();
                    try
                    {
                        GetModelValue<T>(a, dt.Columns, dr);
                        item.Add(a);
                    }
                    catch (Exception e)
                    {
                        WriteLog.Log_Error("public List<T> SetTModelValue<T>(DataTable dt) where T:new () 循环赋值出错了");
                    }

                }
                return item;
            }
            catch (Exception ee)
            {
             
                WriteLog.Log_Error("public List<T> SetTModelValue<T>(DataTable dt) where T:new () 出错");
         
            }
            return null;
        }


        #endregion


        #region [public Method]

        /// <summary>
        /// 给T对象赋值。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="sqlQuery">查询语句</param>
        /// <returns>返回一个T集合对象</returns>
        public List<T> GetTModelValue<T>(string sqlQuery) where T : new()
        {
            List<T> item = new List<T>();
            int res = 0;
            try
            {
                DataTable dt = AFC.WS.UI.Common.Util.DataBase.SqlQuery(out res, sqlQuery).Tables[0];
                if (res != 0)
                    return null;
                item = GetTModelValue<T>(dt);
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error("public List<T> SetTModeValue<T>(string sqlQuery) where T : new() 方法出错。");
                //Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return item;
        }

        /// <summary>
        /// 给T对象赋值。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="sqlQuery">查询语句</param>
        /// <returns>返回T内容</returns>
        public T GetModelValue<T>(string sqlQuery) where T : new()
        {
            return GetTModelValue<T>(sqlQuery).GetTContext<T>();
        }


        /// <summary>
        /// 插入数据库表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">泛型对象 需要有无参的构造函数</param>
        /// <param name="tableName">表名</param>
        /// <returns>成功返回1，否则返回错误代码</returns>
        public int InsertTable<T>(T entity, string tableName) where T : class, new()
        {
            if (entity == null || string.IsNullOrEmpty(tableName))
            {
                WriteLog.Log_Error("InsertObject<T>(T entity,string tableName),params error!");
                return -1;
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder fieldString = new StringBuilder();
            StringBuilder fieldValue = new StringBuilder();
            Type t = entity.GetType();
            PropertyInfo[] fieldArray = t.GetProperties();
            int i = 0;
            foreach (var temp in fieldArray)
            {
                fieldString.Append(temp.Name);
                object fieldData = t.GetProperty(temp.Name).GetValue(entity, null);
                if (fieldData is DateTime)
                {
                    string dateTime = ((DateTime)(fieldData)).ToString("yyyyMMdd");
                    fieldValue.Append(string.Format("{0}",
                        string.Format("to_date('{0}','yyyyMMdd')", dateTime)));
                }
                else
                {
                    fieldValue.Append(string.Format("'{0}'",
                        fieldData == null ? string.Empty : fieldData.ToString()));
                }
                if (i < fieldArray.Length - 1)
                {
                    fieldValue.Append(",");
                    fieldString.Append(",");
                }
                i++;
            }
            sb.Append(string.Format("insert into {0} ({1}) values ({2})", tableName, fieldString.ToString(), fieldValue.ToString()));
            WriteLog.Log_Info(string.Format("insert cmd={0}", sb.ToString()));
            int retCode = 0;
            return Util.DataBase.SqlCommand(out retCode, sb.ToString());
        }

        /// <summary>
        /// 更新数据库表
        /// </summary>
        /// <typeparam name="T">泛型（数据库实体类）</typeparam>
        /// <param name="instance">数据库实体类对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="objectParams">条件 （只是支持 a='1' 等号操作不支持>,！= 这些操作符）</param>
        /// <returns>成功返回1，否则返回0</returns>
        public  int UpdateTable<T>(T instance, string tableName, params KeyValuePair<string, string>[] objectParams) where T : class
        {
            Type t = typeof(T);
            try
            {
                StringBuilder updateParams = new StringBuilder();
                StringBuilder queryCondition = new StringBuilder();
                for (int i = 0; i < t.GetProperties().Count(); i++)
                {
                    bool flag = false;
                    foreach (var temp in objectParams)
                    {
                        if (temp.Key.Equals(t.GetProperties()[i].Name))
                            flag = true;
                    }
                    if (flag)
                    {
                        if (i == t.GetProperties().Count() - 1)
                            updateParams.Remove(updateParams.Length - 1, 1);
                        continue;
                    }

                    object data = t.GetProperties()[i].GetValue(instance, null);
                    if (data is DateTime)
                    {
                        updateParams.Append(t.GetProperties()[i].Name + "= " + string.Format("to_date({0},'yyyyMMdd')", ((DateTime)data).ToString("yyyyMMdd")));
                    }
                    else
                    {
                        string value = (data == null ? string.Empty : data.ToString());
                        updateParams.Append(t.GetProperties()[i].Name + "= '" + value + "'");
                    }
                    if (i != t.GetProperties().Count() - 1)
                    {
                        updateParams.Append(",");
                    }
                }

                if (objectParams.Length > 0)
                {
                    queryCondition.Append("where");
                }
                for (int i = 0; i < objectParams.Length; i++)
                {

                    queryCondition.Append(" ");
                    queryCondition.Append(objectParams[i].Key + "='" + objectParams[i].Value + "'");
                    if (i != objectParams.Length - 1)
                        queryCondition.Append(" and ");
                }

                string updateSql = string.Format("update {0} set {1}  {2}", tableName, updateParams.ToString(), queryCondition.ToString());
                WriteLog.Log_Info(updateSql);
                int code = 0;
                return Util.DataBase.SqlCommand(out code, updateSql);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// 更新数据库表
        /// </summary>
        /// <typeparam name="T">泛型（数据库实体类）</typeparam>
        /// <param name="instance">数据库实体类对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="objectParams">条件 （只是支持 a='1' 等号操作不支持>,！= 这些操作符）</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTableAll<T>(T instance, string tableName, params KeyValuePair<string, string>[] objectParams) where T : class
        {
            Type t = typeof(T);
            try
            {
                StringBuilder updateParams = new StringBuilder();
                StringBuilder queryCondition = new StringBuilder();
                for (int i = 0; i < t.GetProperties().Count(); i++)
                {

                    object data = t.GetProperties()[i].GetValue(instance, null);
                    if (data is DateTime)
                    {
                        updateParams.Append(t.GetProperties()[i].Name + "= " + string.Format("to_date({0},'yyyyMMdd')", ((DateTime)data).ToString("yyyyMMdd")));
                    }
                    else
                    {
                        string value = (data == null ? string.Empty : data.ToString());
                        updateParams.Append(t.GetProperties()[i].Name + "= '" + value + "'");
                    }
                    if (i != t.GetProperties().Count() - 1)
                    {
                        updateParams.Append(",");
                    }
                }

                if (objectParams.Length > 0)
                {
                    queryCondition.Append("where");
                }
                for (int i = 0; i < objectParams.Length; i++)
                {

                    queryCondition.Append(" ");
                    queryCondition.Append(objectParams[i].Key + "='" + objectParams[i].Value + "'");
                    if (i != objectParams.Length - 1)
                        queryCondition.Append(" and ");
                }

                string updateSql = string.Format("update {0} set {1}  {2}", tableName, updateParams.ToString(), queryCondition.ToString());
                WriteLog.Log_Info(updateSql);
                int code = 0;
                return Util.DataBase.SqlCommand(out code, updateSql);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 通过ActionparamsList传递的参数，来创建实体类
        /// </summary>
        /// <typeparam name="T">泛型来设置实体类类型</typeparam>
        /// <param name="actionParamsList">actionParams集合</param>
        /// <returns>返回实体类的对象</returns>
        public  T CreateModelData<T>(List<QueryCondition> actionParamsList) where T : class, new()
        {
            if (actionParamsList == null ||
                actionParamsList.Count == 0)
            {
                WriteLog.Log_Error("action params error!");
                return default(T);
            }
            T instance = new T();
            for (int i = 0; i < actionParamsList.Count; i++)
            {
                try
                {
                    PropertyInfo pi=instance.GetType().GetProperty(actionParamsList[i].bindingData);
                    if(pi==null)
                        throw new Exception("propery name ["+actionParamsList[i].bindingData+"] not exist");
                    object value=Util.ParsePropertyValue(pi,actionParamsList[i].value.ToString());
                    pi.SetValue(instance, value, null);
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                }
            }
            return instance;

        }




        /// <summary>
        /// 通过ActionparamsList传递的参数，来创建实体类
        /// </summary>
        /// <typeparam name="T">泛型来设置实体类类型</typeparam>
        /// <param name="actionParamsList">actionParams集合</param>
        /// <returns>返回实体类的对象</returns>
        public object CreateModelData(string typeClass,List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null ||
                actionParamsList.Count == 0)
            {
                WriteLog.Log_Error("action params error!");
                return null;
            }
            object instance = Activator.CreateInstance(Type.GetType(typeClass)) as object;
            for (int i = 0; i < actionParamsList.Count; i++)
            {
                try
                {
                    string[] valueArray = actionParamsList[i].bindingData.Split('.');
                   string valueData=valueArray.Length>1?valueArray[1]:valueArray[0];

                    PropertyInfo pi = instance.GetType().GetProperty(valueData);
                    if (pi == null)
                        throw new Exception("propery name [" + actionParamsList[i].bindingData + "] not exist");
                    object value = Util.ParsePropertyValue(pi, actionParamsList[i].value.ToString());
                    pi.SetValue(instance, value, null);
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                }
            }
            return instance;

        }
        #endregion
        #region [       给T对象赋值。       ]
        /// <summary>
        /// 给对象T内容赋值。
        /// </summary>
        /// <typeparam name="T">T类对象</typeparam>
        /// <param name="item">要赋值的对象</param>
        /// <param name="columns">列的集合</param>
        /// <param name="dr">行内容</param>
        /// <returns>返回T内容</returns>
        public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new()
        {
            if (item == null)
            {
                return default(T);
            }
            try
            {
                Type t = item.GetType();
                MemberInfo[] miList = t.GetMembers();
                foreach (var mi in miList)
                {
                    bool isExist = false;
                    foreach (DataColumn dc in columns)
                    {
                        if (dc.ColumnName.ToLower() == mi.Name.ToLower())
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (mi is PropertyInfo)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        if (isExist)
                        {
                            try
                            {
                                pi.SetValue(item, JudgeValue(pi.PropertyType.Name.ToString(), dr, mi.Name), null);
                            }
                            catch (Exception ee)
                            {

                                WriteLog.Log_Error("public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new() 方法,给T赋值时出错。");
                            }
                        }
                    }
                    if (mi is FieldInfo)
                    {
                        FieldInfo fi = mi as FieldInfo;
                        if (isExist)
                        {
                            try
                            {
                                fi.SetValue(item, dr[mi.Name]);
                            }
                            catch (Exception ee)
                            {
                                WriteLog.Log_Error("public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new() 方法,给T赋值时出错。");
                          
                            }
                        }
                    }
                }
                return item;
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error("public T SetModelValue<T>(T item, DataColumnCollection columns, DataRow dr) where T : new() 方法出错。");
                return default(T);
            }
        }

        /// <summary>
        /// 获取DataTable里的内容。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="dt">datatable</param>
        /// <returns>返回一个T集合对象</returns>
        public List<T> SetTModelValue<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            try
            {
                List<T> item = new List<T>();
                foreach (DataRow dr in dt.Rows)
                {
                    T a = new T();
                    try
                    {
                        SetModelValue<T>(a, dt.Columns, dr);
                        item.Add(a);
                    }
                    catch (Exception e)
                    {
                        WriteLog.Log_Error("public List<T> SetTModelValue<T>(DataTable dt) where T:new () 循环赋值出错了");
                       
                    }

                }
                return item;
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error("public List<T> SetTModelValue<T>(DataTable dt) where T:new () 出错");
              
            }
            return null;
        }

        /// <summary>
        /// 给T对象赋值。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="sqlQuery">查询语句</param>
        /// <returns>返回一个T集合对象</returns>
        public List<T> SetTModelValue<T>(string sqlQuery) where T : new()
        {
            List<T> item = new List<T>();
            try
            {
                DataTable dt = GetDatatable(sqlQuery);
                item = SetTModelValue<T>(dt);
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error("public List<T> SetTModeValue<T>(string sqlQuery) where T : new() 方法出错。");
            }
            return item;
        }
        /// <summary>
        /// 给T对象赋值。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="sqlQuery">查询语句</param>
        /// <returns>返回T内容</returns>
        public T SetModelValue<T>(string sqlQuery) where T : new()
        {
            return SetTModelValue<T>(sqlQuery).GetTContext<T>();
        }


        public DataTable GetDatatable(string sqlQuery)
        {
            DataTable dt = null;
            try
            {
                int retCode = 0;
                DataSet ds = Util.DataBase.SqlQuery(out retCode, sqlQuery);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (System.StackOverflowException soe)
            {
                WriteLog.Log_Error("出错的SQL语句[" + sqlQuery + "]。");
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error("出错的SQL语句[" + sqlQuery + "]。");
            }
            return dt;
        }

        #endregion

    }

    public static class ExternClass
    {
        #region --> 对T进行操作
        /// <summary>
        /// 获取T对象里的内容。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="ie">IEnumerable接口</param>
        /// <param name="item">T集合对象</param>
        public static List<T> GetListTContext<T>(this IEnumerable ie) where T : new()
        {
            List<T> item = new List<T>();
            IEnumerator temp = ie.GetEnumerator();
            while (temp.MoveNext())
            {
                item.Add((T)temp.Current);
            }
            return item;
        }

        /// <summary>
        /// 获取T对象里的内容。此方法返回的对象不为空。
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="ie">IEnumerable接口</param>
        /// <returns>返回T对象</returns>
        public static T GetTContext<T>(this IEnumerable ie) where T : new()
        {
            if (ie == null)
            {
                return (new T());
            }
            IEnumerator temp = ie.GetEnumerator();
            while (temp.MoveNext())
            {
                return (T)temp.Current;
            }
            return (new T());
        }

        /// <summary>
        /// 获取T对象里的内容。些方法返回的对象可能为空
        /// </summary>
        /// <typeparam name="T">类对象</typeparam>
        /// <param name="ie">IEnumerable接口</param>
        /// <returns>返回T对象</returns>
        public static T ReturnT<T>(this IEnumerable ie) where T : new()
        {
            IEnumerator temp = ie.GetEnumerator();
            while (temp.MoveNext())
            {
                return (T)temp.Current;
            }
            return default(T);
        }

        #endregion --> 对T进行操作


        #region 字符串转换成十六进制数据

        /// <summary>
        /// 将16进制的字符串转换成数字
        /// </summary>
        /// <param name="value">16进制显示的字符串</param>
        /// <returns>转换之后的数值</returns>
        public static uint ConvertHexStringToUint(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            uint number=0;
            if (uint.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out number))
                return number;
            return 0;
        }

        /// <summary>
        /// 将10进制字符串转换成uint
        /// </summary>
        /// <param name="value">10进制的字符串</param>
        /// <returns>返回转换后的数值</returns>
        public static uint ConvertNumberStringToUint(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            uint number = 0;
            if (uint.TryParse(value, out number))
                return number;
            return 0;
        }


        public static uint ConvertDateTimeToUnit(this string value)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0);
            var current = DateTime.ParseExact(value, "yyyy-MM-dd", null);
            var totalSeconds = current.Subtract(dt).TotalSeconds-8*60*60;
            return (uint)totalSeconds;
        }

        /// <summary>
        /// 将Uint转换成16进制字符串
        /// </summary>
        /// <param name="value">uint整数</param>
        /// <returns></returns>
        public static string ConvertNumberToHexString(this uint value)
        {
            return value.ToString("x2");
        }

        /// <summary>
        /// 将16进制的字符串转换成数字
        /// </summary>
        /// <param name="value">16进制显示的字符串</param>
        /// <returns>转换之后的数值</returns>
        public static ushort ConvertHexStringToUshort(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            ushort number = 0;
            if (ushort.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out number))
                return number;
            return 0;
        }

        /// <summary>
        /// 将10进制字符串转换成uint
        /// </summary>
        /// <param name="value">10进制的字符串</param>
        /// <returns>返回转换后的数值</returns>
        public static ushort ConvertNumberStringToUshort(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            ushort number = 0;
            if (ushort.TryParse(value, out number))
                return number;
            return 0;
        }

        /// <summary>
        /// 将Uint转换成16进制字符串
        /// </summary>
        /// <param name="value">uint整数</param>
        /// <returns></returns>
        public static string ConvertNumberToHexString(this ushort value)
        {
            return value.ToString("x2");
        }

        /// <summary>
        /// 判断是否为空或""值。返回true为空，否则不为空。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回true为空，否则不为空。</returns>
        public static bool JudgeIsNullOrEmpty(this string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

         /// <summary>
        /// 由字符串转成短整型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回短整型</returns>
        public static ushort ToUShort(this string s)
        {
            try
            {
                if (String.IsNullOrEmpty(s))
                {
                    return 0;
                }
                else
                {

                    ushort u = ushort.Parse(s);
                    return u;
                }
            }
            catch (Exception ee)
            {
                
                return 0;
            }
        }
        /// <summary>
        /// 由decimal类型转成十六进制的ushort类型。
        /// </summary>
        /// <param name="s">decimal类型</param>
        /// <returns>十六进制的ushort类型</returns>
        public static ushort ToHexNumberUShort(this decimal @s)
        {
            try
            {
                ushort u = ushort.Parse(@s.ToString(),NumberStyles.HexNumber);
                return u;
            }
            catch (Exception ee)
            {
                
                return 0;
            }
        }

        /// <summary>
        /// 由字符按十六制转成短整型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回短整型</returns>
        public static ushort StringConvertUShort(this string s)
        {
            try
            {
                return ushort.Parse(s, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception ee)
            {
              
                return 0;
            }
        }
        /// <summary>
        /// 由字符按十六制转成短整型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回短整型</returns>
        public static ushort ToHexNumberUShort(this string @s)
        {
            return StringConvertUShort(@s);
        }

        /// <summary>
        /// 由字符按十六制转成整型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回整型</returns>
        public static int ToHexNumberInt32(this string @s)
        {
            try
            {
                if (String.IsNullOrEmpty(@s))
                {
                    return 0;
                }
                else
                {
                    return int.Parse(@s, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ee)
            {
               
                return 0;
            }
        }

        /// <summary>
        /// 将字符串转成32位整形
        /// </summary>a
        /// <param name="s">字符串</param>
        /// <returns>返回将字符串转成32位整形</returns>
        public static int ToInt32(this string s)
        {
            try
            {
                if (!String.IsNullOrEmpty(s))
                {
                    return Convert.ToInt32(s.Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ee)
            {
              
                return 0;
            }
        }


        /// <summary>
        /// 将字符串转成64位整形
        /// </summary>a
        /// <param name="s">字符串</param>
        /// <returns>返回将字符串转成64位整形</returns>
        public static Int64 ToInt64(this string s)
        {
            try
            {
                if (!String.IsNullOrEmpty(s))
                {
                    return Convert.ToInt64(s.Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ee)
            {
                
                return 0;
            }
        }

        public static uint ToUInt32(this string @s)
        {
            try
            {
                if (!String.IsNullOrEmpty(s))
                {
                    return Convert.ToUInt32(s.Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ee)
            {
               
                return 0;
            }
        }

        /// <summary>
        /// 由整型转成短整型
        /// </summary>
        /// <param name="number">整型</param>
        /// <returns>返回短整型</returns>
        public static ushort ToUShort(this int number)
        {
            return number.ToString().ToUShort();
        }

        /// <summary>
        /// 由字符串转成byte类型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回byte类型</returns>
        public static byte ToByte(this string s)
        {
            try
            {
                if (String.IsNullOrEmpty(s))
                {
                    return 0;
                }
                else
                {
                    byte b = Convert.ToByte(s);
                    return b;
                }
            }
            catch (Exception ee)
            {
               
                return 0;
            }
        }
        /// <summary>
        /// 将字符串按十六制转成byte类型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>返回byte类型</returns>
        public static byte ToHexNumberByte(this string @s)
        {
            try
            {
                if (String.IsNullOrEmpty(@s))
                {
                    return 0;
                }
                else
                {
                    byte b = byte.Parse(@s, System.Globalization.NumberStyles.HexNumber);
                    return b;
                }
            }
            catch (Exception ee)
            {
               
                return 0;
            }
        }

        /// <summary>
        /// 转成十六进制。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns> 转成十六进制。</returns>
        public static byte ToHexNumber(this string s)
        {
            try
            {
                if (String.IsNullOrEmpty(s))
                {
                    return 0;
                }
                else
                {
                    return byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ee)
            {
              
                return 0;
            }
        }

        public static byte ToNumber(this string @s)
        {
            try
            {
                if (String.IsNullOrEmpty(s)||s=="-")
                {
                    return 0;
                }
                else
                {
                    return byte.Parse(s, System.Globalization.NumberStyles.Number);
                }
            }
            catch (Exception ee)
            {
              
                return 0;
            }
        }

        public static Decimal ToDecimalNumber(this string s)
        {
            try
            {
                if (String.IsNullOrEmpty(s) || s == "-")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(s);
                }
            }
            catch (Exception ee)
            {

                return 0;
            }

        }


        /// <summary>
        /// 将byte类型转为十六进制的byte
        /// </summary>
        /// <param name="b">byte值</param>
        /// <returns>转成十六进制</returns>
        public static byte ToHexNumber(this byte b)
        {
            return ToHexNumber(b.ToString());
        }


        public static IEnumerable<DependencyObject> GetVisuals(this DependencyObject root)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                yield return child;
                foreach (var descendants in child.GetVisuals())
                {
                    yield return descendants;
                }
            }
        }


        /// <summary>
        /// 将元转化为分
        /// </summary>
        /// <param name="value">元</param>
        /// <returns>转换之后为分</returns>
        public static string ConvertYuanToFen(this string valueYuan)
        {
            if (string.IsNullOrEmpty(valueYuan))
                return "0";
            double valueYuan1 = Convert.ToDouble(valueYuan)*100;
            return valueYuan1.ToString();
        }

        /// <summary>
        /// 将分转化为元
        /// </summary>
        /// <param name="value">分</param>
        /// <returns>转换之后为元</returns>
        public static string ConvertFenToYuan(this string valueFen)
        {
            if (string.IsNullOrEmpty(valueFen))
                return "0";
            double valueFen1 = Convert.ToDouble(valueFen) / 100;
            return valueFen1.ToString();
        }
    }


 

        #endregion
 }
    
