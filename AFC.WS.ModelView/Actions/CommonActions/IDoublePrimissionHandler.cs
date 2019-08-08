using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.CommonActions
{
    /// <summary>
    /// 双权限的认证Action。接口是为了能够记录第二个
    /// 操作员的操作内容。
    /// </summary>
    public interface IDoublePrimissionHandler
    {
        bool HandleDoublePrimission(string operatorId);
    }
}
