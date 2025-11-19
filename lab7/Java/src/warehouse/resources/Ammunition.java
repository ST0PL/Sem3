package warehouse.resources;

import enums.Caliber;
import enums.MeasureUnit;
import enums.SupplyType;
import java.util.Random;
import request.SupplyRequestDetail;

public class Ammunition extends Resource implements Cloneable{
    Caliber caliber;
    public Ammunition(int id, String name, Caliber caliber, int count)
    {
        super(id, name, MeasureUnit.ITEM, count);
        this.caliber = caliber;
    }

    @Override
    public Boolean isMatches(SupplyRequestDetail detail) {
        return detail.getSupplyType() == SupplyType.AMMUNITION &&
               detail.getCaliber() == caliber;
    }

    public Caliber getCaliber() {
        return caliber;
    }

    @Override
    public Ammunition clone() throws CloneNotSupportedException{
        var clone = (Ammunition)super.clone();
        clone.id = new Random().nextInt(0,9999);
        return clone;
    }
}
