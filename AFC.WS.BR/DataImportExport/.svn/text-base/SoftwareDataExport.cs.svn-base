using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.Model.Const;

namespace AFC.WS.BR.DataImportExport
{
    public class SoftwareDataExport:AbstractDataExport
    {
        private int fileCount = 0;

        private List<ParaLocalFullVerInfo> listParaInfo = new List<ParaLocalFullVerInfo>();

        public override FTPInfo GetFtpInfo()
        {
            FTPInfo ftpInfo = new FTPInfo();
            ftpInfo.downPath = "runs/data/para_data/all";
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
                List<ParaLocalFullVerInfo> listPara = DBCommon.Instance.GetTModelValue<ParaLocalFullVerInfo>(strSql);
                if (listPara == null || listPara.Count == 0)
                    return null;
                listParaInfo = listPara;
                List<string> listFileName = new List<string>();
                foreach (var temp in listPara)
                {
                    if (!string.IsNullOrEmpty(temp.para_file_name))
                    {
                        listFileName.Add(temp.para_file_name);
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
            //LCC参数
            if (cc.paraType == "01" && cc.verType == "01")
            {
                strSQL = "select * from para_local_full_ver_info t where t.edition_type='0' and t.para_master_type='4300' and t.para_file_name is not null";
            }
            if (cc.paraType == "01" && cc.verType == "02")
            {
                strSQL = "select * from para_local_full_ver_info t where t.edition_type='1' and t.para_master_type='4300' and t.para_file_name is not null";
            }
            if (cc.paraType == "01" && cc.verType == "03")
            {
                strSQL = "select * from para_local_full_ver_info t where t.para_master_type='4300' and t.para_file_name is not null";
            }

            if (string.IsNullOrEmpty(strSQL))
                return null;
            return strSQL;
        }

        private IndexFileData SetParaData()
        {
            IndexFileData fileData = IndexFileData.CreateIndexFileData(OperateType.SOFT_DATA_FILE);
            fileData.header.fileType = "2";
            fileData.header.fileCount = fileCount.ToString();
            fileData.header.createTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            fileData.header.modifiedType = DateTime.Now.ToString("yyyyMMddHHmmss");
            Header header = fileData.header as Header;
            if (listParaInfo == null || listParaInfo.Count == 0)
                return null;
            ParamIndexFileBody paraBody = new ParamIndexFileBody();
            foreach (var temp in listParaInfo)
            {
                ParamBody body = new ParamBody();
                body.activeDateTime = temp.active_date + temp.active_time;
                body.devicePartID = "0000";
                body.fileName = "export\\para_soft\\" + temp.para_file_name;
                body.manufactureID = "00";
                body.parameterType = temp.para_type;
                body.parameterVersion = temp.para_version;
                body.versionType = temp.edition_type;
                paraBody.listParam.Add(body);
            }

            fileData.body = paraBody;
            return fileData;
        }

        public override int RewriteFile(string fileName)
        {
            IndexFileData fileData = SetParaData();
            int res = 0;
            ParaIndexFileHandle paraHandle = new ParaIndexFileHandle();
            res = paraHandle.CreateIndexFile(fileName, fileData);
            if (res != 0)
                return -1;
            return res;
        }

        public override int ExportSuccHandle()
        {
            //todo:记录操作日志
            int res = BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.soft_data_import, "0", "导出软件程序成功！");
            if (res != 0)
                return -1;
            return res;
        }

    }
}
