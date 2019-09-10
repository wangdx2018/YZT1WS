using System;
using System.Collections.Generic;
using System.Text;

namespace TJComm
{
    using AFC.BJComm.Data;
    /// <summary>
    /// 存活包，包体
    /// </summary>
    public class BeartHeartData
    {
        /// <summary>
        /// 接收方ID //上层写 4
        /// </summary>
        [PackOrder(1)]
        [PackInt(4, ByteOrder.Intel)]
        public uint receiveId;













        
        /// <summary>
        /// 自定义的通讯头
        /// </summary>
        //[PackOrder(1), PackStruct(0, ByteOrder.Moto)]
        //public CommHeaderData selfDefPacketHeaderData;

        /// <summary>
        /// 公共数据
        /// </summary>
        //[PackOrder(2), PackStruct(0, ByteOrder.Moto)]
        //public CommBodyData comPackageData;

        /// <summary>
        ///预留字段 
        /// </summary>
        //[PackOrder(3), PackInt(4, ByteOrder.Moto)]
        //public int resove;
    }
}
