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
    public class DelParaDraftVersion  :IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要删除的草稿版的参数类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                string paraType = actionParamsList.Single(temp => temp.bindingData.Equals("para_type")).value.ToString();
                string paraVersion = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();

                if (paraVersion != "0000")
                {
                    MessageDialog.Show("请选择草稿版", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            throw new NotImplementedException();
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
                    res=BuinessRule.GetInstace().paraManager.del4043DraftPara(info);
                    break;
                case "4044":
                    res=BuinessRule.GetInstace().paraManager.del4044DraftPara(info);
                    break;
                case "4045":
                    res=BuinessRule.GetInstace().paraManager.del4045DraftPara(info);
                    break;
                case "4042":
                    res = BuinessRule.GetInstace().paraManager.del4042DraftPara(info);
                    break;
                case "4314":
                    res = BuinessRule.GetInstace().paraManager.del4314DraftPara(info);
                    break;
                case "0206":
                    res = BuinessRule.GetInstace().paraManager.del0206DraftPara(info);
                    break;
                default:
                    break;
            }
            if (res == 0)
            {
                MessageDialog.Show("草稿版参数删除成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Del_Para_Draft_Version, "0", "草稿版参数删除成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                MessageDialog.Show("草稿版参数删除失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Del_Para_Draft_Version, "1", "草稿版参数删除失败");
            }
            return null;
        }

        #endregion
    }
}
