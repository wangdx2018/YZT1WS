using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    /// <summary>
    /// 通讯的 命令代码
    /// </summary>
    public class ControlCode
    {
        /// <summary>
        /// 电源关闭
        /// </summary>
        public const ushort POWER_OFF = 0x0101;

        /// <summary>
        /// 电源开启
        /// </summary>
        public const ushort POWER_ON = 0x0102;

        /// <summary>
        /// 运营开始
        /// </summary>
        public const ushort RUN_START = 0x0103;

        /// <summary>
        /// 运营结束
        /// </summary>
        public const ushort RUN_END = 0x0104;

        /// <summary>
        /// 睡眠模式
        /// </summary>
        public const ushort SLEEP_MODE = 0x0105;

        /// <summary>
        /// 远程唤醒
        /// </summary>
        public const ushort REMOTE_WEAK_UP = 0x0106;

        /// <summary>
        /// 正常服务
        /// </summary>
         public const ushort NORMAL_SERVICE=0x0107;

        /// <summary>
        /// 暂停服务
        /// </summary>
         public const ushort PAUSE_SERVICE=0x0108;

        /// <summary>
        /// 进站
        /// </summary>
         public const ushort AG_ENTER=0x0201;

        /// <summary>
        /// 出站
        /// </summary>
         public const ushort AG_EXIT = 0x0202;

        /// <summary>
        /// 双向
        /// </summary>
         public const ushort AG_DOUBLE_WAY = 0x0203;

        /// <summary>
        /// AG门常开模式
        /// </summary>
         public const ushort AG_GATE_STILL_OPEN = 0x0204;

        /// <summary>
        /// AG门常闭模式
        /// </summary>
         public const ushort AG_GATE_STILL_CLOSE = 0x0205;

        /// <summary>
        /// 降级模式开
        /// </summary>
         public const ushort REDUCE_RUN_OPEN = 0x0301;

        /// <summary>
        /// 降级模式关
        /// </summary>
         public const ushort REDUCE_RUN_OFF = 0x0302;


         /// <summary>
         /// 无找零模式
         /// </summary>
         public const ushort NO_CHARGE_MODE = 0x0303;

        /// <summary>
        /// 不收纸币模式
        /// </summary>
         public const ushort NO_RECEIVE_PAPER_MONEY=0x0304;
 

         /// <summary>
         /// 无售票模式
         /// </summary>
         public const ushort NO_SALE_TICK = 0x0305;


        /// <summary>
         /// 无打印模式
        /// </summary>
         public const ushort NO_PRINT = 0x0306;

        /// <summary>
        /// 无纸币找零模式
        /// </summary>
         public const ushort NO_BILL_CHARGE = 0x0307;

        /// <summary>
        /// 无硬币找零模式
        /// </summary>
         public const ushort NO_COIN_CHARGE = 0x0308;

         /// <summary>
         /// 正常模式
         /// </summary>
         public const ushort RECOVER_NORMAL_SERVICE=0x0309;

        /// <summary>
        /// 清空钱币
        /// </summary>
         public const ushort CLEAR_MONEY=0x030A;

        /// <summary>
        /// 无硬币找零模式
        /// </summary>
         public const ushort NO_RECEIVE_COIN_MODE = 0x030B;

        /// <summary>
         /// 取钱箱随机密码验证
        /// </summary>
         public const ushort GET_MONEY_BOX_RADOM_PWD_CHECK = 0x030C;

        /// <summary>
        /// 只售票模式
        /// </summary>
         public const ushort ONLY_SALE_TICKET = 0x0401;

        /// <summary>
        /// 只充值
        /// </summary>
         public const ushort ONLY_COMPENSATION = 0x0402;

        /// <summary>
        /// 售补票模式
        /// </summary>
         public const ushort SALE_COMPENSACTION_MODE = 0x0403;



    }
}
