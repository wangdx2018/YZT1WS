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
using AFC.WS.BR;
using AFC.BOM2.UIController;
using AFC.BOM2.MessageDispacher;
using AFC.WS.Model.Const;
using System.Xml.Serialization;
using System.IO;
using AFC.WS.UI.Common;
using System.Data;
//using AFC.WS.UI.BR.Data.SysManager;
using AFC.WS.UI.CommonControls;
using System.Configuration;
using System.Xml;
using AFC.WS.UI.Convertors;
using AFC.WS.UI.BR.Data.SysManager;

namespace AFC.WS.UI.CoreUI
{
    /// <summary>
    /// SysConfigRewrite.xaml 的交互逻辑
    /// </summary>
    public partial class SysConfigRewrite : UserControlBase
    {
        BaseWindow BW;
        public SysConfigRewrite()
        {
            InitializeComponent();
        }
        public SysConfigRewrite(BaseWindow bw)
            : this()
        {
            InitLoad();
            if (null != bw)
            {
                this.BW = bw;
                this.BW.Title = "系统配置";
            }
        }

        private void InitLoad()
        {
           // this.btnSave.Click += new RoutedEventHandler(btnSave_Click);
            BindingData();
        }

        public void BindingData()
        {
           // ConvertToDecimalDevCode convertDecimal = new ConvertToDecimalDevCode();
            this.StationCode.Text = SysConfig.GetSysConfig().LocalParamsConfig.StationCode.ToString();
            this.DeviceCode.Text = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode.ToString();
            this.LocalIPaddress.Text = SysConfig.GetSysConfig().LocalParamsConfig.LocalIPaddress.ToString();
            this.ScIpAddress.Text = SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress.ToString();
            this.ScPort.Text = SysConfig.GetSysConfig().CommParamsConfig.ScPort.ToString();
            //this.ScRecPort.Text = SysConfig.GetSysConfig().CommParamsConfig.ScRecPort.ToString();
            //this.FtpIpAddress.Text = SysConfig.GetSysConfig().CommParamsConfig.FtpIpAddress.ToString();
            //this.FtpPort.Text = SysConfig.GetSysConfig().CommParamsConfig.FtpPort.ToString();
            this.DbConnectionString.Text = SysConfig.GetSysConfig().LocalParamsConfig.DBConnectionString;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = AFC.WS.UI.CommonControls.MessageDialog.Show("您是否要真的重启系统保存配置！", "警告", AFC.WS.UI.CommonControls.MessageBoxIcon.Warning, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
            if (message != MessageBoxResult.Yes)
            {
                return;
            }
            if (CheckValide())
            {
                try
                {
                    //ConvertToHexDevCode converHex = new ConvertToHexDevCode();
                    SysConfig changSysConfig = null;
                    SysConfigCheck check = new SysConfigCheck();
                    XmlSerializer serializer = XmlSerializer.FromTypes(new Type[] { typeof(SysConfig) })[0];
                    using (Stream s = File.Open(@".\Config\SysConfig.xml", FileMode.OpenOrCreate))
                    {
                        changSysConfig = serializer.Deserialize(s) as SysConfig;
                    }

                    changSysConfig.LocalParamsConfig.StationCode = this.StationCode.Text;
                    changSysConfig.LocalParamsConfig.DeviceCode = this.DeviceCode.Text;
                    changSysConfig.LocalParamsConfig.LocalIPaddress = this.LocalIPaddress.Text;
                    changSysConfig.CommParamsConfig.ScIPAddress = this.ScIpAddress.Text;
                    changSysConfig.LocalParamsConfig.DBConnectionString = this.DbConnectionString.Text;
                   // changSysConfig.CommParamsConfig.ScSendPort = this.ScSendPort.Text;
                   // changSysConfig.CommParamsConfig.ScRecPort = this.ScRecPort.Text;
                   // changSysConfig.CommParamsConfig.FtpIpAddress = this.FtpIpAddress.Text;
                   // changSysConfig.CommParamsConfig.FtpPort = this.FtpPort.Text;
                    
                    string DbIp = check.getDBIP(this.DbConnectionString.Text);
                    if (string.IsNullOrEmpty(DbIp))
                    {
                        this.labTip.Content = "数据库连接有误！";
                        return;
                    }
                    if (!DbIp.Equals(this.ScIpAddress.Text))
                    {
                        this.labTip.Content = "数据库服务器与SC服务器IP地址不一致！";
                        return;
                    }
                    
                    int ret = check.validDevice(this.DbConnectionString.Text, changSysConfig.LocalParamsConfig.DeviceCode, changSysConfig.LocalParamsConfig.LocalIPaddress);
                    
                    //int ret = 1;
                    if (ret == 1)
                    {

                        if (!string.IsNullOrEmpty(this.DbConnectionString.Text))
                        {
                            if (!this.DbConnectionString.Text.Equals(ConfigurationSettings.AppSettings["DBConnectionString"]))
                            {
                                RewriteAppConfig("DbConnectionString", this.DbConnectionString.Text);
                            }
                        }

                        int res = SysConfig.GetSysConfig().WrtieChangeSysConfigFile(changSysConfig);
                        if (res != 0)
                        {
                            this.labTip.Content = "保存系统配置信息失败,请重试!";
                        }
                        else
                        {
                            this.labTip.Content = "保存系统配置信息成功,请重启生效";

                            //无登录
                            if (string.IsNullOrEmpty(BuinessRule.GetInstace().brConext.CurrentOperatorId))
                            {
                                AFC.WS.UI.CommonControls.MessageDialog.Show("系统即将关闭!", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            }
                            //有登录
                            else
                            {
                                int result = BuinessRule.GetInstace().commProcess.LogOut(BuinessRule.GetInstace().brConext.CurrentOperatorId.ConvertNumberStringToUint());
                              
                                    if (result == 0)
                                    {
                                        AFC.WS.UI.CommonControls.MessageDialog.Show("登出成功!", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, MessageBoxButtons.Ok);
                                        BuinessRule.GetInstace().brConext.CurrentOperatorId = "";
                                    }
                                    else
                                    {
                                        AFC.WS.UI.CommonControls.MessageDialog.Show("登出失败!错误代码 " + result.ToString(), "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Error, MessageBoxButtons.Ok);
                                        BuinessRule.GetInstace().brConext.CurrentOperatorId = string.Empty;
                                    }

                                }
                                

                            }
                            System.Windows.Application.Current.Shutdown(0);
                        }

                 if (ret == 0)
                {

                 this.labTip.Content = "设备编码或者IP地址不存在";
                }

                if (ret == -1)
                {
                       this.labTip.Content = "数据库连接有误！";
                }


                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                    this.labTip.Content = "存在不合法的输入项!";//, "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Error, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                    return;
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (null != this.BW)
            {
                BW.Close();
            }
        }

        private void RewriteAppConfig(string section, string value)
        {
            try
            {
               // string path = "D:\\0803\\BJWSUI\\SCWS\\App.config";
                //string curPath = Environment.CurrentDirectory.ToString();
                //int k = curPath.LastIndexOf(@"\");
                //string myPath = curPath.Substring(0, k);
                //string path = myPath.Substring(0,myPath.LastIndexOf(@"\")) + @"\App.config";
                //string path = @".\SCWS.exe.config";
                string path = @".\" + SysConfig.GetSysConfig().LocalParamsConfig.SystemName + ".exe.config";
                XmlDocument xd = new XmlDocument();
                xd.Load(path);

                //如果没有appSetting，则添加 
                if (xd.SelectNodes("//appSettings").Count == 0)
                {
                    xd.DocumentElement.AppendChild(xd.CreateElement("appSettings"));
                }

                //判断节点是否存在，如果存在则修改当前节点 
                bool addNode = true;
                foreach (XmlNode xn1 in xd.SelectNodes("/configuration/appSettings/add"))
                {
                    if (xn1.Attributes["key"].Value == section)
                    {
                        addNode = false;
                        xn1.Attributes["value"].Value = value;
                        xd.Save(path); 
                        break;
                    }
                } 

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }
        }

        private bool CheckValide()
        {
            Boolean valide = false;
            if(string.IsNullOrEmpty(this.StationCode.Text))
            {
                this.labTip.Content = "请填写车站信息！";
                return false;
            }
            if (string.IsNullOrEmpty(this.DeviceCode.Text))
            {
                this.labTip.Content = "请填写设备编码！";
                return false;
            }
            if (string.IsNullOrEmpty(this.LocalIPaddress.Text))
            {
                this.labTip.Content = "请填写本机IP！";
                return false;
            }
            if (string.IsNullOrEmpty(this.ScIpAddress.Text))
            {
                this.labTip.Content = "请填写服务器IP！";
                return false;
            }
            if (string.IsNullOrEmpty(this.ScPort.Text))
            {
                this.labTip.Content = "请填写端口号！";
                return false;
            }
            /*
            if (string.IsNullOrEmpty(this.ScRecPort.Text))
            {
                this.labTip.Content = "请填写接收端口号！";
                return false;
            }
            if (string.IsNullOrEmpty(this.FtpIpAddress.Text))
            {
                this.labTip.Content = "请填写FTP服务器IP！";
                return false;
            }*/

            if (string.IsNullOrEmpty(this.DbConnectionString.Text))
            {
                this.labTip.Content = "请填写数据库配置信息";
                return false;
            }

            return true;
        }

        private void ScRecPort_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /*   public int validDevice(string connstring, string DeviceCode, string LocalIPaddress)
           {
               //1代表查询有数；0代表查询无；-1代表数据库连接失败
               int isExist = -1;
              // DBO dbo = null;
               try
               {
                   //获取消息中文名称
                   string SQL_DeviceInfo = "select * from  basi_dev_info t where t.device_id='" + DeviceCode + "' and t.device_ip ='" + LocalIPaddress + "'";

                   DataTable dt = null;
                   int retCode = 0;
                  // dbo = new DBO(connstring);
                 //  if (dbo != null)
                  // {
                      // DataSet ds = dbo.SqlQuery(out retCode, SQL_DeviceInfo);
                       //DataSet ds = Util.DataBase.SqlQuery(out res, string.Format("select t.para_file_name from basi_para_type_info t where t.para_type='{0}'", value.ToString()));
                       DataSet ds = Util.DataBase.SqlQuery(out res, string.Format("select * from  basi_dev_info t where t.device_id='{0}' and t.device_ip ='{1}'", DeviceCode, LocalIPaddress));                    
                   if (ds.Tables.Count > 0)
                       {
                           dt = ds.Tables[0];
                           if (dt != null && dt.Rows.Count > 0)
                           {
                               isExist = 1;
                           }
                           else
                           {
                               isExist = 0;
                           }

                       }
                       /*  else
                         {
                             if (retCode == -206)
                             {
                                 isExist = -1;
                             }
                             else
                             {
                                 isExist = 0;
                             }

                         }
                     }
                       return isExist;

               }
               catch (Exception ex)
               {
                   WriteLog.Log_Error(ex.ToString());
                   return -1;
               }
               finally
               {
                   if (dbo != null)
                   {
                       dbo.SqlClose();
                   }
               }

           }*/
    }
}
