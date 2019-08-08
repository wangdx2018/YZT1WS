using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace AFC.WS.BR
{
    using AFC.WS.BR.LogManager;
    using AFC.WorkStation.DB;
    using AFC.WS.UI.Common;
    using System.Reflection;
    using AFC.WS.BR.CommBuiness;
    using AFC.WS.Model.DB;
    using AFC.WS.BR.Primission;
    using AFC.WS.BR.ParamsManager;
    using AFC.WS.BR.TimeSyn;
    using AFC.WS.BR.RunManager;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.BR.SLEMonitorManager;
using AFC.WS.BR.ReportManager;

    public partial class BuinessRule
    {
        #region [ public Fields ]

        private static BuinessRule br = null;

        private BuinessRule()
        {

        }

        public static BuinessRule GetInstace()
        {
            if (br == null)
            {

                br = new BuinessRule();
                try
                {
                    string value=ConfigurationManager.AppSettings["CommService"];
                    br.commProcess = Activator.CreateInstance(Type.GetType(value)) as ICommProcess;
                    WriteLog.Log_Info(string.Format("Create type=[{0}] successfully", value));
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                }
                try
                {
                    string value = ConfigurationManager.AppSettings["RFIDRW"];
                    br.rfidRw = Activator.CreateInstance(Type.GetType(value)) as IRfidRW;
                    WriteLog.Log_Info(string.Format("Create type=[{0}] successfully", value));
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                }
            }
            return br;
        }

        /// <summary>
        /// 存储业务相关的变量如当前的操作员ID，数据库的连接状态等
        /// </summary>
        public BuinessContext brConext = new BuinessContext();

        /// <summary>
        /// 记录日志的操作类
        /// </summary>
        public LogManager.LogManager logManager = new LogManager.LogManager();

        /// <summary>
        /// 功能管理对象
        /// </summary>
        public FunctionManager functionManager = new FunctionManager();

        /// <summary>
        /// 操作员管理
        /// </summary>
        public OperatorManager operationManager = new OperatorManager();


        /// <summary>
        /// 报警监听线程
        /// </summary>
        public AlarmMonitor alarmMonitor = new AlarmMonitor();

        /// <summary>
        /// 角色管理
        /// </summary>
        public RoleManager roleManager = new RoleManager();

        /// <summary>
        /// 参数管理
        /// </summary>
        public ParaManager paraManager = new ParaManager();

        /// <summary>
        /// 通讯的接口
        /// </summary>
        public ICommProcess commProcess=null;

        /// <summary>
        /// RFID读写器变量
        /// </summary>
        public IRfidRW rfidRw = null;


        /// <summary>
        /// 时钟管理
        /// </summary>
        public TimeSynManager tsm = new TimeSynManager();


        /// <summary>
        /// 报表管理
        /// </summary>
        public ReportManagerHelper rptManager = new ReportManagerHelper();

        /// <summary>
        /// 运营管理
        /// </summary>
        public AFC.WS.BR.RunManager.RunManager rm = new AFC.WS.BR.RunManager.RunManager();

        /// <summary>
        /// 票箱管理
        /// </summary>
        public AFC.WS.BR.TickBoxManager.TickBoxManager tickMan = new AFC.WS.BR.TickBoxManager.TickBoxManager();


        /// <summary>
        /// 设备监控业务实体
        /// </summary>
        public SLEMonitorManager.SLEMonitorManager sleMonitor = new AFC.WS.BR.SLEMonitorManager.SLEMonitorManager();

        #endregion

        #region [public method]

        /// <summary>
        /// 获得数据的连击状态
        /// </summary>
        /// <returns>获得数据库的连接状态</returns>
        public bool ConnectDB(string dbConnectStr)
        {
            if (string.IsNullOrEmpty(dbConnectStr))
                return false;
            DBO dbo = new DBO(dbConnectStr);
       
            int res = dbo.SqlConnect();
            if (res != 0)
                return false;
            dbo.SqlCommand(out res, "select count(*) from dual");
            return res == 0;
          
        }

        /// <summary>
        /// 获取设置操作员ID，登出后赋值为00000000.不能赋值为Empty
        /// </summary>
        public string OperatorId
        {
            set
            {
                this.brConext.CurrentOperatorId = value;
                this.commProcess.currentLogInOperatorId = value.ConvertHexStringToUint();//将通讯层的字符串设置赋值
            }
            get
            {
                return this.brConext.CurrentOperatorId;
            }
        }

        #endregion
    }
}
