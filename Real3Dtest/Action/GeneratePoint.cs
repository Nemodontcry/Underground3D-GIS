using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMap.Realspace;
//using SuperMap.Realspace.SpatialAnalyst;
//using SuperMap.Analyst.SpatialAnalyst;
using SuperMap.Data;
using SuperMap.UI;
using System.Diagnostics;
//using Real3Dtest.Dialog;
using Mono.Csv;

namespace Real3Dtest.Action
{
    class GeneratePoint
    {
        private Workspace m_workspace;
        private SceneControl m_sceneControl;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;
        private Datasource m_datasource;

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


        public Point3D GeneratePointWithxyz(double x, double y, double z)
        {
            Point3D testpoint = new Point3D(x, y, z);
            return testpoint;
        }

        public void addPointstoScene()
        {
            CreateUDBDatasource();
            List<List<object>> csvdata = getCSVfile();
            DatasetVector pointDataset = CSVtoDataset(csvdata);
            m_sceneControl.Scene.Layers.Add(pointDataset, new Layer3DSettingVector(), true);
            m_sceneControl.Scene.Refresh();
        }

        public List<List<object>> getCSVfile()
        {
            List<List<string>> csvfile = CsvFileReader.ReadAll("I:\\ServerTool\\supermap-iobjectsdotnet-10.1.2-19530-86195-all\\SampleData\\测试数据\\testdata28.csv", Encoding.GetEncoding("gbk"));
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
            DatasetVectorInfo info = new DatasetVectorInfo("testRandomData", DatasetType.Point3D);
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
            return dataset;
        }



    }


}
