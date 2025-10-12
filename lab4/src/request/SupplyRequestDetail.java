package request;

import enums.*;
import java.util.ArrayList;

public class SupplyRequestDetail {
    int id;
    SupplyType supplyType;
    Caliber caliber;
    VehicleType vehicleType;
    FuelType fuelType;
    float count;
    
    public SupplyRequestDetail(int id, SupplyType supplyType, float count){
        this.id = id;
        this.supplyType = supplyType;
        this.count = count;
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
    public void setCount(float count){
        this.count = count;
    }

    public static String ToString(ArrayList<SupplyRequestDetail> requestDetails){
        String result = "";
        for (SupplyRequestDetail detail: requestDetails) {
            result += detail.ToString().concat("\n");
        }

        return result;
    }
    public String ToString(){
        String result = SupplyTypeToString(supplyType) + ": ";

        result += switch (supplyType) {
            case AMMUNITION -> result += CaliberToString(caliber) + ", " + count + "шт";
            case FUEL -> result += FuelTypeToString(fuelType) + ", " + count + "л";
            case VEHICLE -> VehicleTypeToString(vehicleType) + ", " +
                            FuelTypeToString(fuelType) + ", " +
                            count + "шт";
            case WEAPON -> CaliberToString(caliber) + ", " + count + "шт";
        };

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
