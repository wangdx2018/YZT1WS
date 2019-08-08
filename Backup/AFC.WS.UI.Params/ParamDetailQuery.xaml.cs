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
using System.Data;
using System.IO;

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR.ParamsManager;
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.DataSources;

   

    /// <summary>
    /// 参数详细信息查询 ，added by wangdx 
    /// date 20110328
    /// ？？SC参数查询，和LC参数查询是否相同，参数查询和参数编辑是否需要编辑
    /// 两个规则文件呢？？
    /// </summary>
    public partial class ParamDetailQuery : UserControlBase
    {

        public ParamDetailQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 参数ID
        /// </summary>
        private string paramType;

        /// <summary>
        /// 参数版本号
        /// </summary>
        private string para_version;

        /// <summary>
        /// 参数配置
        /// </summary>
        private ParamConfig config = null;

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;

            try
            {
                paramType = list.Single(temp => temp.bindingData.Equals("t.para_type")).value.ToString();//todo Get QueryCondition List data
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(paramType))
                {
                    paramType = list.Single(temp => temp.bindingData.Equals("para_type")).value.ToString();//todo Get 
                }
            }

            para_version = list.Single(temp => temp.bindingData.Equals("para_version")).value.ToString();

            config = BuinessRule.GetInstace().paraManager.GetParamConfig(paramType);

            CreateTab(config, para_version);
            /* 
             * CreateTab
             * 
             * 
             * */
            
        }

        /// <summary>
        /// 创建TabItem项
        /// </summary>
        /// <param name="config">参数配置</param>
        /// <param name="versionNo">版本号</param>
        public void CreateTab(ParamConfig config,string versionNo)
        {
            for (int i = 0; i < config.ItemList.Count; i++)
            {
                try
                {
                    TabItem item = new TabItem();
                    item.Header = config.ItemList[i].Description;
                    if (!string.IsNullOrEmpty(config.ItemList[i].ControlType))
                    {
                        UserControlBase ucb = Activator.CreateInstance(Type.GetType(config.ItemList[i].ControlType)) as UserControlBase;
                        if (ucb != null)
                        {
                            string tableName = config.ItemList[i].TableName;
                            string cmd = string.Format("select * from {0} where para_version='{1}'", tableName, versionNo);
                            int res = 0;
                            DataTable dt = Util.DataBase.SqlQuery(out res, cmd).Tables[0]; //edited by wangdx 20111220
                            ucb.Tag = dt;
                            ucb.InitControls();
                            item.Content = ucb;
                        }
                        this.tc.Items.Add(item);
                        continue;
                    }
                    if (config.ItemList[i].Style == AFC.WS.BR.ParamsManager.Style.List)
                    {
                        if (File.Exists(config.ItemList[i].FileName))
                        {

                            DataListRule dlr = Utility.Instance.GetDataListObject(@config.ItemList[i].FileName);
                            if (dlr == null)
                            {
                                item.Content = new Label { Content = "Load" + config.ItemList[i].FileName + " error!" };
                            }
                            else
                            {
                                DataListControl dlc = new DataListControl();
                                dlc.Initliaize(dlr);
                                IDataSource dataSource = DataSourceManager.LookupDataSourceByName(dlr.DataSourceName);
                                if (dataSource != null &&
                                    dataSource is DefaultDataSource)
                                {
                                    DefaultDataSource dds = dataSource as DefaultDataSource;
                                    dds.WhereParams = string.Format("t.para_version='{0}'", para_version);
                                    DataSourceManager.NotfiyDataSourceChange(dlr.DataSourceName);
                                }
                                item.Content = dlc;
                            }
                        }
                        else
                        {
                            item.Content = new Label { Content = config.ItemList[i].FileName + " not exist!" };
                        }
                        this.tc.Items.Add(item);
                        continue;
                    }
                    if (config.ItemList[i].Style == AFC.WS.BR.ParamsManager.Style.Struct)
                    {
                        string tableName = config.ItemList[i].TableName;
                        string cmd = string.Format("select * from {0} where para_version='{1}'", tableName, versionNo);
                        int res = 0;
                        DataTable dt = Util.DataBase.SqlQuery(out res, cmd).Tables[0];
                   
                        InteractiveControlRule icr = Utility.Instance.GetInteractiveControlObject(@config.ItemList[i].FileName);
                        if (icr == null)
                        {
                            item.Content = new Label { Content = "Load" + config.ItemList[i].FileName + " error!" };
                            break;
                        }
                        for (int j = 0; j < icr.ControlList.Count; j++)
                        {
                            string value = icr.ControlList[j].BindingField.Contains('.') ? icr.ControlList[j].BindingField.Split('.')[1] : icr.ControlList[j].BindingField;
                            if (res == 0 && dt.Rows.Count == 0)
                            {
                                item.Content = new Label { Content = "table [" + tableName + "] not find data version=[" + para_version + "]" };
                                break;
                            }
                            icr.ControlList[j].InitValue = dt.Rows[0][value].ToString();
                        }
                        InteractiveControl ic = new InteractiveControl();
                        ic.Initialize(icr);
                        item.Content = ic;
                        this.tc.Items.Add(item);
                        continue;
                    }
                  
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                    continue;
                }
               
               

            
            }
        }

        public override void UnLoadControls()
        {
            foreach (var temp in this.config.ItemList)
            {
                if (!string.IsNullOrEmpty(temp.DataSourceName))
                {
                    DataSourceManager.DisponseDataSource(temp.DataSourceName);
                }
            }
            //base.UnLoadControls();
        }


       
    }
}
