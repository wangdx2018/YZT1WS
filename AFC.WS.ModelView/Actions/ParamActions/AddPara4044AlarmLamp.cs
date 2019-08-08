using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.WS.Model.DB;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class AddPara4044AlarmLampAction:IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            string paraVersion = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();
            string cardIssuerId = actionParamsList.Single(temp => temp.bindingData.Equals("card_issuer_id")).value.ToString();
            string tickProType = actionParamsList.Single(temp => temp.bindingData.Equals("tick_product_type")).value.ToString();
            string lampControl = actionParamsList.Single(temp => temp.bindingData.Equals("lamp_control")).value.ToString();
            string voiceControl = actionParamsList.Single(temp => temp.bindingData.Equals("voice_control")).value.ToString();

            if (string.IsNullOrEmpty(paraVersion))
            {
                MessageDialog.Show("请输入参数版本！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(cardIssuerId))
            {
                MessageDialog.Show("请输入发行商ID！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(tickProType))
            {
                MessageDialog.Show("请输入车票产品类型！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(cardIssuerId))
            {
                MessageDialog.Show("请输入灯处理信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(tickProType))
            {
                MessageDialog.Show("请输入声音处理信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            Para4044CustomAlarmLamp para4044 = new Para4044CustomAlarmLamp();
            //para4044.para_version = actionParamsList.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();
            para4044.card_issuer_id = actionParamsList.Single(temp => temp.bindingData.Equals("card_issuer_id")).value.ToString();
            para4044.tick_product_type = actionParamsList.Single(temp => temp.bindingData.Equals("tick_product_type")).value.ToString();
            para4044.para_version = "-1";
            para4044.lamp_control = actionParamsList.Single(temp => temp.bindingData.Equals("lamp_control")).value.ToString();
            para4044.voice_control = actionParamsList.Single(temp => temp.bindingData.Equals("voice_control")).value.ToString();

            int result = DBCommon.Instance.InsertTable(para4044, "para_4044_custom_alarm_lamp");
            if (result != 1)
            {
                MessageDialog.Show("数据库插入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                MessageDialog.Show("参数增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);

                DataSourceManager.NotfiyDataSourceChange("ds_para_4044_custom_alarm_lamp");
            }

            return null;
        }

        #endregion
    }
}
