using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;

namespace AFC.WS.UI.RfidRW
{
    using AFC.BJComm.Data;
    /// <summary>
    /// 票箱RFID实体信息对象 
    /// 负责人：王冬欣 最后修改日期：20100121
    /// </summary>
    /// <remarks>
    /// 票箱的RFID对应的实体类
    /// 参见导则第三章 6.2.2	票箱RFID数据结构
    /// </remarks>
    [Description("票箱RFID信息"), TypeConverter(typeof(ExpandableObjectConverter))]
    public class RfidTicketboxInfo:ICloneable
    {
        /// <summary>
        /// 票箱编码	4	HEX
        /// </summary>
        [PackOrder(1)]
        [PackBCD(4,BCDSourceType.NumberString)]
        public string ticketboxId;

        [Description("票箱编码 4 hex")]
        [DisplayName("票箱编码")]
        /// <summary>
        /// 票箱编码 4 hex
        /// </summary>
        public string TicketboxId
        {
            get { return ticketboxId; }
            set { ticketboxId = value; }
        }

        /// <summary>
        /// 操作员编码	3	BCD
        /// </summary>
        [PackOrder(2)]
        [PackBCD(3, BCDSourceType.NumberString)]
        public string operatorId = string.Empty;

        [Description("操作员编码")]
        [DisplayName("操作员编码 3 BCD")]
        public string OperatorId
        {
            get { return operatorId; }
            set { operatorId = value; }
        }

        /// <summary>
        /// 设备编码	4
        /// </summary>
        [PackOrder(3)]
        [PackBCD(4, BCDSourceType.NumberString,trimZero=false)]
        public string deviceId = string.Empty;

        [Description("设备编码 4HEx")]
        [DisplayName("设备编码")]
        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        /// <summary>
        /// 票箱位置状态	1
        /// </summary>
        [PackOrder(4)]
        [PackInt(1, ByteOrder.Intel)]
        public byte ticketboxLoactionStatus;

        [Description(" 票箱位置状态	1Hex")]
        [DisplayName("票箱位置状态")]
        public byte TicketboxLoactionStatus
        {
            get { return ticketboxLoactionStatus; }
            set { ticketboxLoactionStatus = value; }
        }

        /// <summary>
        /// 操作后票箱状态	1
        /// </summary>
        [PackOrder(5)]
        [PackInt(1, ByteOrder.Intel)]
        public byte operatorTicketboxStatus;

        [Description(" 操作后票箱状态	1 Hex")]
        [DisplayName("操作后票箱状态")]
        public byte OperatorTicketboxStatus
        {
            get { return operatorTicketboxStatus; }
            set { operatorTicketboxStatus = value; }
        }

        /// <summary>
        /// 票卡发行商ID	4
        /// </summary>
        [PackOrder(6)]
        [PackInt(4, ByteOrder.Intel)]
        public int cardIssueId;

        [Description("票卡发行商ID	4 Hex")]
        [DisplayName("票卡发行商ID")]
        public int CardIssueId
        {
            get { return cardIssueId; }
            set { cardIssueId = value; }
        }

        /// <summary>
        /// 票卡物理类型	1
        /// </summary>
        [PackOrder(7)]
        [PackInt(1, ByteOrder.Intel)]
        public byte cardPhysicalType;

        [Description("物理卡类型 1 byte")]
        [DisplayName("物理卡类型")]
        public byte CardPhysicalType
        {
            get { return cardPhysicalType; }
            set { cardPhysicalType = value; }
        }

        /// <summary>
        /// 车票产品种类	2	HEX
        /// </summary>
        [PackOrder(8)]
        [PackInt(2, ByteOrder.Intel)]
        public ushort ticketProductType;

        [Description("车票产品种类	2 Hex")]
        [DisplayName("车票产品种类")]
        public ushort TicketProductType
        {
            get { return ticketProductType; }
            set { ticketProductType = value; }
        }

        /// <summary>
        /// 预赋值属性	1	HEX
        /// </summary>
        [PackOrder(9)]
        [PackInt(1, ByteOrder.Intel)]
        public byte prevAddValueCard;

        [Description("预赋值属性	1	HEX")]
        [DisplayName("预赋值属性")]
        public byte PrevAddValueCard
        {
            get { return prevAddValueCard; }
            set { prevAddValueCard = value; }
        }

        /// <summary>
        /// 衍生产品ID	1	HEX	参见注4
        /// </summary>
        [PackOrder(10)]
        [PackInt(1, ByteOrder.Intel)]
        public byte extendProductId;

        [Description("衍生产品ID	1	HEX	参见注4")]
        [DisplayName("衍生产品ID")]
        public byte ExtendProductId
        {
            get { return extendProductId; }
            set { extendProductId = value; }
        }

        /// <summary>
        /// 票卡数量	2	HEX	票箱内的票卡数量
        /// </summary>
        [PackOrder(11)]
        [PackInt(2, ByteOrder.Intel)]
        public ushort ticketNumber;

        [Description("票卡数量	2	HEX	票箱内的票卡数量")]
        [DisplayName("票卡数量")]
        public ushort TicketNumber
        {
            get { return ticketNumber; }
            set { ticketNumber = value; }
        }

        /// <summary>
        /// 车站	2	BCD
        /// </summary>
        [PackOrder(12)]
        [PackBCD(2, BCDSourceType.NumberString ,trimZero=false)]
        public string stationCode = string.Empty;

        [Description("车站	2	BCD")]
        [DisplayName("车站")]
        public string StationCode
        {
            get { return stationCode; }
            set { stationCode = value; }
        }

        /// <summary>
        /// 安装位置	1	HEX	描述票箱在设备中的安装位置
        /// </summary>
        [PackOrder(13)]
        [PackInt(1, ByteOrder.Intel)]
        public byte setupLoaction;

        [Description("安装位置	1	HEX	描述票箱在设备中的安装位置")]
        [DisplayName("安装位置")]
        public byte SetupLoaction
        {
            get { return setupLoaction; }
            set { setupLoaction = value; }
        }

        /// <summary>
        /// 块操作标记	4	HEX	块之间滚动交替写入标志。注6
        /// </summary>
        [PackOrder(14)]
        [PackInt(4, ByteOrder.Intel)]
        public int blockOpeatorFlag;

        [DisplayName("块操作标记")]
        [Description("块操作标记	4	HEX	块之间滚动交替写入标志。注6")]
        public int BlockOpeatorFlag
        {
            get { return blockOpeatorFlag; }
            set { blockOpeatorFlag = value; }
        }

        /// <summary>
        /// 最后操作时间	7	BCD	RFID最后一次写入时间
        /// </summary>
        [PackOrder(15)]
        [PackBCD(7, BCDSourceType.NumberString)]
        public string lastOpeatorTime = string.Empty;

        [DisplayName("最后操作时间")]
        [Description("最后操作时间	7	BCD	RFID最后一次写入时间")]
        public string LastOpeatorTime
        {
            get { return lastOpeatorTime; }
            set { lastOpeatorTime = value; }
        }

        /// <summary>
        /// 校验字段	1	HEX	对块1第0到32字节的校验字段。注5
        /// </summary>
        [PackOrder(16)]
        [PackInt(2, ByteOrder.Intel)]
        public ushort checkField;

        [DisplayName("校验字段")]
        [Description("校验字段	1	HEX	对块1第0到32字节的校验字段。注5")]
        public ushort CheckField
        {
            get { return checkField; }
            set { checkField = value; }
        }

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(17)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd1;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(18)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd2;


        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(19)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd3;


        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(20)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd4;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(21)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd5;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(22)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd6;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(23)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd7;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(24)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd8;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(25)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd9;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(26)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd10;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(27)]
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd11;

        /// <summary>
        /// 预留字段	13
        /// </summary>
        [PackOrder(28)]
        
        [PackInt(1, ByteOrder.Intel)]
        public byte resverd12;





        public override string ToString()
        {
            Type t = typeof(RfidTicketboxInfo);
            System.Reflection.FieldInfo[] fiArray = t.GetFields();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fiArray.Length; i++)
            {
                sb.Append(fiArray[i].Name);
                sb.Append("=");
                sb.Append(fiArray[i].GetValue(this));
                sb.Append("       ");
            }
            return sb.ToString();
            //return base.ToString();
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
