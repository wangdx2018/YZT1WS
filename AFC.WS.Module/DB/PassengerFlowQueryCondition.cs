using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.DB
{
    /// <summary>
    /// 客流历史查询条件
    /// </summary>
    public class PassengerFlowQueryCondition
    {
        string _DeviceType;
        string _PassengerFlowType;
        string _StationID;
        string _StationHallID;
        int _TimeInteval;
        string  _ControlBeginTime;
        string  _ControlEndTime;
        string  _CardIssueID;
        DateTime  _BeginTime;
        DateTime  _EndTime;
        DateTime _DtControlBeginTime;
        DateTime _DtControlEndTime;
        /// <summary>
        /// 查询条件开始时间。
        /// </summary>
        public DateTime  BeginTime
        {
            get { return _BeginTime; }
            set { _BeginTime = value; }
        }
       
        /// <summary>
        /// 查询条件结束时间
        /// </summary>
        public DateTime  EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        /// <summary>
        /// 卡发行商ID
        /// </summary>
        public string CardIssueID
        {
            get { return _CardIssueID; }
            set { _CardIssueID = value; }
        }
        /// <summary>
        /// 控件结束时间
        /// </summary>
        public string ControlEndTime
        {
            get { return _ControlEndTime; }
            set { _ControlEndTime = value; }
        }
        /// <summary>
        /// 控件开始时间
        /// </summary>
        public string ControlBeginTime
        {
            get { return _ControlBeginTime; }
            set { _ControlBeginTime = value; }
        }

        /// <summary>
        /// 控件开始时间
        /// </summary>
        public DateTime DtControlBeginTime
        {
            get { return _DtControlBeginTime; }
            set { _DtControlBeginTime = value; }
        }
        /// <summary>
        /// 控件结束时间
        /// </summary>
        public DateTime DtControlEndTime
        {
            get { return _DtControlEndTime; }
            set { _DtControlEndTime = value; }
        }
        
        /// <summary>
        /// 时间间隔
        /// </summary>
        public int TimeInterval
        {
            get { return _TimeInteval; }
            set { _TimeInteval = value; }
        }
        /// <summary>
        /// 站厅ID
        /// </summary>
        public string StationHallID
        {
            get { return _StationHallID; }
            set { _StationHallID = value; }
        }
        /// <summary>
        /// 车站ID
        /// </summary>
        public string StationID
        {
            get { return _StationID; }
            set { _StationID = value; }
        }
        /// <summary>
        /// 客流类型
        /// </summary>
        public string PassengerFlowType
        {
            get { return _PassengerFlowType; }
            set { _PassengerFlowType = value; }
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType
        {
            get { return _DeviceType; }
            set { _DeviceType = value; }
        }
    }
}
