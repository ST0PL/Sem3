package warehouse.equipments;

import enums.Caliber;
import enums.MaterialType;
import enums.SupplyType;
import java.util.Random;
import request.SupplyRequestDetail;

public class Weapon extends Equipment implements Cloneable {
    Caliber caliber;
    public Weapon(int id, String name, Caliber caliber, int count)
    {
        super(id, name, MaterialType.WEAPON, count);
        this.caliber = caliber;
    }

    @Override
    public Boolean isMatches(SupplyRequestDetail detail) {
        return detail.getSupplyType() == SupplyType.WEAPON &&
               detail.getCaliber() == caliber;
    }

    public Caliber getCaliber() {
        return caliber;
    }

    @Override
    public Weapon clone() throws CloneNotSupportedException{
        var clone = (Weapon)super.clone();
        clone.id = new Random().nextInt(0,9999);
        return clone;
    }    
}
