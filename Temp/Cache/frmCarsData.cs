using System;
using System.Collections.Generic;
using System.Data;

namespace Bargh_GIS
{
    public partial class frmCars
    {
        private void fillComboArea() {
            Bargh_GIS.Classes.CDatabase.InitGISDB();
            Classes.CDatabase db = new Classes.CDatabase();
            db = new Classes.CDatabase();

            int lAreaId = -1;
            try
            {
                if (mRequestId > -1)
                {
                    DataTable dtRequest = db.ExecSQL("select * from TblRequest where RequestId = " + mRequestId.ToString());
                    if (dtRequest.Rows.Count > 0)
                    {
                        lAreaId = (int)dtRequest.Rows[0]["AreaId"];
                    }
                }
            }
            catch (Exception){}

            uCars = new wpf.TazarvMapUC_Cars(Bargh_GIS.Classes.CDatabase.mapSetting, fn, true, lAreaId);
            uCars.Name = "Map";
            MapElementHost.Child = uCars;
            string LSQL = "";
            if (mAreaIds != null && mAreaIds.Length > 0)
            {
                LSQL = " where AreaId in (";
                int i = 0;
                for (i = 0; i < mAreaIds.Length - 1; i++)
                {
                    LSQL = LSQL + mAreaIds[i].ToString() + ",";
                }
                LSQL = LSQL + mAreaIds[i].ToString() + ")";
            }
            DataTable dt = db.ExecSQL("select * from tbl_Area" + LSQL);
            chkArea.Fill(dt, "Area", "AreaId");
            mAreaIDs = chkArea.GetAllList();
        }
        private void GetOnCall()
        {
            Classes.CDatabase db = new Classes.CDatabase();
            string lSQL = "EXEC [Homa].[spGetOnCall] " + mOnCallChecksum + "," + mRequestId.ToString() + ",'" + mAreaIDs + "'";
            onCallDT = db.ExecSQL(lSQL);
            if (mOnCallChecksum == -1) dg.DataSource = onCallDT;
            FilterOnCall();
        }
        private void FilterOnCall()
        {
            if (onCallDT == null || onCallDT.Rows.Count == 0)
                return;
            onCallRows = onCallDT.Select("IsChecked = 1");
        }
        private void LoadOnCall() {
            try
            {
                long newCheckSum = 0;
                if (onCallDT.Rows.Count > 0)
                {
                    newCheckSum = Convert.ToInt64(onCallDT.Rows[0]["CheckSum"]);
                    if (newCheckSum != mOnCallChecksum)
                        uCars.LoadOnCall(onCallRows);
                }
                else if (newCheckSum != mOnCallChecksum)
                    uCars.LoadOnCall(onCallRows);

                mOnCallChecksum = newCheckSum;
            }
            catch (Exception E)
            {
                Console.WriteLine("{0}" + E, "Homa MAP, frmCars (timerRefreshOncall_Tick): ");
            }
        }
        private void selectAll() {
            if (onCallDT == null || onCallDT.Rows.Count == 0)
                return;
            foreach (DataRow row in onCallDT.Rows)
                row["IsChecked"] = true;
        }
        private void selectAllRevese() {
            if (onCallDT == null || onCallDT.Rows.Count == 0)
                return;
            foreach (DataRow row in onCallDT.Rows)
                row["IsChecked"] = ! Convert.ToBoolean(row["IsChecked"]);
        }
        private void selectNone() {
            if (onCallDT == null || onCallDT.Rows.Count == 0)
                return;
            foreach (DataRow row in onCallDT.Rows)
                row["IsChecked"] = false;
        }
    }
}
