using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;

    public class OperatorForceLogOut_1306 : AbstractCommBody
    {

        /// <summary>
        /// 操作员ID
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint operatorId;

        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public uint deviceId;
    }
}
