using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 存放Dll里面类的属性；
    /// 
    /// 主要是用来记录用户导入ＤＬＬ后，ＤＬＬ里面类的一些属性，类的程序集名称、类的全名称、
    /// 
    /// 类名称、类的名称空间以及类的Type。
    /// </summary>
    public class DllClassProperty
    {
        #region --> Property.

        /// <summary>
        /// 程序集名称
        /// </summary>
        private string _AssemblyName;
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName
        {
            get { return _AssemblyName; }
            set { _AssemblyName = value; }
        }
        /// <summary>
        /// 类全名称
        /// </summary>
        private string _FullName;
        /// <summary>
        /// 类全名称
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        /// 类名称
        /// </summary>
        private string _ClassName;
        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        /// <summary>
        /// 名称空间
        /// </summary>
        private string _NameSpace;
        /// <summary>
        /// 名称空间
        /// </summary>
        public string NameSpace
        {
            get { return _NameSpace; }
            set { _NameSpace = value; }
        }
        /// <summary>
        /// Dll的Class类的类型。
        /// </summary>
        private Type _DllClassType;
        
        /// <summary>
        /// Dll的Class类的类型。
        /// </summary>
        public Type DllClassType
        {
            get { return _DllClassType; }
            set { _DllClassType = value; }
        }

        #endregion --> Property.

        #region --> Conformation Methods
        /// <summary>
        /// DLL类属性赋值
        /// </summary>
        public DllClassProperty()
            : this(null)
        {
        }
        /// <summary>
        /// DLL类属性赋值
        /// </summary>
        /// <param name="dllClassType">Dll的Class类的类型</param>
        public DllClassProperty(Type dllClassType)
            : this(dllClassType, null)
        {
        }
        
        /// <summary>
        /// DLL类属性赋值
        /// </summary>
        /// <param name="dllClassType">Dll的Class类的类型</param>
        /// <param name="assemblyName">程序集名称</param>
        public DllClassProperty(Type dllClassType, string assemblyName)
            : this(dllClassType, assemblyName, null)
        {
        }
        
        /// <summary>
        /// DLL类属性赋值
        /// </summary>
        /// <param name="dllClassType">Dll的Class类的类型</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="fullName">类全名称</param>
        public DllClassProperty(Type dllClassType, string assemblyName, string fullName)
            : this(dllClassType, assemblyName, fullName, null)
        {
        }
        
        /// <summary>
        /// DLL类属性赋值
        /// </summary>
        /// <param name="dllClassType">Dll的Class类的类型</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="fullName">类全名称</param>
        /// <param name="nameSpace">名称空间</param>
        public DllClassProperty(Type dllClassType, string assemblyName, string fullName, string nameSpace)
            : this(dllClassType, assemblyName, fullName, nameSpace, null)
        {
        }
        
        /// <summary>
        /// DLL类属性赋值
        /// </summary>
        /// <param name="dllClassType">Dll的Class类的类型</param>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="fullName">类全名称</param>
        /// <param name="nameSpace">名称空间</param>
        /// <param name="className">类名称</param>
        public DllClassProperty(Type dllClassType, string assemblyName, string fullName, string nameSpace, string className)
        {
            this.DllClassType = dllClassType;
            this.NameSpace = nameSpace;
            this.ClassName = className;
            this.FullName = fullName;
            this.AssemblyName = assemblyName;
        }

        #endregion --> Conformation Methods
    }
}
