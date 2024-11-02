using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace DUWENINK.ConfigManagerHelper
{
    public class ConfigManager : IConfigManager
    {
        private readonly string _configPath;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<ConfigManager> _logger;
        private readonly ConfigManagerOptions _options;
        
        // Thread-safe cache for configurations
        private readonly ConcurrentDictionary<Type, object> _configCache;
        
        // File system watcher to monitor config file changes
        private readonly FileSystemWatcher _fileWatcher;

        public ConfigManager(IOptions<ConfigManagerOptions> options, ILogger<ConfigManager> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _configPath = _options.ConfigPath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _options.DefaultConfigDirectory);
            _jsonSerializerOptions = _options.JsonSerializerOptions ?? new JsonSerializerOptions { WriteIndented = true };
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configCache = new ConcurrentDictionary<Type, object>();

            try
            {
                Directory.CreateDirectory(_configPath);
                
                // Initialize file watcher
                _fileWatcher = new FileSystemWatcher(_configPath)
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                    Filter = "*.json",
                    EnableRaisingEvents = true
                };

                _fileWatcher.Changed += OnConfigFileChanged;
                _fileWatcher.Created += OnConfigFileChanged;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "无法初始化配置管理器：{_configPath}", _configPath);
                throw;
            }
        }

        public async Task SaveConfigAsync<T>(T config) where T : new()
        {
            try
            {
                string configPath = GetConfigPath<T>();
                
                // Update cache first
                _configCache.AddOrUpdate(typeof(T?), addValue: config, (_, _) => config);
                
                // Then save to file
                string json = JsonSerializer.Serialize(config, _jsonSerializerOptions);
                await File.WriteAllTextAsync(configPath, json, Encoding.UTF8);
                
                _logger.LogInformation("配置文件 {ConfigName} 成功保存到 {ConfigPath}", typeof(T).Name, configPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存配置文件 {ConfigName} 失败", typeof(T).Name);
                throw;
            }
        }

        public async Task<T> GetConfigAsync<T>(T defaultConfig = default) where T : new()
        {
            // Try to get from cache first
            if (_configCache.TryGetValue(typeof(T), out var cachedConfig))
            {
                return (T)cachedConfig;
            }

            string configPath = GetConfigPath<T>();
            try
            {
                T config;
                if (File.Exists(configPath))
                {
                    string json = await File.ReadAllTextAsync(configPath);
                    config = JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions) ?? new T();
                }
                else
                {
                    config = defaultConfig ?? new T();
                    await SaveConfigAsync(config);
                }

                // Add to cache
                _configCache.TryAdd(typeof(T), config);
                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置文件 {ConfigName} 失败", typeof(T).Name);
                throw;
            }
        }

        private void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Get the type name from filename
                string typeName = Path.GetFileNameWithoutExtension(e.FullPath);
                Type configType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == typeName);

                if (configType != null)
                {
                    // Remove from cache to force reload on next get
                    _configCache.TryRemove(configType, out _);
                    _logger.LogInformation("配置文件 {ConfigName} 已更改，缓存已清除", typeName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理配置文件更改事件失败");
            }
        }

        private string GetConfigPath<T>()
        {
            string fileName = _options.CustomFileName ?? $"{typeof(T).Name}.json";
            return Path.Combine(_configPath, fileName);
        }
    }
}
