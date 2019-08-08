using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.CodeDom.Compiler;
using System.CodeDom;
using AFC.WS.UI.Config;
using System.Reflection;
using AFC.WS.UI.UIRuleFileCreator;
using System.Configuration;
using AFC.WS.UI.Common;
using System.IO;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TestForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public TestForm()
        {
            InitializeComponent();
            InitLoad();
        }

        private void InitLoad()
        {
            clbTableName.Items.Clear();
            List<string> tableNameList = DbHelper.Instance.GetAllTables();
            foreach (string str in tableNameList)
            {
                clbTableName.Items.Add(str.ToLower(),false);
            }
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            string nameSpace = this.txtNamespace.Text.Trim();
            if (String.IsNullOrEmpty(nameSpace))
            {
                nameSpace = "AFC.WS.UI.BR.Data";
            }
            string dirPath = this.txtDirecotry.Text.Trim();
            if (String.IsNullOrEmpty(dirPath))
            {
                dirPath = @"Codes";
            }
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            bool isBulid = false;
            for (int i = 0; i < this.clbTableName.Items.Count; i++)
            {
                if (this.clbTableName.GetItemChecked(i))
                {
                    isBulid = true;
                    object tableName = this.clbTableName.Items[i];
                    GenerCode.BulidCode(nameSpace, dirPath, tableName.ToString());
                }
            }
            if (isBulid == true)
            {
                MessageBox.Show("成功生成。", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                MessageBox.Show("请选择要生成的表。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAllChoose_Click(object sender, EventArgs e)
        {
            SetChecked(true);
        }
        private void SetChecked(bool isChoose)
        {
            for (int i = 0; i < this.clbTableName.Items.Count; i++)
            {
                this.clbTableName.SetItemChecked(i, isChoose);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetChecked(false);
        }

        private void clbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = this.clbTableName.SelectedIndex;
            bool aaa = this.clbTableName.GetItemChecked(i);
            this.clbTableName.SetItemChecked(i, !aaa);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string condition = this.txtCondition.Text.Trim();

            clbTableName.Items.Clear();
            List<string> tableNameList = DbHelper.Instance.GetAllTables();
            foreach (string str in tableNameList)
            {
                if (str.ToLower().Contains(condition.ToLower()))
                {
                    clbTableName.Items.Add(str.ToLower(), false);
                }
            }
        }
    }
}
/// <summary>
/// 
/// </summary>
public class GenerCode
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Namespace"></param>
    /// <param name="dirPath"></param>
    public static void BulidCode(string Namespace,string dirPath,string tableName)
    {
        string temp = ConfigurationManager.AppSettings["IsBuildCode"].ToString();
        if (temp == "1")
        {
            //List<string> tableNameList = DbHelper.Instance.GetAllTables();
            //foreach (string tableName in tableNameList)
            //{
                Utility.Instance.ConsoleWriteLine(tableName, LogFlag.Info);
                List<TableFieldProperty> fieldItem = GetField(tableName);
                GenerateCode(fieldItem, Namespace, @dirPath, tableName);
            //}
        }
    }
    private static List<TableFieldProperty> GetField(string tableName)
    {
        List<TableFieldProperty> item = new List<TableFieldProperty>();
        List<TableFieldProperty> ccItem = GetAllColumnComments();

        foreach (TableFieldProperty obj in ccItem)
        {
            if (obj.TableName.ToUpper() == tableName.ToUpper())
            {
                TableFieldProperty tfp = new TableFieldProperty();
                tfp = obj;
                if (String.IsNullOrEmpty(tfp.Comments))
                {
                    tfp.Comments = GetComment(tfp.ColumnName, ccItem);
                }
                item.Add(obj);
            }
        }

        return item;
    }
    private static string GetComment(string columnName,List<TableFieldProperty> ccItem)
    {
        string comment = "";
        foreach(TableFieldProperty tfp in ccItem)
        {
            if(!String.IsNullOrEmpty(tfp.Comments)&& 
                tfp.ColumnName.ToLower() == columnName.ToLower())
            {
                comment = tfp.Comments;
                break;
            }
        }
        return comment;
    }
    private static List<TableFieldProperty> _GetAllColumnComments;
    /// <summary>
    /// 获取数据中所有表字段备注
    /// </summary>
    /// <returns>返回 TableColumnComments 集合</returns>
    public static List<TableFieldProperty> GetAllColumnComments()
    {
        if (_GetAllColumnComments == null ||
            _GetAllColumnComments.Count <= 0)
        {
            _GetAllColumnComments = new List<TableFieldProperty>();

            string sqlQuery = string.Format(@"SELECT
	table_name as 'TABLE_NAME',
	column_name as 'COLUMN_NAME',
	column_comment as 'comments',
	data_type as 'DATA_TYPE'
FROM
	information_schema. COLUMNS
WHERE
	table_name IN (
		SELECT
			table_name
		FROM
			information_schema. TABLES
		WHERE
			table_schema = '{0}'
		AND table_type = 'base table'
	)", ConfigurationManager.AppSettings["DBName"]);
            //sqlQuery += string.Format(" where atc.owner = '{0}' order by atc.table_name ",DbHelper.Instance.DatabaseOwner);

            Utility.Instance.ConsoleWriteLine(sqlQuery, LogFlag.Info);
            int retCode = 0;
            DataSet ds = Util.DataBase.SqlQuery(out retCode, sqlQuery);
            if (retCode != 0)
            {
                return null;
            }
            int MaxCount = ds.Tables[0].Rows.Count;
            TableFieldProperty tcc = null;
            for (int i = 0; i < MaxCount; i++)
            {

                tcc = new TableFieldProperty();
                tcc.TableName = GetColumnValue(ds.Tables[0].Rows[i], "TABLE_NAME");
                tcc.ColumnName = GetColumnValue(ds.Tables[0].Rows[i], "COLUMN_NAME");
                tcc.DateType = GetColumnValue(ds.Tables[0].Rows[i], "DATA_TYPE");
                tcc.Comments = GetColumnValue(ds.Tables[0].Rows[i], "comments");

                _GetAllColumnComments.Add(tcc);
            }
        }

        return _GetAllColumnComments;
    }
    private static string GetColumnValue(DataRow dr,string columnName)
    {
        string str = "";
        try
        {
            str = dr[columnName] == null ? "" : dr[columnName].ToString().ToLower();
        }
        catch (Exception ee)
        {
            Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
        }
        return str;
    }
    private static void GenerateCode(List<TableFieldProperty> fieldItem,
        string nameSpace, string outputFile, string className)
    {
        string tableName = className;

        //准备一个代码编译器单元
        CodeCompileUnit unit = new CodeCompileUnit();

        string aaa = className;
        string[] strtemp = aaa.ToLower().Split('_');
        StringBuilder sb = new StringBuilder();
        foreach(string str  in strtemp)
        {
            try
            {
                string a = str.Substring(0, 1).ToUpper();
                string b = a+ str.Substring(1, str.Length - 1);
                sb.Append(b);
            }
            catch (Exception ee)
            {

            }
        }
        className = sb.ToString();
        //outputFile = className + ".cs";
        //准备必要的命名空间（这个是指要生成的类的空间）
        CodeNamespace sampleNamespace = new CodeNamespace(nameSpace);

        //导入必要的命名空间
        sampleNamespace.Imports.Add(new CodeNamespaceImport("System"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Text"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));


        //准备要生成的类的定义
        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(className);

        //指定这是一个Class
        Customerclass.IsClass = true;
        
        Customerclass.TypeAttributes = TypeAttributes.Public;// | TypeAttributes.Sealed;

        //把这个类放在这个命名空间下
        sampleNamespace.Types.Add(Customerclass);

        //把该命名空间加入到编译器单元的命名空间集合中
        unit.Namespaces.Add(sampleNamespace);

        //添加字段
        foreach (TableFieldProperty pv in fieldItem)
        {
            CodeMemberField field = new CodeMemberField(GetDateType(pv.DateType), "_" + pv.ColumnName);
            string comments = "";
            if (String.IsNullOrEmpty(pv.Comments) == true)
            {
                comments = "COLUMN: " + pv.ColumnName.ToUpper();
            }
            else
            {
                comments = pv.Comments;
            }
            field.Attributes = MemberAttributes.Private;
            field.Comments.Add(new CodeCommentStatement("<summary>", true));
            field.Comments.Add(new CodeCommentStatement(comments, true));
            field.Comments.Add(new CodeCommentStatement("</summary>", true));

            Customerclass.Members.Add(field);

            //添加属性
            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.Name = pv.ColumnName;
            property.HasGet = true;
            property.HasSet = true;
            property.Type = new CodeTypeReference(GetDateType(pv.DateType));
            

            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),"_"+ pv.ColumnName)
                        )
                        );

            property.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "_" + pv.ColumnName
                        ), new CodePropertySetValueReferenceExpression()
                        )
                        );
            property.Comments.Add(new CodeCommentStatement("<summary>", true));
            property.Comments.Add(new CodeCommentStatement(comments, true));
            property.Comments.Add(new CodeCommentStatement("</summary>", true));
            //property.Comments.Add(new CodeCommentStatement(new CodeComment("<summary>", true)));
            //property.Comments.Add(new CodeCommentStatement(new CodeComment(pv.Comments, true)));
            //property.Comments.Add(new CodeCommentStatement(new CodeComment("</summary>", true)));
            Customerclass.Members.Add(property);

        }
        //Customerclass.Comments
        Customerclass.Comments.Add(new CodeCommentStatement("<summary>", true));
        Customerclass.Comments.Add(new CodeCommentStatement("数据库表名称：" + tableName, true));
        Customerclass.Comments.Add(new CodeCommentStatement("</summary>", true));
        //Customerclass.Comments.Add(new CodeCommentStatement(new CodeComment("<summary>", true)));
        //Customerclass.Comments.Add(new CodeCommentStatement(new CodeComment("数据库表名称："+tableName, true)));
        //Customerclass.Comments.Add(new CodeCommentStatement(new CodeComment("</summary>", true)));


        //添加方法（使用CodeMemberMethod)--此处略
        //添加构造器(使用CodeConstructor) --此处略
        //添加程序入口点（使用CodeEntryPointMethod） --此处略
        //添加事件（使用CodeMemberEvent) --此处略
        //添加特征(使用 CodeAttributeDeclaration)
        //Customerclass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));

        //生成代码
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        options.BlankLinesBetweenMembers = true;
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile +"\\"+ className + ".cs"))
        {
            provider.GenerateCodeFromCompileUnit(unit, sw, options);
        }
    }
    private static Type GetDateType(string dateType)
    {
        switch (dateType)
        {
            case "varchar2":
                return typeof(System.String);
                
            case "number":
                return typeof(System.Decimal);

            case "date":
                return typeof(System.DateTime);

            case "char":
                return typeof(System.String);

            case "nchar":
                return typeof(System.String);

            default:

                return typeof(System.String);
        }
    }
}