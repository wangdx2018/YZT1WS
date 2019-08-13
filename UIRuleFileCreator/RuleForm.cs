using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Xml;
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;
//using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 规则文件生成器界面，提供开发人员使用的界面。
    /// </summary>
    public partial class RuleForm : Form
    {
        #region --> Property 
        /// <summary>
        /// 是否预览
        /// </summary>
        private bool IsPreview = false;
        /// <summary>
        /// 修改规则文件类型列表。
        /// </summary>
        List<ItemCom> RuleFileList = null;
        /// <summary>
        /// 修改规则文件类型列表。
        /// </summary>
        List<ItemCom> ModifyRuleFileList = null;
        /// <summary>
        /// 旧的查询语句。
        /// </summary>
        string oldSqlSentence = string.Empty;

        /// <summary>
        /// 以前所选表名称。
        /// </summary>
        private string OldTableName = string.Empty;
        /// <summary>
        /// 存放来至于事件传过来的SQL语句。
        /// </summary>
        string oldEvenSql = string.Empty;

        #endregion --> Property

        #region --> Init 
        /// <summary>
        /// 构造方法
        /// </summary>
        public RuleForm()
        {
            InitializeComponent();

            InitLoading();
        }

        /// <summary>
        /// 初始化加载方法。
        /// </summary>
        private void InitLoading()
        {
            Utility.ImportLogDll();
            
            this.listBoxTable.DataSource = DbHelper.Instance.GetAllTables();
            RuleFileList = RuleHelper.Instance.RuleFileTypeList();
            ModifyRuleFileList = RuleHelper.Instance.RuleFileTypeList(); 
            this.cbbRuleItem.DataSource = RuleFileList;
            Utility.Instance.JudgeDataSourceNameIsExistEvent += new DelegateJudgeDataSourceIsExist(JudgeDataSourceNameIsExistEvent);
            Utility.Instance.PreviewFormCloseEvent += new DelegatePreviewFormClose(PreviewFormCloseEvent);
            Utility.Instance.SqlSetMethodEvent += new DelegateSqlSetMethod(SqlSetMethodEvent);
            Utility.Instance.RefurbishPropertyGridEvent += new DelegateRefurbishHandler(RefurbishPropertyGridEvent);
        }
        /// <summary>
        /// 刷新PropertyGrid方法。
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e">用于刷新propertyGrid的事件类对象。</param>
        void RefurbishPropertyGridEvent(object sender, RefurbishPropertyGridEventArgs e)
        {
            if (e != null)
            {
                if (e.CanRefurbish)
                {
                    this.pgRuleFile.Refresh();
                }
            }
        }

        #endregion --> Init

        #region --> Event
        /// <summary>
        /// 设置SQL的语的事件.
        /// </summary>
        /// <param name="sender">object对象,可写null 或this,或别的.</param>
        /// <param name="e">事件类。</param>
        void SqlSetMethodEvent(object sender, SqlSentenceEventArgs e)
        {
            if (e != null)
            {
                if (e.Flag == EventFlag.SetToTextBox)
                {
                    if (string.IsNullOrEmpty( e.SqlSentence) )
                    {
                        return;
                    }
                    StringBuilder sb = new StringBuilder();

                    string groupby = string.Empty;
                    string orderby = string.Empty;
                    string where = string.Empty;
                    string having = string.Empty;

                    object obj = e.Target;
                    Type t = obj.GetType();
                    PropertyInfo[] piList = t.GetProperties();

                    foreach (PropertyInfo pi in piList)
                    {
                        object temp;

                        temp = pi.GetValue(obj, null);

                        #region --> Params

                        switch (pi.Name.ToLower())
                        {
                            case "groupparams":

                                groupby = temp != null ? temp.ToString() : null;

                                break;

                            case "havingparams":

                                having = temp != null ? temp.ToString() : null;

                                break;

                            case "whereparams":

                                where = temp != null ? temp.ToString() : null;

                                break;

                            case "orderbyparams":

                                orderby = temp != null ? temp.ToString() : null;

                                break;

                            default :

                                break;
                        }
                        #endregion --> Params
                    }
                    //-->将查询语句显示出来。
                    sb.Append(e.SqlSentence);
                    //-->判断是否有where。
                    if (!string.IsNullOrEmpty(where))
                    {
                        sb.Append(" where ").Append(where);
                    }
                    //-->判断是否配置了group by 参数
                    if (!string.IsNullOrEmpty(groupby))
                    {
                        sb.Append(" Group By ").Append(groupby);
                    }
                    //-->判断是否配置了Order by 参数。
                    if (!string.IsNullOrEmpty(orderby))
                    {
                        sb.Append(" Order by ").Append(orderby);
                    }
                    //-->判断是否配置了Having参数。
                    if (!string.IsNullOrEmpty(having))
                    {
                        sb.Append(" Having ").Append(having);
                    }

                    this.rtbSqlSentence.Text = sb.ToString();

                    JudgeSqlSentenceIsRight(sb.ToString());
                }
            }
        }
        /// <summary>
        /// 判断数据源名称是否存在。
        /// </summary>
        /// <param name="datasourceName">数据源名称</param>
        void JudgeDataSourceNameIsExistEvent(string datasourceName)
        {
            if (IsPreview==false)
            {
                string SaveFile = RuleHelper.Instance.FileSavePathDataSource + "\\" + datasourceName + ".xml";
                if (!this.chbIsModify.Checked)
                {
                    RuleHelper.Instance.JudgeFileIsExist(SaveFile);//, datasourceName);
                }
            }
        }
        /// <summary>
        /// 判断SQL语句是否合法。 
        /// </summary>
        /// <param name="sqlSentence">SQL语句</param>
        void JudgeSqlSentenceIsRight(string sqlSentence)
        {
            if( string.IsNullOrEmpty( sqlSentence.Trim() ))
            {
                btnSetDataSourceSql.Enabled = false;
                return;
            }
            
            //-->判断是否是查询语句。
            string[] sqlArray = sqlSentence.Trim().Split(' ');

            if (sqlArray[0].ToLower() != "select")
            {
                btnSetDataSourceSql.Enabled = false;
                lblSqlSentenceMessage.Text = "输入的SQL语有误。";

                dgvTableField.DataSource = new List<TableFieldProperty>();
            }
            else
            {
                if (!sqlSentence.Trim().Equals(oldSqlSentence))
                {
                    int retCode = 0;

                    List<TableFieldProperty> TableFieldList = DbHelper.Instance.GetFieldListBySqlSentence(out retCode, sqlSentence);

                    if (retCode == -206)
                    {
                        btnSetDataSourceSql.Enabled = false;
                        lblSqlSentenceMessage.Text = "表或视图不存在";
                    }
                    else if( retCode != 0)
                    {
                        btnSetDataSourceSql.Enabled = false;
                        lblSqlSentenceMessage.Text = "输入的查询语句，语法不对。";
                    }
                    else
                    {
                        btnSetDataSourceSql.Enabled = true;
                        lblSqlSentenceMessage.Text = "";
                    }
                    if (TableFieldList != null)
                    {
                        this.dgvTableField.DataSource = TableFieldList;
                    }

                    oldSqlSentence = sqlSentence.Trim() ;
                }
            }
        }
        /// <summary>
        /// 预览窗体
        /// </summary>
        /// <param name="value">false:窗体关闭</param>
        void PreviewFormCloseEvent(bool value)
        {
            IsPreview = value;
        }
        #endregion --> Event

        #region --> Control Event
        /// <summary>
        /// listBox选择表名字事件.
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void listBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string tableName = this.listBoxTable.SelectedValue.ToString();

            if (OldTableName != tableName)
            {

                List<TableFieldProperty> TableFieldList = DbHelper.Instance.FieldListByTableName(tableName);

                this.dgvTableField.DataSource = TableFieldList;

                this.rtbSqlSentence.Text = DbHelper.Instance.GetSqlSentence(TableFieldList, tableName);

                OldTableName = tableName;
            }
        }
        /// <summary>
        /// 查询SQL语句事件.
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnQuerySentence_Click(object sender, EventArgs e)
        {
            JudgeSqlSentenceIsRight(this.rtbSqlSentence.Text.Trim());
        }
        /// <summary>
        /// ComboBox的SelectedIndex事件.
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void cbbRuleItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtFileName.Text = "";

            if (cbbRuleItem.Text.Trim().ToLower() != "chart")
            {
                //-->判断DLL是否已经导入。
                if (cbbRuleItem.SelectedIndex != 0)
                {
                    if (listBoxDll.Items.Count == 0 || listBoxDll.DataSource == null)
                    {
                        cbbRuleItem.SelectedIndex = 0;
                        MessageBox.Show("请选择要导入的DLL", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            ItemCom il = this.cbbRuleItem.SelectedItem as ItemCom;

            this.pgRuleFile.SelectedObject = il.Tag;

            if (!this.chbIsModify.Checked)
            {
                Utility.Instance.IsModify = false;

                txtFileName.Enabled = true;

                if (il.Name.ToLower().Equals("datasource"))
                {
                    txtFileName.Enabled = false;
                }//如果选择得是数据源时，txtFileName不用输入要保存的文件名称。
            }

            if (this.cbbRuleItem.Text.Trim().ToLower() == "datasource")
            {
                this.btnPreviewXmlFile.Enabled = false;
            }
            else
            {
                this.btnPreviewXmlFile.Enabled = true;
            }
            
        }
        /// <summary>
        /// 导入DLL事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnImportDll_Click(object sender, EventArgs e)
        {
            string[] filePathArray;

            this.ofdDll.Filter = "选择DLL(*.dll)|*.dll";
            this.ofdDll.FilterIndex = 1;
            this.ofdDll.Multiselect = true;
            this.ofdDll.InitialDirectory = Application.StartupPath;

            this.ofdDll.Title = "请选择要导入的DLL";

            if (this.ofdDll.ShowDialog(this) == DialogResult.OK)
            {
                filePathArray = this.ofdDll.FileNames;

                RuleHelper.Instance.ImportDll(filePathArray, RuleFileList);

                if (this.listBoxDll.DataSource != null)
                {
                    this.listBoxDll.DataSource = null;
                }
                this.listBoxDll.DataSource = RuleHelper.Instance.ICList;// RuleHelper.Instance.DllPropertyList;

            }

            
        }
        /// <summary>
        /// 生成规则文件事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnBuildFile_Click(object sender, EventArgs e)
        {
            ItemCom il = new ItemCom();
            il.Tag = this.pgRuleFile.SelectedObject;
            il.Name = cbbRuleItem.Text;

            string txtFileName = GetFileName(il);

            if (string.IsNullOrEmpty(txtFileName))
            {
                return;
            }

            if (RuleHelper.Instance.Save(il, txtFileName, this.chbIsModify.Checked))
            {
                MessageBox.Show(this, "规则文件生成成功。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "规则文件生成失败。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 获取文件名称.
        /// </summary>
        /// <param name="il">ItemCom类对象</param>
        /// <returns>返回文件名称.</returns>
        private string GetFileName(ItemCom il )
        {
            string fileName = string.Empty;

            if (this.chbIsModify.Checked)
            {
                if (this.cbbRuleItem.Text.ToLower() == "datasource")
                {
                    DataSourceRule dsr = il.Tag as DataSourceRule;
                    
                    string dataSourceName = this.txtRuleFilePath.Text.Substring(this.txtRuleFilePath.Text.LastIndexOf("\\") + 1, this.txtRuleFilePath.Text.LastIndexOf(".") - this.txtRuleFilePath.Text.LastIndexOf("\\") - 1);

                    if (!dataSourceName.Equals(dsr.DataSourceName))
                    {
                        MessageBox.Show(this, "数据源名称必须与生成的xml文件名一致", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        fileName = "";
                    }
                    else
                    {
                        fileName = this.txtRuleFilePath.Text.Trim();
                    }
                }
                else
                {
                    fileName = this.txtRuleFilePath.Text.Trim();

                    if (String.IsNullOrEmpty(fileName))
                    {
                        MessageBox.Show(this, "请选择要修改的规则文件。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            else
            {
                if (this.cbbRuleItem.Text.ToLower() == "datasource")
                {
                    DataSourceRule dsr = il.Tag as DataSourceRule;
                    fileName = dsr.DataSourceName;
                }
                else
                {
                    fileName = this.txtFileName.Text.Trim();

                    if (string.IsNullOrEmpty(fileName))
                    {
                        MessageBox.Show(this, "请输入规则文件名称。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            return fileName;
        }
        /// <summary>
        /// 获取规则文件路径事件.
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnRuleFilePath_Click(object sender, EventArgs e)
        {
            if (this.cbbRuleItem.SelectedIndex > 0)
            {
                this.ofdXML.Filter = "选择(*.xml)|*.xml";
                
                this.ofdXML.FilterIndex = 1;

                if (this.ofdXML.ShowDialog(this) == DialogResult.OK)
                {
                    txtRuleFilePath.Text = this.ofdXML.FileName;

                    this.txtRuleFilePath.Tag = this.ofdXML.SafeFileName;

                    //-->导入规则文件时操作。
                    Utility.Instance.IsModify = true;

                    ItemCom il = this.cbbRuleItem.SelectedItem as ItemCom;

                    this.pgRuleFile.SelectedObject = RuleHelper.Instance.Loading(il, this.txtRuleFilePath.Text.Trim());

                    for (int i = 0; i < RuleFileList.Count; i++)
                    {
                        if (RuleFileList[i].Name == il.Name)
                        {
                            RuleFileList[i] = il;
                            break;
                        }
                    }
                    this.cbbRuleItem.DataSource = RuleFileList;
                    this.cbbRuleItem.Text = il.Name;
                    btnBuildFile.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show(this, "请选择规则文件类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 开始修改控件事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnStartModify_Click(object sender, EventArgs e)
        {
            Utility.Instance.IsModify = false;
        }
        /// <summary>
        /// 是否修改事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void chbIsModify_CheckedChanged(object sender, EventArgs e)
        {
            if (chbIsModify.Checked)
            {
                txtFileName.Enabled = false;
                this.btnStartModify.Enabled = true;
                this.txtRuleFilePath.Enabled = true;
                this.btnRuleFilePath.Enabled = true;
                this.btnBuildFile.Enabled = false;
            }
            else
            {
                this.btnBuildFile.Enabled = true;
                this.txtFileName.Enabled = true;
                this.btnStartModify.Enabled = false;
                this.txtRuleFilePath.Enabled = false;
                this.btnRuleFilePath.Enabled = false;

                Utility.Instance.IsModify = false;

                this.txtRuleFilePath.Text = string.Empty;
            }

            if (this.cbbRuleItem.Text.ToLower() == "datasource")
            {
                this.txtFileName.Enabled = false;
            }
        }
        /// <summary>
        /// 将DataGridView里的内容放到DataList中的.ColumnList里去.
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnTableFieldOK_Click(object sender, EventArgs e)
        {
            //-->判断当前选择中得是否是DataList;
            ItemCom ic = this.cbbRuleItem.SelectedItem as ItemCom;

            if (ic.Name.ToLower().Equals("datalist"))
            {
                DataListRule dlr = ic.Tag as DataListRule;

                dlr.ColumnList.Clear();

                List<TableFieldProperty> tfpList = this.dgvTableField.DataSource as List<TableFieldProperty>;

                foreach (TableFieldProperty tfp in tfpList)
                {
                    ColumnProperty cp = new ColumnProperty();
                    if (String.IsNullOrEmpty(tfp.BindingField))
                    {
                        cp.BindingField = tfp.ColumnName;
                    }
                    else
                    {
                        cp.BindingField = tfp.BindingField;
                    }
                    cp.HeaderName = tfp.Comments;
                    dlr.ColumnList.Add(cp);
                }

            }
            else
            {
                MessageBox.Show(this, "请将规则文件选择为[DataList]类型。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 规则文件预览
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnPreviewXmlFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cbbRuleItem.SelectedIndex > 0)
                {
                    ItemCom ic = new ItemCom();
                    ic.Name = this.cbbRuleItem.Text;
                    ic.Tag = this.pgRuleFile.SelectedObject;
                    IsPreview = true;
                    RuleHelper.Instance.Preview(ic);
                }
                else
                {
                    MessageBox.Show(this, "请选择规则文件类型。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }
        /// <summary>
        /// 把TextBox中的Sql语句设置到DataSource上去事件。
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void btnSetDataSourceSql_Click(object sender, EventArgs e)
        {
            JugdeCondition();
        }
        /// <summary>
        /// 针对将RichTextBox里输入的SQL语句，放到DataSource 中的sql属性里去的条件判断。
        /// </summary>
        private void JugdeCondition()
        {
            //bool result = false;
            bool isImportDatasource = false;
            ////-->判断SQL语句是否合法。
            //->判断DataSource DLL是否改导入进去了。
            if (RuleHelper.Instance.DllPropertyList.Count > 0)
            {
                foreach (DllProperty dp in RuleHelper.Instance.DllPropertyList)
                {
                    if (dp.Namespace.Contains("DataSource"))
                    {
                        //-->判断规则文件类型是否选择得是DataList
                        if (this.cbbRuleItem.Text.ToLower().Equals("datasource"))
                        {
                            //-->判断"选择DataSource"项是否为空。
                            DataSourceRule dsr = this.pgRuleFile.SelectedObject as DataSourceRule;

                            if (!String.IsNullOrEmpty(dsr.ComboBoxDataSource))
                            {
                                //-->查询键值对里是否有SQL。

                                //Utility.Instance.SetSqlSentence(this.rtbSqlSentence.Text.Trim());
                                //Utility.Instance.SqlSetMethod(EventFlag.SetToDataSourceSQL, this, this.rtbSqlSentence.Text.Trim());
                                Utility.Instance.SqlSetMethod(this, 
                                    new SqlSentenceEventArgs(EventFlag.SetToDataSourceSQL, this, this.rtbSqlSentence.Text.Trim()));
                            }
                            else
                            {
                                MessageBox.Show(this, "请选择一个[选择DataSource]项名称。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }//End 
                        }
                        else
                        {

                            MessageBox.Show(this, "请将规则文件类型改为\"DataSource\"类型。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        isImportDatasource = true;
                        break;
                    }
                }//End foreach;

                if (!isImportDatasource)
                {
                    MessageBox.Show(this, "没有导入数据源DLL。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(this, "请选择要导入的DLL", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //return result;
        }

        /// <summary>
        /// 将DataGridView里的内容放到UIConfig中的ControlList里去.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImoprtUIConfig_Click(object sender, EventArgs e)
        {

            //-->判断当前选择中得是否是UIConfig;
            ItemCom ic = this.cbbRuleItem.SelectedItem as ItemCom;

            if (ic.Name.ToLower().Equals("uiconfig"))
            {
                InteractiveControlRule icr = ic.Tag as InteractiveControlRule ;

                icr.ControlList.Clear();

                List<TableFieldProperty> tfpList = this.dgvTableField.DataSource as List<TableFieldProperty>;

                foreach (TableFieldProperty tfp in tfpList)
                {
                    ControlProperty cp = new ControlProperty();

                    if (String.IsNullOrEmpty(tfp.BindingField))
                    {
                        cp.BindingField = tfp.ColumnName;
                    }
                    else
                    {
                        cp.BindingField = tfp.BindingField;
                    }
                    if (String.IsNullOrEmpty(tfp.Comments))
                    {
                        cp.Lable = cp.BindingField;
                    }
                    else
                    {
                        cp.Lable = tfp.Comments;
                    }

                    cp.ControlName = "btn_" + tfp.BindingField.Replace('.', '_');
                    icr.ControlList.Add(cp);
                }

            }
            else
            {
                MessageBox.Show(this, "请将规则文件选择为[UIConfig]类型。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion --> Control Event
    }
}