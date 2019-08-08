using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    /// <summary>
    /// 模式变化通知
    /// </summary>
    public class ModeChangeNotify_1342:AbstractCommBody
    {
        /// <summary>
        /// 模式车站ID
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public ushort modeStationId;

        /// <summary>
        /// 模式代码
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public ushort modeCode;
    }
}
