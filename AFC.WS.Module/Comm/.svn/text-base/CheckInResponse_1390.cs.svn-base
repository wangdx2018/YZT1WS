using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    /// <summary>
    /// 设备签到认证应答
    /// </summary>
    public class CheckInResponse_1390:AbstractCommBody
    {
        /// <summary>
        /// 结果代码 0、成功；1、失败
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public byte resultCode;

        /// <summary>
        /// 附加代码 错误原因详细代码
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public int resultStatusCode;

        /// <summary>
        /// FTP操作员
        /// </summary>
        [PackOrder(5),PackArray(4, ByteOrder.Moto, 1, ByteOrder.Moto,isXDR=true)]
        public string ftpUser;

        /// <summary>
        /// FTP密码
        /// </summary>
        [PackOrder(6),PackArray(4, ByteOrder.Moto, 1, ByteOrder.Moto,isXDR=true)]
        public string ftpPwd;
    }
}
