using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.ParamsManager
{
    public class Para4042Added:IParamDataAdded
    {
        public int AddParamsData(string paraVersion)
        {
            ParaManager pm = new ParaManager();

            int res = pm.AddParamsData<Para4042DeviceInfo>(paraVersion, "para_4042_device_info");
            if (res != 0)
                return res;

            return 0;

        }




    }
}
