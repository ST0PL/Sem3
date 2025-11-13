package warehouse.equipments;

import enums.Caliber;
import enums.SupplyType;
import request.SupplyRequestDetail;

public class Weapon extends Equipment {
    Caliber caliber;
    public Weapon(int id, String name, Caliber caliber, int count)
    {
        super(id, name, count);
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
}
