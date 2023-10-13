using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wpf = System.Windows;
using Fms = System.Windows.Forms;
using Microsoft.Win32;  // 配合注册表读写操作  Works with registry read and write operations
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks

namespace KMS_Activator
{
    internal class Win_Activator
    {
        internal void ActWin(string kmsServerName)
        {
            string key = string.Empty;               
            foreach (string version in volKeys.Keys)
            {
                // 当读取到的产品名包含字典中的版本名或版本名中包含有产品名时
                // When the read product name contains the version name in the dictionary or the version name contains the product name
                if (version.Contains(WIN_VERSION) || WIN_VERSION.Contains(version))  
                {                                                                    
                    // 从字典中获取对应版本名的VOL密钥
                    // Obtain the VOL key corresponding to the version name from the dictionary
                    key = volKeys[version];
                    // 当考虑到当前Windows版本可能是Evaluation版本时
                    // When considering that the current Windows version may be an Evaluation version
                    if (WIN_VERSION.ToUpper().Contains("EVALUATION"))  
                    {   
                        // 截取字典中的正式版产品名的最后一个单词 
                        // Take the last word of the official product name in the dictionary
                        string[] edition_to_arr = version.Split(' ');
                        string target_edition = edition_to_arr[edition_to_arr.Length - 1];
                        // 设置dism.exe使用的命令行参数，然后传递给它并获取输出（dism.exe用以将Evaluation版本转换为其他正式版）
                        // Set the command-line arguments used by dism.exe, then pass them to it and get the output (dism.exe is used to convert the Evaluation version to another standard version)
                        string target_args = "/online /set-edition:Server" + target_edition + " /productkey:" + key + " /accepteula";
                        string dism_output = RunProcess("dism.exe", target_args, SYS32_PATH, false);
                        // 当dism.exe的输出为空字符串时，说明转换失败
                        // If the output of dism.exe is an empty string, the conversion fails
                        if (dism_output == string.Empty)
                        {
                            Fms::MessageBox.Show
                            (
                                "您的Windows产品\"" + WIN_VERSION + "\"在转换为正式版时发生错误，请您手动转换为正式版后再试\n\n点击“确定“以退出程序",
                                "抱歉",
                                Fms::MessageBoxButtons.OK,
                                Fms::MessageBoxIcon.Error
                            );
                            Wpf::Application.Current.Shutdown();
                            return;
                            ///// 待补充 /////
                            ///// 可以考虑退出应用程序或进行其他引导 /////
                        }

                        /* 可补充GUI相关的一些行为操作 */
                        Fms::DialogResult msg = Fms::MessageBox.Show
                        (
                            "您的Windows产品\"" + WIN_VERSION + "\"已成功转换为正式版，请重新启动以保留更改\n\n是否现在重启？",
                            "提示",
                            Fms::MessageBoxButtons.YesNo,
                            Fms::MessageBoxIcon.Information
                        );
                        if (msg == Fms::DialogResult.Yes)
                        {
                            RunProcess("shutdown.exe", "/r /t 0", SYS32_PATH, true);
                            Wpf::Application.Current.Shutdown();
                            return;
                            ///// 待补充 /////
                            ///// 可以考虑提示重启或者其他操作 /////
                        }
                        // 顺利执行下来的话就可以先结束该函数的运行
                        // If it is successfully executed, it can end the running of the function now
                        break;
                    }
                    break;
                }
            }

            // 在遍历完字典后，若无对应的版本，则说明不支持激活
            // After traversing the dictionary, if no corresponding version exists, activation is not supported
            if (key == string.Empty)
            {
                Fms::MessageBox.Show
                (
                    "您的Windows产品\"" + WIN_VERSION + "\"不受支持，请更换Windows版本或使用其他开发者的（KMS）激活器\n\n点击“确定“以退出程序",
                    "抱歉",
                    Fms::MessageBoxButtons.OK,
                    Fms::MessageBoxIcon.Error
                );
                return;
                /* 待补充或与GUI相关的行为与操作 */
            }


            // 开始使用slmgr.vbs进行激活
            // Start using slmgr.vbs to activate

            // 首先写入VOL密钥
            // The VOL key is written first
            try
            {
                RunProcess
                (
                    CSCRIPT,
                    @"//Nologo slmgr.vbs /ipk " + key,
                    SYS32_PATH,
                    true
                );
            }
            catch (Exception setVOL_Error)
            {
                /* 待补充操作 */
                return;
            }

            // 开始配置连接KMS服务器
            // The configuration starts to connect to the KMS server
            try
            {
                RunProcess
                (
                    CSCRIPT,
                    @"//Nologo slmgr.vbs /skms " + kmsServerName,
                    SYS32_PATH,
                    true
                );
            }
            catch (Exception setServer_Error)
            {
                /* 待补充的操作 */
                return;
            }

            // 应用到系统激活
            // Apply to system activation
            try
            {
                RunProcess
                (
                    CSCRIPT,
                    @"//Nologo slmgr.vbs /ato",
                    SYS32_PATH,
                    true
                );
                Fms::MessageBox.Show
                (
                    "已完成对 "+ WIN_VERSION + " 产品的激活！",
                    "恭喜",
                    Fms::MessageBoxButtons.OK,
                    Fms::MessageBoxIcon.Information
                );
            }
            catch (Exception apply_Error)
            {
                /* 待补充的操作 */
                return;
            }

            mainWindow.Dispatcher.Invoke
            (
                () =>
                {
                    Animations_Related.MainW_SlideBack(mainWindow.mainGrid);
                }
            );
        }

    #region 静态变量与常量区  Region for static variable and constant
    /// <summary>
    ///     <para>
    ///         该泛型字典用以将VOL Key与版本号对应联系起来
    ///     </para>
    ///     <para>
    ///         This generic dictionary is used to associate VOL Key with version number correspondence
    ///     </para>
    /// </summary>
    /// <typeparam name="string">Windows版本名或产品名</typeparam>
    /// <typeparam name="string">VOL Keys</typeparam>
    /// <returns>一个<see langword="string"/>值，即密钥<br/>A string value, which is a VOL key</returns>
    internal static Dictionary<string, string> volKeys = new Dictionary<string, string>()
    {
        {"Windows 11 Professional", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
        {"Windows 11 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
        {"Windows 10 Professional", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
        {"Windows 10 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
        {"Windows 8.1 Professional", "GCRJD-8NW9H-F2CDX-CCM8D-9D6T9" },
        {"Windows 8.1 Enterprise", "MHF9N-XY6XB-WVXMC-BTDCT-MKKG7" },
        {"Windows 7 Professional", "FJ82H-XT6CR-J8D7P-XQJJ2-GPDD4" },
        {"Windows 7 Enterprise", "33PXH-7Y6KF-2VJC9-XBBR8-HVTHH" },
        {"Windows Server 2022 Standard", "VDYBN-27WPP-V4HQT-9VMD4-VMK7H" },
        {"Windows Server 2022 Datacenter", "WX4NM-KYWYW-QJJR4-XV3QB-6VM33" },
        {"Windows Server 2019 Standard", "N69G4-B89J2-4G8F4-WWYCC-J464C" },
        {"Windows Server 2019 Datacenter", "WMDGN-G9PQG-XVVXX-R3X43-63DFG" },
        {"Windows Server 2016 Standard", "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY" },
        {"Windows Server 2016 Datacenter", "CB7KF-BWN84-R7R2Y-793K2-8XDDG" },
        {"Windows Server 2012 R2 Server Standard", "D2N9P-3P6X9-2R39C-7RTCD-MDVJX" },
        {"Windows Server 2012 R2 Datacenter", "W3GGN-FT8W3-Y4M27-J84CP-Q3VJ9"},
        {"Windows Server 2008 R2 Standard", "YC6KT-GKW9T-YTKYR-T4X34-R7VHC" },
        {"Windows Server 2008 R2 Datacenter", "74YFP-3QFB3-KQT8W-PMXWJ-7M648" },
        {"Windows Server 2008 R2 Enterprise", "489J6-VHDMP-X63PK-3K798-CPX3Y" }
    };
        // Use the dictionary generic class to associate VOL Key with the version number
    #endregion

    }
}