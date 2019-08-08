using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TJComm
{
    using AFC.BJComm.Data;
    /// <summary>
    /// 通用的包体打包数据，
    /// 只是针对于天津自定义的报文,导则的约定中没有该头
    /// 
    /// 修改人：王冬欣   日期：20100411
    /// 增加了设备范围字段
    /// </summary>
    public class CommHeaderData
    {
        /// <summary>
        ///设备ID
        /// </summary>
        [PackOrder(1),PackInt(4,ByteOrder.Moto)]
        public uint deviceId;

        /// <summary>
        /// 车站ID
        /// </summary>
        [PackOrder(2),PackInt(4,ByteOrder.Moto)]
        public ushort stationId;

        /// <summary>
        /// 操作员ID
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public uint opeatorId;

        /// <summary>
        /// 发生时间
        /// </summary>
        [PackOrder(4),PackInt(4,ByteOrder.Moto)]
        public uint occurTime;

        /// <summary>
        /// 设备范围
        /// </summary>
        [PackOrder(5), PackArray(4,ByteOrder.Moto,0,ByteOrder.Moto,null)]
        public List<DeviceRange> deviceRange = new List<DeviceRange>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceId">设备Id</param>
        /// <param name="sationId">车站ID</param>
        /// <param name="opeatorId">操作员ID</param>
        public CommHeaderData(uint deviceId, ushort sationId, uint opeatorId)
        {
            this.deviceId = deviceId;
            this.stationId =sationId;
            this.opeatorId = opeatorId;
            this.occurTime = GetCurrentAFCTime_t();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="stationId">车站ID</param>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="list">设备范围列表</param>
        public CommHeaderData(uint deviceId, ushort stationId, uint operatorId, List<DeviceRange> list):this(deviceId,stationId,operatorId)
        {
            this.deviceRange = list;
        }

        public CommHeaderData()
        {
            this.occurTime = GetCurrentAFCTime_t();
        }


        public CommHeaderData(uint operatorId)
        {
            deviceId = TJCommMessage.localDeviceId;
            stationId = TJCommMessage.stationId;
            this.opeatorId = operatorId;
            this.occurTime = GetCurrentAFCTime_t();
        }

        /// <summary>
        /// 参照系为格林威治1970年1月1日0分0秒
        /// </summary>
        /// <returns>返回afctime_t</returns>
        private  uint GetCurrentAFCTime_t()
        {
            DateTime dt = DateTime.Now;
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            return (uint)dt.Subtract(dt1970).TotalSeconds - 8 * 60 * 60;
        
        }

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            FieldInfo[] fiArray = this.GetType().GetFields();
            if (fiArray == null || fiArray.Length == 0)
                sb.Append("not public fileds");
            else
            {
                sb.Append("CommHeaderData:");
                for (int i = 0; i < fiArray.Length; i++)
                {
                    sb.Append(string.Format("[{0}]=[{1}]\t", fiArray[i].Name.ToString(), fiArray[i].GetValue(this) == null ? "NULL" : fiArray[i].GetValue(this).ToString()));
                }
            }
            return sb.ToString();
        }
    }



}
