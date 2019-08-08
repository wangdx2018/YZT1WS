using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
     /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110221
    /// 代码功能：定义了TJWS系统中的同步消息类型。
    /// 修订记录：20110510 增加了车站选择的同步消息的处理
    /// </summary>
   public class SynMessageType
    {
       /// <summary>
       /// UI切换
       /// </summary>
       public const string SwichPage = "SwitchPage";

       /// <summary>
       /// 创建导航列表
       /// </summary>
       public const string CreateNavigatorList = "CreateNavigatorList";


       /// <summary>
       /// 登出
       /// </summary>
       public const string LogOut = "LogOut";

       /// <summary>
       /// 登录
       /// </summary>
       public const string LogIn = "LogIn";


       /// <summary>
       /// 车站设备选择
       /// </summary>
       public const string Device_Station_Selected = "Device_Station_Selected";

       /// <summary>
       /// 操作员现金归还时点击增加预付值卡
       /// </summary>
       public const string Add_Tick_Return = "Add_Tick_Return";

       /// <summary>
       /// 操作员现金归还时增加预付值卡结束
       /// </summary>
       public const string Add_Tick_Finish = "Add_Tick_Finish";

       /// <summary>
       /// 运营开始
       /// </summary>
       public const string Run_Start = "Run_Start";

       /// <summary>
       /// 运营结束
       /// </summary>
       public const string Run_End = "Run_End";


       /// <summary>
       /// 模式变化
       /// </summary>
       public const string Mode_Change = "Mode_Change";

       /// <summary>
       /// 监视设备图被选中
       /// </summary>
       public const string Dev_Image_Selected = "Dev_Image_Selected";


       /// <summary>
       ///导航选择 
       /// </summary>
       public const string NavigationSelection = "NavigationSelection";
      
    }
}
