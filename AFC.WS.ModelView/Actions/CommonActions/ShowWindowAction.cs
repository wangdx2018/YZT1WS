using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AFC.WS.ModelView.Actions.CommonActions
{
    using AFC.WS.UI.Common;
    using AFC.BOM2.UIController;
    using AFC.WS.UI.CommonControls;


    /// 作者：王冬欣 
    /// 日期：20110221
    /// 代码功能：通用的显示界面的Action。
    /// 通过配置UserBaseControl的反射类名来创建界面,action的参数在界面的Tag属性中取值。
    /// 该控件的类型必须是从UserControlBase中集成得到
    /// 修订记录：20110328  wangdx 增加了Closing的处理，为了调用到UnLoadControl，如果为列表
    ///                    可以释放数据源。
    public class ShowWindowAction:IAction
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        private string controlType;

        [Filter()]
        /// <summary>
        /// 控件类型（类型名称）
        /// </summary>
        public string ControlType
        {
            set { this.controlType = value; }
            get { return this.controlType; }
        }

        /// <summary>
        /// 窗体的Width
        /// </summary>
        [Filter()]
        public double Width
        {
            set;
            get;
        }

        /// <summary>
        /// 窗体的Height
        /// </summary>
        [Filter()]
        public double Height
        {
            set;
            get;
        }

        /// <summary>
        /// 窗体的标题，可以在运行时配置
        /// </summary>
        [Filter()]
        public string Title
        {
            set;
            get;
        }

        [Filter()]
        public bool IsCheckNULL
        {
            set;
            get;
        }

        /// <summary>
        /// 如果为选择的文本提示
        /// </summary>
        [Filter()]
        public string NULLCheckTip
        {
            set;
            get;
        }


        public UserControlBase ucb = null;

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (ucb != null)
                return true;
            
              bool res=!string.IsNullOrEmpty(ControlType);
              if (!res)
                  return res;
              if (IsCheckNULL)
              {

                  if (actionParamsList == null || actionParamsList.Count == 0 )
                  {
                      if (string.IsNullOrEmpty(NULLCheckTip))
                      {
                          MessageDialog.Show("请选择操作对象", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                      }
                      else
                      {
                          MessageDialog.Show(NULLCheckTip, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                      }
                      return false;
                  }
                  return res;

              }
              return res;
              
            
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            if (ucb == null)
            {
                ucb = Activator.CreateInstance(Type.GetType(ControlType)) as UserControlBase;
                if (ucb == null)
                {
                    WriteLog.Log_Error("Create type=[" + ControlType + "] error!");
                    return null;
                }
            }

            BaseWindow bw = new BaseWindow();
            bw.Closing += new System.ComponentModel.CancelEventHandler(bw_Closing);
            UserControl uc = ucb as UserControl;
            actionParamsList.Add(new QueryCondition() { bindingData = "window", value = bw });//window为窗体对象，如果需要设置Title属性，在该处即可。
            uc.Tag = actionParamsList;
            ucb.InitControls(); //todo 获取数据从tag上得到

            bw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            bw.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            bw.ResizeMode = System.Windows.ResizeMode.NoResize;
            bw.WindowState = System.Windows.WindowState.Normal;

            if (!string.IsNullOrEmpty(Title)&&
                string.IsNullOrEmpty(bw.Title))//没有外部的东西设置Title
            {
                bw.Title = Title;
            }
            if (this.Width != 0)
            {
                bw.Width = this.Width;
            }
            if (this.Height != 0)
            {
                bw.Height = this.Height;
            }
            
            bw.Content = ucb;
            bw.ShowDialog();
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        private void bw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ucb != null)
            {
                try
                {
                    BaseWindow bw = sender as BaseWindow;
                    bw.SetValue(BaseWindow.ContentProperty, null);
                    try
                    {
                        ucb.UnLoadControls();
                    }
                    catch (Exception ex1)
                    {
                        WriteLog.Log_Error(ex1.Message);
                    }
                  
                    ucb = null;
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                }
            }
            //throw new NotImplementedException();
        }

        #endregion
    }
}
