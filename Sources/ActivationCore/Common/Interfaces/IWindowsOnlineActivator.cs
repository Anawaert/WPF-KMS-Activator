namespace Activator
{
    /// <summary>
    /// <para>所有在线 Windows 激活后端类应当实现的接口，包含一系列在线激活必要的函数定义</para>
    /// <para>All Windows online activator classes should implement this interface, which contains a series of necessary functions for online activation</para>
    /// </summary>
    public interface IWindowsOnlineActivator
    {
        /// <summary>
        /// <para>设置 KMS 批量激活密钥</para>
        /// <para>Set the KMS volume activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>批量激活密钥</para>
        /// <para>Volume activation key</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功设置</para>
        /// <para>A <see langword="bool"/> value indicating whether the setting was successful</para>
        /// </returns>
        public bool SetActivationKey(string key);

        /// <summary>
        /// <para>设置 KMS 服务器地址</para>
        /// <para>Set the KMS server address</para>
        /// </summary>
        /// <param name="serverName">
        /// <para>KMS 服务器地址</para>
        /// <para>KMS server address</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功设置</para>
        /// <para>A <see langword="bool"/> value indicating whether the setting was successful</para>
        /// </returns>
        public bool SetKMSServer(string serverName);

        /// <summary>
        /// <para>应用激活</para>
        /// <para>Apply activation</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功激活</para>
        /// <para>A <see langword="bool"/> value indicating whether the activation was successful</para>
        /// </returns>
        public bool ApplyActivation();

        /// <summary>
        /// <para>移除激活</para>
        /// <para>Remove activation</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功移除</para>
        /// <para>A <see langword="bool"/> value indicating whether the removal was successful</para>
        /// </returns>
        public bool RemoveActivation();

        /// <summary>
        /// <para>注册手动续签任务</para>
        /// <para>Register manual renewal task</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功注册</para>
        /// <para>A <see langword="bool"/> value indicating whether the registration was successful</para>
        /// </returns>
        public bool RegisterManualRenewalTask();

        /// <summary>
        /// <para>移除手动续签任务</para>
        /// <para>Remove manual renewal task</para>
        /// </summary>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，指示是否已成功移除</para>
        /// <para>A <see langword="bool"/> value indicating whether the removal was successful</para>
        /// </returns>
        public bool RemoveManualRenewalTask();
    }
}
