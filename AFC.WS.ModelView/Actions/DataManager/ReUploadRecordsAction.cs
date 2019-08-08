using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.DataManager
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    
    /// <summary>
    /// added by wangdx  20110511
    /// LC运营开始的Action
    /// </summary>
    public class ReUploadRecordsAction : IAction
    {
        #region IAction 成员
        string deviceId="";
        string dataType = "";
        string tranDateBegin = "";
        string tranDateEnd = "";

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("deviceId")).value != null)
            {
                deviceId = actionParamsList.Single(temp => temp.bindingData.Equals("deviceId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("dataType")).value != null)
            {
                dataType = actionParamsList.Single(temp => temp.bindingData.Equals("dataType")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tranDateBegin")).value != null)
            {
                tranDateBegin = actionParamsList.Single(temp => temp.bindingData.Equals("tranDateBegin")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("tranDateEnd")).value != null)
            {
                tranDateEnd = actionParamsList.Single(temp => temp.bindingData.Equals("tranDateEnd")).value.ToString();
            }
            if (string.IsNullOrEmpty(deviceId))
            {
                Wrapper.ShowDialog("设备编号不能为空。");
                return false;
            }
            if (string.IsNullOrEmpty(dataType))
            {
                Wrapper.ShowDialog("数据类型不能为空。");
                return false;
            }
            if (string.IsNullOrEmpty(tranDateBegin))
            {
                Wrapper.ShowDialog("开始日期不能为空。");
                return false;
            }
            if (string.IsNullOrEmpty(tranDateEnd))
            {
                Wrapper.ShowDialog("结束日期不能为空。");
                return false;
            }
            if (tranDateBegin.ConvertDateTimeToUnit() > tranDateEnd.ConvertDateTimeToUnit())
            {
                Wrapper.ShowDialog("开始日期要在结束日期之前，请重新选择。");
                return false;
            }
            //if (BuinessRule.GetInstace().rm.GetRunDate().ConvertDateTimeToUnit() > tranDateEnd.ConvertDateTimeToUnit())
            //{
            //    Wrapper.ShowDialog("超过当前运营日，请重新选择。");
            //    return false;
            //}
            return true;
        }


        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            int res = BR.BuinessRule.GetInstace().commProcess.ReUploadRecords((uint)deviceId.ToHexNumberInt32(), (uint)dataType.ToHexNumberInt32(), tranDateBegin.ConvertDateTimeToUnit(), tranDateEnd.ConvertDateTimeToUnit());
            if (res != 0)
            {
                MessageDialog.Show("发送设备数据补传命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_Start, "1", "LC发送设备数据补传命令失败");
                return null;
            }
            MessageDialog.Show("发送设备数据补传命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_Start, "0", "LC发送设备数据补传命令成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IAction 成员


        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
