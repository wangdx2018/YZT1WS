using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestComm
{
    using AFC.BJComm.Data;
    using AFC.WS.Model.Comm;


    public class TestMsgContent
    {
        [PackOrder(1),PackStruct(0,ByteOrder.Moto)]
        public OperatorLogInOut_1301 info1 = new OperatorLogInOut_1301();

        [PackOrder(2),PackStruct(0,ByteOrder.Moto)]
        public OperatorUnLock_1305 info2 = new OperatorUnLock_1305();
    }
}
