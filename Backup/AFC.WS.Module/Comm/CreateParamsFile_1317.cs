using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    /// <summary>
    /// 手工生成参数文件
    /// </summary>
    public class CreateParamsFile_1317:AbstractCommBody
    {
        /// <summary>
        /// 预留
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint remark;
    }
}
