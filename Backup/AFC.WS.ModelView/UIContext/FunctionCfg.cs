using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AFC.BOM2.Common;

namespace AFC.WS.ModelView
{
    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110222
    /// 代码功能：导航菜单内容,创建方式 单件。
    /// 修订记录：
    /// </summary>
    public class FunctionDataCollection
    {

        private static FunctionDataCollection fdc = null;


        private FunctionDataCollection()
        {

        }

        public static FunctionDataCollection GetFunctionDataCollection()
        {
            if (fdc == null)
                fdc = new FunctionDataCollection();
            return fdc;
        }

        /// <summary>
        /// 每个操作员都有的角色
        /// </summary>
        public const string totalAllPrimissionFunction = "00000000";

        //-->mainList(主菜单，只是有一级的子菜单)
        /// <summary>
        /// mainList(主菜单，只是有一级的子菜单)
        /// </summary>
        private List<Function> mainList = new List<Function>();

        //-->该字典中包含了所有的菜单集合
        /// <summary>
        /// 该字典中包含了所有的菜单集合
        /// </summary>
        private Dictionary<uint, Function> dict = new Dictionary<uint, Function>();

        //--->加载解析配置文件
        /// <summary>
        /// 加载功能按钮的配置文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>加载解析成功返回true，否则返回false</returns>
        public bool LoadFunctionConfig(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                WriteLog.Log_Error("fileName=[" + fileName + "] doesn't exist!");
                return false;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(fileName);
                    XmlElement element = doc.DocumentElement;
                    XmlNodeList list = element.ChildNodes;
                    for (int i = 0; i < list.Count; i++)
                    {
                        int res = 0;
                        Function fun = ParseFunction(list[i], out res);
                        if (fun != null)
                            mainList.Add(fun);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                    return false;
                }
            }
        }

        //---> 通过ID找到具体的某个功能菜单
        /// <summary>
        /// 通过ID找到具体的某个功能菜单
        /// </summary>
        /// <param name="id">菜单的ID</param>
        /// <returns>如果存在该ID 返回，否则返回null</returns>
        public Function GetFunctionById(uint id)
        {
            if (!dict.ContainsKey(id))
            {
                WriteLog.Log_Error("can't find  function  key=[ " + id.ToString() + "]");
                return null;
            }
            return dict[id];
        }

        /// <summary>
        /// 通过BuinessControlName获得具体的Function
        /// </summary>
        /// <param name="name">业务控件名</param>
        /// <returns>返回functionID</returns>
        public uint GetFunctionByBuinessControlName(string name)
        {
            return this.dict.Single(temp => temp.Value.buinessControlId.Equals(name)).Key;
        }

        //-->通过节点的ID找到相应的子节点的ID集合
        /// <summary>
        /// 通过节点的ID找到相应的子节点的ID
        /// </summary>
        /// <param name="id">父节点的ID</param>
        /// <returns>返回该ID的子节点的集合</returns>
        public List<Function> GetAllChildernByParentsId(uint id)
        {
            Function func = GetFunctionById(id);
            if (func == null)
                return null;
            else
                return func.Childern;
        }

        //-->解析某个Function
        /// <summary>
        /// 解析某个Function
        /// </summary>
        /// <param name="node">XmlNode</param>
        /// <returns>成功0，否则-1</returns>
        private Function ParseFunction(XmlNode node, out int result)
        {
            bool res = false;
            Function function = new Function();
            function.Childern = new List<Function>();
            res = XmlOperator.FetchAttributeValue(node, "id", ref function.functionId);
            if (!res)
            {
                result = -1;
                return null;
            }
            res = XmlOperator.FetchAttributeValue(node, "buinessControlId", ref function.buinessControlId);
            if (!res)
            {
                result = -1;
                return null;
            }

            res = XmlOperator.FetchAttributeValue(node, "parentId", ref function.parentId);


            res = XmlOperator.FetchAttributeValue(node, "text", ref function.text);
            if (!res)
            {
                result = -1;
                return null;
            }

            res = XmlOperator.FetchAttributeValue(node, "IsVisable", ref function.isVisable);
            if (!res)
            {
                result = -1;
                return null;
            }

            res = XmlOperator.FetchAttributeValue(node, "SysFunctionId", ref function.sysFunctionId);
            //if (!res)
            //{
            //    result = -1;
            //    return null;
            //}

            res = XmlOperator.FetchAttributeValue(node, "paramsData", ref function.paramsData);
            //if (!res)
            //{
            //    result = -1;
            //    return null;
            //}

            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    Function fun = ParseFunction(node.ChildNodes[i], out result);
                    if (result == 0)
                    {

                        function.Childern.Add(fun);
                    }
                }
            }
            if (!dict.ContainsKey(function.functionId))
            {
                dict.Add(function.functionId, function);
                result = 0;
                return function;
            }

            result = -1;
            return null;
        }

        //--->得到可以看见的菜单
        /// <summary>
        /// 得到可见的菜单项
        /// </summary>
        /// <param name="id">菜单的ID</param>
        /// <returns>返回对应菜单的子项</returns>
        public List<Function> GetAllVisableChildrenByParentId(uint id)
        {
            Function fun = GetFunctionById(id);
            if (fun != null && fun.Childern != null)
            {
                var temp = from data in fun.Childern
                           where data.isVisable
                           select data;
                return temp.ToList();
            }
            return null;
        }

        //-->根据控件的Text得到该菜单对象的实例
        /// <summary>
        /// 根据控件的Text得到该菜单对象的实例
        /// </summary>
        /// <param name="text">菜单的文本</param>
        /// <returns>返回拥该文本的菜单对象</returns>
        public Function GetFunctionByText(string text)
        {
            var temp = from data in dict.Values
                       where data.text.Equals(text)
                       select data;
            List<Function> list = temp.ToList();
            if (list != null && list.Count > 0)
                return list[0];
            return null;
        }

        //-->得到主菜单
        /// <summary>
        /// 得到MainFunctionList的菜单
        /// </summary>
        /// <returns>得到主菜单</returns>
        public List<Function> GetMainFunctionList()
        {
            //todo通过子节点中如果有不为false的则为最终的结果。
            return this.mainList;
        }

        /// <summary>
        /// 通过系统功能ID得到的Function的信息列表，配置中SysFunctionID=“00000000”的为所有
        /// 操作员不分角色都有的功能
        /// </summary>
        /// <param name="sysFunctionIdCollection">系统功能ID集合（权限信息集合）</param>
        /// <returns></returns>
        private List<Function> GetFunctionInfoBySysFunctionId(List<string> sysFunctionIdCollection)
        {
            if (sysFunctionIdCollection == null)
                return null;
            List<Function> totalFunctionList = (from temp in this.dict select temp.Value).ToList();//所有的数据类型

            List<Function> collection = (from temp in sysFunctionIdCollection
                                         from data in totalFunctionList
                                         where temp.ToLower().Equals(data.sysFunctionId.ToLower())
                                         select data).ToList();
            collection.AddRange(totalFunctionList.Where(temp => temp.sysFunctionId.Equals("00000000")));//添加所有操作员都存在的功能
            return collection.ToList();
        }

        /// <summary>
        /// 得到系统的功能ID，key对应parentFunction，Value对应子功能列表
        /// </summary>
        /// <param name="functionIdCollection">子功能的Function集合</param>
        /// <returns></returns>
        public Dictionary<Function, List<Function>> GetParentFunctionCollection(List<string> functionIdCollection)
        {
            List<Function> list = GetFunctionInfoBySysFunctionId(functionIdCollection);
            Dictionary<Function, List<Function>> dict = new Dictionary<Function, List<Function>>();
            var collection = list.GroupBy(temp => temp.parentId).OrderBy(temp => temp.Key);
            foreach (var temp in collection)
            {
                dict.Add(GetFunctionById(temp.Key), temp.ToList().OrderBy(a => a.functionId).ToList());
            }
            return dict;
        }

        /// <summary>
        /// 通过控件名称得到系统的功能ID
        /// </summary>
        /// <param name="buinessControlName">组件名称</param>
        /// <returns>返回功能名称</returns>
        public uint GetFunctionIdByBuinessControlName(string buinessControlName)
        {
            try
            {
                return dict.Single(temp => temp.Value.buinessControlId == buinessControlName).Key;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message + " buinessControlName=" + buinessControlName);
                return 0;
            }
        }
    }

    //-->导航功能菜单的实体类
    /// <summary>
    /// 导航功能菜单的实体类
    /// </summary>
    public class Function
    {
        //--> Function 的编号
        /// <summary>
        /// Function 功能的编号
        /// </summary>
        public uint functionId;

        //-->控件上显示的文本信息
        /// <summary>
        /// 控件上显示的文本信息
        /// </summary>
        public string text;

        /// <summary>
        /// 业务控件的ID
        /// </summary>
        public string buinessControlId;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool isVisable;

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public uint parentId;

        /// <summary>
        /// 切换界面中带入的数据
        /// </summary>
        public string paramsData;


        public string sysFunctionId = string.Empty;

        //--->该级别目录下的子菜单
        /// <summary>
        /// 该级别目录下的子菜单
        /// </summary>
        public List<Function> Childern
        {
            set;
            get;
        }

        //--->构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Function()
        {

        }



    }
}
