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
using System.Windows.Forms;

namespace KMS_Activator
{
    internal class Office_Activator
    {
        internal void ActOffice(string kmsServerName)
        {
            if (!isOfficeCoreFound)
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

            bool isNoInstalledKey, activationCondition = IsOfficeActivated(osppPosition, out isNoInstalledKey);
            if (activationCondition)
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
            else
            {
                if (isNoInstalledKey)
                {
                    DialogResult result = Fms::MessageBox.Show
                    (
                        "您的Office尚未安装密钥，请检查您的Office是否已与Microsoft账户绑定激活。若是，请点击“是”退出；若不是，请单机“否”以继续",
                        "提示",
                        Fms::MessageBoxButtons.YesNo,
                        Fms::MessageBoxIcon.Information
                    );
                    if (result == DialogResult.Yes)
                    {
                        return;
                    }

                }
                else
                {
                    DialogResult result = Fms::MessageBox.Show
                    (
                        "您的Office已经安装了密钥，是否继续？",
                        "提示",
                        Fms::MessageBoxButtons.YesNo,
                        Fms::MessageBoxIcon.Information
                    );
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            ConvertStatus status;
            bool isSuccess = ConvertToVOL(osppPosition, out status);
            bool is_NoInstalledKey;
            if (isSuccess && (status == ConvertStatus.RetailVersion || status == ConvertStatus.AlreadyVOL))
            {
                try
                {
                    RunProcess
                    (
                        CSCRIPT,
                        @"//Nologo ospp.vbs /sethst:" + kmsServerName,
                        osppPosition,
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
                        osppPosition,
                        true
                    );
                }
                catch (Exception ActOffice_Error)
                {
                    /* 待补充 */
                    return;
                }
            }
            if (IsOfficeActivated(osppPosition, out is_NoInstalledKey))
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
                if (is_NoInstalledKey)
                {
                    Fms::MessageBox.Show
                    (
                        "您的Office应已与Microsoft账户绑定，并且由Office自己完成了数字激活。如情况不属实，请联系Microsoft或在Github上提交Issue",
                        "抱歉",
                        Fms::MessageBoxButtons.OK,
                        Fms::MessageBoxIcon.Information
                    );

                }
                else
                {
                    Fms::MessageBox.Show
                    (
                        "您的Office未能即时激活，这可能是外部程序打断造成的，请重试",
                        "抱歉",
                        Fms::MessageBoxButtons.OK,
                        Fms::MessageBoxIcon.Information
                    );
                }
            }
        }

        #region 静态变量与常量区

        #endregion
    }
}