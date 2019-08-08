using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.ParamsManager
{
    public class Para4045Added:IParamDataAdded
    {
        public int AddParamsData(string paraVersion)
        {
            ParaManager pm = new ParaManager();

            int res = pm.AddParamsData<Para4045BomMainLogin>(paraVersion, "para_4045_bom_main_login");
            if (res != 0)
                return res;

            pm.AddParamsData<Para4045BomTickBox>(paraVersion, "para_4045_bom_tick_box");
            if (res != 0)
                return res;

            pm.AddParamsData<Para4045MinTranQuery>(paraVersion, "para_4045_min_tran_query");
            if (res != 0)
                return res;

            pm.AddParamsData<Para4045TickReaderData>(paraVersion, "para_4045_tick_reader_data");
            if (res != 0)
                return res;

            return 0;

        }
    }
}
