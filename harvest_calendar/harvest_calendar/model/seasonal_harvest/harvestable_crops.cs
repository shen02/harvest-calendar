using HarvestCalendar.Model.DailyHarvestInfo;
using HarvestCalendar.Model.DataTypes;
using StardewValley;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;

namespace HarvestCalendar.Model.SeasonHarvestInfo;
// HarvestableCrops is an object that maps each day in a season to a list of crops that are harvestable on that given day. 
internal class HarvestableCrops
{
    // invariant: harvestableCrops.size is equal to the number of days in the game's seasons.
    protected Dictionary<int, DailyHarvest> harvestableCrops = new Dictionary<int, DailyHarvest>();
    protected int daysInSeason = 28;

    public HarvestableCrops()
    {
        harvestableCrops = getAllCropsByDate();
    }

    public HarvestableCrops(int daysInSeason)
    {
        this.daysInSeason = daysInSeason;
        harvestableCrops = getAllCropsByDate();
    }

    public Dictionary<int, DailyHarvest> getAllHarvestableCrops()
    {
        return this.harvestableCrops;
    }

    // TODO: need abstraction + rewrite
    protected Dictionary<int, DailyHarvest> getAllCropsByDate()
    {
        Dictionary<int, DailyHarvest> allCropsByDate = new Dictionary<int, DailyHarvest>();

        List<Crop> farmCrops = getAllCropsInLocation(Game1.getFarm());
        List<Crop> islandCrops = getAllCropsInLocation(Game1.getLocationFromName("IslandWest"));
        List<Crop> greenHouseCrops = getAllCropsInLocation(Game1.getLocationFromName("Greenhouse"));

        Dictionary<int, HashSet<CropWithQuantity>> farmSet = mapByHarvestDate(farmCrops);

        Dictionary<int, HashSet<CropWithQuantity>> islandSet = islandCrops.Count > 0 ? mapByHarvestDate(islandCrops) : new Dictionary<int, HashSet<CropWithQuantity>>();

        Dictionary<int, HashSet<CropWithQuantity>> greenHouseSet = greenHouseCrops.Count > 0 ? mapByHarvestDate(greenHouseCrops) : new Dictionary<int, HashSet<CropWithQuantity>>();

        for (int i = 0; i <= daysInSeason; i++)
        {
            bool hasHarvest = false;
            DailyHarvest daily = new DailyHarvest();

            if (farmSet.ContainsKey(i))
            {
                daily.addCrops(FarmableLocationNames.Farm, farmSet[i]);
                hasHarvest = true;
            }

            if (islandSet.ContainsKey(i))
            {
                daily.addCrops(FarmableLocationNames.IslandWest, islandSet[i]);
                hasHarvest = true;
            }

            if (greenHouseSet.ContainsKey(i))
            {
                daily.addCrops(FarmableLocationNames.Greenhouse, greenHouseSet[i]);
                hasHarvest = true;
            }

            if (hasHarvest)
            {
                allCropsByDate.Add(i, daily);
            }

        }

        return allCropsByDate;
    }

    // Takes a list of Crops, sort into a hashset according to crop type and quantity, then map to their respective number of days until harvest.
    // Note: this function kind of does a few too many things. Might be able to abstract?
    protected Dictionary<int, HashSet<CropWithQuantity>> mapByHarvestDate(List<Crop> cropList)
    {
        Dictionary<int, HashSet<CropWithQuantity>> cropsByHarvestDay = new Dictionary<int, HashSet<CropWithQuantity>>();

        // Group the list of crops into subsequences according to their harvest dates.
        IEnumerable<IGrouping<int, Crop>> groupedCropsByHarvestDay = from crop in cropList group crop by getTimeUntilHarvest(crop) into cropGroups select cropGroups;

        foreach (IGrouping<int, Crop> cropGroup in groupedCropsByHarvestDay)
        {
            // Further group each subsequence in groupedCropsByHarvestDay by the crop type, create the according CropWithQuantity object, then
            // add them to a hashset of crops mapped to the days until harvest.
            HashSet<CropWithQuantity> harvestableCropSet = (from crop in cropGroup
                                                            group crop by crop.netSeedIndex into sameCropList
                                                            select new CropWithQuantity(sameCropList.ToList()[0], sameCropList.Count())).ToHashSet();

            // Sort the CropWithQuantity objects by descending quantities
            harvestableCropSet = harvestableCropSet.OrderByDescending(crop => crop.getQuantity()).ToHashSet();

            cropsByHarvestDay.Add(cropGroup.Key, harvestableCropSet);
        }


        return cropsByHarvestDay;
    }

    // Returns a list of all planted, living crops in the given locatoin
    protected List<Crop> getAllCropsInLocation(GameLocation location)
    {
        List<Crop> allPlantedCrops = new List<Crop>();

        // condition acquired from decompiled game source v1.6
        foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures.Pairs)
        {
            // crop is not null; crop is able to produce harvest; crop is not weed.
            if (pair.Value is HoeDirt { crop: not null, crop.dead.Value: not true, crop.indexOfHarvest: not null, crop.indexOfHarvest.Value: not "0", crop.whichForageCrop.Value: not Crop.forageCrop_gingerID })
            {
                allPlantedCrops.Add((pair.Value as HoeDirt).crop);
            }
        }
        return allPlantedCrops;
    }

    // Return the time remaining for the given crop to become harvestable.
    // Invariant: the last two members of the crop.phaseDays are always [9999, ''] to prevent further phase progression after the crop is ready for harvest.
    protected int getTimeUntilHarvest(Crop crop)
    {
        // If the plant is regrowing but not yet ready for re-harvesr
        if (crop.RegrowsAfterHarvest() && crop.fullyGrown.Value && !crop.Dirt.readyForHarvest())
            return crop.dayOfCurrentPhase.Value;

        // sum days in all future phases and add days in current phase
        int daysInRemainingPhases = crop.phaseDays.GetRange(crop.currentPhase.Value + 1, crop.phaseDays.Count - 1 - crop.currentPhase.Value - 1).Sum();
        // The conditional statement is a temporary workarount for the reset of currentPhase's value after the crop is fully grown.
        int daysRemainingInCurrentPhase = crop.currentPhase.Value > crop.phaseDays.Count - 2 ? 0 : crop.phaseDays[crop.currentPhase.Value] - crop.dayOfCurrentPhase.Value;

        return daysInRemainingPhases + daysRemainingInCurrentPhase;
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