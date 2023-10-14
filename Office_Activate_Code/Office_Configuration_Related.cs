using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;  // 配合注册表读写操作  Works with registry read and write operations
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks

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
        /// <returns>
        ///     <para>
        ///         一个<see langword="bool"/>值，true表示已激活
        ///     </para>
        ///     <para>
        ///         A Boolean value, true indicates active
        ///     </para>
        /// </returns>
        public static bool IsOfficeActivated(string osppDirectory)
        {
        /*  ProcessStartInfo startCheckInfo = new ProcessStartInfo
            {
                FileName = "cscript.exe",
                WorkingDirectory = osppDirectory,
                Arguments = @"//Nologo ospp.vbs /dstatus",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            // 通过读取返回的输出来判断是否正确激活
            // The returned output is read to determine whether the activation is correct
            Process startCheck = new Process { StartInfo = startCheckInfo };
        */
            string checkInfo = string.Empty;
            try
            {
                checkInfo = RunProcess(CSCRIPT, @"//Nologo ospp.vbs /dstatus", osppDirectory, true);
            }
            catch (Exception check_Error)
            {
                /* 待补充的操作与行为 */
                return false;
            }

            if (checkInfo.Contains("activation successful")        ||
                checkInfo.Contains("0xC004F009")                   || 
                checkInfo.Contains("LICENSE STATUS:  ---LICENSED---"))
            {
                /* 待补充 */
                return true;
            }
            else
            {
                /* 待补充 */
                return false;
            }
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
        ///         一个<see langword="bool"/>值,true即代表已被正确获取 
        ///     </para>
        ///     <para>
        ///         A <see langword="bool"/> value, true indicates that it was retrieved correctly
        ///     </para>
        /// </returns>
        public static bool IsOfficePathFound(out string osppPath, out string officeVersion)
        {
            try
            {
                // 首先判断系统是32位还是64位环境,然后获取正确的分支
                // First determine whether the system is a 32-bit or 64-bit environment, and then get the correct branch
                RegistryKey regKey = Environment.Is64BitOperatingSystem                                         ? 
                                     RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64):
                                     RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                string officePath = string.Empty;
                RegistryKey? officeBaseKey64 = regKey.OpenSubKey("SOFTWARE\\Microsoft\\Office");
                RegistryKey? officeBaseKey32 = regKey.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Office");

                if (officeBaseKey64 == null && officeBaseKey32 == null)
                {
                    throw new Exception("Office尚未安装");
                }

                // 以下为具体的Office版本判断与从Office在注册表中提供的安装目录转化为OSPP.vbs所在目录的过程
                // The following is the specific Office version determination and the process of converting the installation directory provided by Office in the registry to the directory where OSPP.vbs is located
                if (officeBaseKey64?.OpenSubKey("16.0") != null || officeBaseKey32?.OpenSubKey("16.0") != null)
                {
                    RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey("16.0\\Word\\InstallRoot");
                    RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey("16.0\\Word\\InstallRoot");

                    if (assumedKey64?.GetValue("Path") == null && assumedKey32?.GetValue("Path") == null)
                    {
                        throw new Exception("Office已损坏");
                    }

                    // 从注册表中读取Office的安装路径，并用路径中是否有root字段来判断版本是否为Office 2019或Office 2021
                    // Read the Office installation path from the registry and use the root field in the path to determine whether the version is Office 2019 or Office 2021
                    // 当然，OSPP.vbs一般仍然在C:\Program Files\Microsoft Office\Office16\下
                    // Of course, OSPP.vbs is still generally under C:\Program Files\Microsoft Office\Office16\
                    officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
                    if (officePath.Contains("root"))
                    {
                        officePath = officePath.Replace("\\root", string.Empty);
                        officeVersion = "Office 2019/2021";
                        /* 待补充其他关于版本判断的操作 */
                        /* 版本为Office 2019或2021 */
                    }
                    else
                    {
                        officeVersion = "Office 2016";
                        /* 待补充其他关于版本判断的操作 */
                        /* 版本为Office 2016 */
                    }
                }
                else if (officeBaseKey64?.OpenSubKey("15.0") != null || officeBaseKey32?.OpenSubKey("15.0") != null)
                {
                    RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey("15.0\\Word\\InstallRoot");
                    RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey("15.0\\Word\\InstallRoot");

                    if (assumedKey64?.GetValue("Path") == null || assumedKey32?.GetValue("Path") == null)
                    {
                        throw new Exception("Office已损坏");
                    }
                    officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
                    officeVersion = "Office 2013";
                    /* 版本为Office 2013 */
                }
                else if (officeBaseKey64?.OpenSubKey("14.0") != null || officeBaseKey32?.OpenSubKey("14.0") != null)
                {
                    RegistryKey? assumedKey64 = officeBaseKey64?.OpenSubKey("14.0\\Word\\InstallRoot");
                    RegistryKey? assumedKey32 = officeBaseKey32?.OpenSubKey("14.0\\Word\\InstallRoot");

                    if (assumedKey64?.GetValue("Path") == null || assumedKey32?.GetValue("Path") == null)
                    {
                        throw new Exception("Office已损坏");
                    }
                    officePath = (assumedKey64 != null ? assumedKey64?.GetValue("Path") : assumedKey32?.GetValue("Path"))?.ToString() ?? string.Empty;
                    officeVersion = "Office 2010";
                    /* 版本为Office 2010 */
                }
                else
                {
                    officeVersion = "Office 2007 or lower";
                    /* Office 2007或更低,不支持 */
                }
                osppPath = officePath;
                return true;
            }
            catch (Exception found_Error)
            {
                /* 有可能是未安装或安装出现错误导致的,可以建议重新安装 */
                osppPath = "Not_Found";
                officeVersion = "Not_Found";
                return false;
            }
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
        ///         一个<see langword="string"/>值，应传入OSPP.vbs所在的目录 
        ///     </para>
        ///     <para>
        ///         A <see langword="string"/> value should be passed in the directory where OSPP.vbs is located
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
        ///         一个<see langword="bool"/>值，<see langword="true"/>则代表转换成功  
        ///     </para>
        ///     <para>
        ///         A <see langword="bool"/> value, and <see langword="true"/> represents a successful conversion
        ///     </para>
        /// </returns>
        public static bool ConvertToVOL(string osppDirectory, out ConvertStatus convertStatus)
        {
            // 首先通过OSPP.vbs读取对Office信息的查询结果
            // Firstly, the query results of Office information are read through OSPP.vbs
            string checkOutput = string.Empty;
        /*  ProcessStartInfo startCheckLicenseInfo = new ProcessStartInfo
            {
                    FileName = "cscript.exe",
                    WorkingDirectory = osppDirectory,
                    Arguments = @"//Nologo ospp.vbs /dstatus",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden
            };
            Process startCheckLicense = new Process { StartInfo = startCheckLicenseInfo };
        */
            try
            {
                checkOutput = RunProcess(CSCRIPT, @"//Nologo ospp.vbs /dstatus", osppDirectory, true);
            }
            catch (Exception checkLicense_Error)
            {
                /* 待补充的操作 */
                convertStatus = ConvertStatus.ConvertError;
                return false;
            }

            // 若读取的输出中已经有Volume字样，则代表已经是VOL版本了
            // If the output already contains the word Volume, it is already the VOL version
            if (checkOutput.ToUpper().Contains("VOLUME"))
            {

                convertStatus = ConvertStatus.AlreadyVOL;
                return true;
            }

            // 由OPSS.vbs路径推导获取各版本Office Pro Plus的KMS证书
            // Derive KMS certificates for each version of Office Pro Plus from OPSS.vbs path
            try
            {
                // 截取"\Office1X"前的所有内容，并修改为"..\root\LicenseXX"作为KMS证书的正确目录
                // Truncate everything before "\Office1X" and change it to ".. \root\LicenseXX" as the correct directory for the KMS certificate
                string[] osppDirectory_Array = osppDirectory.Split('\\');
                string licenseDirectory = string.Empty;
                for (int i = 0; i < osppDirectory_Array.Length - 2; i++)
                {
                    licenseDirectory += osppDirectory_Array[i] + "\\";
                }
                licenseDirectory += "root\\License";

                string officeversion = string.Empty, officekey = string.Empty, visiokey = string.Empty;
                if (osppDirectory.EndsWith("Office16"))
                {
                    licenseDirectory += "16\\";
                    if (checkOutput.Contains("Office 19"))
                    {
                        officeversion = "2019";
                        officekey = officeKeys["Office 2019"]; visiokey = visioKeys["Visio 2019"];
                    }
                    else
                    {
                        officeversion = "2016";
                        officekey = officeKeys["Office 2016"]; visiokey = visioKeys["Visio 2016"];
                    }
                }
                else if (osppDirectory.EndsWith("Office15"))
                {
                    licenseDirectory += "15\\";
                    officeversion = "2013";
                    officekey = officeKeys["Office 2013"]; visiokey = visioKeys["Visio 2013"];
                }
                else if (osppDirectory.EndsWith("Office14"))
                {
                    licenseDirectory += "14\\";
                    officeversion = "2010";
                    officekey = officeKeys["Office 2010"]; visiokey = visioKeys["Visio 2010"];
                }
                else
                {
                    /* 不受支持的版本 */
                    convertStatus = ConvertStatus.ConvertError;
                    return false;
                }
                // 调用Shared类的RunProcess函数以安装所有的Pro Plus VL版本证书
                // Call the RunProcess function of the Shared class to install all the Pro Plus VL certificates
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "ProPlusVL_KMS_Client-ppd.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "ProPlusVL_KMS_Client-ul.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "ProPlusVL_KMS_Client-ul-oob.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "VisioProVL_KMS_Client-ppd.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "VisioProVL_KMS_Client-ul.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "VisioProVL_KMS_Client-ul-oob.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inslic:" + licenseDirectory + "pkeyconfig-office.xrm-ms", osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + officekey, osppDirectory, false);
                RunProcess(CSCRIPT, "//NoLogo ospp.vbs /inpkey:" + visiokey, osppDirectory, false);
                
                /* 可编写一些告诉用户发生了什么的操作 */
                convertStatus = ConvertStatus.AlreadyVOL;
                return true;
            }
            catch (Exception ConvertError)
            {
                /* 待补充的操作和行为 */
                convertStatus = ConvertStatus.ConvertError;
                return false;
            }
        }
        #region 静态变量与常量区
        public static Dictionary<string, string> officeKeys = new Dictionary<string, string>()
        {
            {"Office 2021", "NMMKJ-6RK4F-KMJVX-8D9MJ-6MWKP"},
            {"Office 2019", "NMMKJ-6RK4F-KMJVX-8D9MJ-6MWKP"},
            {"Office 2016", "XQNVK-8JYDB-WJ9W3-YJ8YR-WFG99"},
            {"Office 2013", "YC7DK-G2NP3-2QQC3-J6H88-GVGXT"},
            {"Office 2010", "VYBBJ-TRJPB-QFQRF-QFT4D-H3GVB"}
        };

        public static Dictionary<string, string> visioKeys = new Dictionary<string, string>()
        {
            {"Visio 2021", "9BGNQ-K37YR-RQHF2-38RQ3-7VCBB"},
            {"Visio 2019", "9BGNQ-K37YR-RQHF2-38RQ3-7VCBB"},
            {"Visio 2016", "PD3PC-RHNGV-FXJ29-8JK7D-RJRJK"},
            {"Visio 2013", "C2FG9-N6J68-H8BTJ-BW3QX-RM3B3"},
            {"Visio 2010", "7MCW8-VRQVK-G677T-PDJCM-Q8TCP"}
        };
        #endregion
    }

    public enum ConvertStatus
    {
        ConvertError = 0x00,
        AlreadyVOL = 0x02,
        RetailVersion = 0x04
    }
}