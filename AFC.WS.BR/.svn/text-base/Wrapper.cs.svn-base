using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Controls;
using AFC.WS.UI.CommonControls;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Collections;
using AFC.WS.UI.Config;
using System.Windows;
using Microsoft.Windows.Controls;
using System.Windows.Controls.Primitives;
using AFC.WS.UI.Common;
using System.Windows.Documents;
using System.Windows.Media;

namespace AFC.WS.BR
{
    public class Wrapper
    {
        /// <summary>
        /// 实例化Wrapper类
        /// </summary>
        private static Wrapper _Instance;
        /// <summary>
        /// 实例化Wrapper类
        /// </summary>
        public static Wrapper Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance = new Wrapper();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <param name="cbb">ComboBox控件名称</param>
        /// <param name="dt">Datatable</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        /// <param name="IsAddChoose">是否添加"--请选择--"内容</param>
        /// <param name="canSelectedIndex">是否将ComboBox.SelectedIndex赋值，true:ComboBox.SelectedIndex=0；否则不进行赋值操作。</param>
        public static void FullComboBox(ComboBox cbb, DataTable dt, string displayName,
            string value, bool IsAddChoose, bool canSelectedIndex)
        {
            if (cbb == null)
            {
                return;
            }
            cbb.Items.Clear();
            if (dt != null)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Name = "cbb_" + displayName;
                item.Tag = dt;
                item.Uid = "%";
                if (IsAddChoose && dt.Rows.Count > 0)
                {
                    item.Content = "--请选择--";
                    cbb.Items.Add(item);
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new ComboBoxItem();
                        try
                        {
                            item.Content = ConvertToString(dr, displayName);
                            item.Name = "cbb_" + ConvertToString(dr, value);
                            item.Uid = ConvertToString(dr, value);
                            item.Tag = dr;
                            cbb.Items.Add(item);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.DebugFormat);
                        }
                    }
                }
                else
                {
                    item.Content = "无";
                    cbb.Items.Add(item);
                }
            }

            if (cbb.Items.Count > 1 && canSelectedIndex)
            {
                cbb.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <param name="cbb">ComboBox控件名称</param>
        /// <param name="dt">Datatable</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        /// <param name="IsAddChoose">是否添加"--请选择--"内容</param>
        /// <param name="canSelectedIndex">是否将ComboBox.SelectedIndex赋值，true:ComboBox.SelectedIndex=0；否则不进行赋值操作。</param>
        public static void FullComboBox(ComboBoxExtend cbb, DataTable dt, string displayName,
            string value, bool IsAddChoose, bool canSelectedIndex)
        {
            ComboBox obj = cbb as ComboBox;
            FullComboBox(obj, dt, displayName, value, IsAddChoose, canSelectedIndex);
        }

        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <param name="cbb">ComboBox控件名称</param>
        /// <param name="dt">Datatable</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        public static void FullComboBox(ComboBoxExtend cbb, DataTable dt, string displayName,
            string value)
        {
            FullComboBox(cbb, dt, displayName.Trim(), value.Trim(), true);
        }

        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <param name="cbb">ComboBox控件名称</param>
        /// <param name="dt">Datatable</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        /// <param name="canSelectedIndex">是否将ComboBox.SelectedIndex赋值，true:ComboBox.SelectedIndex=0；否则不进行赋值操作。</param>
        public static void FullComboBox(ComboBoxExtend cbb, DataTable dt, string displayName,
            string value, bool canSelectedIndex)
        {
            FullComboBox(cbb, dt, displayName, value, true, canSelectedIndex);
        }

        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <typeparam name="T">T类对象</typeparam>
        /// <param name="cbb">ComboBoxExtend控件名称</param>
        /// <param name="item">要赋值的对象</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        public static void FullComboBox<T>(ComboBox cbb, List<T> item, string displayName,
            string value, bool IsAddChoose, bool canSelectedIndex) where T : new()
        {
            int counter = 0;
            if (cbb == null)
            {
                return;
            }
            cbb.Items.Clear();
            ComboBoxItem var = new ComboBoxItem();
            if (IsAddChoose)
            {
                var.Content = "全部";
                var.Name = "cbb_" + cbb.Name + "_" + (++counter).ToString("X4");
                var.Tag = item;
                var.Uid = "%";
                cbb.Items.Add(var);
            }
            if (item == null || item.Count == 0)
            {
                cbb.SelectedIndex = 0;
                return;
            }
            foreach (T a in item)
            {
                string content = null;
                string uid = null;
                Type t = a.GetType();
                PropertyInfo[] piList = t.GetProperties();
                foreach (PropertyInfo pi in piList)
                {
                    try
                    {
                        object objValue = pi.GetValue(a, null);
                        if (displayName.ToLower() == pi.Name.ToLower() && null != objValue)
                        {
                            content = objValue.ToString();
                        }
                        if (value.ToLower() == pi.Name.ToLower() && null != objValue)
                        {
                            uid = objValue.ToString();
                        }
                        if (content.JudgeIsNullOrEmpty() == false &&
                            uid.JudgeIsNullOrEmpty() == false)
                        {
                            break;
                        }
                    }
                    catch (Exception ee)
                    {

                    }
                }
                if (content.JudgeIsNullOrEmpty() == false &&
                    uid.JudgeIsNullOrEmpty() == false)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Name = "cbb_" + cbb.Name + "_" + (++counter).ToString("X4");
                    cbi.Content = content;
                    cbi.Uid = uid;
                    cbi.Tag = a;
                    cbb.Items.Add(cbi);
                }
            }
            if (cbb.Items.Count >= 1 && canSelectedIndex)
            {
                cbb.SelectedIndex = 0;
            }

        }

        private void ConsoleWriteLine(Exception ee, LogFlag logFlag)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <typeparam name="T">T类对象</typeparam>
        /// <param name="cbb">ComboBoxExtend控件名称</param>
        /// <param name="item">要赋值的对象</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        public static void FullComboBox<T>(ComboBoxExtend cbbe, List<T> item, string displayName,
            string value, bool canSelectedIndex) where T : new()
        {
            ComboBox obj = cbbe as ComboBox;
            FullComboBox<T>(obj, item, displayName, value, false, canSelectedIndex);
        }
        /// <summary>
        /// 填充ComboBox控件。
        /// </summary>
        /// <typeparam name="T">T类对象</typeparam>
        /// <param name="cbb">ComboBoxExtend控件名称</param>
        /// <param name="item">要赋值的对象</param>
        /// <param name="displayName">ComboBox显示的内容</param>
        /// <param name="value">ComboBox中key值</param>
        public static void FullComboBox<T>(ComboBoxExtend cbbe, List<T> item, string displayName,
            string value) where T : new()
        {
            ComboBox obj = cbbe as ComboBox;
            FullComboBox<T>(obj, item, displayName, value, true, true);
        }

        /// <summary>
        /// 将行里的内容转成字符串
        /// </summary>
        /// <param name="dr">DataRow内容</param>
        /// <param name="columnName">列的名称</param>
        /// <returns>返回DataRow内容</returns>
        private static string ConvertToString(DataRow dr, string columnName)
        {
            try
            {
                return dr.IsNull(columnName) == true ? "" : dr[columnName].ToString();
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.DebugFormat);
                return "";
            }
        }

        /// <summary>
        /// 获取选择ComboBox里UId的值。
        /// </summary>
        /// <param name="cbb">ComboBox控件</param>
        /// <param name="isFirstRecrod">输入出BOOL值</param>
        /// <returns>返回ComboBox里UId的值</returns>
        public static string GetComboBoxUid(ComboBox cbb, out bool isFirstRecrod)
        {
            if (cbb == null)
            {
                isFirstRecrod = false;
                return "";
            }
            if (cbb.SelectedIndex > 0)
            {
                ComboBoxItem item = cbb.SelectedItem as ComboBoxItem;
                isFirstRecrod = true;
                return item.Uid;
            }
            else
            {
                isFirstRecrod = false;
                return "";
            }
        }

        /// <summary>
        /// 获取选择ComboBox里UId的值。
        /// </summary>
        /// <param name="cbb">ComboBox控件</param>
        /// <returns>返回ComboBox里UId的值</returns>
        public static string GetComboBoxUid(ComboBox cbb)
        {
            if (cbb == null)
            {
                return "";
            }
            bool isComplete = false;
            string value = GetComboBoxUid(cbb, out isComplete);
            if (isComplete == false)
            {
                ComboBoxItem item = cbb.SelectedItem as ComboBoxItem;
                value = item == null ? "" : item.Uid;
            }
            return value;
        }

        /// <summary>
        /// 获取选择ComboBox里Content的内容
        /// </summary>
        /// <param name="cbb">ComboBox控件</param>
        /// <returns>返回ComboBox里Content的内容</returns>
        public static string GetComboBoxText(ComboBox cbb)
        {
            if (cbb == null)
            {
                return "";
            }
            ComboBoxItem item = cbb.SelectedItem as ComboBoxItem;
            return item == null ? "" : item.Content.ToString();
        }

        /// <summary>
        /// 获取选择ComboBoxExtend里Content的内容
        /// </summary>
        /// <param name="cbbe">ComboBoxExtend控件</param>
        /// <returns>返回ComboBoxExtend里Content的内容</returns>
        public static string GetComboBoxText(ComboBoxExtend cbbe)
        {
            if (cbbe == null)
            {
                return "";
            }
            ComboBox obj = cbbe as ComboBox;
            return GetComboBoxText(obj);
        }

        /// <summary>
        /// 获取选择ComboBox里UId的值。
        /// </summary>
        /// <param name="cbb">ComboBoxExtend控件</param>
        /// <param name="isFirstRecrod">输入出BOOL值</param>
        /// <returns>返回ComboBoxExtend里UId的值</returns>
        public static string GetComboBoxUid(ComboBoxExtend cbb, out bool isFirstRecrod)
        {
            if (cbb == null)
            {
                isFirstRecrod = false;
                return "";
            }
            ComboBox obj = cbb as ComboBox;
            return GetComboBoxUid(obj, out isFirstRecrod);
        }

        /// <summary>
        /// 获取选择ComboBox里UId的值。
        /// </summary>
        /// <param name="cbb">ComboBox控件</param>
        /// <returns></returns>
        public static string GetComboBoxUid(ComboBoxExtend cbb)
        {
            ComboBox obj = cbb as ComboBox;
            return GetComboBoxUid(obj);
        }
        /// <summary>
        /// 获取控件成员名内容。
        /// </summary>
        /// <typeparam name="TControl">控件类型</typeparam>
        /// <param name="SControl">控件</param>
        /// <param name="MemberName">成员名</param>
        /// <returns>返回MemberName的内容</returns>
        public static object GetControlMemberNameContent<TControl>(TControl SControl, string MemberName) where TControl : Control
        {
            object obj = new object();
            if (SControl == null)
            {
                return null;
            }
            try
            {
                Type t = SControl.GetType();
                MemberInfo[] miList = t.GetMembers();
                foreach (var mi in miList)
                {
                    if (mi.Name.ToLower() == MemberName.ToLower())
                    {
                        if (mi is PropertyInfo)
                        {
                            obj = (mi as PropertyInfo).GetValue(SControl, null);
                            break;
                        }
                        else if (mi is FieldInfo)
                        {
                            obj = (mi as FieldInfo).GetValue(SControl);
                            break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                obj = null;
            }

            return obj;
        }

        /// <summary>
        /// 选择内容。
        /// </summary>
        /// <param name="cbb">ComboBox控件</param>
        /// <param name="uid">ComboBox控件Uid的值</param>
        public static void ComboBoxSelectedItem(ComboBox cbb, string uid)
        {
            if (cbb == null || cbb.Items.Count < 1)
            {
                return;
            }
            ComboBoxItem value = null;
            foreach (ComboBoxItem item in cbb.Items)
            {
                if (item.Uid == uid)
                {
                    value = item;
                    break;
                }
            }
            if (value != null)
            {
                cbb.SelectedItem = value;
            }
        }

        /// <summary>
        /// 选择内容。
        /// </summary>
        /// <param name="cbb">ComboBox控件</param>
        /// <param name="uid">ComboBox控件Uid的值</param>
        public static void ComboBoxSelectedItem(ComboBoxExtend cbb, string uid)
        {
            ComboBox obj = cbb as ComboBox;
            ComboBoxSelectedItem(obj, uid);
        }

        /// <summary>
        /// 获取ComboBox.Tag里T对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cb"></param>
        /// <returns></returns>
        public T GetComboBoxTValue<T>(ComboBox cb) where T : new()
        {
            if (cb == null || cb.Text == "全部")
            {
                return default(T);
            }
            ComboBoxItem ci = cb.SelectedItem as ComboBoxItem;
            if (ci != null || ci.Uid.JudgeIsNullOrEmpty() == false)
            {
                try
                {
                    return (T)ci.Tag;
                }
                catch (Exception ee)
                {
                    ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    return default(T);
                }
            }

            return default(T);
        }

        #region --> CheckBox 控件的。

        /// <summary>
        /// 获取CheckBox控件的值。
        /// </summary>
        /// <param name="cb">CheckBox控件</param>
        /// <returns>返回true-选中，否则没有选中</returns>
        public bool GetCheckBoxIsChecked(CheckBox cb)
        {
            if (cb == null)
            {
                return false;
            }
            else
            {
                return cb.IsChecked.GetValueOrDefault();
            }
        }

        /// <summary>
        /// 获取CheckBoxExtend控件的值。
        /// </summary>
        /// <param name="cb">CheckBoxExtend控件</param>
        /// <returns>返回true-选中，否则没有选中</returns>
        public bool GetCheckBoxIsChecked(CheckBoxExtend cb)
        {
            CheckBox obj = cb as CheckBox;
            return GetCheckBoxIsChecked(obj);
        }

        #endregion --> CheckBox 控件的。

        #region --> 获取object里的内容。
        /// <summary>
        /// 获取Object里的内容。
        /// </summary>
        /// <param name="item">要获取的对象</param>
        /// <param name="members">item的成员名称</param>
        /// <returns>返回object里的condition成员内容</returns>
        public object GetObjectValue(object item, string members)
        {
            if (item == null)
            {
                return null;
            }
            object temp = null;
            try
            {
                Type t = item.GetType();
                MemberInfo[] miList = t.GetMembers();
                foreach (var mi in miList)
                {
                    if (mi.Name == members)
                    {
                        if (mi is PropertyInfo)
                        {
                            PropertyInfo pi = mi as PropertyInfo;
                            try
                            {
                                temp = pi.GetValue(item, null);
                                break;
                            }
                            catch (Exception ee)
                            {
                                Wrapper.Instance.ConsoleWriteLine("public string GetObjectValue(object item,string members) 方法,获取Object里指定内容时出错。", LogFlag.InfoFormat);
                                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                            }
                        }
                        else if (mi is FieldInfo)
                        {
                            FieldInfo fi = mi as FieldInfo;
                            try
                            {
                                temp = fi.GetValue(item);
                                break;
                            }
                            catch (Exception ee)
                            {
                                Wrapper.Instance.ConsoleWriteLine("public string GetObjectValue(object item,string members) 方法,获取Object里指定内容时出错。", LogFlag.InfoFormat);
                                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                            }
                        }
                    }//End if
                }//End foreach
                return temp;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine("public string GetObjectValue(object item,string members) 方法出错。", LogFlag.InfoFormat);
                Wrapper.Instance.ConsoleWriteLine(ee, AFC.WS.UI.Config.LogFlag.ErrorFormat);
                return temp;
            }//End try
        }

        #endregion --> 获取object里的内容。

        #region --> 弹出对话框模式。

        public static MessageBoxResult ShowDialog(string content, MessageBoxIcon icon)
        {
            return ShowDialog(content, true, icon);
        }

        /// <summary>
        /// 弹出对话模框
        /// </summary>
        /// <param name="content">对话里显示的内容</param>
        /// <returns>返回结果是MessageBoxResult类型</returns>
        public static MessageBoxResult ShowDialog(string content)
        {
            return ShowDialog(content, true, MessageBoxIcon.Information);
        }
        public static MessageBoxResult ShowDialog(string content, bool isSuccess)
        {
            return ShowDialog(content, isSuccess, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// 弹出对话框模式。选择Yes或No两种。
        /// </summary>
        /// <param name="content">对话里显示的内容</param>
        /// <param name="isSuccess">是否有YesNo的选项</param>
        /// <returns>返回结果是MessageBoxResult类型</returns>
        public static MessageBoxResult ShowDialog(string content, bool isSuccess, MessageBoxIcon icon)
        {
            if (isSuccess == true)
            {
                return MessageDialog.Show(content, "信息", icon, MessageBoxButtons.Ok);
            }
            else
            {
                return MessageDialog.Show(content, "警告", icon, MessageBoxButtons.YesNo);
            }
        }

        #endregion --> 弹出对话框模式。

        #region --> ItemsControl控件。

        /// <summary>
        /// 给控件添加内容
        /// </summary>
        /// <typeparam name="TControl">继承ItemsControl的控件</typeparam>
        /// <typeparam name="TSource">源</typeparam>
        /// <param name="control">控件名称</param>
        /// <param name="Source">数据源集合</param>
        /// <param name="setUIdFieldName">给TControl控件设置Uid值</param>
        /// <param name="setContentFieldName">给TControl控件设置Content值</param>
        public void SetControlItems<TControl, TSource>(TControl control, List<TSource> Source,
            string setUIdFieldName, string setContentFieldName)
            where TControl : ItemsControl
            where TSource : new()
        {
            if (control == null || Source == null || Source.Count < 1)
            {
                return;
            }
            control.Items.Clear();
            foreach (var a in Source)
            {
                try
                {
                    CheckBox cb = new CheckBox();
                    cb.FontSize = 14;
                    cb.Tag = a;
                    cb.Content = GetPropertyValue<TSource>(a, setContentFieldName);
                    object obj = GetPropertyValue<TSource>(a, setUIdFieldName);
                    cb.Uid = obj == null ? "" : obj.ToString();
                    control.Items.Add(cb);
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
            }
        }

        /// <summary>
        /// 获取属性内容
        /// </summary>
        /// <typeparam name="T">属性所在的类</typeparam>
        /// <param name="source">获取属性的类</param>
        /// <param name="fieldOrPropertyName">source类的成员名称</param>
        /// <returns>返回source类的成员内容</returns>
        object GetPropertyValue<T>(T source, string memberName) where T : new()
        {
            if (source == null)
            {
                return null;
            }
            Type t = source.GetType();
            MemberInfo[] miList = t.GetMembers();
            foreach (var mi in miList)
            {
                if (mi.Name.ToLower() == memberName.ToLower())
                {
                    try
                    {
                        if (mi is PropertyInfo)
                        {
                            return (mi as PropertyInfo).GetValue(source, null);
                        }
                        else if (mi is FieldInfo)
                        {
                            return (mi as FieldInfo).GetValue(source);
                        }
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 设置Items控件里的TSource.IsChecked 是否选中。
        /// </summary>
        /// <typeparam name="TControl">ItemsControl控件</typeparam>
        /// <typeparam name="TSource">ToggleButton控件</typeparam>
        /// <param name="control">控件名称</param>
        /// <param name="IsChecked">true-全选；false-反选；</param>
        public void SetControlItemsIsChecked<TControl, TSource>(TControl control, bool IsChecked)
            where TControl : ItemsControl
            where TSource : ToggleButton
        {
            if (control == null)
            {
                return;
            }
            foreach (var v in control.Items)
            {
                TSource t = v as TSource;
                if (t != null)
                {
                    t.IsChecked = IsChecked;
                }
            }
        }

        #endregion --> ItemsControl控件。

        /// <summary>
        /// 设置DataGrid的列的Header名称
        /// </summary>
        /// <param name="dg">DataGrid控件</param>
        /// <param name="columnsName">列名称集合</param>
        public void SetDataGridColumnHeader(DataGrid dg, List<string> columnsName)
        {
            if (columnsName == null || columnsName.Count == 0)
            {
                return;
            }
            dg.Columns.Clear();
            for (int i = 0; i < columnsName.Count; i++)
            {
                DataGridTextColumn c = new DataGridTextColumn();
                c.Header = columnsName[i];
                dg.Columns.Add(c);
            }
            dg.ItemsSource = null;
        }

        /// <summary>
        /// 向控制台打印日志。
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="flag">日誌類型</param>
        public void ConsoleWriteLine(object value, LogFlag flag)
        {
            string currentTime = DateTime.Now.ToShortDateString()
                + " " + DateTime.Now.ToLongTimeString()
                + " " + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
            if (SysConfig.GetSysConfig().LocalParamsConfig.IsOutPutConsole)
            {
                Console.WriteLine("{0} --> {1}", currentTime, value);
            }

            if (value == null)
            {
                return;
            }
            try
            {
                switch (flag)
                {
                    case LogFlag.Error:

                        WriteLog.Log_Error(value as Exception);

                        break;

                    case LogFlag.ErrorFormat:

                        WriteLog.Log_ErrorFormat("Error", "0", value.ToString());

                        break;

                    case LogFlag.Info:

                        WriteLog.Log_Info(value.ToString());

                        break;

                    case LogFlag.InfoFormat:

                        WriteLog.Log_InfoFormat("Info", "0", value.ToString());

                        break;

                    case LogFlag.Debug:

                        WriteLog.Log_Debug(value.ToString());

                        break;

                    case LogFlag.DebugFormat:

                        WriteLog.Log_DebugFormat("Debug", "0", value.ToString());

                        break;

                    case LogFlag.Fatal:

                        WriteLog.Log_Fatal(value.ToString());

                        break;

                    case LogFlag.FatalFormat:

                        WriteLog.Log_FatalFormat("Fatal", "0", value.ToString());

                        break;

                    case LogFlag.Warn:

                        WriteLog.Log_Warn(value.ToString());

                        break;

                    case LogFlag.WarnFormat:

                        WriteLog.Log_WarnFormat("Warn", "0", value.ToString());

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("{0} --> {1}", currentTime, ee);
            }
        }

        #region --> 初始化控件信息。

        /// <summary>
        /// 初始化控件信息。
        /// </summary>
        /// <typeparam name="TInitControl">要初始化的控件</typeparam>
        /// <typeparam name="TParentControl">初始化控件的所在的父控件</typeparam>
        /// <param name="ParentControl">父控件名称</param>
        public void InitControlValue<TInitControl, TParentControl>(TParentControl ParentControl)
            where TInitControl : TextBoxExtend
            where TParentControl : ContentControl
        {
            InitControlValue<TInitControl, TParentControl>(ParentControl, "");
        }

        /// <summary>
        /// 初始化控件信息。
        /// </summary>
        /// <typeparam name="TInitControl">要初始化的控件</typeparam>
        /// <typeparam name="TParentControl">初始化控件的所在的父控件</typeparam>
        /// <param name="ParentControl">父控件名称</param>
        /// <param name="InitValue">初始值内容</param>
        public void InitControlValue<TInitControl, TParentControl>(TParentControl ParentControl, object InitValue)
            where TInitControl : TextBoxExtend
            where TParentControl : ContentControl
        {
            if (ParentControl == null)
            {
                return;
            }
            if (InitValue == null)
            {
                InitValue = "";
            }
            Grid g = ParentControl.Content as Grid;
            if (g != null)
            {
                InitGridControlValue<TInitControl>(g, InitValue);
            }
            TextBox tb = new TextBox();
        }

        /// <summary>
        /// 初始化控件信息
        /// </summary>
        /// <typeparam name="TInitControl">要初始化的控件</typeparam>
        /// <param name="g">Grid控件</param>
        /// <param name="InitValue">初始值内容</param>
        void InitGridControlValue<TInitControl>(Grid g, object InitValue)
            where TInitControl : TextBoxExtend
        {
            foreach (var v in g.Children)
            {
                if (v is Grid)
                {
                    Grid tempG = v as Grid;
                    InitGridControlValue<TInitControl>(tempG, InitValue);
                }
                if (v is TInitControl)
                {
                    TInitControl tb = (TInitControl)v;
                    tb.Text = InitValue.ToString();
                }
            }
        }

        public void AddQueryConditionToList(List<QueryCondition> list,string bindingData, object value)
        {
            try
            {
                if (string.IsNullOrEmpty(bindingData))
                    return;
                QueryCondition qc = list.Single(temp => temp.bindingData.Equals(bindingData));
                qc.value = value;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                QueryCondition qc1 = new QueryCondition();
                qc1.bindingData = bindingData;
                qc1.value = value;
                list.Add(qc1);
                return;
            }
        }


        #region --> 针对DateTimePickerExtend 控件。
        /// <summary>
        /// 给日期控件赋值。
        /// </summary>
        /// <param name="dtpe">日期控件</param>
        /// <param name="type">格式类型</param>
        /// <param name="number">值</param>
        public static void SetDateTimePickerExtend(DateTimePickerExtend dtpe, DateTimeType type, double number)
        {
            try
            {
                if (dtpe == null)
                {
                    return;
                }
                DateTime dt = DateTime.Now;
                switch (type)
                {
                    case DateTimeType.Minutes:
                        dt = DateTime.Now.AddMinutes(number);
                        break;
                    case DateTimeType.Hours:
                        dt = DateTime.Now.AddHours(number);
                        break;
                    case DateTimeType.Day:
                        dt = DateTime.Now.AddDays(number);
                        break;
                    case DateTimeType.Months:
                        dt = DateTime.Now.AddMonths(Convert.ToInt32(number));
                        break;
                    case DateTimeType.Year:
                        dt = DateTime.Now.AddYears(Convert.ToInt32(number));
                        break;
                    default:
                        break;
                }
                Grid g = dtpe.Content as Grid;
                if (g != null)
                {
                    foreach (var a in g.Children)
                    {
                        if (a is Microsoft.Windows.Controls.DatePicker)
                        {
                            Microsoft.Windows.Controls.DatePicker dp = a as Microsoft.Windows.Controls.DatePicker;
                            dp.SelectedDate = dt;
                            dp.DisplayDate = dt;
                            break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 获取DateTimePickerExtend里的时间。
        /// </summary>
        /// <param name="dtpe">日期控件</param>
        /// <returns>返回DateTimePickerExtend里的时间</returns>
        public static DateTime GetDateTimePickerValue(DateTimePickerExtend dtpe)
        {
            if (dtpe != null)
            {
                Grid g = dtpe.Content as Grid;
                if (g != null)
                {
                    foreach (var a in g.Children)
                    {
                        Microsoft.Windows.Controls.DatePicker dp = a as Microsoft.Windows.Controls.DatePicker;
                        return dp == null ? DateTime.Now : (dp.SelectedDate == null ? dp.DisplayDate : dp.SelectedDate.Value);
                    }
                }
            }
            return DateTime.Now;
        }

        #endregion --> 针对DateTimePickerExtend 控件。



        /// <summary>
        /// 获取RFID读取成功的信息。
        /// </summary>
        /// <returns></returns>
        public string GetRfidSuccessMessageInfo()
        {
            return "成功读取RFID信息...";
        }


        #endregion --> 初始化控件信息。


        #region --> 关闭进程。

        /// <summary>
        /// 关闭进程操作。
        /// </summary>
        /// <param name="processName">进程名称</param>
        public void CloseProcessByProcessName(string processName)
        {
            if (processName.JudgeIsNullOrEmpty())
            {
                return;
            }

            System.Diagnostics.Process[] Processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (Processes == null || Processes.Length == 0)
            {
                return;
            }
            for (int i = 0; i < Processes.Length; i++)
            {
                try
                {
                    Processes[i].Kill();
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
            }
        }

        #endregion --> 关闭进程。
    }
    public enum DateTimeType
    {
        /// <summary>
        /// 分钟
        /// </summary>
        Minutes = 0,
        /// <summary>
        /// 小时
        /// </summary>
        Hours = 1,
        /// <summary>
        /// 天
        /// </summary>
        Day = 11,
        /// <summary>
        /// 月
        /// </summary>
        Months = 22,
        /// <summary>
        /// 年
        /// </summary>
        Year = 33,
    }

       
}