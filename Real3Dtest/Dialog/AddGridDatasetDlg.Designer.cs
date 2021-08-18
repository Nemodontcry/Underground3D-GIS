
namespace Real3Dtest.Dialog
{
    partial class AddGridDatasetDlg
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
            this.m_checkBoxAddAsImage = new System.Windows.Forms.CheckBox();
            this.m_checkBoxAddAsTerrain = new System.Windows.Forms.CheckBox();
            this.m_buttonComfirm = new System.Windows.Forms.Button();
            this.m_buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_checkBoxAddAsImage
            // 
            this.m_checkBoxAddAsImage.AutoSize = true;
            this.m_checkBoxAddAsImage.Location = new System.Drawing.Point(75, 27);
            this.m_checkBoxAddAsImage.Name = "m_checkBoxAddAsImage";
            this.m_checkBoxAddAsImage.Size = new System.Drawing.Size(108, 16);
            this.m_checkBoxAddAsImage.TabIndex = 0;
            this.m_checkBoxAddAsImage.Text = "数据集影像添加";
            this.m_checkBoxAddAsImage.UseVisualStyleBackColor = true;
            // 
            // m_checkBoxAddAsTerrain
            // 
            this.m_checkBoxAddAsTerrain.AutoSize = true;
            this.m_checkBoxAddAsTerrain.Location = new System.Drawing.Point(75, 62);
            this.m_checkBoxAddAsTerrain.Name = "m_checkBoxAddAsTerrain";
            this.m_checkBoxAddAsTerrain.Size = new System.Drawing.Size(108, 16);
            this.m_checkBoxAddAsTerrain.TabIndex = 1;
            this.m_checkBoxAddAsTerrain.Text = "数据集地形添加";
            this.m_checkBoxAddAsTerrain.UseVisualStyleBackColor = true;
            // 
            // m_buttonComfirm
            // 
            this.m_buttonComfirm.Location = new System.Drawing.Point(171, 119);
            this.m_buttonComfirm.Name = "m_buttonComfirm";
            this.m_buttonComfirm.Size = new System.Drawing.Size(85, 23);
            this.m_buttonComfirm.TabIndex = 2;
            this.m_buttonComfirm.Text = "确定";
            this.m_buttonComfirm.UseVisualStyleBackColor = true;
            this.m_buttonComfirm.Click += new System.EventHandler(this.m_buttonComfirm_Click);
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Location = new System.Drawing.Point(262, 119);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(85, 23);
            this.m_buttonCancel.TabIndex = 3;
            this.m_buttonCancel.Text = "取消";
            this.m_buttonCancel.UseVisualStyleBackColor = true;
            this.m_buttonCancel.Click += new System.EventHandler(this.m_buttonCancel_Click);
            // 
            // AddGridDatasetDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 151);
            this.Controls.Add(this.m_buttonCancel);
            this.Controls.Add(this.m_buttonComfirm);
            this.Controls.Add(this.m_checkBoxAddAsTerrain);
            this.Controls.Add(this.m_checkBoxAddAsImage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddGridDatasetDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Realspace";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_checkBoxAddAsImage;
        private System.Windows.Forms.CheckBox m_checkBoxAddAsTerrain;
        private System.Windows.Forms.Button m_buttonComfirm;
        private System.Windows.Forms.Button m_buttonCancel;
    }
}