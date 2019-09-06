using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{

    using AFC.BJComm.Data;
    /// <summary>
    /// 控制命令报文定义
    /// </summary>
    public  class ControlCmd_3000 : AbstractCommBody
    {
        /// <summary>
        /// 发起时间
        /// </summary>
        [PackOrder(1)]
        [PackArray(0, ByteOrder.Intel, 1, ByteOrder.Intel)]
        public byte[] SendTime = new byte[7];
 
        /// <summary>
        /// send设备ID
        /// </summary>
        [PackOrder(2), PackInt(4, ByteOrder.Moto)]
        public uint SendDeviceId;
        /// <summary>
        /// dest设备ID
        /// </summary>
        [PackOrder(3), PackInt(4, ByteOrder.Moto)]
        public uint DestDeviceId;

         /// <summary>
        /// 操作员id
        /// </summary>
        [PackOrder(4), PackString(10)]
        public string operatorId = string.Empty;
        /// <summary>
        /// 控制代码 
        /// </summary>
        [PackOrder(5), PackInt(2, ByteOrder.Moto)]
        public ushort controlCode;
        
    }
}
