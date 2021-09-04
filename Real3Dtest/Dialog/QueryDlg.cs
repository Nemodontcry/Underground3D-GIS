using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Real3Dtest.Action;

namespace Real3Dtest.Dialog
{
    public partial class QueryDlg : Form
    {
        public QueryDlg()
        {
            InitializeComponent();
        }

        private void m_buttonComfirm_Click(object sender, EventArgs e)
        {
            string filter = m_textBoxAttributrFilter.Text;
            if (filter != null && filter.Trim().Length > 0)
            {
                QueryAction.m_filter = filter;
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void m_buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
