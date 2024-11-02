# 发布到 NuGet 指南

本项目使用 GitHub Actions 自动发布到 NuGet。以下是发布新版本的步骤：

## 1. 设置 NuGet API Key

1. 登录 [NuGet.org](https://www.nuget.org/)
2. 在个人账户中创建一个新的 API Key
3. 在 GitHub 仓库中添加 Secret：
   - 进入仓库的 Settings > Secrets and variables > Actions
   - 创建新的 secret，名称为 `NUGET_API_KEY`
   - 值设置为你从 NuGet.org 获取的 API Key

## 2. 发布新版本

要发布新版本，只需要创建一个新的 tag 并推送到 GitHub：

```bash
# 更新版本号（在 .csproj 文件中）
# 创建新的 git tag
git tag v1.0.0

# 推送 tag 到 GitHub
git push origin v1.0.0
```

GitHub Actions 将自动：
1. 构建项目
2. 创建 NuGet 包
3. 发布到 NuGet.org

## 3. 版本号规范

- 版本号格式：v主版本.次版本.修订号
- 示例：v1.0.0, v1.0.1, v1.1.0, v2.0.0

## 4. 检查发布状态

1. 在仓库的 Actions 标签页查看工作流运行状态
2. 发布成功后，包将在 [NuGet.org](https://www.nuget.org/packages/DUWENINK.ConfigManagerHelper) 上可用

## 注意事项

- 确保在创建新 tag 前已更新 .csproj 文件中的版本号
- 每个版本号只能使用一次
- 发布后等待几分钟才能在 NuGet.org 上搜索到新版本
