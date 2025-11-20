> **_\*Gasp!\*_ You found my Forgejo repo!**  
This is a mirror-backup repository for [Harvest-Day-Calendar](https://github.com/shen02/StardewValley-Harvest-Day-Calendar) and frequent manual sync cannot be guaranteed. If you find something  out-of-date, please be redirected to the repository linked above.

# Harvest Day Calendar

<a href="https://ibb.co/n8NNLVYz"><img src="https://i.ibb.co/3Y99C8jB/harvest-day-calendar.png" alt="harvest-day-calendar" border="0" /></a>

### Table of Contents
1. [Function](#function)
2. [Design](#design)
3. [Mod Installation](#installation)
4. [For Modders: If you're looking through my code...](#useful-snippets)
5. [Resources](#resources)


## Function
This programme serves as an injected modification to the game [Stardew Valley](https://www.stardewvalley.net/). 

Its purpose is to collect data on all player-planted crops among the game's 3 farmable locations, compute the crops' respective dates of harvest within the season, then render the data onto an in-game calendar of 28 days.

![Harvest-Components.png](https://camo.githubusercontent.com/e1895cb1b9c42e6e51e74cf50cfddeead7cd4a1a3c08f3c4c02652cdf90ab845/68747470733a2f2f692e706f7374696d672e63632f776a3774514353372f486172766573742d436f6d706f6e656e74732e706e67)

## Design


## Installation
Right [here](https://www.nexusmods.com/stardewvalley/mods/39419?tab=files)!

Please also refer to the [Harvest Day Calendar](https://www.nexusmods.com/stardewvalley/mods/39419) page on Nexus Mods for additional information regarding the program's behavior in the native game environment. 


## Useful-Snippets
If you're looking through my code, you might be looking for examples in...


|   	|   	|
|---	|---	|
|   Traversing nearby terrain blocks	|   [[getAllHarvestablesInLocation]](https://github.com/shen02/StardewValley-Harvest-Day-Calendar/blob/3e5985940d565f4d4d554d3570da4ea97a2652a0/HarvestDayCalendar/HarvestDayCalendar/model/seasonal_harvest/harvestable_crops.cs#L108)	|
|  Calculating time until harvest (there are quite a few edge cases)	|   [[getTimeUntilHarvest]](https://github.com/shen02/StardewValley-Harvest-Day-Calendar/blob/0a4a7575f5dc451d006fdbeb3a1d3a513e6efdf7/HarvestDayCalendar/HarvestDayCalendar/model/seasonal_harvest/harvestable_crops.cs#L126)	|
|  Rendering the blank calendar	|   [[draw]](https://github.com/shen02/StardewValley-Harvest-Day-Calendar/blob/3e5985940d565f4d4d554d3570da4ea97a2652a0/HarvestDayCalendar/HarvestDayCalendar/view/harvest_calendar_menu.cs#L186)	|
|  Rendering the hover menu	|  [[performHoverAction]](https://github.com/shen02/StardewValley-Harvest-Day-Calendar/blob/3e5985940d565f4d4d554d3570da4ea97a2652a0/HarvestDayCalendar/HarvestDayCalendar/view/harvest_calendar_menu.cs#L29) [[drawHoverMenu]](https://github.com/shen02/StardewValley-Harvest-Day-Calendar/blob/3e5985940d565f4d4d554d3570da4ea97a2652a0/HarvestDayCalendar/HarvestDayCalendar/view/harvest_calendar_menu.cs#L103)	|
|  Getting Item sprite by ID	|   [[drawHarvestIcons]](https://github.com/shen02/StardewValley-Harvest-Day-Calendar/blob/3e5985940d565f4d4d554d3570da4ea97a2652a0/HarvestDayCalendar/HarvestDayCalendar/view/harvest_calendar_menu.cs#L91)	|

## Resources

Below are some of the resources that I found helpful in the making of this program...

+ Stardew Valley Decompiled [[v1.5.6]](https://github.com/WeDias/StardewValley) [[v1.6]](https://github.com/Dannode36/StardewValleyDecompiled)
+ [SMAPI references](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs)
+ [Stardew Valley Modding Wiki](https://stardewvalleywiki.com/Modding:Index)
