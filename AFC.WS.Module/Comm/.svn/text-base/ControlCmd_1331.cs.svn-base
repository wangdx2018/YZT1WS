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
    public class ControlCmd_1331:AbstractCommBody
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint deviceId;

        /// <summary>
        /// 控制类型
        /// 0x01	通用设备控制
        /// 0x02	AGM个性控制
        /// 0x03	TVM个性控制
        /// 0x04	BOM个性控制
        ///  0X05	AVM个性控制
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public byte controlType;

        /// <summary>
        /// 控制代码 
        /// </summary>
        [PackOrder(5),PackInt(4,ByteOrder.Moto)]
        public ushort controlCode;

        /// <summary>
        /// 预留字段
        /// </summary>
        [PackOrder(6),PackInt(4,ByteOrder.Moto)]
        public uint remark;

    }
}
