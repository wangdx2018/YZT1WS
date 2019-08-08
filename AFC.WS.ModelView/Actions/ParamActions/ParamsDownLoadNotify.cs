using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// 参数下载通知Action
    /// </summary>
    public class ParamsDownLoadNotify:IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null ||
                actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择需要下载的设备", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
           
            return true;
           // throw new NotImplementedException();
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //todo check download result ,when download succ then tip operator,else send download notify msg!

            string messageType = actionParamsList.Single(temp => temp.bindingData.Equals("para_type")).value.ToString();

         
            //todo call send notify msg

            return new ResultStatus { resultData = 0, resultCode = 0 };
            //throw new NotImplementedException();
        }

        #endregion
    }
}
