using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Activator
{
    public class OfficeOnlineActivator : IOnlineActivator
    {
        public static OfficeOnlineActivator Instance { get; } = new OfficeOnlineActivator();

        public bool ConvertToVOL()
        {
            try
            {
                if (OfficeInfo.LicenseDirectory is not null && OfficeInfo.OsppDirectory is not null &&
                    OfficeInfo.OfficeProduct != OfficeProductName.Unsupported && OfficeInfo.VisioProduct != VisioProductName.Unsupported)
                {
                    return InstallVolumeLicense();
                }
                return false;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }

        private bool InstallVolumeLicense()
        {
            if (OfficeInfo.OfficeProduct == OfficeProductName.Unsupported || string.IsNullOrEmpty(OfficeInfo.OsppDirectory))
            {
                return false;
            }

            try
            {
                // 调用Shared类的RunProcess函数以安装所有的Pro Plus VL版本证书
                // Call the RunProcess function of the Shared class to install all the Pro Plus VL certificates
                if (OfficeInfo.OfficeProduct == OfficeProductName.Office_2021)
                {
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlus2021VL_KMS_Client_AE-ul.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlus2021VL_KMS_Client_AE-ul-oob.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioPro2021VL_KMS_Client_AE-ppd.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioPro2021VL_KMS_Client_AE-ul.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioPro2021VL_KMS_Client_AE-ul-oob.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "pkeyconfig-office.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                }   
                else if (OfficeInfo.OfficeProduct == OfficeProductName.Office_2019)
                {
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlus2019VL_KMS_Client_AE-ppd.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlus2019VL_KMS_Client_AE-ul.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlus2019VL_KMS_Client_AE-ul-oob.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioPro2019VL_KMS_Client_AE-ppd.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioPro2019VL_KMS_Client_AE-ul.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioPro2019VL_KMS_Client_AE-ul-oob.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "pkeyconfig-office.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                }
                else if (OfficeInfo.OfficeProduct == OfficeProductName.Office_2016)
                {
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlusVL_KMS_Client-ppd.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlusVL_KMS_Client-ul.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "ProPlusVL_KMS_Client-ul-oob.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioProVL_KMS_Client-ppd.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioProVL_KMS_Client-ul.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "VisioProVL_KMS_Client-ul-oob.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                    Utility.RunProcess(Shared.Cscript, "//NoLogo ospp.vbs /inslic:" + "\"" + OfficeInfo.LicenseDirectory + "pkeyconfig-office.xrm-ms\"", OfficeInfo.OsppDirectory, true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetActivationKey(string key)
        {
            if (OfficeInfo.OfficeProduct == OfficeProductName.Unsupported || string.IsNullOrEmpty(OfficeInfo.OsppDirectory))
            {
                return false;
            }

            try
            {
                string setOfficeKeyOutput = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//NoLogo ospp.vbs /inpkey:" + key, 
                    OfficeInfo.OsppDirectory,
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

        public bool SetVisioActivationKey(string key)
        {
            if (OfficeInfo.VisioProduct == VisioProductName.Unsupported || string.IsNullOrEmpty(OfficeInfo.OsppDirectory))
            {
                return false;
            }

            try
            {
                string setVisioKeyOutput = Utility.RunProcess
                (
                    Shared.Cscript,
                    @"//NoLogo ospp.vbs /inpkey:" + key,
                    OfficeInfo.OsppDirectory,
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

        public bool SetKMSServer(string serverName)
        {
            if (string.IsNullOrEmpty(OfficeInfo.OsppDirectory))
            {
                return false;
            }

            try
            {
                string setServerOutput = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /sethst:" + serverName,
                    OfficeInfo.OsppDirectory,
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

        public bool ApplyActivation()
        {
            if (string.IsNullOrEmpty(OfficeInfo.OsppDirectory))
            {
                return false;
            }

            try
            {
                string applyActivationOutput = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /act", 
                    OfficeInfo.OsppDirectory,
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

        public bool RemoveActivation()
        {
            try
            {
                if (string.IsNullOrEmpty(OfficeInfo.OsppDirectory))
                {
                    return false;
                }

                // 匹配OSPP.vbs输出结果中---与---之间的内容
                // Match the content between --- in the OSPP.vbs output
                string pattern = @"(?<=-{39})(.*?)(?=-{39})";
                string retVal = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /dstatus", 
                    OfficeInfo.OsppDirectory,
                    true
                );

                MatchCollection checkInfoCollection = Regex.Matches(retVal, pattern, RegexOptions.Singleline);
                foreach (Match match in checkInfoCollection)
                {
                    // 再次匹配已安装密钥
                    // Match the installed key again
                    string installedKeyPattern = @"(?<=installed product key: )[A-Za-z0-9]{5}";
                    Match matchedkey = Regex.Match(match.Value, installedKeyPattern);
                    // 若匹配成功，则执行卸载密钥
                    // If the match is successful, uninstall the key
                    if (matchedkey.Success/* && match.Value.Contains("LICENSE STATUS:  ---LICENSED---") */)
                    {
                        Utility.RunProcess
                        (
                            Shared.Cscript, 
                            @"//NoLogo ospp.vbs /unpkey:" + matchedkey.Value, 
                            OfficeInfo.OsppDirectory,
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