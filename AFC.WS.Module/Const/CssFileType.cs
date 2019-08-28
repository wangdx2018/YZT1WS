using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    /// <summary>
    /// 参数文件类型，参数AFCEX.x文件
    /// </summary>
    public enum CssFileType_t
    {
        CssMT_YptTransaction = 0x2001,/*一票通交易文件      */
        CssMT_YktTransaction = 0x2002,/*一卡通交易文件      */
        CssMT_SleFinance = 0x2003,/*设备收益文件        */
        CssMT_SleAuditRegister = 0x2004,/*设备审计文件        */
        CssMT_SleDeviceEvent = 0x3001,/*设备事件文件        */
        CssMT_EodMasterControl = 0x4001,/*参数控制文件        */
        CssMT_RunMasterControl = 0x4002,/*运营配置控制文件    */
        CssMT_YktBlacklist = 0x4011,/*一卡通黑名单文件    */
        CssMT_YptAllBlackList = 0x4012,/*一票通黑名单全量文件*/
        CssMT_YptAddBlackList = 0x4013,/*一票通增量黑名单文件*/
        CssMT_YptRangeBlackList = 0x4014,/*一票通号段黑名单    */
        CssMT_StaffBlackList = 0x4015,/*员工卡黑名单        */
        CssMT_ModeHistoryList = 0x4021,/*模式履历            */
        CssMT_EodSystemControl = 0x4031,/*系统控制参数文件    */
        CssMT_EodTopo = 0x4032,/*路网信息参数文件    */
        CssMT_EodFareMediaType = 0x4033,/*车票参数参数文件    */
        CssMT_EodDiscount = 0x4034,/*优惠方案参数文件    */
        CssMT_EodCalendar = 0x4035,/*日历时间段参数文件  */
        CssMT_EodFare = 0x4036,/*费率参数文件        */
        CssMT_EodTvmUi = 0x4037,/*TVM界面参数文件     */
        CssMT_EodTrainSchedule = 0x4038,/*列车首末班车时刻文件*/
        CssMT_EodPriviledge = 0x4041,/*权限参数文件        */
        CssMT_EodStationLayout = 0x4042,/*车站设备节点配置文件*/
        CssMT_EodTvmRunConfig = 0x4043,/*TVM运营参数文件     */
        CssMT_EodAgmRunConfig = 0x4044,/*AGM运营参数文件     */
        CssMT_EodBomRunConfig = 0x4045,/*BOM运营参数文件     */
        CssMT_SleSoftware = 0x4091,/*设备软件            */
        CssMT_TicketStockSnapshot = 0x5001,/*库存快照文件        */
        CssMT_FtpAudit = 0x6001,/*文件对帐审计        */
        CssMT_AccSettleTxResult = 0x6002,/*交易对帐文件        */
        CssMT_AccSettleTxAdjust = 0x6003,/*可疑交易调整文件    */
        CssMT_AccSettleClear = 0x6004,/*清分结果文件        */
        CssMT_LcEodMasterControl = 0x4300,/*LC主控参数文件*/
        CssMT_TVMSoftware = 0x4301,/*TVM设备软件          */
        CssMT_BOMSoftware = 0x4302,/*BOM设备软件          */
        CssMT_AGSoftware = 0x4303,/*AG设备软件          */
        CssMT_EQMSoftware = 0x4304,/*EQM设备软件          */
        CssMT_Tpu1Software = 0x4305,/*TPU软件          */
        CssMT_Tpu2Software = 0x4306,/*TPU软件          */
        CssMT_SCWSSoftware = 0x4310,/*SCWS软件          */
        CssMT_LCWSSoftware = 0x4311,   /*LCWS软件          */


        CssMT_StationCfs = 0x0206,   /*车站配置文件          */
    };
}
