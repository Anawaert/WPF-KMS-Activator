using System.Runtime.ExceptionServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Fms = System.Windows.Forms;
using Microsoft.Win32;  // 配合注册表读写操作  Works with registry read and write operations
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks
using static KMS_Activator.Office_Configurator;

namespace KMS_Activator
{
    internal class Office_Activator
    {
        internal void ActOffice(string kmsServerName)
        {

        /*  ProcessStartInfo startSetServerInfo = new ProcessStartInfo
            {
                FileName = CSCRIPT,
                WorkingDirectory = string.Empty,  
                Arguments = "//Nologo ospp.vbs /sethst:" + kmsServerName,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            ProcessStartInfo startApplyInfo = new ProcessStartInfo
            {
                FileName = CSCRIPT,
                WorkingDirectory = string.Empty,  
                Arguments = "//Nologo ospp.vbs /act",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };  */

            string ospp_root, office_ver; 
            if (!IsOfficePathFound(out ospp_root, out office_ver))
            {
                Fms::MessageBox.Show
                (
                    "无法推断Office核心配置库的路径，请检查您的Office的完整性或正确安装Office",
                    "抱歉",
                    Fms::MessageBoxButtons.OK,
                    Fms::MessageBoxIcon.Information
                );
                return;
            }

            mainWindow.Dispatcher.Invoke
            (
                () => 
                { 
                    mainWindow.officeVersion_Label.Content = office_ver; 
                }
            );

            if (IsOfficeActivated(ospp_root))
            {
                Fms::MessageBox.Show
                (
                    "您的Office已激活，无需再次激活",
                    "提示",
                    Fms::MessageBoxButtons.OK,
                    Fms::MessageBoxIcon.Information
                );
                return;
            }

            ConvertStatus status;
            bool isSuccess = ConvertToVOL(ospp_root, out status);
            if (isSuccess && (status == ConvertStatus.RetailVersion || status == ConvertStatus.AlreadyVOL))
            {
                try
                {
                /*  startSetServerInfo.WorkingDirectory = ospp_root;
                    Process startSetServer = new Process { StartInfo = startSetServerInfo };
                    startSetServer.Start();
                    /* 可以考虑读取一下输出信息 
                    startSetServer.WaitForExit();  */
                    RunProcess
                    (
                        CSCRIPT,
                        @"//Nologo ospp.vbs /sethst:" + kmsServerName,
                        ospp_root,
                        true
                    );

                    /*  startApplyInfo.WorkingDirectory = ospp_root;
                        Process startApply = new Process { StartInfo = startApplyInfo };
                        startApply.Start();
                        /*同理
                        startApply.WaitForExit(); */
                    RunProcess
                    (
                        CSCRIPT,
                        @"//Nologo ospp.vbs /act",
                        ospp_root,
                        true
                    );
                }
                catch (Exception ActOffice_Error)
                {
                    /* 待补充 */
                    return;
                }
            }
            if (IsOfficeActivated(ospp_root))
            {
                Fms::MessageBox.Show
                (
                    "您的Office已成功激活，请重启计算机以应用更改",
                    "恭喜",
                    Fms::MessageBoxButtons.OK,
                    Fms::MessageBoxIcon.Information
                );
            }
            else
            {
                Fms::MessageBox.Show
                (
                    "您的Office未能即时激活，请重试",
                    "抱歉",
                    Fms::MessageBoxButtons.OK,
                    Fms::MessageBoxIcon.Information
                );
            }

            mainWindow.Dispatcher.Invoke
            (
                () =>
                {
                    Animations_Related.MainW_SlideBack(mainWindow.mainGrid);
                }
            );
        }

        #region 静态变量与常量区

        #endregion
    }
}