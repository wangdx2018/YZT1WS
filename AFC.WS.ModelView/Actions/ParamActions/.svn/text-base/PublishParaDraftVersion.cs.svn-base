using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    using AFC.WS.BR.ParamsManager;
    using AFC.WS.Model.Const;
    using System.ComponentModel;

    public class PublishParaDraftVersion:  IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            //如果是普通发布，要检查生效日期
            if (PublishStatus.Equals("00"))
            {

                if (string.IsNullOrEmpty(ParaPublishSelDate.strParaActiveDate))
                {
                    MessageDialog.Show("请选择生效日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                string DateTimeFormat = "yyyy-MM-dd";

                DateTime dtActive = DateTime.ParseExact(ParaPublishSelDate.strParaActiveDate, DateTimeFormat, null);

                //当前日期
                string currRunDate = BuinessRule.GetInstace().rm.GetRunDate();
                DateTime curDate = DateTime.ParseExact(currRunDate, "yyyy年MM月dd日", null);

                if (dtActive.Subtract(curDate).Days < 0)
                {
                    MessageDialog.Show("生效日期应该不小于运营日期", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    return false;
                }
            }
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要发布的草稿版", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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

            try
            {
                List<ushort> listParaType = new List<ushort>();
                uint uintActive;
                List<QueryCondition> conditions = actionParamsList.FindAll(temp => temp.bindingData.Equals("para_type"));
                if (conditions != null && conditions.Count > 0)
                {
                  
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        string paraTypeStr = conditions[i].value.ToString();
                        ushort paraType = paraTypeStr.ConvertHexStringToUshort();
                        listParaType.Add(paraType);
                    }
                }
                //如果是普通发布，生效日期界面可选
                if (PublishStatus.Equals("00"))
                {
                    //生效日期
                    uintActive = ParaPublishSelDate.strParaActiveDate.ConvertDateTimeToUnit();
                }
                //如果是权限发布，生效日期为当前日期
                else
                {
                    uintActive = DateTime.Now.Date.ToString("yyyy-MM-dd").ConvertDateTimeToUnit();
                }

                //发布日期
                uint uintPublish = DateTime.Now.Date.ToString("yyyy-MM-dd").ConvertDateTimeToUnit();
                int res = BuinessRule.GetInstace().commProcess.PublishParaDraft(uintActive, uintPublish, listParaType);
                if (res == 0)
                {
                    MessageDialog.Show("参数发布成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Publish_Para_Draft_Version, "0", "参数发布成功");
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
                MessageDialog.Show("参数发布失败,错误代码" +AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent(CommMsgType.Params_Publish.ToString("X4"), res.ToString()), "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Publish_Para_Draft_Version, "1", "参数发布失败");
                return null;


            }
            catch (Exception ex)
            {
                return null;
            }


           
        }

        [Description("发布的状态设置选择。普通 = 00 ,权限 = 01"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Filter()]
        public string PublishStatus
        {
            get;
            set;
        }

        #endregion
    }
}
