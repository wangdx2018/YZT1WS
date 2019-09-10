using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;
    using AFC.WS.Model.Const;

    /// <summary>
    /// WS通讯Body基类
    /// </summary>
    public abstract class AbstractCommBody
    {
        ///// <summary>
        ///// 通用头
        ///// </summary>
        //[PackOrder(1), PackStruct(0, ByteOrder.Moto)]
        //public CommHeaderData headerData = new CommHeaderData();

        ///// <summary>
        ///// 通用Body
        ///// </summary>
        //[PackOrder(2), PackStruct(0, ByteOrder.Moto)]
        //public CommBodyData commBody = new CommBodyData();

        /// <summary>
        /// 创建AbstractBody
        /// </summary>
        /// <param name="header">Commheader</param>
        /// <param name="opeatorId">操作员ID（该OperatorId是登录的操作员ID）</param>
        /// <returns>返回AbstractBody子类对象</returns>
        public static AbstractCommBody CreateCommBody(CommHeader header, uint opeatorId)
        {
            AbstractCommBody body = null;
            switch (header.messageType)
            {
                case CommMsgType.Log_In_Out:
                    body = new OperatorLogInOut_1301();
                    break;
                case CommMsgType.Operator_Locked:
                    body = new OperatorLocked_1304();
                    break;
                case CommMsgType.Operator_Unlocked:
                    body = new OperatorUnLock_1305();
                    break;
                case CommMsgType.Param_Download_Notify:
                    body = new ParamsDownLoadNotify_1309();
                    break;
                case CommMsgType.Specail_Params_Download_Notify:
                    body = new SpecialParamsDownLoadNotify_1313();
                    break;
                case CommMsgType.Params_Publish:
                    body = new ParamsPublish_1315();
                    break;
                case CommMsgType.Change_Pwd:
                    body = new OperatorChangePwd_1302();
                    break;
                case CommMsgType.Check_In:
                    body = new CheckIn_1390();
                    break;
                case CommMsgType.Operator_Force_LogOut:
                    body = new OperatorForceLogOut_1306();
                    break;
                case CommMsgType.Control_CMD:
                    body = new ControlCmd_1331();
                    break;
                case CommMsgType.Mode_Change_CMD:
                    body = new ModeChange_1341();
                    break;
                case CommMsgType.Mode_Change_Notify:
                    body = new ModeChangeNotify_1342();
                    break;
                case CommMsgType.Time_Syn:
                    body = new SetTimeSyn_1334();
                    break;
                case CommMsgType.Force_Time_Syn:
                    body = new ForcenTimeSyn_1333();
                    break;
                case CommMsgType.Run_Start:
                    body = new RunStart_1351();
                    break;
                case CommMsgType.Run_End:
                    body = new RunEnd_1352();
                    break;
                case CommMsgType.Dev_Status_Report:
                    body = new DevStatus_1325();
                    break;
                case CommMsgType.Data_Import_Notify:
                    body = new DataImportNotify_1009();
                    break;
                case CommMsgType.Date_Settlement:
                    body = new DateSettlement_1353();
                    break;  
                case CommMsgType.Tick_UpdateNotify:
                    body = new TickUpdateNotify_1361();
                    break;
                case CommMsgType.Data_ReUploadRecords:
                    body= new ReUploadRecords_1363();
                    break;
            }
            //body.commBody = new CommBodyData(header);
            //body.headerData = new CommHeaderData(opeatorId);

            return body;

        }
    }
}
