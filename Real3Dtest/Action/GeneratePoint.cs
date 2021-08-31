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
            double x = 112.5;
            double y = 28.1;
            double z = 1000;
            Point3D testpoint = new Point3D(x, y, z);
            GeoPoint3D geoPoint3D = new GeoPoint3D(testpoint);

            DatasetVectorInfo info = new DatasetVectorInfo(g_pointDataasetName, DatasetType.Point3D);
            DatasetVector dataset = m_datasource.Datasets.Create(info);
            Recordset recordset = dataset.GetRecordset(false, CursorType.Dynamic);

            recordset.AddNew(geoPoint3D);
            recordset.Update();
            Console.WriteLine(recordset.GetValues());
            PrjCoordSys dl = new PrjCoordSys();
            dl.FromEPSGCode(4326);//4326为GCS_WGS_1984
            dataset.PrjCoordSys = dl;

            m_sceneControl.Scene.Layers.Add(dataset, new Layer3DSettingVector(), true);

        }

    }
}
