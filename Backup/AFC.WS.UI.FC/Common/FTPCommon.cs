using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    
    using AFC.WorkStation.FTP;

    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110221
    /// 代码功能：FTP操作类，能够下载FTP文件信息。
    /// 修订记录：
    /// </summary>
    public class FTPCommon
    {
        /// <summary>
        /// ftp user
        /// </summary>
        private  string fTPUser;

        /// <summary>
        /// ftp password
        /// </summary>
        private string ftpPassword;

        /// <summary>
        /// ftpAddress
        /// </summary>
        private string ftpAddress;



       public FTPCommon(string userName, string pwd,string ftpAddress)
        {
            this.ftpPassword = pwd;
            this.fTPUser = userName;
            this.ftpAddress = ftpAddress;
        }


        #region [       Ftp Methods       ]

        /// <summary>
        /// 通过FTP上传指定文件.
        /// </summary>
        /// <param name="souceFullPath">将要上传的文件本机绝对路径（包含文件名）</param>
        /// <param name="desPath">将要上传至FTP服务器的相对路径，若为根目录则为“/”。</param>
        /// <param name="desFileName">上传后的文件名</param>
        /// <returns>成功返回0，否则返回错误代码<returns>
        public int FTPUpLoad(string souceFullPath, string desPath, string desFileName)
        {
            //if (string.IsNullOrEmpty(this.fTPUser))
            //{
            //    GetFTPInfo();
            //}
            FTP.FtpUserID = fTPUser;
            FTP.FtpPassword = ftpPassword;
            // 构造目标uri
            string uri = "ftp://" + this.ftpAddress + "/" + desPath + "/" + desFileName;

            int ret = FTP.Upload(uri, souceFullPath);
            if (ret == 0)
            {
                //log
                int logRet;
                //Log.InsertCommLogInfo(
                //    out logRet, LogType.Comm_Log, CommLogSubType.SC_To_WS_Exception, "", Log_Level.Hight, SysConfig.GetSysConfig().LocalParamsConfig.SystemName,SysConfig.GetSysConfig().CommParamsConfig.ScIpAddress, FTPAddress, FTPAddress, "FTP",
                //    "FTP Upload :" + souceFullPath + "to:" + uri);
            }
            else
            {
                WriteLog.Log_Error("FTP失败,操作员：" + this.fTPUser + "密码：" + this.ftpPassword + "源路径：" + souceFullPath + "目的路径:" + desPath);
            }

            return ret;

        }

        /// <summary>
        /// 通过FTP下载指定文件，并按指定的绝对路径存储。
        /// </summary>
        /// <param name="soucePath">将要下载的文件相对路径（包含文件名）</param>
        /// <param name="desFullPath">存储绝对路径（包含文件名）</param>
        /// <returns></returns>
        public int FTPDownLoad(string soucePath, string desFullPath)
        {
            //if (string.IsNullOrEmpty(fTPUser))
            //{
            //    GetFTPInfo();
            //}
            FTP.FtpUserID = fTPUser;
            FTP.FtpPassword = ftpPassword;
            //构造源uri
            string uri = "ftp://" + ftpAddress + "/" + soucePath;

            int ret = FTP.DownLoad(uri, desFullPath);

            if (ret == 0)
            {
                //log
                int logRet;
                //Log.InsertCommLogInfo(
                //    out logRet, LogType.Comm_Log, CommLogSubType.SC_To_WS_Exception, "", Log_Level.Hight, SysConfig.GetSysConfig().LocalParamsConfig.SystemName,SysConfig.GetSysConfig().CommParamsConfig.ScIpAddress, FTPAddress, FTPAddress, "FTP",
                //    "FTP Download :" + desFullPath + "form:" + uri);
            }
            else
            {
                WriteLog.Log_Error("FTP失败,操作员：" + this.fTPUser + "密码：" + this.ftpPassword + "源路径：" + soucePath + "目的路径:" + desFullPath);
            }

            return ret;
        }

        /// <summary>
        /// 获取目标路径文件夹下所有文件信息
        /// </summary>
        /// <param name="desFullPath">目标路径</param>
        /// <returns>文件信息</returns>
        public string[] FTPGetFileInfo(string desFullPath)
        {
            try
            {
                //if (string.IsNullOrEmpty(fTPUser))
                //{
                //    GetFTPInfo();
                //}
                FTP.FtpUserID = fTPUser;
                FTP.FtpPassword = ftpPassword;
                //构造源uri
                string uri = "ftp://" + ftpAddress + "/" + desFullPath;

                string[] fileInfo = FTP.GetFileList(uri);

                return fileInfo;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            return null;
        }

       
        #endregion
    }
}
