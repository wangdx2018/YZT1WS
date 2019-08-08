using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    
    using TJComm;
    /// <summary>
    /// 通用的报文应答消息 added by wangdx
    /// 20110311
    /// </summary>
    public class CommonResponseMsg:AbstractCommBody
    {
       
        /// <summary>
        /// 结果代码。0、成功；1、失败
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public byte resultCode;

        /// <summary>
        /// 附加代码,错误原因详细代码
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public uint resultStatusCode;
    }
}
