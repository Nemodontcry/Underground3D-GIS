
namespace Real3Dtest.Dialog
{
    partial class QueryDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_labelAttributeFilter = new System.Windows.Forms.Label();
            this.m_textBoxAttributrFilter = new System.Windows.Forms.TextBox();
            this.m_buttonComfirm = new System.Windows.Forms.Button();
            this.m_buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_labelAttributeFilter
            // 
            this.m_labelAttributeFilter.AutoSize = true;
            this.m_labelAttributeFilter.Location = new System.Drawing.Point(12, 55);
            this.m_labelAttributeFilter.Name = "m_labelAttributeFilter";
            this.m_labelAttributeFilter.Size = new System.Drawing.Size(53, 12);
            this.m_labelAttributeFilter.TabIndex = 0;
            this.m_labelAttributeFilter.Text = "查询条件";
            // 
            // m_textBoxAttributrFilter
            // 
            this.m_textBoxAttributrFilter.Location = new System.Drawing.Point(81, 52);
            this.m_textBoxAttributrFilter.Name = "m_textBoxAttributrFilter";
            this.m_textBoxAttributrFilter.Size = new System.Drawing.Size(331, 21);
            this.m_textBoxAttributrFilter.TabIndex = 1;
            // 
            // m_buttonComfirm
            // 
            this.m_buttonComfirm.Location = new System.Drawing.Point(242, 106);
            this.m_buttonComfirm.Name = "m_buttonComfirm";
            this.m_buttonComfirm.Size = new System.Drawing.Size(75, 23);
            this.m_buttonComfirm.TabIndex = 2;
            this.m_buttonComfirm.Text = "确定";
            this.m_buttonComfirm.UseVisualStyleBackColor = true;
            this.m_buttonComfirm.Click += new System.EventHandler(this.m_buttonComfirm_Click);
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Location = new System.Drawing.Point(337, 106);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.m_buttonCancel.TabIndex = 3;
            this.m_buttonCancel.Text = "取消";
            this.m_buttonCancel.UseVisualStyleBackColor = true;
            this.m_buttonCancel.Click += new System.EventHandler(this.m_buttonCancel_Click);
            // 
            // QueryDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 141);
            this.Controls.Add(this.m_buttonCancel);
            this.Controls.Add(this.m_buttonComfirm);
            this.Controls.Add(this.m_textBoxAttributrFilter);
            this.Controls.Add(this.m_labelAttributeFilter);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueryDlg";
            this.ShowInTaskbar = false;
            this.Text = "SQL查询";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labelAttributeFilter;
        private System.Windows.Forms.TextBox m_textBoxAttributrFilter;
        private System.Windows.Forms.Button m_buttonComfirm;
        private System.Windows.Forms.Button m_buttonCancel;
    }
}