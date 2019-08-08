using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.IO;

namespace AFC.WS.BR.DataImportExport
{
    public abstract class AbstractDataImport
    {
        //Ftp信息实体类
        protected FTPInfo ftpInfo=new FTPInfo();

        //磁盘相关操作类
        DiskOperation diskOperation = new DiskOperation();

        //磁盘读取，验证相关类
        ValidateAuthPhysicalSN usbOper = new ValidateAuthPhysicalSN();

        //根据不同子类重写该方法获取不同ftp信息
        public abstract FTPInfo GetFtpInfo();

        /// <summary>
        /// 导入主函数
        /// </summary>
        /// <param name="indexFileName">索引文件在磁盘中的绝对路劲</param>
        /// <returns></returns>
        public int ImportFile(string indexFileName)
        {
            ///验证磁盘
            if (!CheckDiskValid())
            {
                return -1;
            }
            ///获取FTP信息
            ftpInfo = GetFtpInfo();
            if (ftpInfo == null)
            {
                return -2;
            }
            ///获取文件名（索引文件中带有相对路劲的文件名）
            List<string> fileCollection = GetNeedImportFiles(indexFileName);
            if (fileCollection == null ||
                fileCollection.Count == 0)
            {
                return -3;
            }

            int res = 0;
            ///上传文件
            for (int i = 0; i < fileCollection.Count; i++)
            {
                string fileName = Path.GetFileName(fileCollection[i]);
                fileCollection[i] = ImportExportManager.ConvertPathToOpposite(fileCollection[i]);//将路劲中的正斜杠变成反斜杠
                string strFilePath = usbOper.getFirstUSB() + fileCollection[i];
                res = this.UpLoadFile(strFilePath, fileName);

                if (res != 0)
                {
                    return -4;
                }
                else
                {
                    //发消息，内容为上传成功的百分比和文件名，UI层接收消息
                    int percent = (i+1)*100 / fileCollection.Count;
                    ImportExportManager.SendMessage(MessageType.ImportMessage, percent, fileName);
                }
            }
             ///上传索引文件
            switch (Path.GetFileName(indexFileName))
            {
                case "ParameterIndexFile.dat":
                    res = this.UpLoadFile(usbOper.getFirstUSB() + "ParameterIndexFile.dat", "ParameterIndexFile.dat");
                    break;
                case "MemoryIndexFile.dat":
                    res = this.UpLoadFile(usbOper.getFirstUSB() + "MemoryIndexFile.dat", "MemoryIndexFile.dat");
                    break;
            }

            if (res != 0)
            {
                return -5;
            }

            return 0;
        }

        /// <summary>
        /// 获取带入文件名
        /// </summary>
        /// <param name="indexFileName">索引文件名</param>
        /// <returns></returns>
        public abstract List<string> GetNeedImportFiles(string indexFileName);

        /// <summary>
        /// 导入成功后的处理：发报文、记录操作日志
        /// </summary>
        /// <returns></returns>
        public abstract int ImportSuccHandle();

        /// <summary>
        /// 创建子类的工厂方法，在Action中调用
        /// </summary>
        /// <param name="operateType">枚举</param>
        /// <returns></returns>
        public static AbstractDataImport CreateInstance(OperateType operateType)
        {
            AbstractDataImport dataImport;
            switch (operateType)
            {
                case OperateType.BUSI_DATA_FILE:
                    dataImport = new BusiDataImport();
                    break;
                case OperateType.TRADE_DATA_FILE:
                    dataImport = new TradeDataImport();
                    break;
                case OperateType.PARA_DATA_FILE:
                    dataImport = new ParamDataImport();
                    break;
                case OperateType.SOFT_DATA_FILE:
                    dataImport =new SoftwareDataImport();
                    break;
                default:
                    return null;
            }
            return dataImport;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">绝对路劲</param>
        /// <param name="desFileName">上传后文件名</param>
        /// <returns>成功返回0.失败返回-1；</returns>
        public int UpLoadFile(string fileName,string desFileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(desFileName))
                return -1;

            string ftpUser = ftpInfo.user;//SysConfig.GetSysConfig().CommParamsConfig.FtpUserName;
            string ftpPwd = ftpInfo.pwd;//SysConfig.GetSysConfig().CommParamsConfig.FtpUserPwd;
            string ftpAddr = ftpInfo.ftpAddress;//SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress;
            string ftpPath = ftpInfo.uploadPath;//SysConfig.GetSysConfig().FtpDirectory.DealDataFilePath;

            if (string.IsNullOrEmpty(ftpUser) ||
                    string.IsNullOrEmpty(ftpPwd) ||
                    string.IsNullOrEmpty(ftpAddr) ||
                     string.IsNullOrEmpty(ftpPath))
            {
                return -1;
            }

            FTPCommon ftpCommon = new FTPCommon(ftpUser, ftpPwd, ftpAddr);
            return ftpCommon.FTPUpLoad(fileName, ftpPath, desFileName);
        }

        /// <summary>
        /// 磁盘验证
        /// </summary>
        /// <returns></returns>
        protected bool CheckDiskValid()
        {
            return diskOperation.CheckDiskValid();
        }


    }
}
