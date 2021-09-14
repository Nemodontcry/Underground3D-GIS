using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMap.Realspace;
using SuperMap.Data;
using SuperMap.UI;
using System.Diagnostics;
using SuperMap.Realspace.ThreeDDesigner;
using Mono.Csv;
using System.Drawing;


namespace Real3Dtest.Action
{
    class GeoModelBoolCal
    {
        private Workspace m_workspace;
        private SceneControl m_sceneControl;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;
        private Datasource m_datasource;

        public GeoModelBoolCal(WorkspaceControl workspaceControl,
        LayersControl layersControl,
        SceneControl sceneControl,
        Workspace workspace)
        {
            m_workspaceControl = workspaceControl;
            m_layersControl = layersControl;
            m_sceneControl = sceneControl;
            m_workspace = workspace;
        }

        public void testfunc()
        {
            CreateUDBDatasource();
            List<List<object>> csvdata = getCSVfile();
            DatasetVector targetDataset = CSVtoDataset(csvdata);
            TestScene(targetDataset, m_sceneControl.Scene);
            Geomodeltest(csvdata);

        }
        private void CreateUDBDatasource()
        {
            try
            {
                DatasourceConnectionInfo info = new DatasourceConnectionInfo();

                // 设置数据源位置为内存中，类型为UDB
                // The location of datasource is set in memory and the type of datasource is UDB
                info.Server = ":memory:";
                info.EngineType = EngineType.UDB;

                m_datasource = m_workspace.Datasources.Create(info);
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public List<List<object>> getCSVfile()
        {
            List<List<string>> csvfile = CsvFileReader.ReadAll("I:\\ServerTool\\supermap-iobjectsdotnet-10.1.2-19530-86195-all\\SampleData\\测试数据\\testdata28_500.csv", Encoding.GetEncoding("gbk"));
            //新建一个list保存csv中的数据 
            List<List<object>> dataGrid = new List<List<object>>();
            //把数字从str转为double
            foreach (var row in csvfile)
            {
                List<object> rowList = new List<object>();
                foreach (var cell in row)
                {
                    double numDouble3;
                    if (double.TryParse(cell, out numDouble3))
                    {
                        rowList.Add(numDouble3);
                    }
                    else
                    {
                        rowList.Add(cell);
                    }
                }
                dataGrid.Add(rowList);
            }
            return dataGrid;
        }

        public DatasetVector CSVtoDataset(List<List<object>> csvdata)
        {

            DatasetVectorInfo info = new DatasetVectorInfo("testdata_point", DatasetType.Point3D);
            DatasetVector dataset = m_datasource.Datasets.Create(info);
            Recordset recordset = dataset.GetRecordset(false, CursorType.Dynamic);

            //Point3Ds point3Dsarr = new Point3Ds();
            for (int i = 1; i < csvdata.Count; i++)//csvdata.Count
            {
                double x = (double)csvdata[i][1];
                double y = (double)csvdata[i][2];
                double z = (double)csvdata[i][3];
                //Point3D tmppoint = new Point3D(x, y, z);
                //point3Dsarr.Add(tmppoint);

                GeoPoint3D geoPoint3D = new GeoPoint3D(x, y, z);
                recordset.AddNew(geoPoint3D);
                recordset.Update();
            }
            //List<double> maxHighList = new List<double>();
            //List<double> minHighList = new List<double>();
            //for (int i = 0; i < point3Dsarr.Count; i++)
            //{
            //    maxHighList.Add(i + 1000);
            //    minHighList.Add(i + 100);
            //}

            PrjCoordSys dl = new PrjCoordSys();
            dl.FromEPSGCode(4326);//4326为GCS_WGS_1984
            dataset.PrjCoordSys = dl;

            return dataset;
        }


        public void TestScene(DatasetVector targetDataset, Scene sceneObject)
        {
            //进行三维地图场景的设置
            sceneObject.Atmosphere.IsVisible = true;
            Camera camera = new Camera(102, 31, 10000, AltitudeMode.RelativeToGround);
            sceneObject.Camera = camera;
            Fog fog = new Fog();
            fog.Mode = FogMode.LINEAR;
            fog.IsEnabled = true;
            sceneObject.Fog = fog;
            sceneObject.IsScaleLegendVisible = true;
            sceneObject.Name = "二维矢量数据的三维显示";

            //设置矢量数据集在三维场景中的显示风格，并进行显示
            Layer3DSettingVector layer3DSettingVector = new Layer3DSettingVector();
            GeoStyle3D geoStyle3D = new GeoStyle3D();
            geoStyle3D.MarkerColor = Color.Red;
            geoStyle3D.MarkerSize = 10;
            layer3DSettingVector.Style = geoStyle3D;

            //必须设置数据集高度模式，才可以正常显示高程
            AltitudeMode altitudeMode = new AltitudeMode();
            altitudeMode = AltitudeMode.Absolute;
            layer3DSettingVector.Style.AltitudeMode = altitudeMode;

            Layer3DDataset layer3DDatasetPoint = sceneObject.Layers.Add(targetDataset, layer3DSettingVector, true);
            layer3DDatasetPoint.UpdateData();

            //全幅显示三维地图场景
            sceneObject.ViewEntire();
            sceneObject.Refresh();

            //获取三维场景的XML形式的描述
            String descriptionScene = sceneObject.ToXML();
            Console.WriteLine("当前三维场景的信息：" + descriptionScene);
        }



        public void Geomodeltest(List<List<object>> datagrid)
        {

            Point3Ds point3Ds = new Point3Ds();
            List<double> maxHighList = new List<double>();
            List<double> minHighList = new List<double>();
            for (int i = 1; i < datagrid.Count; i++)//csvdata.Count
            {
                double x = (double)datagrid[i][1];
                double y = (double)datagrid[i][2];
                double z = (double)datagrid[i][3];
                double height = (double)datagrid[i][4];
                maxHighList.Add(z);
                minHighList.Add(z - height);
                Point3D tmppoint = new Point3D(x, y, z);
                point3Ds.Add(tmppoint);
            }


            GeoModel3D geoModel3D_1 = ModelBuilder3D.BuildGeoBody(point3Ds, maxHighList, minHighList);
            maxHighList[1] = 1000;
            GeoModel3D geoModel3D_2 = ModelBuilder3D.BuildGeoBody(point3Ds, maxHighList, minHighList);

            Geometry3D result = BooleanOperator3D.Union(geoModel3D_1, geoModel3D_2);

            //必须设置模型style的高度模式，才可以正常显示高程位置
            //这里使用的是绝对海拔高程模式

            //新建一个style3d对象
            GeoStyle3D m_style3d = new GeoStyle3D();
            m_style3d.AltitudeMode = AltitudeMode.Absolute;
            m_style3d.FillForeColor = Color.BurlyWood;
            //设置geomodel3d对象的style3d属性
            geoModel3D_1.Style3D = m_style3d;

            GeoStyle3D m_style3d2 = new GeoStyle3D();
            m_style3d2.AltitudeMode = AltitudeMode.Absolute;
            m_style3d2.FillForeColor = Color.DarkGray;
            //设置geomodel3d对象的style3d属性
            geoModel3D_2.Style3D = m_style3d2;

            GeoStyle3D m_style3d3 = new GeoStyle3D();
            m_style3d3.AltitudeMode = AltitudeMode.Absolute;
            m_style3d3.FillForeColor = Color.OrangeRed;
            //设置geomodel3d对象的style3d属性
            result.Style3D = m_style3d3;

            //m_sceneControl.Scene.TrackingLayer.Add(geoModel3D_1, "geoModel3D_1");
            //m_sceneControl.Scene.TrackingLayer.Add(geoModel3D_2, "geoModel3D_2");
            m_sceneControl.Scene.TrackingLayer.Add(result, "result");

        }

    }
}
