using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.DataImportExport
{
    public class MemIndexFileBody:Body
    {
        /// <summary>
        /// 文件记录循环体
        /// </summary>
        public List<MemIndexFile> listMemIndexFile = new List<MemIndexFile>();

    }

    public class MemIndexFile
    {
        /// <summary>
        /// 文件数量
        /// </summary>
        public string fileCount;

        /// <summary>
        /// 文件名称
        /// </summary>
        public List<string> listFileName = new List<string>();
    }
}
