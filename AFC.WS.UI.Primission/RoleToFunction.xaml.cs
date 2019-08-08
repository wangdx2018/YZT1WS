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
    using AFC.WS.Model.DB;
    using System.Collections.ObjectModel;
    using AFC.WS.BR;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.Const;
    /// <summary>
    /// 角色和功能对应关系
    /// </summary>
    public partial class RoleToFunction : UserControlBase
    {
        public RoleToFunction()
        {
            InitializeComponent();
        }
        private string roleId;

        public override void InitControls()
        {

           List<QueryCondition> list = this.Tag as List<QueryCondition>;
           if (list != null)
           {
               this.roleId = list.Single(temp => temp.bindingData.Equals("role_id")).value.ToString();
           }
            if (!string.IsNullOrEmpty(roleId))
            {
                this.roleToFun.SetGroupHeader("角色功能关系");
                this.roleToFun.SetCurrentLabel("当前功能列表");            
                this.roleToFun.SetLeftLabel("可选的功能列表");
                this.roleToFun.BindingCurrent(InitCurrenFunctionInfo(roleId));
                this.roleToFun.BindingLeft(InitAllFunctionInfo(roleId));
                this.roleToFun.OnOKButtonClicked += new RelactionContol.FunctionCliecked(OnOKButtonClicked);
            }
        }
        private void OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            MessageBoxResult res = AFC.WS.UI.CommonControls.MessageDialog.Show("是否要调整角色" + roleId + "?", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Question, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    AFC.WS.BR.Primission.RoleManager roleManger = new AFC.WS.BR.Primission.RoleManager();
                    roleManger.DeleteRoleToFunctionRelaction(roleId);
                    for (int i = 0; i < e.current.Count; i++)
                    {
                        roleManger.AddRoleToFunctionRelaction(roleId, e.current[i].ID);
                    }
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Function_To_Role, "0");
                    AFC.WS.UI.CommonControls.MessageDialog.Show("调整角色功能成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                catch (Exception ex)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Add_Role_To_Operator, "1");
                    AFC.WS.UI.CommonControls.MessageDialog.Show("调整角色功能失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
            }

        }

        public ObservableCollection<Data> InitAllFunctionInfo(string roleId)
        {
            AFC.WS.BR.Primission.RoleManager roleManger = new AFC.WS.BR.Primission.RoleManager();
            List<PrivFunctionInfo> funcList = roleManger.getFuncByRoleID(roleId);
            List<PrivFunctionInfo> listRoleInfo = roleManger.GetAllNormalFunctionInfos(roleId);
            if (listRoleInfo != null && funcList!=null)
            {
            listRoleInfo = listRoleInfo.Where(temp =>
            {
                bool flag = true;
                for (int i = 0; i < funcList.Count; i++)
                {
                    PrivFunctionInfo info = (PrivFunctionInfo)funcList[i];
                    if (temp.function_id == info.function_id)
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
                    PrivFunctionInfo info = (PrivFunctionInfo)listRoleInfo[i];
                    list.Add(new Data { ID = info.function_id.ToString(), Text = info.function_name.ToString() });
                }
            }
            return list;


        }

        public ObservableCollection<Data> InitCurrenFunctionInfo(string roleId)
        {
            AFC.WS.BR.Primission.RoleManager roleManger = new AFC.WS.BR.Primission.RoleManager();
            List<PrivFunctionInfo> funcList = roleManger.getFuncByRoleID(roleId);
            ObservableCollection<Data> list = new ObservableCollection<Data>();
            if (funcList != null)
            {
                for (int i = 0; i < funcList.Count; i++)
                {
                    PrivFunctionInfo info = (PrivFunctionInfo)funcList[i];
                    list.Add(new Data { ID = info.function_id.ToString(), Text = info.function_name.ToString() });
                }
            }
            return list;

        }
    }

}









