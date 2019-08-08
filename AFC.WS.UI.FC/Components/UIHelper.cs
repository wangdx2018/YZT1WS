using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;
using System.Windows.Controls;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace AFC.WS.UI.Components
{
    
    /// <summary>
    /// 该类中定义了该组件中所需要的一些通用方法
    /// </summary>
    public class UIHelper
    {
        /// <summary>
        /// 设置Control的Style
        /// </summary>
        /// <param name="control">控件对象</param>
        /// <param name="key">样式ID</param>
        /// <returns>成功返回0， 异常返回-1</returns>
        public static int SetControlStyle(FrameworkElement control, string key)
        {
            try
            {

                /*********************************************************************
                 *  Add Date：2009-07-29 PM
                 *  
                 *      Note：加一个判断，判断config是否为空。
                 * 
                 * *******************************************************************/
                if (control == null)
                {
                    WriteLog.Log_Error(" set control params is null at function [ public static int SetControlStyle(FrameworkElement control, string key) ] ");
                    return -1;
                }

                if (string.IsNullOrEmpty(key))
                {
                    WriteLog.Log_Warn(" style  Key  is null or empty! control name is ["+control.Name+"]");
                    return -1;
                }
              

                WriteLog.Log_Info("set control name=[" + control.Name + "]" + " key=[" + key + "]");
                control.SetResourceReference(FrameworkElement.StyleProperty, key);
                return 0;
            }
            catch (Exception ex)
            {
               WriteLog.Log_Error(" Set control style error !ControlName=["+control.Name+"] Style=["+key+"] " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 在设置属性的时候将字符串转换成需要设置的数据类型
        /// </summary>
        /// <param name="pi">属性信息</param>
        /// <param name="value">字段</param>
        /// <returns>返回转换之后的数据</returns>
        public  static object ParsePropertyValue(PropertyInfo pi, string value)
        {
            try
            {
                if (pi.PropertyType.IsEnum)
                {
                    return Enum.Parse(pi.PropertyType, value);
                }

                if (pi.PropertyType.Name == "Boolean")
                {
                    return bool.Parse(value);
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

                if (pi.PropertyType.Name == "Decimal")
                {
                    Decimal res = 0;
                    if (Decimal.TryParse(value, out res))
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

        /// <summary>
        /// action对应Button单击事件
        /// </summary>
        /// <param name="sender">发起者</param>
        /// <param name="e">路由事件源</param>
        public delegate void ButtonClicked(object sender, RoutedEventArgs e);

        /// <summary>
        /// 创建控件中的按钮，如到出到Excel，翻页等
        /// </summary>
        /// <param name="content">控件上显示的内容</param>
        /// <param name="clicked">按钮单击事件</param>
        /// <param name="controlName">控件的名字</param>
        /// <param name="key"></param>
        /// <returns>返回Button的实例</returns>
        public static Button CreateButton(string content, ButtonClicked clicked,string controlName,string key)
        {
            Button btn = new Button();
            SetControlStyle(btn, key);
            SetButtonProperty(btn);
            btn.Content = content;
            btn.Click+=new RoutedEventHandler(clicked);
            btn.Name = controlName;
            return btn;
        }


        public static void SetButtonProperty( Button btn)
        {
                SetButtonProperty(btn, Button.HeightProperty);
                SetButtonProperty(btn, Button.WidthProperty);
        }

        /// <summary>
        /// 设置Button的属性
        /// </summary>
        /// <param name="btn">按钮</param>
        /// <param name="property"></param>
        private static void SetButtonProperty(Button btn,DependencyProperty property)
        {
            try
            {
                if (btn.Style == null)
                {
                    btn.Height = ActionManager.Btn_Default_Height;
                    btn.Width = ActionManager.Btn_Default_Width;
                    return;
                }
                SetterBase setter = btn.Style.Setters.Single(temp =>
                {
                    Setter data = temp as Setter;
                    return data.Property == property;
                });
                if (setter != null)
                {
                    if(property==Button.WidthProperty)
                    btn.Width = Convert.ToDouble((setter as Setter).Value);
                    if (property == Button.HeightProperty)
                        btn.Height = Convert.ToDouble((setter as Setter).Value);
                }
            }
            catch (Exception ex)
            {
                if(property==Button.HeightProperty)
                btn.Height = ActionManager.Btn_Default_Height;
                if (property == Button.WidthProperty)
                    btn.Width = ActionManager.Btn_Default_Width;
                WriteLog.Log_Error(ex);
            }
        }
        /// <summary>
        /// 控件间隔的宽度
        /// </summary>
         public const int ControlSpaceWidth = 10;

        /// <summary>
        /// 控件间隔的高度
        /// </summary>
         public const int ControlSpaceHeight = 10;

        /// <summary>
        /// 添加一个控件的间隔
        /// </summary>
        /// <returns>控件间隔</returns>
         public static UIElement GetControlSpace()
         {
             TextBlock tb = new TextBlock { Width = ControlSpaceWidth, Height = ControlSpaceHeight };
             return tb;
         }

         /// <summary>
         /// 通过键值取得资源文件中的Value
         /// </summary>
         /// <param name="key">资源字典中定义的Key</param>
         /// <returns>返回Key对应的Value字段</returns>
         public static string GetResouceValueByID(object key)
         {
             string value = string.Empty;
             if (key == null)
             {
                 return string.Empty;
             }
             try
             {
                 object data = Application.Current.FindResource(key);
                 if (data != null)
                     return data.ToString();
                 else
                     return key.ToString();


             }
             catch
             {
                 value = key.ToString();
             }
             return value;

         }

        /// <summary>
        /// 导入资源文件
        /// </summary>
        /// <param name="fileName">资源字典文件名</param>
         public static void LoadLanguage(string fileName)
         {
             CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;
             try
             {
                 WriteLog.Log_Info("load resource fileName=[" + fileName + "] !");
                 ResourceDictionary rd = Application.LoadComponent(

                  new Uri(fileName, UriKind.Relative)) as ResourceDictionary;

                 if (rd != null)
                 {
                     Application.Current.Resources.MergedDictionaries.Add(rd);
                 }
             }
             catch
             {

             }

         }

        /// <summary>
        /// 检查是否是Binding Key字段，Format字段中
        /// 是否有{ }
        /// </summary>
        /// <param name="value">key字段</param>
        /// <returns>成功返回true，否则返回false</returns>
         public static bool CheckIsKeyBinding(string value)
         {
             if (string.IsNullOrEmpty(value))
                 return false;
             return value.Contains('{') && value.Contains('}');
         }

        /// <summary>
        /// 返回BindingFormatValue字符串
        /// </summary>
        /// <param name="value">得到binding值的字符串</param>
        /// <returns>返回返回的数值</returns>
         public static string GetBindingFormatValue(string value)
         {
             if (!CheckIsKeyBinding(value))
                 return value;
             else
             {
                 int startIndex = value.IndexOf('{');
                 int endIndex = value.IndexOf('}');
                 string key=value.Substring(startIndex, endIndex - startIndex);
                 return GetResouceValueByID(key);
             }
         }
    }


}

