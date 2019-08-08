using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using System.Windows.Forms;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class DataFileDownAction:  IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Count < 1)
            {
                MessageDialog.Show("请选择要下载的数据文件!", "错误", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            //dusj modify begin 20121024 修改标识
            if (this.Status.Equals("1"))
            {
                FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                DialogResult result = folderBrowserDialog1.ShowDialog();
                string ftpDownPath = string.Empty;
                string serverPath = string.Empty;

                switch (this.Status)
                {
                    case "1":
                        serverPath = SysConfig.GetSysConfig().FtpDirectory.DealDataFilePath;
                        break;
                    case "2":
                        serverPath = SysConfig.GetSysConfig().FtpDirectory.UpDataFilePath;
                        break;
                }
                AFC.WS.BR.DataManager.DataManager dataManager = AFC.WS.BR.DataManager.DataManager.Instance;
                dataManager.AbortDataExportThread();
                if (result == DialogResult.OK)
                {
                    ftpDownPath = folderBrowserDialog1.SelectedPath;
                    var collection = actionParamsList.Where(temp => temp.bindingData.Equals("file_name")).ToList();
                    List<string> ftpFileNames = new List<string>();
                    for (int i = 0; i < collection.Count(); i++)
                    {
                        string currentFilename = collection[i].value.ToString();
                        ftpFileNames.Add(currentFilename);
                    }

                    dataManager.StartDataExportThread(serverPath, ftpDownPath, ftpFileNames);
                }
            }else
            {
                var collection = actionParamsList.Where(temp => temp.bindingData.Equals("file_name")).ToList();
                Util.DataBase.BeginTransaction();
                for (int i = 0; i < collection.Count(); i++)
                {
                    string Filename = collection[i].value.ToString();
                    DataFileUpInfo dataInfo =
                        DBCommon.Instance.GetModelValue<DataFileUpInfo>(
                            string.Format("select t.* from data_file_up_info t where t.file_name='{0}'", Filename));
                    if (dataInfo != null)
                    {
                        //状态变成未处理
                        dataInfo.up_result = "3";
                        List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
                        tempList.Add(new KeyValuePair<string, string>("FILE_NAME", dataInfo.file_name));
                        int result = DBCommon.Instance.UpdateTable(dataInfo, "data_file_up_info", tempList.ToArray());
                        if (result < 1)
                        {
                            MessageDialog.Show("处理失败", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information,
                                               AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                            Util.DataBase.Rollback();
                            return null;
                        }
                    }
                }
                Util.DataBase.Commit();
                MessageDialog.Show("处理成功", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information,
                                   AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
            }
            //dusj modify end 20121024 修改标识
          return new ResultStatus {resultCode = 0, resultData = 0};
        }


        /// <summary>
        /// 定义状态信息
        /// </summary>
        [Filter()]
        public string Status
        {
            set;
            get;
        }

        #endregion
    }
}
