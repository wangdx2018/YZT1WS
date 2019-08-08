using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls;
using AFC.WS.UI.Common;
using System.Data;
using System.Windows.Documents;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using System.Collections.ObjectModel;
using System.Collections;
using AFC.WS.Model.Const;
using AFC.WS.UI.Convertors;

namespace AFC.WS.BR.DeviceMonitor
{
    public class SLEGroupControlManager
    {
        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        private static SLEGroupControlManager _Instance;

        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        public static SLEGroupControlManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SLEGroupControlManager();
                }
                return _Instance;
            }
        }
        /// <summary>
        /// 设备控制命令信息反馈结果存储。
        /// </summary>
        private ObservableCollection<CommandResultInfo> cmdResult = new ObservableCollection<CommandResultInfo>();
        /// <summary>
        /// 控制命令结果信息结构体
        /// </summary>

        /// <summary>
        /// 根据Msg_no获得控制命令发送结果。
        /// </summary>
        private const string SQL_DevCtlCommandResult = "select * from (select t.* from dev_msg_send_log t order by t.msg_sn)a where rownum <=50 ";


        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="dg"></param>
        public IEnumerable GetDevInfo(string hall, string group1, string devType, string stationCode) 
        {
            System.Data.DataTable dt = null;
            try
            {
                List<BasiDevInfo> query = null;

                if (null != BuinessRule.GetInstace().GetBasiDevInfo(stationCode))
                {
                   if (!(string.IsNullOrEmpty(hall)) && string.IsNullOrEmpty(group1) && string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.station_hall_id == hall)
                                 && (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04" )
                                 select item).ToList();
                    }

                    if (!(string.IsNullOrEmpty(group1)) && string.IsNullOrEmpty(hall) && string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.hall_group_id == group1)
                                 && (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }

                    if (!(string.IsNullOrEmpty(devType)) && string.IsNullOrEmpty(hall) && string.IsNullOrEmpty(group1))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.device_type == devType)
                                 select item).ToList();
                    }
                    if (!(string.IsNullOrEmpty(hall)) && !string.IsNullOrEmpty(group1) && string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where  item.station_hall_id == hall
                                 && item.hall_group_id == group1 && (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }
                    if (!(string.IsNullOrEmpty(hall)) && string.IsNullOrEmpty(group1) && !string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where  (item.station_hall_id == hall) 
                                 && (item.device_type == devType)
                                 select item).ToList();
                    }
                    if (string.IsNullOrEmpty(hall) && !string.IsNullOrEmpty(group1) && !string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.hall_group_id == group1)
                                 && (item.device_type == devType)
                                 select item).ToList();
                    }
                    if (!(string.IsNullOrEmpty(hall)) && !string.IsNullOrEmpty(group1) && !string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.station_hall_id == hall)
                                 && (item.device_type == devType) && (item.hall_group_id == group1) 
                                 select item).ToList();
                    }
                    if (string.IsNullOrEmpty(hall) && string.IsNullOrEmpty(devType) && string.IsNullOrEmpty(group1))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }

                  

                    var query1 = from item in query
                                 select new
                                     {
                                         设备编码 = item.device_id,
                                         设备类型 = BuinessRule.GetInstace().GetBasiDevTypInfo(item.device_type).device_name,
                                         站厅编码 = item.station_hall_id,
                                         站厅名称 = BuinessRule.GetInstace().GetBasiStationHallById(item.station_id, item.station_hall_id).station_hall_name,
                                         车站组编码 = item.hall_group_id,
                                         车站组名称 = BuinessRule.GetInstace().GetBasiHallGroupById(item.station_id, item.station_hall_id, item.hall_group_id).hall_group_name,
                                         车站组序号 = item.group_serial_no,
                                         设备序号 = item.device_serial_no
                                     };
                   WriteLog.Log_Info("sql time=" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    return query1.ToList();
                }
                else
                {
                    var query1 = from list in new string[0]
                                 select new
                                {
                                    设备编码 = "",
                                    设备类型 = "",
                                    站厅编码 = "",
                                    站厅名称 = "",
                                    车站组编码 = "",
                                    车站组名称 = "",
                                    车站组序号 = "",
                                    设备序号 = "",
                                };
                    return query1.ToList();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("群组控制中异常:" + ex.ToString());
                return null;
            }
          
        }

           /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="dg"></param>
        public List<BasiDevInfo> GetPartBasiDevInfo(string hall, string group1, string devType, string stationCode)
        {
            try
            {
                List<BasiDevInfo> query = null;
                if (null != BuinessRule.GetInstace().GetBasiDevInfo(stationCode))
                {
                    if (!(string.IsNullOrEmpty(hall)) && string.IsNullOrEmpty(group1) && string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.station_hall_id == hall)
                                 && (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }

                    if (!(string.IsNullOrEmpty(group1)) && string.IsNullOrEmpty(hall) && string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.hall_group_id == group1)
                                 && (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }

                    if (!(string.IsNullOrEmpty(devType)) && string.IsNullOrEmpty(hall) && string.IsNullOrEmpty(group1))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.device_type == devType)
                                 select item).ToList();
                    }
                    if (!(string.IsNullOrEmpty(hall)) && !string.IsNullOrEmpty(group1) && string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where item.station_hall_id == hall
                                 && item.hall_group_id == group1 && (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }
                    if (!(string.IsNullOrEmpty(hall)) && string.IsNullOrEmpty(group1) && !string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.station_hall_id == hall)
                                 && (item.device_type == devType)
                                 select item).ToList();
                    }
                    if (string.IsNullOrEmpty(hall) && !string.IsNullOrEmpty(group1) && !string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.hall_group_id == group1)
                                 && (item.device_type == devType)
                                 select item).ToList();
                    }
                    if (!(string.IsNullOrEmpty(hall)) && !string.IsNullOrEmpty(group1) && !string.IsNullOrEmpty(devType))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.station_hall_id == hall)
                                 && (item.device_type == devType) && (item.hall_group_id == group1)
                                 select item).ToList();
                    }
                    if (string.IsNullOrEmpty(hall) && string.IsNullOrEmpty(devType) && string.IsNullOrEmpty(group1))
                    {
                        query = (from item in BuinessRule.GetInstace().GetBasiDevInfo(stationCode)
                                 where (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")
                                 select item).ToList();
                    }
                }
                return query;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("群组控制中异常:" + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 获取SC发送控制命令
        /// </summary>
        /// <param name="dg">显示所有控制命令</param>
        public ObservableCollection<CommandResultInfo> GetSendCommandResultInfo(IEnumerable drv,int seq, bool isEnable)
        {
            try
            {
                if (seq != 0)
                {

                    int ret = 0;
                    DataSet ds = Util.DataBase.SqlQuery(out ret, SQL_DevCtlCommandResult, seq);
                    DateTimeConvert dateConvert = new DateTimeConvert();
                    cmdResult.Clear();
                         
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        DataTable dt = ds.Tables[0];
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string 线路 = BuinessRule.GetInstace().GetLineInfoById(dt.Rows[i]["line_id"].ToString()).line_name;
                            string 车站 = BuinessRule.GetInstace().GetStationInfoById(dt.Rows[i]["station_id"].ToString()).station_cn_name;
                            string 设备名称 = BuinessRule.GetInstace().GetBasiDevTypInfo(dt.Rows[i]["dest_device_id"].ToString().Substring(0, 2)).device_name;
                            string 控制设备编码 = dt.Rows[i]["DEST_DEVICE_ID"].ToString();
                            string 消息发送结果 = GetSendResult(dt.Rows[i]["SEND_STATUS"].ToString());
                            string 发送日期 = dateConvert.Convert(dt.Rows[i]["SEND_DATE"].ToString(), null, null, null).ToString();
                            string 发送时间 = convertToTime(dt.Rows[i]["SEND_TIME"].ToString());
                            cmdResult.Add(new CommandResultInfo(isEnable, 线路, 车站, 设备名称, 控制设备编码, 消息发送结果, 发送日期, 发送时间));
                        }                       
                    }
                }
                if (seq == 0)
                {
                    cmdResult = new ObservableCollection<CommandResultInfo>();
                    foreach (CommandResultInfo result in drv)
                    {
                        cmdResult.Add(new CommandResultInfo(isEnable, result.线路, result.车站, result.设备名称, result.控制设备编码, result.消息发送结果, result.发送日期, result.发送时间));
                    }
                }
                return cmdResult;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
                return null;
            }
        }
        private string GetSendResult(string flag)
        {
            switch (flag)
            {
                case "00":
                    return "消息已经发送";

                case "01":
                    return "消息发送失败";

                case "02":
                    return "消息发送成功";

                default:
                    return "失败";
            }
            return "";
        }

        private string convertToTime(string time)
        {
            string result;
            switch (time.Length)
            {
                case 4:
                    result = time.Substring(0, 2) + ":" + time.Substring(2);
                    break;
                case 5:
                    result = time.Substring(0, 1) + ":" + time.Substring(1, 2) + ":" + time.Substring(3);
                    break;
                case 6:
                    result = time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4);
                    break;
                default:
                    result = time;
                    break;
            }
            return result;
        }

        public string GetComandEn(uint command)
        {
            switch (command)
            {
                case CtrolCommandCode.Close_degraded:
                    return CtrolCommandCode.Close_degraded_en;

                case CtrolCommandCode.Closed_gate:
                    return CtrolCommandCode.Closed_gate_en;

                case CtrolCommandCode.Empty_coin:
                    return CtrolCommandCode.Empty_coin_en;

                case CtrolCommandCode.Fare_adjustment_mode:
                    return CtrolCommandCode.Fare_adjustment_mode_en;

                case CtrolCommandCode.No_accept_coins:
                    return CtrolCommandCode.No_accept_coins_en;

                case CtrolCommandCode.No_accept_notes:
                    return CtrolCommandCode.No_accept_notes_en;

                case CtrolCommandCode.No_change_mode:
                    return CtrolCommandCode.No_change_mode_en;

                case CtrolCommandCode.No_coins_change:
                    return CtrolCommandCode.No_coins_change_en;

                case CtrolCommandCode.No_notes_change:
                    return CtrolCommandCode.No_notes_change_en;

                case CtrolCommandCode.No_print_mode:
                    return CtrolCommandCode.No_print_mode_en;

                case CtrolCommandCode.No_ticket_mode:
                    return CtrolCommandCode.No_ticket_mode_en;

                case CtrolCommandCode.Open_degraded:
                    return CtrolCommandCode.Open_degraded_en;

                case CtrolCommandCode.Open_gate:
                    return CtrolCommandCode.Open_gate_en;

                case CtrolCommandCode.Outbound:
                    return CtrolCommandCode.Outbound_en;

                case CtrolCommandCode.Password_authentication:
                    return CtrolCommandCode.Password_authentication_en;

                case CtrolCommandCode.Power_off:
                    return CtrolCommandCode.Power_off_en;

                case CtrolCommandCode.Replacement_ticket_mode:
                    return CtrolCommandCode.Replacement_ticket_mode_en;

                case CtrolCommandCode.Restart:
                    return CtrolCommandCode.Restart_en;

                case CtrolCommandCode.Return_normal_mode:
                    return CtrolCommandCode.Return_normal_mode_en;

                case CtrolCommandCode.Run_begin:
                    return CtrolCommandCode.Run_begin_en;

                case CtrolCommandCode.Run_end:
                    return CtrolCommandCode.Run_end_en;

                case CtrolCommandCode.Sleep_mode:
                    return CtrolCommandCode.Sleep_mode_en;

                case CtrolCommandCode.Start_service:
                    return CtrolCommandCode.Start_service_en;

                case CtrolCommandCode.Stop:
                    return CtrolCommandCode.Stop_en;

                case CtrolCommandCode.Suspended:
                    return CtrolCommandCode.Suspended_en;

                case CtrolCommandCode.Ticket_mode:
                    return CtrolCommandCode.Ticket_mode_en;

                case CtrolCommandCode.Two_way:
                    return CtrolCommandCode.Two_way_en;

                case CtrolCommandCode.Wake_up:
                    return CtrolCommandCode.Wake_up_en;
                default:
                    return "未知";
            }
        }

        //获得群组控制各个命令对应的业务代码（操作代码）
        public string GetComandsOperationCode(uint command)
        {
            switch (command)
            {
                case CtrolCommandCode.Close_degraded:
                    return OperationCode.Close_degraded_en;

                case CtrolCommandCode.Closed_gate:
                    return OperationCode.Closed_gate_en;

                case CtrolCommandCode.Empty_coin:
                    return OperationCode.Empty_coin_en;

                case CtrolCommandCode.Fare_adjustment_mode:
                    return OperationCode.Fare_adjustment_mode_en;

                case CtrolCommandCode.No_accept_coins:
                    return OperationCode.No_accept_coins_en;

                case CtrolCommandCode.No_accept_notes:
                    return OperationCode.No_accept_notes_en;

                case CtrolCommandCode.No_change_mode:
                    return OperationCode.No_change_mode_en;

                case CtrolCommandCode.No_coins_change:
                    return OperationCode.No_coins_change_en;

                case CtrolCommandCode.No_notes_change:
                    return OperationCode.No_notes_change_en;

                case CtrolCommandCode.No_print_mode:
                    return OperationCode.No_print_mode_en;

                case CtrolCommandCode.No_ticket_mode:
                    return OperationCode.No_ticket_mode_en;

                case CtrolCommandCode.Open_degraded:
                    return OperationCode.Open_degraded_en;

                case CtrolCommandCode.Open_gate:
                    return OperationCode.Open_gate_en;

                case CtrolCommandCode.Outbound:
                    return OperationCode.Outbound_en;

                case CtrolCommandCode.Password_authentication:
                    return OperationCode.Password_authentication_en;

                case CtrolCommandCode.Power_off:
                    return OperationCode.Power_off_en;

                case CtrolCommandCode.Replacement_ticket_mode:
                    return OperationCode.Replacement_ticket_mode_en;

                case CtrolCommandCode.Restart:
                    return OperationCode.Restart_en;

                case CtrolCommandCode.Return_normal_mode:
                    return OperationCode.Return_normal_mode_en;

                case CtrolCommandCode.Run_begin:
                    return OperationCode.Run_begin_en;

                case CtrolCommandCode.Run_end:
                    return OperationCode.Run_end_en;

                case CtrolCommandCode.Sleep_mode:
                    return OperationCode.Sleep_mode_en;

                case CtrolCommandCode.Start_service:
                    return OperationCode.Start_service_en;

                case CtrolCommandCode.Stop:
                    return OperationCode.Stop_en;

                case CtrolCommandCode.Suspended:
                    return OperationCode.Suspended_en;

                case CtrolCommandCode.Ticket_mode:
                    return OperationCode.Ticket_mode_en;

                case CtrolCommandCode.Two_way:
                    return OperationCode.Two_way_en;

                case CtrolCommandCode.Wake_up:
                    return OperationCode.Wake_up_en;
                default:
                    return "未知";
            }
        }


    }
    public class CommandResultInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lineName">线路</param>
        /// <param name="stationName">车站</param>
        /// <param name="devName">设备名称</param>
        /// <param name="devId">设备编码</param>
        /// <param name="interCode">内部交易代码</param>
        /// <param name="ctlCode">控制代码</param>
        /// <param name="cmdName">命令名称</param>
        /// <param name="sendResult">发送结果</param>
        /// <param name="sendDate">发送日期</param>
        /// <param name="sendTime">发送时间</param>
        public CommandResultInfo(bool isEnable, string lineName, string stationName, string devName,
            string devId, string sendResult, string sendDate, string sendTime
            )
        {
            IsEnable = isEnable;
            线路 = lineName;
            车站 = stationName;
            设备名称 = devName;
            控制设备编码 = devId;
            消息发送结果 = sendResult;
            发送日期 = sendDate;
            发送时间 = sendTime;
        }
        public bool IsEnable { get; set; }
        /// <summary>
        /// 线路属性
        /// </summary>
        public string 线路 { get; set; }
        /// <summary>
        /// 车站
        /// </summary>
        public string 车站 { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string 设备名称 { get; set; }
        /// <summary>
        /// 控制设备编码
        /// </summary>
        public string 控制设备编码 { get; set; }
        /// <summary>
        /// 内部交易代码
        /// </summary>
        public string 内部交易代码 { get; set; }
        /// <summary>
        /// 控制代码
        /// </summary>
        public string 控制代码 { get; set; }
        /// <summary>
        /// 命令名称
        /// </summary>
        public string 命令名称 { get; set; }
        /// <summary>
        /// 消息发送结果
        /// </summary>
        public string 消息发送结果 { get; set; }
        /// <summary>
        /// 发送日期
        /// </summary>
        public string 发送日期 { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string 发送时间 { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TeamModel
    {
        #region Properties
        /// <summary>
        /// 设置界面控制命令结果信息绑定
        /// </summary>
        public ObservableCollection<CommandResultInfo> CmdResults { get; set; }

        #endregion
    }
}
