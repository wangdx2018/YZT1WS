using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR.DataImportExport;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.DataImportExport
{
    public class SoftwareImportAction:IAction
    {
        SoftwareDataImport paraImport = AbstractDataImport.CreateInstance(OperateType.SOFT_DATA_FILE) as SoftwareDataImport;

        ValidateAuthPhysicalSN usbOper = new ValidateAuthPhysicalSN();

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            string cmbValue = actionParamsList.Single(temp => temp.bindingData.Equals("cmbText")).value.ToString();
            if (string.IsNullOrEmpty(cmbValue))
            {
                MessageDialog.Show("请选择导入的数据类型！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
           // FTPInfo ftpInfo = paraImport.GetFtpInfo();
            string paraIndexPath = usbOper.getFirstUSB() + "ParameterIndexFile.dat";
            int res = paraImport.ImportFile(paraIndexPath);
            switch (res)
            {
                case -1:
                    MessageDialog.Show("磁盘验证失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -2:
                    MessageDialog.Show("Ftp信息为空失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -3:
                    MessageDialog.Show("解析索引文件失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -4:
                    MessageDialog.Show("上传程序文件失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                case -5:
                    MessageDialog.Show("上传索引文件失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                default:
                    break;

            }
            res = paraImport.ImportSuccHandle();
            if (res == -1)
            {
                MessageDialog.Show("报文发送失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            if (res == -2)
            {
                MessageDialog.Show("记录操作日志失败！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            MessageDialog.Show("程序文件导入成功！等待后台处理！", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
