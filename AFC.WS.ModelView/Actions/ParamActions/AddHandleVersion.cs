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

    public class AddHandleVersion:  IAction
    {

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            ParaManager pa = BuinessRule.GetInstace().paraManager;
            if (pa.IsExistParaInfo(ParaType))
            {
                MessageDialog.Show("此参数类型已经存在草稿版", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            ParaVersionInfo info = new ParaVersionInfo();
            info.para_type = ParaType;
            info.para_version = "0000";
            info.para_version_type = "00";
            int res = 0;
            switch (ParaType)
            {
                case "4043":
                    res = BuinessRule.GetInstace().paraManager.addHandle4043DraftPara(info);
                    break;
                case "4044":
                    res = BuinessRule.GetInstace().paraManager.addHandle4044DraftPara(info);
                    break;
                case "4045":
                    res= BuinessRule.GetInstace().paraManager.addHandle4045DraftPara(info);
                    break;
                case "4042":
                    res = BuinessRule.GetInstace().paraManager.add4042DraftPara(info);
                    break;
                case "4314":
                    res = BuinessRule.GetInstace().paraManager.add4314DraftPara(info);
                    break;
                case "0206":
                    res = BuinessRule.GetInstace().paraManager.add0206DraftPara(info);
                    break;
                default:
                    break;
            }

            if (res == 0)
            {
                MessageDialog.Show("草稿版参数增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Handle_Version, "0", "草稿版参数增加成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                MessageDialog.Show("草稿版参数增加失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Handle_Version, "1", "草稿版参数增加失败");
                return null;
            }
       
            
        }

        #endregion

        [Filter()]
        public string ParaType
        {
            get;
            set;
        }
    }
}
