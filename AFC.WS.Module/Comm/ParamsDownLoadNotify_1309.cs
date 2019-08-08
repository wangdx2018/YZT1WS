using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;

    /// <summary>
    /// 参数下载通知 包括：ACC参数主控、ACC运营配置主控、LCC参数主控。 
    /// </summary>
    public class ParamsDownLoadNotify_1309 : AbstractCommBody
    {

        /// <summary>
        /// 参数类型
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public ushort paramType;
    }
}
