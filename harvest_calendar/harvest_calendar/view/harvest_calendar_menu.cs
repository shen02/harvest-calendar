using HarvestCalendar.Model.SeasonHarvestInfo;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley;
using Microsoft.Xna.Framework;
using HarvestCalendar.Model.DataTypes;
using HarvestCalendar.Model.DailyHarvestInfo;
using HarvestCalendar.Model.Translator;

namespace HarvestCalendar.View.Menu;

internal class HarvestCalendarMenu : Billboard
{
  protected const string BackgroundTexturePath = "LooseSprites\\Billboard";

  public Dictionary<int, Dictionary<FarmableLocationNames, List<Tuple<string, int>>>> harvestData;

  public HarvestCalendarMenu()
  {
    HarvestableCrops allHravestableCrops = new HarvestableCrops(calendarDays.Count);
    harvestData = HarvestablesTranslator.translate(Game1.dayOfMonth, allHravestableCrops);
  }

  public override void performHoverAction(int x, int y) { }

  // Background texture for the calendar
  protected void drawBackgroundTexture(SpriteBatch b)
  {
    Texture2D billboardTexture = Game1.temporaryContent.Load<Texture2D>(BackgroundTexturePath);

    b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
    b.Draw(billboardTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle(0, 198, 301, 198), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
  }

  // Draw title for the calendar
  protected void drawCalendarHeader(SpriteBatch b)
  {
    b.DrawString(Game1.dialogueFont, "Harvest Calendar <3", new Vector2((float)(this.xPositionOnScreen + 160), (float)(this.yPositionOnScreen + 80)), Game1.textColor);
  }

  // Draw each grid in the calendar
  protected void drawCalendarGrids(SpriteBatch b)
  {
    for (int index = 0; index < this.calendarDays.Count; ++index)
    {
      if (Game1.dayOfMonth > index + 1)
      {
        b.Draw(Game1.staminaRect, this.calendarDays[index].bounds, Color.Gray * 0.25f);

      }
      else if (Game1.dayOfMonth == index + 1)
      {
        int num = (int)(4.0 * (double)Game1.dialogueButtonScale / 8.0);
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), this.calendarDays[index].bounds.X - num, this.calendarDays[index].bounds.Y - num, this.calendarDays[index].bounds.Width + num * 2, this.calendarDays[index].bounds.Height + num * 2, Color.LightPink, 4f, false);
      }
    }
  }

  protected void drawHarvestIcons(SpriteBatch b)
  {
    for (int date = Game1.dayOfMonth; date <= this.calendarDays.Count; date++)
    {
      if (harvestData.ContainsKey(date))
      {
        string harvestIndex = harvestData[date].First().Value.First().Item1;

        var metadata = ItemRegistry.GetMetadata(harvestIndex);
        var data = metadata.GetParsedData();

        Texture2D texture = data.GetTexture();
        b.Draw(texture, new Rectangle(this.calendarDays[date - 1].bounds.X + 32, this.calendarDays[date - 1].bounds.Y + 50, this.calendarDays[date - 1].bounds.Width / 2, this.calendarDays[date - 1].bounds.Height / 2), data.GetSourceRect(), Color.White);
      }
    }
  }

  public override void draw(SpriteBatch b)
  {
    drawBackgroundTexture(b);
    drawCalendarHeader(b);
    drawCalendarGrids(b);
    drawHarvestIcons(b);
  }
}

