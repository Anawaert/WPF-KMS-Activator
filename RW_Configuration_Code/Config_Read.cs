using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace KMS_Activator
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
        ///         该静态函数用于读取配置文件，并反序列化为特定的类型
        ///     </para>
        ///     <para>
        ///         This static function is used to read the configuration file and deserialize it to a specific type
        ///     </para>
        /// </summary>
        /// <typeparam name="T">
        ///     <para>
        ///         需要反序列化的类型，且无有参构造
        ///     </para>
        ///     <para>
        ///         The type to deserialize
        ///     </para>
        /// </typeparam>
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
        ///         反序列化的类型，默认为<see cref="Config_Type"/>
        ///     </para>
        ///     <para>
        ///         Deserialized type, defaults to <see cref="Config_Type"/>
        ///     </para>
        /// </returns>
        internal static T ReadConfig<T>(string configPath) where T : new()
        {
            try
            {
                string jsonContent = File.ReadAllText(configPath, Encoding.UTF8);
                T? configObject = JsonSerializer.Deserialize<T>(jsonContent);
                return configObject ?? new T();
            }
            catch 
            { 
                return new T(); 
            }
        }

        /// <summary>
        ///     <para>
        ///         该静态异步函数用于读取配置文件，并反序列化为特定的类型
        ///     </para>
        ///     <para>
        ///         This static asynchronous function is used to read the configuration file and deserialize it to a specific type
        ///     </para>
        /// </summary>
        /// <typeparam name="T">
        ///     <para>
        ///         需要反序列化的类型，且无有参构造
        ///     </para>
        ///     <para>
        ///         The type to deserialize
        ///     </para>
        /// </typeparam>
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
        ///         一个<see cref="Task{TResult}"/>类型，默认为<see cref="Config_Type"/>
        ///     </para>
        ///     <para>
        ///         A <see cref="Task{TResult}"/> type, defaults to <see cref="Config_Type"/>
        ///     </para>
        /// </returns>
        internal static async Task<T> ReadConfigAsync<T>(string configPath) where T : new()
        {
            try
            {
                using (FileStream stream = File.OpenRead(configPath))
                {
                    T? configObject = await JsonSerializer.DeserializeAsync<T>(stream);
                    return configObject ?? new T();
                }
            }
            catch 
            { 
                return new T();
            }
        }
    }
}
