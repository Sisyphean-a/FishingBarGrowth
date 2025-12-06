using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Tools;

namespace FishingBarGrowth;

/// <summary>
/// 钓鱼统计HUD显示类
/// </summary>
public class FishingHUD
{
    private readonly ModConfig _config;
    private readonly Func<string> _getTranslation;

    public FishingHUD(ModConfig config, Func<string> getTranslation)
    {
        _config = config;
        _getTranslation = getTranslation;
    }

    /// <summary>
    /// 绘制HUD
    /// </summary>
    public void Draw(SpriteBatch spriteBatch)
    {
        // 检查是否启用HUD
        if (!_config.ShowFishingHUD || !_config.EnableMod)
            return;

        // 检查玩家是否手持鱼竿
        if (Game1.player?.CurrentTool is not FishingRod)
            return;

        // 获取统计数据 (HUD不需要调试日志,避免刷屏)
        int totalFish = FishCounter.GetTotalFishCount(_config.ExcludeAlgae, false);
        int bonusPixels = FishCounter.CalculateBonusPixels(totalFish, _config.FishPerPixel);
        int baseBarHeight = 96; // 默认基础高度
        int currentBarHeight = baseBarHeight + bonusPixels;

        // 应用最大高度限制
        if (_config.MaxBarHeight > 0 && currentBarHeight > _config.MaxBarHeight)
        {
            currentBarHeight = _config.MaxBarHeight;
        }

        // 计算显示位置
        int x = _config.HudXOffset;
        int y = Game1.uiViewport.Height - _config.HudYOffset;

        // 绘制半透明背景 (4行文本,每行32px,加上边距)
        DrawBackground(spriteBatch, x - 10, y - 10, 350, 140);

        // 绘制文本
        int lineHeight = 32;
        int currentY = y;

        // 标题
        DrawText(spriteBatch, "=== 钓鱼统计 ===", x, currentY, Color.Gold);
        currentY += lineHeight;

        // 总鱼数
        DrawText(spriteBatch, $"已钓鱼数: {totalFish} 条", x, currentY, Color.White);
        currentY += lineHeight;

        // 钓鱼条长度
        DrawText(spriteBatch, $"钓鱼条长度: {currentBarHeight} px", x, currentY, Color.LightGreen);
        currentY += lineHeight;

        // 额外增益
        string bonusText = bonusPixels > 0 
            ? $"  (基础: {baseBarHeight} + 奖励: {bonusPixels})" 
            : $"  (基础: {baseBarHeight})";
        DrawText(spriteBatch, bonusText, x, currentY, Color.LightBlue);
    }

    /// <summary>
    /// 绘制半透明背景
    /// </summary>
    private void DrawBackground(SpriteBatch spriteBatch, int x, int y, int width, int height)
    {
        // 创建1x1的纯色纹理
        Texture2D pixel = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.Black });

        // 绘制半透明黑色背景
        spriteBatch.Draw(
            pixel,
            new Rectangle(x, y, width, height),
            Color.Black * 0.7f
        );
    }

    /// <summary>
    /// 绘制文本
    /// </summary>
    private void DrawText(SpriteBatch spriteBatch, string text, int x, int y, Color color)
    {
        // 使用游戏的小字体
        SpriteFont font = Game1.smallFont;

        // 绘制阴影
        spriteBatch.DrawString(
            font,
            text,
            new Vector2(x + 2, y + 2),
            Color.Black * 0.8f
        );

        // 绘制文本
        spriteBatch.DrawString(
            font,
            text,
            new Vector2(x, y),
            color
        );
    }
}

