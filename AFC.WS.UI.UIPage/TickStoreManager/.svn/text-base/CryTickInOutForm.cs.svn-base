using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.ObjectModel;

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    public partial class CryTickInOutForm : Form
    {
        private string paraValues;
        string[] paraKey;
        ObservableCollection<TickOperationData> list;
        public CryTickInOutForm(string paraValues, ObservableCollection<TickOperationData> list)
        {
            InitializeComponent();
            this.paraValues = paraValues;
            this.list = list;
        }

        private void CryTickInOutForm_Load(object sender, EventArgs e)
        {
            paraKey = new string[6] { "ReportTitle11", "RequestBatchNo", "OperatorID", "DispatchLocationID", "RequestLocationID", "DispatchType"};
            CrystalTickInOutReport crystalTickInOutReport = new CrystalTickInOutReport();
            crystalTickInOutReport.SetDataSource(getTickStorageTable());
            setParameters(paraValues, crystalTickInOutReport);
            this.crystalReportViewer1.ReportSource = crystalTickInOutReport;
        }
        private DataTable getTickStorageTable()
        {
            DataTable dt = new DataTable();
            //增加列

            dt.Columns.Add(new DataColumn("StorageManagerType", typeof(string)));
            dt.Columns.Add(new DataColumn("TicketStateName", typeof(string)));
            dt.Columns.Add(new DataColumn("TicketBoxNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("DispatchWay", typeof(string)));

            //增加行
      
            for (int i = 0; i < list.Count; i++)
            {
                DataRow newCustomersRow = dt.NewRow();
                newCustomersRow["StorageManagerType"] = list[i].TickStoreTypeName.ToString();
                newCustomersRow["TicketStateName"] = list[i].TickStoreType.ToString();
                newCustomersRow["TicketBoxNumber"] = list[i].TickNum.ToString();
                newCustomersRow["DispatchWay"] = "";
                dt.Rows.Add(newCustomersRow);
            }


            return dt;

        }

        private void setParameters(string paras, CrystalTickInOutReport report)
        {
            string[] paraArray = paras.Split(',');

            for (int i = 0; i < paraArray.Length; i++)
            {
                string key = paraKey[i];
                string value = paraArray[i];
                report.SetParameterValue(key, value);
            }
        }
    }
}
