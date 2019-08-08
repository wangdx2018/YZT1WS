using System;
using System.Collections.Generic;

using System.Text;

namespace AFC.WS.UI.RfidRW
{
    using AFC.BJComm.Data;
    /// <summary>
    /// Header头信息
    /// </summary>
    public class Header
    {
        /// <summary>
        /// 虚拟地址
        /// </summary>
        [PackOrder(1)]
        [PackInt(1, ByteOrder.Moto)]
        public byte virtualAddress = 0x2F;

        /// <summary>
        /// 通道号
        /// </summary>
        [PackOrder(2)]
        [PackInt(1, ByteOrder.Moto)]
        public byte pathNumber;

        /// <summary>
        /// 读，写，状态操作
        /// </summary>
        [PackOrder(3)]
        [PackInt(1, ByteOrder.Moto)]
        public byte wrOperation;

        /// <summary>
        /// 块号
        /// </summary>
        [PackOrder(4)]
        [PackInt(1, ByteOrder.Moto)]
        public byte blockAddress;
    }
}
