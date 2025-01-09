using System.IO;
using System.Diagnostics;

namespace Activator
{
    public class WindowsOnlineActivator : IWindowsOnlineActivator
    {
        public string? GetActivationKey(WindowsProductName windowsProduct)
        {
            try
            {
                return WindowsActivationKey.OnlineActivationKey[windowsProduct];
            }
            catch
            {
                return null;
            }
        }

        public bool ConvertToReleaseVersion(WindowsProductName targetWindowsProduct, bool isReleaseVersionCurrent)
        {
            if (!isReleaseVersionCurrent)
            {
                // ���迼�Ǻ�׺��N��G�İ汾
                string targetEdition = targetWindowsProduct.ToString().Split('_')[^1];

                string targetArgs = "/online /Set-Edition:Server" + targetEdition + " /ProductKey:" + GetActivationKey(targetWindowsProduct) + " /AcceptEula";
                string output = Utility.RunProcess(Shared.Dism, targetArgs, Shared.System32Path, false);

                return output != string.Empty;
            }
            else
            {
                return true;
            }
        }

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
                return output != string.Empty;
            }
            catch
            {
                return false;
            }
        }

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
                return output != string.Empty;
            }
            catch
            {
                return false;
            }
        }

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

                return output != string.Empty;
            }
            catch
            {
                return false;
            }
        }

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

                return uninstallKeyOutput != string.Empty || cleanServerOutput != string.Empty;
            }
            catch
            {
                return false;
            }
        }

        public bool RegisterManualRenewalTask()
        {
            try
            {
                string savedExecutablePath = Shared.UserDocumentsActivatorPath + "Activator.exe";
                ProcessModule? module = Process.GetCurrentProcess().MainModule;
                string? currentExecutablePath = module?.FileName;
                if (currentExecutablePath != null)
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
                    
                    return Utility.RunProcess(execName, execArgs, Shared.System32Path, true) != string.Empty;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveManualRenewalTask()
        {
            try
            {
                string execName = "schtasks.exe";
                string execArgs = "/delete /tn \\Anawaert\\KMS_Renew_Windows" + " /f";

                return Utility.RunProcess(execName, execArgs, Shared.System32Path, true) != string.Empty;
            }
            catch
            {
                return false;
            }
        }

        public WindowsOnlineActivator() { }
    }
}