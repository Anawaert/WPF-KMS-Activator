using System;
using System.Windows;
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks
using static KMS_Activator.Office_Configurator;  // 使用与Office信息与配置相关的功能代码
using static KMS_Activator.UI_Thread_Operations;

namespace KMS_Activator
{
    /// <summary>
    ///     <para>
    ///         该类主要用于执行激活Office的操作
    ///     </para>
    ///     <para>
    ///         This class is primarily used to perform actions that activate Office
    ///     </para>
    /// </summary>
    public class Office_Activator
    {
        /// <summary>
        ///     <para>
        ///         该函数用以激活Office
        ///     </para>
        ///     <para>
        ///         This function activates Office
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
        public void ActOffice(string kmsServerName)
        {
            ShiftAwaitingAnimationEffectsTalker(1);

            if (!isOfficeCoreFound)
            {
                MessageBox.Show
                (
                    "无法找到 Office 的核心库路径，请检查您的 Office 的完整性或重新安装 Office。",
                    "发生错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            // 获取Office的密钥安装情况与激活情况
            // Get Office key installation and activation
            bool isNoInstalledKey, activationCondition = IsOfficeActivated(osppPosition, out isNoInstalledKey);
            if (activationCondition)
            {
                MessageBox.Show
                (
                    "您的 Office 已激活，无需再次激活",
                    "消息提示",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }
            else
            {
                // 读取到没有安装密钥时
                // When no installation key is read
                if (isNoInstalledKey)
                {
                    MessageBoxResult result = MessageBox.Show
                    (
                        "您的 Office 尚未安装密钥，请检查您的 Office 是否已与 Microsoft 账户绑定激活。若是，请点击“是”以退出；若不是，请点击“否”以继续",
                        "消息提示",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information
                    );
                    if (result == MessageBoxResult.Yes)
                    {
                        return;
                    }

                }
                else
                {
                    MessageBoxResult result = MessageBox.Show
                    (
                        "您的 Office 已安装密钥，若想继续执行激活，则需要将这些密钥卸载，是否继续？",
                        "消息提示",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information
                    );
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    RemoveAllInstalledKeys(osppPosition);
                }
            }

            // 尝试将Office转为Volume版本
            // Try converting Office to the Volume version
            ShiftAwaitingAnimationEffectsTalker(2);

            ConvertStatus status;
            bool isSuccess = ConvertToVOL(osppPosition, out status);
            if (isSuccess && (status == ConvertStatus.RetailVersion || status == ConvertStatus.AlreadyVOL))
            {
                try
                {
                    // 指定KMS服务器
                    // Specifying a KMS server

                    ShiftAwaitingAnimationEffectsTalker(3);


                    RunProcess
                    (
                        CSCRIPT,
                        @"//Nologo ospp.vbs /sethst:" + kmsServerName,
                        osppPosition,
                        true
                    );

                    // 执行激活命令
                    // Execute the activation command

                    ShiftAwaitingAnimationEffectsTalker(4);

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
                    MessageBox.Show
                    (
                        $"激活过程中出现错误。错误原因：{ActOffice_Error.Message}。若重复出现错误，请检查系统配置或在该项目的Github页中提交您的问题。",
                        "发生错误",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }
            }
            // 再次读取输出信息以检查Office的激活情况
            // Read the output again to check Office activation
            if (IsOfficeActivated(osppPosition, out bool is_NoInstalledKey))
            {
                ShiftAwaitingAnimationEffectsTalker(5);
                MessageBoxResult msg = MessageBox.Show
                (
                    "您的 Office 已成功激活，请尽快重启计算机以应用更改。\n\n是否立即重启？",
                    "恭喜",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information
                );

                if (msg == MessageBoxResult.Yes)
                {
                    RunProcess("shutdown.exe", "/r /t 0", SYS32_PATH, true);
                    Application.Current.Shutdown();
                    return;
                }
            }
            else
            {
                // 若输出信息中还显示没有安装密钥，则大概率表明Office已经自动完成了数字激活
                // If the output also shows no installation key, chances are that Office has automatically completed digital activation
                if (is_NoInstalledKey)
                {
                    MessageBox.Show
                    (
                        "您的 Office 或与 Microsoft 账户绑定，且已自动完成了数字激活。如情况不实，请联系 Microsoft 或在该项目的Github页中提交您的问题。",
                        "消息提示",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                }
                else
                {
                    MessageBox.Show
                    (
                        "您的 Office 未能即时激活，未查询到有效的激活凭证。请重启程序或重启计算机后重试。",
                        "发生错误",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        #region 静态变量与常量区

        #endregion
    }
}