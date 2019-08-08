using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.Const;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.RunManager
{
    public class SCDevStartAction : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            /*if (!BR.BuinessRule.GetInstace().rm.CheckHasRunStart())
            {
                MessageDialog.Show("服务器运营开始失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }*/
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
            
            int res = BR.BuinessRule.GetInstace().commProcess.ControlCmd(Convert.ToByte("01"),"0103".ConvertHexStringToUshort(),list);

            if (res != 0)
            {
                MessageDialog.Show("发送设备运营开始命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.DEV_RUN_START, "1", "设备运营开始指令发送失败");
            }
            else
            {
                MessageDialog.Show("发送设备运营开始命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.DEV_RUN_START, "0", "设备运营开始指令发送成功");
                BR.BuinessRule.GetInstace().rm.StartDevRunMonitor(AsynMessageType.DeviceRunStart);
            }
         
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
