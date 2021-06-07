using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace TZServicesCSharp
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RegisterRoutes(System.Web.Routing.RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        private void RegisterRoutes(System.Web.Routing.RouteCollection aRoutes)
        {
            aRoutes.MapPageRoute("BaseInfo",
                "BaseInfo/{BaseInfoType}",
                "~/RestServices/GetBaseInfo.aspx");

            aRoutes.MapPageRoute("NewRequest",
                "NewRequest",
                "~/RestServices/AddNewRequest.aspx");
            //------------<omid>----------
            aRoutes.MapPageRoute("NewTamirRequest",
                "NewTamirRequest",
                "~/RestServices/AddNewTamirRequest.aspx");
            //------------</omid>----------
            aRoutes.MapPageRoute("AddEvent",
                "AddEvent",
                "~/RestServices/AddNewEvent.aspx");
            aRoutes.MapPageRoute("HomaEvent",
                "HomaEvent/{WebMethodName}",
                "~/RestServices/AddNewIllegalEvent.aspx");

            aRoutes.MapPageRoute("AddIllegal",
                "AddIllegal",
                "~/RestServices/AddNewIllegalEvent.aspx");

            aRoutes.MapPageRoute("CallRequestInfo",
                "CallRequestInfo",
                "~/RestServices/GetCallRequestInfo.aspx");

            aRoutes.MapPageRoute("NewPartReady",
                "NewPartsReady",
                "~/RestServices/SetNewPartsReady.aspx");

            aRoutes.MapPageRoute("NewSMS",
                "NewSMS",
                "~/RestServices/AddNewSMSInfo.aspx");
            aRoutes.MapPageRoute("NewCall",
                "NewCall",
                "~/RestServices/AddNewCallInfo.aspx");
            aRoutes.MapPageRoute("NewFax",
                "NewFax",
                "~/RestServices/AddNewFaxInfo.aspx");
            aRoutes.MapPageRoute("NewEmail",
                "NewEmail",
                "~/RestServices/AddNewEmailInfo.aspx");

            aRoutes.MapPageRoute("SaveRequest",
                "SaveRequest",
                "~/RestServices/SaveMobileRequest.aspx");

            aRoutes.MapPageRoute("FollowRequest",
                "FollowRequest",
                "~/RestServices/FollowRequests.aspx");

            aRoutes.MapPageRoute("RequestTypeList",
                "RequestTypeList",
                "~/RestServices/GetRequestTypeList.aspx");

            aRoutes.MapPageRoute("CityList",
                "CityList",
                "~/RestServices/GetCilyList.aspx");

            aRoutes.MapPageRoute("RequestStatusList",
                "RequestStatusList",
                "~/RestServices/GetRequestStatusList.aspx");

            aRoutes.MapPageRoute("GISLogReport",
                "GISLogReport",
                "~/WebForms/GISLogReport.aspx");
            aRoutes.MapPageRoute("ErjaLogReport",
                "ErjaLogReport",
                "~/WebForms/ErjaBiling.aspx");
            aRoutes.MapPageRoute("CCiOutage",
                "TacOutage",
                "~/WebForms/cciGetOutageInfo.aspx");
            aRoutes.MapPageRoute("Version",
                "Version",
                "~/WebForms/Version.aspx");

            aRoutes.MapPageRoute("SetGISData",
                "SetGISData",
                "~/RestServices/AddGISBaseData.aspx");
            aRoutes.MapPageRoute("DelGISData",
                "DelGISData",
                "~/RestServices/DelGISBaseData.aspx");
            aRoutes.MapPageRoute("SetKeyData",
                "SetKeyData",
                "~/RestServices/SetRequestKeyData.aspx");

            aRoutes.MapPageRoute("Informs",
                "Inform/{*InformType}",
                "~/RestServices/HInforms.aspx");

            aRoutes.MapPageRoute("Homa",
                "Homa/{*WebMethodName}",
                "~/RestServices/Homa.aspx");

            aRoutes.MapPageRoute("Ohm",
                "Ohm/{*WebMethodName}",
                "~/RestServices/Ohm.aspx");

            aRoutes.MapPageRoute("Raya",
                "Raya/{*WebMethodName}",
                "~/RestServices/Raya.aspx");

            aRoutes.MapPageRoute("Scada",
				"Scada/{*MethodName}",
                "~/RestServices/Scada.aspx");

            aRoutes.MapPageRoute("ShowGISInfo",
                "ShowGISInfo/{WebMethodName}",
                "~/WebForms/ShowMap.aspx");

            aRoutes.MapPageRoute("Reports",
                "Reports/{ReportType}",
                "~/RestServices/RestReports.aspx");


            aRoutes.MapPageRoute("PostFiles",
                "PostFile",
                "~/RestServices/GetPostedFiles.aspx");

            aRoutes.MapPageRoute("TacMobileApp",
                "TacMobileApp/{RequestType}",
                "~/RestServices/TacMobileApp.aspx");
        }
    }
}
