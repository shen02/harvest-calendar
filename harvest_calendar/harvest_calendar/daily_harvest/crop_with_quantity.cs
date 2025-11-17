using StardewValley;

namespace HarvestCalendar.DailyHarvestInfo;

// CropWithQuantity is an object representing a Tuple<Crop, int> that restores a given harvestable crop and its according quantity.
// Invariant: all crops used to instatiate a CropWithQuantity object are harvestable at the time of instantiation, hence quantity > 0;
internal sealed class CropWithQuantity
{
    private Crop _crop;
    private int _quantity = 0;

    public CropWithQuantity(Crop c, int q)
    {
        _crop = c;
        _quantity = q;
    }

    public Crop getCrop()
    {
        return this._crop;
    }

    public int getQuantity()
    {
        return this._quantity;
    }
}