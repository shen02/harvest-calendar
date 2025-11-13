using HarvestCalendar.DailyHarvestInfo;
using HarvestCalendar.DataTypes;
using StardewValley;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;
using Force.DeepCloner;
using System.Reflection.Metadata;

namespace HarvestCalendar.SeasonHarvestInfo;
// HarvestableCrops is an object that maps each day in a season to a list of crops that are harvestable on that given day. 
internal class HarvestableCrops
{
    // invariant: harvestableCrops.size is equal to the number of days in the game's seasons.
    public Dictionary<int, DailyHarvest> harvestableCrops;

    public HarvestableCrops()
    {
        this.harvestableCrops = new Dictionary<int, DailyHarvest>();
    }


    // Returns a list of all planted, living crops in the given locatoin
    protected List<Crop> getAllCropsInLocation(GameLocation location)
    {

        List<Crop> allPlantedCrops = new List<Crop>();

        // condition acquired from decompiled game source v1.6
        foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures.Pairs)
        {
            if (pair.Value is HoeDirt { crop: not null })
            {
                allPlantedCrops.Add((pair.Value as HoeDirt).crop);
            }
        }

        return allPlantedCrops;
    }

    // Returns a FarmableLocatoinNames object (Farm, Greenhouse, IslandWest) accoding to the given GameLocation.
    // Invariant: The given GameLocatoin will be one of Farm, Greenhouse, and IslandWest.
    protected FarmableLocationNames getLocationNameByGameLocation(GameLocation location)
    {
        // Really not great design due to usage of string constants but, for now, oh well.
        switch (location.Name)
        {
            case "Farm":
                return FarmableLocationNames.Farm;
            case "Greenhouse":
                return FarmableLocationNames.Greenhouse;
            case "IslandWest":
                return FarmableLocationNames.IslandWest;
            default:
                return FarmableLocationNames.Farm;
        }
    }

    // Return the time remaining for the given crop to become harvestable.
    protected int getCropHarvestTime(Crop crop)
    {
        // sum from crop.currentPhase.Value (exclusive) to end and then plus daysInPhase
        int sum = crop.phaseDays.Sum();
        return sum - crop.phaseDays[crop.currentPhase.Value];
    }
}