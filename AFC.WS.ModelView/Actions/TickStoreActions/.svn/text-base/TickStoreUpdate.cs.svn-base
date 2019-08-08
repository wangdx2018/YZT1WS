using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.RfidRW;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Windows;
using AFC.WS.BR.LogManager;
using AFC.WS.BR.TickBoxManager;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.TickStoreActions
{
    public class TickStoreUpdate : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员

        string TickStoreType = string.Empty;
        string txtTickName = string.Empty;
        string PhyTypeName = string.Empty;
        string PhyTypeId = string.Empty;
        string ProductTypeName = string.Empty;
        string ProductTypeId = string.Empty;
        string CardIssue = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("TickStoreType")).value != null)
            {
                TickStoreType = actionParamsList.Single(temp => temp.bindingData.Equals("TickStoreType")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("txtTickName")).value != null)
            {
                txtTickName = actionParamsList.Single(temp => temp.bindingData.Equals("txtTickName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("PhyTypeName")).value != null)
            {
                PhyTypeName = actionParamsList.Single(temp => temp.bindingData.Equals("PhyTypeName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("PhyTypeId")).value != null)
            {
                PhyTypeId = actionParamsList.Single(temp => temp.bindingData.Equals("PhyTypeId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProductTypeName")).value != null)
            {
                ProductTypeName = actionParamsList.Single(temp => temp.bindingData.Equals("ProductTypeName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProductTypeId")).value != null)
            {
                ProductTypeId = actionParamsList.Single(temp => temp.bindingData.Equals("ProductTypeId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("CardIssue")).value != null)
            {
                CardIssue = actionParamsList.Single(temp => temp.bindingData.Equals("CardIssue")).value.ToString();
            }
            if (string.IsNullOrEmpty(TickStoreType))
            {
                Wrapper.ShowDialog("请填写库存类型。");
                return false;
            }
            if (string.IsNullOrEmpty(txtTickName))
            {
                Wrapper.ShowDialog("请填写库存类型名称。");
                return false;
            }
            if (string.IsNullOrEmpty(PhyTypeName))
            {
                Wrapper.ShowDialog("请填写库存物理类型。");
                return false;
            }
            if (string.IsNullOrEmpty(ProductTypeName))
            {
                Wrapper.ShowDialog("请填写票种基本类型。");
                return false;
            }
            if (string.IsNullOrEmpty(CardIssue))
            {
                Wrapper.ShowDialog("请填写卡发行商ID。");
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //开启事务
            Util.DataBase.BeginTransaction();
            int res = 0;
            BasiTickManaTypeInfo info = new BasiTickManaTypeInfo();
            info.tick_mana_type = TickStoreType;
            info.tick_mana_type_name = txtTickName;
            info.ticket_family_type = ProductTypeId;
            info.ticket_family_type_name = ProductTypeName;
            info.ticket_phy_type = PhyTypeId;
            info.ticket_phy_type_name = PhyTypeName;
            info.card_issue_id = CardIssue;

            //插入新票卡种类
            res = DBCommon.Instance.UpdateTable(info, "basi_tick_mana_type_info", new KeyValuePair<string, string>("tick_mana_type", TickStoreType));
      
            if (res == 1)
            {
                Wrapper.ShowDialog("库存管理类型重命名成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_Store_Update, "0", "库存管理类型重命名成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("库存管理类型重命名失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_Store_Update, "1", "库存管理类型重命名失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Tick_Store_Update, operatorId, "票卡库存调整");
            return res == 0;
        }

        #endregion
    }

}