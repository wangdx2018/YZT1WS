using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AFC.WS.ModelView.UIContext
{
    using AFC.BOM2.UIController;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using System.Globalization;
    using AFC.WS.BR.CommBuiness;
   
    
  

    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110222
    /// 代码功能：处理界面相关的信息，如初始化UI，初始化配置文件，初始化通讯，初始化。
    /// 修订记录：
    /// </summary>
    public class UIOperation
    {
        /// <summary>
        /// 主窗体对象
        /// </summary>
        private Window myWindow = null;

        private static UIOperation operation = null;
        
        public static UIOperation GetInstance()
        {
            if (operation == null)
                operation = new UIOperation();
            return operation;
        }

        /// <summary>
        /// 系统业务类对象
        /// </summary>
        public BuinessRule BR = null;

        #region [public methods]

        /// <summary>
        /// 初始化UI
        /// </summary>
        /// <param name="window">主窗体对象</param>
        public void InitliaizeUIOpeation(Window window)
        {
            System.Threading.Thread.CurrentThread.Name = "MainThread";
            SetOperationWindow(window);
            Init();
        }

        
        /// <summary>
        /// 初始化UI数据
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int Init()
        {
            UIController.GetControllerInstance().SetListenWindow(myWindow); //设置监听窗口
            bool res = UIController.GetControllerInstance().LoadConfigFile(@".\Config\layoutTemplate.xml", @".\Config\BuinessControlCfg.xml");
            if(!res)
             return -1;
            UIMessageHandle handle = new UIMessageHandle();
            handle.SetWindow(myWindow);
            UIController.GetControllerInstance().SetUIControllerMessageHandler(handle);//设置消息处理类
            AFC.WS.UI.Common.SysConfig.GetSysConfig(@".\Config\SysConfig.xml");//加载系统配置文件
            this.SetDefaultDataSourceDir(SysConfig.GetSysConfig().LocalParamsConfig.DataSourceDirectory);//设置数据源的路径
            AFC.WS.UI.DataSources.DataSourceManager.ConnectStr = SysConfig.GetSysConfig().LocalParamsConfig.DBConnectionString;//设置联机字符串
            this.InitSysLog(@".\Config\logCppConfig.ini");//初始化日志模
            this.BR = BuinessRule.GetInstace();
      //      this.BR.logManager.InitLog(@".\Config\logCppConfig.ini", "ErrorCode",@".\SelfLogFile\ErrorCode.txt");
            FunctionDataCollection.GetFunctionDataCollection().LoadFunctionConfig(@".\Config\FunctionCfg.xml");//初始化菜单信息
            this.BR.paraManager.LoadParamsConfigFile(@".\Config\ParamConfig.xml");
            MessageCfg.LoadMessageConfigFile(@".\Config\MessageConfig.xml");
            //todoGet ModeInfo ; todo Get VersionInfo and Set BRContext fieldis
          
            return 0;
        }

     
        /// <summary>
        /// 设置监听窗体
        /// </summary>
        /// <param name="window">Window对象，主窗体对象</param>
        private void SetOperationWindow(Window window)
        {
            if (window != null)
            {
                this.myWindow = window;
            }
        }

        /// <summary>
        /// 发送系统启动消息,进入到系统自检界面
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int SwitchUI(string buinessControlId)
        {
            Message msg = new Message();
            msg.MessageType = SynMessageType.SwichPage;
            msg.Content = buinessControlId;
            MessageManager.SendMessasge(msg);
            return 0;
        }


        #endregion

        #region[ private methods]

        /// <summary>
        /// 加载系统的配置文件
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int LoadSysConfig()
        {
            return 0;
        }

        /// <summary>
        /// 加载Function配置文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>加载成功为0，否则为-1</returns>
        private int LoadFunctionConfig(string fileName)
        {
            return FunctionDataCollection.GetFunctionDataCollection().LoadFunctionConfig(@".\Config\FunctionCfg.xml") ? 0:  -1;
        }

        /// <summary>
        /// 设置皮肤文件，从数据库中获得
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>成功返回0，否则返回-1</returns>
       private int SetSkinFile(string fileName)
        {
            UIController.GetControllerInstance().ApplySkinFile(fileName,
                Application.Current, true);
            return 0;
        }

        /// <summary>
        /// 初始化系统log
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>成功返回0，否则返回-1</returns>
       private int InitSysLog(string fileName)
       {
           AFC.WS.UI.Common.LogCommon log = new AFC.WS.UI.Common.LogCommon();
           return log.InitLogDll(fileName) ? 0: -1;
           
       }

        /// <summary>
        /// 初始化通讯
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
       private int InitComm()
       {
           return 0;
       }

        /// <summary>
        /// 设置数据源存放的目录
        /// </summary>
        /// <param name="dir">数据源路径</param>
       private void SetDefaultDataSourceDir(string dir)
       {
           AFC.WS.UI.DataSources.DataSourceManager.dataSourcePathDir = dir;
       }

        /// <summary>
        /// 启动监听进程
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
       private int StartMonitor()
       {
           return 0;
       }

        /// <summary>
        /// 停止监听进程
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
       private int StopMonitor()
       {
           return 0;
       }

    

        #endregion


    }
}
