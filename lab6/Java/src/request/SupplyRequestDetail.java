package request;

import enums.*;
import java.util.ArrayList;
import java.util.Random;

public class SupplyRequestDetail implements Cloneable{
    int id;
    SupplyType supplyType;
    Caliber caliber;
    VehicleType vehicleType;
    FuelType fuelType;
    float count;
    
    public SupplyRequestDetail(int id, SupplyType supplyType, float count){
        
        if(count < 1){
            throw new IllegalArgumentException("Недопустимое количество");
        }        

        this.id = id;
        this.supplyType = supplyType;
        setCount(count);
    }

    public SupplyRequestDetail withCaliber(Caliber caliber){
        this.caliber = caliber;
        return this;
    }
    public SupplyRequestDetail withFuelType(FuelType fuelType){
        this.fuelType = fuelType;
        return this;
    }

    public SupplyRequestDetail withVehicleType(VehicleType vehicleType){
        this.vehicleType = vehicleType;
        return this;
    }

    public int getId(){
        return id;
    }

    public SupplyType getSupplyType(){
        return supplyType;
    }

    public Caliber getCaliber(){
        return caliber;
    }

    public VehicleType getVehicleType(){
        return vehicleType;
    }

    public FuelType getFuelType(){
        return fuelType;
    }

    public float getCount(){
        return count;
    }

    public final void setCount(float count){
        this.count = switch(supplyType){
            case AMMUNITION, VEHICLE, WEAPON-> Math.round(count);
            default->count;
        };
    }

    @Override
    public String toString(){
        String result = SupplyTypeToString(supplyType) + ": ";

        result += switch (supplyType) {
            case AMMUNITION, WEAPON ->  CaliberToString(caliber) + ", " + count + " шт";
            case FUEL -> FuelTypeToString(fuelType) + ", " + count + " л";
            case VEHICLE -> VehicleTypeToString(vehicleType) + ", " + FuelTypeToString(fuelType) + ", " + count + "шт";
        };

        return result;
    }
    @Override
    public SupplyRequestDetail clone() throws CloneNotSupportedException{
        var clone = (SupplyRequestDetail)super.clone();
        clone.id = new Random().nextInt();
        return clone;
    }

    public static String toString(ArrayList<SupplyRequestDetail> requestDetails){
        String result = "";
        for (SupplyRequestDetail detail: requestDetails) {
            result += detail.toString().concat("\n");
        }

        return result;
    }

    static String SupplyTypeToString(SupplyType stype){
        return switch(stype){
            case AMMUNITION -> "Боеприпасы";
            case FUEL -> "Топливо";
            case VEHICLE -> "Транспорт";
            case WEAPON -> "Вооружение";
            default -> "Неизвестно";
        };        
    }

    static String CaliberToString(Caliber caliber){
        return switch(caliber){
            case C_122MM -> "122мм";
            case C_545MM -> "5.45мм";
            default -> "Неизвестно";
        }; 
    }

    static String FuelTypeToString(FuelType ftype){
        return switch(ftype){
            case GASOLINE -> "Бензин";
            case DIESEL -> "Дизель";
            default -> "Неизвестно";
        };        
    }

    static String VehicleTypeToString(VehicleType vtype){
        return switch(vtype){
            case ARMORED_VEHICLE -> "Бронемашина";
            case TANK -> "Танк";
            case MOTORBIKE -> "Мотоцикл";
            default -> "Неизвестно";
        };
    }

}
