package request;

import java.time.LocalDateTime;
import java.util.ArrayList;
import unit.Unit;

public class SupplyRequest {
    int id;
    int requestUnitId;
    Unit requestUnit;
    LocalDateTime creationTime;
    ArrayList<SupplyRequestDetail> details = new ArrayList<>();
    public SupplyRequest(int id, ArrayList<SupplyRequestDetail> details, Unit requestUnit){
        this.id = id;
        this.details = details;
        this.creationTime = LocalDateTime.now();
        if(requestUnit != null){
            this.requestUnitId = requestUnit.getId();
            this.requestUnit = requestUnit;
        }
    }
    public int getId(){
        return id;
    }
    public Unit getRequestUnit(){
        return requestUnit;
    }
    public void setRequestUnit(Unit unit){
        if(unit != null){
            requestUnitId = unit.getId();
            requestUnit = unit;
        }
    }
    public ArrayList<SupplyRequestDetail> getDetails(){
        return details;
    }
    public LocalDateTime getCreationTime(){
        return creationTime;
    }
}
