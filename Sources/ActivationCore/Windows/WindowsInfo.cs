using System;
using Microsoft.Win32;
using Serilog;

namespace Activator
{
    /// <summary>
    /// <para>该静态类包含了一系列与 Windows 系统信息相关的属性与方法</para>
    /// <para>This static class contains a series of properties and methods related to Windows system information</para>
    /// </summary>
    public static class WindowsInfo
    {
        /// <summary>
        /// <para>Windows 系统的产品名称</para>
        /// <para>The product name of the Windows system</para>
        /// </summary>
        public static WindowsProductName WindowsProduct { get; private set; }

        /// <summary>
        /// <para>Windows 系统是否已激活</para>
        /// <para>Whether the Windows system is activated</para>
        /// </summary>
        public static bool IsWindowsActivated { get; private set; }

        /// <summary>
        /// <para>Windows 系统是否为正式版本而非评估版本（主要针对 Windows Server）</para>
        /// <para>Whether the Windows system is a release version rather than an evaluation version (mainly for Windows Server)</para>
        /// </summary>
        public static bool IsReleaseVersion { get; private set; }

        /// <summary>
        /// <para>当 Windows 系统的产品名称发生变化时引发的事件</para>
        /// <para>此事件主要用于在产品名称发生变化时通知其它对象获取更新</para>
        /// <para>** 未来将逐步改为更加完善的事件机制 **</para> 
        /// <para>The event that is raised when the product name of the Windows system changes</para>
        /// <para>This event is mainly used to notify other objects to get updated when the product name changes</para>
        /// <para>** Will gradually be replaced by a more complete event mechanism in the future **</para>
        /// </summary>
        public static event Action<WindowsProductName>? WindowsProductChanged;

        /// <summary>
        /// <para>检查 Windows 系统的激活状态</para>
        /// <para>Check the activation status of the Windows system</para>
        /// </summary>
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
                Log.Logger.Information("Windows activation status checked: {IsWindowsActivated}", IsWindowsActivated);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to check Windows activation status");
            }
        }

        /// <summary>
        /// <para>获取 Windows 系统的产品名称</para>
        /// <para>Get the product name of the Windows system</para>
        /// </summary>
        private static void GetWindowsVersion()
        {
            try
            {
                // 从注册表中获取 Windows 系统的产品名称
                // Get the product name of the Windows system from the registry
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
                        if (productNameFromRegistry.Contains(product.ToString().Replace("_Point_", ".").Replace("_", " ")))
                        {
                            WindowsProduct = product;
                            break;
                        }
                    }
                }
                Log.Logger.Information("Got Windows product name: {WindowsProduct} from registry value: {productNameFromRegistry}", WindowsProduct, productNameFromRegistry);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to get Windows product name");
            }
        }

        /// <summary>
        /// <para>检查 Windows 系统是否为正式版本而非评估版</para>
        /// <para>Check whether the Windows system is a release version rather than an evaluation version</para>
        /// </summary>
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
                Log.Logger.Information("Checked Windows release version : {IsReleaseVersion} with product name: {productNameFromRegistry}", IsReleaseVersion, productNameFromRegistry);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to check Windows release version");
            }
        }

        /// <summary>
        /// <para>初始化 Windows 系统信息</para>
        /// <para>Initialize Windows system information</para>
        /// </summary>
        public static void InitializeWindowsInfo()
        {
            GetWindowsVersion();
            CheckWindowsActivation();
            CheckWindowsIsReleaseVersion();
            // 触发 Windows 系统产品名称变化事件以通知其它对象
            // Trigger the Windows system product name change event to notify other objects
            WindowsProductChanged?.Invoke(WindowsProduct);
        }

        /// <summary>
        /// <para>刷新 Windows 系统信息，效果同初始化信息</para>
        /// <para>Refresh Windows system information, the same effect as initializing information</para>
        /// </summary>
        public static void RefreshWindowsInfo() => InitializeWindowsInfo();
    }
}
