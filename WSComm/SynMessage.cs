using System;
using System.Collections.Generic;
using System.Text;

namespace TJComm
{
    /// <summary>
    /// 同步消息
    /// </summary>
    public class SynMessage
    {
        /// <summary>
        /// 发送的同步消息
        /// </summary>
        public TJCommMessage sendMsg;

        /// <summary>
        /// 接收的同步消息
        /// </summary>
        public TJCommMessage receiveMsg;

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime sendTime;

    }
}
