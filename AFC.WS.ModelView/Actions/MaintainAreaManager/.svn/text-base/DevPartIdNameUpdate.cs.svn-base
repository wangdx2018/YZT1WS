using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.RfidRW;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Windows;
using AFC.WS.BR.LogManager;
using AFC.WS.BR.TickBoxManager;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.MaintainAreaManager
{
    public class DevPartIdNameUpdate:  IAction
    {
        #region IAction 成员

        string DevPartId = string.Empty;
        string  DevType = string.Empty;
        string DevPartIdName = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("DevPartId")).value != null)
            {
                DevPartId = actionParamsList.Single(temp => temp.bindingData.Equals("DevPartId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("DevType")).value != null)
            {
                DevType = actionParamsList.Single(temp => temp.bindingData.Equals("DevType")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("DevPartIdName")).value != null)
            {
                DevPartIdName = actionParamsList.Single(temp => temp.bindingData.Equals("DevPartIdName")).value.ToString();
            }
            if (string.IsNullOrEmpty(DevPartId))
            {
                Wrapper.ShowDialog("请填写设备部件编码。");
                return false;
            }
            if (string.IsNullOrEmpty(DevType))
            {
                Wrapper.ShowDialog("请填写所属设备类型。");
                return false;
            }
            if (string.IsNullOrEmpty(DevPartIdName))
            {
                Wrapper.ShowDialog("请填写设备部件名称。");
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //开启事务
            Util.DataBase.BeginTransaction();
            int res = 0;
            BasiDevPartIdInfo info = new BasiDevPartIdInfo();
            info.dev_part_id = DevPartId;
            info.device_type = DevType;
            info.dev_part_cn_name = DevPartIdName;

            //更新设备部件名称
            res = DBCommon.Instance.UpdateTable(info, "basi_dev_part_id_info", new KeyValuePair<string, string>("dev_part_id", DevPartId));
      
            if (res == 1)
            {
                Wrapper.ShowDialog("更新设备部件名称成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Dev_PartId_Name_Update, "0", "更新设备部件名称成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("更新设备部件名称失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Dev_PartId_Name_Update, "1", "更新设备部件名称失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}