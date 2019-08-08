using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR.DataImportExport;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.DataImportExport
{
    public class InitAuthenticationFileAction:IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            throw new NotImplementedException();
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            try
            {
                ValidateAuthPhysicalSN validAuthPhysical = new ValidateAuthPhysicalSN();
                GetUSBPhysicalSN gun = new GetUSBPhysicalSN();
                string PathU = validAuthPhysical.getFirstUSB();
                if (PathU == "")
                {
                    AFC.WS.UI.CommonControls.MessageDialog.Show("请正确插入移动硬盘!", "警告", AFC.WS.UI.CommonControls.MessageBoxIcon.Warning, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                    return null;
                }

                Boolean valid = validAuthPhysical.writeConf(PathU);
                if (valid)
                {
                    MessageDialog.Show("USB初始化成功!", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                    //this.txtPhysicalSN.Text = gun.SerchByDeviceLetter(PathU);

                    //this.lblResult.Content = "USB初始化成功!";
                }
                else
                {
                    MessageDialog.Show("USB初始化失败!", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return null;
                    //this.lblResult.Content = "USB初始化失败!";
                }
            }
            catch
            {
                MessageDialog.Show("USB初始化失败!", "提示！", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
                //this.lblResult.Content = "USB初始化失败!";
            }
        }

        #endregion
    }
}
