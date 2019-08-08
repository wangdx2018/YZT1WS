using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;

namespace AFC.WS.UI.RfidRW
{
    using AFC.BJComm.Data;
    /// <summary>
    /// 钱箱的RIFD
    /// </summary>
    [Description("票箱RFID信息"), TypeConverter(typeof(ExpandableObjectConverter))]
    public class MoneyBoxRFID:ICloneable
    {
        /// <summary>
        /// 钱箱编码	4	HEX
        /// </summary>
        [PackOrder(1)]
        [PackBCD(4, BCDSourceType.NumberString,trimZero=false)]
        public string moneyBoxId;

        public string MoneyBoxId
        {
            get { return moneyBoxId; }
            set { moneyBoxId = value; }
        }

        /// <summary>
        /// 操作员编码	3	BCD	
        /// </summary>
        [PackOrder(2)]
        [PackBCD(3, BCDSourceType.NumberString)]
        public string operatorId;

        public string OperatorId
        {
            get { return operatorId; }
            set { operatorId = value; }
        }

        /// <summary>
        /// 设备编码	4	BCD	
        /// </summary>
        [PackOrder(3)]
        [PackBCD(4, BCDSourceType.NumberString,trimZero=false)]
        public string deviceId;

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        /// <summary>
        /// 钱箱位置状态	1	HEX	参见注1.
        /// </summary>
        [PackOrder(4)]
        [PackInt(1, ByteOrder.Intel)]
        public byte moneyBoxLocationId;

        public byte MoneyBoxLocationId
        {
            get { return moneyBoxLocationId; }
            set { moneyBoxLocationId = value; }
        }

        /// <summary>
        /// 钱箱操作后状态	1	HEX	参见注2
        /// </summary>
        [PackOrder(5)]
        [PackInt(1, ByteOrder.Intel)]
        public byte moneyBoxOperatorStatus;

        public byte MoneyBoxOperatorStatus
        {
            get { return moneyBoxOperatorStatus; }
            set { moneyBoxOperatorStatus = value; }
        }

        /// <summary>
        /// 币种代码	1	HEX	回收箱填0x00，表示多个币种。注3
        /// </summary>
        [PackOrder(6)]
        [PackInt(1, ByteOrder.Intel)]
        public byte moneyCode;

        public byte MoneyCode
        {
            get { return moneyCode; }
            set { moneyCode = value; }
        }

        /// <summary>
        /// 钱币总数量	2	HEX	纸币的张数或硬币的总张数或枚数。注3
        /// </summary>
        [PackOrder(7)]
        [PackInt(2, ByteOrder.Intel)]
        public ushort moneyTotalNumber;

        public ushort MoneyTotalNumber
        {
            get { return moneyTotalNumber; }
            set { moneyTotalNumber = value; }
        }

        /// <summary>
        /// 钱币总金额	4	HEX	注3
        /// </summary>
        [PackOrder(8)]
        [PackInt(4, ByteOrder.Intel)]
        public uint moneyTotalCount;

        public uint MoneyTotalCount
        {
            get { return moneyTotalCount; }
            set { moneyTotalCount = value; }
        }

        /// <summary>
        /// 车站	2	BCD	
        /// </summary>
        [PackOrder(9)]
        [PackBCD(2, BCDSourceType.NumberString,trimZero=false)]
        public string stationCode;

        public string StationCode
        {
            get { return stationCode; }
            set { stationCode = value; }
        }

        /// <summary>
        /// 安装位置	1	HEX	描述票箱在设备中的安装位置，自定义
        /// </summary>
        [PackOrder(10)]
        [PackInt(1, ByteOrder.Intel)]
        public byte setupLocation;

        public byte SetupLocation
        {
            get { return setupLocation; }
            set { setupLocation = value; }
        }

        /// <summary>
        /// 块操作标记	4	HEX	块之间滚动交替写入标志。注5
        /// </summary>
        [PackOrder(11)]
        [PackInt(4, ByteOrder.Intel)]
        public int blockOperatorFlag;

        public int BlockOperatorFlag
        {
            get { return blockOperatorFlag; }
            set { blockOperatorFlag = value; }
        }

        /// <summary>
        /// 最后操作时间	7	BCD	RFID最后一次写入时间，参见通用数据约定
        /// </summary>
        [PackOrder(12)]
        [PackBCD(7, BCDSourceType.NumberString)]
        public string lastOperatorTime;

        public string LastOperatorTime
        {
            get { return lastOperatorTime; }
            set { lastOperatorTime = value; }
        }

        /// <summary>
        /// 校验字段	2	HEX	对块1第0到32字节的校验字段。注4
        /// </summary>
        [PackOrder(13)]
        [PackInt(2, ByteOrder.Intel)]
        public ushort checkArea;

        public ushort CheckArea
        {
            get { return checkArea; }
            set { checkArea = value; }
        }

      


        public override string ToString()
        {
            Type t = typeof(MoneyBoxRFID);
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
        }



        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
