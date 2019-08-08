using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.Components;
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 此类专门提供与规则文件有关的方法。
    /// </summary>
    public class RuleHelper
    {
        #region --> 属性

        #region --> 获取名称空间。

        /// <summary>
        /// 获取Action名称空间
        /// </summary>
        private string _ActionNamespace;
        /// <summary>
        /// 获取CommonControl名称空间
        /// </summary>
        private string _CommonControlNamespace;
        /// <summary>
        /// 获取DataSource名称空间
        /// </summary>
        private string _DataSourceNamespace;
        /// <summary>
        /// 获取Convertor名称空间
        /// </summary>
        private string _ConvertorNamespace;

        /// <summary>
        /// 获取Convertor名称空间
        /// </summary>
        public string ConvertorNamespace
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_ConvertorNamespace))
                    {
                        _ConvertorNamespace = ConfigurationManager.AppSettings["ConvertorNamespace"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }

                return _ConvertorNamespace;
            }
        }

        /// <summary>
        /// 获取DataSource名称空间
        /// </summary>
        public string DataSourceNamespace
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_DataSourceNamespace))
                    {
                        _DataSourceNamespace = ConfigurationManager.AppSettings["DataSourceNamespace"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                } return _DataSourceNamespace;
            }
        }

        /// <summary>
        /// 获取CommonControl名称空间
        /// </summary>
        public string CommonControlNamespace
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_CommonControlNamespace))
                    {
                        _CommonControlNamespace = ConfigurationManager.AppSettings["CommonControlNamespace"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                } return _CommonControlNamespace;
            }
        }

        /// <summary>
        /// 获取Action名称空间
        /// </summary>
        public string ActionNamespace
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_ActionNamespace))
                    {
                        _ActionNamespace = ConfigurationManager.AppSettings["ActionNamespace"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _ActionNamespace;
            }
        }

        #endregion --> 获取名称空间。

        #region --> 获取Dll

        /// <summary>
        /// 获取Action Dll的名称
        /// </summary>
        private string _ActionDll;
        /// <summary>
        /// 获取Common Control Dll的名称
        /// </summary>
        private string _CommonEditDll;
        /// <summary>
        /// 获取DataSource Dll的名称
        /// </summary>
        private string _DataSourceDll;
        /// <summary>
        /// 获取Convertor Dll的名称
        /// </summary>
        private string _ConvertorDll;

        /// <summary>
        /// 获取Convertor Dll的名称
        /// </summary>
        public string ConvertorDll
        {
            get
            {

                try
                {
                    if (String.IsNullOrEmpty(_ConvertorDll))
                    {
                        _ConvertorDll = ConfigurationManager.AppSettings["IConvertor"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }

                return _ConvertorDll;
            }
        }

        /// <summary>
        /// 获取DataSource Dll的名称
        /// </summary>
        public string DataSourceDll
        {
            get {

                try
                {
                    if (String.IsNullOrEmpty(_DataSourceDll))
                    {
                        _DataSourceDll = ConfigurationManager.AppSettings["IDataSource"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _DataSourceDll; }
        }

        /// <summary>
        /// 获取Common Control Dll的名称
        /// </summary>
        public string CommonEditDll
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_CommonEditDll))
                    {
                        _CommonEditDll = ConfigurationManager.AppSettings["ICommonEdit"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _CommonEditDll;
            }
        }
        /// <summary>
        /// 获取Action Dll的名称
        /// </summary>
        public string ActionDll
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_ActionDll))
                    {
                        _ActionDll = ConfigurationManager.AppSettings["IAction"].ToString();
                    }
                }
                catch (NullReferenceException nre)
                {
                    Utility.Instance.ConsoleWriteLine(nre, LogFlag.ErrorFormat);
                }
                catch (Exception ee)
                {
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _ActionDll;
            }
        }

        #endregion --> 获取Dll

        /// <summary>
        /// DLL列表
        /// </summary>
        private List<DllProperty> _DllPropertyList = new List<DllProperty>();
        
        /// <summary>
        /// 用于存放DLL及DLL程序集名称的一个集合。
        /// </summary>
        public List<ItemCom> ICList = new List<ItemCom>();
        /// <summary>
        /// 实例化RuleHelper类对象。
        /// </summary>
        private static RuleHelper _Instance;

        /// <summary>
        /// DLL列表
        /// </summary>
        public List<DllProperty> DllPropertyList
        {
            get { return _DllPropertyList; }
            set { _DllPropertyList = value; }
        }
      
        /// <summary>
        /// 实例化RuleHelper类对象。
        /// </summary>
        public static RuleHelper Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance = new RuleHelper();
                }
                return _Instance;
            }
            set { _Instance = value; }
        }

        #endregion --> 属性

        #region --> 获取导入DLL的里指定的类
        /// <summary>
        /// 将DLL里的类的属性添加到DllClassProperty里面去。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="assemblyName"></param>
        private void AddDllProperty(Type t,string assemblyName)
        {
            bool isExist = false;

            for (int i = 0; i < DllPropertyList.Count;i++ )
            {
                if (DllPropertyList[i].Namespace == t.Namespace)
                {
                    DllClassProperty dcp = new DllClassProperty(t, assemblyName, t.FullName, t.Namespace, t.Name);
                    DllPropertyList[i].Add(dcp);
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                DllProperty dp = new DllProperty();
                dp.Namespace = t.Namespace;
                dp.Add(new DllClassProperty(t, assemblyName, t.FullName, t.Namespace, t.Name));
                DllPropertyList.Add(dp);
            }
        }

        /// <summary>
        /// 获取Dll的属性。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void GetDllProperty(string filePath)
        {
            if (String.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return;
            }//判断DLL的文件名称是否为NULL或空，以及DLL文件是否存在。

            try
            {
                Assembly ass = Assembly.LoadFile(filePath);
                bool isExist = false;
                foreach (ItemCom icObj in ICList)
                {
                    if (icObj.Name == ass.GetName().Name)
                    {
                        isExist = true;
                    }
                }
                if (isExist)
                {
                    return;
                }

                bool isAdd = false;
                foreach (Type t in ass.GetTypes())
                {
                    #region --> try
                    try
                    {
                        if (!t.IsClass)
                        {
                            continue;
                        }
                        Utility.Instance.ConsoleWriteLine(t.FullName, LogFlag.Info);

                        object obj = Activator.CreateInstance(t);

                        if (obj is IAction)
                        {
                            AddDllProperty(t, ass.GetName().Name);
                            isAdd = true;
                        }
                        if (obj is IDataSource)
                        {
                            AddDllProperty(t, ass.GetName().Name);
                            isAdd = true;
                        }
                        if (obj is IConvertor)
                        {
                            AddDllProperty(t, ass.GetName().Name);
                            isAdd = true;
                        }
                        if (obj is ICommonEdit)
                        {
                            AddDllProperty(t, ass.GetName().Name);
                            isAdd = true;
                        }
                    }
                    catch (MissingMethodException mme)
                    {
                        Utility.Instance.ConsoleWriteLine(t.FullName, LogFlag.ErrorFormat);
                        Utility.Instance.ConsoleWriteLine(mme, LogFlag.ErrorFormat);
                    }
                    catch (Exception foree)
                    {
                        Utility.Instance.ConsoleWriteLine(t.FullName, LogFlag.ErrorFormat);
                        Utility.Instance.ConsoleWriteLine(foree, LogFlag.ErrorFormat);
                    }
                    #endregion --> try
                }
                if (isAdd)
                {
                    ItemCom ic = new ItemCom();
                    ic.Name = ass.GetName().Name;
                    ic.Tag = DllPropertyList;
                    ICList.Add(ic);
                }

            }
            catch (Exception ee)
            {
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }
        
        /// <summary>
        /// 导入DLL
        /// </summary>
        /// <param name="dllFilePathArrary">文件路径数组</param>
        /// <param name="ilList">ItemoCom 集合对象</param>
        public void ImportDll(string[] dllFilePathArrary, List<ItemCom> ilList)
        {
            ICList.Clear();
            Utility.Instance.ClassPropertyActionList.Clear();
            Utility.Instance.ClassPropertyControlsList.Clear();
            Utility.Instance.ClassPropertyConvertorList.Clear();
            Utility.Instance.ClassPropertyDataSourceList.Clear();
            if (dllFilePathArrary == null)
            {
                return;
            }
            GetDllClassProperty(dllFilePathArrary);
            EvaluateMethod(ilList);
            #region -->
            //foreach (string str in dllFilePathArrary)
            //{
            //    string[] Action = this.ActionDll.Split(',');
            //    string[] CommodEdit = this.CommonEditDll.Split(',');
            //    string[] DataSource = this.DataSourceDll.Split(',');
            //    string[] Convertor = this.ConvertorDll.Split(',');
            //    foreach (string temp in Action)
            //    {
            //        if (str.Contains(temp))
            //        {
            //            GetDllProperty(str);
            //        }
            //    }
            //    foreach (string temp in CommodEdit)
            //    {
            //        if (str.Contains(temp))
            //        {
            //            GetDllProperty(str);
            //        }
            //    }
            //    foreach (string temp in DataSource)
            //    {
            //        if (str.Contains(temp))
            //        {
            //            GetDllProperty(str);
            //        }
            //    }
            //    foreach (string temp in Convertor)
            //    {
            //        if (str.Contains(temp))
            //        {
            //            GetDllProperty(str);
            //        }
            //    }
            //    //if (str.Contains(this.ActionDll) || str.Contains(this.CommonEditDll) ||
            //    //    str.Contains(this.DataSourceDll) || str.Contains(this.ConvertorDll))
            //    //{
                   
            //    //}
                
            //}

            //if (ilList == null)
            //{
            //    return;
            //}
            #endregion -->

            #region --> 将指定的DLL类存放到 UIConfig、DataSource、DataList里去。
            //if (DllPropertyList.Count > 0)
            //{
            //    string[] AN = ActionNamespace.Split(',');
            //    string[] CCN = CommonControlNamespace.Split(',');
            //    string[] DSN = DataSourceNamespace.Split(',');
            //    string[] CN = ConvertorNamespace.Split(',');

            //    foreach (DllProperty dp in DllPropertyList)
            //    {
            //        foreach (ItemCom il in ilList)
            //        {
            //            switch (il.Name)
            //            {
            //                case "UIConfig":
            //                    InteractiveControlRule ui = il.Tag as InteractiveControlRule;
            //                    foreach (string temp in AN)
            //                    {
            //                        if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp) &&
            //                            dp.DllClassPropertyList.Count > 0)
            //                        {
            //                            Utility.Instance.ClassPropertyActionList.AddRange(dp.DllClassPropertyList);
            //                        }
            //                    }
            //                    foreach (string temp in CCN)
            //                    {
            //                        if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp))
            //                        {
            //                            Utility.Instance.ClassPropertyControlsList = dp.DllClassPropertyList;
            //                        }
            //                    }
            //                    //if (!String.IsNullOrEmpty(ActionNamespace) && dp.Namespace.Contains(ActionNamespace))
            //                    //{
            //                    //    Utility.Instance.ClassPropertyActionList = dp.DllClassPropertyList;
            //                    //}
            //                    //if (!String.IsNullOrEmpty(CommonControlNamespace) && dp.Namespace.Contains(CommonControlNamespace))
            //                    //{
            //                    //    Utility.Instance.ClassPropertyControlsList = dp.DllClassPropertyList;
            //                    //}
            //                    il.Tag = ui;

            //                    break;

            //                case "DataSource":
            //                    DataSourceRule ds = il.Tag as DataSourceRule;
            //                    foreach (string temp in DSN)
            //                    {
            //                        if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp))
            //                        {
            //                            Utility.Instance.ClassPropertyDataSourceList = dp.DllClassPropertyList;
            //                        }
            //                    }
                                
            //                    //if (!String.IsNullOrEmpty(DataSourceNamespace) && dp.Namespace.Contains(DataSourceNamespace))
            //                    //{
            //                    //    Utility.Instance.ClassPropertyDataSourceList = dp.DllClassPropertyList;
            //                    //}
            //                    il.Tag = ds;

            //                    break;
            //                case "DataList":
            //                    DataListRule rfdl = il.Tag as DataListRule;
            //                    foreach (string temp in AN)
            //                    {
            //                        if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp))
            //                        {
            //                            Utility.Instance.ClassPropertyActionList = dp.DllClassPropertyList;
            //                        }
            //                    }
            //                    foreach (string temp in CN)
            //                    {
            //                        if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp))
            //                        {
            //                            Utility.Instance.ClassPropertyConvertorList = dp.DllClassPropertyList;
            //                        }
            //                    }
            //                    //if (!String.IsNullOrEmpty(ActionNamespace) && dp.Namespace.Contains(ActionNamespace))
            //                    //{
            //                    //    Utility.Instance.ClassPropertyActionList = dp.DllClassPropertyList;
            //                    //}
            //                    //if (!String.IsNullOrEmpty(ConvertorNamespace) && dp.Namespace.Contains(ConvertorNamespace))
            //                    //{
            //                    //    Utility.Instance.ClassPropertyConvertorList = dp.DllClassPropertyList;
            //                    //}

            //                    il.Tag = rfdl;
            //                    break;
            //                case "Chart":
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //    }
            //}
            #endregion --> 
        }


        private void GetDllClassProperty(string[] dllFilePathArray)
        {
            ICList.Clear();
            DllPropertyList.Clear();

            foreach (string str in dllFilePathArray)
            {
                //-->判断DLL
                foreach (string dll in GetDllItem())
                {
                    if (str.Contains(dll))
                    {
                        ReadDllClass(str);
                    }
                }
            }
        }

        private void EvaluateMethod(List<ItemCom> ilList)
        {
            List<string> naItem = GetNameSpaceItem();
            foreach (DllProperty dp in DllPropertyList)
            {
                foreach (ItemCom il in ilList)
                {
                    switch (il.Name)
                    {
                        case "UIConfig":
                            InteractiveControlRule ui = il.Tag as InteractiveControlRule;
                            foreach (string temp in ActionNamespace.Split(','))
                            {
                                if (dp.Namespace.Contains(temp))
                                {
                                    JugdeDllClassPropertyIsExist(dp.DllClassPropertyList, Utility.Instance.ClassPropertyActionList);
                                }
                            }
                            foreach (string temp in CommonControlNamespace.Split(','))
                            {
                                if (dp.Namespace.Contains(temp))
                                {
                                    JugdeDllClassPropertyIsExist(dp.DllClassPropertyList, Utility.Instance.ClassPropertyControlsList);
                                }
                            }
                            il.Tag = ui;

                            break;

                        case "DataSource":
                            DataSourceRule ds = il.Tag as DataSourceRule;
                            foreach (string temp in DataSourceNamespace.Split(','))
                            {
                                if (dp.Namespace.Contains(temp))
                                {
                                    JugdeDllClassPropertyIsExist(dp.DllClassPropertyList, Utility.Instance.ClassPropertyDataSourceList);
                                }
                            }
                            il.Tag = ds;

                            break;
                        case "DataList":
                            DataListRule rfdl = il.Tag as DataListRule;
                            foreach (string temp in ActionNamespace.Split(','))
                            {
                                if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp))
                                {
                                    JugdeDllClassPropertyIsExist(dp.DllClassPropertyList, Utility.Instance.ClassPropertyActionList);
                                }
                            }
                            foreach (string temp in ConvertorNamespace.Split(','))
                            {
                                if (!String.IsNullOrEmpty(temp) && dp.Namespace.Contains(temp))
                                {
                                    JugdeDllClassPropertyIsExist(dp.DllClassPropertyList, Utility.Instance.ClassPropertyConvertorList);
                                }
                            }
                            il.Tag = rfdl;
                            break;
                        case "Chart":
                            break;
                        default:
                            break;
                    }//End switch
                }//End foreach
            }//End foreach
        }

        private void JugdeDllClassPropertyIsExist(List<DllClassProperty> source, List<DllClassProperty> target)
        {
            List<DllClassProperty> dcpItem = new List<DllClassProperty>();

            foreach (DllClassProperty dcp1 in source)
            {
                bool isExist = false;
                foreach (DllClassProperty dcp2 in target)
                {
                    if (dcp1.FullName == dcp2.FullName)
                    {
                        isExist = true;
                    }
                }
                if (isExist == false)
                {
                    dcpItem.Add(dcp1);
                }
            }
            target.AddRange(dcpItem);
            //target = target.OrderBy(p => p.ClassName).ToList();
            //return dcpItem;
        }

        private void ReadDllClass(string dllPath)
        {
            try
            {
                Assembly ass = Assembly.LoadFile(dllPath);

                Type[] Item = ass.GetTypes();
                List<string> naItem = GetNameSpaceItem();
                bool isAdd = false;
                foreach (Type t in Item)
                {
                    bool isAddClass = false;
                    foreach (var temp in naItem)
                    {
                        if (temp == t.Namespace)
                        {
                            isAddClass = true;
                            break;
                        }
                    }
                    if (isAddClass != true)
                    {
                        continue;
                    }
                    #region --> try

                    try
                    {
                        if (!t.IsClass)
                        {
                            continue;
                        }
                        Utility.Instance.ConsoleWriteLine(t.FullName, LogFlag.Info);

                        object obj = Activator.CreateInstance(t);

                        if ((obj is IAction) ||
                            (obj is IDataSource) ||
                            (obj is IConvertor) ||
                            (obj is ICommonEdit))
                        {
                            AddDllProperty(t, ass.GetName().Name);
                            isAdd = true;
                        }
                        
                    }
                    catch (MissingMethodException mme)
                    {
                        Utility.Instance.ConsoleWriteLine(t.FullName, LogFlag.ErrorFormat);
                        Utility.Instance.ConsoleWriteLine(mme, LogFlag.ErrorFormat);
                    }
                    catch (Exception foree)
                    {
                        Utility.Instance.ConsoleWriteLine(t.FullName, LogFlag.ErrorFormat);
                        Utility.Instance.ConsoleWriteLine(foree, LogFlag.ErrorFormat);
                    }
                    #endregion --> try
                }
                if (isAdd)
                {
                    ItemCom ic = new ItemCom();
                    ic.Name = ass.GetName().Name;
                    ic.Tag = DllPropertyList;
                    ICList.Add(ic);
                }

            }
            catch (Exception ee)
            {
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }

        private List<string> GetDllItem()
        {
            List<string> item = new List<string>();

            string[] Action = this.ActionDll.Split(',');
            item.AddRange(Action);
            string[] CommodEdit = this.CommonEditDll.Split(',');
            JudugeStringIsExist(CommodEdit, item);
            string[] DataSource = this.DataSourceDll.Split(',');
            JudugeStringIsExist(DataSource, item);
            string[] Convertor = this.ConvertorDll.Split(',');
            JudugeStringIsExist(Convertor, item);

            return item;
        }

        private List<string> GetNameSpaceItem()
        {
            List<string> item = new List<string>();

            string[] AN = ActionNamespace.Split(',');
            item.AddRange(AN);
            string[] CCN = CommonControlNamespace.Split(',');
            JudugeStringIsExist(CCN, item);
            string[] DSN = DataSourceNamespace.Split(',');
            JudugeStringIsExist(DSN, item);
            string[] CN = ConvertorNamespace.Split(',');
            JudugeStringIsExist(CN, item);

            return item;
        }

        private void JudugeStringIsExist(string[] temp, List<string> item)
        {
            foreach (string obj in temp)
            {
                bool isExist = false;
                foreach (string str in item)
                {
                    if (obj == str)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    item.Add(obj);
                }
            }
        }






        #endregion -->获取导入DLL的里指定的类。

        #region --> 文件路径
        /// <summary>
        /// 获取配置文件中保存规则文件的路径。
        /// </summary>
        private string RuleFileSavePath = ConfigurationManager.AppSettings["RuleFileSavePath"].ToString();
        /// <summary>
        /// 默认保存的路径。
        /// </summary>
        private string DefaultFileSavePath = @"..\..\..\Config";
        /// <summary>
        /// UI交互界面规则文件保存的路径
        /// </summary>
        private string FileSavePathUI
        {
            get
            {
                string path = RuleFileSavePath;

                if( String.IsNullOrEmpty(path))
                {
                    path = DefaultFileSavePath;
                }
                return path;
            }
        }
        /// <summary>
        /// 图形规则文件保存的路径。
        /// </summary>
        private string FileSavePathChart
        {
            get
            {
                string path = RuleFileSavePath;
                
                if (String.IsNullOrEmpty(path))
                {
                    path = DefaultFileSavePath;
                }

                return path;
            }
        }
        /// <summary>
        /// DataList规则文件保存的路径。
        /// </summary>
        private string FileSavePathDataList
        {
            get
            {
                string path = RuleFileSavePath;

                if (String.IsNullOrEmpty(path))
                {
                    path = DefaultFileSavePath;
                }
                return path;
            }
        }
        /// <summary>
        /// 数据库规则文件保存的路径。
        /// </summary>
        public string FileSavePathDataSource
        {
            get
            {
                string path = RuleFileSavePath;

                if (String.IsNullOrEmpty(path))
                {
                    path = DefaultFileSavePath;
                }

                return path;
            }
        }
        #endregion -->文件路径

        #region --> 保存规则文件操作。 

        /// <summary>
        /// 生成规则文件操作。
        /// </summary>
        /// <param name="il">ItemCom 对象</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="isModify">是否是修改</param>
        /// <returns>返回true： 生成成功；false：生成失败</returns>
        public bool Save(ItemCom il, string fileName,bool isModify)
        {
            bool result = true;
            string SaveFile = string.Empty;

            if (il == null)
            {
                return false;
            }
            switch (il.Name)
            {
                case "UIConfig":

                    #region -->UIConfig

                    if (isModify)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        SaveFile = fileName;
                    }
                    else
                    {
                        SaveFile = FileSavePathUI + "\\" + fileName + ".xml";
                    }

                    if( JudgeDirectoryIsExist(FileSavePathUI,SaveFile))
                    {
                        InteractiveControlRule ui = il.Tag as InteractiveControlRule;

                        List<ActionProperty> apList = ui.ActionList;

                        List<ItemCom> objList = new List<ItemCom>();
                        ItemCom obj;
                        foreach (ActionProperty ap in ui.ActionList)
                        {
                            obj = new ItemCom();
                            obj.Tag = ap.TargetAction;
                            obj.Name = ap.ControlName;

                            objList.Add(obj);
                        }
                        for (int i = 0; i < apList.Count; i++)
                        {
                            ui.ActionList[i].TargetAction = null;
                        }


                        List<ControlProperty> cpList = ui.ControlList;
                        List<ItemCom> cpItemCom = new List<ItemCom>();

                        foreach (ControlProperty cp in cpList)
                        {
                            obj = new ItemCom();
                            obj.Tag = cp.TargetConvertor;
                            obj.Name = cp.ControlName;
                            cpItemCom.Add(obj);
                            cp.TargetConvertor = null;
                        }

                        XmlSerializer serial = new XmlSerializer(ui.GetType());

                        Stream s = File.Open(SaveFile,FileMode.OpenOrCreate);

                        if (s != null)
                        {
                            using (s)
                            {
                                try
                                {
                                    serial.Serialize(s, ui);
                                    result = true;

                                    foreach (ItemCom ilobj in objList)
                                    {
                                        for (int i = 0; i < ui.ActionList.Count;i++ )
                                        {
                                            if (ilobj.Name == ui.ActionList[i].ControlName)
                                            {
                                                ui.ActionList[i].TargetAction = ilobj.Tag;
                                            }
                                        }
                                    }

                                    foreach (ItemCom cpItem in cpItemCom)
                                    {
                                        for (int i = 0; i < ui.ControlList.Count; i++)
                                        {
                                            if (cpItem.Name == ui.ControlList[i].ControlName)
                                            {
                                                ui.ControlList[i].TargetConvertor = cpItem.Tag;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ee)
                                {
                                    result = false;

                                    //WriteLog.Log_Error(ee);
                                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                                }
                                s.Close();
                                s.Dispose();
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                    #endregion -->UIConfig

                    break;
                case "Chart":

                    #region -->Chart
                    if (isModify)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        SaveFile = fileName;
                    }
                    else
                    {
                        SaveFile = FileSavePathChart + "\\" + fileName + ".xml";
                    }

                    if( JudgeDirectoryIsExist(FileSavePathChart,SaveFile) )
                    {
                        ChartRule ui = il.Tag as ChartRule;

                        XmlSerializer serial = new XmlSerializer(ui.GetType());

                        Stream s = File.Open(SaveFile,FileMode.OpenOrCreate);
                        
                        if (s != null)
                        {
                            using (s)
                            {
                                try
                                {
                                    serial.Serialize(s, ui);
                                    result = true;
                                }
                                catch (Exception ee)
                                {
                                    result = false;
                                   // WriteLog.Log_Error(ee);
                                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                                }
                                s.Close();
                                s.Dispose();
                            }

                        }
                    }
                    else
                    {
                        result = false;
                    }
                    #endregion -->Chart

                    break;
                case "DataSource":

                    #region -->DataSource
                    DataSourceRule dsr = il.Tag as DataSourceRule;

                    if (isModify)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        SaveFile = fileName;
                    }
                    else
                    {
                        SaveFile = FileSavePathDataSource + "\\" + dsr.DataSourceName + ".xml";
                    }

                    if( JudgeDirectoryIsExist(FileSavePathDataSource,SaveFile ))
                    {
                        ItemCom obj = new ItemCom();
                        obj.Name = dsr.DataSourceName;
                        obj.Tag = dsr.TargetDataSource;

                        dsr.TargetDataSource = null;

                        XmlSerializer serial = new XmlSerializer(dsr.GetType());

                        Stream s = File.Open(SaveFile,FileMode.OpenOrCreate);

                        if (s != null)
                        {
                            using (s)
                            {
                                try
                                {
                                    serial.Serialize(s, dsr);
                                    result = true;
                                    dsr.TargetDataSource = obj.Tag;
                                    il.Tag = dsr;
                                }
                                catch (Exception ee)
                                {
                                    result = false;
                                    //WriteLog.Log_Error(ee);
                                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                                }//End try
                                s.Close();
                                s.Dispose();
                            }//End using;
                        }//End if;
                    }
                    else
                    {
                        result = false;
                    }//End if;
                    #endregion -->DataSource

                    break;
                case "DataList":

                    #region -->DataList

                    if (isModify)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        SaveFile = fileName;
                    }
                    else
                    {
                        SaveFile = FileSavePathDataList + "\\" + fileName + ".xml";
                    }

                    if( JudgeDirectoryIsExist(FileSavePathDataList,SaveFile) )
                    {
                        DataListRule ui = il.Tag as DataListRule;

                        List<ItemCom> objList1 = new List<ItemCom>();
                        ItemCom obj;
                        foreach (ActionProperty ap in ui.ActionList)
                        {
                            obj = new ItemCom();
                            obj.Tag = ap.TargetAction;
                            obj.Name = ap.ControlName;

                            objList1.Add(obj);
                        }

                        foreach (ColumnProperty cp in ui.ColumnList)
                        {
                            obj = new ItemCom();
                            obj.Name = cp.BindingField;
                            obj.Tag = cp.TargetConvertor;

                            objList1.Add(obj);
                        }
                        for (int i = 0; i < ui.ActionList.Count; i++)
                        {
                            ui.ActionList[i].TargetAction = null;
                        }
                        for (int i = 0; i < ui.ColumnList.Count; i++)
                        {
                            ui.ColumnList[i].TargetConvertor = null;
                        }

                        XmlSerializer serial = new XmlSerializer(ui.GetType());
                        Stream s = File.Open(SaveFile,FileMode.OpenOrCreate);

                        if (s != null)
                        {
                            using (s)
                            {
                                try
                                {
                                    serial.Serialize(s, ui);
                                    result = true;

                                    foreach (ItemCom ilobj in objList1)
                                    {
                                        for (int i = 0; i < ui.ActionList.Count; i++)
                                        {
                                            if (ilobj.Name == ui.ActionList[i].ControlName)
                                            {
                                                ui.ActionList[i].TargetAction = ilobj.Tag;
                                                break;
                                            }
                                        }
                                        for (int i = 0; i < ui.ColumnList.Count; i++)
                                        {
                                            if (ilobj.Name == ui.ColumnList[i].BindingField)
                                            {
                                                ui.ColumnList[i].TargetConvertor = ilobj.Tag;
                                                break;
                                            }
                                        }//End for;
                                    }//ENd foreach;
                                }
                                catch (Exception ee)
                                {
                                    result = false;
                                    //WriteLog.Log_Error(ee);
                                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                                }//End try
                                s.Close();
                                s.Dispose();
                            }//End using;

                        }//End if
                    }
                    else
                    {
                        result = false;
                    }//End if;

                    #endregion -->DataList

                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 判断文件是否存在。
        /// </summary>
        /// <param name="pathFile">文件路径，文件名称</param>
        /// <returns>返回true：存在；false：不存在</returns>
        public bool JudgeFileIsExist(string pathFile)
        {
            bool result = true;

            if (string.IsNullOrEmpty(pathFile))
            {
                MessageBox.Show("文件路径为空。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (File.Exists(pathFile))
            {
                if (MessageBox.Show("文件名" + pathFile + "已经存在，是否删除原来的文件。", "警告",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(pathFile);
                        result = true;
                    }
                    catch (Exception ee)
                    {
                        result = false;
                        //WriteLog.Log_Error(ee);
                        Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                        MessageBox.Show("删除[" + pathFile + "]文件时出错。", ee.Message);
                    }
                }
                else
                {
                    result = false;      //文件没有删除。
                }
            }
            else
            {
                result = true;
            }

            return result;

        }

        /// <summary>
        /// 判断目录是否存在。
        /// </summary>
        /// <param name="directoryName">目录</param>
        /// <param name="pathFile">文件路径，文件名称</param>
        /// <returns>返回true：目录存在；false：目录不存在</returns>
        private bool JudgeDirectoryIsExist(string directoryName, string pathFile)
        {
            bool result = true;

            if (!String.IsNullOrEmpty(directoryName))
            {

                if (Directory.Exists(directoryName))
                {
                    result = JudgeFileIsExist(pathFile);
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(directoryName);

                        result = true;
                    }
                    catch (DirectoryNotFoundException dnfe)
                    {
                        result = false;
                        //WriteLog.Log_Error(dnfe);
                        Utility.Instance.ConsoleWriteLine(dnfe, LogFlag.Error);
                        MessageBox.Show("创建文件目录时出错，请确认配置文件里路径配置是否正确。", dnfe.Message);

                    }
                    catch (Exception ee)
                    {
                        result = false;
                        //WriteLog.Log_Error(ee);
                        Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                        MessageBox.Show("创建文件目录时出错。", ee.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("文件目录为空。","警告",  MessageBoxButtons.OK , MessageBoxIcon.Information);
                result = false;
            }
            return result;
        }

        #endregion --> 保存规则文件操作。

        #region --> 方法

        /// <summary>
        /// 规则文件类型
        /// ItemList.Name = 规则文件类型，
        /// ItemList.Tag = 规则文件的类。
        /// </summary>
        /// <returns>返回ItemCom 集合对象</returns>
        public List<ItemCom> RuleFileTypeList()
        {
            List<ItemCom> iList = new List<ItemCom>();

            ItemCom il = new ItemCom("--请选择-- ", null);
            iList.Add(il);

            il = new ItemCom("UIConfig", new InteractiveControlRule());
            iList.Add(il);

            il = new ItemCom("Chart", new ChartRule());
            iList.Add(il);

            il = new ItemCom("DataList", new DataListRule());
            iList.Add(il);

            il = new ItemCom("DataSource", new DataSourceRule());
            iList.Add(il);

            return iList;
        }

        /// <summary>
        /// 反序列化规则文件
        /// </summary>
        /// <param name="il">ItemCom 对象</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回一个规则文件对象。</returns>
        public object Loading(ItemCom il, string filePath)
        {
            object obj = null;
            if (il == null)
            {
                return null;
            }
            switch (il.Name)
            {
                case "UIConfig":

                    obj = Utility.Instance.GetInteractiveControlObject(filePath);

                    break;
                case "Chart":

                    obj = Utility.Instance.GetChartRuleObject(filePath);

                    break;
                case "DataSource":

                    obj = Utility.Instance.GetDataSourceObject(filePath);

                    break;
                case "DataList":

                    obj = Utility.Instance.GetDataListObject(filePath);

                    break;
                default :
                    obj = null;
                    break;

            }//End switch;
            il.Tag = obj;
            return obj;
        }

        // ---> 初始化日志对象，用于记录日志系统本身的日志 
        /// <summary>
        /// 初始化日志对象，用于记录日志系统本身的日志 
        /// </summary>
        /// <returns>成功则返回true；失败则返回false</returns>
        /// <remarks>
        ///     该函数必须要系统初始化时首先调用。因为其他模块进行初始化时，均要记录日志。
        /// </remarks>
        public bool ImportLogDll()
        {
            try
            {
                string logSavePath = @".\SelfLogFile";
                if (!System.IO.Directory.Exists(logSavePath))
                    System.IO.Directory.CreateDirectory(logSavePath);

                string logConfigIniPath = @".\Dll\logCppConfig.ini";

                WriteLog.InitLogInstance(logConfigIniPath, "WS_RULE");

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion --> 方法
        
        #region --> 预览
        /// <summary>
        /// 预览。
        /// </summary>
        /// <param name="ic">ItemCom 对象</param>
        public void Preview(ItemCom ic)
        {
            System.Windows.Window w = new System.Windows.Window();
            //this.ic = ic;
            //-->将ic对象存放到w.Tag里面去，当前窗体要关闭的时候要用到。
            w.Tag = ic;
            w.Closed += new EventHandler(w_Closed);
            
            try
            {
                switch (ic.Name)
                {
                    case "UIConfig":

                        InteractiveControl uiConfig = new InteractiveControl();
                        uiConfig.LabWidthPercent = 25;
                        uiConfig.RowHeight = 50;
                        uiConfig.RowSpace = 20;
                        uiConfig.LabWidthPercent = 45;
                        uiConfig.Initialize(ic.Tag as InteractiveControlRule);
                        

                        w.Title = "预览UI交互界面";

                        w.Content = uiConfig;

                        break;

                    case "Chart":

                        ChartControl ccConfig = new ChartControl();

                        ccConfig.Initialize(ic.Tag as ChartRule);

                        w.Title = "预览图表界面";

                        w.Content = ccConfig;

                        break;

                    case "DataList":

                        DataListControl dlcConfig = new DataListControl();

                        dlcConfig.Initliaize(ic.Tag as DataListRule);

                        w.Title = "预览DataList界面";

                        w.Content = dlcConfig;

                        break;

                    default:

                        break;
                }
                w.ShowDialog();
            }
            catch (Exception ee)
            {
                WriteLog.Log_Error(ee);
            }
        }
        /// <summary>
        /// 返回预览窗体事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        void w_Closed(object sender, EventArgs e)
        {
            System.Windows.Window w = sender as System.Windows.Window;
            //object obj = w.Tag;
            ItemCom ic = w.Tag as ItemCom;

            string dataSourceName = null ;
            if (ic.Tag is ChartRule)
            {
                ChartRule cr = ic.Tag as ChartRule;
                if (cr != null)
                    dataSourceName = cr.DataSourceName;
            }
            if (ic.Tag is DataListRule)
            {
                DataListRule dlr = ic.Tag as DataListRule;
                if (dlr != null)
                    dataSourceName = dlr.DataSourceName;
            }
            DataSourceManager.DisponseDataSource(dataSourceName);
            Utility.Instance.PreviewFormClose(false);
        }
        #endregion --> 预览
    }
}