using StardewValley;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;

// HarvestableCrops is an object that maps each day in a season (1 ~ 28) to a list of crops that are harvestable on that given day. 
internal class HarvestableCrops
{
    public Dictionary<int, List<Crop>> harvestableCrops;

    // locationKey maps each day in a season to an integer array that provides information on the indices of harvestable crops in the corresponding 
    // 
    protected Dictionary<int, int[]> locationKey;
    protected string[] farmableLocationNames = { "Farm", "Greenhouse", "IslandWest" };

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