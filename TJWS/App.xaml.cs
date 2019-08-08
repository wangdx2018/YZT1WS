using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace TJWS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string skinPath =    AFC.WS.UI.Common.SysConfig.GetSysConfig(@".\Config\SysConfig.xml").LocalParamsConfig.SkinPath;//加载系统配置文件

            AFC.BOM2.UIController.UIController.GetControllerInstance().ApplySkinFile(skinPath, this, false);

            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            //ResourceDictionary rd = Application.LoadComponent(new Uri(skinPath, UriKind.RelativeOrAbsolute)) as ResourceDictionary;
            //if (rd != null)
            //{
            //    Application.Current.Resources.MergedDictionaries.Add(rd);
            //}
            ////base.OnStartup(e);

            log4net.Config.XmlConfigurator.Configure();
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(e.Exception.Message);
            }
            catch (Exception)
            {

            }
        }
        
        
    }
}
