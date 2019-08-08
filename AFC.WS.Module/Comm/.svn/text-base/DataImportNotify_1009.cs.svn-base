using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    public class DataImportNotify_1009 : AbstractCommBody
    {
        ///// <summary>
        ///// 0x00：操作成功；
        /////0x01：操作失败；
        /////0x03：部分数据操作失败
        ///// </summary>
        //public byte importReslut;

        /// <summary>
        /// 导入类型	ImportType	1	U8	
        /// 0x00：交易数据文件；
        /// 0x01：业务数据文件；
        /// 0x03：参数文件；
        /// 0x04：程序文件
        /// </summary>
        [PackOrder(3),PackInt(1,ByteOrder.Moto)]
        public byte importType;

        /// <summary>
        /// 导入目录	ImportPath	UncodeString	128	导入的数据在系统中的存放路径（绝对路径）
        /// </summary>
        [PackOrder(4), PackString(128)]
        public string importPath = string.Empty;

        ///// <summary>
        ///// 导入的数据总数
        ///// </summary>
        //public uint importTotalFileCount;

        ///// <summary>
        ///// 导出处理出错文件数
        ///// </summary>
        //public uint dealFailCount;

    }
}
