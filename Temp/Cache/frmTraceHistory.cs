using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Bargh_Common;
using Janus.Windows.GridEX;
using Janus.Windows.Common;
using Bargh_GIS.TazarvEvent;

namespace Bargh_GIS
{
    public partial class frmTraceHistory : FormBase
    {
        private Classes.CDatabase db;
        private DataSet mDS;
        private long mRequestId;
        private SqlConnection mCnn = null;
        ComboBox FakeComboBox = new ComboBox();
        CallWpfFuctions fn;
        wpf.TazarvMapUC_Cars uCars;
        public frmTraceHistory(long aRequestId)
        {
            InitializeComponent();
            this.mRequestId = aRequestId;
            initialData();
            handleUI03();
        }
       
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetFirstRowData();
            handleUI01();
        }
        private void btnRequests_Click(object sender, EventArgs e)
        {
            GetRequestData();
            handleUI02();
        }
        private void btnCloseFilter_Click(object sender, EventArgs e)
        {
            handleUI03();
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            //---------Gather and show data:
            string lSQL = "EXECUTE spOmidTest";
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Trace_Info"
                , ref FakeComboBox, aIsClearTable : true);
            uCars.ShowTrace(mDS.Tables["Trace_Info"]);
            //----------update UI:
            pnlSearchExpand.Expanded = false;
        }

        private void pnlSearchExpand_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if(pnlSearchExpand.Expanded == true)
                pnlSearchExpand.Size = GetSize(-1, true);
        }
    }
}
