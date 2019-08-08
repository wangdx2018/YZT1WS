using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using System.Reflection;
using System.Configuration;

namespace AFC.WS.UI.DataSources 
{
    /// <summary>
    /// 数据数据管理器对象
    /// 
    /// edit by wangdx  date:20100608 
    /// 
    /// 增加了CancleRegisiter函数
    /// 
    /// 20110228 添加了数据源存放的目录配置
    /// </summary>
    public class DataSourceManager
    {
        /// <summary>
        /// 数据源存放的目录，
        /// </summary>
        public static string dataSourcePathDir;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectStr;

        /// <summary>
        /// 数据源字典
        /// </summary>
        private static Dictionary<string, KeyValuePair<IDataSource,List<IDataSourceClient>>> dataSourceDict = new  Dictionary<string,KeyValuePair<IDataSource,List<IDataSourceClient>>>();
 
        /// <summary>
        /// 查找数据源
        /// </summary>
        /// <param name="dataSourceName">数据源名称</param>
        /// <returns>返回IDataSource数据源对象</returns>
        public static IDataSource LookupDataSourceByName(string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
                return null;
            if (string.IsNullOrEmpty(dataSourcePathDir))
            {
                try
                {
                   dataSourcePathDir = ConfigurationManager.AppSettings["DBDictionaryName"];
                }
                catch(Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                    return null;
                }
            }
            string configFilePath = dataSourcePathDir + "\\" + dataSourceName + ".xml";
            if (dataSourceDict.ContainsKey(dataSourceName))
                return dataSourceDict[dataSourceName].Key;
            try
            {
                if (!System.IO.File.Exists(configFilePath))
                    throw new System.IO.FileNotFoundException("file name: [" + configFilePath + "]" + "not found");
                DataSourceRule config = AFC.WS.UI.Config.Utility.Instance.GetDataSourceObject(configFilePath);
                IDataSource ds= CreateDataSource(config);
                if (ds != null)
                {
                  int res= RegesiterDataSource(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 注册一个DataSource对象
        /// </summary>
        /// <param name="dataSource">数据源对象</param>
        /// <returns>注册成功返回0，否则返回-1</returns>
        private static int RegesiterDataSource(IDataSource dataSource)
        {
            try
            {
                if (dataSource == null)
                {
                    return -1;
                }

                if (dataSourceDict.ContainsKey(dataSource.DataSourceName))
                    throw new Exception("dataSource has been regesitered");
                dataSourceDict.Add(dataSource.DataSourceName, new KeyValuePair<IDataSource,List<IDataSourceClient>>(dataSource,new List<IDataSourceClient>()));
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return -1;
            }
        }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        /// <param name="config">数据源规则</param>
        /// <returns>数据源对象</returns>
        private static IDataSource CreateDataSource(DataSourceRule config)
        {
            try
            {
                if (config == null)
                    throw new Exception("数据源规则文件对象为空");
                IDataSource dataSource = Activator.CreateInstance(Type.GetType(config.DataSourceTypeName)) as IDataSource;
                if (dataSource != null)
                {
                    for (int i = 0; i < config.PropertyValues.Count; i++)
                    {
                        PropertyInfo pi = dataSource.GetType().GetProperty(config.PropertyValues[i].Key);
                        if (pi != null)
                        {
                            object value = Util.ParsePropertyValue(pi, config.PropertyValues[i].Value);
                            pi.SetValue(dataSource, value, null);
                        }
                    }
                    dataSource.DataSourceName = config.DataSourceName;
                    return dataSource;
                }
                throw new Exception("dataSource create failed name=[" + config.DataSourceName + "] type= [" + config.DataSourceTypeName + "]");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("dataSource create failed name=[" + config.DataSourceName + "] type= [" + config.DataSourceTypeName + "]");
                WriteLog.Log_Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 释放数据源
        /// </summary>
        /// <param name="dataSourceName">数据源名称</param>
        public static void DisponseDataSource(string dataSourceName)
        {
            if (String.IsNullOrEmpty(dataSourceName)) return;
            if (dataSourceDict.ContainsKey(dataSourceName))
            {
                IDataSource ds = dataSourceDict[dataSourceName].Key;
                using (ds)
                {
                    List<IDataSourceClient> clientList = dataSourceDict[dataSourceName].Value;
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        try
                        {
                            clientList[i].HandleDataSourceDispose();
                        }
                        catch (Exception ex)
                        {
                            WriteLog.Log_Error("there is an error in  [" + clientList[i].ToString() + "] HandleDataSourceDispose()");
                            AFC.WS.UI.Config.Utility.Instance.ConsoleWriteLine(ex, LogFlag.DebugFormat);
                        }
                    }
                    dataSourceDict.Remove(dataSourceName);
                }
            }
        }

       /// <summary>
       /// 为数据源注册数据源客户端
       /// </summary>
       /// <param name="dataSource">数据源对象</param>
       /// <param name="client">数据源客户程序</param>
       /// <returns>成功返回0，否则返回false</returns>
        public static int RegesiterDataSourceClient(IDataSource dataSource,IDataSourceClient client)
        {
            try
            {
                if (string.IsNullOrEmpty(dataSource.DataSourceName))
                    throw new Exception("dataSource has not a validity name is null or empty");
                if (dataSourceDict.ContainsKey(dataSource.DataSourceName))
                {
                    if (!dataSourceDict[dataSource.DataSourceName].Value.Contains(client))
                    {
                        dataSourceDict[dataSource.DataSourceName].Value.Add(client);
                    }
                    return 0;
                }
                else
                    throw new Exception("can't find that dataSource please create first !  name = [" + dataSource.DataSourceName + "]");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return -1;
            }
        }

        /// <summary>
        /// 通知数据源变化
        /// </summary>
        /// <param name="dataSourceName">数据源</param>
        public static void NotfiyDataSourceChange(string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
                return;
            if (!dataSourceDict.ContainsKey(dataSourceName))
            {
                WriteLog.Log_Error("can't find dataSource name=[" + dataSourceName + " ] please create and regisiter it first");
                return;
            }
               List<IDataSourceClient> clientList= dataSourceDict[dataSourceName].Value;
               for (int i = 0; i < clientList.Count; i++)
               {
                   try
                   {
                       clientList[i].HandleDataSourceChange();
                   }
                   catch (Exception ex)
                   {
                       WriteLog.Log_Error("there is an error in HandleDataSourceChange() , type=[ " + clientList[i].ToString() + " ].");
                       WriteLog.Log_Error(ex);
                   }
               }
        }


        /// <summary>
        /// 取消该数据源的所有的注册客户端
        /// </summary>
        /// <param name="dataSourceName">数据源名称</param>
        public static void CancleRegesiterDataSource(string dataSourceName)
        {
            if (String.IsNullOrEmpty(dataSourceName)) return;
            if (dataSourceDict.ContainsKey(dataSourceName))
            {
                IDataSource ds = dataSourceDict[dataSourceName].Key;
                using (ds)
                {
                    dataSourceDict[dataSourceName].Value.Clear();
                }
            }
        }

    }
}
