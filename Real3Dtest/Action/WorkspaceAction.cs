using SuperMap.Data;
using SuperMap.UI;
using System;
using System.Windows.Forms;

namespace Real3Dtest.Action
{
    class WorkspaceAction
    {
        private Workspace m_workspace;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;
        private SceneControl m_sceneControl;

        public static WorkspaceConnectionInfo m_workspaceConnectionInfo;
        public static bool m_isNewWorkspace;
        private FileDialog m_fileDialog;

        public WorkspaceAction(WorkspaceControl workspaceControl,
                                LayersControl layersControl,
                                SceneControl sceneControl,
                                Workspace workspace)
        {
            m_workspaceControl = workspaceControl;
            m_layersControl = layersControl;
            m_sceneControl = sceneControl;
            m_workspace = workspace;
        }

        //打开工作空间

        public void OpenWorkspace()
        {
            m_fileDialog = new OpenFileDialog();
            m_fileDialog.Title = "打开工作空间";
            m_fileDialog.FileName = "";
            m_fileDialog.InitialDirectory = @"I:\ServerTool\supermap-iobjectsdotnet-10.1.2-19530-86195-all\SampleData";
            m_fileDialog.FilterIndex = 1;
            m_fileDialog.RestoreDirectory = true;
            m_fileDialog.Filter = "SuperMap工作空间文件(*.smwu;*.sxwu)|*.smwu;*.sxwu";
            if (m_fileDialog.ShowDialog() == DialogResult.OK)
            {
                String filename = m_fileDialog.FileName;
                m_workspaceConnectionInfo = new WorkspaceConnectionInfo(filename);
                if (m_workspaceConnectionInfo != null)
                {
                    m_workspace.Close();
                    m_workspace = new Workspace();
                    m_isNewWorkspace = true;
                    if (m_workspace.Open(m_workspaceConnectionInfo))
                    {
                        m_isNewWorkspace = false;
                        m_workspaceControl.WorkspaceTree.Workspace = m_workspace;
                        m_sceneControl.Scene.Workspace = m_workspace;
                        m_layersControl.Scene = m_sceneControl.Scene;
                        m_sceneControl.Scene.ViewEntire();
                    }
                    else
                    {
                        MessageBox.Show("打开工作空间失败！");
                    }
                }
            }
        }
    }
}