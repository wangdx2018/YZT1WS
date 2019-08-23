using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR
{
    using AFC.WS.Model;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.BR.Data;
    using AFC.WS.Model.Const;
    using System.Data;
    /// <summary>
    /// 该文件中定义基础类的查询代码
    /// edit by wangdx 20110510 增加了函数 GetAllDevRunStatusDetail(),
    /// GetAllDevRunStatusInfo
    /// edit by wangdx 20110518 增加了GetTickBoxStausInfo()函数
    /// 增加了 TickStoreageInfo()函数
    /// edit by wangdx 20110526 增加了GetTickManaTypeInfoById(string id)
    /// 
    /// edit by wangdx 20110624 增加了获得模式名称的函数
    /// </summary>
    public partial class BuinessRule
    {
        #region Basic DB Data function
        /// <summary>
        /// 得到所有的车站信息
        /// </summary>
        /// <returns>返回车站信息集合</returns>
        public List<BasiStationInfo> GetAllStationInfo()
        {
            return DBCommon.Instance.GetTModelValue<BasiStationInfo>("select * from basi_station_info where location_type ='2' and line_id='03' order by station_id");
        }

        /// <summary>
        /// 得到票箱与设备关系信息
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="installPosition">安装位置</param>
        /// <returns>成功返回TickBoxInDevInfo，否则返回null</returns>
        public  List<TickBoxInDevInfo> GetTickInDevInfosByDeviceId(string deviceId)
        {
          return DBCommon.Instance.GetTModelValue<TickBoxInDevInfo>(string.Format("select t.line_id,t.station_id,t.device_id,t.position_in_dev,t.ticket_box_id,t.install_status, "
            + " drs.status_value as tick_store_status,t.update_date,t.update_time from tick_box_in_dev_info t "
            + " left join dev_run_status_detail drs on drs.device_id = t.device_id and drs.status_id = '1A'||decode(substr(t.device_id,5,2)||t.position_in_dev,'0607','05','0608','06',t.position_in_dev) where t.device_id='{0}' ", deviceId));
        }

        /// <summary>
        /// 得到Hopper与设备关系信息
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <returns>成功返回Hopper状态信息，否则返回null</returns>
        public List<DevRunStatusDetail> GetHopperStatusByDeviceId(string deviceId)
        {
            return DBCommon.Instance.GetTModelValue<DevRunStatusDetail>(string.Format("select * from dev_run_status_detail t where t.device_id='{0}' and "
            + " (t.status_id='1A0A' or t.status_id = '1A09') ", deviceId));
        }

        /// <summary>
        /// 得到票箱ID
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="positionInDev">位置编码</param>
        /// <returns>返回设备中的票箱ID</returns>
        public string GetDevTickBoxId(string deviceId, string positionInDev)
        {
            string cmd = string.Format("select t.tick_box_id from tick_box_in_dev_info t where t.device_id='{0}' and t.position_in_dev='{1}'",
                deviceId, positionInDev);

            DataTable dt=  DBCommon.Instance.GetDatatable(cmd);

            if (dt != null &&
                dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "00000000";
        }

        /// <summary>
        /// 根据线路得到所有的车站信息
        /// </summary>
        /// <returns>返回车站信息集合</returns>
        public List<BasiStationInfo> GetAllStationInfo(string lineId)
        {
            return DBCommon.Instance.GetTModelValue<BasiStationInfo>(string.Format("select * from basi_station_info where line_id='{0}' and location_type ='2'", lineId));
        }

        public List<BasiStationInfo> GetAllStationAndLCInfo(string lineId)
        {
            return DBCommon.Instance.GetTModelValue<BasiStationInfo>(string.Format("select * from basi_station_info where line_id='{0}' and location_type <>3 order by station_id", lineId));
        }

        /// <summary>
        /// 根据线路得到所有的车站信息(除本站以外的车站）
        /// </summary>
        /// <returns>返回车站信息集合</returns>
        public List<BasiStationInfo> GetAllStationInfo(string lineId,string stationId)
        {
            return DBCommon.Instance.GetTModelValue<BasiStationInfo>(string.Format("select * from basi_station_info where line_id='{0}' and location_type in ('1','2') and station_id <>'{1}' order by station_id", lineId, stationId));
        }
        /// <summary>
        /// 根据车站ID得到车站信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回车站信息</returns>
        public BasiStationInfo GetStationInfoById(string stationId)
        {
            return DBCommon.Instance.GetModelValue<BasiStationInfo>(string.Format("select * from basi_station_info t where t.station_id='{0}'", stationId));
        }

        /// <summary>
        /// 根据车站ID得到车站信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回车站信息</returns>
        public BasiStationInfo GetStationInfoByDevId(string devId)
        {
            return DBCommon.Instance.GetModelValue<BasiStationInfo>(string.Format("select * from basi_station_info t where t.device_id='{0}'", devId));
        }

        /// <summary>
        /// 根据状态ID，状态值得到状态信息
        /// </summary>
        /// <param name="statusId">状态ID</param>
        /// <param name="statusValue">状态值</param>
        /// <returns>返回基本的状态信息</returns>
        public BasiStatusIdInfo GetBasiStatusIdInfo(string statusId, string statusValue)
        {
            string cmd = string.Format("select * from basi_status_id_info t where t.status_id='{0}' and t.status_value='{1}'", statusId, statusValue);

            return DBCommon.Instance.GetModelValue<BasiStatusIdInfo>(cmd);
        }

        /// <summary>
        /// 根据车站得到车站信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回车站信息</returns>
        public BasiStationInfo GetStationInfoByName(string stationName)
        {
            return DBCommon.Instance.GetModelValue<BasiStationInfo>(string.Format("select t.* from basi_station_info t where t.station_cn_name like '{0}'", stationName));
        }

        /// <summary>
        /// 根据设备类型名称得到设备类型信息
        /// </summary>
        /// <param name="stationId">设备类型</param>
        /// <returns>设备类型信息</returns>
        public BasiDevTypeInfo GetDevTypeInfoByName(string devtypeName)
        {
            return DBCommon.Instance.GetModelValue<BasiDevTypeInfo>(string.Format("select t.* from basi_dev_type_info t where t.device_name  like '{0}'", devtypeName));
        }

        /// <summary>
        /// 根据设备部件名称得到设备部件ID
        /// </summary>
        /// <param name="devpartName">设备部件名称</param>
        /// <returns>设备部件ID</returns>
        public BasiDevPartIdInfo GetBasiDevPartIdInfoByName(string devpartName)
        {
            return DBCommon.Instance.GetModelValue<BasiDevPartIdInfo>(string.Format("select * from basi_dev_part_id_info t where t.dev_part_cn_name  = '{0}'", devpartName));
        }

        /// <summary>
        /// 根据设备类型ID得到设备类型信息
        /// </summary>
        /// <param name="devTypeId">设备类型ID</param>
        /// <returns>返回该设备类型的基本信息</returns>
        public BasiDevTypeInfo GetDevTypeInfoById(string devTypeId)
        {
            return DBCommon.Instance.GetModelValue<BasiDevTypeInfo>
                (string.Format("select t.device_name,t.device_type from basi_dev_type_info t where t.device_type='{0}'", devTypeId));
        }

        /// <summary>
        /// 根据设备ID得到设备资源信息
        /// </summary>
        /// <param name="devTypeId">设备ID</param>
        /// <returns>返回该设备的即时资源信息</returns>
        public DevResUseInfo GetDevResUseInfo(string devId)
        {
            return DBCommon.Instance.GetModelValue<DevResUseInfo>
                (string.Format("select t.* from dev_res_use_info t where t.device_id='{0}'", devId));
        }

        /// <summary>
        /// 根据线路ID到车站信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回车站信息</returns>
        public BasiLineIdInfo GetLineInfoById(string lineID)
        {
            return DBCommon.Instance.GetModelValue<BasiLineIdInfo>(string.Format("select t.* from basi_line_id_info t where t.line_id ='{0}'", lineID));
        }

        /// <summary>
        /// 根据线路到车站信息
        /// </summary>
        /// <param name="lineName">线路</param>
        /// <returns>返回车站信息</returns>
        public BasiLineIdInfo GetLineInfoByName(string lineName)
        {
            return DBCommon.Instance.GetModelValue<BasiLineIdInfo>(string.Format("select t.* from basi_line_id_info t where t.line_name ='{0}'", lineName));
        }


        /// <summary>
        /// 得到所有的设备类型
        /// </summary>
        /// <returns>返回所有的设备类型</returns>
        public List<BasiDevTypeInfo> GetAllDevciceType()
        {
            return DBCommon.Instance.GetTModelValue<BasiDevTypeInfo>("select * from basi_dev_type_info");
        }

        /// <summary>
        /// 得到所有的票卡的产品类型
        /// </summary>
        /// <returns>返回所有的票卡的产品类型</returns>
        public List<BasiProductTypeInfo> GetAllProducType()
        {
            return DBCommon.Instance.GetTModelValue<BasiProductTypeInfo>("select * from basi_product_type_info");
        }

        /// <summary>
        /// 得到票卡的卡发行商
        /// </summary>
        /// <returns>返回所有票卡的卡发行商</returns>
        public List<BasiProductTypeInfo> GetCardIssueId()
        {
            return DBCommon.Instance.GetTModelValue<BasiProductTypeInfo>("select distinct t.card_issue_id from basi_product_type_info t");
        }

        /// <summary>
        /// 得到票卡的物理类型
        /// </summary>
        /// <returns>返回所有票卡的物理类型</returns>
        public List<BasiProductTypeInfo> GetTicketPhyType()
        {
            return DBCommon.Instance.GetTModelValue<BasiProductTypeInfo>("select distinct t.ticket_phy_type,t.ticket_phy_type_name from basi_product_type_info t");
        }

        public List<BasiDevTypeInfo> GetSleDevTypeInfoItem()
        {
            return DBCommon.Instance.GetTModelValue<BasiDevTypeInfo>("select * from basi_dev_type_info  where device_type in('01','02','04','06')");
        }

        /// <summary>
        /// 得到该操作员能操作的所有设备类型
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <returns></returns>
        public List<BasiDevTypeInfo> GetOperatorDevType(string operatorId)
        {
            return DBCommon.Instance.GetTModelValue<BasiDevTypeInfo>(string.Format("select t.* from basi_dev_type_info t left join priv_operation_device_info tt on tt.dev_type=t.device_type where tt.operator_id='{0}'", operatorId));
        }

        /// <summary>
        /// 通过车站ID和设备类型得到设备信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="devType"></param>
        /// <returns></returns>
        public List<BasiDevInfo> GetBasiDevInfo(string stationId, string devType)
        {
            return DBCommon.Instance.GetTModelValue<BasiDevInfo>(string.Format("select * from basi_dev_info t where t.station_id like '{0}' and t.device_type like '{1}' order by t.device_id", stationId, devType));
        }
        public BasiDevInfo GetBasiDeviceIdInfo(string deviceId)
        {
            return DBCommon.Instance.GetModelValue<BasiDevInfo>(string.Format("select * from basi_dev_info t where t.device_id = '{0}'", deviceId));
        }
        public BasiDevInfo GetBasiDeviceIdInfos(string deviceId)
        {
            return DBCommon.Instance.GetModelValue<BasiDevInfo>(string.Format("select * from basi_dev_info t where t.device_id not in(select t.device from dev_ups-map t) t.station_id='{0}' and t.device_type='{1}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode, "06"));
        }
        /// <summary>
        /// 通过车站ID得到设备信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<BasiDevInfo> GetBasiDevInfo(string stationId)
        {
            List<BasiDevInfo> list = new List<BasiDevInfo>();
            string lineId = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            if (string.IsNullOrEmpty(stationId))
            {
                list= DBCommon.Instance.GetTModelValue<BasiDevInfo>(string.Format("select * from basi_dev_info t where t.line_id='{0}' order by t.station_id,t.device_id", lineId));
            }
            else
            {
                list= DBCommon.Instance.GetTModelValue<BasiDevInfo>(string.Format("select * from basi_dev_info t where t.line_id='{0}' and t.station_id like '{1}' order by t.device_id ", lineId, stationId));
            }
            if (list == null)
                return new List<BasiDevInfo>();
            return list;
        }


        /// <summary>
        /// 通过车站得到设备信息，只包括SLE设备
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回车站SLE的设备信息</returns>
        public List<BasiDevInfo> GetBasiDevInfoOnlySLEDevice(string stationId)
        {
            List<BasiDevInfo> list = GetBasiDevInfo(stationId);
            if (list != null)
            {
                var collection = from temp in list where 
                                           temp.device_type.Equals (DevType.DEV_TVM) ||
                                           temp.device_type.Equals(DevType.DEV_BOM) ||
                                           temp.device_type.Equals(DevType.DEV_AGM)
                                           orderby temp.device_id,temp.device_type
                                           select temp;
                return collection.ToList();
            }
            return null;
            
        }

        /// <summary>
        /// 返回设备类型信息
        /// </summary>
        /// <param name="devType">设备类型ID</param>
        /// <returns>返回基本的设备类型信息</returns>
        public BasiDevTypeInfo GetBasiDevTypInfo(string devType)
        {
            return DBCommon.Instance.GetModelValue<BasiDevTypeInfo>(string.Format("select * from basi_dev_type_info t where t.device_type='{0}' ", devType));
        }

        /// <summary>
        /// 获得所有线路信息
        /// </summary>
        /// <returns>返回所有线路信息</returns>
        public List<BasiLineIdInfo> GetAllLineInfos()
        {
            return DBCommon.Instance.GetTModelValue<BasiLineIdInfo>("select * from basi_line_id_info t");
        }

        /// <summary>
        /// 获得所有设备部件信息
        /// </summary>
        /// <returns>返回所有设备部件信息</returns>
        public List<BasiDevPartIdInfo> GetAllDevPartId()
        {
            return DBCommon.Instance.GetTModelValue<BasiDevPartIdInfo>("select * from basi_dev_part_id_info t");
        }

        /// <summary>
        /// 通过车站和站厅ID得到站厅信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="hallId"></param>
        /// <returns></returns>
        public BasiStationHallIdInfo GetBasiStationHallById(string stationId, string hallId)
        {
            return DBCommon.Instance.GetModelValue<BasiStationHallIdInfo>(string.Format("select t.* from basi_station_hall_id_info t  where t.station_id='{0}' and  t.station_hall_id ='{1}' order by t.station_id ,t.station_hall_id ", stationId, hallId));
        }

        /// <summary>
        /// 通过车站和站厅名称得到站厅信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="hallId"></param>
        /// <returns></returns>
        public BasiStationHallIdInfo GetBasiStationHallByName(string stationId, string hallName)
        {
            return DBCommon.Instance.GetModelValue<BasiStationHallIdInfo>(string.Format("select t.* from basi_station_hall_id_info t  where t.station_id='{0}' and  t.station_hall_name ='{1}' order by t.station_id ,t.station_hall_name ", stationId, hallName));
        }

        /// <summary>
        /// 根据车站返回所有设备的设备运行状态
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回该车站所有设备的运行状态信息</returns>
        public List<DevRunStatusInfo> GetAllDevRunStatusInfo(string stationId)
        {
            string cmd = string.Format("select * from dev_run_status_info t where t.station_id='{0}'", stationId);
            return DBCommon.Instance.GetTModelValue<DevRunStatusInfo>(cmd);
        }

        /// <summary>
        /// 通过车站得到站厅信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<BasiStationHallIdInfo>  GetBasiStationHall(string stationId)
        {

            return DBCommon.Instance.GetTModelValue<BasiStationHallIdInfo>(string.Format("select t.* from basi_station_hall_id_info t  where t.station_id='{0}'", stationId));
        }

        /// <summary>
        /// 通过车站得到组信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<BasiHallGroupIdInfo>  GetBasiHallGroup(string stationId)
        {
            return DBCommon.Instance.GetTModelValue<BasiHallGroupIdInfo>(string.Format("select t.* from basi_hall_group_id_info t where t.station_id='{0}' ", stationId));
        }

        /// <summary>
        /// 通过车站、站厅和组ID得到组信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="hallId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public BasiHallGroupIdInfo GetBasiHallGroupById(string stationId, string hallId, string groupId)
        {
             return DBCommon.Instance.GetModelValue<BasiHallGroupIdInfo>(string.Format("select t.* from basi_hall_group_id_info t where t.station_id='{0}' and  t.station_hall_id ='{1}'and t.hall_group_id='{2}'",stationId,hallId,groupId));
        }

        /// <summary>
        /// 通过车站、站厅和组名称得到组信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="hallId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public BasiHallGroupIdInfo GetBasiHallGroupByName(string stationId, string hallId, string groupName)
        {
            return DBCommon.Instance.GetModelValue<BasiHallGroupIdInfo>(string.Format("select t.* from basi_hall_group_id_info t where t.station_id='{0}' and  t.station_hall_id ='{1}'and t.hall_group_name='{2}'", stationId, hallId, groupName));
        }

        /// <summary>
        /// 通过票箱的ID得到票箱状态信息
        /// </summary>
        /// <param name="tickBoxId">票箱ID</param>
        /// <returns>返回票箱状态信息</returns>
        public TickBoxStatusInfo GetTickBoxStausInfo(string tickBoxId)
        {
            return DBCommon.Instance.GetModelValue<TickBoxStatusInfo>(string.Format("select * from tick_box_status_info t where t.ticket_box_id='{0}'", tickBoxId));
        }

        /// <summary>
        /// 得到所有的票箱状态信息
        /// </summary>
        /// <returns>返回票箱状态所有信息</returns>
        public List<TickBoxStatusInfo> GetTickBoxStatusInfos()
        {
            return DBCommon.Instance.GetTModelValue<TickBoxStatusInfo>(string.Format("select * from tick_box_status"));
        }

        /// <summary>
        /// 得到该设备的所有状态信息
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <returns>设备运行状态详细信息</returns>
        //public List<DevRunStatusDetail> GetAllDevRunStatusDetail(string deviceId)
        //{
        //    string cmd = string.Format("select * from dev_run_status_details t where t.device_id='{0}'", deviceId);
        //    return DBCommon.Instance.GetTModelValue<DevRunStatusDetail>(cmd);
        //}

        public List<BasiMoneyTypeInfo> GetAllMoneyTypeCodeInfo()
        {
            return DBCommon.Instance.GetTModelValue<BasiMoneyTypeInfo>("select t.* from basi_money_type_info t order by t.currency_code asc ");
        }



        public List<BasiMoneyTypeInfo> GetCoinMultMoneyTypeCodeInfo()
        {
            return DBCommon.Instance.GetTModelValue<BasiMoneyTypeInfo>("select t.* from basi_money_type_info t where t.currency_code in('00','11') ");
        }

        /// <summary>
        /// 得到所有现金的库存信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <param name="runDate">运营日</param>
        /// <returns>现金库存详细信息</returns>

        public List<CashStorageInfo> GetAllMoneyStoreInfo(string stationId, string runDate)
        {
            string ll = "select t.* from cash_storage_info t where t.station_id = '" + stationId + "' and t.update_date = '" + runDate + "'";
            return DBCommon.Instance.GetTModelValue<CashStorageInfo>(ll);
        }

        /// <summary>
        /// 得到所有票卡的库存信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <param name="runDate">运营日</param>
        /// <returns>现金库存详细信息</returns>

        public List<TickStorageInfo> GetAllTicketsStoreInfo(string stationId, string runDate)
        {
            return DBCommon.Instance.GetTModelValue<TickStorageInfo>("select t.line_id,t.station_id,t.tick_mana_type,t.in_store_num,t.total_num,t.yesterday_total_num,t.update_date,t.update_time,decode(t.ticket_status,'00','正常','01','废票') as ticket_status from tick_storage_info t where t.station_id = '" + stationId + "' and t.update_date =  '" + runDate + "'");
        }

        /// <summary>
        /// 根据库存管理类型得到票卡
        /// </summary>
        /// <param name="tickManType">库存管理类型</param>
        /// <param name="status">车票状态00：正常，01：异常</param>
        /// <returns>返回库存管理类型集合</returns>
        public TickStorageInfo GetTickStoreInfoByTickManType(string tickManType,params string[] status)
        {
            string tick_status = status.Count() > 0 ? status[0] : "00";
            return DBCommon.Instance.GetModelValue<TickStorageInfo>(string.Format("select * from tick_storage_info t where  t.ticket_status='{3}' and t.tick_mana_type='{0}' and t.line_id='{1}' and t.station_id='{2}'", tickManType
                ,SysConfig.GetSysConfig().LocalParamsConfig.LineCode,
                SysConfig.GetSysConfig().LocalParamsConfig.StationCode,tick_status)
                );
        }


        public TickStorageHistoryInfo GetTickStoreHistoryInfoByTickManType(string tickManType)
        {
            return DBCommon.Instance.GetModelValue<TickStorageHistoryInfo>(string.Format("select * from tick_storage_history_info t where  t.ticket_status='00' and t.tick_mana_type='{0}' and t.line_id='{1}' and t.station_id='{2}'", tickManType
                , SysConfig.GetSysConfig().LocalParamsConfig.LineCode,
                SysConfig.GetSysConfig().LocalParamsConfig.StationCode));
        }

        public List<TickStorageHistoryInfo> GetAllTickStoreInfo()
        {
            return DBCommon.Instance.GetTModelValue<TickStorageHistoryInfo>("select * from tick_storage_history_info t");
        }

        /// <summary>
        /// 返回所有的库存管理类型信息
        /// </summary>
        /// <returns>返回库存管理类型列表</returns>
        public List<TickStorageInfo> GetAllTickStoreHistoryInfo()
        {
            return DBCommon.Instance.GetTModelValue<TickStorageInfo>("select * from tick_storage_info t");
        }

        /// <summary>
        /// 返回所有的现金管理类型信息
        /// </summary>
        /// <returns>返回现金管理类型列表</returns>
        public List<BasiMoneyTypeInfo> GetBasiMoneyTypeInfo()
        {
            return DBCommon.Instance.GetTModelValue<BasiMoneyTypeInfo>("select * from basi_money_type_info t");
        }


        /// <summary>
        /// 返回所有的现金管理类型信息
        /// </summary>
        /// <returns>返回现金管理类型列表</returns>
        public CashStorageInfo GetCashStorageById(string moneyCode)
        {
            return DBCommon.Instance.GetModelValue<CashStorageInfo>(string.Format("select t.* from cash_storage_info t where t.currency_code='{0}' and t.line_id='{1}' and t.station_id='{2}'", moneyCode,SysConfig.GetSysConfig().LocalParamsConfig.LineCode,SysConfig.GetSysConfig().LocalParamsConfig.StationCode));
        }


        /// <summary>
        /// 返回所有的现金管理类型信息
        /// </summary>
        /// <returns>返回现金管理类型列表</returns>
        public CashWaitingToBankInfo GetCashWaitingById(string moneyCode)
        {
            return DBCommon.Instance.GetModelValue<CashWaitingToBankInfo>(string.Format("select t.* from cash_waiting_to_bank_info t where t.currency_code='{0}' and t.line_id='{1}' and t.station_id='{2}'", moneyCode, SysConfig.GetSysConfig().LocalParamsConfig.LineCode, SysConfig.GetSysConfig().LocalParamsConfig.StationCode));
        }



        /// <summary>
        /// 返回所有的钱币种类型信息
        /// </summary>
        /// <returns></returns>
        public List<BasiMoneyTypeInfo> GetBasiMoneyTypeCodeInfo()
        {
            return DBCommon.Instance.GetTModelValue<BasiMoneyTypeInfo>("select t.* from basi_money_type_info t");
        }

         ///<summary>
        /// 返回所有的库存类型
         ///</summary>
         ///<returns>库存管理类型集合</returns>
        public List<BasiTickManaTypeInfo> GetBasiTickManaTypeInfo(bool contrainsSelfDef)
        {
            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>("select * from basi_tick_mana_type_info");
        }


        //dusj modify begin 20121022 增加自定义票种
        public List<BasiTickManaTypeInfo> GetBasiTickManaTypeInfo()
        {
            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>("select t.tick_mana_type,t.tick_mana_type_name,t.card_issue_id,t.ticket_phy_type,t.ticket_phy_type_name,t.ticket_family_type,t.ticket_family_type_name from basi_tick_mana_type_info t union select  p.tick_mana_type,p.tick_mana_type_name,p.card_issue_id,p.ticket_phy_type,'' ticket_phy_type_name,''ticket_family_type,'' ticket_family_type_name from tick_valued_product_info p");
        }

        /// <summary>
        /// 返回所有自定义库存类型
        /// </summary>
        /// <returns>自定义库存类型</returns>
        public List<TickValuedProductInfo> GetTickValuedProductInfo()
        {
            return DBCommon.Instance.GetTModelValue<TickValuedProductInfo>("select * from tick_valued_product_info");
        }
        /// <summary>
        /// 根据库存管理类型返回基本的库存管理类型信息
        /// </summary>
        /// <param name="id">库存管理类型</param>
        /// <returns>基本的库存管理类型信息</returns>
        public BasiTickManaTypeInfo GetBasiTickManaTypeInfoById(string id)
        {
            return DBCommon.Instance.GetModelValue<BasiTickManaTypeInfo>(string.Format("select * from basi_tick_mana_type_info t where t.tick_mana_type='{0}'", id));
        }

        /// <summary>
        /// 根据库存管理类型返回基本的库存管理类型信息
        /// </summary>
        /// <param name="id">库存管理类型</param>
        /// <returns>基本的库存管理类型信息</returns>
        public BasiMoneyTypeInfo GetBasiMoneyTypeInfoById(string id)
        {
            return DBCommon.Instance.GetModelValue<BasiMoneyTypeInfo>(string.Format("select * from basi_money_type_info t where t.currency_code='{0}'", id));
        }


        /// <summary>
        /// 通过库存管理类型得到基本库存信息
        /// </summary>
        /// <param name="type">库存管理类型</param>
        /// <returns>库存信息</returns>
        public TickStorageInfo GetTickStorageInfoByTickManaType(string type,params string[] tickStatus)
        {
            string tick_status = tickStatus.Count() > 0 ? tickStatus[0] : "00";

            return DBCommon.Instance.GetModelValue<TickStorageInfo>(string.Format("select * from tick_storage_info t1 where t1.tick_mana_type='{0}' and t1.line_id='{1}' and t1.station_id='{2}' and t1.ticket_status='{3}'",
                type,
                SysConfig.GetSysConfig().LocalParamsConfig.LineCode,
            SysConfig.GetSysConfig().LocalParamsConfig.StationCode,tick_status));
        }

        public TickStorageHistoryInfo GetTickStorageHistoryInfoByTickManaType(string type)
        {
            return DBCommon.Instance.GetModelValue<TickStorageHistoryInfo>(string.Format("select * from tick_storage_history_info t1 where t1.tick_mana_type='{0}' and t1.line_id='{1}' and t1.station_id='{2}'",
                type,
                SysConfig.GetSysConfig().LocalParamsConfig.LineCode,
            SysConfig.GetSysConfig().LocalParamsConfig.StationCode));
        }

        /// <summary>
        /// 根据操作员ID得到在操作员手中
        /// 票卡的张数
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="tickManaType">库存管理类型</param>
        /// <returns>返回在操作员手中的实体信息</returns>
        public TickInOperatorInfo GetTickInOperatorInfoByOperatorId(string operatorId,string tickManaType)
        {
            if (string.IsNullOrEmpty(operatorId) || string.IsNullOrEmpty(tickManaType))
                return null;
            string cmd = string.Format("select * from tick_in_operator_info t2 where t2.operator_id='{0}' and t2.tick_mana_type='{1}'", operatorId, tickManaType);
            return DBCommon.Instance.GetModelValue<TickInOperatorInfo>(cmd);
        }

        /// <summary>
        /// 根据钱币代码得到钱币的库存信息
        /// </summary>
        /// <param name="code">钱币代码</param>
        /// <returns>币种的库存信息</returns>
        public CashStorageInfo GetCashStorageInfoByCashCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            string cmd = string.Format("select * from cash_storage_info cat where cat.currency_code='{0}' and cat.line_id='{1}' and cat.station_id='{2}'", code, SysConfig.GetSysConfig().LocalParamsConfig.LineCode, SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            return DBCommon.Instance.GetModelValue<CashStorageInfo>(cmd);
        }


        /// <summary>
        /// 根据钱币代码和操作人员代码得到钱币的库存信息
        /// </summary>
        /// <param name="code">钱币代码</param>
        /// <returns>币种的库存信息</returns>
        public CashInOperatorInfo GetCahInOperatorByKey(string operatorCode,string moneyCode)
        {
            if (string.IsNullOrEmpty(operatorCode))
                return null;
            if (string.IsNullOrEmpty(moneyCode))
                return null;
            string cmd = string.Format("select t.* from cash_in_operator_info t  where t.operator_id='{0}' and t.currency_code ='{1}'", operatorCode, moneyCode);
            return DBCommon.Instance.GetModelValue<CashInOperatorInfo>(cmd);
        }

        /// <summary>
        /// 获取车站站厅
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns></returns>
        public List<BasiStationHallIdInfo> GetBasiStationHallIdInfo(string stationId)
        {
            string cmd = string.Format("select t.* from basi_station_hall_id_info t  where t.station_id ='{0}' order by t.station_id ,t.station_hall_id", stationId);
            return DBCommon.Instance.GetTModelValue<BasiStationHallIdInfo>(cmd);
        }

        /// <summary>
        /// 得到车站站厅信息
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <param name="hallId">展厅ID</param>
        /// <returns>返回车站展厅信息</returns>
        public BasiStationHallIdInfo GetBasiStationHallIdInfo(string stationId, string hallId)
        {
            string cmd = string.Format("select t.* from basi_station_hall_id_info t where t.station_id ='{0}' and t.station_hall_id='{1}' order by t.station_id ,t.station_hall_id", stationId, hallId);
            return DBCommon.Instance.SetModelValue<BasiStationHallIdInfo>(cmd);
        }

        /// <summary>
        /// 获取车站组
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns></returns>
        public List<BasiHallGroupIdInfo> GetBasiHallGroupIdInfo(string stationId, string hallId)
        {
            string cmd = string.Format("select t.* from basi_hall_group_id_info t  where t.station_id ='{0}' and t.station_hall_id ='{1}' order by t.station_id ,t.station_hall_id", stationId, hallId);
            return DBCommon.Instance.GetTModelValue<BasiHallGroupIdInfo>(cmd);
        }

        /// <summary>
        /// 根据操作员编码和结算日期取得结算数据
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="settData"></param>
        /// <returns></returns>
        public List<DataDevSettlementInfo> GetDevSettlementInfo(string operatorId, string settData)
        {
            string cmd = string.Format("select t.* from data_dev_settlement_info t where t.operator_id='{0}' and t.settlement_date ='{1}' and t.settlement_status='01'", operatorId, settData);
            return DBCommon.Instance.GetTModelValue<DataDevSettlementInfo>(cmd);
        }


        public List<DataDevSettlementInfo> GetDevSettlementByData(string operatorId, string settData)
        {
            string cmd = string.Format("select t.line_id,t.station_id,t.device_id,t.operator_id,t.busi_udsn,t.occur_date_time,t.settlement_date,t.card_issuer_id,ba.tran_name as tran_type,t.tran_sub_type,t.tran_acount,t.deposit_amount,decode(t.tran_type,'06',-t.tran_amount,'07',-t.tran_amount,'EF',-t.tran_amount,'EE',-t.tran_amount,t.tran_amount)tran_amount,decode(t.tran_type,'06',-t.deposit_amount,'07',-t.deposit_amount,'EF',-t.deposit_amount,'EE',-t.deposit_amount,t.deposit_amount)deposit_amount,t.cost_amount,t.settlement_status,t.setilement_udsn,t.begin_settlement_time,t.end_settlement_time from data_dev_settlement_info t left join basi_tran_type_info ba on t.tran_type = ba.tran_type and t.tran_sub_type=ba.tran_sub_type  where t.operator_id='{0}' and t.settlement_date ='{1}' and t.settlement_status='01'", operatorId, settData);
            return DBCommon.Instance.GetTModelValue<DataDevSettlementInfo>(cmd);
        }

        /// <summary>
        /// 根据操作员取得手中的金额
        /// </summary>
        /// <param name="operatorCode"></param>
        /// <param name="moneyCode"></param>
        /// <returns></returns>
        public List<CashInOperatorInfo> GetCahInOperatorInfo(string operatorCode)
        {
            if (string.IsNullOrEmpty(operatorCode))
                return null;
            string cmd = string.Format("select t.* from cash_in_operator_info t  where t.operator_id='{0}'", operatorCode);
            return DBCommon.Instance.GetTModelValue<CashInOperatorInfo>(cmd);
        }

        public DataOperSettlementInfo GetDataOperSettlementInfo(string operatorId, string settData)
        {
            string cmd = string.Format("select t.* from data_oper_settlement_info t where t.operator_id='{0}' and t.run_date ='{1}'", operatorId, settData);
            return DBCommon.Instance.GetModelValue<DataOperSettlementInfo>(cmd);
        }

        /// <summary>
        /// 根据Params的ID得到Params的Value
        /// </summary>
        /// <param name="paramsId">参数ID</param>
        /// <returns>返回参数ID的值</returns>
        public BasiRunParamInfo GetBasiRunParamsValue(string paramsCode)
        {
            string cmd = string.Format("select * from basi_run_param_info t where t.param_code='{0}'", paramsCode);
            return DBCommon.Instance.GetModelValue<BasiRunParamInfo>(cmd);
        }

        /// <summary>
        /// 得到FTP的操作员
        /// </summary>
        /// <returns>FTP操作员称，否则返回null</returns>
        public string GetFTPUserName()
        {
            BasiRunParamInfo brpi = GetBasiRunParamsValue("000B");
            if (brpi != null && !string.IsNullOrEmpty(brpi.param_code))
                return brpi.param_value;
            else
                return null;
        }

        /// <summary>
        /// 返回FTP密码
        /// </summary>
        /// <returns>返回FTP密码，否则返回null</returns>
        public string GetFTPUserPwd()
        {
            BasiRunParamInfo brpi = GetBasiRunParamsValue("000C");
            if (brpi != null && !string.IsNullOrEmpty(brpi.param_code))
                return brpi.param_value;
            else
                return null;
        }

        /// <summary>
        /// 返回FTP地址
        /// </summary>
        /// <returns>返回FTP地址，否则返回null</returns>
        public string GetFTPAddress()
        {
            BasiRunParamInfo brpi = GetBasiRunParamsValue("000A");
            if (brpi != null && !string.IsNullOrEmpty(brpi.param_code))
                return brpi.param_value;
            else
                return null;
        }


        public string GetFtpSoftwarePath()
        {
            BasiRunParamInfo brpi = GetBasiRunParamsValue("0302");
            if (brpi != null && !string.IsNullOrEmpty(brpi.param_code))
                return brpi.param_value;
            else
                return null;
        }
        /// <summary>
        /// 得到车站的模式
        /// </summary>
        /// <returns></returns>
        public string GetCurrentMode()
        {
            try
            {
                string cmd = string.Format("select nvl(tm.mode_cn_name,'未知模式') mode_cn_name ,t.run_mode_code from run_mode_status t left join basi_run_mode_code_info tm on tm.run_mode_code =t.run_mode_code where t.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
                return DBCommon.Instance.GetDatatable(cmd).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
              return   "未知模式";
            }

        }

        /// <summary>
        /// 根据UPS id得到UPS信息
        /// </summary>
        /// <param name="upsId">upsId</param>
        /// <returns>返回UPS信息</returns>
        public DevUpsStatus GetDevUpsMap(string upsId)
        {
            return DBCommon.Instance.GetModelValue<DevUpsStatus>(string.Format("select t.* from dev_ups_status t where t.ups_id = '{0}'", upsId));
        }
        public string GetMaxUpsId()
        {
            //return "46";
            string cmd=string.Format("select max(t.ups_id) from dev_ups_status t");
            return DBCommon.Instance.GetDatatable(cmd).Rows[0][0].ToString();
        }
        public List<DevUpsMap> GetDevUpsMaps(string upsId)
        {
            return DBCommon.Instance.SetTModelValue<DevUpsMap>(string.Format("select t.* from dev_ups_map t where t.ups_id = '{0}'", upsId));
        }

        public DevUpsStatus GetDevUpsStatus(string deviceID)
        {
            return DBCommon.Instance.GetModelValue<DevUpsStatus>(string.Format("select t.* from dev_ups_status t where t.device_id='{0}'", deviceID));
        }

        public DevUpsStatus GetDevUpsStatusdev(string upsID)
        {
            return DBCommon.Instance.GetModelValue<DevUpsStatus>(string.Format("select t.* from dev_ups_status t where t.ups_id='{0}'", upsID));
        }


        /// <summary>
        /// 得到某个车站的具体的模式代码
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回车站的模式名称</returns>
        public List<RunModeStatus> GetStationRunModeInfos()
        {
            string cmd = string.Format("select * from run_mode_status where t.line_id='{0}'",
                SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
            return DBCommon.Instance.SetTModelValue<RunModeStatus>(cmd);
        }

        /// <summary>
        /// 取得供应商基本信息
        /// </summary>
        /// <returns></returns>
        public List<BasiProviderInfo> GetBasiProviderInfo()
        {
            return DBCommon.Instance.SetTModelValue<BasiProviderInfo>("select * from basi_provider_info");
        }

        /// <summary>
        /// 取得部件类型的基本信息
        /// </summary>
        /// <returns></returns>
        public List<BasiDevPartIdInfo> GetBasiDevTypeInfo()
        {
            return DBCommon.Instance.SetTModelValue<BasiDevPartIdInfo>("select * from basi_dev_part_id_info");
        }

        /// <summary>
        /// 取得设备类型类型的基本信息通过设备类型名称
        /// </summary>
        /// <returns></returns>
        public BasiDevTypeInfo GetBasiDevTypeInfoByName(string devTypeName)
        {
            return DBCommon.Instance.GetModelValue<BasiDevTypeInfo>(string.Format("select t.* from basi_dev_type_info t where t.device_name = '{0}'", devTypeName));        
        }


        /// <summary>
        /// 获得工区对应的所有车站信息
        /// </summary>
        /// <param name="maintainareaid">工区对应</param>
        /// <returns>返回该工区对应的所有车站信息</returns>

        public List<BasiStationInfo> GetMaintainAreaStationInfo(string maintainareaid)
        {
            List<BasiStationInfo> list = DBCommon.Instance.GetTModelValue<BasiStationInfo>(string.Format("select * from basi_station_info t right join (select masi.maintain_area_id, masi.station_id from maintain_area_station_info masi where masi.maintain_area_id = '{0}') t1 on t1.station_id = t.station_id where t.line_id = '{1}'", maintainareaid, SysConfig.GetSysConfig().LocalParamsConfig.LineCode));
            return list;
        }

        /// <summary>
        /// 得到所有的交易类型
        /// </summary>
        /// <returns>返回所有交易类型对象</returns>
        public List<BasiTranTypeInfo> GetAllBasiTranTypeInfos()
        {
            List<BasiTranTypeInfo> list = DBCommon.Instance.GetTModelValue<BasiTranTypeInfo>(
                string.Format("select distinct(t.afc_type),t.afc_name from basi_tran_type_info t where t.afc_type is not NULL order by t.afc_type"));
            return list;
        }

        /// <summary>
        /// 得到所有的操作员信息
        /// </summary>
        /// <returns>返回操作员信息列表</returns>
        public List<PrivOperatorInfo> GetAllOperatorInfo()
        {
            string cmd = string.Format("select * from priv_operator_info t where t.validity_status='00'");
            return DBCommon.Instance.GetTModelValue<PrivOperatorInfo>(cmd);
        }


        public List<BasiParaTypeInfo> GetAllParaTypeInfo()
        {
            string cmd = string.Format("select * from basi_para_type_info t order by t.para_type");
            return DBCommon.Instance.GetTModelValue<BasiParaTypeInfo>(cmd);
        }
        public List<ParaVersionInfo> GetAllParaVersionInfo(string paraType)
        {
            string cmd = string.Format("select t.* from para_version_info t where t.para_version<>'-1' and t.para_type='{0}' order by t.para_version", paraType);
            return DBCommon.Instance.GetTModelValue<ParaVersionInfo>(cmd);
        }

        public BasiDevInfo GetBasiDevInfoById(string deviceID)
        {
            return DBCommon.Instance.GetModelValue<BasiDevInfo>(string.Format("select t.* from basi_dev_info t where t.device_id='{0}'", deviceID));
        }


        public Para4314AutorunTime GetPara4314AutorunTime(string deviceID,string ctrlCode)
        {
            return DBCommon.Instance.GetModelValue<Para4314AutorunTime>(string.Format("select t.* from para_4314_autorun_time t where t.device_id='{0}'  and t.control_code='{1}'  and t.para_version='-1'", deviceID, ctrlCode));
        }

        public ParaLocalFullVerInfo GetCurrentVersionPara()
        {
            string inStr = "";
            if ("LCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
                inStr = "4311";
            if ("SCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
                inStr = "4310";

           return DBCommon.Instance.GetModelValue<ParaLocalFullVerInfo>(string.Format("select t.para_type,t.edition_type,t.para_version,t.para_sub_type,t.para_file_name,t.active_date,t.active_time from para_local_full_ver_info t where t.edition_type='0' and to_date(t.active_date,'yyyy-MM-dd') <= sysdate and t.para_type='{0}' ", inStr));          
        }

        public ParaLocalFullVerInfo GetFutureVersionPara()
        {
            string inStr = "";
            if ("LCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
                inStr = "4311";
            if ("SCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
                inStr = "4310";

            return DBCommon.Instance.GetModelValue<ParaLocalFullVerInfo>(string.Format("select t.para_type,t.edition_type,t.para_version,t.para_sub_type,t.para_file_name,t.active_date,t.active_time from para_local_full_ver_info t where t.edition_type = '1'and t.active_date || t.active_time =(select min(tt.active_date || tt.active_time) from para_local_full_ver_info tt where tt.edition_type = '1' and tt.para_type = '{0}' group by tt.para_type) and t.para_type = '{1}' ", inStr, inStr));          

        }
        /// <summary>
        /// 根据ID取得自定义库存类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public TickValuedProductInfo GetTickValuedByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            string cmd = string.Format("select * from tick_valued_product_info where tick_mana_type='{0}'", code);
            return DBCommon.Instance.GetModelValue<TickValuedProductInfo>(cmd);
        }
        /// <summary>
        /// 获取阀值
        /// </summary>
        /// <returns></returns>
        public BasiRunParamInfo GetRunParamByCode()
        {
            string cmd = string.Format("select * from basi_run_param_info t where t.param_code='0602'");
            return DBCommon.Instance.GetModelValue<BasiRunParamInfo>(cmd);
        }

       /// <summary>
       /// 取得自定义库存类型最大值
       /// </summary>
       /// <returns></returns>
        public string GetMaxTickValuedCode()
       {
           string code = "G0";
           string cmd = string.Format("select max(t.tick_mana_type) from tick_valued_product_info t");
           DataTable dt = DBCommon.Instance.GetDatatable(cmd);

           if (dt != null && dt.Rows.Count > 0)
           {
               if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
               {

                   code = dt.Rows[0][0].ToString();
                   char[] buffer = code.ToCharArray();
                   int res = Convert.ToInt16(buffer[1].ToString());
                   if (res < 9)
                   {
                       res = res + 1;
                       buffer[1] = Convert.ToChar(res.ToString());
                   }
                   else
                   {
                       buffer[1] = '0';
                       int ascCode = (int) buffer[0];
                       ascCode = ascCode + 1;
                       buffer[0] = (char) ascCode;
                   }
                   return new string(buffer);
               }
           }
           return code;
       }

        #endregion
    }
}
