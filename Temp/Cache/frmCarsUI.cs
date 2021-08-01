using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bargh_GIS
{
    public partial class frmCars {
        private void refreshMap()
        {
            uCars.RefreshTrace(mRequestId);
            m_OnCallIds = uCars.m_OnCallIds;
            uCars.RefreshRequest(mRequestId, mAreaIDs);
        }
        private void handleUI01()
        {
            pnlExpandable.Size = GetSize(0);
            pnlButtons.Visible = false;
            dg.Visible = false;
        }
        private void handleUI02()
        {
            pnlExpandable.Size = GetSize(1);
            pnlButtons.Visible = true;
            dg.Visible = true;
        }
        private Size GetSize(int i)
        {
            return new Size(this.Size.Width - 22, heights[i]);
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
