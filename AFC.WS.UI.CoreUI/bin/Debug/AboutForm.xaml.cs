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
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.BOM2.UIController;
//using AFC.WS.UI.BR.Data.ParameterManager;
using AFC.WS.UI.BR.Data;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.BR.ParamsManager;

namespace AFC.WS.UI.CoreUI
{
    /// <summary>
    /// AboutForm.xaml 的交互逻辑
    /// </summary>
    public partial class AboutForm : UserControlBase
    {

        BaseWindow BW;

        public AboutForm()
        {
            InitializeComponent();
        }

        public AboutForm(BaseWindow bw)
            : this()
        {
            InitLoad();
            if (null != bw)
            {
                this.BW = bw;
                this.BW.Title = "关于";
                BW.KeyDown += new KeyEventHandler(AboutForm_KeyDown);
            }
        }

        void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                btnOK_Click(sender, null);
            }
        }

        void InitLoad()
        {
            //ParaVersionInfo version = new ParaVersionInfo();
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                string staionName = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
                this.lblClientName.Content = staionName + "工作站";
                //ParaManager pm = new ParaManager();
                this.lblClientVersion.Content = BuinessRule.GetInstace().GetCurrentVersionPara().para_version;
               // version = pm.GetCurrentParaVersionInfo("4310");
            }
            else
            {
                string lineName = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
                this.lblClientName.Content = lineName+"工作站";
                this.lblClientVersion.Content = BuinessRule.GetInstace().GetCurrentVersionPara().para_version;
                //ParaManager pm1 = new ParaManager();
                //version = pm1.GetCurrentParaVersionInfo("4311");
            }
            //ParaVersionSynInfo version = ParameterManagerHelper.Instance.GetParaVersionSynInfo();
            //int version = 0;
            //bool versionNumber = GetVersionInfo(ref version);
            //this.lblClientVersion.Content = version.para_version;// version != null ? version.para_version.ToString() : "1.0";
            this.lblCurrentRunDate.Content = BuinessRule.GetInstace().rm.GetRunDate();
        }

        private ParaVersionInfo GetParaVersionInfo(string p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取当前版本信息
        /// 
        /// para_version_syn_info数据库表
        /// 
        /// para_test_state='0' and para_vesion_state='0'
        /// </summary>
        /// <returns>true:有新版本软件，false:无新版本软件</returns>
      /*  private bool GetVersionInfo(ref int verNum)
        {
            try
            {
                DataSet dataSet = new DataSet();
                DataSet ds = null;
                List<ParaVersionSynInfo> paraVersionSynInfo = ParameterManagerHelper.Instance.GetParaVersionSynInfo(out ds);
                if (paraVersionSynInfo == null)
                    return false;
                if (paraVersionSynInfo.Count == 0)
                    return false;
                if (ds == null)
                {
                    AFC.WS.UI.Common.WriteLog.Log_Error("数据库表para_version_syn_info无版本信息!");
                    return false;
                }
                string path = Environment.CurrentDirectory;
                //若存在，则读取其内容。
                dataSet.ReadXml(path + @"\VersionFile\WsHistory\WsFileVerInfo.xml");

                if (dataSet.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        if (dataSet.Tables[0].Rows[i]["para_type_code"] != null && dataSet.Tables[0].Rows[i]["para_version"] != null)
                        {
                            string paraTypeCode = dataSet.Tables[0].Rows[i]["para_type_code"].ToString();

                            int paraVersion = Convert.ToInt32(dataSet.Tables[0].Rows[i]["para_version"]);

                            if (paraTypeCode.Equals(paraVersionSynInfo[i].para_type_code))
                            {
                                verNum = paraVersion;
                                if (paraVersion < paraVersionSynInfo[i].para_version)
                                {

                                    return true;
                                }
                                else
                                    return false;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error("GetVersionInfo函数异常：" + ex.ToString());
            }
            return false;
        }
        */
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (null!= this.BW )
            {
                BW.Close();
            }
        }
    }
}
