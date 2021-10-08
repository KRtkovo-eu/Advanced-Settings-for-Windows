using System;
#if !DEBUG
using System.Diagnostics;
#endif
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TaskbarAdvancedSettings.Helpers
{
    public static class WindowsHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResult);

        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);
        private const int WM_SETTINGCHANGE = 0x1a;
        private const int SMTO_ABORTIFHUNG = 0x0002;


        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_TASKLIST = 0xF130;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int WM_SYSCOMMAND, int SC_TASKLIST, int param);
        
        /// <summary>
        /// Soft restart or reload of Windows taskbar
        /// </summary>
        public static void RefreshLegacyExplorer()
        {
            // Refresh explorer.exe to commit changes in registry
            SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, IntPtr.Zero, "TraySettings", SMTO_ABORTIFHUNG, 100, IntPtr.Zero);
        }

        /// <summary>
        /// Hard restart of Windows Explorer
        /// </summary>
        public static void RestartLegacyExplorer()
        {
            // Restart explorer.exe
#if !DEBUG
            Process.Start(@"taskkill.exe", @"/F /IM explorer.exe");
            System.Threading.Thread.Sleep(1000);
            Process.Start("explorer.exe");
#endif
        }

        /// <summary>
        /// Soft restart or reload Windows Explorer
        /// </summary>
        public static void RefreshWindowsExplorer()
        {

            // based on http://stackoverflow.com/questions/2488727/refresh-windows-explorer-in-win7
            Guid CLSID_ShellApplication = new Guid("13709620-C279-11CE-A49E-444553540000");
            Type shellApplicationType = Type.GetTypeFromCLSID(CLSID_ShellApplication, true);

            object shellApplication = Activator.CreateInstance(shellApplicationType);
            object windows = shellApplicationType.InvokeMember("Windows", System.Reflection.BindingFlags.InvokeMethod, null, shellApplication, new object[] { });

            Type windowsType = windows.GetType();
            object count = windowsType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, windows, null);
            for (int i = 0; i < (int)count; i++)
            {
                object item = windowsType.InvokeMember("Item", System.Reflection.BindingFlags.InvokeMethod, null, windows, new object[] { i });
                Type itemType = item.GetType();

                // only refresh windows explorers
                string itemName = (string)itemType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, null, item, null);
                if ((itemName == "Windows Explorer") || (itemName == "File Explorer"))
                {
                    itemType.InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
                }
            }
        }

        [DllImport("shell32.dll", EntryPoint = "#261",
                   CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
          string username,
          UInt32 whatever, // 0x80000000
          StringBuilder picpath, int maxLength);

        public static string GetUserTilePath(string username)
        {   // username: use null for current user
            var sb = new StringBuilder(1000);
            GetUserTilePath(username, 0x80000000, sb, sb.Capacity);
            return sb.ToString();
        }

        public static Image GetUserTile(string username)
        {
            return Image.FromFile(GetUserTilePath(username));
        }

        public enum TaskbarPosition
        {
            Left = 0,
            Top = 1,
            Right = 2,
            Bottom = 3
        }

        public static byte[] TaskbarPositionLeft = new byte[]
        {
            0x30, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0xFF, 0xFF,
            0x02, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x3E, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xFB, 0x03, 0x00, 0x00,
            0x00, 0x0F, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00,
            0x60, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00
        };
        public static byte[] TaskbarPositionTop = new byte[]
        {
            0x30, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0xFF, 0xFF,
            0x02, 0x14, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x3E, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xFB, 0x03, 0x00, 0x00,
            0x00, 0x0F, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00,
            0x60, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00
        };
        public static byte[] TaskbarPositionRight = new byte[]
        {
            0x30, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0xFF, 0xFF,
            0x02, 0x14, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0x3E, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xFB, 0x03, 0x00, 0x00,
            0x00, 0x0F, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00,
            0x60, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00
        };
        public static byte[] TaskbarPositionBottom = new byte[]
        {
            0x30, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0xFF, 0xFF,
            0x02, 0x14, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00,
            0x3E, 0x00, 0x00, 0x00, 0x3D, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xFB, 0x03, 0x00, 0x00,
            0x00, 0x0F, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00,
            0x60, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00
        };

        public static void SetTaskbarPosition(TaskbarPosition taskbarPosition)
        {
            byte[] taskbarPositionValue;

            //00 Left, 01 Top, 02 Right, 03 Bottom
            switch (taskbarPosition)
            {
                case TaskbarPosition.Left:
                    taskbarPositionValue = TaskbarPositionLeft;
                    break;
                case TaskbarPosition.Top:
                    taskbarPositionValue = TaskbarPositionTop;
                    break;
                case TaskbarPosition.Right:
                    taskbarPositionValue = TaskbarPositionRight;
                    break;
                case TaskbarPosition.Bottom:
                default:
                    taskbarPositionValue = TaskbarPositionBottom;
                    break;
            }

            RegistryHelper.Write(RegistryHelper.TaskbarLocationRegPath, taskbarPositionValue);
        }

        public static TaskbarPosition GetCurrentTaskbarPosition()
        {
            byte[] currentPosition = RegistryHelper.Read<byte[]>(RegistryHelper.TaskbarLocationRegPath);

            if(currentPosition.SequenceEqual(TaskbarPositionLeft))
            {
                return TaskbarPosition.Left;
            }
            else if (currentPosition.SequenceEqual(TaskbarPositionTop))
            {
                return TaskbarPosition.Top;
            }
            else if (currentPosition.SequenceEqual(TaskbarPositionRight))
            {
                return TaskbarPosition.Right;
            }
            else if (currentPosition.SequenceEqual(TaskbarPositionBottom))
            {
                return TaskbarPosition.Bottom;
            }

            return TaskbarPosition.Bottom;
        }

        public static void ShowStartMenu()
        {
            Task.Run(() => SendMessageTimeout(HWND_BROADCAST, WM_SYSCOMMAND, new IntPtr(SC_TASKLIST), null, 0, 0, IntPtr.Zero));

        }
    }
}
