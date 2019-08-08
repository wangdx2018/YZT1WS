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
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using System.Collections;
using System.Data;

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using System.Data;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;

    /// <summary>
    /// Para4043ValueAddAmountType.xaml 的交互逻辑
    /// </summary>
    public partial class Para4043ValueAddAmountType : UserControlBase
    {
        public Para4043ValueAddAmountType()
        {
            InitializeComponent();
        }

        //public void Initialize()
        //{

        //}

        public override void InitControls()
        {
 
        }

        public void SetVisible()
        {
            this.checkBoxType2.Visibility = Visibility.Collapsed;
            this.checkBoxType3.Visibility = Visibility.Collapsed;
            this.checkBoxType4.Visibility = Visibility.Collapsed;
            this.checkBoxType5.Visibility = Visibility.Collapsed;
            this.checkBoxType6.Visibility = Visibility.Collapsed;
            this.btnGetValue.Visibility = Visibility.Collapsed;

 
        }

        private void btnGetValue_Click(object sender, RoutedEventArgs e)
        {

        }

        //public object GetControlValue()
        //{
        //    return null;
 
        //}

        //public void SetControlValue(object value)
        //{
 
        //}
    }
}
