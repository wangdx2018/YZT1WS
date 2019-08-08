using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.BJComm.Data;

namespace AFC.WS.BR.DataImportExport
{
    public class Body : IMutableInstance
    {

        #region IMutableInstance 成员

        public object JudgePackedInstance(object parent, System.IO.MemoryStream s)
        {
            IndexFileData fileData=parent as IndexFileData;
            if (fileData != null)
            {
                return IndexFileData.CreateBody(fileData.operateType);
            }
            return null;
        }

        #endregion
    }
}
