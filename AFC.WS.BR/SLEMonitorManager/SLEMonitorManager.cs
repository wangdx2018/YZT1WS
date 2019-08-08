using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AFC.WS.BR.SLEMonitorManager
{
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    

    /// <summary>
    /// added by wangdx 20110510
    /// 
    /// 设备监控管理器
    /// </summary>
    public class SLEMonitorManager
    {

        /// <summary>
        /// 设备监控图列表
        /// </summary>
        public SleImageCfgCollection imageCollection = new SleImageCfgCollection();

        /// <summary>
        /// 监听线程目前测试为5s发送一个同步消息
        /// </summary>
        private Thread monitorThread = null;

        /// <summary>
        /// 启动设备监视的监听
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        public int StartMonitorDevice(string currentStationId)
        {
                monitorThread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(30000);
                        SendAsynMessage(currentStationId);
                    }
                }));
                monitorThread.Name = "DevInterval Thread";
                monitorThread.Start();
            
            return 0;
        }

        /// <summary>
        /// 关闭设备监听线程
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        public int ShutdownMonitorDevice()
        {
            if (monitorThread == null)
            {
                WriteLog.Log_Error("plase call start dev monitor thread first!");
                return -1;
            }
            try
            {
                monitorThread.Abort();
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// 发送异步消息
        /// </summary>
        public void SendAsynMessage(string currentStationId)
        {
            Message msg = new Message();
            msg.MessageType = AsynMessageType.DeviceMonitor;
            msg.Content = currentStationId;
            MessageManager.SendMessasge(msg);
        }


        /// <summary>
        /// 2013年5月29日根据马晓春要求增加针对设备状态的配置管理功能
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="statusValue"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateDevStatusLogStatus(string statusId, string statusValue, string status)
        {
            //function 001:
            //todo: 001 :accordding statusid,statusValue get entity data
            //todo: 002  set log_flag=status;
            //todo: update db

            //function 002:
            //string cmd="update basi_status_id_info set log_flag=status where CSS_STATUS_ID=statusid and statusValue";
            string cmd = string.Format("update basi_status_id_info t set t.is_log='{0}' where t.css_status_id = '{1}' and t.css_status_value = '{2}'", status, statusId, statusValue);
            // if succ return 0,else return -1;  
            ///(1)
            //BasiStatusIdInfo bsi = new BasiStatusIdInfo();
            //List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            //DBCommon.Instance.UpdateTable<BasiStatusIdInfo>(bsi,"basi_status_id_info",tempList.ToArray());
            //(2)
            int res = 0;
            Util.DataBase.SqlCommand(out res, cmd);
            if (res != 0)
            {

                return -1;
            }

            //todo: log every steps result

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="statusValue"></param>
        /// <param name="visiable"></param>
        /// <returns></returns>
        public int UpdateDEVStatus(string devType, string statusId, string statusValue, string status)
        {
            //function 001:
            //todo: 001 :accordding statusid,statusValue get entity data
            //todo: 002  set log_flag=status;
            //todo: update db

            //function 002:
            //string cmd="update basi_status_id_info set log_flag=status where CSS_STATUS_ID=statusid and statusValue";
            string cmd = string.Format("update basi_status_id_info t set t.{0}='{1}' where t.css_status_id = '{2}' and t.css_status_value = '{3}'", devType, status, statusId, statusValue);
            // if succ return 0,else return -1;  
            ///(1)
            //BasiStatusIdInfo bsi = new BasiStatusIdInfo();
            //List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            //DBCommon.Instance.UpdateTable<BasiStatusIdInfo>(bsi,"basi_status_id_info",tempList.ToArray());
            //(2)
            int res = 0;
            Util.DataBase.SqlCommand(out res, cmd);
            if (res != 0)
            {

                return -1;
            }

            //todo: log every steps result

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="statusValue"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateDevAlarmStyle(string statusId, string statusValue, string style)
        {
            //function 001:
            //todo: 001 :accordding statusid,statusValue get entity data
            //todo: 002  set log_flag=status;
            //todo: update db

            //function 002:
            //string cmd="update basi_status_id_info set log_flag=status where CSS_STATUS_ID=statusid and statusValue";
            string cmd = string.Format("update basi_status_id_info set is_alarm='{2}' where CSS_STATUS_ID= '{0}'and CSS_STATUS_VALUE='{1}'", statusId, statusValue, style);
            // if succ return 0,else return -1;  
            ///(1)
            //BasiStatusIdInfo bsi = new BasiStatusIdInfo();
            //List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            //DBCommon.Instance.UpdateTable<BasiStatusIdInfo>(bsi,"basi_status_id_info",tempList.ToArray());
            //(2)
            int res = 0;
            Util.DataBase.SqlCommand(out res, cmd);
            if (res != 0)
            {
                //log.Error("cmd" + "[" + cmdMulti + "]" + "Erroe!");
                // Util.DataBase.Rollback();

                return -1;
            }

            //todo: log every steps result

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="statusValue"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateStatusLevel(string statusId, string statusValue, string level)
        {
            //function 001:
            //todo: 001 :accordding statusid,statusValue get entity data
            //todo: 002  set log_flag=status;
            //todo: update db

            //function 002:
            //string cmd="update basi_status_id_info set log_flag=status where CSS_STATUS_ID=statusid and statusValue";
            string cmd = string.Format("update basi_status_id_info set STATUS_LEVEL='{2}' where CSS_STATUS_ID= '{0}'and CSS_STATUS_VALUE='{1}'", statusId, statusValue, level);
            // if succ return 0,else return -1;  
            ///(1)
            //BasiStatusIdInfo bsi = new BasiStatusIdInfo();
            //List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            //DBCommon.Instance.UpdateTable<BasiStatusIdInfo>(bsi,"basi_status_id_info",tempList.ToArray());
            //(2)
            int res = 0;
            Util.DataBase.SqlCommand(out res, cmd);
            if (res != 0)
            {
                //log.Error("cmd" + "[" + cmdMulti + "]" + "Erroe!");
                // Util.DataBase.Rollback();

                return -1;
            }

            //todo: log every steps result

            return 0;
        }
    }
}
