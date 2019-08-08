using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;

    /// <summary>
    /// 操作员锁定报文数据
    /// </summary>
    public class OperatorLocked_1304 : AbstractCommBody
    {

        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint operatorId;

       /// <summary>
       /// 锁定原因
       /// </summary>
       [PackOrder(4),PackInt(4,ByteOrder.Moto)]
       public byte lockReason;



    }
}
