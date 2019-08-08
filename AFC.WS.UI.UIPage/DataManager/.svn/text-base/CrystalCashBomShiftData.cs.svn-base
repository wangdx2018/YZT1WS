using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms;
using System.Data;
using CrystalDecisions.Shared;

namespace AFC.WS.UI.UIPage.DataManager
{
    public class CrystalCashBomShiftData
    {
        private ReportClass rptInstance = null;

        public void SetRptFileInstance(ReportClass instance)
        {
            if (instance == null)
            {
                WriteLog.Log_Error("params error instance==null");
                return;
            }
            this.rptInstance=instance;
        }

        private int SetParamsData(Dictionary<string, string> dict)
        {
            if(dict==null||
                dict.Count<=0)
            {
                WriteLog.Log_Error("params dict is null or empty");
                return -1;
            }
            List<string> paramsNameCollection=new List<string>();

            for(int i=0;i<this.rptInstance.ParameterFields.Count;i++)
            {
               paramsNameCollection.Add(this.rptInstance.ParameterFields[i].Name);
            }
            foreach(var temp in dict)
            {
                if (paramsNameCollection.Contains(temp.Key))
                {
                    this.rptInstance.SetParameterValue(temp.Key, temp.Value);
                }
                else
                {
                    WriteLog.Log_Error("that key not config in that template[" + temp.Key + "]");
                    continue;
                    //todo:log hrere
                }
            }
            return 0;
        }

        public int SetDataTableData(System.Data.DataTable dt, string tableName)
        {
            if (string.IsNullOrEmpty(tableName) ||
                dt == null ||
                dt.Rows.Count <= 0)
            {
                WriteLog.Log_Error("params error");
                return -1;
            }

         //   this.rptInstance.DataDefinition.ParameterFields
            this.rptInstance.SetDataSource(dt);
            return 0;
        }

        public Form myForm = new Form();

        public void ShowRptDialog(ReportClass instance,Dictionary<string,string> dict,DataTable dt)
        {
            try
            {

                this.SetRptFileInstance(instance);

                int res = this.SetDataTableData(dt, "dataTable");
                //if (res != 0)
                //    return;

                res = this.SetParamsData(dict);
                //if (res != 0)
                //    return;

             
                myForm.Text = this.rptInstance.Name;

                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new ReportDocument();
                this.rptInstance.ExportToDisk(ExportFormatType.PortableDocFormat, DateTime.Now.ToString("yyyyyMMddHHmmss") + "_temp.pdf");





                CrystalReportViewer crViewer = new CrystalReportViewer();
                myForm.Controls.Add(crViewer);
                crViewer.Dock = DockStyle.Fill;

                crViewer.ReportSource = this.rptInstance;
                myForm.ShowDialog();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }
        }
    }
}
