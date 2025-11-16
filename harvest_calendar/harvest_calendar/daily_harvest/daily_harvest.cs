using HarvestCalendar.DataTypes;
using xTile.Dimensions;

namespace HarvestCalendar.DailyHarvestInfo;

// Daily harvest represents a dictionary that maps a list of harvestable crops with their quantities to the GameLocation to which they are planted.
// The object construtor takes in an enumeration of locationNames, which represents all farmable location in the game's given context.
// Invariant: GameLocation is one of: Farm, IslandWest, Greenhouse.
internal sealed class DailyHarvest
{
    public Dictionary<FarmableLocationNames, HashSet<CropWithQuantity>> dailyHarvest;

    public DailyHarvest()
    {
        dailyHarvest = new Dictionary<FarmableLocationNames, HashSet<CropWithQuantity>>();
    }

    public void addCrops(FarmableLocationNames locationName, HashSet<CropWithQuantity> crop)
    {
        if (dailyHarvest.ContainsKey(locationName))
            dailyHarvest[locationName].UnionWith(crop);
        else
            dailyHarvest.Add(locationName, crop);
    }

    // Most likely won't be used in the context of this mod but created for data stucture design
    public void removeCrop(FarmableLocationNames locationName, CropWithQuantity crop)
    {
        dailyHarvest[locationName].Remove(crop);
    }

    public HashSet<CropWithQuantity> getCropSetByLocation(FarmableLocationNames location)
    {
        return dailyHarvest[location];
    }
}