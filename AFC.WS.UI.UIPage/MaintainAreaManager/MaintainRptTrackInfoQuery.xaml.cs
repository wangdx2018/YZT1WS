﻿using System;
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

namespace AFC.WS.UI.UIPage.MaintainAreaManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
   
    public partial class MaintainRptTrackInfoQuery : UserControlBase
    {
        public MaintainRptTrackInfoQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_maintain_rpt_track_info.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_maintain_rpt_track_info.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
            if (dlr != null)
            {
                this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.StatusColorSetting());
                this.list.Initliaize(dlr);
            }
        }

        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_maintain_rpt_track_info.xml");
            //base.UnLoadControls();
        }
    }
}
