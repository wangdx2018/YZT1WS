using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    public class RunStart_1351:AbstractCommBody
    {  
        [PackOrder(3), PackInt(4, ByteOrder.Moto)]
        public uint remark;
    }
}
