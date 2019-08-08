using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    public class StationRunStatus
    {

        /// <summary>
        /// 运营开始成功
        /// </summary>
        public const int RUN_START_SUC = 0x00;

        /// <summary>
        /// 运营开始中
        /// </summary>
        public const int RUN_STARTING = 0x01;

        /// <summary>
        /// 运营开始失败
        /// </summary>
        public const  int RUN_START_FAILED = 0x02;

        /// <summary>
        /// 运营结束成功
        /// </summary>
        public const int RUN_END_SUC = 0x03;

        /// <summary>
        /// 运营结束中
        /// </summary>
        public const  int RUN_ENDING = 0x04;

        /// <summary>
        /// 运营结束失败
        /// </summary>
        public  const int RUN_END_FAILED = 0x05;
    }
}
