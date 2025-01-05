using System;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Activator
{
    /// <summary>
    ///     <para>
    ///         该静态类主要用于实现对配置文件的读写
    ///     </para>
    ///     <para>
    ///         This static class is mainly used to read and write configuration files
    ///     </para>
    /// </summary>
    internal static partial class ConfigOperations
    {
        /// <summary>
        ///     <para>
        ///         该静态函数用于将特定类型序列化为配置JSON文件
        ///     </para>
        ///     <para>
        ///         This static function is used to serialize a specific type into a configuration JSON file
        ///     </para>
        /// </summary>
        /// <param name="config">
        ///     <para>
        ///         需要序列化的源类型
        ///     </para>
        ///     <para>
        ///         The source type to serialize
        ///     </para>
        /// </param>
        /// <param name="configPath">
        ///     <para>
        ///         配置文件的绝对路径
        ///     </para>
        ///     <para>
        ///         Absolute path to the configuration file
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         一个<see cref="bool"/>值，指示是否写入成功
        ///     </para>
        ///     <para>
        ///         A <see cref="bool"/> value indicating whether the write was successful
        ///     </para>
        /// </returns>
        internal static bool WriteConfig(Config_Type config, string configPath)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(config);
                File.WriteAllText(configPath, jsonString, Encoding.UTF8);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        ///     <para>
        ///         该静态异步函数用于将特定类型序列化为配置JSON文件
        ///     </para>
        ///     <para>
        ///         This static asynchronous function is used to serialize a specific type into a configuration JSON file
        ///     </para>
        /// </summary>
        /// <param name="config">
        ///     <para>
        ///         需要序列化的源类型
        ///     </para>
        ///     <para>
        ///         The source type to serialize
        ///     </para>
        /// </param>
        /// <param name="configPath">
        ///     <para>
        ///         配置文件的绝对路径
        ///     </para>
        ///     <para>
        ///         Absolute path to the configuration file
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         一个<see cref="Task{TResult}"/>类型，<see cref="bool"/>类型值用于指示是否写入成功
        ///     </para>
        ///     <para>
        ///         A <see cref="Task{TResult}"/>, <see cref="bool"/> value is used to indicate whether the write was successful
        ///     </para>
        /// </returns>
        internal static async Task<bool> WriteConfigAsync(Config_Type config, string configPath)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(config);
                await File.WriteAllTextAsync(configPath, jsonString, Encoding.UTF8);
                return true;
            }
            catch { return false; }
        }
    }
}
