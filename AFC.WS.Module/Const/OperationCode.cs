using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    /// <summary>
    /// added by wangdx 20100111
    /// 该类定义了操作员权限操作的的操作代码
    /// </summary>
    public class OperationCode
    {
        /// <summary>
        /// 登录
        /// </summary>
        public const string Operator_LogIn = "1001";

        /// <summary>
        /// 登出
        /// </summary>
        public const string Operator_LogOut = "1002";

        /// 操作员锁定
        /// </summary>
        public const string Operator_Locked = "1201";

        /// <summary>
        /// 操作员解锁
        /// </summary>
        public const string Operator_UnLocked = "1202";


        /// <summary>
        /// 操作员修改密码
        /// </summary>
        public const string Operator_Pwd_Change = "1203";



        /// <summary>
        /// 新建操作员
        /// </summary>
        public const string Create_New_Operator = "1003";

        /// <summary>
        /// 更新操作员
        /// </summary>
        public const string Update_Operator = "1004";

        /// <summary>
        /// 删除操作员
        /// </summary>
        public const string Delete_Operator="1005";


        /// <summary>
        /// 启用操作员
        /// </summary>
        public const string Start_Using_Operator = "1006";


        /// <summary>
        /// 操作员密码终止
        /// </summary>
        public const string End_Operator_Pwd = "1007";


        /// <summary>
        /// 操作员禁用
        /// </summary>
        public const string Operator_Disable = "1008";

        /// <summary>
        /// 操作员密码重置
        /// </summary>
        public const string Operator_Pwd_Reset = "1009";


        /// <summary>
        /// 为操作员添加角色
        /// </summary>
        public const string Add_Role_To_Operator = "100A";

        /// <summary>
        /// 为操作员删除角色
        /// </summary>
        public const string Delete_Role_To_Operator = "100B";


        /// <summary>
        /// 为操作员分配工作地点
        /// </summary>
        public const string Add_WorkLocation_To_Operator = "100C";

        /// <summary>
        /// 操作员删除工作地点
        /// </summary>
        public const string Delete_WorkLocation_To_Operator = "100D";

        /// <summary>
        /// 增加角色
        /// </summary>
        public const string Add_Role = "100E";

        /// <summary>
        /// 修改角色
        /// </summary>
        public const string Update_Role = "100F";

        /// <summary>
        /// 删除角色
        /// </summary>
        public const string Delete_Role = "1010";

        /// <summary>
        /// 启用角色
        /// </summary>
        public const string Start_Using_Role = "1011";

        /// <summary>
        /// 禁用角色
        /// </summary>
        public const string Disable_Role = "1012";

        /// <summary>
        /// 增加功能到角色
        /// </summary>
        public const string Add_Function_To_Role = "1013";

        /// <summary>
        /// 删除功能角色
        /// </summary>
        public const string Delete_Role_To_Function = "1014";

        /// <summary>
        /// 新增功能
        /// </summary>
        public const string Add_New_Function = "1015";

        /// <summary>
        /// 更新功能
        /// </summary>
        public const string Update_Function = "1016";

        /// <summary>
        /// 删除功能
        /// </summary>
        public const string Delete_Function = "1017";

        /// <summary>
        /// 启用功能
        /// </summary>
        public const string Start_Using_Function = "1018";

        /// <summary>
        /// 禁用功能
        /// </summary>
        public const string Disable_Function = "1019";


        public static string passwordSetMode = string.Empty;


        public const string Control_command = "170B";


        /// <summary>
        /// LC、SC运营开始
        /// </summary>
        public const string Run_Start = "2101";

        /// <summary>
        /// LC、SC运营结束
        /// </summary>
        public const string Run_End = "2102";

        /// <summary>
        /// 设备运营开始
        /// </summary>
        public const string DEV_RUN_START = "2103";

        /// <summary>
        /// 设备运营结束
        /// </summary>
        public const string DEV_RUN_END = "2104";

        /// <summary>
        /// 现金调入
        /// </summary>
        public const string Cash_Call_In = "160D";

        /// <summary>
        /// 现金调出
        /// </summary>
        public const string Cash_Call_Out = "160E";

        /// <summary>
        /// 现金归还
        /// </summary>
        public const string Cash_Check_In = "160C";

        /// <summary>
        /// 现金领用
        /// </summary>
        public const string Cash_Check_Out = "1608";

        /// <summary>
        /// 现金库存调整
        /// </summary>
        public const string Cash_Store_Adjust_Action = "1610";

        /// <summary>
        /// 现金解行
        /// </summary>
        public const string Cash_Solution = "1615";

        /// <summary>
        /// 现金待解行
        /// </summary>
        public const string Cash_Wait_Solution = "1616";

        /// <summary>
        /// 群组控制命令下发
        /// </summary>
        public const string SLE_Group_Control_Action = "170C";

        /// <summary>
        /// 模式设置命令下发
        /// </summary>
        public const string Set_Mode_Action = "1901";

        /// <summary>
        /// 货币种类添加
        /// </summary>
        public const string Cash_Store_Add = "1611";

        /// <summary>
        /// 货币名称修改
        /// </summary>
        public const string Cash_Store_Update = "1612";

        /// <summary>
        /// 增加草稿版参数
        /// </summary>
        public const string Add_Handle_Version = "1A04";

        /// <summary>
        /// 电源关闭
        /// </summary>
        public const string Power_off_en = "1709";

        /// <summary>
        /// 电源开启
        /// </summary>
        public const string Restart_en = "170A";

        /// <summary>
        /// 运营开始
        /// </summary>
        public const string Run_begin_en = "170B";

        /// <summary>
        /// 运营结束
        /// </summary>
        public const string Run_end_en = "170C";

        /// <summary>
        /// 睡眠模式
        /// </summary>
        public const string Sleep_mode_en = "170D";

        /// <summary>
        /// 远程唤醒
        /// </summary>
        public const string Wake_up_en = "170E";

        /// <summary>
        /// 正常服务
        /// </summary>
        public const string Start_service_en = "170F";

        /// <summary>
        /// 暂停服务
        /// </summary>
        public const string Suspended_en = "1710";

        /// <summary>
        /// AG进站模式
        /// </summary>
        public const string Stop_en = "1711";

        /// <summary>
        /// AG出站模式
        /// </summary>
        public const string Outbound_en = "1712";

        /// <summary>
        /// AG双向模式
        /// </summary>
        public const string Two_way_en = "1713";

        /// <summary>
        /// AG门常开模式
        /// </summary>
        public const string Open_gate_en = "1714";

        /// <summary>
        /// AG门常闭模式
        /// </summary>
        public const string Closed_gate_en = "1715";

        /// <summary>
        /// TVM降级模式开
        /// </summary>
        public const string Open_degraded_en = "1716";

        /// <summary>
        /// TVM降级模式关
        /// </summary>
        public const string Close_degraded_en = "1717";

        /// <summary>
        /// TVM无找零模式
        /// </summary>
        public const string No_change_mode_en = "1718";

        /// <summary>
        /// TVM不收纸币模式
        /// </summary>
        public const string No_accept_notes_en = "1719";

        /// <summary>
        /// TVM无售票模式
        /// </summary>
        public const string No_ticket_mode_en = "171a";

        /// <summary>
        /// TVM无打印模式
        /// </summary>
        public const string No_print_mode_en = "171b";

        /// <summary>
        /// TVM无纸币找零模式
        /// </summary>
        public const string No_notes_change_en = "171c";

        /// <summary>
        /// TVM无硬币找零模式
        /// </summary>
        public const string No_coins_change_en = "171d";

        /// <summary>
        /// TVM正常模式
        /// </summary>
        public const string Return_normal_mode_en = "171e";

        /// <summary>
        /// TVM清空钱币
        /// </summary>
        public const string Empty_coin_en = "171f";

        /// <summary>
        /// TVM不收硬币模式
        /// </summary>
        public const string No_accept_coins_en = "1720";

        /// <summary>
        /// TVM取钱箱随机密码验证
        /// </summary>
        public const string Password_authentication_en = "1721";

        /// <summary>
        /// BOM只售票模式
        /// </summary>
        public const string Ticket_mode_en = "1722";

        /// <summary>
        /// BOM只补票
        /// </summary>
        public const string Fare_adjustment_mode_en = "1723";

        /// <summary>
        /// BOM售补票模式
        /// </summary>
        public const string Replacement_ticket_mode_en = "1724";

        /// <summary>
        /// 生成权限参数
        /// </summary>
        public const string Builder_Primission_Param_Action = "1A06";

        /// <summary>
        /// 删除参数草稿版
        /// </summary>
        public const string Del_Para_Draft_Version = "1A07";

        /// <summary>
        /// 发布参数草稿版
        /// </summary>
        public const string Publish_Para_Draft_Version = "1A08";

        /// <summary>
        /// 软件上传
        /// </summary>
        public const string SoftWare_UpLoad_Action = "1B02";

        /// <summary>
        /// 更新草稿版参数
        /// </summary>
        public const string Update_Para_Draft_Version = "1A09";

        /// <summary>
        /// 增加功能
        /// </summary>
        public const string Add_Function = "1205";

        /// <summary>
        /// 增加操作员
        /// </summary>
        public const string Add_Operator = "1206";

        /// <summary>
        /// 增加角色
        /// </summary>
        public const string Add_Role_Action = "1207";

        /// <summary>
        /// 操作员强制登出
        /// </summary>
        public const string Force_LogOut_Action = "120C";

        /// <summary>
        /// 功能管理
        /// </summary>
        public const string Function_Manager_Action = "1209";

        /// <summary>
        /// 操作员登录
        /// </summary>
        public const string Login_Action = "120A";

        /// <summary>
        /// 操作员解锁
        /// </summary>
        public const string Operator_Lock_And_UnLock_Action = "120D";

        /// <summary>
        /// 操作员状态管理
        /// </summary>
        public const string Operator_Status_Action = "120E";

        /// <summary>
        /// 角色管理
        /// </summary>
        public const string Role_Manager_Action = "120F";

        /// <summary>
        /// 更新功能
        /// </summary>
        public const string Update_Function_Action = "1210";

        /// <summary>
        /// 更新操作员
        /// </summary>
        public const string Update_Operator_Action = "1211";

        /// <summary>
        /// 更新操作员操作
        /// </summary>
        public const string Update_Operator_Operation_Action = "1212";
        
        /// <summary>
        /// 更新角色
        /// </summary>
        public const string Update_Role_Action = "1213";
        
        /// <summary>
        /// 自动时钟同步
        /// </summary>
        public const string Auto_Time_Syn_Action = "1411";
        
        /// <summary>
        /// 强制时钟同步
        /// </summary>
        public const string Force_Time_Syn_Action = "1412";

        /// <summary>
        /// 票箱RFID初始化
        /// </summary>
        public const string TickBox_RFID_Init_Action = "1501";

        /// <summary>
        /// 票箱登记
        /// </summary>
        public const string TickBox_Register_Action = "1502";

         /// <summary>
        /// 票箱压票
        /// </summary>
        public const string TickBox_PutIn_Action = "1503";
        
        /// <summary>
        /// 票箱领用
        /// </summary>
        public const string TickBox_CheckOut_Action = "1504";

        /// <summary>
        /// 票箱归还
        /// </summary>
        public const string TickBox_CheckIn_Action = "1505";

        /// <summary>
        /// 票箱清点
        /// </summary>
        public const string TickBox_Clear_Action = "1506";

        /// <summary>
        /// 票箱调出
        /// </summary>
        public const string Empty_TickBox_CallOut_Action = "1509";

        /// <summary>
        /// 钱箱RFID初始化
        /// </summary>
        public const string CashBox_RFID_Init_Action = "1614";

        /// <summary>
        /// 车票库存调整
        /// </summary>
        public const string TickStore_Adjust_Action = "1C06";

        /// <summary>
        /// 钱箱登记
        /// </summary>
        public const string MoneyBox_Register_Action = "1601";

        /// <summary>
        /// 钱箱压钱
        /// </summary>
        public const string MoneyBox_Put_Action = "1602";

        /// <summary>
        /// 钱箱领用
        /// </summary>
        public const string MoneyBox_CheckIn_Action = "1603";

        /// <summary>
        /// 钱箱归还
        /// </summary>
        public const string MoneyBox_CheckOut_Action = "1604";

        /// <summary>
        /// 钱箱清点
        /// </summary>
        public const string MoneyBox_Clear_Action = "1605";

        /// <summary>
        /// 空钱箱调出
        /// </summary>
        public const string Empty_MoneyBox_Out_Action = "1613";

        /// <summary>
        /// 票卡调入
        /// </summary>
        public const string Tick_CallIn_Action = "1C02";

        /// <summary>
        /// 票卡调出
        /// </summary>
        public const string Tick_CallOut_Action = "1C03";

        /// <summary>
        /// 车票领用
        /// </summary>
        public const string Tick_CheckIn_Action = "1C04";

        /// <summary>
        /// 车票归还
        /// </summary>
        public const string Tick_CheckOut_Action = "1C05";

        /// <summary>
        /// 票卡种类添加
        /// </summary>
        public const string Tick_Store_Add = "1C07";

        /// <summary>
        /// 库存管理类型修改
        /// </summary>
        public const string Tick_Store_Update = "1C08";

        /// <summary>
        /// 参数下载
        /// </summary>
        public const string Param_DownLoad_Notify = "1A02";


        /// <summary>
        /// 特定参数下载
        /// </summary>
        public const string Special_Param_DownLoad_Notify = "1A10";

        /// <summary>
        /// 增加新的设备部件类型
        /// </summary>
        public const string Dev_PartId_Add = "2201";

        /// <summary>
        /// 更新设备部件类型名称
        /// </summary>
        public const string Dev_PartId_Name_Update = "2202";

        /// <summary>
        /// 增加新供应商
        /// </summary>
        public const string Basi_Provider_Info_Add = "2203";

        /// <summary>
        /// 更新供应商
        /// </summary>
        public const string Basi_Provider_Info_Update = "2204";

        /// <summary>
        /// 增加新工区
        /// </summary>
        public const string Basi_MaintainArea_Info_Add = "2205";

        /// <summary>
        /// 更新工区
        /// </summary>
        public const string Basi_MaintainArea_Info_Update = "2206";

        /// <summary>
        /// 删除工区
        /// </summary>
        public const string Del_Basi_MaintainArea_Info = "2207";

        /// <summary>
        /// 增加新设备参数信息
        /// </summary>
        public const string Para_4042_Device_Info_Add = "2208";

        /// <summary>
        /// 更新设备参数信息
        /// </summary>
        public const string Para_4042_Device_Info_Update = "2209";

        /// <summary>
        /// 删除设备参数信息
        /// </summary>
        public const string Para_4042_Device_Info_Delete = "2210";

        /// <summary>
        /// 增加故障单信息
        /// </summary>
        public const string Maintain_Fault_Rpt_Status_Add = "2211";

        /// <summary>
        /// 更新故障单信息
        /// </summary>
        public const string Maintain_Fault_Rpt_Status_Update = "2212";

        /// <summary>
        /// 关闭故障单信息
        /// </summary>
        public const string Maintain_Fault_Rpt_Status_Delete = "2213";

        /// <summary>
        /// 增加新定时任务
        /// </summary>
        public const string Task_Execute_Schedule_Add = "2301";

        /// <summary>
        /// 更新定时任务
        /// </summary>
        public const string Task_Execute_Scheduleo_Update = "2302";

        /// <summary>
        /// 增加新站厅
        /// </summary>
        public const string basi_station_hall_id_info_Add = "2401";

        /// <summary>
        /// 更新站厅信息
        /// </summary>
        public const string basi_station_hall_id_info_Update = "2402";

        /// <summary>
        /// 删除站厅信息
        /// </summary>
        public const string basi_station_hall_id_info_Delete = "2403";

        /// 增加新站厅组别
        /// </summary>
        public const string basi_hall_group_id_info_Add = "2404";

        /// <summary>
        /// 更新站厅组别信息
        /// </summary>
        public const string basi_hall_group_id_info_Update = "2405";

        /// <summary>
        /// 删除站厅组别信息
        /// </summary>
        public const string basi_hall_group_id_info_Delete = "2406";

        /// <summary>
        /// 更新设备站厅组别序号
        /// </summary>
        public const string basi_dev_info_sn_Update = "2407";

        /// <summary>
        /// 参数数据
        /// </summary>
        public const string para_data_import = "2408";

        /// <summary>
        /// 软件数据
        /// </summary>
        public const string soft_data_import = "2049";

        /// <summary>
        /// 交易数据
        /// </summary>
        public const string trade_data_import = "2050";

        /// <summary>
        /// 业务数据
        /// </summary>
        public const string busi_data_import = "2051";

        /// <summary>
        /// 添加UPS
        /// </summary>
        public const string dev_ups_status_add = "2501";

        /// <summary>
        /// 删除UPS
        /// </summary>
        public const string dev_ups_status_delete = "2502";

        /// <summary>
        /// 修改UPS
        /// </summary>
        public const string dev_ups_status_update = "2503";

        /// <summary>
        /// 分配设备
        /// </summary>
        public const string dev_ups_status_allot = "2504";

        /// <summary>
        /// 修改设备状态是否记录
        /// </summary>
        public const string Update_Is_Log = "2601";

        /// <summary>
        /// 修改设备状态是否应用于BOM
        /// </summary>
        public const string Update_Is_BOM = "2602";

        /// <summary>
        /// 修改设备状态是否应用于AGM
        /// </summary>
        public const string Update_Is_AGM = "2604";

        /// <summary>
        /// 修改设备状态是否应用于TVM
        /// </summary>
        public const string Update_Is_TVM = "2603";

        /// <summary>
        /// 修改设备状态是否应用于EQM
        /// </summary>
        public const string Update_Is_EQM = "2605";

        /// <summary>
        /// 设置设备状态报警方式
        /// </summary>
        public const string Update_Alarm_Style = "2606";

        /// <summary>
        /// 设置设备状态故障级别
        /// </summary>
        public const string Update_Status_Level = "2607";

        /// <summary>
        /// 更新是否监控
        /// </summary>
        public const string Update_Is_Visible = "2608";

        /// <summary>
        /// 报警状态打开
        /// </summary>
        public const string Alarm_Status_Open = "2609";

        /// <summary>
        /// 报警状态关闭
        /// </summary>
        public const string Alarm_Status_CLose = "2610";

        /// <summary>
        /// 设备报警确认
        /// </summary>
        public const string Dev_Alarm_Confirm = "2611";



        
    }
}
