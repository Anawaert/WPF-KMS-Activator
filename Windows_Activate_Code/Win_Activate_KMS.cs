using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;  // 配合注册表读写操作  Works with registry read and write operations
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks
using System.Windows.Media;

namespace KMS_Activator
{
    /// <summary>
    ///     <para>
    ///         该类主要用于实现激活Windows的功能
    ///     </para>
    ///     <para>
    ///         This class is mainly used to realize the function of activating Windows
    ///     </para>
    /// </summary>
    public class Win_Activator
    {
        #region 静态变量与常量区  Region for static variable and constant
        /// <summary>
        ///     <para>
        ///         该泛型字典用以将VOL Key与版本号对应联系起来
        ///     </para>
        ///     <para>
        ///         This generic dictionary is used to associate VOL Key with version number correspondence
        ///     </para>
        /// </summary>
        /// <returns>
        ///     <para>
        ///         一个 <see cref="string"/> 类型值，即密钥
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value, which is a VOL key
        ///     </para>
        /// </returns>
        private static Dictionary<string, string> volKeys = new Dictionary<string, string>()
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
        #endregion

        /// <summary>
        ///     <para>
        ///         该函数主要用于激活Windows
        ///     </para>
        ///     <para>
        ///         This function is primarily used to activate Windows
        ///     </para>
        /// </summary>
        /// <param name="kmsServerName">
        ///     <para>
        ///         一个 <see cref="string"/> 类型值，需要传入目标KMS服务器的地址
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value that requires passing the address of the destination KMS server
        ///     </para>
        /// </param>
        public void ActWin(string kmsServerName)
        {
            //ChangeGroupBoxVisibility(Wpf::Visibility.Visible, mainWindow.winSteps_GroupBox);
            //ChangeLabelFontFamily("等线", mainWindow.winChechVerLabel);

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
                            MessageBox.Show
                            (
                                "您的Windows产品\"" + WIN_VERSION + "\"在转换为正式版时发生错误，请您手动转换为正式版后再试\n\n点击“确定“以返回主页面",
                                "抱歉",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                            );
                            return;
                        }

                        // 当转换成功时
                        // When the conversion is successful
                        MessageBoxResult msg = MessageBox.Show
                        (
                            "您的Windows产品\"" + WIN_VERSION + "\"已成功转换为正式版，请重新启动以保留更改\n\n是否现在重启？",
                            "提示",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information
                        );
                        if (msg == MessageBoxResult.Yes)
                        {
                            RunProcess("shutdown.exe", "/r /t 0", SYS32_PATH, true);
                            Application.Current.Shutdown();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    break;
                }
            }

            // 在遍历完字典后，若无对应的版本，则说明不支持激活
            // After traversing the dictionary, if no corresponding version exists, activation is not supported
            if (key == string.Empty)
            {
                MessageBox.Show
                (
                    "您的Windows产品\"" + WIN_VERSION + "\"不受支持，请更换Windows版本或使用其他开发者的（KMS）激活器\n\n点击“确定“以退出程序",
                    "抱歉",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }


            // 开始使用slmgr.vbs进行激活
            // Start using slmgr.vbs to activate

            // 首先写入VOL密钥
            // The VOL key is written first
            // ChangeLabelFontFamily("等线 Light", mainWindow.winChechVerLabel);
            // ChangeLabelFontFamily("等线", mainWindow.winInstallKeyLabel);
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
                MessageBox.Show
                (
                    $"装载密钥：{key}时发生错误。错误原因：{setVOL_Error.Message}。若您反复看到该消息，请联系Microsoft或在该项目的Github主页的Issue页中提交您的问题",
                    "抱歉",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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
                MessageBox.Show
                (
                    $"连接KMS服务器时发生错误。错误原因：{setServer_Error.Message}。若您反复看到该消息，请检查网络设置或在该项目的Github主页的Issue页中提交您的问题",
                    "抱歉",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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

                if (IsWinActivated())
                {
                    MessageBox.Show
                    (
                        "已完成对 " + WIN_VERSION + " 产品的激活！",
                        "恭喜",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show
                    (
                        "已完成对 " + WIN_VERSION + " 产品的激活操作，但未能即时查询到成功，请重新计算机以刷新激活状态。\n" +
                        "是否立即重启您的计算机？",
                        "提示",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Question
                    );
                    
                    if (result == MessageBoxResult.OK)
                    {
                        RunProcess("shutdown.exe", "/r /t 0", SYS32_PATH, true);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception apply_Error)
            {
                MessageBox.Show
                (
                    $"执行激活Windows发生错误。错误原因：{apply_Error.Message}。若您反复看到该消息，请检查系统设置或在该项目的Github主页的Issue页中提交您的问题",
                    "抱歉",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }
        }
    }
}