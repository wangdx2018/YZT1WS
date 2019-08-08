using System;
using System.Collections.Generic;

using System.Text;

namespace AFC.WS.UI.RfidRW
{
    /// <summary>
    /// RFIDRW 操作，读，写，获取状态
    /// </summary>
    public enum RFIDOperatorEnum : byte
    {
        /// <summary>
        /// 写操作
        /// </summary>
        Write = 1,

        /// <summary>
        /// 读操作
        /// </summary>
        Read = 2,

        /// <summary>
        /// 状态
        /// </summary>
        Status = 3
    }
}
