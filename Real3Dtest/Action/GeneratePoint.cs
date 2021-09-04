using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SuperMap.Realspace.SpatialAnalyst;
//using SuperMap.Analyst.SpatialAnalyst;
using SuperMap.Realspace;
using SuperMap.Data;
using SuperMap.UI;
using System.Diagnostics;
//using Real3Dtest.Dialog;
using Mono.Csv;
using System.Windows.Forms;
using System.Drawing;
using SuperMap.Realspace.ThreeDDesigner;

namespace Real3Dtest.Action
{
    class GeneratePoint
    {
        private Workspace m_workspace;
        private SceneControl m_sceneControl;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;
        private Datasource m_datasource;
        private FileDialog m_fileDialog;
        private GeoStyle3D m_style3d;
        private string filepath;
        private static String g_layerPointName;
        private static String g_themeLayerName;

        private static String g_pointDataasetName = "RedomPoints";
        private static String g_filedName = "MarkerSize";

        public GeneratePoint(WorkspaceControl workspaceControl,
                LayersControl layersControl,
                SceneControl sceneControl,
                Workspace workspace)
        {
            m_workspaceControl = workspaceControl;
            m_layersControl = layersControl;
            m_sceneControl = sceneControl;
            m_workspace = workspace;
        }

        /// <summary>
        /// 创建UDB数据源
        /// Create UDB datasource
        /// </summary>
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


        public void addPointstoScene()
        {
            OpenWorkspace();
            if (filepath != null)
            {
                CreateUDBDatasource();
                List<List<object>> csvdata = getCSVfile(filepath);
                DatasetVector pointDataset = CSVtoDataset(csvdata, filepath);

                //构造三维矢量图层设置对象并设置其属性
                Layer3DSettingVector layer3DSettingVector = new Layer3DSettingVector();
                GeoStyle3D style = new GeoStyle3D();
                style.FillForeColor = Color.HotPink;
                layer3DSettingVector.Style = style;

                //Geomodeltest(csvdata);
                m_sceneControl.Scene.Layers.Add(pointDataset, layer3DSettingVector, true);
                m_sceneControl.Scene.Refresh();
            }
            

        }

        public List<List<object>> getCSVfile(string filepath)
        {
            List<List<string>> csvfile = CsvFileReader.ReadAll(filepath, Encoding.GetEncoding("gbk"));
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

        public DatasetVector CSVtoDataset(List<List<object>> csvdata, string filepath)
        {
            string filename = System.IO.Path.GetFileName(filepath);
            string[] temp = filename.Split('.');
            DatasetVectorInfo info = new DatasetVectorInfo(temp[0], DatasetType.Point3D);
            DatasetVector dataset = m_datasource.Datasets.Create(info);
            Recordset recordset = dataset.GetRecordset(false, CursorType.Dynamic);


            for (int i = 1; i < csvdata.Count; i++)//csvdata.Count
            {
                double x = (double)csvdata[i][1];
                double y = (double)csvdata[i][2];
                double z = (double)csvdata[i][3];
                Point3D tmppoint = new Point3D(x, y, z);
                GeoPoint3D geoPoint3D = new GeoPoint3D(tmppoint);
                recordset.AddNew(geoPoint3D);
                recordset.Update();
            }
            
            PrjCoordSys dl = new PrjCoordSys();
            dl.FromEPSGCode(4326);//4326为GCS_WGS_1984
            dataset.PrjCoordSys = dl;

            Recordset test = dataset.GetRecordset(false, CursorType.Static);
            Dictionary<Int32, Feature> allFeatures = recordset.GetAllFeatures();
            //Geometry geometry = allFeatures[0].GetGeometry();
            Console.WriteLine(allFeatures);

            return dataset;
        }

        public void OpenWorkspace()
        {
            m_fileDialog = new OpenFileDialog();
            m_fileDialog.Title = "打开工作空间";
            m_fileDialog.FileName = "";
            m_fileDialog.InitialDirectory = @"I:\ServerTool\supermap-iobjectsdotnet-10.1.2-19530-86195-all\SampleData\测试数据";
            m_fileDialog.FilterIndex = 1;
            m_fileDialog.RestoreDirectory = true;
            m_fileDialog.Filter = "SuperMap工作空间文件(*.csv;)|*.csv;";
            if (m_fileDialog.ShowDialog() == DialogResult.OK)
            {
                filepath = m_fileDialog.FileName;
                
            }

        }

        public void Geomodeltest(List<List<object>> datagrid)
        {
            //string filename = System.IO.Path.GetFileName(filepath);
            //string[] temp = filename.Split('.');
            //DatasetVectorInfo info = new DatasetVectorInfo(temp[0], DatasetType.Point3D);
            //DatasetVector dataset = m_datasource.Datasets.Create(info);
            //Recordset recordset = dataset.GetRecordset(false, CursorType.Dynamic);

            //List<Point3D> point3Darr = new List<Point3D>();
            Point3Ds point3Ds = new Point3Ds();
            for (int i = 1; i < datagrid.Count; i++)//csvdata.Count
            {
                double x = (double)datagrid[i][1];
                double y = (double)datagrid[i][2];
                double z = (double)datagrid[i][3];
                Point3D tmppoint = new Point3D(x, y, z);
                point3Ds.Add(tmppoint);

                //GeoPoint3D geoPoint3D = new GeoPoint3D(tmppoint);
                //recordset.AddNew(geoPoint3D);
                //recordset.Update();
            }
            List<double> maxHighList = new List<double>();
            List<double> minHighList = new List<double>();
            for (int i = 0; i < point3Ds.Count; i++)
            {
                maxHighList.Add(i + 1000);
                minHighList.Add(i + 100);
            }
            GeoModel3D geoModel3D = ModelBuilder3D.BuildGeoBody(point3Ds, maxHighList, minHighList);
            m_style3d = new GeoStyle3D();
            //相对地下高度模式
            m_style3d.AltitudeMode = AltitudeMode.RelativeToUnderground;
            //设置进入地下时所处的深度
            m_sceneControl.Scene.Underground.Depth = 200;
            m_sceneControl.Scene.Underground.IsVisible = true;

            m_sceneControl.Scene.TrackingLayer.Add(geoModel3D, "test");
            //Add(geoModel3D);
            //PrjCoordSys dl = new PrjCoordSys();
            //dl.FromEPSGCode(4326);//4326为GCS_WGS_1984
            //dataset.PrjCoordSys = dl;

            //return dataset;
        }





    }
}
