using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace AFC.WS.BR.DataManager
{
    using System.Threading;
    using AFC.WS.UI.Common;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;
    using System.Data;
    using AFC.WS.UI.CommonControls;

    public class DataManager
    {
        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        private static DataManager _Instance;

        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        public static DataManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DataManager();
                }
                return _Instance;
            }
        }
        /// <summary>
        /// ftp下载时监听类
        /// </summary>
        private Thread dataExportThread = null;

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AbortDataExportThread()
        {
            if (dataExportThread == null)
            {
                WriteLog.Log_Error("plase call start thread  first!");
                return -1;
            }
            try
            {
                dataExportThread.Abort();
                dataExportThread = null;

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 启动FTP下载线程
        /// </summary>
        /// <param name="ftpPath">服务器的文件目录</param>
        /// <param name="exportPath">下载到本地的文件目录</param>
        /// <param name="filesName">要下载的文件名</param>
        public void StartDataExportThread(string ftpPath,string exportPath,List<string> filesName)
        {

           //dataExportThread = new Thread(new ThreadStart(() =>
           // {

           //     int time = 0;
               
                   ExportFiles(ftpPath, exportPath, filesName);
               
            //}));
            //dataExportThread.Name = "DataExportThread";
            //dataExportThread.IsBackground = true;
            //dataExportThread.Start();
        }


        private void ExportFiles(string ftpPath, string exportPath, List<string> filesName)
        {
            int count = 0;
            FTPCommon ftpComm = new FTPCommon(BuinessRule.GetInstace().GetFTPUserName(), BuinessRule.GetInstace().GetFTPUserPwd(), BuinessRule.GetInstace().GetFTPAddress());
            for (int i = 0; i < filesName.Count; i++)
            {
                string currentFtpFile = ftpPath + filesName[i].ToString();
                int result = ftpComm.FTPDownLoad(currentFtpFile, exportPath + "\\" + filesName[i].ToString());
                if (0 == result)
                {
                    count++;
                }
                else
                {
                    MessageDialog.Show(filesName[i].ToString() + "文件下载失败！", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                    AFC.WS.UI.Common.WriteLog.Log_Info(filesName[i].ToString() + "文件下载失败！");
                    return;
                }
            }

            MessageDialog.Show("文件下载成功！", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
          
        }
    }
}
