using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using TZServiceTools;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Globalization;
using TZServicesLib;

namespace TZServicesCSharp
{
    /// <summary>
    /// Summary description for HavadesTamir
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HavadesTamir : System.Web.Services.WebService
    {
        Boolean mIsTamirPowerMonthly = false;
        public enum TamirRequestStates
        {
            trs_None = -1,
            trs_PreNew = 0,
            trs_WaitFor121 = 1,
            trs_WaitForSetad = 2,
            trs_Confirm = 3,
            trs_Allowed = 4,
            trs_Disconnected = 5,
            trs_Finish = 6,
            trs_NotDone = 7,
            trs_NotConfirm = 8,
            
        }
        public enum TamirNetworkTypes
        {
            tnt_MP=2,
            tnt_LP=3
        }
        public enum TamirTypes
        {
            tt_BarnameriziShodeh = 1,
            tt_BaMovafeghat = 2,
            tt_Ezterari =3
        }
      
        [WebMethod]
        // لیست نواحی به جز مراکز 121
        public DataSet GetAreaList()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("AreaList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " Tbl_Area.AreaId, Tbl_Area.Area " +
                    " from " +
                        " Tbl_Area " +
                    " where " +
                        " Tbl_Area.IsCenter = 0";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Area");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        //لیست پست‌های فوق توزیع
        public DataSet GetMPPostList(int AreaId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("MPPostList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " MPPostId, MPPostName " +
                    " from " +
                        " Tbl_MPPost " +
                     " where IsActive = 1 " +
                        " AND AreaId = " + AreaId;

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_MPPost");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        // لیست فیدرهای فشار متوسط
        public DataSet GetMPFeederList(int AreaId, int MPPostId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("MPFeederList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " MPFeederId, MPFeederName " +
                    " from " +
                        " Tbl_MPFeeder " +
                     " where IsActive = 1 " +
                        " AND AreaId = " + AreaId +
                        " AND  MPPostId = " + MPPostId;

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_MPFeeder");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        // لیست تکه فیدرها
        public DataSet GetFeederPartList(int AreaId, int MPFeederId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("FeederPartList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " FeederPartId, FeederPart AS FeederPartName " +
                    " from " +
                        " Tbl_FeederPart " +
                     " where IsActive = 1 " +
                        " AND AreaId = " + AreaId +
                        " AND MPFeederId = " + MPFeederId;

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_FeederPart");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        // ليست پست‌هاي توزيع
        public DataSet GetLPPostList(int AreaId, int MPFeederId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("LPPostList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " LPPostId, LPPostName " +
                    " from " +
                        " Tbl_LPPost " +
                     " where IsActive = 1 " +
                        " AND AreaId = " + AreaId +
                        " AND MPFeederId = " + MPFeederId;

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_LPPost");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        //ليست فيدرهاي فشار ضعيف
        public DataSet GetLPFeederList(int AreaId, int LPPostId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("LPFeederList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " LPFeederId, LPFeederName " +
                    " from " +
                        " Tbl_LPFeeder " +
                    " where IsActive = 1 " +
                        " AND AreaId = " + AreaId +
                        " AND LPPostId = " + LPPostId ;

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_LPFeeder");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        //لیست ناظر
        public DataSet GetNazerList()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("NazerList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " NazerId, NazerName " +
                    " from " +
                        " Tbl_Nazer ";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Nazer");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        //لیست پیمانکار
        public DataSet GetPeymankarList(int AreaId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("PeymankarList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "SELECT PeymankarId, PeymankarName " +
                    "FROM Tbl_Peymankar " +
                    "WHERE IsActive = 1 AND (AreaId = " + AreaId + " OR AreaId IS NULL)";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Peymankar");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        // لیست  درخواست کننده
        public DataSet GetDepartmentList()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("DepartmentList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " DepartmentId, Department " +
                    " from " +
                        " Tbl_Department " +
                     " where IsActive = 1 " ;

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Department");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        //لیست اقدام کننده
        public DataSet GetReferToList()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("ReferToList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " ReferToId, ReferTo " +
                    " from " +
                        " Tbl_ReferTo ";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_ReferTo");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        // نوع شبکه
        public DataSet GetTamirNetworkTypeList()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("TamirNetworkTypeList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " TamirNetworkTypeId, TamirNetworkType " +
                    " from " +
                        " Tbl_TamirNetworkType " +
                     " where IsActive = 1 ";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_TamirNetworkType");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        // لیست انواع مانور
        public DataSet GetManoeuvreTypeList()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("ManoeuvreTypeList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " ManoeuvreTypeId, ManoeuvreType " +
                    " from " +
                        " Tbl_ManoeuvreType ";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_ManoeuvreType");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;

        }

        [WebMethod]
        //لیست عملیات خاموشی
        public DataSet GetTamirOperationList(int TamirNetworkTypeId)
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("TamirOperationList");
            DataTable lTbl;
            string lSql;
            try
            {
                lSql =
                    "select " +
                        " TamirOperationId, TamirOperation " +
                    " from " +
                        " Tbl_TamirOperation " +
                     " where (TamirNetworkTypeId Is Null OR TamirNetworkTypeId = " + TamirNetworkTypeId + ")";

                TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_TamirOperation");
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_AVL_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }

        [WebMethod]
        //لیست ناحیه ها به همراه سقف خاموشی
        public DataSet GetEnergyQuota()
        {
            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSet lDs = new DataSet("AreaList");
            DataTable lTbl;
            string lSql;
            try
            {
                Boolean lIsServer121 = Convert.ToBoolean(CConfig.ReadConfig("IsTamirServer121", false));
                if (lIsServer121)
                {
                    lSql = " SELECT Tbl_TamirInfo.TamirInfoId, Tbl_Server121.Server121Id, Tbl_Server121.Server121, " +
                        " ISNULL(dbo.Tbl_TamirInfo.MaxWeakPower, 0) AS QuotaValue  FROM Tbl_Server121 LEFT OUTER JOIN Tbl_TamirInfo ON " +
                        " Tbl_Server121.Server121Id = Tbl_TamirInfo.Server121Id AND Tbl_TamirInfo.AreaId IS NULL ";
                    TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                    TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Server121");
                }
                else
                {
                    lSql = "SELECT Tbl_TamirInfo.TamirInfoId, Tbl_Server121.Server121Id, Tbl_Area.AreaId, " +
                        " Tbl_Area.Area,ISNULL(dbo.Tbl_TamirInfo.MaxWeakPower, 0) AS QuotaValue" +
                        " FROM Tbl_Area " +
                        " LEFT OUTER JOIN Tbl_TamirInfo ON Tbl_Area.AreaId = Tbl_TamirInfo.AreaId" +
                        " LEFT OUTER JOIN Tbl_Server121 ON Tbl_Area.Server121Id = Tbl_Server121.Server121Id" ;
                    TZServiceTools.mdl_Publics.RemoveMoreSpaces(ref lSql);
                    TZServiceTools.mdl_Publics.BindingTable(lSql, ref lCnn, lDs, "Tbl_Area");
                }
               
              
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_AVL_Logs");
                lTbl = new DataTable("Tbl_Error");
                lTbl.Columns.Add("ErrorMessage");
                lTbl.Rows.Add(ex.Message);
                lDs.Tables.Add(lTbl);
            }
            return lDs;
        }
        [WebMethod]
        //دریافت اطلاعات ثبت خاموشی
        public String SetTamirRequestInfo(long TamirId, TamirInfo TamirRequestInfo)
        {
            string lMsg = "";
            string lNType = "";
            string lErr = "";

            TZDataSet.TblResultDataTable lTblResult = TZServicesLib.CFunctions.GetPublicResultDataTable();
            TZDataSet.TblResultRow lRowResult = lTblResult[0];

            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSetTamir lDs = new DataSetTamir();
            DataSetTamir.TblTamirRequestDataTable lTblTamirRequest = null;
            DataSetTamir.TblTamirRequestRow lTamirRow = null;
            DataSetTamir.TblTamirOperationListDataTable lTblTamirOperationList=null;
            DataSetTamir.TblTamirOperationListRow lTamirOperationListRow=null;

            frmUpdateDataset lUpdate = new frmUpdateDataset();

            TamirInfo lDCSaveInfo = TamirRequestInfo;

            SqlTransaction lTrans = null;

            try
            {
                string lAccess = System.Configuration.ConfigurationManager.AppSettings["HavadesTamirAccess"];
                if (lAccess == null) lAccess = "false";
                if (lAccess.ToLower() != "true")
                {
                    throw new Exception("Tamir - Access Denied");
                }
                if (TamirId > 0)
                {
                    string lSSQL = "SELECT * From TblTamirRequest WHERE TamirRequestId = " + TamirId;
                    TZServiceTools.mdl_Publics.BindingTable(lSSQL, ref lCnn, lDs, "TblTamirRequest");

                    if (lDs.Tables["TblTamirRequest"].Rows.Count == 0)
                        throw new Exception("TamirId is invalid.");
                    else 
                        if( lDs.TblTamirRequest[0].TamirRequestStateId != (int)TamirRequestStates.trs_PreNew )
                            throw new Exception("This request is sent to confirm or canceled and cannot be edit.");

                    lTamirRow = lDs.TblTamirRequest[0];

                    lSSQL = "SELECT * FROM TblTamirOperationList WHERE TamirRequestId = " + TamirId;
                    TZServiceTools.mdl_Publics.BindingTable(lSSQL, ref lCnn, lDs, "TblTamirOperationList");
               }

                lMsg = IsSaveTamirOK(lCnn, lDCSaveInfo);
                if (lMsg.Length > 0)
                    throw new Exception(lMsg);

                lErr = CheckDCSaveInfo(lDCSaveInfo, ref lCnn);
                if (lErr.Length > 0)
                    throw new Exception(lErr);

                lTblTamirRequest = lDs.TblTamirRequest;
                if (lTamirRow == null)
                    lTamirRow = lDs.TblTamirRequest.NewTblTamirRequestRow();

                lTamirRow.AreaId = TamirRequestInfo.AreaId;

                string lSQL = "SELECT CityId FROM Tbl_Area WHERE AreaId = " + lTamirRow.AreaId;
                TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_Area");

                if (lDs.Tables["Tbl_Area"].Rows.Count == 0)
                    throw new Exception("AreaId is invalid.");

                lTamirRow.CityId =Int32.Parse(lDs.Tables["Tbl_Area"].Rows[0]["CityId"].ToString() );
                lTamirRow.TamirTypeId = (int)TamirTypes.tt_BaMovafeghat;

                if (TamirRequestInfo.NazerId > 0)
                    lTamirRow.NazerId = TamirRequestInfo.NazerId;
                lTamirRow.PeymankarId = TamirRequestInfo.PeymankarId;
                lTamirRow.DepartmentId = TamirRequestInfo.DepartmentId;
                lNType = TamirRequestInfo.NetworkType.ToUpper();
                if (lNType == "MP")
                    lTamirRow.TamirNetworkTypeId =(int)TamirNetworkTypes.tnt_MP;
                if (lNType == "LP")
                    lTamirRow.TamirNetworkTypeId =(int)TamirNetworkTypes.tnt_LP;
 
                DateTime lKDT = GetDT(TamirRequestInfo.DisconnectDate, TamirRequestInfo.DisconnectTime);
                lTamirRow.DisconnectDT = lKDT;
                lTamirRow.DisconnectDatePersian = TamirRequestInfo.DisconnectDate;
                lTamirRow.DisconnectTime = TamirRequestInfo.DisconnectTime;

                DateTime lDT = GetDT(TamirRequestInfo.ConnectDate, TamirRequestInfo.ConnectTime);
                lTamirRow.ConnectDT = lDT;
                lTamirRow.ConnectDatePersian = TamirRequestInfo.ConnectDate;
                lTamirRow.ConnectTime = TamirRequestInfo.ConnectTime;

                if (lTamirRow.DisconnectDT >= DateTime.Now.AddDays(365))
                    throw new Exception("DisconnectDate is invalid.");
                if (lTamirRow.DisconnectDT < DateTime.Now)
                    throw new Exception("DisconnectDate is invalid.");
                int DateDiff;
                DateDiff = int.Parse((((lTamirRow.ConnectDT - lTamirRow.DisconnectDT).Days).ToString()));
                if (DateDiff >= 7)
                    throw new Exception("Date Difference between DisconnectDate and ConnectDate must be less than 7 days");

                lTamirRow.DisconnectInterval = Convert.ToInt32((lTamirRow.ConnectDT - lTamirRow.DisconnectDT).TotalMinutes);

                if(lTamirRow.DisconnectInterval<=0)
                    throw new Exception("ConnectDate must be after DisconnectDate ");
                
                lTamirRow.DisconnectPower = 0.0;
                lTamirRow.AreaUserId = mdl_Publics.Find_AVL_User(lCnn);

                TZServiceTools.PersianDate.KhayamDateTime lKh_DE = new PersianDate.KhayamDateTime();
                DateTime lDT_DE = DateTime.Now;
                lKh_DE.SetMiladyDate(lDT_DE);
                lTamirRow.DEDT = lDT_DE;
                lTamirRow.DEDatePersian = lKh_DE.GetShamsiDateStr();
                lTamirRow.DETime = String.Format("{0}:{1}", lDT_DE.Hour.ToString("0#"), lDT_DE.Minute.ToString("0#"));
                
                lTamirRow.IsWatched = false;

                lTamirRow.MPPostId = TamirRequestInfo.MPPostId;
                lTamirRow.MPFeederId = TamirRequestInfo.MPFeederId;

                if (TamirRequestInfo.FeederPartId > 0 && TamirRequestInfo.NetworkType == "MP")
                    lTamirRow.FeederPartId = TamirRequestInfo.FeederPartId;

                if (TamirRequestInfo.LPPostId > 0 && TamirRequestInfo.FeederPartId > 0)
                {
                    lSQL = "SELECT * FROM Tbl_LPPost WHERE LPPostId = " + TamirRequestInfo.LPPostId + " AND FeederPartId = " + TamirRequestInfo.FeederPartId;
                    TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_LPPost");

                    if (lDs.Tables["Tbl_LPPost"].Rows.Count == 0)
                        throw new Exception("LPPostId and FeederPartId is not match.");
                }

                if (TamirRequestInfo.LPPostId > 0)
                    lTamirRow.LPPostId = TamirRequestInfo.LPPostId;

                if (TamirRequestInfo.LPFeederId > 0 && TamirRequestInfo.NetworkType == "LP" )
                    lTamirRow.LPFeederId = TamirRequestInfo.LPFeederId;

                lTamirRow.IsWarmLine = TamirRequestInfo.IsWarmLine == 0 ? false : true;
                lTamirRow.CurrentValue = TamirRequestInfo.CurrentValue;
                lTamirRow.WorkingAddress = TamirRequestInfo.WorkingAddress.Trim();
                if (TamirRequestInfo.GpsX > 0) lTamirRow.GpsX = TamirRequestInfo.GpsX;
                if (TamirRequestInfo.GpsY > 0) lTamirRow.GpsY = TamirRequestInfo.GpsY;
                lTamirRow.CriticalsAddress = TamirRequestInfo.CriticalsAddress.Trim();
                lTamirRow.IsInCityService = TamirRequestInfo.IsInCityService == 0 ? false : true;
                
                lTamirRow.IsManoeuvre = TamirRequestInfo.IsManoeuvre == 0 ? false : true;
                if (lTamirRow.IsManoeuvre)
                {
                    lTamirRow.ManoeuvreDesc = TamirRequestInfo.ManoeuvreDesc.Trim();
                    lTamirRow.ManoeuvreTypeId = TamirRequestInfo.ManoeuvreTypeId;
                }
                
                lTamirRow.SaturdayDT=GetFirstDateOfTamirScope(lTamirRow.DisconnectDT , mIsTamirPowerMonthly);
                lTamirRow.IsRequestByPeymankar = true;
                if (TamirRequestInfo.ReferToId > 0)
                    lTamirRow.ReferToId = TamirRequestInfo.ReferToId;
                
                lTamirRow.TamirRequestStateId = (int)TamirRequestStates.trs_PreNew;
                //lTblTamirRequest.Rows.Add(lTamirRow);
                //lTblTamirRequest.AddTblTamirRequestRow(lTamirRow);
                if (TamirId <= 0)
                {   
                    lTamirRow.TamirRequestId = mdl_Publics.GetAutoInc();
                    lTamirRow.TamirRequestNo = -1;
                    lDs.TblTamirRequest.Rows.Add(lTamirRow);
                }
                lTblTamirOperationList = lDs.TblTamirOperationList;

                if (TamirRequestInfo.OperationList != null)
                {
                    lSQL = "SELECT * from Tbl_TamirOperation";
                    TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_TamirOperation", -1, 0, true);
                    lErr = CheckTamirOperation(lDCSaveInfo, lDs.Tbl_TamirOperation);
                    if (lErr.Length > 0)
                        throw new Exception(lErr);
                }
                if (TamirRequestInfo.OperationList != null && TamirRequestInfo.OperationList.Length > 0)
                {
                    DataRow[] lRowOperationList = null;
                    
                    SortedList<int, int> lTimes = new SortedList<int, int>();

                    foreach (OperationInfo lOperationInfo in TamirRequestInfo.OperationList)
                    {                   
                        lRowOperationList = lTblTamirOperationList.Select("TamirOperationId = " + lOperationInfo.OperationId);
                        if (lRowOperationList.Length > 0)
                            lTamirOperationListRow = lRowOperationList[0] as DataSetTamir.TblTamirOperationListRow;
                        else
                        {
                            lTamirOperationListRow = lTblTamirOperationList.NewTblTamirOperationListRow();
                            lTamirOperationListRow.TamirOperationListId = mdl_Publics.GetIntAutoInc();
                        }
                        lTamirOperationListRow.TamirRequestId = lTamirRow.TamirRequestId;
                        lTamirOperationListRow.TamirOperationId = lOperationInfo.OperationId;
                        lTamirOperationListRow.OperationCount = lOperationInfo.OperationCount;
                        
                        if (lOperationInfo.GroupCount > 0)
                            lTamirOperationListRow.PeoplesCount = lOperationInfo.GroupCount;
                        else if (lOperationInfo.GroupCount ==0)
                            lTamirOperationListRow.SetPeoplesCountNull();

                        if (lOperationInfo.OperationGroupId > 0)
                            lTamirOperationListRow.TamirOperationGroupId = lOperationInfo.OperationGroupId;
                        else if (lOperationInfo.OperationGroupId == 0)
                            lTamirOperationListRow.SetTamirOperationGroupIdNull();

                        DataSetTamir.Tbl_TamirOperationRow[] lOprows = lDs.Tbl_TamirOperation.Select(
                            String.Format("TamirOperationId = {0}", lTamirOperationListRow.TamirOperationId)
                        ) as DataSetTamir.Tbl_TamirOperationRow[];
                        if (lOprows.Length > 0)
                        {
                            int lPeopleCount = 1;
                            if (!lTamirOperationListRow.IsPeoplesCountNull())
                                lPeopleCount = lTamirOperationListRow.PeoplesCount;
                            if (lPeopleCount <= 0) lPeopleCount = 1;

                            lTamirOperationListRow.Duration = (lTamirOperationListRow.OperationCount * lOprows[0].Duration / lPeopleCount);
                        }

                        if (lRowOperationList.Length == 0)
                            lTblTamirOperationList.AddTblTamirOperationListRow(lTamirOperationListRow);

                        int lGroupId = 0;
                        if (!lTamirOperationListRow.IsTamirOperationGroupIdNull())
                            lGroupId = lTamirOperationListRow.TamirOperationGroupId;
                        
                        if (!lTimes.ContainsKey(lGroupId))
                            lTimes.Add(lGroupId, 0);

                        lTimes[lGroupId] += lTamirOperationListRow.Duration;
                    }
                    int lMaxTime = 0;
                    foreach (int lDuration in lTimes.Values)
                        if (lDuration > lMaxTime)
                            lMaxTime = lDuration;

                    if (lTamirRow.DisconnectInterval > lMaxTime)
                        throw new Exception("DisconnectInterval must be less than Total Operations Duration");
                }
                if (lCnn.State != ConnectionState.Open)
                    lCnn.Open();
                lTrans = lCnn.BeginTransaction();

                lUpdate.UpdateDataSet("TblTamirRequest", lDs, lTamirRow.AreaId, -1, true, lTrans);

                System.Collections.ArrayList lDeletes = new System.Collections.ArrayList();
                foreach (DataSetTamir.TblTamirOperationListRow lRowOperation in lTblTamirOperationList)
                {
                    lRowOperation.TamirRequestId = lTamirRow.TamirRequestId;
                    if (lRowOperation.OperationCount == 0)
                        lDeletes.Add(lRowOperation);
                }

                foreach (DataSetTamir.TblTamirOperationListRow lRowOperation in lDeletes)
                    lRowOperation.Delete();

                lUpdate.UpdateDataSet("TblTamirOperationList", lDs, lTamirRow.AreaId, -1, true, lTrans);

                lTrans.Commit();

                lRowResult.ResultCode = (short)TZServicesLib.CFunctions.TZResultCodes.Success;
                lRowResult.ResultValue = lTamirRow.TamirRequestId.ToString();
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, ex.Message, "TZServices_Tamir_Logs");
                if (lTrans != null)
                    lTrans.Rollback();

                lRowResult.ResultCode = (short)TZServicesLib.CFunctions.TZResultCodes.Error;
                lRowResult.ResultValue = ex.Message;
            }
            finally
            {
                if (lCnn.State == ConnectionState.Open)
                    lCnn.Close();
            }
            String lResult = TZServicesLib.CFunctions.GetJsonDataTable(lTblResult);
            return lResult;
            //-----------flag----flag----
        }
        private static string IsSaveTamirOK(SqlConnection aCnn, TamirInfo aDCInfo)
        {
            String lResult = "";
            try
            {

                if (aDCInfo.AreaId <= 0) 
                    throw new Exception("AreaId is not valid.");
                if (aDCInfo.PeymankarId <= 0)
                    throw new Exception("PeymankarId is not valid.");
                if (aDCInfo.DepartmentId <= 0)
                    throw new Exception("DepartmentId is not valid.");
                if (!Regex.IsMatch(aDCInfo.NetworkType, "^(MP|LP)$", RegexOptions.IgnoreCase))
                    throw new Exception("NetworkType parameter is not valid.");
                if (aDCInfo.DisconnectDate.Trim().Length == 0)
                    throw new Exception("DisconnectDate is not valid.");
                else
                {
                    string lValidDate = mdl_Publics.ValidDate(aDCInfo.DisconnectDate);
                    if (lValidDate == "Error")
                        throw new Exception("DisconnectDate is not valid.");
                    else
                        aDCInfo.DisconnectDate = lValidDate;
                }
                if (aDCInfo.DisconnectTime.Trim().Length == 0)
                    throw new Exception("DisconnectTime is not valid.");
                else
                {
                    string lValidTime = mdl_Publics.ValidTime(aDCInfo.DisconnectTime);
                    if (lValidTime == "Error")
                        throw new Exception("DisconnectTime is not valid.");
                }
                if (aDCInfo.ConnectDate.Trim().Length == 0)
                    throw new Exception("ConnectDate is not valid.");
                else
                {
                    string lValidDate = mdl_Publics.ValidDate(aDCInfo.ConnectDate);
                    if (lValidDate == "Error")
                        throw new Exception("ConnectDate is not valid.");
                }
                if (aDCInfo.ConnectTime.Trim().Length == 0)
                    throw new Exception("ConnectTime is not valid.");
                else
                {
                    string lValidTime = mdl_Publics.ValidTime(aDCInfo.ConnectTime);
                    if (lValidTime == "Error")
                        throw new Exception("ConnectTime is not valid.");
                }
                if (aDCInfo.MPPostId <=0 )
                    throw new Exception("MPPostId is not valid.");
                if (aDCInfo.MPFeederId <= 0)
                    throw new Exception("MPFeederId is not valid.");
                if (aDCInfo.NetworkType == "LP" && aDCInfo.LPPostId <= 0)
                    throw new Exception("LPPostId is not valid");

                if (aDCInfo.IsWarmLine < 0 || aDCInfo.IsWarmLine > 1)
                    throw new Exception("IsWarmLine is not valid.");
                if (aDCInfo.IsWarmLine == 0 && aDCInfo.CurrentValue <= 0)
                    throw new Exception("CurrentValue is not valid.");
                if (aDCInfo.WorkingAddress.Trim().Length == 0)
                    throw new Exception("WorkingAddress is not valid.");
                if (aDCInfo.CriticalsAddress.Trim().Length == 0)
                    throw new Exception("CriticalsAddress is not valid.");
                if (aDCInfo.IsInCityService < 0 || aDCInfo.IsInCityService > 1)
                    throw new Exception("IsInCityService is not valid.");
                if (aDCInfo.IsManoeuvre < 0 || aDCInfo.IsManoeuvre > 1)
                    throw new Exception("IsManoeuvre is not valid.");  
                if(aDCInfo.IsManoeuvre == 1 && aDCInfo.ManoeuvreTypeId <= 0)
                    throw new Exception("ManoeuvreTypeId is not valid."); 

            }
            catch (Exception ex)
            {
                lResult = ex.Message;
            }
            return lResult;
        }
        private static string CheckDCSaveInfo(TamirInfo aDCInfo, ref SqlConnection aCnn)
        {
            string lErrMsg = "";
            DataSet lDs = new DataSet() ;
            try
            {
                string lSQL = "SELECT * FROM Tbl_";
                if (aDCInfo.NazerId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "Nazer WHERE NazerId = " + aDCInfo.NazerId, ref aCnn, lDs, "Tbl_Nazer");
                    if (lDs.Tables.Contains("Tbl_Nazer") && lDs.Tables["Tbl_Nazer"].Rows.Count == 0)
                        lErrMsg += " - NazerId is invalid";
                }
                if (aDCInfo.PeymankarId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "Peymankar WHERE PeymankarId = " + aDCInfo.PeymankarId, ref aCnn, lDs, "Tbl_Peymankar");
                    if (lDs.Tables.Contains("Tbl_Peymankar") && lDs.Tables["Tbl_Peymankar"].Rows.Count == 0)
                        lErrMsg += " - PeymankarId is invalid";
                }
                if (aDCInfo.DepartmentId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "Department WHERE DepartmentId = " + aDCInfo.DepartmentId, ref aCnn, lDs, "Tbl_Department");
                    if (lDs.Tables.Contains("Tbl_Department") && lDs.Tables["Tbl_Department"].Rows.Count == 0)
                        lErrMsg += " - DepartmentId is invalid";
                }
                if (aDCInfo.AreaId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "Area WHERE AreaId = " + aDCInfo.AreaId, ref aCnn, lDs, "Tbl_Area");
                    if (lDs.Tables.Contains("Tbl_Area") && lDs.Tables["Tbl_Area"].Rows.Count == 0)
                        lErrMsg += " - AreaId is invalid";
                }
                if (aDCInfo.LPFeederId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "LPFeeder WHERE LPFeederId = " + aDCInfo.LPFeederId, ref aCnn, lDs, "Tbl_LPFeeder");
                    if (lDs.Tables.Contains("Tbl_LPFeeder") && lDs.Tables["Tbl_LPFeeder"].Rows.Count == 0) 
                        lErrMsg += " - LPFeederId is invalid";
                }
                if (aDCInfo.LPPostId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "LPPost WHERE LPPostId = " + aDCInfo.LPPostId, ref aCnn, lDs, "Tbl_LPPost");
                    if (lDs.Tables.Contains("Tbl_LPPost") && lDs.Tables["Tbl_LPPost"].Rows.Count == 0)
                        lErrMsg += " - LPPostId is invalid";
                }
                if (aDCInfo.FeederPartId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "FeederPart WHERE FeederPartId = " + aDCInfo.FeederPartId, ref aCnn, lDs, "Tbl_FeederPart");
                    if (lDs.Tables.Contains("Tbl_FeederPart") && lDs.Tables["Tbl_FeederPart"].Rows.Count == 0)
                        lErrMsg += " - FeederPartId is invalid";
                }
                if (aDCInfo.MPFeederId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "MPFeeder WHERE MPFeederId = " + aDCInfo.MPFeederId, ref aCnn, lDs, "Tbl_MPFeeder");
                    if (lDs.Tables.Contains("Tbl_MPFeeder") && lDs.Tables["Tbl_MPFeeder"].Rows.Count == 0)
                        lErrMsg += " - MPFeederId is invalid";
                }
                if (aDCInfo.MPPostId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "MPPost WHERE MPPostId = " + aDCInfo.MPPostId, ref aCnn, lDs, "Tbl_MPPost");
                    if (lDs.Tables.Contains("Tbl_MPPost") &&  lDs.Tables["Tbl_MPPost"].Rows.Count == 0)
                        lErrMsg += " - MPPostId is invalid";
                }
                if (aDCInfo.ReferToId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "ReferTo WHERE ReferToId = " + aDCInfo.ReferToId, ref aCnn, lDs, "Tbl_ReferTo");
                    if (lDs.Tables.Contains("Tbl_ReferTo") && lDs.Tables["Tbl_ReferTo"].Rows.Count == 0)
                        lErrMsg += " - ReferToId is invalid";
                }
                if (aDCInfo.ManoeuvreTypeId > 0)
                {
                    TZServiceTools.mdl_Publics.BindingTable(lSQL + "ManoeuvreType WHERE ManoeuvreTypeId = " + aDCInfo.ManoeuvreTypeId, ref aCnn, lDs, "Tbl_ManoeuvreType");
                    if (lDs.Tables.Contains("Tbl_ManoeuvreType") && lDs.Tables["Tbl_ManoeuvreType"].Rows.Count == 0)
                        lErrMsg += " - ManoeuvreTypeId is invalid";
                }

            }
            catch (Exception)
            {
                throw;
            }

            if (lErrMsg.Length > 0)
                lErrMsg = "Network Error: (" + lErrMsg + ")";
            return lErrMsg;
        }

        [WebMethod]
        //لغو خاموشی درخواست شده
        public String CancelTamir(long TamirId, string CancelReason)
        {
            TZDataSet.TblResultDataTable lTblResult = TZServicesLib.CFunctions.GetPublicResultDataTable();
            TZDataSet.TblResultRow lRowResult = lTblResult[0];

            SqlConnection lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
            DataSetTamir lDs = new DataSetTamir();
            DataSetTamir.TblTamirRequestRow lRowTamir = null;

            DataSetRequest lDsReq = new DataSetRequest();
            DataSetRequest.TblRequestRow lRowRequest = null;

            SqlTransaction lTrans = null;
            frmUpdateDataset lUpdate = new frmUpdateDataset();

            TamirRequestStates lTamirState = TamirRequestStates.trs_None;
            long lRequestId = -1;

            try
            {
                string lAccess = System.Configuration.ConfigurationManager.AppSettings["HavadesTamirAccess"];
                if (lAccess == null) lAccess = "false";
                if (lAccess.ToLower() != "true")
                {
                    throw new Exception("Tamir - Access Denied");
                }

                if (TamirId <= 0)
                    throw new Exception("TamirId is invalid.");

                string lSQL = "SELECT * From TblTamirRequest WHERE TamirRequestId = " + TamirId;
                TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblTamirRequest");

                if (lDs.Tables["TblTamirRequest"].Rows.Count == 0)
                    throw new Exception("TamirId is invalid.");

                lRowTamir = lDs.TblTamirRequest[0];
                lTamirState = (TamirRequestStates)lRowTamir.TamirRequestStateId;

                if (lTamirState == TamirRequestStates.trs_Finish || lTamirState == TamirRequestStates.trs_Allowed)
                    throw new Exception("Outage is happened and cannot cancel it");

                if (lTamirState >= TamirRequestStates.trs_Confirm)
                {
                    lSQL = "SELECT * FROM TblTamirRequestConfirm WHERE TamirRequestId = " + TamirId;
                    TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "TblTamirRequestConfirm");
                    if (lDs.Tables.Contains("TblTamirRequestConfirm") && lDs.Tables["TblTamirRequestConfirm"].Rows.Count > 0)
                    {
                        lRequestId = (int)lDs.Tables["TblTamirRequestConfirm"].Rows[0]["RequestId"];
                        lSQL = "SELECT * FROM TblRequest WHERE RequestId = " + lRequestId;
                        TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDsReq, "TblRequest");

                        lRowRequest = lDsReq.TblRequest[0];

                        lRowRequest.EndJobStateId = (int)mdl_Publics.EndJobStates.ejs_NotDone;
                        if (lRowRequest.IsExtraCommentsNull())
                            lRowRequest.ExtraComments = "";
                        lRowRequest.ExtraComments += "\n" + CancelReason;
                    }
                }

                lRowTamir.TamirRequestStateId = (int)TamirRequestStates.trs_NotDone;

                if (lRowTamir.IsOperationDescNull())
                    lRowTamir.OperationDesc = "";
                lRowTamir.OperationDesc += " : " + CancelReason;

                if (lCnn.State != ConnectionState.Open)
                    lCnn.Open();
                lTrans = lCnn.BeginTransaction();

                lUpdate.UpdateDataSet("TblTamirRequest", lDs, lRowTamir.AreaId, -1, true, lTrans);
                if( lRequestId > -1 )
                    lUpdate.UpdateDataSet("TblRequest", lDsReq, lRowRequest.AreaId, -1, true, lTrans);

                lTrans.Commit();

                lRowResult.ResultCode = (short)TZServicesLib.CFunctions.TZResultCodes.Success;
                lRowResult.ResultValue = "خاموشی درخواست شده، لغو گردید.";
            }
            catch (Exception ex)
            {
                TZServiceTools.mdl_Publics.LogError(Server, "Cancel Tamir Error: " + ex.Message, "TZServices_Tamir_Logs");
                if (lTrans != null)
                    lTrans.Rollback();

                lRowResult.ResultCode = (short)TZServicesLib.CFunctions.TZResultCodes.Error;
                lRowResult.ResultValue = ex.Message;
            }
            finally
            {
                if (lCnn.State == ConnectionState.Open)
                    lCnn.Close();
            }

            String lResult = TZServicesLib.CFunctions.GetJsonDataTable(lTblResult);
            return lResult;
        }

        private static string CheckTamirOperation(TamirInfo aDCInfo, DataSetTamir.Tbl_TamirOperationDataTable aTamirOperationTable)
        {
            string lErrMsg="";
            try
            {
                foreach (OperationInfo lOperationInfo in aDCInfo.OperationList)
                {
                    if (lOperationInfo.OperationCount < 0)
                        throw new Exception("OperationCount is not valid.");
                    if (lOperationInfo.OperationGroupId > 0 && (lOperationInfo.OperationGroupId < 1 || lOperationInfo.OperationGroupId > 5))
                        throw new Exception("OperationGroupId is not valid.");
                    if (aTamirOperationTable.Select("TamirOperationId = " + lOperationInfo.OperationId).Length == 0)
                        lErrMsg += (lErrMsg.Length > 0 ? ", " : "") + lOperationInfo.OperationId;                          
                }
                if (lErrMsg.Length > 0)
                {
                    throw new Exception("Invalid OperationId(s): (" + lErrMsg + ")");
                }

            }
            catch (Exception ex)
            {

                lErrMsg = ex.Message;
            }
            return lErrMsg;
        }

        private static DateTime GetSaturdayDT(DateTime aDate)
        {
            
            DateTime lDT;
           
            DayOfWeek lDOW = aDate.DayOfWeek;
            byte lDiff=0;
            switch  (lDOW)
            {
                case System.DayOfWeek.Saturday:
                lDiff = 0;
                break;
           
                case System.DayOfWeek.Sunday:
                lDiff = 1;
                    break;
                case System.DayOfWeek.Monday:
                lDiff = 2;
                    break;
                case System.DayOfWeek.Tuesday:
                lDiff = 3;
                    break;
                case System.DayOfWeek.Wednesday:
                lDiff = 4;
                    break;
                case System.DayOfWeek.Thursday:
                lDiff = 5;
                    break;
                case System.DayOfWeek.Friday:
                lDiff = 6;
                    break;
            }
        
            lDT = aDate.AddDays(-lDiff) ;
            lDT = lDT.AddSeconds(-lDT.Second);
            lDT = lDT.AddMinutes(-lDT.Minute);
            lDT = lDT.AddHours(-lDT.Hour);
            lDT = lDT.AddMilliseconds(-lDT.Millisecond);
            return lDT;
        
        }

        private static DateTime GetFirstDateOfMonth(DateTime aDate)
        {
            DateTime lDT ;
            lDT = DateTime.MinValue;
            
            try 
            {
                string lPDate;
               TZServiceTools.PersianDate.KhayamDateTime lKh = new PersianDate.KhayamDateTime();
               
                lKh.SetMiladyDate(aDate);
                lPDate = lKh.GetShamsiDateStr();
                lPDate = Regex.Replace(lPDate, "\\d\\d", "01");
                lKh.SetShamsiDate(lPDate);
                lDT = lKh.GetMiladyDate();
            }
            catch
            {
            }

            lDT = lDT.AddSeconds(-lDT.Second);
            lDT = lDT.AddMinutes(-lDT.Minute);
            lDT = lDT.AddHours(-lDT.Hour);
            lDT = lDT.AddMilliseconds(-lDT.Millisecond);
            return lDT;

        }

        private static DateTime GetFirstDateOfTamirScope(DateTime aDate ,bool IsTamirPowerMonthly)
        {
            if (IsTamirPowerMonthly)
                return GetFirstDateOfMonth(aDate);
            else
                return GetSaturdayDT(aDate);    
        }

        private static DateTime GetDT(string aShamsiDate, string aTime)
        {
            TZServiceTools.PersianDate.KhayamDateTime lKh = new PersianDate.KhayamDateTime();
            lKh.SetShamsiDate(aShamsiDate);
            DateTime lDT = lKh.GetMiladyDate();
            int lHour; int.TryParse(aTime.Substring(0, 2), out lHour);
            int lMinute; int.TryParse(aTime.Substring(3, 2), out lMinute);
            lDT = lDT.AddHours(-lDT.Hour).AddHours(lHour);
            lDT = lDT.AddMinutes(-lDT.Minute).AddMinutes(lMinute);
            lDT = lDT.AddSeconds(-lDT.Second).AddSeconds(0);
            return lDT;
        }

        private class TamirRequest
        {
            //-------------Fields---------------------
            public string lMsg = "";
            public string lNType = "";
            public string lErr = "";
            public TZDataSet.TblResultDataTable lTblResult;
            public TZDataSet.TblResultRow lRowResult;
            public SqlConnection lCnn;
            public DataSetTamir lDs;
            public DataSetTamir.TblTamirRequestDataTable lTblTamirRequest;
            public DataSetTamir.TblTamirRequestRow lTamirRow;
            public DataSetTamir.TblTamirOperationListDataTable lTblTamirOperationList;
            public DataSetTamir.TblTamirOperationListRow lTamirOperationListRow;
            public frmUpdateDataset lUpdate;
            public TamirInfo lDCSaveInfo;
            public TamirInfo TamirRequestInfo;
            public SqlTransaction lTrans;
            public long TamirId;
            public SortedList<int, int> lTimes;
            //-------------Methods---------------------
            public TamirRequest(TamirInfo TamirRequestInfo)
            {
                this.TamirRequestInfo = TamirRequestInfo;
                this.lTblResult = TZServicesLib.CFunctions.GetPublicResultDataTable();
                this.lRowResult = lTblResult[0];
                this.lDs = new DataSetTamir();
                this.lCnn = new SqlConnection(TZServiceTools.mdl_Publics.mConnectionString);
                this.lDCSaveInfo = TamirRequestInfo;
                this.lUpdate = new frmUpdateDataset();
                // this. = ;
            }
            public TamirRequest setAccess()
            {
                string lAccess = System.Configuration.ConfigurationManager.AppSettings["HavadesTamirAccess"];
                if (lAccess == null) lAccess = "false";
                if (lAccess.ToLower() != "true")
                    throw new Exception("Tamir - Access Denied");
                return this;
            }
            public TamirRequest setTamirID(long aTamirId)
            {
                this.TamirId = aTamirId;
                if (aTamirId <= 0) goto Skip;

                string lSSQL = "SELECT * From TblTamirRequest WHERE TamirRequestId = " + TamirId;
                TZServiceTools.mdl_Publics.BindingTable(lSSQL, ref lCnn, lDs, "TblTamirRequest");

                if (lDs.Tables["TblTamirRequest"].Rows.Count == 0)
                    throw new Exception("TamirId is invalid.");
                else
                    if (lDs.TblTamirRequest[0].TamirRequestStateId != (int)TamirRequestStates.trs_PreNew)
                        throw new Exception("This request is sent to confirm or canceled and cannot be edit.");

                lTamirRow = lDs.TblTamirRequest[0];

                lSSQL = "SELECT * FROM TblTamirOperationList WHERE TamirRequestId = " + TamirId;
                TZServiceTools.mdl_Publics.BindingTable(lSSQL, ref lCnn, lDs, "TblTamirOperationList");
            Skip:
                lMsg = IsSaveTamirOK(lCnn, lDCSaveInfo);
                if (lMsg.Length > 0)
                    throw new Exception(lMsg);

                lErr = CheckDCSaveInfo(lDCSaveInfo, ref lCnn);
                if (lErr.Length > 0)
                    throw new Exception(lErr);
                lTblTamirRequest = lDs.TblTamirRequest;
                if (lTamirRow == null)
                    lTamirRow = lDs.TblTamirRequest.NewTblTamirRequestRow();
                return this;
            }
            public TamirRequest setAreaID() {
                lTamirRow.AreaId = TamirRequestInfo.AreaId;

                string lSQL = "SELECT CityId FROM Tbl_Area WHERE AreaId = " + lTamirRow.AreaId;
                TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_Area");
                if (lDs.Tables["Tbl_Area"].Rows.Count == 0)
                    throw new Exception("AreaId is invalid.");
                lTamirRow.CityId = Int32.Parse(lDs.Tables["Tbl_Area"].Rows[0]["CityId"].ToString());

                return this;
            }
            public TamirRequest setTamirTypeID() {
                lTamirRow.TamirTypeId = (int)TamirTypes.tt_BaMovafeghat;
                return this;
            }
            public TamirRequest setNazerID()
            {
                if (TamirRequestInfo.NazerId > 0)
                  lTamirRow.NazerId = TamirRequestInfo.NazerId;
                return this;
            }
            public TamirRequest setPeymankarID()
            {
                lTamirRow.PeymankarId = TamirRequestInfo.PeymankarId;
                return this;
            }
            public TamirRequest setDepartmentID()
            {
                lTamirRow.DepartmentId = TamirRequestInfo.DepartmentId;
                return this;
            }
            public TamirRequest setNetworkTypeID()
            {
                lNType = TamirRequestInfo.NetworkType.ToUpper();
                if (lNType == "MP")
                    lTamirRow.TamirNetworkTypeId = (int)TamirNetworkTypes.tnt_MP;
                if (lNType == "LP")
                    lTamirRow.TamirNetworkTypeId = (int)TamirNetworkTypes.tnt_LP;
                return this;
            }
            public TamirRequest setDisconnectDT()
            {
                DateTime lKDT = GetDT(TamirRequestInfo.DisconnectDate, TamirRequestInfo.DisconnectTime);
                lTamirRow.DisconnectDT = lKDT;
                lTamirRow.DisconnectDatePersian = TamirRequestInfo.DisconnectDate;
                lTamirRow.DisconnectTime = TamirRequestInfo.DisconnectTime;
                return this;
            }
            public TamirRequest setConnectDT()
            {
                DateTime lDT = GetDT(TamirRequestInfo.ConnectDate, TamirRequestInfo.ConnectTime);
                lTamirRow.ConnectDT = lDT;
                lTamirRow.ConnectDatePersian = TamirRequestInfo.ConnectDate;
                lTamirRow.ConnectTime = TamirRequestInfo.ConnectTime;
                return this;
            }
            public TamirRequest checkConnDisDT() {
                if (lTamirRow.DisconnectDT >= DateTime.Now.AddDays(365))
                    throw new Exception("DisconnectDate is invalid.");
                if (lTamirRow.DisconnectDT < DateTime.Now)
                    throw new Exception("DisconnectDate is invalid.");
                int DateDiff;
                DateDiff = int.Parse((((lTamirRow.ConnectDT - lTamirRow.DisconnectDT).Days).ToString()));
                if (DateDiff >= 7)
                    throw new Exception("Date Difference between DisconnectDate and ConnectDate must be less than 7 days");
                return this;
            }
            public TamirRequest setDisconnectInterval()
            {
                lTamirRow.DisconnectInterval = Convert.ToInt32((lTamirRow.ConnectDT - lTamirRow.DisconnectDT).TotalMinutes);

                if (lTamirRow.DisconnectInterval <= 0)
                    throw new Exception("ConnectDate must be after DisconnectDate ");
                return this;
            }
            public TamirRequest setDisconnectPower(){
              lTamirRow.DisconnectPower = 0.0;
               return this;
            }
            public TamirRequest setAreaUserID(){
                lTamirRow.AreaUserId = mdl_Publics.Find_AVL_User(lCnn);
                return this;
            }
            public TamirRequest setDEDT()
            {
                TZServiceTools.PersianDate.KhayamDateTime lKh_DE = new PersianDate.KhayamDateTime();
                DateTime lDT_DE = DateTime.Now;
                lKh_DE.SetMiladyDate(lDT_DE);
                lTamirRow.DEDT = lDT_DE;
                lTamirRow.DEDatePersian = lKh_DE.GetShamsiDateStr();
                lTamirRow.DETime = String.Format("{0}:{1}", lDT_DE.Hour.ToString("0#"), lDT_DE.Minute.ToString("0#"));
               return this;
            }
            public TamirRequest setIsWatched(bool state = false){
                lTamirRow.IsWatched = state;
               return this;
            }
            public TamirRequest setMPPostID(){
                lTamirRow.MPPostId = TamirRequestInfo.MPPostId;
               return this;
            }
            public TamirRequest setMPFeederID()
            {
                lTamirRow.MPFeederId = TamirRequestInfo.MPFeederId;
               return this;
            }
            public TamirRequest setFeederPartID()
            {
                if (TamirRequestInfo.FeederPartId > 0 && TamirRequestInfo.NetworkType == "MP")
                    lTamirRow.FeederPartId = TamirRequestInfo.FeederPartId;

                if (TamirRequestInfo.LPPostId > 0 && TamirRequestInfo.FeederPartId > 0)
                {
                    string lSQL = "SELECT * FROM Tbl_LPPost WHERE LPPostId = " + TamirRequestInfo.LPPostId + " AND FeederPartId = " + TamirRequestInfo.FeederPartId;
                    TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_LPPost");

                    if (lDs.Tables["Tbl_LPPost"].Rows.Count == 0)
                        throw new Exception("LPPostId and FeederPartId is not match.");
                }
               return this;
            }
            public TamirRequest setLPPostID()
            {
                if (TamirRequestInfo.LPPostId > 0)
                    lTamirRow.LPPostId = TamirRequestInfo.LPPostId;
               return this;
            }
            public TamirRequest setLPFeederID()
            {
                if (TamirRequestInfo.LPFeederId > 0 && TamirRequestInfo.NetworkType == "LP")
                    lTamirRow.LPFeederId = TamirRequestInfo.LPFeederId;
                return this;
            }
            public TamirRequest setIsWarmLine()
            {
                lTamirRow.IsWarmLine = TamirRequestInfo.IsWarmLine == 0 ? false : true;
                return this;
            }
            public TamirRequest setCurrentValue()
            {
                lTamirRow.CurrentValue = TamirRequestInfo.CurrentValue;
                return this;
            }
            public TamirRequest setWorkingAddress()
            {
                lTamirRow.WorkingAddress = TamirRequestInfo.WorkingAddress.Trim();
                return this;
            }
            public TamirRequest setGPS()
            {
                if (TamirRequestInfo.GpsX > 0) lTamirRow.GpsX = TamirRequestInfo.GpsX;
                if (TamirRequestInfo.GpsY > 0) lTamirRow.GpsY = TamirRequestInfo.GpsY;
                return this;
            }
            public TamirRequest setCriticalsAddress()
            {
                lTamirRow.CriticalsAddress = TamirRequestInfo.CriticalsAddress.Trim();
                return this;
            }
            public TamirRequest setIsInCityService()
            {
                lTamirRow.IsInCityService = TamirRequestInfo.IsInCityService == 0 ? false : true;
                return this;
            }
            public TamirRequest setIsManoeuvre()
            {
                lTamirRow.IsManoeuvre = TamirRequestInfo.IsManoeuvre == 0 ? false : true;
                if (lTamirRow.IsManoeuvre)
                {
                    lTamirRow.ManoeuvreDesc = TamirRequestInfo.ManoeuvreDesc.Trim();
                    lTamirRow.ManoeuvreTypeId = TamirRequestInfo.ManoeuvreTypeId;
                }
                return this;
            }
            public TamirRequest setSaturdayDT(bool state = false)
            {
                lTamirRow.SaturdayDT = GetFirstDateOfTamirScope(lTamirRow.DisconnectDT , state);
                return this;
            }
            public TamirRequest setIsRequestByPeymankar()
            {
                lTamirRow.IsRequestByPeymankar = true;
                return this;
            }
            public TamirRequest setReferToID()
            {
                if (TamirRequestInfo.ReferToId > 0)
                    lTamirRow.ReferToId = TamirRequestInfo.ReferToId;
                return this;
            }
            public TamirRequest setTamirRequestStateID()
            {
                lTamirRow.TamirRequestStateId = (int)TamirRequestStates.trs_PreNew;
                return this;
            }
            public TamirRequest checkTamirID()
            {
                if (TamirId > 0) goto End;

                lTamirRow.TamirRequestId = mdl_Publics.GetAutoInc();
                lTamirRow.TamirRequestNo = -1;
                lDs.TblTamirRequest.Rows.Add(lTamirRow);
            End:
                return this;
            }
            public TamirRequest setOperationList()
            {
                lTblTamirOperationList = lDs.TblTamirOperationList;
                if (TamirRequestInfo.OperationList == null) goto End_Check;

                string lSQL = "SELECT * from Tbl_TamirOperation";
                TZServiceTools.mdl_Publics.BindingTable(lSQL, ref lCnn, lDs, "Tbl_TamirOperation", -1, 0, true);
                lErr = CheckTamirOperation(lDCSaveInfo, lDs.Tbl_TamirOperation);
                if (lErr.Length > 0)
                    throw new Exception(lErr);
            End_Check:
                if (TamirRequestInfo.OperationList == null || TamirRequestInfo.OperationList.Length == 0) goto End;
                DataRow[] lRowOperationList = null;
                    
                this.lTimes = new SortedList<int, int>();

                foreach (OperationInfo lOperationInfo in TamirRequestInfo.OperationList)
                {
                    lRowOperationList = lTblTamirOperationList.Select("TamirOperationId = " + lOperationInfo.OperationId);
                    if (lRowOperationList.Length > 0)
                        lTamirOperationListRow = lRowOperationList[0] as DataSetTamir.TblTamirOperationListRow;
                    else
                    {
                        lTamirOperationListRow = lTblTamirOperationList.NewTblTamirOperationListRow();
                        lTamirOperationListRow.TamirOperationListId = mdl_Publics.GetIntAutoInc();
                    }
                    lTamirOperationListRow.TamirRequestId = lTamirRow.TamirRequestId;
                    lTamirOperationListRow.TamirOperationId = lOperationInfo.OperationId;
                    lTamirOperationListRow.OperationCount = lOperationInfo.OperationCount;

                    if (lOperationInfo.GroupCount > 0)
                        lTamirOperationListRow.PeoplesCount = lOperationInfo.GroupCount;
                    else if (lOperationInfo.GroupCount == 0)
                        lTamirOperationListRow.SetPeoplesCountNull();

                    if (lOperationInfo.OperationGroupId > 0)
                        lTamirOperationListRow.TamirOperationGroupId = lOperationInfo.OperationGroupId;
                    else if (lOperationInfo.OperationGroupId == 0)
                        lTamirOperationListRow.SetTamirOperationGroupIdNull();

                    DataSetTamir.Tbl_TamirOperationRow[] lOprows = lDs.Tbl_TamirOperation.Select(
                        String.Format("TamirOperationId = {0}", lTamirOperationListRow.TamirOperationId)
                    ) as DataSetTamir.Tbl_TamirOperationRow[];
                    if (lOprows.Length > 0)
                    {
                        int lPeopleCount = 1;
                        if (!lTamirOperationListRow.IsPeoplesCountNull())
                            lPeopleCount = lTamirOperationListRow.PeoplesCount;
                        if (lPeopleCount <= 0) lPeopleCount = 1;

                        lTamirOperationListRow.Duration = (lTamirOperationListRow.OperationCount * lOprows[0].Duration / lPeopleCount);
                    }

                    if (lRowOperationList.Length == 0)
                        lTblTamirOperationList.AddTblTamirOperationListRow(lTamirOperationListRow);

                    int lGroupId = 0;
                    if (!lTamirOperationListRow.IsTamirOperationGroupIdNull())
                        lGroupId = lTamirOperationListRow.TamirOperationGroupId;

                    if (!lTimes.ContainsKey(lGroupId))
                        lTimes.Add(lGroupId, 0);

                    lTimes[lGroupId] += lTamirOperationListRow.Duration;
                }
             End:
                return this;
            }
            public TamirRequest checkDuration()
            {
                int lMaxTime = 0;
                foreach (int lDuration in lTimes.Values)
                    if (lDuration > lMaxTime)
                        lMaxTime = lDuration;

                if (lTamirRow.DisconnectInterval > lMaxTime)
                    throw new Exception("DisconnectInterval must be less than Total Operations Duration");
                return this;
            }
            public TamirRequest sqlOperation()
            {
                if (lCnn.State != ConnectionState.Open)
                    lCnn.Open();
                lTrans = lCnn.BeginTransaction();

                lUpdate.UpdateDataSet("TblTamirRequest", lDs, lTamirRow.AreaId, -1, true, lTrans);

                System.Collections.ArrayList lDeletes = new System.Collections.ArrayList();
                foreach (DataSetTamir.TblTamirOperationListRow lRowOperation in lTblTamirOperationList)
                {
                    lRowOperation.TamirRequestId = lTamirRow.TamirRequestId;
                    if (lRowOperation.OperationCount == 0)
                        lDeletes.Add(lRowOperation);
                }

                foreach (DataSetTamir.TblTamirOperationListRow lRowOperation in lDeletes)
                    lRowOperation.Delete();

                lUpdate.UpdateDataSet("TblTamirOperationList", lDs, lTamirRow.AreaId, -1, true, lTrans);

                lTrans.Commit();

                lRowResult.ResultCode = (short)TZServicesLib.CFunctions.TZResultCodes.Success;
                lRowResult.ResultValue = lTamirRow.TamirRequestId.ToString();

                return this;
            }
            public TamirRequest catchOperation(Exception ex) {
                if (lTrans != null)
                    lTrans.Rollback();

                lRowResult.ResultCode = (short)TZServicesLib.CFunctions.TZResultCodes.Error;
                lRowResult.ResultValue = ex.Message;
                return this;
            }
            public TamirRequest finallyOperation() {
                if (lCnn.State == ConnectionState.Open)
                    lCnn.Close(); 
                return this;
            }
            public string getResult() {
                return TZServicesLib.CFunctions.GetJsonDataTable(lTblResult);
            }
        }
    }
}
public class TamirInfo
{
    private string mNetworkType;
    public int NazerId;
    public int PeymankarId;
    public int DepartmentId;
    public string DisconnectDate;
    public string DisconnectTime;
    public string ConnectDate;
    public string ConnectTime;
    public string NetworkType
    {
        get { return mNetworkType; }
        set
        {
            if (Regex.IsMatch(value, "^(MP|LP)$", RegexOptions.IgnoreCase))
                mNetworkType = value.ToUpper();
            else
                mNetworkType = "";
        }
    }
    
    
    public int AreaId;
    public int MPPostId;
    public int MPFeederId;
    public int FeederPartId;
    public int LPPostId;
    public int LPFeederId;

    public int ReferToId;

    public int IsWarmLine;
    public double CurrentValue;
    public string WorkingAddress;
    public float GpsX;
    public float GpsY;
    public string CriticalsAddress;
    public int IsInCityService;
    public int IsManoeuvre;
    public string ManoeuvreDesc;
    public int ManoeuvreTypeId;
    public  OperationInfo[] OperationList;
}
public class OperationInfo
{
    public int OperationId;
    public int OperationCount;
    public int GroupCount;
    public int OperationGroupId;

}
