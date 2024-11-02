namespace DUWENINK.ConfigManagerHelper
{
    public interface IConfigManager
    {
        Task SaveConfigAsync<T>(T config) where T : new();
        Task<T?> GetConfigAsync<T>(T? defaultConfig = default) where T : new();
    }
}
