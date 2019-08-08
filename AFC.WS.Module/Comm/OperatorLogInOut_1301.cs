using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;
    /// <summary>
    /// added by wangdx: 20110311
    /// 操作员登录登出报文结构
    /// </summary>
    public class OperatorLogInOut_1301 : AbstractCommBody
    {
        [PackOrder(3)]
        [PackInt(4,ByteOrder.Moto)]
        public uint operatorId;

        /// <summary>
        /// 操作员密码
        /// </summary>
        [PackOrder(4)]
        [PackArray(4, ByteOrder.Moto, 1, ByteOrder.Moto,isXDR=true)]
        public string password;


        /// <summary>
        /// 操作员登录，登出标志 0：登录，1：登出
        /// </summary>
        [PackOrder(5),PackInt(4,ByteOrder.Moto)]
        public int loginType;

    }
}
