using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using TZServiceTools;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using TZServicesLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TZServicesCSharp
{
    public class SubscriberOutageStateInfo
    {
        public bool IsSuccess;
        public string Name = "";
        public string Address = "";
        public string Telephone = "";
        public string Mobile = "";
        public string CallReason = "";
        public string State = "";
        public string EzamDate = "";
        public string ErrorMessage = "";
        public string AreaId = "";
        public string Area = "";
    }

    /// <summary>
    /// Summary description for HavadesReports
    /// </summary>
    [WebService(Namespace = "http://tazarv.com/tzwebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HavadesReports : System.Web.Services.WebService
    {
        #region Enums
        private enum DisconnectTypes
        {
            IsTamir,
            NotIsTamir,
            Both
        }

        private enum NetworkTypes
        {
            LP,
            MP,
            FT,
            All
        }

        #endregion

        #region Private Methods
        private double GetPartPower(int aAreaId, DateTime aFromDate, DateTime aToDate, string aFromDatePersian, string aToDatePersian,
            DisconnectTypes aDisconnectType, NetworkTypes aNetworkType)
        {
            string lSQL = "";
            DataSet lDs = new DataSet();
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            string lFromDate, lToDate;

            string lWhereFTinMP =
                " ( TblMPRequest.DisconnectGroupSetId IN (1129,1130) " +
                " OR (TblMPRequest.DisconnectReasonId >= 1200 AND " +
                " TblMPRequest.DisconnectReasonId <= 1299 AND NOT TblMPRequest.DisconnectReasonId IS NULL) ) ";

            lFromDate = aFromDate.ToShortDateString() + " " + aFromDate.Hour.ToString() + ":" + aFromDate.Minute.ToString();
            lToDate = aToDate.ToShortDateString() + " " + aToDate.Hour.ToString() + ":" + aToDate.Minute.ToString();

            lSQL +=
                " SELECT " +
                "	SUM " +
                "	( " +
                "		dbo.MinuteCount " +
                "		( " +
                "			CONVERT(DATETIME, '" + lFromDate + "' , 102), " +
                "			CONVERT(DATETIME, '" + lToDate + "' , 102), " +
                "			TblRequest.DisconnectDT, " +
                "			TblRequest.ConnectDT " +
                "		) " +
                "		/ TblRequest.DisconnectInterval " +
                "		* TblRequest.DisconnectPower " +
                "	) AS PartPower " +
                " FROM TblRequest";

            if (aNetworkType == NetworkTypes.MP || aNetworkType == NetworkTypes.FT)
                lSQL += " LEFT OUTER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId";

            lSQL +=
                " WHERE " +
                "	(TblRequest.DisconnectInterval > 0) AND (NOT (TblRequest.ConnectDT IS NULL)) " +
                "	AND TblRequest.DisconnectDatePersian >= '" + aFromDatePersian + "' AND TblRequest.DisconnectDatePersian <= '" + aToDatePersian + "' " +
                "	AND " + (aNetworkType == NetworkTypes.LP ? "IsLPRequest=1" :
                        (aNetworkType == NetworkTypes.MP ? "(IsMPRequest=1 AND NOT " + lWhereFTinMP + ")" :
                        (aNetworkType == NetworkTypes.FT ? "(IsFogheToziRequest=1 OR (IsMpRequest=1 AND" + lWhereFTinMP + "))" :
                "(IsMPRequest=1 OR IsLPRequest=1 OR IsFogheToziRequest=1)"))) +
                "	" + (aDisconnectType == DisconnectTypes.IsTamir ? "AND IsTamir=1" :
                        (aDisconnectType == DisconnectTypes.NotIsTamir ? "AND IsTamir=0" : "")) +
                "	" + (aAreaId > 0 ? "AND TblRequest.AreaId = " + aAreaId.ToString() : "") +
                " ORDER BY  " +
                "	PartPower DESC ";

            mdl_Publics.RemoveMoreSpaces(ref lSQL);
            mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblPartPower");

            if (lDs.Tables["TblPartPower"].Rows.Count > 0)
            {
                object lPower = lDs.Tables["TblPartPower"].Rows[0]["PartPower"];
                if (lPower is DBNull)
                    lPower = 0;
                return Math.Round(Convert.ToDouble(lPower), 3);
            }
            else
            {
                return 0;
            }
        }

        private void DateValidation(string aDate, bool aCheckIsEmpty, string aParamName)
        {
            if (aCheckIsEmpty)
            {
                if (string.IsNullOrEmpty(aDate))
                {
                    throw new Exception("[" + aParamName + "] is Empty");
                }
            }

            Regex lRgx = new Regex(@"^\d{4}(?:/\d{2}){2}$");
            if (!lRgx.IsMatch(aDate))
            {
                throw new Exception("[" + aParamName + "] is Invalid");
            }
        }

        private DataSet GetAreasExcept121()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("DsAreaList");
            string lSql;
            lSql =
                "select " +
                    " Tbl_Area.AreaId, Tbl_Area.Area " +
                " from " +
                    " Tbl_Area " +
                " where " +
                    " Tbl_Area.IsCenter = 0";

            TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
            TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Area");
            return lDs;
        }

        private string MakeWhereArea(string aAreaId)
        {
            int lAreaId;
            int.TryParse(aAreaId, out lAreaId);
            string lAreaIds = "";

            if (lAreaId > 0)
                lAreaIds = lAreaId.ToString();
            else
            {
                lAreaIds = " select " +
                                " Tbl_Area.AreaId " +
                           " from " +
                                " Tbl_Area " +
                           " where " +
                                " Tbl_Area.IsCenter = 0";
            }
            return lAreaIds;
        }

        private void SaveLog(string aLogMessage, System.Diagnostics.EventLogEntryType aLogType = System.Diagnostics.EventLogEntryType.Information)
        {
            mdl_Publics.SaveLog(Server, aLogMessage, "TZServices_Reports_Logs", aLogType: aLogType);
        }

		private DataTable GetErrorTable(string aMessage)
		{
			DataTable lTbl = new DataTable("Tbl_Error");
			lTbl.Columns.Add("ErrorMessage");
			lTbl.Rows.Add(aMessage);
			return lTbl;
		}

		#endregion

		#region BaseInfo Methods
        [WebMethod]
        // لیست نواحی به جز مراکز 121
        public DataSet GetAreaList()
        {
            DataSet lDs = new DataSet("DsAreaList");
            try
            {
                lDs = GetAreasExcept121();
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }
            return lDs;
        }

        // لیست پست های فوق توزیع
        [WebMethod]
        public DataSet GetMPPostList(string AreaId)
        {
            DataSet lDs = new DataSet("DsMPPostList");

            try
            {
                string lWhereArea = "";
                if (!string.IsNullOrEmpty(AreaId))
                    lWhereArea = " WHERE AreaId = " + AreaId + " ";

                string lSQL = "SELECT MPPostId, MPPostName FROM Tbl_MPPost " + lWhereArea;
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_MPPost");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // لیست فیدرهای فشار قوی
        [WebMethod]
        public DataSet GetMPFeederList(string MPPostId)
        {
            DataSet lDs = new DataSet("DsMPFeederList");

            try
            {
                string lWhereMPPost = "";
                if (!string.IsNullOrEmpty(MPPostId))
                    lWhereMPPost = " WHERE MPPostId = " + MPPostId + " ";

                string lSQL = "SELECT MPFeederId, MPFeederName FROM Tbl_MPFeeder " + lWhereMPPost;
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_MPFeeder");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // لیست پست های توزیع
        [WebMethod]
        public DataSet GetLPPostList(string MPFeederId)
        {
            DataSet lDs = new DataSet("DsMPPostList");

            try
            {
                string lWhereMPFeeder = "";
                if (!string.IsNullOrEmpty(MPFeederId))
                    lWhereMPFeeder = " WHERE MPFeederId = " + MPFeederId + " ";

                string lSQL = "SELECT LPPostId, LPPostName FROM Tbl_LPPost " + lWhereMPFeeder;
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_LPPost");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // لیست انواع مالکیت
        [WebMethod]
        public DataSet GetOwnershipList()
        {
            DataSet lDs = new DataSet("DsOwnershipList");

            try
            {
                string lSQL = "SELECT * FROM Tbl_Ownership";
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_Ownership");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }
		#endregion

		// دریافت کد MD5 پارامتر ورودی
        [WebMethod]
        public string GetMD5(string Code)
        {
            string lRet = "";
            Cryptography lCr = new Cryptography();
            using (MD5 lMD5 = MD5.Create())
            {
                lRet = lCr.GetMd5Hash(lMD5, Code);
            }
            return lRet;
        }

        // گزارش انرژی توزیع نشده ساعت به ساعت 
        [WebMethod]
        public DataSet GetHourByHourEnergy(string AreaId, string FromDatePersian, string ToDatePersian,
            int DisconnectType = -1, int NetworkType = -1)
        {
            DataSet lDs = new DataSet("DsReport_10_3");
            DataTable lTbl;
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Zahedan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DisconnectTypes lDisconnectType;
                NetworkTypes lNetworkType;

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");

                switch (DisconnectType)
                {
                    case 0:
                        lDisconnectType = DisconnectTypes.IsTamir;
                        break;
                    case 1:
                        lDisconnectType = DisconnectTypes.NotIsTamir;
                        break;
                    default:
                        lDisconnectType = DisconnectTypes.Both;
                        break;
                }

                switch (NetworkType)
                {
                    case 0:
                        lNetworkType = NetworkTypes.LP;
                        break;
                    case 1:
                        lNetworkType = NetworkTypes.MP;
                        break;
                    case 2:
                        lNetworkType = NetworkTypes.FT;
                        break;
                    default:
                        lNetworkType = NetworkTypes.All;
                        break;
                }

                int lAreaId;
                bool lCasted = int.TryParse(AreaId, out lAreaId);
                if (!lCasted)
                    lAreaId = 0;

                lTbl = new DataTable("Tbl_Report_10_3");
                object[] lTempCols = new object[27];
                double lSumPower;

                lTbl.Columns.Add("DatePersian");
                lTbl.Columns.Add("DT");

                string lClmName = "";
                for (int i = 0; i < 24; i++)
                {
                    lClmName = string.Format("W{0}_{1}", (i == 0 ? "00" : i.ToString()), (i < 23 ? (i + 1).ToString() : "00"));
                    lTbl.Columns.Add(lClmName, System.Type.GetType("System.Double"));
                }

                lTbl.Columns.Add("SumPower", System.Type.GetType("System.Double"));

                DateTime lFromDate, lToDate, lTempDate;
                PersianDate.KhayamDateTime lPd = new PersianDate.KhayamDateTime();
                lPd.SetShamsiDate(FromDatePersian);
                lFromDate = lPd.GetMiladyDate().Date;
                lPd.SetShamsiDate(ToDatePersian);
                lToDate = lPd.GetMiladyDate().Date;

                while (lFromDate <= lToDate)
                {
                    lPd.SetMiladyDate(lFromDate);
                    lTempCols[0] = lPd.GetShamsiDateStr();
                    lTempCols[1] = lFromDate.ToShortDateString();
                    lTempDate = lFromDate;
                    lSumPower = 0;
                    for (int i = 0; i < 24; i++)
                    {
                        lTempCols[i + 2] = GetPartPower(lAreaId, lTempDate, lTempDate.AddHours(1), FromDatePersian, ToDatePersian,
                            lDisconnectType, lNetworkType);
                        lSumPower += Convert.ToDouble(lTempCols[i + 2]);
                        lTempDate = lTempDate.AddHours(1);
                    }
                    lTempCols[26] = lSumPower;
                    lTbl.Rows.Add(lTempCols);
                    lFromDate = lFromDate.AddDays(1);
                }

                lDs.Tables.Add(lTbl);
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش 1-6
        [WebMethod]
        public DataSet GetReport6_1(string AreaId, string MPFeederBazdidFromDatePersian, string MPFeederBazdidToDatePersian,
            string LPPostBazdidFromDatePersian, string LPPostBazdidToDatePersian, string LPFeederBazdidFromDatePersian, string LPFeederBazdidToDatePersian)
        {
            DataSet lDs = new DataSet("DsReport_6_1");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                #region Input validation
                DateValidation(MPFeederBazdidFromDatePersian, true, "MPFeederBazdidFromDatePersian");
                DateValidation(MPFeederBazdidToDatePersian, true, "MPFeederBazdidToDatePersian");
                DateValidation(LPPostBazdidFromDatePersian, true, "LPPostBazdidFromDatePersian");
                DateValidation(LPPostBazdidToDatePersian, true, "LPPostBazdidToDatePersian");
                DateValidation(LPFeederBazdidFromDatePersian, true, "LPFeederBazdidFromDatePersian");
                DateValidation(LPFeederBazdidToDatePersian, true, "LPFeederBazdidToDatePersian");
                #endregion

                string lAreaIds = MakeWhereArea(AreaId);

                string lSQL = "";
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                lSQL = "exec spGetReport_LPFeeder_6_1 '" + LPFeederBazdidFromDatePersian + "','" + LPFeederBazdidToDatePersian + "'," + "'" + lAreaIds + "'";
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "LPFeeder_6_1");

                lSQL = "exec spGetReport_LPPost_6_1 '" + LPPostBazdidFromDatePersian + "','" + LPPostBazdidToDatePersian + "'," + "'" + lAreaIds + "'";
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "LPPost_6_1");

                lSQL = "exec spGetReport_MPFeeder_6_1 '" + MPFeederBazdidFromDatePersian + "','" + MPFeederBazdidToDatePersian + "'," + "'" + lAreaIds + "'";
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "MPFeeder_6_1");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش تعداد فیدرهای پر عارضه و بحرانی
        [WebMethod]
        public DataSet GetBohraniFeeders(string AreaId, string Year, string Month, int ValueForPorAreze, int ValueForBohrani)
        {
            DataSet lDs = new DataSet("DsReportBohraniFeeders");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lFromDate = "";
                string lToDate = "";
                if (!(string.IsNullOrEmpty(Year) || string.IsNullOrEmpty(Month)))
                {
                    lFromDate = string.Format("{0}/{1}/01", Year, Month);
                    lToDate = string.Format("{0}/{1}/31", Year, Month);
                }

                DateValidation(lFromDate, true, "Year or Month");
                DateValidation(lToDate, true, "Year or Month");


                string lAreaIds = MakeWhereArea(AreaId);

                string lWhere = "";

                lWhere += " AND TblRequest.AreaId IN (" + lAreaIds + ")";
                //خاموشی بی برنامه
                lWhere += " AND ISNULL(TblRequest.IsTamir,0) = 0 ";
                lWhere += " AND ISNULL(TblMPRequest.IsWarmLine, 0) = 0 ";
                lWhere += " AND TblRequest.EndJobStateId IN (2,3) ";
                //بدون فوق توزیع و بالاتر
                lWhere += " AND ((TblMPRequest.DisconnectReasonId IS NULL OR NOT (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299)) " +
                        " AND (TblMPRequest.DisconnectGroupSetId IS NULL OR NOT (TblMPRequest.DisconnectGroupSetId = 1129 OR TblMPRequest.DisconnectGroupSetId = 1130))) ";
                //منجر به قطع کامل فیدر فشار متوسط
                lWhere += " AND TblRequest.IsDisconnectMPFeeder = 1 ";

                lWhere += " AND TblRequest.DisconnectDatePersian >= '" + lFromDate + "'";
                lWhere += " AND TblRequest.DisconnectDatePersian <= '" + lToDate + "'";

                lWhere = Regex.Replace(lWhere, "^ AND", " WHERE");

                string lSQL = "";
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                lSQL +=
                    " SELECT PorCount, BohraniCount " +
                    " FROM " +
                    "   (SELECT 1 as Id, COUNT(t1.MPFeederId) AS PorCount " +
                    "   FROM " +
                    "   (SELECT  " +
                    "	    MPFeederId " +
                    "   FROM  " +
                    "	    TblRequest " +
                    "	    INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId " +
                    lWhere +
                    "   GROUP BY MPFeederId " +
                    "    HAVING  " +
                    "       COUNT(TblRequest.RequestId) >= " + ValueForPorAreze + " AND COUNT(TblRequest.RequestId) < " + ValueForBohrani + " " +
                    "   ) t1 ) tPor " +
                    " INNER JOIN " +
                    "   (SELECT 1 as Id, COUNT(t2.MPFeederId) AS BohraniCount " +
                    "   FROM " +
                    "   (SELECT  " +
                    "	    MPFeederId " +
                    "   FROM  " +
                    "	    TblRequest " +
                    "	    INNER JOIN TblMPRequest ON TblRequest.MPRequestId = TblMPRequest.MPRequestId " +
                    lWhere +
                    "   GROUP BY MPFeederId " +
                    "   HAVING  " +
                    "       COUNT(TblRequest.RequestId) >= " + ValueForBohrani + " " +
                    "   ) t2 ) tBohrani " +
                    " ON tPor.Id = tBohrani.Id ";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_BohraniFeedersCount");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش 1-3
        [WebMethod]
        public DataSet GetReport3_1(string AreaId, string FromDatePersian, string ToDatePersian, int LeadingTo = -1, int IsTamir = -1)
        {
            DataSet lDs = new DataSet("DsReport3_1");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");

                string lLeadingToWhere = "";

                switch (LeadingTo)
                {
                    case 0:
                        //منجر به قطع فیدر فشار متوسط شده است
                        lLeadingToWhere = " AND TblRequest.IsDisconnectMPFeeder = 1 ";
                        break;
                    case 1:
                        //منجر به قطعی در سر خط شده است
                        lLeadingToWhere = " AND TblMPRequest.IsNotDisconnectFeeder = 1 ";
                        break;
                    case 2:
                        //منجر به قطع کامل پست شده است
                        lLeadingToWhere = " AND TblMPRequest.IsTotalLPPostDisconnected = 1 ";
                        break;
                    case 3:
                        //منجر به قطع کامل پست نشده است
                        lLeadingToWhere = " AND TblMPRequest.IsTotalLPPostDisconnected = 0 ";
                        break;
                    default:
                        lLeadingToWhere = "";
                        break;
                }

                string lAreaIds = MakeWhereArea(AreaId);

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL =
                "   SELECT COUNT(TblRequest.RequestId) AS DisconnectCount " +
                "   FROM " +
                "       TblRequest INNER JOIN TblMPRequest " +
                "   ON " +
                "       TblRequest.MPRequestId = TblMPRequest.MPRequestId " +
                "   WHERE " +
                "       TblRequest.DisconnectDatePersian >= '" + FromDatePersian + "' " +
                "       AND TblRequest.DisconnectDatePersian <= '" + ToDatePersian + "' " +
                "       AND TblRequest.AreaId IN (" + lAreaIds + ") " +
                (IsTamir != -1 ? " AND ISNULL(TblRequest.IsTamir,0) = " + (IsTamir == 1 ? 1 : 0) + " " : "") +
                "       AND ( " +
                "               (TblMPRequest.DisconnectReasonId IS NULL " +
                "                   OR NOT (TblMPRequest.DisconnectReasonId >= 1200 AND TblMPRequest.DisconnectReasonId <= 1299)) " +
                "               AND (TblMPRequest.DisconnectGroupSetId IS NULL " +
                "                   OR NOT (TblMPRequest.DisconnectGroupSetId = 1129 OR TblMPRequest.DisconnectGroupSetId = 1130))) " +
                lLeadingToWhere;

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_MPRequest3_1");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // نرخ انرژی توزیع نشده
        [WebMethod]
        public DataSet GetDisconnectPowerNerkh(string AreaId, string Year, string Month, int IsTamir = -1)
        {
            DataSet lDs = new DataSet("DsDisconnectPowerNerkh");
            DataSet lDs2 = new DataSet();
            DataTable lTbl;
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(string.Format("{0}/{1}/01", Year, Month), true, "Year or Month");
                string lAreaIds = MakeWhereArea(AreaId);
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                string lSQL =
                    "   SELECT " +
                    "       ISNULL(SUM(ISNULL(DisconnectPower, 0)), 0) AS SumPower " +
                    "   FROM " +
                    "       TblRequest " +
                    "   WHERE " +
                    "       LEFT(DisconnectDatePersian,7) = '" + string.Format("{0}/{1}", Year, Month) + "' " +
                    (IsTamir != -1 ? " AND ISNULL(IsTamir,0) = " + (IsTamir == 1 ? 1 : 0) + " " : "") +
                    "       AND AreaId IN (" + lAreaIds + ")" +
                    "       AND EndJobStateId IN (2,3)";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblSumPower");
                double lSumPower = Convert.ToDouble(lDs2.Tables["TblSumPower"].Rows[0]["SumPower"]);

                lSQL =
                    "   SELECT " +
                    "       1 AS id, ISNULL(SUM(ISNULL(Energy,0)),0) AS SumEnergy " +
                    "   FROM " +
                    "       TblSubscribers " +
                    "   WHERE " +
                    "       YearMonth = '" + string.Format("{0}/{1}", Year, Month) + "' " +
                    "   AND AreaId IN (" + lAreaIds + ")";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblSumEnergy");
                double lSumEnergy = Convert.ToDouble(lDs2.Tables["TblSumEnergy"].Rows[0]["SumEnergy"]);

                double lDisconnectPowerNerkh, lSoorat, lMakhraj;
                lSoorat = lSumPower * 1000;
                lMakhraj = lSumPower + lSumEnergy;

                if (lMakhraj == 0)
                    lDisconnectPowerNerkh = 0;
                else
                    lDisconnectPowerNerkh = lSoorat / lMakhraj;

                lTbl = new DataTable("Tbl_DisconnectPowerNerkh");
                lTbl.Columns.Add("DisconnectPowerNerkh");
                lTbl.Rows.Add(lDisconnectPowerNerkh.ToString());
                lDs.Tables.Add(lTbl);

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // زمان خاموشی هر مشترک
        [WebMethod]
        public DataSet GetSubscriberDisconnectTime(string AreaId, string Year, string Month, int IsTamir = -1)
        {
            DataSet lDs = new DataSet("DsSubscriberDisconnectTime");
            DataSet lDs2 = new DataSet();
            DataTable lTbl;
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(string.Format("{0}/{1}/01", Year, Month), true, "Year or Month");
                string lAreaIds = MakeWhereArea(AreaId);
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                string lSQL =
                    "   SELECT " +
                    "       ISNULL(SUM(ISNULL(DisconnectPower, 0)), 0) AS SumPower " +
                    "   FROM " +
                    "       TblRequest " +
                    "   WHERE " +
                    "       LEFT(DisconnectDatePersian,7) = '" + string.Format("{0}/{1}", Year, Month) + "' " +
                    (IsTamir != -1 ? " AND ISNULL(IsTamir,0) = " + (IsTamir == 1 ? 1 : 0) + " " : "") +
                    "       AND AreaId IN (" + lAreaIds + ")" +
                    "       AND EndJobStateId IN (2,3)";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblSumPower");
                double lSumPower = Convert.ToDouble(lDs2.Tables["TblSumPower"].Rows[0]["SumPower"]);

                lSQL =
                    "   SELECT " +
                    "       1 AS id, ISNULL(SUM(ISNULL(Energy,0)),0) AS SumEnergy " +
                    "   FROM " +
                    "       TblSubscribers " +
                    "   WHERE " +
                    "       YearMonth = '" + string.Format("{0}/{1}", Year, Month) + "' " +
                    "   AND AreaId IN (" + lAreaIds + ")";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblSumEnergy");
                double lSumEnergy = Convert.ToDouble(lDs2.Tables["TblSumEnergy"].Rows[0]["SumEnergy"]);

                double lSubscriberDisconnectTime, lDisconnectPowerNerkh, lSoorat, lMakhraj;
                lSoorat = lSumPower * 1000;
                lMakhraj = lSumPower + lSumEnergy;

                if (lMakhraj == 0)
                    lDisconnectPowerNerkh = 0;
                else
                    lDisconnectPowerNerkh = lSoorat / lMakhraj;

                lSubscriberDisconnectTime = 1.44 * lDisconnectPowerNerkh;

                lTbl = new DataTable("Tbl_SubscriberDisconnectTime");
                lTbl.Columns.Add("SubscriberDisconnectTime");
                lTbl.Rows.Add(lSubscriberDisconnectTime.ToString());
                lDs.Tables.Add(lTbl);

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // تعداد قطعی به طول شبکه  در هر صد متر
        [WebMethod]
        public DataSet GetDisconnectCountIn100km(string AreaId, string FromDatePersian, string ToDatePersian, int RequestType)
        {
            string lRequestType;
            string lIsLightRequestQuery = "";

            if (RequestType == 0)
                lRequestType = "MP";
            else
            {
                lRequestType = "LP";
                lIsLightRequestQuery = " AND ISNULL(IsLightRequest,0) = 0 ";
            }

            DataSet lDs = new DataSet("DsDisconnectCountIn100km");
            DataSet lDs2 = new DataSet();
            DataTable lTbl;
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");
                string lAreaIds = MakeWhereArea(AreaId);
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                string lSQL =
                        " SELECT " +
                        "   COUNT(" + lRequestType + "RequestId) AS RequestCount " +
                        " FROM " +
                        "   Tbl" + lRequestType + "Request " +
                        " WHERE " +
                        "   DisconnectDatePersian >= '" + FromDatePersian + "' " +
                        "   AND DisconnectDatePersian <= '" + ToDatePersian + "' " +
                        "   AND AreaId IN (" + lAreaIds + ") " +
                        "   AND ISNULL(IsWarmLine,0) = 0 " +
                        "   AND EndJobStateId IN (2,3) " +
                        lIsLightRequestQuery;

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblRequestCount");

                int lRequestCount = Convert.ToInt32(lDs2.Tables["TblRequestCount"].Rows[0]["RequestCount"]);

                lSQL =
                    " SELECT " +
                    "   SUM(" + lRequestType + "FeederHavayiLen) + SUM(" + lRequestType + "FeederZaminiLen) AS NetworkLengh " +
                    " FROM " +
                    "   Tbl_NetworkInfo " +
                    " WHERE " +
                    "   AreaId IN (" + lAreaIds + ") " +
                    "   AND PersianYear = '" + ToDatePersian.Substring(0, 4) + "'";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblNetworkLengh");

                double lNetworkLengh = Convert.ToDouble(lDs2.Tables["TblNetworkLengh"].Rows[0]["NetworkLengh"]);

                double lDisconnectCountIn100m;
                if (lNetworkLengh == 0)
                    lDisconnectCountIn100m = 0;
                else
                    lDisconnectCountIn100m = (lRequestCount / lNetworkLengh) * 100;

                lTbl = new DataTable("Tbl_DisconnectCountIn100km");
                lTbl.Columns.Add("DisconnectCountIn100km");
                lTbl.Rows.Add(lDisconnectCountIn100m.ToString());
                lDs.Tables.Add(lTbl);
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // تعداد قطعی به تعداد فیدر
        [WebMethod]
        public DataSet GetDisconnectPerFeeder(string AreaId, string FromDatePersian, string ToDatePersian, int RequestType)
        {
            string lRequestType;
            string lIsLightRequestQuery = "";

            if (RequestType == 0)
                lRequestType = "MP";
            else
            {
                lRequestType = "LP";
                lIsLightRequestQuery = " AND ISNULL(IsLightRequest,0) = 0 ";
            }

            DataSet lDs = new DataSet("DsGetDisconnectPerFeeder");
            DataSet lDs2 = new DataSet();
            DataTable lTbl;
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");
                string lAreaIds = MakeWhereArea(AreaId);
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                string lSQL =
                        " SELECT " +
                        "   COUNT(" + lRequestType + "RequestId) AS RequestCount " +
                        " FROM " +
                        "   Tbl" + lRequestType + "Request " +
                        " WHERE " +
                        "   DisconnectDatePersian >= '" + FromDatePersian + "' " +
                        "   AND DisconnectDatePersian <= '" + ToDatePersian + "' " +
                        "   AND AreaId IN (" + lAreaIds + ") " +
                        "   AND ISNULL(IsWarmLine,0) = 0 " +
                        "   AND EndJobStateId IN (2,3) " +
                        lIsLightRequestQuery;

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblRequestCount");

                int lRequestCount = Convert.ToInt32(lDs2.Tables["TblRequestCount"].Rows[0]["RequestCount"]);

                string lFeederCountQuery = "";
                if (RequestType == 0)
                    lFeederCountQuery = " ISNULL(SUM(ISNULL(MPFeederPublicCount,0) + ISNULL(MPFeederPrivateCount,0)),0) ";
                else
                    lFeederCountQuery = " ISNULL(SUM(ISNULL(LPFeederCount, 0)),0) ";

                lSQL =
                    " SELECT " +
                    lFeederCountQuery + " AS FeederCount " +
                    " FROM " +
                    "   Tbl_NetworkInfo " +
                    " WHERE " +
                    "   AreaId IN (" + lAreaIds + ") " +
                    "   AND PersianYear = '" + ToDatePersian.Substring(0, 4) + "'";

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblFeederCount");

                double lFeederCount = Convert.ToDouble(lDs2.Tables["TblFeederCount"].Rows[0]["FeederCount"]);

                double lDisconnectPerFeeder;
                if (lFeederCount == 0)
                    lDisconnectPerFeeder = 0;
                else
                    lDisconnectPerFeeder = (lRequestCount / lFeederCount) * 100;

                lTbl = new DataTable("Tbl_DisconnectPerFeeder");
                lTbl.Columns.Add("DisconnectPerFeeder");
                lTbl.Rows.Add(lDisconnectPerFeeder.ToString());
                lDs.Tables.Add(lTbl);
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // متوسط زمان رفع خاموشی
        [WebMethod]
        public DataSet GetDisconnectAverage(string AreaId, string FromDatePersian, string ToDatePersian, int RequestType)
        {
            string lRequestType;
            string lIsLightRequestQuery = "";

            if (RequestType == 0)
                lRequestType = "MP";
            else
            {
                lRequestType = "LP";
                lIsLightRequestQuery = " AND ISNULL(IsLightRequest,0) = 0 ";
            }

            DataSet lDs = new DataSet("DsGetDisconnectAverage");
            DataSet lDs2 = new DataSet();
            DataTable lTbl;
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");
                string lAreaIds = MakeWhereArea(AreaId);
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                string lSQL =
                        " SELECT " +
                        "   COUNT(" + lRequestType + "RequestId) AS RequestCount " +
                        " FROM " +
                        "   Tbl" + lRequestType + "Request " +
                        " WHERE " +
                        "   DisconnectDatePersian >= '" + FromDatePersian + "' " +
                        "   AND DisconnectDatePersian <= '" + ToDatePersian + "'" +
                        "   AND AreaId IN (" + lAreaIds + ") " +
                        "   AND ISNULL(IsWarmLine,0) = 0 " +
                        "   AND EndJobStateId IN (2,3) " +
                        lIsLightRequestQuery;

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblRequestCount");

                int lRequestCount = Convert.ToInt32(lDs2.Tables["TblRequestCount"].Rows[0]["RequestCount"]);

                lSQL =
                    " SELECT " +
                    "   ISNULL(SUM(ISNULL(DisconnectInterval,0)),0) AS DisconnectInterval " +
                    " FROM " +
                    "   Tbl" + lRequestType + "Request " +
                    " WHERE " +
                    "   AND DisconnectDatePersian >= '" + FromDatePersian + "' " +
                    "   AND DisconnectDatePersian <= '" + ToDatePersian + "'" +
                    "   AND AreaId IN (" + lAreaIds + ") " +
                    "   AND ISNULL(IsWarmLine,0) = 0 " +
                    "   AND EndJobStateId IN (2,3) " +
                    lIsLightRequestQuery;

                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs2, "TblDisconnectInterval");

                double lDisconnectInterval = Convert.ToDouble(lDs2.Tables["TblDisconnectInterval"].Rows[0]["DisconnectInterval"]);

                double lDisconnectAverage;
                if (lRequestCount == 0)
                    lDisconnectAverage = 0;
                else
                    lDisconnectAverage = (lDisconnectInterval / lRequestCount);

                lTbl = new DataTable("Tbl_DisconnectAverage");
                lTbl.Columns.Add("DisconnectAverage");
                lTbl.Rows.Add(lDisconnectAverage.ToString());
                lDs.Tables.Add(lTbl);
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش 2-6
        [WebMethod]
        public DataSet GetReport6_2(string AreaId, string FromDatePersian, string ToDatePersian, bool IsActive = false)
        {
            DataSet lDs = new DataSet("DsReport6_2");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");
                int lIsActive = -1;
                if (IsActive)
                    lIsActive = 1;
                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = lSQL = "exec spGetReport_6_2 '" + FromDatePersian + "','" + ToDatePersian + "','" + AreaId + "','1,2,3'," + lIsActive;
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_Report6_2");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش 2-2-4
        [WebMethod]
        public DataSet GetReport4_2_2(string AreaId, string FromDatePersian, string ToDatePersian, string MPPostId, string MPFeederId, string LPPostId, string OwnershipId, bool IsActive = false)
        {
            DataSet lDs = new DataSet("DsReport4_2_2");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Markazi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDatePersian, true, "FromDatePersian");
                DateValidation(ToDatePersian, true, "ToDatePersian");

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                DataSet lDs2 = new DataSet();
                int lIsActive = -1, lMPPostId = -1, lMPFeederId = -1, lLPPostId = -1, lOwnershipId = -1;

                if (IsActive)
                    lIsActive = 1;
                if (!string.IsNullOrEmpty(OwnershipId))
                    int.TryParse(OwnershipId, out lOwnershipId);
                if (!string.IsNullOrEmpty(MPPostId))
                {
                    int.TryParse(MPPostId, out lMPPostId);
                    mdl_Publics.BindingTable("SELECT COUNT(MPPostId) AS count FROM Tbl_MPPost WHERE " +
                        " AreaId = '" + AreaId + "' AND MPPostId = '" + MPPostId + "' ", ref lCnn, lDs2, "Tbl_MPPost");
                    if (Convert.ToInt32(lDs2.Tables["Tbl_MPPost"].Rows[0]["count"]) == 0)
                        throw new Exception("Selcted [MPPostId] is not valid for [AreaId]");
                }
                if (!string.IsNullOrEmpty(MPFeederId))
                {
                    int.TryParse(MPFeederId, out lMPFeederId);
                    mdl_Publics.BindingTable("SELECT COUNT(MPFeederId) AS count FROM Tbl_MPFeeder WHERE " +
                        " MPPostId = '" + MPPostId + "' AND MPFeederId = '" + MPFeederId + "' ", ref lCnn, lDs2, "Tbl_MPFeeder");
                    if (Convert.ToInt32(lDs2.Tables["Tbl_MPFeeder"].Rows[0]["count"]) == 0)
                        throw new Exception("Selcted [MPFeederId] is not valid for [MPPostId]");
                }
                if (!string.IsNullOrEmpty(LPPostId))
                {
                    int.TryParse(LPPostId, out lLPPostId);
                    mdl_Publics.BindingTable("SELECT COUNT(LPPostId) AS count FROM Tbl_LPPost WHERE " +
                        " MPFeederId = '" + MPFeederId + "' AND LPPostId = '" + LPPostId + "' ", ref lCnn, lDs2, "Tbl_LPPost");
                    if (Convert.ToInt32(lDs2.Tables["Tbl_LPPost"].Rows[0]["count"]) == 0)
                        throw new Exception("Selcted [LPPostId] is not valid for [MPFeederId]");
                }

                string lSQL = lSQL = lSQL = "exec spGetReport_4_2_2 '" + FromDatePersian + "','" + ToDatePersian + "','" + AreaId + "'," + lMPPostId + "," + lMPFeederId + "," + lLPPostId + "," + lOwnershipId + "," + lIsActive;
                mdl_Publics.RemoveMoreSpaces(ref lSQL);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_Report4_2_2");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش انرژی توزیع نشده ـ جنوب کرمان
        [WebMethod]
        public DataSet GetReportDisconnectPower(string AreaId, string UserName, string Password, string FromDatePersian, string ToDatePersian, string OwnershipId = "")
        {
            DataSet lDs = new DataSet("DsReportDisconnectPower");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.JonoobKerman))
                {
                    throw new Exception("RPT - Access Denied");
                }

                //Check user access
                UserTrustee lChkUser = new UserTrustee();
                int lUserLoginResult = lChkUser.CheckUser(Convert.ToInt32(AreaId), UserName, Password, Convert.ToInt32(ConstantsData.Applications.Havades));
                switch (lUserLoginResult)
                {
                    case -1:
                        throw new Exception("User name not found or access denied");
                    case 0:
                        throw new Exception("Password is incorrect");
                    case 1:
                        DateValidation(FromDatePersian, true, "FromDatePersian");
                        DateValidation(ToDatePersian, true, "ToDatePersian");

                        SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                        string lSQL = "exec spjk_GetDisconnectPower '" + FromDatePersian + "','" + ToDatePersian + "'," + AreaId + "," + OwnershipId;
                        mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_DisconnectPower");
                        break;
                }
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش شاخصهای انرژي - جنوب کرمان
        [WebMethod]
        public DataSet GetReportPowerIndex(string UserName, string Password, string AreaId, string FromDatePersian, string ToDatePersian)
        {
            string lMsg = string.Format(
                "UserName({0}),Password({1}),AreaId({2}),FromDatePersian({3}),ToDatePersian({4})",
                UserName, Password, AreaId, FromDatePersian, ToDatePersian
            );
            mdl_Publics.LogMessage(Server, lMsg, "HavadesReports_Log");

            DataSet lDs = new DataSet("DsReportPowerIndex");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.JonoobKerman))
                {
                    throw new Exception("RPT - Access Denied");
                }
                UserTrustee lChkUser = new UserTrustee();
                int lUserLoginResult = lChkUser.CheckUser(Convert.ToInt32(AreaId), UserName, Password, Convert.ToInt32(ConstantsData.Applications.Havades));

                switch (lUserLoginResult)
                {
                    case -1:
                        throw new Exception("User name not found or access denied");
                    case 0:
                        throw new Exception("Password is incorrect");
                    case 1:
                        string lFromDate = FromDatePersian;
                        string lToDate = ToDatePersian;
                        if (lFromDate.Length == 8)
                            lFromDate = "13" + lFromDate;
                        if (lToDate.Length == 8)
                            lToDate = "13" + lToDate;
                        DateValidation(lFromDate, true, "FromDatePersian");
                        DateValidation(lToDate, true, "ToDatePersian");

                        SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                        string lAreaId = "-1";
                        if (AreaId != "")
                            lAreaId = AreaId;
                        string lSQL = "exec spjk_GetPowerIndex '" + lFromDate + "','" + lToDate + "'," + lAreaId;
                        mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_PowerIndex", aIsShowError: true, aTimeOut: 300);
                        mdl_Publics.LogMessage(Server, string.Format("Result: {0} record(s).", lDs.Tables["Tbl_PowerIndex"].Rows.Count), "HavadesReports_Log");
                        break;
                }
            }
            catch (Exception ex)
            {
                mdl_Publics.LogError(Server, ex.Message, "HavadesReports_Log");
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش اطلاعات بازدید و سرویس - سیستان و بلوچستان
        [WebMethod]
        public DataSet GetReportBazdidServiceInfo(string AreaId, string FromDatePersian, string ToDatePersian, string BalancedValue)
        {
            DataSet lDs = new DataSet("DsReportBazdidServiceInfo");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Zahedan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lFromDate = FromDatePersian;
                string lToDate = ToDatePersian;
                if (lFromDate.Length == 8)
                    lFromDate = "13" + lFromDate;
                if (lToDate.Length == 8)
                    lToDate = "13" + lToDate;
                DateValidation(lFromDate, true, "FromDatePersian");
                DateValidation(lToDate, true, "ToDatePersian");

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lAreaId = "-1";
                if (AreaId != "")
                    lAreaId = AreaId;

                string lBalanced = "20";
                if (BalancedValue != "")
                    lBalanced = BalancedValue;

                string lSQL = "exec spMIS_BazdidServiceInfo " + lAreaId + ", '" + lFromDate + "','" + lToDate + "'," + lBalanced;
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_BazdidServiceInfo");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // فهرست خاموشیهای فشار متوسط
        [WebMethod]
        public DataSet GetReportMPOutage(string AreaId, string FromDatePersian, string ToDatePersian)
        {
            DataSet lDs = new DataSet("DsReportOutage");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lFromDate = FromDatePersian;
                string lToDate = ToDatePersian;
                if (lFromDate.Length == 8)
                    lFromDate = "13" + lFromDate;
                if (lToDate.Length == 8)
                    lToDate = "13" + lToDate;
                DateValidation(lFromDate, true, "FromDatePersian");
                DateValidation(lToDate, true, "ToDatePersian");

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                if (AreaId.Trim() == "")
                    throw new Exception("AreaId Is Invalid");

                string lSQL =
                       string.Format("exec spGilan_ReportMPOutage '{0}', '{1}', {2}", lFromDate, lToDate, Int32.Parse(AreaId));

                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblMPOutage");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // فهرست خاموشیهای فشار ضعيف
        [WebMethod]
        public DataSet GetReportLPOutage(string AreaId, string FromDatePersian, string ToDatePersian)
        {
            DataSet lDs = new DataSet("DsReportOutage");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lFromDate = FromDatePersian;
                string lToDate = ToDatePersian;
                if (lFromDate.Length == 8)
                    lFromDate = "13" + lFromDate;
                if (lToDate.Length == 8)
                    lToDate = "13" + lToDate;
                DateValidation(lFromDate, true, "FromDatePersian");
                DateValidation(lToDate, true, "ToDatePersian");

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                if (AreaId.Trim() == "")
                    throw new Exception("AreaId Is Invalid");

                string lSQL =
                       string.Format("exec spGilan_ReportLPOutage '{0}', '{1}', {2}", lFromDate, lToDate, Int32.Parse(AreaId));

                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblLPOutage");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // فهرست خاموشیهای بابرنامه تایید شده
        [WebMethod]
        public DataSet GetReportWantedOutage(string AreaId, string FromDatePersian, string ToDatePersian)
        {
            DataSet lDs = new DataSet("DsReportOutage");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lFromDate = FromDatePersian;
                string lToDate = ToDatePersian;
                if (lFromDate.Length == 8)
                    lFromDate = "13" + lFromDate;
                if (lToDate.Length == 8)
                    lToDate = "13" + lToDate;
                DateValidation(lFromDate, true, "FromDatePersian");
                DateValidation(lToDate, true, "ToDatePersian");

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);

                if (AreaId.Trim() == "")
                    throw new Exception("AreaId Is Invalid");

                string lAreaWhere = "";
                if (AreaId != "-1")
                    lAreaWhere = "  AND TblTamirRequest.AreaId = " + AreaId;

                string lSQL =
                        "SELECT " +
                        "   TblTamirRequest.AreaId," +
                        "	TblTamirRequest.DisconnectDatePersian as OutageDate," +
                        "	TblTamirRequest.DisconnectTime as OutageTime," +
                        "	TblTamirRequest.ConnectDatePersian as ConnectDate," +
                        "	TblTamirRequest.ConnectTime as ConnectTime," +
                        "	Tbl_MPPost.MPPostCode," +
                        "	Tbl_MPPost.MPPostName," +
                        "	Tbl_MPFeeder.MPFeederCode," +
                        "	Tbl_MPFeeder.MPFeederName," +
                        "	Tbl_LPPost.LPPostCode," +
                        "	Tbl_LPPost.LPPostName," +
                        "	Tbl_LPFeeder.LPFeederCode," +
                        "	Tbl_LPFeeder.LPFeederName, " +
                        "   TblRequest.RequestNumber as OutageNumber, " +
                        "   ISNULL(TblTamirRequest.IsWarmLine, 0) as IsWarmLine, " +
                        "   TblTamirRequest.TamirRequestNo as TamirNumber " +
                        "FROM " +
                        "	TblTamirRequest" +
                        "	LEFT JOIN TblTamirRequestConfirm ON TblTamirRequest.TamirRequestId = TblTamirRequestConfirm.TamirRequestId" +
                        "	INNER JOIN TblRequest ON ISNULL(TblTamirRequestConfirm.RequestId, TblTamirRequest.EmergencyRequestId) = TblRequest.RequestId" +
                        "	LEFT JOIN Tbl_LPFeeder ON TblTamirRequest.LPFeederId = Tbl_LPFeeder.LPFeederId" +
                        "	LEFT JOIN Tbl_MPFeeder ON TblTamirRequest.MPFeederId = Tbl_MPFeeder.mPFeederId" +
                        "	LEFT JOIN Tbl_LPpost ON TblTamirRequest.LPPostId = Tbl_LPPost.LPPostId" +
                        "	LEFT JOIN Tbl_MPPost ON TblTamirRequest.MPPostId = Tbl_MPPost.MPPostId " +
                        "WHERE " +
                        "	TblTamirRequest.DisconnectDatePersian >= '" + lFromDate + "' " +
                        "	AND TblTamirRequest.DisconnectDatePersian <= '" + lToDate + "' " +
                        "	AND TblRequest.EndJobStateId IN (4, 5) " +
                        "	AND TblTamirRequest.TamirRequestStateId <> 5 " +
                        lAreaWhere;

                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblWantedOutage");
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // آمار خاموشی به تفکیک فیدر فشار متوسط و بابرنامه/بی برنامه و علت قطع - ایلام
        [WebMethod]
        public DataSet GetReportMPOutageInfo(string AreaId, string FromDate, string ToDate, string MPPostIDs, string DisconnectGroupSetIDs)
        {
            DataSet lDs = new DataSet("DsReportMPOutageInfo");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Ilam))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                string lAreaId = "";
                string lMPPostIDs = "";
                string lDCGSetIDs = "";

                if (AreaId == "-1")
                    lAreaId = "";
                else
                    lAreaId = AreaId;

                if (MPPostIDs == "-1")
                    lMPPostIDs = "";
                else
                    lMPPostIDs = MPPostIDs;

                if (DisconnectGroupSetIDs == "-1")
                    lDCGSetIDs = "";
                else
                    lDCGSetIDs = DisconnectGroupSetIDs;

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spGetReportMPOutageForIlam '" + lAreaId + "', '" + FromDate + "', '" + ToDate + "', '" + MPPostIDs + "','" + DisconnectGroupSetIDs + "'";
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblMPOutage");

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }


        // فهرست اطلاعات پایه و پیک بار پست های توزیع - ایلام
        [WebMethod]
        public DataSet GetReportLPPostPeak(string AreaId, string FromDate, string ToDate, string MPPostIDs)
        {
            DataSet lDs = new DataSet("DsReportLPPostPeak");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Ilam))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                string lAreaId = "";
                string lMPPostIDs = "";

                if (AreaId == "-1")
                    lAreaId = "";
                else
                    lAreaId = AreaId;

                if (MPPostIDs == "-1")
                    lMPPostIDs = "";
                else
                    lMPPostIDs = MPPostIDs;

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spGetReportLPPostPeakForIlam '" + lAreaId + "', '" + FromDate + "', '" + ToDate + "', '" + MPPostIDs + "'";
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblLPPostLoad");

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // فهرست اطلاعات پایه و پیک بار فیدرهای فشار متوسط - ایلام
        [WebMethod]
        public DataSet GetReportMPFeederPeak(string AreaId, string FromDate, string ToDate, string MPPostIDs)
        {
            DataSet lDs = new DataSet("DsReportLPPostPeak");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Ilam))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                string lAreaId = "";
                string lMPPostIDs = "";

                if (AreaId == "-1")
                    lAreaId = "";
                else
                    lAreaId = AreaId;

                if (MPPostIDs == "-1")
                    lMPPostIDs = "";
                else
                    lMPPostIDs = MPPostIDs;

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spGetReportMPFeederPeakForIlam '" + lAreaId + "', '" + FromDate + "', '" + ToDate + "', '" + MPPostIDs + "'";
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblMPFeederPeak");

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش شاخصهای قابلیت اطمینان - آذربایجان غربی 
        [WebMethod]
        public DataSet GetReportIndexInfo(string AreaId, string FromDate, string ToDate, int IsDCFeeder, int IsWithFogheTozi, int IsTamir, int IsAll)
        {
            DataSet lDs = new DataSet("DsReport_IndexInfo");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.AzGharbi))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                string lAreaId = "";


                if (AreaId == "-1")
                    lAreaId = "";
                else
                    lAreaId = AreaId;

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spGetIndexInfo '" + FromDate + "', '" + ToDate + "','" + lAreaId + "'," + IsDCFeeder + "," + IsWithFogheTozi + "," + IsTamir + "," + IsAll;
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblIndexInfo");

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش آماری قطع پستهای توزیع - شمال کرمان
        [WebMethod]
        public DataSet GetReportLPPostOutage(string AreaId, string FromDate, string ToDate, string IsTamir)
        {
            DataSet lDs = new DataSet("DsReport_LPPostOutage");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.ShomalKerman))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                string lAreaId = "";


                if (AreaId == "-1")
                    lAreaId = "";
                else
                    lAreaId = AreaId;

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spShK_GetPostOutage '" + lAreaId + "','" + FromDate + "', '" + ToDate + "','" + IsTamir + "'";
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblLPPostOutage", aIsClearTable: true);

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        // گزارش تعداد ارجاعات به دلیل افت ولتاژ - شمال کرمان
        [WebMethod]
        public DataSet GetReportErjaByVoltageReason(string AreaId, string FromDate, string ToDate)
        {
            DataSet lDs = new DataSet("DsReport_VoltageReason");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.ShomalKerman))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                string lAreaId = "";


                if (AreaId == "-1")
                    lAreaId = "";
                else
                    lAreaId = AreaId;

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spShK_GetErjaOutage '" + lAreaId + "','" + FromDate + "', '" + ToDate + "'";
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblErjaByVoltageReason", aIsClearTable: true);

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        //  اطلاعات خاموشی ها جهت نمایش در داشبورد مدیریتی - البرز
        [WebMethod]
        public DataSet GetOutageStatsInfo(string AreaId, string FromDate, string ToDate)
        {
            DataSet lDs = new DataSet("DsReport_OutageStats");
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Alborz))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");

                int lAreaId = -1;
                try
                {
                    if (Convert.ToInt32(AreaId) > 0)
                        lAreaId = Convert.ToInt32(AreaId);
                }
                catch (Exception)
                {
                    lAreaId = -1;
                }

                SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
                string lSQL = "EXEC spGetOutageStatsInfo " + lAreaId + ",'" + FromDate + "', '" + ToDate + "'";
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblOutageInfo", aIsClearTable: true);

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(CFunctions.GetErrorDataTable(Server, ex));
            }

            return lDs;
        }

        //  فهرست خاموشی های بی برنامه فشار متوسط - البرز
        [WebMethod]
        public DataSet SANJESH_AVGUNWANTED(string FromDate, string ToDate)
        {
            SaveLog(String.Format("Calling SANJESH_AVGUNWANTED (FromDate={0} , ToDate={1})", FromDate, ToDate));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Alborz))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");


                string lSQL = "";
                lSQL = string.Format("exec spAlborz_SANJESH_AVGUNWANTED '{0}', '{1}' ", FromDate, ToDate);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spAlborz_SANJESH_AVGUNWANTED IS SUCCESS"));

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spAlborz_SANJESH_AVGUNWANTED IS ERROR"));
            }

            return lDs;
        }

        //  فهرست خاموشی های بابرنامه فشار متوسط - البرز
        [WebMethod]
        public DataSet SANJESH_AVGWANTED(string FromDate, string ToDate)
        {
            SaveLog(String.Format("Calling SANJESH_AVGWANTED (FromDate={0} , ToDate={1})", FromDate, ToDate));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Alborz))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");


                string lSQL = "";
                lSQL = string.Format("exec spAlborz_SANJESH_AVGWANTED '{0}', '{1}' ", FromDate, ToDate);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spAlborz_SANJESH_AVGWANTED IS SUCCESS"));

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spAlborz_SANJESH_AVGWANTED IS ERROR"));
            }

            return lDs;
        }

        //  فهرست خاموشی های فوق توزيع - البرز
        [WebMethod]
        public DataSet SANJESH_HIGHPOSTOFF(string FromDate, string ToDate)
        {
            SaveLog(String.Format("Calling SANJESH_HIGHPOSTOFF (FromDate={0} , ToDate={1})", FromDate, ToDate));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Alborz))
                {
                    throw new Exception("RPT - Access Denied");
                }

                DateValidation(FromDate, true, "FromDate");
                DateValidation(ToDate, true, "ToDate");


                string lSQL = "";
                lSQL = string.Format("exec spAlborz_SANJESH_HIGHPOSTOFF '{0}', '{1}' ", FromDate, ToDate);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spAlborz_SANJESH_HIGHPOSTOFF IS SUCCESS"));

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spAlborz_SANJESH_HIGHPOSTOFF IS ERROR"));
            }

            return lDs;
        }

        // آخرین وضعیت پرونده مشترک با توجه به شماره پیگیری - آذربایجان غربی
        [WebMethod]
        public SubscriberOutageStateInfo GetSubscriberOutageStateInfo(string Code, string TypeId = "1", string EventTypeId = "-1")
        {
            SaveLog(String.Format("Calling GetSubscriberOutageStateInfo (Code={0} , TypeId={1}, EventTypeId={2})", Code, TypeId, EventTypeId));
            SubscriberOutageStateInfo lSubOutageInfo = new SubscriberOutageStateInfo();
            lSubOutageInfo.IsSuccess = false;
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                string lSQL = "";
                string lCodeName = "کد پيگيري";
                if (TypeId == "1")
                {
                    long lTrackingCode; long.TryParse(Code, out lTrackingCode);
                    if (lTrackingCode == 0)
                        throw new Exception("کد پیگیری ارسال شده، معتبر نیست");
                    lSQL = string.Format(
                        "EXEC spGetSubscriberOutageStateInfoByTrackingCode @aTrackingCode = '{0}'", Code);
                    SaveLog("Executing query: " + lSQL);
                }
                else if (TypeId == "2")
                {
                    lCodeName = "شماره تلفن";
                    long lTelNo; long.TryParse(Code, out lTelNo);
                    if (lTelNo == 0)
                        throw new Exception("شماره تلفن ارسال شده، معتبر نیست");
                    lSQL = string.Format(
                        "EXEC spGetSubscriberOutageStateInfoByTelNo @CallerId = '{0}', @EventTypeId = '{1}' ", Code, EventTypeId);
                    SaveLog("Executing query: " + lSQL);
                }
                else
                    throw new Exception("پارامتر TypeId، معتبر نیست");

                string lMsg = mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "ViewOutageInfo", aIsClearTable: true);
                if (lDs.Tables.Contains("ViewOutageInfo") && lDs.Tables["ViewOutageInfo"].Rows.Count > 0)
                {
                    DataRow lRow = lDs.Tables["ViewOutageInfo"].Rows[0];

                    if (lRow["RequestId"] == DBNull.Value)
                        throw new Exception(lCodeName + " ارسال شده، معتبر نیست");

                    lSubOutageInfo.Name = lRow["SubscriberName"].ToString();
                    lSubOutageInfo.Address = lRow["Address"].ToString();
                    lSubOutageInfo.Telephone = lRow["Telephone"].ToString();
                    lSubOutageInfo.Mobile = lRow["Mobile"].ToString();
                    lSubOutageInfo.CallReason = lRow["CallReason"].ToString();
                    lSubOutageInfo.State = lRow["EndJobState"].ToString();
                    lSubOutageInfo.EzamDate = lRow["EzamDate"].ToString();
                    lSubOutageInfo.AreaId = lRow["AreaId"].ToString();
                    lSubOutageInfo.Area = lRow["Area"].ToString();
                }
                else
                    throw new Exception(lMsg);

                lSubOutageInfo.IsSuccess = true;
                lSubOutageInfo.ErrorMessage = "";
            }
            catch (Exception ex)
            {
                lSubOutageInfo.IsSuccess = false;
                lSubOutageInfo.ErrorMessage = ex.Message;
                SaveLog(String.Format("Error GetSubscriberOutageStateInfo (ErrorMessage={0})", ex.ToString()));
            }

            SaveLog(string.Format("GetSubscriberOutageStateInfo Result (Code={0},TypeId={1},EventTypeId={2}): Json Result={3}", Code, TypeId, EventTypeId, mdl_Publics.GetJSonString(lSubOutageInfo)));

            return lSubOutageInfo;
        }


        // گیلان - فهرست اطلاعات پايه پستهاي توزيع
        [WebMethod]
        public DataSet Danesh_GetLPPostBaseInfo(int aAreaId, int aMPFeederId)
        {
            SaveLog(String.Format("Calling Danesh_GetLPPostBaseInfo (AreaId={0} , MPFeederId={1})", aAreaId, aMPFeederId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPPostInfo {0}, {1} ", aAreaId, aMPFeederId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPPostInfo IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLPPostInfo IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست اطلاعات پايه فیدرهای فشار ضعیف
        [WebMethod]
        public DataSet Danesh_GetLPFeederBaseInfo(int aAreaId, int aLPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLPFeederInfo (AreaId={0} , LPPostId={1})", aAreaId, aLPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPFeederInfo {0}, {1} ", aAreaId, aLPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPFeederInfo IS SUCCESS"));

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLPFeederInfo IS ERROR"));

            }

            return lDs;
        }

        // گیلان - فهرست اطلاعات پايه فیدرهای فشار متوسط
        [WebMethod]
        public DataSet Danesh_GetMPFeederBaseInfo(int aAreaId, int aMPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetMPFeederInfo (AreaId={0} , MPPostId={1})", aAreaId, aMPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetMPFeederInfo {0}, {1} ", aAreaId, aMPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetMPFeederInfo IS SUCCESS"));

            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetMPFeederInfo IS ERROR"));

            }

            return lDs;
        }

        // گیلان - فهرست اطلاعات بارگيري پستهای توزیع
        [WebMethod]
        public DataSet Danesh_GetLPPostLoad(string aFromDate, string aToDate, int aAreaId, int aLPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLPPostLoad (FromDate={0}, ToDate = {1}, AreaId={2} , LPPostId={3})", aFromDate, aToDate, aAreaId, aLPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPPostLoad '{0}', '{1}', {2},{3} ", aFromDate, aToDate, aAreaId, aLPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPPostLoad IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLPPostLoad IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست اطلاعات ولتاژگيري پستهای توزیع
        [WebMethod]
        public DataSet Danesh_GetLPPostVoltage(string aFromDate, string aToDate, int aAreaId, int aLPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLPPostVoltage (FromDate={0}, ToDate = {1}, AreaId={2} , LPPostId={3})", aFromDate, aToDate, aAreaId, aLPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPPostVoltage '{0}', '{1}', {2},{3} ", aFromDate, aToDate, aAreaId, aLPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPPostVoltage IS SUCCESS"));

            }
            catch (Exception ex)
            {
                SaveLog(string.Format("Result spDanesh_GetLPPostVoltage IS ERROR"));
                lDs.Tables.Add(GetErrorTable(ex.Message));
            }

            return lDs;
        }

        // گیلان - فهرست اطلاعات بارگيري فيدرهاي فشار ضعيف
        [WebMethod]
        public DataSet Danesh_GetLPFeederLoad(string aFromDate, string aToDate, int aAreaId, int aLPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLPFeederLoad (FromDate={0}, ToDate = {1}, AreaId={2} , LPPostId={3})", aFromDate, aToDate, aAreaId, aLPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPFeederLoad '{0}', '{1}', {2},{3} ", aFromDate, aToDate, aAreaId, aLPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPFeederLoad IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLPFeederLoad IS ERROR"));
            }



            return lDs;
        }

        // گیلان - فهرست اطلاعات ولتاژگيري فيدرهاي فشار ضعيف
        [WebMethod]
        public DataSet Danesh_GetLPFeederVoltage(string aFromDate, string aToDate, int aAreaId, int aLPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLPFeederVoltage (FromDate={0}, ToDate = {1}, AreaId={2} , LPPostId={3})", aFromDate, aToDate, aAreaId, aLPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPFeederVoltage '{0}', '{1}', {2},{3} ", aFromDate, aToDate, aAreaId, aLPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPFeederVoltage IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLPFeederVoltage IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست اطلاعات بارگیری فیدرهای فشار متوسط
        [WebMethod]
        public DataSet Danesh_GetMPFeederPeak(string aFromDate, string aToDate, int aAreaId, int aMPFeederId)
        {
            SaveLog(String.Format("Calling spDanesh_GetMPFeederPeak (FromDate={0}, ToDate = {1}, AreaId={2} , MPFeederId={3})", aFromDate, aToDate, aAreaId, aMPFeederId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetMPFeederPeak '{0}', '{1}', {2},{3} ", aFromDate, aToDate, aAreaId, aMPFeederId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetMPFeederPeak IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetMPFeederPeak IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست خاموشیهای بابرنامه
        [WebMethod]
        public DataSet Danesh_GetTamirRequest(string aFromDate, string aToDate, int aAreaId, int aMPFeederId)
        {
            SaveLog(String.Format("Calling spDanesh_GetTamirRequest (FromDate={0}, ToDate = {1}, AreaId={2} , MPFeederId={3})", aFromDate, aToDate, aAreaId, aMPFeederId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetTamirRequest '{0}', '{1}', {2},{3} ", aFromDate, aToDate, aAreaId, aMPFeederId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetTamirRequest IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetTamirRequest IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست خاموشیهای روشنایی معابر
        [WebMethod]
        public DataSet Danesh_GetLightRequest(string FromDate, string ToDate, int AreaId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLightRequest (FromDate={0}, ToDate = {1}, AreaId={2})", FromDate, ToDate, AreaId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLightRequest '{0}', '{1}', {2} ", FromDate, ToDate, AreaId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLightRequest IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLightRequest IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست قطعات مصرفی خاموشیهای فشار ضعیف
        [WebMethod]
        public DataSet Danesh_GetLPUsePart(string FromDate, string ToDate, int AreaId, int LPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLPUsePart (FromDate={0}, ToDate = {1}, AreaId={2}, LPPostId={3})", FromDate, ToDate, AreaId, LPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLPUsePart '{0}', '{1}', {2}, {3} ", FromDate, ToDate, AreaId, LPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLPUsePart IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLPUsePart IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست قطعات مصرفی خاموشیهای فشار متوسط
        [WebMethod]
        public DataSet Danesh_GetMPUsePart(string FromDate, string ToDate, int AreaId, int MPFeederId)
        {
            SaveLog(String.Format("Calling spDanesh_GetMPUsePart (FromDate={0}, ToDate = {1}, AreaId={2}, MPFeederId={3})", FromDate, ToDate, AreaId, MPFeederId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetMPUsePart '{0}', '{1}', {2}, {3} ", FromDate, ToDate, AreaId, MPFeederId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetMPUsePart IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetMPUsePart IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست قطعات مصرفی خاموشیهای روشنایی معابر
        [WebMethod]
        public DataSet Danesh_GetLightUsePart(string FromDate, string ToDate, int AreaId, int LPPostId)
        {
            SaveLog(String.Format("Calling spDanesh_GetLightUsePart (FromDate={0}, ToDate = {1}, AreaId={2}, LPPostId={3})", FromDate, ToDate, AreaId, LPPostId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetLightUsePart '{0}', '{1}', {2}, {3} ", FromDate, ToDate, AreaId, LPPostId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetLightUsePart IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetLightUsePart IS ERROR"));
            }

            return lDs;
        }

        // گیلان - فهرست وصلهای چند مرحله ای
        [WebMethod]
        public DataSet Danesh_GetMultiStepConnection(string FromDate, string ToDate, int AreaId, int MPFeederId)
        {
            SaveLog(String.Format("Calling spDanesh_GetMultiStepConnection (FromDate={0}, ToDate = {1}, AreaId={2}, MPFeederId={3})", FromDate, ToDate, AreaId, MPFeederId));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.Gilan))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spDanesh_GetMultiStepConnection '{0}', '{1}', {2}, {3} ", FromDate, ToDate, AreaId, MPFeederId);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true);
                SaveLog(string.Format("Result spDanesh_GetMultiStepConnection IS SUCCESS"));
            }
            catch (Exception ex)
            {
                lDs.Tables.Add(GetErrorTable(ex.Message));
                SaveLog(string.Format("Result spDanesh_GetMultiStepConnection IS ERROR"));
            }

            return lDs;
        }

		// چهار محال و بختیاری ـ فهرست تمامی خاموشی‌ها
		[WebMethod]
		public DataSet GetOutages(string FromDate, string ToDate, string AreaId)
		{
			SaveLog(String.Format("Calling GetOutages (FromDate={0}, ToDate = {1}, AreaId={2})", FromDate, ToDate, AreaId));
			SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
			DataSet lDs = new DataSet("OutageList");

			try
			{
				if (!AccessManager.IsAccess(AccessManager.AccessCodes.ChaharMahal))
					throw new Exception("RPT - Access Denied");

				int lAreaId; int.TryParse(AreaId, out lAreaId);
				if (lAreaId <= 0) lAreaId = 0;

				if (string.IsNullOrWhiteSpace(FromDate) || string.IsNullOrWhiteSpace(ToDate))
					throw new Exception("Please specify <FromDate> and <ToDate> parameters.");
				
				DateValidation(FromDate, false, "FromDate");
				DateValidation(ToDate, false, "ToDate");

				string lSQL = "";
				lSQL = string.Format("exec spChaharMahal_GetOutages '{0}', '{1}', {2} ", FromDate, ToDate, lAreaId);
				string lErr = mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "OutageItem", aIsClearTable: true, aIsShowError: true);
				SaveLog(string.Format("Result spChaharMahal_GetOutages SUCCESS"));
			}
			catch (Exception ex)
			{
				lDs.Tables.Add(GetErrorTable(ex.Message));
				SaveLog(string.Format("Result spChaharMahal_GetOutages ERROR: {0}", ex.Message));
			}

			return lDs;
		}

		// اهواز ـ فهرست خاموشی‌های بابرنامه 
		[WebMethod]
		public DataSet GetPlannedOutages(string FromDate, string ToDate, string AreaId)
		{
			SaveLog(String.Format("Calling GetPlannedOutages (FromDate={0}, ToDate = {1}, AreaId={2})", FromDate, ToDate, AreaId));
			SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
			DataSet lDs = new DataSet("OutageList");

			try
			{
				if (!AccessManager.IsAccess(AccessManager.AccessCodes.Ahvaz))
					throw new Exception("RPT - Access Denied");

				int lAreaId; int.TryParse(AreaId, out lAreaId);
				if (lAreaId <= 0) lAreaId = 0;

				if (string.IsNullOrWhiteSpace(FromDate) || string.IsNullOrWhiteSpace(ToDate))
					throw new Exception("Please specify <FromDate> and <ToDate> parameters.");
				
				DateValidation(FromDate, false, "FromDate");
				DateValidation(ToDate, false, "ToDate");

				string lSQL = "";
				lSQL = string.Format("exec spAhvaz_GetPlannedOutages '{0}', '{1}', {2} ", FromDate, ToDate, lAreaId);
				string lErr = mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "OutageItem", aIsClearTable: true, aIsShowError: true);
				SaveLog(string.Format("Result spAhvaz_GetPlannedOutages SUCCESS"));
			}
			catch (Exception ex)
			{
				lDs.Tables.Add(GetErrorTable(ex.Message));
				SaveLog(string.Format("Result spAhvaz_GetPlannedOutages ERROR: {0}", ex.Message));
			}

			return lDs;
		}

        // غرب مازندران - فهرست بارگيري هاي پست توزيع
        [WebMethod]
        public Danesh_Result GMaz_GetLPPostLoad(string LocalCode, string LPPostCode, string FromDate, string ToDate, int? Count)
        {
            Danesh_Result lRes = new Danesh_Result();
            SaveLog(String.Format("Calling spGMaz_GetLPPostLoad (LocalCode={0}, LPPostCode = {1}, FromDate={2}, ToDate={3}, Count={4})", LocalCode, LPPostCode, FromDate, ToDate, Count));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            lRes.IsSuccess = true;
            lRes.ErrorMessage = "";
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.GharbMazandaran))
                {
                    throw new Exception("RPT - Access Denied");
                }

                string lSQL = "";
                lSQL = string.Format("exec spGMaz_GetLPPostLoad '{0}', '{1}', '{2}', '{3}', {4} ", LocalCode, LPPostCode, FromDate, ToDate, Count);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true, aIsShowError: true);
                SaveLog(string.Format("Result spGMaz_GetLPPostLoad IS SUCCESS"));
                lRes.Data = mdl_Publics.GetClassFromJson<object>(mdl_Publics.GetJSonString(lDs.Tables["Result"]));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
                SaveLog(string.Format("Result spGMaz_GetLPPostLoad IS ERROR"));
            }

            return lRes;
        }
        // غرب مازندران ـ ایجاد وب سرویس متناظر گزارش 14-1 در نرم افزار
        [WebMethod]
        public Danesh_Result GMaz_Report_14_1(string FromDate, string ToDate)
        {
            Danesh_Result lRes = new Danesh_Result();
            SaveLog(String.Format("Calling spGMaz_Report_14_1 (FromDate={0}, ToDate={1})", FromDate, ToDate));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            lRes.IsSuccess = true;
            lRes.ErrorMessage = "";
            try
            {
              if (!AccessManager.IsAccess(AccessManager.AccessCodes.GharbMazandaran))
                    throw new Exception("RPT - Access Denied");
                string lSQL = string.Format("exec spGMaz_Report_14_1 '{0}', '{1}'", FromDate, ToDate);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true, aIsShowError: true);
                SaveLog(string.Format("Result spGMaz_Report_14_1 IS SUCCESS"));
                lRes.Data = mdl_Publics.GetClassFromJson<object>(mdl_Publics.GetJSonString(lDs.Tables["Result"]));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
                SaveLog(string.Format("Result spGMaz_GetLPPostLoad IS ERROR"));
            }
            return lRes;
        }
        // غرب مازندران ـ ایجاد وب سرویس گزارش فیدرهای ناپایدار
        [WebMethod]
        public Danesh_Result GMaz_Unstable_Feeders(string FromDate, string ToDate, int MinCount)
        {
            Danesh_Result lRes = new Danesh_Result();
            SaveLog(String.Format("Calling spGMaz_Report_14_1 (FromDate={0}, ToDate={1} , MinCount={2})", FromDate, ToDate, MinCount));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            lRes.IsSuccess = true;
            lRes.ErrorMessage = "";
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.GharbMazandaran))
                      throw new Exception("RPT - Access Denied");

                string lSQL = string.Format("exec spGMaz_GetUnstableFeeders '{0}', '{1}' , {2}", FromDate, ToDate, MinCount);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true, aIsShowError: true);
                SaveLog(string.Format("Result spGMaz_GetUnstableFeeders IS SUCCESS"));
                lRes.Data = mdl_Publics.GetClassFromJson<object>(mdl_Publics.GetJSonString(lDs.Tables["Result"]));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
                SaveLog(string.Format("Result spGMaz_GetUnstableFeeders IS ERROR"));
            }
            return lRes;
        }
        // آذربایجان غربی ـ ایجاد وب سرویس دریافت فهرست پیک بار ماهانه
        [WebMethod]
        public Danesh_Result AzarGH_FeederPeak_Monthly(string FromDate, string ToDate, string FeederCode)
        {
            Danesh_Result lRes = new Danesh_Result();
            SaveLog(String.Format("Calling spGetReport_FeederPeak (FeederCode={0}, FromDate={1}, ToDate={2})",FeederCode, FromDate, ToDate));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            lRes.IsSuccess = true;
            lRes.ErrorMessage = "";
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.AzGharbi))
                    throw new Exception("RPT - Access Denied");

                string lSQL = string.Format("exec spGetReport_FeederPeak '{0}', '{1}' , {2}", FeederCode, FromDate, ToDate);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Result", aIsClearTable: true, aIsShowError: true);
                SaveLog(string.Format("Result spGetReport_FeederPeak IS SUCCESS"));
                lRes.Data = mdl_Publics.GetClassFromJson<object>(mdl_Publics.GetJSonString(lDs.Tables["Result"]));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
                SaveLog(string.Format("Result spGetReport_FeederPeak IS ERROR"));
            }
            return lRes;
        }
        // آذربایجان غربی ـ ایجاد وب سرویس دریافت فهرست بارگیری های چند دوره اخیر پست و فیدر
        [WebMethod]
        public Danesh_Result AzarGH_PostFeederLoad(string FromDate, string ToDate, string PostCode, int MinCount)
        {
            Danesh_Result lRes = new Danesh_Result();
            SaveLog(String.Format("Calling spGetReport_PostFeederLoad (FeederCode={0}, FromDate={1}, ToDate={2})", PostCode, FromDate, ToDate));
            SqlConnection lCnn = new SqlConnection(mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet();

            lRes.IsSuccess = true;
            lRes.ErrorMessage = "";
            try
            {
                if (!AccessManager.IsAccess(AccessManager.AccessCodes.AzGharbi))
                    throw new Exception("RPT - Access Denied");

                string lSQL = string.Format("exec spGetReport_PostFeederLoad '{0}', '{1}' , '{2}', {3}, {4}",
                    PostCode, FromDate, ToDate , MinCount , 1);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "PostLoad", aIsClearTable: true, aIsShowError: true);
                lSQL = string.Format("exec spGetReport_PostFeederLoad '{0}', '{1}' , '{2}', {3}, {4}",
                    PostCode, FromDate, ToDate, MinCount, 0);
                mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "FeederLoad", aIsClearTable: true, aIsShowError: true);
                SaveLog(string.Format("Result spGetReport_PostFeederLoad IS SUCCESS"));
                lRes.Data = mdl_Publics.GetClassFromJson<object>(mdl_Publics.GetJSonString(lDs.Tables["Result"]));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
                SaveLog(string.Format("Result spGetReport_PostFeederLoad IS ERROR"));
            }
            return lRes;
        }
    }

	#region Public Classes
	public class Danesh_Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
    public class Danesh_BasePostList
    {
        public List<Danesh_BasePost> DataList { get; set; }
    }
    public class Danesh_BasePost
    {
        public string LPPostName { get; set; }
        public string LPPostId { get; set; }
        public string LPPostCode { get; set; }
        public string MPFeederName { get; set; }
        public string MPFeederId { get; set; }
        public string MPFeederCode { get; set; }
        public string MPPostName { get; set; }
        public string MPPostId { get; set; }
        public string MPPostCode { get; set; }
        public string PostCapacity { get; set; }
        public string Ownership { get; set; }
        public string AreaId { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public bool IsHavayi { get; set; }
        public bool IsActive { get; set; }
    }

	#endregion
}
