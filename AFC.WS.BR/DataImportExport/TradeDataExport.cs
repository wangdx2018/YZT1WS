using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.DataImportExport
{
    public class TradeDataExport:AbstractDataExport
    {
        private int fileCount = 0;

        private List<DataFileUpInfo> listData = new List<DataFileUpInfo>();

        public override FTPInfo GetFtpInfo()
        {
            FTPInfo ftpInfo = new FTPInfo();
            ftpInfo.downPath = "runs/data/tran_data/send";
            ftpInfo.ftpAddress = SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress;
            ftpInfo.port = SysConfig.GetSysConfig().CommParamsConfig.ScPort.ToString();
            ftpInfo.pwd = SysConfig.GetSysConfig().CommParamsConfig.FtpUserPwd;
            ftpInfo.uploadPath = string.Empty;
            ftpInfo.user = SysConfig.GetSysConfig().CommParamsConfig.FtpUserName;
            return ftpInfo;
        }

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
                listData = listTrade;
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
                fileCount = listFileName.Count;
                return listFileName;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        private string GetSQl(ConditionClass cc)
        {
            string strSQL = string.Empty;
            if (cc.upResult == "01")
            {
                strSQL = string.Format("select * from data_file_up_info t where t.up_result='0' and (t.file_type='2001' or t.file_type='2002') and (t.set_date between '{0}' and '{1}') and t.file_name is not null", cc.startDate, cc.endDate);
            }
            if (cc.upResult == "02")
            {
                strSQL = string.Format("select * from data_file_up_info t where t.up_result='1' and (t.file_type='2001' or t.file_type='2002') and (t.set_date between '{0}' and '{1}') and t.file_name is not null", cc.startDate, cc.endDate);
            }
            if (cc.upResult == "03")
            {
                strSQL = string.Format("select * from data_file_up_info t where (t.file_type='2001' or t.file_type='2002') and (t.set_date between '{0}' and '{1}') and t.file_name is not null", cc.startDate, cc.endDate);
            }

            if (string.IsNullOrEmpty(strSQL))
                return null;
            return strSQL;
        }

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

        public override int ExportSuccHandle()
        {
            //todo:记录操作日志
            int res = BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.trade_data_import, "0", "导交易数据成功！");
            if (res != 0)
                return -1;
            return res;
        }
    }
}
