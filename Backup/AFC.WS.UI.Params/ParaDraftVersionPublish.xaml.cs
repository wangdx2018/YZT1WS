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
    public partial class ParaDraftVersionPublish : UserControlBase
    {
       
        public ParaDraftVersionPublish()
        {
            InitializeComponent();

            UIElement elementPick = this.effectiveDate;

            if (elementPick != null && elementPick is DateTimePickerExtend)
            {
                DateTimePickerExtend pickExtend = elementPick as DateTimePickerExtend;
                Grid g = pickExtend.Content as Grid;
                if (g != null)
                {
                    foreach (var a in g.Children)
                    {
                        if (a is Microsoft.Windows.Controls.DatePicker)
                        {
                            Microsoft.Windows.Controls.DatePicker dp = a as Microsoft.Windows.Controls.DatePicker;
                            dp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dp_SelectedDateChanged);
                            break;
                        }
                    }
                }
            }


           /* UIElement elementPublish = this.publishDate;

            if (elementPublish != null && elementPublish is DateTimePickerExtend)
            {
                DateTimePickerExtend publishPick = elementPublish as DateTimePickerExtend;
                Grid g = publishPick.Content as Grid;
                if (g != null)
                {
                    foreach (var a in g.Children)
                    {
                        if (a is Microsoft.Windows.Controls.DatePicker)
                        {
                            Microsoft.Windows.Controls.DatePicker pb = a as Microsoft.Windows.Controls.DatePicker;
                            pb.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(pb_SelectedDateChanged);
                            break;
                        }
                    }
                }
            }*/
        }

        public override void InitControls()
        {
            ParaPublishSelDate.strParaActiveDate = "";
            ParaPublishSelDate.strParaPublishDate = "";
            this.effectiveDate.Initialize();
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_param_publish.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
            DataListRule primdlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_prim_param_publish.xml");
            if (primdlr != null)
            {
                this.primList.Initliaize(primdlr);
            }
        }

        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UIElement element = this.effectiveDate;
            if (element != null && element is DateTimePickerExtend)
             {
                 DateTimePickerExtend cmb = element as DateTimePickerExtend;
                 if (string.IsNullOrEmpty((sender as DatePicker).Text))
                     return;
                 DateTime strActiveDate = (DateTime)(sender as DatePicker).SelectedDate;
                 if (strActiveDate != null)
                 {                   
                     ParaPublishSelDate.strParaActiveDate =strActiveDate.ToString("yyyy-MM-dd");
                 }
             }
           
        }

       /* private void pb_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UIElement element = this.effectiveDate;
            if (element != null && element is DateTimePickerExtend)
            {
                DateTimePickerExtend cmb = element as DateTimePickerExtend;
                if (string.IsNullOrEmpty((sender as DatePicker).Text))
                    return;
                DateTime strPublishDate = (DateTime)(sender as DatePicker).SelectedDate;
                if (strPublishDate != null)
                {
                    ParaPublishSelDate.strParaPublishDate = strPublishDate.ToString("yyyy-MM-dd");
                }
            }

        }*/
 
        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_para_publish");
            DataSourceManager.DisponseDataSource("ds_prim_para_publish");
            ParaPublishSelDate.strParaActiveDate = "";
            ParaPublishSelDate.strParaPublishDate = "";
            this.effectiveDate.SetControlValue(null);
            //this.publishDate.SetControlValue(null);
            //this.effectiveDate.SetControlValue(string.Empty);
            //this.publishDate.SetControlValue(string.Empty);
        }
    }
}
