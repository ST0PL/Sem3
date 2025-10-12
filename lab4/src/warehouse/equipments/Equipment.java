package warehouse.equipments;

import request.SupplyRequestDetail;
import warehouse.Warehouse;
import warehouse.WarehouseEntry;

public abstract class Equipment extends WarehouseEntry<Integer> {

    Integer count;

    public Equipment(int id, String name, int count) {
        super(id, name);
        this.count = count;
    }
    @Override
    public void increase(Integer value){
        count+=value;
    }

    @Override
    public Integer decrease(Integer value){
        int taken = Math.min(count, value);
        count-=taken;
        return taken;
    }
    
    @Override
    public Boolean isEmpty(){
        return count < 1;
    }

    @Override
    public void assignWarehouse(Warehouse warehouse){
        this.assignedWarehouseId = warehouse.getId();
    }

    @Override
    public abstract Boolean isMatches(SupplyRequestDetail detail);
}
