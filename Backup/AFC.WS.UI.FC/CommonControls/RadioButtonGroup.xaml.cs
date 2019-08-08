#region [       Using namespaces       ]
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
using System.ComponentModel;
using AFC.WS.UI.Common;
#endregion

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// RadioButtonGroup.xaml 的交互逻辑
    /// </summary>
    public partial class RadioButtonGroup : UserControl, ICommonEdit
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RadioButtonGroup()
        {
            InitializeComponent();
        }

        #region ICommonEdit 成员

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得控件值
        /// </summary>
        /// <returns></returns>
        public object GetControlValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="value"></param>
        public void SetControlValue(object value)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
