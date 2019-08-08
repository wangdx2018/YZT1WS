using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace AFC.WS.UI.Config
{    /// <summary>
    /// DataSeries属性;
    /// 
    /// 主要是配置DataSeries当中图例名称以及Y轴绑定字段
    /// </summary>
    public class DataSeriesProperty
    {
        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(LegnedName))
            {
                return this.GetType().Name;
            }
            else
            {
                return LegnedName;
            }
        }

        #endregion --> Methods

        #region --> Property.

        /// <summary>
        /// Y轴绑定字段
        /// </summary>
        private string _AxisYValue;
        /// <summary>
        /// Y轴绑定字段
        /// </summary>
        [DisplayName("Y轴绑定字段"),
        XmlAttribute(),
        Category("属性设置")]
        public string AxisYValue
        {
            get { return _AxisYValue; }
            set { _AxisYValue = value; }
        }

        /// <summary>
        /// 图例名称
        /// </summary>
        private string _LegendName;
        /// <summary>
        /// 图例名称
        /// </summary>
        [XmlAttribute(),
        DisplayName("图例名称"),
        Category("属性设置")]
        public string LegnedName
        {
            get { return _LegendName; }
            set { _LegendName = value; }
        }

        #endregion --> Property

    }
}
