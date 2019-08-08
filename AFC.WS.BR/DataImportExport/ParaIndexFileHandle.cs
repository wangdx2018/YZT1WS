using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AFC.WS.BR.DataImportExport
{
    public class ParaIndexFileHandle:IIndexFileHandler
    {
        #region IIndexFileHandler 成员

        public List<string> ParseIndexFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))//此处fileName为带路径的文件名
                return null;
            string datFileName = Path.GetFileName(fileName);
            if (!datFileName.Equals("ParameterIndexFile.dat"))
                return null;
            AccessDatFile datFileOper = new AccessDatFile(fileName);
            string strParaCount = datFileOper.DatReadValue("FILE", "ParameterFileCount");
            if (string.IsNullOrEmpty(strParaCount))
                return null;

            List<string> listParams=new List<string>();
            for(int i=0;i<Convert.ToUInt32(strParaCount);i++)
            {
                listParams.Add((i+1).ToString("d4"));
            }

            List<string> listFileName = new List<string>();
            foreach (var temp in listParams)
            {
                string paraFileName = datFileOper.DatReadValue("Parameter"+temp, "Filename");
                listFileName.Add(paraFileName);
            }

            if (listFileName == null || listFileName.Count == 0)
                return null;
            return listFileName;
        }

        public int CreateIndexFile(string exportPath, IndexFileData fileContent)
        {
            AccessDatFile accessDateFile = null;
            Header header = fileContent.header as Header;
            ParamIndexFileBody paraBody = fileContent.body as ParamIndexFileBody;
            FileInfo indexFile = new FileInfo(exportPath + "ParameterIndexFile.dat");
            if (indexFile.Exists)
            {
                indexFile.Delete();
            }
            accessDateFile = new AccessDatFile(exportPath + "ParameterIndexFile.dat");
            accessDateFile.DatWriteValue("FILE", "FileType", header.fileType);
            accessDateFile.DatWriteValue("FILE", "CreatedTime", header.createTime);
            accessDateFile.DatWriteValue("FILE", "ModifiedTime", header.modifiedType);
            accessDateFile.DatWriteValue("FILE", "ParameterFileCount", header.fileCount);
            for (int i = 0; i < Int32.Parse(header.fileCount); i++)
            {
                string paraNo = "Parameter" + (i+1).ToString("d4");
                accessDateFile.DatWriteValue(paraNo, "ParameterType", paraBody.listParam[i].parameterType);
                accessDateFile.DatWriteValue(paraNo, "ParameterVersion", paraBody.listParam[i].parameterVersion);
                accessDateFile.DatWriteValue(paraNo, "VersionType", paraBody.listParam[i].versionType);
                accessDateFile.DatWriteValue(paraNo, "ActiveDateTime", paraBody.listParam[i].activeDateTime);
                accessDateFile.DatWriteValue(paraNo, "Filename", ImportExportManager.ConvertPathToPositive(paraBody.listParam[i].fileName));//将路径中的反斜杠变成正斜杠
                accessDateFile.DatWriteValue(paraNo, "ManufactureID", paraBody.listParam[i].manufactureID);
                accessDateFile.DatWriteValue(paraNo, "DevicePartID", paraBody.listParam[i].devicePartID);
            }
            if (accessDateFile == null)
                return -1;
            return 0;
        }

        #endregion
    }
}
