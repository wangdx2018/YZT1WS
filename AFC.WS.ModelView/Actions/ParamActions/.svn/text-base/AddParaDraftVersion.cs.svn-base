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

    public class AddParaDraftVersion:  IAction
    {
        #region IAction 成员

        ParaManager pa = BuinessRule.GetInstace().paraManager;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要增加的草稿版的参数类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                string paraType = actionParamsList.Single(temp => temp.bindingData.Equals("para_type")).value.ToString();
                string paraVersion = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();
               

                if (paraVersion == "-1")
                {
                    MessageDialog.Show("请选择正式版", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                if (pa.IsExistParaInfo(paraType))
                {
                    MessageDialog.Show("此参数类型已经存在草稿版", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            string paraType = actionParamsList.Single(temp => temp.bindingData.Equals("para_type")).value.ToString();
            string paraVersion = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();
            ParaVersionInfo info = BuinessRule.GetInstace().paraManager.GetParaByVersion(paraType, paraVersion);
            int res = 0;
            switch (paraType)
            {
                case "4043":
                    res = BuinessRule.GetInstace().paraManager.add4043DraftPara(info);
                    break;
                case "4044":
                    res = BuinessRule.GetInstace().paraManager.add4044DraftPara(info);
                    break;
                case "4045":
                   res= BuinessRule.GetInstace().paraManager.add4045DraftPara(info);
                    break;
                default:
                    break;
            }

            if (res == 0)
            {
                MessageDialog.Show("草稿版参数增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Handle_Version, "0", "草稿版参数增加成功");
            }
            else
            {
                MessageDialog.Show("草稿版参数增加失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Handle_Version, "1", "草稿版参数增加失败");
            }
           return null;

        }


    
        #endregion
    }
}
