using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestComm
{
    using TJComm;
    using AFC.WS.UI.Common;
    using AFC.BJComm.Data;
    using AFC.WS.Model.Comm;
    using AFC.WS.Model.Const;
    

    public class UnPackHandle:IMutableInstance
    {
        #region IMutableInstance 成员

        public object JudgePackedInstance(object parent, System.IO.MemoryStream s)
        {
            TJCommMessage msg = parent as TJCommMessage;
            switch (msg.header.messageType)
            {
                case CommMsgType.Log_In_Out:
                    return new OperatorLogInOut_1301();
                case CommMsgType.Operator_Locked:
                    return new OperatorLocked_1304();
                case CommMsgType.Operator_Unlocked:
                    return new OperatorUnLock_1305();
                case CommMsgType.Unspecified:
                    return new TestMsgContent();
                default:
                    return new CommonResponseMsg();
                    
            }
        }

        #endregion
    }
}
