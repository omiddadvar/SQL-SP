using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Bargh_Common;
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
        public frmTraceHistory()
        {
            try
            {
                InitializeComponent();
                initialData();
                handleUI03();
            }
            catch (Exception e) {
                CommonFunctions.ShowError(e);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetFirstRowData()) return;
                if (checkData("Tbl_Tablet", "Tbl_Master", "Tbl_Area"))
                    handleUI01();
                else
                {
                    if(mDS.Tables.Contains("Tbl_Request"))
                        mDS.Tables["Tbl_Request"].Clear();
                    handleUI03();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ShowError(ex);
            }
        }
        private void pnlExpandSearch_Click(object sender, EventArgs e)
        {
            //pnlSearchExpand.Size = GetSize(-1, true);
            this.iconPnl.Image = pnlExpandable.Visible
                ? global::Bargh_GIS.Properties.Resources.ArrowDown
                : global::Bargh_GIS.Properties.Resources.ArrowUp;

            pnlExpandable.Visible = !pnlExpandable.Visible;
        }

        private void btnRequests_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetRequestData()) return;
                if (checkData("Tbl_Request"))
                    handleUI02();
                else
                {
                    if (mDS.Tables.Contains("Tbl_Request"))
                        mDS.Tables["Tbl_Request"].Clear();
                    handleUI01();
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.ShowError(ex);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (!getFilters())
            {
                CommonFunctions.ShowInfo(errorString);
                return;
            }
            try
            {
                uCars.ResetMap();
                showAllTrace();
                showAllFlags();
                pnlExpandSearch_Click(null ,e);
            }
            catch (Exception ex)
            {
                CommonFunctions.ShowError(ex);
            }
        }

        private void btnCloseFilter_Click(object sender, EventArgs e)
        {
            handleUI03();
            dgArea.DataSource = null;
            dgMaster.DataSource = null;
            dgTablet.DataSource = null;
            dgRequest.DataSource = null;
        }
        private void pnlExpandSearch_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush linGrBrush = new LinearGradientBrush(
                pnlSearch.ClientRectangle,
                ColorTranslator.FromHtml("#6a93cb"),
                ColorTranslator.FromHtml("#a4bfef"),
                LinearGradientMode.Horizontal
            );
            e.Graphics.FillRectangle(linGrBrush, pnlSearch.ClientRectangle);
        }
    }
}
