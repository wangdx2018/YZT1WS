using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.BR.ParamsManager;
    using AFC.WS.Model.Const;

    /// <summary>
    /// 软件版本制作Action
    /// </summary>
    public class SoftWareUpLoadAction: IAction
    {

        private string publishDate;
        private string publishTime;
        private  string filePath;
        private  string softwareType;
        private uint softwareNo=0;

        #region IAction 成员

        /// <summary>
        /// 检查publishDate,publishTime
        /// </summary>
        /// <param name="actionParamsList"></param>
        /// <returns></returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
      

            //#region Check PublishDate
            //try
            //{
            //    publishDate = actionParamsList.Single(temp => temp.bindingData.Equals("publishDate")).value.ToString();
            //}
            //catch (Exception ex)
            //{
            //    WriteLog.Log_Error(ex.Message);
            //    MessageDialog.Show("请选择发布日期!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //#endregion

            //#region check publishTime
            //try
            //{
            //    publishTime = actionParamsList.Single(temp => temp.bindingData.Equals("publishTime")).value.ToString();
            //}
            //catch (Exception ex)
            //{
            //    WriteLog.Log_Error(ex.Message);
            //    MessageDialog.Show("请选择发布时间!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //#endregion

            #region check filePath
            try
            {
                filePath = actionParamsList.Single(temp => temp.bindingData.Equals("filePath")).value.ToString();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                MessageDialog.Show("请选择上传软件的文件名!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            #endregion

            //#region check softwareNo
            //try
            //{
            //    softwareNo = actionParamsList.Single(temp => temp.bindingData.Equals("softwareNo")).value.ToString();
            //}
            //catch (Exception ex)
            //{
            //    WriteLog.Log_Error(ex.Message);
            //    MessageDialog.Show("请输入软件批次号!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //#endregion

            #region check software type
            try
            {
                softwareType = actionParamsList.Single(temp => temp.bindingData.Equals("softwareType")).value.ToString();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                MessageDialog.Show("请选择上传的软件类型!", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            #endregion

            //#region check current date is more now

            //DateTime softwarePublishDate = DateTime.ParseExact(publishDate, "yyyy-MM-dd", null);
            //if (softwarePublishDate.Subtract(DateTime.Now).TotalDays <= 0)
            //{
            //    MessageDialog.Show("发布日期应该大于当前日期", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            //    return false;
            //}
            //#endregion

            #region software no
            if (softwareType.Equals("4306"))
            {
                try
                {
                    uint paramVersion = actionParamsList.Single(temp => temp.bindingData.Equals("versionNo")).value.ToString().ConvertNumberStringToUint();
                    int currentNo = BuinessRule.GetInstace().paraManager.GetCurrentParamVersionNo(softwareType);
                    if (paramVersion <= currentNo)
                    {
                        MessageDialog.Show("输入的版本号不应该小于" + currentNo.ToString(),
                            "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return false;
                    }
                    this.softwareNo = paramVersion;
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                }
            }

            #endregion

            string keyWord=string.Empty;
            bool res= CheckSoftwareName(softwareType, filePath,out keyWord);

            if (!res)
            {
                MessageDialog.Show("软件名不合法,文件名必须包含"+keyWord+"!", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return res;
            }
            /* 1.检查文件名是否合法
             * 
             * 2. 检查生效日期不能小于当前日期
             * 
             * 3. 类型不能为空，日期不能为空，生效时间不能为空，文件名不能为空
             * 
             */
    
            return true;
           // throw new NotImplementedException();
        }

        public bool CheckPremission(object authInfo)
        {
          
            throw new NotImplementedException();
        }

        /// <summary>
        ///    /*
        ///    ** 001 需要传递类型
        ///    *   002：文件名称
        ///    *   003：软件批次号
        ///    *   004：发布日期
        ///    *   005：发布时间
        ///    *   
        ///    * 1.根据发布的软件类型在相应的CSSFIleType_t中添加一条记录
        ///      * 
        ///    * 插入一条记录草稿版，如果系统中存在了删除了草稿版，提示操作员
        ///      * 
        ///   * 系统有存在一个草稿版本是否将其删除。如果删除则在数据库中先将
        ///    * 
        /// * 以前的草稿版本删除，然后插入一条草稿版本。
        /// * 
        ///     * 2.将文件FTP到All目录下。
        ///   * 
        /// * 3.软件版本制作完成，提示进入参数发布界面选中该软件进行发布。
        ///  *   
        ///     * 4.完成
        ///   *
        ///    */
        /// </summary>
        /// <param name="actionParamsList"></param>
        /// <returns></returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            Util.DataBase.BeginTransaction();

            ParaVersionInfo pvi = BuinessRule.GetInstace().paraManager.GetParaByVersion(this.softwareType, "0000");
            int result;
            string[] nameArray=filePath.Split('\\');
            string fileName=nameArray[nameArray.Length-1];
            if (pvi != null&&!string.IsNullOrEmpty(pvi.para_type))
            {
               MessageBoxResult res=
                   MessageDialog.Show("已经存在了该软件的草稿版,如果继续将覆盖上次未发布的版本，\r\n是否发布新的软件？", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
               if (res == MessageBoxResult.Yes)
               {
                   //001delete params
                   // 002 d 
                   result = Draft4044ParaDel.DelParaVersionInfo(softwareType, "0000");
                   if (res == 0)
                   {
                       MessageDialog.Show("删除草稿版失败！", "提示", MessageBoxIcon.Question, MessageBoxButtons.Ok);
                       BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.SoftWare_UpLoad_Action, "1", "删除草稿版失败");
                       return null;
                   }
               }
            }
                   pvi = new ParaVersionInfo();
                   pvi.para_type = this.softwareType;
                   pvi.para_version = "0000";
                   pvi.update_date = DateTime.Now.ToString("yyyyMMdd");
                   pvi.update_time = DateTime.Now.ToString("HHmmss");
                   pvi.master_para_version = "0000";
                   pvi.active_date = DateTime.Now.ToString("yyyyMMdd");
                   pvi.active_time = DateTime.Now.ToString("HHmmss");
                   pvi.para_or_soft_flag = "02";
                   //pvi.para_sub_type = "0";
                   pvi.para_file_name = fileName;
                   pvi.master_para_type = this.softwareType;
                   pvi.occur_date = pvi.update_date;
                   pvi.occur_time = pvi.update_time;
                   pvi.para_version_type = "00";//草稿版 
                   //获取当前版本的最高版本
                   int iVersionNo = BuinessRule.GetInstace().paraManager.GetCurrentParamVersionNo(this.softwareType);
                   //PRO.XXXX.AA.LLLL.VVVV. FILENAME .YYYYMMDDhh24mmss
                   pvi.para_file_name = "PRO." + this.softwareType + "." + "99" + "." + "0199" + "." + (iVersionNo+1).ToString("D4") + "."+ fileName + "." + DateTime.Now.ToString("yyyyMMddHHmmss");
                   result = DBCommon.Instance.InsertTable(pvi, "para_version_info");
                   if (result != 1)
                   {
                       MessageDialog.Show("增加草稿版本失败！", "提示", MessageBoxIcon.Question, MessageBoxButtons.Ok);
                       Util.DataBase.Rollback();
                       BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.SoftWare_UpLoad_Action, "1", "增加草稿版本失败");
                       return null;
                   }
              
            string ftpUser = BuinessRule.GetInstace().GetFTPUserName();
            string ftpPwd = BuinessRule.GetInstace().GetFTPUserPwd();
            string ftpAddress = BuinessRule.GetInstace().GetFTPAddress();
            string ftpPath = BuinessRule.GetInstace().GetFtpSoftwarePath();
            if (string.IsNullOrEmpty(ftpAddress) ||
                string.IsNullOrEmpty(ftpPwd) ||
                string.IsNullOrEmpty(ftpAddress)||
                string.IsNullOrEmpty(ftpPath))
            {    
                MessageDialog.Show("获取FTP信息失败，无法上传！", "提示", MessageBoxIcon.Question, MessageBoxButtons.Ok);
                Util.DataBase.Rollback();

                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.SoftWare_UpLoad_Action, "1", "获取FTP信息失败，无法上传");
                return null;
            }

            WriteLog.Log_Info("ftpUser=" + ftpUser + ",ftpPwd=" + ftpPwd + ",ftpAddress=" + ftpAddress + ",ftpPath=" + ftpPath);

            FTPCommon ftpComm = new FTPCommon(ftpUser, ftpPwd, ftpAddress);
                
                 result = ftpComm.FTPUpLoad(this.filePath,ftpPath, nameArray[nameArray.Length - 1]);
                 if (result == 0)
                 {
                     if (softwareNo != 0) //选择文件的版本信息
                     {
                        bool isUpdate= BuinessRule.GetInstace().paraManager.UpdateMaxParamVersionNo(softwareType, softwareNo);
                        if (!isUpdate)
                        {
                            MessageDialog.Show("更新最大版本信息失败", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                            return null;
                        }
                     }
                     //todoUpdateParamsCode
                     MessageDialog.Show("软件已经上传到服务器上\r\n请进入参数发布选择该软件进行发布!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                     Util.DataBase.Commit();//提交事务
                     BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.SoftWare_UpLoad_Action, "0", "软件已经上传到服务器上");
                     return new ResultStatus { resultCode = 0, resultData = 0 };
                 }
                 else
                 {
                     MessageDialog.Show("文件FTP上传失败,请重新上传！！", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                     Util.DataBase.Rollback();
                     BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.SoftWare_UpLoad_Action, "1", "草稿版插入成功，上传FTP失败");
                     return null;
                 }

            return null;


            
        }


        /// <summary>
        /// 检查设备软件的文件名
        /// </summary>
        /// <param name="softwareType">软件类型</param>
        /// <param name="filePath">文件名称</param>
        /// <returns>合法返回true，否则返回false</returns>
        private bool CheckSoftwareName(string softwareType, string filePath,out string keyWord)
        {
            keyWord = string.Empty;
            if (string.IsNullOrEmpty(softwareType) || string.IsNullOrEmpty(filePath))
            {
                WriteLog.Log_Error("sofewrare type is null or empty,filePath is null or empty!");
                return false;
            }
            string[] fileNameSplitArray = filePath.Split('\\');
            string fileName = fileNameSplitArray[fileNameSplitArray.Length - 1];
            switch (softwareType)
            {
                //case "FF05":
                //    keyWord = "TVMSoftware";
                //    return fileName.Contains("TVMSoftware");
                //case "4302":
                //    keyWord = "BOMSoftware";
                //    return fileName.Contains("BOMSoftware");
                //case "4303":
                //    keyWord = "AGSoftware";
                //    return fileName.Contains("AGSoftware");
                //case "4304":
                //    keyWord = "EQMSoftware";
                //    return fileName.Contains("EQMSoftware");
                //case "4305":
                //    keyWord = "Tpu1Software";
                //    return fileName.Contains("Tpu1Software");
                //case "4306":
                //    keyWord = "Tpu2Software";
                //    return fileName.Contains("Tpu2Software");
                //case "4310":
                //    keyWord = "SCWSSoftware";
                //    return fileName.Contains("SCWSSoftware");
                //case "4311":
                //    keyWord = "LCWSSoftware";
                //    return fileName.Contains("LCWSSoftware");
                case "FF05":
                    keyWord = "TVM";
                    return fileName.Contains("TVM");
                case "FF11":
                    keyWord = "AGM";
                    return fileName.Contains("AGM");
                case "FF16":
                    keyWord = "PCA";
                    return fileName.Contains("PCA");
                case "FF18":
                    keyWord = "SDG";
                    return fileName.Contains("SDG");
                case "FF20":
                    keyWord = "IC";
                    return fileName.Contains("IC");
                case "0022":
                    keyWord = "TPU";
                    return fileName.Contains("TPU");
                case "4311":
                    keyWord = "LCWS";
                    return fileName.Contains("LCWS");
                default:
                    return false;
            }


        
        }

        private string GetFormatTime(string value)
        {
            StringBuilder sb = new StringBuilder();
            string[] array = value.Split(':');
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]);
            }
            return sb.ToString();
        }



        #endregion
    }
}
