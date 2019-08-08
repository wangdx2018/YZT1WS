using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class AddOperContentAction : IAction
    {
        #region IAction 成员
        //string lineID = string.Empty;
        //string stationID = string.Empty;
        string projectName = string.Empty;
        string content = string.Empty;
        string operatorID = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            //if (actionParamsList.Single(temp => temp.bindingData.Equals("line_id")).value != null)
            //{
            //    lineID = actionParamsList.Single(temp => temp.bindingData.Equals("line_id")).value.ToString();
            //}
            //if (actionParamsList.Single(temp => temp.bindingData.Equals("station_id")).value != null)
            //{
            //    stationID = actionParamsList.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();
            //}
            projectName = actionParamsList.Single(temp => temp.bindingData.Equals("project_name")).value.ToString();
            content = actionParamsList.Single(temp => temp.bindingData.Equals("content")).value.ToString();
            operatorID = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            //if (string.IsNullOrEmpty(lineID))
            //{
            //    MessageDialog.Show("请选择线路", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //if (string.IsNullOrEmpty(stationID))
            //{
            //    MessageDialog.Show("请选择车站", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            if (string.IsNullOrEmpty(projectName))
            {
                MessageDialog.Show("请输入项目名称", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(content))
            {
                MessageDialog.Show("请备注内容", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            OperContentLogInfo logInfo = new OperContentLogInfo();
            int seq = 0;
            logInfo.content_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString().ToInt64();
            logInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            logInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            logInfo.project_name = projectName;
            logInfo.content = content;
            logInfo.operator_id = operatorID;
            logInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            logInfo.update_time = DateTime.Now.ToString("HHmmss");
            int result = DBCommon.Instance.InsertTable(logInfo, "oper_content_log_info");
            if (result != 1)
            {
                MessageDialog.Show("数据库插入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                MessageDialog.Show("备注增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }

            return null;
        }

        #endregion
    }
}
