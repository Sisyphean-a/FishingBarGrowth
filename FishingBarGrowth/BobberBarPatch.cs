using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using System.Reflection;

namespace FishingBarGrowth;

/// <summary>
/// BobberBar类的Harmony补丁
/// </summary>
[HarmonyPatch]
public static class BobberBarPatch
{
    private static ModConfig? _config;
    private static Action<string, bool>? _logDebug;

    /// <summary>
    /// 初始化补丁配置
    /// </summary>
    public static void Initialize(ModConfig config, Action<string, bool> logDebug)
    {
        _config = config;
        _logDebug = logDebug;
    }

    /// <summary>
    /// 手动指定要补丁的目标方法
    /// </summary>
    [HarmonyTargetMethod]
    public static MethodBase TargetMethod()
    {
        // 获取BobberBar的所有构造函数
        var constructors = typeof(BobberBar).GetConstructors();

        // 返回第一个构造函数(通常BobberBar只有一个公共构造函数)
        return constructors[0];
    }

    /// <summary>
    /// BobberBar构造函数的Postfix补丁
    /// 在游戏计算完钓鱼条高度后,添加基于捕获数量的额外高度
    /// </summary>
    [HarmonyPostfix]
    public static void Constructor_Postfix(BobberBar __instance)
    {
        try
        {
            // 检查配置和玩家
            if (_config == null || !_config.EnableMod || Game1.player == null)
                return;

            // 统计有效鱼类总数
            int totalFish = FishCounter.GetTotalFishCount(_config.ExcludeAlgae);

            // 计算额外像素
            int bonusPixels = FishCounter.CalculateBonusPixels(totalFish, _config.FishPerPixel);

            if (bonusPixels <= 0)
                return;

            // 使用反射获取私有字段 bobberBarHeight
            FieldInfo? heightField = AccessTools.Field(typeof(BobberBar), "bobberBarHeight");

            if (heightField == null)
            {
                _logDebug?.Invoke("无法找到 bobberBarHeight 字段!", true);
                return;
            }

            // 获取当前高度
            int currentHeight = (int)heightField.GetValue(__instance)!;
            int newHeight = currentHeight + bonusPixels;

            // 应用最大高度限制
            if (_config.MaxBarHeight > 0 && newHeight > _config.MaxBarHeight)
            {
                newHeight = _config.MaxBarHeight;
            }

            // 设置新高度
            heightField.SetValue(__instance, newHeight);

            // 输出调试信息
            if (_config.ShowDebugInfo)
            {
                _logDebug?.Invoke(
                    $"钓鱼条高度已修改: 原始={currentHeight}px, 奖励={bonusPixels}px (总鱼数={totalFish}), 最终={newHeight}px",
                    false
                );
            }
        }
        catch (Exception ex)
        {
            _logDebug?.Invoke($"应用钓鱼条补丁时出错: {ex.Message}", true);
        }
    }
}

