using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.CommBuiness
{
    using TJComm;
    using AFC.WS.Model.Const;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.Comm;
    using AFC.WS.BR.DataImportExport;
    using AFC.WS.Model.DB;

    /// <summary>
    /// 定义了通讯业务数据
    /// added by wangdx ;date:20110314
    /// 通讯程序API中 操作员ID的取值一律从currentLogInOperatorId
    /// 中获得，不要引用BR中的变量。避免依赖关系，BR调用通讯，这样BR
    /// 和通讯的关系就是相互依赖了。
    /// edited by wangdx 20120802 
    /// 在重连接事件中增加了是否为第一次重练判断机制
    /// </summary>
    public class CommBuiness:ICommProcess
    {
        private bool isFirstCall;

        /// <summary>
        /// 连接对象
        /// </summary>
        public CommConnection con = null;

        /// <summary>
        /// 联机状态
        /// </summary>
        private bool onlineStatus;

        ///// <summary>
        ///// 当前操作员ID
        ///// </summary>
        //public uint currentLogInOperatorId;

        public SysConfig sysConfig = SysConfig.GetSysConfig();
        
        /// <summary>
        /// 初始化
        /// </summary>
        public bool Init()
        {
            con = new CommConnection(sysConfig.CommParamsConfig.ScIPAddress,
                sysConfig.CommParamsConfig.ScPort,
                50);
            CommConfigs config = con.LoadCommConfigFile();
            if (config != null)
            {
                con.SetupCommConfig(config);
                TJCommMessage.SetDevcieId(sysConfig.LocalParamsConfig.DeviceCode.ConvertHexStringToUint(),
                sysConfig.CommParamsConfig.SCDeviceId.ConvertHexStringToUint(),
                (ushort)sysConfig.LocalParamsConfig.StationCode.ConvertHexStringToUint());
            }
           con.ConnectionStateChanged += new EventHandler(con_ConnectionStateChanged);
           return con.Connect(3) == 0;
        }

        private  void con_ConnectionStateChanged(object sender, EventArgs e)
        {
            if (!isFirstCall)
            {
                isFirstCall = true;
                return;
            }

            CommConnection con = sender as CommConnection;
            if (con.Connected)
            {
                BuinessRule.GetInstace().brConext.NetworkStatus = true;
                int res = BuinessRule.GetInstace().commProcess.CheckIn();
                if (res == 0)
                {
                    BuinessRule.GetInstace().brConext.OnlineStatus = true;
                    return;
                }
                BuinessRule.GetInstace().brConext.OnlineStatus = false;
                con.Disconnect();
            }
            else
            {
                BuinessRule.GetInstace().brConext.OnlineStatus = false;
                BuinessRule.GetInstace().brConext.NetworkStatus = false;
            }
            
           
           

           //System.Windows.MessageBox.Show("Connect status:"+(sender as CommConnection).Connected.ToString());
           
        }

        /// <summary>
        /// WS连接认证
        /// </summary>
        /// <returns>0successful，否则错误</returns>
        public int CheckIn()
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Check_In, CommandType.RESQUEST);
            CheckIn_1390 body = AbstractCommBody.CreateCommBody(header, 0x00) as CheckIn_1390;
            body.remark= 0;
            TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage outMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(msg, out outMsg);
            if (res ==0)
            {
                CheckInResponse_1390 resBody = outMsg.packageBody as CheckInResponse_1390;
                if (resBody == null)
                    return -3;//配置错误
                if (resBody.resultCode == 0)
                {
                    BuinessRule.GetInstace().brConext.FtpUser = resBody.ftpUser;
                    BuinessRule.GetInstace().brConext.FtpPwd = resBody.ftpPwd;
                    return resBody.resultCode;
                }
                else
                    return resBody.resultStatusCode;
            }
            con.Disconnect();//send failed disconnect socket
            return res;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="pwd">操作员密码</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int LogIn(uint operatorId, string pwd)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Log_In_Out, CommandType.RESQUEST);
            OperatorLogInOut_1301 info = AbstractCommBody.CreateCommBody(header,
                operatorId) as OperatorLogInOut_1301;
            info.loginType = 0;
            info.password = pwd;
            info.operatorId = operatorId;
            TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage outMsg=new TJCommMessage();
            int res= con.SendMsgAndReceive(msg, out outMsg);
            if (res==0)
            {
                CommonResponseMsg msg1 = outMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                return (int)msg1.resultStatusCode;
            }
            return res;
        }

        /// <summary>
        /// 登录，登出
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public int LogOut(uint operatorId)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Log_In_Out, CommandType.RESQUEST);
            OperatorLogInOut_1301 info = AbstractCommBody.CreateCommBody(header,
                operatorId) as OperatorLogInOut_1301;
            info.loginType = 1;
            info.password = "0";
            info.operatorId = operatorId;
            TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage outMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(msg, out outMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = outMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        /// <summary>
        /// 操作员修改密码
        /// </summary>
        /// <param name="opeatorId">操作员ID</param>
        /// <param name="oldPwd">操作员旧密码</param>
        /// <param name="newPwd">操作员新密码</param>
        /// <returns>0：成功，其他错误</returns>
        public int ChangePwd(uint opeatorId, string oldPwd, string newPwd)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Change_Pwd, CommandType.RESQUEST);
            OperatorChangePwd_1302 info = AbstractCommBody.CreateCommBody(header, opeatorId) as OperatorChangePwd_1302;
            info.operatorId = opeatorId;
            info.newPwd = newPwd;
            info.oldPwd = oldPwd;
            info.change_reason = 2;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
           
        }

        /// <summary>
        /// 操作员解锁
        /// </summary>
        /// <param name="unLockOperatorId">解锁的操作员ID</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int UnlockOperator(uint unLockOperatorId)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Operator_Unlocked, CommandType.RESQUEST);
            OperatorUnLock_1305 body = AbstractCommBody.CreateCommBody(header,
                BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as OperatorUnLock_1305;
            body.unlockedOperatorId = unLockOperatorId;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
           int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
           if (res == 0)
           {
               CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
               if (msg1 == null) return -3;
               if (msg1.resultCode == 0)
                   return msg1.resultCode;
               else
                   return (int)msg1.resultStatusCode;
           }
           return res;
          
        }

        public int LockOperator(uint operatorId)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Operator_Locked, CommandType.RESQUEST);
            OperatorLocked_1304 info = AbstractCommBody.CreateCommBody(header, operatorId) as OperatorLocked_1304;
            info.lockReason = 0;
            info.operatorId = operatorId;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = null;
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
            //todo:Check

        }
        /// <summary>
        /// 操作员强制登出
        /// </summary>
        /// <param name="operatorId">操作员</param>
        /// <returns></returns>

        public int OperatorForceLogOut(uint operatorId,uint deviceId,ushort stationId)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Operator_Force_LogOut, CommandType.RESQUEST);
            OperatorForceLogOut_1306 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as OperatorForceLogOut_1306;
            body.operatorId = operatorId;
            body.deviceId = deviceId;
            List<DeviceRange> list = new List<DeviceRange>();
            List<uint>devList=new List<uint>();
            devList.Add(deviceId);
            list.Add(new DeviceRange { special_flag = 2, 
                stationId = stationId, 
                 deviceRange=devList });
            //body.headerData.deviceRange = list;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;

           
        }

        /// 发布草稿版
        /// </summary>
        /// <param name="activeDate">生效日期</param>
        /// <param name="publishDate">发布日期</param>
        /// <param name="listPara">参数类型</param>
        /// <returns></returns>
        public int PublishParaDraft(uint activeDate, uint publishDate, List<ushort> listPara)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Params_Publish, CommandType.RESQUEST);
            ParamsPublish_1315 info = AbstractCommBody.CreateCommBody(header,
                BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as ParamsPublish_1315;
            info.activeDate = activeDate;
            info.publishDate = publishDate;
            info.listParamType = listPara;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = null;
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        /// <summary>
        /// 运营日结算
        /// </summary>
        /// <param name="CashDateSettlementInfo"></param>
        /// <returns></returns>
        public int DateSettlement(CashDateSettlementInfo ds,DateTime dt)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Date_Settlement, CommandType.RESQUEST);
            DateSettlement_1353 info = AbstractCommBody.CreateCommBody(header,
                BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as DateSettlement_1353;
            info.account_income = Convert.ToInt64(ds.account_income);
            info.bom_income = Convert.ToInt64(ds.bom_income);
            info.coin_store_amount = Convert.ToInt64(ds.coin_store_amount);
            info.group_tickets_income = Convert.ToInt64(ds.group_tickets_income);
            info.income_store = Convert.ToInt64(ds.income_store);
            info.oper_time = GetCurrentAFCTime_t(dt);
            info.others_income=  Convert.ToInt64(ds.others_income);
            info.run_date = GetCurrentAFCDate_t(DateTime.ParseExact(ds.run_date, "yyyyMMdd", null));
            info.tickets_remain=  Convert.ToInt64(ds.tickets_remain);
            info.today_cash_bank_total=  Convert.ToInt64(ds.today_cash_bank_total);
            info.today_diff_amount=  Convert.ToInt64(ds.today_diff_amount);
            info.today_income_amount=  Convert.ToInt64(ds.today_income_amount);
            info.today_subway_income=  Convert.ToInt64(ds.today_subway_income);
            info.tomorrow_bank_income = Convert.ToInt64(ds.tomorrow_bank_income);
            info.tvm_income=  Convert.ToInt64(ds.tvm_income);
            info.urgency_tickets_income=  Convert.ToInt64(ds.urgency_tikets_income);
            info.yesterday_income_amount = Convert.ToInt64(ds.yesterday_income_amount);
            TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage outMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(msg, out outMsg);
          
            if (res == 0)
            {
                CommonResponseMsg msg1 = outMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }


        public int TickUpdateNotify(TickValuedProductInfo tickInfo)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Tick_UpdateNotify, CommandType.RESQUEST);    
            TickUpdateNotify_1361 info = AbstractCommBody.CreateCommBody(header,
                BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as TickUpdateNotify_1361;
            //info.headerData.deviceRange = new List<DeviceRange>();
            //info.headerData.deviceRange.Add(new DeviceRange { deviceRange = new List<uint>(), special_flag = 0 });
            info.cardIssueID = tickInfo.card_issue_id.ToHexNumberByte();
            info.productType = tickInfo.product_flag.ToHexNumberByte();
            info.preStoreMoney = Convert.ToUInt32(tickInfo.pre_store_money);
            info.tickDeposit = Convert.ToUInt32(tickInfo.tick_deposit);
            info.tickManaType = tickInfo.tick_mana_type;
            info.tickManaTypeName = tickInfo.tick_mana_type_name;
            info.tickPhyType = 0;
            info.tickSaleValue = Convert.ToUInt32(tickInfo.tick_sale_value);
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public uint GetCurrentAFCTime_t(DateTime dt)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            return (uint)dt.Subtract(dt1970).TotalSeconds - 8 * 60 * 60;

        }

        public uint GetCurrentAFCDate_t(DateTime dt)
        {
            DateTime dt2000 = new DateTime(2000, 1, 1, 0, 0, 0);
            return (uint)dt.Subtract(dt2000).TotalDays;

        }

        public int ParamsDownloadNotify(ushort paramType,List<DeviceRange> list)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Param_Download_Notify, CommandType.RESQUEST);
            ParamsDownLoadNotify_1309 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as ParamsDownLoadNotify_1309;
            body.paramType = paramType;
            //body.headerData.deviceRange = list;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public int DataImportNotify(byte type, string strImportPath)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Data_Import_Notify, CommandType.RESQUEST);
            DataImportNotify_1009 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as DataImportNotify_1009;
            body.importType = type;
            body.importPath = strImportPath;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }


        public int SpecialParamsDownLoadNotify(List<ParamsData> paraList, List<DeviceRange> list)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Specail_Params_Download_Notify, CommandType.RESQUEST);
            SpecialParamsDownLoadNotify_1313 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as SpecialParamsDownLoadNotify_1313;
            body.parmsData = paraList;
            //body.headerData.deviceRange = list;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public uint currentLogInOperatorId
        {
            set;
            get;
        }

        #region ICommProcess 成员


        public bool OnLineStatus
        {
            get
            {
                return this.onlineStatus;
            }
            set
            {
                this.onlineStatus = value;
            }
        }

        #endregion

        #region ICommProcess 成员


        public int ForceTimeSyn(uint currentTime)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Force_Time_Syn, CommandType.RESQUEST);
            ForcenTimeSyn_1333 info = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as ForcenTimeSyn_1333;
            info.currentTime = currentTime;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = null;
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public int TimeSyn(uint currentTime)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Time_Syn, CommandType.RESQUEST);
            SetTimeSyn_1334 info = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as SetTimeSyn_1334;
            info.currentTime = currentTime;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = null;
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public int BuildParamFile()
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Param_Download_Notify, CommandType.RESQUEST);
            CreateParamsFile_1317 info = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as CreateParamsFile_1317;
            info.remark = 0;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, info);
            TJCommMessage receiveMsg = null;
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public int ModeChange(uint stationId, uint modeCode)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Mode_Change_CMD, CommandType.RESQUEST);
            ModeChange_1341 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as ModeChange_1341;
            body.mode_station_id = stationId;
            body.mode_code=modeCode;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        

        #endregion

        #region ICommProcess 成员


        public int ControlCmd(byte controlType, ushort controlCode, List<DeviceRange> list)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Control_CMD, CommandType.RESQUEST);
            ControlCmd_3000 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as ControlCmd_3000;
            //body.SendTime = BitConverter.GetBytes(DateTime.Now.ToBinary());
            body.SendDeviceId = sysConfig.LocalParamsConfig.DeviceCode.ConvertHexStringToUint();

            body.operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            body.controlCode = controlCode;

            int res = 0;
            for (int iIndex = 0; iIndex < list.Count; iIndex++)
            {
                if(0 == list[iIndex].special_flag)
                {
                    body.DestDeviceId = 0x01000000; 
                }
                else if (1 == list[iIndex].special_flag)
                {
                    //body.DestDeviceId = list[iIndex].stationId;
                }
                else if (2 == list[iIndex].special_flag)
                {
                    
                    //body.DestDeviceId = list[iIndex].stationId;
                }

                TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
                TJCommMessage receiveMsg = new TJCommMessage();
                res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
                if (0 != res)
                { 
                    //log 
                    return res;
                }
                
            }
            return res;
        }

        public int RunStart()
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Run_Start, CommandType.RESQUEST);
            RunStart_1351 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as RunStart_1351;
            body.remark = 0;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        public int RunEnd()
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Run_End, CommandType.RESQUEST);
            RunEnd_1352 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as RunEnd_1352;
            body.remark = 0;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }

        #endregion
    
         #region ICommProcess 成员



         /// <summary>
        /// 数据补传
        /// </summary>
        /// <param name="ReUploadRecords"></param>
        /// <returns></returns>
        public int ReUploadRecords(uint deviceId, uint dataType, uint tranDateBegin, uint tranDateEnd)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Data_ReUploadRecords, CommandType.RESQUEST);
            ReUploadRecords_1363 body = AbstractCommBody.CreateCommBody(header, BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint()) as ReUploadRecords_1363;
            //body.deviceId = deviceId;
            body.beginTime = tranDateBegin;
            body.endTime = tranDateEnd;
            body.operType = 0x02;
            body.dataType = dataType;
            List<DeviceRange> list = new List<DeviceRange>();
            List<uint> devList = new List<uint>();
            devList.Add(deviceId);
            list.Add(new DeviceRange
            {
                special_flag = 2,
                stationId = sysConfig.LocalParamsConfig.StationCode.ToHexNumberUShort(),
                deviceRange = devList
            });
            //body.headerData.deviceRange = list;
            TJCommMessage sendMsg = TJCommMessage.CreateTJCommMsg(header, body);
            TJCommMessage receiveMsg = new TJCommMessage();
            int res = con.SendMsgAndReceive(sendMsg, out receiveMsg);
            if (res == 0)
            {
                CommonResponseMsg msg1 = receiveMsg.packageBody as CommonResponseMsg;
                if (msg1 == null) return -3;
                if (msg1.resultCode == 0)
                    return msg1.resultCode;
                else
                    return (int)msg1.resultStatusCode;
            }
            return res;
        }


#endregion
}
}
