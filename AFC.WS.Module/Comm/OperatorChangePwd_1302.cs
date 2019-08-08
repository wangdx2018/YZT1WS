using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;
    /// <summary>
    /// 
    /// 操作员修改密码
    /// </summary>
    public class OperatorChangePwd_1302 : AbstractCommBody
    {

        /// <summary>
        ///被修改的 操作员ID
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint operatorId;

        /// <summary>
        /// 操作员旧密码
        /// </summary>
        [PackOrder(4),PackArray(4,ByteOrder.Moto,1,ByteOrder.Moto,isXDR=true)]
        public string oldPwd;

        /// <summary>
        /// 操作员新密码
        /// </summary>
        [PackOrder(5),PackArray(4,ByteOrder.Moto,1,ByteOrder.Moto,isXDR=true)]
        public string newPwd;

        /// <summary>
        /// 修改原因 密码修改原因：1：密码将过期 2：密码修改 3、首次登录
        /// </summary> 
        [PackOrder(6),PackInt(4,ByteOrder.Moto)]
        public byte change_reason;


    }
}
