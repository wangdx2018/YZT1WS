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
using Microsoft.Windows.Controls;


namespace AFC.WS.UI.Params
{
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.ParamsManager;
    using AFC.WS.BR.SysStart;
    using AFC.WS.UI.SmartClient;
    /// <summary>
    /// ParaDraftVersionPublish.xaml 的交互逻辑
    /// 
    /// 
    /// added by wangdx 20110622 增加了DateTimePicker的事件处理的非空操作
    /// </summary>
    public partial class PrimParamPublish : UserControlBase
    {

        public PrimParamPublish()
        {
            InitializeComponent();

            //UIElement elementPick = this.effectiveDate;

            //if (elementPick != null && elementPick is DateTimePickerExtend)
            //{
            //    DateTimePickerExtend pickExtend = elementPick as DateTimePickerExtend;
            //    Grid g = pickExtend.Content as Grid;
            //    if (g != null)
            //    {
            //        foreach (var a in g.Children)
            //        {
            //            if (a is Microsoft.Windows.Controls.DatePicker)
            //            {
            //                Microsoft.Windows.Controls.DatePicker dp = a as Microsoft.Windows.Controls.DatePicker;
            //                dp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dp_SelectedDateChanged);
            //                break;
            //            }
            //        }
            //    }
            


            //UIElement elementPublish = this.publishDate;

            //if (elementPublish != null && elementPublish is DateTimePickerExtend)
            //{
            //    DateTimePickerExtend publishPick = elementPublish as DateTimePickerExtend;
            //    Grid g = publishPick.Content as Grid;
            //    if (g != null)
            //    {
            //        foreach (var a in g.Children)
            //        {
            //            if (a is Microsoft.Windows.Controls.DatePicker)
            //            {
            //                Microsoft.Windows.Controls.DatePicker pb = a as Microsoft.Windows.Controls.DatePicker;
            //                pb.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(pb_SelectedDateChanged);
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        public override void InitControls()
        {
            ParaPublishSelDate.strParaActiveDate = DateTime.Now.ToString("yyyy-MM-dd");
            ParaPublishSelDate.strParaPublishDate = DateTime.Now.ToString("yyyy-MM-dd");
         
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_prim_param_publish.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
        }

  

        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_para_publish");
            ParaPublishSelDate.strParaActiveDate = "";
            ParaPublishSelDate.strParaPublishDate = "";
            //this.effectiveDate.SetControlValue(null);
            //this.publishDate.SetControlValue(null);
            //this.effectiveDate.SetControlValue(string.Empty);
            //this.publishDate.SetControlValue(string.Empty);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoftAndParaUpdate update = new SoftAndParaUpdate();
                List<string> addressName = new List<string>();
                SmartClient client = new SmartClient();
                client.loadConfig();
                //ParaPublishSelDate.strParaPublishDate = this.publishDate.GetControlValue().ToString();
                //ParaPublishSelDate.strParaActiveDate = this.effectiveDate.GetControlValue().ToString();
            }
            catch (Exception ex)
            {

            }

        }

        /*private string getFormatDate(string value)
        {
            string[] getDate = value.Split('-');
            string returnValue = getDate[0] + "-"+(getDate[1].Length == 2 ? getDate[1] : "0" + getDate[1]) +"-"+ (getDate[2].Length == 2 ? getDate[2] : "0" + getDate[2]);
            return returnValue;
            
        }*/
    }
}
