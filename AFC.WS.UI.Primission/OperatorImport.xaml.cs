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
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.BR.Primission;
using AFC.BOM2.UIController;
using AFC.WS.UI.FC.CommonControls;
using System.Threading;
using System.Text.RegularExpressions;

namespace AFC.WS.UI.Primission
{
    /// <summary>
    /// OperatorImport.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorImport : UserControlBase
    {
        public int operatorIdCell = 1;
        public int companyCell = 2;
        public int operatorNameCell = 3;
        public int contact1Cell = 4;
        public int contact2Cell = 5;
        public int addressCell = 6;
        public int emailCell = 7;

        string fileName = string.Empty;


        public OperatorImport()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "xls文件|*.xls|xlsx文件|*.xlsx";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.txtFileName.Text = openFileDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.ApplicationClass xApp = null;
            Workbook compareBook = null;
            Worksheet compSheet = null;
            fileName = this.txtFileName.Text;
            if (string.IsNullOrEmpty(this.txtFileName.Text))
            {
                System.Windows.MessageBox.Show("请选择文件路径！");
                return;
            }
            this.label5.Content = "正在导入，请稍候......";
            this.Cursor = System.Windows.Input.Cursors.AppStarting;
            Thread thread = new Thread(new ThreadStart(importOperator));//开辟一个新的线程
            thread.Start();
            do
            {
                if (thread.ThreadState != ThreadState.Stopped)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                else

                    break;
            }
            while (true);

            this.label5.Content = "导入完成";
            this.Cursor = System.Windows.Input.Cursors.Arrow;
                
        }

        private void importOperator()
        {
            ApplicationClass xApp = null;
            Workbook compareBook = null;
            Worksheet compSheet = null;
            try
            {
                OperatorManager manager = new OperatorManager();
                xApp = new ApplicationClass();
                compareBook = xApp.Workbooks._Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                compSheet = (Worksheet)compareBook.Sheets[1];
                int compRows = compSheet.UsedRange.Rows.Count;
                int columns = compSheet.UsedRange.Columns.Count;
                //特殊字符
                string except_chars = ": ‘ '）$";
                for (int i = 0; i < compRows - 1; i++)
                {
                    PrivOperatorInfo info = new PrivOperatorInfo();
                    info.operator_id = (compSheet.Cells[i + 2, operatorIdCell] as Range).Value2 == null ? "" : (compSheet.Cells[i + 2, operatorIdCell] as Range).Value2.ToString().Trim();
                    info.company_id = (compSheet.Cells[i + 2, companyCell] as Range).Value2 == null ? "" : (compSheet.Cells[i + 2, companyCell] as Range).Value2.ToString().Trim();
                    info.operator_name = (compSheet.Cells[i + 2, operatorNameCell] as Range).Value2 == null ? "" : Regex.Replace((compSheet.Cells[i + 2, operatorNameCell] as Range).Value2.ToString().Trim(), "[" + Regex.Escape(except_chars) + "]", "");
                    info.contact_info1 = (compSheet.Cells[i + 2, contact1Cell] as Range).Value2 == null ? "" : Regex.Replace((compSheet.Cells[i + 2, contact1Cell] as Range).Value2.ToString().Trim(), "[" + Regex.Escape(except_chars) + "]", "");
                    info.contact_info2 = (compSheet.Cells[i + 2, contact2Cell] as Range).Value2 == null ? "" : Regex.Replace((compSheet.Cells[i + 2, contact2Cell] as Range).Value2.ToString().Trim(),"[" + Regex.Escape(except_chars) + "]", "");
                    info.contact_address = (compSheet.Cells[i + 2, addressCell] as Range).Value2 == null ? "" : Regex.Replace((compSheet.Cells[i + 2, addressCell] as Range).Value2.ToString().Trim(),"[" + Regex.Escape(except_chars) + "]", "");
                    info.email_address = (compSheet.Cells[i + 2, emailCell] as Range).Value2 == null ? "" : Regex.Replace((compSheet.Cells[i + 2, emailCell] as Range).Value2.ToString().Trim(),"[" + Regex.Escape(except_chars) + "]", "");
                    info.password_his1 = string.Empty;
                    info.password_his2 = string.Empty;
                    info.is_multi_login = "01";
                    //info.lock_status = "00";
                    //info.login_status = "01";
                    info.password = "000000";
                    info.password_new = "000000";
                    info.operator_display_id = string.Empty;
                    info.pwd_set_mode = "01";
                    info.pwd_invalidity_date = DateTime.Now.AddYears(2).ToString("yyyyMMdd");
                    info.update_date = DateTime.Now.ToString("yyyyMMdd");
                    info.update_time = DateTime.Now.ToString("HHmmss");
                    info.upd_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                    info.validity_date_end = DateTime.Now.AddYears(2).ToString("yyyyMMdd");
                    info.validity_date_first_login = DateTime.Now.AddMonths(6).ToString("yyyyMMdd");
                    info.validity_date_start = DateTime.Now.ToString("yyyyMMdd");
                    info.operator_status = "05";

                    int res = manager.AddNewOperator(info);

                }
            }

            catch (Exception ex)
            {

            }
            finally
            {
                compSheet = null;
                compareBook = null;
                xApp.Quit(); //这一句是非常重要的，否则Excel对象不能从内存中退出 
                xApp = null;
            }
        }
    }
}
