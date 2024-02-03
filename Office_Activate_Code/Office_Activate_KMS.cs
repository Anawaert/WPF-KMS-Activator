using System;
using System.Windows;
using static KMS_Activator.Shared;  // 使用共享的功能代码  Use shared functional code blocks
using static KMS_Activator.Office_Configurator;  // 使用与Office信息与配置相关的功能代码

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
            if (!isOfficeCoreFound)
            {
                MessageBox.Show
                (
                    "无法推断Office核心配置库的路径，请检查您的Office的完整性或正确安装Office",
                    "抱歉",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
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
                    "您的Office已激活，无需再次激活",
                    "提示",
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
                        "您的Office尚未安装密钥，请检查您的Office是否已与Microsoft账户绑定激活。若是，请点击“是”退出；若不是，请单机“否”以继续",
                        "提示",
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
                        "您的Office已经安装了密钥，是否继续？",
                        "提示",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information
                    );
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }

            // 尝试将Office转为Volume版本
            // Try converting Office to the Volume version
            ConvertStatus status;
            bool isSuccess = ConvertToVOL(osppPosition, out status);
            if (isSuccess && (status == ConvertStatus.RetailVersion || status == ConvertStatus.AlreadyVOL))
            {
                try
                {
                    // 指定KMS服务器
                    // Specifying a KMS server
                    RunProcess
                    (
                        CSCRIPT,
                        @"//Nologo ospp.vbs /sethst:" + kmsServerName,
                        osppPosition,
                        true
                    );

                    // 执行激活命令
                    // Execute the activation command
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
                        $"激活过程中出现错误。错误原因：{ActOffice_Error.Message}。若重复出现错误，请检查系统配置或在该项目的Github主页的Issue页中提交您的问题",
                        "抱歉",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    return;
                }
            }
            // 再次读取输出信息以检查Office的激活情况
            // Read the output again to check Office activation
            if (IsOfficeActivated(osppPosition, out bool is_NoInstalledKey))
            {
                MessageBox.Show
                (
                    "您的Office已成功激活，请重启计算机以应用更改",
                    "恭喜",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            else
            {
                // 若输出信息中还显示没有安装密钥，则大概率表明Office已经自动完成了数字激活
                // If the output also shows no installation key, chances are that Office has automatically completed digital activation
                if (is_NoInstalledKey)
                {
                    MessageBox.Show
                    (
                        "您的Office应已与Microsoft账户绑定，并且已自动完成了数字激活。如情况不属实，请联系Microsoft或在该项目的Github主页的Issue页中提交您的问题",
                        "抱歉",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                }
                else
                {
                    MessageBox.Show
                    (
                        "您的Office未能即时激活，这可能是外部程序打断造成的，请重启程序或重启计算机后重试",
                        "抱歉",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
        }

        #region 静态变量与常量区

        #endregion
    }
}