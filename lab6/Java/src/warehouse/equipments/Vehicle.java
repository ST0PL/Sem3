package warehouse.equipments;

import enums.FuelType;
import enums.SupplyType;
import enums.VehicleType;
import java.util.Random;
import request.SupplyRequestDetail;

public class Vehicle extends Equipment implements Cloneable {
    VehicleType type;
    FuelType fuelType;

    public Vehicle(int id, String name, VehicleType type, FuelType fuelType, int count)
    {
        super(id, name, count);
        this.type = type;
        this.fuelType = fuelType;
    }

    @Override
    public Boolean isMatches(SupplyRequestDetail detail) {
        return detail.getSupplyType() == SupplyType.VEHICLE &&
               detail.getVehicleType() == type &&
               detail.getFuelType() == fuelType;
    }

    public  VehicleType getType() {
        return type;
    }

    public  FuelType getFuelType() {
        return fuelType;
    }

    @Override
    public Vehicle clone() throws CloneNotSupportedException{
        var clone = (Vehicle)super.clone();
        clone.id = new Random().nextInt(0,9999);
        return clone;
    }
}
