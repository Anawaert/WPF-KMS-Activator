using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Activator
{
    /// <summary>
    /// <para>存放与实现所有与 Office 安装与配置相关信息的类</para>
    /// <para>Class that stores and implements all information related to Office installation and configuration</para>
    /// </summary>
    public class OfficeInfo
    {
        /// <summary>
        /// <para>已安装的 Office 版本名称，未安装或无法识别时默认为 <see cref="OfficeEditionName.Unsupported"/></para>
        /// <para>Installed Office edition name, default to <see cref="OfficeEditionName.Unsupported"/> when not installed or cannot be recognized</para>
        /// </summary>
        public static OfficeEditionName OfficeEdition { get; private set; }

        /// <summary>
        /// <para>已安装的 Visio 版本名称，未安装或无法识别时默认为 <see cref="VisioEditionName.Unsupported"/></para>
        /// <para>Installed Visio edition name, default to <see cref="VisioEditionName.Unsupported"/> when not installed or cannot be recognized</para>
        /// </summary>
        public static VisioEditionName VisioEdition { get; private set; }

        /// <summary>
        /// <para>指示程序是否已找到 Office 的安装核心目录</para>
        /// <para>Indicates whether the program has found the installation core directory of Office</para>
        /// </summary>
        public static bool IsOfficeCoreFound { get; private set; }

        /// <summary>
        /// <para>指示是否已检测到当前 Office 已处于激活状态</para>
        /// <para>Indicates whether the current Office has been detected to be activated</para>
        /// </summary>
        public static bool IsOfficeActivated { get; private set; }

        /// <summary>
        /// <para>指示当前 Office 是否仍未安装任何密钥</para>
        /// <para>Indicates whether the current Office has not yet installed any keys</para>
        /// </summary>
        public static bool IsOfficeNoKeys { get; private set; }

        /// <summary>
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录，未找到时为 <see langword="null"/></para>
        /// <para>Directory where the Office Software Protection Platform (ospp.vbs) file is located, <see langword="null"/> when not found</para>
        /// </summary>
        public static string? OsppDirectory { get; private set; }

        /// <summary>
        /// <para>存放批量授权证书的目录，未找到时为 <see langword="null"/></para>
        /// <para>Directory where the volume licensing certificate is stored, <see langword="null"/> when not found</para>
        /// </summary>
        public static string? LicenseDirectory { get; private set; }

        /// <summary>
        /// <para>当 Office 版本发生变化时应当触发的事件，便于向外部传递变化消息</para>
        /// <para>Event that should be triggered when the Office version changes, making it easier to pass change messages to the outside</para>
        /// </summary>
        public static event Action<OfficeEditionName>? OfficeEditionChanged;

        private static void FindOfficeCoreDirectory()
        {
            try
            {
                // 首先判断系统是32位还是64位环境,然后获取正确的分支
                // First determine whether the system is a 32-bit or 64-bit environment, and then get the correct branch
                RegistryKey regKey = Environment.Is64BitOperatingSystem ?
                                     RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                                     RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

                // 由于在安装时可能会出现在64位系统中安装了32位的情况，所以使用officeBaseKey__来区分32位与64位在注册表中的位置
                // officeBaseKey__ is used to distinguish the location of 32 bits from 64 bits in the registry because it may happen that 32 bits are installed on 64-bit systems at installation time
                RegistryKey? officeBaseKey64 = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Office");
                RegistryKey? officeBaseKey32 = regKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Office");

                // 未安装Office
                // Office is not installed
                if (officeBaseKey64 is null && officeBaseKey32 is null)
                {
                    return;
                }

                // 改用循环来判断Office的版本，因为注册表中的Office的版本可能不止一个
                // Use a loop to determine the version of Office, because there may be more than one version of Office in the registry
                string? osppDir = null;
                string[] officeInternalVersions = new string[] { "16.0", "15.0", "14.0" };
                foreach (string ver in officeInternalVersions)
                {
                    // 由于安装了Office，那么注册表中一定会有一个Word的键，所以可以通过Word键来判断是否安装了Office
                    // * 前提：该用户只安装了一个版本的Office
                    // Since Office is installed, there will definitely be a Word key in the registry, so you can determine if Office is installed through the Word key
                    // * Premise: The user has only installed one version of Office
                    bool isSubKeyExistent = (officeBaseKey64?.OpenSubKey(ver) is not null) || (officeBaseKey32?.OpenSubKey(ver) is not null);
                    bool isSubKeyHasWordKey = (officeBaseKey64?.OpenSubKey(ver + "\\Word") is not null) || (officeBaseKey32?.OpenSubKey(ver + "\\Word") is not null);
                    if (isSubKeyExistent && isSubKeyHasWordKey)
                    {
                        // 读取Word键下InstallRoot键的Path键值以获取Office完整安装路径
                        // Read the Path key of the InstallRoot key under the Word key to get the full Office installation path
                        RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey(ver + "\\Word\\InstallRoot");
                        RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey(ver + "\\Word\\InstallRoot");

                        // 若存在Word键，但是Path键值为空，那么说明Office安装出现问题
                        // If the Word key exists, but the Path key value is empty, it means that there is a problem with the Office installation
                        if (assumedKey64?.GetValue("Path") is null && assumedKey32?.GetValue("Path") is null)
                        {
                            return;
                        }

                        // 从注册表中读取Office的安装路径，并用路径中是否有root字段来判断版本是否为Office 2019或Office 2021
                        // Read the Office installation path from the registry and use the root field in the path to determine whether the version is Office 2019 or Office 2021
                        osppDir = (assumedKey64 is not null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString();
                        if (ver == "16.0" && (osppDir?.Contains("root") ?? false))
                        {
                            // 把"\root"给去掉，就是OSPP.vbs的所在目录了
                            // Remove the \root from the path, and you now have the OSPP.vbs directory
                            osppDir = osppDir.Replace("\\root", string.Empty);
                        }
                    }
                    // 若不存在Word键，那么说明该版本的Office并未安装
                    // If the Word key does not exist, the version of Office is not installed
                    else
                    {
                        continue;
                    }
                }
                OsppDirectory = osppDir;
                IsOfficeCoreFound = !string.IsNullOrEmpty(osppDir);
            }
            catch
            {
                OsppDirectory = null;
                IsOfficeCoreFound = false;
            }
        }

        private static void FindOfficeLicenseDirectory()
        {
            try
            {
                if (OsppDirectory is null)
                {
                    LicenseDirectory = null;
                    return;
                }

                string retVal = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /dstatus", 
                    OsppDirectory, 
                    true
                );
                // 截取"\Office1X"前的所有内容，并修改为"..\root\LicensesXX"作为KMS证书的正确目录
                // Truncate everything before "\Office1X" and change it to "..\root\LicensesXX" as the correct directory for the KMS certificate
                string[] osppDirectoryArray = OsppDirectory.Split('\\');
                string? licenseDirectory = null;
                for (int i = 0; i < osppDirectoryArray.Length - 2; i++)
                {
                    licenseDirectory += osppDirectoryArray[i] + "\\";
                }
                licenseDirectory += "root\\Licenses";

                // OSPP.vbs如果是在“...\Mircrosoft Office\Office1X\”下，那么其证书就在“..\Microsoft Office\root\Licenses1X\”下
                // If OSPP.vbs is under "...\Mircrosoft Office\Office1X\", then its certificate is under "..\Microsoft Office\root\Licenses1X\"
                OfficeEditionName officeEdition;
                VisioEditionName visioEdition;
                if (OsppDirectory.EndsWith("Office16\\"))
                {
                    licenseDirectory += "16\\";

                    // 当证书文件夹中出现有Office Pro Plus 2021 VL的证书时，则默认用户下载安装的是2021版本的Office
                    // When the certificate of Office Pro Plus 2021 VL appears in the certificate folder, the default user downloads and installs the 2021 version of Office
                    if (File.Exists(licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms"))
                    {
                        officeEdition = OfficeEditionName.Office_2021;
                        visioEdition = VisioEditionName.Visio_2021;
                    }

                    // 当证书文件夹中没有2021的证书，但有2019的证书时，则默认用户下载安装的是2019版本的Office
                    // When there is no certificate for 2021 in the certificates folder, but there is a certificate for 2019, the default user downloads and installs the 2019 version of Office
                    else if (!File.Exists(licenseDirectory + "ProPlus2021VL_KMS_Client_AE-ppd.xrm-ms") &&
                             File.Exists(licenseDirectory + "ProPlus2019VL_KMS_Client_AE-ppd.xrm-ms"))
                    {
                        officeEdition = OfficeEditionName.Office_2019;
                        visioEdition = VisioEditionName.Visio_2019;
                    }
                    else
                    {
                        officeEdition = OfficeEditionName.Office_2016;
                        visioEdition = VisioEditionName.Visio_2016;
                    }
                }
                else if (OsppDirectory.EndsWith("Office15\\"))
                {
                    licenseDirectory += "15\\";

                    officeEdition = OfficeEditionName.Office_2013;
                    visioEdition = VisioEditionName.Visio_2013;
                }
                else if (OsppDirectory.EndsWith("Office14\\"))
                {
                    licenseDirectory += "14\\";

                    officeEdition = OfficeEditionName.Office_2010;
                    visioEdition = VisioEditionName.Visio_2010;
                }
                else
                {
                    officeEdition = OfficeEditionName.Unsupported;
                    visioEdition = VisioEditionName.Unsupported;
                }

                OfficeEdition = officeEdition;
                VisioEdition = visioEdition;
                LicenseDirectory = licenseDirectory;
            }
            catch
            {
                OfficeEdition = OfficeEditionName.Unsupported;
                VisioEdition = VisioEditionName.Unsupported;
                LicenseDirectory = null;
            }
        }

        private static void CheckOfficeActivation()
        {
            try
            {
                if (OsppDirectory is null)
                {
                    IsOfficeActivated = false;
                    return;
                }
                string retVal = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /dstatus", 
                    OsppDirectory,
                    true
                );
                // 使用正则表达式来匹配OSPP.vbs的输出结果中---与---之间的内容，若为Office Pro Plus的VL版本且"LICENSE STATUS:  ---LICENSED---"，则说明已激活
                string pattern = @"(?<=-{39})(.*?)(?=-{39})";
                MatchCollection checkInfoCollection = Regex.Matches(retVal, pattern, RegexOptions.Singleline);
                foreach (Match match in checkInfoCollection)
                {
                    string proPlusPattern = @"Office(\d{2})?ProPlus(\d{4})?(VL)?";
                    if (Regex.Match(match.Value, proPlusPattern).Success && match.Value.Contains("LICENSE STATUS:  ---LICENSED---"))
                    {
                        IsOfficeActivated = true;
                        return;
                    }
                }

                IsOfficeActivated = false;
            }
            catch
            {
                IsOfficeActivated = false;
            }
        }

        private static void CheckOfficeInstalledKey()
        {
            try
            {
                if (OsppDirectory is null)
                {
                    IsOfficeNoKeys = true;
                    return;
                }

                string retVal = Utility.RunProcess
                (
                    Shared.Cscript, 
                    @"//Nologo ospp.vbs /dstatus", 
                    OsppDirectory,
                    true
                );
                IsOfficeNoKeys = retVal.Contains("No installed product keys detected");
            }
            catch
            {
                IsOfficeNoKeys = true;
            }
        }

        /// <summary>
        /// <para>初始化 Office 信息以获取当前的 Office 状态</para>
        /// <para>Initialize Office information to get the current Office status</para>
        /// </summary>
        public static void InitializeOfficeInfo()
        {
            FindOfficeCoreDirectory();
            FindOfficeLicenseDirectory();
            CheckOfficeActivation();
            CheckOfficeInstalledKey();

            OfficeEditionChanged?.Invoke(OfficeEdition);
        }

        /// <summary>
        /// <para>刷新 Office 信息以获取最新的 Office 状态</para>
        /// <para>Refresh Office information to get the latest Office status</para>
        /// </summary>
        public static void RefreshOfficeInfo() => InitializeOfficeInfo();
    }
}
