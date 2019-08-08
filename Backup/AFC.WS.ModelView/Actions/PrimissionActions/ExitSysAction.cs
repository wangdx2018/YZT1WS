using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;


    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110225
    /// 代码功能：退出系统功能。
    /// 修订记录：editor by wangdx 20130205 修改了 ALT+F4后进程无法退出的问题
    /// </summary>
    public class ExitSysAction: IAction
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
           System.Windows.MessageBoxResult res=
               AFC.WS.UI.CommonControls.MessageDialog.Show("是否要退出WS系统？", "提示", MessageBoxIcon.Question,MessageBoxButtons.YesNo);
           if (res == MessageBoxResult.Yes)
           {
               Environment.Exit(0);
               return new ResultStatus { resultCode = 0, resultData = 0 };
           }
           return null;
        }

        #endregion
    }
}
