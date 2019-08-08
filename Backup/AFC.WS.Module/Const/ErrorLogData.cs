using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace AFC.WS.Model.Const
{
    /// <summary>
    /// 错误代码日志，addedby wangdx date:20110322
    /// </summary>
    public class ErrorLogData
    {
        /// <summary>
        /// 操作员非法
        /// </summary>
       [Description("操作员非法")]
        public const uint Priv_Operator_Not_Valid = 0x00010001;

        /// <summary>
       /// 登录操作非法
        /// </summary>
       [Description("登录操作非法")]
        public const uint Priv_logIn_Not_Valid = 0x00010002;

        /// <summary>
        /// 登出操作非法
        /// </summary>
       [Description("登出操作非法")]
        public const uint Priv_Logout_Not_Valid = 0x00010003;

        /// <summary>
        /// 密码为空
        /// </summary>
       [Description("密码为空")]
        public const uint Priv_Pwd_Is_Empty=0x00010004;

        /// <summary>
        /// 发送登录请求失败
        /// </summary>
       [Description("发送登录请求失败")]
        public const uint Priv_Send_LogIn_Request_Failed = 0x00010005;

        /// <summary>
        /// 操作员格式非法
        /// </summary>
       [Description("操作员格式非法")]
        public const uint Priv_Operator_ID_Format_Error = 0x00010006;

        /// <summary>
        /// 密码为空
        /// </summary>
       [Description("密码为空")]
        public const uint Priv_PassWord_Is_Empty = 0x00010007;

        /// <summary>
        /// 该用户不存在
        /// </summary>
       [Description("该用户不存在")]
        public const uint Priv_Operator_Not_Exist = 0x00010008;

        /// <summary>
        /// 操作员在该车站无权限
        /// </summary>
       [Description("操作员在该车站无权限")]
        public const uint Priv_Station_Not_Primission = 0x00010009;

        /// <summary>
        /// 操作员设备类型无权限
        /// </summary>
       [Description("操作员设备类型无权限")]
        public const uint Priv_DeviceType_Not_Primission = 0x00010010;

        /// <summary>
        /// 数据库连接失败
        /// </summary>
       [Description("数据库连接失败")]
        public const uint DB_Connect_Failed = 0x000100011;

        /// <summary>
        /// 操作员密码错误
        /// </summary>
       [Description("操作员密码错误")]
        public const uint Priv_OperatorID_Pwd_Error =0x000100012;

        /// <summary>
        /// 操作员已锁定
        /// </summary>
       [Description("操作员已锁定")]
        public const uint Priv_Operator_Locked = 0x000100013;

        /// <summary>
        /// 操作员不允许重复登录
        /// </summary>
       [Description("操作员不允许重复登录")]
        public const uint Priv_Operator_Not_ReLogin = 0x000100014;

        /// <summary>
        /// 操作员已经密码终止
        /// </summary>
       [Description("操作员已经密码终止")]
        public const uint Priv_Operator_Has_Pwd_End = 0x000100015;

        /// <summary>
        /// 旧密码为空
        /// </summary>
       [Description("旧密码为空")]
        public const uint Priv_Operator_Old_Pwd_Is_Empty = 0x000100016;
        
        /// <summary>
        /// 旧密码错误
        /// </summary>
        [Description("旧密码错误")]
        public const uint Priv_Old_Pwd_Error = 0x000100017;

        /// <summary>
        /// 新密码为空
        /// </summary>
        [Description("新密码为空")]
        public const uint Priv_New_Pwd_Is_Empty = 0x00010018;

        /// <summary>
        /// 密码输入的两次不同
        /// </summary>
        [Description("密码输入的两次不同")]
        public const uint Priv_PWD_NOT_SAME = 0x00010019;

     
        /// <summary>
        /// 发送修改密码请求失败
        /// </summary>
        [Description("发送修改密码请求失败")]
        public const uint Priv_Send_Pwd_Request_Failed= 0x00010020;


        /// <summary>
        /// 发送登出报文失败
        /// </summary>
        [Description("发送登出报文失败")]
        public const uint Priv_Send_logOut_Msg_Error=0x00010022;

        /// <summary>
        /// 发送锁定报文失败
        /// </summary>
        [Description("发送锁定报文失败")]
        public const uint Priv_Send_Lock_Msg_Error = 0x00010024;

        /// <summary>
        /// 发送操作员解锁失败
        /// </summary>
        [Description("发送操作员解锁失败")]
        public const uint Priv_Send_Unlock_Msg_Error = 0x00010026;

        /// <summary>
        /// 当前操作员密码错误
        /// </summary>
        [Description("当前操作员密码错误")]
        public const uint Priv_PWD_Error = 0x00010028;

        /// <summary>
        /// 第二个操作员不存在
        /// </summary>
        [Description("第二个操作员不存在")]
        public const uint Priv_Double_Primission_Sec_Operator_Not_Exist = 0x00010029;

        /// <summary>
        /// 第二个操作员已锁定
        /// </summary>
        [Description("第二个操作员已锁定")]
        public const uint Priv_Double_Primission_Sec_Operator_Locked= 0x00010030;

        /// <summary>
        /// 第二个操作员本站无权限
        /// </summary>
        [Description("第二个操作员本站无权限")]
        public const uint Priv_Double_Primission_Sec_Operator_No_Primission_Station = 0x00010031;

        /// <summary>
        /// 第二个操作员本站设备无权限
        /// </summary>
        [Description("第二个操作员本站设备无权限")]
        public const uint Priv_Double_Primission_Sec_Operator_No_Primission_DeviceType = 0x00010032;

        /// <summary>
        /// 第二个操作员没有操作该功能的权限
        /// </summary>
        [Description("第二个操作员没有操作该功能的权限")]
        public const uint Priv_Double_Primission_Sec_Operator_No_Primission_Function = 0x00010033;

        /// <summary>
        /// 第二个操作员密码错误
        /// </summary>
        [Description("第二个操作员密码错误")]
        public const uint Priv_Double_Primission_Sec_Operator_Pwd_Error = 0x00010034;

        /// <summary>
        /// 发送参数下载通知请求失败
        /// </summary>
        [Description("发送参数下载通知请求失败")]
        public const uint Para_Send_Download_Msg_Failed = 0x00010037;

        /// <summary>
        /// 角色ID非法
        /// </summary>
        [Description("角色ID非法")]
        public const uint Priv_Role_ID_Not_Valid = 0x00010038;

        /// <summary>
        /// 存在该角色ID
        /// </summary>
        [Description("存在该角色ID")]
        public const uint Priv_Role_ID_Exist = 0x00010039;

        /// <summary>
        /// 角色名称为空
        /// </summary>
        [Description("角色名称为空")]
        public const uint Priv_Role_Name_Is_Empty = 0x00010040;


        /// <summary>
        /// 功能ID非法
        /// </summary>
        [Description("功能ID非法")]
        public const uint Priv_Function_ID_Not_Valid = 0x00010042;

        /// <summary>
        /// 存在该功能ID
        /// </summary>
        [Description("存在该功能ID")]
        public const uint Priv_Function_ID_Exist = 0x00010043;

        /// <summary>
        /// 功能名称为空
        /// </summary>
        [Description("功能名称为空")]
        public const uint Priv_Function_Name_Is_Empty = 0x00010044;



    }
}
