using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.DataImportExport
{
    public interface IIndexFileHandler
    {
        /// <summary>
        /// 解析索引文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        List<string> ParseIndexFile(string fileName);

        /// <summary>
        /// 创建所有文件
        /// </summary>
        /// <param name="fileInstance"></param>
        /// <returns></returns>
        int CreateIndexFile(string downPath,IndexFileData fileData);
    }
}
