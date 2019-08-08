using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.ComponentModel;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;
using System.Data;
using AFC.WS.UI.Common;
using System.Windows;
using AFC.WS.UI.Config;

namespace AFC.WS.BR
{
    /// <summary>
    /// BR变量，定义BR中的变量
    /// </summary>
    public class BuinessContext:INotifyPropertyChanged
    {

        /// <summary>
        /// 定义当前登录的操作员
        /// </summary>
        private string currentOperatorId="00000000";
        public static ResourceDictionary langRd;

        public string CurrentOperatorId
        {
            get { return currentOperatorId; }
            set
            {
                currentOperatorId = value;
                OnPropertyChange("CurrentOperatorId");
            }
        }

        /// <summary>
        /// 数据库的连击状态
        /// </summary>
        private bool dbOnLineStatus;

        public bool DbOnLineStatus
        {
            get { return dbOnLineStatus; }
            set
            {
                dbOnLineStatus = value;
                OnPropertyChange("DbOnLineStatus");
            }
        }

        /// <summary>
        /// 网络的连接状态
        /// </summary>
        private bool networkStatus;


        public bool NetworkStatus
        {
            get { return networkStatus; }
            set
            {
                networkStatus = value;
                OnPropertyChange("NetworkStatus");
            }
        }


        private bool onlineStatus;

        public bool OnlineStatus
        {
            set
            {
                this.onlineStatus = value;
                OnPropertyChange("OnlineStatus");
            }
            get { return this.onlineStatus; }
        }
   

        /// <summary>
        /// 是否运营开始
        /// </summary>
        private bool isRunBegin;

        public bool IsRunBegin
        {
            get { return isRunBegin; }
            set
            {
                isRunBegin = value;
                OnPropertyChange("IsRunBegin");
            }
        }

        public string operatorDevId;

        /// <summary>
        /// FTP操作员
        /// </summary>
        private string ftpUser;


        public string FtpUser
        {
            get { return ftpUser; }
            set { ftpUser = value; }
        }

        /// <summary>
        /// FTP密码
        /// </summary>
        private string ftpPwd;

        public string FtpPwd
        {
            get { return ftpPwd; }
            set { ftpPwd = value; }
        }


        private string ftpAddress;


        public string FtpAddress
        {
            set { this.ftpAddress = value; }
            get { return this.ftpAddress; }
        }



        /// <summary>
        /// 当前模式
        /// </summary>
        private byte currentMode;

        /// <summary>
        /// 当前模式
        /// </summary>
        public byte CurrentMode
        {
            get { return currentMode; }
            set { currentMode = value; }
        }


        /// <summary>
        /// 当前运营状态
        /// </summary>
        private int runStatus;

        public int RunStatus
        {
            get { return runStatus; }
            set { runStatus = value; }
        }


        public string GetCurrentStationRunStatus()
        {
            BasiStationInfo staInfo = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            string deviceId = staInfo.device_id;
            string cmd = string.Format("select t.status_value from dev_run_status_detail t where t.status_id='0A01' and t.device_id='{0}'", deviceId);
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                bool res ;
                res=int.TryParse(dt.Rows[0][0].ToString(), out this.runStatus);
                if (!res)
                    throw new Exception("convert error");
                switch (runStatus)
                {
                    case StationRunStatus.RUN_START_SUC:
                        return "运营开始";
                    case StationRunStatus.RUN_STARTING:
                        return "运营开始进行中";
                    case StationRunStatus.RUN_START_FAILED:
                        return "运营开始失败";
                    case StationRunStatus.RUN_END_SUC:
                        return "运营结束成功";
                    case StationRunStatus.RUN_ENDING:
                        return "运营结束中";
                    case StationRunStatus.RUN_END_FAILED:
                        return "运营结束失败";
                    default:
                        return "未知运营状态";
                }
            }
            catch (Exception ex)
            {
                return "未知运营状态";
            }
        }

        /// <summary>
        /// 当前报警状态
        /// </summary>
        private int alarmCurrentStatus;

        public int AlarmCurrentStatus
        {
            get { return alarmCurrentStatus; }
            set { alarmCurrentStatus = value; }
        }

        public string GetAlarmStatus()
        {
            string cmd = string.Format("select t.param_value from basi_run_param_info t where t.param_code = '0603'");
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                bool res;
                res = int.TryParse(dt.Rows[0][0].ToString(), out this.alarmCurrentStatus);
                if (!res)
                    throw new Exception("convert error");
                switch (alarmCurrentStatus)
                {
                    case AlarmStatus.ALARM_STATUS_CLOSE:
                        return "报警关闭";
                    case AlarmStatus.ALARM_STATUS_OPEN:
                        return "报警打开";
                    default:
                        return "未知报警状态";
                }
            }
            catch (Exception ex)
            {
                return "未知报警状态";
            }
        }


        /// <summary>
        /// 得到车站的当前运营模式
        /// </summary>
        /// <returns>返回车站运营模式代码</returns>
        public string GetRunStatus()
        {
            return string.Empty;
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 判断程序是否已经启动了。
        /// </summary>
        public void JudgeCustomApplicationIsStart()
        {
            try
            {
                string name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                string clientName = GetClientName();
                System.Diagnostics.Process[] app = System.Diagnostics.Process.GetProcessesByName(name);

                if (app.Length > 1)
                {
                    Wrapper.ShowDialog("[" + clientName + "]" + "程序已经启动。");
                    System.Environment.Exit(0);
                }//End if;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 获取客户端名称
        /// </summary>
        /// <returns></returns>
        public string GetClientName()
        {
            string clientName = "";
            switch (SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
            {
                case "SCWS":
                    clientName = "车站工作站";
                    break;
                case "LCWS":
                    clientName = "线路中心工作站";
                    break;
                case "MCWS":
                    clientName = "维修中心工作站";
                    break;
                case "TCWS":
                    clientName = "票务管理中心工作站";
                    break;
                case "AGWS":
                    clientName = "站区工作站";
                    break;
                default:
                    break;
            }
            return clientName;
        }
       
    }
}
