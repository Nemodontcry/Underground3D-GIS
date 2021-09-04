using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMap.Data;
using SuperMap.UI;
using SuperMap.Realspace;
using System.Diagnostics;
using Real3Dtest.Action;
using Real3Dtest.Dialog;

namespace Real3Dtest
{
    public partial class MainForm : Form
    {
        SceneControl m_sceneControl = null;
        private Workspace m_workspace;
        private WorkspaceAction m_workspaceAction;
        private Layer3DsAction m_layer3DsAction;
        private SceneAction m_sceneAction;
        private AnalystAction m_analystAction;
        private GeneratePoint m_generatePoint;
        private PointsBuildGeoBody m_PointsBuildGeoBody;
        public MainForm()
        {
            InitializeComponent();
            this.CreateSceneControl();
        }

        private void CreateSceneControl()
        {
            //////////////////////////////////////////////////////////////////////////////
            if (m_sceneControl == null || m_sceneControl.IsDisposed)
            {
                m_sceneControl = new SuperMap.UI.SceneControl();

                m_sceneControl.Action = SuperMap.UI.Action3D.Pan;
                m_sceneControl.BackColor = System.Drawing.Color.White;
                m_sceneControl.Dock = System.Windows.Forms.DockStyle.Fill;
                m_sceneControl.InteractionMode = SuperMap.UI.InteractionMode3D.Default;
                m_sceneControl.IsAlwaysUpdate = false;
                m_sceneControl.IsCursorCustomized = false;
                m_sceneControl.IsFPSVisible = false;
                m_sceneControl.IsKeyboardNavigationEnabled = false;
                m_sceneControl.IsMouseNavigationEnabled = true;
                m_sceneControl.IsStatusBarVisible = true;
                m_sceneControl.IsWaitCursorEnabled = false;
                m_sceneControl.Location = new System.Drawing.Point(3, 3);
                m_sceneControl.Margin = new System.Windows.Forms.Padding(0);
                m_sceneControl.Name = "m_sceneControl";
                m_sceneControl.Size = new System.Drawing.Size(475, 356);
                m_sceneControl.TabIndex = 0;

                this.tabPage1.Controls.Add(m_sceneControl);
                m_sceneControl.Scene.Workspace = m_workspace;

                //this.Initialize();
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_workspace == null)
                {
                    m_workspace = new Workspace();

                }
                m_workspaceControl.WorkspaceTree.Workspace = m_workspace;
                m_sceneControl.Scene.Workspace = m_workspace;
                m_layersControl.Scene = m_sceneControl.Scene;
                m_sceneControl.Action = SuperMap.UI.Action3D.Pan;
                //m_sceneControl.IsKeyboardNavigationEnabled = true;
                m_sceneControl.IsMouseNavigationEnabled = true;
            }
            catch (System.Exception ex)
            {

                Trace.WriteLine(ex.Message);
            }
            m_workspaceControl.WorkspaceTree.AllowDefaultAction = true;
            m_workspaceControl.WorkspaceTree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(WorkspaceTree_NodeMouseDoubleClick);

            m_workspaceAction = new WorkspaceAction(m_workspaceControl, m_layersControl, m_sceneControl, m_workspace);
            WorkspaceAction.m_isNewWorkspace = true;

            m_layer3DsAction = new Layer3DsAction(m_workspaceControl, m_layersControl, m_sceneControl, m_workspace);
            m_sceneAction = new SceneAction(m_workspace, m_sceneControl);
            InitSceneProperties();

            m_analystAction = new AnalystAction(m_workspaceControl, m_layersControl, m_sceneControl);
            m_generatePoint = new GeneratePoint(m_workspaceControl, m_layersControl, m_sceneControl, m_workspace);
            m_PointsBuildGeoBody = new PointsBuildGeoBody(m_workspaceControl, m_layersControl, m_sceneControl, m_workspace);
        }

        void WorkspaceTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            WorkspaceTreeNodeBase node = e.Node as WorkspaceTreeNodeBase;
            WorkspaceTreeNodeDataType type = node.NodeType;
            if ((type & WorkspaceTreeNodeDataType.Dataset) != WorkspaceTreeNodeDataType.Unknown)
            {
                type = WorkspaceTreeNodeDataType.Dataset;
            }

            switch (type)
            {
                case WorkspaceTreeNodeDataType.Dataset:
                { 
                    Dataset dataset = node.GetData() as Dataset;
                        Console.WriteLine(dataset.PrjCoordSys.EPSGCode);
                        Console.WriteLine(dataset.PrjCoordSys.Name);
                    if (m_layersControl.Map != null)
                    {
                        m_layersControl.Map.Layers.Add(dataset, true);
                    }
                    else if (m_layersControl.Scene != null)
                    {
                        //栅格数据
                        if (dataset.Type == DatasetType.Grid)
                        {
                                AddGridDatasetDlg dialog = new AddGridDatasetDlg();
                                dialog.m_sceneControl = m_sceneControl;
                                dialog.dataset = dataset;
                                dialog.Show();
                        }
                        //其他类型数据
                        else
                        {
                            m_layersControl.Scene.Layers.Add(dataset, (Layer3DSetting)null, true);
                        }
                    }
                }
                    break;
                case WorkspaceTreeNodeDataType.MapName:
                    {
                        String mapName = node.GetData() as String;
                        if (m_layersControl.Scene != null)
                        {
                            m_layersControl.Scene.Layers.Add(mapName, Layer3DType.Map, true, mapName);
                            m_sceneControl.Scene.Refresh();

                        }

                    }
                    break;
                case WorkspaceTreeNodeDataType.SceneName:
                    {
                        String sceneName = node.GetData() as String;
                        if (m_layersControl.Scene != null)
                        {
                            m_layersControl.Scene.Open(sceneName);
                            m_sceneControl.Scene.Refresh();
                        }
                    }
                    break;
                case WorkspaceTreeNodeDataType.SymbolMarker:
                    {
                        SymbolLibraryDialog.ShowDialog(m_sceneControl.Scene.Workspace.Resources, SymbolType.Marker);
                    }
                    break;
                case WorkspaceTreeNodeDataType.SymbolLine:
                    {
                        SymbolLibraryDialog.ShowDialog(m_sceneControl.Scene.Workspace.Resources, SymbolType.Line);
                    }
                    break;
                case WorkspaceTreeNodeDataType.SymbolFill:
                    {
                        SymbolLibraryDialog.ShowDialog(m_sceneControl.Scene.Workspace.Resources, SymbolType.Fill);
                    }
                    break;
                default:
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (m_sceneControl != null)
                {
                    m_sceneControl.Dispose();
                }
                if (m_workspace != null)
                {
                    m_workspace.Close();
                    m_workspace.Dispose();
                }
            }
            catch (System.Exception ex)
            {

                Trace.WriteLine(ex.Message);
            }
        }

        private void m_menuOpenWorkspace_Click(object sender, EventArgs e)
        {
            m_workspaceAction.OpenWorkspace();
        }

        private void m_menuAddVectorLayer_Click(object sender, EventArgs e)
        {
            m_layer3DsAction.AddVectorLayer();
        }

        private void m_checkBoxStatusBar_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.StatusBar = m_checkBoxStatusBar.Checked;
        }

        private void m_checkBoxScaleLegend_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.ScaleLegend = m_checkBoxScaleLegend.Checked;
        }

        private void m_checkBoxNavigation_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.Navigation = m_checkBoxNavigation.Checked;
        }

        private void m_checkBoxLatLonGrid_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.LatLonGrid = m_checkBoxLatLonGrid.Checked;
        }

        private void m_checkBoxOcean_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.Ocean = m_checkBoxOcean.Checked;
        }

        private void m_checkBoxAtmosphere_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.Atmosphere = m_checkBoxAtmosphere.Checked;
        }

        private void m_checkBoxSun_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.Sun = m_checkBoxSun.Checked;
        }
        private void InitSceneProperties()
        {
            SceneAction sceneAction = new SceneAction(m_workspace, m_sceneControl);
            m_checkBoxSun.Checked = sceneAction.Sun;
            m_checkBoxOcean.Checked = sceneAction.Ocean;
            m_checkBoxAtmosphere.Checked = sceneAction.Atmosphere;
            m_checkBoxLatLonGrid.Checked = sceneAction.LatLonGrid;
            m_checkBoxNavigation.Checked = sceneAction.Navigation;
            m_checkBoxScaleLegend.Checked = sceneAction.ScaleLegend;
            m_checkBoxStatusBar.Checked = sceneAction.StatusBar;
        }

        private void m_checkBoxUnderground_CheckedChanged(object sender, EventArgs e)
        {
            m_sceneAction.Underground = m_checkBoxUnderground.Checked;
            if (m_checkBoxUnderground.Checked)
            {
                m_buttonAddExcavationRegion.Enabled = true;
                m_hScrollBarTransparency.Enabled = true;
            }
            else
            {
                m_buttonAddExcavationRegion.Enabled = false;
                m_buttonRemoveExcavationRegion.Enabled = false;
                m_hScrollBarTransparency.Enabled = false;
            }
        }

        private void m_hScrollBarTransparency_Scroll(object sender, ScrollEventArgs e)
        {
            m_sceneAction.Transparency = m_hScrollBarTransparency.Value;
        }

        private void m_buttonAddExcavationRegion_Click(object sender, EventArgs e)
        {
            if (m_sceneAction != null)
            {
                m_sceneAction.UnregisterEvent();
                //在跟踪图层上绘制地表开挖区域
                m_sceneAction.AddExcavationRegion();
                m_buttonRemoveExcavationRegion.Enabled = true;
            }
        }

        private void m_buttonRemoveExcavationRegion_Click(object sender, EventArgs e)
        {
            m_sceneAction.RemoveExcavationRegion();
            m_buttonRemoveExcavationRegion.Enabled = false;
        }

        private void m_menuSetRegion_Click(object sender, EventArgs e)
        {
            m_analystAction.SetRegion();
            m_menuCutFill.Enabled = true;
        }

        private DatasetGrid GetSelectedDatasetGrid()
        {
            WorkspaceTreeNodeBase node = m_workspaceControl.WorkspaceTree.SelectedNode as WorkspaceTreeNodeBase;
            WorkspaceTreeNodeDataType type = node.NodeType;
            DatasetGrid datasetGrid = null;
            if (type == WorkspaceTreeNodeDataType.DatasetGrid)
            {
                datasetGrid = node.GetData() as DatasetGrid;
            }
            return datasetGrid;
        }

        private void m_menuCutFill_Click(object sender, EventArgs e)
        {
            if (GetSelectedDatasetGrid() != null)
            {
                m_analystAction.DoCalculateCutFillByRegion(GetSelectedDatasetGrid());
                m_menuCutFill.Enabled = false;
            }
        }

        private void m_menuClearResult_Click(object sender, EventArgs e)
        {
            m_sceneControl.Scene.TrackingLayer.Clear();
        }

        private void m_menuCalculateSlope_Click(object sender, EventArgs e)
        {
            if (GetSelectedDatasetGrid() != null)
            {
                m_analystAction.CalculateSlope(GetSelectedDatasetGrid());
            }
        }

        private void m_menuCalculateAspect_Click(object sender, EventArgs e)
        {
            if (GetSelectedDatasetGrid() != null)
            {
                m_analystAction.CalculateAspect(GetSelectedDatasetGrid());
            }
        }

        private void test3DPoints_Click(object sender, EventArgs e)
        {
            //m_generatePoint.CreateUDBDatasource();
            m_generatePoint.addPointstoScene();
        }

        private void m_createDatasource_Click(object sender, EventArgs e)
        {
            m_PointsBuildGeoBody.testfuc();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.m_sceneControl.Scene.ViewEntire();
        }
    }
}
