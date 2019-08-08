using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;

namespace AFC.WS.UI.Config
{

    public class DataSourceRule
    {
        private string _SaveFileName;
        [Description("保存文件名"),
        DisplayName("保存文件名"),
        Category("属性")]
        public string SaveFileName
        {
            get { return _SaveFileName; }
            set { _SaveFileName = value; }
        }

        private List<DataSourceProperty> _DataSourceList;
        [Description("DataSource集合"),
        DisplayName("DataSource集合"),
        Category("属性")]
        public List<DataSourceProperty> DataSourceList
        {
            get
            {
                if (null == _DataSourceList)
                {
                    _DataSourceList = new List<DataSourceProperty>();
                }
                return _DataSourceList;
            }
            set { _DataSourceList = value; }
        }
    }
}
