//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace TJComm
//{
//    using AFC.BJComm.Data;

//    /// <summary>
//    /// 天津报文的数据结构，每个报文的Body中
//    /// 都包括该两个内容
//    /// </summary>
//    public class CommBodyData
//    {
//        /// <summary>
//        /// 消息类型
//        /// </summary>
//        [PackOrder(1), PackInt(4, ByteOrder.Moto)]
//        public ushort messageType;

//        /// <summary>
//        /// 命令类型
//        /// </summary>
//        [PackOrder(2), PackInt(4, ByteOrder.Moto)]
//        public CommandType sessionFlag;

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="messageType">消息类型</param>
//        /// <param name="sessionFlag">命令类型</param>
//        public CommBodyData(ushort messageType, CommandType sessionFlag)
//        {
//            this.messageType = messageType;
//            this.sessionFlag = sessionFlag;
//        }

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="header">通用的数据类型</param>
//        public CommBodyData(CommHeader header)
//        {
//            this.messageType = header.messageType;
//            this.sessionFlag = header.sessionFlagMap;
//        }


//        public CommBodyData()
//        {

//        }

//    }
//}
