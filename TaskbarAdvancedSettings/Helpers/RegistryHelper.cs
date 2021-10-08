using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace TaskbarAdvancedSettings.Helpers
{
    public static class RegistryHelper
    {
        public struct RegistryKeyValue
        {
            public RegistryKeyValue(RegistryKey regBase, string regPath, string regKey)
            {
                RegistryBase = regBase;
                RegistryPath = regPath;
                RegistryKey = regKey;
            }

            public RegistryKey RegistryBase { get; }
            public string RegistryPath { get; }
            public string RegistryKey { get; }
        }

        public static RegistryKeyValue TaskbarStyleSwitchRegPath = new RegistryKeyValue(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Shell\Update\Packages", "UndockingDisabled");
        public static RegistryKeyValue TaskbarButtonsCombinationRegPath = new RegistryKeyValue(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarGlomLevel");
        public static RegistryKeyValue TaskbarLockRegPath = new RegistryKeyValue(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSizeMove");
        public static RegistryKeyValue TaskbarSmallButtonsRegPath = new RegistryKeyValue(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSmallIcons");
        public static RegistryKeyValue TaskbarLocationRegPath = new RegistryKeyValue(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3", "Settings");
        public static RegistryKeyValue TaskbarShowClockSecondsRegPath = new RegistryKeyValue(Registry.CurrentUser, @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock");
        public static RegistryKeyValue DesktopContextMenuRegPath = new RegistryKeyValue(Registry.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{e2bf9676-5f8f-435c-97eb-11607a5bedf7}");
        public static RegistryKeyValue AdvSettingsInContextMenuRegPath = new RegistryKeyValue(Registry.ClassesRoot, @"Directory\Background\shell\Advanced settings\Command", "");
        public static RegistryKeyValue AdvSettingsInContextMenuRegParentPath = new RegistryKeyValue(Registry.ClassesRoot, @"Directory\Background\shell\Advanced settings", "");
        public static RegistryKeyValue AdvSettingsFirstRunRegPath = new RegistryKeyValue(Registry.LocalMachine, @"Software\KRtkovo.eu\Advanced Settings for Windows", "FirstRunDone");
        public static RegistryKeyValue AdvSettingsInstallLocationRegPath = new RegistryKeyValue(Registry.LocalMachine, @"Software\KRtkovo.eu\Advanced Settings for Windows", "InstallLocation");
        public static RegistryKeyValue EnvironmentUserPATHregPath = new RegistryKeyValue(Registry.CurrentUser, @"Environment", "Path");

        public static T Read<T>(RegistryKeyValue regPath)
        {
            // Opening the registry key
            RegistryKey rk = regPath.RegistryBase.OpenSubKey(regPath.RegistryPath);
            // Open a subKey as read-only

            if (rk == null)
            {
                return (T)Convert.ChangeType(default(T), typeof(T));
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value or null is returned.        
                    return (T)rk.GetValue(regPath.RegistryKey.ToUpper());
                }
                catch (Exception e)
                {

                    return (T)Convert.ChangeType(default(T), typeof(T));
                }
            }
        }

        public static void Write<T>(RegistryKeyValue regPath, T KeyValue, RegistryValueKind registryValueKind = RegistryValueKind.DWord)
        {
            // Opening the registry key
            RegistryKey rk = regPath.RegistryBase.OpenSubKey(regPath.RegistryPath, true);
            // Open a subKey as read-only

            if (rk == null)
            {
                rk = regPath.RegistryBase.CreateSubKey(regPath.RegistryPath, true);
            }

            if (registryValueKind != RegistryValueKind.DWord)
            {
                rk.SetValue(regPath.RegistryKey, KeyValue, registryValueKind);
            }
            else
            {
                rk.SetValue(regPath.RegistryKey, KeyValue);
            }
        }

        public static void Delete(RegistryKeyValue regPath, bool deleteKey = false)
        {
            // Opening the registry key
            RegistryKey rk = regPath.RegistryBase.OpenSubKey(regPath.RegistryPath, true);
            // Open a subKey as read-only

            if (rk != null)
            {
                try
                {
                    rk.DeleteValue(regPath.RegistryKey);
                }
                catch(Exception)
                {

                }

                if(deleteKey)
                {
                    regPath.RegistryBase.DeleteSubKey(regPath.RegistryPath);
                }
                
            }
        }

    }
}
