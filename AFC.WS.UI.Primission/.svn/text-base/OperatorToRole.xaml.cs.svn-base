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

namespace AFC.WS.UI.Primission
{
    using AFC.BOM2.UIController;
    
    using AFC.WS.UI.Common;
    using System.Collections.ObjectModel;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    /// </summary>
    public partial class OperatorToRole : UserControlBase
    {
        private string operatorId;

        public OperatorToRole()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            if (list != null)
            {
                this.operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            }
            if (!string.IsNullOrEmpty(operatorId))
            {
                this.OperTo.SetCurrentLabel("当前的操作员角色列表");
                this.OperTo.SetGroupHeader("操作员角色信息关系");
                this.OperTo.SetLeftLabel("可选的角色列表");
                this.OperTo.BindingCurrent(InitCurrenRoleInfo(operatorId));
                this.OperTo.BindingLeft(InitAllRoleInfo(operatorId));
                this.OperTo.OnOKButtonClicked += new RelactionContol.FunctionCliecked(OnOKButtonClicked);
            }
        }

        private void OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            MessageBoxResult res = AFC.WS.UI.CommonControls.MessageDialog.Show("是否要调整操作员" + operatorId + "的角色?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    AFC.WS.BR.Primission.PrimissionManager primManger = new AFC.WS.BR.Primission.PrimissionManager();
                    primManger.DeleteOperatorToRoleRelaction(operatorId);
                    for (int i = 0; i < e.current.Count; i++)
                    {
                        primManger.AddOperatorToRoleRelaction(operatorId, e.current[i].ID);
                    }
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Role_To_Operator,"0");
                    AFC.WS.UI.CommonControls.MessageDialog.Show("调整操作员角色成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                catch (Exception ex)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Role_To_Operator, "1");
                    AFC.WS.UI.CommonControls.MessageDialog.Show("调整操作员角色失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
            }

        }

        public ObservableCollection<Data> InitAllRoleInfo(string operatorId)
        {
            AFC.WS.BR.Primission.PrimissionManager primManger = new AFC.WS.BR.Primission.PrimissionManager();
            List<PrivRoleInfo> priRoleList = primManger.GetRoleInfoByOperatorId(operatorId);
            List<PrivRoleInfo> listRoleInfo = primManger.GetAllNormalRoleInfos();
            if (priRoleList != null && listRoleInfo != null)
            {
                listRoleInfo = listRoleInfo.Where(temp =>
                {
                    bool flag = true;
                    for (int i = 0; i < priRoleList.Count; i++)
                    {
                        PrivRoleInfo info = (PrivRoleInfo)priRoleList[i];
                        if (temp.role_id == info.role_id)
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                }).ToList();
            }
            ObservableCollection<Data> list = new ObservableCollection<Data>();
            if (listRoleInfo != null)
            {
                for (int i = 0; i < listRoleInfo.Count; i++)
                {
                    PrivRoleInfo info = (PrivRoleInfo)listRoleInfo[i];
                    list.Add(new Data { ID = info.role_id.ToString(), Text = info.role_name.ToString() });
                }
            }
            return list;


        }

        public ObservableCollection<Data> InitCurrenRoleInfo(string operatorId)
        {
            AFC.WS.BR.Primission.PrimissionManager primManger = new AFC.WS.BR.Primission.PrimissionManager();
            List<PrivRoleInfo> priRoleList = primManger.GetRoleInfoByOperatorId(operatorId);
            ObservableCollection<Data> list = new ObservableCollection<Data>();
            if (priRoleList != null)
            {
                for (int i = 0; i < priRoleList.Count; i++)
                {
                    PrivRoleInfo info = (PrivRoleInfo)priRoleList[i];
                    list.Add(new Data { ID = info.role_id.ToString(), Text = info.role_name.ToString() });
                }
            }
            return list;

        }
    }   
}
