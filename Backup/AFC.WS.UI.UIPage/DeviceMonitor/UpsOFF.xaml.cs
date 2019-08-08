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
    /// UpsOFF.xaml 的交互逻辑
    /// </summary>
    public partial class UpsOFF : UserControlBase
    {
        public UpsOFF()
        {
            InitializeComponent();
        }
        private string upsId;
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;

            System.Windows.Window window = list.Single(temp => temp.bindingData.Equals("window")).value as System.Windows.Window;
            window.Title = "发送关机命令";
            upsId = list.Single(temp => temp.bindingData.Equals("t.ups_id")).value.ToString();
            //if (list == null || list.Count == 0)
            //{
            //    MessageDialog.Show("请选择UPS", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //}
            if (list.Single(temp => temp.bindingData.Equals("t.power_status")).value.Equals("00") &&
                list.Single(temp => temp.bindingData.Equals("t.power_percent")).value.ToString().ToInt32()
                > BuinessRule.GetInstace().GetRunParamByCode().param_value.ToInt32())
            {
                MessageDialog.Show("该UPS不需要关机", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                window.Close();
                return;
            }

                DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\LogQuery\list_AgUpsMapQuery.xml");
                if (dlr != null)
                {
                    this.dataList.Initliaize(dlr);
                }
                IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_AgUpsMapQuery");
                if (dataSource != null)
                {
                    List<string> upsList = new List<string>();
                    upsList.Add(string.Format("t.ups_id='{0}'", upsId));
                    dataSource.SetQueryParams(upsList);
                }
        }
    }
}
