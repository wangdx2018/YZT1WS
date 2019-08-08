using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AFC.WS.UI.UIPage.SLEMonitor
{
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using System.Data;
    /// <summary>
    /// BoxManager.xaml 的交互逻辑
    /// </summary>
    public partial class BoxManager : UserControl
    {
        public BoxManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        private BasiDevInfo devInfo = null;

  

        DevStatus devStatus = new DevStatus();

        /// <summary>
        /// 箱子类型
        /// </summary>
        private byte boxType;

     

        public void Init(BasiDevInfo info, byte boxType)
        {
            this.devInfo = info;
            this.boxType = boxType;
           

            if (boxType == 0 &&
                devInfo.device_type == DevType.DEV_AGM)
            {
              StackPanel stackPanel=CreateAGTickBox();
              sp.Children.Clear();
              sp.Children.Add(stackPanel);
            }
            if (boxType == 0 &&
                devInfo.device_type == DevType.DEV_TVM)
            {
                StackPanel stackPanel = CreateTVMTickBox();
                sp.Children.Clear();
                sp.Children.Add(stackPanel);
            }
            if (boxType == 1 &&
                devInfo.device_type == DevType.DEV_TVM)
            {
                StackPanel stackPanel = CreateTVMCashBox();
                sp.Children.Clear();
                sp.Children.Add(stackPanel);
            }
            if (boxType == 2 &&
             devInfo.device_type == DevType.DEV_TVM)
            {
                StackPanel stackPanel = CreateHopper();
                sp.Children.Clear();
                sp.Children.Add(stackPanel);
            }
                
        }

        /// 修改人：吴萌 
        /// 修订记录：20130313  票箱库存状态从dev_run_status_detail表中对应位置状态ID的status_value中得到
        /// 之前是从tick_box_in_dev_info中的tick_store_status中取得，由于服务器在开始运营时操作此字段会造成消息堵塞，所以服务器不对此值进行处理，因此修改　
        /// 如果查询到的票箱的“installStatus”值取到的结果为卸下，则此票箱不显示
        /// <summary>
        /// 得到票箱的状态信息
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <param name="position">位置编码</param>
        public void GetTickBoxInfo(string deviceId,string position,out string tickBoxId,out string tickStoreStatus,out string installStatus,out string tickBoxNum)
        {
            string tds = string.Format("select t.ticket_box_id, " +
       " drs.status_value as tick_store_status, " +
       " tbd.install_status, " +
       " t.tickets_num " +
       " from tick_box_status_info t " +
       " left join tick_box_in_dev_info tbd on tbd.line_id = t.line_id " +
       " and tbd.station_id = t.station_id " +
       " and t.ticket_box_id = tbd.ticket_box_id " +
       " left join dev_run_status_detail drs on drs.device_id = tbd.device_id " +
       " and drs.status_id = '1A'||decode(substr(tbd.device_id,5,2)||tbd.position_in_dev,'0607','05','0608','06',tbd.position_in_dev)  where tbd.device_id = '{0}' and tbd.position_in_dev = '{1}' ", deviceId, position);
            DataTable dt = DBCommon.Instance.GetDatatable(tds);
            if (dt == null||dt.Rows.Count==0)
            {
                tickBoxId = "00000000";
                tickStoreStatus = "未安装";
                installStatus = "未安装";
                tickBoxNum = "0";
            }
            else
            {
                tickBoxId = dt.Rows[0][0].ToString();
                tickBoxNum = dt.Rows[0][3].ToString();
                tickStoreStatus=GetTickBoxStoreStatus(dt.Rows[0][1]!=null?dt.Rows[0][1].ToString():string.Empty);
                installStatus = GetTickBoxStatus(dt.Rows[0][2] != null ? dt.Rows[0][2].ToString() : string.Empty);
            }
    
        }

        /// 修改人：吴萌 
        /// 修订记录：20130313  钱箱库存状态从dev_run_status_detail表中对应位置状态ID的status_value中得到
        /// 之前是从cash_box_in_dev_info中的currency_store_status中取得，由于服务器在开始运营时操作此字段会造成消息堵塞，所以服务器不对此值进行处理，因此修改　
        /// 如果查询到的钱箱的“installStatus”值取到的结果为卸下，则此钱箱不显示
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devceId"></param>
        /// <param name="position"></param>
        /// <param name="boxId"></param>
        /// <param name="storeStatus"></param>
        /// <param name="installStatus"></param>
        /// <param name="totalNum"></param>
        /// <param name="cashCode"></param>
        /// <param name="totalValue"></param>
        public void GetCashBoxInfo(string devceId, string position, out string boxId, out string storeStatus, out string installStatus, out string totalNum, out string cashCode, out string totalValue)
        {
          string positionStatusId = GetCashBoxPositionStatusId(position);
          string cbs=  string.Format("select bmti.currency_name, " +
           " cbt.currency_num, " +
           " cbt.total_money_value, " +
           " cbt.money_box_id, " +
           " cbdi.install_status, " +
           " drs.status_value as currency_store_status " +
           " from cash_box_status_info cbt " +
           " inner join cash_box_in_dev_info cbdi on cbdi.line_id = cbt.line_id " +
           " and cbdi.station_id = cbt.station_id " +
           " and cbdi.money_box_id = cbt.money_box_id " +
           " left join basi_money_type_info bmti on bmti.currency_code = cbt.currency_code " +
           " left join dev_run_status_detail drs on drs.device_id = cbdi.device_id and drs.status_id = '{0}' " +
           " where cbdi.device_id='{1}' and cbdi.position_in_dev='{2}' ", positionStatusId, devceId, position);
          DataTable dt = DBCommon.Instance.GetDatatable(cbs);
          if (dt == null||dt.Rows.Count==0)
          {
              boxId = "00000000";
              storeStatus = "未安装";
              installStatus = "未安装";
              totalNum = "0";
              totalValue = "0";
              cashCode = "未获取";
          }
          else
          {
              boxId = dt.Rows[0][3].ToString();
              storeStatus = GetTickBoxStoreStatus(dt.Rows[0][5] != null ? dt.Rows[0][5].ToString() : string.Empty);
              installStatus = GetTickBoxStatus(dt.Rows[0][4] != null ? dt.Rows[0][4].ToString() : string.Empty);
              totalNum = dt.Rows[0][1] != null ? dt.Rows[0][1].ToString() : "0";
              totalValue = dt.Rows[0][2] != null ? (Convert.ToDouble(dt.Rows[0][2].ToString())/100).ToString() : "0";
              cashCode = dt.Rows[0][0] != null ? dt.Rows[0][0].ToString() : "N/A";
          }
        }

        private string GetTickBoxStatus(string installStatus)
        {
            switch (installStatus)
            {
                case "01":
                    return "正常安装";
                case "02":
                    return "非法安装";
                case "03":
                    return "正常卸下";
                case "04":
                    return "非法卸下";
                default:
                    return "未安装";
          
            }
        }

        /// 修改人：吴萌 
        /// 修订记录：20130313  增加通过位置信息找到dev_run_status_detail中对应的状态ID（status_id）的方法
        private string GetCashBoxPositionStatusId(string position)
        {
            switch (position)
            {
                case "01":
                    return "1E06";
                case "02":
                    return "1E01";
                case "03":
                    return "1C03";
                default:
                    return "0000";
          
            }
        }

        /// 修改人：吴萌 
        /// 修订记录：20130313  调整为basi_status_id_info表中对应位置状态ID（0A01,0A02,0A03,0A04）保持一致
        private string GetTickBoxStoreStatus(string storeStatus)
        {
            switch (storeStatus)
            {
                case "00":
                    return "正常";
                case "01":
                    return "已空";
                case "02":
                    return "将空";
                case "03":
                    return "已满";
                case "04":
                    return "将满";
                default:
                    return "未安装";
            }
        }

        private StackPanel CreateAGTickBox()
        {
            string tickBox1Id;
            string box1SetupStatus;
            string box1NumStatus;
            string box1Num;
            GetTickBoxInfo(devInfo.device_id, "07",out tickBox1Id,out box1NumStatus,out box1SetupStatus,out box1Num);
            BoxInfo info1= new BoxInfo(tickBox1Id, box1SetupStatus, box1NumStatus, box1Num);
            info1.Header = "回收箱1";

            string box2Id;
            string box2SetupStatus;
            string box2NumStatus;
            string box2Num; //todo: get tick num
            GetTickBoxInfo(devInfo.device_id, "08", out box2Id, out box2NumStatus, out box2SetupStatus, out box2Num);
            BoxInfo info2 = new BoxInfo(box2Id, box2SetupStatus, box2NumStatus, box2Num);
            info2.Header = "回收箱2";
            StackPanel sp = new StackPanel();
            sp.Children.Add(info1);
            sp.Children.Add(info2); 
            // 票箱1，票箱2
            return sp;
        }

        private StackPanel CreateHopper()
        {
            string hopper1NumStatus = "";
            string hopper1Num = "";
            GetHopperStatus("1A09", out hopper1NumStatus, out hopper1Num);
            HopperInfo hopperInfo1 = new HopperInfo(hopper1NumStatus, hopper1Num);
            hopperInfo1.Header = "Hopper1";

            string hopper2NumStatus = "";
            string hopper2Num = "";
            GetHopperStatus("1A0A", out hopper2NumStatus, out hopper2Num);
            HopperInfo hopperInfo2 = new HopperInfo(hopper2NumStatus, hopper2Num);
            hopperInfo2.Header = "Hopper2";

            StackPanel sp = new StackPanel();
            sp.Children.Add(hopperInfo1);
            sp.Children.Add(hopperInfo2);
            // 票箱1，票箱2
            return sp;
        }

        private string GetHopperStatus(string statusId, out string hopperNumStatus, out string hopperNum)
        {
            DevRunStatusDetail a = devStatus.GetDevRunStatusDetail(devInfo.device_id.Substring(0, 4),
                devInfo.device_id,
                statusId);
            BasiStatusIdInfo info = devStatus.GetBasiStatusIdInfoByCssId(a.status_id, a.status_value);
            TickBoxStatusInfo b = devStatus.GetHoppeStatusInfo(devInfo.device_id.Substring(0, 4),
            devInfo.device_id,
            statusId);
            if (string.IsNullOrEmpty(b.tickets_num.ToString()))
                hopperNum = "N/A";
            else
                hopperNum = b.tickets_num.ToString();
            hopperNumStatus = devStatus.GetStatusValueCNName(info);
            if (string.IsNullOrEmpty(hopperNumStatus))
                return "N/A";
            else
                return hopperNumStatus;
        }

        private string GetStatus(string statusId)
        {
            DevRunStatusDetail a = devStatus.GetDevRunStatusDetail(devInfo.device_id.Substring(0,4),
                devInfo.device_id,
                statusId);
           BasiStatusIdInfo info=devStatus.GetBasiStatusIdInfoByCssId(a.status_id, a.status_value);
           string value = devStatus.GetStatusValueCNName(info);
           if (string.IsNullOrEmpty(value))
               return "N/A";
           else
               return value;
        }

        private StackPanel CreateTVMCashBox()
        {
            string box1SetupStatus;
            string box1NumStatus;
            string box1Num;
            string box1CashCode;
            string box1Id;
            string box1TotalValue;
            GetCashBoxInfo(devInfo.device_id, "01", out box1Id, out box1NumStatus, out box1SetupStatus, out box1Num, out box1CashCode, out box1TotalValue);
            BoxInfo info1 = new BoxInfo(box1Id, box1SetupStatus, box1NumStatus, box1Num, box1CashCode, box1TotalValue, Visibility.Visible);
            info1.Header = "纸币回收箱";

            string box2SetupStatus;
            string box2NumStatus;
            string box2Num;
            string box2CashCode;
            string box2Id;
            string box2TotalValue;
            GetCashBoxInfo(devInfo.device_id, "02", out box2Id, out box2NumStatus, out box2SetupStatus, out box2Num, out box2CashCode, out box2TotalValue);
            BoxInfo info2 = new BoxInfo(box2Id, box2SetupStatus, box2NumStatus, box2Num, box2CashCode, box2TotalValue, Visibility.Visible);
            info2.Header = "纸币补充箱";


            string box3SetupStatus;
            string box3NumStatus;
            string box3Num;
            string box3CashCode;
            string box3Id;
            string box3TotalValue;
            GetCashBoxInfo(devInfo.device_id, "03", out box3Id, out box3NumStatus, out box3SetupStatus, out box3Num, out box3CashCode, out box3TotalValue);
            BoxInfo info3 = new BoxInfo(box3Id, box3SetupStatus, box3NumStatus, box3Num, box3CashCode, box3TotalValue, Visibility.Visible);
            info3.Header = "硬币回收箱";


            StackPanel sp = new StackPanel();
            sp.Children.Add(info1);
            sp.Children.Add(info2);
            sp.Children.Add(info3);

            //硬币回收，纸币补充，纸币回收，
            return sp;
        }

        private StackPanel CreateTVMTickBox()
        {
            //票箱1，票箱2，回收箱1，废票箱
            //2013年5月7日增加hopper1,hopper2数量，及数量状态

            string tickBox1Id;
            string box1SetupStatus;
            string box1NumStatus;
            string box1Num;
            GetTickBoxInfo(devInfo.device_id, "01", out tickBox1Id, out box1NumStatus, out box1SetupStatus, out box1Num);
            BoxInfo info1 = new BoxInfo(tickBox1Id, box1SetupStatus, box1NumStatus, box1Num);
            info1.Header = "补充箱1";

            string tickBox2Id;
            string box2SetupStatus;
            string box2NumStatus;
            string box2Num;
            GetTickBoxInfo(devInfo.device_id, "02", out tickBox2Id, out box2NumStatus, out box2SetupStatus, out box2Num);
            BoxInfo info2 = new BoxInfo(tickBox2Id, box2SetupStatus, box2NumStatus, box2Num);
            info2.Header = "补充箱2";


            string tickBox3Id;
            string box3SetupStatus;
            string box3NumStatus;
            string box3Num;
            GetTickBoxInfo(devInfo.device_id, "03", out tickBox3Id, out box3NumStatus, out box3SetupStatus, out box3Num);
            BoxInfo info3 = new BoxInfo(tickBox3Id, box3SetupStatus, box3NumStatus, box3Num);
            info3.Header = "废票箱";


            string tickBox4Id;
            string box4SetupStatus;
            string box4NumStatus;
            string box4Num;
            GetTickBoxInfo(devInfo.device_id, "04", out tickBox4Id, out box4NumStatus, out box4SetupStatus, out box4Num);
            BoxInfo info4 = new BoxInfo(tickBox4Id, box4SetupStatus, box4NumStatus, box4Num);
            info4.Header = "回收箱"; 

            StackPanel sp = new StackPanel();
            sp.Children.Add(info1);
            sp.Children.Add(info2);
            sp.Children.Add(info3);
            sp.Children.Add(info4);
     
            return sp;
        }

     
    }


  
}
