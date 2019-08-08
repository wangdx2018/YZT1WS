using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.SysStart
{

    using AFC.WS.UI.Common;
    using AFC.WorkStation.DB;
    using AFC.BJComm;
    using System.Data;
    using System.IO;
    using System.Collections;
    using System.Diagnostics;
    using System.Xml;
    using System.Runtime.InteropServices;
    using AFC.WS.Model.DB;


    public class SoftAndParaUpdate
    {
        /// <summary>
        /// 启动绝对路径
        /// </summary>
        private string path = Environment.CurrentDirectory;

        /// <summary>
        /// 参数版本信息集合
        /// </summary>
        private DataSet ds = null;

        /// <summary>
        /// 参数版本信息
        /// </summary>
        private List<ParaLocalFullVerInfo> paraVersionSynInfo = null;
        /// <summary>/// 软件更新
        /// </summary>
        public int SoftWareUpdate()
        {
            try
            {
                bool isDownLoad = DownLoadProgram();

                if (isDownLoad)
                {
                    bool copyResult = CopyTempToCurrent();
                    if (copyResult)
                    {
                        StartAssistantProgram();
                    }
                    else
                    {
                        AFC.WS.UI.Common.WriteLog.Log_Error("CopyTempToCurrent方法执行失败，可能原因运营日活激活日期引起的。");
                        return -1;
                    }
                }
                else
                {
                    AFC.WS.UI.Common.WriteLog.Log_Info("此软件为最新版本或服务器不存在此版本软件！");
                }
                return 0;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("SoftWareUpdate异常:" + ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 通过FTP从服务下载需要下载的软件或参数.
        /// </summary>
        /// <returns>true:成功，false:失败</returns>
        private bool DownLoadProgram()
        {
            try
            {
                string wsFtpDownPath = GetAppValue("ParamFTPDownLoadPath");
                int count = 0;
                List<string> addressName = new List<string>();

                if (GetVersionInfo(ref addressName))
                {

                    AFC.WS.UI.Common.WriteLog.Log_Error("GetVersionInfo成功，取得可以下载的列表");
                    foreach (string paraAdressName in addressName)
                    {
                        FTPCommon ftpComm = new FTPCommon(BuinessRule.GetInstace().GetFTPUserName(), BuinessRule.GetInstace().GetFTPUserPwd(), BuinessRule.GetInstace().GetFTPAddress());
                        string SystemName = SysConfig.GetSysConfig().LocalParamsConfig.SystemName;
                        int result = ftpComm.FTPDownLoad(paraAdressName, path + wsFtpDownPath + SystemName+"EXE.exe");
                       // int result = BuinessRule.GetInstace().ftpCommon.FTPDownLoad(paraAdressName, path + wsFtpDownPath);

                        if (0 == result)
                        {
                            count++;
                            AFC.WS.UI.Common.WriteLog.Log_Info(paraAdressName + "软件下载成功！");
                        }
                        else
                        {
                            AFC.WS.UI.Common.WriteLog.Log_Info(paraAdressName + "软件下载失败！");
                        }
                    }
                }
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("DownLoadProgram方法异常:" + ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 获取当前版本信息
        /// 
        /// para_local_full_ver_info数据库表
        /// 
        /// edition_type='0' and active_date<=当前系统日期
        /// </summary>
        /// <returns>true:有新版本软件，false:无新版本软件</returns>
        public bool GetVersionInfo(ref List<string> updateFile)
        {
            bool isUpdate = false;
            DataRow[] dataRows = null;
            try
            {
                DataSet dataSet = new DataSet();

                paraVersionSynInfo = BuinessRule.GetInstace().paraManager.GetParaVersionSynInfo(out ds);
                if (paraVersionSynInfo == null)
                    return false;
                if (paraVersionSynInfo.Count == 0)
                    return false;
                if (ds == null)
                {
                    AFC.WS.UI.Common.WriteLog.Log_Error("数据库表para_local_full_ver_info无版本信息!");
                    return false;
                }
              
                ds.WriteXml(path + @"\VersionFile\ParaTemp\WsFileVerInfo.xml");
                //若存在，则读取其内容。
                dataSet.ReadXml(path + @"\VersionFile\ParaCurrent\WsFileVerInfo.xml");

                if (dataSet.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string paraTypeCode = ds.Tables[0].Rows[i]["para_type"].ToString();
                        int paraVersion = Convert.ToInt32(ds.Tables[0].Rows[i]["para_version"].ToString());
                        //取得当前版本上传路径
                        string severPath = BuinessRule.GetInstace().GetBasiRunParamsValue("0304").param_value;
                        //取得当前文件的绝对路径
                        string paraAdressName = severPath + "/" + ds.Tables[0].Rows[i]["para_file_name"].ToString();

                        AFC.WS.UI.Common.WriteLog.Log_Debug("GetVersionInfo取得下载路径：" + paraAdressName);

                        dataRows = dataSet.Tables[0].Select("para_type ='" + paraTypeCode + "'");
                        if (dataRows != null && dataRows.Count()>0)
                        {
                            int curParaVersion = Convert.ToInt32(dataRows[0]["para_version"].ToString());
                            if (curParaVersion < paraVersion)
                            {
                                updateFile.Add(paraAdressName);
                            }

                        }
                        else
                        {
                            updateFile.Add(paraAdressName);
                        }
                    }

                }

                if (updateFile.Count > 0)
                {
                    isUpdate = true;
                }
                return isUpdate;

            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("GetVersionInfo函数异常：" + ex.ToString());
            }
            return isUpdate;
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <returns>true:成功，false:失败</returns>
       private bool CopyTempToCurrent()
        {
            try
            {
                int count = 0;
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(path + @"\VersionFile\ParaTemp\WsFileVerInfo.xml");
                dataSet.WriteXml(path + @"\VersionFile\ParaCurrent\WsFileVerInfo.xml");
                return true;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("更新程序异常：" + ex.ToString());
                return false;
            }
           
        }

        /// <summary>
        /// 
        ///判断是否已经存在辅助程序，若存在则删除，
        ///
        /// 若不存在，则启动辅助程序。
        /// </summary>
        /// <returns>true:成功，false:失败</returns>
        public bool StartAssistantProgram()
        {
            try
            {
                Process[] allProcess = Process.GetProcessesByName("AFC.WS.UI.SmartClient");

                if (allProcess.Count() != 0)
                {
                    foreach (Process pc in allProcess)
                    {
                        pc.Kill();
                    }
                }

                if (allProcess.Count() == 0)
                {
                    Process mypro = new Process();
                    mypro.StartInfo.FileName = Environment.CurrentDirectory + @"\" + "AFC.WS.UI.SmartClient.exe";
                    mypro.Start();
                    mypro.WaitForExit();//wait when RAR is ok;
                }

                return true;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("启动辅助程序异常：" + ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 获取app.config中的某个key的value.
        /// </summary>
        /// <param name="AppKey">key</param>
        private string GetAppValue(string AppKey)
        {

            XmlDocument xDoc;
            bool isAppTrue = CreateXmlDocument(out xDoc, SysConfig.GetSysConfig().LocalParamsConfig.SystemName + ".exe.config");
            return GetValueResult(xDoc, AppKey);
        }

        /// <summary>
        /// 根据文件名称创建XmLDocument对象，加载配置文件。
        /// </summary>
        /// <param name="Xdoc">xmlDocument变量</param>
        /// <param name="filename">文件名称</param>
        /// <returns>True:成功，False:失败</returns>
        private bool CreateXmlDocument(out XmlDocument Xdoc, string filename)
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
                AFC.WS.UI.Common.WriteLog.Log_Error("查找文件出错:" + ex.ToString());
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
                AFC.WS.UI.Common.WriteLog.Log_Error("保存更新结果异常：" + ex.ToString());
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


    }
}
