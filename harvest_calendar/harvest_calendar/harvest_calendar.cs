using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

    internal sealed class harvestCalendar : Mod
    {

    public override void Entry(IModHelper helper)
    {


        helper.Events.Display.RenderedWorld += this.OnRenderedActiveCalendar;
        helper.Events.Input.ButtonPressed += this.OnButtonPressed;


    }

        
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
  {
            if (e.Button == SButton.MouseMiddle)
            {
                Game1.activeClickableMenu = new harvestCalendarMenu();
            }
        }

        private void OnRenderedActiveCalendar(object? sender, RenderedWorldEventArgs e)
        {
        //IClickableMenu.drawTextureBox(e.SpriteBatch, Game1.mouseCursors, new Rectangle(100, 100, 500, 500), 100, 100, 100, 100, Color.White);
        e.SpriteBatch.DrawString(Game1.dialogueFont, "Hello", new Vector2((float)100, (float)100), Color.White);
        // Game1.activeClickableMenu = (IClickableMenu) new Billboard(false);
        //NumberSprite.draw(12345, Game1.spriteBatch, new Vector2(150, 150), Color.White, 1, 1, 1, 0);
        Utility.drawTinyDigits(12345, e.SpriteBatch, new Vector2(150,150), 2f, 1f, Color.White);
  
    }
}