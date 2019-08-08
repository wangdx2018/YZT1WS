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


    public class UpdateParaDraftVersion : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            return true;
            //throw new NotImplementedException();
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            Type type = Type.GetType(ClassTypeName);
            if (type == null)
                throw new Exception("Get action Type error [" + ClassTypeName + "]");

            Object obj = DBCommon.Instance.CreateModelData(ClassTypeName, actionParamsList);
            int res = BuinessRule.GetInstace().paraManager.updateDraftPara(TableName, obj);

            if (res == 0)
            {
                MessageDialog.Show("草稿版参数修改成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Para_Draft_Version, "0", "草稿版参数修改成功");
            }
            else
            {
                MessageDialog.Show("草稿版参数修改失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Para_Draft_Version, "1", "草稿版参数修改失败");
            }
            return null;

        }
        


        [Filter()]
        public string ClassTypeName
        {
            get;
            set;
        }
        [Filter()]
        public string TableName
        {
            get;
            set;
        }

        #endregion
    }
}
