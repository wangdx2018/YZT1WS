using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;

    /// <summary>
    /// 操作员解锁.交易数据
    /// </summary>
    public class OperatorUnLock_1305 : AbstractCommBody
    {

        /// <summary>
        /// 解锁的操作员ID
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint unlockedOperatorId;
    }
}
