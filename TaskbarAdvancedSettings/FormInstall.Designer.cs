
namespace TaskbarAdvancedSettings
{
    partial class FormInstall
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInstall));
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.installBtn = new System.Windows.Forms.Button();
            this.runBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(442, 175);
            this.panel2.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(206, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "KRtkovo.eu - Ondřej Kotas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(206, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Advanced Settings for Windows";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(115, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Publisher:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(115, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Module name:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::TaskbarAdvancedSettings.Properties.Resources.logo_48;
            this.pictureBox2.Location = new System.Drawing.Point(52, 11);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(48, 48);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.installBtn);
            this.panel3.Controls.Add(this.runBtn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 190);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(442, 55);
            this.panel3.TabIndex = 2;
            // 
            // installBtn
            // 
            this.installBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.installBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.installBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.installBtn.Location = new System.Drawing.Point(300, 13);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(130, 30);
            this.installBtn.TabIndex = 0;
            this.installBtn.Text = "Install";
            this.installBtn.UseVisualStyleBackColor = true;
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.runBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.runBtn.Font = new System.Drawing.Font("Segoe UI Variable Display", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.runBtn.Location = new System.Drawing.Point(12, 13);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(130, 30);
            this.runBtn.TabIndex = 9;
            this.runBtn.Text = "Run once";
            this.runBtn.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::TaskbarAdvancedSettings.Properties.Resources.gradient;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(442, 70);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Display", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(94, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(319, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "Do you want to enable the following module\r\nby installation or run it once?";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::TaskbarAdvancedSettings.Properties.Resources.uac_48;
            this.pictureBox1.Location = new System.Drawing.Point(28, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(115, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(296, 45);
            this.label6.TabIndex = 5;
            this.label6.Text = "This tool brings back settings options for legacy features\r\npresent in all Window" +
    "s versions however not accessible\r\nin latest Windows 11.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Variable Small", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(49, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 15);
            this.label7.TabIndex = 6;
            this.label7.Text = "Summary:";
            // 
            // FormInstall
            // 
            this.AcceptButton = this.installBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.runBtn;
            this.ClientSize = new System.Drawing.Size(442, 245);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInstall";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enable Advanced Settings";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button installBtn;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}