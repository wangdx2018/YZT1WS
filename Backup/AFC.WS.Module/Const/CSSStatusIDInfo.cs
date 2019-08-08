using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Const
{
    public class CSSStatusIDInfo
    {
        /// <summary>
        /// 服务模式
        /// 0x00-设备正常服务             
        ///0x01-设备警告服务              
        ///0x02-设备暂停服务              
        ///0x03-设备故障关闭              
        ///0x04-设备维修关闭              
        ///0x05-设备睡眠模式              
        ///0x06-设备停止服务（运营结束）       
        ///0x07-设备紧急服务
        /// </summary>
        public const string SERVER_MODE = "0101";

        /// <summary>
        /// 参数生效状态
        /// 正常：00，异常：02
        /// </summary>
        public const string PARAM_EFFECTIVE_STATUS = "0201";

        /// <summary>
        /// 参数下载状态
        /// 正常：00，异常：02
        /// </summary>
        public const string PARAM_DOWN_LOAD_STATUS = "0301";




        /// <summary>
        /// 温度越界 正常：00，异常: 02
        /// </summary>
        public const string TEMPEPERTURE_OVER = "0901";

        /// <summary>
        /// 0x00-运营开始成功 
        ///0x01-运营开始中   
        ///0x02-运营开始失败 
        ///0x03-运营结束成功 
        ///0x04-运营结束中 
        ///0x05-运营结束失败
        /// </summary>
        public const string RUN_STATUS = "0A01";

  

        /// <summary>
        /// 通讯控制板状态0x00－正常   0x02－错误
        /// </summary>
        public const string COMM_CONTROL_BOARD_STATUS = "0D01";

        /// <summary>
        /// 模式代码
        /// 取值	定义描述
        ///0	正常模式（00000000B）
        ///1	列车故障模式（00000001B）
        ///2	乘车时间免检模式（00000010B）
        ///4	车票日期免检模式（00000100B）
        ///8	车票免检模式（00001000B）
        ///16	进出站次序免检模式（00010000B）
        ///32	大客流进入模式（00100000B）
        ///64	预赋值模式（01000000B）
        ///128	紧急模式（10000000B）
        /// </summary>
        public const string MODE_STATUS = "0E01";

        /// <summary>
        /// 24小时运营
        /// 0:24小时运营，1：非24小时运营
        /// </summary>
        public const string RUN_24_Mode = "0F01";


        /// <summary>
        /// 延长运营
        /// 0：延长，2：非延长
        /// </summary>
        public const string EXTEND_RUN_MODE = "1001";

        /// <summary>
        /// 连接状态
        /// 00：通讯中断
        /// 01：通讯正常
        /// 02：通讯恢复中
        /// </summary>
        public const string ON_LINE_STATUS = "1101";

        /// <summary>
        /// 时钟异常
        /// 00：正常
        /// 01：警告
        /// 02：故障
        /// </summary>
        public const string TIME_CLOCK_EXCEPTION = "1201";

        /// <summary>
        /// 终端登录状态
        /// 00：有人登录,登录成功后的状态  
        /// 01：无人登录,退出登录后的状态 
        /// 02：账户锁定,账户锁定后的状态，有人登录成功后状态改变
        /// </summary>
        public const string DEV_LOG_IN_STATUS = "1301";

        /// <summary>
        /// 设备故障状态
        /// 00：无故障
        /// 01：有故障
        /// </summary>
        public const string DEV_FAILED_STATUS = "1401";

        /// <summary>
        /// 传输审计状态   
        ///0x00-传输审计状态异常  
        ///0x01-传输审计状态正常  
        ///0x02-日传输审计结束 
        /// </summary>
        public const string AR_STAUTS = "1501";

        /// <summary>
        /// SC自动运行状态
        /// 00：车站自动运行状态停用
        /// 01：车站自动运行状态启用
        /// </summary>
        public const string SC_AUTO_RUN_STATUS = "1600";

        /// <summary>
        /// 00:运营结束
        /// 01:运营中
        /// 02：运营开始
        /// </summary>
        public const string SC_RUN_MODE_STATUS = "1601";

        /// <summary>
        /// 00:运营结束
        /// 01:运营中
        /// 02：运营开始
        /// </summary>
        public const string LC_RUN_MODE_STATUS = "1701";

        /// <summary>
        /// 系统硬盘状态
        /// 00：系统磁盘空间良好
        /// 01：系统磁盘空间正常
        /// 02：系统磁盘空间不足
        /// 03：系统磁盘空间严重不足
        /// </summary>
        public const string SYS_HARD_DISK_STATUS = "1801";


        /// <summary>
        /// 系统数据库磁盘空间状态
        /// 00：良好
        /// 01：正常
        /// 02：磁盘空间不足
        /// </summary>
        public const string SYS_DB_DISK_STATUS = "1802";


        /// <summary>
        /// CPU状态
        /// 00:良好
        /// 01：正常
        /// 02：利用率偏高
        /// 03：CPU忙
        /// </summary>
        public const string SYS_CUP_RUN_STATUS = "1803";


        /// <summary>
        /// 内存状态
        /// 00:良好
        /// 01：正常
        /// 02：利用率偏高
        /// 03：CPU忙
        /// </summary>
        public const string SYS_MEMORY_STATUS = "1804";


        /// <summary>
        /// 00：正常
        /// 01：警告
        /// 02：故障
        /// </summary>
        public const string TICK_BOX_STATUS = "1900";


        /// <summary>
        /// 票箱1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_1_SETUP_STATUS = "1901";



        /// <summary>
        /// 票箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_2_SETUP_STATUS = "1902";




        /// <summary>
        /// 票箱3安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_3_SETUP_STATUS = "1903";



        /// <summary>
        /// 票箱4安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_4_SETUP_STATUS = "1904";




        /// <summary>
        /// 回收箱1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_RECYCLING_1_SETUP_STATUS = "1905";


        /// <summary>
        /// 回收箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_RECYCLING_2_SETUP_STATUS = "1906";


        /// <summary>
        /// 废票箱1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_WASTE_1_SETUP_STATUS = "1907";



        /// <summary>
        /// 废票箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string TICK_BOX_WASTE_2_SETUP_STATUS = "1908";


        /// <summary>
        /// 票箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_1_STAUT = "1A01";



        /// <summary>
        /// 票箱2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_2_STAUT = "1A02";

        /// <summary>
        /// 票箱3状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_3_STAUT = "1A03";


        /// <summary>
        /// 票箱4状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_4_STAUT = "1A04";



        /// <summary>
        /// 回收箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_RECYCLING_1_STAUT = "1A05";



        /// <summary>
        /// 回收箱2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_RECYCLING_2_STAUT = "1A06";


        /// <summary>
        /// 废票箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_WASTE_1_STAUT = "1A07";

        /// <summary>
        /// 废票箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string TICK_BOX_WASTE_2_STAUT = "1A08";



        /// <summary>
        /// 发行单元Hopper1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string ICU_HOPPER_1_STAUS = "1A09";


        /// <summary>
        /// 发行单元Hopper2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string ICU_HOPPER_2_STAUS = "1A0A";


        /// <summary>
        /// 设备钱箱状态
        /// 00：正常
        /// 01：警告
        /// 02：故障
        /// </summary>
        public const string CASH_BOX_TOTAL_STATUS = "1B00";



        /// <summary>
        /// 硬币找零箱安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string COIN_CASH_BOX_1_SETUP_STAUTS = "1B01";



        /// <summary>
        /// 硬币找零箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string COIN_CASH_BOX_2_SETUP_STAUTS = "1B02";


        /// <summary>
        /// 硬币回收箱1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string COIN_CASH_BOX_RECYCLING_1_SETUP_STAUTS = "1B03";


        /// <summary>
        /// 硬币回收箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string COIN_CASH_BOX_RECYCLING_2_SETUP_STAUTS = "1B04";



        /// <summary>
        /// 硬币Hopper1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string COIN_HOPPER_1_SETUP_STAUTS = "1B05";


        /// <summary>
        /// 硬币Hopper2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string COIN_HOPPER_2_SETUP_STAUTS = "1B06";


        /// <summary>
        /// 硬币钱箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string COIN_CASH_BOX_1_STATUS = "1C01";


        /// <summary>
        /// 硬币钱箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string COIN_CASH_BOX_2_STATUS = "1C02";



        /// <summary>
        /// 硬币回收箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string COIN_CASH_BOX_RECYCLING_1_STATUS = "1C03";


        /// <summary>
        /// 硬币回收箱2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string COIN_CASH_BOX_RECYCLING_2_STATUS = "1C04";

        /// <summary>
        /// 硬币Hopper1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string COIN_HOPPER_1_STATUS = "1C05";

        /// <summary>
        /// 硬币Hopper2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string COIN_HOPPER_2_STATUS = "1C06";


        /// <summary>
        /// 设备纸币钱箱状态
        /// 00:正常
        /// 01:警告
        /// 02:故障
        /// </summary>
        public const string DEV_BILL_BOX_STATUS = "1D00";

        /// <summary>
        /// 纸币钱箱1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_1_SETUP_STATUS = "1D01";

        /// <summary>
        /// 纸币钱箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_2_SETUP_STATUS = "1D02";

        /// <summary>
        /// 纸币钱箱3安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_3_SETUP_STATUS = "1D03";

        /// <summary>
        /// 纸币钱箱4安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_4_SETUP_STATUS = "1D04";

        /// <summary>
        /// 纸币废币安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_WASTE_SETUP_STATUS = "1D05";

        /// <summary>
        /// 纸币回收箱1安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_RECYCLING_1_SETUP_STATUS = "1D06";

        /// <summary>
        /// 纸币回收箱2安装状态
        /// 01:正常安装
        /// 02:非法安装
        /// 03:正常卸下
        /// 04:非法卸下
        /// </summary>
        public const string BILL_CASH_BOX_RECYCLING_2_SETUP_STATUS = "1D07";


        /// <summary>
        /// 纸币钱箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_1_STATUS = "1E01";

        /// <summary>
        /// 纸币钱箱2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_2_STATUS = "1E02";

        /// <summary>
        /// 纸币钱箱3状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_3_STATUS = "1E03";

        /// <summary>
        /// 纸币钱箱4状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_4_STATUS = "1E04";

        /// <summary>
        /// 纸币废钱箱状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_WASTE_STATUS = "1E05";

        /// <summary>
        /// 纸币回收箱1状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_RECYCLING_1_STATUS = "1E06";

        /// <summary>
        /// 纸币回收箱2状态
        /// 00：正常
        /// 01：已空
        /// 02：将空
        /// 03：已满
        /// 04：将满
        /// </summary>
        public const string BILL_CASH_BOX_RECYCLING_2_STATUS = "1E07";


        /// <summary>
        /// 现金安全门
        /// 00：正常关闭
        /// 01：正常打开
        /// 02：非法打开
        /// </summary>
        public const string CASH_SAFE_DOOR_STATUS = "1F01";


        /// <summary>
        ///  招援按钮状态  
        /// 0x00-招援按钮正常 
        ///0x01-招援按钮按下 
        ///0x02-招援按钮故障 
        /// </summary>
        public const string HELP_BUTTON_STATUS = "2001";


        /// <summary>
        /// AG设备子类型
        /// "0x01-标准进站
        ///0x02-标准出站
        ///0x03-标准双向AG
        ///0x04-宽通道进站AG
        ///0x05-宽通道出站AG
        ///0x06-宽通道双向AG"
        /// </summary>
        public const string AG_DEV_SUB_TYPE = "2101";


        /// <summary>
        /// 闸门状态
        /// 0x00-闸门正常
        ///0x01-闸门故障
        ///0x02-乘客闯入闸门
        ///0x03-强制拉开闸门
        /// </summary>
        public const string AG_DOOR_STATUS = "2201";


        /// <summary>
        /// 乘客检测 
        /// 0x00-乘客检测关闭
        ///0x01-乘客检测开启
        /// </summary>
        public const string AG_PASSANGER_CHECK_STATUS = "2301";


        /// <summary>
        /// 传感器状态
        /// 0x00-传感器正常
        ///0x01-传感器故障
        /// </summary>
        public const string SENSOR_STATUS = "2401";


        /// <summary>
        /// 0x00 进站
        /// 
        /// 0x01 出站
        /// 
        /// 0x02 双向
        /// </summary>
        public const string AG_DEV_TYPE = "2601";


     

        /// <summary>
        /// 读写器1总体状态
        /// "0x00-读写器正常    
        ////0x01-读写器故障    
        ////0x02-读写器丢失    
        ////0x03-读写器SAM错误 "
        /// </summary>
        public const string RW1_TOTAL_STATUS = "2A01";

        /// <summary>
        /// 读卡器1通信状态
        /// 00:正常
        /// 01：故障
        /// </summary>
        public const string RW1_COMM_STATUS = "2A02";

        /// <summary>
        /// 读卡器1SAM卡状态
        /// 00:正常
        /// 其他：出错
        /// </summary>
        public const string RW1_SAM_STATUS = "2A03";

        /// <summary>
        /// TPU软件更新
        /// 00:成功
        /// 02：失败
        /// </summary>
        public const string RW1_TPU_SOFTWARE_UPDATE_STATUS = "2A04";

        /// <summary>
        /// 读写器1参数更新状态
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW1_PARAM_UPDATE_STATUS = "2A05";

        /// <summary>
        /// 读写器1黑名单更新
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW1_PARAM_BLACKLIST_UPDATE_STATUS = "2A06";

        /// <summary>
        /// 读写器1模式履历
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW1_PARAM_MODE_HIS_UPDATE_STATUS = "2A07";



        /// <summary>
        /// 读写器2总体状态
        /// "0x00-读写器正常    
        ////0x01-读写器故障    
        ////0x02-读写器丢失    
        ////0x03-读写器SAM错误 "
        /// </summary>
        public const string RW2_TOTAL_STATUS = "2B01";

        /// <summary>
        /// 读卡器2通信状态
        /// 00:正常
        /// 01：故障
        /// </summary>
        public const string RW2_COMM_STATUS = "2B02";

        /// <summary>
        /// 读卡器2SAM卡状态
        /// 00:正常
        /// 其他：出错
        /// </summary>
        public const string RW2_SAM_STATUS = "2B03";

        /// <summary>
        /// TPU软件更新
        /// 00:成功
        /// 02：失败
        /// </summary>
        public const string RW2_TPU_SOFTWARE_UPDATE_STATUS = "2B04";

        /// <summary>
        /// 读写器2参数更新状态
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW2_PARAM_UPDATE_STATUS = "2B05";

        /// <summary>
        /// 读写器2黑名单更新
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW2_PARAM_BLACKLIST_UPDATE_STATUS = "2B06";

        /// <summary>
        /// 读写器2模式履历
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW2_PARAM_MODE_HIS_UPDATE_STATUS = "2B07";



        /// <summary>
        /// 读写器3总体状态
        /// "0x00-读写器正常    
        ////0x01-读写器故障    
        ////0x02-读写器丢失    
        ////0x03-读写器SAM错误 "
        /// </summary>
        public const string RW3_TOTAL_STATUS = "2C01";

        /// <summary>
        /// 读卡器3通信状态
        /// 00:正常
        /// 01：故障
        /// </summary>
        public const string RW3_COMM_STATUS = "2C02";

        /// <summary>
        /// 读卡器3SAM卡状态
        /// 00:正常
        /// 其他：出错
        /// </summary>
        public const string RW3_SAM_STATUS = "2C03";

        /// <summary>
        /// TPU软件更新
        /// 00:成功
        /// 02：失败
        /// </summary>
        public const string RW3_TPU_SOFTWARE_UPDATE_STATUS = "2C04";

        /// <summary>
        /// 读写器3参数更新状态
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW3_PARAM_UPDATE_STATUS = "2C05";

        /// <summary>
        /// 读写器3黑名单更新
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW3_PARAM_BLACKLIST_UPDATE_STATUS = "2C06";

        /// <summary>
        /// 读写器3模式履历
        /// 00:成功
        /// 02:失败
        /// </summary>
        public const string RW3_PARAM_MODE_HIS_UPDATE_STATUS = "2C07";


        /// <summary>
        /// 银行卡读写器总体状态 
        /// "0x00-读写器正常    
        ///0x01-读写器故障    
        ///0x02-读写器丢失    
        ///0x03-读写器SAM错误 
        /// </summary>
        public const string BANK_CARD_RW_TOTAL_STATUS = "2D00";


        /// <summary>
        /// 银行卡读写器通讯状态
        /// "0x00-正常
        ///0x02－通讯中断
        /// </summary>
        public const string BANK_CARD_RW_COMM_STATUS = "2D01";

        /// <summary>
        /// 银行卡读写器连续读写失败
        /// "0x00-正常
        ///0x02－错误
        /// </summary>
        public const string BANK_CARD_RW_CONTUNIOUS_RW_FAILED = "2D02";

        /// <summary>
        /// SAM卡1类型
        /// "0x01-一票通ISAM  
        ///0x02-一票通PSAM  
        ///0x03-一卡通ISAM  
        ///0x04-一卡通PSAM  "
        /// </summary>
        public const string SAM1_TYPE = "2E01";

        /// <summary>
        /// SAM卡2类型
        /// "0x01-一票通ISAM  
        ///0x02-一票通PSAM  
        ///0x03-一卡通ISAM  
        ///0x04-一卡通PSAM  "
        /// </summary>
        public const string SAM2_TYPE = "2E02";


        /// <summary>
        /// SAM卡3类型
        /// "0x01-一票通ISAM  
        ///0x02-一票通PSAM  
        ///0x03-一卡通ISAM  
        ///0x04-一卡通PSAM  "
        /// </summary>
        public const string SAM3_TYPE = "2E03";


        /// <summary>
        /// SAM卡4类型
        /// "0x01-一票通ISAM  
        ///0x02-一票通PSAM  
        ///0x03-一卡通ISAM  
        ///0x04-一卡通PSAM  "
        /// </summary>
        public const string SAM4_TYPE = "2E04";


        /// <summary>
        /// ISAM卡1状态
        /// 00：未验证
        /// 01：已验证
        /// </summary>
        public const string ISAM1_TYPE = "2F01";


        /// <summary>
        /// ISAM卡2状态
        /// 00：未验证
        /// 01：已验证
        /// </summary>
        public const string ISAM2_TYPE = "2F02";


        /// <summary>
        /// ISAM卡3状态
        /// 00：未验证
        /// 01：已验证
        /// </summary>
        public const string ISAM3_TYPE = "2F03";


        /// <summary>
        /// ISAM卡4状态
        /// 00：未验证
        /// 01：已验证
        /// </summary>
        public const string ISAM4_TYPE = "2F04";


        /// <summary>
        /// "打印机1总体状态
        /// "0x00-正常
        ///0x01－缺纸
        ///0x02－通讯故障"
        /// </summary>
        public const string PRINT1_TOTAL_STATUS = "3001";


        /// <summary>
        /// "打印机2总体状态
        /// "0x00-正常
        ///0x01－缺纸
        ///0x02－通讯故障"
        /// </summary>
        public const string PRINT2_TOTAL_STATUS = "3101";

        /// <summary>
        /// 电源状态 
        /// "0x00-电源正在上电
       ///0x01-电源断电    "
        /// </summary>
        public const string POWER_SUPPLY = "3301";

        /// <summary>
        /// 电池状态                     
        /// "0x00-电池正常   
       ///  0x01-电池故障  "
        /// </summary>
        public const string BATTORY_SUPPLY = "3401";


        /// <summary>
        /// 维修指示灯
        /// 0x00-维修指示灯正常    
        ///0x01-维修指示灯通讯故障
        ///0x02-维修指示灯断电    
        ///0x03-维修指示灯红      
        ///0x04-维修指示灯绿      
        ///0x05-维修指示灯灭      
        ///0x06-维修指示灯红色闪烁
        ///0x07-维修指示灯绿色闪烁
        /// </summary>
        public const string MAINTANCE_LIGHT_STATUS = "3501";


        /// <summary>
        /// 维修键盘状态           
        /// "0x00-维修键盘正常工作 
       ///0x01-维修键盘通信故障 
       ///0x02-维修键盘断电    "
        /// </summary>
        public const string MAINTANCE_KEYBOARD_STATUS = "3601";


        /// <summary>
        /// 维修门综合状态  
        /// "0x00-维修门正常打开     
        ///0x01-维修门正常关闭     
        ///0x02-维修门非法打开     
        ///0x03-维修门关闭未锁    "
        /// </summary>
        public const string MAINTANCE_DOOR_TOTAL_STATUS = "3701";



        /// <summary>
        /// 前维修门综合状态  
        /// "0x00-维修门正常打开     
        ///0x01-维修门正常关闭     
        ///0x02-维修门非法打开     
        ///0x03-维修门关闭未锁    "
        /// </summary>
        public const string MAINTANCE_FRONT_DOOR_TOTAL_STATUS = "3702";


        /// <summary>
        /// 后维修门综合状态  
        /// "0x00-维修门正常打开     
        ///0x01-维修门正常关闭     
        ///0x02-维修门非法打开     
        ///0x03-维修门关闭未锁    "
        /// </summary>
        public const string MAINTANCE_BACK_DOOR_TOTAL_STATUS = "3703";


        /// <summary>
        /// 侧维修门综合状态  
        /// "0x00-维修门正常打开     
        ///0x01-维修门正常关闭     
        ///0x02-维修门非法打开     
        ///0x03-维修门关闭未锁    "
        /// </summary>
        public const string MAINTANCE_SIDE_DOOR_TOTAL_STATUS = "3704";


        /// <summary>
        /// 纸币模块总体状态  
        /// "0 正常 
        ///1 警告
        ///2 报警"
        /// </summary>
        public const string BILL_MOUDLE_STATUS = "3801";


        /// <summary>
        /// 纸币模块接受状态
        /// "0 正常 
       ///2 通信终端"
        /// </summary>
        public const string BILL_RECEIVER_STATUS="3802";


        /// <summary>
        /// 纸币卡票
        /// "0 正常 
       ///2 通信终端"
        /// </summary>
        public const string BILL_JAM = "3803";

        /// <summary>
        /// 纸币找零通信
        /// "0 正常 
        ///2 通信终端"
        /// </summary>
        public const string BILL_CHANGE_COMM_STATUS = "3805";


        /// <summary>
        /// 票卡模块状态
        /// "0 正常 
        ///1 警告
        ///2 报警"

        /// </summary>
        public const string TICK_MOUDLE_TOTAL_STATUS = "3901";

        /// <summary>
        /// 发售模块通讯状态    
        /// "0 正常 
        ///2 错误"
        /// </summary>
        public const string TICK_SALE_MOUDLE_STATUS = "3902";

        /// <summary>
        /// 卡票   
        /// "0 正常 
        ///2 错误"
        /// </summary>
        public const string TICK_JAM = "3903";

        /// <summary>
        /// 硬币识别器状态 
        /// "0 正常 
        ///1 警告
        ///2 报警"
        /// </summary>
        public const string COIN_INDENTIFY_TOTAL_STATUS = "3A01";

        /// <summary>
        /// 卡币
        /// "0 正常 
        ///2 错误"
        /// </summary>
        public const string COIN_JAM = "3A02";

        /// <summary>
        /// 硬币补币锁被打开
        /// "0 正常 
       ///2 硬币补充锁打开"
        /// </summary>
        public const string COIN_ADD_LOCK_OPEN = "3A03";

     
        /// <summary>
        /// 售/补模式
        /// "0x00－售票模式
        ///0x01－补票模式
        ///0x02－售补票模式"
        /// </summary>
        public const string BOM_WORK_MODE = "3C03";

        /// <summary>
        /// 维修模式
       ///"0x00-正常
       ///0x01－进入维修模式"
        /// </summary>
        public const string MAINTANCE_MODE = "3C05";

        /// <summary>
        /// TVM工作模式
        /// 0x00－售充值模式
        ///0x01－仅售票模式
        ///0x02－仅储值卡售票
        ///0x03－仅充值模式
        /// </summary>
        public const string TVM_WORK_MODE = "3C07";

        /// <summary>
        /// EQM工作模式
        ///0x00-正常
        ///0x01－进入维修模式
        /// </summary>
        public const string EQM_WORK_MODE = "3C06";

        /// <summary>
            /// 支付模式
            /// "0x00－纸币、硬币、储值卡
            ///0x01－仅纸币、硬币
            ///0x02－仅硬币
            ///0x03－仅纸币
            ///0x04—仅储值卡"
            /// </summary>
        public const string TVM_PAMMENT_METHOD = "3C08";

        /// <summary>
        /// 找零模式
        /// 0x00—不找零（补充其他）
        /// </summary>
        public const string TVM_CHANGE_MODE = "3C09";

        /// <summary>
        /// "0x00-正常
        ///0x01－警告
        ///0x02－报警"
        /// </summary>
        public const string AG_SYS_SAFT_STATUS = "3D01";

        /// <summary>
        /// "0x00-正常
        ///0x01－安全性遭破坏或入侵"
        /// </summary>
        public const string SAFTY_STATUS = "3D02";

        /// <summary>
        /// "0x00-正常
       ///0x01－未初始化"
        /// </summary>
        public const string DEV_INIT_STATUS = "3D03";

        /// <summary>
        /// 0x00-正常
        ///0x01－编码/验证失败
        /// </summary>
        public const string DEV_SEC_CODING = "3D04";

        /// <summary>
        /// 使用的票的状态
        /// "0x00-正常
        ///0x02－使用黑名单上的票"
        /// </summary>
        public const string TICK_USING_STATUS = "3D05";
    }
}
