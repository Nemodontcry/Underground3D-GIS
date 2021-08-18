using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using SuperMap.Data;
using SuperMap.UI;
using SuperMap.Realspace;
using System.Diagnostics;
using System.Drawing;


namespace Real3Dtest.Action
{
    class SceneAction
    {
        private Workspace m_workspace;
        private SceneControl m_sceneControl;

        private GeoStyle3D m_style3d;
        private readonly String m_squareMeter = "平方米";
        private readonly String m_area = "挖方面积";
        //标识地表开挖区域
        private Int32 m_Index = -1;

        public SceneAction(Workspace workspace, SceneControl sceneControl)
        {
            m_sceneControl = sceneControl;
            m_workspace = workspace;
        }
        
        //构造地表开挖区域
        public void AddExcavationRegion()
        {
            try
            {
                m_sceneControl.Action = Action3D.MeasureArea;
                m_sceneControl.Tracking += new Tracking3DEventHandler(m_sceneControl_Tracking);
                m_sceneControl.Tracked += new Tracked3DEventHandler(m_sceneControl_Tracked);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }


        void m_sceneControl_Tracking(object sender, Tracking3DEventArgs e)
        {
            try
            {
                Point3D location = new Point3D(0, 0, 0);
                //获取当前正在绘制的三维几何对象
                //Geometry3D geometry = e.Geometry;
                Geometry3D geometry = e.Geometry;
                //Console.WriteLine(geometry+"57");
                Point3D point3D = new Point3D(e.X, e.Y, e.CurrentHeight);
                point3D.Z = m_sceneControl.Scene.GetAltitude(e.X, e.Y);
                Point point = m_sceneControl.Scene.GlobeToPixel(point3D);
                Int32 index = m_sceneControl.Scene.TrackingLayer.IndexOf("area");
                //Console.WriteLine(index);
                String text = String.Empty;
                if (m_sceneControl.Action == Action3D.MeasureArea)
                {
                    //Console.WriteLine(geometry+"66");
                    if (index != -1)
                    {
                        m_sceneControl.Scene.TrackingLayer.Remove(index);
                    }
                    //Console.WriteLine(geometry+"71");
                    text = String.Format("{0}{1}{2}", m_area, e.TotalArea, m_squareMeter);
                    //Console.WriteLine(text);
                    if (geometry != null)
                    {
                        location = geometry.InnerPoint3D;
                        GeoText3D geoText = new GeoText3D(new TextPart3D(text, location));
                        m_sceneControl.Scene.TrackingLayer.Add(geoText, "area");
                    }

                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                
            }
        }

        void m_sceneControl_Tracked(object sender, Tracked3DEventArgs e)
        {
            try
            {
                m_Index++;
                Int32 index = m_sceneControl.Scene.TrackingLayer.IndexOf("area");
                if (index != -1)
                {
                    m_sceneControl.Scene.TrackingLayer.Remove(index);
                }
                Geometry3D geometry = e.Geometry;
                m_sceneControl.Scene.GlobalImage.AddExcavationRegion(geometry, "ExcavationRegion" + m_Index);
                m_sceneControl.Action = Action3D.Pan2;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        //清除地表开挖区域
        public void RemoveExcavationRegion()
        {
            if (m_sceneControl.Scene.GlobalImage.ExcavationRegionCount > 0)
            {
                m_sceneControl.Scene.GlobalImage.ClearExcavationRegions();
            }
        }

        public void UnregisterEvent()
        {
            m_sceneControl.Tracking -= new Tracking3DEventHandler(m_sceneControl_Tracking);
            m_sceneControl.Tracked -= new Tracked3DEventHandler(m_sceneControl_Tracked);
        }
        
        public Boolean Underground
        {
            get { return m_sceneControl.Scene.Underground.IsVisible; }
            set
            {
                m_style3d = new GeoStyle3D();
                if (value)
                {
                    //相对地下高度模式
                    m_style3d.AltitudeMode = AltitudeMode.RelativeToUnderground;
                    //设置进入地下时所处的深度
                    m_sceneControl.Scene.Underground.Depth = 200;
                }
                else
                {
                    //相对地面高度模式
                    m_style3d.AltitudeMode = AltitudeMode.RelativeToGround;
                }
                m_sceneControl.Scene.Underground.IsVisible = value;
            }
        }
        public Int32 Transparency
        {
            get { return m_sceneControl.Scene.GlobalImage.Transparency; }
            set { m_sceneControl.Scene.GlobalImage.Transparency = value; }
        }

        public Boolean Sun
        {
            get { return m_sceneControl.Scene.Sun.IsVisible; }
            set
            {
                m_sceneControl.Scene.Sun.IsVisible = value;
                m_sceneControl.Scene.Refresh();
            }
        }
        public Boolean Ocean
        {
            get { return m_sceneControl.Scene.Ocean.IsVisible; }
            set
            {
                m_sceneControl.Scene.Ocean.IsVisible = value;
                m_sceneControl.Scene.Refresh();
            }
        }
        public Boolean ScaleLegend
        {
            get { return m_sceneControl.Scene.IsScaleLegendVisible; }
            set
            {
                m_sceneControl.Scene.IsScaleLegendVisible = value;
                //m_sceneControl.Scene.Refresh();
            }
        }
        public Boolean LatLonGrid
        {
            get { return m_sceneControl.Scene.LatLonGrid.IsVisible; }
            set
            {
                m_sceneControl.Scene.LatLonGrid.IsVisible = value;
            }
        }
        public Boolean StatusBar
        {
            get { return m_sceneControl.IsStatusBarVisible; }
            set
            {
                m_sceneControl.IsStatusBarVisible = value;
            }
        }
        public Boolean Navigation
        {
            get { return m_sceneControl.NavigationControl.IsVisible; }
            set
            {
                m_sceneControl.NavigationControl.IsVisible = value;
            }
        }
        public Boolean Atmosphere
        {
            get { return m_sceneControl.Scene.Atmosphere.IsVisible; }
            set
            {
                m_sceneControl.Scene.Atmosphere.IsVisible = value;
            }
        }
    }
}
