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



namespace Real3Dtest.Dialog
{

    public partial class AddGridDatasetDlg : Form
    {
        public SceneControl m_sceneControl;
        public Dataset dataset;
        public AddGridDatasetDlg()
        {
            InitializeComponent();
            m_checkBoxAddAsImage.Checked = true;
            //Console.WriteLine("default check");
        }

        private void m_buttonComfirm_Click(object sender, EventArgs e)
        {
            if (m_checkBoxAddAsImage.Checked)
            {
                Console.WriteLine(dataset);
                m_sceneControl.Scene.Layers.Add(dataset, new Layer3DSettingGrid(), true);
            }
            if (m_checkBoxAddAsTerrain.Checked)
            {
                m_sceneControl.Scene.TerrainLayers.Add(dataset as DatasetGrid, true);
            }
            this.Close();
        }

        private void m_buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
