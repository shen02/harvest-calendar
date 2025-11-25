using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using HarvestCalendar.View.Menu;
using HarvestCalendar.Controller.Translator;
using HarvestCalendar.Model.SeasonHarvestInfo;
using HarvestCalendar.Model.Config;
using HarvestCalendar.External;

internal sealed class harvestCalendar : Mod
{
    HarvestCalendarConfig menuTriggerSettings = new HarvestCalendarConfig();

    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.GameLaunched += this.onGameLaunched;
        helper.Events.Input.ButtonsChanged += this.OnButtonPressed;
    }

    // Handle data loading and integration with other mods
    private void onGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        this.loadConfigSettings();
    }

    private void loadConfigSettings()
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        // Read the current existing user settings
        menuTriggerSettings = this.Helper.ReadConfig<HarvestCalendarConfig>();

        configMenu.Register(
            mod: ModManifest,
            reset: () => menuTriggerSettings = new HarvestCalendarConfig(),
            save: () => this.Helper.WriteConfig(menuTriggerSettings)
        );

        configMenu.AddKeybindList(mod: ModManifest,
                                  getValue: () => menuTriggerSettings.menuTrigger,
                                  setValue: trigger => menuTriggerSettings.menuTrigger = trigger,
                                  name: () => "Calendar Hotkey(s)",
                                  tooltip: () => "Enter the key(s) you would like to use to open / close the calendar");
    }

    // Handle the actions of controlling the calendar menu
    private void OnButtonPressed(object? sender, ButtonsChangedEventArgs e)
    {
        if (Context.IsWorldReady)
        {
            if (menuTriggerSettings.menuTrigger.JustPressed())
            {
                HarvestableCrops allHravestableCrops = new HarvestableCrops(Game1.Date.TotalDays);
                HarvestCalendarMenu menu = new HarvestCalendarMenu(HarvestablesTranslator.translate(Game1.dayOfMonth, allHravestableCrops));

                Game1.activeClickableMenu = Game1.activeClickableMenu == null || Game1.activeClickableMenu.GetType() != typeof(HarvestCalendarMenu) ? menu : null;
            }
        }
    }
}

