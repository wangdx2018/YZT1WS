using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WorkStation.DB;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Data;
using AFC.WS.UI.FC;

namespace AFC.WS.UI.Common
{
    /**********************************
     * 
     * 
     * 20091020 增加Between...And 操作符和
     * 
     * not like操作符。
     * 
     * 
     * 20100414 增加了通用的DataTable转换器处理函数
     * 
     * 
     * ************************************/
    /// <summary>
    /// 操作符
    /// </summary>
    public enum OperationSymbols
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,
        /// <summary>
        /// 
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// 
        /// </summary>
        More = 2,
        /// <summary>
        /// 
        /// </summary>
        Less = 3,
        /// <summary>
        /// 
        /// </summary>
        MoreEqual = 4,

        SubMoreEqual = 15,
        /// <summary>
        /// 
        /// </summary>
        LessEqual = 5,
        SubLessEqual = 16,
        /// <summary>
        /// 
        /// </summary>
        Like = 6,
        /// <summary>
        /// 
        /// </summary>
        In = 7,
        /// <summary>
        /// 
        /// </summary>
        NotIn = 8,
        /// <summary>
        /// 
        /// </summary>
        IsNull = 9,
        /// <summary>
        /// 
        /// </summary>
        IsNotNull = 10,
        /// <summary>
        /// 
        /// </summary>
        BetweenAnd=11,

        /// <summary>
        /// 
        /// </summary>
        NotLike=12,

        /// <summary>
        /// 将数据展缓成字符串toDate类型
        /// </summary>
        Date=13,

        /// <summary>
        /// 不是为日期格式转化的区间数据
        /// </summary>
        BetweenAndNotForDate=14

    }
    /// <summary>
    /// 通用公共类，内容为经常使用，开发人员大量使用。
    /// </summary>
    public class Util
    {
        private static string _GetCurrentApplicationDirectory;
        /// <summary>
        /// 获取当前程序所在的目录。
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentApplicationDirectory
        {
            get
            {
                if (String.IsNullOrEmpty(_GetCurrentApplicationDirectory))
                {
                    _GetCurrentApplicationDirectory = Environment.CurrentDirectory.ToString();
                }
                return _GetCurrentApplicationDirectory;
            }
        }
        /// <summary>
        /// 存放Util类对象
        /// </summary>
        private static Util _Instance;

        /// <summary>
        /// 单件创建Util类对象
        /// </summary>
        public static Util Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance = new Util();
                }
                return Util._Instance;
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        private static string connString = string.Empty;
        /// <summary>
        /// 存放数据库访问类对象
        /// </summary>
        private static DBO _dataBase;

        /// <summary>
        /// 数据库操作
        /// </summary>
        public static DBO DataBase
        {
            get
            {
                connString = SysConfig.GetSysConfig(@".\Config\SysConfig.xml").LocalParamsConfig.DBConnectionString;
                if (_dataBase == null)
                    _dataBase = new DBO(connString);
                return _dataBase;
            }
            set
            {
                _dataBase = value;
            }
        }

        /// <summary>
        /// 在设置属性的时候将字符串转换成需要设置的数据类型
        /// </summary>
        /// <param name="pi">属性信息</param>
        /// <param name="value">字段</param>
        /// <returns>返回转换之后的数据</returns>
        public static object ParsePropertyValue(PropertyInfo pi, string value)
        {

            try
            {
                if (pi.PropertyType.IsEnum)
                {
                    return Enum.Parse(pi.PropertyType, value);
                }

                if (pi.PropertyType.Name == "String")
                {
                    return value;
                }

                if (pi.PropertyType.Name == "Double")
                {
                    double res = 0;
                    if (double.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Single")
                {
                    float res = 0;
                    if (float.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Decimal")
                {
                    decimal res = 0;
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (decimal.TryParse(value, out res))
                            return res;
                    }
                    return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Byte")
                {
                    byte res = 0;
                    if (byte.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Uint16")
                {
                    UInt16 res = 0;
                    if (UInt16.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Int16")
                {
                    Int16 res = 0;
                    if (Int16.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Int32")
                {
                    int res = 0;
                    if (int.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "UInt32")
                {
                    uint res = 0;
                    if (uint.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Uint64")
                {
                    UInt64 res = 0;
                    if (UInt64.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }

                if (pi.PropertyType.Name == "Int64")
                {
                    Int64 res = 0;
                    if (Int64.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + pi.Name + "], type=[" + pi.PropertyType + "],value=[" + value + "]");
                }
                throw new Exception("System can't Handle that type [" + pi.PropertyType.Name + "]");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }


        public static object ParseFieldValue(FieldInfo fi, string value)
        {
            try
            {
                if (fi.FieldType.IsEnum)
                {
                    return Enum.Parse(fi.FieldType, value);
                }

                if (fi.FieldType.Name == "String")
                {
                    return value;
                }

                if (fi.FieldType.Name == "Double")
                {
                    double res = 0;
                    if (double.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Single")
                {
                    float res = 0;
                    if (float.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Byte")
                {
                    byte res = 0;
                    if (byte.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "UInt16")
                {
                    UInt16 res = 0;
                    if (UInt16.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Int16")
                {
                    Int16 res = 0;
                    if (Int16.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Int32")
                {
                    int res = 0;
                    if (int.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "UInt32")
                {
                    uint res = 0;
                    if (uint.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Uint64")
                {
                    UInt64 res = 0;
                    if (UInt64.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Int64")
                {
                    Int64 res = 0;
                    if (Int64.TryParse(value, out res))
                        return res;
                    throw new Exception("PropertyName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }
                throw new Exception("System can't Handle that type [" + fi.FieldType.Name + "]");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 导出到Excel中
        /// </summary>
        /// <param name="dt">DataTabe</param>
        /// <param name="fileName">文件名</param>
        public static void exportToExcel(System.Data.DataTable dt, string fileName)
        {
            /*********************************************************************
             *  Add Date：2009-07-29 PM
             *  
             *      Note：加两个判断，第一判断 fileName 是否为空。
             *            第二判断 dt 是否为空。
             * 
             * *******************************************************************/
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            if (dt == null)
            {
                return;
            }

            if (File.Exists(fileName))
                File.Delete(fileName);

            System.IO.StreamWriter excelDoc;

            excelDoc = new System.IO.StreamWriter(fileName);
            const string startExcelXML = "<xml version>\r\n<Workbook " +
                  "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
                  " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                  "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
                  "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
                  "office:spreadsheet\">\r\n <Styles>\r\n " +
                  "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                  "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                  "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                  "\r\n <Protection/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                  "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                  "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                  " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                  "ss:Format=\"0\"/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                  "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                  "ss:Format=\"yyyy年/mm月/dd日;@\"/>\r\n </Style>\r\n " +
                  "</Styles>\r\n ";
            const string endExcelXML = "</Workbook>";

            int rowCount = 0;
            int sheetCount = 1;
            /*
           <xml version>
           <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
           xmlns:o="urn:schemas-microsoft-com:office:office"
           xmlns:x="urn:schemas-microsoft-com:office:excel"
           xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
           <Styles>
           <Style ss:ID="Default" ss:Name="Normal">
             <Alignment ss:Vertical="Bottom"/>
             <Borders/>
             <Font/>
             <Interior/>
             <NumberFormat/>
             <Protection/>
           </Style>
           <Style ss:ID="BoldColumn">
             <Font x:Family="Swiss" ss:Bold="1"/>
           </Style>
           <Style ss:ID="StringLiteral">
             <NumberFormat ss:Format="@"/>
           </Style>
           <Style ss:ID="Decimal">
             <NumberFormat ss:Format="0.0000"/>
           </Style>
           <Style ss:ID="Integer">
             <NumberFormat ss:Format="0"/>
           </Style>
           <Style ss:ID="DateLiteral">
             <NumberFormat ss:Format="mm/dd/yyyy;@"/>
           </Style>
           </Styles>
           <Worksheet ss:Name="Sheet1">
           </Worksheet>
           </Workbook>
           */
            excelDoc.Write(startExcelXML);
            excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
            excelDoc.Write("<Table>");
            excelDoc.Write("<Row>");
            for (int x = 0; x < dt.Columns.Count; x++)
            {
                excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                excelDoc.Write(dt.Columns[x].ColumnName);
                excelDoc.Write("</Data></Cell>");
            }
            excelDoc.Write("</Row>");
            foreach (DataRow x in dt.Rows)
            {
                rowCount++;
                //if the number of rows is > 64000 create a new page to continue output
                if (rowCount == 64000)
                {
                    rowCount = 0;
                    sheetCount++;
                    excelDoc.Write("</Table>");
                    excelDoc.Write(" </Worksheet>");
                    excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                    excelDoc.Write("<Table>");
                }
                excelDoc.Write("<Row>"); //ID=" + rowCount + "
                for (int y = 0; y < dt.Columns.Count; y++)
                {
                    System.Type rowType;
                    rowType = x[y].GetType();
                    switch (rowType.ToString())
                    {
                        case "System.String":
                            string XMLstring = x[y].ToString();
                            XMLstring = XMLstring.Trim();
                            XMLstring = XMLstring.Replace("&", "&");
                            XMLstring = XMLstring.Replace(">", ">");
                            XMLstring = XMLstring.Replace("<", "<");
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                           "<Data ss:Type=\"String\">");
                            excelDoc.Write(XMLstring);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DateTime":
                            //Excel has a specific Date Format of YYYY-MM-DD followed by  
                            //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
                            //The Following Code puts the date stored in XMLDate 
                            //to the format above
                            DateTime XMLDate = (DateTime)x[y];
                            string XMLDatetoString = ""; //Excel Converted Date
                            XMLDatetoString = XMLDate.Year.ToString() +
                                 "-" +
                                 (XMLDate.Month < 10 ? "0" +
                                 XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                                 "-" +
                                 (XMLDate.Day < 10 ? "0" +
                                 XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                                 "T" +
                                 (XMLDate.Hour < 10 ? "0" +
                                 XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                                 ":" +
                                 (XMLDate.Minute < 10 ? "0" +
                                 XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                                 ":" +
                                 (XMLDate.Second < 10 ? "0" +
                                 XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                                 ".000";
                            excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                                         "<Data ss:Type=\"DateTime\">");
                            excelDoc.Write(XMLDatetoString);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Boolean":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                        "<Data ss:Type=\"String\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
                                    "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
                                  "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DBNull":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                  "<Data ss:Type=\"String\">");
                            excelDoc.Write("");
                            excelDoc.Write("</Data></Cell>");
                            break;
                        default:
                            throw (new Exception(rowType.ToString() + " not handled."));
                    }
                }
                excelDoc.Write("</Row>");
            }
            excelDoc.Write("</Table>");
            excelDoc.Write(" </Worksheet>");
            excelDoc.Write(endExcelXML);
            excelDoc.Close();
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <param name="actionParamsList">查询条件</param>
        /// <returns>返回查询条件集合</returns>
        public static List<string> CreateQueryConditions(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null)
                return null;
            List<string> listCondtions = new List<string>();// save handle not in and in 

            Dictionary<KeyValuePair<string, OperationSymbols>, List<object>> dict = new Dictionary<KeyValuePair<string, OperationSymbols>, List<object>>();
            //bandingValue ,operatorSyhmos ==>list 数据集合
            for (int i = 0; i < actionParamsList.Count; i++)
            {
                if (actionParamsList[i].operation != OperationSymbols.In
                    && actionParamsList[i].operation != OperationSymbols.NotIn
                    && actionParamsList[i].operation != OperationSymbols.BetweenAnd
                    && actionParamsList[i].operation != OperationSymbols.BetweenAndNotForDate)
                {
                    string temp = HandleQuerySymbols(actionParamsList[i]);
                    if (temp != null)
                        listCondtions.Add(temp);
                }
                else
                {
                    // 1.key in handle 
                    // 2.key not in handle
                    // 3.key in handle and key not in handle
                    if (!dict.ContainsKey(new KeyValuePair<string, OperationSymbols>(actionParamsList[i].bindingData, actionParamsList[i].operation)))
                    {
                        List<object> list = new List<object>();//存放控件中的数据
                        if (actionParamsList[i].value != null)
                        {
                            list.Add(actionParamsList[i].value);
                            dict.Add(new KeyValuePair<string, OperationSymbols>(actionParamsList[i].bindingData, actionParamsList[i].operation), list);
                        }
                    }
                    else
                    {
                        dict[new KeyValuePair<string, OperationSymbols>(actionParamsList[i].bindingData, actionParamsList[i].operation)].Add(actionParamsList[i].value);
                    }
                }
            }
            if (dict.Count > 0)
            {
                var betweenAndCollection = from temp in dict
                                           where temp.Key.Value == OperationSymbols.BetweenAnd
                                              || temp.Key.Value == OperationSymbols.BetweenAndNotForDate
                                           select temp;
                var InOrNotInCollection = from temp1 in dict
                                          where temp1.Key.Value == OperationSymbols.In || temp1.Key.Value == OperationSymbols.NotIn
                                          select temp1;
                if (betweenAndCollection.Count() > 0)
                {
                    Dictionary<KeyValuePair<string, OperationSymbols>, List<object>> dictBetweenAnd = new Dictionary<KeyValuePair<string, OperationSymbols>, List<object>>();
                    foreach (var temp in betweenAndCollection)
                    {
                        bool res = true;
                        for (int i = 0; i < temp.Value.Count; i++)
                        {
                            if (temp.Value[i] != null)
                                continue;
                            else
                                res = false;
                        }
                        if (res)
                        {
                            dictBetweenAnd.Add(temp.Key, temp.Value);
                        }
                    }
                    if (dictBetweenAnd.Count > 0)
                    {
                        listCondtions.AddRange(HandleBetweenAdnQuerySymbols(dictBetweenAnd));
                    }
                }
                if (InOrNotInCollection.Count() > 0)
                {
                    Dictionary<KeyValuePair<string, OperationSymbols>, List<object>> dictInOrNotIn = new Dictionary<KeyValuePair<string, OperationSymbols>, List<object>>();
                    foreach (var temp in InOrNotInCollection)
                    {
                        dictInOrNotIn.Add(temp.Key, temp.Value);
                    }
                    listCondtions.AddRange(HandleInOrNotInQuerySymbols(dictInOrNotIn));
                }
            }
            return listCondtions;
        }

        /// <summary>
        /// 处理不是in和not in 和 不是Between and 
        /// </summary>
        /// <param name="paramsData">Action的参数类型</param>
        /// <returns>字符串类型</returns>
        private static string HandleQuerySymbols(QueryCondition paramsData)
        {
            if (paramsData == null || string.IsNullOrEmpty(paramsData.bindingData) || paramsData.value == null)
            {
                WriteLog.Log_Error("params is error: paramsData is not validity");
                return null;
            }
            if (string.IsNullOrEmpty(paramsData.value.ToString()))
                return null;
            switch (paramsData.operation)
            {
                case OperationSymbols.Equal:
                    //todo: return key=value
                    return paramsData.bindingData + "=" + "'" + paramsData.value.ToString() + "'";
                case OperationSymbols.NotEqual:
                    //todo: key<>value
                    return paramsData.bindingData + "<>'" + paramsData.value.ToString() + "'";
                case OperationSymbols.More:
                    // todo: key>value
                    return paramsData.bindingData + ">'" + paramsData.value.ToString() + "'";
                case OperationSymbols.Less:
                    //todo: key<value
                    return paramsData.bindingData + "<'" + paramsData.value.ToString() + "'";
                case OperationSymbols.MoreEqual:
                    //todo: key>=value
                    return paramsData.bindingData + ">='" + paramsData.value.ToString() + "'";
                case OperationSymbols.LessEqual:
                    //todo: key<=value
                    return paramsData.bindingData + "<='" + paramsData.value.ToString() + "'";
                //2010.8.5 dusj add begin
                case OperationSymbols.SubMoreEqual:
                    //todo: key>=value
                    return "substr(" + paramsData.bindingData + ",0,8)" + ">='" + paramsData.value.ToString().Replace("-", "") + "'";
                case OperationSymbols.SubLessEqual:
                    //todo: key<=value
                    return "substr(" + paramsData.bindingData + ",0,8)" + "<='" + paramsData.value.ToString().Replace("-", "") + "'";
                //2010.8.5 dusj add end
                case OperationSymbols.IsNull:
                    //todo: key is Null
                    return paramsData.bindingData + " is NULL";
                case OperationSymbols.IsNotNull:
                    //todo: key is not null
                    return paramsData.bindingData + " is not NULL";
                case OperationSymbols.Like:
                    //todo: key is like '%value%',need ' '!!
                    return paramsData.bindingData + " like '%" + paramsData.value.ToString() + "%'";
                case OperationSymbols.NotLike:
                    return paramsData.bindingData + " not like '%" + paramsData.value.ToString() + "%'";
                case OperationSymbols.Date:
                    return paramsData.bindingData + string.Format("=to_date('{0}','yyyy-MM-dd')", paramsData.value.ToString());
                default:
                    return null;
            }
        }

        /// <summary>
        /// 处理in和 not in
        /// </summary>
        /// <param name="dict">数据字典</param>
        /// <returns>数据集合</returns>
        private static List<string> HandleInOrNotInQuerySymbols(Dictionary<KeyValuePair<string, OperationSymbols>, List<object>> dict)
        {
            List<string> list = new List<string>();
            foreach (var temp in dict)
            {
                string str;

                if (temp.Key.Value == OperationSymbols.In)
                {
                    str = temp.Key.Key + " in ( {0}" + ")";
                }
                else
                {
                    str = temp.Key.Key + "  not in ( {0}" + ")";
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < temp.Value.Count; i++)
                {
                    if (i > 0)
                        sb.Append(",");
                    sb.Append(temp.Value[i].ToString());
                }

                list.Add(string.Format(str, sb.ToString()));
            }
            return list;
        }

        //--->对于Between and的处理
        /// <summary>
        /// BeTween and 操作符的处理
        /// </summary>
        /// <param name="dict">数据字典</param>
        /// <returns>成功</returns>
        private static List<string> HandleBetweenAdnQuerySymbols(Dictionary<KeyValuePair<string, OperationSymbols>, List<object>> dict)
        {
            if (dict.Count == 0)
                return null;
            /*
             * between to_date('2008-10-01','yyyy-MM-dd') and to_date('2009-10-21','yyyy-MM-dd')*/
            List<string> list = new List<string>();
            foreach (var temp in dict)
            {
                if (temp.Value.Count != 2)
                    continue;
                else
                {
                    List<string> valueArray = new List<string>();
                    foreach (var temp1 in temp.Value)
                    {
                        valueArray.Add(temp1.ToString());
                    }
                    string finalString = string.Empty;
                    if (temp.Key.Value == OperationSymbols.BetweenAnd)
                    {

                        if (valueArray[1].CompareTo(valueArray[0]) >= 0)
                        {
                            finalString = temp.Key.Key + " " + string.Format("between to_date('{0}','yyyy-MM-dd') and to_date('{1}','yyyy-MM-dd')", valueArray[0], valueArray[1]);
                        }
                        else
                        {
                            finalString = temp.Key.Key + " " + string.Format("between to_date('{0}','yyyy-MM-dd') and to_date('{1}','yyyy-MM-dd')", valueArray[1], valueArray[0]);
                        }
                    }
                    else
                    {

                        // string finalString = string.Empty;
                        if (valueArray[1].CompareTo(valueArray[0]) >= 0)
                        {
                            finalString = temp.Key.Key + " " + string.Format("between '{0}' and '{1}'", valueArray[0], valueArray[1]);
                        }
                        else
                        {
                            finalString = temp.Key.Key + " " + string.Format("between '{0}' and '{1}'", valueArray[1], valueArray[0]);
                        }
                    }
                    list.Add(finalString);
                }
            }
            return list;
        }


        /// <summary>
        /// 设置初始化查询为了SCWS，LCWS的查询
        /// </summary>
        /// <param name="comboxName">combox名称</param>
        /// <param name="comboxInitValue">初始值</param>
        /// <param name="actionButtonName">action按钮名称</param>
        /// <param name="ic">交互界面用户控件</param>
        public void SetInitQuery(string comboxName, object comboxInitValue, string actionButtonName, AFC.WS.UI.Components.InteractiveControl ic)
        {
            try
            {

                AFC.WS.UI.CommonControls.ComboBoxExtend cmbEx = ic.GetCommonControlByName(comboxName) as AFC.WS.UI.CommonControls.ComboBoxExtend;
                if (cmbEx != null)
                {

                    cmbEx.SetControlValue(comboxInitValue);
                    cmbEx.CanReadOnly = true;
                    cmbEx.IsEditable = false;
                    ic.Config.ControlList.Single(temp => temp.ControlName.Equals(comboxName)).InitValue = comboxInitValue.ToString();
                    ic.HandleAction(actionButtonName);

                }
            }
            catch (Exception ex)
            {

            }

        }
    }


     
}
