package warehouse.equipments;

import request.SupplyRequestDetail;
import warehouse.WarehouseEntry;

public abstract class Equipment extends WarehouseEntry<Integer> {

    int count;

    public Equipment(int id, String name, int count) { 
        super(id, name);
        if(count < 1){
            throw new IllegalArgumentException("Недопустимое количество");
        }         
        this.count = count;
    }

    public int getCount(){
        return count;
    }
    @Override
    public void increase(Number value){
        count+=value.intValue();
    }

    @Override
    public Integer decrease(Number value){
        int taken = Math.min(count, value.intValue());
        count-=taken;
        return taken;
    }
    
    @Override
    public Boolean isEmpty(){
        return count < 1;
    }

    @Override
    public abstract Boolean isMatches(SupplyRequestDetail detail);
}
