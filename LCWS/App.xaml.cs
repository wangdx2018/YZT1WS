using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace LCWS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string skinPath = AFC.WS.UI.Common.SysConfig.GetSysConfig(@".\Config\SysConfig.xml").LocalParamsConfig.SkinPath;//加载系统配置文件

            AFC.BOM2.UIController.UIController.GetControllerInstance().ApplySkinFile(skinPath, this, false);
        }
    }
}
