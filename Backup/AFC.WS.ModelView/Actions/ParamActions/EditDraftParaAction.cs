using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    using System.Data;
    using System.Reflection;
    using AFC.WorkStation.DB;
    using System.Configuration;
    using System.Windows;

    public class EditDraftParaAction:  IAction
    {
        #region IAction 成员
        //定义变量
        string paraType = string.Empty;
        string paraVersion = string.Empty;
        string paraMasterType = string.Empty;
        string paraFileName = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要制作草稿版的参数模板", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            
            try
            {
                paraType = actionParamsList.Single(temp => temp.bindingData.Equals("para_type")).value.ToString();
                paraVersion = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();
                paraMasterType = actionParamsList.Single(temp => temp.bindingData.Equals("para_master_type")).value.ToString();
                paraFileName = actionParamsList.Single(temp => temp.bindingData.Equals("para_file_name")).value.ToString();

                if ("4044".Equals(paraType) || "4045".Equals(paraType) || "4043".Equals(paraType) || "4042".Equals(paraType) || "4314".Equals(paraType))
                {
                }
                else
                {
                    MessageDialog.Show("此参数不是可编辑参数，不能生成草稿版。", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
               
                return true;
            }

            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            if (BuinessRule.GetInstace().paraManager.IsExistParaInfo(paraType))
            {
                if (Wrapper.ShowDialog("此参数类型已存在草稿版，是否删除已存在的草稿版") == MessageBoxResult.No)
                {
                    return null;
                }
            }
            
            
            int insertRes = BuinessRule.GetInstace().paraManager.InsertParaVersion(paraType, paraFileName, paraMasterType);
            Util.DataBase.BeginTransaction();
            int res = 0;
            try
            {

                switch (paraType)
                {
                    case "4044":
                        Param4044Added para4044 =new Param4044Added();
                        res = para4044.AddParamsData(paraVersion);
                        break;
                    case "4045":
                        Para4045Added para4045 = new Para4045Added();
                        res = para4045.AddParamsData(paraVersion);
                        break;
                    case "4043":
                        Param4043Added para4043 = new Param4043Added();
                        res = para4043.AddParamsData(paraVersion);
                        break;
                    case "4042":
                        Para4042Added para4042 = new Para4042Added();
                        res = para4042.AddParamsData(paraVersion);
                        break;
                    case "4314":
                        Para4314Added para4314 = new Para4314Added();
                        res = para4314.AddParamsData(paraVersion);
                        break;
                    default:
                        break;
                }
                if (insertRes == 0 && res == 0)
                {
                    MessageDialog.Show("草稿版参数增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Handle_Version, "0", "草稿版参数增加成功");
                    Util.DataBase.Commit();
                }
                else
                {
                    MessageDialog.Show("草稿版参数增加失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    Util.DataBase.Rollback();
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Handle_Version, "1", "草稿版参数增加失败");
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }

            return null;
        }
           
    }
     #endregion 

}
