using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    using AFC.WS.UI.Common;

    public class UpdateMaintainFaultRptStatus:  IAction
    {
        #region IAction 成员

        string fault_doc_id = string.Empty;
        string line_name = string.Empty;
        string station_cn_name = string.Empty;
        string maintenance_level = string.Empty;
        string device_id = string.Empty;
        string fault_date = string.Empty;
        string fault_time = string.Empty;
        string input_operator_id = string.Empty;
        string assign_operator_id = string.Empty;
        string fault_status = string.Empty;
        string dev_part_cn_name = string.Empty;
        string maintenance_area_id = string.Empty;
        string remark = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("fault_doc_id")).value != null)
            {
                fault_doc_id = actionParamsList.Single(temp => temp.bindingData.Equals("fault_doc_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("line_name")).value != null)
            {
                line_name = actionParamsList.Single(temp => temp.bindingData.Equals("line_name")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value != null)
            {
                station_cn_name = actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("maintenance_level")).value != null)
            {
                maintenance_level = actionParamsList.Single(temp => temp.bindingData.Equals("maintenance_level")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value != null)
            {
                device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("fault_date")).value != null)
            {
                fault_date = actionParamsList.Single(temp => temp.bindingData.Equals("fault_date")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("fault_time")).value != null)
            {
                fault_time = actionParamsList.Single(temp => temp.bindingData.Equals("fault_time")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("input_operator_id")).value != null)
            {
                input_operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("input_operator_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("assign_operator_id")).value != null)
            {
                assign_operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("assign_operator_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("fault_status")).value != null)
            {
                fault_status = actionParamsList.Single(temp => temp.bindingData.Equals("fault_status")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("dev_part_cn_name")).value != null)
            {
                dev_part_cn_name = actionParamsList.Single(temp => temp.bindingData.Equals("dev_part_cn_name")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("maintenance_area_id")).value != null)
            {
                maintenance_area_id = actionParamsList.Single(temp => temp.bindingData.Equals("maintenance_area_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("remark")).value != null)
            {
                remark = actionParamsList.Single(temp => temp.bindingData.Equals("remark")).value.ToString();
            }

            if (string.IsNullOrEmpty(line_name))
            {
                Wrapper.ShowDialog("请填写故障线路。");
                return false;
            }
            if (string.IsNullOrEmpty(station_cn_name))
            {
                Wrapper.ShowDialog("请填写故障车站。");
                return false;
            }
            if (string.IsNullOrEmpty(maintenance_level))
            {
                Wrapper.ShowDialog("请填写故障级别。");
                return false;
            }
            if (string.IsNullOrEmpty(device_id))
            {
                Wrapper.ShowDialog("请填写设备编号。");
                return false;
            }
            else if (device_id.Length < 8)
            {
                Wrapper.ShowDialog("请填写完整的设备编号。");
                return false;
            }
            if (string.IsNullOrEmpty(fault_date))
            {
                Wrapper.ShowDialog("请填写故障日期。");
                return false;
            }
            if (string.IsNullOrEmpty(fault_time))
            {
                Wrapper.ShowDialog("请填写故障时间。");
                return false;
            }
            if (string.IsNullOrEmpty(input_operator_id))
            {
                Wrapper.ShowDialog("请填写录入操作员。");
                return false;
            }
            if (string.IsNullOrEmpty(fault_status))
            {
                Wrapper.ShowDialog("请填写故障状态。");
                return false;
            }
            if (string.IsNullOrEmpty(dev_part_cn_name))
            {
                Wrapper.ShowDialog("请填写设备部件名称。");
                return false;
            }
            if (string.IsNullOrEmpty(maintenance_area_id))
            {
                Wrapper.ShowDialog("请填写工区编号。");
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
            int res1 = 0;

            MaintainFaultRptStatus info = new MaintainFaultRptStatus();
            info.fault_doc_id = fault_doc_id;
            info.line_id = BuinessRule.GetInstace().GetLineInfoByName(line_name).line_id;
            info.station_id = BuinessRule.GetInstace().GetStationInfoByName(station_cn_name).station_id;
            info.maintenance_level = maintenance_level;
            info.device_id = device_id;
            info.fault_date = fault_date;
            info.fault_time = fault_time;
            info.input_operator_id = input_operator_id;
            info.input_date = DateTime.Now.ToString("yyyyMMdd");
            info.input_time = DateTime.Now.ToString("HHmmss");
            info.assign_operator_id = assign_operator_id;
            info.fault_status = fault_status;
            info.dev_part_id = BuinessRule.GetInstace().GetBasiDevPartIdInfoByName(dev_part_cn_name).dev_part_id; 
            info.maintenance_area_id = maintenance_area_id;
            info.remark = remark;
            //将故障单指派给维修员
            //res = DBCommon.Instance.InsertTable(info, "maintain_fault_rpt_status");
            res = DBCommon.Instance.UpdateTable(info, "maintain_fault_rpt_status",
            new KeyValuePair<string, string>("fault_doc_id", fault_doc_id));

            int seq = 0;
            MaintainRptTrackInfo info1 = new MaintainRptTrackInfo();
            string seqNum = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            info1.fault_track_id = seqNum.ToInt32();
            info1.line_id = BuinessRule.GetInstace().GetLineInfoByName(line_name).line_id;
            info1.station_id = BuinessRule.GetInstace().GetStationInfoByName(station_cn_name).station_id;
            info1.fault_doc_id = fault_doc_id;
            info1.status = "02";
            info1.remark = remark;
            info1.update_date = DateTime.Now.ToString("yyyyMMdd");
            info1.update_time = DateTime.Now.ToString("HHmmss");
            //插入故障单跟踪信息表
            res1 = DBCommon.Instance.InsertTable(info1, "maintain_rpt_track_info");

            if (res == 1 && res1==1)
            {
                Wrapper.ShowDialog("将故障单指派给维修员成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Maintain_Fault_Rpt_Status_Update, "0", "故障单信息添加成功");
                Util.DataBase.Commit();
                return null;
            }
            else if (res != 1 && res1 == 1)
            {
                Wrapper.ShowDialog("将故障单指派给维修员失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Maintain_Fault_Rpt_Status_Update, "1", "故障单信息添加失败");
                Util.DataBase.Rollback();
                return null;
            }
            else if (res == 1 && res1 != 1)
            {
                Wrapper.ShowDialog("将故障单指派给维修员失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Maintain_Fault_Rpt_Status_Update, "1", "故障单跟踪信息添加失败");
                Util.DataBase.Rollback();
                return null;
            }
            else if (res != 1 && res1 != 1)
            {
                Wrapper.ShowDialog("将故障单指派给维修员失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Maintain_Fault_Rpt_Status_Update, "1", "故障单信息、故障单跟踪信息添加失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}