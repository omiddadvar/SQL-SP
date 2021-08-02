using System;
using System.Data;
using Bargh_GIS.TazarvEvent;
using Bargh_Common;
using System.Collections.Generic;


namespace Bargh_GIS
{
    public partial class frmCars : FormBase
    {
        private int[] mAreaIds;
        private long mOnCallChecksum = -1;
        private string m_OnCallIds = "--";
        private long mRequestId = -1;
        private string mAreaIDs = "";
        //private int lCntRunTimer = 0;
        DataTable onCallDT , onCallTmp;
        DataRow[] onCallRows;
        private static int[] heights = new int[2] { 50, 260 };
        CallWpfFuctions fn = new CallWpfFuctions();
        wpf.TazarvMapUC_Cars uCars;

        public frmCars(int[] aAreaIds)
        {
            mAreaIds = aAreaIds;
            InitializeComponent();
        }
        public frmCars(long aRequestId)
        {
            mRequestId = aRequestId;
            InitializeComponent();
        }
        private void initWithReqID()
        {
            if (mRequestId < 0) return;
            GetOnCall();
            refreshMap();
            handleUI02();
            pnlExpandSearch_Click(null, null);
        }
        private void frmCars_Load(object sender, EventArgs e)
        {
            timerRefreshOncall.Enabled = false;
            handleUI01();
            fillComboArea();
            initWithReqID();
        }
        private void BttnShowEkip_Click(object sender, EventArgs e)
        {
            handleUI02();
            mAreaIDs = "";
            if (chkArea.GetDataList().Length > 0)
                mAreaIDs = chkArea.GetDataList();
            else
                mAreaIDs = chkArea.GetAllList();

            mOnCallChecksum = -1;
            uCars.m_OnCallIds = "";
            GetOnCall();
            timerRefreshOncall.Enabled = false;
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            pnlExpandSearch_Click(null , null);
            mOnCallChecksum = -1;
            FilterOnCall();
            timerRefreshOncall_Tick(null, null);
            //timerRefreshOncall.Enabled = true;
        }
        private void btnCloseFilter_Click(object sender, EventArgs e)
        {
            handleUI01();
        }
        private void pnlExpandSearch_Click(object sender, EventArgs e)
        {
            this.iconPnl.Image = pnlExpandable.Visible
                ? global::Bargh_GIS.Properties.Resources.ArrowDown
                : global::Bargh_GIS.Properties.Resources.ArrowUp;

            pnlExpandable.Visible = !pnlExpandable.Visible;
        }
        /// <summary>
        /// TimerTask (every 10 Sec) => To update "Map"
        /// </summary>
        private void timerRefreshOncall_Tick(object sender, EventArgs e)
        {
            if (uCars == null)
                return;
            timerRefreshOncall.Enabled = false;

            GetOnCall();
            LoadOnCall();
            refreshMap();

            timerRefreshOncall.Interval = 10000;
            timerRefreshOncall.Enabled = true;
        }
        private void cmdSelAll_Click(object sender, EventArgs e)
        {
            selectAll();
        }
        private void cmdSelRev_Click(object sender, EventArgs e)
        {
            selectAllRevese();
        }
        private void cmdSelNone_Click(object sender, EventArgs e)
        {
            selectNone();
        }
    }
}
