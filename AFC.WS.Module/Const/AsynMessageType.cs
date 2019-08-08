using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    /// <summary>
    /// 异步消息类型
    /// 
    /// added by wangdx 20130624 增加了报警监视消息类型
    /// </summary>
    public class AsynMessageType
    {
        /// <summary>
        /// 通讯的异步消息
        /// </summary>
        public const string CommAsynMsg = "CommAsynMsg";

        /// <summary>
        /// 设备监控
        /// </summary>
        public const string DeviceMonitor = "DeviceMonitor";

        /// <summary>
        /// 运营开始
        /// </summary>
        public const string RunStart = "RunStart";

        /// <summary>
        /// 运营结束
        /// </summary>
        public const string RunEnd = "RunEnd";

        /// <summary>
        /// 报警开始
        /// </summary>
        public const string AlarmStatusOpen = "AlarmStatusOpen";

        /// <summary>
        /// 报警结束
        /// </summary>
        public const string AlarmStatusClose = "AlarmStatusClose";


        /// <summary>
        /// 报警监视（added by wangdx 20130624)
        /// </summary>
        public const string AlarmMonitor = "AlarmMonitor";

        /// <summary>
        /// 设备运营开始
        /// </summary>
        public const string DeviceRunStart = "DeviceRunStart";

        /// <summary>
        /// 设备运营结束
        /// </summary>
        public const string DeviceRunEnd = "DeviceRunEnd";
    }
}
