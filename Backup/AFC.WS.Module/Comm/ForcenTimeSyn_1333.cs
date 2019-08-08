using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    public class ForcenTimeSyn_1333:AbstractCommBody
    {
        /// <summary>
        /// 当前时间 afctime_t
        /// </summary>
        /// 
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint currentTime;
    }
}
