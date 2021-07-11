using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Bargh_Common;
using Janus.Windows.GridEX;

namespace Bargh_GIS
{
    partial class frmTraceHistory
    {
        private string fromDate = "" , toDate = "" , fromTime = "" , toTime = ""
            , masterIDs = "" , areaIDs = "" , tabletIDs = "";
        private int areaId;
        private void getFilters() {
            fromDate = txtFromDate.Text == "____/__/__" ? "" : txtFromDate.Text;
            toDate = txtFromDate.Text == "____/__/__" ? "" : txtFromDate.Text;
            fromTime = txtFromDate.Text == "__:__" ? "" : txtFromDate.Text;
            toTime = txtFromDate.Text == "__:__" ? "" : txtFromDate.Text;
            areaId = cboArea.SelectedIndex > 0 ? Convert.ToInt32(cboArea.SelectedValue) : 0;
        }
        private string prepareSQLString(string SpName , bool isRequset = false) {
            string sql = "EXEC " + SpName
                + " '" + fromDate
                + "' ,'" + toDate
                + "' ,'" + fromTime
                + "' ,'" + toTime
                + "' ," + areaId;
            if (isRequset)
                sql += ",'" + masterIDs
                    + "' ,'" + tabletIDs
                    + "' ,'" + areaIDs + "'";
            return sql;
        }
        private async Task GetMasterData()
        {
            string lSQL = prepareSQLString("Homa.spTraceMaster");
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Master"
                , ref FakeComboBox, aIsClearTable: true);
            dgMaster.DataSource = mDS.Tables["Tbl_Master"];
        }
        private async Task GetAreaData()
        {
            string lSQL = prepareSQLString("Homa.spTraceArea");
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Area"
                , ref FakeComboBox, aIsClearTable: true);
            dgArea.DataSource = mDS.Tables["Tbl_Area"];
        }
        private async Task GetTabletData()
        {
            string lSQL = prepareSQLString("Homa.spTraceTablet");
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Tablet"
                , ref FakeComboBox, aIsClearTable: true);
            dgTablet.DataSource = mDS.Tables["Tbl_Tablet"];
        }
        private void GetFirstRowData() {
            List<Task> tasks = new List<Task>();
            tasks.Add(GetMasterData());
            tasks.Add(GetAreaData());
            tasks.Add(GetTabletData());
            Task.WhenAll(tasks);
        }
        private void SetRequestFilters()
        {
            SetRequestFilterIDs(mDS.Tables["Tbl_Tablet"] ,ref tabletIDs , "TabletId");
            SetRequestFilterIDs(mDS.Tables["Tbl_Master"], ref masterIDs , "MasterId");
            SetRequestFilterIDs(mDS.Tables["Tbl_Area"], ref areaIDs , "AreaId");
        }
        private void SetRequestFilterIDs(DataTable dt, ref string idObject , string id) {
            idObject = "";
            foreach (DataRow row in dt.Rows)
                if ((bool)row["IsChecked"])
                    idObject += "," + row[id];
            idObject = idObject.Length > 0 ? idObject.Substring(1) : "";
        }
        private void GetRequestData() {
            SetRequestFilters();
            string lSQL = prepareSQLString("Homa.spTraceRequest" , true);
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Request"
                , ref FakeComboBox, aIsClearTable: true);
            dgRequest.DataSource = mDS.Tables["Tbl_Request"];
        }
    }
}
