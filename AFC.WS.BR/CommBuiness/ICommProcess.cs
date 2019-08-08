using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJComm;
using AFC.WS.Model.Comm;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.CommBuiness
{
    public interface ICommProcess
    {

        bool Init();

        int CheckIn();

        uint currentLogInOperatorId
        {
            set;
            get;
        }
         

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="pwd">操作员密码</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int LogIn(uint operatorId, string pwd);
     

        /// <summary>
        /// 登录，登出
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        int LogOut(uint operatorId);
      

        /// <summary>
        /// 操作员修改密码
        /// </summary>
        /// <param name="opeatorId">操作员ID</param>
        /// <param name="oldPwd">操作员旧密码</param>
        /// <param name="newPwd">操作员新密码</param>
        /// <returns>0：成功，其他错误</returns>
        int ChangePwd(uint opeatorId, string oldPwd, string newPwd);
       

        /// <summary>
        /// 操作员解锁
        /// </summary>
        /// <param name="unLockOperatorId">解锁的操作员ID</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int UnlockOperator(uint unLockOperatorId);

        /// <summary>
        /// 发布草稿版
        /// </summary>
        /// <param name="activeDate">生效日期</param>
        /// <param name="publishDate">发布日期</param>
        /// <param name="listPara">参数类型</param>
        /// <returns></returns>
        int PublishParaDraft(uint activeDate, uint publishDate, List<ushort> listPara);

         /// <summary>
        /// 运营日结算
        /// </summary>
        /// <param name="CashDateSettlementInfo">运营日结算数据</param>
        /// <returns></returns>
        int DateSettlement(CashDateSettlementInfo ds,DateTime dt);

        /// <summary>
        /// 数据补传
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int ReUploadRecords(uint deviceId, uint dataType, uint tranDateBegin, uint tranDateEnd);

        /// <summary>
        /// 操作员锁定
        /// </summary>
        /// <param name="lockOperator">锁定的操作员</param>
        /// 
        /// <returns>锁定成功返回0，否则返回错误代码</returns>
        int LockOperator(uint lockOperator);

        /// <summary>
        /// 操作员强制登出
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int OperatorForceLogOut(uint operatorId,uint deviceId,ushort stationId);

        /// <summary>
        /// 联机状态，当联机成功后可以设置
        /// </summary>
        bool OnLineStatus
        {
            set;
            get;
        }

        int ParamsDownloadNotify(ushort paramType, List<DeviceRange> list);


        int SpecialParamsDownLoadNotify(List<ParamsData> paraList, List<DeviceRange> list);

        /// <summary>
        /// 强制时钟同步
        /// </summary>
        /// <param name="currentTime">afctimeT时间</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int ForceTimeSyn(uint currentTime);

        /// <summary>
        /// 手工设置服务器时间
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int TimeSyn(uint currentTime);

        /// <summary>
        ///创建生成参数文件
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int BuildParamFile();

        /// <summary>
        /// 模式变化
        /// </summary>
        /// <param name="stationId">模式车站ID</param>
        /// <param name="modeCode">模式代码</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int ModeChange(uint stationId, uint modeCode);

        /// <summary>
        /// 控制命令
        /// </summary>
        /// <param name="controlType">控制类型</param>
        /// <param name="controlCode">控制代码</param>
        /// <param name="list">设备范围列表</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int ControlCmd(byte controlType, ushort controlCode, List<DeviceRange> list);

        /// <summary>
        /// 运营开始
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int RunStart();

        /// <summary>
        /// 运营结束
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int RunEnd();


        /// <summary>
        /// 数据导入发送1009报文通知
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="strImportPath">导入目录</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int DataImportNotify(byte type, string strImportPath);

        /// <summary>
        /// 库存类型更新通知
        /// </summary>
        /// <param name="tickInfo">库存类型</param>
        /// <returns></returns>
        int TickUpdateNotify(TickValuedProductInfo tickInfo);
       

       
    }
}
