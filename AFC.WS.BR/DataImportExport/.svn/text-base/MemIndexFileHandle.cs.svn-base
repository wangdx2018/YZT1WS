using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace AFC.WS.BR.DataImportExport
{
    public class MemIndexFileHandle : IIndexFileHandler
    {
        #region IIndexFileHandler 成员

        public List<string> ParseIndexFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            string datFileName = Path.GetFileName(fileName);
            if (!datFileName.Equals("MemoryIndexFile.dat"))
                return null;
            AccessDatFile datFileOper = new AccessDatFile(fileName);
            string strDevCount = datFileOper.DatReadValue("FILE", "DeviceCount");
            if (string.IsNullOrEmpty(strDevCount))
                return null;
            int devCount = int.Parse(strDevCount);
            string[] deviceID = datFileOper.getDevName(strDevCount);
            List<string> listMenName = new List<string>();
            for (int i = 0; i < deviceID.Length; i++)
            {
                int fileCount = int.Parse(datFileOper.DatReadValue(deviceID[i], "FileCount"));
                for (int j = 0; j < fileCount; j++)
                {
                    string strFileName = datFileOper.DatReadValue(deviceID[i], "FileName_" + (j + 1).ToString("d4"));
                    listMenName.Add(strFileName);
                }
            }
            if (listMenName == null || listMenName.Count == 0)
                return null;
            return listMenName;
        }

        public int CreateIndexFile(string exportPath, IndexFileData fileData)
        {
            AccessDatFile accessDatFile = null;
            HeaderExtern header = fileData.header as HeaderExtern;
            MemIndexFileBody memBody = fileData.body as MemIndexFileBody;
            FileInfo indexFile = new FileInfo(exportPath + "MemoryIndexFile.dat");
            if (indexFile.Exists)
            {
                indexFile.Delete();
            }
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            accessDatFile = new AccessDatFile(exportPath + "MemoryIndexFile.dat");
            accessDatFile.DatWriteValue("FILE", "FileType", header.fileType);
            accessDatFile.DatWriteValue("FILE", "CreatedTime", header.createTime);
            accessDatFile.DatWriteValue("FILE", "ModifiedTime", header.modifiedType);
            accessDatFile.DatWriteValue("FILE", "DeviceCount", header.fileCount);
            for (int i = 0; i < Int32.Parse(header.fileCount); i++)
            {
                string strFileCount = memBody.listMemIndexFile[i].fileCount;
                accessDatFile.DatWriteValue("FILE", "DeviceID" + (i + 1).ToString(), header.listDevice[i]);
                accessDatFile.DatWriteValue(header.listDevice[i], "FileCount", strFileCount);
                for (int j = 0; j < Int32.Parse(strFileCount); j++)
                {
                    accessDatFile.DatWriteValue(header.listDevice[i], "FileName_" + (j + 1).ToString("d4"), ImportExportManager.ConvertPathToPositive(memBody.listMemIndexFile[i].listFileName[j]));//将路径中的反斜杠变成正斜杠
                }
            }
            if (accessDatFile == null)
                return -1;
            return 0;
        }
        #endregion
    }
}
