using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.IO;

namespace AFC.WS.BR.DataImportExport
{
    public abstract class AbstractDataExport
    {
        //Ftp信息实体类
        protected FTPInfo ftpInfo = new FTPInfo();

        //磁盘相关操作类
        DiskOperation diskOperation = new DiskOperation();

        //磁盘读取，验证相关类
        ValidateAuthPhysicalSN usbOper = new ValidateAuthPhysicalSN();

        
        /// <summary>
        ///  根据不同子类重写该方法获取不同ftp信息
        /// </summary>
        /// <returns></returns>
        public abstract FTPInfo GetFtpInfo();

        /// <summary>
        /// 获取导出文件名
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public abstract List<string> GetExportFiles(ConditionClass cc);

        /// <summary>
        /// 生成索引文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public abstract int RewriteFile(string fileName);

        /// <summary>
        /// 导出成功后的处理：记录操作日志
        /// </summary>
        /// <returns></returns>
        public abstract int ExportSuccHandle();

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileName">将要下载的文件相对路径（包含文件名）</param>
        /// <param name="downloadDict">存储绝对路径（包含文件名）</param>
        /// <returns></returns>
        public int DownloadFile(string fileName, string downloadDict)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(downloadDict))
                return -1;
            string ftpUser = ftpInfo.user;//SysConfig.GetSysConfig().CommParamsConfig.FtpUserName;
            string ftpPwd = ftpInfo.pwd;//SysConfig.GetSysConfig().CommParamsConfig.FtpUserPwd;
            string ftpAddr = ftpInfo.ftpAddress;//SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress;
            string ftpPath = ftpInfo.downPath;//SysConfig.GetSysConfig().FtpDirectory.DealDataFilePath;

            if (string.IsNullOrEmpty(ftpUser) ||
                    string.IsNullOrEmpty(ftpPwd) ||
                    string.IsNullOrEmpty(ftpAddr) ||
                     string.IsNullOrEmpty(ftpPath))
            {
                return -1;
            }

            FTPCommon ftpCommon = new FTPCommon(ftpUser, ftpPwd, ftpAddr);
            int res = 0;
            res = ftpCommon.FTPDownLoad(fileName, downloadDict);
            if (res != 0)
                return -1;

            return res;
        }

        /// <summary>
        /// 创建导出实体类的工厂方法
        /// </summary>
        /// <param name="operateType"></param>
        /// <returns></returns>
        public static AbstractDataExport CreateInstance(OperateType operateType)
        {
            AbstractDataExport dataExport;
            switch (operateType)
            {
                case OperateType.BUSI_DATA_FILE:
                    dataExport = new BusiDataExport();
                    break;
                case OperateType.TRADE_DATA_FILE:
                    dataExport = new TradeDataExport();
                    break;
                case OperateType.PARA_DATA_FILE:
                    dataExport = new ParamDataExport();
                    break;
                case OperateType.SOFT_DATA_FILE:
                    dataExport = new SoftwareDataExport();
                    break;
                default:
                    return null;
            }
            return dataExport;
         }

        /// <summary>
        /// 导出主要功能函数
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public int ExportFiles(ConditionClass cc)
        {
            if (!CheckDiskValid())//验证磁盘
            {
                return -1;
            }
            ftpInfo = GetFtpInfo();//获取Ftp信息
            if (ftpInfo == null)
            {
                return -2;
            }
            List<string> fileCollection = GetExportFiles(cc);//获取需要导出文件名
            if (fileCollection == null ||
                fileCollection.Count == 0)
            {
                return -3;
            }
            int res = 0;
            for (int i = 0; i < fileCollection.Count; i++)
            {
                string fileName = Path.GetFileName(fileCollection[i]);
                string strFilePath = usbOper.getFirstUSB() + "export\\data\\" + fileName;
                res = this.DownloadFile(ftpInfo.downPath+"/" + fileCollection[i], strFilePath);//下载文件
                if (res != 0)
                {
                    File.Delete(strFilePath);//下载失败时，删除本地生成的空文件
                    return -4;
                }
                else
                {
                    //发消息，内容为下载成功的百分比和文件名，UI层接收消息
                    int percent = (i + 1) * 100 / fileCollection.Count;
                    ImportExportManager.SendMessage(MessageType.ExportMessage, percent, fileName);
                }
            }
            return 0;
        }

        /// <summary>
        /// 验证磁盘
        /// </summary>
        /// <returns></returns>
        protected bool CheckDiskValid()
        {
            return diskOperation.CheckDiskValid();
        }

    }
}
