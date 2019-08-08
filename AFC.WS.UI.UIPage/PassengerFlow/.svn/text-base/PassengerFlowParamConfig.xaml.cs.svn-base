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
using AFC.WS.BR.ParamsManager;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.BR.PassengerFlow;
using AFC.WS.Model.DB;
using AFC.WS.BR;

namespace AFC.WS.UI.UIPage.PassengerFlow
{
    /// <summary>
    /// PassengerFlowParamConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PassengerFlowParamConfig : UserControlBase
    {
        /// <summary>
        /// 定义一个窗体
        /// </summary>
        BaseWindow BW;
        /// <summary>
        /// 客流监视参数设置类
        /// </summary>
        PassengerFlowParamCfg pfpc;
        /// <summary>
        /// 保存是否成功。
        /// </summary>
        public bool IsSaveSuccess = false;
        /// <summary>
        /// 
        /// </summary>
        private int _Sign;
        /// <summary>
        /// 标志。0:时时客流；1：历史客流。
        /// </summary>
        public int Sign
        {
            get { return _Sign; }
            set { _Sign = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public PassengerFlowParamConfig()
        {
            InitializeComponent();
            InitLoad();
            //Sign = 0;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sign">标志。0:时时客流；1：历史客流。</param>
        public PassengerFlowParamConfig(int sign)
            : this()
        {
            this.Sign = sign;
            if (this.Sign == 1)
            {
                gpTimeSet.Visibility = Visibility.Hidden;
            }
            else
            {
                gpTimeSet.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// 设置窗体关闭退出
        /// </summary>
        /// <param name="w">窗體控件</param>
        public void SetWindowClose(BaseWindow w)
        {
            BW = w;
        }
        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            pfpc = SysConfig.GetSysConfig().PassengerFlowParamCfg;
            List<string> passList = new List<string>();
            List<string> checkList = new List<string>();
           

            List<PassFlowConfig> pcList = new List<PassFlowConfig>();
            foreach (var cfg in pfpc.MonitorParamItems)
            {
                    passList.Add(cfg.Value);
                    if (cfg.IsMonitor)
                    {
                        checkList.Add(cfg.Value);
                    }               
            }
            this.nudTimeInterval.Value = Convert.ToDecimal(pfpc.TimeInterval);
            this.nudRefurbishInterval.Value = Convert.ToDecimal(pfpc.RefurbishInterval);
            this.nudPagePointInterval.Value = Convert.ToDecimal(pfpc.PagePoint);
            this.ParamSet.DataBindType = DataBindType.ListBind;
            this.ParamSet.ListName = passList;
            this.ParamSet.SetCheckedList = checkList;
            this.ParamSet.ColumnBindCheckBoxCount = pfpc.ColumnBindCheckBoxCount;
            this.ParamSet.ColumnWidth = pfpc.ColumnWidth;
            this.ParamSet.RowIntervalHeight = pfpc.RowIntervalHeight;
            this.ParamSet.ColumnIntervalWidth = pfpc.ColumnIntervalWidth;
            this.ParamSet.StackPanelHorizontalAlignment = HorizontalAlignment.Center;
            this.ParamSet.CheckBoxVerticalAlignment = VerticalAlignment.Top;
        }

        /// <summary>
        /// 重写控件UnLoad的方法
        /// </summary>
        public override void UnLoadControls()
        {
            base.UnLoadControls();
        }
        /// <summary>
        /// 保存修改操作。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            switch (Sign)
            {
                case 0:
                    SaveSet();
                    break;
                case 1:
                    SaveHistorySet();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 取消修改操作。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件類</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsSaveSuccess = false;
            BW.Close();
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        private void SaveHistorySet()
        {
            PassengerFlowParamCfg cfg = new PassengerFlowParamCfg();
            List<string> valueItem = this.ParamSet.GetValueList;
            if (valueItem.Count == 0)
            {
                Wrapper.ShowDialog("请选择要监视的客流类型");
                return;
            }
            List<PassFlowConfig> pcList = new List<PassFlowConfig>();
            pcList.AddRange(pfpc.MonitorParamItems);
            foreach (PassFlowConfig var in pcList)
            {
                bool isMonitor = false;
                foreach (string str in valueItem)
                {
                    if (str == var.Value)
                    {
                        isMonitor = true;
                        break;
                    }
                }
                var.IsMonitor = isMonitor;
            }
            cfg.MonitorParamItems = pcList;
            cfg.RefurbishInterval = Convert.ToInt32(this.nudRefurbishInterval.Value);
            cfg.TimeInterval = Convert.ToInt32(this.nudTimeInterval.Value);
            cfg.PagePoint = Convert.ToInt32(this.nudPagePointInterval.Value);
            PassengerFlowHelper.HistoryCfg = cfg;
            this.IsSaveSuccess = true;
            BW.Close();
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        private void SaveSet()
        {
            List<string> valueItem = this.ParamSet.GetValueList;

            if (valueItem.Count == 0)
            {
                Wrapper.ShowDialog("请选择要监视的客流类型");
                return;
            }
            foreach (PassFlowConfig var in pfpc.MonitorParamItems)
            {
                bool isMonitor = false;
                foreach (string str in valueItem)
                {
                    if (str == var.Value)
                    {
                        isMonitor = true;
                        break;
                    }
                }
                var.IsMonitor = isMonitor;
            }
            pfpc.RefurbishInterval = Convert.ToInt32(this.nudRefurbishInterval.Value);
            pfpc.TimeInterval = Convert.ToInt32(this.nudTimeInterval.Value);
            pfpc.PagePoint = Convert.ToInt32(this.nudPagePointInterval.Value);

            if (SysConfig.GetSysConfig().WrtieSysConfigFile() == 0)
            {
                IsSaveSuccess = true;
                Wrapper.ShowDialog("参数设置保存成功");
                BW.Close();
            }
            else
            {
                Wrapper.ShowDialog("参数设置保存失败");
            }
        }
    }
}
