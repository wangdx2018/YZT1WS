using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AFC.WS.UI.Common
{
    internal class WriteLogApi
    {
        /// <summary>
        /// 初始化日志模块
        /// </summary>
        /// <param name="strConfigureFileName">配置文件的路径和文件名</param>
        /// <param name="strInstanceName"></param>
        /// <returns>是否初始化成功</returns>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll", EntryPoint = "InitLogInstance")]
        internal extern static IntPtr InitLogInstance(string strConfigureFileName, string strInstanceName);

        ///// <summary>
        ///// 设置生成的日志文件名
        ///// </summary>
        ///// <param name="lpConfigureFileName">日志配置文件名称</param>
        ///// <param name="lpLogFileDir">生成的日志文件目录</param>
        ///// <param name="lpDeviceCode">本机的设备编码</param>
        ///// <param name="iFileNum">生成不同文件时编号</param>
        ///// <returns></returns>
        //[DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        ////internal static extern bool SetLogFileName(string lpConfigureFileName, string lpLogFileDir, string lpDeviceCode, int iFileNum);

        /// <summary>
        /// 记录debug级别的日志
        /// </summary>
        /// <param name="logHandle"></param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_Debug(IntPtr logHandle, string message);
        /// <summary>
        /// 记录带格式的debug级别的日志
        /// </summary>
        /// <param name="logHandle"></param>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_DebugFormat(IntPtr logHandle, string LogCode, string LogSubCode, string message);
        /// <summary>
        /// 记录info级别的日志
        /// </summary>
        /// <param name="logHandle"></param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_Info(IntPtr logHandle, string message);
        /// <summary>
        /// 记录带格式的info级别的日志
        /// </summary>
        /// <param name="logHandle"></param>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_InfoFormat(IntPtr logHandle, string LogCode, string LogSubCode, string message);
        /// <summary>
        /// 记录warn级别的日志
        /// </summary>
        /// <param name="message">日志文本</param>
        /// <param name="logHandle"></param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_Warn(IntPtr logHandle, string message);
        /// <summary>
        /// 记录带格式的warn级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="logHandle"></param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_WarnFormat(IntPtr logHandle, string LogCode, string LogSubCode, string message);
        /// <summary>
        /// 记录error级别的日志
        /// </summary>
        /// <param name="logHandle"></param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_Error(IntPtr logHandle, string message);
        /// <summary>
        /// 记录带格式的error级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="message">日志文本</param>
        /// <param name="logHandle"></param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_ErrorFormat(IntPtr logHandle, string LogCode, string LogSubCode, string message);
        /// <summary>
        /// 记录fatal级别的日志
        /// </summary>
        /// <param name="logHandle"></param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_Fatal(IntPtr logHandle, string message);
        /// <summary>
        /// 记录带格式的fatal级别的日志
        /// </summary>
        /// <param name="LogCode">日志码，使用前请在日志规则中添加相应规则</param>
        /// <param name="LogSubCode">日志子码，目前为保留字段，默认为0</param>
        /// <param name="logHandle"></param>
        /// <param name="message">日志文本</param>
        [DllImport(@".\Dll\TerminalUnitLogDll.dll")]
        internal static extern void Log_FatalFormat(IntPtr logHandle, string LogCode, string LogSubCode, string message);
    }
}
