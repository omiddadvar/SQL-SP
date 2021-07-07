using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System.Diagnostics;
using Esri.ArcGISRuntime.WebMap;
using Esri.ArcGISRuntime.Symbology;
//using Esri.ArcGISRuntime.Data;
//using Esri.ArcGISRuntime.Geometry;
//using Esri.ArcGISRuntime.Mapping;
//using Esri.ArcGISRuntime.Symbology;
//using Esri.ArcGISRuntime.UI;
//using Esri.ArcGISRuntime.UI.Controls;
using System.Drawing;
using Esri.ArcGISRuntime.Data;
using static Esri.ArcGISRuntime.Layers.BingLayer;
using Color = System.Windows.Media.Color;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Security;
using System.Collections.ObjectModel;
using System.Net;
using Microsoft.SqlServer.Types;
using System.Reflection;
using System.IO;
using Bargh_GIS.TazarvEvent;
using System.Data;
using System.Windows.Resources;
using System.Text.RegularExpressions;
using Bargh_Common;

namespace Bargh_GIS.wpf
{
    /// <summary>
    /// Interaction logic for TazarvMapUC_Cars.xaml
    /// </summary>
    public partial class TazarvMapUC_Cars : UserControl
    {
        private Dictionary<string, string> attr;
        private PolylineBuilder boatPositions;
        private double speed;
        string MasterName = "", TabletName = "";
        OnCallClass prev, current;
        static Random randomizer = new Random();
        //لایه کلاستر
        
        int LayerCounter = 0;        
        ObservableCollection<Tuple<string, int, bool>> mylayerTOC = new ObservableCollection<Tuple<string, int, bool>>();
        private ArcGISDynamicMapServiceLayer _usaLayer;
        GraphicsOverlay _linesOverlay = new GraphicsOverlay();
        private const int MAX_GRAPHICS = 50;       
        Graphic _editGraphic = null;
        public string m_OnCallIds = "";
        public OnCallClass[] m_OnCallClass;
        public DataTable mDt = new DataTable();
        public bool mIsReadyPolygan = false;
        public bool mIsNoCheckList = false;

        /// <summary>
        /// سازنده پیش فرض
        /// </summary>
        /// <param name="setting">اطلاعات پیکربندی</param>
        /// <param name="enent"> windows  form   رویداد های</param>
        public TazarvMapUC_Cars(Bargh_GIS.TazarvEvent.MapSetting setting, CallWpfFuctions enent,bool IsCarMode, int aAreaId )
        {
            //double x1 = double.Parse(setting.EnvelopExtent.Split('*')[0]);
            //double y1 = double.Parse(setting.EnvelopExtent.Split('*')[1]);
            //double x2 = double.Parse(setting.EnvelopExtent.Split('*')[2]);
            //double y2 = double.Parse(setting.EnvelopExtent.Split('*')[3]);
            m_OnCallClass = new OnCallClass[1000];
            try
            {
                Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ClientId = "1bL7YrsjxvPxQYzQ";
                Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.Initialize();
                ArcGISRuntimeEnvironment.License.SetLicense("runtimeadvanced,1000,rud000228325,none,3M10F7PZB0YH463EM164");
            }
            catch (Exception e)
            {

            }

            InitializeComponent();
            // Disable the label fade animation
            mapview.Labeling.IsAnimationEnabled = false;


            //روش  دوم  تنظیم نقطه اولیه
            //Envelope initialLocation = new Envelope(
            //     5655978.829570528,4227844.161013588,5778278.074826779, 4287923.665245721,
            //    SpatialReferences.WebMercator);

            //  Envelope initialLocation = new Envelope(x1, y1, x2, y2, SpatialReferences.WebMercator);
            // mapview.Map.InitialViewpoint = new Viewpoint(initialLocation);

            //روش اول تنظیم نقطه اولیه

            double lBaseGISX = 51.338024;
            double lBaseGISY = 35.699730;
            int lBaseGISZoomLevel = 1000000;

            int lAreaId = Bargh_Common.CommonVariables.WorkingAreaId;
            if (aAreaId > -1)
                lAreaId = aAreaId;

            Classes.CDatabase db = new Classes.CDatabase();
            DataTable dt = db.ExecSQL("select ISNULL(BaseGISX,-1) AS BaseGISX, ISNULL(BaseGISY,-1) AS BaseGISY, ISNULL(BaseGISZoomLevel,-1) AS BaseGISZoomLevel from Tbl_AreaInfo where AreaId = " + lAreaId.ToString());
            if (dt.Rows.Count>0)
            {
                DataRow lRow = dt.Rows[0];
                if (double.Parse(lRow["BaseGISX"].ToString()) > -1)
                    lBaseGISX = (double)lRow["BaseGISX"];

                if (double.Parse(lRow["BaseGISY"].ToString()) > -1)
                    lBaseGISY = (double)lRow["BaseGISY"];

                if (Int32.Parse(lRow["BaseGISZoomLevel"].ToString()) > -1)
                    lBaseGISZoomLevel = (int)lRow["BaseGISZoomLevel"];
            }

            var pointInitial = new MapPoint(lBaseGISX, lBaseGISY, SpatialReferences.Wgs84);
            mapview.Map.InitialViewpoint = new Viewpoint(pointInitial, lBaseGISZoomLevel);


            OpenStreetMapLayer osm = new OpenStreetMapLayer();
            osm.IsVisible = false;
            osm.ID = "OSM";
            // BingLayer bingLayer = new BingLayer("AjmZ9JR1nuGAJRBLnsGS4kpbwfNBCIx8PLCfYU9OsHhhRMGttqU8b9VSBSyu77fX");
            BingLayer bingLayer = new BingLayer(setting.BingKey);
            


            bingLayer.MapStyle = LayerType.AerialWithLabels;
            bingLayer.ID = "Bing";
            bingLayer.IsVisible = false;
            mapview.LayerLoaded += Mapview_LayerLoaded;
            mapview.Map.Layers.Add(osm);
            mapview.Map.Layers.Add(bingLayer);
            //var imgUri = new Uri("http://services.arcgisonline.com/arcgis/rest/services/World_Street_Map/MapServer");


            string lGISArcURL = "";
            lGISArcURL = Convert.ToString(Bargh_Common.mdlHashemi.ReadConfig("GISArcUrl", ""));

            if (lGISArcURL == "")
            {
                Bargh_Common.MsgBoxFarsi.MsgBoxF("URL جهت نمايش نقشه GIS تعريف نشده است","خطاي نمايش نقشه", System.Windows.Forms.MessageBoxButtons.OK, Bargh_Common.MsgBoxIcon.MsgIcon_Error, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                return;
            }

            var imgUri = new Uri(lGISArcURL + "arcgis/rest/services/World_Street_Map/MapServer");
            var agsImageLayer = new Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer(imgUri);
            agsImageLayer.ID = "World_Street_Map";
            agsImageLayer.IsVisible = true;
            mapview.Map.Layers.Add(agsImageLayer);
            imgUri = new Uri(lGISArcURL + "ArcGIS/rest/services/World_Topo_Map/MapServer");
            agsImageLayer = new Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer(imgUri);
            agsImageLayer.ID = "World_Topo_Map";
            agsImageLayer.IsVisible = false;
            mapview.Map.Layers.Add(agsImageLayer);
            imgUri = new Uri(lGISArcURL + "ArcGIS/rest/services/World_Imagery/MapServer");
            agsImageLayer = new Esri.ArcGISRuntime.Layers.ArcGISTiledMapServiceLayer(imgUri);
            agsImageLayer.ID = "World_Imagery";
            agsImageLayer.IsVisible = false;
            mapview.Map.Layers.Add(agsImageLayer);

            if (IsCarMode)
            {
                BorderOncall.Visibility = Visibility.Visible;
                BttnFindEyb.Visibility = Visibility.Hidden;
                pnlBazdidInfo.Visibility = Visibility.Hidden;
                pnlInfo.Children.Remove(BttnFindEyb);
            }
            else
            {
                pnlInfo.Children.Remove(BttnInformation);
                BorderOncall.Visibility = Visibility.Hidden;
                pnlBazdidInfo.Visibility = Visibility.Visible;
            }
            //افزودن لایه  گرافیکی
            mapview.GraphicsOverlays.Add(_linesOverlay);
            //اعمال  تنظیمات
            if (setting.Provider == "Arc")
            {
                if (setting.IsSecure == true)
                {
                    //  اگر  لایه  امنیت داشت
                    LoadSecureLayer(setting);
                }
                else
                {
                    //  اگر  باز بود
                    if (!string.IsNullOrEmpty(setting.WmsUrl))
                        LoadUnScurelayer(setting);
                }
            }
            if (setting.Provider == "geoserver")
                LoadGeoServerlayer(setting);
            //نمایش ماشین ها
            enent.ContractNumber_3Event += Enent_ContractNumber_3Event;
            //حذف ماشین ها
            enent.ContractNumber_4Event += Enent_ContractNumber_4Event;

            //فعال سازی  پاپ آپ
            enent.ContractNumber_5Event += Enent_ContractNumber_5Event;

        }
        
        //نمایش ماشین ها
        public void Enent_ContractNumber_3Event(object sender, TazarvArg e)
        {
           /* DataAccess da = new DataAccess();

            var allcars = da.GetAllCarIds();

            foreach (var item in allcars)
            {

                Dictionary<string, string> attr = new Dictionary<string, string>();

                attr.Add("id", "CarId_" + item.CarId.ToString());
                addCarLog(item.gpsY, item.gpsX, attr);

            }*/
        }
        
        private int GetAngle(double X1, double Y1, double X2, double Y2)
        {
            if (Y2 == Y1)
            {
                if (X1 > X2)
                    return 90; //System.Math.PI;
                else
                    return -90;


            }
            if (X2 == X1)
            {
                return (Y2 > Y1) ? -180 : 0; //(Y2 > Y1) ? System.Math.PI / 2 : 1.5 * Math.PI;
            }
            var tangent = (X2 - X1) / (Y2 - Y1);
            var ang = System.Math.Atan(tangent);
            if (Y2 - Y1 < 0)
                ang -= Math.PI;
            int Angle = Convert.ToInt32(Math.Round(ang * 180 / Math.PI / 45, 0) * 45);
            //if (Angle != 0 && Angle != 45 && Angle != 90 && Angle != -225 && Angle != -180 && Angle != -135 && Angle != -90 && Angle != -45)
            //    Angle = 90;
            if (Angle==-270)
                Angle = 90;


            return Angle;
        }
        private async void addCarLogByArrow(double x, double y, Dictionary<string, string> attributes, int Arrow , bool isFindJob = false)
        {
            string fileType = isFindJob ? "Nav" : "Arrow";
            string fileName = "Bargh_GIS.Images."+ fileType + Arrow.ToString() + ".png";
            try
            {
                // Create new symbol using asynchronous factory method from uri.
                PictureMarkerSymbol campsiteSymbolz = new PictureMarkerSymbol()
                {
                    Width = 20,
                    Height = 20
                };

                //برای  تغیر  مکان  عکس یا آیکون شما
                // campsiteSymbolz.XOffset = 5;
                System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream(fileName);
                if (st != null)
                    await campsiteSymbolz.SetSourceAsync(st);
                
                MapPoint campsitePointz = new MapPoint(x, y, SpatialReferences.Wgs84);

                Graphic campsiteGraphicz = new Graphic(campsitePointz, campsiteSymbolz);

                foreach (var items in attributes)
                {

                    campsiteGraphicz.Attributes.Add(items.Key, items.Value);
                }
                campsiteGraphicz.ZIndex = 1000;
                _linesOverlay.Graphics.Add(campsiteGraphicz);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            } 
        }
        private async void addRequest(double x, double y, Dictionary<string, string> attributes,int EndjobStateId,int OutageTypeId,int ZIndex)
        {

            try
            {
                // Create new symbol using asynchronous factory method from uri.
                PictureMarkerSymbol campsiteSymbolz = new PictureMarkerSymbol()
                {
                    Width = 40,
                    Height = 40
                };

                //برای  تغیر  مکان  عکس یا آیکون شما
                // campsiteSymbolz.XOffset = 5;
                string ImageName = EndjobStateId.ToString() + "Outage" + OutageTypeId.ToString() + ".png";
                if (EndjobStateId==1)
                    ImageName = EndjobStateId.ToString() + ".png";

                System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream("Bargh_GIS.Images.EndJobState" + ImageName);
                //System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream("Bargh_GIS.Images.Arrow0.png");
                if (st != null)
                    await campsiteSymbolz.SetSourceAsync(st);

                MapPoint campsitePointz = new MapPoint(x, y, SpatialReferences.Wgs84);

                Graphic campsiteGraphicz = new Graphic(campsitePointz, campsiteSymbolz);

                foreach (var items in attributes)
                {

                    campsiteGraphicz.Attributes.Add(items.Key, items.Value);
                }
                campsiteGraphicz.ZIndex = 1000+ZIndex;
                _linesOverlay.Graphics.Add(campsiteGraphicz);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }
        private async void addBazdidAddress(double x, double y, Dictionary<string, string> attributes,  int ZIndex, int aImageTypeId)
        {

            try
            {
                // Create new symbol using asynchronous factory method from uri.
                PictureMarkerSymbol campsiteSymbolz = new PictureMarkerSymbol()
                {
                    Width = 40,
                    Height = 40
                };

                //برای  تغیر  مکان  عکس یا آیکون شما
                // campsiteSymbolz.XOffset = 5;

                string lImage = "Bargh_GIS.Images.pin.png";
                if (aImageTypeId == 1)
                    lImage = "Bargh_GIS.Images.pin_mvfeederEmbedded.png";
                else if (aImageTypeId == 2)
                    lImage = "Bargh_GIS.Images.pin_lppostEmbedded.png";
                else if (aImageTypeId == 3)
                    lImage = "Bargh_GIS.Images.pin_lvfeederEmbedded.png";
                else if (aImageTypeId == 11)
                    lImage = "Bargh_GIS.Images.pin_mvfeederwork.png";
                else if (aImageTypeId == 21)
                    lImage = "Bargh_GIS.Images.pin_lppostwork.png";
                else if (aImageTypeId == 31)
                    lImage = "Bargh_GIS.Images.pin_lvfeederwork.png";
                else if (aImageTypeId == 0)
                    lImage = "Bargh_GIS.Images.pin_noeybEmbedded.png";


                System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream(lImage);

                //System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream("Bargh_GIS.Images.Arrow0.png");
                if (st != null)
                    await campsiteSymbolz.SetSourceAsync(st);

                MapPoint campsitePointz = new MapPoint(x, y, SpatialReferences.Wgs84);

                Graphic campsiteGraphicz = new Graphic(campsitePointz, campsiteSymbolz);

                foreach (var items in attributes)
                {

                    campsiteGraphicz.Attributes.Add(items.Key, items.Value);
                }
                campsiteGraphicz.ZIndex = 1000 + ZIndex;
                _linesOverlay.Graphics.Add(campsiteGraphicz);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

        void RefreshZoom(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count <= 0)
                    return;
                double x;
                double y;
                double MinX = 0;
                double MinY = 0;
                double MaxX = 0;
                double MaxY = 0;
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    x = Convert.ToDouble(row["gpsX"]);
                    y = Convert.ToDouble(row["gpsY"]);
                    if (i == 0)
                    {
                        MinX = x;
                        MinY = y;
                        MaxX = x;
                        MaxY = y;
                    }
                    else
                    {
                        if (x < MinX)
                            MinX = x;
                        if (y < MinY)
                            MinY = y;
                        if (x > MaxX)
                            MaxX = x;
                        if (y > MaxY)
                            MaxY = y;
                    }
                    i = i + 1;
                }
                x = MinX - MaxX;
                y = MinY - MaxY;

                var pointInitial = new MapPoint( (MinX + MaxX) / 2, (MinY + MaxY) / 2, SpatialReferences.Wgs84);
                double l = Math.Sqrt(x * x + y * y) * 100000.0;
                if (l == 0) return;
                mapview.Map.InitialViewpoint = new Viewpoint(pointInitial, l * 10);
                mapview.SetView(new Viewpoint(pointInitial, l * 15));
                mapview.SetViewAsync(new Viewpoint(pointInitial, l * 15));

            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }

        public void ShowTrace(long OnCallId, long aRequestId)
        {
            try
            {
                bool IsNewMode = false;
                int lOnCallClassIndex = -1;
                for (int i = 0; i < 1000; i++)
                {
                    if (m_OnCallClass[i] == null)
                    {
                        m_OnCallClass[i] = new OnCallClass();
                        m_OnCallClass[i].OnCallId = OnCallId;
                        lOnCallClassIndex = i;                        
                        IsNewMode = true;
                        break;
                    }else
                    if (m_OnCallClass[i].OnCallId==OnCallId)
                    {
                        lOnCallClassIndex = i;
                        break;
                    }
                }
                Classes.CDatabase db = new Classes.CDatabase();
                DataTable dt = db.ExecSQL("exec [Homa].[GetTrace] " + OnCallId.ToString() + "," + +m_OnCallClass[lOnCallClassIndex].LastTraceId + "," + aRequestId.ToString());

                if (m_OnCallIds.IndexOf(OnCallId.ToString()+",")<0)
                    m_OnCallIds = m_OnCallIds + OnCallId.ToString() + ",";

                if (dt.Rows.Count <= 1)
                    return;

                if (aRequestId > 0)
                    RefreshZoom(dt);

                clearGrapics("CarId_" + OnCallId.ToString());
                //clearGrapics("TraceId_" + OnCallId.ToString());

                this.boatPositions = new PolylineBuilder(SpatialReferences.Wgs84);

                int Angle = 0;
                string TabletName="";
                string MasterName = "";
                OnCallClass PrevOnCall = m_OnCallClass[lOnCallClassIndex];
            
                foreach (DataRow row in dt.Rows)
                {
                    boatPositions.AddPoint(new MapPoint(Convert.ToDouble(row["gpsX"]), Convert.ToDouble(row["gpsY"])));
                    TabletName = row["TabletName"].ToString();
                    MasterName = row["MasterName"].ToString();
                    if (PrevOnCall.LastTraceId > 0)
                    {                    
                        this.attr = new Dictionary<string, string>();
                        Angle = GetAngle(PrevOnCall.GpsX, PrevOnCall.GpsY
                            , Convert.ToDouble(row["gpsX"]), Convert.ToDouble(row["gpsY"]));
                        attr.Add("id", "TraceId_" + OnCallId.ToString()+"_" + PrevOnCall.LastTraceId.ToString());
                        attr.Add("DT", PrevOnCall.TraceDatePersian +" " + PrevOnCall.TraceTime);
                        long s = ((TimeSpan)(Convert.ToDateTime(row["TraceDT"]) - PrevOnCall.TraceDT)).Seconds;
                        if (s >0)
                        {
                            double x = Convert.ToDouble(PrevOnCall.GpsX) - Convert.ToDouble(row["gpsX"]);

                            double y= Convert.ToDouble(PrevOnCall.GpsY) - Convert.ToDouble(row["gpsY"]);
                            double l = Math.Sqrt(x * x + y * y)* 100000.0;
                            double speed = 3600 * l / (s * 1000);
                            TabletName = TabletName + " - " + speed + "kmh";
                        }
                        attr.Add("TabletName", TabletName);
                        attr.Add("MasterName", MasterName);
                        addCarLogByArrow(PrevOnCall.GpsX, PrevOnCall.GpsY, attr, Angle);                    
                    }
                    PrevOnCall.GpsY = Convert.ToDouble(row["gpsY"]);
                    PrevOnCall.GpsX = Convert.ToDouble(row["gpsX"]);
                    PrevOnCall.TraceDT = Convert.ToDateTime(row["TraceDT"]);
                    PrevOnCall.LastTraceId = Convert.ToInt64(row["TraceId"]);
                    PrevOnCall.TraceDatePersian = (string)row["TraceDatePersian"];
                    PrevOnCall.TraceTime = (string)row["TraceTime"];

                }
                if (PrevOnCall.LastTraceId > 0)
                {
                    Dictionary<string,string> attr = new Dictionary<string, string>();
                    attr.Add("id", "CarId_" + OnCallId.ToString() + "_" + PrevOnCall.LastTraceId.ToString());
                    attr.Add("DT", PrevOnCall.TraceDatePersian + " " + PrevOnCall.TraceTime);
                    attr.Add("TabletName", TabletName);
                    attr.Add("MasterName", MasterName.ToString());
                    addCar(PrevOnCall.GpsX, PrevOnCall.GpsY, attr, Angle);
                    m_OnCallClass[lOnCallClassIndex] = PrevOnCall;
                }
                                
                // get the polyline from the builder
                var boatRoute = boatPositions.ToGeometry();
                //   var lineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Dash, System.Drawing.Color.FromArgb(255, 128, 0, 128), 1.0);
                // new SampleSymbol(new PictureFillSymbol() { Outline = blackOutlineSymbol, Width = 24, Height = 24 }, "pack://application:,,,/ArcGISRuntimeSamplesDesktop;component/Assets/x-24x24.png"),
                var lineSymbol = new SimpleLineSymbol();
                lineSymbol.Style = SimpleLineStyle.Solid;
                lineSymbol.Color = System.Windows.Media.Color.FromArgb(173, 255, 47, 128);
                // lineSymbol.Color = System.Windows.Media.Color.FromArgb(255, 128, 0, 128);//بنفش
                lineSymbol.Width = 4;

                var boatTripGraphic = new Graphic(boatRoute, lineSymbol);
                boatTripGraphic.Attributes.Add("id", "OnCallId_" + OnCallId.ToString());
                boatTripGraphic.Attributes.Add("name", "lineAA");
                boatTripGraphic.Attributes.Add("name2", "pointAAAA");
                //boatTripGraphic.
                //oatTripGraphic.
                _linesOverlay.Graphics.Add(boatTripGraphic);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }
        //Show Trace For Trace-History
        public void ShowTrace(DataTable dt) {
            //--------------Omid---------------            
            DataRow row;
            if (dt.Rows.Count == 0) return;
            initializeTrace();
            prev.IsFindJob = Convert.ToBoolean(dt.Rows[0]["IsFindJob"]);
            for(int i = 0 ; i < dt.Rows.Count; i++) {
                row = dt.Rows[i];
                FillOnCallObject(ref current , row);
                if (prev.IsFindJob != current.IsFindJob || i == 0) {
                    if (i > 0) DrawLine();
                    this.boatPositions = new PolylineBuilder(SpatialReferences.Wgs84);
                    //else setAttributes(aIsFirst: true);
                }
                boatPositions.AddPoint(new MapPoint(current.GpsX , current.GpsY));
                setAttributes();
                CalculateSpeed(prev, current);
                FillOnCallObject(ref prev, row);
            }
            DrawLine(isFindChanged: false);
            setAttributes(true);
        }
        //-----Helper For ShowTrace(DataTable)
        private void setAttributes(bool aIsCar = false) {
            int angle = GetAngle(prev.GpsX, prev.GpsY, current.GpsX, current.GpsY);
            this.attr = new Dictionary<string, string>();
            attr.Add("id", "TraceId_" + current.OnCallId + "_" + current.TraceId);
            attr.Add("DT", current.TraceDatePersian + " " + current.TraceTime);
            attr.Add("TabletName", TabletName + " - " + (int)speed + " kmh");
            attr.Add("MasterName", MasterName);
            CalculateSpeed(prev, current);
            if(aIsCar)
                addCar(current.GpsX, current.GpsY, attr, angle);
            else
                addCarLogByArrow(current.GpsX, current.GpsY, attr, angle , current.IsFindJob);
        }
        //-----Helper For ShowTrace(DataTable)
        private void initializeTrace() {
            this.prev = new OnCallClass();
            this.current = new OnCallClass();
            Classes.CDatabase db = new Classes.CDatabase();
            DataTable dt = db.ExecSQL("exec [Homa].[spOmidInfo]");
            if (dt.Rows.Count > 0) {
                TabletName = dt.Rows[0]["TabletName"].ToString();
                MasterName = dt.Rows[0]["MasterName"].ToString();
            }
        }
        //-----Helper For ShowTrace(DataTable)
        private void FillOnCallObject(ref OnCallClass obj , DataRow row) {
            obj.IsFindJob = Convert.ToBoolean(row["IsFindJob"]);
            obj.OnCallId = (long) Convert.ToUInt64(row["OnCallId"]);
            obj.TraceDatePersian = Convert.ToString(row["TraceDatePersian"]);
            obj.TraceTime = Convert.ToString(row["TraceTime"]);
            obj.TraceDT = Convert.ToDateTime(row["TraceDT"]);
            obj.GpsX = Convert.ToDouble(row["GpsX"]);
            obj.GpsY = Convert.ToDouble(row["GpsY"]);
            obj.TraceId = (long) Convert.ToUInt64(row["TraceId"]);
        }
        //-----Helper For ShowTrace
        private void CalculateSpeed(OnCallClass aOld , OnCallClass aNew) {
            this.speed = 0;
            long sec = ((TimeSpan)(aNew.TraceDT - aOld.TraceDT)).Seconds;
            if (sec > 0 && aOld.TraceTime.Length > 0)
            {
                double x = aNew.GpsX - aOld.GpsX;
                double y = aNew.GpsY - aOld.GpsY;
                double l = Math.Sqrt(x * x + y * y) * 100000.0;
                speed = 3600 * l / (sec * 1000);
            }
        }
        //-----Helper For ShowTrace(DataTable)
        private void DrawLine(bool isFindChanged = true) {
            if(isFindChanged)
                boatPositions.AddPoint(new MapPoint(current.GpsX, current.GpsY));
            var boatRoute = boatPositions.ToGeometry();
            var lineSymbol = new SimpleLineSymbol();
            lineSymbol.Style = SimpleLineStyle.Solid;
            //lineSymbol.Color = (isFindChanged ? prev.IsFindJob : current.IsFindJob)?
            lineSymbol.Color = prev.IsFindJob?
                 Color.FromArgb(173, 0, 255, 0) //Green
                : Color.FromArgb(173, 0, 0, 255); //Blue
            var boatTripGraphic = new Graphic(boatRoute, lineSymbol);
            boatTripGraphic.Attributes.Add("id", "OnCallId_" + prev.OnCallId);
            boatTripGraphic.Attributes.Add("time",prev.TraceTime);
            _linesOverlay.Graphics.Add(boatTripGraphic);
        }

        public void RefreshTrace(long aRequestId )
        {
            try
            {
                //long AreaId = (int)cboArea.SelectedValue;
                if (m_OnCallIds.Length>0)
                {
                    string[] lOnCallIds = m_OnCallIds.Split(',');
                    for (int i=0; i < lOnCallIds.Length;i++)
                        if (lOnCallIds[i].Length>0)
                    {
                            ShowTrace(Convert.ToInt64(lOnCallIds[i]), aRequestId);
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }
        public void RefreshRequest(long aRequestId, string aAreaIDs )
        {
            try
            {
                Classes.CDatabase db = new Classes.CDatabase();
                DataTable dt = db.ExecSQL("exec [Homa].[spGISGetRequest] " + aRequestId.ToString() + ",'" + aAreaIDs + "'");
                clearGrapics("RequestId_");
                int ZIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    ZIndex = ZIndex + 1;
                    string RequestNumber = row["RequestNumber"].ToString();
                    string MasterName = row["MasterName"].ToString();
                    string SubscriberName= row["SubscriberName"].ToString();
                    string RequestId= row["RequestId"].ToString();
                    
                    Dictionary<string, string> attr = new Dictionary<string, string>();
                       
                    attr.Add("id", "RequestId_" + RequestId);
                    attr.Add("MasterName", MasterName);
                    attr.Add("SubscriberName", SubscriberName);
                    attr.Add("RequestNumber", RequestNumber);
                    int EndjobStateId = Convert.ToInt32(row["EndjobStateId"]);
                    int OutageTypeId = Convert.ToInt32(row["OutageTypeId"]);

                    
                    addRequest(Convert.ToDouble(row["GpsX"]), Convert.ToDouble(row["GpsY"]), attr, EndjobStateId,OutageTypeId, ZIndex);                    
                }                
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }
        public void ShowBazdid(DataTable dt)
        {
            try
            {                
                clearGrapics("BazdidResultAddressId_");
                int ZIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    ZIndex = ZIndex + 1;
                    
                    string Address = row["Address"].ToString();
                    string BazdidResultAddressId= row["BazdidResultAddressId"].ToString();

                    Dictionary<string, string> attr = new Dictionary<string, string>();

                    attr.Add("id", "BazdidResultAddressId_" + BazdidResultAddressId);
                    attr.Add("Address", Address);



                    addBazdidAddress(Convert.ToDouble(row["GpsX"]), Convert.ToDouble(row["GpsY"]), attr,  ZIndex, Convert.ToInt32(row["BazdidTypeId"]));
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
        }


        //حذف ماشین ها
        private void Enent_ContractNumber_4Event(object sender, TazarvArg e)
        {
            clearGrapics("CarId");

        }

        //popup
        //فعال سازی  پاپ آپ
        private void Enent_ContractNumber_5Event(object sender, TazarvArg e)
        {
            mapview.MapViewTapped += MyMapView_MapViewTapped2;
        }

        //بعد از هر گونه زوم باید  نقاط کلاستر شده بروز شوند
        private void MyMapView_MouseMove(object sender, MouseEventArgs e)
        {
            if (mapview.GetCurrentViewpoint(ViewpointType.BoundingGeometry) == null)
                return;

            System.Windows.Point screenPoint = e.GetPosition(mapview);
            

            MapPoint mapPoint = mapview.ScreenToLocation(screenPoint);

            if (mapPoint == null)
            {
                return;
            }
            if (mapview.WrapAround)
                mapPoint = GeometryEngine.NormalizeCentralMeridian(mapPoint) as MapPoint;
            
            MapCoordsTextBlock.Text =ConvertCoordinate.ToDecimalDegrees(mapPoint, 4);
        }
        //بعد از هر گونه زوم باید  نقاط کلاستر شده بروز شوند
        private void mapview_NavigationCompleted(object sender, EventArgs e)
        {



            //clusterer.ClusterResolution = mapview.UnitsPerPixel;


            //clusterer.UpdateBounds(mapview.Extent);


        }
        public void LoadOnCall(DataTable dt)
        {
            ObservableCollection<Tuple<string, long, bool>> lOncalls = new ObservableCollection<Tuple<string, long, bool>>();
            if (m_OnCallIds != "")
            {
                string[] Ids = m_OnCallIds.Split(',');
                foreach (string aId in Ids)
                    if (Regex.IsMatch(aId, @"^[\d]+$"))
                    {
                        bool IsFind = false;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            long OnCallId = Convert.ToInt64(dt.Rows[i]["OnCallId"]);
                            if (OnCallId == Convert.ToInt64(aId))
                            {
                                IsFind = true;
                                break;
                            }
                        }
                        if (!IsFind)
                            ChangeOnCallCheckBox(Convert.ToInt64(aId), false);

                    }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                long OnCallId = Convert.ToInt64(dt.Rows[i]["OnCallId"]);
                string MasterName = Convert.ToString(dt.Rows[i]["MasterName"]);
                if (m_OnCallIds.IndexOf(OnCallId.ToString() + ",") < 0)
                    m_OnCallIds = m_OnCallIds + OnCallId.ToString() + ",";
                lOncalls.Add(new Tuple<string, long, bool>(MasterName, OnCallId, true));
            }
            visibleOncall.ItemsSource = lOncalls;
        }

        //خاموش  و روشن کردن لایه ها
        private void LayerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;
            if (checkBox != null)
            {
                int layerIndex = ((Tuple<string, int, bool>)checkBox.Tag).Item2;
                string Layername = ((Tuple<string, int, bool>)checkBox.Tag).Item1;

                for (int i=0; i < mapview.Map.Layers.Count;i++)
                    if (mapview.Map.Layers[i].ID== Layername)
                {
                        (mapview.Map.Layers[i]).IsVisible = !(mapview.Map.Layers[i]).IsVisible;
                }
                
            }
        }

        private void ChangeOnCallCheckBox(long OnCallId, bool? IsChecked)
        {
            if (IsChecked == true)
                if (m_OnCallIds.IndexOf(OnCallId.ToString() + ",") < 0)
                {
                    //ShowTrace(OnCallId);
                    m_OnCallIds = m_OnCallIds + OnCallId.ToString() + ",";
                }
            if (IsChecked == false)
                if (m_OnCallIds.IndexOf(OnCallId.ToString() + ",") >= 0)
                {
                    m_OnCallIds = m_OnCallIds.Replace(OnCallId.ToString() + ",", "");
                    clearGrapics("OnCallId_" + OnCallId.ToString());
                    clearGrapics("TraceId_" + OnCallId.ToString());
                    clearGrapics("CarId_" + OnCallId.ToString());
                    for (int i = 0; i < 1000; i++)
                    {
                        if (m_OnCallClass[i] == null)
                        {
                            break;
                        }
                        else
                        if (m_OnCallClass[i].OnCallId == OnCallId)
                        {
                            m_OnCallClass[i].LastTraceId = 0;
                        }
                    }
                }
        }
        private void OnCallCheckBox_Click(object sender, RoutedEventArgs e)
        {

            /*for(int i=0; i < visibleOncall.Items.Count;i++)
            {
                Tuple<string, long, bool> obj = (Tuple<string, long, bool>)visibleOncall.Items[i];
                object obj2 = visibleOncall.ItemContainerGenerator.Items[i];
            }*/
            var checkBox = e.OriginalSource as CheckBox;
            if (checkBox != null)
            {
                long OnCallId = ((Tuple<string, long, bool>)checkBox.Tag).Item2;
                string Layername = ((Tuple<string, long, bool>)checkBox.Tag).Item1;

                ChangeOnCallCheckBox(OnCallId, checkBox.IsChecked);
                /*
                for (int i = 0; i < mapview.Map.Layers.Count; i++)
                    if (mapview.Map.Layers[i].ID == Layername)
                    {
                        (mapview.Map.Layers[i]).IsVisible = !(mapview.Map.Layers[i]).IsVisible;
                    }*/
            }
        }
        //بارگزاری لایه  از  ژئوسرور
        private void LoadGeoServerlayer(Bargh_GIS.TazarvEvent.MapSetting setting)
        {
            WmsLayer wmsLayer = new WmsLayer(new Uri(setting.WmsUrl));
            var loadlayer = new List<string>();
            var ListofLayerName = setting.WmsLayerNames.Split('*');
            foreach (var items in ListofLayerName)
            {

                loadlayer.Add(items);

            }
            wmsLayer.Layers = loadlayer;
            mapview.Map.Layers.Add(wmsLayer);

        }

        //بعد از بالا  آمدن   نقشه  لیست لایه ها تنظیم میشوند
        private void Mapview_LayerLoaded(object sender, LayerLoadedEventArgs e)
        {
            if (e.LoadError != null)
            {
                if (e.Layer.ID.ToLower() == "bing")
                    return;
                string lStr = "خطا در بارگذاري لايه " + e.Layer.ID + " " + Environment.NewLine + e.LoadError.Message;
                CommonFunctions.ShowError(lStr);
                return;
            }

            

            //int layerIndex = ((Tuple<string, int, bool>)checkBox.Tag).Item2;
            mylayerTOC.Add(new Tuple<string, int, bool>(e.Layer.ID, LayerCounter, e.Layer.IsVisible));
            LayerCounter = LayerCounter + 1;
            //  visibleLayers.ItemsSource.
            //  visibleLayers.ItemsSource = _usaLayer.ServiceInfo.Layers
            // .Select((info, idx) => new Tuple<string, int, bool>(info.Name, idx, info.DefaultVisibility));

            visibleLayers.ItemsSource = mylayerTOC;


        }

        // بارگزاری لایه امن  از  آرک جی آی اس
        private async void LoadSecureLayer(Bargh_GIS.TazarvEvent.MapSetting setting)
        {
            try
            {
                var GetTokenurl = setting.TokenUrl + "?request=getToken&username=" + setting.UserName + "&password=" + setting.Password + "&expiration=30";
                System.Net.WebRequest request = System.Net.WebRequest.Create(GetTokenurl);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader readStream = new System.IO.StreamReader(responseStream);
                var theToken = await readStream.ReadToEndAsync();
                var layer = new ArcGISDynamicMapServiceLayer(new Uri(setting.WmsUrl));
                layer.ID = "network";
                layer.Token = theToken;
                mapview.Map.Layers.Add(layer);
                await mapview.SetViewAsync(layer.FullExtent);

            }
            catch (Exception ex)
            {
                lblAlarm.Visibility = Visibility.Visible;
                lblGISError.Visibility = Visibility.Visible;
                lblGISError.Content = ex.Message;
            }

        }

        // بارگزاری لایه   غیر امن  از  آرک جی آی اس
        private async void LoadUnScurelayer(Bargh_GIS.TazarvEvent.MapSetting setting)
        {
            var layer = new ArcGISDynamicMapServiceLayer(new Uri(setting.WmsUrl));
            layer.ID = "network";
            mapview.Map.Layers.Add(layer);
            await mapview.SetViewAsync(layer.FullExtent);

        }

        //استفاده شده   برای  کلیک  روی  نقاط
        private async Task<IEnumerable<Graphic>> FindIntersectingGraphicsAsync()
        {
            // var mapRect = await mapview.Editor.RequestShapeAsync(DrawShape.Envelope) as Envelope;
            var mapRect = await mapview.Editor.RequestShapeAsync(DrawShape.Envelope) as Envelope;
            Rect winRect = new Rect(
                mapview.LocationToScreen(new MapPoint(mapRect.XMin, mapRect.YMax, mapview.SpatialReference)),
                mapview.LocationToScreen(new MapPoint(mapRect.XMax, mapRect.YMin, mapview.SpatialReference)));
            // _linesOverlay.h
            return await _linesOverlay.HitTestAsync(mapview, winRect, MAX_GRAPHICS);
        }
        
        //پیدا کردن نقطه کلیک شده
        private async void StartSearc_click(object sender, RoutedEventArgs e)
        {
            try
            {
                var graphics = await FindIntersectingGraphicsAsync();
                foreach (var graphic in graphics)
                {
                    graphic.IsSelected = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private async void StartSearcpoint(object sender, RoutedEventArgs e)
        {
            try
            {
                var graphics = await FindIntersectingGraphicsAsync();
                foreach (var graphic in graphics)
                {
                    graphic.IsSelected = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }

        public void trace(object sender, RoutedEventArgs e)
        {

            Esri.ArcGISRuntime.Geometry.Polyline trail = null;


            int trailGraphicId;
            if (trail == null)
            {
                //https://community.esri.com/thread/74269
                //trail = new Esri.ArcGISRuntime.Geometry.Polyline();
                //trail.startPath(prevLocationOnMap);
                //PolylineBuilder builder = new PolylineBuilder(polyline); // Create new builder from existing polyline  
                //builder.AddPoint(new MapPoint(x3, y3, polyline.SpatialReference)); // how to add new point to end of the polyline  
                //builder.Parts[0][1] = new MapPoint(x3, y3, polyline.SpatialReference); // how to change one specific point in the polyline   
                //graphic.Geometry = builder.ToGeometry();
                //  WebMercator mercator = new WebMercator();
            }
            else
            {

            }


        

        }

        private async void MyMapView_MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            var resultGeometry = _editGraphic == null ? null : _editGraphic.Geometry;
            if (mapview.Editor.IsActive)
                return;

            var graphic = await _linesOverlay.HitTestAsync(mapview, e.Position);
            if (graphic != null)
            {
                //Clear previous selection
                foreach (GraphicsOverlay gOLay in mapview.GraphicsOverlays)
                {
                    gOLay.ClearSelection();
                }

                //Cancel editing if started
                if (mapview.Editor.Cancel.CanExecute(null))
                    mapview.Editor.Cancel.Execute(null);

                _editGraphic = graphic;
                _editGraphic.IsSelected = true;

                var r = await mapview.Editor.EditGeometryAsync(_editGraphic.Geometry);
                resultGeometry = r ?? resultGeometry;
                SpatialReference sp = new SpatialReference(4326);
                GeometryEngine.Project(r, sp);
                _editGraphic.Geometry = resultGeometry;
                _editGraphic.IsSelected = false;


                var ddd = GeometryEngine.Project(r, sp);
            }




            mapview.MapViewTapped -= MyMapView_MapViewTapped;
        }

        //فعال سازی  پاپ آپ
        private async void MyMapView_MapViewTapped2(object sender, MapViewInputEventArgs e)
        {
            // var resultGeometry = _editGraphic == null ? null : _editGraphic.Geometry;
            if (mapview.Editor.IsActive)
                return;

            var graphic = await _linesOverlay.HitTestAsync(mapview, e.Position);
            // var graphic = await clusterLayer.HitTestAsync(mapview, e.Position);

            if (graphic != null)
            {
                mapview.MapViewTapped -= MyMapView_MapViewTapped2;
                #region Trace
                if (graphic.Attributes["id"].ToString().Contains("TraceId"))
                {
                    var TraceId = graphic.Attributes["id"].ToString();

                    //DAL.Model.GISDBEntities db = new DAL.Model.GISDBEntities();
                    PopupInfo pf = new PopupInfo();
                    var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;
                    var point = ((Esri.ArcGISRuntime.Geometry.MapPoint)graphic.Geometry);
                    Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, point);


                    labl1.Content = "نام تبلت :";
                    labl2.Content = "استادکار :";
                    labl3.Content = "زمان :";

                    FirstProptxt.Content = graphic.Attributes["TabletName"].ToString();
                    SecondProptxt.Content = graphic.Attributes["MasterName"].ToString();
                    thirdProptxt.Content = graphic.Attributes["DT"].ToString();
                    utahMapTip.Visibility = Visibility.Visible;
                    mapview.MapViewTapped += MyMapView_MapViewTapped2;
                }

                #endregion
                #region car
                if (graphic.Attributes["id"].ToString().Contains("CarId"))
                {
                    var CarId = graphic.Attributes["id"].ToString();

                    //DAL.Model.GISDBEntities db = new DAL.Model.GISDBEntities();
                    PopupInfo pf = new PopupInfo();
                    var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;
                    var point = ((Esri.ArcGISRuntime.Geometry.MapPoint)graphic.Geometry);
                    Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, point);


                    labl1.Content = "شهر :";
                    labl2.Content = "دلیل عیب:";


                    FirstProptxt.Content = CarId;
                    SecondProptxt.Content = "Car Name";

                    thirdProptxt.Content = "1398/11/16";


                    utahMapTip.Visibility = Visibility.Visible;
                    

                }


                #endregion
                #region Request
                if (graphic.Attributes["id"].ToString().Contains("RequestId"))
                {
                    var RequestId = graphic.Attributes["id"].ToString();

                    //DAL.Model.GISDBEntities db = new DAL.Model.GISDBEntities();
                    PopupInfo pf = new PopupInfo();
                    var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;
                    var point = ((Esri.ArcGISRuntime.Geometry.MapPoint)graphic.Geometry);
                    Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, point);


                    labl1.Content = "استادکار :";
                    labl2.Content = "نام مشترک :";
                    labl3.Content = "شماره پرونده :";

                    FirstProptxt.Content = graphic.Attributes["MasterName"].ToString();
                    SecondProptxt.Content = graphic.Attributes["SubscriberName"].ToString();
                    thirdProptxt.Content = graphic.Attributes["RequestNumber"].ToString();


                    utahMapTip.Visibility = Visibility.Visible;
                    mapview.MapViewTapped += MyMapView_MapViewTapped2;


                }
                

                #endregion

                #region eyb
                if (graphic.Attributes["id"].ToString().Contains("EybGps_"))
                {

                    /*
                    var EybId = graphic.Attributes["name1"].ToString();

                    DataAccess da = new DataAccess();

                  var dd=  da.GetEybInfoByID(int.Parse(EybId));

                    //DAL.Model.GISDBEntities db = new DAL.Model.GISDBEntities();
                    PopupInfo pf = new PopupInfo();
                    var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;
                    var point = ((Esri.ArcGISRuntime.Geometry.MapPoint)graphic.Geometry);
                    Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, point);

                    labl1.Content = "علت عیب";
                    labl1.Content = "توضیحات";
                    FirstProptxt.Content = EybId;
                    SecondProptxt.Content = "Car Name";

                    thirdProptxt.Content = "1398/11/16";


                    utahMapTip.Visibility = Visibility.Visible;*/
                }
                #endregion

                #region eyb filter shode
                if (graphic.Attributes["id"].ToString().Contains("EybGpsFilter_"))
                {
/*

                    var EybId = graphic.Attributes["name1"].ToString();

                    DataAccess da = new DataAccess();

                    var dd = da.GetEybInfoByID(int.Parse(EybId));

                    //DAL.Model.GISDBEntities db = new DAL.Model.GISDBEntities();
                    PopupInfo pf = new PopupInfo();
                    var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;
                    var point = ((Esri.ArcGISRuntime.Geometry.MapPoint)graphic.Geometry);
                    Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, point);

                    labl1.Content = "علت عیب";
                    labl1.Content = "توضیحات";
                    FirstProptxt.Content = EybId;
                    SecondProptxt.Content = "Car Name";

                    thirdProptxt.Content = "1398/11/16";


                    utahMapTip.Visibility = Visibility.Visible;*/
                }
                #endregion


                #region روش دیگر  نمایش  پاپ آپ  که استفاده نشده
                //   // mapview.ShowCalloutAt(e.Location, calloutDef);
                //   // CalloutDefinition myCalloutDefinition = new CalloutDefinition("Location:", mapLocationDescription);
                //   PopupInfo pf = new PopupInfo();

                //   // Esri.ArcGISRuntime.WebMap.
                //   // Popup 
                //   // mapview.ShowCalloutAt(mapLocation, myCalloutDefinition);
                //   //https://developers.arcgis.com/net/10-2/sample-code/GraphicsLayerSelection/


                //   // InfoWindow 
                //   //   mapview.Map.pop

                //   var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;

                //   // Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, e.Location);
                //   //var MapPoint=new  MapPoint((Esri.ArcGISRuntime.Geom)graphic.Geometry)

                //var point=   ((Esri.ArcGISRuntime.Geometry.MapPoint)graphic.Geometry);
                //  // Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, e.Location);
                //   Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, point);

                //   // MapPoint  mp=new MapPoint (graphic.Geometry.x)
                //   // Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(utahMapTip, graphic.Geometry.map);

                //   FirstProptxt.Content =  (string)graphic.Attributes["name1"];
                //   SecondProptxt.Content = (string)graphic.Attributes["name2"];

                //   thirdProptxt.Content = "1398/11/16";


                //   utahMapTip.Visibility = Visibility.Visible;
                #endregion

            }



            //حذف رویداد  پاپ آپ
            
        }

        //افزودن نقاط  به نقشه
        private async void addCarLog(double x, double y, Dictionary<string, string> attributes)
        {



            #region Ok


            Uri symbolUriz = new Uri("pack://application:,,/Images/arrow45.png");


            // Create new symbol using asynchronous factory method from uri.
            PictureMarkerSymbol campsiteSymbolz = new PictureMarkerSymbol()
            {
                Width = 24,
                Height = 24
            };

            //برای  تغیر  مکان  عکس یا آیکون شما
          // campsiteSymbolz.XOffset = 5;
           await campsiteSymbolz.SetSourceAsync(symbolUriz);
         
            MapPoint campsitePointz = new MapPoint(x, y, SpatialReferences.Wgs84);
           
            Graphic campsiteGraphicz = new Graphic(campsitePointz, campsiteSymbolz);
          
            foreach (var items in attributes)
            {

                campsiteGraphicz.Attributes.Add(items.Key, items.Value);
            }
            campsiteGraphicz.ZIndex = 1000;


            _linesOverlay.Graphics.Add(campsiteGraphicz);

           

            #endregion


        }
        
        private async void addCar(double x, double y, Dictionary<string, string> attributes, int Arrow)
        {



            #region Ok
            //string ImagePath = "pack://application:,,/Images/Car" + Arrow.ToString() + ".png";
            //Uri symbolUriz = new Uri(ImagePath, UriKind.Absolute);
            //Uri uri = new Uri("/Images/Car" + Arrow.ToString() + ".png", UriKind.Relative);
            //StreamResourceInfo info = Application.GetContentStream(uri);
            System.IO.Stream st= this.GetType().Assembly.GetManifestResourceStream("Bargh_GIS.Images.Car" + Arrow.ToString() + ".png");

            // Create new symbol using asynchronous factory method from uri.
            PictureMarkerSymbol campsiteSymbolz = new PictureMarkerSymbol()
            {
                Width = 32,
                Height = 32
            };

            //برای  تغیر  مکان  عکس یا آیکون شما
            // campsiteSymbolz.XOffset = 5;
            //await campsiteSymbolz.SetSourceAsync(symbolUriz);
            await campsiteSymbolz.SetSourceAsync(st);

            MapPoint campsitePointz = new MapPoint(x, y, SpatialReferences.Wgs84);

            Graphic campsiteGraphicz = new Graphic(campsitePointz, campsiteSymbolz);

            foreach (var items in attributes)
            {

                campsiteGraphicz.Attributes.Add(items.Key, items.Value);
            }
            campsiteGraphicz.ZIndex = 1000;


            _linesOverlay.Graphics.Add(campsiteGraphicz);



            #endregion


        }

        //پاک کردن نقطه با ارسال  آی دی
        private void clearGrapics(string SuffixIdName)
        {
            // var points = _linesOverlay.Graphics.Where(x => x.Attributes["id"] == "graphicPoints").ToList();
            //  var points = _linesOverlay.Graphics.Where(x => SuffixIdName.Contains(x.Attributes["id"].ToString())).ToList();
            var points = _linesOverlay.Graphics.Where(x => x.Attributes["id"].ToString().Contains(SuffixIdName)).ToList();

            if (points != null)
            {
                foreach (var item in points)
                {
                    _linesOverlay.Graphics.Remove(item);
                }
            }
        }

        //بستن  پاپ  آپ
        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var utahMapTip = this.mapview.Overlays.Items[0] as FrameworkElement;

            utahMapTip.Visibility = Visibility.Collapsed;
        }

        //زوم این
        private void ZoomIn_click(object sender, RoutedEventArgs e)
        {
            mapview.ZoomAsync(1.5);         
        }

        //زوم اوت
        private void ZoomOut_click(object sender, RoutedEventArgs e)
        {

            mapview.ZoomAsync(0.5);
        }
        //حرکت روی نقشه   با  ترسیم مستطیل
        private async void FullExtent_click(object sender, RoutedEventArgs e)
        {

            var newExtent = await mapview.Editor.RequestShapeAsync(Esri.ArcGISRuntime.Controls.DrawShape.Envelope);

            // set the map view extent with the Envelope
            await mapview.SetViewAsync(newExtent);
        }

        #region متد های استفاده نشده  
        public void savePolgonMethod(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadSecurLayer(string servicurl, string tokenUrl, string username, string pass, string expiration)
        {

            var GetTokenurl = tokenUrl + "?request=getToken&username=" + username + "&password=" + pass + "&expiration=" + expiration;


            System.Net.WebRequest request = System.Net.WebRequest.Create(GetTokenurl);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            System.IO.StreamReader readStream = new System.IO.StreamReader(responseStream);
            var theToken = await readStream.ReadToEndAsync();
            var layer = new ArcGISDynamicMapServiceLayer(new Uri(servicurl));
            layer.ID = "network";
            layer.Token = theToken;
            mapview.Map.Layers.Add(layer);
            await mapview.SetViewAsync(layer.FullExtent);
        }

        private async void Mapview_MapViewTapped(object sender, MapViewInputEventArgs e)
        {
            double tolerance = 10d; // Use larger tolerance for touch
            int maximumResults = 1; // Only return one graphic  
            bool onlyReturnPopups = false; // Return more than popups

            //   var graphics = await FindIntersectingGraphicsAsync();

            try
            {
                MapPoint tappedPoint = (MapPoint)GeometryEngine.NormalizeCentralMeridian(e.Location);
                //  GeometryEngine.
                // _linesOverlay.Graphics.Where(x=>x.Geometry.)
                // Esri.ArcGISRuntime.Tasks.Query.IdentifyTask myIdentifyTask = new Esri.ArcGISRuntime.Tasks.Query.IdentifyTask(new Uri(""));
                // Esri.ArcGISRuntime.Tasks.Query.
                //  Esri.ArcGISRuntime.Tasks.Query.IdentifyParameters myIdentifyParameters = new Esri.ArcGISRuntime.Tasks.Query.IdentifyParameters(myMapPoint, myMapExtent, myTolerance, myHeight, myWidth);
                // Esri.ArcGISRuntime.Tasks.

                //  Esri.ArcGISRuntime.Tasks.Query.IdentifyResult myIdentifyResult =  myIdentifyTask.ExecuteAsync(myIdentifyParameters);

                // GeoElement
                //  _linesOverlay.Graphics.
                // _linesOverlay.Graphics.First(x=>x.Geometry.)

                // Esri.ArcGISRuntime.Data.

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }





        }

        // دریافت توکن از  آرک  جی آی اس سرور
        private void GetToken(string url1, string username, string pass)
        {
            //https://sampleserver6.arcgisonline.com/arcgis/tokens/   + "?request=getToken&username=" + '@usernamewms' + "&password=" + '@passwms'
            //http://10.132.160.70:6080/arcgis/tokens/generateToken
            var url = url1 + "?request=getToken&username=" + username + "&password=" + pass;

            // var url = tokenServiceUrl + "?request=getToken&username=" + '@usernamewms' + "&password=" + '@passwms'


            WebClient tokenService = new WebClient();
            tokenService.DownloadStringCompleted += (sender, args) =>
            {

                var theToken = args.Result;


            };
            tokenService.DownloadStringAsync(new Uri(url));

        }
        #endregion

        private void BttnInformation_Click(object sender, RoutedEventArgs e)
        {
            
                mapview.MapViewTapped += MyMapView_MapViewTapped2;
        }
        string JosnPoly = "";
        private async void StartDrawPolygon()
        {

            #region MyRegion
            mapview.Cursor = Cursors.Cross;
            //var mapRect = await mapview.Editor.RequestShapeAsync(DrawShape.Polygon) as Envelope;
            var r = await mapview.Editor.RequestShapeAsync(DrawShape.Polygon);

            //           var polygonSymbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Solid, Color.FromRgb(226, 119, 40),
            //new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Color.FromRgb(0, 0, 255), 2));
            var polygonSymbol = new SimpleFillSymbol();
            polygonSymbol.Style = SimpleFillStyle.Solid;
            polygonSymbol.Color = System.Windows.Media.Color.FromRgb(226, 119, 40);
            polygonSymbol.Outline = new SimpleLineSymbol();
            polygonSymbol.Outline.Style = SimpleLineStyle.Solid;
            polygonSymbol.Outline.Color = System.Windows.Media.Color.FromRgb(0, 0, 255);
            polygonSymbol.Outline.Width = 2;
            //SimpleLineSymbolStyle.Solid, Color.FromRgb(0, 0, 255), 2)
            //Graphic polygonGraphic = new Graphic(polygon, polygonSymbol);

            _linesOverlay.Graphics.Add(new Graphic(r, polygonSymbol));

            JosnPoly = r.ToJson();


            mapview.Cursor = Cursors.Arrow;
            #endregion

        }
        public async void FindEyb()
        {

            clearGrapics("filterGraohic");
            #region MyRegion
            mapview.Cursor = Cursors.Cross;

            //فعال کردن  ترسیم 
            var r = await mapview.Editor.RequestShapeAsync(DrawShape.Polygon);


            //ظاهر پالیگون که قرار است ترسیم شود
            SimpleFillSymbol coloradoSimpleFillSymbol = new SimpleFillSymbol();
            coloradoSimpleFillSymbol.Style = SimpleFillStyle.Solid;
            coloradoSimpleFillSymbol.Color = System.Windows.Media.Color.FromArgb(34, 0, 0, 255);
            coloradoSimpleFillSymbol.Outline = new SimpleLineSymbol();
            coloradoSimpleFillSymbol.Outline.Style = SimpleLineStyle.Solid;

            coloradoSimpleFillSymbol.Outline.Color = System.Windows.Media.Color.FromRgb(0, 0, 255);
            coloradoSimpleFillSymbol.Outline.Width = 2;


            Graphic filterGaphic = new Graphic(r, coloradoSimpleFillSymbol);
            filterGaphic.Attributes.Add("id", "filterGraohic");
            _linesOverlay.Graphics.Add(filterGaphic);

            Esri.ArcGISRuntime.Geometry.Polygon lPolygon = (Esri.ArcGISRuntime.Geometry.Polygon)r;
            SpatialReference sp = new SpatialReference(4326);
            var dd = GeometryEngine.Project(r, sp);

            Esri.ArcGISRuntime.Geometry.Polygon lPolygon2 = (Esri.ArcGISRuntime.Geometry.Polygon)dd;

            var points = lPolygon2.Parts[0].GetPoints();

            //استخراج نقاط  پالیگون
            var stringPoly = "POLYGON ((";
            var and = "";
            foreach (var itemsd in points)
            {
                stringPoly += and;
                var pointx = itemsd.Y + " ";  //52.11
                stringPoly += pointx;
                var pointy = itemsd.X; //33.54
                stringPoly += pointy;
                //    Esri.ArcGISRuntime.Geometry.Geometry g=new  




                and = ",";

            }

            stringPoly += "))";
            try
            {
                var Location = System.Data.Entity.Spatial.DbGeometry.PolygonFromText(stringPoly, 4326);
                Location = System.Data.Entity.Spatial.DbGeometry.PolygonFromText(stringPoly, 4326);
            }
            catch (Exception e)
            {

            }

            Classes.CDatabase db = new Classes.CDatabase();
            DataTable lDt = db.ExecSQL("exec spGetBazdidResultAddressFromPolygan '" + stringPoly + "'");
            mDt = lDt;

            mIsNoCheckList = false;
            mIsReadyPolygan = false;

            if (mDt != null && mDt.Rows.Count > 0)
                mIsReadyPolygan = true;
            else if (mDt.Rows.Count == 0)
                mIsNoCheckList = true;
            else
                mIsReadyPolygan = false;


            mapview.Cursor = Cursors.Arrow;
            /*

            //DAL.Model.GISDBEntities db = new DAL.Model.GISDBEntities();
            //   var Location = System.Data.Entity.Spatial.DbGeometry.PolygonFromText("POLYGON ((35.702179 51.333565, 35.696661 51.332540, 35.695313 51.3430933, 35.703237 51.343888,35.704803 51.338780,35.702179 51.333565))", 4326);
            
            //Select ** 

            if (Location.IsValid)
            {
                //انجام جستجو روی ویو-Intersects
                var items = db.VW_EybShape.Where(x => x.Shape.Intersects(Location) == true).ToList();


                foreach (var point in items)
                {

                    var shp = point;
                    var name = point.EybDesc;
                    var id = point.EybId;


                    var shape = point.Shape;
                    var x = (double)shape.XCoordinate;
                    var y = (double)shape.YCoordinate;


                    Dictionary<string, string> attr = new Dictionary<string, string>();
                    attr.Add("id", "EybGpsFilter_" + id);

                    attr.Add("name1", name.ToString());
                    attr.Add("name2", id.ToString());

                    //  addGpsLog(y, x, attr);
                    addCarLog(y, x, attr);


                }

            }
            else
            {

                MessageBox.Show("please drow another polygon");
            }

            mapview.Cursor = Cursors.Arrow;
            */
            #endregion
        }

        private void BttnPolyline_Click(object sender, RoutedEventArgs e)
        {
            StartDrawPolygon();
        }

        private void BttnFindEyb_Click(object sender, RoutedEventArgs e)
        {
            FindEyb();
        }
    }




}
