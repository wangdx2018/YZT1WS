using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.ModelView.Convertors;
    using AFC.WS.BR;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.UI.DataSources;
    using AFC.WS.Model.Const;

    public class TickBoxRegisterAction: IAction
    {

        public TicketOrMoneyBoxIdConvetor convetor = new TicketOrMoneyBoxIdConvetor();

        #region IAction 成员
        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null && actionParamsList.Count == 0)
                return false;
            QueryCondition qc = actionParamsList.First(temp => temp.bindingData.Equals("ticketboxId"));
            QueryCondition qc1 = actionParamsList.First(temp => temp.bindingData.Equals("ticketboxRfid"));
            if (qc != null && qc1 != null)
            {
                if (qc.value.ToString().Length < 8)
                {
                    MessageDialog.Show("票箱编码不合法长度小于8", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }

                if (string.IsNullOrEmpty(qc1.value.ToString()))
                {
                    MessageDialog.Show("请输入票箱RFID", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }

                if (qc1.value.ToString().Length < 8)
                {
                    MessageDialog.Show("票箱RFID不合法长度小于8", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }

                string lineId = qc.value.ToString().Substring(0, 2);
                string ticketboxType = qc.value.ToString().Substring(2, 2);
                string ticketboxNumber = qc.value.ToString().Substring(4, 4);

                try
                {
                    if (lineId != SysConfig.GetSysConfig().LocalParamsConfig.LineCode)
                    {
                        MessageDialog.Show("票箱线路编号必须为" + SysConfig.GetSysConfig().LocalParamsConfig.LineCode, "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return false;
                    }
                    byte ticketboxTypeData = 0;
                    if (byte.TryParse(ticketboxType, out ticketboxTypeData))
                    {
                        if (ticketboxTypeData != 1 && ticketboxTypeData != 2 && ticketboxTypeData != 3)
                        {
                            MessageDialog.Show("票箱编码第3，4位只能为01[正常票箱],02[废票箱], 03[回收箱]", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                            return false;
                        }
                    }
                    AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor convert = new TicketOrMoneyBoxIdConvetor();
                    if (BuinessRule.GetInstace().tickMan.CheckTickBoxHasRegister(convert.ConvertBack(qc.value.ToString(),null,null,null).ToString()))
                    {
                        MessageDialog.Show("票箱已登记", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                        return false;
                    }
                    return true;

                }
                catch (Exception ex)
                {
                    MessageDialog.Show("票箱编码不合法", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            return true;
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            QueryCondition qc = actionParamsList.First(temp => temp.bindingData.Equals("ticketboxId"));
            QueryCondition qc1 = actionParamsList.First(temp => temp.bindingData.Equals("ticketboxRfid"));

            int res = 0;

            if (qc != null || qc1 != null)
            {
                RfidTicketboxInfo rti =BuinessRule.GetInstace().rfidRw.ReadTicketBoxRFID(1, out res);
                if (res != 0)
                {
                    MessageDialog.Show("RFID信息读取失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Register_Action, "1", "票箱登记时RFID标签读取失败");
                    return null;
                }
                if (rti != null)
                {
                    rti.stationCode = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
             
                    rti.lastOpeatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    res = BuinessRule.GetInstace().rfidRw.WriteTicketBoxRFID(rti, 1);
                    if (res != 0)
                    {
                        MessageDialog.Show("RFID写入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Register_Action, "1", "票箱登记时RFID标签写入失败");
                        return null;
                    }
              
                  res = BuinessRule.GetInstace().tickMan.TickBoxReg(this.convetor.ConvertBack(qc.value.ToString(), null, null, null).ToString(), qc1.value.ToString());
          
                    if (res == 0)
                    {
                        IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_tick_box_reg_info");
                        if (dataSource == null)
                        {
                            MessageDialog.Show("票箱 " + qc.value.ToString() + " 登记成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Register_Action, "0", "票箱登记成功");
                            return new ResultStatus { resultCode = 0, resultData = 0 };
                        }
                        DefaultDataSource ds = dataSource as DefaultDataSource;
                        ds.OrderByParams = "update_date desc,update_time desc";
                        DataSourceManager.NotfiyDataSourceChange("ds_tick_box_reg_info");
                        MessageDialog.Show("票箱 " + qc.value.ToString() + " 登记成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Register_Action, "0", "票箱登记成功");
                    }
                    else
                    {
                        MessageDialog.Show("票箱写入RFID成功，数据库更新失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Register_Action, "1", "票箱登记写入RFID成功，数据库更新失败");
                        return null;
                    }
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
                //else
                //{
                //    MessageDialog.Show(string.Format("票箱 " + qc.value.ToString() + " 登记成功，通知SC失败！"), "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                //    return null;
                //}

            }
            return null;
        }

        #endregion
    }
}
