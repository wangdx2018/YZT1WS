using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.BJComm.Data;

namespace AFC.WS.Model.DB
{
    
    /// <summary>
    /// 领用归还数据结构。
    /// </summary>
    public class MoneyBoxInOrOutBody
    {
        List<MoneyTypeCodeInfo> _Border = new List<MoneyTypeCodeInfo>();
        /// <summary>
        /// 钱箱记录体：记录每个钱箱的详细信息
        /// </summary>
        public List<MoneyTypeCodeInfo> Body
        {
            get { return _Border; }
            set { _Border = value; }
        }
    }
    /// <summary>
    /// 币种种类代码
    /// </summary>
    public class MoneyTypeCodeInfo
    {
        /// <summary>
        /// 金额
        /// </summary>
        int _Cash;
        /// <summary>
        /// 钱箱类型
        /// </summary>
        string _MoneyTypeCode;

        /// <summary>
        /// 币种代码	1	HEX	
        /// </summary>
        public string MoneyTypeCode
        {
            get { return _MoneyTypeCode; }
            set { _MoneyTypeCode = value; }
        }
        /// <summary>
        /// 金额  钱币数量	2	HEX	
        /// </summary>
        public int Cash
        {
            get { return _Cash; }
            set { _Cash = value; }
        }
    }
}
