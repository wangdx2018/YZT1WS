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
using AFC.BOM2.UIController;
using AFC.WS.BR;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// AgUpsInfoAdd.xaml 的交互逻辑
    /// </summary>
    public partial class AgUpsInfoAdd : UserControlBase
    {
        public AgUpsInfoAdd()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            List<AFC.WS.UI.Common.QueryCondition> list = this.Tag as List<AFC.WS.UI.Common.QueryCondition>;

            System.Windows.Window window = list.Single(temp => temp.bindingData.Equals("window")).value as System.Windows.Window;

            window.Title = "新增Ag UPS 信息";
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\LogQuery\ui_addAgUpsInfo.xml");
           
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }

            //todo init text box
           TextBoxExtend tb= this.ic.GetCommonControlByName("btn_ups_id") as TextBoxExtend;
           string upsId = (BuinessRule.GetInstace().GetMaxUpsId().ConvertNumberStringToUint() + 1).ToString("d2");
           tb.Text = upsId;


            //todo inti combox

           ComboBoxExtend cb = this.ic.GetCommonControlByName("btn_device_id") as ComboBoxExtend;

           cb.SqlContent = string.Format("select * from basi_dev_info a where a.device_id not in (select t.device_id from dev_ups_map t) and a.device_id not in (select t.device_id from dev_ups_status t) and a.device_type = '06' and a.station_id = '{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            cb.BindType = BindType.SqlBindData;
            cb.BindDisplayField = "device_id";
            cb.BindHideField = "device_id";
            cb.Initialize();

            
            
        }

        public class from
        {
        }

        public override void InitlizeCompleteDone()
        {
            //string upsId = (BuinessRule.GetInstace().GetDevUpsStatus().ups_id.ToString().ConvertNumberStringToUint() + 1).ToString("d2");
            //Util.Instance.SetInitQuery("btn_ups_id", upsId, "btnAddOperator", ic);
        }
    }
}
