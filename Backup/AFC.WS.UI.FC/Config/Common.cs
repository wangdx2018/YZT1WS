using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using AFC.WS.UI.Common;
using System.Configuration;

namespace AFC.WS.UI.Config
{
    #region --> delegate
    /// <summary>
    /// 设置SQL语句委托。
    /// </summary>
    /// <param name="sender">对象</param>
    /// <param name="e">事件内容</param>
    public delegate void DelegateSqlSetMethod(object sender, SqlSentenceEventArgs e);

    /// <summary>
    /// 判断数据源名称是否存在。
    /// </summary>
    /// <param name="datasourceName">数据源名称</param>
    public delegate void DelegateJudgeDataSourceIsExist(string datasourceName);
    /// <summary>
    /// 赋初始值委托。
    /// </summary>
    /// <param name="content">内容</param>
    public delegate void DelegateInitValue(string content);
    /// <summary>
    /// 关闭操作事件委托。
    /// </summary>
    /// <param name="value">是否关闭</param>
    public delegate void DelegatePreviewFormClose(bool value);
    /// <summary>
    /// 用于刷新的事件委托。
    /// </summary>
    /// <param name="sender">发送对象</param>
    /// <param name="e">用于刷新propertyGrid的事件类对象。</param>
    public delegate void DelegateRefurbishHandler(object sender,RefurbishPropertyGridEventArgs e);

    #endregion --> delegate

    #region --> Enum

    /// <summary>
    /// 控件显示方式
    /// </summary>
    public enum LayoutModes
    {

        /// <summary>
        /// 
        /// </summary>
        Flow = 0,
        /// <summary>
        /// 
        /// </summary>
        Stack = 1
    }

    /// <summary>
    /// 图表Chart皮肤
    /// </summary>
    public enum ThemeEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Theme1 = 0,
        /// <summary>
        /// 
        /// </summary>
        Theme2 = 1,
        /// <summary>
        /// 
        /// </summary>
        Theme3 = 2
    }

    /// <summary>
    /// 数据选择列
    /// </summary>
    public enum SelectionModes
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 单选
        /// </summary>
        Single = 1,
        /// <summary>
        /// 多选
        /// </summary>
        MultiChoice = 2
    }

    /// <summary>
    /// 对齐方式
    /// </summary>
    public enum ActionAligns
    {
        /// <summary>
        /// 左
        /// </summary>
        Left = 0,
        /// <summary>
        /// 右
        /// </summary>
        Right = 1,
        /// <summary>
        /// 中间
        /// </summary>
        Middle = 2
    }

    /// <summary>
    /// 位置
    /// </summary>
    public enum ActionLocations
    {
        /// <summary>
        /// 上
        /// </summary>
        Top = 0,
        /// <summary>
        /// 下
        /// </summary>
        Bottom = 1,
        /// <summary>
        /// 左
        /// </summary>
        Left = 2,
        /// <summary>
        /// 右
        /// </summary>
        Right = 3
    }
    
    /// <summary>
    /// 图表的位置
    /// </summary>
    public enum ChartLocations
    {
        /// <summary>
        /// 上
        /// </summary>
        Top = 0,
        /// <summary>
        /// 下
        /// </summary>
        Bottom = 1
    }
    
    /// <summary>
    /// 事件标志。
    /// </summary>
    public enum EventFlag
    {
        /// <summary>
        /// 把TextBox中的Sql语句设置到DataSource上去。
        /// </summary>
        SetToDataSourceSQL = 0,
        /// <summary>
        /// 把DataSource中的Sql语句设置到TextBox上去。
        /// </summary>
        SetToTextBox = 1
    }

    /// <summary>
    /// 日誌類型。
    /// </summary>
    public enum LogFlag
    {
        /// <summary>
        /// 记录error级别的日志
        /// </summary>
        Error = 0,
        /// <summary>
        /// 记录带格式的error级别的日志
        /// </summary>
        ErrorFormat = 1,
        /// <summary>
        /// 记录info级别的日志
        /// </summary>
        Info = 2,
        /// <summary>
        /// 记录带格式的info级别的日志
        /// </summary>
        InfoFormat = 3,
        /// <summary>
        /// 记录debug级别的日志
        /// </summary>
        Debug = 4,
        /// <summary>
        /// 记录带格式的debug级别的日志
        /// </summary>
        DebugFormat = 5,
        /// <summary>
        /// 记录warn级别的日志
        /// </summary>
        Warn = 6,
        /// <summary>
        /// 记录带格式的warn级别的日志
        /// </summary>
        WarnFormat  = 7,
        /// <summary>
        /// 记录fatal级别的日志
        /// </summary>
        Fatal = 8,
        /// <summary>
        /// 记录带格式的fatal级别的日志
        /// </summary>
        FatalFormat = 9
    }

    /// <summary>
    /// 字段类型
    /// </summary>
    public enum DataFileType
    {
        /// <summary>
        /// 二进制类型
        /// </summary>
        Binary,
        /// <summary>
        /// 整形。
        /// </summary>
        Integer,
        /// <summary>
        /// Bool类型
        /// </summary>
        Boolean,
        /// <summary>
        /// 字符串类型
        /// </summary>
        String,
        /// <summary>
        /// 枚举类型
        /// </summary>
        Enum,
        /// <summary>
        /// 
        /// </summary>
        Byte,
        /// <summary>
        /// 
        /// </summary>
        Char,
        /// <summary>
        /// 
        /// </summary>
        DateTime,
        /// <summary>
        /// 
        /// </summary>
        Decimal,
        /// <summary>
        /// 
        /// </summary>
        Double,
        /// <summary>
        /// 
        /// </summary>
        Int16,
        /// <summary>
        /// 
        /// </summary>
        Int32,
        /// <summary>
        /// 
        /// </summary>
        Int64,
        /// <summary>
        /// 
        /// </summary>
        SByte,
        /// <summary>
        /// 
        /// </summary>
        Single,
        /// <summary>
        /// 
        /// </summary>
        UInt16,
        /// <summary>
        /// 
        /// </summary>
        UInt32,
        /// <summary>
        /// 
        /// </summary>
        UInt64,
    }

    #endregion --> Enum

    /// <summary>
    /// 公共方法类。
    /// 
    /// 里面包括定义的事件、事件方法、获取规则文件类的属性、初始化日志对象
    /// 
    /// </summary>
    public class Utility
    {
        #region --> Event
        /// <summary>
        /// 设置SQL语句事件。
        /// </summary>
        public event DelegateSqlSetMethod SqlSetMethodEvent;

        /// <summary>
        /// 判断数据源名称是否存在事件。
        /// </summary>
        public event DelegateJudgeDataSourceIsExist JudgeDataSourceNameIsExistEvent;

        /// <summary>
        /// 初始值事件。
        /// </summary>
        public event DelegateInitValue InitValueEvent;
        
        /// <summary>
        /// 关闭窗体事件。 
        /// </summary>
        public event DelegatePreviewFormClose PreviewFormCloseEvent;
        /// <summary>
        /// 刷新PropertyGrid的事件。
        /// </summary>
        public event DelegateRefurbishHandler RefurbishPropertyGridEvent;
        #endregion --> Event

        #region --> 属性 
        
        /// <summary>
        /// 实例化Utility对象
        /// </summary>
        private static Utility _Instance;
        /// <summary>
        /// 实例化Utility对象
        /// </summary>
        public static Utility Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Utility();
                }
                return _Instance;
            }
        }

        #region --> Dll用的。

        /// <summary>
        /// 存储ConvertTo DLL里的类。
        /// </summary>
        private List<DllClassProperty> _ClassPropertyConvertorList = new List<DllClassProperty>();
        /// <summary>
        /// 存储ConvertTo DLL里的类。
        /// </summary>
        public List<DllClassProperty> ClassPropertyConvertorList
        {
            get { return _ClassPropertyConvertorList; }
            set { _ClassPropertyConvertorList = value; }
        }

        /// <summary>
        /// 存储UserControl DLL里的类。
        /// </summary>
        private List<DllClassProperty> _ClassPropertyControlsList = new List<DllClassProperty>();
        /// <summary>
        /// UserControl DLL里的类。
        /// </summary>
        public List<DllClassProperty> ClassPropertyControlsList
        {
            get { return _ClassPropertyControlsList; }
            set { _ClassPropertyControlsList = value; }
        }

        /// <summary>
        /// 存储DataSource DLL里的类。
        /// </summary>
        private List<DllClassProperty> _ClassPropertyDataSourceList = new List<DllClassProperty>();
        /// <summary>
        /// DataSource DLL里的类。
        /// </summary>
        public List<DllClassProperty> ClassPropertyDataSourceList
        {
            get { return _ClassPropertyDataSourceList; }
            set { _ClassPropertyDataSourceList = value; }
        }

        /// <summary>
        /// 存放Action Dll类中属性
        /// </summary>
        private List<DllClassProperty> _ClassPropertyActionList = new List<DllClassProperty>();
        /// <summary>
        /// 存放Action类中属性
        /// </summary>
        public List<DllClassProperty> ClassPropertyActionList
        {
            get { return _ClassPropertyActionList; }
            set { _ClassPropertyActionList = value; }
        }


        #endregion -->Dll用的。

        #endregion --> 属性
        
        #region --> 获取规则文件类

        /// <summary>
        /// 获取Chart规则文件类
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>ChartRule对象</returns>
        public ChartRule GetChartRuleObject(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
            ChartRule cr = new ChartRule();
            
            XmlSerializer xs = new XmlSerializer(cr.GetType());
            try
            {
                Stream s = File.Open(path, FileMode.Open);

                using (s)
                {
                    try
                    {
                        cr = xs.Deserialize(s) as ChartRule;
                        s.Close();
                        s.Dispose();
                    }
                    catch (Exception ee)
                    {
                        s.Close();
                        s.Dispose();
                        //WriteLog.Log_Error(ee);
                        Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                    }
                }
                if (cr != null)
                {
                    return cr;
                }
            }
            catch (Exception streEx)
            {
                //WriteLog.Log_Error(streEx);
                Utility.Instance.ConsoleWriteLine(streEx, LogFlag.Error);
            }
            return null;
        }
        /// <summary>
        /// 获取DataList规则文件类
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>DataListRule对象</returns>
        public DataListRule GetDataListObject(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
            DataListRule cr = new DataListRule();

            XmlSerializer xs = new XmlSerializer(cr.GetType());
            try
            {
                if (File.Exists(path) == true)
                {
                    Stream s = File.Open(path, FileMode.Open);

                    using (s)
                    {
                        try
                        {
                            cr = xs.Deserialize(s) as DataListRule;
                            s.Close();
                            s.Dispose();
                        }
                        catch (Exception ee)
                        {
                            s.Close();
                            s.Dispose();
                            //WriteLog.Log_Error(ee);
                            Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                        }
                    }
                    if (cr != null)
                    {
                        return cr;
                    }
                }
                else
                {
                    Utility.Instance.ConsoleWriteLine("文件路径不存在：文件路径["+path+"]", LogFlag.Error);
                }
            }
            catch (Exception streEx)
            {
                //WriteLog.Log_Error(streEx);
                Utility.Instance.ConsoleWriteLine(streEx, LogFlag.Error);
            }

            return null;
        }
        /// <summary>
        /// 获取数据源规则文件类
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>DataSourceRule对象</returns>
        public DataSourceRule GetDataSourceObject(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
            try
            {
                DataSourceRule cr = new DataSourceRule();
                XmlSerializer xs = new XmlSerializer(cr.GetType());

                Stream s = File.Open(path, FileMode.Open);

                using (s)
                {
                    try
                    {
                        cr = xs.Deserialize(s) as DataSourceRule;
                        s.Close();
                        s.Dispose();
                    }
                    catch (Exception ee)
                    {
                        s.Close();
                        s.Dispose();
                        //WriteLog.Log_Error(ee);
                        Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                    }
                }
                if (cr != null)
                {
                    return cr;
                }
            }
            catch (Exception e)
            {
                //WriteLog.Log_Error(e);
                Utility.Instance.ConsoleWriteLine(e, LogFlag.Error);
            }
            return null;
        }
        /// <summary>
        /// 获取交互界面规则文件类。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>InteractiveControlRule对象</returns>
        public InteractiveControlRule GetInteractiveControlObject(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
            InteractiveControlRule cr = new InteractiveControlRule();

            XmlSerializer xs = new XmlSerializer(cr.GetType());
            try
            {
                Stream s = File.Open(path, FileMode.Open);

                using (s)
                {
                    try
                    {
                        cr = xs.Deserialize(s) as InteractiveControlRule;
                        s.Close();
                        s.Dispose();
                    }
                    catch (Exception ee)
                    {
                        s.Close();
                        s.Dispose();
                        //WriteLog.Log_Error(ee);
                        Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                    }
                }
                if (cr != null)
                {
                    return cr;
                }
            }
            catch (Exception streEx)
            {
                //WriteLog.Log_Error(streEx);
                Utility.Instance.ConsoleWriteLine(streEx, LogFlag.Error);
            }
            return null;
        }

        #endregion --> 获取规则文件类

        #region --> Event Method

        /// <summary>
        /// 判断数据源名称是否存在。
        /// </summary>
        /// <param name="datasourceName">数据源名称</param>
        public void JudgeDataSourceIsExist(string datasourceName)
        {
            if (JudgeDataSourceNameIsExistEvent != null)
            {
                JudgeDataSourceNameIsExistEvent(datasourceName);
            }
        }

        /// <summary>
        /// 给内容赋初始值。
        /// </summary>
        /// <param name="content">内容</param>
        public void InitValueContent(string content)
        {
            if (InitValueEvent != null)
            {
                InitValueEvent(content);
            }
        }
        /// <summary>
        /// 预览窗体关闭事件。 
        /// </summary>
        /// <param name="value">是否关闭窗体用,true关闭，否则不关闭</param>
        public void PreviewFormClose(bool value)
        {
            if (PreviewFormCloseEvent != null)
            {
                PreviewFormCloseEvent(value);
            }
        }

        /// <summary>
        /// SQL语句的设置操作。
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e">SQL语句的设置事件类对象。</param>
        public void SqlSetMethod(object sender,SqlSentenceEventArgs e)
        {
            if (SqlSetMethodEvent != null)
            {
                SqlSetMethodEvent(sender, e);
            }
        }

        /// <summary>
        /// 刷新PropertyGrid方法
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e">用于刷新propertyGrid的事件类对象</param>
        public void RefurbishPropertyGrid(object sender, RefurbishPropertyGridEventArgs e)
        {
            if (RefurbishPropertyGridEvent != null)
            {
                RefurbishPropertyGridEvent(sender, e);
            }
        }

        #endregion --> Event Method

        #region --> Other Method

        /// <summary>
        /// 是否修改规则文件。默认为 false
        /// </summary>
        private bool _IsModify = false;
        /// <summary>
        /// 是否修改规则文件。默认为 false
        /// </summary>
        public bool IsModify
        {
            get { return _IsModify; }
            set { _IsModify = value; }
        }

        // ---> 初始化日志对象，用于记录日志系统本身的日志 
        /// <summary>
        /// 初始化日志对象，用于记录日志系统本身的日志 
        /// </summary>
        /// <returns>成功则返回true；失败则返回false</returns>
        /// <remarks>
        ///     该函数必须要系统初始化时首先调用。因为其他模块进行初始化时，均要记录日志。
        /// </remarks>
        public static bool ImportLogDll()
        {
            try
            {
                string logSavePath = @".\SelfLogFile";
                if (!System.IO.Directory.Exists(logSavePath))
                    System.IO.Directory.CreateDirectory(logSavePath);

                string logConfigIniPath = @".\Dll\logCppConfig.ini";

                AFC.WS.UI.Common.WriteLog.InitLogInstance(logConfigIniPath, "WS_RULE");

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否向控制台输入内容。
        /// </summary>
        public bool IsOutPutConsole
        {
            get
            {
                try
                {
                    string str = ConfigurationSettings.AppSettings["IsOutPutConsole"];
                    if (String.IsNullOrEmpty(str))
                    {
                        return false;
                    }
                    else if (str == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleWriteLine(ex, LogFlag.Error);
                    //WriteLog.Log_Error("IsOutPutConsole: " + ex.ToString());
                    return false;
                }

            }
        }

        /// <summary>
        /// 向控制台打印日志。
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="flag">日誌類型</param>
        public void ConsoleWriteLine(object value,LogFlag flag)
        {
            string currentTime = DateTime.Now.ToShortDateString()
                + " " + DateTime.Now.ToLongTimeString()
                + " " + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            if (IsOutPutConsole)
            {
                Console.WriteLine("{0} --> {1}", currentTime, value);
            }

            if (value == null)
            {
                return;
            }
            switch (flag)
            {
                case LogFlag.Error:

                    WriteLog.Log_Error(value as Exception);

                    break;

                case LogFlag.ErrorFormat:

                    WriteLog.Log_ErrorFormat("Error","0",value.ToString());

                    break;

                case LogFlag.Info:

                    WriteLog.Log_Info(value.ToString());

                    break;

                case LogFlag.InfoFormat:

                    WriteLog.Log_InfoFormat("Info", "0", value.ToString());

                    break;

                case LogFlag.Debug:

                    WriteLog.Log_Debug(value.ToString());

                    break;

                case LogFlag.DebugFormat:

                    WriteLog.Log_DebugFormat("Debug", "0", value.ToString());

                    break;

                case LogFlag.Fatal:

                    WriteLog.Log_Fatal(value.ToString());

                    break;

                case LogFlag.FatalFormat:

                    WriteLog.Log_FatalFormat("Fatal", "0", value.ToString());

                    break;

                case LogFlag.Warn:

                    WriteLog.Log_Warn(value.ToString());

                    break;

                case LogFlag.WarnFormat:

                    WriteLog.Log_WarnFormat("Warn", "0", value.ToString());

                    break;

                default :
                    break;
            }
        }

        /// <summary>
        /// 判断输入的属性名称，是否正确：如控件名称，必须是以下划线及字母开头。
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>true:正确；false:错误</returns>
        public bool JudegPropertyNameIsRight(string value)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    char c = Convert.ToChar(value.Substring(0, 1));
                    int asciiValue = Convert.ToInt32(c);
                    if ((asciiValue >= 33 && asciiValue <= 64)
                        || (asciiValue >= 91 && asciiValue <= 94)
                        || (asciiValue == 96)
                        || (asciiValue >= 123 && asciiValue <= 126))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 判断输入的字符串是否是正确的IP地址。
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>true:正确；false:错误</returns>
        public bool IsValidIP(string ip)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                string[] ips = ip.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            else
                return false;
        }

        /// <summary>
        /// 判断输入的字符串是否是正确的时间。
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>true:正确；false:错误</returns>
        public bool IsValidTIME(string time)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(time, "(20|21|22|23|[0-1][0-9])[0-5][0-9][0-5][0-9]"))
            {
                string hour = time.Substring(0,2);
                string min  = time.Substring(2,2);
                string sec  = time.Substring(4,2);
                if (time.Length == 6)
                {
                    if (System.Int32.Parse(hour) < 24 && System.Int32.Parse(min) < 60 & System.Int32.Parse(sec) < 60)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            else
                return false;
        }


        #endregion --> Other Method
    }
}