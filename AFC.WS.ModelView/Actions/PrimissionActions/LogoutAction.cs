using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.Const;
    using AFC.BOM2.MessageDispacher;

    public class LogoutAction: IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //todoswitch ui
            //todosend to sc
            //todoset br currentOperatorId  null
            //todosend msg to ui change tip is 登录

            System.Windows.MessageBoxResult result = MessageDialog.Show("您是否要登出系统", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                Message msg = new Message();
                msg.MessageType = SynMessageType.LogOut;
                MessageManager.SendMessasge(msg);
                
            }
            return null;
        }

        #endregion
    }
}
