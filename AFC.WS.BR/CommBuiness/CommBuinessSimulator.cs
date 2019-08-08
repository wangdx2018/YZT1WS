using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.CommBuiness
{
    using AFC.WS.UI.Common;
    using TJComm;
    using AFC.WS.Model.Comm;
    using AFC.WS.Model.DB;

    public class CommBuinessSimulator:ICommProcess
    {
        #region ICommProcess 成员

        public bool Init()
        {
            return true;
        }


        public int CheckIn()
        {
            return 0;
        }

        public int LogIn(uint operatorId, string pwd)
        {
            return 0;
        }

        public int LogOut(uint operatorId)
        {
            return 0;
        }

        public int ChangePwd(uint opeatorId, string oldPwd, string newPwd)
        {
            return 0;
        }

        public int UnlockOperator(uint unLockOperatorId)
        {
            return 0;
        }

        /// <summary>
        /// 发布草稿版
        /// </summary>
        /// <param name="activeDate">生效日期</param>
        /// <param name="publishDate">发布日期</param>
        /// <param name="listPara">参数类型</param>
        /// <returns></returns>
        public int PublishParaDraft(uint activeDate, uint publishDate, List<ushort> listPara)
        {
            return 0;
        }

        /// <summary>
        /// 操作员强制登出
        /// </summary>
        /// <param name="operatorId">操作员</param>
        /// <returns></returns>
        public int OperatorForceLogOut(uint operatorId,uint deviceId,ushort stationId)
        {
            return 0;
        }

        public uint currentLogInOperatorId
        {
            set;
            get;
        }

       /// <summary>
       /// 操作员锁定
       /// </summary>
       /// <param name="lockOperator">锁定的操作员</param>
       /// <returns>锁定成功返回0，否则返回错误代码</returns>
        public int LockOperator(uint lockOperator)
        {
           
            return 0;
        }

        #endregion

        #region ICommProcess 成员

        public bool OnLineStatus
        {
            get;
            set;

        }

        #endregion

        public int ParamsDownloadNotify(ushort paramType, List<DeviceRange> list)
        {
            return 0;
        }

        public int SpecialParamsDownLoadNotify(List<ParamsData> paraList, List<DeviceRange> list)
        {
            return 0;
        }

        #region ICommProcess 成员


        public int ForceTimeSyn(uint currentTime)
        {
            throw new NotImplementedException();
        }

        public int TimeSyn(uint currentTime)
        {
            return 0;
            //throw new NotImplementedException();
        }

        public int BuildParamFile()
        {
            return 0;
        }

        public int ModeChange(uint stationId, uint modeCode)
        {
            return 0;
        }

        public int ModeChangeNotify(ushort stationId, uint modeCode)
        {
            return 0;
        }

        #endregion

        #region ICommProcess 成员


        public int ControlCmd(byte controlType, ushort controlCode, List<DeviceRange> list)
        {
           
            return 0;
        }

        public int RunStart()
        {
            return 0;
        }

        public int RunEnd()
        {
            return 0;
        }

        public int DateSettlement(CashDateSettlementInfo ds,DateTime dt)
        {
            return 0;
        }
        public int TickUpdateNotify(TickValuedProductInfo tickInfo)
        {
            return 0;
        }

        #endregion

        #region ICommProcess 成员


        public int DataImportNotify(byte type, string strImportPath)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICommProcess 成员


        public int ReUploadRecords(uint deviceId, uint dataType, uint tranDateBegin, uint tranDateEnd)
        {
          //  throw new NotImplementedException();
            return 0;
        }

        #endregion
    }
}
