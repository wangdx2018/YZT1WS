using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
   /// <summary>
   /// create by wangdx 20110712
   /// 钱票箱的操作类型常量
   /// 操作类型01：安装;02：卸下; 03:清点;04:压钱 05：领用 06：归还 07：rfid初始化 08：票箱登记,09 票箱调出
   /// </summary>
   public class BoxOperationType
    {
       /// <summary>
       /// 安装
       /// </summary>
       public const string SET_UP = "01";

       /// <summary>
       /// 卸下
       /// </summary>
       public const string SET_DOWN = "02";

       /// <summary>
       /// 清点
       /// </summary>
       public const string CLEAR = "03";

       /// <summary>
       /// 压入
       /// </summary>
       public const string ADD = "04";

       /// <summary>
       /// 领用
       /// </summary>
       public const string Check_Out = "05";

       /// <summary>
       /// 归还
       /// </summary>
       public const string Check_In= "06";

       /// <summary>
       /// 标签初始化
       /// </summary>
       public const string RFID_INIT = "07";

       /// <summary>
       /// 登记
       /// </summary>
       public const string BOX_REG = "08";


       /// <summary>
       /// 空票箱调出
       /// </summary>
       public const string BOX_CALL_OUT = "09";
    }
}
