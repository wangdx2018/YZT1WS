using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.ParamsManager
{

    /// <summary>
    /// added by 
    /// 
    /// 复制AGM参数到草稿版
    /// </summary>
    public partial class Param4044Added : IParamDataAdded
    {
         /// <summary>
        /// 复制AGM参数到草稿版
        /// </summary>
        /// <param name="AddParamsData">功能ID</param>
        /// <param name="paraVersion">功能名字</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddParamsData(string paraVersion)
        {
            //

            ParaManager pm = new ParaManager();

            //Para4044AgmTickBox aa = new Para4044AgmTickBox();
           int res= pm.AddParamsData<Para4044AgmTickBox>(paraVersion, "para_4044_agm_tick_box");
           if (res != 0)
               return res;

           res = pm.AddParamsData<Para4044AgmTickRw>(paraVersion, "para_4044_agm_tick_rw");
           if (res != 0)
               return res;

           res = pm.AddParamsData<Para4044AlarmLampData>(paraVersion, "para_4044_alarm_lamp_data");
           if (res != 0)
               return res;

           res = pm.AddParamsData<Para4044CustomAlarmLamp>(paraVersion, "para_4044_custom_alarm_lamp");
           if (res != 0)
               return res;

           res = pm.AddParamsData<Para4044MainLogin>(paraVersion, "para_4044_main_login");
           if (res != 0)
               return res;

           res = pm.AddParamsData<Para4044MinTranQuery>(paraVersion, "para_4044_min_tran_query");
           if (res != 0)
               return res;

           res = pm.AddParamsData<Para4044PassControlData>(paraVersion, "para_4044_pass_control_data");
           if (res != 0)
               return res;


         

            //int res = 0;
            //string cmd = "";
            //cmd = string.Format("select t.* from para_4044_agm_tick_box t where t.para_version='{0}'", paraVersion);
            //Para4044AgmTickBox infotbox = DBCommon.Instance.GetModelValue<Para4044AgmTickBox>(cmd);
            //if (infotbox.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infotbox.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infotbox, "para_4044_agm_tick_box");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            //cmd = string.Format("select t.* from para_4044_agm_tick_rw t where t.para_version='{0}'", paraVersion);
            //Para4044AgmTickRw infotrw = DBCommon.Instance.GetModelValue<Para4044AgmTickRw>(cmd);
            //if (infotrw.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infotrw.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infotrw, "para_4044_agm_tick_rw");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            //cmd = string.Format("select t.* from para_4044_alarm_lamp_data t where t.para_version='{0}'", paraVersion);
            //Para4044AlarmLampData infotldata = DBCommon.Instance.GetModelValue<Para4044AlarmLampData>(cmd);
            //if (infotldata.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infotldata.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infotldata, "para_4044_alarm_lamp_data");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            //cmd = string.Format("select t.* from para_4044_custom_alarm_lamp t where t.para_version='{0}'", paraVersion);
            //Para4044CustomAlarmLamp infolamp = DBCommon.Instance.GetModelValue<Para4044CustomAlarmLamp>(cmd);
            //if (infolamp.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infolamp.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infolamp, "para_4044_custom_alarm_lamp");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            //cmd = string.Format("select t.* from para_4044_main_login t where t.para_version='{0}'", paraVersion);
            //Para4044MainLogin infologin = DBCommon.Instance.GetModelValue<Para4044MainLogin>(cmd);
            //if (infologin.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infologin.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infologin, "para_4044_main_login");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            //cmd = string.Format("select t.* from para_4044_min_tran_query t where t.para_version='{0}'", paraVersion);
            //Para4044MinTranQuery infoquery = DBCommon.Instance.GetModelValue<Para4044MinTranQuery>(cmd);
            //if (infoquery.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infoquery.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infoquery, "para_4044_min_tran_query");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            //cmd = string.Format("select t.* from para_4044_pass_control_data t where t.para_version='{0}'", paraVersion);
            //Para4044PassControlData infocdata = DBCommon.Instance.GetModelValue<Para4044PassControlData>(cmd);
            //if (infocdata.para_version != paraVersion)
            //{
            //    return -1;
            //}
            //else
            //{
            //    infocdata.para_version = "-1";
            //}

            //try
            //{
            //    res = DBCommon.Instance.InsertTable(infocdata, "para_4044_pass_control_data");
            //    if (res != 1)
            //    {
            //        return -1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return -1;
            //}

            return 0;
        }       
    }
}
