# Fishing Bar Growth (钓鱼条无限增长)

## 简介 / Description

这是一个《星露谷物语》(Stardew Valley) 模组,实现了基于捕获总量的钓鱼条无限增长机制。

A Stardew Valley mod that implements unlimited fishing bar growth based on total fish caught.

## 功能特性 / Features

- ✅ **无限增长**: 钓鱼条长度不再受等级限制,可以无限增长
- ✅ **产量驱动**: 每钓10条鱼(可配置),钓鱼条增加1像素
- ✅ **精准统计**: 自动过滤垃圾和藻类,只统计真正的鱼类
- ✅ **追溯既往**: 安装后立即获得之前捕获数量带来的加成
- ✅ **HUD显示**: 手持鱼竿时在屏幕左下角显示钓鱼统计信息
- ✅ **完全可配置**: 通过Generic Mod Config Menu进行配置
- ✅ **多语言支持**: 支持中文和英文

## 安装方法 / Installation

1. 安装 [SMAPI](https://smapi.io/)
2. (可选但推荐) 安装 [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098)
3. 将本模组解压到 `Stardew Valley/Mods` 文件夹
4. 运行游戏

## 配置选项 / Configuration

如果安装了Generic Mod Config Menu,可以在游戏内配置:

### 主要设置
- **启用模组**: 开启/关闭功能
- **每像素需要的鱼数**: 默认10条鱼增加1像素
- **最大钓鱼条高度**: 默认600像素,设为0表示无限制
- **排除藻类**: 是否将海草和藻类计入鱼类统计

### HUD显示设置
- **显示钓鱼HUD**: 手持鱼竿时显示统计信息
- **HUD水平位置**: 距离屏幕左边缘的距离(默认20像素)
- **HUD垂直位置**: 距离屏幕底部的距离(默认100像素)

### 调试设置
- **显示调试信息**: 在控制台显示详细信息

也可以手动编辑 `config.json` 文件。

## 工作原理 / How It Works

1. 使用Harmony库拦截钓鱼迷你游戏的初始化
2. 统计玩家钓到的所有有效鱼类(排除垃圾和藻类)
3. 根据配置计算额外像素: `额外像素 = 总鱼数 / 每像素鱼数`
4. 将额外像素添加到钓鱼条的基础高度上
5. 应用最大高度限制(如果设置)

## 技术细节 / Technical Details

- **目标类**: `StardewValley.Menus.BobberBar`
- **补丁类型**: Harmony Postfix
- **兼容性**: 客户端模组,不影响多人游戏
- **性能**: 仅在开始钓鱼时计算一次,性能影响可忽略

## 兼容性 / Compatibility

- ✅ Stardew Valley 1.5.6+
- ✅ SMAPI 3.18.0+
- ✅ 单人和多人游戏
- ✅ Windows / Linux / macOS
- ⚠️ 可能与大幅修改钓鱼机制的模组冲突

## 数据示例 / Examples

- 钓了 **50条鱼**: 钓鱼条增加 **5像素** (~0.6个等级)
- 钓了 **500条鱼**: 钓鱼条增加 **50像素** (~6个等级)
- 钓了 **5000条鱼**: 钓鱼条增加 **500像素** (超强!)

## 开源协议 / License

MIT License

## 作者 / Author

Xavier Nico

## 致谢 / Credits

- 基于技术分析文档实现
- 使用 [SMAPI](https://smapi.io/) 和 [Harmony](https://github.com/pardeike/Harmony)
- 集成 [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098)

