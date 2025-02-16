using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;  // 配合注册表读写操作  Works with registry read and write operations
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks
using static KMS_Activator.UI_Thread_Operations;  // 使用UI线程操作  Use UI thread operations

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
            {"Windows 11 Pro", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
            {"Windows 11 Pro N", "MH37W-N47XK-V7XM9-C7227-GCQG9" },
            {"Windows 11 Pro Education", "6TP4R-GNPTD-KYYHQ-7B7DP-J447Y" },
            {"Windows 11 Pro Education N", "YVWGF-BXNMC-HTQYQ-CPQ99-66QFC" },
            {"Windows 11 Education", "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2" },
            {"Windows 11 Education N", "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ" },
            {"Windows 11 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
            {"Windows 11 Enterprise N", "DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4" },
            {"Windows 11 Enterprise G", "YYVX9-NTFWV-6MDM3-9PT4T-4M68B" },
            {"Windows 11 Enterprise G N", "44RPN-FTY23-9VTTB-MP9BX-T84FV" },

            {"Windows 10 Pro", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
            {"Windows 10 Pro N", "MH37W-N47XK-V7XM9-C7227-GCQG9" },
            {"Windows 10 Pro Education", "6TP4R-GNPTD-KYYHQ-7B7DP-J447Y" },
            {"Windows 10 Pro Education N", "YVWGF-BXNMC-HTQYQ-CPQ99-66QFC" },
            {"Windows 10 Education", "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2" },
            {"Windows 10 Education N", "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ" },
            {"Windows 10 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
            {"Windows 10 Enterprise N", "DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4" },
            {"Windows 10 Enterprise G", "YYVX9-NTFWV-6MDM3-9PT4T-4M68B" },
            {"Windows 10 Enterprise G N", "44RPN-FTY23-9VTTB-MP9BX-T84FV" },
            {"Windows 10 Enterprise LTSC 2021", "M7XTQ-FN8P6-TTKYV-9D4CC-J462D" },
            {"Windows 10 Enterprise LTSC 2019", "M7XTQ-FN8P6-TTKYV-9D4CC-J462D" },
            {"Windows 10 Enterprise N LTSC 2021", "92NFX-8DJQP-P6BBQ-THF9C-7CG2H" },
            {"Windows 10 Enterprise N LTSC 2019", "92NFX-8DJQP-P6BBQ-THF9C-7CG2H" },
            {"Windows 10 Enterprise LTSB 2016", "DCPHK-NFMTC-H88MJ-PFHPY-QJ4BJ" },
            {"Windows 10 Enterprise N LTSB 2016", "QFFDN-GRT3P-VKWWX-X7T3R-8B639" },
            {"Windows 10 Enterprise LTSB 2015", "WNMTR-4C88C-JK8YV-HQ7T2-76DF9" },
            {"Windows 10 Enterprise LTSB N 2015", "2F77B-TNFGY-69QQF-B8YKP-D69TJ" },
            
            {"Windows 8.1 Pro", "GCRJD-8NW9H-F2CDX-CCM8D-9D6T9" },
            {"Windows 8.1 Pro N", "HMCNV-VVBFX-7HMBH-CTY9B-B4FXY" },
            {"Windows 8.1 Enterprise", "MHF9N-XY6XB-WVXMC-BTDCT-MKKG7" },
            {"Windows 8.1 Enterprise N", "TT4HM-HN7YT-62K67-RGRQJ-JFFXW" },

            {"Windows 8 Pro", "NG4HW-VH26C-733KW-K6F98-J8CK4" },
            {"Windows 8 Pro N", "XCVCF-2NXM9-723PB-MHCB7-2RYQQ" },
            {"Windows 8 Enterprise", "32JNW-9KQ84-P47T8-D8GGY-CWCK7" },
            {"Windows 8 Enterprise N", "JMNMF-RHW7P-DMY6X-RF3DR-X2BQT" },

            {"Windows 7 Professional", "FJ82H-XT6CR-J8D7P-XQJJ2-GPDD4" },
            {"Windows 7 Professional N", "MRPKT-YTG23-K7D7T-X2JMM-QY7MG" },
            {"Windows 7 Professional E", "W82YF-2Q76Y-63HXB-FGJG9-GF7QX" },
            {"Windows 7 Enterprise", "33PXH-7Y6KF-2VJC9-XBBR8-HVTHH" },
            {"Windows 7 Enterprise N", "YDRBP-3D83W-TY26F-D46B2-XCKRJ" },
            {"Windows 7 Enterprise E", "C29WB-22CC8-VJ326-GHFJW-H9DH4" },

            {"Windows Vista Business", "YFKBB-PQJJV-G996G-VWGXY-2V3X8" },
            {"Windows Vista Business N", "HMBQG-8H2RH-C77VX-27R82-VMQBT" },
            {"Windows Vista Enterprise", "VKK3X-68KWM-X2YGT-QR4M6-4BWMV" },
            {"Windows Vista Enterprise N", "VTC42-BM838-43QHV-84HX6-XJXKV" },

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

        //private static Dictionary<string, string> volKeys = new Dictionary<string, string>()
        //{
        //    {"Windows 11 Professional", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
        //    {"Windows 11 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
        //    {"Windows 10 Professional", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
        //    {"Windows 10 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
        //    {"Windows 8.1 Professional", "GCRJD-8NW9H-F2CDX-CCM8D-9D6T9" },
        //    {"Windows 8.1 Enterprise", "MHF9N-XY6XB-WVXMC-BTDCT-MKKG7" },
        //    {"Windows 7 Professional", "FJ82H-XT6CR-J8D7P-XQJJ2-GPDD4" },
        //    {"Windows 7 Enterprise", "33PXH-7Y6KF-2VJC9-XBBR8-HVTHH" },
        //    {"Windows Server 2022 Standard", "VDYBN-27WPP-V4HQT-9VMD4-VMK7H" },
        //    {"Windows Server 2022 Datacenter", "WX4NM-KYWYW-QJJR4-XV3QB-6VM33" },
        //    {"Windows Server 2019 Standard", "N69G4-B89J2-4G8F4-WWYCC-J464C" },
        //    {"Windows Server 2019 Datacenter", "WMDGN-G9PQG-XVVXX-R3X43-63DFG" },
        //    {"Windows Server 2016 Standard", "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY" },
        //    {"Windows Server 2016 Datacenter", "CB7KF-BWN84-R7R2Y-793K2-8XDDG" },
        //    {"Windows Server 2012 R2 Server Standard", "D2N9P-3P6X9-2R39C-7RTCD-MDVJX" },
        //    {"Windows Server 2012 R2 Datacenter", "W3GGN-FT8W3-Y4M27-J84CP-Q3VJ9"},
        //    {"Windows Server 2008 R2 Standard", "YC6KT-GKW9T-YTKYR-T4X34-R7VHC" },
        //    {"Windows Server 2008 R2 Datacenter", "74YFP-3QFB3-KQT8W-PMXWJ-7M648" },
        //    {"Windows Server 2008 R2 Enterprise", "489J6-VHDMP-X63PK-3K798-CPX3Y" }
        //};
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
            ShiftAwaitingAnimationEffectsTalker(1);

            string key = string.Empty;               
            foreach (string version in volKeys.Keys)
            {
                // 当读取到的产品名包含字典中的版本名且版本名中包含有产品名时
                // When the read product name contains the version name in the dictionary or the version name contains the product name
                if (version.Trim().Contains(WIN_VERSION) && WIN_VERSION.Trim().Contains(version))  
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
                                "您的 Windows 产品\"" + WIN_VERSION + "\"在转换为正式版时发生错误，请您手动转换为正式版后再尝试激活\n\n点击“确定“以返回主页面",
                                "发生错误",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                            );
                            return;
                        }

                        // 当转换成功时
                        // When the conversion is successful
                        MessageBoxResult msg = MessageBox.Show
                        (
                            "您的 Windows 产品\"" + WIN_VERSION + "\"已成功转换为正式版，请重新启动以保留更改。\n\n是否现在重启？",
                            "消息提示",
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
                    "您的 Windows 产品\"" + WIN_VERSION + "\"不受支持，请更换 Windows 版本或使用其他开发者的激活工具。\n\n点击“确定“以退出程序",
                    "发生错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }


            // 开始使用slmgr.vbs进行激活
            // Start using slmgr.vbs to activate

            // 首先写入VOL密钥
            // The VOL key is written first
            ShiftAwaitingAnimationEffectsTalker(2);
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
                    $"装载密钥：{key} 时发生错误。错误原因：{setVOL_Error.Message}。若您反复看到该消息，请联系Microsoft或在该项目的Github页中提交您的问题。",
                    "发生错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            // 开始配置连接KMS服务器
            // The configuration starts to connect to the KMS server
            ShiftAwaitingAnimationEffectsTalker(3);
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
                    $"连接KMS服务器时发生错误。错误原因：{setServer_Error.Message}。若您反复看到该消息，请检查网络或在该项目的页中提交您的问题。",
                    "发生错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            // 应用到系统激活
            // Apply to system activation
            ShiftAwaitingAnimationEffectsTalker(4);
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
                    ShiftAwaitingAnimationEffectsTalker(5);
                    MessageBox.Show
                    (
                        "已完成对 " + WIN_VERSION + " 产品的激活！",
                        "消息提示",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    ShiftAwaitingAnimationEffectsTalker(5);
                    MessageBoxResult result = MessageBox.Show
                    (
                        "已完成对 " + WIN_VERSION + " 产品的激活操作，但未能即时查询到成功，请重启计算机以更新激活状态。\n" +
                        "是否立即重启您的计算机？",
                        "消息提示",
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
                    $"执行激活 Windows 发生错误。错误原因：{apply_Error.Message}。若您反复看到该消息，请检查系统设置或在该项目的Github页中提交您的问题。",
                    "发生错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }
        }
    }
}