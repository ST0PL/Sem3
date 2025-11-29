package unit;

import enums.SupplyResponseStatus;
import enums.UnitType;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.function.Function;
import java.util.function.Predicate;
import java.util.stream.Collectors;
import request.SupplyRequest;
import request.SupplyRequestDetail;
import request.SupplyResponse;
import warehouse.Warehouse;

public class Unit implements Suppliable {

    int id;
    String name;
    UnitType type;
    int parentId;
    Unit parent;
    ArrayList<Unit> children = new ArrayList<>();
    ArrayList<Staff> personnel = new ArrayList<>();
    int commanderId;
    Staff commander;
    int assignedWarehouseId;
    Warehouse assignedWarehouse;

    public Unit(int id, String name, UnitType type){
        this.id = id;
        this.name = name;
        this.type = type;
    }

    public int getId(){
        return id;
    }

    public String getName(){
        return name;
    }

    public UnitType getType(){
        return type;
    }

    public Unit getParent(){
        return parent;
    }

    public Staff getCommander(){
        return commander;
    }

    public void setParent(Unit parent){
        if(parent != null && parent.type.compareTo(type) > 0){
            this.parentId = parent.getId();
            this.parent = parent;
        }
    }

    public void addChildUnit(Unit unit){
        if(unit != null && !hasChild(unit) && unit.getType().compareTo(type) < 0){
            this.children.add(unit);
            unit.setParent(this);
        }
    }

    public int addChildUnits(ArrayList<Unit> units){
        var copy = new ArrayList<Unit>(units);
        copy.removeIf(i->i == null || i.getType().compareTo(type) > 0);
        copy.removeIf(i->children.stream().anyMatch(j -> j.getId() == i.getId()));
        var distincted = copy.stream().filter(distinctBy(u->u.getId())).collect(Collectors.toList());
        for(Unit unit : distincted){
            unit.setParent(this);
        }
        children.addAll(distincted);
        return distincted.size();
    }

    public Boolean removeChildUnit(int id){
        return children.removeIf(c->c.getId() == id);
    }

    public List<Unit> getChildren(){
        return Collections.unmodifiableList(children);
    }    

    public void addSoldier(Staff soldier){
        if(soldier != null && !hasPersonnel(soldier)){
            soldier.setUnit(this);
            personnel.add(soldier);
        }
    }

    public int addSoldiers(ArrayList<Staff> soldiers){
        var copy = new ArrayList<Staff>(soldiers);
        copy.removeIf(i->personnel.stream().anyMatch(j->j.getId() == i.getId()));
        var distincted = copy.stream().filter(distinctBy(s->s.getId())).collect(Collectors.toList());
        for(Staff soldier : distincted){
            soldier.setUnit(this);
        }        
        personnel.addAll(distincted);
        return distincted.size();
    }

    public Boolean removeSoldier(int id){
        return personnel.removeIf(p->p.getId() == id);
    }

    public List<Staff> getPersonnel(){
        return Collections.unmodifiableList(personnel);
    }

    public ArrayList<Staff> findByName(String name){
        ArrayList<Staff> result = new ArrayList<>();
        for(var soldier : personnel){
            if(soldier.fullName.toLowerCase().contains(name)){
                result.add(soldier);
            }
        }
        return result;
    }

    public void assignWarehouse(Warehouse warehouse){
        if(warehouse != null){
            this.assignedWarehouseId = warehouse.getId();
            this.assignedWarehouse = warehouse;
        }
    }

    public void assignCommander(Staff commander){
        // добавить в список staff
        if(commander != null){
            this.commanderId = commander.getId();
            this.commander = commander;
        }
    }

    @Override
    public SupplyResponse makeSupplyRequest(SupplyRequest request){

        Boolean isCorrected = false;

        if(request == null)
            return new SupplyResponse(SupplyResponseStatus.DENIED, "Передан пустой запрос"); 

        
        if(assignedWarehouse != null){
            isCorrected = assignedWarehouse.processSupplyRequestDetails(request.getDetails());
        }

        if(!request.getDetails().isEmpty()){
            if(parent != null){
                var parentResponse = parent.makeSupplyRequest(request);
                
                if(parentResponse.getStatus() == SupplyResponseStatus.DENIED)
                    return isCorrected ? new SupplyResponse(SupplyResponseStatus.PARTIAL, request.getDetails()) :  parentResponse;

                return parentResponse;

            }
            return new SupplyResponse(SupplyResponseStatus.DENIED, "Ни один из складов дерева не смог удовлетворить запрос");

        }

        return new SupplyResponse(SupplyResponseStatus.SUCCESS, "");
    }

    public List<String> getStafflList(){
        return personnel.stream().map(p->p.toString()).toList();
    }
    public List<Staff> findStaffByFullName(String fullName){
        return personnel.stream().filter(p->p.getFullName().toLowerCase().contains(fullName.toLowerCase())).toList();
    }

    public SupplyRequest createRequest(ArrayList<SupplyRequestDetail> details){
        return new SupplyRequest(0, details, this);
    }

    Boolean hasPersonnel(Staff soldier){
        return personnel.stream().anyMatch(s->s.getId() == soldier.getId());
    }

    Boolean hasChild(Unit child){
        return children.stream().anyMatch(s->s.getId() == child.getId());
    }    

    static <T> Predicate<T> distinctBy(Function<? super T, ?> keyExtractor) {
        Set<Object> seen = ConcurrentHashMap.newKeySet();
        return t -> seen.add(keyExtractor.apply(t));
    }
}
