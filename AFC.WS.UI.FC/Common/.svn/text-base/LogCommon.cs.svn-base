using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    using AFC.WS.UI.Common;

   public class LogCommon
    {
        // ---> 初始化日志对象，用于记录日志系统本身的日志 
        /// <summary>
        /// 初始化日志对象，用于记录日志系统本身的日志 
        /// <param name="logConfigPath">Log的配置信息路径</param>
        /// </summary>
        /// <returns>成功则返回true；失败则返回false</returns>
        /// <remarks>
        ///     该函数必须要系统初始化时首先调用。因为其他模块进行初始化时，均要记录日志。
        /// </remarks>
        public bool InitLogDll(string logConfigPath)
        {
            try
            {
                string logSavePath = @".\SelfLogFile";
                if (!System.IO.Directory.Exists(logSavePath))
                    System.IO.Directory.CreateDirectory(logSavePath);
                bool res = WriteLog.InitLogInstance(logConfigPath, "TJWS2.0");
                return res;
            }
            catch
            {
                return false;
            }
        }

    }
}
