using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using HarvestCalendar.View.Menu;
using StardewModdingAPI.Utilities;

internal sealed class harvestCalendar : Mod
{
    private KeybindList _menuTrigger { get; set; } = KeybindList.Parse("LeftShift + C");
    public override void Entry(IModHelper helper)
    {
        helper.Events.Input.ButtonsChanged += this.OnButtonPressed;
    }

    private void OnButtonPressed(object? sender, ButtonsChangedEventArgs e)
    {
        if (Context.IsWorldReady)
            if (_menuTrigger.JustPressed())
                Game1.activeClickableMenu = Game1.activeClickableMenu.GetType() == typeof(HarvestCalendarMenu) ? null : new HarvestCalendarMenu();
    }
}

