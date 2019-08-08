using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestComm
{
    using TJComm;
   public class AsynMessageHandle:IServerMessageHandler
    {
        #region IServerMessageHandler 成员

        public byte[] HandleServerMessage(byte[] requestMessage, out int retcode)
        {
            retcode = 0;
            TJCommMessage msg = TJCommMessage.UnPackData(requestMessage);
            if (msg != null)
                System.Windows.MessageBox.Show("receive msg messageType=[" + msg.header.messageType.ToString("x2") + "]");
            //throw new NotImplementedException();
            return null;
        }

        public byte[] HandleServerError(int errorCode, out int retcode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
