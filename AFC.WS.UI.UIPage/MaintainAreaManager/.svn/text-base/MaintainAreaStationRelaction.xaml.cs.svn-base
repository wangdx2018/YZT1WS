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
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIPage.MaintainAreaManager
{
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.BR;
    using AFC.WS.Model.DB;
    using System.Collections.ObjectModel;

    /// <summary>
    /// OpeatorStationRelaction.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainAreaStationRelaction : UserControlBase
    {
        public MaintainAreaStationRelaction()
        {
            InitializeComponent();
        }

        private string maintainareaid;
        private string maintainareaname;

        public override void InitControls()
        {
            this.MaintainAreaStation.SetCurrentLabel("当前车站列表");
            this.MaintainAreaStation.SetGroupHeader("维修工区与车站关系");
            this.MaintainAreaStation.SetLeftLabel("可选的车站列表");
            this.MaintainAreaStation.BindingCurrent(GetCurrentStations());
            this.MaintainAreaStation.BindingLeft(GetLeftStations());
            this.MaintainAreaStation.OnOKButtonClicked += new RelactionContol.FunctionCliecked(MaintainAreaStation_OnOKButtonClicked);
            //base.InitControls();
        }

        private void MaintainAreaStation_OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            var collection = from temp in e.current
                       select temp.ID;
            List<string> list = collection.ToList();
            PrimissionManager manager = new PrimissionManager();
            MessageBoxResult result = AFC.WS.UI.CommonControls.MessageDialog.Show("是否要调整工区" + this.maintainareaname + "对应的车站?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                int res = manager.AdjustMaintainAreaStationPrimission(maintainareaid, list);
                if (res == 0)
                    MessageDialog.Show("操作成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                else
                    MessageDialog.Show("操作失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
        }
        
        /// <summary>
        /// 得到当前的车站信息
        /// </summary>
        /// <returns>返回当前的车站信息集合</returns>
        private ObservableCollection<Data> GetCurrentStations()
        {
            List<QueryCondition> list=this.Tag as List<QueryCondition>;
            if (list != null)
            {
                maintainareaid = list.Single(temp => temp.bindingData.Equals("maintain_area_id")).value.ToString();
                maintainareaname = list.Single(temp => temp.bindingData.Equals("maintain_area_name")).value.ToString();
                List<BasiStationInfo> stationList = BuinessRule.GetInstace().GetMaintainAreaStationInfo(maintainareaid);
                ObservableCollection<Data> collection = new ObservableCollection<Data>();
                if (stationList == null)
                {
                    //Wrapper.ShowDialog("此工区无车站对应");
                    return new ObservableCollection<Data>();
                }
                if (stationList.Count == 1 && string.IsNullOrEmpty(stationList[0].station_id))
                {
                    return new ObservableCollection<Data>();
                }
                for (int i = 0; i < stationList.Count; i++)
                {
                    collection.Add(new Data { ID = stationList[i].station_id, Text = stationList[i].station_cn_name, IsChecked = false });
                }
                return collection;
            }
            return   new ObservableCollection<Data>();
        }

        /// <summary>
        /// 得到该操作员剩余的车站列表
        /// </summary>
        /// <returns>返回剩余的车站列表</returns>
        private ObservableCollection<Data> GetLeftStations()
        {
            List<BasiStationInfo> list = BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
           ObservableCollection<Data> leftData = new ObservableCollection<Data>();
           ObservableCollection<Data> currentData = GetCurrentStations();
           if (currentData.Count==1&&string.IsNullOrEmpty(currentData[0].ID))
           {
               //for (int i = 0; i < list.Count; i++)
               //{
               //    leftData.Add(new Data { ID = list[i].station_id, Text = list[i].station_cn_name, IsChecked = false });
               //}
               return leftData;
           }

           for (int i = 0; i < currentData.Count; i++)
           {
               list.Remove(list.Single(temp => temp.station_id.Equals(currentData[i].ID)));
           }
           for (int i = 0; i < list.Count; i++)
           {
               leftData.Add(new Data { ID = list[i].station_id, Text = list[i].station_cn_name, IsChecked = false });
           }
           return leftData;
        }
    }
}
