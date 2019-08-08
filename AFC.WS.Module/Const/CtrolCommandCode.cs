using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
   public  class CtrolCommandCode
    {

       public const uint Power_off = 0x0101;
       public const string Power_off_en = "电源关闭";

       public const uint Restart = 0x0102;
       public const string Restart_en = "重新启动";


       public const uint Run_begin = 0x0103;
       public const string Run_begin_en = "运营开始";



       public const uint Run_end = 0x0104;
       public const string Run_end_en = "运营结束";



       public const uint Sleep_mode = 0x0105;
       public const string Sleep_mode_en = "睡眠模式";


       public const uint Wake_up = 0x0106;
       public const string Wake_up_en = "远程唤醒";


        public const uint Start_service = 0x0107;
       public const string Start_service_en = "开始服务";

       public const uint Suspended = 0x0108;
       public const string Suspended_en = "暂停服务";


       public const uint Stop = 0x0201;
       public const string Stop_en = "进站";


       public const uint Outbound = 0x0202;
       public const string Outbound_en = "出站";


       public const uint Two_way = 0x0203;
       public const string Two_way_en = "双向";


       public const uint Open_gate = 0x0204;
       public const string Open_gate_en = "闸门常开";


       public const uint  Closed_gate = 0x0205;
       public const string Closed_gate_en = "闸门常闭";


       public const uint Open_degraded  = 0x0301;
       public const string Open_degraded_en = "允许进入降级模式";



       public const uint Close_degraded = 0x0302;
       public const string Close_degraded_en = "禁止进入降级模式";


       public const uint No_change_mode = 0x0303;
       public const string No_change_mode_en = "无找零模式";



       public const uint No_accept_notes = 0x0304;
       public const string No_accept_notes_en = "不收纸币模式";


       public const uint No_ticket_mode = 0x0305;
       public const string No_ticket_mode_en = "无售票模式";


       public const uint No_print_mode = 0x0306;
       public const string No_print_mode_en = "无打印模式";


       public const uint No_notes_change = 0x0307;
       public const string No_notes_change_en = "无纸币找零";


       public const uint No_coins_change = 0x0308;
       public const string No_coins_change_en = "无硬币找零";


       public const uint Return_normal_mode = 0x0309;
       public const string Return_normal_mode_en = "退出降级运行模式";


       public const uint Empty_coin = 0x030A;
       public const string Empty_coin_en = "清空钱币";



       public const uint No_accept_coins = 0x030B;
       public const string No_accept_coins_en = "不收硬币模式";


       public const uint Password_authentication  = 0x030C;
       public const string Password_authentication_en = "取钱箱随机密码验证";

       public const uint Ticket_mode  = 0x0401;
       public const string Ticket_mode_en = "售票模式";


       public const uint Fare_adjustment_mode  = 0x0402;
       public const string Fare_adjustment_mode_en = "补票模式";

         public const uint Replacement_ticket_mode  = 0x0403;
       public const string Replacement_ticket_mode_en = "售补票模式";

    }
}
