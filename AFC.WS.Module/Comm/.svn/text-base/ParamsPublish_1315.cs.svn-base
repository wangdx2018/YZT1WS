using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;
    /// <summary>
    /// 参数发布
    /// </summary>
    public class ParamsPublish_1315 : AbstractCommBody
    {

        /// <summary>
        /// 生效日期
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint activeDate;

        /// <summary>
        /// 发布时间
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public uint publishDate;

        /// <summary>
        /// 参数类型
        /// </summary>
        [PackOrder(5),PackArray(4,ByteOrder.Moto,4,ByteOrder.Moto)]
        public List<ushort> listParamType = new List<ushort>();
    }
}
