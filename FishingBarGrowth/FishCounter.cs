using StardewValley;
using StardewValley.GameData.Objects;

namespace FishingBarGrowth;

/// <summary>
/// 鱼类统计工具类
/// </summary>
public static class FishCounter
{
    // 需要排除的物品ID: 海草(152), 绿藻(153), 白藻(157)
    private static readonly string[] AlgaeIds = { "152", "153", "157" };

    // 日志回调
    private static Action<string, bool>? _logCallback;

    /// <summary>
    /// 初始化日志回调
    /// </summary>
    public static void Initialize(Action<string, bool> logCallback)
    {
        _logCallback = logCallback;
    }

    /// <summary>
    /// 计算玩家钓到的有效鱼类总数
    /// </summary>
    /// <param name="excludeAlgae">是否排除藻类和海草</param>
    /// <param name="enableDebug">是否启用调试日志</param>
    /// <returns>有效鱼类总数</returns>
    public static int GetTotalFishCount(bool excludeAlgae = true, bool enableDebug = false)
    {
        if (Game1.player == null)
        {
            if (enableDebug)
                _logCallback?.Invoke("[FishCounter] 玩家对象为null", false);
            return 0;
        }

        if (enableDebug)
        {
            _logCallback?.Invoke($"[FishCounter] 开始统计鱼类, fishCaught字典大小: {Game1.player.fishCaught.Count()}", false);
        }

        int totalCount = 0;
        int validFishCount = 0;
        int excludedCount = 0;

        // 遍历玩家的钓鱼统计数据
        // 在1.6中,fishCaught是SerializableDictionary<string, int[]>
        // 需要使用Pairs属性来遍历
        foreach (var pair in Game1.player.fishCaught.Pairs)
        {
            string fishId = pair.Key;
            int[] stats = pair.Value;
            int count = stats[0]; // stats[0] 是捕获总数

            if (enableDebug)
            {
                _logCallback?.Invoke($"[FishCounter] 检查物品ID: {fishId}, 数量: {count}", false);
            }

            // 验证是否为有效的鱼类
            if (IsValidFish(fishId, excludeAlgae, enableDebug))
            {
                totalCount += count;
                validFishCount++;

                if (enableDebug)
                {
                    _logCallback?.Invoke($"[FishCounter] ✓ 有效鱼类: {fishId}, 数量: {count}", false);
                }
            }
            else
            {
                excludedCount++;

                if (enableDebug)
                {
                    _logCallback?.Invoke($"[FishCounter] ✗ 排除: {fishId}, 数量: {count}", false);
                }
            }
        }

        if (enableDebug)
        {
            _logCallback?.Invoke($"[FishCounter] 统计完成 - 总计: {totalCount}条, 有效种类: {validFishCount}, 排除种类: {excludedCount}", false);
        }

        return totalCount;
    }

    /// <summary>
    /// 判断物品ID是否为有效的鱼类
    /// </summary>
    /// <param name="itemId">物品ID</param>
    /// <param name="excludeAlgae">是否排除藻类</param>
    /// <param name="enableDebug">是否启用调试日志</param>
    /// <returns>是否为有效鱼类</returns>
    private static bool IsValidFish(string itemId, bool excludeAlgae, bool enableDebug = false)
    {
        // 检查是否需要排除藻类
        if (excludeAlgae && Array.Exists(AlgaeIds, id => id == itemId))
        {
            if (enableDebug)
                _logCallback?.Invoke($"[FishCounter]   -> 排除藻类: {itemId}", false);
            return false;
        }

        // 尝试从游戏数据中获取物品信息
        // 在1.6中使用ItemRegistry来获取物品数据
        // 注意: fishCaught中的ID可能已经是完整的qualified ID,也可能只是数字ID
        string qualifiedId = itemId.StartsWith("(") ? itemId : "(O)" + itemId;

        var itemData = StardewValley.ItemRegistry.GetDataOrErrorItem(qualifiedId);

        if (itemData == null)
        {
            if (enableDebug)
                _logCallback?.Invoke($"[FishCounter]   -> 无法获取物品数据: {qualifiedId}", false);
            return false;
        }

        if (enableDebug)
        {
            _logCallback?.Invoke($"[FishCounter]   -> 物品类型: {itemData.ObjectType ?? "null"}, 类别: {itemData.Category}", false);
        }

        // 检查ObjectType字段是否包含"Fish"
        if (itemData.ObjectType != null && itemData.ObjectType.Contains("Fish", StringComparison.OrdinalIgnoreCase))
        {
            if (enableDebug)
                _logCallback?.Invoke($"[FishCounter]   -> 匹配ObjectType=Fish", false);
            return true;
        }

        // 检查Category是否为-4 (鱼类的类别ID)
        if (itemData.Category == -4)
        {
            if (enableDebug)
                _logCallback?.Invoke($"[FishCounter]   -> 匹配Category=-4 (鱼类)", false);
            return true;
        }

        if (enableDebug)
            _logCallback?.Invoke($"[FishCounter]   -> 不是鱼类", false);

        return false;
    }

    /// <summary>
    /// 根据鱼类总数计算额外的像素增益
    /// </summary>
    /// <param name="totalFish">鱼类总数</param>
    /// <param name="fishPerPixel">每多少条鱼增加1像素</param>
    /// <returns>额外像素数</returns>
    public static int CalculateBonusPixels(int totalFish, int fishPerPixel)
    {
        if (fishPerPixel <= 0)
            return 0;

        return totalFish / fishPerPixel;
    }
}

