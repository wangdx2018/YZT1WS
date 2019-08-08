using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.BR.ParamsManager;
using System.Data;
using System.Windows.Controls;
using System.Collections;
using AFC.WS.UI.BR.Data.PassengerFlow;
using System.Text.RegularExpressions;

/*
 * edit by wangdx  
 * 
 *  date:20111017 增加了按照票价统计的分时客流的底层支持。
 * 
 * */
namespace AFC.WS.BR.PassengerFlow
{
    /// <summary>
    /// 客流统计。
    /// </summary>
    public enum PassengerFlowCounterEnum
    {
        /// <summary>
        /// 无。
        /// </summary>
        None,
        /// <summary>
        /// 拆线图。
        /// </summary>
        Line,
        /// <summary>
        /// 进站饼图
        /// </summary>
        EntryPie,
        /// <summary>
        /// 出站饼图。
        /// </summary>
        ExitPie
    }
   public class PassengerFlowHelper
    {
        #region --> 事件和方法。
        /// <summary>
        /// 客流统计事件
        /// </summary>
        public static event PassengerFlowNumberEventHandler PassengerFlowNumber;
        /// <summary>
        /// 历史客流查询结果数据事件
        /// </summary>
        public static event HistoryPassengerFlowQueryResultEventHandler HistoryPassengerFlowQuery;
        /// <summary>
        /// 历史客流查询结果饼图数据事件
        /// </summary>
        public static event HistoryPassengerFlowPieEventHandler HistoryPassengerFlowPie;
        /// <summary>
        /// 历史客流查询结果饼图事件方法
        /// </summary>
        /// <param name="sender">存放当前类内容 this</param>
        /// <param name="e">历史客流查询结果饼图事件类</param>
        static void HistoryPassengerFlowPieMethod(object sender, HistoryPassengerFlowPieEventArgs e)
        {
            if (HistoryPassengerFlowPie != null)
            {
                HistoryPassengerFlowPie(sender, e);
            }
        }
        /// <summary>
        /// 客流统计事件方法
        /// </summary>
        /// <param name="sender">存放当前类内容 this</param>
        /// <param name="e">客流统计事件类</param>
        static void PassengerFlowNumberMethod(object sender, PassengerFlowNumberEventArgs e)
        {
            if (PassengerFlowNumber != null)
            {
                PassengerFlowNumber(sender, e);
            }
        }
        /// <summary>
        /// 历史客流查询结果数据事件方法
        /// </summary>
        /// <param name="sender">存放当前类内容 this</param>
        /// <param name="e">历史客流查询结果事件类</param>
        static void HistoryPassengerFlowQueryMethod(object sender, HistoryPassengerFlowQueryResultEventArgs e)
        {
            if (HistoryPassengerFlowQuery != null)
            {
                HistoryPassengerFlowQuery(sender, e);
            }
        }

        #endregion --> 事件和方法。

        static List<AFC.WS.UI.Common.PassFlowConfig> _PPManagerTypeInfoItem;
    
        /// <summary>
        /// 获取客流类型
        /// </summary>
        public static List<AFC.WS.UI.Common.PassFlowConfig> PPManagerTypeInfoItem
        {
            get
            {
                if (_PPManagerTypeInfoItem == null || _PPManagerTypeInfoItem.Count == 0)
                {
                    _PPManagerTypeInfoItem = SysConfig.GetSysConfig().PassengerFlowParamCfg.MonitorParamItems;
                }
                return _PPManagerTypeInfoItem;
            }
        }

        /// <summary>
        /// 客流管理类型
        /// </summary>
        public static List<PropertyValue> PassengerFlowTypeItem
        {
            get
            {
                List<PropertyValue> pvList = new List<PropertyValue>();
                if (PPManagerTypeInfoItem != null &&
                    PPManagerTypeInfoItem.Count > 0)
                {
                    foreach (var temp in PPManagerTypeInfoItem)
                    {
                        PropertyValue pv = new PropertyValue();

                        pv.Value = temp.Value;
                        pv.Key = temp.Key;
                        pvList.Add(pv);
                    }
                }
                return pvList;
            }
        }

        #region --> 针对 TreeViewItem 操作。

        /// <summary>
        /// 设置车站
        /// </summary>
        /// <param name="parent">TreeViewItem控件</param>
        /// <param name="lineId">线路ID</param>
        public static void SetStationInfo(TreeViewItem parent, string lineId)
        {
            if (parent == null)
            {
                return;
            }
            try
            {
                List<BasiStationInfo> obj = BuinessRule.GetInstace().GetAllStationInfo(lineId);// PassengerFlowHelper.GetBasiStationInfo(lineId);
                foreach (BasiStationInfo var in obj)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = var.station_cn_name;
                    item.Uid = var.station_id;
                    item.Tag = var;
                    SetHallInfo(item, var.station_id);
                    parent.Items.Add(item);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 设备站厅
        /// </summary>
        /// <param name="parent">TreeViewItem控件</param>
        /// <param name="stationId">车站ID</param>
        public static void SetHallInfo(TreeViewItem parent, string stationId)
        {
            if (parent == null)
            {
                return;
            }
            try
            {
                List<BasiStationHallIdInfo> obj = BuinessRule.GetInstace().GetBasiStationHallIdInfo(stationId);
                foreach (BasiStationHallIdInfo var in obj)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = var.station_hall_name;
                    item.Tag = var;
                    item.Uid = var.station_hall_id;
                    SetGroupHallInfo(item, stationId, var.station_hall_id);
                    parent.Items.Add(item);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 设置站厅组
        /// </summary>
        /// <param name="parent">TreeViewItem控件</param>
        /// <param name="stationId">车站ID</param>
        /// <param name="hallId">站厅ID</param>
        public static void SetGroupHallInfo(TreeViewItem parent, string stationId, string hallId)
        {
            try
            {
                List<BasiHallGroupIdInfo> obj = BuinessRule.GetInstace().GetBasiHallGroupIdInfo(stationId, hallId);
                foreach (BasiHallGroupIdInfo var in obj)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = var.hall_group_name;
                    item.Uid = var.hall_group_id;
                    item.Tag = var;
                    parent.Items.Add(item);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 获取当前选择时客流监视信息。
        /// </summary>
        /// <param name="item">TreeViewItem控件</param>
        /// <returns>返回当前选择客流监视信息字符串</returns>
        public static string GetCurrentMonitorInfo(TreeViewItem item)
        {
            string content = "";

            if (item != null && item.Parent != null)
            {
                TreeViewItem obj = item.Parent as TreeViewItem;

                if (obj != null)
                {
                    string str = GetCurrentMonitorInfo(obj);
                    if (!String.IsNullOrEmpty(str))
                    {
                        content += str + "-->";
                    }
                }
                content += item.Header;
            }
            return content;
        }

        /// <summary>
        /// 设置选择内容。
        /// </summary>
        /// <param name="item">TreeViewItem控件</param>
        public static void SetTreeViewItemIsExpanded(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }
            item.IsExpanded = false;
            if (item.Items.Count > 0)
            {
                foreach (TreeViewItem obj in item.Items)
                {
                    SetTreeViewItemIsExpanded(obj);
                }
            }
        }
 

        /// <summary>
        /// 获取监视条件。
        /// </summary>
        /// <param name="item">TreeViewItem控件</param>
        public static void GetMonitorCondition(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }
            object obj = item.Tag;
            if (obj != null)
            {
                if (obj is BasiStationInfo)
                {
                    StationID = item.Uid;
                }
                else if (obj is BasiStationHallIdInfo)
                {
                    StationHallID = item.Uid;
                }
                else if (obj is BasiHallGroupIdInfo)
                {
                    HallGroupID = item.Uid;
                }

                TreeViewItem var = item.Parent as TreeViewItem;
                if (var != null)
                {
                    GetMonitorCondition(var);
                }
            }
        }

        /// <summary>
        /// 清空监视条件信息。
        /// </summary>
        public static void ClearMonitorCondition()
        {
            StationID = "";
            StationHallID = "";
            StationAreaID = "";
            HallGroupID = "";
        }

        #endregion --> 针对 TreeViewItem 操作。

        #region --> 客流。

        #region --> 属性。

        /// <summary>
        /// 客流类型:分三段，第段三列。
        /// </summary>
        static PassengerFlowTypeMonitorInfo pftm = new PassengerFlowTypeMonitorInfo();

        /// <summary>
        /// 线路ID
        /// </summary>
        public static string LineID;

        /// <summary>
        /// 车站ID
        /// </summary>
        public static string StationID;

        /// <summary>
        /// 站区ID
        /// </summary>
        public static string StationAreaID;

        /// <summary>
        /// 站厅ID
        /// </summary>
        public static string StationHallID;

        /// <summary>
        /// 站厅组ID
        /// </summary>
        public static string HallGroupID;

        /// <summary>
        /// 设备类型:TVM、BOM、AG、EQM。
        /// </summary>
        public static string DeviceName;

        /// <summary>
        /// 设备类型代码
        /// </summary>
        public static string DeviceType;

        /// <summary>
        /// 页面点的个数。默认为50个点。
        /// </summary>
        public static int PagePoint = 50;

        /// <summary>
        /// 页面索引值，当前第几页。
        /// </summary>
        public static int PageCurrentIndex = 1;

        /// <summary>
        /// 页面最大值。
        /// </summary>
        public static int PageCount = 3;

        /// <summary>
        /// 时间间隔 默认为 10分钟
        /// </summary>
        public static int TimeInterval = 10;

        /// <summary>
        /// 列统计记录数
        /// </summary>
        static int columnCounter = 0;

        /// <summary>
        /// 行统计记录数
        /// </summary>
        static int rowCounter = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        static string StartTime = "";

        /// <summary>
        /// 结束时间
        /// </summary>
        static string EndTime = "";

        #endregion --> 属性。

        /// <summary>
        /// 客流类型集合信息
        /// </summary>
        public static List<PassengerFlowTypeMonitorInfo> PassengerFlowMonitorItem = new List<PassengerFlowTypeMonitorInfo>();

        /// <summary>
        /// 获取客流监视信息：如当前一卡通进站客流1000等。
        /// </summary>
        /// <param name="pc">客流参数</param>
        /// <param name="item">客流监视信息集合</param>
        /// <returns>返回客流监视信息集合</returns>
        /// 取得各种票卡客流的当前数量和总共数量
        static List<PassengerMonitor> SetPassengerFlowTypeMonitorInfo(PassFlowConfig pc,
            List<PassengerMonitor> item)
        {
            List<PassengerMonitor> pmList = new List<PassengerMonitor>();
            decimal currentTotal = 0;
            decimal total = 0;
            if (item != null)
            {
                foreach (PassengerMonitor pm in item)
                {
                    if (pc.Key == (pm.Afc_type + pm.Issuer_id.ToHexNumberUShort().ConvertNumberToHexString()))
                    {
                        pmList.Add(pm);
                        total += pm.Total;

                        if (Convert.ToInt32(StartTime) > Convert.ToInt32(EndTime))
                        {
                            int allTime = 2359;
                            int time = 0;
                            if (Convert.ToInt32(StartTime) <= Convert.ToInt32(pm.Tran_time_min) &&
                                 Convert.ToInt32(pm.Tran_time_min) < allTime)
                            {
                                currentTotal += pm.Total;
                            }
                            if (Convert.ToInt32(time) <= Convert.ToInt32(pm.Tran_time_min) &&
                              Convert.ToInt32(pm.Tran_time_min) < Convert.ToInt32(EndTime))
                            {
                                currentTotal += pm.Total;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(StartTime) <= Convert.ToInt32(pm.Tran_time_min) &&
                                Convert.ToInt32(pm.Tran_time_min) < Convert.ToInt32(EndTime))
                            {
                                currentTotal += pm.Total;
                            }
                        }
                    }
                }
            }
            if (pc == null)
            {
                return pmList;
            }
            string card_issue_name = string.Empty;
            if (pc.CardIssueId == "1")
            {
                card_issue_name = "一票通";
            }
            else
            {
                card_issue_name = "一卡通";
            }
    
            if (columnCounter == 0)
            {
                pftm.CardIssueName = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber = currentTotal.ToString();
                pftm.PassengerFlowTotal = total.ToString();
                pftm.PassengerFlowTypeName = pc.Value;// card.card_issue_name + pp.pass_type_name;
                columnCounter++;
            }
            else if (columnCounter == 1)
            {
                pftm.CardIssueName1 = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber1 = currentTotal.ToString();
                pftm.PassengerFlowTotal1 = total.ToString();
                pftm.PassengerFlowTypeName1 = pc.Value;//  card.card_issue_name + pp.pass_type_name;
                columnCounter++;
            }
            else if (columnCounter == 2)
            {
                pftm.CardIssueName2 = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber2 = currentTotal.ToString();
                pftm.PassengerFlowTotal2 = total.ToString();
                pftm.PassengerFlowTypeName2 = pc.Value;//  card.card_issue_name + pp.pass_type_name;
                PassengerFlowMonitorItem.Add(pftm);
                pftm = new PassengerFlowTypeMonitorInfo();
                columnCounter = 0;
            }
            return pmList;
        }



        /// <summary>
        /// 获取客流监视信息：如当售票5元票客流1000等。
        /// </summary>
        /// <param name="pc">客流参数</param>
        /// <param name="item">客流监视信息集合</param>
        /// <returns>返回客流监视信息集合</returns>
        /// 取得各种票价客流的当前数量和总共数量
        static List<PassengerMonitor> SetPassengerFareTypeMonitorInfo(PassFlowConfig pc,
            List<PassengerMonitor> item)
        {
            List<PassengerMonitor> pmList = new List<PassengerMonitor>();
            decimal currentTotal = 0;
            decimal total = 0;
            if (item != null)
            {
                foreach (PassengerMonitor pm in item)
                {
                    if (pc.Key == (pm.Afc_type + pm.Tran_value.ToString()))
                    {
                        pmList.Add(pm);
                        total += pm.Total;

                        if (Convert.ToInt32(StartTime) > Convert.ToInt32(EndTime))
                        {
                            int allTime = 2359;
                            int time = 0;
                            if (Convert.ToInt32(StartTime) <= Convert.ToInt32(pm.Tran_time_min) &&
                                 Convert.ToInt32(pm.Tran_time_min) < allTime)
                            {
                                currentTotal += pm.Total;
                            }
                            if (Convert.ToInt32(time) <= Convert.ToInt32(pm.Tran_time_min) &&
                              Convert.ToInt32(pm.Tran_time_min) < Convert.ToInt32(EndTime))
                            {
                                currentTotal += pm.Total;
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(StartTime) <= Convert.ToInt32(pm.Tran_time_min) &&
                                Convert.ToInt32(pm.Tran_time_min) < Convert.ToInt32(EndTime))
                            {
                                currentTotal += pm.Total;
                            }
                        }
                    }
                }
            }
            if (pc == null)
            {
                return pmList;
            }
            string card_issue_name = string.Empty;
            if (pc.CardIssueId == "1")
            {
                card_issue_name = "一票通";
            }
            else
            {
                card_issue_name = "一卡通";
            }

            if (columnCounter == 0)
            {
                pftm.CardIssueName = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber = currentTotal.ToString();
                pftm.PassengerFlowTotal = total.ToString();
                pftm.PassengerFlowTypeName = pc.Value;// pass_type_name + tran_value
                columnCounter++;
            }
            else if (columnCounter == 1)
            {
                pftm.CardIssueName1 = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber1 = currentTotal.ToString();
                pftm.PassengerFlowTotal1 = total.ToString();
                pftm.PassengerFlowTypeName1 = pc.Value;//  pass_type_name + tran_value
                columnCounter++;
            }
            else if (columnCounter == 2)
            {
                pftm.CardIssueName2 = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber2 = currentTotal.ToString();
                pftm.PassengerFlowTotal2 = total.ToString();
                pftm.PassengerFlowTypeName2 = pc.Value;//  pass_type_name + tran_value
                PassengerFlowMonitorItem.Add(pftm);
                pftm = new PassengerFlowTypeMonitorInfo();
                columnCounter = 0;
            }
            return pmList;
        }

        /// <summary>
        /// 客流类型
        /// </summary>
        /// <returns>返回客流类型字符串</returns>
        static string GetBissTypeCode(params int[] a)
        {
            StringBuilder sb = new StringBuilder();
            List<PassFlowConfig> pcItem = null;
            if (a.Length == 0)
            {
               pcItem = GetPassengerMonitorConfig();
            }
            else
            {
                 pcItem = GetPassengerMonitorConfig(1);
            }
            if (pcItem != null)
            {
                foreach (PassFlowConfig pc in pcItem)
                {
                    if (!sb.ToString().Contains(pc.Key))
                    {
                        sb.Append("'").Append(pc.Key).Append("'").Append(",");
                    }
                }
            }
            string context = sb.ToString().Substring(0, sb.ToString().Length - 1);
            return context;
        }

        /// <summary>
        /// 客流参数集合
        /// </summary>
        /// <returns>返回客流参数集合</returns>
        public static List<PassFlowConfig> GetPassengerMonitorConfig(params int[] a)
        {
            if (a.Length == 0)
            {
                try
                {
                    return SysConfig.GetSysConfig().PassengerFlowParamCfg.MonitorParamItems.Where(p => p.IsMonitor == true).GetListTContext<PassFlowConfig>();
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    return null;
                }
            }
            else
            {
                return GetPassangerFlowCfgByParam();
                ////todo:
                //return SysConfig.GetSysConfig().PassengerFlowParamCfg.MonitorParamItems.Where(p => p.IsMonitor == true).GetListTContext<PassFlowConfig>();
            }
        }


       /// <summary>
       /// added by wangdx 20111017 
       /// 通过获得当前的自动运行参数来获得客流配置信息
       /// //todo:001:找到当前的所用到的票价表参数
       /// 002:
       /// </summary>
       /// <returns>得到客流配置信息</returns>
        private static List<PassFlowConfig> GetPassangerFlowCfgByParam()
        {
            string cmd = "select distinct fare.fare_value" +
   " from para_4036_fare_value fare" +
  " where fare.para_version in" +
        "((select t.para_version" +
         "   from para_dev_full_ver_info t" +
         "  where t.para_type = '4036'" +
         "    and t.edition_type = '0'" +
         "    and t.device_id in" +
         "        (select sti.device_id from basi_station_info sti)" +
         " )) order by fare.fare_value";

            DataTable dt=DBCommon.Instance.GetDatatable(cmd);
            List<double> list=new List<double>();
           for(int i=0;i<dt.Rows.Count;i++)
           {
               list.Add(Convert.ToDouble(dt.Rows[i][0].ToString()));
           }

            List<PassFlowConfig> pfList = new List<PassFlowConfig>();

           for(int i=0;i<list.Count;i++)
           {
               pfList.Add(new PassFlowConfig { CardIssueId = "01", IsMonitor = true, Key = "01" + list[i], Value = string.Format("售{0}元票", list[i] / 100) });
               pfList.Add(new PassFlowConfig { CardIssueId = "04", IsMonitor = true, Key = "04" + list[i], Value = string.Format("出站{0}元", list[i] / 100) });
           }
           return pfList;
        }



        /// <summary>
        /// 获取时时客流SQL语句。
        /// </summary>
        /// <param name="bissTypeCode">客流类型</param>
        /// <returns>返回时时客流SQL语句</returns>
        static string GetSqlSentence(string afcType)
        {

            string sqlString = string.Format("select tt.issuer_id, tt.afc_type,bdi.device_type,tt.tran_time_min,sum(tt.passenger_num) as total  ");
            sqlString += string.Format(" from data_pass_flow_info tt ");
            sqlString += string.Format(" left join basi_dev_info bdi on bdi.device_id = tt.device_id ");
            sqlString += string.Format(" left join basi_station_info bsi on bsi.station_id = bdi.station_id  and bsi.line_id = bdi.line_id ");
            sqlString += string.Format(" left join basi_dev_type_info bdti on bdti.device_type = bdi.device_type ");

            sqlString += string.Format(" where tt.tran_date = '{0}'", DateTime.Now.ToString("yyyyMMdd"));
            sqlString += string.Format(" and tt.tran_time_min <= '{0}' ",DateTime.Now.ToString("HHmm"));
            if (!String.IsNullOrEmpty(StationAreaID))
            {
                sqlString += string.Format(" and bdi.station_area_id = '{0}'", StationAreaID);
            }
            if (!String.IsNullOrEmpty(StationID))
            {
                sqlString += string.Format(" and bdi.station_id = '{0}'", StationID);
            }
            if (!String.IsNullOrEmpty(StationHallID))
            {
                sqlString += string.Format(" and bdi.station_hall_id = '{0}'", StationHallID);
            }
            if (!String.IsNullOrEmpty(HallGroupID))
            {
                sqlString += string.Format(" and bdi.hall_group_id = '{0}'", HallGroupID);
            }
            if (!String.IsNullOrEmpty(DeviceType))
            {
                sqlString += string.Format(" and bdi.device_type = '{0}'", DeviceType.ToLower());
            }
            if (!String.IsNullOrEmpty(afcType))
            {
                sqlString += string.Format("  and concat(tt.afc_type,lpad(tt.issuer_id,2,'0'))  in ({0})", afcType);
               
            }
            sqlString += string.Format(" and to_number(tt.issuer_id) in ('99', '1')");
            sqlString += string.Format(" group by tt.issuer_id,tt.afc_type,bdi.device_type,tt.tran_time_min");

            return sqlString;
        }

        /// <summary>
        /// 获取票价时时客流SQL语句。
        /// </summary>
        /// <param name="bissTypeCode">客流类型</param>
        /// <returns>返回时时客流SQL语句</returns>
        static string GetSqlFare(string afcType)
        {

            string sqlString = string.Format("select  tt.afc_type,bdi.device_type,tt.tran_time_min,tt.tran_value,sum(tt.passenger_num) as total  ");
            sqlString += string.Format(" from data_pass_flow_info tt ");
            sqlString += string.Format(" left join basi_dev_info bdi on bdi.device_id = tt.device_id ");
            sqlString += string.Format(" left join basi_station_info bsi on bsi.station_id = bdi.station_id  and bsi.line_id = bdi.line_id ");
            sqlString += string.Format(" left join basi_dev_type_info bdti on bdti.device_type = bdi.device_type ");

            sqlString += string.Format(" where tt.tran_date = '{0}'", DateTime.Now.ToString("yyyyMMdd"));
            sqlString += string.Format(" and tt.tran_time_min <= '{0}' ", DateTime.Now.ToString("HHmm"));
            if (!String.IsNullOrEmpty(StationAreaID))
            {
                sqlString += string.Format(" and bdi.station_area_id = '{0}'", StationAreaID);
            }
            if (!String.IsNullOrEmpty(StationID))
            {
                sqlString += string.Format(" and bdi.station_id = '{0}'", StationID);
            }
            if (!String.IsNullOrEmpty(StationHallID))
            {
                sqlString += string.Format(" and bdi.station_hall_id = '{0}'", StationHallID);
            }
            if (!String.IsNullOrEmpty(HallGroupID))
            {
                sqlString += string.Format(" and bdi.hall_group_id = '{0}'", HallGroupID);
            }
            if (!String.IsNullOrEmpty(DeviceType))
            {
                sqlString += string.Format(" and bdi.device_type = '{0}'", DeviceType.ToLower());
            }
            if (!String.IsNullOrEmpty(afcType))
            {
                sqlString += string.Format("  and concat(tt.afc_type,tt.tran_value)  in ({0})", afcType);

            }
            sqlString += string.Format(" and to_number(tt.issuer_id) in ('99', '1')");
            sqlString += string.Format(" group by tt.afc_type,bdi.device_type,tt.tran_time_min,tt.tran_value");

            return sqlString;
        }

       /// <summary>
       /// 取得最大页数和当前有几页
       /// </summary>
        public static  void GetTotalCurrentPage()
        {
              //-->获取参数。
            PassengerFlowParamCfg cfg = SysConfig.GetSysConfig().PassengerFlowParamCfg;
            //-->获取分钟。
            int minute = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            //-->获取页面点得个数。
            PassengerFlowHelper.PagePoint = cfg.PagePoint;
            PassengerFlowHelper.TimeInterval = cfg.TimeInterval;
            //-->获取有多少页。
            int count = minute % (cfg.PagePoint * cfg.TimeInterval);
            if (count > 0)
            {
                count = Convert.ToInt32(minute / (cfg.PagePoint * cfg.TimeInterval)) + 1;
            }
            else
            {
                count = Convert.ToInt32(minute / (cfg.PagePoint * cfg.TimeInterval));
            }
            PassengerFlowHelper.PageCount = count;

            //-->判断当前是第几页。
            int pageMinute = cfg.PagePoint * cfg.TimeInterval;
            if (Convert.ToInt32(minute / pageMinute) > 0)
            {
                PassengerFlowHelper.PageCurrentIndex = Convert.ToInt32(minute / pageMinute) + 1;
            }
            else
            {
                PassengerFlowHelper.PageCurrentIndex = 1;
            }
        }

        /// <summary>
        /// 设置客流监视X轴Y轴的点
        /// </summary>
        /// <param name="XValue">X轴坐标值</param>
        /// <param name="YValue">Y轴坐标值</param>
        /// <param name="Interval">时间间隔</param>
        public static void SetPassengerMonitor(out List<object> XValue, out List<List<string>> YValue, int Interval)
        {

            XValue = null;
            YValue = null;
            XValue = new List<object>();
            YValue = new List<List<string>>();
            columnCounter = 0;
            PassengerFlowMonitorItem.Clear();
            pftm = null;
            rowCounter = 0;
            try
            {
                #region -->
                int beginTime = 0;
                if (PageCurrentIndex == 0)
                    beginTime = (PageCurrentIndex - 1) * PagePoint;
                else
                    beginTime = ((PageCurrentIndex - 1) * PagePoint) - 1;

                int currentTimePoint = PageCurrentIndex * PagePoint;
                for (int i = beginTime; i < currentTimePoint; i++)
                {
                    int timePoint = i * TimeInterval;
                    int modminute = (timePoint % 60);
                    int hours = timePoint / 60;
                    int modhours = hours % 24;
                    int days = hours / 24;
                    if (days > 0)
                    {
                        if (modminute >= 0)
                            XValue.Add((modhours.ToString()).PadLeft(2, "0"[0]) + ":" + (modminute.ToString()).PadLeft(2, "0"[0]));
                    }
                    else
                    {
                        if (modminute >= 0)
                            XValue.Add((modhours.ToString()).PadLeft(2, "0"[0]) + ":" + (modminute.ToString()).PadLeft(2, "0"[0]));
                    }
                }
                StartTime = XValue[0].ToString().Replace(":", "");
                if (XValue.Count > 0)
                {
                    EndTime = XValue[XValue.Count - 1].ToString().Replace(":", "");
                }
                else
                {
                    EndTime = StartTime;
                }

                #endregion -->

                //取得监视客流的类型
                List<PassFlowConfig> pcItem = GetPassengerMonitorConfig();
                if (pcItem == null || pcItem.Count == 0)
                {
                    return;
                }
                //取得afc_type的值
                string afcTypeCode = GetBissTypeCode();
                //组SQL文
                string sqlQuery = GetSqlSentence(afcTypeCode);
                //从数据库中取客流信息
                string databeginTime = "数据库中取客流信息开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(databeginTime, LogFlag.InfoFormat);
                List<PassengerMonitor> item = SetPassengerMonitor(sqlQuery);
                string dataendTime = "数据库中取客流信息结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(dataendTime, LogFlag.InfoFormat);
                //设置客流统计行数
                if (pcItem != null)
                {
                    if ((pcItem.Count % 3) == 0)
                    {
                        rowCounter = pcItem.Count / 3;
                    }
                    else
                    {
                        rowCounter = pcItem.Count / 3 + 1;
                    }
                }
                pftm = new PassengerFlowTypeMonitorInfo();

                string YbeginTime = "计算Y轴开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(YbeginTime, LogFlag.InfoFormat);

                //根据不同的客流类型进行统计
                for (int ii = 0; ii < pcItem.Count(); ii++)
                {

                    PassFlowConfig pc = pcItem[ii];
                    //传入客流类型和数据库中取客流信息
                    List<PassengerMonitor> pmList = SetPassengerFlowTypeMonitorInfo(pc, item);
                    if (pmList != null && pmList.Count > 0 &&
                        pmList.Where(p => p.Total > 0).ReturnT<PassengerMonitor>() != null)
                    {
                        List<string> strList = GetList(pmList, beginTime, currentTimePoint, TimeInterval);
                        YValue.Add(strList);
                    }
                    else
                    {
                        List<string> ss = new List<string>();
                        ss.Add("0");
                        YValue.Add(ss);
                    }
                }
                string YendTime = "计算Y轴结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(YendTime, LogFlag.InfoFormat);


                if (rowCounter > PassengerFlowMonitorItem.Count)
                {
                    PassengerFlowMonitorItem.Add(pftm);
                }
                //取得最大页数和当前页数
                //GetTotalCurrentPage();

                PassengerFlowNumberEventArgs e = new PassengerFlowNumberEventArgs();
                e.CurrentDateTime = DateTime.Now;
                //e.CurrentPage = PassengerFlowHelper.PageCount.ToString();
                //e.TotalPage = PassengerFlowHelper.PageCurrentIndex.ToString();
                e.PassengerFlowNumberItem = PassengerFlowMonitorItem;
                PassengerFlowNumberMethod(null, e);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }




        /// <summary>
        /// 设置票价客流监视X轴Y轴的点
        /// </summary>
        /// <param name="XValue">X轴坐标值</param>
        /// <param name="YValue">Y轴坐标值</param>
        /// <param name="Interval">时间间隔</param>
        public static void SetPassengerFareMonitor(out List<object> XValue, out List<List<string>> YValue, int Interval)
        {

            XValue = null;
            YValue = null;
            XValue = new List<object>();
            YValue = new List<List<string>>();
            columnCounter = 0;
            PassengerFlowMonitorItem.Clear();
            pftm = null;
            rowCounter = 0;
            try
            {
                #region -->
                int beginTime = 0;
                if (PageCurrentIndex == 0)
                    beginTime = (PageCurrentIndex - 1) * PagePoint;
                else
                    beginTime = ((PageCurrentIndex - 1) * PagePoint) - 1;

                int currentTimePoint = PageCurrentIndex * PagePoint;
                for (int i = beginTime; i < currentTimePoint; i++)
                {
                    int timePoint = i * TimeInterval;
                    int modminute = (timePoint % 60);
                    int hours = timePoint / 60;
                    int modhours = hours % 24;
                    int days = hours / 24;
                    if (days > 0)
                    {
                        if (modminute >= 0)
                            XValue.Add((modhours.ToString()).PadLeft(2, "0"[0]) + ":" + (modminute.ToString()).PadLeft(2, "0"[0]));
                    }
                    else
                    {
                        if (modminute >= 0)
                            XValue.Add((modhours.ToString()).PadLeft(2, "0"[0]) + ":" + (modminute.ToString()).PadLeft(2, "0"[0]));
                    }
                }
                StartTime = XValue[0].ToString().Replace(":", "");
                if (XValue.Count > 0)
                {
                    EndTime = XValue[XValue.Count - 1].ToString().Replace(":", "");
                }
                else
                {
                    EndTime = StartTime;
                }

                #endregion -->

                //取得监视客流的类型
                List<PassFlowConfig> pcItem = GetPassengerMonitorConfig(1);
                if (pcItem == null || pcItem.Count == 0)
                {
                    return;
                }
                //取得afc_type的值
                string afcTypeCode = GetBissTypeCode(1);
                //组SQL文
                string sqlQuery = GetSqlFare(afcTypeCode);
                //从数据库中取客流信息
                string databeginTime = "数据库中取客流信息开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(databeginTime, LogFlag.InfoFormat);
                List<PassengerMonitor> item = SetPassengerFareMonitorItem(sqlQuery);
                string dataendTime = "数据库中取客流信息结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(dataendTime, LogFlag.InfoFormat);
                //设置客流统计行数
                if (pcItem != null)
                {
                    if ((pcItem.Count % 3) == 0)
                    {
                        rowCounter = pcItem.Count / 3;
                    }
                    else
                    {
                        rowCounter = pcItem.Count / 3 + 1;
                    }
                }
                pftm = new PassengerFlowTypeMonitorInfo();

                string YbeginTime = "计算Y轴开始：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(YbeginTime, LogFlag.InfoFormat);

                //根据不同的客流类型进行统计
                for (int ii = 0; ii < pcItem.Count(); ii++)
                {

                    PassFlowConfig pc = pcItem[ii];
                    //传入客流类型和数据库中取客流信息
                    List<PassengerMonitor> pmList = SetPassengerFareTypeMonitorInfo(pc, item);
                    if (pmList != null && pmList.Count > 0 &&
                        pmList.Where(p => p.Total > 0).ReturnT<PassengerMonitor>() != null)
                    {
                        List<string> strList = GetList(pmList, beginTime, currentTimePoint, TimeInterval);
                        YValue.Add(strList);
                    }
                    else
                    {
                        List<string> ss = new List<string>();
                        ss.Add("0");
                        YValue.Add(ss);
                    }
                }
                string YendTime = "计算Y轴结束：" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
                Wrapper.Instance.ConsoleWriteLine(YendTime, LogFlag.InfoFormat);


                if (rowCounter > PassengerFlowMonitorItem.Count)
                {
                    PassengerFlowMonitorItem.Add(pftm);
                }
                //取得最大页数和当前页数
                //GetTotalCurrentPage();

                PassengerFlowNumberEventArgs e = new PassengerFlowNumberEventArgs();
                e.CurrentDateTime = DateTime.Now;
                e.PassengerFlowNumberItem = PassengerFlowMonitorItem;
                PassengerFlowNumberMethod(null, e);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }


        /// <summary>
        /// 设置客流监视
        /// </summary>
        /// <param name="sqlQuery">查询语句</param>
        /// <returns>返回客流监视集合</returns>
        static List<PassengerMonitor> SetPassengerMonitor(string sqlQuery)
        {
            List<PassengerMonitor> pMList = new List<PassengerMonitor>();

            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(sqlQuery);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PassengerMonitor pm = new PassengerMonitor();
                        try
                        {
                            pm.Tran_time_min = dr.IsNull("tran_time_min") == true ? "" : dr["tran_time_min"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        } try
                        {
                            pm.Afc_type = dr.IsNull("afc_type") == true ? "" : dr["afc_type"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            pm.Device_type = dr.IsNull("device_type") == true ? "" : dr["device_type"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        } 
                        try
                        {
                            pm.Total = dr.IsNull("Total") == true ? 0 : Convert.ToDecimal(dr["Total"]);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            pm.Issuer_id = dr.IsNull("issuer_id") == true ? 0 : Convert.ToDecimal(dr["issuer_id"]);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        } 
                        pMList.Add(pm);
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return pMList;
        }


        /// <summary>
        /// 设置客流监视
        /// </summary>
        /// <param name="sqlQuery">查询语句</param>
        /// <returns>返回客流监视集合</returns>
        static List<PassengerMonitor> SetPassengerFareMonitorItem(string sqlQuery)
        {
            List<PassengerMonitor> pMList = new List<PassengerMonitor>();

            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(sqlQuery);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PassengerMonitor pm = new PassengerMonitor();
                        try
                        {
                            pm.Tran_time_min = dr.IsNull("tran_time_min") == true ? "" : dr["tran_time_min"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        } try
                        {
                            pm.Afc_type = dr.IsNull("afc_type") == true ? "" : dr["afc_type"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            pm.Device_type = dr.IsNull("device_type") == true ? "" : dr["device_type"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            pm.Total = dr.IsNull("Total") == true ? 0 : Convert.ToDecimal(dr["Total"]);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            pm.Tran_value = dr.IsNull("tran_value") == true ? 0 : Convert.ToDecimal(dr["tran_value"]);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        pMList.Add(pm);
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return pMList;
        }

        /// <summary>
        /// 获得Y轴坐标集合
        /// </summary>
        /// <param name="dt">数据集合</param>
        /// <param name="beginTime">起始时间</param>
        /// <param name="currentTimePoint">当前时间</param>
        /// <param name="Interval">时间间隔</param>
        /// <returns>Y轴坐标集合</returns>
        private static List<string> GetList(List<PassengerMonitor> pmList, int beginTime, int currentTimePoint, int Interval)
        {
            if (pmList == null)
            {
                return new List<string>();
            }
            //--->准备hashTable
            Hashtable hashValue = new Hashtable();
            //--->为hashTable添值
            foreach (PassengerMonitor pm in pmList)
            {
                int keyTime = (int.Parse(pm.Tran_time_min.ToString().Substring(0, 2))) * 60 +
                    int.Parse(pm.Tran_time_min.ToString().Substring(2, 2));

                int hashKey = keyTime;
                if (!hashValue.Contains(hashKey.ToString()))
                {
                    hashValue.Add(hashKey.ToString(), pm.Total.ToString());
                }
                else
                {
                    try
                    {
                        hashValue[keyTime.ToString()] = decimal.Parse(hashValue[hashKey.ToString()].ToString()) + pm.Total;
                    }
                    catch { }
                    // WriteLog.Log_Info("pmList" + keyTime.ToString());
                }
            }

            List<string> list = new List<string>();
            for (int i = beginTime; i <= currentTimePoint; i++)
            {
                int temp = 0;
                for (int k = i * Interval; k < (i + 1) * Interval; k++)
                {
                    if (hashValue[k.ToString()] != null)
                    {
                        try
                        {
                            temp = temp + int.Parse(hashValue[k.ToString()].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
                list.Add(temp.ToString());
            }
            return list;
        }

        #endregion --> 客流。

        #region -->历史客流。

        #region --> 属性。

        /// <summary>
        /// 历史客流查询条件。
        /// </summary>
        static PassengerFlowQueryCondition _QueryCondition = new PassengerFlowQueryCondition();

        /// <summary>
        /// 客流参数。
        /// </summary>
        private static PassengerFlowParamCfg _HistoryCfg = new PassengerFlowParamCfg();

        /// <summary>
        /// 当前第几页。
        /// </summary>
        private static int _HistoryPageCurrentIndex;

        /// <summary>
        /// 页的数量。
        /// </summary>
        private static int _HistoryPageCount;

        /// <summary>
        /// 页的点数。
        /// </summary>
        private static int _HistoryPagePoint = 50;

        /// <summary>
        /// 时间间隔。
        /// </summary>
        private static int _HistoryTimeInterval;

        /// <summary>
        /// 开始时间
        /// </summary>
        private static string HistoryStartTime = "";

        /// <summary>
        /// 结束时间
        /// </summary>
        private static string HistoryEndTime = "";


        /// <summary>
        /// 客流参数。
        /// </summary>
        public static PassengerFlowParamCfg HistoryCfg
        {
            get { return _HistoryCfg; }
            set { _HistoryCfg = value; }
        }

        /// <summary>
        /// 当前第几页。
        /// </summary>
        public static int HistoryPageCurrentIndex
        {
            get { return _HistoryPageCurrentIndex; }
            set { _HistoryPageCurrentIndex = value; }
        }

        /// <summary>
        /// 页的数量。
        /// </summary>
        public static int HistoryPageCount
        {
            get { return _HistoryPageCount; }
            set { _HistoryPageCount = value; }
        }

        /// <summary>
        /// 页的点数。
        /// </summary>
        public static int HistoryPagePoint
        {
            get { return _HistoryPagePoint; }
            set { _HistoryPagePoint = value; }
        }

        /// <summary>
        /// 时间间隔。
        /// </summary>
        public static int HistoryTimeInterval
        {
            get { return _HistoryTimeInterval; }
            set { _HistoryTimeInterval = value; }
        }

        public static string initStartDateTime = null;

        public static string initEndDateTime = null;
        /// <summary>
        /// 历史客流查询条件
        /// </summary>
        public static PassengerFlowQueryCondition HistoryQueryCondition
        {
            get { return _QueryCondition; }
            set { _QueryCondition = value; }
        }

        #endregion --> 属性。

        /// <summary>
        /// 获取历史客流客流类型。
        /// </summary>
        /// <returns>获取参数</returns>
        public static List<PassFlowConfig> HistoryPassengerMonitoryCfg()
        {
            try
            {
                if (HistoryCfg.MonitorParamItems.Count == 0)
                {
                    return SysConfig.GetSysConfig().PassengerFlowParamCfg.MonitorParamItems.Where(p => p.IsMonitor == true).GetListTContext<PassFlowConfig>();
                }
                else
                {
                    return HistoryCfg.MonitorParamItems.Where(p => p.IsMonitor == true).GetListTContext<PassFlowConfig>();
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
        }

        /// <summary>
        /// 获取历史客流客流类型条件。
        /// </summary>
        /// <returns>返回获取历史客流客流类型条件字符串</returns>
        static string HistoryBissTypeCode()
        {
            List<PassFlowConfig> pcItem = HistoryPassengerMonitoryCfg();
            if (pcItem == null || pcItem.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (PassFlowConfig pc in pcItem)
            {
                if (!sb.ToString().Contains(pc.Key))
                {
                    sb.Append("'").Append(pc.Key).Append("'").Append(",");
                }
            }
            string context = sb.ToString().Substring(0, sb.ToString().Length - 1);
            return context;
        }

        /// <summary>
        /// 历史客流数据统计。
        /// </summary>
        /// <param name="pc">客流参数</param>
        /// <param name="item">历史客流数据集合</param>
        /// <returns>返回历史客流数据统计集合</returns>
        static List<PassengerMonitor> HistoryPassengerFlowTypeMonitorInfo(
            PassFlowConfig pc, List<PassFlowConfig> item, DateTime beginTime, DateTime endTime)
        {
            List<PassengerMonitor> pmList = new List<PassengerMonitor>();
            decimal currentTotal = 0;
            decimal total = 0;
            if (item != null && item.Count > 0)
            {
                List<HistoryPassengerFlowData> itemList = null;

                if (!string.IsNullOrEmpty(pc.Key))
                {
                    itemList = historyPassenger[pc.Key] as List<HistoryPassengerFlowData>;
                }
                foreach (PassengerMonitor pm in itemList)
                {
                    pmList.Add(pm);
                    total += pm.Total;
                    string org = pm.Tran_date + pm.Tran_time_min;
                    string _b = beginTime.ToString("yyyyMMddHHmm");
                    string _e = endTime.ToString("yyyyMMddHHmm");
                    if (Convert.ToDouble(_b) <= Convert.ToDouble(org) &&
                                Convert.ToDouble(org) < Convert.ToDouble(_e))
                    {
                        currentTotal += pm.Total;
                    }
                }
            }


            if (pc == null)
            {
                return pmList;
            }
            string card_issue_name = string.Empty;
            if (pc.CardIssueId == "1")
            {
                card_issue_name = "一票通";
            }
            else
            {
                card_issue_name = "一卡通";
            }
    
          
            if (columnCounter == 0)
            {
                pftm.CardIssueName = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber = currentTotal.ToString();
                pftm.PassengerFlowTotal = total.ToString();
                pftm.PassengerFlowTypeName = pc.Value;
                columnCounter++;
            }
            else if (columnCounter == 1)
            {
                pftm.CardIssueName1 = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber1 = currentTotal.ToString();
                pftm.PassengerFlowTotal1 = total.ToString();
                pftm.PassengerFlowTypeName1 = pc.Value;
                columnCounter++;
            }
            else if (columnCounter == 2)
            {
                pftm.CardIssueName2 = card_issue_name;
                pftm.CurrentPagePassengerFlowNumber2 = currentTotal.ToString();
                pftm.PassengerFlowTotal2 = total.ToString();
                pftm.PassengerFlowTypeName2 = pc.Value;
                PassengerFlowMonitorItem.Add(pftm);
                pftm = new PassengerFlowTypeMonitorInfo();
                columnCounter = 0;
            }

            return pmList;
        }

        private static Hashtable historyPassenger = new Hashtable();

        /// <summary>
        /// 历史客流拆线图数据集合。
        /// </summary>
        /// <param name="sqlQuery">SQL语句</param>
        /// <returns>返回历史客流拆线图数据集合</returns>
        static List<HistoryPassengerFlowData> HistoryPassengerFlowData(string sqlQuery, List<PassFlowConfig> pcItem)
        {
            historyPassenger.Clear();
            List<HistoryPassengerFlowData> item = new List<HistoryPassengerFlowData>();
            if (pcItem.Count == 0)
            {
                WriteLog.Log_Info("参数配置为空，请查询是否存在客流管理参数！");
                return null;
            }
            foreach (PassFlowConfig pc in pcItem)
            {
                historyPassenger.Add(pc.Key, new List<HistoryPassengerFlowData>());
            }
            DataTable dt = null;
            try
            {
                dt = DBCommon.Instance.GetDatatable(sqlQuery);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                List<BasiStationInfo> bsiItem = BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
                foreach (DataRow dr in dt.Rows)
                {
                    HistoryPassengerFlowData pm = new HistoryPassengerFlowData();
                    try
                    {
                        pm.Tran_time_min = dr.IsNull("tran_time_min") == true ? "" : dr["tran_time_min"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Afc_type = dr.IsNull("afc_type") == true ? "" : dr["afc_type"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Issuer_id = dr.IsNull("issuer_id") == true ? 0 : Convert.ToDecimal(dr["issuer_id"]);
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Device_type = dr.IsNull("device_type") == true ? "" : dr["device_type"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Total = dr.IsNull("total") == true ? 0 : Convert.ToDecimal(dr["total"]);
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Afc_type_name = dr.IsNull("tran_name") == true ? "" : dr["tran_name"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Tran_date = dr.IsNull("tran_date") == true ? "" : dr["tran_date"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.station_cn_name = dr.IsNull("station_cn_name") == true ? "" : dr["station_cn_name"].ToString();
                        if (!String.IsNullOrEmpty(pm.station_cn_name))
                        {
                            pm.Station_id = bsiItem.Where(p => p.station_cn_name == pm.station_cn_name).GetTContext<BasiStationInfo>().station_id;
                        }
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.station_hall_id = dr.IsNull("station_hall_id") == true ? "" : dr["station_hall_id"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    try
                    {
                        pm.Device_name = dr.IsNull("Device_name") == true ? "" : dr["Device_name"].ToString();
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }

                    if (!string.IsNullOrEmpty(pm.Afc_type + pm.Issuer_id.ToHexNumberUShort().ConvertNumberToHexString()))
                    {
                        if (historyPassenger.Count > 0)
                        {
                            if (historyPassenger.ContainsKey(pm.Afc_type + pm.Issuer_id.ToHexNumberUShort().ConvertNumberToHexString()))
                            {
                                ((List<HistoryPassengerFlowData>)historyPassenger[pm.Afc_type + pm.Issuer_id.ToHexNumberUShort().ConvertNumberToHexString()]).Add(pm);
                            }
                        }
                    }
                    item.Add(pm);
                }//End foreach;
            }//End foreach;
            return item;
        }

        /// <summary>
        /// 历史客流饼状图数据集合。
        /// </summary>
        /// <param name="sqlQuery">SQL语句</param>
        /// <returns>返回历史客流饼状图数据集合</returns>
        static List<HistoryPassengerFlowPieData> HistoryPassengerFlowPieData(string sqlQuery)
        {
            List<HistoryPassengerFlowPieData> pList = new List<HistoryPassengerFlowPieData>();
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(sqlQuery);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        HistoryPassengerFlowPieData p = new HistoryPassengerFlowPieData();
                        try
                        {
                            p.Card_issue_name = dr.IsNull("issuer_name") == true ? "" : dr["issuer_name"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            p.Product_type_name_cn = dr.IsNull("tick_mana_type_name") ? "" : dr["tick_mana_type_name"].ToString();
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        try
                        {
                            p.Total = dr.IsNull("total") ? 0 : Convert.ToDecimal(dr["total"]);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        pList.Add(p);
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return pList;
        }

        /// <summary>
        /// 按客流类型统计拆线图
        /// </summary>
        /// <param name="XValue">X轴坐标值</param>
        /// <param name="YValue">Y轴坐标值</param>
        /// <param name="Interval">时间间隔</param>
        public static void HistorySetPassengerFlowLine(out List<object> XValue,
            out List<List<string>> YValue, int Interval)
        {
            Interval = HistoryTimeInterval;
            int TimeInterval = HistoryQueryCondition.TimeInterval;
            columnCounter = 0;
            PassengerFlowMonitorItem.Clear();
            pftm = null;
            rowCounter = 0;


            /**
             * 时间格式 02-10 20:02。
             * */
            string f = "MM-dd HH:mm";

            #region -->
            XValue = null;
            YValue = null;
            XValue = new List<object>();
            YValue = new List<List<string>>();
            if (HistoryQueryCondition == null)
            {
                return;
            }
            int beginTimePoint = 0;
            int currentTimePoint = HistoryPagePoint;

            for (int i = 0; i < currentTimePoint; i++)
            {
                //-->1、获取
                string value = HistoryQueryCondition.BeginTime.AddMinutes(i * HistoryTimeInterval).ToString(f);
                XValue.Add(value);
            }
            #endregion -->

            List<PassFlowConfig> pcItem = HistoryPassengerMonitoryCfg();
            if (pcItem == null || pcItem.Count == 0)
            {
                return;
            }

            string afcTypeCode = HistoryBissTypeCode();
            string sqlQuery = HistorySqlSentence(afcTypeCode);
            List<HistoryPassengerFlowData> item = HistoryPassengerFlowData(sqlQuery, pcItem);

            if ((pcItem.Count % 3) == 0)
            {
                rowCounter = pcItem.Count / 3;
            }
            else
            {
                rowCounter = pcItem.Count / 3 + 1;
            }

            //2011.9.26 dusj modify begin
            //DateTime D_BeginTime = HistoryQueryCondition.DtControlBeginTime;
            //DateTime D_EndTime = HistoryQueryCondition.DtControlEndTime;

            DateTime D_BeginTime = HistoryQueryCondition.BeginTime;
            DateTime D_EndTime = HistoryQueryCondition.EndTime;

            //2011.9.26 dusj modify end 

            pftm = new PassengerFlowTypeMonitorInfo();
            foreach (PassFlowConfig pc in pcItem)
            {
                List<PassengerMonitor> pmList = HistoryPassengerFlowTypeMonitorInfo(pc, pcItem, D_BeginTime, D_EndTime);

                if (pmList != null && pmList.Count > 0)
                {
                    List<string> strList = HistoryGetListPassengerFlowLine(pmList,
                        beginTimePoint, currentTimePoint, D_BeginTime, D_EndTime, Interval);

                    YValue.Add(strList);
                }
                else
                {
                    List<string> ss = new List<string>();
                    ss.Add("0");
                    YValue.Add(ss);
                }

            }

            HistoryPassengerFlowQueryResultEventArgs e = new HistoryPassengerFlowQueryResultEventArgs();
            e.PassengerFlowQueryResultList = item;
            HistoryPassengerFlowQueryMethod(item, e);

            if (rowCounter > PassengerFlowMonitorItem.Count)
            {
                PassengerFlowMonitorItem.Add(pftm);
            }
            PassengerFlowNumberEventArgs en = new PassengerFlowNumberEventArgs();
            en.CurrentDateTime = DateTime.Now;
            en.PassengerFlowNumberItem = PassengerFlowMonitorItem;
            PassengerFlowNumberMethod(null, en);
        }
        Dictionary<string, string> ss = new Dictionary<string, string>();
        /// <summary>
        /// 获得Y轴坐标集合
        /// </summary>
        /// <param name="pmList">数据集合</param>
        /// <param name="beginTime">起始时间</param>
        /// <param name="currentTimePoint">当前时间</param>
        /// <param name="Interval">时间间隔</param>
        /// <returns>Y轴坐标集合</returns>
        static List<string> HistoryGetListPassengerFlowLine(List<PassengerMonitor> pmList,
            int beginTimePoint, int currentTimePoint,
            DateTime beginTime, DateTime endTime, int Interval)
        {
            try
            {
                if (pmList == null || pmList.Count == 0)
                {
                    return new List<string>();
                }

                //--->准备hashTable
                Hashtable hashValue = new Hashtable();
                //--->为hashTable添值
                foreach (PassengerMonitor pm in pmList)
                {
                    string keyTime = pm.Tran_date + pm.Tran_time_min;

                    if (!string.IsNullOrEmpty(keyTime))
                    {
                        if (!hashValue.Contains(keyTime.ToString()))
                        {
                            hashValue.Add(keyTime.ToString(), pm.Total.ToString());
                        }
                        else
                        {
                            try
                            {
                                hashValue[keyTime.ToString()] = decimal.Parse(hashValue[keyTime.ToString()].ToString()) + pm.Total;
                            }
                            catch { }
                            // WriteLog.Log_Info("pmList" + keyTime.ToString());
                        }
                    }

                }

               
                List<string> list = new List<string>();

                for (int i = beginTimePoint; i < currentTimePoint; i++)
                {
                    decimal total = 0;
                    for (int j = i * Interval; j < (i + 1) * Interval; j++)
                    {
           

                        if (hashValue[beginTime.AddMinutes(j).ToString("yyyyMMddHHmm")] != null)
                        {
                            try
                            {
                                total = total + int.Parse(hashValue[beginTime.AddMinutes(j).ToString("yyyyMMddHHmm")].ToString());
                            }
                            catch
                            {
                            }
                        }
                    }

                    list.Add(total.ToString());
                }

                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 获取历史客流查询语句。
        /// </summary>
        /// <param name="bissTypeCode">客流类型</param>
        /// <returns>返回历史客流查询语句字符串</returns>
        static string HistorySqlSentence(string bissTypeCode)
        {

            string sqlString = string.Format("select tt.issuer_id, tt.station_id, tt.afc_type,bdi.device_type,tt.tran_date,tt.tran_time_min,sum(tt.passenger_num) as total,bsi.station_cn_name, bdi.device_name,tran.tran_name, bdi.station_hall_id ");
           sqlString += string.Format(" from data_pass_flow_info_his tt ");
            sqlString += string.Format(" left join basi_dev_info bdi on bdi.device_id = tt.device_id ");
            sqlString += string.Format(" left join basi_station_info bsi on bsi.station_id = bdi.station_id  and bsi.line_id = bdi.line_id ");
            sqlString += string.Format(" left join basi_dev_type_info bdti on bdti.device_type = bdi.device_type ");
            sqlString += string.Format(" left join basi_tran_type_info tran on tran.tran_type = tt.tran_type and tran.afc_type = tt.afc_type ");

            sqlString += string.Format("  where tt.tran_date||tt.tran_time_min  between '{0}'", initStartDateTime);
            sqlString += string.Format(" and '{0}'", initEndDateTime);


            if (!String.IsNullOrEmpty(HistoryQueryCondition.StationID))
            {
                sqlString += string.Format(" and bdi.station_id = '{0}'", HistoryQueryCondition.StationID);
            }
            if (!String.IsNullOrEmpty(HistoryQueryCondition.StationHallID))
            {
                sqlString += string.Format(" and bdi.station_hall_id = '{0}'", HistoryQueryCondition.StationHallID);
            }
           
            if (!String.IsNullOrEmpty(HistoryQueryCondition.DeviceType))
            {
                sqlString += string.Format(" and bdi.device_type = '{0}'", HistoryQueryCondition.DeviceType.ToLower());
            }
            if (!String.IsNullOrEmpty(HistoryQueryCondition.PassengerFlowType))
            {
                sqlString += string.Format("  and concat(tt.afc_type,lpad(tt.issuer_id, 2, '0'))  in ({0})", HistoryQueryCondition.PassengerFlowType);
               
            }
            sqlString += string.Format(" and to_number(tt.issuer_id) in ('99', '1')");
            sqlString += string.Format(" group by  tt.station_id,tt.issuer_id,tt.afc_type,bdi.device_type,tt.tran_date,tt.tran_time_min,tran.tran_name,bsi.station_cn_name,bdi.device_name, bdi.station_hall_id");
            sqlString += string.Format(" order by  tt.station_id,tt.issuer_id,tt.afc_type,bdi.device_type,tt.tran_date,tt.tran_time_min,tran.tran_name,bsi.station_cn_name,bdi.device_name, bdi.station_hall_id");
            return sqlString;


        }

        /// <summary>
        /// 出站客流SQL语句。
        /// </summary>
        /// <returns>返回出站客流SQL语句字符串</returns>
        static string HistorySqlSentenceExitPie()
        {
            string sqlQuery = string.Format("select decode(t.issuer_id, '1', '一卡通', '99', '一票通') as issuer_name,tick.tick_mana_type_name,t.afc_type,sum(t.passenger_num) as total ");
            sqlQuery += string.Format(" from data_pass_flow_info_his t ");
            //AG产生的交易数据
            sqlQuery += string.Format(" left join basi_dev_info bdi on bdi.device_id = t.device_id  and lower(bdi.device_type) = '06'");
            sqlQuery += string.Format(" left join basi_tick_mana_type_info tick on t.tick_card_type = tick.tick_mana_type");
         
            sqlQuery += string.Format(" where ");
          

            if (HistoryQueryCondition != null)
            {
                sqlQuery += string.Format(" t.tran_date || t.tran_time_min between '{0}'", HistoryQueryCondition.ControlBeginTime);
                sqlQuery += string.Format(" and '{0}'", HistoryQueryCondition.ControlEndTime);
                if (!String.IsNullOrEmpty(HistoryQueryCondition.StationID))
                {
                    sqlQuery += string.Format(" and bdi.station_id = '{0}'", HistoryQueryCondition.StationID);
                }
                if (!String.IsNullOrEmpty(HistoryQueryCondition.StationHallID))
                {
                    sqlQuery += string.Format(" and bdi.station_hall_id = '{0}'", HistoryQueryCondition.StationHallID);
                }
            }
            //afc_type='04' 出站
            sqlQuery += string.Format(" and t.afc_type = '04'");
            sqlQuery += string.Format(" group by t.issuer_id, t.afc_type, tick.tick_mana_type_name");
            return sqlQuery;
        }

        /// <summary>
        /// 进站客流SQL语句。
        /// </summary>
        /// <returns>返回进站客流SQL语句字符串</returns>
        static string HistorySqlSentenceEntryPie()
        {
            string sqlQuery = string.Format("select decode(t.issuer_id, '1', '一卡通', '99', '一票通') as issuer_name,tick.tick_mana_type_name,t.afc_type,sum(t.passenger_num) as total ");
            sqlQuery += string.Format(" from data_pass_flow_info_his t ");
            //AG产生的交易数据
            sqlQuery += string.Format(" left join basi_dev_info bdi on bdi.device_id = t.device_id  and lower(bdi.device_type) = '06'");
            sqlQuery += string.Format(" left join basi_tick_mana_type_info tick on t.tick_card_type = tick.tick_mana_type");

            sqlQuery += string.Format(" where ");


            if (HistoryQueryCondition != null)
            {
                sqlQuery += string.Format(" t.tran_date || t.tran_time_min between '{0}'", HistoryQueryCondition.ControlBeginTime);
                sqlQuery += string.Format(" and '{0}'", HistoryQueryCondition.ControlEndTime);
                if (!String.IsNullOrEmpty(HistoryQueryCondition.StationID))
                {
                    sqlQuery += string.Format(" and bdi.station_id = '{0}'", HistoryQueryCondition.StationID);
                }
                if (!String.IsNullOrEmpty(HistoryQueryCondition.StationHallID))
                {
                    sqlQuery += string.Format(" and bdi.station_hall_id = '{0}'", HistoryQueryCondition.StationHallID);
                }
            }
            //afc_type=‘03’进站
            sqlQuery += string.Format(" and t.afc_type ='03'");
            sqlQuery += string.Format(" group by t.issuer_id, t.afc_type, tick.tick_mana_type_name");
            return sqlQuery;
        }

        /// <summary>
        /// 按库存管理类型出站统计饼状图
        /// </summary>
        /// <param name="XValue">X轴坐标值</param>
        /// <param name="YValue">Y轴坐标值</param>
        /// <param name="Interval">时间间隔</param>
        public static void HistorySetPassengerFlowExitPie(out List<object> XValue,
            out List<List<string>> YValue, int Interval)
        {
            XValue = null;
            YValue = null;
            XValue = new List<object>();
            YValue = new List<List<string>>();
            string sqlQuery = HistorySqlSentenceExitPie();
            List<HistoryPassengerFlowPieData> pList = HistoryPassengerFlowPieData(sqlQuery);
            List<HistoryPassengerFlowPieData> dsList = pList.OrderBy(p => p.Card_issue_name).GetListTContext<HistoryPassengerFlowPieData>();
            if (pList != null && pList.Count > 0)
            {
                HistoryPassengerFlowPieEventArgs e = new HistoryPassengerFlowPieEventArgs();
                e.HpList = dsList;
                e.PieType = PieEnum.Exit;
                HistoryPassengerFlowPieMethod(pList, e);
                foreach (HistoryPassengerFlowPieData p in dsList)
                {
                    List<string> a = new List<string>();
                    a.Add(p.Total.ToString());
                    YValue.Add(a);
                    XValue.Add(p.Card_issue_name + p.Product_type_name_cn);
                }
            }
        }

        /// <summary>
        /// 按库存管理类型进站统计饼状图
        /// </summary>
        /// <param name="XValue">X轴坐标值</param>
        /// <param name="YValue">Y轴坐标值</param>
        /// <param name="Interval">时间间隔</param>
        public static void HistorySetPassengerFlowEntryPie(out List<object> XValue,
            out List<List<string>> YValue, int Interval)
        {
            XValue = null;
            YValue = null;
            XValue = new List<object>();
            YValue = new List<List<string>>();
            string sqlQuery = HistorySqlSentenceEntryPie();
            List<HistoryPassengerFlowPieData> pList = HistoryPassengerFlowPieData(sqlQuery);
            List<HistoryPassengerFlowPieData> dsList = pList.OrderBy(p => p.Card_issue_name).GetListTContext<HistoryPassengerFlowPieData>();
            if (pList != null && pList.Count > 0)
            {
                HistoryPassengerFlowPieEventArgs e = new HistoryPassengerFlowPieEventArgs();
                e.HpList = dsList;
                e.PieType = PieEnum.Entry;
                HistoryPassengerFlowPieMethod(pList, e);
                if (dsList != null && dsList.Count > 0)
                {
                    foreach (HistoryPassengerFlowPieData p in dsList)
                    {
                        List<string> a = new List<string>();
                        a.Add(p.Total.ToString());
                        YValue.Add(a);
                        XValue.Add(p.Card_issue_name + p.Product_type_name_cn);
                    }
                }
            }
        }

        public static string getFormatText(string str)
        {
            string except_chars = ": ‘ '）$+=-_";
            string result = Regex.Replace(str, "[" + Regex.Escape(except_chars) + "]", "");
            return result;

        }

        #endregion -->历史客流。
    }
}
