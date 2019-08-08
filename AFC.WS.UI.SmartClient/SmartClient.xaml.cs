#region [       Copyright (C), 2009,  中软AFC     ]
#endregion

#region [       Using namespaces       ]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Collections;

#endregion

namespace AFC.WS.UI.SmartClient
{
    /// <summary>
    /// 从WsCurrent文件夹中取最新文件，移动到当前目录，解压文件，更新文件，
    /// 
    /// 启动主程序。
    /// </summary>
    ///<remarks>
    /// 删除主程序进程，取更新文件更新，启动主程序。
    /// </remarks>
    public partial class SmartClient : Window
    {
        #region [       Declarations       ]
        /// <summary>
        /// 文件名称
        /// </summary>
        private string filename = null;

        /// <summary>
        /// 配置文件名称
        /// </summary>
        private string fileAppName = null;

        /// <summary>
        /// WS应用程序的名称
        /// </summary>
        private string WsApplicationName = null;

        /// <summary>
        /// 当前文件存放路径
        /// </summary>
        private string wsCurrentDirectory = null;

        /// <summary>
        /// 临时历史文件存放路径
        /// </summary>
        private string wsTempHistoryDirectory = null;

        /// <summary>
        /// Config\sysConfig中系统名称.
        /// </summary>
        private string SystemName = null; 

        /// <summary>
        ///更新程序委托
        /// </summary>
        private delegate void updateDelegate();

        #endregion

        #region [       Constructor       ]

        /// <summary>
        /// 构造函数，初始化控件。
        /// </summary>
        public SmartClient()
        {
            InitializeComponent();

            this.ShowInTaskbar = false;

            InitLogDll();
        }

        #endregion

        #region [       Private Methods       ]

        /// <summary>
        /// 窗体加载函数，启动线程，执行更新方法。
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadConfig();
        }

        /// <summary>
        /// 加载线程，初始化函数
        /// </summary>
        public void loadConfig()
        {
            try
            {
                XmlDocument xDoc;
                bool isReadSucess = CreateXmlDocument(out xDoc, @"Config\SysConfig.xml");
                if (!isReadSucess)
                {
                    WriteLog.Log_Error("读SysConfig.xml文件异常，请检查此文件是否存在!");
                }
                SystemName = GetConfigFileValue(xDoc, "SystemName");

                WsApplicationName = SystemName;

                wsCurrentDirectory = GetAppValue("ParamFTPDownLoadPath");

                wsTempHistoryDirectory = GetAppValue("TempFTPDownLoadPath");

                Thread thread = new Thread(new ThreadStart(UpdatePrograming));
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("WindowLoad更新程序事件中异常" + ex.ToString());
                this.Close();
            }
        }

        /// <summary>
        ///  调用删除主程序函数，删除主程序。
        ///  
        ///  调用更新程序函数，更新程序。
        ///  
        ///  调用启动主程序函数，启动主程序。
        /// </summary>
        private void UpdatePrograming()
        {
            try
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new updateDelegate(() =>
                {
                    this.labUpdateInfo.Content = "正在删除主程序进程.......";

                    this.progressBar.Value = 10;

                    Thread.Sleep(1000);

                    DoEvents();
                    
                     bool killIsTrue = KillProcess(WsApplicationName);

                    //调试时用
                    //bool killIsTrue = KillProcess("LCWS.vshost");

                    this.progressBar.Value = 20;

                    if (killIsTrue)
                    {
                        this.labUpdateInfo.Content = "删除主程序进程成功!";

                        Thread.Sleep(1000);

                        DoEvents();

                        WriteLog.Log_Info("删除主程序进程成功！");

                        this.labUpdateInfo.Content = "正在更新主程序.......";

                        Thread.Sleep(1000);

                        DoEvents();

                        bool updateIsTrue = UpdateWsFile();

                        if (updateIsTrue)
                        {
                            this.labUpdateInfo.Content = "正在启动主程序.......";

                            WriteLog.Log_Info("更新程序成功！");

                            Thread.Sleep(1000);

                            DoEvents();

                            this.progressBar.Value = 100;

                            string startName = WsApplicationName + ".exe";
                            //string startName = "LCWS.exe";

                            if (File.Exists(startName))
                            {
                                StartMainProgram(startName);

                                WriteLog.Log_Info("启动主程序成功！");
                            }
                        }
                        else
                        {
                            WriteLog.Log_Info("更新程序失败！");
                            
                            this.progressBar.Value = 100;

                            this.labUpdateInfo.Content = "正在启动主程序.......";

                            Thread.Sleep(1000);

                            DoEvents();

                            string startName = WsApplicationName + ".exe";

                            if (File.Exists(startName))
                            {
                                StartMainProgram(startName);

                                WriteLog.Log_Info("启动主程序成功！");
                            }
                            else
                            {
                                WriteLog.Log_Error("不存在启动程序:" + startName);
                                Environment.Exit(0);
                            }
                        }
                    }
                }));
            }
            catch(Exception ex)
            {
                WriteLog.Log_Error("更新程序中异常:" + ex.ToString());
            }
        }

        /// <summary>
        /// 处理消息队列的内容
        /// </summary>
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate(object f)
                {
                    ((DispatcherFrame)f).Continue = false;

                    return null;
                }
                    ), frame);
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// 删除主程序进程
        /// </summary>
        /// <param name="processName">主程序名称</param>
        private bool KillProcess(string processName)
        {
            try
            {
                Process[] allProcess = Process.GetProcessesByName(processName);
                if (allProcess.Count() != 0)
                {
                    foreach (Process pc in allProcess)
                    {
                        pc.Kill();
                    }
                    WriteLog.Log_Info("删除名称为：" + processName + "的进程。");
                    return true;
                }
                else
                {
                    this.Close();
                    WriteLog.Log_Info("不存在名称为：" + processName + "的进程。");
                }
                return true;
               
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("删除进程出现异常：" + ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// 启动主程序
        /// </summary>
        /// <param name="mainName">主程序名称</param>
        private void StartMainProgram(string mainName)
        {
            try
            {
                if (!File.Exists(mainName))
                {
                    WriteLog.Log_Error("不存在可执行文件：" + mainName);
                    this.Close();
                }
                ProcessStartInfo processStartInfo = new ProcessStartInfo(mainName);

                Process.Start(processStartInfo);

                this.Close();
            }
            catch(Exception ex)
            {
                WriteLog.Log_Error("启动主程序出错：" + ex.ToString());

                this.Close();
            }
        }
       
        /// <summary>
        /// 从WsCurrent目录取程序到当前文件夹,自解压到当前文件夹。
        /// </summary>
        /// <returns>True:成功，False:失败</returns>
        private bool UpdateWsFile()
        {
            try
            {
                XmlDocument xDoc;

                bool isAppTrue = CreateXmlDocument(out xDoc, WsApplicationName + ".exe.config");

                int progressBarValue = 0;

                string wsCurrentPath = Environment.CurrentDirectory + wsCurrentDirectory;

                string wsTempHistoryPath = Environment.CurrentDirectory + wsTempHistoryDirectory;
               

                this.labUpdateInfo.Content = "正在自解压程序.........";

                DoEvents();

                Thread.Sleep(1000);
                if (!Directory.Exists(wsCurrentPath))
                {
                    WriteLog.Log_Error("当前文件夹不存在，不存在此路径:" + wsCurrentDirectory);
                    return false;
                }


                string[] Files = Directory.GetFiles(wsCurrentPath);

                int fileCount = Files.Count() - 1;
                //把文件移到根目录 上一层  current ---- Execute root
                foreach (string file in Files)
                {
                    string filename = System.IO.Path.GetFileName(file);//更新的文件名称,更改为新的文件名称

                    string fileExtension = System.IO.Path.GetExtension(file);//扩展名

                    if (fileExtension == ".exe")
                    {

                        string newFilename = filename.Substring(0, filename.Length - 4) + "_new" + ".exe";
                        this.progressBar.Value = 50;
                        this.labUpdateInfo.Content = "正在拷贝文件到当前目录.........";

                        DoEvents();

                        Thread.Sleep(1000);

                        //将压缩文件 移到 启动目录
                        File.Copy(file, Environment.CurrentDirectory + @"\" + newFilename, true);

                        WriteLog.Log_Info("拷贝信息到当前目录。");

                        //解压 文件
                        if (File.Exists(Environment.CurrentDirectory + @"\" + newFilename))
                        {
                            try
                            {
                                this.progressBar.Value = 60;
                                this.labUpdateInfo.Content = "正在自解压" + newFilename + "程序.........";
                                DoEvents();
                                Thread.Sleep(1000);


                                Process mypro = new Process();
                                mypro.StartInfo.FileName = Environment.CurrentDirectory + @"\" + newFilename;
                                mypro.Start();
                                mypro.WaitForExit();//wait when RAR is ok;

                                WriteLog.Log_Info("解压程序到当前目录。");

                                //   SetAppValue(filename, "True");
                            }
                            catch (Exception ex)
                            {
                                WriteLog.Log_Error(ex.ToString());

                                //SetAppValue(filename, "False");
                            }
                        }
                        this.progressBar.Value = 70;
                        this.labUpdateInfo.Content = "正在删除" + newFilename + "文件.........";

                        Thread.Sleep(1000);

                        DoEvents();

                        //删除更新文件
                        try
                        {
                            if (File.Exists(Environment.CurrentDirectory + @"\" + newFilename))
                            {
                                File.Delete(Environment.CurrentDirectory + @"\" + newFilename);

                                WriteLog.Log_Info("删除" + newFilename + "文件");

                                this.labUpdateInfo.Content = "删除" + newFilename + "文件成功.........";

                                DoEvents();

                                Thread.Sleep(1000);
                            }
                            else
                            {
                                WriteLog.Log_Info("删除文件" + newFilename + "不存在");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog.Log_Error("删除文件名称：" + newFilename + ex.ToString() + "出错。");

                        }
                        //progressBarValue = progressBarValue + (90 / fileCount);

                        //this.progressBar.Value = 5 + progressBarValue;
                    }
                }
                string[] tempFiles = Directory.GetFiles(wsCurrentPath);
                //删除WsCurrent文件夹内.exe文件
                foreach (string wsfile in tempFiles)
                {
                    string fileExtension2 = System.IO.Path.GetExtension(wsfile);//扩展名
                    if (fileExtension2 == ".exe")
                    {
                        File.Delete(wsfile);
                        WriteLog.Log_Info("删除" + wsfile);
                    }
                }
                Thread.Sleep(1000);
                this.progressBar.Value = 80;
                this.labUpdateInfo.Content = "更新完成，正在准备完成启动主程序,请稍后.....";
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("更新文件异常:" + ex.ToString());

                return false;
            }
        }

        /// <summary>
        /// 设置app.config中的某个key的value.
        /// </summary>
        /// <param name="AppKey">key</param>
        /// <param name="AppValue">value</param>
        private void SetAppValue(string AppKey, string AppValue)
        {
            try
            {
                filename = ConfigurationSettings.AppSettings["UpdateResultFile"].ToString();

                fileAppName = ConfigurationSettings.AppSettings["FileName"].ToString();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("读App.config异常:" + ex.ToString());

                filename = "AFC.WS.UI.SmartClient.exe.config";
            }

            XmlDocument xDoc;

            XmlDocument xDocApp;

            bool isAppTrue = CreateXmlDocument(out xDocApp, fileAppName);

            bool isTrue = CreateXmlDocument(out xDoc, filename);

            if (isTrue && isAppTrue)
            {
                bool saveApp = SaveUpdateResult(xDocApp, AppKey, AppValue, fileAppName);

                if (saveApp == true)
                {
                    WriteLog.Log_Info("保存更新结果到App.config成功！");
                }
                else
                {
                    WriteLog.Log_Info("保存更新结果到App.config失败！");
                }

                bool saveUpdateFile = SaveUpdateResult(xDoc, AppKey, AppValue, filename);

                if (saveApp == true)
                {
                    WriteLog.Log_Info("保存更新结果到AFC.WS.UI.SmartClient.exe.config成功！");
                }
                else
                {
                    WriteLog.Log_Info("保存更新结果到AFC.WS.UI.SmartClient.exe.config失败！");
                }
            }
            else
            {
                WriteLog.Log_Info("创建XmlDocument对象失败！");
            }
        }

        /// <summary>
        /// 获取app.config中的某个key的value.
        /// </summary>
        /// <param name="AppKey">key</param>
        private string GetAppValue(string AppKey)
        {
            XmlDocument xDoc;
            bool isAppTrue = CreateXmlDocument(out xDoc, SystemName + ".exe.config");
            return GetValueResult(xDoc, AppKey);
        }

        /// <summary>
        /// 根据文件名称创建XmLDocument对象，加载配置文件。
        /// </summary>
        /// <param name="Xdoc">xmlDocument变量</param>
        /// <param name="filename">文件名称</param>
        /// <returns>True:成功，False:失败</returns>
        private bool CreateXmlDocument(out XmlDocument Xdoc,string filename)
        {
            Xdoc = new XmlDocument();

            try
            {
                string fileName = FindConfigFile(filename);
                //此处配置文件在程序目录下
                Xdoc.Load(fileName);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("查找文件出错:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="xDoc">XmlDocument对象</param>
        /// <param name="AppKey">Key</param>
        /// <param name="AppValue">Value</param>
        /// <param name="filename">保存文件名称</param>
        /// <returns>True：保存成功，False：保存失败</returns>
        private bool SaveUpdateResult(XmlDocument xDoc, string AppKey, string AppValue, string filename)
        {
            try
            {
                string fileName = FindConfigFile(filename);

                XmlNode xNode;

                XmlElement xElem1;

                XmlElement xElem2;

                xNode = xDoc.SelectSingleNode("//appSettings");

                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");

                if (xElem1 != null)
                {
                    
                    xElem1.SetAttribute("value", AppValue);
                }
                else
                {
                    xElem2 = xDoc.CreateElement("add");
                    xElem2.SetAttribute("key", AppKey);
                    xElem2.SetAttribute("value", AppValue);
                    xNode.AppendChild(xElem2);
                }
                xDoc.Save(fileName);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("保存更新结果异常：" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="xDoc">XmlDocument对象</param>
        /// <param name="AppKey">Key</param>
        /// <param name="AppValue">Value</param>
        /// <param name="filename">保存文件名称</param>
        /// <returns>True：保存成功，False：保存失败</returns>
        private string GetValueResult(XmlDocument xDoc, string AppKey)
        {
            try
            {
                XmlNode xNode;

                XmlElement xElem1;

                xNode = xDoc.SelectSingleNode("//appSettings");

                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");

                if (xElem1 != null)
                {
                    return xElem1.GetAttribute("value");
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("保存更新结果异常：" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 获取配置文件Config/sysconfig中值。
        /// </summary>
        /// <param name="xDoc">XmlDocument对象</param>
        /// <param name="AppKey">Key</param>
        /// <param name="AppValue">Value</param>
        /// <param name="filename">保存文件名称</param>
        /// <returns>True：保存成功，False：保存失败</returns>
        private string GetConfigFileValue(XmlDocument xDoc, string AppKey)
        {
            try
            {
                XmlNode xNode;

                XmlNode xElem1;

                xNode = xDoc.SelectSingleNode("//LocalParamsConfig");

                xElem1 = xNode.SelectSingleNode(AppKey);

                if (xElem1 != null)
                {
                    return xElem1.ChildNodes[0].Value;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                //WriteLog.Log_Error("保存更新结果异常：" + ex.ToString());
            }
            return null;
        }

        // ---> 查找配置文件
        /// <summary>
        /// 查找路径，首先在到bin下找，在到程序目录找。
        /// 在上两层的目录下找配置文件。
        /// </summary>
        /// <returns>配置文件的路径</returns>
        /// <param name="fileName">配置文件名称</param>
        private string FindConfigFile(string fileName)
        {
            try
            {
                FileInfo fp = null;

                //按照文件名称在默认路径下进行查找
                string[] fileNames = new string[]
				{
					fileName , 
					"../../" + fileName
				};

                for (int i = 0; i < fileNames.Length; i++)
                {
                    fp = new FileInfo(fileNames[i]);
                    if (fp.Exists)
                        return fp.FullName;
                }

                //按照默认文件名默认文件路径进行查找
                return null;
            }
            catch
            {
                return "";
            }
        }

        // ---> 初始化日志对象，用于记录日志系统本身的日志 
        /// <summary>
        /// 初始化日志对象，用于记录日志系统本身的日志 
        /// </summary>
        /// <returns>成功则返回true；失败则返回false</returns>
        /// <remarks>
        ///     该函数必须要系统初始化时首先调用。因为其他模块进行初始化时，均要记录日志。
        /// </remarks>
        public bool InitLogDll()
        {
            try
            {
                string logSavePath = @".\SelfLogFile";
                if (!System.IO.Directory.Exists(logSavePath))
                    System.IO.Directory.CreateDirectory(logSavePath);

                string logConfigIniPath = @".\Dll\logCppConfig.ini";

                WriteLog.InitLogInstance(logConfigIniPath, "WSFC_JG");

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断解压的文件名称是否存在。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>True：成功，False：失败</returns>
        private bool IsExistZipFileName(string fileName, XmlDocument xDoc)
        {
            try
            {
                //IDictionary dictionarys = ConfigurationSettings.GetConfig("ZipFileName") as IDictionary;
                XmlNode xNode;

                xNode = xDoc.SelectSingleNode("//ZipFileName");

                foreach (XmlNode dic in xNode)
                {
                    XmlAttributeCollection Values=dic.Attributes;
                    string value = Values[1].Value;
                    if (fileName == value)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("判断压缩文件是否存出现异常：" + ex.ToString());

                return false;
            }
        }

        #endregion
    }
}
