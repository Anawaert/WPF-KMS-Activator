namespace Activator
{
    /// <summary>
    /// <para>所有在线 Office 激活后端类应当实现的接口，包含一系列在线激活必要的函数定义</para>
    /// <para>All online Office activator classes should implement this interface, which contains a series of necessary functions for online activation</para>
    /// </summary>
    public interface IOfficeOnlineActivator
    {
        /// <summary>
        /// <para>设置 Office 激活密钥</para>
        /// <para>Set Office activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>Office 批量激活密钥</para>
        /// <para>Office volume activation key</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示设置密钥是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the key is set successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool SetOfficeActivationKey(string key, string? osppDirectory);

        /// <summary>
        /// <para>设置 Visio 激活密钥</para>
        /// <para>Set Visio activation key</para>
        /// </summary>
        /// <param name="key">
        /// <para>Visio 批量激活密钥</para>
        /// <para>Visio volume activation key</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示设置密钥是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the key is set successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool SetVisioActivationKey(string key, string? osppDirectory);

        /// <summary>
        /// <para>设置在线激活的服务器地址</para>
        /// <para>Set the server address for online activation</para>
        /// </summary>
        /// <param name="serverName">
        /// <para>KMS 服务器地址</para>
        /// <para>KMS server address</para>
        /// </param>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示设置 KMS 服务器是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the KMS server is set successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool SetKMSServer(string serverName, string? osppDirectory);

        /// <summary>
        /// <para>对 Office/Visio 应用在线批量激活</para>
        /// <para>Apply online volume activation to Office/Visio</para>
        /// </summary>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示激活是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the activation is successful, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool ApplyActivation(string? osppDirectory);

        /// <summary>
        /// <para>移除 Office/Visio 应用的激活状态</para>
        /// <para>Remove the activation status of Office/Visio</para>
        /// </summary>
        /// <param name="osppDirectory">
        /// <para>Office Software Protection Platform (ospp.vbs) 文件所在目录</para>
        /// <para>Directory where Office Software Protection Platform (ospp.vbs) file is located</para>
        /// </param>
        /// <returns>
        /// <para>指示移除激活是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Indicates whether the removal of activation is successful, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool RemoveActivation(string? osppDirectory);

        /// <summary>
        /// <para>注册手动续签任务</para>
        /// <para>Register manual renewal task</para>
        /// <para>* 请不要调用此函数</para>
        /// <para>* Do not call this function</para>
        /// </summary>
        /// <returns>
        /// <para>注册手动续签任务是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Whether the manual renewal task is registered successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool RegisterManualRenewalTask();

        /// <summary>
        /// <para>移除手动续签任务</para>
        /// <para>Remove manual renewal task</para>
        /// <para>* 请不要调用此函数</para>
        /// <para>* Do not call this function</para>
        /// </summary>
        /// <returns>
        /// <para>移除手动续签任务是否成功，若成功则返回 <see langword="true"/></para>
        /// <para>Whether the manual renewal task is removed successfully, return <see langword="true"/> if successful</para>
        /// </returns>
        public bool RemoveManualRenewalTask();
    }
}
