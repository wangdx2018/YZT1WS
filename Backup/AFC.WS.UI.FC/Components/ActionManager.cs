using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.Actions;

namespace AFC.WS.UI.Components
{
    /// <summary>
    /// 该类负责创建Action和创建Action的布局
    /// </summary>
    internal class ActionManager
    {
        /// <summary>
        /// action对应Button单击事件
        /// </summary>
        /// <param name="sender">发起者</param>
        /// <param name="e">路由事件源</param>
        public delegate void ActionClicked(object sender, RoutedEventArgs e);

        /// <summary>
        /// 创建Action布局,返回一个StackPanel action控件容器
        /// </summary>
        /// <param name="location">Action 容器的位置 ：上，下，左，右</param>
        /// <param name="aligns">Action中的Button的对齐方式，左，中，右</param>
        /// <returns>返回Action的布局容器</returns>
        public static StackPanel CreateActionLayout(ActionLocations location, ActionAligns aligns)
        {

            StackPanel sp = new StackPanel();
           
            switch (location)
            {
                case ActionLocations.Bottom:
                case ActionLocations.Top: // 在上，下为横行摆放
                    {
                        sp.Orientation = Orientation.Horizontal;
                        switch (aligns)
                        {
                            case ActionAligns.Left:
                                sp.HorizontalAlignment = HorizontalAlignment.Left;
                                break;
                            case ActionAligns.Middle:
                                sp.HorizontalAlignment = HorizontalAlignment.Center;
                                break;
                            case ActionAligns.Right:
                                sp.HorizontalAlignment = HorizontalAlignment.Right;
                                break;
                        }
                    }
                    break;
                case ActionLocations.Left:
                case ActionLocations.Right: //左右为竖行摆放
                    {
                        sp.Orientation = Orientation.Vertical;
                        switch (aligns)
                        {
                            case ActionAligns.Left:
                                sp.VerticalAlignment = VerticalAlignment.Top;
                                break;
                            case ActionAligns.Right:
                                sp.VerticalAlignment = VerticalAlignment.Bottom;
                                break;
                            case ActionAligns.Middle:
                                sp.VerticalAlignment = VerticalAlignment.Center ;
                                break;
                        }
                    }
                    break;
            }
            return sp;
        }

        /// <summary>
        /// Button的默认的宽度
        /// </summary>
        public const double Btn_Default_Width = 75;

        /// <summary>
        /// Button的默认的高度
        /// </summary>
        public const double Btn_Default_Height = 22.5;

        /// <summary>
        /// 创建Action
        /// </summary>
        /// <param name="property">Action中控件的属性</param>
        /// <param name="eventHandler">ActionClicked函数代理</param>
        /// <param name="btnStyle">控件样式</param>
        /// <returns></returns>
        public static KeyValuePair<string, KeyValuePair<IAction, Button>> CreateAction(ActionProperty property, ActionClicked eventHandler, string btnStyle, int tabIndex)
        {
            try
            {
                WriteLog.Log_Info("will create action type name=[" + property.ActionTypeName + "]");
                Type type = Type.GetType(property.ActionTypeName);
                if (type == null)
                    throw new Exception("Get action Type error: [" + property.ActionTypeName + "]");
                IAction action = Activator.CreateInstance(type) as IAction;
                if (action != null)
                {
                    Button btn = new Button();//create button
                    btn.IsDefault = property.IsDefaultButton;
                 //   btn.IsDefaulted = btn.IsDefault;
                    btn.TabIndex = tabIndex;
                    UIHelper.SetControlStyle(btn, btnStyle);//set button style
                    UIHelper.SetButtonProperty(btn);
                    btn.Name = property.ControlName;//set control name
                    btn.Content = property.Content;
                   // btn.IsDefault = true;
                    btn.Click += new RoutedEventHandler(eventHandler);
                    for (int i = 0; i < property.PropertyValues.Count; i++) //initliaize userdefined properties.
                    {
                        PropertyInfo pi = action.GetType().GetProperty(property.PropertyValues[i].Key);
                        if (pi == null)
                        {
                          //  WriteLog.Log_Error("Get Property error: [" + pi.Name + "] not found!! ");
                            break;
                        }
                        object res = UIHelper.ParsePropertyValue(pi, property.PropertyValues[i].Value);
                        if(res!=null)
                        pi.SetValue(action, res, null);
                    }
                    return new KeyValuePair<string, KeyValuePair<IAction, Button>>(property.ControlName, new KeyValuePair<IAction, Button>(action, btn));
                }
                throw new Exception("create action error:Type=[" + property.ActionTypeName + "]");

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return new KeyValuePair<string, KeyValuePair<IAction, Button>>(string.Empty, new KeyValuePair<IAction, Button>(null, null));
            }
        }


        
    }
}
