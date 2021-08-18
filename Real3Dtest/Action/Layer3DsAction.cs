using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMap.Data;
using SuperMap.UI;
using SuperMap.Realspace;


namespace Real3Dtest.Action
{
    class Layer3DsAction
    {
        private Workspace m_workspace;
        private WorkspaceControl m_workspaceControl;
        private LayersControl m_layersControl;
        private SceneControl m_sceneControl;

        public Layer3DsAction(WorkspaceControl workspaceControl,
                        LayersControl layersControl,
                        SceneControl sceneControl,
                        Workspace workspace)
        {
            m_workspaceControl = workspaceControl;
            m_layersControl = layersControl;
            m_sceneControl = sceneControl;
            m_workspace = workspace;
        }

        public void AddVectorLayer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supermap矢量/模型缓存文件（*.scv）|*.scv|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog()== DialogResult.OK)
            {
                try
                {
                    Layer3Ds layers = m_sceneControl.Scene.Layers;
                    layers.Add(openFileDialog.FileName, Layer3DType.VectorFile, true);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("矢量/缓存图层加载失败：" + ex.Message);
                }
            }
        }
        public void AddImageLayer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supermap矢量/模型缓存文件（*.scv）|*.scv|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Layer3Ds layers = m_sceneControl.Scene.Layers;
                    layers.Add(openFileDialog.FileName, Layer3DType.VectorFile, true);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("矢量/缓存图层加载失败：" + ex.Message);
                }
            }
        }
        public void AddTerrainLayer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supermap矢量/模型缓存文件（*.scv）|*.scv|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Layer3Ds layers = m_sceneControl.Scene.Layers;
                    layers.Add(openFileDialog.FileName, Layer3DType.VectorFile, true);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("矢量/缓存图层加载失败：" + ex.Message);
                }
            }
        }
        public void AddKMLLayer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supermap矢量/模型缓存文件（*.scv）|*.scv|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Layer3Ds layers = m_sceneControl.Scene.Layers;
                    layers.Add(openFileDialog.FileName, Layer3DType.VectorFile, true);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("矢量/缓存图层加载失败：" + ex.Message);
                }
            }
        }
    }
}
