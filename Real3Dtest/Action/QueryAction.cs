using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMap.Data;
using SuperMap.UI;
using SuperMap.Realspace;
using Real3Dtest.Dialog;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;


namespace Real3Dtest.Action
{
    class QueryAction
    {
        private Workspace m_workspace;
        private SceneControl m_sceneControl;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;

        public static string m_filter;

        public QueryAction(WorkspaceControl workspaceControl,
        LayersControl layersControl,
        SceneControl sceneControl,
        Workspace workspace)
        {
            m_workspaceControl = workspaceControl;
            m_layersControl = layersControl;
            m_sceneControl = sceneControl;
            m_workspace = workspace;
        }

        public void SQLQuery(Layer3DDataset layer)
        {
            DatasetVector datasetVector = layer.Dataset as DatasetVector;
            QueryDlg queryDlg = new QueryDlg();
            if (queryDlg.ShowDialog() == DialogResult.OK)
            {
                if (m_filter != null)
                {
                    QueryParameter queryParameter = new QueryParameter();
                    queryParameter.AttributeFilter = m_filter;
                    queryParameter.CursorType = CursorType.Static;
                    Recordset recordset = datasetVector.Query(queryParameter);
                    Selection3D selection = layer.Selection;
                    if (recordset.RecordCount > 0)
                    {
                        while (!recordset.IsEOF)
                        {
                            selection.Add(recordset.GetID());
                            recordset.MoveNext();
                        }
                        recordset.Dispose();
                    }
                    layer.Selection.UpdateData();
                    m_sceneControl.Scene.EnsureVisible(layer.Bounds);
                    queryParameter.Dispose();
                    m_sceneControl.Scene.Refresh();
                }
            }
        }
    }
}
