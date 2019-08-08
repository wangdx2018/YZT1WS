using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace TestComm
{
    using AFC.BOM2.Common;
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitLogDll(@".\DLL\logCppConfig.ini");
            //base.OnStartup(e);
            System.Threading.Thread.CurrentThread.Name = "MainThread";
        }

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
