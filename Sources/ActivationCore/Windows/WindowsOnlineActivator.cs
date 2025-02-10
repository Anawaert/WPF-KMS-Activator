using System.IO;
using System.Diagnostics;
using Serilog;
using System;

namespace Activator
{
    /// <summary>
    /// <para>Windows 在线激活对象</para>
    /// <para>Windows online activator object</para>
    /// </summary>
    public class WindowsOnlineActivator : IWindowsOnlineActivator
    {
        /// <summary>
        /// <para>获取 Windows 产品的对应批量激活密钥</para>
        /// <para>Get the corresponding volume activation key of the Windows product</para>
        /// </summary>
        /// <param name="windowsProduct">
        /// <para>需要激活的 Windows 产品名称</para>
        /// <para>The name of the Windows product that needs to be activated</para>
        /// </param>
        /// <returns>
        /// <para>返回对应的激活密钥</para>
        /// <para>Return the corresponding activation key</para>
        /// </returns>
        public string? GetActivationKey(WindowsProductName windowsProduct)
        {
            try
            {
                string? key = WindowsActivationKey.OnlineActivationKey[windowsProduct];
                Log.Logger.Information("Successfully got the online activation key: {key} of Windows product: {product}", key, windowsProduct);
                return key;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to get the online activation key of Windows product: {product}", windowsProduct);
                return null;
            }
        }

        /// <summary>
        /// <para>将 Windows 产品转换为对应的批量激活版本，若当前系统已为正式版，则不会有任何动作</para>
        /// <para>Convert the Windows product to the corresponding volume activation version, if the current system is already a release version, there will be no action</para>
        /// </summary>
        /// <param name="targetWindowsProduct">
        /// <para>需要转换的目标 Windows 产品名称</para>
        /// <para>The target Windows product name to be converted</para>
        /// </param>
        /// <param name="isReleaseVersionCurrent">
        /// <para>当前系统的版本是否已经为正式版本</para>
        /// <para>Whether the current version of the system is already a release version</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功转换，若系统已为正式版则直接返回 <see langword="true"/></para>
        /// <para>A <see langword="bool"/> value indicating whether the conversion was successful, if the system is already a release version, it will return <see langword="true"/> directly</para>
        /// </returns>
        public bool ConvertToReleaseVersion(WindowsProductName targetWindowsProduct, bool isReleaseVersionCurrent)
        {
            if (!isReleaseVersionCurrent)
            {
                /* 还需考虑后缀带N和G的版本，目前暂未实现 */
                /* Need to consider versions with suffixes N and G, which are not currently implemented */
                string targetEdition = targetWindowsProduct.ToString().Split('_')[^1];

                string targetArgs = "/online /Set-Edition:Server" + targetEdition + " /ProductKey:" + GetActivationKey(targetWindowsProduct) + " /AcceptEula";
                string output = Utility.RunProcess(Shared.Dism, targetArgs, Shared.System32Path, false);

                if (!string.IsNullOrEmpty(output))
                {
                    Log.Logger.Information("Successfully converted the Windows product to the target version: {product}", targetWindowsProduct);
                    return true;
                }
                else
                {
                    Log.Logger.Error("Converted the Windows product to the target version: {product} with no response", targetWindowsProduct);
                    return false;
                }
            }
            else
            {
                Log.Logger.Information("The current system is already a release version");
                return true;
            }
        }

        /// <summary>
        /// <para>设置 KMS 批量激活密钥</para>
        /// <para>Set the KMS volume activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>批量激活密钥</para>
        /// <para>Volume activation key</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功设置</para>
        /// <para>A <see langword="bool"/> value indicating whether the setting was successful</para>
        /// </returns>
        public bool SetActivationKey(string key)
        {
            try
            {
                string output = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//Nologo slmgr.vbs /ipk " + key,
                    Shared.System32Path,
                    true
                );

                /* TODO: 需要修改逻辑来精确判断，不得仅仅通过是否有输出来判断是否设置成功 */
                if (!string.IsNullOrEmpty(output))
                {
                    Log.Logger.Information("Successfully set the Windows online activation key: {key}", key);
                    return true;
                }
                else
                {
                    Log.Logger.Error("Set the Windows online activation key: {key} with no response", key);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to set the Windows online activation key: {key}", key);
                return false;
            }
        }

        /// <summary>
        /// <para>设置 KMS 服务器地址</para>
        /// <para>Set the KMS server address</para>
        /// </summary>
        /// <param name="serverName">
        /// <para>KMS 服务器地址</para>
        /// <para>KMS server address</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功设置</para>
        /// <para>A <see langword="bool"/> value indicating whether the setting was successful</para>
        /// </returns>
        public bool SetKMSServer(string serverName)
        {
            try
            {
                string output = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//Nologo slmgr.vbs /skms " + serverName,
                    Shared.System32Path,
                    true
                );
                if (!String.IsNullOrEmpty(output))
                {
                    Log.Logger.Information("Successfully set the Windows KMS server address: {server}", serverName);
                    return true;
                }
                else
                {
                    Log.Logger.Error("Set the Windows KMS server address: {server} with no response", serverName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to set the Windows KMS server address: {server}", serverName);
                return false;
            }
        }

        /// <summary>
        /// <para>应用激活</para>
        /// <para>Apply activation</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功激活</para>
        /// <para>A <see langword="bool"/> value indicating whether the activation was successful</para>
        /// </returns>
        public bool ApplyActivation()
        {
            try
            {
                string output = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//Nologo slmgr.vbs /ato",
                    Shared.System32Path,
                    true
                );

                if (!string.IsNullOrEmpty(output))
                {
                    Log.Logger.Information("Successfully applied the Windows online activation");
                    return true;
                }
                else
                {
                    Log.Logger.Error("Applied the Windows online activation with no response");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to apply the Windows online activation");
                return false;
            }
        }

        /// <summary>
        /// <para>移除激活</para>
        /// <para>Remove activation</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功移除</para>
        /// <para>A <see langword="bool"/> value indicating whether the removal was successful</para>
        /// </returns>
        public bool RemoveActivation()
        {
            try
            {
                string uninstallKeyOutput = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//Nologo slmgr.vbs /upk",
                    Shared.System32Path,
                    true
                );
                string cleanServerOutput = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//Nologo slmgr.vbs /ckms",
                    Shared.System32Path,
                    true
                );

                if (!string.IsNullOrEmpty(uninstallKeyOutput) || !string.IsNullOrEmpty(cleanServerOutput))
                {
                    Log.Logger.Information("Successfully removed the activation");
                    return true;
                }
                else
                {
                    Log.Logger.Error("Removed the activation with no response");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to remove the activation");
                return false;
            }
        }

        /// <summary>
        /// <para>注册手动续签任务</para>
        /// <para>Register manual renewal task</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功注册</para>
        /// <para>A <see langword="bool"/> value indicating whether the registration was successful</para>
        /// </returns>
        public bool RegisterManualRenewalTask()
        {
            try
            {
                string savedExecutablePath = Shared.UserDocumentsActivatorPath + "Activator.exe";
                ProcessModule? module = Process.GetCurrentProcess().MainModule;
                string? currentExecutablePath = module?.FileName;
                if (currentExecutablePath is not null)
                {
                    // 将自身拷贝到“C:\Users\%username%\KMS Activator\”下后利用schtasks.exe设定定时任务，在180天内再次启动
                    // Copy itself to "C:\Users\%username%\KMS Activator\" and use schtasks.exe to set a scheduled task and start it again in 180 days
                    if (currentExecutablePath != savedExecutablePath)
                    {
                        File.Copy(currentExecutablePath, savedExecutablePath, true);
                    }
                    string execName = "schtasks.exe";
                    // "/sm"开关为"StartMode"的缩写，"renew"指示这次启动程序是以续签的身份启动，--renew后面的参数"windows"或"office"指示将续签什么类型的激活，/rl指示使用最高权限启动
                    // The "/sm" switch is short for "StartMode", "renew" indicates that this time the boot program is started as a renewal, and the parameter "Windows" or "Office" after renew indicates what type of activation will be renewed
                    string execArgs = "/CREATE /TN \\Anawaert\\KMS_Renew_Windows" + " /TR " + "\"\\\"" + Shared.UserDocumentsActivatorPath + "Activator.exe\\\" --renew Windows" + "\" /SC DAILY /MO 180 /RL HIGHEST";

                    if (!string.IsNullOrEmpty(Utility.RunProcess(execName, execArgs, Shared.System32Path, true)))
                    {
                        Log.Logger.Information("Successfully registered the manual renewal task");
                        return true;
                    }
                    else
                    {
                        Log.Logger.Error("Registered the manual renewal task with no response");
                        return false;
                    }
                }
                else
                {
                    Log.Logger.Error("Failed to get the current executable path, which meant that programme itself could not be copied");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to register the manual renewal task");
                return false;
            }
        }

        /// <summary>
        /// <para>移除手动续签任务</para>
        /// <para>Remove manual renewal task</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功移除</para>
        /// <para>A <see langword="bool"/> value indicating whether the removal was successful</para>
        /// </returns>
        public bool RemoveManualRenewalTask()
        {
            try
            {
                string execName = "schtasks.exe";
                string execArgs = "/delete /tn \\Anawaert\\KMS_Renew_Windows" + " /f";

                if (string.IsNullOrEmpty(Utility.RunProcess(execName, execArgs, Shared.System32Path, true)))
                {
                    Log.Logger.Information("Successfully removed the manual renewal task");
                    return true;
                }
                else
                {
                    Log.Logger.Error("Removed the manual renewal task with no response");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to remove the manual renewal task");
                return false;
            }
        }

        /// <summary>
        /// <para>在线 Windows 激活对象的无参构造函数</para>
        /// <para>Parameterless constructor of the online Windows activator object</para>
        /// </summary>
        public WindowsOnlineActivator() { }
    }
}
