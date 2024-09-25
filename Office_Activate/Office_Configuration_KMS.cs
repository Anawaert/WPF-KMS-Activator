using System;
using System.Collections.Generic;
using Microsoft.Win32;  // 配合注册表读写操作  Works with registry read and write operations
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks
using System.IO;
using System.Text.RegularExpressions;

namespace KMS_Activator
{
    /// <summary>
    ///     <para>
    ///         该类用于存放各种用来配置Office或获取Office信息的函数
    ///     </para>
    ///     <para>
    ///         This class is used to store various functions used to configure Office or obtain Office information
    ///     </para>
    /// </summary>
    public static class Office_Configurator
    {
        /// <summary>
        ///     <para>
        ///         该函数用于判断当前Office是否已经被激活
        ///     </para>
        ///     <para>
        ///         This function is used to determine whether the current Office is active
        ///     </para>
        /// </summary>
        /// <param name="osppDirectory">
        ///     <para>
        ///         Office的OSPP.vbs所在目录
        ///     </para>
        ///     <para>
        ///         OSPP.vbs directory of Office
        ///     </para>
        /// </param>
        /// <param name="isStillNoInstalledKey">
        ///     <para>
        ///         一个 <see langword="out"/> 参数，指示当前Office是否仍未安装密钥
        ///     </para>
        ///     <para>
        ///         A <see langword="out"/> parameter indicating whether the current Office key is still not installed
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         一个<see cref="bool"/>值，<see langword="true"/>表示已激活
        ///     </para>
        ///     <para>
        ///         A <see cref="bool"/> value, <see langword="true"/> indicates active
        ///     </para>
        /// </returns>
        public static bool IsOfficeActivated(string osppDirectory, out bool isStillNoInstalledKey)
        {
            // 从OSPP.vbs获取Office的信息
            // Get information about Office from OSPP.vbs
            string checkInfo = string.Empty;
            try
            {
                checkInfo = RunProcess(CSCRIPT, @"//Nologo ospp.vbs /dstatus", osppDirectory, true);
            }
            catch
            {
                // 若出现异常，则认为Office并未激活，同时也未安装密钥
                // If an exception occurs, Office is not active and the key is not installed
                isStillNoInstalledKey = false;
                return false;
            }

            bool activationState = false;

            // 使用正则表达式来匹配OSPP.vbs的输出结果中---与---之间的内容，若为Office Pro Plus的VL版本且"LICENSE STATUS:  ---LICENSED---"，则说明已激活
            string pattern = @"(?<=-{39})(.*?)(?=-{39})";
            MatchCollection checkInfoCollection = Regex.Matches(checkInfo, pattern, RegexOptions.Singleline);
            foreach (Match match in checkInfoCollection)
            {
                string proPlusPattern = @"Office(\d{2})?ProPlus(\d{4})?(VL)?";
                if (Regex.Match(match.Value, proPlusPattern).Success && match.Value.Contains("LICENSE STATUS:  ---LICENSED---"))
                {
                    activationState = true;
                    break;
                }
            }

            // 检查是否为未安装密钥
            // Check if the key is not installed
            isStillNoInstalledKey = checkInfo.Contains("No installed product keys detected");

            return activationState;    
        }

        /// <summary>
        ///     <para>
        ///       该函数用以判断Office或OSPP.vbs所在路径或目录是否已经被查找到  
        ///     </para>
        ///     <para>
        ///         This function is used to determine whether the path or directory of Office or OSPP.vbs has been found
        ///     </para>
        /// </summary>
        /// <returns>
        ///     <para>
        ///         一个 <see cref="bool"/> 值，<see langword="true"/> 即代表已被正确获取 
        ///     </para>
        ///     <para>
        ///         A <see cref="bool"/> value, <see langword="true"/> indicates that it was retrieved correctly
        ///     </para>
        /// </returns>
        /// <param name="osppPath">
        ///     <para>
        ///         一个 <see langword="out"/> 参数，用以向外传参指示OSPP.vbs的所在路径
        ///     </para>
        ///     <para>
        ///         A <see langword="out"/> parameter to pass out indicating the path of OSPP.vbs
        ///     </para>
        /// </param>
        /// <param name="officeVersion">
        ///     <para>
        ///         一个 <see langword="out"/> 参数，用来向外传参以指示当前已安装在本计算机上最高版本的Office
        ///     </para>
        ///     <para>
        ///         A <see langword="out"/> parameter to pass parameters indicating the highest version of Office currently installed on the computer
        ///     </para>
        /// </param>
        public static bool IsOfficePathFound(out string osppPath, out string officeVersion)
        {
            try
            {
                // 首先判断系统是32位还是64位环境,然后获取正确的分支
                // First determine whether the system is a 32-bit or 64-bit environment, and then get the correct branch
                RegistryKey regKey = Environment.Is64BitOperatingSystem ?
                                     RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                                     RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                string officePath = string.Empty;
                // 由于在安装时可能会出现在64位系统中安装了32位的情况，所以使用officeBaseKey__来区分32位与64位在注册表中的位置
                // officeBaseKey__ is used to distinguish the location of 32 bits from 64 bits in the registry because it may happen that 32 bits are installed on 64-bit systems at installation time
                RegistryKey? officeBaseKey64 = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Office");
                RegistryKey? officeBaseKey32 = regKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Office");
                // 若两者都为空，那么说明该计算机上并未安装Office（至少未在注册表中注册）
                // If both are empty, Office is not installed on the machine (at least not registered in the registry)
                if (officeBaseKey64 == null && officeBaseKey32 == null)
                {
                    throw new Exception("Office is not installed yet");
                }

                // 改用循环来判断Office的版本，因为注册表中的Office的版本可能不止一个
                // Use a loop to determine the version of Office, because there may be more than one version of Office in the registry
                string[] officeInternalVersions = new string[] { "16.0", "15.0", "14.0" };
                string office_ver = "Not_Found";
                foreach (string ver in officeInternalVersions)
                {
                    // 由于安装了Office，那么注册表中一定会有一个Word的键，所以可以通过Word键来判断是否安装了Office
                    // * 前提：该用户只安装了一个版本的Office
                    // Since Office is installed, there will definitely be a Word key in the registry, so you can determine if Office is installed through the Word key
                    // * Premise: The user has only installed one version of Office
                    bool isSubKeyExist = officeBaseKey64?.OpenSubKey(ver) != null || officeBaseKey32?.OpenSubKey(ver) != null;
                    bool isSubKeyHasWordKey = officeBaseKey64?.OpenSubKey(ver + "\\Word") != null || officeBaseKey32?.OpenSubKey(ver + "\\Word") != null;
                    if (isSubKeyExist && isSubKeyHasWordKey)
                    {
                        // 读取Word键下InstallRoot键的Path键值以获取Office完整安装路径
                        // Read the Path key of the InstallRoot key under the Word key to get the full Office installation path
                        RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey(ver + "\\Word\\InstallRoot");
                        RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey(ver + "\\Word\\InstallRoot");

                        // 若存在Word键，但是Path键值为空，那么说明Office安装出现问题
                        // If the Word key exists, but the Path key value is empty, it means that there is a problem with the Office installation
                        if (assumedKey64?.GetValue("Path") == null && assumedKey32?.GetValue("Path") == null)
                        {
                            throw new Exception("Installation of Office is damaged");
                        }

                        // 从注册表中读取Office的安装路径，并用路径中是否有root字段来判断版本是否为Office 2019或Office 2021
                        // Read the Office installation path from the registry and use the root field in the path to determine whether the version is Office 2019 or Office 2021
                        officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
                        if (ver == "16.0" && officePath.Contains("root"))
                        {
                            // 把"\root"给去掉，就是OSPP.vbs的所在目录了
                            // Remove the \root from the path, and you now have the OSPP.vbs directory
                            officePath = officePath.Replace("\\root", string.Empty);
                            office_ver = "Office 2019/2021";
                        }
                        // 没有"\root"，那么就是Office 2016
                        // If there is no "\root", it is Office 2016
                        else if (ver == "16.0")
                        {
                            office_ver = officePath != string.Empty ? "Office 2016" : "Not_Found";
                        }
                        // 以下同理
                        // The following is the same
                        else if (ver == "15.0")
                        {
                            office_ver = officePath != string.Empty ? "Office 2013" : "Not_Found";
                        }
                        else if (ver == "14.0")
                        {
                            office_ver = officePath != string.Empty ? "Office 2010" : "Not_Found";
                        }
                        else
                        {
                            office_ver = "Not_Found";
                        }
                    }
                    // 若不存在Word键，那么说明该版本的Office并未安装
                    // If the Word key does not exist, the version of Office is not installed
                    else
                    {
                        continue;
                    }
                }
                // officePath默认为空，依据其内容来获取OSPP.vbs的所在目录
                // 同理officeVersion
                // officePath is empty by default, and the directory where OSPP.vbs is located is obtained based on its content
                // Same with officeVersion
                osppPath = officePath != string.Empty ? officePath : "Not_Found";
                officeVersion = office_ver;
                return true;
            }
            catch
            {
                // 以上任何一个环节出现错误，则说明要么注册表出现问题，要么Office安装出现问题，那么显然OSPP.vbs的位置和Office的版本都是未定义
                // If any of the above links is wrong, it means that there is either a problem with the registry, or there is a problem with the Office installation, then obviously the location of OSPP.vbs and the version of Office are undefined
                osppPath = "Not_Found";
                officeVersion = "Not_Found";
                return false;
            }

            // try
            // {
            //     // 首先判断系统是32位还是64位环境,然后获取正确的分支
            //     // First determine whether the system is a 32-bit or 64-bit environment, and then get the correct branch
            //     RegistryKey regKey = Environment.Is64BitOperatingSystem                                         ? 
            //                          RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64):
            //                          RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            //     string officePath = string.Empty;
            //     // 由于在安装时可能会出现在64位系统中安装了32位的情况，所以使用officeBaseKey__来区分32位与64位在注册表中的位置
            //     // officeBaseKey__ is used to distinguish the location of 32 bits from 64 bits in the registry because it may happen that 32 bits are installed on 64-bit systems at installation time
            //     RegistryKey? officeBaseKey64 = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Office");
            //     RegistryKey? officeBaseKey32 = regKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Office");
            //     // 若两者都为空，那么说明该计算机上并未安装Office（至少未在注册表中注册）
            //     // If both are empty, Office is not installed on the machine (at least not registered in the registry)
            //     if (officeBaseKey64 == null && officeBaseKey32 == null)
            //     {
            //         throw new Exception("Office is not installed yet");
            //     }
               
            //     // 以下为具体的Office版本判断与从Office在注册表中提供的安装目录转化为OSPP.vbs所在目录的过程
            //     // The following is the specific Office version determination and the process of converting the installation directory provided by Office in the registry to the directory where OSPP.vbs is located
            //     // 当两处的注册表键值至少存在一个时候
            //     // When at least one of the two registry keys exists
            //     if (officeBaseKey64?.OpenSubKey("16.0") != null || officeBaseKey32?.OpenSubKey("16.0") != null)
            //     {
            //         // 读取Word键下InstallRoot键的Path键值以获取Office完整安装路径（若安装了Office，Word一定是必装的）
            //         // Read the Path key of the InstallRoot key under the Word key to get the full Office installation path (if you already have Office installed, Word is definitely required)
            //         RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey("16.0\\Word\\InstallRoot");
            //         RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey("16.0\\Word\\InstallRoot");
               
            //         if (assumedKey64?.GetValue("Path") == null && assumedKey32?.GetValue("Path") == null)
            //         {
            //             throw new Exception("Office已损坏");
            //         }
               
            //         // 从注册表中读取Office的安装路径，并用路径中是否有root字段来判断版本是否为Office 2019或Office 2021
            //         // Read the Office installation path from the registry and use the root field in the path to determine whether the version is Office 2019 or Office 2021
            //         // OSPP.vbs一般仍然在C:\Program Files\Microsoft Office\Office1X\下
            //         // OSPP.vbs is still generally under C:\Program Files\Microsoft Office\Office1X\
            //         officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
            //         if (officePath.Contains("root"))
            //         {
            //             // 把\root给”Trim“掉，就是OSPP.vbs的所在目录了
            //             // Remove the \root from the path, and you now have the OSPP.vbs directory
            //             officePath = officePath.Replace("\\root", string.Empty);
            //             officeVersion = "Office 2019/2021";
            //         }
            //         else
            //         {
            //             officeVersion = officePath != string.Empty ? "Office 2016" : "Not_Found";
            //         }
            //     }
            //     else if (officeBaseKey64?.OpenSubKey("15.0") != null || officeBaseKey32?.OpenSubKey("15.0") != null)
            //     {
            //         // 如果找到的子键为15.0，那么说明安装的为Office 2013
            //         // If the subkey found is 15.0, Office 2013 is installed
            //         RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey("15.0\\Word\\InstallRoot");
            //         RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey("15.0\\Word\\InstallRoot");
               
            //         if (assumedKey64?.GetValue("Path") == null || assumedKey32?.GetValue("Path") == null)
            //         {
            //             throw new Exception("Office已损坏");
            //         }
            //         officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
            //         officeVersion = officePath != string.Empty ? "Office 2013" : "Not_Found";
            //     }
            //     else if (officeBaseKey64?.OpenSubKey("14.0") != null || officeBaseKey32?.OpenSubKey("14.0") != null)
            //     {
            //         RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey("14.0\\Word\\InstallRoot");
            //         RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey("14.0\\Word\\InstallRoot");
               
            //         if (assumedKey64?.GetValue("Path") == null || assumedKey32?.GetValue("Path") == null)
            //         {
            //             throw new Exception("Office已损坏");
            //         }
            //         officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
            //         officeVersion = officePath != string.Empty ? "Office 2010" : "Not_Found";
            //     }
            //     else
            //     {
            //         // 如果都没有，说明要么键都存在，但是子键值都不存在（卸载了）；要么说明键都是null，也就是未安装或安装损坏
            //         // If neither is present, either both keys are present, but neither child key value is present (unloaded). Either the keys are null, which means that they are not installed or that the installation is broken
            //         officeVersion = "Not_Found";
            //     }
            //     osppPath = officePath != string.Empty ? officePath : "Not_Found";
            //     return true;
            // }
            // catch
            // {
            //     // 以上任何一个环节出现错误，则说明要么注册表出现问题，要么Office安装出现问题，那么显然OSPP.vbs的位置和Office的版本都是未定义
            //     // If any of the above links is wrong, it means that there is either a problem with the registry, or there is a problem with the Office installation, then obviously the location of OSPP.vbs and the version of Office are undefined
            //     osppPath = "Not_Found";
            //     officeVersion = "Not_Found";
            //     return false;
            // }
        }

        /// <summary>
        ///     <para>
        ///         该函数用于将Retail版本的Office转换为Volume版本，并输出一些相关信息
        ///     </para>
        ///     <para>
        ///         This function is used to convert the Retail version of Office to the Volume version and output some relevant information
        ///     </para>
        /// </summary>
        /// <param name="osppDirectory">
        ///     <para>
        ///         一个 <see cref="string"/> 值，应传入OSPP.vbs所在的目录 
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value should be passed in the directory where OSPP.vbs is located
        ///     </para>
        /// </param>
        /// <param name="convertStatus">
        ///     <para>
        ///         一个<see cref="ConvertStatus"/> 枚举值，指示在转换过程中函数内部的情况
        ///     </para>
        ///     <para>
        ///         A <see cref="ConvertStatus"/> enumeration indicating what is going on inside the function during the conversion process
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         一个 <see cref="bool"/> 值，<see langword="true"/> 则代表转换成功  
        ///     </para>
        ///     <para>
        ///         A <see cref="bool"/> value, and <see langword="true"/> represents a successful conversion
        ///     </para>
        /// </returns>
        public static bool ConvertToVOL(string osppDirectory, out ConvertStatus convertStatus)
        {
            // 首先通过OSPP.vbs读取对Office信息的查询结果
            // Firstly, the query results of Office information are read through OSPP.vbs
            string checkOutput = string.Empty;
            try
            {
                checkOutput = RunProcess(CSCRIPT, @"//Nologo ospp.vbs /dstatus", osppDirectory, true);
            }
            catch
            {
                // 出现错误，则将convertStatus枚举赋以转换错误“ConvertStatus.ConvertError”
                // There is an error, then transform convertStatus enumeration with as errors "convertStatus.ConvertError"
                convertStatus = ConvertStatus.ConvertError;
                return false;
            }


            // 由OPSS.vbs路径推导获取各版本Office Pro Plus的KMS证书
            // Derive KMS certificates for each version of Office Pro Plus from OPSS.vbs path
            try
            {
                // 截取"\Office1X"前的所有内容，并修改为"..\root\LicensesXX"作为KMS证书的正确目录
                // Truncate everything before "\Office1X" and change it to "..\root\LicensesXX" as the correct directory for the KMS certificate
                string[] osppDirectory_Array = osppDirectory.Split('\\');
                string licenseDirectory = string.Empty;
                for (int i = 0; i < osppDirectory_Array.Length - 2; i++)
                {
                    licenseDirectory += osppDirectory_Array[i] + "\\";
                }
                licenseDirectory += "root\\Licenses";

                string officeVer = string.Empty, officekey = string.Empty, visiokey = string.Empty;

                // OSPP.vbs如果是在“...\Mircrosoft Office\Office1X\”下，那么其证书就在“..\Microsoft Office\root\Licenses1X\”下
                if (osppDirectory.EndsWith("Office16\\"))
                {
                    licenseDirectory += "16\\";
                    // 当证书文件夹中出现有Office Pro Plus 2021 VL的证书时，则默认用户下载安装的是2021版本的Office
                    // When the certificate of Office Pro Plus 2021 VL appears in the certificate folder, the default user downloads and installs the 2021 version of Office

                    // * 旧版代码中的判断条件，现已被注释掉，原因是考虑到有些Office会安装多个密钥。
                    // * The judgment condition in the old code has been commented out, because some Office will install multiple keys.

                    if (/* checkOutput.Contains("Office 21") && */ File.Exists(licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms"))
                    {
                        officeVer = "2021";
                        officeProduct = "Office 2021";
                        officekey = officeKeys["Office 2021"]; visiokey = visioKeys["Visio 2021"];
                    }
                    // 当证书文件夹中没有2021的证书，但有2019的证书时，则默认用户下载安装的是2019版本的Office
                    // When there is no certificate for 2021 in the certificates folder, but there is a certificate for 2019, the default user downloads and installs the 2019 version of Office
                    else if (/* checkOutput.Contains("Office 19")                                      && */ 
                             !File.Exists(licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms") && 
                             File.Exists(licenseDirectory + "ProPlus2019VL_KMS_Client_AE-ppd.xrm-ms"))
                    {
                        officeVer = "2019";
                        officeProduct = "Office 2019";
                        officekey = officeKeys["Office 2019"]; visiokey = visioKeys["Visio 2019"];
                    }
                    else
                    {
                        officeVer = "2016";
                        officeProduct = "Office 2016";
                        officekey = officeKeys["Office 2016"]; visiokey = visioKeys["Visio 2016"];
                    }
                }
                // 同理上述
                // Same to the upon parts
                else if (osppDirectory.EndsWith("Office15\\"))
                {
                    licenseDirectory += "15\\";
                    officeVer = "2013";
                    officeProduct = "Office 2013";
                    officekey = officeKeys["Office 2013"]; visiokey = visioKeys["Visio 2013"];
                }
                else if (osppDirectory.EndsWith("Office14\\"))
                {
                    licenseDirectory += "14\\";
                    officeVer = "2010";
                    officeProduct = "Office 2010";
                    officekey = officeKeys["Office 2010"]; visiokey = visioKeys["Visio 2010"];
                }
                else
                {
                    // 如果都没有，那么发生了错误，要么是Office 2007或更低版本，那么不受支持
                    // If neither is available, then an error has occurred and either Office 2007 or lower is not supported
                    convertStatus = ConvertStatus.ConvertError;
                    return false;
                }

                // 通过InstallVolumeLicense函数来完成VL证书的安装，这是一个极其耗时的操作
                // The installation of the VL certificate is done via the InstallVolumeLicense function, which is an extremely time-consuming operation
                InstallVolumeLicense(licenseDirectory, osppDirectory, officekey, visiokey);

                // 执行完成
                // Completed
                convertStatus = ConvertStatus.AlreadyVOL;
                return true;
            }
            catch
            {
                convertStatus = ConvertStatus.ConvertError;
                return false;
            }
        }

        /// <summary>
        ///     <para>
        ///         该函数用以卸载所有已安装的密钥
        ///     </para>
        ///     <para>
        ///         This function is used to uninstall all installed keys
        ///     </para>
        /// </summary>
        /// <param name="osppDirectory">
        ///     <para>
        ///         需要传入一个 <see cref="string"/> 类型值，表示OSPP.vbs所在的目录
        ///     </para>
        ///     <para>
        ///         Requires passing in a <see cref="string"/> value indicating the directory where OSPP.vbs is located
        ///     </para>
        /// </param>
        public static void RemoveAllInstalledKeys(string osppDirectory)
        {
            // 匹配OSPP.vbs输出结果中---与---之间的内容
            // Match the content between --- in the OSPP.vbs output
            string pattern = @"(?<=-{39})(.*?)(?=-{39})";
            string chkInfo = RunProcess(CSCRIPT, @"//Nologo ospp.vbs /dstatus", osppDirectory, true);

            MatchCollection checkInfoCollection = Regex.Matches(chkInfo, pattern, RegexOptions.Singleline);
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
                    RunProcess(CSCRIPT, "//NoLogo ospp.vbs /unpkey:" + matchedkey.Value, osppDirectory, true);
                }
            }
        }

        /// <summary>
        ///     <para>
        ///         该函数用以执行对Office的Pro Plus VL证书的安装
        ///     </para>
        ///     <para>
        ///         This function performs the installation of the Office Pro Plus VL Licenses
        ///     </para>
        /// </summary>
        /// <param name="licenseDir">
        ///     <para>
        ///         一个 <see cref="string"/> 类型值，需要传入Pro Plus VL证书所在的目录（带反斜杠“\”）
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value that requires the directory of the Pro Plus VL certificate (with a backslash "\")
        ///     </para>
        /// </param>
        /// <param name="osppDir">
        ///     <para>
        ///         一个 <see cref="string"/> 类型值，需要传入OSPP.vbs所在的目录（带反斜杠“\”）
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value, passing in the directory of OSPP.vbs (with a backslash "\")
        ///     </para>
        /// </param>
        /// <param name="o_Key">
        ///     <para>
        ///         一个 <see cref="string"/> 类型值，需要传入对应的Office的KMS密钥
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> type value that requires passing in the corresponding Office KMS key
        ///     </para>
        /// </param>
        /// <param name="v_Key">
        ///     <para>
        ///         一个 <see cref="string"/> 类型值，需要传入对应的Visio的KMS密钥
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value that requires the corresponding Visio KMS key
        ///     </para>
        /// </param>
        private static void InstallVolumeLicense(string licenseDir, string osppDir, string o_Key, string v_Key)
        {
            // 调用Shared类的RunProcess函数以安装所有的Pro Plus VL版本证书
            // Call the RunProcess function of the Shared class to install all the Pro Plus VL certificates

            // 当public静态变量officeProduct为"Office 2021"时，安装Pro Plus 2021 VL的KMS证书
            // When the public static variable officeProduct is "Office 2021", install the KMS certificate of the Pro Plus 2021 VL
            if (officeProduct == "Office 2021")
            {
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlus2021VL_KMS_Client_AE-ul.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlus2021VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioPro2021VL_KMS_Client_AE-ppd.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioPro2021VL_KMS_Client_AE-ul.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioPro2021VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "pkeyconfig-office.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + o_Key, osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + v_Key, osppDir, true);
            }
            else if (officeProduct == "Office 2019")
            {
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlus2019VL_KMS_Client_AE-ppd.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlus2019VL_KMS_Client_AE-ul.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlus2019VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioPro2019VL_KMS_Client_AE-ppd.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioPro2019VL_KMS_Client_AE-ul.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioPro2019VL_KMS_Client_AE-ul-oob.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "pkeyconfig-office.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + o_Key, osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + v_Key, osppDir, true);
            }
            else if (officeProduct == "Office 2016")
            {
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlusVL_KMS_Client-ppd.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlusVL_KMS_Client-ul.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "ProPlusVL_KMS_Client-ul-oob.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioProVL_KMS_Client-ppd.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioProVL_KMS_Client-ul.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "VisioProVL_KMS_Client-ul-oob.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + "\"" + licenseDir + "pkeyconfig-office.xrm-ms\"", osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + o_Key, osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + v_Key, osppDir, true);
            }
            // 对于Office 2013和2010，其证书文件夹中没有证书，直接安装Pro Plus密钥即可
            // For Office 2013 and 2010, there are no certificates in the certificate folder, just install the Pro Plus key
            else
            {
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + o_Key, osppDir, true);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + v_Key, osppDir, true);
            }
        }

        #region 静态变量与常量区
        /// <summary>
        ///     <para>
        ///         一个由 <see cref="string"/> 对应 <see cref="string"/> 类型的字典，用于存储对应Office Pro Plus版本的密钥
        ///     </para>
        ///     <para>
        ///         A dictionary of type <see cref="string"/> to store the corresponding version of the Office key
        ///     </para>
        /// </summary>
        private static Dictionary<string, string> officeKeys = new Dictionary<string, string>()
        {
            {"Office 2021", "FXYTK-NJJ8C-GB6DW-3DYQT-6F7TH"},
            {"Office 2019", "NMMKJ-6RK4F-KMJVX-8D9MJ-6MWKP"},
            {"Office 2016", "XQNVK-8JYDB-WJ9W3-YJ8YR-WFG99"},
            {"Office 2013", "YC7DK-G2NP3-2QQC3-J6H88-GVGXT"},
            {"Office 2010", "VYBBJ-TRJPB-QFQRF-QFT4D-H3GVB"}
        };

        /// <summary>
        ///     <para>
        ///         一个由 <see cref="string"/> 对应 <see cref="string"/> 类型的字典，用于存储对应Visio版本的密钥
        ///     </para>
        ///     <para>
        ///         A dictionary of type <see cref="string"/> to store the corresponding Visio version of the key
        ///     </para>
        /// </summary>
        private static Dictionary<string, string> visioKeys = new Dictionary<string, string>()
        {
            {"Visio 2021", "KNH8D-FGHT4-T8RK3-CTDYJ-K2HT4"},
            {"Visio 2019", "9BGNQ-K37YR-RQHF2-38RQ3-7VCBB"},
            {"Visio 2016", "PD3PC-RHNGV-FXJ29-8JK7D-RJRJK"},
            {"Visio 2013", "C2FG9-N6J68-H8BTJ-BW3QX-RM3B3"},
            {"Visio 2010", "7MCW8-VRQVK-G677T-PDJCM-Q8TCP"}
        };

        /// <summary>
        ///     <para>
        ///         一个 <see cref="string"/> 类型的静态值，用以存储OSPP.vbs所在的目录
        ///     </para>
        ///     <para>
        ///         A static value of type <see cref="string"/> to store the directory where OSPP.vbs is located
        ///     </para>
        /// </summary>
        public static string osppPosition;
        /// <summary>
        ///     <para>
        ///         一个 <see cref="string"/> 类型的静态值，用以存储Office产品的版本名
        ///     </para>
        ///     <para>
        ///         A static value of type <see cref="string"/> to store the version name of the Office product
        ///     </para>
        /// </summary>
        public static string officeProduct;
        /// <summary>
        ///     <para>
        ///         一个 <see cref="bool"/> 类型的静态值，用以指示Office安装的核心位置是否已经被找到
        ///     </para>
        ///     <para>
        ///         A static value of type <see cref="bool"/> indicating whether the core location of the Office installation has been found
        ///     </para>
        /// </summary>
        public static bool isOfficeCoreFound = IsOfficePathFound(out osppPosition, out officeProduct);
        #endregion
    }
    /// <summary>
    ///     <para>
    ///         该 <see langword="enum"/> 类型用于指示在将Office版本转换为Volume过程中的转换情况
    ///     </para>
    ///     <para>
    ///         The <see langword="enum"/> type is used to indicate the conversion during the conversion of the Office version to Volume
    ///     </para>
    /// </summary>
    public enum ConvertStatus
    {
        /// <summary>
        ///     <para>
        ///         该值表示转换失败
        ///     </para>
        ///     <para>
        ///         This value indicates that the conversion failed
        ///     </para>
        /// </summary>
        ConvertError = 0x00,
        /// <summary>
        ///     <para>
        ///         该值表示（Office）已经为Volume版本
        ///     </para>
        ///     <para>
        ///         This value indicates that (Office) is already the Volume version
        ///     </para>
        /// </summary>
        AlreadyVOL = 0x02,
        /// <summary>
        ///     <para>
        ///         该值表示（Office）为零售版本
        ///     </para>
        ///     <para>
        ///         This value indicates that (Office) is the retail version
        ///     </para>
        /// </summary>
        RetailVersion = 0x04
    }
}