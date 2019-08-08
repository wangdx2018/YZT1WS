﻿using System;
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
using AFC.WS.UI.BR.Data;

namespace AFC.WS.ModelView.Actions.MaintainAreaManager
{
    public class BasiMaintainAreaInfoUpdate:  IAction
    {
        #region IAction 成员

        string MaintainAreaID = string.Empty;
        string MaintainAreaName = string.Empty;
        string MaintainAreaAddress = string.Empty;
        string MaintainAreaContector = string.Empty;
        string MaintainAreaPhone = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaID")).value != null)
            {
                MaintainAreaID = actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaName")).value != null)
            {
                MaintainAreaName = actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaAddress")).value != null)
            {
                MaintainAreaAddress = actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaAddress")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaContector")).value != null)
            {
                MaintainAreaContector = actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaContector")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaPhone")).value != null)
            {
                MaintainAreaPhone = actionParamsList.Single(temp => temp.bindingData.Equals("MaintainAreaPhone")).value.ToString();
            }
            if (string.IsNullOrEmpty(MaintainAreaID))
            {
                Wrapper.ShowDialog("请填写工区编码。");
                return false;
            }
            if (string.IsNullOrEmpty(MaintainAreaName))
            {
                Wrapper.ShowDialog("请填写工区名称。");
                return false;
            }
            if (string.IsNullOrEmpty(MaintainAreaAddress))
            {
                Wrapper.ShowDialog("请填写工区地址。");
                return false;
            }
            if (string.IsNullOrEmpty(MaintainAreaName))
            {
                Wrapper.ShowDialog("请填写工区联系人。");
                return false;
            }
            if (string.IsNullOrEmpty(MaintainAreaAddress))
            {
                Wrapper.ShowDialog("请填写工区联系电话。");
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
            BasiMaintainAreaInfo info = new BasiMaintainAreaInfo();
            info.maintain_area_id = MaintainAreaID;
            info.maintain_area_name = MaintainAreaName;
            info.maintain_area_address = MaintainAreaAddress;
            info.dute_person = MaintainAreaContector;
            info.phone = MaintainAreaPhone;
            info.status = "00";
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");

            //更新设备部件名称
            res = DBCommon.Instance.UpdateTable(info, "basi_maintain_area_info", new KeyValuePair<string, string>("maintain_area_id", MaintainAreaID));
      
            if (res == 1)
            {
                Wrapper.ShowDialog("更新供应商信息成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Basi_Provider_Info_Update, "0", "更新供应商信息成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("更新供应商信息失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Basi_Provider_Info_Update, "1", "更新供应商信息失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}