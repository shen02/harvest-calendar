using HarvestCalendar.DailyHarvestInfo;
using HarvestCalendar.DataTypes;
using StardewValley;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;


//TODO : Improve function scope + finish class functions

namespace HarvestCalendar.SeasonHarvestInfo;
// HarvestableCrops is an object that maps each day in a season to a list of crops that are harvestable on that given day. 
internal class HarvestableCrops
{
    // invariant: harvestableCrops.size is equal to the number of days in the game's seasons.
    public Dictionary<int, DailyHarvest> harvestableCrops;

    public HarvestableCrops()
    {
        harvestableCrops = new Dictionary<int, DailyHarvest>();
    }

    // Dictionary<int, Dictionary<FarmableLocationNames, HashSet<CropWithQuantity>>
    protected void cropGroupToSet()
    {

    }

    // Takes a list of Crops, sort into a hashset according to crop type and quantity, then map to their respective number of days until harvest.
    protected Dictionary<int, HashSet<CropWithQuantity>> groupByHarvestDate(List<Crop> cropList)
    {
        Dictionary<int, HashSet<CropWithQuantity>> cropsByHarvestDay = new Dictionary<int, HashSet<CropWithQuantity>>();

        // Group the list of crops into subsequences according to their harvest dates.
        IEnumerable<IGrouping<int, Crop>> groupedCropsByHarvestDay = cropList.GroupBy(getTimeUntilHarvest);

        foreach (IGrouping<int, Crop> cropGroup in groupedCropsByHarvestDay)
        {
            // Further group each subsequence in groupedCropsByHarvestDay by the crop type, create the according CropWithQuantity object, then
            // add them to a hashset of crops mapped to the days until harvest.
            HashSet<CropWithQuantity> harvestableCropSet = (from crop in cropGroup
                                                            group crop by crop.netSeedIndex into sameCropList
                                                            select new CropWithQuantity(sameCropList.ToList()[0], sameCropList.Count())).ToHashSet();

            cropsByHarvestDay.Add(cropGroup.Key, harvestableCropSet);
        }

        return cropsByHarvestDay;
    }

    // Returns a list of all planted, living crops in the given locatoin
    protected List<Crop> getAllCropsInLocation(GameLocation location)
    {

        // What we start with: list with all planted crop in a location
        // What we end with: A hashset of CropwithQuantity mapped to the locations mapped to the harvest dates
        List<Crop> allPlantedCrops = new List<Crop>();

        // condition acquired from decompiled game source v1.6
        foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures.Pairs)
        {
            if (pair.Value is HoeDirt { crop: not null })
            {

                CropWithQuantity cropWithQuantity = new CropWithQuantity((pair.Value as HoeDirt).crop, 1);
            }
        }

        return allPlantedCrops;
    }

    // Return the time remaining for the given crop to become harvestable.
    protected int getTimeUntilHarvest(Crop crop)
    {
        // sum days in all future phases and add days in current phase
        int daysInRemainingPhases = crop.phaseDays.GetRange(crop.currentPhase.Value + 1, crop.phaseDays.Count).Sum(); ;

        return daysInRemainingPhases + crop.dayOfCurrentPhase.Value;
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
}