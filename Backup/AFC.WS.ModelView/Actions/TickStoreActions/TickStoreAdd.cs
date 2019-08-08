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
    public class TickStoreAdd: IAction
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
            res = DBCommon.Instance.InsertTable(info, "basi_tick_mana_type_info");
            if (res != 1)
            {
                Wrapper.ShowDialog("票卡种类表操作失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_Store_Add, "1", "票卡种类表操作失败");
                Util.DataBase.Rollback();
                return null;
            }

            TickStorageInfo info1 = new TickStorageInfo();
            int res1 = 0;
            info1.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info1.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info1.tick_mana_type = TickStoreType;
            info1.ticket_status = "00";
            info1.in_store_num = 0;
            info1.total_num = 0;
            info1.update_date = DateTime.Now.ToString("yyyyMMdd");
            info1.update_time = DateTime.Now.ToString("HHmmss");
            res1 = DBCommon.Instance.InsertTable(info1, "tick_storage_info");
            if (res1 == 1)
            {
                Wrapper.ShowDialog("新库存管理类型添加成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_Store_Add, "0", "新库存管理类型添加成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("票卡库存表操作失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_Store_Add, "1", "票卡库存表操作失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}