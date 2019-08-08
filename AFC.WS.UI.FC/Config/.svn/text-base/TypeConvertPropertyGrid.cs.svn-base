using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// Convertor转换类。
    /// 
    /// 主要是用来将Convertor DLL里的类名称集体一个ArrayList集体一样，
    /// 
    /// 然后把这个ArrayList存放到PropertyGrid上去。
    /// </summary>
    public class TypeConvertPropertyGridConvertor : ComboBoxTypeConvert
    {  
        /// <summary>
        /// 获取转换列集合。
        /// </summary>
        public override void GetConvertList()
        {
            this.DllClassNameList = Utility.Instance.ClassPropertyConvertorList;
        }
    }

    /// <summary>
    /// 操作按钮转换类
    /// 
    /// 主要是用来将Action DLL里的类名称集体一个ArrayList集体一样，
    /// 
    /// 然后把这个ArrayList存放到PropertyGrid上去。
    /// </summary>
    public class TypeConvertPropertyGridAction : ComboBoxTypeConvert
    {
        /// <summary>
        /// 获取转换列集合。
        /// </summary>
        public override void GetConvertList()
        {
            this.DllClassNameList = Utility.Instance.ClassPropertyActionList;
        }
    }

    /// <summary>
    /// 数据源转换类
    /// 
    /// 主要是用来将DataSource DLL里的类名称集体一个ArrayList集体一样，
    /// 
    /// 然后把这个ArrayList存放到PropertyGrid上去。
    /// </summary>
    public class TypeConvertPropertyGridDataSource : ComboBoxTypeConvert
    {
        /// <summary>
        /// 获取转换列集合。
        /// </summary>
        public override void GetConvertList()
        {
            this.DllClassNameList = Utility.Instance.ClassPropertyDataSourceList;
        }
    }

    /// <summary>
    /// 控件类转换类
    /// 
    /// 主要是用来将Controls DLL里的类名称集体一个ArrayList集体一样，
    /// 
    /// 然后把这个ArrayList存放到PropertyGrid上去。
    /// </summary>
    public class TypeConvertPropertyGridControls : ComboBoxTypeConvert
    {
        /// <summary>
        /// 获取转换列集合。
        /// </summary>
        public override void GetConvertList()
        {
            this.DllClassNameList = Utility.Instance.ClassPropertyControlsList;
        }
    }

}
