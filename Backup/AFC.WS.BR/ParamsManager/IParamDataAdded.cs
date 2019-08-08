using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.ParamsManager
{
    /// <summary>
    /// 参数拷贝实现该接口
    /// added by wangdx
    /// date:20111229
    /// </summary>
    public interface IParamDataAdded
    {
        /// <summary>
        ///拷贝参数
        /// </summary>
        /// <param name="paraVersion">参数版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        int AddParamsData(string paraVersion);
    }
}
