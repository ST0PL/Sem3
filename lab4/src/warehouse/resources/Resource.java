package warehouse.resources;

import enums.MeasureUnit;
import request.SupplyRequestDetail;
import warehouse.WarehouseEntry;

public abstract class Resource extends WarehouseEntry<Float> {

    Float quantity;
    MeasureUnit measureUnit;

    public Resource(int id, String name, MeasureUnit measureUnit, float quantity) {
        super(id, name);
        if(quantity < 1.0f){
            throw new IllegalArgumentException("Недопустимое количество");
        }            
        this.measureUnit = measureUnit;
        this.quantity = quantity;
    }

    public MeasureUnit getMeasureUnit(){
        return measureUnit;
    }

    public float getQuantity(){
        return quantity;
    }

    @Override
    public void increase(Number value){
        quantity+=value.floatValue();
    }

    @Override
    public Float decrease(Number value){
        Float taken = Math.min(quantity, value.floatValue());
        quantity-=taken;
        return taken;
    }
    
    @Override
    public Boolean isEmpty(){
        return quantity < 1.0f;
    }

    @Override
    public abstract Boolean isMatches(SupplyRequestDetail detail);
}
