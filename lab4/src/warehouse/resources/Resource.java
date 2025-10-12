package warehouse.resources;

import request.SupplyRequestDetail;
import warehouse.Warehouse;
import warehouse.WarehouseEntry;

public abstract class Resource extends WarehouseEntry<Float> {

    Float quantity;

    public Resource(int id, String name, float quantity) {
        super(id, name);
        this.quantity = quantity;
    }
    @Override
    public void increase(Float value){
        quantity+=value;
    }

    @Override
    public Float decrease(Float value){
        Float taken = Math.min(quantity, value);
        quantity-=taken;
        return taken;
    }
    
    @Override
    public Boolean isEmpty(){
        return quantity < 1;
    }

    @Override
    public void assignWarehouse(Warehouse warehouse){
        this.assignedWarehouseId = warehouse.getId();
    }

    @Override
    public abstract Boolean isMatches(SupplyRequestDetail detail);
}
