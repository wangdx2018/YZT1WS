using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

    public class DevStatus_1325:AbstractCommBody
    {
        /// <summary>
        /// 设备整体状态
        /// 0：正常服务，
        /// 1：报警，
        /// 2：故障，
        /// 3：通讯中断
        /// </summary>
        [PackOrder(3),PackInt(4,ByteOrder.Moto)]
        public byte devStatus;

        /// <summary>
        /// 设备状态信息
        /// </summary>
        [PackOrder(4), PackArray(4, ByteOrder.Moto, 0, ByteOrder.Moto, null)]
        public List<DevStatusInfo> devStatusInfo = new List<DevStatusInfo>();



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            switch(devStatus)
            {
                case 0:
                    sb.Append("正常服务");
                    break;
                case 1:
                    sb.Append("报警");
                    break;
                case 2:
                    sb.Append("故障");
                    break;
                case 3:
                    sb.Append("通讯中断");
                    break;
            }
            for (int i = 0; i < this.devStatusInfo.Count; i++)
            {
                sb.Append("\n");
                sb.Append(this.devStatusInfo[i].ToString());
            }
            return sb.ToString();
            //return base.ToString();
        }

    }


    /// <summary>
    /// 设备状态上报
    /// </summary>
    public class DevStatusInfo
    {
        [PackOrder(1),PackInt(4,ByteOrder.Moto)]
        /// <summary>
        /// 状态ID
        /// </summary>
        public ushort statusId;

        [PackOrder(2),PackInt(4,ByteOrder.Moto)]
        /// <summary>
        /// 状态值
        /// </summary>
        public byte statusValue;


        public override string ToString()
        {
            return string.Format("状态ID={0},状态值={1}", statusId.ToString("x2"), statusValue.ToString("x2"));
            //return base.ToString();
        }
    }
}
