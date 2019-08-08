using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.Const;
using AFC.WS.BR;
using System.Windows;

namespace AFC.WS.ModelView.Actions.RunManager
{
    public class SCDevEndAction :  IAction
    {

         #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            //if (!BR.BuinessRule.GetInstace().rm.CheckHasRunEnd())
            //{
            //    MessageDialog.Show("服务器已经运营结束!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //2011.7.15 如果有设备运营未结束做提示
            //if (BR.BuinessRule.GetInstace().rm.CheckHasDevEnd())
            //{
            //      MessageBoxResult res=
            //       MessageDialog.Show("还有设备处于运营未结束状态,\r\n是否做服务器运营结束？", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
            // if (res == MessageBoxResult.No)
            // {
            //     return false;
            // }
            //}
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            List<TJComm.DeviceRange> list = new List<TJComm.DeviceRange>();
            list.Add(new TJComm.DeviceRange { special_flag = 1, stationId = SysConfig.GetSysConfig().LocalParamsConfig.StationCode.ConvertHexStringToUshort(), deviceRange = new List<uint>() });
            
            int res = BR.BuinessRule.GetInstace().commProcess.ControlCmd(Convert.ToByte("01"),"0104".ConvertHexStringToUshort(),list);

            if (res != 0)
            {
                MessageDialog.Show("发送设备运营结束命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.DEV_RUN_END, "1", "设备运营结束指令发送失败");
            }
            else
            {
                MessageDialog.Show("发送设备运营结束命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.DEV_RUN_END, "0", "设备运营结束指令发送成功");
                BR.BuinessRule.GetInstace().rm.StartDevRunMonitor(AsynMessageType.DeviceRunEnd);
            }
          
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
