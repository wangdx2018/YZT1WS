using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
namespace AFC.WS.BR.ParamsManager
{
    public class Param4043Added:IParamDataAdded
    {
        public int AddParamsData(string paraVersion)
        {
            ParaManager pm = new ParaManager();

            int res = pm.AddParamsData<Para4043MaintainData>(paraVersion, "para_4043_maintain_data");
            if (res != 0)
                return res;

            res = pm.AddParamsData<Para4043MinQueryTranAmoun>(paraVersion,"para_4043_min_query_tran_amoun");
            if (res != 0)
                return res;

            res = pm.AddParamsData<Para4043TvmCashBox>(paraVersion, "para_4043_tvm_cash_box");
            if (res != 0)
                return res;

            res = pm.AddParamsData<Para4043TvmTickBox>(paraVersion, "para_4043_tvm_tick_box");
            if (res != 0)
                return res;

            res = pm.AddParamsData<Para4043TvmTickRead>(paraVersion, "para_4043_tvm_tick_read");
            if (res != 0)
                return res;

            return 0;

        }
    }
}
