using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace Activator.Models
{
    /// <summary>
    /// <para>对所有模型的操作工具类</para>
    /// <para>Utility class for all model operations</para>
    /// </summary>
    public class ModelOperation
    {
        /// <summary>
        /// <para>从指定的配置文件中获取配置信息</para>
        /// <para>Get configuration information from the specified configuration file</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>需要指定反序列化的 Model 的类型，支持 <see langword="class"/> 和 <see langword="new"/>() 类型的泛型</para>
        /// <para>Need to specify the type of Model to be deserialized, supports generics of type <see langword="class"/> and <see langword="new"/>()</para>
        /// </typeparam>
        /// <param name="configFilePath">
        /// <para>配置文件的路径</para>
        /// <para>Path to the configuration file</para>
        /// </param>
        /// <returns>
        /// <para>返回反序列化后的 Model 对象 <typeparamref name="T"/>，如果反序列化过程产生错误，则返回一个新的 Model 对象 <typeparamref name="T"/></para>
        /// <para>Returns the deserialized Model object <typeparamref name="T"/>, if an error occurs during deserialization, a new Model object <typeparamref name="T"/> is returned</para>
        /// </returns>
        public static T GetConfig<T>(string configFilePath) where T : class, new()
        {
            try
            {
                // 设置未知类型处理方式为 JsonNode
                // Set the unknown type handling method to JsonNode
                var options = new JsonSerializerOptions()
                {
                    UnknownTypeHandling = System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonNode
                };
                string? configContent = File.ReadAllText(configFilePath, Encoding.UTF8);
                T? modelObject = JsonSerializer.Deserialize<T>(configContent, options);
                Log.Logger.Information(modelObject is not null ?
                                       "Successfully got configuration {model} from {configFilePath}" :
                                       "Got NULL {model} from {configFilePath}",
                                       modelObject, configFilePath);
                return modelObject ?? new T();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to get configuration from {configFilePath}", configFilePath);
                return new T();
            }
        }

        /// <summary>
        /// <para>异步地从指定的配置文件中获取配置信息</para>
        /// <para>Asynchronously get configuration information from the specified configuration file</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>需要指定反序列化的 Model 的类型，支持 <see langword="class"/> 和 <see langword="new"/>() 类型的泛型</para>
        /// <para>Need to specify the type of Model to be deserialized, supports generics of type <see langword="class"/> and <see langword="new"/>()</para>
        /// </typeparam>
        /// <param name="configFilePath">
        /// <para>配置文件的路径</para>
        /// <para>Path to the configuration file</para>
        /// </param>
        /// <returns>
        /// <para>返回反序列化后的 <see cref="Task{TResult}"/> 包装的 Model 对象 <typeparamref name="T"/></para>
        /// <para>如果反序列化过程产生错误，则返回一个新的 Model 对象 <typeparamref name="T"/></para>
        /// <para>Returns the deserialized <see cref="Task{TResult}"/> wrapped Model object <typeparamref name="T"/></para>
        /// <para>If an error occurs during deserialization, a new Model object <typeparamref name="T"/> is returned</para>
        /// </returns>
        public static async Task<T> GetConfigAsync<T>(string configFilePath) where T : class, new()
        {
            try
            {
                // 设置未知类型处理方式为 JsonNode
                // Set the unknown type handling method to JsonNode
                var options = new JsonSerializerOptions()
                {
                    UnknownTypeHandling = System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonNode
                };
                using FileStream stream = File.OpenRead(configFilePath);
                T? modelObject = await JsonSerializer.DeserializeAsync<T>(stream, options);
                Log.Logger.Information(modelObject is not null ?
                                       "Successfully got configuration {model} from {configFilePath}" :
                                       "Got NULL {model} from {configFilePath}",
                                       modelObject, configFilePath);
                return modelObject ?? new T();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to get configuration from {configFilePath}", configFilePath);
                return new T();
            }
        }

        /// <summary>
        /// <para>保存配置信息到指定的配置文件中</para>
        /// <para>Save configuration information to the specified configuration file</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>需要指定序列化的 Model 的类型，支持 <see langword="class"/> 和 <see langword="new"/>() 类型的泛型</para>
        /// <para>Need to specify the type of Model to be serialized, supports generics of type <see langword="class"/> and <see langword="new"/>()</para>
        /// </typeparam>
        /// <param name="model">
        /// <para>需要保存的 Model 对象 <typeparamref name="T"/></para>
        /// <para>Model object <typeparamref name="T"/> to be saved</para>
        /// </param>
        /// <param name="configFilePath">
        /// <para>配置文件的路径</para>
        /// <para>Path to the configuration file</para>
        /// </param>
        /// <returns>
        /// <para>一个 <see langword="bool"/> 值，表示是否成功保存配置信息</para>
        /// <para>A <see langword="bool"/> value indicating whether the configuration information was saved successfully</para>
        /// </returns>
        public static bool SaveConfig<T>(T model, string configFilePath) where T : class, new()
        {
            try
            {
                // 设置缩进写入配置文件
                // Set indentation to write to the configuration file
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                string jsonContent = JsonSerializer.Serialize(model, options);
                File.WriteAllText(configFilePath, jsonContent, Encoding.UTF8);
                Log.Logger.Information("Successfully saved configuration {model} to {configFilePath}", model, configFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to save configuration {model} to {configFilePath}", model, configFilePath);
                return false;
            }
        }

        /// <summary>
        /// <para>异步地保存配置信息到指定的配置文件中</para>
        /// <para>Asynchronously save configuration information to the specified configuration file</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>需要指定序列化的 Model 的类型，支持 <see langword="class"/> 和 <see langword="new"/>() 类型的泛型</para>
        /// <para>Need to specify the type of Model to be serialized, supports generics of type <see langword="class"/> and <see langword="new"/>()</para>
        /// </typeparam>
        /// <param name="model">
        /// <para>需要保存的 Model 对象 <typeparamref name="T"/></para>
        /// <para>Model object <typeparamref name="T"/> to be saved</para>
        /// </param>
        /// <param name="configFilePath">
        /// <para>配置文件的路径</para>
        /// <para>Path to the configuration file</para>
        /// </param>
        /// <returns>
        /// <para>返回一个 <see cref="Task{TResult}"/> 包装的 <see langword="bool"/> 值，表示是否成功保存配置信息</para>
        /// <para>Returns a <see cref="Task{TResult}"/> wrapped <see langword="bool"/> value indicating whether the configuration information was saved successfully</para>
        /// </returns>
        public static async Task<bool> SaveConfigAsync<T>(T model, string configFilePath) where T : class, new()
        {
            try
            {
                // 启用缩进写入配置文件
                // Enable indentation to write to the configuration file
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                using FileStream stream = File.Create(configFilePath);
                await JsonSerializer.SerializeAsync(stream, model, options);
                Log.Logger.Information("Successfully saved configuration {model} to {configFilePath}", model, configFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to save configuration {model} to {configFilePath}", model, configFilePath);
                return false;
            }
        }
    }
}
