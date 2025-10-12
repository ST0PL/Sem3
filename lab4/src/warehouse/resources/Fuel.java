package warehouse.resources;

import enums.FuelType;
import enums.SupplyType;
import request.SupplyRequestDetail;

public class Fuel extends Resource {
    FuelType type;

    public Fuel(int id, String name, FuelType type, int count)
    {
        super(id, name, count);
        this.type = type;
    }

    @Override
    public Boolean isMatches(SupplyRequestDetail detail) {
        return detail.getSupplyType() == SupplyType.FUEL &&
               detail.getFuelType() == type;
    }

    public FuelType getType() {
        return type;
    }
}
