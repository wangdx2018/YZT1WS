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

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using System.Data;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// ParamsAGMLightErrorHandleEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ParamsAGMLightErrorHandleEdit : UserControlBase
    {
        public ParamsAGMLightErrorHandleEdit()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            try
            {
                this.ctr1.SetHeader("黑名单通行灯设置");
                this.ctr2.SetHeader("坏卡通行灯设置");
                this.ctr3.SetHeader("非法闯入闯出灯设置");

                this.ctrl4.SetHeader("黑名单通行声音处理");
                this.ctrl5.SetHeader("坏卡通行声音处理");
                this.ctrl6.SetHeader("非法闯入闯声音处理");


                this.ctr1.SetRadioButtonLightGroup("blackListRed", "blackListGreen");
                this.ctr2.SetRadioButtonLightGroup("badLightRed", "badLightGreen");
                this.ctr3.SetRadioButtonLightGroup("illegalLihtRed", "illLegalLightGreen");

                this.ctrl4.SetRadButtonVoiceGroup("blackListVoice");
                this.ctrl5.SetRadButtonVoiceGroup("badVoice");
                this.ctrl6.SetRadButtonVoiceGroup("illegalVoice");

                DataTable dt = this.Tag as DataTable;
                if (dt != null)
                {
                    //this.ctr1 as
                    (this.ctr1 as ICommonEdit).SetControlValue(dt.Rows[0][1]);
                    (this.ctr2 as ICommonEdit).SetControlValue(dt.Rows[0][3]);
                    (this.ctr3 as ICommonEdit).SetControlValue(dt.Rows[0][5]);

                    (this.ctrl4 as ICommonEdit).SetControlValue(dt.Rows[0][2]);
                    (this.ctrl5 as ICommonEdit).SetControlValue(dt.Rows[0][4]);
                    (this.ctrl6 as ICommonEdit).SetControlValue(dt.Rows[0][6]);

                    //todo:Binding here
                    //ct1.SetCOntrolValue(); foreach

                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
            
            //base.InitControls();
        }

        private void ctr3_Loaded(object sender, RoutedEventArgs e)
        {
              
        }

        private void ctrl5_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = this.Tag as DataTable;

            //dt.Rows[0][1] = (this.ctr1 as ICommonEdit).GetControlValue();
            //dt.Rows[0][3]=this.ctr2.GetControlValue();
            //dt.Rows[0][5]=this.ctr3.GetControlValue();
            //dt.Rows[0][2]=this.ctrl4.GetControlValue();
            //dt.Rows[0][4]=this.ctrl5.GetControlValue();
            //dt.Rows[0][6] = this.ctrl6.GetControlValue();

            try
            {
                Para4044AlarmLampData data = new Para4044AlarmLampData();
                data.para_version = "-1";
                data.black_list_light_contol = this.ctr1.GetControlValue().ToString();
                data.bad_card_light_control = this.ctr2.GetControlValue().ToString();
                data.unlawed_enter_light_control = this.ctr3.GetControlValue().ToString();

                data.black_list_voice_control = this.ctrl4.GetControlValue().ToString();
                data.bad_card_voice_control = this.ctrl5.GetControlValue().ToString();
                data.unlawed_enter_voice_control = this.ctrl6.GetControlValue().ToString();

                DBCommon.Instance.UpdateTable<Para4044AlarmLampData>(data, "para_4044_alarm_lamp_data", new KeyValuePair<string, string>("para_version", "-1"));
                MessageDialog.Show("更新成功！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);

            }
            catch(Exception ex)
            {
                WriteLog.Log_Error(ex);
                MessageDialog.Show("更新失败！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            


        }
    }
}
