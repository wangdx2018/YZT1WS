using System;
using System.Collections.Generic;

using System.Text;

namespace AFC.WS.UI.RfidRW
{
    /// <summary>
    /// added by wangdx 20100308 配置RFIDRW需要读取的块号
    /// 包括静态区，逻辑区A，B块所占的Block
    /// RFID共有64块，0块为厂商固定的块。每块16个字节
    /// </summary>
    internal class BlockParams
    {
        /// <summary>
        /// 静态区 块号
        /// </summary>
        public const byte StaticArea=1;

        /// <summary>
        /// 票箱块A所占的物理块号
        /// </summary>
        //public static byte[] ticketboxRfidA = new byte[3] { 2, 4, 5 };
        public static byte[] ticketboxRfidA = new byte[3] { 4, 5,6 };

        /// <summary>
        /// 票箱块B所占的物理块号
        /// </summary>
        //public static byte[] ticketboxRfidB = new byte[3] { 6, 8, 9 };
        public static byte[] ticketboxRfidB = new byte[3] { 8, 9, 10 };

        /// <summary>
        /// 钱箱块A所占的物理块号
        /// </summary>
        public static byte[] moneyBoxRifdA = new byte[2] { 12, 13};

        /// <summary>
        /// 钱箱块B所占的物理块号
        /// </summary>
        public static byte[] moneyBoxRfidB = new byte[2] { 16, 17 };

    }
}
