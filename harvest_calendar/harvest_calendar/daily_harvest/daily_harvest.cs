using HarvestCalendar.DataTypes;

namespace HarvestCalendar.DailyHarvestInfo;

// Daily harvest represents a dictionary that maps a list of harvestable crops with their quantities to the GameLocation to which they are planted.
// The object construtor takes in an enumeration of locationNames, which represents all farmable location in the game's given context.
// Invariant: GameLocation is one of: Farm, IslandWest, Greenhouse.
internal sealed class DailyHarvest
{
    private Dictionary<FarmableLocationNames, HashSet<CropWithQuantity>> dailyHarvest;

    public DailyHarvest()
    {
        dailyHarvest = new Dictionary<FarmableLocationNames, HashSet<CropWithQuantity>>();
        dictInit();
    }

    // Fills this.dailyHarvest with the current farmable location names each mapped to an empty list of CropWithQuantity. 
    private void dictInit()
    {
        // convert this.farmableLocationNames into a fixed length array. 
        FarmableLocationNames[] names = (FarmableLocationNames[])Enum.GetValues(typeof(FarmableLocationNames));

        for (int i = 0; i < names.Length; i++)
        {
            dailyHarvest.Add(names[i], new HashSet<CropWithQuantity>());
        }
    }

    public void addCrop(FarmableLocationNames locationName, CropWithQuantity crop)
    {
        dailyHarvest[locationName].Add(crop);
    }

    public void addCrop(FarmableLocationNames locationName, HashSet<CropWithQuantity> crop)
    {
        dailyHarvest[locationName].Concat(crop);
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