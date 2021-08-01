using System.Data.SqlClient;
using Bargh_Common;
using System.Data;
using System.Drawing;
using Bargh_GIS.TazarvEvent;

namespace Bargh_GIS
{
    public partial class frmTraceHistory
    {
        private static int[] heights = new int[3] { 70, 210, 325 };
        private int heightState;
        private Size GetSize(int i , bool current = false)
        {
            return new Size(this.Size.Width - 10, heights[ current ? heightState : i]);
        }
        private void initialData()
        {
            pnlExpandable.Size = GetSize(0);
            this.mCnn = new SqlConnection(CommonFunctions.GetConnection());
            this.mDS = new DataSet();
            this.db = new Classes.CDatabase();
            string lSQL = "SELECT * FROM Tbl_Area";
            CommonFunctions.BindingTable(lSQL, ref mCnn, ref mDS, "Combo_Area", ref FakeComboBox);
            cboArea.DataSource = mDS.Tables["Combo_Area"];

            
            //----------Map----------
            Bargh_GIS.Classes.CDatabase.InitGISDB();
            this.fn = new CallWpfFuctions();
            uCars = new wpf.TazarvMapUC_Cars(Bargh_GIS.Classes.CDatabase.mapSetting, fn, true
                , Bargh_Common.CommonVariables.WorkingAreaId);
            uCars.Name = "Map";
            uCars.ResetMap();
            MapElementHost.Child = uCars;
        }
        private void handleUI01()
        {
            heightState = 1;
            pnlExpandable.Size = GetSize(1);
            dgTablet.Visible = dgArea.Visible = dgMaster.Visible = true;
            pnlButtons.Visible = btnCloseFilter.Visible = btnRequests.Visible = btnShow.Visible = true;
            pnlButtons.Top = dgMaster.Bottom + 5;
            dgRequest.Visible = false;
        }
        private void handleUI02()
        {
            heightState = 2;
            pnlExpandable.Size = GetSize(2);
            pnlButtons.Top = dgRequest.Bottom + 5;
            dgRequest.Visible = true;
        }
        private void handleUI03()
        {
            heightState = 0;
            pnlExpandable.Size = GetSize(0);
            dgTablet.Visible = dgArea.Visible = dgMaster.Visible = false;
            pnlButtons.Visible = btnCloseFilter.Visible = btnRequests.Visible = btnShow.Visible = false;
            dgRequest.Visible = false;
        }
    }
}
