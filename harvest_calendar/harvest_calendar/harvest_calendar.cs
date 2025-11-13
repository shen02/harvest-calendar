
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

internal sealed class harvestCalendar : Mod
{

    public override void Entry(IModHelper helper)
    {

        helper.Events.Input.ButtonPressed += this.OnButtonPressed;

    }

    private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
    {
        if (e.Button == SButton.MouseMiddle)
        {
            Game1.activeClickableMenu = new harvestCalendarMenu();
        }
    }

}