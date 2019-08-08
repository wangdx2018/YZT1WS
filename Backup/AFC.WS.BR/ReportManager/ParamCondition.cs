using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.ReportManager
{
    /// <summary>
    /// 参数条件
    /// </summary>
    public class ParamCondition
    {
        int _timeInterval;
        string _lineID;
        string _stationID;
        string _reportName;
        string _systemDateTime;
        string _stationName;
        string _lineName;
        string _locationTemplateFilePath;
        string _reportSaveFilePath;
        string _classTimeId;
        string _classTimeName;

        string _BissType;
        string _BissSubType;
        string _OperatorId;
        string _BissTypeName;
        string _BissSubTypeName;
        string _OperatorIdName;
        string _ProductTypeId;
        string _CardIssuerId;
        string _DeviceID;
        string _DeviceCode;
        string _StatisticsBeginDatetime;
        string _StatisticsEndDatetime;
        string _StatisticsDateTimeScope;
        string _DeviceType;
        string _DeviceTypeName;
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string DeviceTypeName
        {
            get { return _DeviceTypeName; }
            set { _DeviceTypeName = value; }
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType
        {
            get { return _DeviceType; }
            set { _DeviceType = value; }
        }

        /// <summary>
        /// 统计时间段:2010-11-01 09:20:10 ~ 2010-11-03 10:10:10
        /// </summary>
        public string StatisticsDateTimeScope
        {
            get { return _StatisticsDateTimeScope; }
            set { _StatisticsDateTimeScope = value; }
        }

        /// <summary>
        /// 统计结束时间
        /// </summary>
        public string StatisticsEndDatetime
        {
            get { return _StatisticsEndDatetime; }
            set { _StatisticsEndDatetime = value; }
        }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public string StatisticsBeginDatetime
        {
            get { return _StatisticsBeginDatetime; }
            set { _StatisticsBeginDatetime = value; }
        }


        /// <summary>
        /// 设备代码
        /// </summary>
        public string DeviceCode
        {
            get { return _DeviceCode; }
            set { _DeviceCode = value; }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }

        /// <summary>
        /// 卡发行商ID
        /// </summary>
        public string CardIssuerId
        {
            get { return _CardIssuerId; }
            set { _CardIssuerId = value; }
        }

        /// <summary>
        /// 票卡ID
        /// </summary>
        public string ProductTypeId
        {
            get { return _ProductTypeId; }
            set { _ProductTypeId = value; }
        }
        string _ProductTypeName;
        /// <summary>
        /// 库存管理类型
        /// </summary>
        public string ProductTypeName
        {
            get { return _ProductTypeName; }
            set { _ProductTypeName = value; }
        }

        /// <summary>
        /// 操作员ID名称。主要用于显示到EXCEL上去的。
        /// </summary>
        public string OperatorIdName
        {
            get { return _OperatorIdName; }
            set { _OperatorIdName = value; }
        }

        /// <summary>
        /// 交易类型名称
        /// </summary>
        public string BissTypeName
        {
            get { return _BissTypeName; }
            set { _BissTypeName = value; }
        }

        /// <summary>
        /// 交易子类型名称。
        /// </summary>
        public string BissSubTypeName
        {
            get { return _BissSubTypeName; }
            set { _BissSubTypeName = value; }
        }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string BissType
        {
            get { return _BissType; }
            set { _BissType = value; }
        }

        /// <summary>
        /// 交易子类型
        /// </summary>
        public string BissSubType
        {
            get { return _BissSubType; }
            set { _BissSubType = value; }
        }
        /// <summary>
        /// 操作员ID
        /// </summary>
        public string OperatorId
        {
            get { return _OperatorId; }
            set { _OperatorId = value; }
        }

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ClassTimeName
        {
            get { return _classTimeName; }
            set { _classTimeName = value; }
        }

        /// <summary>
        /// 班次ID。
        /// </summary>
        public string ClassTimeId
        {
            get { return _classTimeId; }
            set { _classTimeId = value; }
        }

        /// <summary>
        /// 时间段
        /// </summary>
        public DateTimeScope TimeScope;

        /// <summary>
        /// 报表信息。
        /// </summary>
        public BasiReportInfo ReportInfo;

        /// <summary>
        /// 报表历史信息
        /// </summary>
        public RptHistoryInfo HistoryInfo;

        /// <summary>
        /// 报表保存的路径
        /// 
        /// 自动保存报表路径    ~/AutoSavePath/
        /// 
        /// 报表打印时临时路径  ~/Reports/
        /// </summary>
        public string ReportSaveFilePath
        {
            get { return _reportSaveFilePath; }
            set { _reportSaveFilePath = value; }
        }

        /// <summary>
        /// 本地文件路径
        /// </summary>
        public string LocationTemplateFilePath
        {
            get { return _locationTemplateFilePath; }
            set { _locationTemplateFilePath = value; }
        }

        /// <summary>
        /// 线路。
        /// </summary>
        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }
        /// <summary>
        /// 车站。
        /// </summary>
        public string StationName
        {
            get { return _stationName; }
            set { _stationName = value; }
        }

        /// <summary>
        /// 系统时间：yyyyMMddHHmmss
        /// </summary>
        public string SystemDateTime
        {
            get { return _systemDateTime; }
            set { _systemDateTime = value; }
        }

        /// <summary>
        /// 线路ID
        /// </summary>
        public string LineID
        {
            get { return _lineID; }
            set { _lineID = value; }
        }
        /// <summary>
        /// 车站
        /// </summary>
        public string StationID
        {
            get { return _stationID; }
            set { _stationID = value; }
        }
        /// <summary>
        /// 报表名称
        /// </summary>
        public string ReportName
        {
            get { return _reportName; }
            set { _reportName = value; }
        }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public int TimeInterval
        {
            get { return _timeInterval; }
            set { _timeInterval = value; }
        }
    }
}
