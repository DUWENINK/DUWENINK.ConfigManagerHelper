# 🎮 DUWENINK.ConfigManagerHelper

> 你的配置文件管理小助手！再也不用担心配置文件的烦恼了~ 

## 🎯 这是什么？

这是一个超级好用的配置管理工具！它就像是你的私人管家，帮你处理所有配置文件相关的琐事。无论是保存配置、读取配置，还是实时监控配置文件的变化，它都能轻松搞定！

## ✨ 特性

- 🚀 异步操作，性能杠杠的！
- 🔄 线程安全的配置缓存，再也不用担心并发问题
- 👀 实时监控配置文件变化，修改立即生效
- 🎯 类型安全的配置访问，告别强制类型转换
- 🎨 完全可自定义的序列化选项
- 📁 灵活的文件路径配置

## 📦 安装

```bash
# 使用 NuGet 包管理器
Install-Package DUWENINK.ConfigManagerHelper

# 或者使用 .NET CLI
dotnet add package DUWENINK.ConfigManagerHelper
```

## 🎮 如何使用

### 1️⃣ 注册服务

```csharp
services.Configure<ConfigManagerOptions>(options =>
{
    options.ConfigPath = "你的配置文件路径";  // 可选，默认在应用程序目录下的 ConfigurationFiles 文件夹
    options.JsonSerializerOptions = new JsonSerializerOptions 
    { 
        WriteIndented = true 
    };  // 可选
});

services.AddScoped<IConfigManager, ConfigManager>();
```

### 2️⃣ 定义你的配置类

```csharp
public class GameConfig
{
    public string PlayerName { get; set; }
    public int Level { get; set; }
    public bool IsMusicEnabled { get; set; }
}
```

### 3️⃣ 使用配置管理器

```csharp
public class GameService
{
    private readonly IConfigManager _configManager;

    public GameService(IConfigManager configManager)
    {
        _configManager = configManager;
    }

    public async Task SaveGameSettings()
    {
        var config = new GameConfig 
        { 
            PlayerName = "勇者",
            Level = 999,
            IsMusicEnabled = true
        };
        await _configManager.SaveConfigAsync(config);
    }

    public async Task LoadGameSettings()
    {
        // 如果配置文件不存在，将使用默认配置
        var config = await _configManager.GetConfigAsync<GameConfig>();
        Console.WriteLine($"欢迎回来，{config.PlayerName}！");
    }
}
```

## 🎯 为什么选择它？

- 😎 超级简单的 API，开箱即用
- 🚀 异步操作，性能优化
- 🔒 线程安全，并发无忧
- 👀 实时监控，配置修改立即生效
- 🎨 高度可定制，满足各种需求

## 🎮 实际应用场景

- 🎮 游戏设置管理
- ⚙️ 应用程序配置
- 🎨 用户偏好设置
- 📱 客户端缓存
- 🔧 开发工具配置

## 📝 注意事项

- 配置文件默认保存为 JSON 格式
- 默认配置文件路径在应用程序目录下的 ConfigurationFiles 文件夹
- 配置文件名默认使用配置类的名称（例如：GameConfig.json）

## 🎉 开源协议

MIT License - 随便用，不用负责！

---

Made with ❤️ by DUWENINK
