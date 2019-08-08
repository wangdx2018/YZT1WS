using System;
using System.Collections.Generic;
using System.Text;

namespace TJComm
{
    /// <summary>
    /// 通讯的消息包类型
    /// </summary>
    public enum CommandType:uint
    {
        /// <summary>
        /// 请求
        /// </summary>
        RESQUEST = 0,

        /// <summary>
        /// 应答
        /// </summary>
         RESPONSE = 1,

        /// <summary>
        /// MACK应答
        /// </summary>
        MACK = 2


    }
}
