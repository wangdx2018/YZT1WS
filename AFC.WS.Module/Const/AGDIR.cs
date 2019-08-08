using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    /// <summary>
    /// AG方向常量。
    /// 横向摆放 左，右，双向
    ///纵向摆放 上，下，双向 
    /// </summary>
    public class AGDIR
    {
        /// <summary>
        /// 横向 箭头朝左边
        /// </summary>
        public const string H_LEFT = "01";

        /// <summary>
        /// 横向 箭头朝右边
        /// </summary>
        public const string H_RIGHT = "02";

        /// <summary>
        /// 横向，双向
        /// </summary>
        public const string H_DOUBLE_WAY= "03";

        /// <summary>
        /// 纵向，箭头朝上
        /// </summary>
        public const string V_TOP = "04";

        /// <summary>
        /// 纵向，箭头朝下
        /// </summary>
        public const string V_DOWN = "05";

        /// <summary>
        /// 纵向，双向
        /// </summary>
        public const string V_DOUBLE_WAY = "06";
    }
}
