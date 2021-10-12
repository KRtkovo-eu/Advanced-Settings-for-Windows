using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TaskbarAdvancedSettings.Helpers;

namespace TaskbarAdvancedSettings
{
    public partial class FormDialogDefaultApp : Form
    {
        private List<WindowsHelper.WebBrowser> webBrowsers = WindowsHelper.GetInstalledWebBrowser();
        internal static List<KeyValuePair<WindowsHelper.WebBrowser, SelectAppDialogPanel>> browserPanels = new List<KeyValuePair<WindowsHelper.WebBrowser, SelectAppDialogPanel>>();

        public FormDialogDefaultApp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var browser = browserPanels.Find(x => x.Value.IsSelected).Key;
            WindowsHelper.SetDefaultWebBrowser(browser);
            Form1.DefaultWebBrowserIcon = browser.BrowserInfo.IconSmall;
            this.Close();
        }

        private void FormDialogDefaultApp_Load(object sender, EventArgs e)
        {
            // set default browser
            WindowsHelper.WebBrowser defaultWebBrowser = webBrowsers.Find(x => x.IsDefault);
            this.defaultWebBrowser.SetBrowserName(defaultWebBrowser.BrowserInfo.ProductName);
            this.defaultWebBrowser.SetBrowserIcon(defaultWebBrowser.BrowserInfo.Icon);
            this.defaultWebBrowser.IsSelected = true;
            browserPanels.Add(new KeyValuePair<WindowsHelper.WebBrowser, SelectAppDialogPanel>(defaultWebBrowser, this.defaultWebBrowser));

            // load all available browsers
            int yPos = 225;
            foreach(WindowsHelper.WebBrowser webBrowser in webBrowsers)
            {
                if(!webBrowser.IsDefault)
                {
                    var browserPanel = new SelectAppDialogPanel(webBrowser);
                    browserPanel.IsSelected = false;
                    browserPanel.Location = new Point(1, yPos);

                    this.Controls.Add(browserPanel);
                    browserPanels.Add(new KeyValuePair<WindowsHelper.WebBrowser, SelectAppDialogPanel>(webBrowser, browserPanel));

                    yPos += browserPanel.Height;
                    this.Height += browserPanel.Height;
                }
            }

            this.CenterToScreen();
            //this.Location = new Point(this.Location.X, this.Location.Y + 30);
        }

        private void FormDialogDefaultApp_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, new Rectangle(0, 0, this.Width, this.Height), Color.Black, ButtonBorderStyle.Solid);
        }
    }
}
