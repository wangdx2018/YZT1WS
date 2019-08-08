using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 客户自定义的操作需要实现该接口 
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// 检查输入是否合法性
        /// </summary>
        ///<param name="actionParamsList">actionParams list的数据集合</param>
        /// <returns>检查合法 true，不合法 false</returns>
        bool CheckValid(List<QueryCondition> actionParamsList);
        
        /// <summary>
        /// 预留的权限接口
        /// </summary>
        /// <param name="authInfo"></param>
        /// <returns>符合该权限返回ture，否则false</returns>
        bool CheckPremission(object authInfo);

        /// <summary>
        /// 执行Action
        /// </summary>
        /// <param name="actionParamsList">actionParams list的数据集合</param>
        /// <returns>执行结果</returns>
        ResultStatus DoAction(List<QueryCondition> actionParamsList);
    }
}
