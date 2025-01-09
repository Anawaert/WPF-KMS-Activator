using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Activator
{
    public class OfficeOnlineActivator : IOfficeOnlineActivator
    {
        private bool InstallVolumeLicense(string? licenseDirectory, string? osppDirectory, OfficeEditionName officeEdition)
        {
            if (officeEdition == OfficeEditionName.Unsupported || string.IsNullOrEmpty(licenseDirectory) || string.IsNullOrEmpty(osppDirectory))
            {
                return false;
            }

            try
            {
                // 调用Shared类的RunProcess函数以安装所有的Pro Plus VL版本证书
                // Call the RunProcess function of the Shared class to install all the Pro Plus VL certificates
                if (officeEdition == OfficeEditionName.Office_2021)
                {
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ul.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioPro2021VL_KMS_Client_AE-ppd.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioPro2021VL_KMS_Client_AE-ul.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioPro2021VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "pkeyconfig-office.xrm-ms\"", osppDirectory, true);
                }   
                else if (officeEdition == OfficeEditionName.Office_2019)
                {
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlus2019VL_KMS_Client_AE-ppd.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlus2019VL_KMS_Client_AE-ul.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlus2019VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioPro2019VL_KMS_Client_AE-ppd.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioPro2019VL_KMS_Client_AE-ul.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioPro2019VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "pkeyconfig-office.xrm-ms\"", osppDirectory, true);
                }
                else if (officeEdition == OfficeEditionName.Office_2016)
                {
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlusVL_KMS_Client-ppd.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlusVL_KMS_Client-ul.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "ProPlusVL_KMS_Client-ul-oob.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioProVL_KMS_Client-ppd.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioProVL_KMS_Client-ul.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "VisioProVL_KMS_Client-ul-oob.xrm-ms\"", osppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDirectory + "pkeyconfig-office.xrm-ms\"", osppDirectory, true);
                }

                return true;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public bool ConvertToVolumeLicense(string? licenseDirectory, string? osppDirectory, OfficeEditionName officeEdition, VisioEditionName visioEdition)
        {
            try
            {
                if (licenseDirectory is not null && osppDirectory is not null &&
                    officeEdition != OfficeEditionName.Unsupported && visioEdition != VisioEditionName.Unsupported)
                {
                    return InstallVolumeLicense(licenseDirectory, osppDirectory, officeEdition);
                }
                return false;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public string? GetOfficeActivationKey(OfficeEditionName officeEdition)
        {
            try
            {
                return OfficeActivationKey.OnlineActivationKey[officeEdition];
            }
            catch
            {
                return null;
            }
        }

        public string? GetVisioActivationKey(VisioEditionName visioEdition)
        {
            try
            {
                return VisioActivationKey.OnlineActivationKey[visioEdition];
            }
            catch
            {
                return null;
            }
        }

        public bool SetOfficeActivationKey(string key, string? osppDirectory)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(osppDirectory))
            {
                return false;
            }

            try
            {
                string setOfficeKeyOutput = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//NoLogo ospp.vbs /inpkey:" + key, 
                    osppDirectory,
                    true
                );

                return setOfficeKeyOutput != string.Empty;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public bool SetVisioActivationKey(string key, string? osppDirectory)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(osppDirectory))
            {
                return false;
            }

            try
            {
                string setVisioKeyOutput = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//NoLogo ospp.vbs /inpkey:" + key,
                    osppDirectory,
                    true
                );

                return setVisioKeyOutput != string.Empty;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public bool SetKMSServer(string serverName, string? osppDirectory)
        {
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(osppDirectory))
            {
                return false;
            }

            try
            {
                string setServerOutput = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /sethst:" + serverName,
                    osppDirectory,
                    true
                );
                return serverName != string.Empty;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public bool ApplyActivation(string? osppDirectory)
        {
            if (string.IsNullOrEmpty(osppDirectory))
            {
                return false;
            }

            try
            {
                string applyActivationOutput = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /act", 
                    osppDirectory,
                    true
                );

                return applyActivationOutput != string.Empty;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public bool RemoveActivation(string? osppDirectory)
        {
            if (string.IsNullOrEmpty(osppDirectory))
            {
                return false;
            }

            try
            {
                // 匹配OSPP.vbs输出结果中---与---之间的内容
                // Match the content between --- in the OSPP.vbs output
                string pattern = @"(?<=-{39})(.*?)(?=-{39})";
                string retVal = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /dstatus",
                    osppDirectory,
                    true
                );

                MatchCollection checkInfoCollection = Regex.Matches(retVal, pattern, RegexOptions.Singleline);
                foreach (Match match in checkInfoCollection)
                {
                    // 再次匹配已安装密钥
                    // Match the installed key again
                    string installedKeyPattern = @"(?<=installed product key: )[A-Za-z0-9]{5}";
                    Match matchedKey = Regex.Match(match.Value, installedKeyPattern);
                    // 若匹配成功，则执行卸载密钥
                    // If the match is successful, uninstall the key
                    if (matchedKey.Success)
                    {
                        Utility.RunProcess
                        (
                            Shared.Cscript, 
                            @"//NoLogo ospp.vbs /unpkey:" + matchedKey.Value,
                            osppDirectory,
                            true
                        );
                    }
                }
                return true;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        public bool RegisterManualRenewalTask()
        {
            throw new NotImplementedException();
        }

        public bool RemoveManualRenewalTask()
        {
            throw new NotImplementedException();
        }

        public OfficeOnlineActivator()
        {
            
        }
    }
}