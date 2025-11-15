package warehouse.resources;

import enums.FuelType;
import enums.MeasureUnit;
import enums.SupplyType;
import java.util.Random;
import request.SupplyRequestDetail;

public class Fuel extends Resource implements Cloneable {
    FuelType type;

    public Fuel(int id, String name, FuelType type, int count)
    {
        super(id, name, MeasureUnit.LITER, count);
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

    @Override
    public Fuel clone() throws CloneNotSupportedException{
        var clone = (Fuel)super.clone();
        clone.id = new Random().nextInt(0,9999);
        return clone;
    }
    
}
