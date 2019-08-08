using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.BR.Primission;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.DeviceMonitor
{
    public class AgUpsManager
    {
        public List<BasiDevInfo> getDeviceByUpsID(string upsId)
        {

            string cmd = string.Format("select * from basi_dev_info t  join "
                + "(select * from dev_ups_map) d on t.device_id=d.device_id "
                + "where d.ups_id='{0}' and t.station_id='{1}' and t.device_type='{2}' order by t.device_id", upsId, SysConfig.GetSysConfig().LocalParamsConfig.StationCode, "06");
            try
            {
                List<BasiDevInfo> funcList = DBCommon.Instance.GetTModelValue<BasiDevInfo>(cmd);
                return funcList;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return null;
            }

        }
        public List<BasiDevInfo> GetAllNormalDeviceInfos()
        {

            int res = 0;
            string cmd = string.Format("select * from basi_dev_info t where t.device_id not in(select t.device_id from dev_ups_status t) and t.device_id not in(select t.device_id from dev_ups_map t) and t.station_id='{0}' and t.device_type='{1}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode, "06");

            try
            {
                List<BasiDevInfo> funcList = DBCommon.Instance.GetTModelValue<BasiDevInfo>(cmd);
                return funcList;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return null;
            }
        }
        public int DeleteUpsToDeviceRelaction(string upsId)
        {
            int res = 0;
            string delSql = string.Format("delete dev_ups_map t where t.ups_id='{0}'", upsId);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                if (res == 0)
                {
                    return -1;
                }
                else
                {
                    Util.DataBase.Commit();
                    PrimissionManager pm = new PrimissionManager();
                    pm.UpdateChangeParams();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                Util.DataBase.Rollback();
                return -1;
            }
        }
        public int AddUpsToDeviceRelaction(string upsId, string deviceId)
        {
            if (string.IsNullOrEmpty(upsId) || string.IsNullOrEmpty(deviceId))
                return -1;

            DevUpsMap info = new DevUpsMap();
            info.device_id = deviceId;
            info.ups_id = upsId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.InsertTable(info, "dev_ups_map");
                if (res != 1)
                {
                    // Util.DataBase.Rollback();
                    return -1;
                }
                else
                {
                    PrimissionManager pm = new PrimissionManager();
                    pm.UpdateChangeParams();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                //   Util.DataBase.Rollback();
                return -1;
            }
        }
        public int UpdateDateTimeInfo(string upsId, List<QueryCondition> actionParamsList)
        {
            if (string.IsNullOrEmpty(upsId))
                return -1;

            DevUpsStatus info = new DevUpsStatus();
            info.ups_id = upsId;
            info.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("t.device_id")).value.ToString();
            info.power_percent = actionParamsList.Single(temp => temp.bindingData.Equals("t.power_percent")).value.ToString();
            info.power_status = actionParamsList.Single(temp => temp.bindingData.Equals("t.power_status")).value.ToString();
            info.ups_status = actionParamsList.Single(temp => temp.bindingData.Equals("t.ups_status")).value.ToString();
            info.is_off = actionParamsList.Single(temp => temp.bindingData.Equals("t.is_off")).value.ToString();
            info.shut_date = actionParamsList.Single(temp => temp.bindingData.Equals("t.shut_date")).value.ToString();
            info.shut_time = actionParamsList.Single(temp => temp.bindingData.Equals("t.shut_time")).value.ToString();
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.operator_id = BuinessRule.GetInstace().OperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.UpdateTable(info, "dev_ups_status", new KeyValuePair<string, string>("UPS_ID", upsId));
                if (res != 1)
                {
                    // Util.DataBase.Rollback();
                    return -1;
                }
                else
                {
                    PrimissionManager pm = new PrimissionManager();
                    pm.UpdateChangeParams();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                //   Util.DataBase.Rollback();
                return -1;
            }
        }
    }
}
