using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TZServicesCSharp.Classes;
using System.Text.RegularExpressions;
using TZServiceTools;
using System.Data;

namespace TZServicesCSharp.RestServices
{
    public partial class RestReports : CPageBase
    {
        private class ReportParams
        {
            public string Code { get; set; }
            public string TypeId { get; set; }
            public string EventTypeId { get; set; }

            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string AreaId { get; set; }
            public string RequestNumber { get; set; }
            public string LPPostId { get; set; }
            public string LPFeederId { get; set; }

            public string MPPostId { get; set; }
            public string MPFeederId { get; set; }

            public string LPPostCode { get; set; }
            public string LocalCode { get; set; }
            public int? RecCount { get; set; }

            public string MinCount { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string lResult = "";
            HavadesReports lReportService = new HavadesReports();

            string lInputData = "";
            string lInformType = "";

            try
            {
                lInformType = GetQueryString("ReportType");

                string lMethodNames =
                    "^(getsubscriberoutagestateinfo|getlppostbaseinfo|getlpfeederbaseinfo|getlppostload|getlppostvoltage|" +
                    "getlpfeederload|getlpfeedervoltage|getmpfeederpeak|gettamirrequest|getlightrequest|getlpusepart|" +
                    "getmpusepart|getlightusepart|getmultistepconnection|getmpfeederbaseinfo|" +
                    "sanjesh_avgunwanted|sanjesh_avgwanted|sanjesh_highpostoff|" +
                    "gmaz_getlppostload|gmaz_report_sabz|gmaz_unstable_feeders|"+
                    "azargh_feederpeak|azargh_postfeederload)$";

                if (!Regex.IsMatch(lInformType, lMethodNames, RegexOptions.IgnoreCase))
                    throw new Exception("درخواست شما معتبر نمی باشد");

                lInputData = GetQueryStringOrFirstParameter("param");
                ReportParams lParams = mdl_Publics.GetClassFromJson<ReportParams>(lInputData);

                if (lParams == null)
                    throw new Exception("پارامتر ورودی نامعتبر است");

                mdl_Publics.LogMessage(Server,
                    string.Format("HavadesReports({0}) REST input data: {1}", lInformType, lInputData),
                    "TZServices_Reports_Logs");
                //-------------<omid>
                Checker ch = new Checker(lParams);
                //-------------</omid>
                string lFromDate = "";
                string lToDate = "";
                string lCode = "";
                int lAreaId = -1;
                int lLPPostId = -1;
                int lMPFeederId = -1;
                int lMPPostId = -1;
                int lMinCount = -1;

                Danesh_Result lRes = new Danesh_Result();
                DataSet lDs = new DataSet();
                switch (lInformType.ToLower())
                {
                    case "getsubscriberoutagestateinfo":
                        if (string.IsNullOrWhiteSpace(lParams.Code))
                            throw new Exception("Code is not specified");
                        else if (string.IsNullOrWhiteSpace(lParams.TypeId))
                            throw new Exception("TypeId is not specified");
                        else if (string.IsNullOrWhiteSpace(lParams.EventTypeId))
                            lParams.EventTypeId = "-1";

                        SubscriberOutageStateInfo lSubOutageInfoResT = lReportService.GetSubscriberOutageStateInfo(lParams.Code, lParams.TypeId, lParams.EventTypeId);
                        lResult = mdl_Publics.GetJSonString(lSubOutageInfoResT);
                        break;
                    case "getlppostbaseinfo":

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                            lMPFeederId = int.Parse(lParams.MPFeederId);

                        lDs = lReportService.Danesh_GetLPPostBaseInfo(lAreaId, lMPFeederId);
                        break;

                    case "getlpfeederbaseinfo":

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLPFeederBaseInfo(lAreaId, lLPPostId);
                        break;
                    case "getmpfeederbaseinfo":

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lMPPostId = int.Parse(lParams.MPPostId);

                        lDs = lReportService.Danesh_GetMPFeederBaseInfo(lAreaId, lMPPostId);
                        break;
                    case "getlppostload":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLPPostLoad(lFromDate, lToDate, lAreaId, lLPPostId);
                        break;
                    case "getlppostvoltage":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLPPostVoltage(lFromDate, lToDate, lAreaId, lLPPostId);
                        break;
                    case "getlpfeederload":

                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLPFeederLoad(lFromDate, lToDate, lAreaId, lLPPostId);
                        break;
                    case "getlpfeedervoltage":

                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLPFeederVoltage(lFromDate, lToDate, lAreaId, lLPPostId);
                        break;
                    case "getmpfeederpeak":

                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                            lMPFeederId = int.Parse(lParams.MPFeederId);

                        lDs = lReportService.Danesh_GetMPFeederPeak(lFromDate, lToDate, lAreaId, lMPFeederId);
                        break;
                    case "gettamirrequest":

                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                            lMPFeederId = int.Parse(lParams.MPFeederId);

                        lDs = lReportService.Danesh_GetTamirRequest(lFromDate, lToDate, lAreaId, lMPFeederId);
                        break;

                    case "getlightrequest":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        lDs = lReportService.Danesh_GetLightRequest(lFromDate, lToDate, lAreaId);

                        break;
                    case "getlpusepart":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLPUsePart(lFromDate, lToDate, lAreaId, lLPPostId);

                        break;
                    case "getmpusepart":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                            lMPFeederId = int.Parse(lParams.MPFeederId);

                        lDs = lReportService.Danesh_GetMPUsePart(lFromDate, lToDate, lAreaId, lMPFeederId);

                        break;
                    case "getlightusepart":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                            lLPPostId = int.Parse(lParams.LPPostId);

                        lDs = lReportService.Danesh_GetLightUsePart(lFromDate, lToDate, lAreaId, lLPPostId);

                        break;
                    case "getmultistepconnection":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                            lAreaId = int.Parse(lParams.AreaId);

                        if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                            lMPFeederId = int.Parse(lParams.MPFeederId);

                        lDs = lReportService.Danesh_GetMultiStepConnection(lFromDate, lToDate, lAreaId, lMPFeederId);

                        break;

                    case "sanjesh_avgunwanted":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        lDs = lReportService.SANJESH_AVGUNWANTED(lFromDate, lToDate);

                        break;

                    case "sanjesh_avgwanted":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        lDs = lReportService.SANJESH_AVGWANTED(lFromDate, lToDate);

                        break;

                    case "sanjesh_highpostoff":
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        lDs = lReportService.SANJESH_HIGHPOSTOFF(lFromDate, lToDate);

                        break;

                    case "gmaz_getlppostload":

                        lDs = null;
                        lFromDate = "";
                        lToDate = "";

                        if (!string.IsNullOrWhiteSpace(lParams.FromDate))
                        {
                            lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                            if (lFromDate == "Error")
                                throw new Exception("FromDate is not Valid");
                        }

                        if (!string.IsNullOrWhiteSpace(lParams.ToDate))
                        {
                            lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                            if (lToDate == "Error")
                                throw new Exception("ToDate is not Valid");
                        }
                        string lLPPostCode = "";
                        string lLocalCode = "";
                        int? lCount = -1;

                        if (!string.IsNullOrWhiteSpace(lParams.LocalCode))
                            lLocalCode = lParams.LocalCode;

                        if (!string.IsNullOrWhiteSpace(lParams.LPPostCode))
                            lLPPostCode = lParams.LPPostCode;

                        if (lParams.RecCount != null && lParams.RecCount > -1)
                            lCount = lParams.RecCount;

                        lRes = lReportService.GMaz_GetLPPostLoad(lLocalCode, lLPPostCode, lFromDate, lToDate, lCount);
                        lResult = mdl_Publics.GetJSonString(lRes);

                        break;
                    case "gmaz_report_sabz":
                        lDs = null;
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");
                        lRes = lReportService.GMaz_Report_14_1(lFromDate, lToDate);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    //----------omid------------
                    case "gmaz_unstable_feeders":
                        lDs = null;
                        if (string.IsNullOrWhiteSpace(lParams.FromDate))
                            throw new Exception("FromDate is not Valid");

                        lFromDate = mdl_Publics.ValidDate(lParams.FromDate);
                        if (lFromDate == "Error")
                            throw new Exception("FromDate is not Valid");

                        if (string.IsNullOrWhiteSpace(lParams.ToDate))
                            throw new Exception("ToDate is not Valid");

                        lToDate = mdl_Publics.ValidDate(lParams.ToDate);
                        if (lToDate == "Error")
                            throw new Exception("ToDate is not Valid");

                        if (!string.IsNullOrWhiteSpace(lParams.MinCount))
                            lMinCount = int.Parse(lParams.MinCount);
                        lRes = lReportService.GMaz_Unstable_Feeders(lFromDate, lToDate, lMinCount);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    //----------omid------------
                    case "azargh_feederpeak":
                        lDs = null;
                        ch.check_From() // Default From
                            .check_To() // Default To
                            .check_Code();
                        lRes = lReportService.AzarGH_FeederPeak_Monthly(ch.lFromDate, ch.lToDate, ch.lCode);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    //----------omid------------
                    case "azargh_postfeederload":
                        lDs = null;
                        ch.check_From()  // Default From
                            .check_To()   // Default To
                            .check_lMinCount(3) 
                            .check_Code();
                        lRes = lReportService.AzarGH_PostFeederLoad(ch.lFromDate, ch.lToDate, ch.lCode , ch.lMinCount);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    default:
                        throw new Exception("درخواست شما معتبر نمی باشد");
                }
                if (lDs != null)
                    lResult = GetResult(lDs);
            }
            catch (Exception ex)
            {
                Danesh_Result lRes = new Danesh_Result();
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
                lResult = mdl_Publics.GetJSonString(lRes);
            }

            TZServiceTools.mdl_Publics.LogMessage(Server,
                string.Format("HavadesReports({0}) REST Result: {1}", lInformType, lResult),
                "TZServices_Reports_Logs");

            Response.ContentType = "application/json";
            Response.Write(lResult);
        }

        private string GetResult(DataSet aDs)
        {
            string lResult = "";
            try
            {
                if (aDs.Tables.Contains("Result"))
                    lResult = mdl_Publics.GetJSonString(aDs.Tables["Result"]);
                else if (aDs.Tables.Contains("Tbl_Error") && aDs.Tables["Tbl_Error"].Rows.Count > 0)
                    lResult = mdl_Publics.GetJSonString(aDs.Tables["Tbl_Error"]);//.Rows[0]["ErrorMessage"].ToString();
                else
                    lResult = "no result";
            }
            catch { }
            return lResult;
        }

        private class Checker
        {
            public string lFromDate = "";
            public string lToDate = "";
            public int lAreaId = -1;
            public int lLPPostId = -1;
            public int lMPFeederId = -1;
            public int lMPPostId = -1;
            public int lMinCount = -1;
            public string lCode = "";
            private ReportParams lParams;

            public Checker(ReportParams aParams)
            {
                this.lParams = aParams;
            }

            public Checker check_From(string aFromDate = "")
            {
                if (string.IsNullOrWhiteSpace(lParams.FromDate))
                {
                    if (string.IsNullOrWhiteSpace(aFromDate))
                        throw new Exception("FromDate is not Valid");
                    lFromDate = aFromDate;
                }
                lFromDate = lFromDate == "" ? mdl_Publics.ValidDate(lParams.FromDate) : lFromDate;
                if (lFromDate == "Error")
                    throw new Exception("FromDate is not Valid");
                return this;
            }
            public Checker check_To(string aToDate = "")
            {
                if (string.IsNullOrWhiteSpace(lParams.ToDate))
                {
                    if (string.IsNullOrWhiteSpace(aToDate))
                        throw new Exception("ToDate is not Valid");
                    lFromDate = aToDate;
                }
                lToDate = lToDate == "" ? mdl_Publics.ValidDate(lParams.ToDate) : lToDate;
                if (lToDate == "Error")
                    throw new Exception("ToDate is not Valid");
                return this;
            }
            public Checker check_Code()
            {
                if (string.IsNullOrWhiteSpace(lParams.Code))
                    throw new Exception("Code is not Valid");
                lCode = lParams.Code;
                return this;
            }
            public Checker check_AreaID()
            {
                if (!string.IsNullOrWhiteSpace(lParams.AreaId))
                    lAreaId = int.Parse(lParams.AreaId);
                return this;
            }
            public Checker check_lLPPostId()
            {
                if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                    lLPPostId = int.Parse(lParams.LPPostId);
                return this;
            }
            public Checker check_lMPPostId()
            {
                if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                    lMPPostId = int.Parse(lParams.MPPostId);
                return this;
            }
            public Checker check_lMPFeederId()
            {
                if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                    lMPFeederId = int.Parse(lParams.MPFeederId);
                return this;
            }
            public Checker check_lMinCount(int aMinCount = -1)
            {
                if (string.IsNullOrWhiteSpace(lParams.MinCount) && aMinCount > -1)
                    lMinCount = aMinCount;
                lMinCount = lMinCount > -1 ? lMinCount : int.Parse(lParams.MinCount);
                return this;
            }
        }
    }
}