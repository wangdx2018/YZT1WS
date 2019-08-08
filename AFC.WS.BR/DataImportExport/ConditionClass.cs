using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.DataImportExport
{
    public class ConditionClass
    {
        ///// <summary>
        ///// ACC参数
        ///// </summary>
        //public bool paraACC;

        ///// <summary>
        ///// LCC参数
        ///// </summary>
        //public bool paraLCC;

        ///// <summary>
        ///// 当前版
        ///// </summary>
        //public bool curVer;

        ///// <summary>
        ///// 将来版
        ///// </summary>
        //public bool futVer;

        ///// <summary>
        ///// 上传成功
        ///// </summary>
        //public bool upSucc;

        ///// <summary>
        ///// 上传失败
        ///// </summary>
        //public bool upFail;

        /// <summary>
        /// 01:LCC参数，02:ACC参数，03:LCC和ACC，00：都没选
        /// </summary>
        public string paraType;

        /// <summary>
        /// 01:当前版，02:将来版，03:当前版和将来版，00：都没选
        /// </summary>
        public string verType;

        /// <summary>
        /// 01:上传成功，02:上传失败，03:成功和失败，00：都没选
        /// </summary>
        public string upResult;

        /// <summary>
        /// 开始日期
        /// </summary>
        public string startDate;

        /// <summary>
        /// 结束日期
        /// </summary>
        public string endDate;
    }
}
