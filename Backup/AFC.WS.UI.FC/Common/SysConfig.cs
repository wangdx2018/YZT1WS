using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.IO.Ports;

namespace AFC.WS.UI.Common
{
    using AFC.BOM2.Common;
    using AFC.WS.UI.Config;
    

    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110222
    /// 代码功能：系统的配置文件可以序列化.该类的访问方式为单件进行访问。
    /// 修订记录：20110314 增加了ServerDeviceid
    /// 
    /// </summary>
    [Serializable]
    public class SysConfig
    {
        /// <summary>
        /// 系统配置文件
        /// </summary>
        private static SysConfig sysConfig = null;
         
        /// <summary>
        /// 序列化对象
        /// </summary>
        private static XmlSerializer serializer = null;

        /// <summary>
        /// 锁变量
        /// </summary>
        private static object mutex = new object();

        private static object changeMutex = new object();

        private SysConfig()
        {

        }

        //--->写配置文件
        /// <summary>
        /// 更改配置文件,重新序列化参数
        /// </summary>
        public int WrtieSysConfigFile()
        {
            try
            {
                lock (mutex)
                {
                    using (Stream s = File.Open(@".\Config\SysConfig.xml", FileMode.OpenOrCreate))
                    {
                        s.SetLength(0L);
                        serializer = XmlSerializer.FromTypes(new Type[] { typeof(SysConfig) })[0];
                        serializer.Serialize(s, this);
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                WriteLog.Log_Debug(e.Message);
                return -1;
            }

        }


        public int WrtieChangeSysConfigFile(SysConfig changSysConfig)
        {
            try
            {
                lock (changeMutex)
                {
                    using (Stream s = File.Open(@".\Config\SysConfig.xml", FileMode.OpenOrCreate))
                    {
                        s.SetLength(0L);
                        serializer = XmlSerializer.FromTypes(new Type[] { typeof(SysConfig) })[0];
                        serializer.Serialize(s, changSysConfig);
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                WriteLog.Log_Debug(e.Message);
                return -1;
            }

        }
        //--->得到系统配置信息类
        /// <summary>
        /// 得到该类的对象，初始化时传递将文件名传递进去初始化，以后调用不必传递参数
        /// </summary>
        /// <param name="fileName">初始化时传递将文件名传递进去初始化，以后调用不必传递参数</param>
        /// <returns>返回配置信息类</returns>
        public static SysConfig GetSysConfig(params string[] fileName)
        {
            if (sysConfig != null)
                return sysConfig;
            serializer = XmlSerializer.FromTypes(new Type[] { typeof(SysConfig) })[0];
            try
            {
                if (fileName != null && fileName.Length != 0)
                {
                    using (Stream s = File.Open(fileName[0], FileMode.OpenOrCreate))
                    {
                        sysConfig = serializer.Deserialize(s) as SysConfig;
                        return sysConfig;
                    }
                }
                throw new Exception("请设置SysConfig的初始化的文件名");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 本机参数
        /// </summary>
        private LocalParamsConfig locationParams = new LocalParamsConfig();

        /// <summary>
        /// 通讯参数
        /// </summary>
        private CommParamsConfig commParamsConfig = new CommParamsConfig();


        /// <summary>
        /// 客流参数
        /// </summary>
        PassengerFlowParamCfg _PassengerFlowParamCfg = new PassengerFlowParamCfg();

        /// <summary>
        /// RFID读写器配置
        /// </summary>
        private SerialDeviceConfig rfidRWConfig = new SerialDeviceConfig();

        /// <summary>
        /// FTP路径
        /// </summary>
        private FtpFileDirectoryConfig _FtpDirectory = new FtpFileDirectoryConfig();


        //--->RFID RW参数配置
        /// <summary>
        /// RFID RW 参数配置
        /// </summary>
        public SerialDeviceConfig RFIDRWConfig
        {
            set { this.rfidRWConfig = value; }
            get { return this.rfidRWConfig; }
        }

        //-->本机参数配置信息
        /// <summary>
        /// 本机参数配置
        /// </summary>
        public LocalParamsConfig LocalParamsConfig
        {
            set { this.locationParams = value; }

            get { return this.locationParams; }
        }

        //--->通讯参数配置
        /// <summary>
        /// 通讯参数配置
        /// </summary>
        public CommParamsConfig CommParamsConfig
        {
            set { this.commParamsConfig = value; }
            get { return this.commParamsConfig; }
        }

        /// <summary>
        /// 设置客流参数。
        /// </summary>
        public PassengerFlowParamCfg PassengerFlowParamCfg
        {
            get { return _PassengerFlowParamCfg; }
            set { _PassengerFlowParamCfg = value; }
        }

        private ReportConfig reportCfg;

        public ReportConfig ReportCfg
        {
            get { return this.reportCfg; }
            set { this.reportCfg = value; }
        }

        /// <summary>
        /// 上传到服务器的目录。
        /// </summary>
        public FtpFileDirectoryConfig FtpDirectory
        {
            get { return _FtpDirectory; }
            set { _FtpDirectory = value; }
        }
    }

    #region LocalParamsConfig
    //-->本机参数定义
    /// <summary>
    /// 本机参数定义
    /// </summary>
    [Serializable]
    public class LocalParamsConfig
    {
        #region [private field]

        //-->线路
        /// <summary>
        /// 线路
        /// </summary>
        private string lineCode;

        //-->车站
        /// <summary>
        /// 车站
        /// </summary>
        private string stationCode;

        //-->设备编码
        /// <summary>
        /// 设备编码
        /// </summary>
        private string deviceCode;
        //-->设备类型
        /// <summary>
        /// 设备类型
        /// </summary>
        private string deviceType;

        //-->皮肤文件存放的目录
        /// <summary>
        /// 皮肤文件存放的目录
        /// </summary>
        private string skinPath;

        //--->数据源文件存放的目录
        /// <summary>
        /// 数据源存放的路径名
        /// </summary>
        private string dataSourceDirectory;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string dbConnectionSting;

        /// <summary>
        /// 系统名称
        /// </summary>
       private string systemName;

        /// <summary>
        /// 城市编码
        /// </summary>
       private string cityCode;

       /// <summary>
       /// 本机IP
       /// </summary>
       private string localIPaddress;

        /// <summary>
        /// 是否输出到控制台
        /// </summary>
       private bool isOutPutConsole;

        /// <summary>
        /// 打印机名称
        /// </summary>
       private string printerName;

        /// <summary>
        /// 语言
        /// </summary>
       private string language;

        /// <summary>
        /// 一卡通代码
        /// </summary>
       private string yktCode;

        /// <summary>
        /// 一票通代码
        /// </summary>
       private string yptCode;

        /// <summary>
        /// 显示状态名称
        /// </summary>
       private int statusValueName;

       
        #endregion

       #region public properties

       /// <summary>
        /// 打印机的名称。
        /// </summary>
        public string PrinterName
        {
            get { return this.printerName; }
            set { this.printerName = value; }
        }

        /// <summary>
        /// 是否输出控制台
        /// </summary>
        public bool IsOutPutConsole
        {
            get { return isOutPutConsole; }
            set { isOutPutConsole = value; }
        }

        /// <summary>
        ///本机IP地址
        /// </summary>
        public string LocalIPaddress
        {
            get { return this.localIPaddress; }
            set { this.localIPaddress = value; }
        }

        /// <summary>
        /// 城市代码
        /// </summary>
        public string CityCode
        {
            get { return this.cityCode; }
            set { cityCode = value; }
        }

        /// <summary>
        /// 系统名称:LCWS、MCWS、SCWS、TCWS
        /// </summary>
        public string SystemName
        {
            get { return this.systemName; }
            set { this.systemName = value; }
        }

        //-->线路
        /// <summary>
        /// 线路
        /// </summary>
        public string LineCode
        {
            set { this.lineCode = value; }
            get { return this.lineCode; }
        }

        //-->车站
        /// <summary>
        /// 车站
        /// </summary>
        public string StationCode
        {
            set { this.stationCode = value; }
            get { return this.stationCode; }
        }

        //-->设备编码
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode
        {
            set { this.deviceCode = value; }
            get { return this.deviceCode; }
        }

        //-->设备类型
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }

        /// <summary>
        /// 数据源配置文件存放的目录
        /// </summary>
        public string DataSourceDirectory
        {
            set { this.dataSourceDirectory = value; }
            get { return this.dataSourceDirectory; }
        }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        public string DBConnectionString
        {
            get { return this.dbConnectionSting; }
            set { this.dbConnectionSting = value; }
        }

        /// <summary>
        /// 皮肤文件存放的目录
        /// </summary>
        public string SkinPath
        {
            set { this.skinPath = value; }
            get { return this.skinPath; }
        }

        /// <summary>
        /// 一票通供应商代码
        /// </summary>
        public string YPTCode
        {
            get { return this.yptCode; }
            set { this.yptCode = value; }
        }

        /// <summary>
        /// 一卡通供应商代码
        /// </summary>
        public string YKTCode
        {
            get { return this.yktCode; }
            set { this.yktCode = value; }
        }

        /// <summary>
        /// 语言
        /// </summary>
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

       #endregion


        /// <summary>
        ///显示状态值名称
        ///0:   显示CSSStatusName
        ///1:   显示具体线路的状态名称
        /// </summary>
        public int StatusValueName
        {
            get { return this.statusValueName; }
            set { this.statusValueName = value; }
        }
    }
    #endregion


    [Serializable]
    public class FtpFileDirectoryConfig
    {
        string _DealDataFilePath;
        string _UpDataFilePath;
        /// <summary>
        ///处理数据文件存放到服务器的目录
        /// </summary>
        public string DealDataFilePath
        {
            get { return _DealDataFilePath; }
            set { _DealDataFilePath = value; }
        }
        /// <summary>
        /// 上传的参数文件存放到服务器的目录。
        /// </summary>
        public string UpDataFilePath
        {
            get { return _UpDataFilePath; }
            set { _UpDataFilePath = value; }
        }
    }

    #region CommParamsConfig
    /// <summary>
    /// 通讯相关的参数配置
    /// </summary>
    [Serializable]
    public class CommParamsConfig
    {
        /// <summary>
        /// sc IP地址
        /// </summary>
        private string scIpAddress;

        /// <summary>
        /// SC 端口号
        /// </summary>
        private int scPort;

        /// <summary>
        /// ftp 操作员
        /// </summary>
        private string ftpUserName;


        /// <summary>
        /// SC的设备ID
        /// </summary>
        private string scDeviceId;


        /// <summary>
        /// ftp 密码
        /// </summary>
        private string ftpPwd;

        /// <summary>
        /// SC IP地址
        /// </summary>
        public string ScIPAddress
        {
            set { this.scIpAddress = value; }
            get { return this.scIpAddress; }
        }

        /// <summary>
        /// SC 端口号
        /// </summary>
        public int ScPort
        {
            set { this.scPort = value; }
            get { return this.scPort; }
        }

        /// <summary>
        /// FTP 操作员
        /// </summary>
        public string FtpUserName
        {
            set { this.ftpUserName = value; }
            get { return this.ftpUserName; }
        }

        /// <summary>
        /// FTP 密码
        /// </summary>
        public string FtpUserPwd
        {
            set { this.ftpPwd = value; }
            get { return this.ftpPwd; }
        }
       
        /// <summary>
        /// SC的设备ID
        /// </summary>
        public string SCDeviceId
        {
            get { return this.scDeviceId; }
            set { this.scDeviceId = value; }
        }




    }

    #endregion 

    #region  SerialDeviceConfig
    //--->定义了串口设备的信息
    /// <summary>
    /// 此类定义了串口的设备的类
    /// </summary>
    [Serializable]
    public class SerialDeviceConfig
    {
        /// <summary>
        /// 串口的名字
        /// </summary>
        private string portName = "com1";

        /// <summary>
        /// 串口属性的停止位
        /// </summary>
        private StopBits stopBits;

        /// <summary>
        /// 波特率
        /// </summary>
        private uint boundRate;

        /// <summary>
        /// 串口的数据位
        /// </summary>
        private int dataBit;

        /// <summary>
        /// 串口校验位
        /// </summary>
        private Parity parity;

        /// <summary>
        /// 串口的名字
        /// </summary>
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        /// <summary>
        /// 串口的停止位
        /// </summary>
        public StopBits PstopBit
        {
            get { return this.stopBits; }
            set { this.stopBits = value; }
        }

        /// <summary>
        /// 串口的波特率
        /// </summary>
        public uint BoundRate
        {
            get { return this.boundRate; }
            set { this.boundRate = value; }
        }

        /// <summary>
        /// 串口的校验位
        /// </summary>
        public Parity Parity
        {
            get { return this.parity; }
            set { this.parity = value; }
        }

        /// <summary>
        /// 串口的数据位
        /// </summary>
        public int DataBit
        {
            get { return this.dataBit; }
            set { this.dataBit = value; }
        }

    }

    #endregion

    #region PassengerFlowParamCfg

    /// <summary>
    /// 客流监视参数设置类。
    /// </summary>
    [Serializable]
    public class PassengerFlowParamCfg
    {
        int _TimeInterval = 10;
        int _RefurbishInterval = 10;
        int _RowIntervalHeight;
        int _ColumnIntervalWidth;
        int _ColumnWidth;
        int _ColumnBindCheckBoxCount;
        int _PagePoint = 50;
        /// <summary>
        /// 每页多少个点，默认为50个。
        /// </summary>
        [XmlAttribute]
        public int PagePoint
        {
            get { return _PagePoint; }
            set { _PagePoint = value; }
        }

        [XmlAttribute]
        public int ColumnBindCheckBoxCount
        {
            get { return _ColumnBindCheckBoxCount; }
            set { _ColumnBindCheckBoxCount = value; }
        }
        [XmlAttribute]
        public int ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }
        [XmlAttribute]
        public int ColumnIntervalWidth
        {
            get { return _ColumnIntervalWidth; }
            set { _ColumnIntervalWidth = value; }
        }
        [XmlAttribute]
        public int RowIntervalHeight
        {
            get { return _RowIntervalHeight; }
            set { _RowIntervalHeight = value; }
        }
        /// <summary>
        /// 客流刷新时间间隔。
        /// </summary>
        [XmlAttribute]
        public int RefurbishInterval
        {
            get { return _RefurbishInterval; }
            set { _RefurbishInterval = value; }
        }
        /// <summary>
        /// 客流数据时间间隔
        /// </summary>
        [XmlAttribute]
        public int TimeInterval
        {
            get { return _TimeInterval; }
            set { _TimeInterval = value; }
        }

        List<PassFlowConfig> _MonitorParamItems = new List<PassFlowConfig>();
        /// <summary>
        /// 客流监视参数
        /// </summary>
        public List<PassFlowConfig> MonitorParamItems
        {
            get { return _MonitorParamItems; }
            set { _MonitorParamItems = value; }
        }
        List<PropertyValue> _MonitorDeviceTypeItems = new List<PropertyValue>();
        /// <summary>
        /// 设备类型
        /// </summary>
        public List<PropertyValue> MonitorDeviceTypeItems
        {
            get { return _MonitorDeviceTypeItems; }
            set { _MonitorDeviceTypeItems = value; }
        }
    }

    #endregion

    #region PassFlowConfig

    public class PassFlowConfig : PropertyValue
    {
        string _CardIssueId;
        bool _IsMonitor;
       
        /// <summary>
        /// 是否监视
        /// </summary>
        [XmlAttribute]
        public bool IsMonitor
        {
            get { return _IsMonitor; }
            set { _IsMonitor = value; }
        }
        /// <summary>
        /// 卡发行商ID
        /// </summary>
        [XmlAttribute]
        public string CardIssueId
        {
            get { return _CardIssueId; }
            set { _CardIssueId = value; }
        }
    }

    #endregion

    #region ReportConfig
    /// <summary>
  /// 报表配置文件
  /// </summary>
  [Serializable]
  public class ReportConfig
  {
      String _AutoPrintReportPath = "/Report/AutoSaverPath/";
      String _AutoPrintTime = "07:30:00";
      String _ReportTempPath = "/Report/temp";
      String _LocalReportPath = "/ReportDir/";
      String _HistoryReportPath = "/Report/HistoryReport/";
      bool _IsAutoSaveToServer = true;
      bool _IsAutoPrint = false;
      bool _IsStatisticsTestData = false;

      /// <summary>
      /// 统计测试数据还是正常数据:true-只统计测试数据；false-只统计正常数据。
      /// 
      /// 默认是 false;
      /// </summary>
      public bool IsStatisticsTestData
      {
          get { return _IsStatisticsTestData; }
          set { _IsStatisticsTestData = value; }
      }
      /// <summary>
      /// 是否自动保存到服务器:true-保存；false-不保存。
      /// </summary>
      public bool IsAutoSaveToServer
      {
          get { return _IsAutoSaveToServer; }
          set { _IsAutoSaveToServer = value; }
      }
      /// <summary>
      /// 是否自动打印；true-打印；false-不打印。
      /// </summary>
      public bool IsAutoPrint
      {
          get { return _IsAutoPrint; }
          set { _IsAutoPrint = value; }
      }

      /// <summary>
      /// 历史报表存放的路径。
      /// </summary>
      public String HistoryReportPath
      {
          get { return _HistoryReportPath; }
          set { _HistoryReportPath = value; }
      }

      /// <summary>
      /// 本地报模板存放的路径。
      /// </summary>
      public String LocalReportPath
      {
          get { return _LocalReportPath; }
          set { _LocalReportPath = value; }
      }

      /// <summary>
      /// 报表临时存放路径。
      /// </summary>
      public String ReportTempPath
      {
          get { return _ReportTempPath; }
          set { _ReportTempPath = value; }
      }

      /// <summary>
      /// 自动报表打印时间。默认是07:30:00
      /// </summary>
      public String AutoPrintTime
      {
          get { return _AutoPrintTime; }
          set { _AutoPrintTime = value; }
      }

      /// <summary>
      /// 自动报表保存的路径
      /// </summary>
      public String AutoPrintReportPath
      {
          get { return _AutoPrintReportPath; }
          set { _AutoPrintReportPath = value; }
      }
  }

    #endregion
}
