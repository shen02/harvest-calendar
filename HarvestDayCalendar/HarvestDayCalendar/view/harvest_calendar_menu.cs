using HarvestCalendar.Model.SeasonHarvestInfo;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley;
using Microsoft.Xna.Framework;
using HarvestCalendar.Model.DataTypes;
using HarvestCalendar.Model.Translator;
using Microsoft.Xna.Framework.Input;

namespace HarvestCalendar.View.Menu;

internal class HarvestCalendarMenu : Billboard
{
  protected const string BackgroundTexturePath = "LooseSprites\\Billboard";
  protected const string Header = "Harvest Day <";

  private Dictionary<int, Dictionary<FarmableLocationNames, List<Tuple<string, int>>>> harvestData;
  private bool hasHarvestableItem = false;

  private int dateOfHover = 0;

  public HarvestCalendarMenu()
  {
    HarvestableCrops allHravestableCrops = new HarvestableCrops(calendarDays.Count);
    harvestData = HarvestablesTranslator.translate(Game1.dayOfMonth, allHravestableCrops);
  }


  public override void performHoverAction(int x, int y)
  {
    for (int index = 0; index < this.calendarDays.Count; index++)
    {
      if (calendarDays[index].bounds.Contains(x, y))
      {
        if (harvestData.ContainsKey(index + 1))
        {
          this.hasHarvestableItem = true;
          this.dateOfHover = index + 1;
        }
        else
        {
          this.hasHarvestableItem = false;
        }
      }

    }
  }

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
    b.DrawString(Game1.dialogueFont, Header, new Vector2((float)(this.xPositionOnScreen + 160), (float)(this.yPositionOnScreen + 80)), Game1.textColor);
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

  // Draw the sprite of the harvestable item
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

        this.drawHoverMenu(b);
      }
    }
  }

  // Draw the box of the hover menu
  private void drawHoverBox(SpriteBatch b, int X, int Y, int iconWidth, int iconDistance, int iconPerLine, int lineHeight, int mouseDistancePadding, int totalPadding)
  {
    int width = (iconWidth + iconDistance) * iconPerLine;
    int numberOfHarvest = harvestData[this.dateOfHover].Sum(item => item.Value.Count);
    int numberOfLocations = harvestData[this.dateOfHover].Count;
    int totalLines = numberOfLocations;

    foreach (var item in harvestData[this.dateOfHover])
    {
      if (item.Value.Count > iconPerLine)
      {
        totalLines += item.Value.Count / (iconPerLine - 1);
      }
      else
      {
        totalLines += 1;
      }
    }
    int height = totalLines * lineHeight + totalPadding * 2;
    IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), X + mouseDistancePadding, Y + mouseDistancePadding, width, height, Color.White);
  }

  // Draw the content of the hover menu
  private void drawHoverContent(SpriteBatch b, int X, int Y, int iconWidth, int iconDistance, int iconPerLine, int lineHeight, int totalPadding)
  {
    int line = 0;

    foreach (var item in harvestData[this.dateOfHover])
    {
      int icon = 0;

      Utility.drawBoldText(b, item.Key.ToString(), Game1.dialogueFont, new Vector2(X + totalPadding, Y + (10 + totalPadding) + (line * lineHeight) + (line * iconDistance / 2)), Color.Black, 0.5f);

      line += 1;

      foreach (var crop in item.Value)
      {
        var metadata = ItemRegistry.GetMetadata(crop.Item1);
        var data = metadata.GetParsedData();

        int icon_x = X + totalPadding + (icon * (iconWidth + iconDistance));
        int icon_y = Y + totalPadding + (line * lineHeight) + (line * iconDistance / 2);

        Texture2D texture = data.GetTexture();
        b.Draw(texture, new Rectangle(icon_x, icon_y, 30, 30), data.GetSourceRect(), Color.White);

        Utility.drawTinyDigits(crop.Item2, b, new Vector2(icon_x + (float)(iconWidth * 0.75), icon_y + (float)(iconWidth * 0.75)), 1.5f, 0, Color.White);

        if (icon < iconPerLine - 1)
        {
          icon += 1;
        }
        else
        {
          line += 1;
          icon = 0;
        }
      }
      line += 1;
    }

  }

  private void drawHoverMenu(SpriteBatch b)
  {
    if (this.hasHarvestableItem)
    {
      int X = Mouse.GetState().X;
      int Y = Mouse.GetState().Y;
      int mouseDistancePadding = 20;
      int contentPadding = 20;
      int totalPadding = mouseDistancePadding + contentPadding;
      int iconWidth = 32; // Height and width of each icon, taken from data.getSourceRect.Width.
      int iconDistance = 10; // Distance between each icon
      int iconPerLine = 6;

      int lineHeight = iconWidth + iconDistance - 10; // 

      this.drawHoverBox(b, X, Y, iconWidth, iconDistance, iconPerLine, lineHeight, mouseDistancePadding, totalPadding);
      this.drawHoverContent(b, X, Y, iconWidth, iconDistance, iconPerLine, lineHeight, totalPadding);
    }
  }

  public override void draw(SpriteBatch b)
  {
    drawBackgroundTexture(b);
    drawCalendarHeader(b);
    drawCalendarGrids(b);
    drawHarvestIcons(b);
    drawMouse(b);
  }
}

