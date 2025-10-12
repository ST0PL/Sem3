package warehouse;

import request.SupplyRequestDetail;

public abstract class WarehouseEntry<T extends Number> {

    protected int id;
    protected String name;
    protected int assignedWarehouseId;
    protected Warehouse assignedWarehouse;

    public WarehouseEntry(int id, String name){
        this.id = id;
        this.name = name;
    }

    public int getId(){
        return id;
    }
    public String getName(){
        return name;
    }
    public abstract void assignWarehouse(Warehouse warehouse);
    public abstract Boolean isMatches(SupplyRequestDetail detail);
    public abstract Boolean isEmpty();
    public abstract void increase(T v);
    public abstract T decrease(T v);
}
