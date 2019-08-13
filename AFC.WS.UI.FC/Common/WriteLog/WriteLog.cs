using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using AFC.WS.UI.Config;
using AFC.BOM2.Common;
using log4net;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 记录程序日志
    /// </summary>
    public class WriteLog
    {

        private static log4net.ILog log = LogManager.GetLogger("Common Log");

        // ---> 初始化日志模块
        /// <summary>
        /// 初始化日志模块
        /// </summary>
        /// <param name="lpFileName">配置文件的路径和文件名</param>
        /// <param name="strInstanceName"></param>
        /// <returns>是否初始化成功</returns>
        public static bool InitLogInstance(string lpFileName, string strInstanceName)
        {
            try
            {
                log = LogManager.GetLogger("Common Log"); 
                return true;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Config.Utility.Instance.ConsoleWriteLine(ex, LogFlag.DebugFormat);
                return false;
            }
        }


        // ---> 记录debug级别的日志
        /// <summary>
        /// 记录debug级别的日志
        /// </summary>
        /// <param name="message">日志文本</param>
        public static void Log_Debug(string message)
        {
            try
            {
               log.Debug(message);
            }
            catch
            {
            }
        }

        // ---> 记录带格式的debug级别的日志
        /// <summary>
        /// 记录带格式的debug级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        public static void Log_DebugFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                log.DebugFormat(string.Format("{0},{1},{2}", LogCode, LogSubCode, message));
            }
            catch
            {
            }
        }

        // ---> 记录info级别的日志
        /// <summary>
        /// 记录info级别的日志
        /// </summary>
        /// <param name="message">日志文本</param>
        public static void Log_Info(string message)
        {
            try
            {
                log.Info(message);
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Config.Utility.Instance.ConsoleWriteLine(ex, LogFlag.DebugFormat);
            }
        }

        // ---> 记录带格式的info级别的日志
        /// <summary>
        /// 记录带格式的info级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        public static void Log_InfoFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                log.InfoFormat(string.Format("{0},{1},{2}", LogCode, LogSubCode, message));
            }
            catch
            {
            }
        }

        // ---> 记录warn级别的日志
        /// <summary>
        /// 记录warn级别的日志
        /// </summary>
        /// <param name="message">日志文本</param>
        public static void Log_Warn(string message)
        {
            try
            {
                log.Warn(message);
            }
            catch
            { }
        }

        // ---> 记录带格式的warn级别的日志
        /// <summary>
        /// 记录带格式的warn级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        public static void Log_WarnFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                log.WarnFormat(string.Format("{0},{1},{2}", LogCode, LogSubCode, message));
            }
            catch { }
        }

        // ---> 记录error级别的日志
        /// <summary>
        /// 记录error级别的日志
        /// </summary>
        /// <param name="message">日志文本</param>
        public static void Log_Error(string message)
        {
            try
            {
                log.Error(message);
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Config.Utility.Instance.ConsoleWriteLine(ex, LogFlag.DebugFormat);
            }
        }

        // ---> 记录带格式的error级别的日志
        /// <summary>
        /// 记录带格式的error级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        public static void Log_ErrorFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                log.ErrorFormat(string.Format("{0},{1},{2}", LogCode, LogSubCode, message));
            }
            catch { }
        }

        // ---> 记录fatal级别的日志
        /// <summary>
        /// 记录fatal级别的日志
        /// </summary>
        /// <param name="message">日志文本</param>
        public static void Log_Fatal(string message)
        {
            try
            {
               log.Fatal(message);
            }
            catch
            { }
        }

        // ---> 记录带格式的fatal级别的日志
        /// <summary>
        /// 记录带格式的fatal级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        public static void Log_FatalFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                log.FatalFormat(string.Format("{0},{1},{2}", LogCode, LogSubCode, message));
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void Log_Error(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("Message: ").Append(ex.Message).Append(",StackTrace: ").Append(ex.StackTrace).Append(",Source: ").Append(ex.Source).Append(",InnerException: ").Append(ex.InnerException);
            sb.Append("Message: ").Append(ex.Message).Append("\n StackTrace: ").Append(ex.StackTrace).Append("\n Source: ").Append(ex.Source).Append("\n InnerException: ").Append(ex.InnerException);

            log.Error(sb.ToString(), ex);
            //System.Windows.MessageBox.Show(sb.ToString());
        }

    }
}
