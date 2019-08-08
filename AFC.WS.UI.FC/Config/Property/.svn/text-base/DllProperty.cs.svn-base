using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 存放DLL的属性类。
    /// 
    /// 主要是包括DLL的名称以及DLL里面类的属性集合。
    /// 
    /// </summary>
    public class DllProperty
    {
        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Namespace))
            {
                return "";
            }
            else
            {

                return Namespace;
            }
        }

        /// <summary>
        /// 添加DllClassProperty类对象到DllClassPropertyList集合中去。
        /// </summary>
        /// <param name="dcp">DllClassProperty类对象</param>
        public void Add(DllClassProperty dcp)
        {
            if (IsExist(dcp))
            {
                Remove(dcp);
            }
            DllClassPropertyList.Add(dcp);
        }

        /// <summary>
        /// 判断DllClassPropertyList中是否已经存在DllClassProperty对象
        /// </summary>
        /// <param name="dcp">DllClassProperty类对象</param>
        /// <returns>true:存在；false:不存在。</returns>
        private bool IsExist(DllClassProperty dcp)
        {
            bool result = false;

            foreach (DllClassProperty obj in DllClassPropertyList)
            {
                if (obj.FullName == dcp.FullName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 删除DllClassPropertyList中是否已经存在DllClassProperty对象
        /// </summary>
        /// <param name="dcp">DllClassProperty类对象</param>
        private void Remove(DllClassProperty dcp)
        {
            DllClassProperty del = null;
            foreach (DllClassProperty obj in DllClassPropertyList)
            {
                if (obj.FullName == dcp.FullName)
                {
                    del = obj;
                    break;
                }
            }

            DllClassPropertyList.Remove(del);
        }

        #endregion --> Methods

        #region --> Property.

        ///// <summary>
        ///// DLL的名称。
        ///// </summary>
        //private string _DllName;
        ///// <summary>
        ///// DLL的名称。
        ///// </summary>
        //public string DllName
        //{
        //    get { return _DllName; }
        //    set { _DllName = value; }
        //}

        /// <summary>
        /// 名稱空間
        /// </summary>
        private string _Namespace;
        /// <summary>
        /// 名稱空間
        /// </summary>
        public string Namespace
        {
            get { return _Namespace; }
            set { _Namespace = value; }
        }

        /// <summary>
        /// Dll当中的类的属性。
        /// </summary>
        private List<DllClassProperty> _DllClassPropertyList = new List<DllClassProperty>();
        /// <summary>
        /// Dll当中的类的属性。
        /// </summary>
        public List<DllClassProperty> DllClassPropertyList
        {
            get { return _DllClassPropertyList; }
            set { _DllClassPropertyList = value; }
        }

        #endregion --> Property.

        #region --> Conformation Methods

        /// <summary>
        /// DLL属性构造方法
        /// </summary>
        public DllProperty()
        {
        }
        
        /// <summary>
        /// DLL属性构造方法
        /// </summary>
        /// <param name="nameSpace">Dll的名称</param>
        public DllProperty(string nameSpace)
            : this(nameSpace, null)
        {
        }
        
        /// <summary>
        /// DLL属性构造方法
        /// </summary>
        /// <param name="nameSpace">Dll的名称</param>
        /// <param name="wsdp">Dll里的类的属性</param>
        public DllProperty(string nameSpace, List<DllClassProperty> wsdp)
        {
            this.Namespace = nameSpace;
            this.DllClassPropertyList = wsdp;
        }

        #endregion --> Conformation Methods.

    }

}
