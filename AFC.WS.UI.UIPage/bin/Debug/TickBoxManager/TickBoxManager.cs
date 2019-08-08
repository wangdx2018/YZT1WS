using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Config;

namespace AFC.WS.BR.TickBoxManager
{
    using AFC.WS.UI.RfidRW;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using System.Data;
using AFC.WorkStation.DB;
    
    /// <summary>
    /// added by wangdx date:20110517
    /// 该类封装了票箱的基本操作信息。
    /// edited by wangdx 20130122 修改了UpdateTickBoxStatusInfo updateDate为yyyyMMdd
    /// 
    /// edit by wangdx 20120807 票箱补充，操作员领用，操作员归还，票箱清点中增加了
    /// 库存变化流水记录。
    /// 
    /// edited by wangdx 20121009 记录票箱清点时候，记录的是本次卸下的设备的deviceId.
    /// 操作员票卡归还时增加了废票归还的处理。废票归还如果当前系统无废票记录时，自动插入
    /// 一条废票的库存管理类型的记录。
    /// </summary>
    public class TickBoxManager
    {
        /// <summary>
        /// 初始化时候创建票箱RFID的初始信息
        /// </summary>
        /// <returns>返回票箱对象</returns>
        public RfidTicketboxInfo CreateTicketBoxInfo(string tickBoxId)
        {
            RfidTicketboxInfo rtbi = new RfidTicketboxInfo();
            rtbi.cardIssueId = 1;
            rtbi.deviceId = "00000000";
            rtbi.ticketboxId = tickBoxId;
            rtbi.stationCode = "0000";
            rtbi.ticketboxLoactionStatus = 0x01;//In store
            rtbi.operatorTicketboxStatus = 0x03;// normal down 
            rtbi.cardIssueId = 0x01;// acc
            rtbi.cardPhysicalType = 0;//UL 
            rtbi.ticketProductType = 0;
            rtbi.prevAddValueCard = 0;// acc init card
            rtbi.extendProductId = 0;
            rtbi.ticketNumber = 0;
            rtbi.stationCode = "0000";
            rtbi.setupLoaction = 0xff;
            rtbi.blockOpeatorFlag = 0;
            rtbi.lastOpeatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            rtbi.operatorId ="000000";
            return rtbi;
        }

        /// <summary>
        /// RFID标签初始化时候创建钱箱对象
        /// </summary>
        /// <returns>返回钱箱对象</returns>
        public MoneyBoxRFID CreateMoneyBoxRFID(string moneyBoxId)
        {
            MoneyBoxRFID mbr = new MoneyBoxRFID();
            mbr.moneyBoxId = moneyBoxId;
            mbr.operatorId = "000000";
            mbr.deviceId = "00000000";
            mbr.moneyBoxLocationId = 0x01;// in store
            mbr.moneyBoxOperatorStatus = 0x03;// normal down 
            mbr.moneyCode = 0x00;
            mbr.moneyTotalNumber = 0;
            mbr.moneyTotalCount = 0;
            mbr.stationCode = "0000";
            mbr.setupLocation = 0xff;
            mbr.blockOperatorFlag = 0x00;
            mbr.lastOperatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            return mbr;
        }

        /// <summary>
        /// 判断数据库表中是否存在该票箱
        /// </summary>
        /// <param name="ticketBoxId">票箱ID</param>
        /// <returns>成功返回true，否则返回false</returns>
        public bool CheckTickBoxHasRegister(string ticketBoxId)
        {
            TickBoxRegistorInfo trInfo = DBCommon.Instance.GetModelValue<TickBoxRegistorInfo>(
                string.Format("select * from tick_box_registor_info t where t.ticket_box_id='{0}'", ticketBoxId.ToUpper()));
            if (trInfo != null&&!string.IsNullOrEmpty(trInfo.ticket_box_id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 票箱登记操作
        /// 1.记录票箱登记表
        /// 2.插入票箱状态表
        /// 3.插入票箱操作流水表
        /// </summary>
        /// <param name="ticketBoxId">票箱ID</param>
        /// <param name="rfidLabelId">票箱物理标签ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int TickBoxReg(string ticketBoxId,string rfidLabelId)
        {
            if (string.IsNullOrEmpty(ticketBoxId) || string.IsNullOrEmpty(rfidLabelId))
            {
                WriteLog.Log_Error("params error! ticketBoxId or rfidLabelId is null or empty");
                return -1;
            }
            int res = 0;
            Util.DataBase.BeginTransaction();
            res = InsertTickBoxReg(ticketBoxId, rfidLabelId);
            if (res != 0)
            {
                WriteLog.Log_Error("Insert tick_box_reg_info error");
                Util.DataBase.Rollback();
                return res;
            }
            res = InsertTickBoxStatus(ticketBoxId);
            if (res != 0)
            {
                WriteLog.Log_Error("Insert tick_box_status_info error");
                Util.DataBase.Rollback();
                return res;
            }
            res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(ticketBoxId, "08");
            if (res != 0)
            {
                WriteLog.Log_Error("Insert tick_box_operation_info error");
                Util.DataBase.Rollback();
                return res;
            }
            Util.DataBase.Commit();
            return 0;

        }

        /// <summary>
        /// 记录票箱登记表
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="rfidLabelId">票箱物理标签ID</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int InsertTickBoxReg(string tickBoxId, string rfidLabelId)
        {
            TickBoxRegistorInfo info = new TickBoxRegistorInfo();
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.ticket_box_id = tickBoxId;
            info.electronic_tag_id = rfidLabelId;
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            return DBCommon.Instance.InsertTable<TickBoxRegistorInfo>(info, "tick_box_registor_info") == 1 ? 0 : 1;
        }

        /// <summary>
        /// 记录票箱状态表
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="rifdLaelId">票箱物理标签ID</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int InsertTickBoxStatus(string tickBoxId)
        {
            TickBoxStatusInfo info = new TickBoxStatusInfo();
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.tick_mana_type = "00";
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.box_position = "01";
            info.tickets_num = 0;
            info.ticket_box_id = tickBoxId;
            return DBCommon.Instance.InsertTable<TickBoxStatusInfo>(info, "tick_box_status_info") == 1?0:1;
        }

        /// <summary>
        /// 更新票箱状态表
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="status">01在库；03 在设备</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int UpdateTickBoxStatusInfo(string tickBoxId, string status,int currentNum,string manType)
        {
            if (string.IsNullOrEmpty(tickBoxId) || string.IsNullOrEmpty(status))
            {
                WriteLog.Log_Error("param is not valid tickBoxId,status is null or empty");
                return -1;
            }
            TickBoxStatusInfo info = BuinessRule.GetInstace().GetTickBoxStausInfo(tickBoxId);
            info.box_position = status;
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.tickets_num = currentNum;
            info.tick_mana_type = manType;
            int res= DBCommon.Instance.UpdateTable<TickBoxStatusInfo>(info, "tick_box_status_info", new KeyValuePair<string, string>("ticket_box_id", tickBoxId));
            if(res==1)
            return 0;
            return res;
        }


        /// <summary>
        /// 票箱归还，领用
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="status">票箱状态</param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int UpdateTickBoxStatusInfo(string tickBoxId, string status)
        {
            if (string.IsNullOrEmpty(tickBoxId) || string.IsNullOrEmpty(status))
            {
                WriteLog.Log_Error("param is not valid tickBoxId,status is null or empty");
                return -1;
            }
            TickBoxStatusInfo info = BuinessRule.GetInstace().GetTickBoxStausInfo(tickBoxId);
            info.box_position = status;
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            int res = DBCommon.Instance.UpdateTable<TickBoxStatusInfo>(info, "tick_box_status_info", new KeyValuePair<string, string>("ticket_box_id", tickBoxId));
            if (res == 1)
                return 0;
            return res;
        }
        /// <summary>
        /// 票箱归还
        /// 1.更新票箱状态表
        /// 2.增加票箱流水表
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int TickBoxCheckIn(string tickBoxId)
        {
            Util.DataBase.BeginTransaction();
            int res = UpdateTickBoxStatusInfo(tickBoxId, "01");//在库
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error!");
                Util.DataBase.Rollback();
                return res;
            }
            res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(tickBoxId, "06");
            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_box_replace_info error!");
                Util.DataBase.Rollback();
                return res;
            }
            Util.DataBase.Commit();
            return 0;
        }

        /// <summary>
        /// 票箱领用
        /// 1.更新票箱状态信息表
        /// 2.增加票箱操作流水表
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="lastNum">操作前张数</param>
        /// <param name="currentNum">操作后张数</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int TickBoxCheckOut(string tickBoxId)
        {
            Util.DataBase.BeginTransaction();
            int res = UpdateTickBoxStatusInfo(tickBoxId, "03");
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error");
                Util.DataBase.Rollback();
            }
            res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(tickBoxId, "05");
            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_box_replace_info");
                Util.DataBase.Rollback();
            }
            Util.DataBase.Commit();
            return 0;

        }

        /// <summary>
        /// 票箱清点(清点后张数比为0)
        /// 1.修改票箱状态表，修改张数0，时间，日期
        /// 2.增加票箱操作流水表。
        /// 3.增加库存中的张数。
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="lastNum">操作前张数</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int TickBoxClear(RfidTicketboxInfo info, int lastNum, string tickManType,params int[] rfidNum)
        {
            int res = 0;
            Util.DataBase.BeginTransaction();
            res = this.UpdateTickBoxStatusInfo(info.ticketboxId, "01", 0, tickManType);
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error");
                Util.DataBase.Rollback();
                return res;
            }


            string tickStatus = "00";
            if (info.ticketboxId.Substring(2, 2) == "02")    //003:  add tick store change log
            {
                tickStatus = "01";
            }//废票箱

            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManType, tickStatus);
            if (tickStoreInfo != null)
            {
                res = this.AddTickStoreChangeLog("04", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num + lastNum), tickManType, "票箱清点",tickStatus);
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    Util.DataBase.Rollback();
                    return res;
                }
            }

            res = this.UpdateTickSotreInfo(tickManType, lastNum, true,tickStatus);
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_store_info error!");
                Util.DataBase.Rollback();
                return res;
            }

            info.deviceId = GetTickBoxDeviceId(info.ticketboxId);

            if(rfidNum==null||
                rfidNum.Length==0)
            res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(info.ticketboxId, "03", tickManType, lastNum, 0,info.deviceId);
            else
            {
                res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(info.ticketboxId, "03", tickManType, rfidNum[0],lastNum,info.deviceId);
            }
            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_box_operation failed");
                Util.DataBase.Rollback();
                return res;
            }

             res = UpdateTickBoxStatusInfo(info.ticketboxId, "01");//在库
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error!");
                Util.DataBase.Rollback();
                return res;
            }

            Util.DataBase.Commit();
            return 0;
        }


        public string GetTickBoxDeviceId(string tickBoxId)
        {
            string cmd = string.Format("select * from(select t.* from tick_box_replace_info t where t.replace_type='01' and t.ticket_box_id = '{0}' and t.line_id='{1}' and t.station_id='{2}' order by t.update_date||t.update_time desc) where rownum=1", tickBoxId, SysConfig.GetSysConfig().LocalParamsConfig.LineCode, SysConfig.GetSysConfig().LocalParamsConfig.StationCode);

       TickBoxReplaceInfo tbri=DBCommon.Instance.GetModelValue<TickBoxReplaceInfo>(cmd);

       if (tbri == null || string.IsNullOrEmpty(tbri.ticket_box_id))
           return new string('0', 8);
       else
           return tbri.device_id;
         
        }


        /// <summary>
        /// 票箱清点(清点后张数比为0)
        /// 1.修改票箱状态表，修改张数0，时间，日期
        /// 2.增加票箱操作流水表。
        /// 3.增加库存中的张数。
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="lastNum">操作前张数</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int TickBoxClear(string tickBoxId, int lastNum, string tickManType,string deviceId, params int[] rfidNum)
        {
            int res = 0;
            Util.DataBase.BeginTransaction();
            res = this.UpdateTickBoxStatusInfo(tickBoxId, "01", 0, tickManType);
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error");
                Util.DataBase.Rollback();
                return res;
            }



            //003:  add tick store change log
            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManType, "00");
            if (tickStoreInfo != null)
            {
                res = this.AddTickStoreChangeLog("04", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num + lastNum), tickManType, "票箱清点");
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    Util.DataBase.Rollback();
                    return res;
                }
            }

            res = this.UpdateTickSotreInfo(tickManType, lastNum, true);
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_store_info error!");
                Util.DataBase.Rollback();
                return res;
            }

            if (rfidNum == null ||
                rfidNum.Length == 0)
                res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(tickBoxId, "03", tickManType, lastNum, 0);
            else
            {
                res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(tickBoxId, "03", tickManType, rfidNum[0], lastNum,deviceId);
            }
            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_box_operation failed");
                Util.DataBase.Rollback();
                return res;
            }

            res = UpdateTickBoxStatusInfo(tickBoxId, "01");//在库
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error!");
                Util.DataBase.Rollback();
                return res;
            }

            Util.DataBase.Commit();
            return 0;
        }
        /// <summary>
        /// 票箱压票
        /// 1.修改票箱状态信息表。增加票卡张数，更新时间
        /// 2.插入票箱操作流水表。
        /// 3.根据库存管理类型增删库存。
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="lastNum">操作前张数</param>
        /// <param name="currentNum">操作后张数</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int TickBoxPutIn(RfidTicketboxInfo info, int lastNum, int currentNum, string tickManType)
        {
            int res = 0;
            Util.DataBase.BeginTransaction();

            //001: update tick box status set tick box num
            res = this.UpdateTickBoxStatusInfo(info.ticketboxId, "02", currentNum, tickManType);
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_box_status_info error");
                Util.DataBase.Rollback();
                return res;
            }

            //003:  add tick store change log
            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManType, "00");
            if (tickStoreInfo != null)
            {
                res = this.AddTickStoreChangeLog("03", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num - currentNum + lastNum), tickManType, "票箱补充");
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    Util.DataBase.Rollback();
                    return res;
                }
            }

            //002: update tick store info reduce tick num.
            res = this.UpdateTickSotreInfo(tickManType, currentNum-lastNum, false);//当前张数-上次张数
            if (res != 0)
            {
                WriteLog.Log_Error("update tick_store_info error!");
                Util.DataBase.Rollback();
                return res;
            }

       

            //004: add tick box operation log
            res = BuinessRule.GetInstace().logManager.WriteTickBoxOperation(info.ticketboxId, "04", tickManType, lastNum, currentNum);
            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_box_operation failed");
                Util.DataBase.Rollback();
                return res;
            }
        ////    res = UpdateTickBoxStatusInfo(tickBoxId, "03");
        //    if (res != 0)
        //    {
        //        WriteLog.Log_Error("update tick_box_status_info error");
        //        Util.DataBase.Rollback();
        //    }
            Util.DataBase.Commit();
            return 0;
        }


        public bool CheckTickStroeNum(string tickManaType, int number,params string[] status)
        {
            string tick_status = status.Count() > 0 ? status[0] : "00";
            string cmd=string.Format("select t.in_store_num from tick_storage_info t where t.tick_mana_type='{0}' and t.ticket_status='{1}'", tickManaType,tick_status);
           DataTable dt= DBCommon.Instance.GetDatatable(cmd);
           if (dt == null||dt.Rows.Count==0)//added by wangdx 20120216
               return false;
           int num = Convert.ToInt32(dt.Rows[0][0].ToString());
           return number <= num;
        }

        /// <summary>
        /// 修改库存变化
        /// </summary>
        /// <param name="tickManType">库存管理类型</param>
        /// <param name="changeNumber">变化量</param>
        /// <param name="dirFlag">变化方向 ture增加库存，false减少库存</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int UpdateTickSotreInfo(string tickManType, int changeNumber, bool dirFlag,params string[] status)
        {
            int res = 0;
            if (string.IsNullOrEmpty(tickManType))
            {
                WriteLog.Log_Error("input params error tickManType is null or empty");
                return -1;
            }
           
            TickStorageInfo info = BuinessRule.GetInstace().GetTickStoreInfoByTickManType(tickManType,status);
            if (info ==null||
                string.IsNullOrEmpty(info.tick_mana_type))
            {
                TickStorageInfo insTickStorage = new TickStorageInfo();
                insTickStorage.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                insTickStorage.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                insTickStorage.tick_mana_type = tickManType;
                insTickStorage.ticket_status = status[0];
                insTickStorage.total_num = changeNumber;
                insTickStorage.in_store_num = changeNumber;
                insTickStorage.update_date = DateTime.Now.ToString("yyyyMMdd");
                insTickStorage.update_time = DateTime.Now.ToString("HHmmss");
                 res = DBCommon.Instance.InsertTable<TickStorageInfo>(insTickStorage, "tick_storage_info");
                return res > 0 ? 0 : -1;
            }
            if (dirFlag)
            {
                info.in_store_num = info.in_store_num + changeNumber;
            }
            else
                info.in_store_num = info.in_store_num - changeNumber;
             res = DBCommon.Instance.UpdateTable<TickStorageInfo>(info, "tick_storage_info",
                new KeyValuePair<string, string>("tick_mana_type", tickManType),
                new KeyValuePair<string, string>("line_id", info.line_id),
                new KeyValuePair<string, string>("station_id", info.station_id),
                new KeyValuePair<string, string>("ticket_status", info.ticket_status));
            return res == 1 ? 0 : 1;
        }

        /// <summary>
        /// 修改库存变化
        /// </summary>
        /// <param name="tickManType">库存管理类型</param>
        /// <param name="changeNumber">变化量</param>
        /// <param name="dirFlag">变化方向 ture增加库存，false减少库存</param>
        /// <param name="status">00 正常，01：废票</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int UpdateTickSotreInfo(string tickManType, int updateNum,params string[] status)
        {
            int res = 0;

            if (string.IsNullOrEmpty(tickManType))
            {
                WriteLog.Log_Error("input params error tickManType is null or empty");
                return -1;
            }

            string tick_status = status.Count() > 0 ? status[0] : "00";

            TickStorageInfo info = BuinessRule.GetInstace().GetTickStoreInfoByTickManType(tickManType,status);
            if (info != null && !string.IsNullOrEmpty(info.tick_mana_type))
            {
                info.in_store_num = updateNum;
                info.update_date = DateTime.Now.ToString("yyyyMMdd");
                info.update_time = DateTime.Now.ToString("HHmmss");

                List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
                tempList.Add(new KeyValuePair<string, string>("LINE_ID", info.line_id));
                tempList.Add(new KeyValuePair<string, string>("STATION_ID", info.station_id));
                tempList.Add(new KeyValuePair<string, string>("TICK_MANA_TYPE", info.tick_mana_type));
                tempList.Add(new KeyValuePair<string, string>("TICKET_STATUS", tick_status));
                
                res = DBCommon.Instance.UpdateTable<TickStorageInfo>(info, "tick_storage_info", tempList.ToArray());
            }
            else
            {
                TickStorageInfo insTickStorage = new TickStorageInfo();
                insTickStorage.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                insTickStorage.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                insTickStorage.tick_mana_type = tickManType;
                insTickStorage.ticket_status = tick_status;
                insTickStorage.total_num = updateNum;
                insTickStorage.in_store_num = updateNum;
                insTickStorage.update_date = DateTime.Now.ToString("yyyyMMdd");
                insTickStorage.update_time = DateTime.Now.ToString("HHmmss");
                res = DBCommon.Instance.InsertTable<TickStorageInfo>(insTickStorage, "tick_storage_info");
            }
           
            return res;
        }


        public List<TickBoxStatusInfo> GetAllTickOutInfo()
        {
            string sqlQuery = string.Format("select t.* from tick_box_status_info t ");
            string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            sqlQuery += string.Format(" where t.box_position ='01' and t.tickets_num =0 and t.line_id ='{0}' and  t.station_id='{1}'", lineID, stationID);
            return DBCommon.Instance.SetTModelValue<TickBoxStatusInfo>(sqlQuery);
        }

        /// <summary>
        /// 删除票箱状态表
        /// </summary>
        /// <param name="cashMoneyStatusInfo"></param>
        /// <returns></returns>
        public int delTickBoxStatusInfo(string tickBoxID)
        {
            int res = 0;
            string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            string delSql = string.Format("delete tick_box_status_info t where t.line_id='{0}' and t.station_id='{1}' and t.ticket_box_id ='{2}'", lineID, stationID, tickBoxID);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                if (res == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 删除票箱登记表
        /// </summary>
        /// <param name="cashMoneyStatusInfo"></param>
        /// <returns></returns>
        public int delTickBoxRegistorInfo(string tickBoxID)
        {
            int res = 0;
            string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            string delSql = string.Format("delete tick_box_registor_info t where t.line_id='{0}' and t.station_id='{1}' and t.ticket_box_id ='{2}'", lineID, stationID, tickBoxID);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                if (res == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 操作员散票领用
        /// 
        /// begin transaction.
        /// 001：reduce tick store
        /// 002：if operator_not_exist insert or update(number=last+current)
        /// 003:write log
        /// commit transaction.
        /// </summary>
        /// <param name="tickManaType">库存管理类型</param>
        /// <param name="operatorId">操作员ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int TickOperatorCheckOut(string tickManaType, string operatorId,int number)
        {
            if (string.IsNullOrEmpty(tickManaType) ||
            string.IsNullOrEmpty(operatorId))
            {
                WriteLog.Log_Error("tickManaType is null or empty,operatorId is null or empty!");
                return -1;
            }
            Util.DataBase.BeginTransaction();
              int res=0;
            //004:  add tick store change log
            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType, "00");
            if (tickStoreInfo != null)
            {
              res= this.AddTickStoreChangeLog("05", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num - number), tickManaType, "操作员领用");
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    Util.DataBase.Rollback();
                    return res;
                }
            }
            TickStorageInfo info = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType);
            int curernt = (int)info.in_store_num- number;
              res= UpdateTickSotreInfo(tickManaType, curernt);
            if (res != 1)
            {
                WriteLog.Log_Error("update tick_store_info failed");
                Util.DataBase.Rollback();
                return res;
            }
            TickInOperatorInfo tickOperatorInfo = BuinessRule.GetInstace().GetTickInOperatorInfoByOperatorId(operatorId, tickManaType);
            if (string.IsNullOrEmpty(tickOperatorInfo.operator_id))
            {
                tickOperatorInfo.operator_id = operatorId;
                tickOperatorInfo.tick_mana_type = tickManaType;
                tickOperatorInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                tickOperatorInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                tickOperatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                tickOperatorInfo.update_time = DateTime.Now.ToString("HHmmss");
                tickOperatorInfo.ticket_in_hand = number;
                res=DBCommon.Instance.InsertTable<TickInOperatorInfo>(tickOperatorInfo, "tick_in_operator_info");
                if (res != 1)
                {
                    WriteLog.Log_Error("insert tick_operator_info failed");
                    Util.DataBase.Rollback();
                    return res;
                }
            }
            else
            {
                tickOperatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                tickOperatorInfo.update_time = DateTime.Now.ToString("HHmmss");
                tickOperatorInfo.ticket_in_hand = number + tickOperatorInfo.ticket_in_hand;
                res = DBCommon.Instance.UpdateTable<TickInOperatorInfo>(tickOperatorInfo,
                    "tick_in_operator_info",
                    new KeyValuePair<string, string>("operator_id", operatorId),
                    new KeyValuePair<string, string>("tick_mana_type", tickManaType));
                if (res != 1)
                {
                    WriteLog.Log_Error("update tick_operator_info failed");
                    Util.DataBase.Rollback();
                    return res;
                }
            }
            res = AddTickCheckInOutLog("00", operatorId, tickManaType, number);
            if (res != 0)
            {
                WriteLog.Log_Error("insert  tick_operator_return_log failed");
                Util.DataBase.Rollback();
                return res;
            }
            Util.DataBase.Commit();
            return 0;
        }

        /// <summary>
        /// 操作员散票归还
        /// begin transaction
        /// 001 add tick store
        /// 002:set tick_in_operator numer=0
        /// 003:write log
        /// commit transaction()
        /// </summary>
        /// <param name="tickManaType">库存管理类型</param>
        /// <param name="opeatorId">操作员ID</param>
        /// <param name="number">数量</param>
        /// <param name="tickStatus">票卡状态</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int TickOperatorCheckIn(string tickManaType, string opeatorId,int number,params string[] tickStatus)
        {
            string tick_status = tickStatus.Count() > 0 ? tickStatus[0] : "00";
            int res=0;
            if (string.IsNullOrEmpty(tickManaType) ||
                string.IsNullOrEmpty(opeatorId))
            {
                WriteLog.Log_Error("tickManaType is null or empty,operatorId is null or empty!");
                return -1;
            }
            Util.DataBase.BeginTransaction();
            //001:  add tick store change log
            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType, tick_status);
            if (tickStoreInfo != null)
            {
              res = this.AddTickStoreChangeLog("06", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num + number), tickManaType, "操作员归还(BOM)",tick_status);
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    Util.DataBase.Rollback();
                    return res;
                }
            }
            //002:update tick store
            TickStorageInfo info = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType,tick_status);
            int current = 0;
            if (info != null &&
                !string.IsNullOrEmpty(info.tick_mana_type) //存在该库存管理类型时候，直接加
                )
            {
                 current = (int)info.in_store_num + number;
            }
            else //insert data
            {
                current = number;
            }
            res = this.UpdateTickSotreInfo(tickManaType, current, tickStatus);
            if (res != 1)
            {
                WriteLog.Log_Error("update tick_store_info failed");
                Util.DataBase.Rollback();
                return res;
            }
            //003:clear ticket num to zero
             TickInOperatorInfo tickOperatorInfo = BuinessRule.GetInstace().GetTickInOperatorInfoByOperatorId(opeatorId, tickManaType);
             if (!string.IsNullOrEmpty(tickOperatorInfo.operator_id))
             {
                 tickOperatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                 tickOperatorInfo.update_time = DateTime.Now.ToString("HHmmss");
                 tickOperatorInfo.ticket_in_hand = 0;
                 res = DBCommon.Instance.UpdateTable<TickInOperatorInfo>(tickOperatorInfo,
                     "tick_in_operator_info",
                     new KeyValuePair<string, string>("operator_id", opeatorId),
                     new KeyValuePair<string, string>("tick_mana_type", tickManaType));
                 if (res != 1)
                 {
                     WriteLog.Log_Error("update tick_operator_info failed");
                     Util.DataBase.Rollback();
                     return res;
                 }
            
             }
             //004 :add operator check in out log
             res = AddTickCheckInOutLog("01", opeatorId, tickManaType, number);
             if (res != 0)
             {
                 WriteLog.Log_Error("insert  tick_operator_return_log failed");
                 Util.DataBase.Rollback();
                 return res;
             }
                Util.DataBase.Commit();
                return 0;
        }


        public int TickOperatorCheckIn(string tickManaType, string opeatorId, int number, string tickStatus, string type)
        {
            string tick_status = tickStatus;
            int res = 0;
            if (string.IsNullOrEmpty(tickManaType) ||
                string.IsNullOrEmpty(opeatorId))
            {
                WriteLog.Log_Error("tickManaType is null or empty,operatorId is null or empty!");
                return -1;
            }
            Util.DataBase.BeginTransaction();
            //001:  add tick store change log
            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType, tick_status);
            if (tickStoreInfo != null)
            {
                if (type == "00")// from bom
                {
                    res = this.AddTickStoreChangeLog("06", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num + number), tickManaType, "操作员归还(BOM)", tick_status);
                }
                else //from waste tickBox
                {
                    res = this.AddTickStoreChangeLog("07", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num + number), tickManaType, "操作员归还(回收盒)", tick_status);
                }
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    Util.DataBase.Rollback();
                    return res;
                }
            }
            //002:update tick store
            TickStorageInfo info = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType, tick_status);
            int current = 0;
            if (info != null &&
                !string.IsNullOrEmpty(info.tick_mana_type) //存在该库存管理类型时候，直接加
                )
            {
                current = (int)info.in_store_num + number;
            }
            else //insert data
            {
                current = number;
            }
            res = this.UpdateTickSotreInfo(tickManaType, current, tickStatus);
            if (res != 1)
            {
                WriteLog.Log_Error("update tick_store_info failed");
                Util.DataBase.Rollback();
                return res;
            }
            //003:clear ticket num to zero
            TickInOperatorInfo tickOperatorInfo = BuinessRule.GetInstace().GetTickInOperatorInfoByOperatorId(opeatorId, tickManaType);
            if (!string.IsNullOrEmpty(tickOperatorInfo.operator_id))
            {
                tickOperatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                tickOperatorInfo.update_time = DateTime.Now.ToString("HHmmss");
                tickOperatorInfo.ticket_in_hand = 0;
                res = DBCommon.Instance.UpdateTable<TickInOperatorInfo>(tickOperatorInfo,
                    "tick_in_operator_info",
                    new KeyValuePair<string, string>("operator_id", opeatorId),
                    new KeyValuePair<string, string>("tick_mana_type", tickManaType));
                if (res != 1)
                {
                    WriteLog.Log_Error("update tick_operator_info failed");
                    Util.DataBase.Rollback();
                    return res;
                }

            }
            //004 :add operator check in out log
            //2013年5月21日根据天津业主要求“回收盒归还”单独统计的需求，在tick_operator_return_log表中增加“回收盒归还”类型，“回收盒归还”类型为“02”
            //res = AddTickCheckInOutLog("01", opeatorId, tickManaType, number);
            if (type == "00")// from bom
            {
                res = AddTickCheckInOutLog("01", opeatorId, tickManaType, number);
            }
            else //from waste tickBox
            {
                res = AddTickCheckInOutLog("02", opeatorId, tickManaType, number);
            }
            if (res != 0)
            {
                WriteLog.Log_Error("insert tick_operator_return_log failed");
                Util.DataBase.Rollback();
                return res;
            }
            Util.DataBase.Commit();
            return 0;
        }

        /// <summary>
        /// 操作员自定义票卡归还
        /// begin transaction
        /// 001 add tick store
        /// 002:set tick_in_operator numer=0
        /// 003:write log
        /// commit transaction()
        /// </summary>
        /// <param name="tickManaType">库存管理类型</param>
        /// <param name="opeatorId">操作员ID</param>
        /// <param name="number">数量</param>
        /// <returns>成功返回1，否则返回-1</returns>
        public int TickOperatorCheckIn(string tickManaType, string opeatorId, int number)
        {
            //正常票
            string tick_status = "00";
            int res = 1;
            if (string.IsNullOrEmpty(tickManaType) ||
                string.IsNullOrEmpty(opeatorId))
            {
                WriteLog.Log_Error("tickManaType is null or empty,operatorId is null or empty!");
                return -1;
            }
            //001:  add tick store change log
            TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType, tick_status);
            if (tickStoreInfo != null)
            {
                res = this.AddTickStoreChangeLog("06", (int)tickStoreInfo.in_store_num, (int)(tickStoreInfo.in_store_num + number), tickManaType, "操作员归还", tick_status);
                if (res != 0)
                {
                    WriteLog.Log_Error("add tick store change log error!");
                    return -1;
                }
            }
            //002:update tick store
            TickStorageInfo info = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(tickManaType, tick_status);
            int current = 0;
            if (info != null &&
                !string.IsNullOrEmpty(info.tick_mana_type) //存在该库存管理类型时候，直接加
                )
            {
                current = (int)info.in_store_num + number;
            }
            else //insert data
            {
                current = number;
            }
            res = this.UpdateTickSotreInfo(tickManaType, current, tick_status);
            if (res != 1)
            {
                WriteLog.Log_Error("update tick_store_info failed");
                return -1;
            }
            //003:clear ticket num to zero
            TickInOperatorInfo tickOperatorInfo = BuinessRule.GetInstace().GetTickInOperatorInfoByOperatorId(opeatorId, tickManaType);
            if (!string.IsNullOrEmpty(tickOperatorInfo.operator_id))
            {
                tickOperatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                tickOperatorInfo.update_time = DateTime.Now.ToString("HHmmss");
                tickOperatorInfo.ticket_in_hand = 0;
                res = DBCommon.Instance.UpdateTable<TickInOperatorInfo>(tickOperatorInfo,
                    "tick_in_operator_info",
                    new KeyValuePair<string, string>("operator_id", opeatorId),
                    new KeyValuePair<string, string>("tick_mana_type", tickManaType));
                if (res != 1)
                {
                    WriteLog.Log_Error("update tick_operator_info failed");
                    return -1;
                }
              
            }

            //004 :add operator check in out log
            res = AddTickCheckInOutLog("01", opeatorId, tickManaType, number);
            if (res != 0)
            {
                WriteLog.Log_Error("insert  tick_operator_return_log failed");
                return -1;
            }
            return 1;
        }

      /// <summary>
      /// 票卡调入log
      /// </summary>
      /// <param name="oriStation">调出车站</param>
      /// <param name="tickManaType">库存类型</param>
      /// <param name="sysNum">系统张数</param>
      /// <param name="realNum">实际张数</param>
      /// <param name="outOperatorId">调出操作员</param>
      /// <param name="status">状态，00 正常，01 废票</param>
      /// <returns></returns>
        public int AddTickCallInLog(string oriStation,string outOperatorId,string tickManaType, int sysNum, int realNum,params string[] status)
        {
            TickDispatchLog log = new TickDispatchLog();
            log.tick_mana_type = tickManaType;
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.dsipatch_out_station_id = oriStation;// 调出车站
            log.dispatch_out_oper_id = outOperatorId;//上一个车站的调出人
            log.dispatch_out_real_num = realNum;//实际调出张数=调入张数
            log.dispatch_out_sys_num = sysNum;//输出来的数据
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_time = DateTime.Now.ToString("HHmmss");
            log.dispatch_in_oper_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;//调入人
            log.dispatch_in_station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            log.dispatch_in_real_num = realNum;
            log.dispatch_in_sys_num = sysNum;
            if (status.Length == 0)
                log.tick_status = "00";
            else
                log.tick_status = status[0];
            int res=0;
            log.dsipatch_no = Util.DataBase.GetSequenceNextVal(out res, "BUSI_UDSN_SEC");
            log.dispatch_type = "01";
            res= DBCommon.Instance.InsertTable<TickDispatchLog>(log, "tick_dispatch_log");
           return res == 1 ? 0 : res;
        }

        /// <summary>
        /// 票卡调出
        /// </summary>
        /// <param name="desStation">调入车站</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <param name="sysNum">调出张数</param>
        /// <param name="realNum">调出实际张数</param>
        /// <returns>成功返回0</returns>
        public int AddTickCallOutLog(string desStation, string tickManType, int sysNum, int realNum,params string[] tick_status)
        {
            TickDispatchLog log = new TickDispatchLog();
            log.dispatch_type = "00";//出库
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.dispatch_in_real_num = realNum;
            log.dispatch_in_station_id = desStation;
            log.dispatch_in_sys_num = sysNum;
            log.dispatch_in_oper_id = string.Empty;
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_time = DateTime.Now.ToString("HHmmss");
            if (tick_status.Length == 0)
                log.tick_status = "00";
            else
                log.tick_status = tick_status[0];
            log.tick_mana_type = tickManType;
            log.dispatch_out_oper_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.dispatch_out_real_num = realNum;
            log.dispatch_out_sys_num = sysNum;
            log.dsipatch_out_station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            int res = 0;
            log.dsipatch_no = Util.DataBase.GetSequenceNextVal(out res, "BUSI_UDSN_SEC");

            res = DBCommon.Instance.InsertTable<TickDispatchLog>(log, "tick_dispatch_log");
            return res == 1 ? 0 : res;


        }

        /// <summary>
        /// 操作员散票领用归还流水记录
        /// </summary>
        /// <param name="type">操作类型，00 领用 ，01 归还</param>
        /// <param name="operatorId">领用归还操作员ID</param>
        /// <param name="tickManaType">库存管理类型</param>
        /// <param name="tickNum">票卡张数</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int AddTickCheckInOutLog(string type,string operatorId,string tickManaType,int tickNum)
        {
            TickOperatorReturnLog torl = new TickOperatorReturnLog();
            torl.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            torl.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            int res=0;
            torl.oper_sec = Util.DataBase.GetSequenceNextVal(out res, "BUSI_UDSN_SEC");
            torl.operate_type = type;
            torl.operator_id = operatorId;
            torl.ticket_num = tickNum;
            torl.tick_mana_type = tickManaType;
            torl.update_date = DateTime.Now.ToString("yyyyMMdd");
            torl.update_time = DateTime.Now.ToString("HHmmss");
            res = DBCommon.Instance.InsertTable<TickOperatorReturnLog>(torl, "tick_operator_return_log");
            return res == 1 ? 0 : res;
        }


        /// <summary>
        /// 记录库存变化日志
        /// </summary>
        /// <param name="changeType">变化类型 
        /// 00 调入，
        /// 01：调出，
        /// 02：库存调整 ,
        /// 03:票箱补充
        /// 04：票箱清点 
        /// 05：操作员领用
        /// 06:操作员归还</param>
        /// <param name="beforeNum">变化前</param>
        /// <param name="afterNum">变化后</param>
        /// <param name="tickManaType">库存管理类型</param>
        /// <param name="tick_status">票卡状态 00 正常 ，01：废票</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddTickStoreChangeLog(string changeType, int beforeNum, int afterNum,string tickManaType,string remark,params string[] tick_status)
        {
            int res=0;
            TickStoreChangeLog log = new TickStoreChangeLog();
            log.oper_sec = Util.DataBase.GetSequenceNextVal(out res, "BUSI_UDSN_SEC");
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            log.tick_mana_type = tickManaType;
            log.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.change_type = changeType;
            if (tick_status.Length > 0)
                log.tick_status = tick_status[0];
            else
                log.tick_status = "00";
            log.original_ticket_num = beforeNum;
            log.later_ticket_num = afterNum;
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_time = DateTime.Now.ToString("HHmmss");
            log.remark = remark;
            res=DBCommon.Instance.InsertTable<TickStoreChangeLog>(log, "tick_store_change_log");
            return res == 1 ? 0 : -1;
        }

        /// <summary>
        /// 取得操作员领用的自定义票种数量
        /// </summary>
        /// <param name="tickManaType">自定义票种</param>
        /// <returns></returns>
        public TickInOperatorInfo GetOperatorInTickNum(string tickManaType,string operatorCode)
        {
            try
            {
                string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                string sqlQuery = string.Format("select t.* from tick_in_operator_info t where t.line_id='{0}' and t.station_id='{1}' and t.tick_mana_type='{2}' and t.operator_id='{3}'", lineID, stationID, tickManaType, operatorCode);
                return DBCommon.Instance.SetModelValue<TickInOperatorInfo>(sqlQuery);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
        }
    }
}
