package warehouse;

import request.SupplyRequestDetail;

public abstract class WarehouseEntry<T extends Number> {

    String name;
    protected int id;
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
    
    public void assignWarehouse(Warehouse warehouse){
        if(warehouse!=null){
            this.assignedWarehouseId = warehouse.getId();
            this.assignedWarehouse = warehouse;
        }
    }
    public abstract Boolean isMatches(SupplyRequestDetail detail);
    public abstract Boolean isEmpty();
    public abstract void increase(Number v);
    public abstract T decrease(Number v);
}
