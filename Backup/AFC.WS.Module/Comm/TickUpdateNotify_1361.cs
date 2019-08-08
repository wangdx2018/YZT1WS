using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;
    /// <summary>
    /// 票卡更新通知
    /// </summary>
    public class TickUpdateNotify_1361 : AbstractCommBody
    {
        /// <summary>
        /// 库存类型
        /// </summary>
        [PackOrder(3), PackArray(4, ByteOrder.Moto, 1, ByteOrder.Moto, isXDR = true)]
        public string tickManaType;

        /// <summary>
        /// 库存类型名称
        /// </summary>
        [PackOrder(4), PackArray(4, ByteOrder.Moto, 1, ByteOrder.Moto, isXDR = true)]
        public string tickManaTypeName;

        /// <summary>
        /// 卡发行商
        /// </summary>
        [PackOrder(5), PackInt(4, ByteOrder.Moto)]
        public byte cardIssueID;

        /// <summary>
        /// 卡物理类型
        /// </summary>
        [PackOrder(6), PackInt(4, ByteOrder.Moto)]
        public byte tickPhyType;

        /// <summary>
        /// 扣费类型
        /// </summary>
        [PackOrder(7), PackInt(4, ByteOrder.Moto)]
        public byte productType;

        /// <summary>
        /// 卡内金额
        /// </summary>
        [PackOrder(8), PackInt(4, ByteOrder.Moto)]
        public uint preStoreMoney;


        /// <summary>
        /// 卡内押金
        /// </summary>
        [PackOrder(9), PackInt(4, ByteOrder.Moto)]
        public uint tickDeposit;

        /// <summary>
        /// 卡售卖金额
        /// </summary>
        [PackOrder(10), PackInt(4, ByteOrder.Moto)]
        public uint tickSaleValue;

    }
}
