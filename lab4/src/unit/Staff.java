package unit;

import enums.Rank;
import enums.Speciality;

public class Staff {
    int id;
    String fullName;
    Rank rank;
    Speciality speciality;
    int unitId;
    Unit unit;

    public Staff(int id, String fullName, Rank rank, Speciality speciality){
        this.id = id;
        this.fullName = fullName;
        this.rank = rank;
        this.speciality = speciality;
    }

    public int getId(){
        return id;
    }
    
    public String getFullName(){
        return fullName;
    }

    public Rank getRank(){
        return rank;
    }

    public Speciality getSpeciality(){
        return speciality;
    }

    public void setFullName(String fullName){
        this.fullName = fullName;
    }

    public void setRank(Rank rank){
        this.rank = rank;
    }

    public void setSpeciality(Speciality speciality){
        this.speciality = speciality;
    }

    public void setUnit(Unit unit){
        if(unit != null){
            this.unitId = unit.getId();
            this.unit = unit;
        }
    }

    public Unit getUnit(){
        return unit;
    }
}
