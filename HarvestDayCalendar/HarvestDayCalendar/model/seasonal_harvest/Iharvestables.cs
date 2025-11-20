using HarvestCalendar.Model.DailyHarvests;
using HarvestCalendar.Model.HarvestableWithQuantity;
using HarvestCalendar.Model.DataTypes;
using StardewValley;

public interface IHarvestable<T, R> where T : Crop where R : HarvestableWithQuantity<T>
{

    // Returns the already-made dictionary of all harvestables mapped to their time until harvest. 
    public Dictionary<int, DailyHarvest<T, R>> getAllHarvestables();

    // Makes and return a dictionary of all harvestables mapped to their time until harvest.
    public Dictionary<int, DailyHarvest<T, R>> getAllHarvestablesByDate();

    // Takes a list of INetObject<NetFields>, sort into a hashset according to crop type and quantity, then map to their respective number of days until harvest.
    public Dictionary<int, HashSet<R>> mapByHarvestDate(List<T> harvestableList);

    // Get all INetObject<Netfields> objects that is ready for harvest in the given game location.
    public List<T> getAllHarvestablesInLocation(GameLocation location);

    // Returns the time until the given InetObject<NetFields> is ready for harvest.
    public int getTimeUntilHarvest(T harvestable);

    // Returns a FarmableLocatoinNames object (Farm, Greenhouse, IslandWest) accoding to the given GameLocation.
    // Invariant: The given GameLocatoin will be one of Farm, Greenhouse, and IslandWest.
    public FarmableLocationNames getLocationNameByGameLocation(GameLocation location)
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
