using HarvestCalendar.Model.DailyHarvestInfo;
using HarvestCalendar.Model.DataTypes;
using HarvestCalendar.Model.SeasonHarvestInfo;

namespace HarvestCalendar.Model.Translator;

// Functional class that rearranges a HarvestableCrops object into a format that is easier to traverse through by the program's visual component. 
internal static class HarvestablesTranslator
{
    // Takes in a harvestableCrops object and flatten it to a dictionary object in the format of Dictionary<date of harvest, Dictionary<location, list of <Crop index of harvest, quantity>>.
    public static Dictionary<int, Dictionary<FarmableLocationNames, List<Tuple<string, int>>>> translate(int currentDate, HarvestableCrops harvestableCrops)
    {
        // Final returning object 
        Dictionary<int, Dictionary<FarmableLocationNames, List<Tuple<string, int>>>> translatedCrops = new Dictionary<int, Dictionary<FarmableLocationNames, List<Tuple<string, int>>>>();

        // The dictionary object mapping days until harvest with their harvests
        Dictionary<int, DailyHarvest> cropsByDate = harvestableCrops.getAllHarvestableCrops();

        foreach (KeyValuePair<int, DailyHarvest> daysUntilHarvest in cropsByDate)
        {
            int harvestDate = daysUntilHarvest.Key + currentDate;

            translatedCrops.Add(harvestDate, getDailyHarvests(daysUntilHarvest.Value));
        }

        return translatedCrops;
    }

    // Takes in a dailyHarvest object and flatten it to a dictionary object with all the location and crop data.
    public static Dictionary<FarmableLocationNames, List<Tuple<string, int>>> getDailyHarvests(DailyHarvest dailyHarvest)
    {
        Dictionary<FarmableLocationNames, List<Tuple<string, int>>> dailyCropHarvest = new Dictionary<FarmableLocationNames, List<Tuple<string, int>>>();

        FarmableLocationNames[] locationNames = (FarmableLocationNames[])Enum.GetValues(typeof(FarmableLocationNames));

        foreach (FarmableLocationNames locationName in locationNames)
        {
            if (dailyHarvest.getAllCrops().ContainsKey(locationName))
                dailyCropHarvest.Add(locationName, cropSetToList(dailyHarvest.getCropSetByLocation(locationName)));
        }

        return dailyCropHarvest;
    }

    // Takes in a hashset of CropWithQuantity and return the according list of Tuple<harvestIndex, quantity>
    public static List<Tuple<string, int>> cropSetToList(HashSet<CropWithQuantity> cropSet)
    {
        List<Tuple<string, int>> harvestIndexWithQuantity = new List<Tuple<string, int>>();

        foreach (CropWithQuantity cropWithQuantity in cropSet)
        {
            harvestIndexWithQuantity.Add(cropWithQuantityToTuple(cropWithQuantity));
        }

        return harvestIndexWithQuantity;
    }

    // Takes in a CropWithQuantity object and return a tuple of the crop's harvestIndex and quantity.
    public static Tuple<string, int> cropWithQuantityToTuple(CropWithQuantity cropWithQuantity)
    {
        return new Tuple<string, int>(cropWithQuantity.getCrop().indexOfHarvest.Value, cropWithQuantity.getQuantity());
    }
}