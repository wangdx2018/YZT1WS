using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace TJComm
{
    using AFC.BJComm.Data;

    /// <summary>
    /// 通讯程序公共头
    /// </summary>
    public class CommHeader
    {
        /// <summary>
        /// 包长度  4
        /// </summary>
        [PackOrder(1)]
        //[PackInt(4,ByteOrder.Moto)]
        //public int packetLength;
        [PackInt(2, ByteOrder.Intel)]
        public int packetLength;

        /// <summary>
        /// 消息类型码 //上层填写 4
        /// </summary>
        [PackOrder(2)]
        //[PackInt(4,ByteOrder.Moto)]
        //public ushort messageType;
        [PackInt(2, ByteOrder.Intel)]
        public ushort messageType;


        /// <summary>
        /// 发起方ID  //上层写 4
        /// </summary>
        [PackOrder(3)]
        [PackInt(4, ByteOrder.Moto)]
        public uint senderId;

        /// <summary>
        /// 接收方ID //上层写 4
        /// </summary>
        [PackOrder(4)]
        [PackInt(4, ByteOrder.Moto)]
        public uint receiveId;

        /// <summary>
        /// 转发方ID //上层写 4
        /// </summary>
        [PackOrder(5)]
        [PackInt(4, ByteOrder.Moto)]
        public uint transmitId;


        /// <summary>
        /// 求消息：发送方分配的，唯一标识本会话的流水号
        ///应答消息：内容与请求消息相同 4
        /// </summary>
        [PackOrder(6)]
        [PackInt(4, ByteOrder.Intel)]
        public uint sessionId;


        /// <summary>
        /// MsgFlag
        /// </summary>
        [PackOrder(7)]
        [PackInt(1, ByteOrder.Intel)]
        public char sessionFlagMap;

        /// <summary>
        /// MsgVer
        /// </summary>
        [PackOrder(8)]
        [PackInt(1, ByteOrder.Intel)]
        public char messageVer;

        /// <summary>
        /// MsgAck
        /// </summary>
        [PackOrder(9)]
        [PackInt(1, ByteOrder.Intel)]
        public char msgAck;

        /// <summary>
        /// 协议号  //固定值 4
        /// </summary>
        
        //[PackOrder(3)]
        //[PackInt(4,ByteOrder.Moto)]
        //public uint protocalVersion;


    
        ///// <summary>
        ///// Bit00~Bit01：请求应该标志（0：请求；1：应答；2：MACK应答；3：保留）
        /////Bit08：加密标志（0：不加密；1：加密）
        /////Bit09~Bit15：加密算法
        /////Bit16：压缩标志（0：不压缩；1：压缩）
        /////Bit17~Bit23：压缩算法
        /////Bit24~Bit31：预留 4
        ///// </summary>
        //[PackOrder(7)]
        //[PackInt(4,ByteOrder.Moto)]
        //public CommandType sessionFlagMap;

        /// <summary>
        /// 头的打包长度
        /// </summary>
        public const int Head_LEN = 23;


        public override string ToString()
        {
            FieldInfo[] fiArray = this.GetType().GetFields();
            StringBuilder sb = new StringBuilder();
            sb.Append("CommHeader:\t");
            for (int i = 0; i < fiArray.Length; i++)
            {
                sb.Append(string.Format("[{0}]=[{1}]\t", 
                    fiArray[i].Name, 
                    fiArray[i].GetValue(this)==null?"NULL":fiArray[i].GetValue(this).ToString()));
            }
            return sb.ToString();
        }
    }
}
