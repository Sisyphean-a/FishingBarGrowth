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

    /// <summary>
    /// 计算玩家钓到的有效鱼类总数
    /// </summary>
    /// <param name="excludeAlgae">是否排除藻类和海草</param>
    /// <returns>有效鱼类总数</returns>
    public static int GetTotalFishCount(bool excludeAlgae = true)
    {
        if (Game1.player == null)
            return 0;

        int totalCount = 0;

        // 遍历玩家的钓鱼统计数据
        // 在1.6中,fishCaught是SerializableDictionary<string, int[]>
        // 需要使用Pairs属性来遍历
        foreach (var pair in Game1.player.fishCaught.Pairs)
        {
            string fishId = pair.Key;
            int[] stats = pair.Value;

            // 验证是否为有效的鱼类
            if (IsValidFish(fishId, excludeAlgae))
            {
                // stats[0] 是捕获总数
                totalCount += stats[0];
            }
        }

        return totalCount;
    }

    /// <summary>
    /// 判断物品ID是否为有效的鱼类
    /// </summary>
    /// <param name="itemId">物品ID</param>
    /// <param name="excludeAlgae">是否排除藻类</param>
    /// <returns>是否为有效鱼类</returns>
    private static bool IsValidFish(string itemId, bool excludeAlgae)
    {
        // 检查是否需要排除藻类
        if (excludeAlgae && Array.Exists(AlgaeIds, id => id == itemId))
            return false;

        // 尝试从游戏数据中获取物品信息
        // 在1.6中使用ItemRegistry来获取物品数据
        var itemData = StardewValley.ItemRegistry.GetDataOrErrorItem("(O)" + itemId);

        if (itemData == null)
            return false;

        // 检查ObjectType字段是否包含"Fish"
        if (itemData.ObjectType != null && itemData.ObjectType.Contains("Fish", StringComparison.OrdinalIgnoreCase))
            return true;

        // 如果没有ObjectType,返回false
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

