using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.CommBuiness
{
    using TJComm;
    using AFC.WS.Model.Comm;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.Const;
    using AFC.BOM2.MessageDispacher;

    /// <summary>
    /// added by wangdx date:20110314
    /// 处理下行数据报文,发送异步消息通知UI
    /// 
    /// modified by wangdx 20130624 不在向报警转发消息
    /// </summary>
    public class HandleServiceMsgHandle:IServerMessageHandler
    {
        #region IServerMessageHandler 成员

        public byte[] HandleServerMessage(byte[] requestMessage, out int retcode)
        {
               retcode = 0;
               TJCommMessage msg = TJCommMessage.UnPackData(requestMessage);
               if (msg.header.messageType == CommMsgType.Dev_Status_Report)
               {
                   CommBuiness cb = BuinessRule.GetInstace().commProcess as CommBuiness;
                   msg.header.sessionFlagMap = CommandType.MACK;
                   AbstractCommBody ack = AbstractCommBody.CreateCommBody(msg.header, BuinessRule.GetInstace().OperatorId.ConvertNumberStringToUint());
                   TJCommMessage ackMsg = new TJCommMessage();
                   ackMsg.header = msg.header;
                   ackMsg.packageBody = ack;
                   cb.con.SendMsg(ackMsg);
               }
              
               WriteLog.Log_Info(msg.ToString());
               Message asyMsg = new Message();
               asyMsg.MessageType = AsynMessageType.CommAsynMsg;
               asyMsg.Content = msg.packageBody.ToString();
               asyMsg.MessageParam = msg.packageBody;
           //    MessageManager.SendMessasge(asyMsg);
               return null;
        }

        public byte[] HandleServerError(int errorCode, out int retcode)
        {
            //return null;
           throw new NotImplementedException();
        }


       


        

        #endregion
    }
}
