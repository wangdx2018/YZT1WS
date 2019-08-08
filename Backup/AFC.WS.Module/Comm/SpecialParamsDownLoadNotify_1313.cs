using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;
    using TJComm;

    /// <summary>
    /// 特定参数下载通知
    /// </summary>
    public class SpecialParamsDownLoadNotify_1313 : AbstractCommBody
    {   
   
        [PackOrder(3)]
        [PackArray(4,ByteOrder.Moto,0,ByteOrder.Moto,null)]
        public List<ParamsData> parmsData = new List<ParamsData>();
    }

    public class ParamsData
    {

        /// <summary>
        /// 参数类型
        /// </summary>
        [PackOrder(1),PackInt(4,ByteOrder.Moto)]
        public ushort paramType;

        /// <summary>
        /// 参数版本号
        /// </summary>
        [PackOrder(2),PackInt(4,ByteOrder.Moto)]
        public ushort paramVersion;

        /// <summary>
        /// 参数文件名
        /// </summary>
        [PackOrder(3), PackArray(4, ByteOrder.Moto, 1, ByteOrder.Moto, isXDR = true)]
        public string paramFileName;


        /// <summary>
        /// 同步方式，
        /// 0：普通同步
        /// 1：指定同步
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public byte paramSynFlag;


    }
}
