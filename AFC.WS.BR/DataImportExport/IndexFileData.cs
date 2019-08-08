using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.DataImportExport
{
    public class IndexFileData 
    {
        /// <summary>
        /// 索引文件头
        /// </summary>
        public Header header;

        /// <summary>
        /// 索引文件体
        /// </summary>
        public Body body;

        /// <summary>
        /// 索引文件类型
        /// </summary>
        public OperateType operateType;

        /// <summary>
        /// 创建索引文件体
        /// </summary>
        /// <param name="operateType"></param>
        /// <returns></returns>
        internal static Body CreateBody(OperateType operateType)
        {
            Body body;
            switch (operateType)
            {
                case OperateType.BUSI_DATA_FILE:
                case OperateType.TRADE_DATA_FILE:
                    body = new MemIndexFileBody();
                    break;
                case OperateType.PARA_DATA_FILE:
                case OperateType.SOFT_DATA_FILE:
                    body = new ParamIndexFileBody();
                    break;
                default:
                    return null;
            }
            return body;
        }

        /// <summary>
        /// 创建索引文件头
        /// </summary>
        /// <param name="operateType"></param>
        /// <returns></returns>
        internal static Header CreateHeader(OperateType operateType)
        {
            Header header;
            switch (operateType)
            {
                case OperateType.BUSI_DATA_FILE:
                case OperateType.TRADE_DATA_FILE:
                    header = new HeaderExtern();
                    break;
                case OperateType.PARA_DATA_FILE:
                case OperateType.SOFT_DATA_FILE:
                    header = new Header();
                    break;
                default:
                    return null;
            }
            return header;
        }

        /// <summary>
        /// 创建索引文件
        /// </summary>
        /// <param name="operateType"></param>
        /// <returns></returns>
        public static IndexFileData CreateIndexFileData(OperateType operateType)
        {
            IndexFileData indexFileData = new IndexFileData();
            indexFileData.operateType = operateType;
            indexFileData.header = CreateHeader(operateType);
            indexFileData.body = CreateBody(operateType);
            return indexFileData;
        }


    }
}
