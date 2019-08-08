using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AFC.WS.BR.DataImportExport
{
    internal class DiskOperation
    {
        /// <summary>
        /// 磁盘初始化
        /// </summary>
        /// <param name="diskPhysicalNo">磁盘物理号</param>
        /// <returns></returns>
        public int InitValidDisk(string diskPhysicalNo)
        {
            return 0;
        }

        /// <summary>
        /// 检查磁盘认证文件
        /// </summary>
        /// <returns></returns>
        public bool CheckDiskValid()
        {
            ValidateAuthPhysicalSN validAuthPhysical = new ValidateAuthPhysicalSN();
            string PathU = validAuthPhysical.getFirstUSB();
            if (string.IsNullOrEmpty(PathU))
                return false;
            if (validAuthPhysical.readConf(Path.GetPathRoot(PathU)))
            {
                return true;   
            }
            else
            {
                return false;
            }
        }


    }
}
