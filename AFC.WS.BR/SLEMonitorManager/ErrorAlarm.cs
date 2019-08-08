using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.SLEMonitorManager
{
    using AFC.WS.UI.Common;
    using System.Data;
    using AFC.WS.Model.DB;

    /// <summary>
    /// added by wangdx  date:20110708
    /// 
    /// 报警方式的设置UI
    /// </summary>
    public class ErrorAlarm
    {
        /// <summary>
        /// 通过报警级别得到报警方式
        /// </summary>
        /// <param name="value">
        /// 报警级别 
        /// 01报警
        /// 02 故障,
        /// 03 通讯终止
        /// </param>
        /// <returns>返回报警方式集合</returns>
        public List<string> GetAlarmStyle(string value)
        {
            List<string> list = new List<string>();
            string cmd = string.Format("select t.alarm_style from dev_status_alarm_cfg t where t.device_id='{0}' and t.run_status='{1}'",
                SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode,
                value);
            DataTable dt=DBCommon.Instance.GetDatatable(cmd);
            if (dt == null)
                return new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i][0].ToString());
            }
            return list;
        }

        /// <summary>
        /// 通过状态ID及状态值得到报警方式
        /// </summary>
        /// <param name="statusID">
        /// <param name="statusValue">
        /// </param>
        /// <returns>返回报警方式</returns>
        public string GetStatusAlarmStyle(string statusID, string statusValue)
        {
            string cmd = string.Format("select t.is_alarm from basi_status_id_info t where t.css_status_id = '{0}' and t.css_status_value = '{1}'",
                statusID, statusValue);

            DataTable dt = DBCommon.Instance.GetDatatable(cmd);

            if (dt != null &&
                dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "00";
        }

        /// <summary>
        /// 通过状态ID,状态值,设备ID
        /// </summary>
        /// <param name="statusID">
        /// <param name="statusValue">
        /// <param name="devID">
        /// </param>
        /// <returns>返回状态ID是否属于此设备</returns>
        public string GetStatusIsDev(string statusID, string statusValue, string devID)
        {
            string cmd = string.Format("select t.* from basi_status_id_info t where t.css_status_id = '{0}' and t.css_status_value = '{1}' "
                + " and (substr('{2}',5,2)||t.bom = '0200' "
                + " or substr('{2}',5,2)||t.tvm = '0100' "
                + " or substr('{2}',5,2)||t.agm = '0600' "
                + " or substr('{2}',5,2)||t.eqm = '0400') ",
                statusID, statusValue, devID);

            DataTable dt = DBCommon.Instance.GetDatatable(cmd);

            if (dt != null &&
                dt.Rows.Count > 0)
            {
                return "00";
            }
            return "01";
        }

        /// <summary>
        /// 更新报警方式
        /// </summary>
        /// <param name="alarmLevel">报警级别</param>
        /// <param name="alarmStyle">报警方式</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int UpdateAlarmStyle(string alarmLevel,List<string>alarmStyle )
        {
            //todo:001 delete all alarm style
            //todo:inert new alarm styles
            string cmd = string.Format("delete from dev_status_alarm_cfg t where t.device_id='{0}' and t.run_status='{1}'",
                SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode,
                alarmLevel);
            int res=0;

            Util.DataBase.BeginTransaction();
           Util.DataBase.SqlCommand(out res, cmd);
           if (res != 0)
           {
               Util.DataBase.Rollback();
               WriteLog.Log_Error("delete dev_status_alarm_cfg device_id=[" + SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode + "],runStaus=[" + alarmLevel + "]");
               return res;
           }

           for (int i = 0; i < alarmStyle.Count; i++)
           {
               DevStatusAlarmCfg cfg = new DevStatusAlarmCfg();
               cfg.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
               cfg.run_status = alarmLevel;
               cfg.alarm_style = alarmStyle[i];
               res=DBCommon.Instance.InsertTable<DevStatusAlarmCfg>(cfg, "dev_status_alarm_cfg");
               if (res != 1)
               {
                   Util.DataBase.Rollback();
                   WriteLog.Log_Error("insert error alarmStyle=[" + alarmStyle[i] + "]");
                   return res;
               }
           }
           Util.DataBase.Commit();
           return 0;
        }
    }
}
