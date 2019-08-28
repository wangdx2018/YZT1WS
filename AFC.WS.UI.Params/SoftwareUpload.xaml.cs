using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.UI.CommonControls;
    //using System.Windows.Forms;
   

    /// <summary>
    /// SoftwareUpload.xaml 的交互逻辑
    /// </summary>
    public partial class SoftwareUpload : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public SoftwareUpload()
        {
            InitializeComponent();
            this.btnBrower.Click += new RoutedEventHandler(btnBrower_Click);
            this.btnSoftwareUnload.Click += new RoutedEventHandler(btnSoftwareUnload_Click);
            this.cmbDevSoftware.SelectionChanged += new SelectionChangedEventHandler(cmbDevSoftware_SelectionChanged);
          
        }

     

        private void AddQueryConditionToList(string bindingData,object value)
        {
            try
            {
                if (string.IsNullOrEmpty(bindingData))
                    return ;
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



        private void cmbDevSoftware_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            try
            {
                ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
                string value = item.Tag.ToString();

                AddQueryConditionToList("softwareType", value);
                this.labTip.Content = string.Empty;
                
                if (value == "0022")
                {
                    this.labVersionNo.Visibility = Visibility.Visible;
                    this.txtParamVersionNo.Visibility = Visibility.Visible;
                }
                else
                {
                    this.txtParamVersionNo.Text = string.Empty;
                    this.labVersionNo.Visibility = Visibility.Hidden;
                    this.txtParamVersionNo.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void RemoveVersionNo()
        {
            QueryCondition qc=null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].bindingData.Equals("versionNo"))
                {
                    qc = list[i];
                    break;
                }
            }
            if (qc != null)
                list.Remove(qc);
        }



        private void btnSoftwareUnload_Click(object sender, RoutedEventArgs e)
        {
            RemoveVersionNo();
           
            if (string.IsNullOrEmpty(this.txtParamVersionNo.Text) &&
                this.txtParamVersionNo.Visibility == Visibility.Visible)
            {
                MessageBoxResult mbr =
                    MessageDialog.Show("如果不指定软件版本号，版本号将依次累加，是否继续上传软件？", "提示",
                     MessageBoxIcon.Question, MessageBoxButtons.YesNo);
                if (mbr != MessageBoxResult.Yes)
                {
                    return;
                }
               

            }
            else
            {
                AddQueryConditionToList("versionNo", this.txtParamVersionNo.Text.ConvertNumberStringToUint());
            }
            AddQueryConditionToList("filePath", this.txtFilePath.Text);
            IAction action = new AFC.WS.ModelView.Actions.ParamActions.SoftWareUpLoadAction();
            if (action.CheckValid(list))
            {
              ResultStatus res= action.DoAction(list);
              if (res!=null&&res.resultCode== 0 &&
                  res.resultData.ToString() == "0")
              {
                  this.labTip.Content = "软件已经上传成功，请到参数发布界面进行发布!";
              }
              else
              {
                  this.labTip.Content = "软件上传失败";
              }
            }
        }

        private void btnBrower_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
       
            this.txtFilePath.Text = ofd.FileName;
          
        }

        public override void InitControls()
        {
            this.labTip.Content = string.Empty;
            string date = DateTime.Now.ToString("yyyy年MM月dd日");
            //this.date.SetControlValue(date);
            //this.timePicker.SelectedTime = DateTime.Now.TimeOfDay;
            this.txtFilePath.IsEnabled = false;
            this.txtFilePath.Text = string.Empty;
            //this.txtSoftwareNo.Text = string.Empty;
            this.cmbDevSoftware.SelectedIndex = 0;
            this.list.Clear();

    
     
        }

        public override void UnLoadControls()
        {
            this.labTip.Content = string.Empty;
            this.list.Clear();
        }

       
    }
}
