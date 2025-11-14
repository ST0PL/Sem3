package request;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Random;
import unit.Unit;

public class SupplyRequest implements Cloneable {
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

    public SupplyRequest clone(Boolean shallow) throws CloneNotSupportedException{
        // Поверхностное клонирование
        var clone = (SupplyRequest)super.clone();
        // глубокое клонирование при shallow = false
        if(!shallow){
            clone.id = new Random().nextInt(0,9999);
            clone.creationTime = LocalDateTime.now();
            clone.details = new ArrayList<>();
            for(var detail : details){
                clone.details.add((SupplyRequestDetail)detail.clone());
            }
        }
        return clone;

    }
}
