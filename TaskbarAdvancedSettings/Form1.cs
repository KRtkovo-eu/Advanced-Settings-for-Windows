using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Management;
using System.Windows.Forms;
using TaskbarAdvancedSettings.Helpers;

namespace TaskbarAdvancedSettings
{
    public partial class Form1 : Form
    {
        private enum TaskbarStyle
        {
            Legacy = 1,
            SunValley = 0
        }
        private TaskbarStyle currentTaskbarStyle;
        private int currentTaskbarButtonCombine, currentLockTaskbar, currentSmallButtons, currentClockSeconds;
        private WindowsHelper.TaskbarPosition currentTaskbarPosition;
        private bool advSettingsInContextMenu;
        private bool runningWindowsLegacy = false; //false = Windows 11, true = Windows 10, Server 2016-2022
        private bool formShown = false;
        private object currentContextMenu;

        private const string DefaultToolLocation = "C:\\Users\\All Users\\KRtkovo.eu\\Advanced Settings for Windows";
        private const string DefaultToolExeName = "advset.exe";

        public Form1()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FirstRun();

            // Visual tweaks
            this.BackColor = ColorTranslator.FromHtml("#f3f3f3");

            // Check program is running on Win11 
            // or Win10, Server 2016, 2019, 2022 in legacy mode
            string windowsVersion = "";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                ManagementObjectCollection information = searcher.Get();
                if (information != null)
                {
                    foreach (ManagementObject obj in information)
                    {
                        windowsVersion = obj["Caption"].ToString() + " - " + obj["OSArchitecture"].ToString();
                    }
                }
                windowsVersion = windowsVersion.Replace("NT 5.1.2600", "XP");
                windowsVersion = windowsVersion.Replace("NT 5.2.3790", "Server 2003");

                if(!windowsVersion.StartsWith("Microsoft Windows 11"))
                {
                    if(windowsVersion.StartsWith("Microsoft Windows 10")
                        || windowsVersion.StartsWith("Microsoft Windows Server 2016")
                        || windowsVersion.StartsWith("Microsoft Windows Server 2019")
                        || windowsVersion.StartsWith("Microsoft Windows Server 2022"))
                    {
                        runningWindowsLegacy = true;
                    }
                    else
                    {
                        MessageBox.Show("Unsupported OS: " + windowsVersion
                            + Environment.NewLine
                            + Environment.NewLine + "Supported operating system is required:"
                            + Environment.NewLine
                            + Environment.NewLine + "Microsoft Windows 10"
                            + Environment.NewLine + "Microsoft Windows 11"
                            + Environment.NewLine + "Microsoft Windows Server 2016"
                            + Environment.NewLine + "Microsoft Windows Server 2019"
                            + Environment.NewLine + "Microsoft Windows Server 2022"
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }

            // Check registry on load
            currentTaskbarStyle = GetCurrentTaskbarStyle();
            currentLockTaskbar = GetCurrentTaskbarLock();
            currentSmallButtons = GetCurrentTaskbarSmallButtons();
            currentTaskbarButtonCombine = RegistryHelper.Read<int>(RegistryHelper.TaskbarButtonsCombinationRegPath);
            taskbarButtonsCombineOption.SelectedIndex = currentTaskbarButtonCombine;
            currentTaskbarPosition = WindowsHelper.GetCurrentTaskbarPosition();
            taskbarPositionComboBox.SelectedIndex = (int)currentTaskbarPosition;
            currentClockSeconds = GetCurrentSecondsClock();
            //currentContextMenu = GetCurrentContextMenu();
            advSettingsInContextMenu = GetAdvSettingsShownInContextMenu();

            // Visual things on load
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, userPictureBox.Width - 3, userPictureBox.Height - 3);
            Region rg = new Region(gp);
            userPictureBox.Region = rg;
            userPictureBox.BackgroundImage = WindowsHelper.GetUserTile(null);
            usernameLbl.Text = Environment.UserName;

            aboutPanel.Dock = DockStyle.Fill;
            //aboutPanel.VerticalScroll.Value = 0;
            aboutPanel.MaximumSize = new Size(1050, int.MaxValue);
            aboutPanel.Visible = false;
            
            startMenuSettingsPanel.Dock = DockStyle.Fill;
            //startMenuSettingsPanel.VerticalScroll.Value = 0;
            startMenuSettingsPanel.MaximumSize = new Size(1050, int.MaxValue);
            startMenuSettingsPanel.Visible = false;

            taskbarSettingsPanel.Dock = DockStyle.Fill;
            //taskbarSettingsPanel.VerticalScroll.Value = 0;
            taskbarSettingsPanel.MaximumSize = new Size(1050, int.MaxValue);

            desktopSettingsPanel.Dock = DockStyle.Fill;
            //desktopSettingsPanel.VerticalScroll.Value = 0;
            desktopSettingsPanel.MaximumSize = new Size(1050, int.MaxValue);
            desktopSettingsPanel.Visible = false;

            extensionsPanel.Dock = DockStyle.Fill;
            //extensionsPanel.VerticalScroll.Value = 0;
            extensionsPanel.MaximumSize = new Size(1050, int.MaxValue);
            extensionsPanel.Visible = false;

            this.Width = 980;
            this.Height = 650;
            this.CenterToScreen();

            panel15.BackColor = ColorTranslator.FromHtml("#eaeaea");

            aboutVersionLbl.Text = "Compiled " + (Environment.Is64BitOperatingSystem ? "x64" : "x86") + " version: " + Application.ProductVersion;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            formShown = true;
        }

        private void taskbarStyle_btn_Click(object sender, EventArgs e)
        {
            if(currentTaskbarStyle == TaskbarStyle.SunValley)
            {
                if(MessageBox.Show("This action should only be performed by an experienced user." +
                                   Environment.NewLine + Environment.NewLine +  "Disabling the Sun Valley taskbar style will not allow you to take advantage of the latest Windows features." +
                                   Environment.NewLine + Environment.NewLine +
                                   "Are you sure you want to continue?", "Before you switch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            // Set taskbar style to the other option
            RegistryHelper.Write(
                RegistryHelper.TaskbarStyleSwitchRegPath,
                ((currentTaskbarStyle == TaskbarStyle.SunValley) ? (int)TaskbarStyle.Legacy : (int)TaskbarStyle.SunValley)
            );

            // Refresh state
            currentTaskbarStyle = GetCurrentTaskbarStyle();
            GetCurrentTaskbarLock();
            GetCurrentTaskbarSmallButtons();
            currentClockSeconds = GetCurrentSecondsClock();

            // Restart explorer.exe
            WindowsHelper.RestartLegacyExplorer();

            MessageBox.Show("Please wait." + Environment.NewLine + "It can take few seconds until the style is fully loaded.", "Style applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private TaskbarStyle GetCurrentTaskbarStyle()
        {
            TaskbarStyle CurrentTaskbarStyle;

            if(!runningWindowsLegacy)
            {
                var curStyle = RegistryHelper.Read<object>(RegistryHelper.TaskbarStyleSwitchRegPath);
                CurrentTaskbarStyle = (TaskbarStyle)(curStyle == null ? TaskbarStyle.SunValley : curStyle);
            }
            else
            {
                CurrentTaskbarStyle = TaskbarStyle.Legacy;
                panel28.Enabled = false;
                taskbarSunValleyPanel.Enabled = false;

                legacyStartMenuLbl.Text = "On";
                legacyStartMenuBtn.BackgroundImage = Properties.Resources.switchOnStateDisabled;
            }


            if (CurrentTaskbarStyle == TaskbarStyle.SunValley)
            {
                taskbarStyle_lbl.Text = "On";
                taskbarStyle_btn.BackgroundImage = runningWindowsLegacy ? Properties.Resources.switchOnStateDisabled : Properties.Resources.switchOnState;
                pictureBox1.BackgroundImage = runningWindowsLegacy ? Properties.Resources.winDisabled : Properties.Resources.win;

                startMenuSunValleyLbl.Text = "On";
                startMenuSunValleyBtn.BackgroundImage = Properties.Resources.switchOnStateDisabled;

                legacyTaskbarLbl.Text = "Off";
                legacyTaskbarBtn.BackgroundImage = runningWindowsLegacy ? Properties.Resources.switchOffStateDisabled : Properties.Resources.switchOffState;
                pictureBox13.BackgroundImage = runningWindowsLegacy ? Properties.Resources.winLegacyDisabled : Properties.Resources.winLegacy;

                legacyContextMenuLbl.Text = "Off";
                legacyContextMenuBtn.BackgroundImage = Properties.Resources.switchOffStateDisabled;

                notificationAreaPanel.Enabled = false;
                behaviorsPanel.Enabled = false;
            }
            else
            {
                taskbarStyle_lbl.Text = "Off";
                taskbarStyle_btn.BackgroundImage = runningWindowsLegacy ? Properties.Resources.switchOffStateDisabled : Properties.Resources.switchOffState;
                pictureBox1.BackgroundImage = runningWindowsLegacy ? Properties.Resources.winDisabled : Properties.Resources.win;

                startMenuSunValleyLbl.Text = "Off";
                startMenuSunValleyBtn.BackgroundImage = Properties.Resources.switchOffStateDisabled;

                legacyTaskbarLbl.Text = "On";
                legacyTaskbarBtn.BackgroundImage = runningWindowsLegacy ? Properties.Resources.switchOnStateDisabled : Properties.Resources.switchOnState;
                pictureBox13.BackgroundImage = runningWindowsLegacy ? Properties.Resources.winLegacyDisabled : Properties.Resources.winLegacy;

                legacyContextMenuLbl.Text = "On";
                legacyContextMenuBtn.BackgroundImage = Properties.Resources.switchOnStateDisabled;

                notificationAreaPanel.Enabled = true;
                behaviorsPanel.Enabled = true;
            }

            return CurrentTaskbarStyle;
        }

        private void showNotificationIconsSettings(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "\"shell:::{05d7b0f4-2121-4eff-bf6b-ed3f69b894d9}\"");
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Open-Shell/Open-Shell-Menu/releases/latest");
        }

        private void taskbarButtonsCombineOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistryHelper.Write(RegistryHelper.TaskbarButtonsCombinationRegPath, taskbarButtonsCombineOption.SelectedIndex);
            if (formShown) WindowsHelper.RefreshLegacyExplorer();
            currentTaskbarButtonCombine = taskbarButtonsCombineOption.SelectedIndex;
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "\"shell:::{05d7b0f4-2121-4eff-bf6b-ed3f69b894d9}\\SystemIcons\"");
        }

        // Visual tweaks
        private void drawPanelBorder(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            

            panel.BackColor = ColorTranslator.FromHtml("#f3f3f3");

            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int nBorderSize = 1;
            int nRadius = 10;
            
            using (GraphicsPath gp = CreatePath(new Rectangle(nBorderSize, nBorderSize, panel.Width - nBorderSize * 2, panel.Height - nBorderSize * 2), nRadius, true))
            {
                Pen pen = new Pen(ColorTranslator.FromHtml("#e5e5e5"), nBorderSize);
                pen.LineJoin = LineJoin.Round;
                e.Graphics.Clear(SystemColors.Control);
                e.Graphics.FillPath(new SolidBrush(ColorTranslator.FromHtml("#fbfbfb")), gp);
                e.Graphics.DrawPath(pen, gp);
            }
        }
        private void drawOrangePanelBorder(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;


            panel.BackColor = ColorTranslator.FromHtml("#fbfbfb");

            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int nBorderSize = 1;
            int nRadius = 10;

            using (GraphicsPath gp = CreatePath(new Rectangle(nBorderSize, nBorderSize, panel.Width - nBorderSize * 2, panel.Height - nBorderSize * 2), nRadius, true))
            {
                Pen pen = new Pen(Color.OrangeRed, nBorderSize);
                pen.LineJoin = LineJoin.Round;
                e.Graphics.Clear(SystemColors.Control);
                e.Graphics.FillPath(new SolidBrush(ColorTranslator.FromHtml("#fbfbfb")), gp);
                e.Graphics.DrawPath(pen, gp);
            }
        }
        private void drawBluePanelBorder(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;


            panel.BackColor = ColorTranslator.FromHtml("#fbfbfb");

            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int nBorderSize = 1;
            int nRadius = 10;

            using (GraphicsPath gp = CreatePath(new Rectangle(nBorderSize, nBorderSize, panel.Width - nBorderSize * 2, panel.Height - nBorderSize * 2), nRadius, true))
            {
                Pen pen = new Pen(Color.RoyalBlue, nBorderSize);
                pen.LineJoin = LineJoin.Round;
                e.Graphics.Clear(SystemColors.Control);
                e.Graphics.FillPath(new SolidBrush(ColorTranslator.FromHtml("#fbfbfb")), gp);
                e.Graphics.DrawPath(pen, gp);
            }
        }

        private void drawGreenPanelBorder(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;


            panel.BackColor = ColorTranslator.FromHtml("#fbfbfb");

            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int nBorderSize = 1;
            int nRadius = 10;

            using (GraphicsPath gp = CreatePath(new Rectangle(nBorderSize, nBorderSize, panel.Width - nBorderSize * 2, panel.Height - nBorderSize * 2), nRadius, true))
            {
                Pen pen = new Pen(Color.SeaGreen, nBorderSize);
                pen.LineJoin = LineJoin.Round;
                e.Graphics.Clear(SystemColors.Control);
                e.Graphics.FillPath(new SolidBrush(ColorTranslator.FromHtml("#fbfbfb")), gp);
                e.Graphics.DrawPath(pen, gp);
            }
        }
        public static GraphicsPath CreatePath(Rectangle rect, int nRadius, bool bOutline)
        {
            int nShift = bOutline ? 1 : 0;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X + nShift, rect.Y, nRadius, nRadius, 180f, 90f);
            path.AddArc((rect.Right - nRadius) - nShift, rect.Y, nRadius, nRadius, 270f, 90f);
            path.AddArc((rect.Right - nRadius) - nShift, (rect.Bottom - nRadius) - nShift, nRadius, nRadius, 0f, 90f);
            path.AddArc(rect.X + nShift, (rect.Bottom - nRadius) - nShift, nRadius, nRadius, 90f, 90f);
            path.CloseFigure();
            return path;
        }
        protected override bool ShowFocusCues => false;

        private void lockTaskbarBtn_Click(object sender, EventArgs e)
        {
            var updatedValue = (currentLockTaskbar == 0 ? 1 : 0);
            RegistryHelper.Write(RegistryHelper.TaskbarLockRegPath, updatedValue);
            currentLockTaskbar = GetCurrentTaskbarLock();
            if (formShown) WindowsHelper.RefreshLegacyExplorer();
        }

        private void smallButtonsBtn_Click(object sender, EventArgs e)
        {
            var updatedValue = (currentSmallButtons == 0 ? 1 : 0);
            RegistryHelper.Write(RegistryHelper.TaskbarSmallButtonsRegPath, updatedValue);
            currentSmallButtons = GetCurrentTaskbarSmallButtons();
            if (formShown) WindowsHelper.RefreshLegacyExplorer();
        }

        private void taskbarPositionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            WindowsHelper.SetTaskbarPosition((WindowsHelper.TaskbarPosition)taskbarPositionComboBox.SelectedIndex);
            currentTaskbarPosition = (WindowsHelper.TaskbarPosition)taskbarPositionComboBox.SelectedIndex;
            if(formShown) WindowsHelper.RestartLegacyExplorer();
        }

        private void showSecondsBtn_Click(object sender, EventArgs e)
        {
            var updatedValue = (currentClockSeconds == 0 ? 1 : 0);
            RegistryHelper.Write(RegistryHelper.TaskbarShowClockSecondsRegPath, updatedValue);
            currentClockSeconds = GetCurrentSecondsClock();
            if (formShown) WindowsHelper.RestartLegacyExplorer();
        }

        private int GetCurrentTaskbarLock() {
            int current = RegistryHelper.Read<int>(RegistryHelper.TaskbarLockRegPath);

            if(current == 0)
            {
                lockTaskbarLbl.Text = "On";
                lockTaskbarBtn.BackgroundImage = (currentTaskbarStyle == TaskbarStyle.Legacy) ? Properties.Resources.switchOnState : Properties.Resources.switchOnStateDisabled;
            }
            else
            {
                lockTaskbarLbl.Text = "Off";
                lockTaskbarBtn.BackgroundImage = (currentTaskbarStyle == TaskbarStyle.Legacy) ? Properties.Resources.switchOffState : Properties.Resources.switchOffStateDisabled;
            }

            return current;
        }

        private void panel15_MouseLeave(object sender, EventArgs e)
        {
            if (!taskbarSettingsPanel.Visible)
            {
                panel15.BackColor = Color.Transparent;
            }
        }

        private void panel15_MouseEnter(object sender, EventArgs e)
        {
            panel15.BackColor = ColorTranslator.FromHtml("#eaeaea");
        }

        private void panel15_Click(object sender, EventArgs e)
        {
            aboutPanel.Visible = false;
            startMenuSettingsPanel.Visible = false;
            taskbarSettingsPanel.Visible = true;
            desktopSettingsPanel.Visible = false;
            extensionsPanel.Visible = false;
            panel15.BackColor = ColorTranslator.FromHtml("#eaeaea");
            panel16.BackColor = Color.Transparent;
            panel21.BackColor = Color.Transparent;
            panel17.BackColor = Color.Transparent;
            panel29.BackColor = Color.Transparent;
            //taskbarSettingsPanel.VerticalScroll.Value = 0;
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            panel16.BackColor = ColorTranslator.FromHtml("#eaeaea");
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            if (!startMenuSettingsPanel.Visible)
            {
                panel16.BackColor = Color.Transparent;
            }
        }

        private void panel16_Click(object sender, EventArgs e)
        {
            aboutPanel.Visible = false;
            startMenuSettingsPanel.Visible = true;
            taskbarSettingsPanel.Visible = false;
            desktopSettingsPanel.Visible = false;
            extensionsPanel.Visible = false;
            panel15.BackColor = Color.Transparent;
            panel16.BackColor = ColorTranslator.FromHtml("#eaeaea");
            panel21.BackColor = Color.Transparent;
            panel17.BackColor = Color.Transparent;
            panel29.BackColor = Color.Transparent;
        }

        private void label39_Click(object sender, EventArgs e)
        {
            aboutPanel.Visible = false;
            startMenuSettingsPanel.Visible = false;
            taskbarSettingsPanel.Visible = false;
            desktopSettingsPanel.Visible = true;
            extensionsPanel.Visible = false;
            panel15.BackColor = Color.Transparent;
            panel16.BackColor = Color.Transparent;
            panel21.BackColor = ColorTranslator.FromHtml("#eaeaea");
            panel17.BackColor = Color.Transparent;
            panel29.BackColor = Color.Transparent;
        }

        private void panel21_MouseEnter(object sender, EventArgs e)
        {
            panel21.BackColor = ColorTranslator.FromHtml("#eaeaea");
        }

        private void panel21_MouseLeave(object sender, EventArgs e)
        {
            if (!desktopSettingsPanel.Visible)
            {
                panel21.BackColor = Color.Transparent;
            }
        }

        private void panel17_MouseEnter(object sender, EventArgs e)
        {
            panel17.BackColor = ColorTranslator.FromHtml("#eaeaea");
        }

        private void panel17_MouseLeave(object sender, EventArgs e)
        {
            if (!aboutPanel.Visible)
            {
                panel17.BackColor = Color.Transparent;
            }
        }


        private void panel29_MouseEnter(object sender, EventArgs e)
        {
            panel29.BackColor = ColorTranslator.FromHtml("#eaeaea");
        }

        private void panel29_MouseLeave(object sender, EventArgs e)
        {
            if (!extensionsPanel.Visible)
            {
                panel29.BackColor = Color.Transparent;
            }
        }

        private void panel29_Click(object sender, EventArgs e)
        {
            aboutPanel.Visible = false;
            startMenuSettingsPanel.Visible = false;
            taskbarSettingsPanel.Visible = false;
            desktopSettingsPanel.Visible = false;
            extensionsPanel.Visible = true;
            panel15.BackColor = Color.Transparent;
            panel16.BackColor = Color.Transparent;
            panel21.BackColor = Color.Transparent;
            panel17.BackColor = Color.Transparent;
            panel29.BackColor = ColorTranslator.FromHtml("#eaeaea");
            //extensionsPanel.VerticalScroll.Value = 0;
        }

        private void panel17_Click(object sender, EventArgs e)
        {
            aboutPanel.Visible = true;
            startMenuSettingsPanel.Visible = false;
            taskbarSettingsPanel.Visible = false;
            desktopSettingsPanel.Visible = false;
            extensionsPanel.Visible = false;
            panel15.BackColor = Color.Transparent;
            panel16.BackColor = Color.Transparent;
            panel21.BackColor = Color.Transparent;
            panel17.BackColor = ColorTranslator.FromHtml("#eaeaea");
            panel29.BackColor = Color.Transparent;
        }

        private void legacyContextMenuBtn_Click(object sender, EventArgs e)
        {
            if(RegistryHelper.Read<object>(RegistryHelper.DesktopContextMenuRegPath) == null)
            {
                RegistryHelper.Write<object>(RegistryHelper.DesktopContextMenuRegPath, "", Microsoft.Win32.RegistryValueKind.String);
            }
            else
            {
                RegistryHelper.Delete(RegistryHelper.DesktopContextMenuRegPath, true);
            }

            currentContextMenu = GetCurrentContextMenu();

            // Restart explorer.exe
            WindowsHelper.RefreshWindowsExplorer();
        }

        private void advSettingsInContextMenuBtn_Click(object sender, EventArgs e)
        {
            if(advSettingsInContextMenu)
            {
                RegistryHelper.Delete(RegistryHelper.AdvSettingsInContextMenuRegPath, true);
                RegistryHelper.Delete(RegistryHelper.AdvSettingsInContextMenuRegParentPath, true);
            }
            else {
                RegistryHelper.Write<string>(RegistryHelper.AdvSettingsInContextMenuRegPath, DefaultToolLocation + "\\" + DefaultToolExeName);
            }

            advSettingsInContextMenu = GetAdvSettingsShownInContextMenu();

            // Refresh explorer.exe
            if (formShown) WindowsHelper.RefreshWindowsExplorer();
        }

        private int GetCurrentTaskbarSmallButtons()
        {
            int current = RegistryHelper.Read<int>(RegistryHelper.TaskbarSmallButtonsRegPath);

            if (current == 1)
            {
                smallButtonsLbl.Text = "On";
                smallButtonsBtn.BackgroundImage = (currentTaskbarStyle == TaskbarStyle.Legacy) ? Properties.Resources.switchOnState : Properties.Resources.switchOnStateDisabled;
            }
            else
            {
                smallButtonsLbl.Text = "Off";
                smallButtonsBtn.BackgroundImage = (currentTaskbarStyle == TaskbarStyle.Legacy) ? Properties.Resources.switchOffState : Properties.Resources.switchOffStateDisabled;
            }

            return current;
        }

        private int GetCurrentSecondsClock()
        {
            int current = RegistryHelper.Read<int>(RegistryHelper.TaskbarShowClockSecondsRegPath);

            if (current == 1)
            {
                showSecondsLbl.Text = "On";
                showSecondsBtn.BackgroundImage = (currentTaskbarStyle == TaskbarStyle.Legacy) ? Properties.Resources.switchOnState : Properties.Resources.switchOnStateDisabled;
            }
            else
            {
                showSecondsLbl.Text = "Off";
                showSecondsBtn.BackgroundImage = (currentTaskbarStyle == TaskbarStyle.Legacy) ? Properties.Resources.switchOffState : Properties.Resources.switchOffStateDisabled;
            }

            return current;
        }

        private void userPictureBox_Click(object sender, EventArgs e)
        {
            WindowsHelper.ShowStartMenu();
        }

        private void label47_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/dremin/RetroBar/releases/latest");
        }

        private void label48_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/cairoshell/cairoshell/releases/latest");
        }

        private void label52_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.startallback.com");
        }

        private void label70_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.stardock.com/products/start11/");
        }

        private void label75_Click(object sender, EventArgs e)
        {
            Process.Start("https://winaerotweaker.com");
        }

        private void label80_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/microsoft/PowerToys/releases/latest");
        }

        private void label84_Click(object sender, EventArgs e)
        {
            Process.Start("https://rammichael.com/7-taskbar-tweaker");
        }

        private object GetCurrentContextMenu()
        {
            object current = RegistryHelper.Read<object>(RegistryHelper.DesktopContextMenuRegPath);

            if (current != null)
            {
                legacyContextMenuLbl.Text = "On";
                legacyContextMenuBtn.BackgroundImage = Properties.Resources.switchOnStateDisabled;
            }
            else
            {
                legacyContextMenuLbl.Text = "Off";
                legacyContextMenuBtn.BackgroundImage = Properties.Resources.switchOffStateDisabled;
            }

            return current;
        }

        private void label30_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string latestVersion = NetworkHelper.CheckUpdate();
            this.Cursor = Cursors.Default;

            if (latestVersion == Application.ProductVersion)
            {
                label30.Text = "Tool is up to date";
            }
            else
            {
                if(MessageBox.Show("Do you want to download the new version now?", "Update is available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(NetworkHelper.AdvSettingsGithubLink);
                }
            }
        }

        private bool GetAdvSettingsShownInContextMenu()
        {
            string current = RegistryHelper.Read<string>(RegistryHelper.AdvSettingsInContextMenuRegPath);
            bool currentVal = (current == null || current == "") ? false : true;

            if(currentVal)
            {
                advSettingsInContextMenuLbl.Text = "On";
                advSettingsInContextMenuBtn.BackgroundImage = Properties.Resources.switchOnState;
            }
            else
            {
                advSettingsInContextMenuLbl.Text = "Off";
                advSettingsInContextMenuBtn.BackgroundImage = Properties.Resources.switchOffState;
            }

            return currentVal;
        }

        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;

        }

        private void FirstRun()
        {
            int isFirstRunDone = RegistryHelper.Read<int>(RegistryHelper.AdvSettingsFirstRunRegPath);
            if(isFirstRunDone == 0)
            {
                // Install tool
                string currentFileName = Process.GetCurrentProcess().MainModule.FileName;
                Directory.CreateDirectory(DefaultToolLocation);
                File.Copy(currentFileName, DefaultToolLocation + "\\" + DefaultToolExeName, true);
                RegistryHelper.Write(RegistryHelper.AdvSettingsInstallLocationRegPath, DefaultToolLocation + "\\" + DefaultToolExeName);

                // Update environment variable PATH with tool path
                string currentUserPATH = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.User);
                string newUserPATH = currentUserPATH + (currentUserPATH.EndsWith(";") ? "" : ";") + DefaultToolLocation + ";";
                RegistryHelper.Write(RegistryHelper.EnvironmentUserPATHregPath, newUserPATH);

                RegistryHelper.Write(RegistryHelper.AdvSettingsFirstRunRegPath, 1);
            }
        }
    }
}
