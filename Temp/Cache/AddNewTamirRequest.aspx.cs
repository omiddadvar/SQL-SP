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
            try {
                lInformType = GetQueryString("RequestType");

                string lMethodNames =
                    "^(maz_setrequesttamir)$";
                if (!Regex.IsMatch(lInformType, lMethodNames, RegexOptions.IgnoreCase))
                    throw new Exception("درخواست شما معتبر نمی باشد");
                lInputData = GetQueryStringOrFirstParameter("param");
                TamirInfo lParams = mdl_Publics.GetClassFromJson<TamirInfo>(lInputData);

                if (lParams == null)
                    throw new Exception("پارامتر ورودی نامعتبر است");
                mdl_Publics.LogMessage(Server,
                    string.Format("HavadesReports({0}) REST input data: {1}", lInformType, lInputData),
                    "TZServices_Reports_Logs");
                Checker ch = new Checker(lParams);
                Danesh_Result lRes = new Danesh_Result();
                DataSet lDs = new DataSet();
                switch (lInformType.ToLower())
                {
                    case "maz_setrequesttamir":
                        //ch.check_AreaID()
                        //    .check_LPPostId();
                        //lRes = lReportService.AzarGH_PostFeederLoad(ch.lCode , ch.lMinCount);
                        //lResult = mdl_Publics.GetJSonString(lRes);
                        break;
                    default:
                        throw new Exception("درخواست شما معتبر نمی باشد");
                }
            }catch(Exception ex){
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
        private class Checker
        {
            public TamirInfo lParams;

            public Checker(TamirInfo aParams)
            {
                this.lParams = aParams;
            }
        }
    }
}