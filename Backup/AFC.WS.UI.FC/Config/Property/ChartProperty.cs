using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visifire.Charts;
using System.Xml.Serialization;
using System.ComponentModel;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 图表属性；
    /// 
    /// 主要是配置图表中图表名称及图表类型（Column、Line、Pie）等。
    /// </summary>
    [Description("图表属性")]
    public class ChartProperty
    {
        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))
            {
                return this.GetType().Name;
            }
            else
            {
                return Name;
            }
        }
        /// <summary>
        /// 对Chart设置值
        /// </summary>
        /// <param name="ra">RenderAs枚举</param>
        private void ChartName(RenderAs ra)
        {
            string temp = "";
            switch (ra)
            {
                case RenderAs.Area:
                    temp = "区域图";
                    break;
                case RenderAs.Bar:
                    temp = "条形图";
                    break;
                case RenderAs.Bubble:
                    temp = "泡沫图";
                    break;
                case RenderAs.Column:
                    temp = "柱状图";
                    break;
                case RenderAs.Doughnut:
                    temp = "圆环图";
                    break;
                case RenderAs.Line:
                    temp = "折线图";
                    break;
                case RenderAs.Pie:
                    temp = "饼状图";
                    break;
                case RenderAs.Point:
                    temp = "点形图";
                    break;
                case RenderAs.StackedArea:
                    temp = "StackedArea";
                    break;
                case RenderAs.StackedArea100:
                    temp = "StackedArea100";
                    break;
                case RenderAs.StackedBar:
                    temp = "StackedBar";
                    break;
                case RenderAs.StackedBar100:
                    temp = "StackedBar100";
                    break;
                case RenderAs.StackedColumn:
                    temp = "StackedColumn";
                    break;
                case RenderAs.StackedColumn100:
                    temp = "StackedColumn100";
                    break;
                default:
                    break;
            }
            if (String.IsNullOrEmpty(this._Name))
            {
                this.Name = temp;
            }
            else
            {
                if (this.Name != temp)
                {
                    this.Name = temp;
                }
            }

        }

        #endregion --> Methods.

        #region --> Property.

        /// <summary>
        /// 图表名称[如:折线图、饼状图、柱状图等]
        /// </summary>
        private string _Name;
        /// <summary>
        /// 图表名称[如:折线图、饼状图、柱状图等]
        /// </summary>
        [Description("图表名称[如:折线图、饼状图、柱状图等]"),
        DisplayName("名称"),
        XmlAttribute(),
        Category("属性设置")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 图表类型
        /// </summary>
        private RenderAs _ChartType = RenderAs.Column;
        /// <summary>
        /// 图表类型
        /// </summary>
        [Description("图表类型"),
        DisplayName("图表类型"),
        XmlAttribute(),
        Category("属性设置")]
        public RenderAs ChartType
        {
            get { return _ChartType; }
            set
            {
                _ChartType = value;
                ChartName(value);
            }
        }

        #endregion --> Property.

    }
}
