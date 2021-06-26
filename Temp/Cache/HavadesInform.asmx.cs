using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using TZServiceTools;
using TZServicesLib;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using TZServicesCSharp.Classes;

namespace TZServicesCSharp
{
	#region Classes

	public class InformResult
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public HomaData Data { get; set; }
    }
    public class NetworkInfo
    {
        public string MPPostCode { get; set; }
        public string MPFeederCode { get; set; }
        public string LPPostCode { get; set; }
        public string LPFeederCode { get; set; }
        public string BillingId { get; set; }
    }
    public class OutageData
    {
        public string RequestNumber { get; set; }
        public string BeginDate { get; set; }
        public string BeginTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public bool IsPlanned { get; set; }
        public string Desciption { get; set; }
        public string Address { get; set; }
        public OutageStatus Status { get; set; }
    }
    public class OutageListResult: InformResult
    {
        public List<OutageData> OutageList { get; set; }
    }

    public class SubscriberOutageInfo
    {
        public OutageStatus Status { get; set; }
        public string BeginDate { get; set; }
        public string BeginTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public bool IsPlanned { get; set; }
        public double DebtPrice { get; set; }
        public string BillingId { get; set; }
    }
    public class SubscriberOutageResult: InformResult
    {
        public SubscriberOutageInfo OutageInfo { get; set; }
    }

    // subscriber tamir outage list -- start
    public class TamirData
    {
        public string BeginDate { get; set; }
        public string BeginTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string VoiceCode { get; set; }
        public string VoiceData { get; set; }
        public string OutageNumber { get; set; }
    }


    public class TamirListResult : InformResult
    {
        public List<TamirData> OutageList { get; set; }
    }

    // subscriber tamir outage list -- end

    public class VoiceInfo
    {
        public string VoiceId { get; set; }
        public string ConnectTime { get; set; }
        public string VoiceData { get; set; }

		public VoiceInfo()
		{
			VoiceId = "0";
			ConnectTime = "";
			VoiceData = "";
		}
    }
    public class OutageVoiceListResult: InformResult
    {
        public List<VoiceInfo> VoiceList{get;set;}
    }

    public class BillingVoiceData
    {
        public string VoiceId { get; set; }
        public string VoiceCode { get; set; }
        public string VoiceData { get; set; }
    }

    public class BillingVoiceResult
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public BillingVoiceData VoiceInfo { get; set; }
    }

    public class SubscriberStatusData
    {
        public bool IsActive { get; set; }
        public string DebtPrice { get; set; }
    }

    public class SubscriberStatusResult
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public SubscriberStatusData Status { get; set; }
    }

    public class OutboundCallInfo
    {
        public long Id { get; set; }
        public string PhoneNo { get; set; }
        public string TrunkExtNo { get; set; }
        public int WaitTime { get; set; }
        public int CallType { get; set; }
        //public long FileServerId { get; set; }
        public string OutageDate { get; set; }
        public string OutageTimeStart { get; set; }
        public string OutageTimeEnd { get; set; }
    }
    public class OutboundCallInfoResult : InformResult
    {
        public OutboundCallInfo CallInfo { get; set; }
    }
    public class OutboundFaxResult : InformResult
    {
        public string FaxData { get; set; }
    }

    public enum BillingTypes
    {
        FileNo = 1,
        SubNo = 2,
        RamzCode = 3,
        BillingId = 4,
        TelNo = 5
    }
    public enum OutageStatus
    {
        OutageForDept = -1,
        NoOutage = 0,
        Done = 1,
        NewOutage = 2,
        WorkingOutage = 3,
        UndoneOutage = 4,
        ReferTo = 5,
        NoPopupEvent = 6,
    }

    public class BillingIDTypeList
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public List<BillingIDTypeData> Data { get; set; }
    }

    public class BillingIDTypeData
    {
        public int BillingIDTypeId { get; set; }
        public string BillingIDType { get; set; }
    }

    public class GetSubscriberBillingIDs
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public List<SubscriberBilling> BillingList { get; set; }
    }

    public class SMSResult
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
    }

    public class SubscriberBilling
    {
        public string BillingId { get; set; }
        public int BillingIDTypeId { get; set; }
        public string BillingIDType { get; set; }
    }

    public class HomaData
    {
        public long RegisterId { get; set; }
        public string SubscriberName { get; set; }
        public string MelliCode { get; set; }
        public string MobileNo { get; set; }
        public string TelNo { get; set; }
        public DateTime RegisterDT { get; set; }
        public string RegisterDatePersian { get; set; }
        public string RegisterTime { get; set; }
        public string Comments { get; set; }
        public string RegisterStatus { get; set; }

        public long RegisterBillingIDId { get; set; }
        public string BillingID { get; set; }
        public int BillingIDTypeId { get; set; }
        public string BillingIDType { get; set; }
        public string Address { get; set; }
        public string AddressInBilling { get; set; }
        public int LPPostId { get; set; }
        public int LPFeederId { get; set; }
        public float? GpsX { get; set; }
        public float? GpsY { get; set; }
    }

    public class SubscriberRegisterInfo
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public long RegisterId { get; set; }
        public string SubscriberName { get; set; }
        public string MelliCode { get; set; }
        public string MobileNo { get; set; }
        public string TelNo { get; set; }
        public DateTime RegisterDT { get; set; }
        public string RegisterDatePersian { get; set; }
        public string RegisterTime { get; set; }
        public string Comments { get; set; }
        public string RegisterStatus { get; set; }
    }


    public class SubscriberBillingInfo
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public long RegisterBillingIDId { get; set; }
        public long RegisterId { get; set; }
        public DateTime RegisterDT { get; set; }
        public string BillingID { get; set; }
        public int BillingIDTypeId { get; set; }
        public string BillingIDType { get; set; }
        public string Comments { get; set; }
        public string SubscriberName { get; set; }
        public string Address { get; set; }
        public string AddressInBilling { get; set; }
        public int LPPostId { get; set; }
        public int LPFeederId { get; set; }
        public int AreaId { get; set; }
        public float? GpsX { get; set; }
        public float? GpsY { get; set; }
    }

    public class HomaResult
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public object Data { get; set; }
    }

    public class SubscriberInfoParams
    {
        public int? MediaTypeId { get; set; }
        public string CallerId { get; set; }
        public string SubscriberName { get; set; }
        public string MelliCode { get; set; }
        public string MobileNo { get; set; }
        public string TelNo { get; set; }
        public string Password { get; set; }
        public int? RegisterStatusId { get; set; }
        public string RegComments { get; set; }
        public string BillingID { get; set; }
        public int? BillingIDTypeId { get; set; }
        public string BillingComments { get; set; }
        public string BillingSubscriberName { get; set; }
        public string Address { get; set; }
        public string AddressInBilling { get; set; }
        public int? LPPostId { get; set; }
        public int? LPFeederId { get; set; }
        public bool? IsInformPlannedOutages { get; set; }
        public float? GpsX { get; set; }
        public float? GpsY { get; set; }
        public bool IsSkipTelMobile { get; set; }
    }

    public class BillingInfoResult
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public List<BillingInfoData> Data { get; set; }
    }

    public class BillingInfoData
    {
        public string BillingID { get; set; }
        public string Address { get; set; }
        public string SubscriberName { get; set; }
    }

	#endregion

	/// <summary>
    /// Summary description for HavadesInform
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HavadesInform : System.Web.Services.WebService
    {
        static object lockMethod = new object();
        
        SqlConnection mCnn = new SqlConnection(mdl_Publics.mConnectionString);
        DataSet mDs = new DataSet();

        [WebMethod(Description="دریافت فهرست خاموشی‌های جاری و آینده روی شبکه معرفی شده")]
        public OutageListResult GetOutageInfo(NetworkInfo NetworkData)
        {
			SaveLog(String.Format("Calling GetOutageInfo(NetworkData={0})", mdl_Publics.GetJSonString(NetworkData)));
            string lMsg = "";

			DataSet lDs = new DataSet();
            NetworkInfo lParams = NetworkData;
            List<OutageData> lOutageList = new List<OutageData>();
            OutageListResult lRes = new OutageListResult();
            lRes.IsSuccess = true;
            lRes.ResultMessage = "";
            lRes.OutageList = lOutageList;

            try
            {
                string lSQL = string.Format(
                    "EXEC spGetOutageInfoFromNetwork " +
                    "@aMPPostCode = '{0}', @aMPFeederCode = '{1}', @aLPPostCode = '{2}', @aLPFeederCode = '{3}'",
                    lParams.MPPostCode, lParams.MPFeederCode, lParams.LPPostCode, lParams.LPFeederCode);
				
				SaveLog(string.Format("Executing query: {0}", lSQL));
                lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewOutageInfo", aIsClearTable: true);

                if (lDs.Tables.Contains("ViewOutageInfo") && lDs.Tables["ViewOutageInfo"].Rows.Count > 0)
                {
                    lRes.ResultMessage = string.Format("تعداد {0} خاموشی یافت شد", lDs.Tables["ViewOutageInfo"].Rows.Count);
                    foreach (DataRow lRow in lDs.Tables["ViewOutageInfo"].Rows)
                    {
                        OutageData lInfo = new OutageData();
                        lInfo.RequestNumber = lRow["RequestNumber"].ToString();
                        lInfo.BeginDate = lRow["DisconnectDatePersian"].ToString();
                        lInfo.BeginTime = lRow["DisconnectTime"].ToString();
                        lInfo.EndDate = lRow["ConnectDatePersian"].ToString();
                        lInfo.EndTime = lRow["ConnectTime"].ToString();
                        lInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
                        lInfo.Address = lRow["Address"].ToString();
                        try
                        {
                            OutageStatus lStatus = (OutageStatus)Convert.ToInt32(lRow["Status"]);
                            lInfo.Status = lStatus;

                        }
                        catch (Exception)
                        { }

                        lInfo.Desciption = "";

                        if (string.IsNullOrWhiteSpace(lInfo.EndTime))
                            lInfo.EndTime = "نامعلوم";

                        lOutageList.Add(lInfo);
                    }
                }
                else
                {
                    lRes.ResultMessage = "هیچ خاموشی یافت نشد";
                    if (lMsg.Length > 0)
                        lRes.ResultMessage += ": " + lMsg;
                }
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(String.Format("GetOutageInfo Result: ({0})", mdl_Publics.GetJSonString(lRes)));
            return lRes;
        }

		[WebMethod(Description = "دریافت فهرست خاموشی‌های جاری و آینده از روی آدرس")]
		public OutageListResult GetOutageInfoFromAddress(string Address, int CityId, int AreaId, string aPDateFrom = "", string aPDateTo = "")
		{
			if (string.IsNullOrWhiteSpace(Address)) Address = "";
			if (CityId == 0) CityId = -1;
			if (AreaId == 0) AreaId = -1;
			SaveLog(String.Format("Calling GetOutageInfoFromAddress(Address={0},CityId={1},AreaId={2},PDateFrom={3},PDateTo={4})", Address, CityId, AreaId, aPDateFrom, aPDateTo));
			string lMsg = "";

			List<OutageData> lOutageList = new List<OutageData>();
			OutageListResult lRes = new OutageListResult();
			lRes.IsSuccess = true;
			lRes.ResultMessage = "";
			lRes.OutageList = lOutageList;

			try
			{
				string lAddress = Address.Replace(" ", ",");
				lAddress = lAddress.Replace(",,", ",");

				string lSQL = "";

				if (!string.IsNullOrWhiteSpace(lAddress))
				{
					lSQL = string.Format(
						 "EXEC spGetOutageByMPFeederAddress @aAddress = '{0}', @aCityId = {1}, @aAreaId = {2}, @aShamsiDateFrom = '{3}', @aShamsiDateTo = '{4}'",
						 lAddress, CityId, AreaId, aPDateFrom, aPDateTo);
				}
				else
				{
					lSQL = string.Format(
						"EXEC spGetOutageByCityArea @aCityId = {0}, @aAreaId = {1}, @aShamsiDateFrom = '{2}', @aShamsiDateTo = '{3}'",
						CityId, AreaId, aPDateFrom, aPDateTo);
				}

				SaveLog("Executing Query: " + lSQL);
				lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewOutageInfo", aIsClearTable: true);
				if (lMsg.Length > 0)
					SaveLog("Error: " + lMsg);

				if (mDs.Tables.Contains("ViewOutageInfo") && mDs.Tables["ViewOutageInfo"].Rows.Count > 0)
				{
					string lNoTime = CConfig.ReadConfig("UnKnownConnectTime", "").ToString();
					if (string.IsNullOrWhiteSpace(lNoTime))
						lNoTime = "نامعلوم";
					else
						lNoTime = string.Format("<span class=\"footnotesign\">{0}</span>", lNoTime);

					string lNoTimeMsg = CConfig.ReadConfig("UnKnownConnectTimeMessage", "").ToString();
					if (string.IsNullOrWhiteSpace(lNoTimeMsg))
						lNoTimeMsg = "";

					lRes.ResultMessage = string.Format("تعداد {0} خاموشی یافت شد", mDs.Tables["ViewOutageInfo"].Rows.Count);
					foreach (DataRow lRow in mDs.Tables["ViewOutageInfo"].Rows)
					{
						OutageData lInfo = new OutageData();
						lInfo.RequestNumber = lRow["RequestNumber"].ToString();
						lInfo.BeginDate = lRow["DisconnectDatePersian"].ToString();
						lInfo.BeginTime = lRow["DisconnectTime"].ToString();
						lInfo.EndDate = lRow["ConnectDatePersian"].ToString();
						lInfo.EndTime = lRow["ConnectTime"].ToString();
						lInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
						lInfo.Desciption = "";
						lInfo.Address = lRow["Address"].ToString();

						if (string.IsNullOrWhiteSpace(lInfo.EndTime))
							lInfo.EndTime = lNoTime;

						lOutageList.Add(lInfo);
					}
				}
				else
				{
					lRes.ResultMessage = "هیچ خاموشی یافت نشد";
					if (lMsg.Length > 0)
						lRes.ResultMessage += ": " + lMsg;
				}
			}
			catch (Exception ex)
			{
				lRes.IsSuccess = false;
				lRes.ResultMessage = ex.Message;
			}

			SaveLog(String.Format("GetOutageInfoFromAddress Result ({0},{1},{2}): {3}", Address, CityId, AreaId, mdl_Publics.GetJSonString(lRes)));
			return lRes;
		}

        [WebMethod(Description="دریافت فهرست خاموشی‌های مربوط به شهرستان مورد نظر در تاریخ خاص")]
        public OutageListResult GetOutageListByCityDate(string OutageDate, int CityId)
        {
			SaveLog(String.Format("Calling GetOutageListByCityDate(OutageDate={0},CityId={1})", OutageDate, CityId));
            string lMsg = "";

            List<OutageData> lOutageList = new List<OutageData>();
            OutageListResult lRes = new OutageListResult();
            lRes.IsSuccess = true;
            lRes.ResultMessage = "";
            lRes.OutageList = lOutageList;

            try
            {

                string lSQL = string.Format(
                    "EXEC spGetOutageByCityDate @aOutageDate = '{0}', @aCityId = {1} ",
                    OutageDate, CityId);

                lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewOutageInfo", aIsClearTable: true);
                if (mDs.Tables.Contains("ViewOutageInfo") && mDs.Tables["ViewOutageInfo"].Rows.Count > 0)
                {
                    lRes.ResultMessage = string.Format("تعداد {0} خاموشی یافت شد", mDs.Tables["ViewOutageInfo"].Rows.Count);
                    foreach (DataRow lRow in mDs.Tables["ViewOutageInfo"].Rows)
                    {
                        OutageData lInfo = new OutageData();
                        lInfo.RequestNumber = lRow["RequestNumber"].ToString();
                        lInfo.BeginDate = lRow["DisconnectDatePersian"].ToString();
                        lInfo.BeginTime = lRow["DisconnectTime"].ToString();
                        lInfo.EndDate = lRow["ConnectDatePersian"].ToString();
                        lInfo.EndTime = lRow["ConnectTime"].ToString();
                        lInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
                        lInfo.Desciption = "";
                        lInfo.Address = lRow["Address"].ToString();

                        if (string.IsNullOrWhiteSpace(lInfo.EndDate))
                            lInfo.EndDate = "نامعلوم";
                        if (string.IsNullOrWhiteSpace(lInfo.EndTime))
                            lInfo.EndTime = "نامعلوم";

                        lOutageList.Add(lInfo);
                    }
                }
                else
                {
                    lRes.ResultMessage = "هیچ خاموشی یافت نشد";
                    if (lMsg.Length > 0)
                        lRes.ResultMessage += ": " + lMsg;
                }
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

			SaveLog(String.Format("GetOutageListByCityDate Result ({0},{1}): {2}", OutageDate, CityId, mdl_Publics.GetJSonString(lRes)));
            return lRes;
        }

        //دریافت پست و فيدر مشترک از روی شماره تلفن
        public SubscriberGISInfo GetSubscriberNetworkByTelNo(string TelNo)
        {
            SaveLog(String.Format("Calling GetSubscriberNetworkByTelNo(TelNo={0})", TelNo));
            SubscriberGISInfo lSGi = GetSubscriberNetwork(TelNo, BillingTypes.TelNo);
			return lSGi;
        }

        [WebMethod(Description = "دریافت وضعیت خاموشی جاری مشترک بر اساس شماره تماس")]
        public SubscriberOutageResult GetSubscriberOutageInfoByTelNo(string TelNo, int BackTimeMinutes)
        {
            SaveLog(String.Format("Calling GetSubscriberOutageInfoByTelNo(TelNo={0},BackTimeMinutes={1})", TelNo, BackTimeMinutes));
            SubscriberOutageResult lOutageResult = new SubscriberOutageResult();
            lOutageResult.OutageInfo = null;
            lOutageResult.IsSuccess = false;
            SubscriberOutageInfo lOutageInfo = null;

            try
            {
				string lSQL = string.Format("EXEC spGetSubscriberOutageInfo @CallerId = '{0}', @BackTime = {1}", 
					TelNo, BackTimeMinutes);
                SaveLog("Executing query: " + lSQL);

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewOutageInfo", aIsClearTable: true);
				if (mDs.Tables.Contains("ViewOutageInfo") && mDs.Tables["ViewOutageInfo"].Rows.Count > 0)
				{
					SaveLog(string.Format("Query Result Count: {0}", mDs.Tables["ViewOutageInfo"].Rows.Count));
					DataRow lRow = mDs.Tables["ViewOutageInfo"].Rows[0];

					OutageStatus lStatus = (OutageStatus)Convert.ToInt32(lRow["Status"]);

					lOutageInfo = new SubscriberOutageInfo();
					lOutageInfo.Status = lStatus;
					if (lRow["IsTamir"] != DBNull.Value)
						lOutageInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
					else
						lOutageInfo.IsPlanned = false;
					lOutageInfo.BeginDate = lRow["DisconnectDate"].ToString();
					lOutageInfo.BeginTime = lRow["DisconnectTime"].ToString();
					lOutageInfo.EndDate = lRow["ConnectDate"].ToString();
					lOutageInfo.EndTime = lRow["ConnectTime"].ToString();
					lOutageInfo.DebtPrice = 0;
				}

                if (lOutageInfo == null || lOutageInfo.Status == OutageStatus.NoOutage)
                {
                    SaveLog(string.Format("! No Query Result !"));
                    SaveLog(string.Format("Fetching from Billing..."));

                    GetSubscriberBillingIDs lSubscriberBillingIDs = GetSubscriberBillings(TelNo);
                    SubscriberInfo lSi = new SubscriberInfo();

                    if (lSubscriberBillingIDs.BillingList != null && lSubscriberBillingIDs.BillingList.Count > 0)
                    {
                        SubscriberBilling lSubscriberBilling = new SubscriberBilling();
                        lSubscriberBilling = lSubscriberBillingIDs.BillingList[0];

                        lSi = GetSubscriberInfoFromBilling(lSubscriberBilling.BillingId, BillingTypes.BillingId);
                        if (lSi != null)
                        {
                            SaveLog(string.Format("Billing Result: {0}", mdl_Publics.GetJSonString(lSi)));
                            lOutageInfo = new SubscriberOutageInfo();

                            if (lSi.Status == 1035)
                                lOutageInfo.Status = OutageStatus.OutageForDept;
                            else if (lOutageInfo.Status == OutageStatus.OutageForDept)
                                lOutageInfo.Status = OutageStatus.NoOutage; //سامانه جامع مشترکین اعلام قطع به علت بدهی ننموده است
                        }
                        else
                            SaveLog("! No Billing Result !");
                    }

                    //if (lOutageInfo == null || lOutageInfo.Status == OutageStatus.NoOutage)
                    //{

                    //    string lToziName = System.Configuration.ConfigurationManager.AppSettings["ToziName"];
                    //    if (string.IsNullOrWhiteSpace(lToziName))
                    //        lToziName = "";
                    //    lToziName = lToziName.Trim().ToLower();

                    //    SaveLog(string.Format("Fetching from GIS (ToziName: {0})...", lToziName));

                    //    if (lToziName == "jonoobkerman" || lToziName == "shomalkerman")
                    //    {
                    //        SaveLog(string.Format("Step 1: GetSubscriberNetworkByTelNo"));
                    //        SubscriberGISInfo lSGi = GetSubscriberNetworkByTelNo(TelNo);
                    //        SaveLog(string.Format("Step1 1 Result: {0}", mdl_Publics.GetJSonString(lSGi)));

                    //        SaveLog(string.Format("Step 2: GetOutagesByGISNetwork"));
                    //        OutageListResult lOLR = GetOutagesByGISNetwork(lSGi);
                    //        SaveLog(string.Format("Step1 2 Result: {0}", mdl_Publics.GetJSonString(lOLR)));

                    //        SaveLog(string.Format("Step 3: GetCurrentOutageFromList"));
                    //        lOutageInfo = GetCurrentOutageFromList(lOLR);
                    //        SaveLog(string.Format("Step1 3 Result: {0}", mdl_Publics.GetJSonString(lOutageInfo)));
                    //    }
                    //}

                    else
                        SaveLog("lOutageInfo is not null.");

                    if (lOutageInfo == null)
                    {
                        lOutageInfo = new SubscriberOutageInfo()
                        {
                            Status = OutageStatus.NoOutage
                        };
                    }

                    if (lSi != null)
                        lOutageInfo.DebtPrice = lSi.DebtValue;
                }

				lOutageResult.OutageInfo = lOutageInfo;
                lOutageResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                lOutageResult.IsSuccess = false;
                lOutageResult.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetSubscriberOutageInfoByTelNo Result ({0}): {1}", TelNo, mdl_Publics.GetJSonString(lOutageResult)));

            return lOutageResult;
        }

        [WebMethod(Description = "دریافت وضعیت خاموشی جاری مشترک بر اساس شماره پرونده او")]
        public SubscriberOutageResult GetSubscriberOutageInfoByFileNo(string FileNo, int BackTimeMinutes)
        {
            string lMsg = "";
            SaveLog(String.Format("Calling GetSubscriberOutageInfoByFileNo(FileNo={0},BackTimeMinutes={1})", FileNo, BackTimeMinutes));
            SubscriberOutageResult lOutageResult = new SubscriberOutageResult();
            lOutageResult.OutageInfo = null;
            lOutageResult.IsSuccess = false;
            SubscriberOutageInfo lOutageInfo = new SubscriberOutageInfo();

            bool lIsFound = false;

            string lSQL = "";
            try
            {
                SubscriberInfo lSi = GetSubscriberInfoFromBilling(FileNo, CStatics.BillingType);
                if (lSi.ErrorMessage.Length > 0)
                    lSi = null;

                if (lSi != null && lSi.Status == 1035) //قطع به علت بدهی
                {
                    lIsFound = true;
                    lOutageInfo.DebtPrice = lSi.DebtValue;
                    lOutageInfo.Status = OutageStatus.OutageForDept;
                    lOutageResult.OutageInfo = lOutageInfo;
                    lOutageResult.IsSuccess = true;
                }
                else
                {

                    lSQL = string.Format("EXEC spGetSubscriberOutageInfoByFileNo @FileNo = '{0}', @BackTime = {1}, @aBillingType = {2}",
                        FileNo, BackTimeMinutes, (int)CStatics.BillingType);
                    SaveLog("Executing query: " + lSQL);

                    lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewOutageInfo", aIsClearTable: true);
                    if (mDs.Tables.Contains("ViewOutageInfo") && mDs.Tables["ViewOutageInfo"].Rows.Count > 0)
                    {
                        DataRow lRow = mDs.Tables["ViewOutageInfo"].Rows[0];

                        OutageStatus lStatus = (OutageStatus)Convert.ToInt32(lRow["Status"]);

                        if (lStatus != OutageStatus.NoOutage)
                        {
                            lIsFound = true;
                            lOutageInfo.Status = lStatus;

                            if (lSi != null && lSi.Status != 1035 && lOutageInfo.Status == OutageStatus.OutageForDept)
                                lOutageInfo.Status = OutageStatus.NoOutage; //سامانه جامع مشترکین اعلام قطع به علت بدهی ننموده است

                            lOutageInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
                            lOutageInfo.BeginDate = lRow["DisconnectDate"].ToString();
                            lOutageInfo.BeginTime = lRow["DisconnectTime"].ToString();
                            lOutageInfo.EndDate = lRow["ConnectDate"].ToString();
                            lOutageInfo.EndTime = lRow["ConnectTime"].ToString();
                        }
                    }

                    if (!lIsFound)
                    {
                        ServiceLog lSLog = null;
                        OutageListResult lOLR = GetSubscriberOutageListByFileNo(FileNo, out lSLog);
                        lOutageInfo = GetCurrentOutageFromList(lOLR);
                    }

                    if (lOutageInfo == null)
                    {
                        lOutageInfo = new SubscriberOutageInfo()
                        {
                            Status = OutageStatus.NoOutage
                        };
                    }

                    if (lSi != null)
                        lOutageInfo.DebtPrice = lSi.DebtValue;

                    lOutageResult.OutageInfo = lOutageInfo;
                    lOutageResult.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                lOutageResult.IsSuccess = false;
                lOutageResult.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetSubscriberOutageInfoByFileNo Result ({0}): {1}", FileNo, mdl_Publics.GetJSonString(lOutageResult)));

            return lOutageResult;
        }

        [WebMethod(Description = "دریافت فهرست خاموشی‌های جاری و آینده مشترک از روی شماره پرونده او")]
        public OutageListResult GetSubscriberOutageListByFileNo(string FileNo, out ServiceLog aoSLog)
        {
			aoSLog = new ServiceLog();
			string lMsg = String.Format("Calling GetSubscriberOutageListByFileNo(FileNo={0})", FileNo);
            SaveLog(lMsg);
			CData.AddSLog(aoSLog, "SubscriberOutageListByFileNo", lMsg);

            OutageListResult lOutageListResult = null;
			SubscriberGISInfo lSGi = GetSubscriberGISInfoFromGIS(FileNo, CStatics.BillingType, aoSLog);

			if (string.IsNullOrWhiteSpace(lSGi.ErrorMessage))
			{
				lOutageListResult = GetOutagesByGISNetwork(lSGi, aoSLog);
				if (lOutageListResult.IsSuccess)
					CData.AddSLog(aoSLog, "SubscriberOutageListByFileNo", "SUCCESS: " + mdl_Publics.GetJSonString(lOutageListResult));
				else
					CData.AddSLog(aoSLog, "SubscriberOutageListByFileNo", "FAIL: " + lOutageListResult.ResultMessage);
			}
			else
			{
				lOutageListResult = new OutageListResult();
				lOutageListResult.IsSuccess = false;
				lOutageListResult.ResultMessage = lSGi.ErrorMessage;
			}

			SaveLog(string.Format("GetSubscriberOutageListByFileNo Result ({0}): {1}", FileNo, mdl_Publics.GetJSonString(lOutageListResult)));
			
			return lOutageListResult;
        }

        [WebMethod(Description = "دریافت پست و فیدر مشترک از روی شماره پرونده او")]
        public NetworkCodeInfo GetSubscriberPostFeederByFileNo(string FileNo)
        {
			SaveLog(String.Format("Calling GetSubscriberPostFeederByFileNo(FileNo={0})", FileNo));
			NetworkCodeInfo lNCi = GetSubscriberGISInfoFromGISCodes(FileNo, CStatics.BillingType);
			SaveLog(string.Format("GetSubscriberPostFeederByFileNo Result ({0}): {1}", FileNo, mdl_Publics.GetJSonString(lNCi)));
            return lNCi;
        }

        [WebMethod(Description = "دریافت وضعیت خاموشی جاری پرونده خاموشی بر اساس شماره پیگیری")]
        public SubscriberOutageResult GetSubscriberOutageInfoByTrackingCode(string TrackingCode)
        {
            SaveLog(String.Format("Calling GetSubscriberOutageInfoByTrackingCode(TrackingCode={0})", TrackingCode));
            SubscriberOutageResult lOutageResult = new SubscriberOutageResult();
            lOutageResult.OutageInfo = null;
            lOutageResult.IsSuccess = false;
            SubscriberOutageInfo lOutageInfo = new SubscriberOutageInfo();

            try
            {
                long lTrackingCode; long.TryParse(TrackingCode, out lTrackingCode);
                if (lTrackingCode == 0)
                    throw new Exception("##" + "کد پیگیری ارسال شده، معتبر نیست");

                string lSQL = string.Format(
                    "EXEC spGetSubscriberOutageInfoByTrackingCode @aTrackingCode = {0}",
                    TrackingCode);
                SaveLog("Executing query: " + lSQL);

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewOutageInfo", aIsClearTable: true);
                if (mDs.Tables.Contains("ViewOutageInfo") && mDs.Tables["ViewOutageInfo"].Rows.Count > 0)
                {
                    DataRow lRow = mDs.Tables["ViewOutageInfo"].Rows[0];

                    if (lRow["IsTamir"] == DBNull.Value)
                        throw new Exception("##" + "کد پیگیری ارسال شده، معتبر نیست");

                    OutageStatus lStatus = (OutageStatus)Convert.ToInt32(lRow["Status"]);
                    lOutageInfo.Status = lStatus;

                    lOutageInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
                    lOutageInfo.BeginDate = lRow["DisconnectDate"].ToString();
                    lOutageInfo.BeginTime = lRow["DisconnectTime"].ToString();
                    lOutageInfo.EndDate = lRow["ConnectDate"].ToString();
                    lOutageInfo.EndTime = lRow["ConnectTime"].ToString();
                }
                else
                    throw new Exception(lMsg);

				if (lOutageInfo == null)
				{
					lOutageInfo = new SubscriberOutageInfo()
					{
						Status = OutageStatus.NoOutage
					};
				}
				
				lOutageResult.OutageInfo = lOutageInfo;
                lOutageResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                lOutageResult.IsSuccess = false;
                lOutageResult.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetSubscriberOutageInfoByTrackingCode Result ({0}): {1}", TrackingCode, mdl_Publics.GetJSonString(lOutageResult)));

            return lOutageResult;
        }

        [WebMethod(Description = "ثبت نتیجه نظر سنجی مربوط به مرکز تماس")]
        public InformResult SetCCSurveyResult(bool IsSMS, string CallId, string AgentNo, string TelNo, string SurveyReplyCode)
        {
            SaveLog(String.Format("Calling SetCCSurveyResult(CallId={0},AgentNo={1},TelNo={2},SurveyReplyCode={3})", CallId, AgentNo, TelNo, SurveyReplyCode));
            InformResult lRes = new InformResult();
            lRes.IsSuccess = false;
            lRes.ResultMessage = "";

            try
            {
                string lSQL = 
                    string.Format("EXEC spCCSetSurveyReply @aIsSMS = {0}, @aCallId = '{1}', @aAgentNo = '{2}', @aTelNo = '{3}', @aSurveyReplyCode = '{4}'", 
                    (IsSMS ? "1" : "0"), CallId, AgentNo, TelNo, SurveyReplyCode);
                SaveLog("Executing query: " + lSQL);

                DataSet lDs = new DataSet();
                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSurveyResult", aIsShowError: true, aIsClearTable: true);

                if (lDs.Tables.Contains("ViewSurveyResult") && lDs.Tables["ViewSurveyResult"].Rows.Count > 0)
                {
                    DataRow lRow = lDs.Tables["ViewSurveyResult"].Rows[0];
                    string lResultCode = lRow["ResultCode"].ToString();
                    if (lResultCode == "0")
                        lRes.IsSuccess = true;
                    else
                        lRes.ResultMessage = string.Format("ErrorCode={0}: ", lResultCode);

                    lRes.ResultMessage += lRow["ResultMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("SetSurveyResult Result (CallId={0},AgentNo={1},TelNo={2},SurveyReplyCode={3}): {4}", CallId, AgentNo, TelNo, SurveyReplyCode, lRes.ResultMessage));

            return lRes;
        }

        [WebMethod(Description = "دریافت صداهای ضبط شده مربوط به خاموشی‌های منطقه از روی پیش شماره تماس")]
        public OutageVoiceListResult GetZoneOutageVoicesFromTelNo(string TelNo)
        {
            SaveLog(String.Format("Calling GetZoneOutageVoicesFromTelNo(TelNo={0})", TelNo));
            OutageVoiceListResult lRes = new OutageVoiceListResult();
            List<VoiceInfo> lVoiceList = new List<VoiceInfo>();
            VoiceInfo lVoiceData = null;

            try
            {
                string lSQL = string.Format("EXEC spGetIVRVoiceData @PhoneNumber = '{0}'", TelNo);
                SaveLog("Executing query: " + lSQL);

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewVoiceData", aIsClearTable: true);
                if (mDs.Tables.Contains("ViewVoiceData") && mDs.Tables["ViewVoiceData"].Rows.Count > 0)
                {
                    foreach (DataRow lRow in mDs.Tables["ViewVoiceData"].Rows)
                    {
                        lVoiceData = new VoiceInfo();
						if (lRow["VoiceId"] != DBNull.Value)
							lVoiceData.VoiceId = lRow["VoiceId"].ToString();
						else
							lVoiceData.VoiceId = lRow["VoiceCode"].ToString();

                        lVoiceData.ConnectTime = lRow["ConnectTime"].ToString();

						try
						{
							if (lRow["VoiceData"] != DBNull.Value && !string.IsNullOrWhiteSpace(lRow["VoiceData"].ToString()))
							{
								byte[] lVoice = (byte[])lRow["VoiceData"];
								string lVoiceFile = System.BitConverter.ToString(lVoice);
								lVoiceFile = "0x" + lVoiceFile.Replace("-", "");
								lVoiceData.VoiceData = lVoiceFile;
							}
							else
								lVoiceData.VoiceData = "";
						}
						catch { }

                        lVoiceList.Add(lVoiceData);
                    }

                    lRes.ResultMessage = string.Format("{0} outage(s) found.", mDs.Tables["ViewVoiceData"].Rows.Count);
                }
                else if (lMsg.Length > 0)
                    throw new Exception(lMsg);
                else
                    lRes.ResultMessage = "No outage found.";

                lRes.IsSuccess = true;
                lRes.VoiceList = lVoiceList;
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetZoneOutageVoicesFromTelNo Result (TelNo={0}): {1}", TelNo, mdl_Publics.GetJSonString(lRes)));

            return lRes;
        }

        [WebMethod(Description = "دریافت فهرست خاموشی‌های جاری و آینده منطقه از روی پیش شماره تماس")]
        public OutageListResult GetZoneOutageListFromTelNo(string TelNo)
        {
			SaveLog(String.Format("Calling GetZoneOutageListFromTelNo(TelNo={0})", TelNo));
			OutageListResult lRes = new OutageListResult();
			List<OutageData> lOutageList = new List<OutageData>();

            try
            {
				string lSQL = string.Format("EXEC spGetIVROutageList @PhoneNumber = '{0}'", TelNo);
                SaveLog("Executing query: " + lSQL);

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewOutageList", aIsClearTable: true);
                if (mDs.Tables.Contains("ViewOutageList") && mDs.Tables["ViewOutageList"].Rows.Count > 0)
                {
                    foreach (DataRow lRow in mDs.Tables["ViewOutageList"].Rows)
                    {
						OutageData lInfo = new OutageData();

						lInfo.RequestNumber = lRow["RequestNumber"].ToString();
						lInfo.BeginDate = lRow["DisconnectDatePersian"].ToString();
						lInfo.BeginTime = lRow["DisconnectTime"].ToString();
						lInfo.EndDate = lRow["ConnectDatePersian"].ToString();
						lInfo.EndTime = lRow["ConnectTime"].ToString();
						lInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);

						lOutageList.Add(lInfo);
					}

                    lRes.ResultMessage = string.Format("{0} outage(s) found.", mDs.Tables["ViewOutageList"].Rows.Count);
                }
                else if (lMsg.Length > 0)
                    throw new Exception(lMsg);
                else
                    lRes.ResultMessage = "No outage found.";

                lRes.IsSuccess = true;
                lRes.OutageList = lOutageList;
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

			SaveLog(string.Format("GetZoneOutageListFromTelNo Result (TelNo={0}): {1}", TelNo, mdl_Publics.GetJSonString(lRes)));

            return lRes;
        }

        [WebMethod(Description = "دريافت شماره پيگيري مربوط به CallId")]
        public InformResult GetCallTrackingCode(string CallId)
        {
            SaveLog(String.Format("Calling GetCallTrackingCode(CallId={0})", CallId));
            InformResult lRes = new InformResult();
            lRes.IsSuccess = false;

            if (CallId.Trim() == "")
            {
                lRes.ResultMessage = "CallId Is Empty";
                return lRes;
            }

            try
            {
                DataRow lNewRow;
				string lSQL = string.Format("exec spGetCallTrackingCode @aCallId = '{0}', @aIsSMSPopup = 1", CallId);
                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "Tbl_CallTracking", aIsClearTable: true);

                if (mDs.Tables.Contains("Tbl_CallTracking"))
                {
                    if (mDs.Tables["Tbl_CallTracking"].Rows.Count > 0)
                    {
                        lRes.IsSuccess = true;
                        lRes.ResultMessage = mDs.Tables["Tbl_CallTracking"].Rows[0]["TrackingCode"].ToString();
                    }
                    else
                        lRes.ResultMessage = "TrackingCode Not Found";
                }
                else if (lMsg.Length > 0)
                    throw new Exception(lMsg);
            }
            catch (Exception ex)
            {
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(String.Format("GetCallTrackingCode Result (CallId={0}): TrackingCode={1}", CallId, lRes.ResultMessage));

            return lRes;
        }

        [WebMethod(Description = "ارسال پيامک راهنمايي به مشترک")]
        public InformResult SendGuideSMS(string TelNo)
        {
            SaveLog(String.Format("Calling SendGuideSMS(CallerId={0})", TelNo));
            InformResult lRes = new InformResult();
            lRes.IsSuccess = false;

            if (TelNo.Trim() == "")
            {
                lRes.ResultMessage = "TelNo Is Empty";
                return lRes;
            }
            try
            {
                Int64.Parse(TelNo);
            }
            catch (Exception ex)
            {
                lRes.ResultMessage = String.Format("SendGuideSMS Result(CallerId={0}, Error={1}) ", TelNo, ex.Message);
                SaveLog(lRes.ResultMessage);
                return lRes;
            }

            bool lIsActiveGuideSMS  = false;
            lIsActiveGuideSMS = Convert.ToBoolean(CConfig.ReadConfig("IsActiveGuideSMSSent", false));
            if (!lIsActiveGuideSMS)
            {
                lRes.IsSuccess = true;
                lRes.ResultMessage = "SendGuideSMS Is Not Active";
                return lRes;
            }

            try
            {
                DataRow lNewRow;
                string lSQL = string.Format("select * from TblGuideSMSSent where CallerId = " + TelNo);
                SaveLog(String.Format("SendGuideSMS T-SQL = {0} ", lSQL));
                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "TblGuideSMSSent", aIsClearTable: true);
                SaveLog("SendGuideSMS END BindingTable ");

                if (mDs.Tables.Contains("TblGuideSMSSent"))
                {
                    if (mDs.Tables["TblGuideSMSSent"].Rows.Count == 0)
                    {
                        lNewRow = mDs.Tables["TblGuideSMSSent"].NewRow();
                        lNewRow["GuideSMSSentId"] = mdl_Publics.GetAutoInc();
                        lNewRow["CallerId"] = TelNo;
                        mDs.Tables["TblGuideSMSSent"].Rows.Add(lNewRow);

                        lSQL = string.Format("exec spCreateGuideSMS '" + TelNo + "'");
                        SaveLog(String.Format("SendGuideSMS T-SQL For Create SMS = {0} ", lSQL));
                        lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "Tbl_SMS", aIsClearTable: true);
                        SaveLog("SendGuideSMS END Create SMS ");

                        if (lMsg == "")
                        {
                            if (mDs.Tables.Contains("Tbl_SMS") && mDs.Tables["Tbl_SMS"].Rows.Count > 0)
                            {
                                if (Convert.ToInt64(mDs.Tables["Tbl_SMS"].Rows[0]["SMSId"]) > -1)
                                {
                                    frmUpdateDataset lUpdate = new frmUpdateDataset();
                                    lUpdate.UpdateDataSet("TblGuideSMSSent", mDs, 99);
                                }
                                else
                                    throw new Exception("SendGuideSMS Setting not found.");
                            }
                            else
                                throw new Exception(String.Format("CallerId {0} Is Not Mobile Number", TelNo));
                        }
                        else
                            throw new Exception(lMsg);
                        
                    }
                    else 
                    {
                        lRes.IsSuccess = true;
                        lRes.ResultMessage = String.Format("SMS has already been sent. Id={0} ", mDs.Tables["TblGuideSMSSent"].Rows[0]["GuideSMSSentId"].ToString());
                        return lRes;
                    }

                    lSQL = string.Format("select * from TblGuideSMSSent where CallerId = '{0}'", TelNo);
                    lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "TblGuideSMSSent", aIsClearTable: true);

                    if (mDs.Tables.Contains("TblGuideSMSSent") && mDs.Tables["TblGuideSMSSent"].Rows.Count > 0)
                    {
                        lRes.IsSuccess = true;
                        lRes.ResultMessage = mDs.Tables["TblGuideSMSSent"].Rows[0]["GuideSMSSentId"].ToString();
                    }
                    else
                        lRes.ResultMessage = "GuideSMSSentId Not Found";
                }
                else if (lMsg.Length > 0)
                    lRes.ResultMessage = lMsg;
            }
            catch (Exception ex)
            {
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(String.Format("SendGuideSMS Result (CallerId={0}): GuideSMSSentId={1}", TelNo, lRes.ResultMessage));

            return lRes;
        }

        [WebMethod(Description = "ارسال پیامک اطلاعات خاموشی‌های جاری و آینده به مشترک")]
        public InformResult SendOutageInfoSMS(string MobileNo, string FileNo)
        {
            SaveLog(String.Format("Calling SendOutageInfoSMS(MobileNo={0},FileNo={1})", MobileNo, FileNo));
            InformResult lRes = new InformResult();
            lRes.ResultMessage = "";
            string lMobileNo = MobileNo;

            try
            {
                if (string.IsNullOrWhiteSpace(lMobileNo))
                    throw new Exception("شماره تلفن همراه برای ارسال پیامک مشخص نمی‌باشد");

				SubscriberGISInfo lSGi = GetSubscriberGISInfoFromGIS(FileNo, CStatics.BillingType);
                if (lSGi.ErrorMessage.Length > 0)
                    throw new Exception(lSGi.ErrorMessage);

                string lSQL = string.Format(
                    "EXEC spSendOutageInfoSMS @aMobileNo = '{0}', @aMPPostId = {1}, @aMPFeederId = {2}, @aLPPostId = {3}, @aLPFeederId = {4}"
                    , lMobileNo, lSGi.MPPostId, lSGi.MPFeederId, lSGi.LPPostId, lSGi.LPFeederId
                    );
                SaveLog("Executing query: " + lSQL);

                DataSet lDs = new DataSet();
                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSMSResult", aIsShowError: true, aIsClearTable: true);

                if (lDs.Tables.Contains("ViewSMSResult") && lDs.Tables["ViewSMSResult"].Rows.Count > 0)
                {
                    DataRow lRow = lDs.Tables["ViewSMSResult"].Rows[0];
                    long lSMSId; long.TryParse(lRow["SMSId"].ToString(), out lSMSId);
                    if (lSMSId > 0)
                    {
                        lRes.IsSuccess = true;
                        lRes.ResultMessage = string.Format("پیامک فهرست خاموشی‌ها با موفقیت ارسال گردید: {0}", lSMSId);
                    }
                    else
                    {
                        lMsg = lRow["ResultMessage"].ToString();
                        throw new Exception(string.Format("اشکال در تولید پیامک فهرست خاموشی: {0}", lMsg));
                    }
                }
                else
                    throw new Exception(string.Format("اشکال در تولید پیامک فهرست خاموشی: {0}", lMsg));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("SendOutageInfoSMS Result (MobileNo={0},FileNo={1}): {2}", MobileNo, FileNo, lRes.ResultMessage));

            return lRes;
        }

        [WebMethod(Description = "رجیستر نمودن مشترکین جهت اطلاع رسانی خاموشی‌ها")]
        public InformResult RegisterSubscriberForOutageInfo(string Name, string FileNo, string MobileNo)
        {
            SaveLog(String.Format("Calling RegisterSubscriberForOutageInfo(Name={0},FileNo={1},MobilrNo={2})", Name, FileNo, MobileNo));
            InformResult lRes = new InformResult();
            lRes.ResultMessage = "";

            int lAreaId = -1;
            int lMPPostId = -1;
            int lMPFeederId = -1;
            int lLPPostId = -1;
            int lLPFeederId = -1;

            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    throw new Exception("نام مشترک مشخص نشده است.");
                if (string.IsNullOrWhiteSpace(FileNo))
                    throw new Exception("شماره پرونده مشترک مشخص نشده است.");
                if (string.IsNullOrWhiteSpace(MobileNo))
                    throw new Exception("شماره تلفن همراه مشترک مشخص نمی‌باشد");

				SubscriberGISInfo lSGi = GetSubscriberGISInfoFromGIS(FileNo, CStatics.BillingType);
                if (lSGi != null && lSGi.ErrorMessage.Length == 0)
                {
                    lMPPostId = lSGi.MPPostId;
                    lMPFeederId = lSGi.MPFeederId;
                    lLPPostId = lSGi.LPPostId;
                    lLPFeederId = lSGi.LPFeederId;
                }

                string lSQL = string.Format(
                    "EXEC spRegisterForOutageSMS @aName = '{0}', @aCode = '{1}', @aCodeType = {2}, " +
                    "@aMobileNo = '{3}', @aAreaId = {4}, @aMPPostId = {5}, @aMPFeederId = {6}, " +
                    "@aLPPostId = {7}, @aLPFeederId = {8}"
					, Name, FileNo, (int)CStatics.BillingType, MobileNo, -1, lAreaId, 
                    lMPPostId, lMPFeederId, lLPPostId, lLPFeederId
                    );
                SaveLog("Executing query: " + lSQL);

                DataSet lDs = new DataSet();
                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewRegisterResult", aIsShowError: true, aIsClearTable: true);

                if (lDs.Tables.Contains("ViewRegisterResult") && lDs.Tables["ViewRegisterResult"].Rows.Count > 0)
                {
                    DataRow lRow = lDs.Tables["ViewRegisterResult"].Rows[0];
                    if (true)
                    {
                        lRes.IsSuccess = true;
                        lRes.ResultMessage = string.Format("مشترک مورد نظر با موفیت ثبت نام گردید.");
                    }
                    else
                    {
                        lMsg = lRow["ResultMessage"].ToString();
                        throw new Exception(string.Format("اشکال در نام مشترک: {0}", lMsg));
                    }
                }
                else
                    throw new Exception(string.Format("اشکال در ثبت نام مشترک: {0}", lMsg));
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("RegisterSubscriberForOutageInfo Result (Name={0},FileNo={1},MobilrNo={2}): {3}", Name, FileNo, MobileNo, lRes.ResultMessage));

            return lRes;
        }

		[WebMethod(Description = "دریافت اطلاعات تماس جدید جهت شماره گیری")]
		public OutboundCallInfoResult GetOutboundCall()
		{
			lock (lockMethod)
			{
				SaveLog(String.Format("Calling GetOutboundCall()"));
				OutboundCallInfoResult lRes = new OutboundCallInfoResult();
				OutboundCallInfo lCallInfo = null;
				lRes.IsSuccess = false;
				lRes.ResultMessage = "";

				try
				{
					string lSQL = string.Format("EXEC spGetOutgoingCall");
					SaveLog("Executing query: " + lSQL);

					string lTblName = "ViewOutgoingCall";
					string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, lTblName, aIsClearTable: true);
					if (mDs.Tables.Contains(lTblName) && mDs.Tables[lTblName].Rows.Count > 0)
					{
						DataRow lRow = mDs.Tables[lTblName].Rows[0];

						lCallInfo = new OutboundCallInfo()
						{
							Id = Convert.ToInt64(lRow["Id"]),
							PhoneNo = lRow["PhoneNo"].ToString(),
							TrunkExtNo = lRow["TrunkExtNo"].ToString(),
							WaitTime = Convert.ToInt32(lRow["WaitTime"]),
							CallType = Convert.ToInt32(lRow["CallType"]),
							//FileServerId = Convert.ToInt64(lRow["FileServerId"]),
							OutageDate = lRow["OutageDate"].ToString(),
							OutageTimeStart = lRow["OutageTimeStart"].ToString(),
							OutageTimeEnd = lRow["OutageTimeEnd"].ToString()
						};

					}

					/* -----------------------
					 * Test web method output
					 * -----------------------
					lCallInfo = new OutboundCallInfo()
					{
						Id = 1,
						PhoneNo = "02123033000",
						TrunkExtNo = "9",
						WaitTime = 60,
						CallType = 11,
						FileServerId = 1,
						OutageDate = "1397/11/16",
						OutageTimeStart = "15:10",
						OutageTimeEnd = "16:30"
					};
					 * -----------------------
					*/

					lRes.CallInfo = lCallInfo;
					lRes.IsSuccess = true;
				}
				catch (Exception ex)
				{
					lRes.IsSuccess = false;
					lRes.ResultMessage = ex.Message;
				}

				SaveLog(string.Format("GetOutboundCall Result: {0}", mdl_Publics.GetJSonString(lRes)));

				return lRes;
			}
		}

        [WebMethod(Description = "ارسال نتیجه تماس با مشترک یا ارسال فکس به سامانه ثبت حوادث")]
        public InformResult EndFaxOrOutboundCall(long UniqueId, int ResultId)
        {
			SaveLog(String.Format("Calling EndOutboundCall(UniqueId={0},ResultId={1})", UniqueId, ResultId));
            InformResult lRes = new InformResult();
            lRes.IsSuccess = false;
            lRes.ResultMessage = "";

			try
			{
				string lSQL = string.Format("EXEC spSetSendFaxCallResult @SMSId = {0}, @aStatusId = {1}", UniqueId, ResultId);
				SaveLog("Executing query: " + lSQL);

				string lTblName = "ViewSendFaxCallResult";
				string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, lTblName, aIsClearTable: true);
				if (mDs.Tables.Contains(lTblName) && mDs.Tables[lTblName].Rows.Count > 0)
				{
					DataRow lRow = mDs.Tables[lTblName].Rows[0];
					string lResCode = lRow["ResultCode"].ToString();
					if (lResCode == "1")
						lRes.IsSuccess = true;
					else
						lRes.IsSuccess = false;
					lRes.ResultMessage = lRow["ResultMessage"].ToString();
				}
			}
			catch (Exception ex)
			{
				lRes.IsSuccess = false;
				lRes.ResultMessage = ex.Message;
			}

			SaveLog(string.Format("EndOutboundCall Result: {0} => {1}", lRes.IsSuccess.ToString(), lRes.ResultMessage));

            return lRes;
        }

        [WebMethod(Description = "دریافت فایل تصویر مربوط به فکس")]
        public OutboundFaxResult GetFaxImage(long UniqueId)
        {
            SaveLog(String.Format("Calling GetFaxImage(UniqueId={0})", UniqueId));
            OutboundFaxResult lRes = new OutboundFaxResult();
            lRes.IsSuccess = false;
            lRes.ResultMessage = "";

            try
            {
                string lSQL = string.Format("EXEC spGetFaxData @SMSId = {0}", UniqueId);
                SaveLog("Executing query: " + lSQL);

				string lTifPath = Server.MapPath("/Files/Fax.tif");
				Image lTiff = Image.FromFile(lTifPath);

				MemoryStream imageStream = new MemoryStream();
				lTiff.Save(imageStream, System.Drawing.Imaging.ImageFormat.Tiff);

				byte[] lImageData = new Byte[imageStream.Length];
				imageStream.Position = 0;
				imageStream.Read(lImageData, 0, (int)imageStream.Length);

				string lImageFile = System.BitConverter.ToString(lImageData);
				lImageFile = "0x" + lImageFile.Replace("-", "");

                lRes.FaxData = lImageFile;
                lRes.IsSuccess = true;
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("EndOutboundCall Result ({0}): {1}", UniqueId, lRes.ResultMessage));

            return lRes;
        }


		[WebMethod(Description = "دریافت گروه IVR از روی پیش شماره تلفن")]
		public InformResult GetIVRCodeFromTelNo(string TelNo, int BackTimeDays)
		{
			SaveLog(String.Format("Calling GetIVRCodeFromTelNo(TelNo={0},BackTimeMinutes={1})", TelNo, BackTimeDays));
			InformResult lRes = new InformResult();
			lRes.IsSuccess = false;

			if (TelNo.Trim() == "")
			{
				lRes.ResultMessage = "TelNo Is Empty";
				return lRes;
			}

			try
			{
				DataRow lNewRow;
				string lSQL = string.Format("exec spGetIVRCodeFromTelNo @CallerId = '{0}', @BackDay = {1}", TelNo, BackTimeDays);
				string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewIVRCodes", aIsClearTable: true);

				if (mDs.Tables.Contains("ViewIVRCodes"))
				{
					if (mDs.Tables["ViewIVRCodes"].Rows.Count > 0)
					{
						string lIVRCode = mDs.Tables["ViewIVRCodes"].Rows[0]["IVRCode"].ToString();
						string lAreaId = mDs.Tables["ViewIVRCodes"].Rows[0]["AreaId"].ToString();
						if (lIVRCode == "-1")
							lRes.ResultMessage = "کد IVR ناحیه مورد نظر در سیستم تعریف نشده است: " + lAreaId;
						else if (lIVRCode == "0")
							lRes.ResultMessage = "مشترک تاکنون تماسی با سیستم نداشته است.";
						else
						{
							lRes.IsSuccess = true;
							lRes.ResultMessage = lIVRCode;
						}
					}
					else
						lRes.ResultMessage = "IVR Code Not Found";
				}
				else if (lMsg.Length > 0)
					throw new Exception(lMsg);
			}
			catch (Exception ex)
			{
				lRes.ResultMessage = ex.Message;
			}

			SaveLog(String.Format("GetIVRCodeFromTelNo Result (TelNo={0},BackTimeMinutes={1}): {2}", TelNo, BackTimeDays, lRes.ResultMessage));

			return lRes;
		}


        /*-------------------------------*/
        /*       Methods for Homa        */
        /*-------------------------------*/

        private BillingTypes BillingType
        {
            get
            {
                BillingTypes lBT = BillingTypes.SubNo;

                string lSubKey = System.Configuration.ConfigurationManager.AppSettings["SubscriberKey"];
                if (string.IsNullOrWhiteSpace(lSubKey))
                    lSubKey = "";

                switch (lSubKey)
                {
                    case "Code":
                        lBT = BillingTypes.SubNo;
                        break;
                    case "RamzCode":
                        lBT = BillingTypes.RamzCode;
                        break;
                    case "BillingID":
                        lBT = BillingTypes.BillingId;
                        break;
                    case "FileNo":
                        lBT = BillingTypes.FileNo;
                        break;
                    case "TelNo":
                        lBT = BillingTypes.TelNo;
                        break;
                }

                return lBT;
            }
        }

        private BillingTypes BillingTypeForGIS
        {
            get
            {
                BillingTypes lBT = BillingTypes.SubNo;

                string lSubKey = System.Configuration.ConfigurationManager.AppSettings["SubscriberKeyForGIS"];
                if (string.IsNullOrWhiteSpace(lSubKey))
                    lSubKey = System.Configuration.ConfigurationManager.AppSettings["SubscriberKey"];
                if (string.IsNullOrWhiteSpace(lSubKey))
                    lSubKey = "";

                switch (lSubKey)
                {
                    case "Code":
                        lBT = BillingTypes.SubNo;
                        break;
                    case "RamzCode":
                        lBT = BillingTypes.RamzCode;
                        break;
                    case "BillingID":
                        lBT = BillingTypes.BillingId;
                        break;
                    case "FileNo":
                        lBT = BillingTypes.FileNo;
                        break;
                    case "TelNo":
                        lBT = BillingTypes.TelNo;
                        break;
                }

                return lBT;
            }
        }

        [WebMethod(Description = "دريافت اطلاعات مشترک از روي شماره تماس")]
        public SubscriberRegisterInfo GetSubscriberRegisterInfo(string CallerId)
        {
            SubscriberRegisterInfo lRes = new SubscriberRegisterInfo();

            lRes.IsSuccess = false;

            if (string.IsNullOrWhiteSpace(CallerId))
            {
                lRes.ResultMessage = "CallerID Is Null";
                return lRes;
            }

            SaveLog(String.Format("Calling GetSubscriberRegisterInfo(CallerId={0})", CallerId));

            try
            {

                DataSet lDs = new DataSet();
                string lSQL = string.Format("exec Homa.spGetSubscriberRegisterInfo @CallerId='{0}'", CallerId);
                SaveLog(String.Format("GetSubscriberRegisterInfo EXEC SQL = {0}", lSQL));

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSubscriberRegisterInfo", aIsClearTable: true);
                if (lMsg != "")
                    throw new Exception(lMsg);

                if (lDs.Tables.Contains("ViewSubscriberRegisterInfo") && lDs.Tables["ViewSubscriberRegisterInfo"].Rows.Count > 0)
                {
                    DataRow lRow = lDs.Tables["ViewSubscriberRegisterInfo"].Rows[0];
                    lRes.IsSuccess = true;
                    lRes.ResultMessage = "";
                    lRes.RegisterId = (long)lRow["RegisterId"];
                    lRes.SubscriberName = lRow["SubscriberName"].ToString();
                    lRes.MelliCode = lRow["MelliCode"].ToString();
                    lRes.MobileNo = lRow["MobileNo"].ToString();
                    lRes.TelNo = lRow["TelNo"].ToString();
                    lRes.RegisterDT = (DateTime)lRow["RegisterDT"];
                    lRes.RegisterDatePersian = lRow["RegisterDatePersian"].ToString();
                    lRes.RegisterTime = lRow["RegisterTime"].ToString();
                    lRes.Comments = lRow["Comments"].ToString();
                    lRes.RegisterStatus = lRow["RegisterStatus"].ToString();



                    string lJson = mdl_Publics.GetJSonString(lRes);

                    SaveLog(String.Format("GetSubscriberRegisterInfo Result (CallerId={0}): {1}", CallerId, lJson));
                }
                else
                    throw new Exception("Subscriber Not Found");

            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
                SaveLog(String.Format("GetSubscriberRegisterInfo Result Error (CallerId={0}): {1}", CallerId, lRes.ResultMessage));
            }

            return lRes;
        }

        [WebMethod(Description = "استعلام شناسه قبض مشترک از روي شماره تماس")]
        public GetSubscriberBillingIDs GetSubscriberBillings(string CallerId, int? BillingIDTypeId = -1)
        {
            GetSubscriberBillingIDs lRes = new GetSubscriberBillingIDs();
            lRes.IsSuccess = false;

            if (string.IsNullOrWhiteSpace(CallerId))
            {
                lRes.ResultMessage = "CallerID Is Null";
                return lRes;
            }
            
            SaveLog(String.Format("Calling GetSubscriberBillingIDs(CallerId={0})", CallerId));

            try
            {
                SubscriberBilling lSubscriberBilling;
                List<SubscriberBilling> lSubscriberBillingList = new List<SubscriberBilling>();

                DataSet lDs = new DataSet();
                string lSQL = string.Format("exec Homa.spGetSubscriberBilling @CallerId='{0}', @BillingIDTypeId={1}", CallerId, BillingIDTypeId);
                SaveLog(String.Format("GetSubscriberBillings EXEC SQL = {0}", lSQL));

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSubscriberBillingList", aIsClearTable: true);

                if (lDs.Tables.Contains("ViewSubscriberBillingList"))
                {
                    foreach (DataRow lRow in lDs.Tables["ViewSubscriberBillingList"].Rows)
                    {
                        lSubscriberBilling = new SubscriberBilling();
                        lSubscriberBilling.BillingId = lRow["BillingId"].ToString();
                        lSubscriberBilling.BillingIDType = lRow["BillingIDType"].ToString();
                        lSubscriberBilling.BillingIDTypeId = (int)lRow["BillingIDTypeId"];
                        lSubscriberBillingList.Add(lSubscriberBilling);
                    }
                    lRes.IsSuccess = true;
                    lRes.BillingList = lSubscriberBillingList;
                    lRes.ResultMessage = "";

                    string lJson = mdl_Publics.GetJSonString(lRes);
                    lJson = CFunctions.GetClearJsonString(lJson, true);
                    SaveLog(String.Format("GetSubscriberBillings Result (CallerId={0}): {1}", CallerId, lJson));
                }
                else
                    throw new Exception(lMsg);

            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.BillingList = null;
                lRes.ResultMessage = ex.Message;
                SaveLog(String.Format("GetSubscriberBillings Result Error (CallerId={0}): {1}", CallerId, lRes.ResultMessage));
            }
            
            return lRes;
        }

        [WebMethod(Description = "دريافت اطلاعات قبض ثبت شده از روي شناسه قبض")]
        public SubscriberBillingInfo GetSubscriberBillingInfo(string BillingId)
        {
            SubscriberBillingInfo lRes = new SubscriberBillingInfo();
            lRes.IsSuccess = false;

            if (string.IsNullOrWhiteSpace(BillingId))
            {
                lRes.ResultMessage = "BillingId Is Null";
                return lRes;
            }

            SaveLog(String.Format("Calling GetSubscriberBillingInfo(BillingId={0})", BillingId));

            try
            {
                SubscriberBillingInfo lSubBillInfo = new SubscriberBillingInfo();

                DataSet lDs = new DataSet();
                string lSQL = string.Format("exec Homa.spGetSubscriberBillingInfo @BillingId='{0}'", BillingId);
                SaveLog(String.Format("GetSubscriberBillingInfo EXEC SQL = {0}", lSQL));

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSubscriberBillingInfo", aIsClearTable: true);
                if (lMsg != "")
                    throw new Exception(lMsg);

                if (lDs.Tables.Contains("ViewSubscriberBillingInfo") && lDs.Tables["ViewSubscriberBillingInfo"].Rows.Count > 0)
                {

                    DataRow lRow = lDs.Tables["ViewSubscriberBillingInfo"].Rows[0];

                    lRes.IsSuccess = true;
                    lRes.ResultMessage = "";
                    lRes.Address = lRow["Address"].ToString();
                    lRes.AddressInBilling = lRow["AddressInBilling"].ToString();
                    lRes.BillingID = lRow["BillingID"].ToString();
                    lRes.BillingIDType = lRow["BillingIDType"].ToString();
                    lRes.BillingIDTypeId = (int)lRow["BillingIDTypeId"];

                    lRes.Comments = lRow["Comments"].ToString();

                    if (lRow["GpsX"] != DBNull.Value)
                        lRes.GpsX = float.Parse(lRow["GpsX"].ToString());
                    if (lRow["GpsY"] != DBNull.Value)
                        lRes.GpsY = float.Parse(lRow["GpsY"].ToString());

                    if (lRow["LPFeederId"] != DBNull.Value && (int)lRow["LPFeederId"] > -1)
                        lRes.LPFeederId = Int32.Parse(lRow["LPFeederId"].ToString());
                    if (lRow["LPPostId"] != DBNull.Value && (int)lRow["LPPostId"] > -1)
                        lRes.LPPostId = Int32.Parse(lRow["LPPostId"].ToString());
                    if (lRow["AreaId"] != DBNull.Value && (int)lRow["AreaId"] > -1)
                        lRes.AreaId = Int32.Parse(lRow["AreaId"].ToString());
                    lRes.RegisterBillingIDId = (long)lRow["RegisterBillingIDId"];
                    lRes.RegisterDT = (DateTime)lRow["RegisterDT"];
                    lRes.RegisterId = (long)lRow["RegisterId"];
                    lRes.SubscriberName = lRow["SubscriberName"].ToString();

                    string lJson = mdl_Publics.GetJSonString(lRes);
                    SaveLog(String.Format("GetSubscriberBillingInfo Result (BillingId={0}): {1}", BillingId, lJson));
                }
                else
                    throw new Exception("BillingId Not Found");

            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
                SaveLog(String.Format("GetSubscriberBillingInfo Result Error (BillingId={0}): {1}", BillingId, lRes.ResultMessage));
            }

            return lRes;
        }

        public ExternalLinks.CSubscriberInfoResult GetBillingDataClassWithBillingID(string aBillingCode, BillingTypes aBillingType)
        {
            ExternalLinks.CSubscriberInfoResult lResult = new ExternalLinks.CSubscriberInfoResult();
            lResult.IsSuccess = false;

            try
            {
                ExternalLinks lExternalLink = new ExternalLinks();

                ExternalLinks.CSubscriberInfo lSubInfo = lExternalLink.GetBillingDataClass(aBillingCode, (ExternalLinks.SubscriberCodeTypes)((int)aBillingType));
                if (string.IsNullOrWhiteSpace(lSubInfo.ErrorMessage) || lSubInfo.ErrorMessage.Length == 0)
                {
                    ExternalLinks.CSubscriberInfo lSubGISInfo = new ExternalLinks.CSubscriberInfo();

                    aBillingType = BillingTypeForGIS;
                    if (aBillingType == BillingTypes.SubNo && !string.IsNullOrWhiteSpace(lSubInfo.SubscriberCode))
                        aBillingCode = lSubInfo.SubscriberCode;


                    ExternalLinks.SubscriberParentParams lParams = new ExternalLinks.SubscriberParentParams();
                    lParams.SubscriberBillingID = lSubInfo.BillingID;
                    lParams.SubscriberFileNo = lSubInfo.FileNo;
                    lParams.SubscriberNo = lSubInfo.SubscriberCode;
                    lParams.SubscriberID = lSubInfo.RamzCode;

                    lSubGISInfo = lExternalLink.GetSubscriberParentClass(lParams);

                    string lSkipGISPostFeederError = System.Configuration.ConfigurationManager.AppSettings["SkipGISPostFeederError"];
                    bool lIsSkipGISPostFeederError = false;
                    if (!string.IsNullOrWhiteSpace(lSkipGISPostFeederError))
                        lIsSkipGISPostFeederError = Convert.ToBoolean(lSkipGISPostFeederError);


                    if (string.IsNullOrWhiteSpace(lSubGISInfo.ErrorMessage) || lSubGISInfo.ErrorMessage.Length == 0)
                    {
                        if (lSubGISInfo.LPFeederId < 0 && lSubGISInfo.LPPostId < 0)
                        {
                            if (!lIsSkipGISPostFeederError)
                                throw new Exception(string.Format("GetSubscriberGISInfoFromGIS Error = No post and feeder found for BillingID:{0}", aBillingCode));
                        }
                        else
                        {
                            lSubInfo.MPPostId = lSubGISInfo.MPPostId;
                            lSubInfo.MPFeederId = lSubGISInfo.MPFeederId;
                            lSubInfo.LPPostId = lSubGISInfo.LPPostId;
                            lSubInfo.LPFeederId = lSubGISInfo.LPFeederId;
                            if (!string.IsNullOrWhiteSpace(lSubGISInfo.GPSx) && float.Parse(lSubGISInfo.GPSx) > 0)
                                lSubInfo.GPSx = lSubGISInfo.GPSx;
                            if (!string.IsNullOrWhiteSpace(lSubGISInfo.GPSy) && float.Parse(lSubGISInfo.GPSy) > 0)
                                lSubInfo.GPSy = lSubGISInfo.GPSy;

                            lSubInfo.AreaId = lSubGISInfo.AreaId;
                        }

                        string lJsonString = mdl_Publics.GetJSonString(lSubInfo);
                        SaveLog(String.Format("GetBillingDataClassWithBillingID Result (BillingID={0}): {1}", aBillingCode, lJsonString));

                        lResult.IsSuccess = true;
                        lResult.SubscriberInfo = lSubInfo;
                    }
                    else if (lIsSkipGISPostFeederError)
                    {
                        string lJsonString = mdl_Publics.GetJSonString(lSubInfo);
                        SaveLog(String.Format("GetBillingDataClassWithBillingID Result (BillingID={0}): {1}", aBillingCode, lJsonString));

                        lResult.IsSuccess = true;
                        lResult.SubscriberInfo = lSubInfo;
                    }
                    else
                        throw new Exception("GetSubscriberGISInfoFromGIS Error = " + lSubGISInfo.ErrorMessage);
                }
                else
                    throw new Exception("GetBillingDataClass Error = " + lSubInfo.ErrorMessage);

            }
            catch (Exception ex)
            {
                lResult.ResultMessage = ex.Message;
                SaveLog(String.Format("GetBillingDataClassWithBillingID Result (BillingID={0}): {1}", aBillingCode, lResult.ResultMessage));
            }

            return lResult;
        }

        [WebMethod(Description = "دریافت فهرست انواع شناسه قبض")]
        public BillingIDTypeList GetBillingIDType()
        {
            BillingIDTypeList lRes = new BillingIDTypeList();

            List<BillingIDTypeData> lBillingIDTypeList = new List<BillingIDTypeData>();
            BillingIDTypeData lBillingIDType;
            try
            {
                lRes.IsSuccess = true;
                lRes.ResultMessage = "";
                DataSet lDs = new DataSet();
                string lMsg = mdl_Publics.BindingTable("select * from Homa.Tbl_BillingIDType", ref mCnn, lDs, "Tbl_BillingIDType", aIsClearTable: true);
                foreach (DataRow lRow in lDs.Tables["Tbl_BillingIDType"].Rows)
                {
                    lBillingIDType = new BillingIDTypeData();
                    lBillingIDType.BillingIDTypeId = (int)lRow["BillingIDTypeId"];
                    lBillingIDType.BillingIDType = lRow["BillingIDType"].ToString();
                    lBillingIDTypeList.Add(lBillingIDType);
                }

                lRes.Data = lBillingIDTypeList;
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
                SaveLog(String.Format("GetBillingIDType Result Error {0}", ex.Message));
            }

            return lRes;
        }

        [WebMethod(Description = "دریافت فهرست خاموشی‌های جاری و آینده مشترک از روی شناسه قبض")]
        public OutageListResult GetSubscriberOutageListByBillingID(string BillingId)
        {
            ServiceLog aoSLog = new ServiceLog();
            string lMsg = String.Format("Calling GetSubscriberOutageListByBillingID(BillingId={0})", BillingId);
            SaveLog(lMsg);
            CData.AddSLog(aoSLog, "SubscriberOutageListByBillingId", lMsg);

            OutageListResult lOutageListResult = null;
            SubscriberGISInfo lSGi = GetSubscriberNetworkFromBillingID(BillingId, aoSLog);
            

            if (string.IsNullOrWhiteSpace(lSGi.ErrorMessage))
            {
                lSGi.BillingId = BillingId;

                if (lSGi.MPPostId == -1)
                {
                    lOutageListResult = new OutageListResult();
                    lOutageListResult.IsSuccess = true;
                    List<OutageData> lOutageList = new List<OutageData>();
                    lOutageListResult.OutageList = lOutageList;
                    lOutageListResult.ResultMessage = "پست و فيدر مشترک از GIS دريافت نشد";
                }
                else
                {
                    lOutageListResult = Homa_GetOutagesByGISNetwork(lSGi, aoSLog);
                    if (lOutageListResult.IsSuccess)
                        CData.AddSLog(aoSLog, "SubscriberOutageListByBillingID", "SUCCESS: " + mdl_Publics.GetJSonString(lOutageListResult));
                    else
                        CData.AddSLog(aoSLog, "SubscriberOutageListByBillingID", "FAIL: " + lOutageListResult.ResultMessage);
                }
            }
            else
            {
                lOutageListResult = new OutageListResult();
                lOutageListResult.IsSuccess = false;
                lOutageListResult.ResultMessage = lSGi.ErrorMessage;
            }

            SaveLog(string.Format("GetSubscriberOutageListByFileNo Result ({0}): {1}", BillingId, mdl_Publics.GetJSonString(lOutageListResult)));

            return lOutageListResult;
        }

        [WebMethod(Description = "دریافت خاموشی جاری مشترک از روی شماره تلفن")]
        public SubscriberOutageResult GetSubscriberOutageInfoByCallerId(string CallerId)
        {
            SubscriberOutageResult lRes = new SubscriberOutageResult();
            lRes.IsSuccess = false;

            SaveLog(String.Format("Calling GetSubscriberOutageInfoByCallerId(CallerId={0})", CallerId));

            try
            {
                SubscriberOutageInfo lSubOutageInfo = new SubscriberOutageInfo();

                DataSet lDs = new DataSet();
                string lSQL = string.Format("exec Homa.spGetOutageInfoByCallerId @CallerId='{0}'", CallerId);
                SaveLog(String.Format("GetSubscriberBillingInfo EXEC SQL = {0}", lSQL));

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSubscriberOutageInfo", aIsClearTable: true);
                if (lMsg != "")
                    throw new Exception(lMsg);

                if (lDs.Tables.Contains("ViewSubscriberOutageInfo") && lDs.Tables["ViewSubscriberOutageInfo"].Rows.Count > 0)
                {

                    DataRow lRow = lDs.Tables["ViewSubscriberOutageInfo"].Rows[0];

                    lSubOutageInfo.BeginDate = lRow["DisconnectDatePersian"].ToString();
                    lSubOutageInfo.BeginTime = lRow["DisconnectTime"].ToString();
                    lSubOutageInfo.EndDate = lRow["ConnectDatePersian"].ToString();
                    lSubOutageInfo.EndTime = lRow["ConnectTime"].ToString();
                    lSubOutageInfo.IsPlanned = Convert.ToBoolean(lRow["IsTamir"]);
                    OutageStatus lStatus = (OutageStatus)Convert.ToInt32(lRow["STATUS"]);
                    lSubOutageInfo.Status = lStatus;
                    lSubOutageInfo.BillingId = lRow["BillingId"].ToString();

                    lRes.OutageInfo = lSubOutageInfo;

                    string lJson = mdl_Publics.GetJSonString(lRes);
                    SaveLog(String.Format("GetSubscriberOutageInfoByCallerId Result (CallerId={0}): {1}", CallerId, lJson));
                }
                else
                    lRes.ResultMessage = "Outage Not Found";

                lRes.IsSuccess = true;

            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
                SaveLog(String.Format("GetSubscriberOutageInfoByCallerId Result Error (CallerId={0}): {1}", CallerId, lRes.ResultMessage));
            }

            return lRes;
        }

        [WebMethod(Description = "دریافت فهرست خاموشی‌های آینده مشترک از روی شناسه قبض")]
        public TamirListResult GetSubscriberTamirListByBillingID(string BillingId, int ToTime)
        {
            ServiceLog aoSLog = new ServiceLog();
            string lMsg = String.Format("Calling GetSubscriberTamirListByBillingID(BillingId={0})", BillingId);
            SaveLog(lMsg);

            TamirListResult lOutageListResult = null;
            string lSQL = string.Format("exec homa.spFindSubscriberTamirOutageByBillingId '{0}',{1} ", BillingId, ToTime);
            DataSet lDs = new DataSet();
            lMsg = mdl_Publics.BindingTable(lSQL,ref mCnn,lDs,"ViewOutageList",aIsClearTable:true);
            if (lMsg.Length>0)
            SaveLog(lMsg);

            if (lDs.Tables.Contains("ViewOutageList") && lDs.Tables["ViewOutageList"].Rows.Count > 0)
            {
                lOutageListResult = new TamirListResult();
                lOutageListResult.IsSuccess = true;
                List<TamirData> lOutageList = new List<TamirData>();
                
                foreach (DataRow aRow in lDs.Tables["ViewOutageList"].Rows)
                {
                    TamirData lOutageData = new TamirData();
                    lOutageData.BeginDate = aRow["OutageStartDate"].ToString();
                    lOutageData.BeginTime = aRow["OutageStartTime"].ToString();
                    lOutageData.EndDate = aRow["OutageEndDate"].ToString();
                    lOutageData.EndTime = aRow["OutageEndTime"].ToString();
                    lOutageData.VoiceCode = aRow["VoiceCode"].ToString();
                    lOutageData.OutageNumber = aRow["OutageNumber"].ToString();
                    try
                    {
                        if (aRow["VoiceData"] != DBNull.Value && !string.IsNullOrWhiteSpace(aRow["VoiceData"].ToString()))
                        {
                            byte[] lVoice = (byte[])aRow["VoiceData"];
                            string lVoiceFile = System.BitConverter.ToString(lVoice);
                            lVoiceFile = "0x" + lVoiceFile.Replace("-", "");
                            lOutageData.VoiceData = lVoiceFile;
                        }
                        else
                            lOutageData.VoiceData = "";
                    }
                    catch { }

                    lOutageList.Add(lOutageData);
                }
                lOutageListResult.OutageList = lOutageList;
                lOutageListResult.ResultMessage = lOutageList.Count.ToString() + " خاموشي براي اين شناسه قبض يافت شد ";
            }
            else if (lDs.Tables.Contains("ViewOutageList") && lDs.Tables["ViewOutageList"].Rows.Count == 0)
            {
                lOutageListResult = new TamirListResult();
                lOutageListResult.IsSuccess = true;
                List<TamirData> lOutageList = new List<TamirData>();
                lOutageListResult.OutageList = lOutageList;
                lOutageListResult.ResultMessage = "هيچ خاموشي براي اين شناسه قبض يافت نشد";
            }
            else
            {
                lOutageListResult = new TamirListResult();
                lOutageListResult.IsSuccess = false;
                lOutageListResult.ResultMessage = lMsg;
            }

            SaveLog(string.Format("GetSubscriberOutageListByFileNo Result ({0}): {1}", BillingId, mdl_Publics.GetJSonString(lOutageListResult)));

            return lOutageListResult;
        }
        [WebMethod(Description="دریافت مشخصات پست و فیدر مشترک از روی شناسه قبض")]
        private SubscriberGISInfo GetSubscriberNetworkFromBillingID(string BillingId, ServiceLog aSLog = null)
        {
            SaveLog(String.Format("Calling GetSubscriberNetworkFromBillingID(BillingID={0})", BillingId));
            SubscriberGISInfo lSGi = new SubscriberGISInfo();

            try
            {
                int lLPFeederId = -1;
                int lLPPostId = -1;
                int lMPFeederId = -1;
                int lMPPostId = -1;

                string lSkipGISPostFeederError = System.Configuration.ConfigurationManager.AppSettings["SkipGISPostFeederError"];
                bool lIsSkipGISPostFeederError = false;
                if (!string.IsNullOrWhiteSpace(lSkipGISPostFeederError))
                    lIsSkipGISPostFeederError = Convert.ToBoolean(lSkipGISPostFeederError);


                DataSet lDs = new DataSet();
                string lSQL = string.Format("select * from Homa.TblRegisterBillingID where BillingID = {0}", BillingId);
                mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "TblRegisterBillingID", aIsClearTable: true);
                if (lDs.Tables.Contains("TblRegisterBillingID") && lDs.Tables["TblRegisterBillingID"].Rows.Count > 0)
                {
                    if (lDs.Tables["TblRegisterBillingID"].Rows[0]["LPFeederId"] != DBNull.Value)
                    {
                        lLPFeederId = (int)lDs.Tables["TblRegisterBillingID"].Rows[0]["LPFeederId"];
                        if (lLPFeederId > -1)
                        {
                            lSQL = string.Format("SELECT * FROM Tbl_LPFeeder WHERE LPFeederId = {0}", lLPFeederId);
                            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPF", aIsClearTable: true);
                            if (mDs.Tables.Contains("ViewLPF") && mDs.Tables["ViewLPF"].Rows.Count > 0)
                            {
                                lLPPostId = (int)mDs.Tables["ViewLPF"].Rows[0]["LPPostId"];
                            }
                        }
                        if (lLPPostId == -1)
                            lLPPostId = (int)lDs.Tables["TblRegisterBillingID"].Rows[0]["LPPostId"];
                        if (lLPPostId > -1)
                        {
                            lSQL = string.Format("SELECT * FROM Tbl_LPPost WHERE LPPostId = {0}", lLPPostId);
                            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPP", aIsClearTable: true);
                            if (mDs.Tables.Contains("ViewLPP") && mDs.Tables["ViewLPP"].Rows.Count > 0)
                            {
                                lMPFeederId = (int)mDs.Tables["ViewLPP"].Rows[0]["MPFeederId"];
                            }
                        }

                        if (lMPFeederId > -1)
                        {
                            lSQL = string.Format("SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = {0}", lMPFeederId);
                            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewMPF", aIsClearTable: true);
                            if (mDs.Tables.Contains("ViewMPF") && mDs.Tables["ViewMPF"].Rows.Count > 0)
                            {
                                lMPPostId = (int)mDs.Tables["ViewMPF"].Rows[0]["MPPostId"];
                            }
                        }

                    }

                    else if (lDs.Tables["TblRegisterBillingID"].Rows[0]["LPPostId"] != DBNull.Value)
                    {
                        lLPPostId = (int)lDs.Tables["TblRegisterBillingID"].Rows[0]["LPPostId"];
                        if (lLPPostId > -1)
                        {
                            lSQL = string.Format("SELECT * FROM Tbl_LPPost WHERE LPPostId = {0}", lLPPostId);
                            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPP", aIsClearTable: true);
                            if (mDs.Tables.Contains("ViewLPP") && mDs.Tables["ViewLPP"].Rows.Count > 0)
                            {
                                lMPFeederId = (int)mDs.Tables["ViewLPP"].Rows[0]["MPFeederId"];
                            }
                        }

                        if (lMPFeederId > -1)
                        {
                            lSQL = string.Format("SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = {0}", lMPFeederId);
                            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewMPF", aIsClearTable: true);
                            if (mDs.Tables.Contains("ViewMPF") && mDs.Tables["ViewMPF"].Rows.Count > 0)
                            {
                                lMPPostId = (int)mDs.Tables["ViewMPF"].Rows[0]["MPPostId"];
                            }
                        }
                    }


                }

                if (!lIsSkipGISPostFeederError && lMPPostId == -1)
                {
                    throw new Exception(string.Format("No Post and Feeder in BillingId {0}", BillingId));
                }


                lSGi.MPPostId = lMPPostId;
                lSGi.MPFeederId = lMPFeederId;
                lSGi.LPPostId = lLPPostId;
                lSGi.LPFeederId = lLPFeederId;

            }
            catch (Exception ex)
            {
                lSGi.ErrorMessage = ex.Message;
                string lMsg = string.Format("GetSubscriberGISInfoFromGIS Error: {0}", ex.Message);
                SaveLog(lMsg, System.Diagnostics.EventLogEntryType.Error);
            }

            return lSGi;
        }

        [WebMethod(Description = "دریافت فهرست خاموشی‌های جاری روی شبکه معرفی شده - طرح هما")]
        public OutageListResult Homa_GetOutageInfo(NetworkInfo NetworkData)
        {
            SaveLog(String.Format("Calling GetOutageInfo(NetworkData={0})", mdl_Publics.GetJSonString(NetworkData)));
            string lMsg = "";

            DataSet lDs = new DataSet();
            NetworkInfo lParams = NetworkData;
            List<OutageData> lOutageList = new List<OutageData>();
            OutageListResult lRes = new OutageListResult();
            lRes.IsSuccess = true;
            lRes.ResultMessage = "";
            lRes.OutageList = lOutageList;

            try
            {
                int lLPFeederId = 0;
                int lLPPostId = 0;
                int lMPFeederId = 0;

                string lSQL = "";

                if (!string.IsNullOrWhiteSpace( lParams.MPFeederCode))
                {
                    lSQL = string.Format("SELECT * FROM Tbl_MPFeeder WHERE MPFeederCode = '{0}'", lParams.MPFeederCode);
                    lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewMPF", aIsClearTable: true);
                    if (mDs.Tables.Contains("ViewMPF") && mDs.Tables["ViewMPF"].Rows.Count > 0)
                        lMPFeederId = (int)mDs.Tables["ViewMPF"].Rows[0]["MPFeederId"];
                }
                if (!string.IsNullOrWhiteSpace(lParams.LPPostCode))
                {
                    lSQL = string.Format("SELECT * FROM Tbl_LPPost WHERE LPPostCode = '{0}'", lParams.LPPostCode);
                    lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPP", aIsClearTable: true);
                    if (mDs.Tables.Contains("ViewLPP") && mDs.Tables["ViewLPP"].Rows.Count > 0)
                        lLPPostId = (int)mDs.Tables["ViewLPP"].Rows[0]["LPPostId"];
                }
                if (!string.IsNullOrWhiteSpace(lParams.LPFeederCode))
                {
                    lSQL = string.Format("SELECT * FROM Tbl_LPFeeder WHERE LPFeederCode = '{0}'", lParams.LPFeederCode);
                    lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPF", aIsClearTable: true);
                    if (mDs.Tables.Contains("ViewLPF") && mDs.Tables["ViewLPF"].Rows.Count > 0)
                        lLPFeederId = (int)mDs.Tables["ViewLPF"].Rows[0]["LPFeederId"];
                }


                lSQL = string.Format(
                    "EXEC Homa.spFindSubscriberOutage " +
                    "@aLPFeederId = {0}, @aLPPostId = {1}, @aMPFeederId = {2}, @aBillingID = '{3}'",
                    lLPFeederId, lLPPostId, lMPFeederId, lParams.BillingId);

                SaveLog(string.Format("Executing query: {0}", lSQL));
                lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewOutageInfo", aIsClearTable: true);

                if (lDs.Tables.Contains("ViewOutageInfo") && lDs.Tables["ViewOutageInfo"].Rows.Count > 0)
                {
                    lRes.ResultMessage = string.Format("تعداد {0} خاموشی یافت شد", lDs.Tables["ViewOutageInfo"].Rows.Count);
                    foreach (DataRow lRow in lDs.Tables["ViewOutageInfo"].Rows)
                    {
                        OutageData lInfo = new OutageData();
                        lInfo.RequestNumber = lRow["RequestNumber"].ToString();
                        lInfo.BeginDate = lRow["StartOutageDate"].ToString();
                        lInfo.BeginTime = lRow["StartOutageTime"].ToString();
                        lInfo.EndDate = lRow["EndOutageDate"].ToString();
                        lInfo.EndTime = lRow["EndOutageTime"].ToString();
                        lInfo.IsPlanned = Convert.ToBoolean(lRow["IsPlanned"]);
                        lInfo.Address = lRow["Address"].ToString();
                        try
                        {
                            OutageStatus lStatus = (OutageStatus)Convert.ToInt32(lRow["EndJobStateId"]);
                            lInfo.Status = lStatus;

                        }
                        catch (Exception)
                        { }

                        lInfo.Desciption = "";

                        if (string.IsNullOrWhiteSpace(lInfo.EndTime))
                            lInfo.EndTime = "نامعلوم";

                        lOutageList.Add(lInfo);
                    }
                }
                else
                {
                    OutageData lInfo = new OutageData();
                    lOutageList.Add(lInfo);
                    lRes.ResultMessage = "هیچ خاموشی یافت نشد";
                    if (lMsg.Length > 0)
                        lRes.ResultMessage += ": " + lMsg;
                }
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(String.Format("GetOutageInfo Result: ({0})", mdl_Publics.GetJSonString(lRes)));
            return lRes;
        }

        [WebMethod(Description = "دريافت صداهاي ضبط شده مربوط به شناسه قبض - طرح هما")]
        public BillingVoiceResult Homa_GetIVRVoice(string BillingId)
        {
            SaveLog(String.Format("Calling GetIVRVoice(BillingId={0})", BillingId));
            BillingVoiceResult lRes = new BillingVoiceResult();
            BillingVoiceData lVoiceData = null;

            try
            {
                string lSQL = string.Format("EXEC Homa.spGetMPVoiceData @BillingId = '{0}'", BillingId);
                SaveLog("Executing query: " + lSQL);

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewVoiceData", aIsClearTable: true);
                if (mDs.Tables.Contains("ViewVoiceData") && mDs.Tables["ViewVoiceData"].Rows.Count > 0)
                {
                    DataRow lRow = mDs.Tables["ViewVoiceData"].Rows[0];
                    lVoiceData = new BillingVoiceData();
                    if (lRow["VoiceId"] != DBNull.Value)
                        lVoiceData.VoiceId = lRow["VoiceId"].ToString();
                    if (lRow["VoiceCode"] != DBNull.Value)
                        lVoiceData.VoiceCode = lRow["VoiceCode"].ToString();

                    try
                    {
                        if (lRow["VoiceData"] != DBNull.Value && !string.IsNullOrWhiteSpace(lRow["VoiceData"].ToString()))
                        {
                            byte[] lVoice = (byte[])lRow["VoiceData"];
                            string lVoiceFile = System.BitConverter.ToString(lVoice);
                            lVoiceFile = "0x" + lVoiceFile.Replace("-", "");
                            lVoiceData.VoiceData = lVoiceFile;
                        }
                        else
                            lVoiceData.VoiceData = "";
                    }
                    catch { }


                    lRes.ResultMessage = string.Format("MPFeederId found. MPFeederId= {0}", lRow["MPFeederId"].ToString());
                }
                else if (lMsg.Length > 0)
                    throw new Exception(lMsg);
                else
                    lRes.ResultMessage = "No Voice found.";

                lRes.IsSuccess = true;
                lRes.VoiceInfo = lVoiceData;
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetIVRVoice Result (BillingId={0}:{1})", BillingId, mdl_Publics.GetJSonString(lRes)));

            return lRes;
        }

        [WebMethod(Description = "دریافت وضعیت مشترک از نظر بدهی - طرح هما")]
        public SubscriberStatusResult Homa_GetSubscriberStatus(string BillingId)
        {
            SaveLog(String.Format("Calling GetSubscriberStatus(BillingId={0})", BillingId));

            string lSubRes = "";
            SubscriberStatusResult lRes = new SubscriberStatusResult();
            try
            {
                SubscriberStatusData lSubData = new SubscriberStatusData();

                ExternalLinks lExLinks = new ExternalLinks();
                lSubRes = lExLinks.GetSubscriberInfo(BillingId, ExternalLinks.SubscriberCodeTypes.BillingID, -1);

                ExternalLinks.CSubscriberInfo lSubscriberInfo = new ExternalLinks.CSubscriberInfo();
                lSubscriberInfo = mdl_Publics.GetClassFromJson<ExternalLinks.CSubscriberInfo>(lSubRes);

                lSubData.IsActive = true;
                if (lSubscriberInfo.SubscriberStateId==1035)
                    lSubData.IsActive = false;
                lSubData.DebtPrice = lSubscriberInfo.DebtPrice.ToString();

                lRes.IsSuccess = true;
                lRes.Status = lSubData;
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }

            lSubRes = mdl_Publics.GetJSonString(lRes);

            SaveLog(string.Format("GetSubscriberStatus Result (BillingId={0}:{1})", BillingId, lSubRes));

            return lRes;
        }

        [WebMethod(Description = "ارسال پيامک به مشترکين - طرح هما")]
        public SMSResult Homa_SendSubscriberOutageSMS(string MobileNo, string SMSBody)
        {
            SaveLog(String.Format("Calling SendSubscriberOutageSMS(MobileNo={0}, SMSBody={1})", MobileNo, SMSBody));

            SMSResult lRes = new SMSResult();
            try
            {
                DataSet lDs = new DataSet();
                string lSQL;
                string lError= "";

                lSQL = string.Format("exec spCreateSMS N'{0}','{1}',N'{2}',NULL", SMSBody, MobileNo, "HomaMsg");

                lError = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "Tbl_SMS", aIsClearTable: true);
                if (lError.Length > 0)
                    throw new Exception(lError);

                lRes.IsSuccess = true;
                lRes.ResultMessage = "SMS Sended";
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ResultMessage = ex.Message;
            }
            SaveLog(string.Format("GetSubscriberStatus Result (MobileNo={0}:{1})", MobileNo, mdl_Publics.GetJSonString(lRes)));

            return lRes;
        }

        [WebMethod(Description = "دريافت همه شناسه قبضهای مربوط به يک شماره موبايل")]
        public BillingInfoResult GetAllBillingFromMobile(string MobileNo)
        {
            BillingInfoResult lRes = new BillingInfoResult();
            lRes.IsSuccess = false;

            if (string.IsNullOrWhiteSpace(MobileNo))
            {
                lRes.ResultMessage = "MobileNo Is Null";
                return lRes;
            }

            SaveLog(String.Format("Calling GetAllBillingFromMobile(MobileNo={0})", MobileNo));

            try
            {
                BillingInfoData lBillingInfo;
                List<BillingInfoData> lBillingData = new List<BillingInfoData>();

                DataSet lDs = new DataSet();
                string lSQL = string.Format("exec Homa.spGetAllBillingFromMobile '{0}'", MobileNo);
                SaveLog(String.Format("GetAllBillingFromMobile EXEC SQL = {0}", lSQL));

                string lMsg = mdl_Publics.BindingTable(lSQL, ref mCnn, lDs, "ViewSubscriberBillingList", aIsClearTable: true);

                if (lDs.Tables.Contains("ViewSubscriberBillingList"))
                {
                    foreach (DataRow lRow in lDs.Tables["ViewSubscriberBillingList"].Rows)
                    {
                        lBillingInfo = new BillingInfoData();
                        lBillingInfo.BillingID = lRow["BillingID"].ToString();
                        lBillingInfo.Address = lRow["Address"].ToString();
                        lBillingInfo.SubscriberName = lRow["SubscriberName"].ToString();
                        lBillingData.Add(lBillingInfo);
                    }
                    lRes.IsSuccess = true;
                    lRes.Data = lBillingData;
                    lRes.ResultMessage = "";

                    string lJson = mdl_Publics.GetJSonString(lRes);
                    lJson = CFunctions.GetClearJsonString(lJson, true);
                    SaveLog(String.Format("GetAllBillingFromMobile Result (MobileNo={0}): {1}", MobileNo, lJson));
                }
                else
                    throw new Exception(lMsg);

            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.Data = null;
                lRes.ResultMessage = ex.Message;
                SaveLog(String.Format("GetAllBillingFromMobile Result Error (MobileNo={0}): {1}", MobileNo, lRes.ResultMessage));
            }

            return lRes;
        }
        //---------------------omid
        [WebMethod(Description = "دریافت فهرست خاموشی‌های جاری و آینده مشترک از روی شناسه قبض")]
        public OutageListResult GetSubscriberOutageListByBillMobAddr(string BillingId, string MobileNo, string Address)
        {
            ServiceLog aoSLog = new ServiceLog();
            string lMsg = String.Format("Calling GetSubscriberOutageListByBillMobAddr(BillingId={0},"+
                "MobileNo={1},Address={2})", BillingId,MobileNo,Address);
            SaveLog(lMsg);
            CData.AddSLog(aoSLog, "GetSubscriberOutageListByBillMobAddr", lMsg);

            OutageListResult lOutageListResult = null;
            //---------------ToDo (The Rest):
            SubscriberGISInfo lSGi = GetSubscriberNetworkFromBillingID(BillingId, aoSLog);

            if (string.IsNullOrWhiteSpace(lSGi.ErrorMessage))
            {
                lSGi.BillingId = BillingId;

                if (lSGi.MPPostId == -1)
                {
                    lOutageListResult = new OutageListResult();
                    lOutageListResult.IsSuccess = true;
                    List<OutageData> lOutageList = new List<OutageData>();
                    lOutageListResult.OutageList = lOutageList;
                    lOutageListResult.ResultMessage = "پست و فيدر مشترک از GIS دريافت نشد";
                }
                else
                {
                    lOutageListResult = Homa_GetOutagesByGISNetwork(lSGi, aoSLog);
                    if (lOutageListResult.IsSuccess)
                        CData.AddSLog(aoSLog, "SubscriberOutageListByBillingID", "SUCCESS: " + mdl_Publics.GetJSonString(lOutageListResult));
                    else
                        CData.AddSLog(aoSLog, "SubscriberOutageListByBillingID", "FAIL: " + lOutageListResult.ResultMessage);
                }
            }
            else
            {
                lOutageListResult = new OutageListResult();
                lOutageListResult.IsSuccess = false;
                lOutageListResult.ResultMessage = lSGi.ErrorMessage;
            }

            SaveLog(string.Format("GetSubscriberOutageListByFileNo Result ({0}): {1}", BillingId, mdl_Publics.GetJSonString(lOutageListResult)));

            return lOutageListResult;
        }
        /*-------------------------------*/
        /*       Private Methods         */
        /*-------------------------------*/

        private void SaveLog(string aLogMessage, System.Diagnostics.EventLogEntryType aLogType = System.Diagnostics.EventLogEntryType.Information)
        {
            mdl_Publics.SaveLog(Server, aLogMessage, "TZServices_Inform_Logs", aLogType: aLogType);
        }

        //[WebMethod(Description="دریافت مشخصات مشترک از روی کد خاص")]
        private SubscriberInfo GetSubscriberInfoFromBilling(string aBillingCode, BillingTypes aBillingType)
        {
			SaveLog(String.Format("Calling GetSubscriberInfoFromBilling(BillingID={0},BillingType={1})", aBillingCode, (int)aBillingType));
            SubscriberInfo lSi = new SubscriberInfo();

            try
            {
                string lToziName = System.Configuration.ConfigurationManager.AppSettings["ToziName"];
                if (string.IsNullOrWhiteSpace(lToziName))
                    lToziName = "";
                string lBillingCo = System.Configuration.ConfigurationManager.AppSettings["BillingCo"];
                if (string.IsNullOrWhiteSpace(lBillingCo))
                    lBillingCo = "";

                lToziName = lToziName.Trim().ToLower();
                lBillingCo = lBillingCo.Trim().ToLower();

                if (lBillingCo == "" || lToziName == "")
                    throw new Exception("وب سرویس سامانه جامع مشترکین مشخص نشده است");

                ExternalLinks lExtLink = new ExternalLinks();
                ExternalLinks.CSubscriberInfo lSubInfo = lExtLink.GetBillingDataClass(aBillingCode, (ExternalLinks.SubscriberCodeTypes)(int)(aBillingType));

                SaveLog(String.Format("Result GetSubscriberInfoFromBilling (lSubInfo={0})", mdl_Publics.GetJSonString(lSubInfo)));

                if (string.IsNullOrWhiteSpace(lSubInfo.ErrorMessage) || lSubInfo.ErrorMessage.Length == 0)
                {
                    lSi.Name = lSubInfo.SubscriberName;
                    lSi.Code = lSubInfo.SubscriberCode;
                    lSi.FileNo = lSubInfo.FileNo;
                    lSi.RamzCode = lSubInfo.RamzCode;
                    lSi.BillingID = lSubInfo.BillingID;
                    lSi.Telephone = lSubInfo.Telephone;
                    lSi.MobileNo = lSubInfo.TelMobile;
                    lSi.AreaId = lSubInfo.AreaId;
                    lSi.Address = lSubInfo.Address;
                    lSi.PostalCode = lSubInfo.PostalCode;
                    lSi.GPSx = lSubInfo.GPSx;
                    lSi.GPSy = lSubInfo.GPSy;
                    lSi.DebtValue = lSubInfo.DebtPrice;
                    lSi.Status = lSubInfo.SubscriberStateId;
                }
                else
                    throw new Exception(lSubInfo.ErrorMessage);

                SaveLog(String.Format("Result GetSubscriberInfoFromBilling (lSi={0})", mdl_Publics.GetJSonString(lSi)));
            }
            catch (Exception ex)
            {
                lSi.ErrorMessage = ex.Message;
                string lMsg = string.Format("GetSubscriberInfoFromBilling Error: {0}", ex.Message);
                SaveLog(lMsg, System.Diagnostics.EventLogEntryType.Error);
            }

            return lSi;
        }
        
        //[WebMethod(Description="دریافت مشخصات پست و فیدر مشترک از روی کد خاص")]
		private SubscriberGISInfo GetSubscriberGISInfoFromGIS(string aBillingCode, BillingTypes aBillingType, ServiceLog aSLog = null)
		{
			SaveLog(String.Format("Calling GetSubscriberGISInfoFromGIS(BillingID={0},BillingType={1})", aBillingCode, (int)aBillingType));
			SubscriberGISInfo lSGi = new SubscriberGISInfo();

			try
			{
				string lToziName = System.Configuration.ConfigurationManager.AppSettings["ToziName"];
				if (string.IsNullOrWhiteSpace(lToziName))
					lToziName = "";
				string lGISCo = System.Configuration.ConfigurationManager.AppSettings["GISCo"];
				if (string.IsNullOrWhiteSpace(lGISCo))
					lGISCo = "";

				lToziName = lToziName.Trim().ToLower();
				lGISCo = lGISCo.Trim().ToLower();

				if (lGISCo == "" || lToziName == "")
					throw new Exception("وب سرویس جی.آی.اس مشخص نشده است");


				ExternalLinks.SubscriberParentParams lParams = new ExternalLinks.SubscriberParentParams();
				if (aBillingType == BillingTypes.FileNo)
					lParams.SubscriberFileNo = aBillingCode;
				else if (aBillingType == BillingTypes.SubNo)
					lParams.SubscriberNo = aBillingCode;
				else if (aBillingType == BillingTypes.RamzCode)
					lParams.SubscriberID = aBillingCode;
				else if (aBillingType == BillingTypes.TelNo)
					lParams.SubscriberTel = aBillingCode;
				else if (aBillingType == BillingTypes.BillingId)
					lParams.SubscriberBillingID = aBillingCode;

				ExternalLinks lExtLink = new ExternalLinks();
				ExternalLinks.CSubscriberInfo lSubInfo = lExtLink.GetSubscriberParentClass(lParams, aSLog);

				if (string.IsNullOrWhiteSpace(lSubInfo.ErrorMessage) || lSubInfo.ErrorMessage.Length == 0)
				{
					lSGi.MPPostId = lSubInfo.MPPostId;
					lSGi.MPFeederId = lSubInfo.MPFeederId;
					lSGi.LPPostId = lSubInfo.LPPostId;
					lSGi.LPFeederId = lSubInfo.LPFeederId;
					lSGi.GPSx = lSubInfo.GPSx;
                    lSGi.GPSy = lSubInfo.GPSy;
                    lSGi.Wkid = lSubInfo.Wkid;
				}
				else
					throw new Exception(lSubInfo.ErrorMessage);
			}
			catch (Exception ex)
			{
				lSGi.ErrorMessage = ex.Message;
				string lMsg = string.Format("GetSubscriberGISInfoFromGIS Error: {0}", ex.Message);
				SaveLog(lMsg, System.Diagnostics.EventLogEntryType.Error);
			}

			return lSGi;
		}
        
		//[WebMethod(Description="دریافت کد پست و فیدر مشترک از روی کد خاص")]
        private NetworkCodeInfo GetSubscriberGISInfoFromGISCodes(string aBillingCode, BillingTypes aBillingType)
        {
            SaveLog(String.Format("Calling GetSubscriberGISInfoFromGIS(BillingID={0},BillingType={1})", aBillingCode, (int)aBillingType));
			NetworkCodeInfo lNCi = null;

            try
            {
                string lToziName = System.Configuration.ConfigurationManager.AppSettings["ToziName"];
                if (string.IsNullOrWhiteSpace(lToziName))
                    lToziName = "";
                string lGISCo = System.Configuration.ConfigurationManager.AppSettings["GISCo"];
                if (string.IsNullOrWhiteSpace(lGISCo))
                    lGISCo = "";

                lToziName = lToziName.Trim().ToLower();
                lGISCo = lGISCo.Trim().ToLower();

                if (lGISCo == "" || lToziName == "")
                    throw new Exception("وب سرویس جی.آی.اس مشخص نشده است");

                ExternalLinks.SubscriberParentParams lParams = new ExternalLinks.SubscriberParentParams();
                if (aBillingType == BillingTypes.FileNo)
                    lParams.SubscriberFileNo = aBillingCode;
                else if (aBillingType == BillingTypes.SubNo)
                    lParams.SubscriberNo = aBillingCode;
                else if (aBillingType == BillingTypes.RamzCode)
                    lParams.SubscriberID = aBillingCode;
                else if (aBillingType == BillingTypes.TelNo)
                    lParams.SubscriberTel = aBillingCode;

                ExternalLinks lExtLink = new ExternalLinks();
                lNCi = lExtLink.GetSubscriberParentCodes(lParams);

				if (lNCi.IsSuccess)
				{
					if (string.IsNullOrWhiteSpace(lNCi.GISCode))
						lNCi.ResultMessage = "هیچ پست و فیدری برای مشترک با کد ارائه شده پیدا نشد.";
				}
            }
            catch (Exception ex)
            {
                string lMsg = string.Format("GetSubscriberGISInfoFromGIS Error: {0}", ex.Message);
                SaveLog(lMsg, System.Diagnostics.EventLogEntryType.Error);
            }

            return lNCi;
        }
        
        //[WebMethod(Description="دریافت پست و فيدر مشترک از روی کد خاص")]
        private SubscriberGISInfo GetSubscriberNetwork(string aBillingCode, BillingTypes aBillingType)
        {
            SaveLog(String.Format("Calling GetSubscriberNetwork(BillingID={0},BillingType={1})", aBillingCode, (int)aBillingType));
            SubscriberGISInfo lSGi = new SubscriberGISInfo();
			try
			{
				SaveLog("Step1");
				lSGi = GetSubscriberGISInfoFromGIS(aBillingCode, aBillingType);
				if (lSGi.ErrorMessage.Length > 0)
					throw new Exception(lSGi.ErrorMessage);
				SaveLog("Step2");
			}
			catch (Exception ex)
			{
				lSGi.ErrorMessage = ex.Message;
				string lMsg = string.Format("GetSubscriberNetwork Error: {0}", ex.Message);
				SaveLog(lMsg, System.Diagnostics.EventLogEntryType.Error);
			}
            return lSGi;
        }

		private OutageListResult GetOutagesByGISNetwork(SubscriberGISInfo aSubscriberGISInfo, ServiceLog aSLog = null)
        {
            string lMsg = "";
            SaveLog(String.Format("Calling GetOutagesByGISNetwork(aSubscriberGISInfo={0})", mdl_Publics.GetJSonString(aSubscriberGISInfo)));
			CData.AddSLog(aSLog, "Find Outages", "فراخوانی خاموشی‌های روی پست و فیدر یافت شده");

            OutageListResult lOutageListResult = null;

            int lMPPostId = -1;
            int lMPFeederId = -1;
            int lLPPostId = -1;
            int lLPFeederId = -1;

            string lMPPostCode = "";
            string lMPFeederCode = "";
            string lLPPostCode = "";
            string lLPFeederCode = "";

            NetworkInfo lNi = null;
            bool lIsFoundGISNetwork = false;

            string lSQL = "";
            try
            {
                SubscriberGISInfo lSGi = aSubscriberGISInfo;
                if (lSGi.ErrorMessage.Length > 0)
                    lSGi = null;
                if (lSGi != null && lSGi.ErrorMessage.Length == 0)
                {
                    lMPPostId = lSGi.MPPostId;
                    lMPFeederId = lSGi.MPFeederId;
                    lLPPostId = lSGi.LPPostId;
                    lLPFeederId = lSGi.LPFeederId;
                }

                SaveLog(string.Format("GIS Network Result: IDs< MPPostId({0}) , MPFeederId({1}) , LPPostId({2}) , LPFeederId({3}) >", lMPPostId, lMPFeederId, lLPPostId, lLPFeederId));
                try
                {
                    //-----------<omid>
                    lIsFoundGISNetwork = Get_MPPost(lMPPostId, ref lMPPostCode) 
                        | Get_LPPost(lLPPostId, ref lLPPostCode)
                        | Get_MPFeeder(lMPFeederId, ref lMPFeederCode)
                        | Get_LPFeeder(lLPFeederId, ref lLPFeederCode);
                    //-----------</omid>
                    SaveLog(string.Format("GIS Network Result: Codes< MPPostCode({0}) , MPFeederCode({1}) , LPPostCode({2}) , LPFeederCode({3}) >", lMPPostCode, lMPFeederCode, lLPPostCode, lLPFeederCode));
                }
                catch (Exception ex)
                {
                    SaveLog(string.Format("GIS Fetch Code Error: {0}", ex.Message));
                }

                if (lIsFoundGISNetwork)
                {
                    lNi = new NetworkInfo()
                    {
                        MPPostCode = lMPPostCode,
                        MPFeederCode = lMPFeederCode,
                        LPPostCode = lLPPostCode,
                        LPFeederCode = lLPFeederCode
                    };
                    lOutageListResult = GetOutageInfo(lNi);
                }
                else
                    throw new Exception("مکان مشترک در شبکه توزیع برق یافت نشد");
            }
            catch (Exception ex)
            {
                lOutageListResult = new OutageListResult();
                lOutageListResult.IsSuccess = false;
                lOutageListResult.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetOutagesByGISNetwork Result: {0}", mdl_Publics.GetJSonString(lOutageListResult)));

            return lOutageListResult;
        }
        private SubscriberOutageInfo GetCurrentOutageFromList(OutageListResult aOLR)
        {
            SubscriberOutageInfo lOutageInfo = null;

            if (aOLR != null && aOLR.IsSuccess && aOLR.OutageList != null && aOLR.OutageList.Count > 0)
            {
                lOutageInfo = new SubscriberOutageInfo();
                OutageData lCurrentOutage = null;

                foreach (OutageData lOutage in aOLR.OutageList)
                {
                    TZServiceTools.PersianDate.CTimeInfo lTi = PersianDate.GetServerTimeInfo();
                    if (lOutage.BeginDate.CompareTo(lTi.ShamsiDate) <= 0 && (lOutage.EndDate == "" || lOutage.EndDate.CompareTo(lTi.ShamsiDate) >= 0))
                    {
                        if (lOutage.BeginTime.CompareTo(lTi.HourMin) <= 0 && (lOutage.EndTime == "" || lOutage.EndTime.CompareTo(lTi.HourMin) >= 0))
                        {
                            lCurrentOutage = lOutage;
                            break;
                        }
                    }
                }
                if (lCurrentOutage != null)
                {
                    lOutageInfo.BeginDate = lCurrentOutage.BeginDate;
                    lOutageInfo.BeginTime = lCurrentOutage.BeginTime;
                    lOutageInfo.EndDate = lCurrentOutage.EndDate;
                    lOutageInfo.EndTime = lCurrentOutage.EndTime;
                    lOutageInfo.IsPlanned = lCurrentOutage.IsPlanned;
                    lOutageInfo.Status = OutageStatus.NewOutage;
                    lOutageInfo.DebtPrice = 0;
                }
            }
            return lOutageInfo;
        }

        private OutageListResult Homa_GetOutagesByGISNetwork(SubscriberGISInfo aSubscriberGISInfo, ServiceLog aSLog = null)
        {
            string lMsg = "";
            SaveLog(String.Format("Calling GetOutagesByGISNetwork(aSubscriberGISInfo={0})", mdl_Publics.GetJSonString(aSubscriberGISInfo)));
            CData.AddSLog(aSLog, "Find Outages", "فراخوانی خاموشی‌های روی پست و فیدر یافت شده");

            OutageListResult lOutageListResult = null;

            int lMPPostId = -1;
            int lMPFeederId = -1;
            int lLPPostId = -1;
            int lLPFeederId = -1;

            string lMPPostCode = "";
            string lMPFeederCode = "";
            string lLPPostCode = "";
            string lLPFeederCode = "";

            NetworkInfo lNi = null;
            bool lIsFoundGISNetwork = false;

            string lSQL = "";
            try
            {
                SubscriberGISInfo lSGi = aSubscriberGISInfo;
                if (lSGi.ErrorMessage.Length > 0)
                    lSGi = null;
                if (lSGi != null && lSGi.ErrorMessage.Length == 0)
                {
                    lMPPostId = lSGi.MPPostId;
                    lMPFeederId = lSGi.MPFeederId;
                    lLPPostId = lSGi.LPPostId;
                    lLPFeederId = lSGi.LPFeederId;
                }
                SaveLog(string.Format("GIS Network Result: IDs< MPPostId({0}) , MPFeederId({1}) , LPPostId({2}) , LPFeederId({3}) >", lMPPostId, lMPFeederId, lLPPostId, lLPFeederId));
                try
                {
                    //-----------<omid>
                    lIsFoundGISNetwork = Get_MPPost(lMPPostId, ref lMPPostCode)
                        | Get_LPPost(lLPPostId, ref lLPPostCode)
                        | Get_MPFeeder(lMPFeederId, ref lMPFeederCode)
                        | Get_LPFeeder(lLPFeederId, ref lLPFeederCode);
                    //-----------</omid>
                    SaveLog(string.Format("GIS Network Result: Codes< MPPostCode({0}) , MPFeederCode({1}) , LPPostCode({2}) , LPFeederCode({3}) >", lMPPostCode, lMPFeederCode, lLPPostCode, lLPFeederCode));
                }
                catch (Exception ex)
                {
                    SaveLog(string.Format("GIS Fetch Code Error: {0}", ex.Message));
                }

                if (lIsFoundGISNetwork)
                {
                    lNi = new NetworkInfo()
                    {
                        MPPostCode = lMPPostCode,
                        MPFeederCode = lMPFeederCode,
                        LPPostCode = lLPPostCode,
                        LPFeederCode = lLPFeederCode,
                        BillingId = lSGi.BillingId
                    };
                    lOutageListResult = Homa_GetOutageInfo(lNi);
                }
                else
                    throw new Exception("مکان مشترک در شبکه توزیع برق یافت نشد");
            }
            catch (Exception ex)
            {
                lOutageListResult = new OutageListResult();
                lOutageListResult.IsSuccess = false;
                lOutageListResult.ResultMessage = ex.Message;
            }

            SaveLog(string.Format("GetOutagesByGISNetwork Result: {0}", mdl_Publics.GetJSonString(lOutageListResult)));

            return lOutageListResult;
        }
        //-------------omid
        private bool Get_MPPost(int aMPPostId, ref string aMPPostCode)
        {
            if (aMPPostId <= 0) return false;
            string lSQL = string.Format("SELECT * FROM Tbl_MPPost WHERE MPPostId = {0}", aMPPostId);
            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewMPP", aIsClearTable: true);
            if (mDs.Tables.Contains("ViewMPP") && mDs.Tables["ViewMPP"].Rows.Count > 0)
            {
                aMPPostCode = mDs.Tables["ViewMPP"].Rows[0]["MPPostCode"].ToString();
                return true;
            }
            return false;
        }
        //-------------omid
        private bool Get_MPFeeder(int aMPFeederId, ref string aMPFeederCode)
        {
            if (aMPFeederId <= 0) return false;
            string lSQL = string.Format("SELECT * FROM Tbl_MPFeeder WHERE MPFeederId = {0}", aMPFeederId);
            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewMPF", aIsClearTable: true);
            if (mDs.Tables.Contains("ViewMPF") && mDs.Tables["ViewMPF"].Rows.Count > 0)
            {
                aMPFeederCode = mDs.Tables["ViewMPF"].Rows[0]["MPFeederCode"].ToString();
                return true;
            }
            return false;
        }
        //-------------omid
        private bool Get_LPPost(int aLPPostId, ref string aLPPostCode)
        {
            if (aLPPostId <= 0) return false;
            string lSQL = string.Format("SELECT * FROM Tbl_LPPost WHERE LPPostId = {0}", aLPPostId);
            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPP", aIsClearTable: true);
            if (mDs.Tables.Contains("ViewLPP") && mDs.Tables["ViewLPP"].Rows.Count > 0)
            {
                aLPPostCode = mDs.Tables["ViewLPP"].Rows[0]["LPPostCode"].ToString();
                return true;
            }
            return false;
        }
        //-------------omid
        private bool Get_LPFeeder(int aLPFeederId, ref string aLPFeederCode)
        {
            if (aLPFeederId <= 0) return false;
            string lSQL = string.Format("SELECT * FROM Tbl_LPFeeder WHERE LPFeederId = {0}", aLPFeederId);
            mdl_Publics.BindingTable(lSQL, ref mCnn, mDs, "ViewLPF", aIsClearTable: true);
            if (mDs.Tables.Contains("ViewLPF") && mDs.Tables["ViewLPF"].Rows.Count > 0)
            {
                aLPFeederCode = mDs.Tables["ViewLPF"].Rows[0]["LPFeederCode"].ToString();
                return true;
            }
            return false;
        }
    }
}
