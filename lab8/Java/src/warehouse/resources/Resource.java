package warehouse.resources;

import enums.MaterialType;
import enums.MeasureUnit;
import request.SupplyRequestDetail;
import warehouse.WarehouseEntry;

public abstract class Resource extends WarehouseEntry<Float> {

    Float quantity;
    MeasureUnit measureUnit;

    public static final float MIN_QUANTITY = 0.1f;

    public Resource(int id, String name, MaterialType materialType, MeasureUnit measureUnit, float quantity) {
        super(id, name, materialType);
        if(quantity < MIN_QUANTITY){
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
        return quantity < MIN_QUANTITY;
    }

    @Override
    public abstract Boolean isMatches(SupplyRequestDetail detail);
}