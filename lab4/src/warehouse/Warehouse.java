package warehouse;

import enums.*;
import java.util.ArrayList;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Function;
import java.util.function.Predicate;
import java.util.stream.Collectors;
import request.SupplyRequestDetail;
import warehouse.equipments.Equipment;
import warehouse.resources.Resource;


public class Warehouse {
    int id;
    WarehouseType type;
    String name;
    public ArrayList<Resource> resources = new ArrayList<>();
    public ArrayList<Equipment> equipments = new ArrayList<>();

    public int getId(){
        return id;
    }
    public WarehouseType getType(){
        return type;
    }
    public String getName(){
        return name;
    };
    
    public void setName(String name){
        this.name = name;
    }

    public Warehouse(int id, String name, WarehouseType type){
        this.id = id;
        this.name = name;
        this.type = type;
    }


    public Boolean addResource(Resource resource){
        Boolean hasId = resources.stream().anyMatch(r->r.getId() == resource.id);
        if(!hasId)
            resources.add(resource);
        return !hasId;
    }
    public int addResources(ArrayList<Resource> rs){
        var copy = new ArrayList<Resource>(rs);
        copy.removeIf(i->resources.stream().anyMatch(j->j.getId() == i.getId()));
        var distincted = copy.stream().filter(distinctBy(r->r.getId())).collect(Collectors.toList());
        resources.addAll(distincted);
        return distincted.size();
    }
    public Boolean addEquipment(Equipment equipment){
        Boolean hasId = equipments.stream().anyMatch(e->e.getId() == equipment.getId());
        if(!hasId)
            equipments.add(equipment);
        return !hasId;
    }
    public int addEquipments(ArrayList<Equipment> eq){
        var copy = new ArrayList<Equipment>(eq);
        copy.removeIf(i->equipments.stream().anyMatch(j->j.getId() == i.getId()));
        var distincted = copy.stream().filter(distinctBy(e->e.getId())).collect(Collectors.toList());
        equipments.addAll(distincted);
        return distincted.size();
    }
    public Boolean processSupplyRequestDetails(ArrayList<SupplyRequestDetail> details){
        if (details.size() < 1)
            return false;

        int origSize = details.size();

        for (SupplyRequestDetail detail : details) {
            switch (detail.getSupplyType()) {
                case AMMUNITION, FUEL -> writeOff(resources, detail);
                case WEAPON, VEHICLE->writeOff(equipments, detail);
            }
        }

        removeEmptyEntries(resources);
        removeEmptyEntries(equipments);
        removeEmptyRequestDetails(details);

        return origSize > details.size();
    }

    <T extends WarehouseEntry> Boolean writeOff(ArrayList<T> entries, SupplyRequestDetail detail){

        Boolean isCorrected = false;
        float remaining = detail.getCount();
        for(WarehouseEntry entry: entries){
            if(entry.isMatches(detail)){
                isCorrected = true;
                remaining-=(float)entry.decrease(remaining);
                if (remaining == 0.0f)
                    break;
            }
        }
        detail.setCount(remaining);
        return isCorrected;
    }

    <T extends WarehouseEntry> void removeEmptyEntries(ArrayList<T> entries){
        entries.removeIf(entry-> entry.isEmpty());
    }

    void removeEmptyRequestDetails(ArrayList<SupplyRequestDetail> details) {
        details.removeIf(detail->detail.getCount() == 0.0f);
    }

    static <T> Predicate<T> distinctBy(Function<? super T, ?> keyExtractor) {
        Set<Object> seen = ConcurrentHashMap.newKeySet();
        return t -> seen.add(keyExtractor.apply(t));
    }
}
