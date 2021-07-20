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
                    "azargh_feederpeak|azargh_postfeederload|khornorth_lppostloadbalance|skerman_dashboardstat)$";

                if (!Regex.IsMatch(lInformType, lMethodNames, RegexOptions.IgnoreCase))
                    throw new Exception("درخواست شما معتبر نمی باشد");

                lInputData = GetQueryStringOrFirstParameter("param");
                ReportParams lParams = mdl_Publics.GetClassFromJson<ReportParams>(lInputData);

                if (lParams == null)
                    throw new Exception("پارامتر ورودی نامعتبر است");

                mdl_Publics.LogMessage(Server,
                    string.Format("HavadesReports({0}) REST input data: {1}", lInformType, lInputData),
                    "TZServices_Reports_Logs");
                Checker ch = new Checker(lParams);
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
                        ch.check_AreaID()
                            .check_MPFeederId();
                        lDs = lReportService.Danesh_GetLPPostBaseInfo(ch.lAreaId, ch.lMPFeederId);
                        break;
                    case "getlpfeederbaseinfo":
                        ch.check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLPFeederBaseInfo(ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getmpfeederbaseinfo":
                        ch.check_AreaID()
                            .check_MPPostId();
                        lDs = lReportService.Danesh_GetMPFeederBaseInfo(ch.lAreaId, ch.lMPPostId);
                        break;
                    case "getlppostload":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLPPostLoad(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getlppostvoltage":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLPPostVoltage(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getlpfeederload":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLPFeederLoad(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getlpfeedervoltage":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLPFeederVoltage(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getmpfeederpeak":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_MPFeederId();
                        lDs = lReportService.Danesh_GetMPFeederPeak(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lMPFeederId);
                        break;
                    case "gettamirrequest":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_MPFeederId();
                        lDs = lReportService.Danesh_GetTamirRequest(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lMPFeederId);
                        break;
                    case "getlightrequest":
                        ch.check_From()
                            .check_To()
                            .check_AreaID();
                        lDs = lReportService.Danesh_GetLightRequest(ch.lFromDate, ch.lToDate, ch.lAreaId);
                        break;
                    case "getlpusepart":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLPUsePart(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getmpusepart":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_MPFeederId();
                        lDs = lReportService.Danesh_GetMPUsePart(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lMPFeederId);
                        break;
                    case "getlightusepart":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_LPPostId();
                        lDs = lReportService.Danesh_GetLightUsePart(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lLPPostId);
                        break;
                    case "getmultistepconnection":
                        ch.check_From()
                            .check_To()
                            .check_AreaID()
                            .check_MPFeederId();
                        lDs = lReportService.Danesh_GetMultiStepConnection(ch.lFromDate, ch.lToDate, ch.lAreaId, ch.lMPFeederId);
                        break;
                    case "sanjesh_avgunwanted":
                        ch.check_From()
                            .check_To();
                        lDs = lReportService.SANJESH_AVGUNWANTED(ch.lFromDate, ch.lToDate);
                        break;
                    case "sanjesh_avgwanted":
                        ch.check_From()
                            .check_To();
                        lDs = lReportService.SANJESH_AVGWANTED(ch.lFromDate, ch.lToDate);
                        break;
                    case "sanjesh_highpostoff":
                        ch.check_From()
                            .check_To();
                        lDs = lReportService.SANJESH_HIGHPOSTOFF(ch.lFromDate, ch.lToDate);
                        break;
                    case "gmaz_getlppostload":
                        lDs = null;
                        lFromDate = "";
                        lToDate = "";
                        ch.check_From()
                            .check_To()
                            .check_LocalCode()
                            .check_LPPostCode()
                            .check_RecCount();
                        lRes = lReportService.GMaz_GetLPPostLoad(ch.lLocalCode, ch.lLPPostCode, ch.lFromDate, ch.lToDate, ch.lCount);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    case "gmaz_report_sabz":
                        lDs = null;
                        ch.check_From()
                            .check_To();
                        lRes = lReportService.GMaz_Report_14_1(ch.lFromDate, ch.lToDate);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    case "gmaz_unstable_feeders":
                        lDs = null;
                        ch.check_From()
                            .check_To()
                            .check_MinCount();
                        lRes = lReportService.GMaz_Unstable_Feeders(ch.lFromDate, ch.lToDate, ch.lMinCount);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    case "azargh_feederpeak":
                        lDs = null;
                        ch.lParams.FromDate += "/01";
                        ch.lParams.ToDate += "/31";
                        ch.check_From()
                            .check_To()
                            .check_Code();
                        lRes = lReportService.AzarGH_FeederPeak_Monthly(ch.lFromDate, ch.lToDate, ch.lCode);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    case "azargh_postfeederload":
                        lDs = null;
                            ch.check_MinCount(3)
                            .check_Code();
                        lRes = lReportService.AzarGH_PostFeederLoad(ch.lCode , ch.lMinCount);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    case "khornorth_lppostloadbalance":
                        lDs = null;
                        ch.check_From()
                            .check_To();

                        if (string.IsNullOrWhiteSpace(lParams.Code))
                            lParams.Code = "";

                        lRes = lReportService.KHorNorth_LPPostLoadBalance(lParams.Code, ch.lFromDate, ch.lToDate);
                        lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    case "skerman_dashboardstat":
                        lDs = null;
                        int apiNum = Convert.ToInt16(lInformType.Replace("skerman_dashboardstat", ""));
                        ch.check_From()
                            .check_To();
                        lRes = lReportService.SKerman_DashboardStat(ch.lFromDate, ch.lToDate, apiNum);
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
                    lResult = mdl_Publics.GetJSonString(aDs.Tables["Tbl_Error"]);
                else
                    lResult = "no result";
            }
            catch { }
            return lResult;
        }
        //----------omid------------
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
            public string lLPPostCode = "";
            public string lLocalCode = "";
            public int? lCount = -1;
            public ReportParams lParams;

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
            public Checker check_LPPostId()
            {
                if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                    lLPPostId = int.Parse(lParams.LPPostId);
                return this;
            }
            public Checker check_MPPostId()
            {
                if (!string.IsNullOrWhiteSpace(lParams.LPPostId))
                    lMPPostId = int.Parse(lParams.MPPostId);
                return this;
            }
            public Checker check_MPFeederId()
            {
                if (!string.IsNullOrWhiteSpace(lParams.MPFeederId))
                    lMPFeederId = int.Parse(lParams.MPFeederId);
                return this;
            }
            public Checker check_MinCount(int aMinCount = -1)
            {
                if (string.IsNullOrWhiteSpace(lParams.MinCount) && aMinCount > -1)
                    lMinCount = aMinCount;
                lMinCount = lMinCount > -1 ? lMinCount : int.Parse(lParams.MinCount);
                return this;
            }
            public Checker check_LPPostCode()
            {
                if (!string.IsNullOrWhiteSpace(lParams.LPPostCode))
                    lLPPostCode = lParams.LPPostCode;
                return this;
            }
            public Checker check_LocalCode()
            {
                if (!string.IsNullOrWhiteSpace(lParams.LocalCode))
                    lLocalCode = lParams.LocalCode;
                return this;
            }
            public Checker check_RecCount()
            {
                if (lParams.RecCount != null && lParams.RecCount > -1)
                    lCount = lParams.RecCount;
                return this;
            }
        }
    }
}