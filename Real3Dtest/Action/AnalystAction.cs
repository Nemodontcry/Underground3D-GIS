using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using SuperMap.Realspace;
using SuperMap.Realspace.SpatialAnalyst;
using SuperMap.Analyst.SpatialAnalyst;
using SuperMap.Data;
using SuperMap.UI;
using Real3Dtest.Dialog;


namespace Real3Dtest.Action
{
    class AnalystAction
    {
        private Workspace m_workspace;
        private SceneControl m_sceneControl;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;

        private Datasource m_datasource;
        private Boolean m_startDraw = false;
        private Boolean m_isDrawing = false;
        private Point3D m_StartPos;
        private GeoRegion m_region;
        private GeoRegion3D m_geoRegion3D;
        private String m_fillcutResultMessage = String.Empty;

        public AnalystAction(WorkspaceControl workspaceControl, LayersControl layersControl, SceneControl sceneControl)
        {
            m_workspaceControl = workspaceControl;
            m_sceneControl = sceneControl;
            m_layersControl = layersControl;
        }

        public void SetRegion()
        {
            try
            {
                //注销鼠标事件
                m_sceneControl.MouseDown -= new MouseEventHandler(m_sceneControl_MouseDown);
                m_sceneControl.MouseMove -= new MouseEventHandler(m_sceneControl_MouseMove);
                //注册鼠标事件
                m_sceneControl.MouseDown += new MouseEventHandler(m_sceneControl_MouseDown);
                m_sceneControl.MouseMove += new MouseEventHandler(m_sceneControl_MouseMove);
                m_startDraw = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        void m_sceneControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_startDraw)
                {
                    m_isDrawing = true;
                    m_startDraw = false;
                    m_StartPos = m_sceneControl.Scene.PixelToGlobe(new Point(e.X, e.Y), PixelToGlobeMode.TerrainAndModel);
                    m_sceneControl.Action = Action3D.Select;
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Left && m_isDrawing)
                {
                    m_isDrawing = false;
                    m_sceneControl.Action = Action3D.Pan;

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        void m_sceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_isDrawing && m_StartPos != null)
                {
                    Point3D currentPos = m_sceneControl.Scene.PixelToGlobe(new Point(e.X, e.Y), PixelToGlobeMode.TerrainAndModel);
                    Point3Ds point3Ds = new Point3Ds();
                    point3Ds.Add(m_StartPos);
                    point3Ds.Add(new Point3D(currentPos.X, m_StartPos.Y, m_StartPos.Z));
                    point3Ds.Add(currentPos);
                    point3Ds.Add(new Point3D(m_StartPos.X, currentPos.Y, currentPos.Z));
                    Point2Ds point2Ds = new Point2Ds();
                    point2Ds.Add(new Point2D(m_StartPos.X, m_StartPos.Y));
                    point2Ds.Add(new Point2D(currentPos.X, m_StartPos.Y));
                    point2Ds.Add(new Point2D(currentPos.X, currentPos.Y));
                    point2Ds.Add(new Point2D(m_StartPos.X, currentPos.Y));
                    //根据起始点和右下点绘制区域
                    GeoRegion geoRegion = new GeoRegion(point2Ds);
                    m_region = geoRegion;
                    //设置绘制区域风格
                    m_geoRegion3D = new GeoRegion3D(point3Ds);
                    m_geoRegion3D.Style3D = new GeoStyle3D();
                    m_geoRegion3D.Style3D.AltitudeMode = AltitudeMode.ClampToGround;
                    m_geoRegion3D.Style3D.FillForeColor = Color.FromArgb(180, 255, 0, 0);
                    int findIndex = m_sceneControl.Scene.TrackingLayer.IndexOf("gBox");
                    if (findIndex != -1)
                    {
                        m_sceneControl.Scene.TrackingLayer.Remove(findIndex);
                    }
                    m_sceneControl.Scene.TrackingLayer.Add(m_geoRegion3D, "gBox");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }


        ///<summary>
        ///选面挖方分析
        ///</summary>
        ///<param name="excavationRegion">计算面</param>
        public void DoCalculateCutFillByRegion(DatasetGrid datasetGrid)
        {
            try
            {
                m_workspace = m_workspaceControl.WorkspaceTree.Workspace;
                String name = datasetGrid.Name;

                foreach (Datasource datasource in m_workspace.Datasources)
                {
                    if (datasource.Datasets.Contains(name))
                    {
                        m_datasource = datasource;
                        break;
                    }
                }
                Dataset dataset = GetConvertGrid(datasetGrid);
                if (dataset != null)
                {
                    //对绘制区域进行投影转换
                    PrjCoordSys srcPrj = new PrjCoordSys();
                    srcPrj.Type = PrjCoordSysType.EarthLongitudeLatitude;
                    srcPrj.GeoCoordSys = datasetGrid.PrjCoordSys.GeoCoordSys;
                    bool success = CoordSysTranslator.Convert(m_region as Geometry, srcPrj, dataset.PrjCoordSys, new CoordSysTransParameter(), CoordSysTransMethod.PositionVector);
                    if (success)
                    {
                        string outputDatasetName = m_datasource.Datasets.GetAvailableDatasetName("cutFillResult");
                        CutFillResult cutFillResult = CalculationTerrain.CutFill(dataset as DatasetGrid, m_region, 10, m_datasource, outputDatasetName);
                        if (cutFillResult != null)
                        {
                            Double cutArea = cutFillResult.CutArea;
                            Double cutVolumn = cutFillResult.CutVolume;
                            Double fillArea = cutFillResult.FillArea;
                            Double fillVolume = cutFillResult.FillVolume;
                            Double remainderArea = cutFillResult.RemainderArea;
                            string m_fillcutResultMessage = "挖掘面积：" + cutArea + "平方米\r\n挖掘体积：" + cutVolumn
                                + "立方米\r\n填充面积：" + fillArea + "平方米\r\n填充体积：" + fillVolume
                                + "立方米\r\n未填挖方面积：" + remainderArea + "平方米";
                            MessageBox.Show(m_fillcutResultMessage);
                        }
                        else
                        {
                            MessageBox.Show("所绘制的挖方区域不在栅格数据范围内！");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public Dataset GetConvertGrid(DatasetGrid datasetGrid)
        {
            try
            {
                if (!datasetGrid.HasPrjCoordSys)
                {
                    PrjCoordSys prj = new PrjCoordSys(PrjCoordSysType.EarthLongitudeLatitude);
                    datasetGrid.PrjCoordSys = prj;
                }

                PrjCoordSys targetPrj = new PrjCoordSys(PrjCoordSysType.Wgs1984Utm50N);
                string gridname = m_datasource.Datasets.GetAvailableDatasetName(datasetGrid.Name);
                if (m_datasource.Datasets.Contains(gridname))
                {
                    m_datasource.Datasets.Delete(gridname);
                }
                Dataset dataset = CoordSysTranslator.Convert(datasetGrid, targetPrj, m_datasource, gridname, new CoordSysTransParameter(), CoordSysTransMethod.PositionVector);
                return dataset;
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return null;
            }
        }


        //坡度计算
        public void CalculateSlope(DatasetGrid datasetGrid)
        {
            try
            {
                m_workspace = m_workspaceControl.WorkspaceTree.Workspace;
                string name = datasetGrid.Name;
                foreach (Datasource datasource in m_workspace.Datasources)
                {
                    if (datasource.Datasets.Contains(name))
                    {
                        m_datasource = datasource;
                        break;
                    }
                }
                string outputDatasetName = m_datasource.Datasets.GetAvailableDatasetName("gridSlopeResult");
                DatasetGrid resultDatasetGrid = CalculationTerrain.CalculateSlope(datasetGrid, SlopeType.Degree, 1.0, m_datasource, outputDatasetName);
                Layer3D layer = m_sceneControl.Scene.Layers.Add(resultDatasetGrid, new Layer3DSettingGrid(), true, outputDatasetName);
                m_sceneControl.Scene.EnsureVisible(layer.Bounds);
                m_sceneControl.Scene.Refresh();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        //坡向计算
        public void CalculateAspect(DatasetGrid datasetGrid)
        {
            try
            {
                m_workspace = m_workspaceControl.WorkspaceTree.Workspace;
                string name = datasetGrid.Name;
                foreach (Datasource datasource in m_workspace.Datasources)
                {
                    if (datasource.Datasets.Contains(name))
                    {
                        m_datasource = datasource;
                        break;
                    }
                }
                string outputDatasetName = m_datasource.Datasets.GetAvailableDatasetName("gridAspectResult");
                DatasetGrid resultDatasetGrid = CalculationTerrain.CalculateAspect(datasetGrid, m_datasource, outputDatasetName);
                Layer3D layer = m_sceneControl.Scene.Layers.Add(resultDatasetGrid, new Layer3DSettingGrid(), true, outputDatasetName);
                m_sceneControl.Scene.EnsureVisible(layer.Bounds);
                m_sceneControl.Scene.Refresh();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }





    }
}
