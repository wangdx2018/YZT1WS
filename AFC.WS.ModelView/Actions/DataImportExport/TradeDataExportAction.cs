using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR.DataImportExport;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.DataImportExport
{
    public class TradeDataExportAction:IAction
    {
        TradeDataExport tradeExport = AbstractDataExport.CreateInstance(OperateType.TRADE_DATA_FILE) as TradeDataExport;

        ValidateAuthPhysicalSN usbOper = new ValidateAuthPhysicalSN();

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0 ||
                actionParamsList.Single(temp => temp.bindingData.Equals("Condition")).value == null)
            {
                MessageDialog.Show("请检查导出条件！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //FTPInfo ftpInfo = tradeExport.GetFtpInfo();
            string memIndexPath = usbOper.getFirstUSB();
            ConditionClass condition = actionParamsList.Single(temp => temp.bindingData.Equals("Condition")).value as ConditionClass;
            if (condition == null)
                return null;
            int res = tradeExport.ExportFiles(condition);
            switch (res)
            {
                case -1:
                    MessageDialog.Show("磁盘验证失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -2:
                    MessageDialog.Show("Ftp信息为空！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -3:
                    MessageDialog.Show("数据库中不存在满足该条件的文件！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -4:
                    MessageDialog.Show("服务器上不存在该文件，下载失败！！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                default:
                    break;

            }
            res = tradeExport.RewriteFile(memIndexPath);
            if (res == -1)
            {
                MessageDialog.Show("索引文件生成失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }

            ///记录操作日志
            //res = tradeExport.ExportSuccHandle();
            //if (res == -1)
            //{
            //    MessageDialog.Show("记录操作日志失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return null;
            //}

            MessageDialog.Show("交易数据导出成功！等待后台处理！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            return new ResultStatus { resultCode = 0, resultData = 0 };

        }

        #endregion
    }
}
