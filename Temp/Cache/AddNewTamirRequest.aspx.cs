using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using TZServicesCSharp.Classes;
using TZServiceTools;
//--------------------omid----------------------
namespace TZServicesCSharp.RestServices
{
    public partial class AddNewTamirRequest : CPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string lResult = "";
            string lInputData = "";
            string lInformType = "";
            HavadesTamir lTamirRequest = new HavadesTamir();
            Danesh_Result lRes = new Danesh_Result();
            lRes.IsSuccess = true;
            try
            {
                lInformType = GetQueryString("TamirType");

                string lMethodNames =
                    "^(maz_setrequesttamir)$";
                if (!Regex.IsMatch(lInformType, lMethodNames, RegexOptions.IgnoreCase))
                    throw new Exception("درخواست شما معتبر نمی باشد");
                lInputData = GetQueryStringOrFirstParameter("param");
                inputParams lParams = mdl_Publics.GetClassFromJson<inputParams>(lInputData);

                if (lParams == null)
                {
                    throw new Exception("پارامتر ورودی نامعتبر است");
                }
                mdl_Publics.LogMessage(Server,
                    string.Format("HavadesReports({0}) REST input data: {1}", lInformType, lInputData),
                    "TZServices_Reports_Logs");

                DataSet lDs = new DataSet();
                switch (lInformType.ToLower())
                {
                    case "maz_setrequesttamir":
                        lRes.Data = lTamirRequest.SetTamirRequestInfo_Maz(lParams.TamirId, lParams.TamirInfo);
                        //lRes = lReportService.AzarGH_PostFeederLoad(ch.lCode , ch.lMinCount);
                        //lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    default:
                        throw new Exception("درخواست شما معتبر نمی باشد");
                }
            }
            catch (Exception ex)
            {
                lRes.IsSuccess = false;
                lRes.ErrorMessage = ex.Message;
            }
            finally {
                lResult = mdl_Publics.GetJSonString(lRes);
            }
            TZServiceTools.mdl_Publics.LogMessage(Server,
                string.Format("HavadesReports({0}) REST Result: {1}", lInformType, lResult),
                "TZServices_Reports_Logs");

            Response.ContentType = "application/json";
            Response.Write(lResult);
        }
        public class inputParams {
            public TamirInfo TamirInfo;
            public long TamirId;
        }
    }
}