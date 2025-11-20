using HarvestCalendar.Model.DataTypes;
using HarvestCalendar.Model.HarvestableWithQuantity;
using Netcode;
using StardewValley;

namespace HarvestCalendar.Model.DailyHarvests;

// Daily harvest represents a dictionary that maps a list of harvestable crops with their quantities to the GameLocation to which they are planted.
// The object construtor takes in an enumeration of locationNames, which represents all farmable location in the game's given context.
// Invariant: GameLocation is one of: Farm, IslandWest, Greenhouse.
public class DailyHarvest<T, R> where T : Crop, INetObject<NetFields> where R : HarvestableWithQuantity<T>
{
    protected Dictionary<FarmableLocationNames, HashSet<R>> dailyHarvest;

    public DailyHarvest()
    {
        dailyHarvest = new Dictionary<FarmableLocationNames, HashSet<R>>();
    }

    public void addHarvestables(FarmableLocationNames locationName, HashSet<R> harvestable)
    {
        if (dailyHarvest.ContainsKey(locationName))
            dailyHarvest[locationName].UnionWith(harvestable);
        else
            dailyHarvest.Add(locationName, harvestable);
    }

    // Note: kind of a weird inverse-design
    public void addHarvestable(FarmableLocationNames locationName, HashSet<R> harvestable)
    {
        addHarvestables(locationName, new HashSet<R>(harvestable));
    }

    // Most likely won't be used in the context of this mod but created for data stucture design
    public void removeHarvestable(FarmableLocationNames locationName, R harvestable)
    {
        dailyHarvest[locationName].Remove(harvestable);
    }

    public HashSet<R> getHarvestableSetByLocation(FarmableLocationNames location)
    {
        return dailyHarvest[location];
    }

    public Dictionary<FarmableLocationNames, HashSet<R>> getAllHarvestables()
    {
        return this.dailyHarvest;
    }
}