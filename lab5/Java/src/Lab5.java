
import enums.Caliber;
import enums.Rank;
import enums.Speciality;
import enums.SupplyType;
import enums.UnitType;
import enums.WarehouseType;
import static java.lang.System.out;
import java.util.ArrayList;
import java.util.Arrays;
import request.SupplyRequestDetail;
import request.SupplyResponse;
import unit.Staff;
import unit.Unit;
import warehouse.Warehouse;
import warehouse.equipments.Weapon;
import warehouse.resources.Ammunition;

class Lab5 {
    public static void main(String[] args) {

        Staff[] staff = new Staff[]
        {
             new Staff(1, "Иванов Иван Иванович", Rank.PRIVATE, Speciality.MEDIC),
             new Staff(2, "Морозов Константин Алексеевич", Rank.PRIVATE, Speciality.ENGINEER),
             new Staff(3, "Васильев Антон Владимирович", Rank.PRIVATE, Speciality.TANKER),
             new Staff(4, "Кузьмин Вячеслав Александрович", Rank.ARMY_GENERAL, Speciality.TANKER),
        };

        Unit army = new Unit(1, "1-я Армия", UnitType.ARMY);
        Unit division = new Unit(2, "1-я Мотострелковая дивизия", UnitType.DIVISION);
        Unit brigade = new Unit(3, "1-я Мотострелковая бригада", UnitType.BRIGADE);
        brigade.addSoldiers(new ArrayList<>(Arrays.asList(staff)));

        var searchResult = brigade.findByName("антон");
        out.println(String.format("Результаты поиска солдатов с именем \"Антон\": %s", searchResult));

        // Попытка создать снаряжение с некорректным колличеством

        try{
            Weapon corrupted = new Weapon(1, "Самоходная артиллерийская установка 2С1 «Гвоздика»", Caliber.C_122MM, 0);
        }
        catch(IllegalArgumentException ex) { out.println(String.format("Обработано исключение при создании снаряжения: %s", ex.getMessage())); }
        finally { out.println("Выполнен finally блок"); }

        army.assignCommander(staff[3]);
        
        Warehouse armyWarehouse = new Warehouse(1, "Армейский склад", WarehouseType.REAR);
        Warehouse divisionWarehouse = new Warehouse(2, "Дивизионный склад", WarehouseType.FIELD);


        armyWarehouse.addEquipment(new Weapon(1, "Самоходная артиллерийская установка 2С1 «Гвоздика»", Caliber.C_122MM, 20));
        armyWarehouse.addResource(new Ammunition(1, "Патроны 5.45мм", Caliber.C_545MM, 2000));
        divisionWarehouse.addResource(new Ammunition(1, "Патроны 5.45мм", Caliber.C_545MM, 100));

        army.assignWarehouse(armyWarehouse);
        division.assignWarehouse(divisionWarehouse);

        army.addChildUnit(division);
        division.addChildUnit(brigade);

        out.println(String.format("Создана иерархия:"));


        printTree(army, "");


        out.println("\nБригада запрашивает 2200 патронов и 25 артиллерийских установок калибра 122мм (на складе дивизии только 100 патронов, на армейском 2000 патронов и 20 артиллерийских установок):");


        SupplyRequestDetail[] details = new SupplyRequestDetail[]{
            new SupplyRequestDetail(1, SupplyType.AMMUNITION, 2200).withCaliber(Caliber.C_545MM),
            new SupplyRequestDetail(2, SupplyType.WEAPON, 25).withCaliber(Caliber.C_122MM)
        };

        var request = brigade.createRequest(new ArrayList<>(Arrays.asList(details)));
        SupplyResponse response = division.makeSupplyRequest(request);
        out.println("\nРезультат\n");
        out.println("Статус: "+SupplyResponse.statusToString(response.getStatus()));
        out.println("Комментарий: " + response.getComment());
    }

    public static void printTree(Unit parent, String tabs){
        
        out.println(tabs+parent.getName());

        
        for(var soldier : parent.getPersonnel()){
            out.println(tabs+'\t'+soldier.toString());
        }

        if(!parent.getChildren().isEmpty()){
            for(var child : parent.getChildren()){
                printTree(child, tabs+'\t');
            }
        }        
    }
}