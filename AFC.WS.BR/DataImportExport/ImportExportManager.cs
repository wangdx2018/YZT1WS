using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.BOM2.MessageDispacher;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.DataImportExport
{
    public class ImportExportManager
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType">消息类型</param>
        /// <param name="msgContent">消息内容</param>
        public static void SendMessage(string msgType, object msgContent,string fileName)
        {
            try
            {
                Message msg = new Message();
                msg.MessageType = msgType;
                msg.Content = msgContent;
                msg.MessageSource = fileName;
                MessageManager.SendMessasge(msg);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Fatal(ex.ToString());
            }
        }

        /// <summary>
        /// 将路径中的正斜杠变反斜杠
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        public static string ConvertPathToOpposite(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            return filePath.Replace('/','\\');
        }

        /// <summary>
        /// 将路径中的反斜杠变成正斜杠
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns></returns>
        public static string ConvertPathToPositive(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            return filePath.Replace('\\', '/');
        }

        /// <summary>
        /// 获取设备ID，该设备ID为车站服务器ID，所以一个车站对于一个ID
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceID()
        {
            try
            {
                string stationID=SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                if (string.IsNullOrEmpty(stationID))
                    return null;
                string strSQL = string.Format("select * from basi_station_info t where t.station_id='{0}'", stationID);
                BasiStationInfo stationInfo = DBCommon.Instance.GetModelValue<BasiStationInfo>(strSQL);
                if (stationInfo == null || string.IsNullOrEmpty(stationInfo.device_id))
                    return null;
                return stationInfo.device_id;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }


    }
}
