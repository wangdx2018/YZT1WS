using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.ParamsManager
{
    public class Para4314Added:IParamDataAdded
    {
        public int AddParamsData(string paraVersion)
        {
            ParaManager pm = new ParaManager();

            int res = pm.AddParamsData<Para4314AutorunTime>(paraVersion, "para_4314_autorun_time");
            if (res != 0)
                return res;

            return 0;
        }
    }
}
