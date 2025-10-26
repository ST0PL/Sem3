package warehouse.equipments;

import request.SupplyRequestDetail;
import warehouse.WarehouseEntry;

public abstract class Equipment extends WarehouseEntry<Integer> {

    int count;

    public static final Integer MIN_COUNT = 1;

    public Equipment(int id, String name, int count) { 
        super(id, name);
        if(count < MIN_COUNT){
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
        return count < MIN_COUNT;
    }

    @Override
    public abstract Boolean isMatches(SupplyRequestDetail detail);
}