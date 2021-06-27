using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TZServicesCSharp.Classes;
using System.Text.RegularExpressions;
using System.Data;
using TZServiceTools;

namespace TZServicesCSharp.RestServices
{
    public partial class HInforms : CPageBase
    {
        private class InformParams
        {
            public string TelNo { get; set; }
            public string FileNo { get; set; }
            public string BillingId { get; set; }//----omid
            public int BackTimeMinutes { get; set; }
			public int BackDays { get; set; }
            public string SurveyReplyCode { get; set; }
            public string CallId { get; set; }
            public string AgentNo { get; set; }
            public string MobileNo { get; set; }
            public string Name { get; set; }
            public bool IsSMS { get; set; }
            public string Address { get; set; }
            public string TrackingCode { get; set; }
            public long UniqueId { get; set; }
            public string Result { get; set; }
			public int CityId { get; set; }
			public string Code { get; set; }
			public string OutageDate { get; set; }
			public int AreaId { get; set; }
			public string ShamsiDateFrom { get; set; }
			public string ShamsiDateTo { get; set; }
			public long RequestNumber { get; set; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string lResult = "";
            HavadesInform lInformService = new HavadesInform();

            string lInputData = "";
            string lInformType = "";

            try
            {
                lInformType = GetQueryString("InformType");

                string lMethodNames =
                    "^(getsubscriberoutagebytelno|getsubscriberoutagebyfileno|setccsurveyresult|getzoneoutagevoicesfromtelno" +
                    "|getcalltrackingcode|sendguidesms|sendoutageinfosms|registersubscriberforoutageinfo" +
					"|getsubscriberoutagebytrackingcode|getoutagelistfromaddress|getoutboundcall|endoutboundcall" +
					"|getfaximage|getsubscriberoutagelistbycode|getoutagelistbycitydate|getzoneoutagelistfromtelno" +
					"|getsubscriberpostfeederbyfileno|getsubscriberpostfeederbycode|getivrcodebytelno|getoutagesubscriberlist"+
                    "|getsubscriberoutagelistbybillmob)$";

                string lNoParamMethods =
                    "^(getoutboundcall)$";
                bool lIsParamMethod = !Regex.IsMatch(lInformType, lNoParamMethods, RegexOptions.IgnoreCase);

                if (!Regex.IsMatch(lInformType, lMethodNames, RegexOptions.IgnoreCase))
                    throw new Exception("درخواست شما معتبر نمی باشد");

                lInputData = GetQueryStringOrFirstParameter("param");
                InformParams lParams = mdl_Publics.GetClassFromJson<InformParams>(lInputData);

                if (lIsParamMethod && lParams == null)
                    throw new Exception("پارامتر ورودی نامعتبر است");

                mdl_Publics.LogMessage(Server,
                    string.Format("HavadesInform({0}) REST input data: {1}", lInformType, lInputData),
                    "TZServices_Inform_Logs");

                InformResult lInformResult = null;

				switch (lInformType.ToLower())
				{
					case "getsubscriberoutagebytelno":
						if (string.IsNullOrWhiteSpace(lParams.TelNo))
							throw new Exception("TelNo is not specified");

						SubscriberOutageResult lSOResT = lInformService.GetSubscriberOutageInfoByTelNo(lParams.TelNo, lParams.BackTimeMinutes);
						lResult = mdl_Publics.GetJSonString(lSOResT);
						break;
					case "getivrcodebytelno":
						if (string.IsNullOrWhiteSpace(lParams.TelNo))
							throw new Exception("TelNo is not specified");

						lInformResult = lInformService.GetIVRCodeFromTelNo(lParams.TelNo, lParams.BackDays);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "getsubscriberoutagebyfileno":
						if (string.IsNullOrWhiteSpace(lParams.FileNo))
							throw new Exception("FileNo is not specified");

						SubscriberOutageResult lSOResF = lInformService.GetSubscriberOutageInfoByFileNo(lParams.FileNo, lParams.BackTimeMinutes);
						lResult = mdl_Publics.GetJSonString(lSOResF);
						break;
					case "getsubscriberoutagelistbycode":
						if (string.IsNullOrWhiteSpace(lParams.Code))
							throw new Exception("Code is not specified");

						ServiceLog lSLog = null;
						OutageListResult lOLR = lInformService.GetSubscriberOutageListByFileNo(lParams.Code, out lSLog);
						lResult = mdl_Publics.GetJSonString(lOLR);
						break;
					case "getsubscriberpostfeederbycode":
						if (string.IsNullOrWhiteSpace(lParams.Code))
							throw new Exception("Code is not specified");

						NetworkCodeInfo lNci = lInformService.GetSubscriberPostFeederByFileNo(lParams.Code);
						lResult = mdl_Publics.GetJSonString(lNci);
						break;
					case "getsubscriberoutagebytrackingcode":
						if (string.IsNullOrWhiteSpace(lParams.TrackingCode))
							throw new Exception("TrackingCode is not specified");

						SubscriberOutageResult lSOResTC = lInformService.GetSubscriberOutageInfoByTrackingCode(lParams.TrackingCode);
						lResult = mdl_Publics.GetJSonString(lSOResTC);
						break;
					case "setccsurveyresult":
						if (string.IsNullOrWhiteSpace(lParams.TelNo))
							throw new Exception("TelNo is not specified");
						else if (string.IsNullOrWhiteSpace(lParams.SurveyReplyCode))
							throw new Exception("SurveyReplyCode is not specified");

						if (string.IsNullOrWhiteSpace(lParams.AgentNo))
							lParams.AgentNo = "";

						if (lParams.IsSMS == null)
							lParams.IsSMS = false;

						lInformResult = lInformService.SetCCSurveyResult(lParams.IsSMS, lParams.CallId, lParams.AgentNo, lParams.TelNo, lParams.SurveyReplyCode);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "getzoneoutagevoicesfromtelno":
						if (string.IsNullOrWhiteSpace(lParams.TelNo))
							throw new Exception("TelNo is not specified");

						OutageVoiceListResult lOVLRes = lInformService.GetZoneOutageVoicesFromTelNo(lParams.TelNo);
						lResult = mdl_Publics.GetJSonString(lOVLRes);
						break;
					case "getzoneoutagelistfromtelno":
						if (string.IsNullOrWhiteSpace(lParams.TelNo))
							throw new Exception("TelNo is not specified");

						OutageListResult lOLTRes = lInformService.GetZoneOutageListFromTelNo(lParams.TelNo);
						lResult = mdl_Publics.GetJSonString(lOLTRes);
						break;
					case "getcalltrackingcode":
						if (string.IsNullOrWhiteSpace(lParams.CallId))
							throw new Exception("CallId is not specified");

						lInformResult = lInformService.GetCallTrackingCode(lParams.CallId);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "sendguidesms":
						if (string.IsNullOrWhiteSpace(lParams.TelNo))
							throw new Exception("TelNo is invalid");

						lInformResult = lInformService.SendGuideSMS(lParams.TelNo);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "sendoutageinfosms":
						if (string.IsNullOrWhiteSpace(lParams.FileNo))
							throw new Exception("FileNo is not specified");
						if (string.IsNullOrWhiteSpace(lParams.MobileNo))
							throw new Exception("MobileNo is not specified");

						lInformResult = lInformService.SendOutageInfoSMS(lParams.MobileNo, lParams.FileNo);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "registersubscriberforoutageinfo":
						if (string.IsNullOrWhiteSpace(lParams.Name))
							throw new Exception("Name is not specified");
						if (string.IsNullOrWhiteSpace(lParams.Code))
							throw new Exception("Code is not specified");
						if (string.IsNullOrWhiteSpace(lParams.MobileNo))
							throw new Exception("MobileNo is not specified");

						lInformResult = lInformService.RegisterSubscriberForOutageInfo(lParams.Name, lParams.Code, lParams.MobileNo);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "getoutagelistfromaddress":
						if (lParams.CityId == null || lParams.CityId <= 0)
							if (lParams.AreaId == null || lParams.AreaId <= 0)
								throw new Exception("CityId or AreaId must be specify");

						string lPDateFrom = "", lPDateTo = "";
						if (lParams.ShamsiDateFrom != null)
							lPDateFrom = lParams.ShamsiDateFrom;
						if (lParams.ShamsiDateTo != null)
							lPDateTo = lParams.ShamsiDateTo;

						OutageListResult lOLRes = lInformService.GetOutageInfoFromAddress(lParams.Address, lParams.CityId, lParams.AreaId, lPDateFrom, lPDateTo);
						lResult = mdl_Publics.GetJSonString(lOLRes);
						break;
					case "getoutagelistbycitydate":
						if (lParams.CityId == null || lParams.CityId <= 0)
							throw new Exception("CityId is not specified");
						if (string.IsNullOrWhiteSpace(lParams.OutageDate))
							throw new Exception("OutageDate must be specify");

						OutageListResult lOLRCityDate = lInformService.GetOutageListByCityDate(lParams.OutageDate, lParams.CityId);
						lResult = mdl_Publics.GetJSonString(lOLRCityDate);
						break;
					case "getoutboundcall":
						OutboundCallInfoResult lOCRes = lInformService.GetOutboundCall();
						lResult = mdl_Publics.GetJSonString(lOCRes);
						break;
					case "endoutboundcall":
						if (lParams.UniqueId == 0)
							throw new Exception("UniqeId parameter is invalid.");
						if (string.IsNullOrWhiteSpace(lParams.Result))
							throw new Exception("Result parameter is not specified");

						int lResultId; int.TryParse(lParams.Result, out lResultId);

						lInformResult = lInformService.EndFaxOrOutboundCall(lParams.UniqueId, lResultId);
						lResult = mdl_Publics.GetJSonString(lInformResult);
						break;
					case "getfaximage":
						if (lParams.UniqueId == 0)
							throw new Exception("No Fax Image Found.");
						OutboundFaxResult lOFRes = lInformService.GetFaxImage(lParams.UniqueId);
						lResult = mdl_Publics.GetJSonString(lOFRes);
						break;
					case "getoutagesubscriberlist":
						if (lParams.RequestNumber == null)
							throw new Exception("شماره خاموشی مشخص نشده است");

						ExternalLinks lExLink = new ExternalLinks();
						GISResult lGR = lExLink.GetOutageSubscribersFromGIS(lParams.RequestNumber);
						lResult = mdl_Publics.GetJSonString(lGR);
						break;
                    case "getsubscriberoutagelistbybillmob": //---------------omid
                        bool lIsNok = string.IsNullOrWhiteSpace(lParams.BillingId)
                            && string.IsNullOrWhiteSpace(lParams.MobileNo);
                        if (lIsNok) throw new Exception("ورودی برای یافتن پست و فیدر یافت نشد.");

                        OutageListResult lResList = lInformService.GetSubscriberOutageListByBillMobAddr(
                            lParams.BillingId, lParams.MobileNo);
                        lResult = mdl_Publics.GetJSonString(lResList);
                        break;
                    default:
						throw new Exception("درخواست شما معتبر نمی باشد");
				}
            }
            catch (Exception ex)
            {
                InformResult lRes = new InformResult();
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
                lResult = mdl_Publics.GetJSonString(lRes);
            }

            TZServiceTools.mdl_Publics.LogMessage(Server,
                string.Format("HavadesInform({0}) REST Result: {1}", lInformType, lResult),
                "TZServices_Inform_Logs");

            Response.ContentType = "application/json";
            Response.Write(lResult);
        }
    }
}