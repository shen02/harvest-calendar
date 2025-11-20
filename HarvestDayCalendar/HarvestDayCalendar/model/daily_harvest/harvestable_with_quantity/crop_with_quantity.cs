using StardewValley;

namespace HarvestCalendar.Model.HarvestableWithQuantity;

// CropWithQuantity is an object representing a Tuple<Crop, int> that restores a given harvestable crop and its according quantity.
// Invariant: all crops used to instatiate a CropWithQuantity object are harvestable at the time of instantiation, hence quantity > 0;
internal class CropWithQuantity : HarvestableWithQuantity<Crop>
{
    public CropWithQuantity(Crop crop, int quantity) : base(crop, quantity) { }
}