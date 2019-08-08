using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    public class ReUploadRecords_1363 : AbstractCommBody
    {
        [PackOrder(3), PackInt(4, ByteOrder.Moto)]
        public uint dataType;

        [PackOrder(4), PackInt(4, ByteOrder.Moto)]
        public uint operType;

        [PackOrder(5), PackInt(4, ByteOrder.Moto)]
        public uint beginTime;

        [PackOrder(6), PackInt(4, ByteOrder.Moto)]
        public uint endTime;

    }
}
