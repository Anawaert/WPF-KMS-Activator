using System.IO;
using System.Diagnostics;
using Serilog;
using System;

namespace Activator
{
    /// <summary>
    /// <para>Windows ���߼������</para>
    /// <para>Windows online activator object</para>
    /// </summary>
    public class WindowsOnlineActivator : IWindowsOnlineActivator
    {
        /// <summary>
        /// <para>��ȡ Windows ��Ʒ�Ķ�Ӧ����������Կ</para>
        /// <para>Get the corresponding volume activation key of the Windows product</para>
        /// </summary>
        /// <param name="windowsProduct">
        /// <para>��Ҫ����� Windows ��Ʒ����</para>
        /// <para>The name of the Windows product that needs to be activated</para>
        /// </param>
        /// <returns>
        /// <para>���ض�Ӧ�ļ�����Կ</para>
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
        /// <para>�� Windows ��Ʒת��Ϊ��Ӧ����������汾������ǰϵͳ��Ϊ��ʽ�棬�򲻻����κζ���</para>
        /// <para>Convert the Windows product to the corresponding volume activation version, if the current system is already a release version, there will be no action</para>
        /// </summary>
        /// <param name="targetWindowsProduct">
        /// <para>��Ҫת����Ŀ�� Windows ��Ʒ����</para>
        /// <para>The target Windows product name to be converted</para>
        /// </param>
        /// <param name="isReleaseVersionCurrent">
        /// <para>��ǰϵͳ�İ汾�Ƿ��Ѿ�Ϊ��ʽ�汾</para>
        /// <para>Whether the current version of the system is already a release version</para>
        /// </param>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ�ת������ϵͳ��Ϊ��ʽ����ֱ�ӷ��� <see langword="true"/></para>
        /// <para>A <see langword="bool"/> value indicating whether the conversion was successful, if the system is already a release version, it will return <see langword="true"/> directly</para>
        /// </returns>
        public bool ConvertToReleaseVersion(WindowsProductName targetWindowsProduct, bool isReleaseVersionCurrent)
        {
            if (!isReleaseVersionCurrent)
            {
                /* ���迼�Ǻ�׺��N��G�İ汾��Ŀǰ��δʵ�� */
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
        /// <para>���� KMS ����������Կ</para>
        /// <para>Set the KMS volume activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>����������Կ</para>
        /// <para>Volume activation key</para>
        /// </param>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ�����</para>
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

                /* TODO: ��Ҫ�޸��߼�����ȷ�жϣ����ý���ͨ���Ƿ���������ж��Ƿ����óɹ� */
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
        /// <para>���� KMS ��������ַ</para>
        /// <para>Set the KMS server address</para>
        /// </summary>
        /// <param name="serverName">
        /// <para>KMS ��������ַ</para>
        /// <para>KMS server address</para>
        /// </param>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ�����</para>
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
        /// <para>Ӧ�ü���</para>
        /// <para>Apply activation</para>
        /// </summary>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ�����</para>
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
        /// <para>�Ƴ�����</para>
        /// <para>Remove activation</para>
        /// </summary>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ��Ƴ�</para>
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
        /// <para>ע���ֶ���ǩ����</para>
        /// <para>Register manual renewal task</para>
        /// </summary>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ�ע��</para>
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
                    // ������������C:\Users\%username%\KMS Activator\���º�����schtasks.exe�趨��ʱ������180�����ٴ�����
                    // Copy itself to "C:\Users\%username%\KMS Activator\" and use schtasks.exe to set a scheduled task and start it again in 180 days
                    if (currentExecutablePath != savedExecutablePath)
                    {
                        File.Copy(currentExecutablePath, savedExecutablePath, true);
                    }
                    string execName = "schtasks.exe";
                    // "/sm"����Ϊ"StartMode"����д��"renew"ָʾ�����������������ǩ�����������--renew����Ĳ���"windows"��"office"ָʾ����ǩʲô���͵ļ��/rlָʾʹ�����Ȩ������
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
        /// <para>�Ƴ��ֶ���ǩ����</para>
        /// <para>Remove manual renewal task</para>
        /// </summary>
        /// <returns>
        /// <para>һ�� <see langword="bool"/> ֵ��ָʾ�Ƿ��ѳɹ��Ƴ�</para>
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
        /// <para>���� Windows ���������޲ι��캯��</para>
        /// <para>Parameterless constructor of the online Windows activator object</para>
        /// </summary>
        public WindowsOnlineActivator() { }
    }
}
