using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AFC.WS.BR.SLEMonitorManager
{
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    /// <summary>
    /// 该类封装具体的状态的数据的取值的编码
    /// 根据SysConfig.xml中的SysStatusValueName 来判断是否是去
    /// CsswebName还是TJName
    /// </summary>
    public class DevStatus
    {
        //模式名称
        public string[] TVM_WORK_MODE_NAME = new string[] {"未知服务", "正常服务", "服务中/无找零", 
            "服务中/只收纸币", "服务中/只收硬币", "服务中/只售票", "服务中/只充值", "服务中/只收充值卡",
            "服务中/只纸币找零", "暂停服务", "停止服务", "服务中/不收纸币", "服务中/不收硬币"};

                /// <summary>
        /// 2013年3月19日根据与马晓春确定的结果，AG,EQM 状态值为 “00”时，为正常，“07”时，为紧急，其它均为暂停
        /// </summary>
        /// <param name="status_id">状态编号</param>
        /// <param name="status_value">状态值</param>
        /// <returns>返回服务状态信息</returns>
        public string GeAGEQMServerModeName(string status_id, string status_value)
        {
            if (status_id.Equals("0101"))
            {
                if (status_value.Equals("00"))
                {
                    return "正常模式";
                }
                else if(status_id.Equals("07"))
                {
                    return "紧急模式";
                }
                else return "暂停模式";
            }
            else return "未知工作模式";
        }


        /// <summary>
        /// 根据TVM的支付类型、工作模式、找零模式计算得到TVM的工作模式,以下代码由杨爽提供C++版本，改写为C#版本
        /// </summary>
        /// <param name="cssId">支付类型</param>
        /// <param name="cssValue">工作模式</param>
        /// <param name="cssValue">找零模式</param>
        /// <returns>返回基本的状态信息</returns>
        public string GeTvmWorkModeName(string iTransType, string iPayType, string iChangeType)
        {  
            int iModeNameId = 0;
            Boolean bflag = false;

           //算法：根据上面的模式iTransType为工作模式，iPayType为支付模式，iChangeType为找零模式，计算出唯一的iModeNameId
           switch (iTransType.ToByte())
           {
               case (byte)EC_PAY_TYPE.NONE_PAY_TYPE:			//无交易方式
                   iModeNameId = 9;
                   bflag = true;
                   break;
               case (byte)EC_TRANS_TYPE.CHARGE_AND_CARD_SALE:	//充值及充值卡售票
                   iModeNameId = 12;
                   bflag = true;
                   break;
               case (byte)EC_TRANS_TYPE.ALL_TRANS_TYPE:			//所有交易方式
               case (byte)EC_TRANS_TYPE.CHARGE_AND_CASH_SALE:	//充值和现金售票
                   iModeNameId = 1;
                   break;
               case (byte)EC_TRANS_TYPE.CHARGE_ONLY:			//只充值
                   iModeNameId = 6;
                   bflag = true;
                   break;
               case (byte)EC_TRANS_TYPE.CASH_SALE_ONLY:			//只现金售票
               case (byte)EC_TRANS_TYPE.CASH_AND_CARD_SALE:	//现金及充值卡售票
                   iModeNameId = 5;
                   break;
               case (byte)EC_TRANS_TYPE.CARD_SALE_ONLY:			//只收充值卡
                   iModeNameId = 7;
                   bflag = true;
                   break;
               default:
                   iModeNameId = 9;
                   bflag = true;
                   break;
           }
           if (!bflag)
           {
               //支付模式
               if (1 == iModeNameId)
               {
                   switch (iPayType.ToByte())
                   {
                       case (byte)EC_PAY_TYPE.NONE_PAY_TYPE:			//无支付方式
                           iModeNameId = 9;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.CARD_ONLY:				//仅充值卡
                           iModeNameId = 7;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.COIN_ONLY:				//只硬币收入
                           iModeNameId = 4;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.COIN_AND_CARD:			//硬币和充值卡
                           iModeNameId = 11;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.BILL_ONLY:				//只纸币收入
                           iModeNameId = 3;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.BILL_AND_CARD:			//纸币和充值卡
                           iModeNameId = 12;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.BILL_AND_COIN:			//硬币纸币
                       case (byte)EC_PAY_TYPE.ALL_PAY_TYPE:			//所有支付方式
                           break;
                       default:
                           iModeNameId = 9;
                           bflag = true;
                           break;
                   }
               }
               if (EC_TRANS_TYPE.CASH_AND_CARD_SALE.Equals(iTransType.ToByte()))
               {
                   switch (iPayType.ToByte())
                   {
                       case (byte)EC_PAY_TYPE.NONE_PAY_TYPE:			//无支付方式
                           iModeNameId = 9;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.CARD_ONLY:				//仅充值卡
                       case (byte)EC_PAY_TYPE.COIN_AND_CARD:			//硬币和充值卡
                           iModeNameId = 11;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.COIN_ONLY:				//只硬币收入
                           iModeNameId = 4;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.BILL_ONLY:				//只纸币收入
                           iModeNameId = 3;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.BILL_AND_CARD:			//纸币和充值卡
                           iModeNameId = 12;
                           bflag = true;
                           break;
                       case (byte)EC_PAY_TYPE.BILL_AND_COIN:			//硬币纸币
                       case (byte)EC_PAY_TYPE.ALL_PAY_TYPE:			//所有支付方式
                           break;
                       default:
                           iModeNameId = 9;
                           bflag = true;
                           break;
                   }
               }
               if (EC_TRANS_TYPE.CASH_SALE_ONLY.Equals(iTransType.ToByte()))
               {
                   switch (iPayType.ToByte())
                   {
                       case (byte)EC_PAY_TYPE.NONE_PAY_TYPE:			//无支付方式
                       case (byte)EC_PAY_TYPE.CARD_ONLY:				//仅充值卡
                           iModeNameId = 9;
                           bflag = true;
                           break;
                       // case COIN_ONLY:				//只硬币收入
                       // case COIN_AND_CARD:			//硬币和充值卡
                       // iModeNameId = 4;
                       // bflag = true;
                       // break;
                       // case BILL_ONLY:				//只纸币收入
                       // case BILL_AND_CARD:			//纸币和充值卡
                       // iModeNameId = 3;
                       // bflag = true;
                       //break;
                       case (byte)EC_PAY_TYPE.BILL_AND_COIN:			//硬币纸币
                       case (byte)EC_PAY_TYPE.ALL_PAY_TYPE:			//所有支付方式
                       case (byte)EC_PAY_TYPE.BILL_ONLY:				//只纸币收入
                       case (byte)EC_PAY_TYPE.COIN_ONLY:				//只硬币收入
                       case (byte)EC_PAY_TYPE.COIN_AND_CARD:			//硬币和充值卡
                       case (byte)EC_PAY_TYPE.BILL_AND_CARD:			//纸币和充值卡
                           break;
                       default:
                           iModeNameId = 9;
                           bflag = true;
                           break;
                   }
               }
           }

           //找零方式
           if (1 == iModeNameId || 3 == iModeNameId || 12 == iModeNameId)
           {
               switch (iChangeType.ToByte())
               {
                   case (byte)EC_CHANGE_TYPE.NONE_CHANGE_TYPE:		//无找零
                   case (byte)EC_CHANGE_TYPE.COIN_CHANGE_ONLY:		//只硬币找零			
                       iModeNameId = 2;
                       bflag = true;
                       break;
                   case (byte)EC_CHANGE_TYPE.BILL_CHANGE_ONLY:		//只纸币找零
                   case (byte)EC_CHANGE_TYPE.ALL_CHANGE_TYPE:		//纸硬币找零
                       bflag = true;
                       break;
                   default:
                       iModeNameId = 9;
                       bflag = true;
                       break;
               }
           }
           if (5 == iModeNameId)
           {
               switch (iChangeType.ToByte())
               {
                   case (byte)EC_CHANGE_TYPE.NONE_CHANGE_TYPE:		//无找零
                   case (byte)EC_CHANGE_TYPE.COIN_CHANGE_ONLY:		//只硬币找零	
                   case (byte)EC_CHANGE_TYPE.ALL_CHANGE_TYPE:		//纸硬币找零		
                       bflag = true;
                       break;
                   case (byte)EC_CHANGE_TYPE.BILL_CHANGE_ONLY:		//只纸币找零		
                       iModeNameId = 8;
                       bflag = true;
                       break;
                   default:
                       iModeNameId = 9;
                       bflag = true;
                       break;
               }
           }  
            //return TVM_WORK_MODE_NAME[iModeNameId].ToString();
           if (iModeNameId == 1 || iModeNameId == 5 || iModeNameId == 8)
           {
               return "正常服务";
           }
           else if (iModeNameId == 9 || iModeNameId == 10)
           {
               return "暂停服务";
           }
           else return "受限服务";
        }


        /// <summary>
        /// 根据CSSID，CSSValue得到基础的状态信息
        /// </summary>
        /// <param name="cssId">CSSID</param>
        /// <param name="cssValue">CSSValue</param>
        /// <returns>返回基本的状态信息</returns>
        public BasiStatusIdInfo GetBasiStatusIdInfoByCssId(string cssId, string cssValue)
        {
           string cmd=string.Format("select * from basi_status_id_info t where t.css_status_id='{0}' and t.css_status_value='{1}'",cssId,cssValue);
           return DBCommon.Instance.GetModelValue<BasiStatusIdInfo>(cmd);
        }

        /// <summary>
        /// 根据CSSID得到基本状态信息集合
        /// </summary>
        /// <param name="cssId">CSSID</param>
        /// <returns>根据CSSID得到基本状态信息列表</returns>
        public List<BasiStatusIdInfo> GetAllBasiStatusIdInfoByCssId(string cssId)
        {
            string cmd = string.Format("select * from basi_status_id_info t where t.css_status_id='{0}' ", cssId);
            return DBCommon.Instance.GetTModelValue<BasiStatusIdInfo>(cmd);
        }

        /// <summary>
        /// 返回该状态的中文名称
        /// </summary>
        /// <param name="info">基本的状态信息</param>
        /// <returns>返回</returns>
        public string GetStatusValueCNName(BasiStatusIdInfo info)
        {
            if(info==null||
              string.IsNullOrEmpty(info.css_status_id))
            {
                WriteLog.Log_Error("params error!");
                return null;
            }
            if (SysConfig.GetSysConfig().LocalParamsConfig.StatusValueName == 0)
            {
                return info.css_status_value_name;
            }
            else
                return info.status_type_name;
        }


        public DevRunStatusDetail GetDevRunStatusDetail(string stationId,string devId,string cssId)
        {
            string cmd = string.Format("select * from dev_run_status_detail t where t.station_id='{0}' and t.device_id='{1}' and t.status_id='{2}'",
                stationId, devId, cssId);
            return DBCommon.Instance.SetModelValue<DevRunStatusDetail>(cmd);

        }

        //2013年5月30日根据业主要求取得Hopper数量及数量状态。
        public TickBoxStatusInfo GetHoppeStatusInfo(string stationId, string devId, string cssId)
        {
            string cmd = "";
            if (cssId == "1A09")
            {
                cmd = string.Format("select * from tick_box_status_info t where t.station_id='{0}' and t.ticket_box_id='{1}'",
                stationId, stationId + devId.Substring(6,2) + "01");
            }
            else
            {
                cmd = string.Format("select * from tick_box_status_info t where t.station_id='{0}' and t.ticket_box_id='{1}'",
                stationId, stationId + devId.Substring(6, 2) + "02");
            }

            return DBCommon.Instance.SetModelValue<TickBoxStatusInfo>(cmd);

        }

        

    }
}
