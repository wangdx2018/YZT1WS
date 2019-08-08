using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.Const;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.DataImportExport
{
    public class TradeDataImport:AbstractDataImport
    {

        public override FTPInfo GetFtpInfo()
        {
            FTPInfo ftpInfo = new FTPInfo();
            ftpInfo.downPath = string.Empty;
            ftpInfo.ftpAddress = SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress;
            ftpInfo.port = SysConfig.GetSysConfig().CommParamsConfig.ScPort.ToString();
            ftpInfo.pwd = SysConfig.GetSysConfig().CommParamsConfig.FtpUserPwd;
            ftpInfo.uploadPath = "runs/data/import/data";
            ftpInfo.user = SysConfig.GetSysConfig().CommParamsConfig.FtpUserName;
            return ftpInfo;
        }

        public override List<string> GetNeedImportFiles(string indexFileName)
        {
            MemIndexFileHandle menFileHandle = new MemIndexFileHandle();
            return menFileHandle.ParseIndexFile(indexFileName);
        }

        public override int ImportSuccHandle()
        {
            //todo:发送1009报文
            FTPInfo ftpInfo = this.GetFtpInfo();
            byte type = Convert.ToByte(OperateType.TRADE_DATA_FILE);

            int res = BuinessRule.GetInstace().commProcess.DataImportNotify(type, ftpInfo.uploadPath);
            if (res != 0)
                return -1;


            //todo:记录操作日志
            res = BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.trade_data_import, "0", "导入交易数据成功！");
            if (res != 0)
                return -1;
            return res;
        }
    }
}
