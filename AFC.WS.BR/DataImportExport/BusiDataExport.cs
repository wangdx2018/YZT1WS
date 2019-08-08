using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.DataImportExport
{
    public class   BusiDataExport:AbstractDataExport
    {
        //保存导出文件个数
        private int fileCount = 0;

        //将从数据库查出的信息保存到内存中
        private List<DataFileUpInfo> listData = new List<DataFileUpInfo>();

        public override FTPInfo GetFtpInfo()
        {
            FTPInfo ftpInfo = new FTPInfo();
            ftpInfo.downPath = "runs/data/oper_data/send";
            ftpInfo.ftpAddress = SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress;
            ftpInfo.port = SysConfig.GetSysConfig().CommParamsConfig.ScPort.ToString();
            ftpInfo.pwd = SysConfig.GetSysConfig().CommParamsConfig.FtpUserPwd;
            ftpInfo.uploadPath = string.Empty;
            ftpInfo.user = SysConfig.GetSysConfig().CommParamsConfig.FtpUserName;
            return ftpInfo;
        }

        /// <summary>
        /// 获取下载文件名
        /// </summary>
        /// <param name="cc">界面传过来的数据库查询条件</param>
        /// <returns></returns>
        public override List<string> GetExportFiles(ConditionClass cc)
        {
            string strSql = string.Empty;
            strSql = this.GetSQl(cc);
            if (string.IsNullOrEmpty(strSql))
                return null;
            try
            {
                List<DataFileUpInfo> listTrade = DBCommon.Instance.GetTModelValue<DataFileUpInfo>(strSql);
                if (listTrade == null || listTrade.Count == 0)
                    return null;
                listData = listTrade;//将查询结果保存到内存中、以便于后面生成索引文件
                List<string> listFileName = new List<string>();
                foreach (var temp in listTrade)
                {
                    if (!string.IsNullOrEmpty(temp.file_name))
                    {
                        listFileName.Add(temp.file_name);
                    }
                }
                if (listFileName == null || listFileName.Count == 0)
                    return null;
                fileCount = listFileName.Count;//保存文件个数
                return listFileName;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据界面选择的不同，给出不同的查询SQL
        /// </summary>
        /// <param name="cc">界面传过来的查询条件</param>
        /// <returns></returns>
        private string GetSQl(ConditionClass cc)
        {
            string strSQL = string.Empty;
            if (cc.upResult == "01")
            {
                strSQL = string.Format("select t.file_name from data_file_up_info t where t.up_result='0' and t.file_type!='2001' and t.file_type!='2002' and (t.set_date between '{0}' and '{1}') and t.file_name is not null", cc.startDate, cc.endDate);
            }
            if (cc.upResult == "02")
            {
                strSQL = string.Format("select t.file_name from data_file_up_info t where t.up_result='1' and t.file_type!='2001' and t.file_type!='2002' and (t.set_date between '{0}' and '{1}') and t.file_name is not null", cc.startDate, cc.endDate);
            }
            if (cc.upResult == "03")
            {
                strSQL = string.Format("select t.file_name from data_file_up_info t where t.file_type!='2001' and t.file_type!='2002' and (t.set_date between '{0}' and '{1}') and t.file_name is not null", cc.startDate, cc.endDate);
            }

            if (string.IsNullOrEmpty(strSQL))
                return null;
            return strSQL;
        }

        /// <summary>
        /// 生成索引文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override int RewriteFile(string fileName)
        {
            IndexFileData fileData = SetMemData();
            int res = 0;
            MemIndexFileHandle memHandle = new MemIndexFileHandle();
            res = memHandle.CreateIndexFile(fileName, fileData);
            if (res != 0)
                return -1;
            return res;
        }

        /// <summary>
        /// 给索引文件实体类赋值
        /// </summary>
        /// <returns></returns>
        private IndexFileData SetMemData()
        {
            IndexFileData fileData = IndexFileData.CreateIndexFileData(OperateType.TRADE_DATA_FILE);
            fileData.header.fileType = "1";
            fileData.header.fileCount = "1";//fileCount.ToString();
            fileData.header.createTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            fileData.header.modifiedType = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            HeaderExtern header = fileData.header as HeaderExtern;
            string devID = ImportExportManager.GetDeviceID();
            if (string.IsNullOrEmpty(devID))
                return null;
            header.listDevice.Add(devID);
            MemIndexFileBody memBody = new MemIndexFileBody();
            MemIndexFile body = new MemIndexFile();
            foreach (var temp in listData)
            {
                body.fileCount = fileCount.ToString();
                body.listFileName.Add("export\\data\\" + temp.file_name);
            }
            memBody.listMemIndexFile.Add(body);

            fileData.body = memBody;
            return fileData;
        }

        public override int ExportSuccHandle()
        {
            //todo:记录操作日志
            int res = BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.busi_data_import, "0", "导交易数据成功！");
            if (res != 0)
                return -1;
            return res;
        }
    }
}
