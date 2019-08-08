using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AFC.WS.BR.SLEMonitorManager
{
    public enum EC_PAY_TYPE : byte
    {
	    NONE_PAY_TYPE		= 0x00,//无支付方式
	    CARD_ONLY			= 0x01,//仅充值卡
	    COIN_ONLY			= 0x02,//只硬币收入
	    COIN_AND_CARD		= 0x03,//硬币和充值卡
	    BILL_ONLY			= 0x04,//只纸币收入
	    BILL_AND_CARD		= 0x05,//纸币和充值卡
	    BILL_AND_COIN		= 0x06,//硬币纸币
	    ALL_PAY_TYPE		= 0x07,//所有支付方式
    };

    //交易模式
    public enum EC_TRANS_TYPE : byte
    {
	    NONE_TRANS_TYPE			= 0x00,//无交易方式
	    CHARGE_ONLY				= 0x01,//只充值
	    CASH_SALE_ONLY			= 0x02,//只现金售票
	    CARD_SALE_ONLY			= 0x03,//只充值卡售票
	    CASH_AND_CARD_SALE		= 0x04,//现金及充值卡售票
	    CHARGE_AND_CARD_SALE	= 0x05,//充值及充值卡售票
	    CHARGE_AND_CASH_SALE	= 0x06,//充值和现金售票
	    ALL_TRANS_TYPE			= 0x07,//所有交易方式
    };

    //找零模式
    public enum EC_CHANGE_TYPE : byte
    {
	    NONE_CHANGE_TYPE		= 0x00,//无找零
	    COIN_CHANGE_ONLY		= 0x01,//只硬币找零
	    BILL_CHANGE_ONLY		= 0x02,//只纸币找零
	    ALL_CHANGE_TYPE			= 0x03,//纸硬币找零
    };


}
