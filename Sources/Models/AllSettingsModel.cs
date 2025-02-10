namespace Activator.Models
{
    /// <summary>
    /// <para>程序所有设置对应的 Model</para>
    /// <para>Model for all settings of the program</para>
    /// <para>本类允许在外部被实例化，但并不推荐。相反，推荐使用 <see cref="AllSettingsModel.Instance"/> 来获取实例</para>
    /// <para>This class allows to be instantiated from outside, but it is not recommended. Instead, it is recommended to use <see cref="AllSettingsModel.Instance"/> to get the instance</para>
    /// </summary>
    public class AllSettingsModel
    {
        /// <summary>
        /// <para><see cref="AllSettingsModel"/> 的实例</para>
        /// <para>Instance of <see cref="AllSettingsModel"/></para>
        /// </summary>
        public static AllSettingsModel Instance { get; set; } = new AllSettingsModel();

        /// <summary>
        /// <para>是否选择了 Windows 激活模式</para>
        /// <para>Whether Windows activation mode is selected</para>
        /// </summary>
        public bool IsWindowsActivationMode { get; set; } = true;

        /// <summary>
        /// <para>是否选择了 Office 激活模式</para>
        /// <para>Whether Office activation mode is selected</para>
        /// </summary>
        public bool IsOfficeActivationMode { get; set; } = false;

        /// <summary>
        /// <para>（在线激活中）选择的服务器索引</para>
        /// <para>(Online activation) Selected server index</para>
        /// </summary>
        public int SelectedServerIndex { get; set; } = 0;

        /// <summary>
        /// <para>是否开启自动检查更新</para>
        /// <para>Whether to enable automatic update check</para>
        /// </summary>
        public bool IsUpdateCheckEnabled { get; set; } = true;

        /// <summary>
        /// <para>（在上一次退出程序时）是否是浅色主题模式</para>
        /// <para>(When the program exited last time) Whether it is in light theme mode</para>
        /// </summary>
        public bool IsLightThemeMode { get; set; } = true;

        /// <summary>
        /// <para>（手动在线激活中）选择的产品名称或版本索引</para>
        /// <para>(During manual online activation) Selected product name or edition index</para>
        /// </summary>
        public int SelectedProductNameOrEditionIndex { get; set; } = 0;

        /// <summary>
        /// <para>手动在线激活中的 KMS 激活服务器地址</para>
        /// <para>KMS activation server address during manual online activation</para>
        /// </summary>
        public string? ManualActivationServerAddress { get; set; }

        /// <summary>
        /// <para>手动在线激活中的批量激活密钥</para>
        /// <para>Volume activation key during manual online activation</para>
        /// </summary>
        public string? ManualActivationVolumeKey { get; set; }
    }
}
