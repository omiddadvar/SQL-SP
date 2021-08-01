using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Bargh_Common;

namespace Bargh_GIS
{
    partial class frmTraceHistory
    {
        private string fromDate = "" , toDate = "" , fromTime = "" , toTime = ""
            , masterIDs = "" , areaIDs = "" , tabletIDs = "" , errorString;
        private DateTime mDTFrom , mDTTo;
        private int areaId;
        private bool getFilters() {
            errorString = "";
            fromDate = txtFromDate.IsOK ? txtFromDate.Text : "";
            toDate = txtToDate.IsOK ? txtToDate.Text : "" ;
            fromTime  = txtFromTime.Text = txtFromTime.IsOK ? txtFromTime.Text : "00:00";
            toTime = txtToTime.Text = txtToTime.IsOK ? txtToTime.Text : "23:59" ;
            areaId = cboArea.SelectedIndex > 0 ? Convert.ToInt32(cboArea.SelectedValue) : 0;

            if (!txtFromDate.IsOK) errorString += "• ورود تاريخ شروع اجباري است." + Environment.NewLine;
            if (!txtToDate.IsOK) errorString += "• ورود تاريخ پايان اجباري است." + Environment.NewLine;

            if (errorString.Length > 0) return false;

            txtFromDate.InsertTime(txtFromTime);
            txtToDate.InsertTime(txtToTime);
            mDTFrom = (DateTime) txtFromDate.MiladiDT;
            mDTTo = (DateTime)txtToDate.MiladiDT;

            return true;
        }
        private string prepareSQLString(string SpName , bool isRequset = false , bool isOnCall = false) {
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
        private bool GetFirstRowData() {
            if (!getFilters())
            {
                CommonFunctions.ShowInfo(errorString);
                return false;
            }
            List<Task> tasks = new List<Task>();
            tasks.Add(GetMasterData());
            tasks.Add(GetAreaData());
            tasks.Add(GetTabletData());
            Task.WhenAll(tasks);
            return true;
        }
        private bool GetRequestData()
        {
            if (!getFilters())
            {
                CommonFunctions.ShowInfo(errorString);
                return false;
            }
            SetRequestFilters();
            string lSQL = prepareSQLString("Homa.spTraceRequest", true);
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Request"
                , ref FakeComboBox, aIsClearTable: true);
            dgRequest.DataSource = mDS.Tables["Tbl_Request"];
            return true;
        }
        private bool checkData(params string[] aTableNames)
        {
            bool state = false;
            foreach (string lNname in aTableNames)
                state |= mDS.Tables.Contains(lNname) && mDS.Tables[lNname].Rows.Count > 0;
            if (state) return true;
            string msg = "موردی یافت نشد.";
            CommonFunctions.ShowInfo(msg);
            return false;
        }
        private void SetRequestFilters()
        {
            SetRequestFilterIDs("Tbl_Tablet", ref tabletIDs, "TabletId");
            SetRequestFilterIDs("Tbl_Master", ref masterIDs, "MasterId");
            SetRequestFilterIDs("Tbl_Area", ref areaIDs, "AreaId");
        }
        private void SetRequestFilterIDs(string tableName, ref string idObject , string id) {
            idObject = "";
            foreach (DataRow row in mDS.Tables[tableName].Rows)
                if (Convert.ToBoolean(row["IsChecked"]))
                    idObject += "," + row[id];
            idObject = idObject.Length > 0 ? idObject.Substring(1) : "";
        }
        private bool showAllTrace() {
            SetRequestFilters();
            string lSQL = prepareSQLString("Homa.spTraceOnCall", true);
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_OnCall"
                , ref FakeComboBox, aIsClearTable: true);
            if (!mDS.Tables.Contains("Tbl_OnCall") || mDS.Tables["Tbl_OnCall"].Rows.Count == 0) return false;
            foreach (DataRow row in mDS.Tables["Tbl_OnCall"].Rows)
                showTrace(Convert.ToInt64(row["OnCallId"]));
            return true;
        }
        private void showTrace(long onCallId) {
            string lSQL = "EXEC Homa.spTraceInfo '" + fromDate
               + "' ,'" + toDate
               + "' ,'" + fromTime
               + "' ,'" + toTime
               + "'," + onCallId;
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_Trace"
                , ref FakeComboBox, aIsClearTable: true);
            if (!mDS.Tables.Contains("Tbl_Trace") || mDS.Tables["Tbl_Trace"].Rows.Count == 0) return;
                uCars.ShowTrace(mDS.Tables["Tbl_Trace"] , onCallId);
        }
        private void showAllFlags() {
            bool dd = false;
            if (!mDS.Tables.Contains("Tbl_Request") || mDS.Tables["Tbl_Request"].Rows.Count == 0) return;
            foreach (DataRow row in mDS.Tables["Tbl_Request"].Rows) {
            if(Convert.ToBoolean(row["IsChecked"]))
                showStateFlag(Convert.ToInt64(row["RequestId"]), Convert.ToInt64(row["OnCallId"]));
            }
        }
        private void showStateFlag(long requestId ,long onCallId)
        {
            string lSQL = "EXEC Homa.spGetStateTrace " + requestId + "," + onCallId
                + ",'" + mDTFrom + "','" + mDTTo + "'";
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Tbl_TraceState"
                , ref FakeComboBox, aIsClearTable: true);
            if(mDS.Tables.Contains("Tbl_TraceState")  && mDS.Tables["Tbl_TraceState"].Rows.Count >0)
                uCars.ShowState(mDS.Tables["Tbl_TraceState"] , onCallId, requestId);
        }
    }
}
