using StardewValley;

namespace HarvestCalendar.Model.HarvestableWithQuantity;

// Interface representing a Tuple<HarvestableObjectType harvest, int quantity>.
public abstract class HarvestableWithQuantity<T> where T : Crop
{

    protected T _harvestable;
    protected int _quantity = 0;

    public HarvestableWithQuantity(T harvestable, int quantity)
    {
        _harvestable = harvestable;
        _quantity = quantity;
    }

    public T getHarvestable() { return this._harvestable; }
    public int getQuantity() { return this._quantity; }
}
