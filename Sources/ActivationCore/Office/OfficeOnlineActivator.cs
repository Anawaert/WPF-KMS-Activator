using Serilog;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Activator
{
    /// <summary>
    /// <para>Office 在线激活对象</para>
    /// <para>Office online activator object</para>
    /// </summary>
    public class OfficeOnlineActivator : IOfficeOnlineActivator
    {
        /// <summary>
        /// <para>为指定版本的 Office 安装批量授权证书</para>
        /// <para>Install volume license certificate for the specified version of Office</para>
        /// </summary>
        /// <param name="licenseDirectory">
        /// <para>存放批量授权证书的目录</para>
        /// <para>Directory where the volume license certificate is stored</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <param name="officeEdition">
        /// <para>需要安装证书的 Office 版本</para>
        /// <para>Office edition to install the certificate</para>
        /// </param>
        /// <returns>
        /// <para>指示是否安装成功，若安装成功，则返回 <see langword="true"/></para>
        /// <para>Indicates whether the installation is successful, return <see langword="true"/> if successful</para>
        /// </returns>
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

                Log.Logger.Information("Installed volume license for Office edition: {0}", officeEdition.ToString());

                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to install volume license for Office edition: {0}", officeEdition.ToString());
                return false;
            }
        }

        /// <summary>
        /// <para>将指定的 Office 版本转换为批量授权版本</para>
        /// </summary>
        /// <param name="licenseDirectory">
        /// <para>存放批量授权证书的目录</para>
        /// <para>Directory where the volume license certificate is stored</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <param name="officeEdition">
        /// <para>需要转换的 Office 版本</para>
        /// <para>Office edition to convert</para>
        /// </param>
        /// <param name="visioEdition">
        /// <para>需要转换的 Visio 版本</para>
        /// <para>Visio edition to convert</para>
        /// </param>
        /// <returns>
        /// <para>指示是否转换成功，若转换成功，则返回 <see langword="true"/></para>
        /// <para>Indicates whether the conversion is successful, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool ConvertToVolumeLicense(string? licenseDirectory, string? osppDirectory, OfficeEditionName officeEdition, VisioEditionName visioEdition)
        {
            try
            {
                if (licenseDirectory is not null && osppDirectory is not null &&
                    officeEdition != OfficeEditionName.Unsupported && visioEdition != VisioEditionName.Unsupported)
                {
                    bool retVal = InstallVolumeLicense(licenseDirectory, osppDirectory, officeEdition);
                    Log.Logger.Information("Successfully converted to volume license of Office edition: {0}", officeEdition.ToString());
                    return retVal;
                }

                Log.Logger.Error("Failed to convert to volume license of Office edition: {0} for it's unsupported or uninstalled", officeEdition.ToString());
                return false;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error occurred while converting to volume license");
                return false;
            }
        }

        /// <summary>
        /// <para>获取指定 Office 版本的在线激活密钥</para>
        /// <para>Get the online activation key of the specified Office edition</para>
        /// </summary>
        /// <param name="officeEdition">
        /// <para>需要获取激活密钥的 Office 版本</para>
        /// <para>Office edition to get the activation key</para>
        /// </param>
        /// <returns>
        /// <para>Office 激活密钥，可能为空</para>
        /// <para>Office activation key, may be null</para>
        /// </returns>
        public string? GetOfficeActivationKey(OfficeEditionName officeEdition)
        {
            try
            {
                string? key = OfficeActivationKey.OnlineActivationKey[officeEdition];
                Log.Logger.Information("Successfully got online activation key: {0} for Office edition: {1}", key, officeEdition.ToString());
                return key;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to get online activation key for Office edition: {0}", officeEdition.ToString());
                return null;
            }
        }

        /// <summary>
        /// <para>获取指定 Visio 版本的在线激活密钥</para>
        /// <para>Get the online activation key of the specified Visio edition</para>
        /// </summary>
        /// <param name="visioEdition">
        /// <para>需要获取激活密钥的 Visio 版本</para>
        /// <para>Visio edition to get the activation key</para>
        /// </param>
        /// <returns>
        /// <para>Visio 激活密钥，可能为空</para>
        /// <para>Visio activation key, may be null</para>
        /// </returns>
        public string? GetVisioActivationKey(VisioEditionName visioEdition)
        {
            try
            {
                string? key = VisioActivationKey.OnlineActivationKey[visioEdition];
                Log.Logger.Information("Successfully got online activation key: {0} for Visio edition: {1}", key, visioEdition.ToString());
                return key;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to get online activation key for Visio edition: {0}", visioEdition.ToString());
                return null;
            }
        }

        /// <summary>
        /// <para>设置 Office 激活密钥</para>
        /// <para>Set Office activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>Office 批量激活密钥</para>
        /// <para>Office volume activation key</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示设置密钥是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the key is set successfully, return <see langword="true"/> if successful</para>
        /// </returns>
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

                if (!string.IsNullOrEmpty(setOfficeKeyOutput))
                {
                    Log.Logger.Information("Successfully set Office online activation key: {key}", key);
                    return true;
                }
                else
                {
                    Log.Logger.Error("Set the Office online activation key: {key} with no response", key);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to set Office online activation key: {key}", key);
                return false;
            }
        }

        /// <summary>
        /// <para>设置 Visio 激活密钥</para>
        /// <para>Set Visio activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>Visio 批量激活密钥</para>
        /// <para>Visio volume activation key</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示设置密钥是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the key is set successfully, return <see langword="true"/> if successful</para>
        /// </returns>
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

                if (!string.IsNullOrEmpty(setVisioKeyOutput))
                {
                    Log.Logger.Information("Successfully set Visio online activation key: {key}", key);
                    return true;
                }
                else
                {
                    Log.Logger.Error("Set the Visio online activation key: {key} with no response", key);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to set Visio online activation key: {key}", key);
                return false;
            }
        }

        /// <summary>
        /// <para>设置在线激活的服务器地址</para>
        /// <para>Set the server address for online activation</para>
        /// </summary>
        /// <param name="serverName">
        /// <para>KMS 服务器地址</para>
        /// <para>KMS server address</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示设置 KMS 服务器是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the KMS server is set successfully, return <see langword="true"/> if successful</para>
        /// </returns>
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
                if (!string.IsNullOrEmpty(serverName))
                {
                    Log.Logger.Information("Successfully set the Office KMS server address: {serverName}", serverName);
                    return true;
                }
                else
                {
                    Log.Logger.Error("Set the Office KMS server address: {serverName} with no response", serverName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to set the Office KMS server address: {serverName}", serverName);
                return false;
            }
        }

        /// <summary>
        /// <para>对 Office/Visio 应用在线批量激活</para>
        /// <para>Apply online volume activation to Office/Visio</para>
        /// </summary>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示激活是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the activation is successful, return <see langword="true"/> if successful</para>
        /// </returns>
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

                if (!string.IsNullOrEmpty(applyActivationOutput))
                {
                    Log.Logger.Information("Successfully applied the Office online activation");
                    return true;
                }
                else
                {
                    Log.Logger.Error("Applied the Office online activation with no response");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to apply the Office online activation");
                return false;
            }
        }

        /// <summary>
        /// <para>移除 Office/Visio 应用的激活状态</para>
        /// <para>Remove the activation status of Office/Visio</para>
        /// </summary>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示移除激活是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the removal of activation is successful, return <see langword="true"/> if successful</para>
        /// </returns>
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
                        string output = Utility.RunProcess
                        (
                            Shared.Cscript,
                            @"//NoLogo ospp.vbs /unpkey:" + matchedKey.Value,
                            osppDirectory,
                            true
                        );

                        if (!string.IsNullOrEmpty(output))
                        {
                            Log.Logger.Information("Successfully removed the Office key: {key}", matchedKey.Value);
                        }
                        else
                        {
                            Log.Logger.Error("Removed the Office key: {key} with no response", matchedKey.Value);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to remove the Office activation");
                return false;
            }
        }

        /// <summary>
        /// <para>注册手动续签任务</para>
        /// <para>Register manual renewal task</para>
        /// <para>* 请不要调用此函数</para>
        /// <para>* Do not call this function</para>
        /// </summary>
        /// <returns>
        /// <para>注册手动续签任务是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Whether the manual renewal task is registered successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// <para>请不要调用此函数，因为函数尚未实现</para>
        /// <para>Do not call this function because it is not implemented yet</para>
        /// </exception>
        public bool RegisterManualRenewalTask()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <para>移除手动续签任务</para>
        /// <para>Remove manual renewal task</para>
        /// <para>* 请不要调用此函数</para>
        /// <para>* Do not call this function</para>
        /// </summary>
        /// <returns>
        /// <para>移除手动续签任务是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Whether the manual renewal task is removed successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// <para>请不要调用此函数，因为函数尚未实现</para>
        /// <para>Do not call this function because it is not implemented yet</para>
        /// </exception>
        public bool RemoveManualRenewalTask()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <para>在线 Office 激活对象的无参构造函数</para>
        /// <para>Parameterless constructor of the online Office activator object</para>
        /// </summary>
        public OfficeOnlineActivator() { }
    }
}
