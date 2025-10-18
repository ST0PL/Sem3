package warehouse.equipments;

import enums.FuelType;
import enums.SupplyType;
import enums.VehicleType;
import request.SupplyRequestDetail;


public class Vehicle extends Equipment {
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
}
