using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.TickMonyBoxManager
{
    /// <summary>
    /// 钱箱信息类。
    /// </summary>
    public class DisplayMoneyBox
    {
        string _DeviceID;
        string _MoneyBoxID;
        string _OperatorID;
        string _TotalCash;
        string _TotalNumber;
        /// <summary>
        /// 总数量
        /// </summary>
        public string TotalNumber
        {
            get { return _TotalNumber; }
            set { _TotalNumber = value; }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        public string TotalCash
        {
            get { return _TotalCash; }
            set { _TotalCash = value; }
        }
        /// <summary>
        /// 操作员编码
        /// </summary>
        public string OperatorID
        {
            get { return _OperatorID; }
            set { _OperatorID = value; }
        }
        /// <summary>
        /// 钱箱ID
        /// </summary>
        public string MoneyBoxID
        {
            get { return _MoneyBoxID; }
            set { _MoneyBoxID = value; }
        }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }
    }
}
