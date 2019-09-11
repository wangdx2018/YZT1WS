using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
   /// <summary>
   /// 消息MessageType的类型
   /// </summary>
   public class CommMsgType
    {
       /// <summary>
       /// 操作员登录登出
       /// </summary>
       public const ushort Log_In_Out = 0x1301;

       /// <summary>
       /// 操作员修改密码
       /// </summary>
       public const ushort Change_Pwd = 0x1302;

       /// <summary>
       /// 操作员锁定
       /// </summary>
       public const ushort Operator_Locked = 0x1304;

       /// <summary>
       /// 操作员解锁
       /// </summary>
       public const ushort Operator_Unlocked = 0x1305;

       /// <summary>
       /// 参数下载通知
       /// </summary>
       public const ushort Param_Download_Notify = 0x1309;

       /// <summary>
       /// 特定参数范围下载
       /// </summary>
       public const ushort Specail_Params_Download_Notify = 0x1313;

       /// <summary>
       /// 操作员强制登出
       /// </summary>
       public const ushort Operator_Force_LogOut = 0x1306;


       /// <summary>
       /// 参数发布
       /// </summary>
       public const ushort Params_Publish = 0x1315;


       public const ushort Unspecified = 0xffff;

       /// <summary>
       /// 签到
       /// </summary>
       public const ushort Check_In = 0x1390;

       /// <summary>
       /// 控制命令
       /// </summary>
       public const ushort Control_CMD = 0x3000;

       /// <summary>
       /// 模式变化指令
       /// </summary>
       public const ushort Mode_Change_CMD = 0x1341;

       /// <summary>
       /// 模式指令变化通知
       /// </summary>
       public const ushort Mode_Change_Notify = 0x1342;

       /// <summary>
       /// 权限参数生成
       /// </summary>
       public const ushort Primission_Param_Building = 0x1317;

       /// <summary>
       /// 强制时钟同步
       /// </summary>
       public const ushort Force_Time_Syn = 0x1333;

       /// <summary>
       /// 时钟同步
       /// </summary>
       public const ushort Time_Syn = 0x1334;

       /// <summary>
       /// 运营开始
       /// </summary>
       public const ushort Run_Start = 0x1351;

       /// <summary>
       /// 运营结束
       /// </summary>
       public const ushort Run_End = 0x1352;

       /// <summary>
       /// 设备状态上报
       /// </summary>
       public const ushort Dev_Status_Report = 0x1325;

       /// <summary>
       /// 数据导入通知
       /// </summary>
       public const ushort Data_Import_Notify = 0x1009;


       /// <summary>
       /// 运营日结算
       /// </summary>
       public const ushort Date_Settlement = 0x1353;

       /// <summary>
       /// 自定义库存类型通知
       /// </summary>
       public const ushort Tick_UpdateNotify = 0x1361;

       /// <summary>
       /// 数据补传命令
       /// </summary>
       public const ushort Data_ReUploadRecords = 0x1363;
    }
}
