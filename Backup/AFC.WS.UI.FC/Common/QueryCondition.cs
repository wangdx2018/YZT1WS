using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 修改人：王冬欣 20100319 增加了控件的名字
    /// 和控件Label显示的文字
    /// Action中需要传递到DoAction中的参数
    /// </summary>
    public class QueryCondition
    {
        /// <summary>
        /// 绑定字段
        /// </summary>
        public string bindingData;

        /// <summary>
        /// 绑定的数值
        /// </summary>
        public object value;


        /// <summary>
        /// 基础组件中的控件名字【从配置中取得】
        /// </summary>
        public string controlName;

        /// <summary>
        /// 数值的运算符号
        /// </summary>
        public OperationSymbols operation;

        /// <summary>
        /// 控件的前面提示信息
        /// </summary>
        public string controlLabelName;
    }
}
