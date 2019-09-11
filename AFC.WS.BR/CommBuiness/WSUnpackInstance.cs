using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.CommBuiness
{
    using AFC.BJComm.Data;
    using TJComm;
    using AFC.WS.Model.Const;
    using AFC.WS.Model.Comm;

    /// <summary>
    /// added by wangdx:20110314
    /// WS解包的通用处理
    /// </summary>
    public class WSUnpackInstance:IMutableInstance
    {
        #region IMutableInstance 成员

        public object JudgePackedInstance(object parent, System.IO.MemoryStream s)
        {
            TJCommMessage msg = parent as TJCommMessage;
            if (msg == null)
                return null;
            switch (msg.header.messageType)
            {
                case CommMsgType.Log_In_Out:
                case CommMsgType.Operator_Locked:
                case CommMsgType.Operator_Unlocked:
                case CommMsgType.Change_Pwd:
                case CommMsgType.Params_Publish:
                case CommMsgType.Specail_Params_Download_Notify:
                case CommMsgType.Param_Download_Notify:
                case CommMsgType.Control_CMD:
                case CommMsgType.Force_Time_Syn:
                case CommMsgType.Time_Syn:
                case CommMsgType.Primission_Param_Building:
                case CommMsgType.Run_Start:
                case CommMsgType.Run_End:
                case CommMsgType.Date_Settlement:
                case CommMsgType.Data_ReUploadRecords:
                case CommMsgType.Operator_Force_LogOut:
                    return new CommonResponseMsg(); //result code ,result status code
                case CommMsgType.Mode_Change_Notify:
                    return new ModeChangeNotify_1342();
                case CommMsgType.Check_In:
                    return new CheckInResponse_1390();
                case CommMsgType.Dev_Status_Report:
                    return new DevStatus_1325();
                case CommMsgType.Mode_Change_CMD:
                    //if (msg.header.sessionFlagMap==CommandType.RESPONSE)
                    //    return new CommonResponseMsg();
                    //else
                    //    return new ModeChange_1341();
                default:
                    return null;
            }
        }

        #endregion
    }
}
