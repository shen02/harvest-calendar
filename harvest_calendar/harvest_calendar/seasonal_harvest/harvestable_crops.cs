using HarvestCalendar.DailyHarvestInfo;
using HarvestCalendar.DataTypes;
using StardewValley;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;

// HarvestableCrops is an object that maps each day in a season (1 ~ 28) to a list of crops that are harvestable on that given day. 
internal class HarvestableCrops
{
    // The list representing the lists of harvestable crop in each location on each day. 
    // invariant: harvestableCrops.length is equal to the number of days in the game's seasons.
    public List<DailyHarvest> harvestableCrops;

    public HarvestableCrops()
    {

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
}