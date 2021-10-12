using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TaskbarAdvancedSettings.Helpers;

namespace TaskbarAdvancedSettings
{
    class SelectAppDialogPanel : UserControl
    {
        private Panel webBrowserPanel;
        private Label webBrowserLbl;
        private PictureBox webBrowserPictureBox;
        private bool _isSelected = false;
        public bool IsSelected { get { return _isSelected; }
            set {
                _isSelected = value;
                if (value)
                {
                    this.webBrowserPanel.BackColor = ColorTranslator.FromHtml("#0B83DA");
                    this.webBrowserLbl.ForeColor = Color.White;
                }
                else
                {
                    this.webBrowserPanel.BackColor = Color.White;
                    this.webBrowserLbl.ForeColor = Color.Black;
                }
            }
        }

        public SelectAppDialogPanel()
        {
            InitializeComponent();
        }
        public SelectAppDialogPanel(WindowsHelper.WebBrowser webBrowser) : this()
        {
            webBrowserLbl.Text = webBrowser.BrowserInfo.ProductName;
            webBrowserPictureBox.Image = webBrowser.BrowserInfo.Icon;
        }

        public void SetBrowserName(string browserName)
        {
            webBrowserLbl.Text = browserName;
        }
        public void SetBrowserIcon(Image browserIcon)
        {
            webBrowserPictureBox.Image = browserIcon;
        }

        private void InitializeComponent()
        {
            this.webBrowserPanel = new System.Windows.Forms.Panel();
            this.webBrowserLbl = new System.Windows.Forms.Label();
            this.webBrowserPictureBox = new System.Windows.Forms.PictureBox();
            this.webBrowserPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webBrowserPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowserPanel
            // 
            this.webBrowserPanel.Controls.Add(this.webBrowserLbl);
            this.webBrowserPanel.Controls.Add(this.webBrowserPictureBox);
            this.webBrowserPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.webBrowserPanel.Location = new System.Drawing.Point(0, 0);
            this.webBrowserPanel.Name = "webBrowserPanel";
            this.webBrowserPanel.Size = new System.Drawing.Size(400, 70);
            this.webBrowserPanel.TabIndex = 10;
            this.webBrowserPanel.Click += new System.EventHandler(this.webBrowserPanel_Click);
            this.webBrowserPanel.MouseEnter += new System.EventHandler(this.webBrowserPanel_MouseEnter);
            this.webBrowserPanel.MouseLeave += new System.EventHandler(this.webBrowserPanel_MouseLeave);
            // 
            // webBrowserLbl
            // 
            this.webBrowserLbl.AutoSize = true;
            this.webBrowserLbl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.webBrowserLbl.Font = new System.Drawing.Font("Segoe UI Variable Display", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.webBrowserLbl.Location = new System.Drawing.Point(86, 25);
            this.webBrowserLbl.Name = "webBrowserLbl";
            this.webBrowserLbl.Size = new System.Drawing.Size(142, 20);
            this.webBrowserLbl.TabIndex = 1;
            this.webBrowserLbl.Text = "Default WebBrowser";
            this.webBrowserLbl.Click += new System.EventHandler(this.webBrowserPanel_Click);
            this.webBrowserLbl.MouseEnter += new System.EventHandler(this.webBrowserPanel_MouseEnter);
            this.webBrowserLbl.MouseLeave += new System.EventHandler(this.webBrowserPanel_MouseLeave);
            // 
            // webBrowserPictureBox
            // 
            this.webBrowserPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.webBrowserPictureBox.Location = new System.Drawing.Point(26, 11);
            this.webBrowserPictureBox.Name = "webBrowserPictureBox";
            this.webBrowserPictureBox.Size = new System.Drawing.Size(48, 48);
            this.webBrowserPictureBox.TabIndex = 0;
            this.webBrowserPictureBox.TabStop = false;
            this.webBrowserPictureBox.Click += new System.EventHandler(this.webBrowserPanel_Click);
            this.webBrowserPictureBox.MouseEnter += new System.EventHandler(this.webBrowserPanel_MouseEnter);
            this.webBrowserPictureBox.MouseLeave += new System.EventHandler(this.webBrowserPanel_MouseLeave);
            // 
            // SelectAppDialogPanel
            // 
            this.Controls.Add(this.webBrowserPanel);
            this.MaximumSize = new System.Drawing.Size(400, 70);
            this.Name = "SelectAppDialogPanel";
            this.Size = new System.Drawing.Size(400, 70);
            this.webBrowserPanel.ResumeLayout(false);
            this.webBrowserPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webBrowserPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        private void webBrowserPanel_Click(object sender, System.EventArgs e)
        {
            SelectAppDialogPanel selectedBrowserPanel = this;

            foreach (KeyValuePair<WindowsHelper.WebBrowser, SelectAppDialogPanel> browserPanel in FormDialogDefaultApp.browserPanels)
            {
                if (browserPanel.Value.Equals(selectedBrowserPanel))
                {
                    browserPanel.Value.IsSelected = true;
                }
                else
                {
                    browserPanel.Value.IsSelected = false;
                }
            }
        }

        private void webBrowserPanel_MouseEnter(object sender, System.EventArgs e)
        {
            if(!IsSelected)
            {
                this.webBrowserPanel.BackColor = ColorTranslator.FromHtml("#0B83DA");
                this.webBrowserLbl.ForeColor = Color.White;
            }
            
        }

        private void webBrowserPanel_MouseLeave(object sender, System.EventArgs e)
        {
            if (!IsSelected)
            {
                this.webBrowserPanel.BackColor = Color.White;
                this.webBrowserLbl.ForeColor = Color.Black;
            }
        }
    }
}
