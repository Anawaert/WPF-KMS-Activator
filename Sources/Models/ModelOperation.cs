using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Activator.Models
{
    public class ModelOperation
    {
        public static T GetConfig<T>(string configFilePath) where T : class, new()
        {
            try
            {
                string? configContent = File.ReadAllText(configFilePath, Encoding.UTF8);
                T? modelObject = JsonSerializer.Deserialize<T>(configContent);
                return modelObject ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public static async Task<T> GetConfigAsync<T>(string configFilePath) where T : class, new()
        {
            try
            {
                using FileStream stream = File.OpenRead(configFilePath);
                T? configObject = await JsonSerializer.DeserializeAsync<T>(stream);
                return configObject ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public static bool SaveConfig<T>(T model, string configFilePath) where T : class, new()
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(model);
                File.WriteAllText(configFilePath, jsonContent, Encoding.UTF8);
                return true;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }
        
        public static async Task<bool> SaveConfigAsync<T>(T model, string configFilePath) where T : class, new()
        {
            try
            {
                using FileStream stream = File.Create(configFilePath);
                await JsonSerializer.SerializeAsync(stream, model);
                return true;
            }
            catch
            {
                // 记得写入日志
                return false;
            }
        }
    }
}
