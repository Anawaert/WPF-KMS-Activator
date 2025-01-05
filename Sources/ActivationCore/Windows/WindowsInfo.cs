using System;
using Microsoft.Win32;

namespace Activator
{
    public static class WindowsInfo
    {
        public static WindowsProductName WindowsProduct { get; private set; }

        public static bool IsWindowsActivated { get; private set; }

        public static bool IsReleaseVersion { get; private set; }

        private static void CheckWindowsActivation()
        {
            try
            {
                string? output = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//NoLogo slmgr.vbs /dli",
                    Shared.System32Path,
                    true
                );
                IsWindowsActivated = (output?.Contains("license status: licensed") ?? false) ||
                                     (output?.Contains("已授权") ?? false);
            }
            catch
            {
                // 写入日志
            }
        }

        private static void GetWindowsVersion()
        {
            try
            {
                string? productNameFromRegistry = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
                                                                    "ProductName",
                                                                    null)?.ToString();
                if (productNameFromRegistry is null)
                {
                    WindowsProduct = WindowsProductName.Unsupported;
                }
                else
                {
                    WindowsProduct = WindowsProductName.Unsupported;
                    foreach (WindowsProductName product in Enum.GetValues(typeof(WindowsProductName)))
                    {
                        if (productNameFromRegistry.Contains(product.ToString().Replace("_", " ")))
                        {
                            WindowsProduct = product;
                            break;
                        }
                    }
                }
            }
            catch
            {
                //   写入日志
            }
        }

        private static void CheckWindowsIsReleaseVersion()
        {
            try
            {
                string? productNameFromRegistry = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
                                                                    "ProductName",
                                                                    null)?.ToString();
                if (productNameFromRegistry is null)
                {
                    IsReleaseVersion = false;
                }
                else
                {
                    IsReleaseVersion = !productNameFromRegistry.ToUpper().Contains("EVAL");
                }
            }
            catch
            {
                // 写入日志
            }
        }

        public static void InitializeWindowsInfo()
        {
            GetWindowsVersion();
            CheckWindowsActivation();
            CheckWindowsIsReleaseVersion();
        }

        public static void RefreshWindowsInfo() => InitializeWindowsInfo();
    }
}
