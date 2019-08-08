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
using AFC.BOM2.UIController;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Actions.ParamActions;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.ModelView.Convertors;
using AFC.WS.ModelView.Convetors;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class UpdatePara4044AlarmLamp:  IAction
    {
        #region IAction 成员

        string para_version = string.Empty;
        string card_issuer_id = string.Empty;
        string tick_product_type = string.Empty;
        string lamo_control = string.Empty;
        string voice_control = string.Empty;


        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要编辑的参数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
           
          //  Util.DataBase.BeginTransaction();
            int res = 0;
            string para_version = "-1";
            Para4044CustomAlarmLamp info = new Para4044CustomAlarmLamp();
            info.para_version = para_version;
            info.card_issuer_id = (actionParamsList.Single(temp => temp.bindingData.Equals("card_issue_id")).value.ToString()) == "ACC" ? "01" : "99";

            info.tick_product_type = actionParamsList.Single(temp=>temp.bindingData.Equals("tick_product_type")).value.ToString();
            info.lamp_control = actionParamsList.Single(temp => temp.bindingData.Equals("lamp_control")).value.ToString();
            info.voice_control = actionParamsList.Single(temp => temp.bindingData.Equals("voice_control")).value.ToString();

            //res = DBCommon.Instance.UpdateTable(info, "para_4044_custom_alarm_lamp",
            //        new KeyValuePair<string, string>("para_version", para_version),
            //        new KeyValuePair<string, string>("card_issue_id", card_issuer_id),
            //        new KeyValuePair<string, string>("tick_product_type", tick_product_type));

            res = DBCommon.Instance.UpdateTable(info, "para_4044_custom_alarm_lamp",
        new KeyValuePair<string, string>("para_version",info.para_version),
        new KeyValuePair<string, string>("card_issuer_id",info.card_issuer_id),
        new KeyValuePair<string, string>("tick_product_type",info.tick_product_type));

            if (res == 1)
            {
                Wrapper.ShowDialog("AGM警告灯参数信息更新成功。");
                DataSourceManager.NotfiyDataSourceChange("ds_para_4044_custom_alarm_lamp");
                return null;
            }
            else
            {
                Wrapper.ShowDialog("AGM警告灯参数信息更新失败。");
                return null;
            }
            return null;
        }

        private void InitAlarmLampDetails(InteractiveControlRule icRule, Para4044CustomAlarmLamp alarmLamp)
        {
            if (icRule == null || alarmLamp == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteractiveControlConfig(icRule.ControlList[i], alarmLamp);
            }
 
        }

        private void ReBindingInteractiveControlConfig(ControlProperty controlInfo, Para4044CustomAlarmLamp alarmLamp)
        {
            try
            {
                
                if (controlInfo == null || alarmLamp == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[0].ToUpper())
                {
                    case "CARD_ISSUER_ID":
                        controlInfo.InitValue = alarmLamp.card_issuer_id;
                        return;
                    case "TICK_PRODUCT_TYPE":
                        controlInfo.InitValue = alarmLamp.tick_product_type;
                        return;
                    case "PARA_VERSION":
                        controlInfo.InitValue = "-1";
                        return;
                    case "LAMP_CONTROL":
                        controlInfo.InitValue = alarmLamp.lamp_control;
                        return;
                    case "VOICE_CONTROL":
                        controlInfo.InitValue = alarmLamp.voice_control;
                        return;

                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }

        #endregion
    }
}
