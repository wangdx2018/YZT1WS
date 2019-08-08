using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.BOM2.MessageDispacher;
using AFC.WS.Model.Const;

namespace AFC.WS.BR.SLEMonitorManager
{
    /// <summary>
    /// added by wangdx 20130624 
    /// 
    /// description：报警监视线程管理，通过报警历史查询来记录报警数据。
    /// 
    /// </summary>
    public class AlarmMonitor
    {
       /*
        * 1.get device status from dev_alarm_history_info(1. confirm='01' and update=DateTime.Now)
        * 
        * 2.set lastTime=DateTime.Now
        * 
        * 3. 设置间隔 updateDate，updateTime between last and current  if contrains call alarmMessage to Server 数据库中存在
        * 
        * 4. 如果有内容（响铃+提示）。
        * 
        * 5.通过报警开启和关闭来启动或者关闭线程。
        * 
        * */

        /// <summary>
        /// 报警监听线程
        /// </summary>
        private Thread alarmMonitorThread = null;

        /// <summary>
        /// 上次运行时间
        /// </summary>
        private DateTime lastDateTime;

        /// <summary>
        /// 系统第一次调用SQL
        /// </summary>
        public const string INIT_SQL = "select * from dev_status_alarm_history t where t.is_confirm='01' and t.update_date='{0}' order by t.update_time desc";

        /// <summary>
        /// 每次调用时候通过between and来获得数据
        /// </summary>
        public const string COM_SQL = "select * from dev_status_alarm_history t where t.is_confirm='01' and t.update_date||t.update_time between '{0}' and '{1}'";

        /// <summary>
        /// 报警轮训时间间隔
        /// </summary>
        private readonly int ALARM_MONITOR_INTERVAL = 10 * 1000;

        public int StartAlarmMonitor()
        {
            alarmMonitorThread = new Thread(new ThreadStart(MonitorStart));
            try
            {
                alarmMonitorThread.Start();
                WriteLog.Log_Info("alarm monitor thread start successfully");
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                WriteLog.Log_Error("Start alarmMontior thread failed");
                return -1;
            }
        }

        /// <summary>
        /// 如果有变化发送消息
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int SendMsg()
        {
            Message msg = new Message();
            msg.MessageType = AsynMessageType.AlarmMonitor;
            MessageManager.SendMessasge(msg);
            return 0;
        }

        /// <summary>
        ///线程函数 
        /// </summary>
        private void MonitorStart()
        {
            bool isFirstStart = true;

              List<DevStatusAlarmHistory> list ;
            while (true)
            {
                if (isFirstStart)
                {
                    list = DBCommon.Instance.GetTModelValue<DevStatusAlarmHistory>(string.Format(INIT_SQL, DateTime.Now.ToString("yyyyMMdd")));
                    if (list!=null&&
                        list.Count> 0)
                    {
                        SendMsg();
                    }
                    isFirstStart = false;
                }
                else
                {
                   list= DBCommon.Instance.GetTModelValue<DevStatusAlarmHistory>(string.Format(COM_SQL, lastDateTime.ToString("yyyyMMddHHmmss"), DateTime.Now.ToString("yyyyMMddHHmmss")));
                    if (list!=null&&
                        list.Count > 0)
                    {
                        SendMsg();
                    }
                }
                lastDateTime = DateTime.Now;
                Thread.Sleep(ALARM_MONITOR_INTERVAL);
            }
        }

        /// <summary>
        /// 关闭线程函数
        /// </summary>
        public int StopAlarmMonitor()
        {
            try
            {
                if (alarmMonitorThread == null)
                    return -1;
                this.alarmMonitorThread.Abort();
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                return -1;
            }
        }


        
    }

}
