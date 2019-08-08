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
using System.Collections.ObjectModel;

namespace AFC.WS.UI.Primission
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;

    /// <summary>
    /// added by wangdx date:20110317
    /// 
    /// 为操作员分配操作的设备类型UI
    /// </summary>
    public partial class OperatorToDeviceType : UserControlBase
    {
        private string operatorId;

        public OperatorToDeviceType()
        {
            InitializeComponent();
            this.operatorDevTypeRel.OnOKButtonClicked += new RelactionContol.FunctionCliecked(operatorDevTypeRel_OnOKButtonClicked);
        }

        public override void InitControls()
        {
            this.operatorDevTypeRel.SetCurrentLabel("操作员的当前可操作设备");
            this.operatorDevTypeRel.SetGroupHeader("操作员设备类型调整");
            this.operatorDevTypeRel.SetLeftLabel("剩余的设备类型");
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            this.operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            this.operatorDevTypeRel.BindingCurrent(GetCurrentData());
            this.operatorDevTypeRel.BindingLeft(GetLeftData());
         
            
        }

        private void operatorDevTypeRel_OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            
            List<string> list = e.current.Select(temp => temp.ID).ToList();
            PrimissionManager pm = new PrimissionManager();
           
            MessageBoxResult result = AFC.WS.UI.CommonControls.MessageDialog.Show("是否要调整操作员" + operatorId + "可操作的设备类型?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                int res = pm.AdjustOperatorDevTypePrimission(operatorId, list);
                if (res == 0)
                    MessageDialog.Show("操作成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                else
                    MessageDialog.Show("操作失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 得到当前的操作员可以操作的设备类型
        /// </summary>
        /// <returns>返回当前的可以操作的设备类型</returns>
        private ObservableCollection<Data> GetCurrentData()
        {
            ObservableCollection<Data> collection = new ObservableCollection<Data>();

           List<BasiDevTypeInfo> list=BuinessRule.GetInstace().GetOperatorDevType(operatorId);

           if (list == null || list.Count == 0)
               return collection;
           for (int i = 0; i < list.Count; i++)
           {
               collection.Add(new Data { ID = list[i].device_type, Text = list[i].device_name, IsChecked = false });
           }
           return collection;
        }



        private ObservableCollection<Data> GetLeftData()
        {
            ObservableCollection<Data> current = GetCurrentData();
            List<BasiDevTypeInfo> list = BuinessRule.GetInstace().GetAllDevciceType();
            if (list != null)
            {
                for (int i = 0; i < current.Count; i++)
                {
                    BasiDevTypeInfo info = list.Single(temp => temp.device_type.Equals(current[i].ID));
                    if (list.Contains(info))
                    {
                        list.Remove(info);
                    }
                }

                ObservableCollection<Data> leftData = new ObservableCollection<Data>();
                for (int i = 0; i < list.Count; i++)
                {
                    leftData.Add(new Data { ID = list[i].device_type, Text = list[i].device_name, IsChecked = false });
                }
                return leftData;
            }
            return new ObservableCollection<Data>();
        }
    }
}
