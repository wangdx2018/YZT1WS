using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.ModelView.Convertors;
    using AFC.WS.BR;
    using System.Windows;
    using AFC.WS.Model.Const;

    public class RFIDInitAction: IAction
    {
        #region IAction 成员


        /// <summary>
        /// 票箱转换器将10进制转换成16进行显示
        /// </summary>
        private TicketOrMoneyBoxIdConvetor convetor = new TicketOrMoneyBoxIdConvetor();

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择初始化的操作的钱票箱类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                string boxId = actionParamsList.Single(temp => temp.bindingData.Equals("boxId")).value.ToString();
                string boxType = actionParamsList.Single(temp => temp.bindingData.Equals("boxType")).value.ToString();
                if (string.IsNullOrEmpty(boxId) ||
                    string.IsNullOrEmpty(boxType))
                {
                    throw new Exception("箱子类型或者箱子编号为空");
                }
                return this.CheckBoxIDValid(boxId.Substring(4,4)) &&
         (MessageDialog.Show("初始化后标签以前数据将清除,您是否要初始化标签?", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo) == MessageBoxResult.Yes);
            }
            catch (Exception ex)
            {
                MessageDialog.Show("请输入箱子ID或者选择箱子类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
          
        }

        /// <summary>
        /// 检查箱子ID和提示
        /// </summary>
        /// <param name="boxId">箱子ID</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckBoxIDValid(string boxId)
        {
            if (string.IsNullOrEmpty(boxId))
            {
                MessageDialog.Show("请输入钱/票箱序号", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            if (boxId.Length < 4)
            {
                MessageDialog.Show("序号必须为4位", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            uint res = 0;
            bool result = uint.TryParse(boxId, out res);
            if (result)
            {
                if (res > 9999)
                {
                    MessageDialog.Show("序号必须小于9999", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                 
                    return false;
                }
                return true;
            }
            else
                return result;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string boxType = actionParamsList.Single(temp => temp.bindingData.Equals("boxType")).value.ToString();
            object boxId = actionParamsList.Single(temp => temp.bindingData.Equals("boxId")).value;
            int res = 0;
            switch (boxType.Substring(2, 2))
            {
                case "01":
                case "02":
                case "03":
                    string Id = convetor.ConvertBack(boxId, null, null, null).ToString();
                  RfidTicketboxInfo info = BuinessRule.GetInstace().tickMan.CreateTicketBoxInfo(Id);
                  res=BuinessRule.GetInstace().rfidRw.WriteTicketBoxRFID(info, 1, "A");
                  if (res != 0)
                  {
                      BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_RFID_Init_Action, "1", "票箱RFID初始化失败");
                      return null;
                  }
                  res=BuinessRule.GetInstace().rfidRw.WriteTicketBoxRFID(info, 1, "B");
                  if (res != 0)
                  {
                      BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_RFID_Init_Action, "1", "票箱RFID初始化失败");
                      return null;
                  }
                  else
                  {
                      BuinessRule.GetInstace().logManager.WriteTickBoxOperation(Id, "07");
                      BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_RFID_Init_Action, "0", "票箱RFID初始化成功");
                  }
                  return new ResultStatus { resultCode = 0, resultData = 0 };
                case "11":
                case "21":
                case "22":
                    Id = convetor.ConvertBack(boxId, null, null, null).ToString();
                    MoneyBoxRFID mInfo = BuinessRule.GetInstace().tickMan.CreateMoneyBoxRFID(Id);
                    res=BuinessRule.GetInstace().rfidRw.WriteMoneyBoxRFID(mInfo,1,"A");
                    if (res != 0)
                    {
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.CashBox_RFID_Init_Action, "1", "钱箱RFID初始化失败");
                        return null;
                    }
                    res=BuinessRule.GetInstace().rfidRw.WriteMoneyBoxRFID(mInfo,1,"B");
                    if (res != 0)
                    {
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.CashBox_RFID_Init_Action, "1", "钱箱RFID初始化失败");
                        return null;
                    }
                    else
                    {
                        BuinessRule.GetInstace().logManager.WriteMoneyBoxOperation(Id, "07");
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.CashBox_RFID_Init_Action, "0", "钱箱RFID初始化成功");
                    }
                    return new ResultStatus{ resultData=0, resultCode=0};
            }

           

            return null;
          
        }

        #endregion
    }
}
